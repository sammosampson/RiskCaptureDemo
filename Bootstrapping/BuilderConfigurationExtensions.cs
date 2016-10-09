namespace AppliedSystems.RiskCapture.Bootstrapping
{
    using SystemDot.Bootstrapping;

    public static class BuilderConfigurationExtensions
    {
        public static BootstrapBuilderConfiguration ConfigureRiskCapture(this BootstrapBuilderConfiguration config)
        {
            config.RegisterBuildAction(c => c.RegisterRiskCapture());
            return config;
        }
    }
}