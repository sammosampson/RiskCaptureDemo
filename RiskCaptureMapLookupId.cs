namespace AppliedSystems.RiskCapture
{
    public class RiskCaptureMapLookupId
    {

        public static RiskCaptureMapLookupId Parse(string productLine)
        {
            return new RiskCaptureMapLookupId(productLine);
        }

        public static implicit operator string(RiskCaptureMapLookupId from)
        {
            return "riskcaptureprojections-riskcapturemaplookup-" + @from.productLine;
        }

        private readonly string productLine;

        private RiskCaptureMapLookupId(string productLine)
        {
            this.productLine = productLine;
        }
    }
}