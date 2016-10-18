namespace AppliedSystems.DataWarehouse
{
    using System.Globalization;
    using AppliedSystems.DataWarehouse.Messages;
    using AppliedSystems.Infrastucture.Data;
    using AppliedSystems.Messaging.Infrastructure.Commands;

    public class CreateRiskTableColumnHandler : ICommandHandler<CreateRiskTableColumn>
    {
        private readonly SqlRunner runner;

        public CreateRiskTableColumnHandler(SqlRunner runner)
        {
            this.runner = runner;
        }

        public void Handle(CreateRiskTableColumn message)
        {
            runner.ExecuteCommand(string.Format(
                CultureInfo.InvariantCulture, 
                "ALTER TABLE [{0}].[{1}] ADD {2} NVARCHAR(250) NULL ", 
                message.Schema, 
                message.TableName, 
                message.ColumnName));
        }
    }
}