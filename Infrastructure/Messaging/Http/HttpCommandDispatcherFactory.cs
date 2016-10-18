namespace AppliedSystems.Infrastucture.Messaging.Http
{
    using AppliedSystems.Messaging.Infrastructure.Commands;
    using AppliedSystems.Messaging.Infrastructure.Commands.Outgoing;
    using AppliedSystems.Messaging.Infrastructure.Commands.Outgoing.InProcess;
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using AppliedSystems.Messaging.Messages;

    public class HttpCommandDispatcherFactory : ICommandDispatcherFactory
    {
        private readonly MessagePipeline pipeline;

        public HttpCommandDispatcherFactory(MessagePipeline pipeline)
        {
            this.pipeline = pipeline;
        }

        public IFixedCommandDispatcher Create(ICommand message)
        {
            return new InProcessCommandDispatcher(this.pipeline, message);
        }
    }
}