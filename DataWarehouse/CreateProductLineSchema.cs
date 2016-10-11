namespace AppliedSystems.DataWarehouse
{
    using AppliedSystems.Messaging.Messages;

    public class CreateProductLineSchema : ICommand
    {
        public string Schema { get; }

        public CreateProductLineSchema(string schema)
        {
            Schema = schema;
        }
    }
}