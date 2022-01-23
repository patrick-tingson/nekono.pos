namespace Nekono.App.Pos.Models
{
    public class InventoryDetails
    {
        public string InventoryNo { get; set; }
        public string Type { get; set; }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string TranDate { get; set; }
        public string TranTime { get; set; }
        public string CreatedBy { get; set; }
        public string Remarks { get; set; }
        public string BranchCode { get; set; }
        public string CollectionReceiptNo { get; set; }
        public string LastUpdatedDate { get; set; }
        public string LastUpdatedTime { get; set; }
        public string LastUpdatedBy { get; set; }
        public int Qty { get; set; }
        public decimal AmountPerQty { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public int Active { get; set; }
    }
}
