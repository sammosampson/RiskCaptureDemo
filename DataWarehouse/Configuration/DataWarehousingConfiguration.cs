namespace AppliedSystems.DataWarehouse.Configuration
{
    using AppliedSystems.Configuration;

    public class DataWarehousingConfiguration
    {
        public static DataWarehousingConfiguration FromAppConfig()
        {
            DataWarehousingConfiguration configuration = ConfigurationReader.Read<DataWarehousingConfiguration>();
            return configuration;
        }

        public string RiskCaptureUrl { get; set; }
    }
}