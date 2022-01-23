namespace Nekono.App.Pos.Models
{
    public class ItemInPOSDetails
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string VendorCode { get; set; }
        public decimal FinalRetailPrice { get; set; }
        public decimal FinalWholesalePrice { get; set; }
        public decimal RetailDiscount { get; set; }
        public decimal WholesaleDiscount { get; set; }
        public int WholesaleMinQty { get; set; }
    }
}
