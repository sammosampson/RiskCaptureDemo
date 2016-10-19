namespace AppliedSystems.DataWarehouse
{
    using System.Collections.Generic;

    public class RiskCaptureProcessState
    {
        public string ProductLine { get; set; }
        public List<RiskCaptureProcessRiskSectionState> Sections { get; set; }

        public RiskCaptureProcessState()
        {
            Sections = new List<RiskCaptureProcessRiskSectionState>();
        }
    }
}