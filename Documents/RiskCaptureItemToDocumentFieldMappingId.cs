namespace AppliedSystems.Documents
{
    public class RiskCaptureItemToDocumentFieldMappingId
    {

        public static RiskCaptureItemToDocumentFieldMappingId Parse(string productLine)
        {
            return new RiskCaptureItemToDocumentFieldMappingId(productLine);
        }

        public static implicit operator string(RiskCaptureItemToDocumentFieldMappingId from)
        {
            return "riskcaptureprojections-riskcaptureitemtodocumentfieldlookup-" + @from.productLine;
        }

        private readonly string productLine;

        private RiskCaptureItemToDocumentFieldMappingId(string productLine)
        {
            this.productLine = productLine;
        }
    }
}