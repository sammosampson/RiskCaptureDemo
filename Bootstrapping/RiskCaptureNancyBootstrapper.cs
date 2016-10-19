namespace AppliedSystems.RiskCapture.Bootstrapping
{
    using AppliedSystems.Messaging.Infrastructure;
    using Nancy;
    using Nancy.Bootstrapper;
    using Nancy.TinyIoc;

    public class RiskCaptureNancyBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
        }
    }
}