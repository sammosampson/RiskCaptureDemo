namespace AppliedSystems.Infrastucture.Messaging.Http
{
    using System;
    using AppliedSystems.Messaging.Infrastructure.Requests.Outgoing.InProcess;

    public class HttpRequestDispatcherEndpoint : IRequestDispatchingEndpoint
    {
        public static HttpRequestDispatcherEndpoint ForUrl(string url)
        {
            return new HttpRequestDispatcherEndpoint(url);
        }

        public string Url { get; }

        private HttpRequestDispatcherEndpoint(string url)
        {
            this.Url = url;
        }

        public Type EndpointBuilderType => typeof(HttpRequestDispatcherEndpointBuilder);
    }
}