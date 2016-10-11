namespace AppliedSystems.DataWarehouse
{
    using System.Globalization;
    using AppliedSystems.Infrastucture.Data;
    using AppliedSystems.Messaging.Infrastructure.Commands;

    public class CreateProductLineSchemaHandler : ICommandHandler<CreateProductLineSchema>
    {
        private readonly SqlRunner runner;

        public CreateProductLineSchemaHandler(SqlRunner runner)
        {
            this.runner = runner;
        }

        public void Handle(CreateProductLineSchema message)
        {
            runner.ExecuteCommand(string.Format(
                CultureInfo.InvariantCulture, 
                "CREATE SCHEMA [{0}] AUTHORIZATION [dbo]", 
                message.Schema));
        }
    }
}