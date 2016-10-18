namespace AppliedSystems.Infrastucture.Messaging.Http
{
    using System;
    using AppliedSystems.Messaging.Infrastructure.Commands.Outgoing;

    public class HttpCommandDispatchingEndpoint : ICommandDispatchingEndpoint
    {

        public static HttpCommandDispatchingEndpoint FromUrl(string url)
        {
            return new HttpCommandDispatchingEndpoint(url);
        }

        public Type EndpointBuilderType => typeof(HttpCommandDispatchingEndpointBuilder);

        public string Url { get; }

        private HttpCommandDispatchingEndpoint(string url)
        {
            Url = url;
        }
    }
}