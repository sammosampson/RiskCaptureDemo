namespace AppliedSystems.Infrastucture.Messaging.Sagas
{
    using AppliedSystems.Messaging.Infrastructure.Sagas;
    using AppliedSystems.Messaging.Infrastructure.Sagas.Bootstrapping;

    public static class SagaPersistenceExtensions
    {
        public static SagaConfiguration WithDatabasePersistence(this SagaConfiguration sagaConfiguration)
        {
            sagaConfiguration.RegisterBuildAction(c => c.RegisterInstance<ISagaStateRepository, DbSagaStateRepository>());
            return sagaConfiguration;
        }
    }
}
