namespace AppliedSystems.DataWarehouse
{
    using System.Globalization;
    using AppliedSystems.DataWarehouse.Messages;
    using AppliedSystems.Infrastucture;
    using AppliedSystems.Infrastucture.Data;
    using AppliedSystems.Messaging.Infrastructure.Commands;

    public class UpdateRiskTableColumnValueHandler : ICommandHandler<UpdateRiskTableColumnValue>
    {
        private readonly SqlRunner runner;

        public UpdateRiskTableColumnValueHandler(SqlRunner runner)
        {
            this.runner = runner;
        }

        public void Handle(UpdateRiskTableColumnValue message)
        {
            GreenLogger.Log("Updating column {0} in table {1}.{2} with value {3}", message.ColumnName, message.Schema, message.TableName, message.ColumnValue);
            
            const string UpsertSql = @"
IF NOT EXISTS (SELECT * FROM [{0}].[{1}] WHERE Id = '{2}') 
	INSERT INTO [{0}].[{1}](Id,{3}) VALUES('{2}','{4}') 
ELSE 
	UPDATE [{0}].[{1}] SET {3} = '{4}' WHERE Id = '{2}'";

            runner.ExecuteCommand(string.Format(
                CultureInfo.InvariantCulture,
                UpsertSql,
                message.Schema,
                message.TableName,
                message.Id,
                message.ColumnName,
                message.ColumnValue));
        }
    }
}