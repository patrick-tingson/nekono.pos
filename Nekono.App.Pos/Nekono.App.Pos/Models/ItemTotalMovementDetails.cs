namespace Nekono.App.Pos.Models
{
    public class ItemTotalMovementDetails
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public int AvailableStock { get; set; }
        public int RestockQty { get; set; }
        public int SaleQty { get; set; }
        public decimal SaleAmount { get; set; }
        public int WholesaleQty { get; set; }
        public decimal WholesaleAmount { get; set; }
        public int TotalSaleQty { get; set; }
        public decimal TotalSaleAmount { get; set; }
        public int PullQty { get; set; }
        public int DamageQty { get; set; }
    }
}
