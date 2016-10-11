namespace AppliedSystems.Infrastucture.Data
{
    using System.Data;
    using System.Data.SqlClient;
    using AppliedSystems.Data.Connections;

    public class SqlConnectionFactory : IConnectionFactory
    {
        private readonly IConnectionStringProvider connectionStringProvider;

        public SqlConnectionFactory(IConnectionStringProvider connectionStringProvider)
        {
            this.connectionStringProvider = connectionStringProvider;
        }

        public IDbConnection Create()
        {
            return new SqlConnection(connectionStringProvider.ConnectionString);
        }
    }
}
