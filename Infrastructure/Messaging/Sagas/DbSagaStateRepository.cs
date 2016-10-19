namespace AppliedSystems.Infrastucture.Messaging.Sagas
{
    using System;
    using System.Data.Odbc;
    using AppliedSystems.Core;
    using AppliedSystems.Data;
    using AppliedSystems.Infrastucture.Data;
    using AppliedSystems.Messaging.Data;
    using AppliedSystems.Messaging.Infrastructure.Sagas;
    using AppliedSystems.Messaging.Messages;
    using Dapper;

    public class DbSagaStateRepository : ISagaStateRepository
    {
        private readonly SqlRunner runner;
        private const string CreationSql = @"
IF (NOT EXISTS 
(
	SELECT * 
	FROM INFORMATION_SCHEMA.TABLES 
	WHERE TABLE_SCHEMA = 'dbo' 
	AND  TABLE_NAME = 'SagaState'
))
BEGIN
	CREATE TABLE [dbo].[SagaState](
		[SagaId] [varchar](100) NOT NULL,
		[SagaState] [varchar](max) NOT NULL,
		[SagaStateType] [varchar](100) NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[SagaId] ASC,
		[SagaStateType] ASC
	)
	WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) 
	ON [PRIMARY]
END";

        public DbSagaStateRepository(SqlRunner runner)
        {
            this.runner = runner;
        }

        public void StoreState<TState>(TState state, string sagaId)
        {
            var stateType = state.GetType().ToString();
            var serialisedState = state.SerialiseToJson();

            try
            {
                runner.ExecuteCommand(CreationSql);
                runner.ExecuteCommand($"INSERT INTO SagaState (SagaId, SagaState, SagaStateType) VALUES ('{sagaId}', '{serialisedState}', '{stateType}')");
            }
            catch (OdbcException ex)
            {
                if (ex.Message.Contains("Violation of PRIMARY KEY constraint"))
                {
                    throw new SagaAlreadyStartedException(typeof(TState), sagaId);
                }
                throw;
            }
        }

        public void RemoveState<TState>(string sagaId)
        {
            var stateType = typeof(TState).ToString();
            runner.ExecuteCommand($"DELETE FROM SagaState WHERE sagaid = '{sagaId}' AND SagaStateType = '{stateType}'");
        }

        public void UpdateState<TState>(TState state, string sagaId)
        {
            var stateType = state.GetType().ToString();
            var serialisedState = state.SerialiseToJson();
            runner.ExecuteCommand($"UPDATE SagaState SET SagaState='{serialisedState}' WHERE SagaId='{sagaId}' AND SagaStateType='{stateType}'");
        }

        public NotRequired<TState> FindState<TState, TMessage>(Func<TMessage, TState, string> sagaIdProvider, TMessage message) where TMessage : IMessage
        {
            CreateStateTableIfNotExists<TState, TMessage>();

            string sagaId = sagaIdProvider(message, default(TState));
            string stateType = typeof(TState).ToString();

            return runner.ExecuteReader(
                $"SELECT SagaState FROM SagaState WHERE sagaid='{sagaId}' AND SagaStateType='{stateType}'", 
                reader =>
                {
                    if (!reader.Read())
                    {
                        return new NotRequired<TState>();
                    }

                    object state = reader.GetString("SagaState").DeserialiseFromJson();

                    if (!(state is TState))
                    {
                        return new NotRequired<TState>();
                    }

                    TState actualValue = (TState)state;

                    return new NotRequired<TState>(actualValue);
                });
        }

        private void CreateStateTableIfNotExists<TState, TMessage>() where TMessage : IMessage
        {
            runner.ExecuteCommand(CreationSql);
        }
    }
}