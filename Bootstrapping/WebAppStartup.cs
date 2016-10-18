namespace AppliedSystems.RiskCapture.Bootstrapping
{
    using Microsoft.Owin.Cors;
    using Nancy;
    using Nancy.Owin;
    using Owin;

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