namespace AppliedSystems.DataWarehouse
{
    using System.Globalization;
    using AppliedSystems.DataWarehouse.Messages;
    using AppliedSystems.Infrastucture;
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
            GreenLogger.Log("Creating schema {0}", message.Schema);
            
            runner.ExecuteCommand(string.Format(
                CultureInfo.InvariantCulture, 
                "CREATE SCHEMA [{0}] AUTHORIZATION [dbo]", 
                message.Schema));
        }
    }
}