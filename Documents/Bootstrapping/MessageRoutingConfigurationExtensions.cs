namespace AppliedSystems.Documents.Bootstrapping
{
    using AppliedSystems.Documents.Messages;
    using Messaging.Infrastructure.Bootstrapping;

    public static class MessageRoutingConfigurationExtensions
    {
        public static MessageRoutingConfiguration WireUpRouting(this MessageRoutingConfiguration config)
        {
            return config
                .Incoming.ForCommands
                    .Handle<MergeFieldValueIntoDocument>().With<MergeFieldValueIntoDocumentHandler>();
        }
    }
}