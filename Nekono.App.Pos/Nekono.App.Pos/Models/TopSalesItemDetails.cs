namespace Nekono.App.Pos.Models
{
    public class TopSalesItemDetails
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public int TotalQty { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
