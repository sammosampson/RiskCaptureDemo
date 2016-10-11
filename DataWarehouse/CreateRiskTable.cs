namespace AppliedSystems.DataWarehouse
{
    using AppliedSystems.Messaging.Messages;

    public class CreateRiskTable : ICommand
    {
        public string Schema { get; }
        public string Name { get; }

        public CreateRiskTable(string schema, string name)
        {
            Schema = schema;
            Name = name;
        }
    }
}