namespace AppliedSystems.DataWarehouse
{
    using AppliedSystems.Messaging.Messages;

    public class CreateRiskTableColumn : ICommand
    {
        public string Schema { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }

        public CreateRiskTableColumn(string schema, string tableName, string columnName)
        {
            Schema = schema;
            TableName = tableName;
            ColumnName = columnName;
        }
    }
}