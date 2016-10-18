namespace AppliedSystems.Infrastucture.Messaging.Http
{
    using AppliedSystems.Messaging.Http;
    using AppliedSystems.Messaging.Infrastructure.Commands.Outgoing;
    using AppliedSystems.Messaging.Infrastructure.Pipelines;

    public class HttpCommandDispatchingEndpointBuilder : ICommandDispatchingEndpointBuilder<HttpCommandDispatchingEndpoint>
    {
        public ICommandDispatcherFactory Build(HttpCommandDispatchingEndpoint endpoint, MessagePipelineBuilder pipelineBuilder)
        {
            return new HttpCommandDispatcherFactory(pipelineBuilder.AddPipelineComponent(new HttpSendMessagePipe(MessagePublisherUrl.Parse(endpoint.Url))).Build());
        }
    }
}