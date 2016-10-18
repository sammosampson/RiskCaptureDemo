namespace AppliedSystems.DataWarehouse
{
    using System.Collections.Generic;

    public class RiskCaptureSagaRiskSectionState
    {
        public int RiskSectionId { get; set; }
        public string RiskSectionName { get; set; }
        public List<RiskCaptureSagaRiskItemState> Items { get; set; }

        public RiskCaptureSagaRiskSectionState()
        {
            Items = new List<RiskCaptureSagaRiskItemState>();
        }
    }
}