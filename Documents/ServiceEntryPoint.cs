namespace AppliedSystems.Documents
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Dynamic;
    using System.Linq;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.Core;
    using AppliedSystems.Data.Bootstrapping;
    using AppliedSystems.Data.Connections;
    using AppliedSystems.Documents.Bootstrapping;
    using AppliedSystems.Documents.Configuration;
    using AppliedSystems.Infrastucture.Data;
    using AppliedSystems.Infrastucture.Messaging.Http;
    using AppliedSystems.Messaging.Http.Receiving;
    using AppliedSystems.Messaging.Http.Serialisation;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Events;
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using AppliedSystems.Messaging.Infrastructure.Receiving;
    using AppliedSystems.RiskCapture.Messages;
    using Microsoft.Owin.Cors;
    using Microsoft.Owin.Hosting;
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Owin;
    using Nancy.TinyIoc;
    using Owin;
    using Topshelf;

    class ServiceEntryPoint
    {
        
        static void Main()
        {

            HostFactory.Run(configurator =>
            {
                var container = new IocContainer(t => t.NameInCSharp());

                HttpReceivingEndpoint httpReceivingEndpoint = HttpReceivingEndpoint.FromUrl(DocumentsConfiguration.FromAppConfig().Url);

                Bootstrap.Application()
                    .ResolveReferencesWith(container)
                    .RegisterBuildAction(c => c.RegisterInstance<IConnectionFactory, SqlConnectionFactory>())
                    .SetupData()
                    .SetupMessaging()
                        .ConfigureReceivingEndpoint(httpReceivingEndpoint)
                        .ConfigureMessageRouting().WireUpRouting()
                    .Initialise();

                Trace.TraceInformation(
                    "Documents service has started up and the types registered into the container are {0}{1}",
                    Environment.NewLine,
                    container.Describe());

                configurator.Service<DocumentsController>(s =>
                {
                    s.ConstructUsing(name => container.Resolve<DocumentsController>());
                    s.WhenStarted(c => c.Start());
                    s.WhenStopped(c => c.Stop());
                });

                configurator.RunAsLocalSystem();
                configurator.SetDescription("Applied Systems Documents Service");
                configurator.SetDisplayName("Applied Systems Documents Service");
                configurator.SetServiceName("Applied Systems Documents Service");
            });
        }
    }

    public class HttpReceivingEndpoint : IReceivingEndpoint
    {
        public Type EndpointBuilderType => typeof(HttpReceivingEndpointBuilder);

        public string Url { get; }

        private HttpReceivingEndpoint(string url)
        {
            Url = url;
        }

        public static HttpReceivingEndpoint FromUrl(string url)
        {
            return new HttpReceivingEndpoint(url);
        }
    }

    public class HttpReceivingEndpointBuilder : IReceivingEndpointBuilder<HttpReceivingEndpoint>
    {
        public IEnumerable<IMessageReceiver> BuildReceivers(HttpReceivingEndpoint endpoint, MessagePipeline pipeline)
        {
            return new List<IMessageReceiver>
            {
                new HttpMessageReceiver(pipeline, endpoint.Url)
            };
        }

        public IEnumerable<ISubscriptionClient> BuildSubscriptionClients(HttpReceivingEndpoint endpoint)
        {
            return Enumerable.Empty<ISubscriptionClient>();
        }
    }

    public class HttpMessageNancyModule : NancyModule
    {
        public HttpMessageNancyModule()
        {
            Post["/messaging", runAsync: true] = async (_, __) =>
            {
                try
                {
                    string request = await Request.Body.ReadAsString();
                    HttpMessageReceiver.Current.Receive(request.DeserialiseFromJson());
                }
                catch (Exception)
                {
                    return Negotiate
                        .WithStatusCode(HttpStatusCode.BadRequest)
                        .WithReasonPhrase("Could not process message");
                }

                return HttpStatusCode.OK;
            };
        }
    }

    public class HttpMessageReceiver : MessageReceiver
    {
        public static HttpMessageReceiver Current { get; private set; }

        private IDisposable webApp;
        private readonly string address;

        public HttpMessageReceiver(MessagePipeline messagePipeline, string address)
            : base(messagePipeline)
        {
            this.address = address;
            Current = this;
        }


        protected override void StartReceiving()
        {
            webApp = WebApp.Start<WebAppStartup>(address);
        }

        public override void StopReceiving()
        {
            webApp.Dispose();
        }

        public void Receive(Message message)
        {
            PutMessageInPipeline(message);
        }
    }

    public class DocumentsNancyBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
        }
    }

    public class WebAppStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll)
                .UseNancy(options => options.PassThroughWhenStatusCodesAre(
                    HttpStatusCode.NotFound,
                    HttpStatusCode.InternalServerError));
        }
    }
}