namespace AppliedSystems.DataWarehouse
{
    using System.Collections.Generic;
    using Messaging.Infrastructure.Events;

    public class DataWarehouseSagaState
    {
        public string ProductLine { get; set; }
        public List<DataWarehouseSagaRiskSectionState> Sections { get; set; }

        public DataWarehouseSagaState()
        {
            Sections = new List<DataWarehouseSagaRiskSectionState>();
        }
    }

    public class DataWarehouseSagaRiskSectionState
    {
        public int RiskSectionId { get; set; }
        public string RiskSectionName { get; set; }
        public List<DataWarehouseSagaRiskItemState> Items { get; set; }

        public DataWarehouseSagaRiskSectionState()
        {
            Items = new List<DataWarehouseSagaRiskItemState>();
        }
    }

    public class DataWarehouseSagaRiskItemState
    {
        public int RiskItemId { get; set; }
        public string RiskItemName { get; set; }
    }
}