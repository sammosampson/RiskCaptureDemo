namespace AppliedSystems.RiskCapture.Mapping
{
    public class LookupItemMappingId
    {

        public static LookupItemMappingId Parse(string productLine)
        {
            return new LookupItemMappingId(productLine);
        }

        public static implicit operator string(LookupItemMappingId from)
        {
            return "riskcaptureprojections-riskcapturemaplookup-" + @from.productLine;
        }

        private readonly string productLine;

        private LookupItemMappingId(string productLine)
        {
            this.productLine = productLine;
        }
    }
}