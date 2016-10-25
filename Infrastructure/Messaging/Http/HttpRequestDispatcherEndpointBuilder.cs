namespace AppliedSystems.Infrastucture.Messaging.Http
{
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using AppliedSystems.Messaging.Infrastructure.Requests.Outgoing.InProcess;

    public class HttpRequestDispatcherEndpointBuilder : IRequestDispatchingEndpointBuilder<HttpRequestDispatcherEndpoint>
    {
        public IRequestDispatcherFactory Build(HttpRequestDispatcherEndpoint endpoint, MessagePipelineBuilder pipelineBuilder)
        {
            MessagePipeline pipeline = pipelineBuilder
                .AddPipelineComponent(new HttpSendRequestMessagePipe(MessageReceiverUrl.Parse(endpoint.Url)))
                .Build();

            return new HttpRequestDispatcherFactory(pipeline);
        }
    }
}