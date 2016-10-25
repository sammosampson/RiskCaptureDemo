namespace AppliedSystems.Infrastucture.Messaging.Http
{
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using AppliedSystems.Messaging.Infrastructure.Requests;
    using AppliedSystems.Messaging.Infrastructure.Requests.Outgoing.InProcess;
    using AppliedSystems.Messaging.Messages;

    public class HttpRequestDispatcherFactory : IRequestDispatcherFactory
    {
        private readonly MessagePipeline pipeline;

        public HttpRequestDispatcherFactory(MessagePipeline pipeline)
        {
            this.pipeline = pipeline;
        }

        public IRequestDispatcher Create(IRequest message)
        {
            return new InProcessRequestDispatcher(pipeline);
        }
    }
}