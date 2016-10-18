namespace AppliedSystems.DataWarehouse.Messages
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