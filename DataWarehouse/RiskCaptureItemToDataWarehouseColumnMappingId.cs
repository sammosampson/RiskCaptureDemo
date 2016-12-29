namespace AppliedSystems.DataWarehouse
{
    public class RiskCaptureItemToDataWarehouseColumnMappingId
    {

        public static RiskCaptureItemToDataWarehouseColumnMappingId Parse(string productLine)
        {
            return new RiskCaptureItemToDataWarehouseColumnMappingId(productLine);
        }

        public static implicit operator string(RiskCaptureItemToDataWarehouseColumnMappingId from)
        {
            return "riskcaptureprojections-riskcaptureitemtodatawarehousecolumnlookup-" + @from.productLine;
        }

        private readonly string productLine;

        private RiskCaptureItemToDataWarehouseColumnMappingId(string productLine)
        {
            this.productLine = productLine;
        }
    }
}