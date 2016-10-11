namespace AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing
{
    using System;
    using System.Data;
    using System.Globalization;
    using AppliedSystems.Data.Connections;

    public class SqlEventIndexStore : IEventIndexStore
    {
        const string TableCreationSql = @"
IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'EventIndexStore'))
BEGIN
	CREATE TABLE [dbo].[EventIndexStore](
		[Stream] [nvarchar](250) NOT NULL,
		[EventIndex] [int] NOT NULL,
	 CONSTRAINT [PK_EventIndexStore] PRIMARY KEY CLUSTERED 
	(
		[Stream] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END";
        const string EventIndexUpsertSql = @"
IF NOT EXISTS (SELECT * FROM dbo.EventIndexStore WHERE Stream = '{0}') 
	INSERT INTO dbo.EventIndexStore(Stream, EventIndex) VALUES('{0}', {1}) 
ELSE 
	UPDATE dbo.EventIndexStore SET Stream = '{0}', EventIndex = {1}";

        const string EventIndexSelectSql = @"
SELECT [EventIndex] FROM [EventIndexStore] WHERE [Stream] = '{0}'";

        private readonly IConnectionFactory connectionFactory;

        public SqlEventIndexStore(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public void Store(string stream, int index)
        {
            ExecuteCommand(TableCreationSql);
            ExecuteCommand(string.Format(CultureInfo.InvariantCulture, EventIndexUpsertSql, stream, index));
        }

        public int? Get(string stream)
        {
            ExecuteCommand(TableCreationSql);
            return ExecuteReader(string.Format(CultureInfo.InvariantCulture, EventIndexSelectSql, stream), reader => reader.GetLastIndex());
        }

        private void ExecuteCommand(string commandText)
        {
            using (IDbConnection connection = connectionFactory.Create())
            {
                connection.Open();

                using (IDbCommand command = CreateCommand(connection, commandText))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private TReturnType ExecuteReader<TReturnType>(string commandText, Func<IDataReader, TReturnType> readerFunc)
        {
            using (IDbConnection connection = connectionFactory.Create())
            {
                connection.Open();

                using (IDbCommand command = CreateCommand(connection, commandText))
                {
                    return readerFunc(command.ExecuteReader());
                }
            }
        }

        private IDbCommand CreateCommand(IDbConnection connection, string commandText)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            return command;
        }
    }
}