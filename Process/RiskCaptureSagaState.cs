namespace AppliedSystems.Documents.Process
{
    using System.Collections.Generic;

    public class RiskCaptureSagaState
    {
        public string ProductLine { get; set; }
        public List<RiskCaptureSagaRiskSectionState> Sections { get; set; }

        public RiskCaptureSagaState()
        {
            Sections = new List<RiskCaptureSagaRiskSectionState>();
        }
    }
}