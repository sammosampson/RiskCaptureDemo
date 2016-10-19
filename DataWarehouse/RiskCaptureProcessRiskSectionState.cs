namespace AppliedSystems.DataWarehouse
{
    using System.Collections.Generic;

    public class RiskCaptureProcessRiskSectionState
    {
        public int RiskSectionId { get; set; }
        public string RiskSectionName { get; set; }
        public List<RiskCaptureProcessRiskItemState> Items { get; set; }

        public RiskCaptureProcessRiskSectionState()
        {
            Items = new List<RiskCaptureProcessRiskItemState>();
        }
    }
}