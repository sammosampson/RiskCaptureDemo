namespace AppliedSystems.DataWarehouse.Messages
{
    using System;
    using AppliedSystems.Messaging.Messages;

    public class UpdateRiskTableColumnValue : ICommand
    {
        public string Schema { get; private set; }
        public Guid Id { get; private set; }
        public string ColumnValue { get; private set; }

        public UpdateRiskTableColumnValue(string schema, Guid id, string columnValue)
        {
            Schema = schema;
            Id = id;
            ColumnValue = columnValue;
        }
    }
}