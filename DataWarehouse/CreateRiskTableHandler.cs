namespace AppliedSystems.DataWarehouse
{
    using System.Globalization;
    using AppliedSystems.DataWarehouse.Messages;
    using AppliedSystems.Infrastucture.Data;
    using AppliedSystems.Messaging.Infrastructure.Commands;

    public class CreateRiskTableHandler : ICommandHandler<CreateRiskTable>
    {
        private readonly SqlRunner runner;

        public CreateRiskTableHandler(SqlRunner runner)
        {
            this.runner = runner;
        }

        public void Handle(CreateRiskTable message)
        {
            const string CreateTableSql = @"
CREATE TABLE [{0}].[{1}](
	[Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_{0}_{1}] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]";
            runner.ExecuteCommand(string.Format(CultureInfo.InvariantCulture, CreateTableSql, message.Schema, message.Name));
        }
    }
}