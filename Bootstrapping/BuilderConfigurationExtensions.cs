namespace AppliedSystems.RiskCapture.Bootstrapping
{
    using SystemDot.Bootstrapping;
    using Nancy;
    using Nancy.Owin;
    using Owin;

    public static class BuilderConfigurationExtensions
    {
        public static BootstrapBuilderConfiguration ConfigureRiskCapture(this BootstrapBuilderConfiguration config)
        {
            config.RegisterBuildAction(c => c.RegisterRiskCapture());
            return config;
        }
    }
}