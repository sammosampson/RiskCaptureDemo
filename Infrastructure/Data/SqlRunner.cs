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

        public void ExecuteCommand(string commandText)
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

        public TReturnType ExecuteReader<TReturnType>(string commandText, Func<IDataReader, TReturnType> readerFunc)
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