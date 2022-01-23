using System;
using System.Collections.Generic;
using System.Text;

namespace Nekono.App.Pos.Models
{
    public class ItemDetails
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public string ItemGroup { get; set; }
        public string ItemGroupDesc { get; set; }
        public string Specification { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string TranDate { get; set; }
        public string TranTime { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedDate { get; set; }
        public string LastUpdatedTime { get; set; }
        public string LastUpdatedBy { get; set; }
        public string Remarks { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal WholesalePrice { get; set; }
        public decimal RetailDiscount { get; set; }
        public decimal WholesaleDiscount { get; set; }
        public decimal FinalRetailPrice { get; set; }
        public decimal FinalWholesalePrice { get; set; }
        public int WholesaleMinQty { get; set; }
        public int AvailableStocks { get; set; }
        public int Active { get; set; }
    }
}
