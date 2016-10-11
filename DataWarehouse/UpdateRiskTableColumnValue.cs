namespace AppliedSystems.DataWarehouse
{
    using System;
    using AppliedSystems.Messaging.Messages;

    public class UpdateRiskTableColumnValue : ICommand
    {
        public string Schema { get; set; }
        public string TableName { get; set; }
        public Guid Id { get; set; }
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }

        public UpdateRiskTableColumnValue(string schema, string tableName, Guid id, string columnName, string columnValue)
        {
            Schema = schema;
            TableName = tableName;
            Id = id;
            ColumnName = columnName;
            ColumnValue = columnValue;
        }
    }
}