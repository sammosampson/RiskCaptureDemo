namespace AppliedSystems.DataWarehouse
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using AppliedSystems.DataWarehouse.Messages;
    using AppliedSystems.Infrastucture;
    using AppliedSystems.Infrastucture.Data;
    using AppliedSystems.Messaging.Infrastructure.Commands;
    using AppliedSystems.Messaging.Infrastructure.Events.Streams;

    public class UpdateRiskTableColumnValueHandler : ICommandHandler<UpdateRiskTableColumnValue>
    {
        private readonly SqlRunner runner;
        private readonly IProjectionStore projectionStore;

        public UpdateRiskTableColumnValueHandler(SqlRunner runner, IProjectionStore projectionStore)
        {
            this.runner = runner;
            this.projectionStore = projectionStore;
        }

        public void Handle(UpdateRiskTableColumnValue message)
        {
            var mapping = projectionStore
                .GetProjection<RiskCaptureItemToDataWarehouseColumnMapping>(RiskCaptureItemToDataWarehouseColumnMappingId.Parse(message.Schema))
                .Single(i => i.ItemId == message.Id);

            GreenLogger.Log("Updating column {0} in table {1}.{2} with value {3}", mapping.TableName, message.Schema, mapping.ColumnName, message.ColumnValue);
            
            const string UpsertSql = @"
IF NOT EXISTS (SELECT * FROM [{0}].[{1}] WHERE Id = '{2}') 
	INSERT INTO [{0}].[{1}](Id,{3}) VALUES('{2}','{4}') 
ELSE 
	UPDATE [{0}].[{1}] SET {3} = '{4}' WHERE Id = '{2}'";

            runner.ExecuteCommand(string.Format(
                CultureInfo.InvariantCulture,
                UpsertSql,
                message.Schema,
                mapping.ColumnName,
                message.Id,
                mapping.TableName,
                message.ColumnValue));
        }
    }
}