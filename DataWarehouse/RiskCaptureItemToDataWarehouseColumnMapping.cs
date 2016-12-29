namespace AppliedSystems.DataWarehouse
{
    using System;

    public class RiskCaptureItemToDataWarehouseColumnMapping
    {
        public Guid ItemId { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
    }
}