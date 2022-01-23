namespace Nekono.App.Pos.Models
{
    public class ItemHistoryDetails
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public string ItemGroup { get; set; }
        public string ItemGroupDesc { get; set; }
        public string Specification { get; set; }
        public string VendorCode { get; set; }
        public string LastUpdatedDate { get; set; }
        public string LastUpdatedTime { get; set; }
        public string LastUpdatedBy { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal WholesalePrice { get; set; }
        public decimal RetailDiscount { get; set; }
        public decimal WholesaleDiscount { get; set; }
        public int WholesaleMinQty { get; set; }
        public int Active { get; set; }
    }
}
