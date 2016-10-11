namespace AppliedSystems.Documents.Bootstrapping
{
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;

    public static class MessageRoutingConfigurationExtensions
    {
        public static MessageRoutingConfiguration WireUpRouting(this MessageRoutingConfiguration config)
        {
            return config
                .Incoming.ForEvents;
        }
    }
}