namespace AppliedSystems.Infrastucture.Data
{
    using System;
    using System.Data;
    using AppliedSystems.Data.Connections;

    public class SqlRunner
    {
        private readonly IConnectionFactory connectionFactory;

        public SqlRunner(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public void ExecuteConnectionAction(Action<IDbConnection> connectionAction)
        {
            using (IDbConnection connection = connectionFactory.Create())
            {
                connection.Open();
                connectionAction(connection);
            }
        }

        public void ExecuteCommand(string commandText)
        {
            ExecuteConnectionAction(connection =>
            {
                using (IDbCommand command = CreateCommand(connection, commandText))
                {
                    command.ExecuteNonQuery();
                }
            });
        }

        public TReturnType ExecuteReader<TReturnType>(string commandText, Func<IDataReader, TReturnType> readerFunc)
        {
            TReturnType returnValue = default(TReturnType);

            ExecuteConnectionAction(connection =>
            {
                using (IDbCommand command = CreateCommand(connection, commandText))
                {
                    returnValue =  readerFunc(command.ExecuteReader());
                }
            });

            return returnValue;
        }

        private IDbCommand CreateCommand(IDbConnection connection, string commandText)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            return command;
        }
    }
}