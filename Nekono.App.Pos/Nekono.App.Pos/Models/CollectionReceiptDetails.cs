using System;
using System.Collections.Generic;

namespace Nekono.App.Pos.Models
{
    public class CollectionReceiptDetails
    {
        public string CollectionReceiptNo { get; set; }
        public string TranDate { get; set; }
        public string TranTime { get; set; }
        public decimal TotalAmount { get; set; }
        public string Remarks { get; set; }
        public string BranchCode { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedDate { get; set; }
        public string LastUpdatedTime { get; set; }
        public string LastUpdatedBy { get; set; }
        public string Type { get; set; }
        public string RefNo { get; set; }
        public string Bank { get; set; }
        public int Active { get; set; }
        public IEnumerable<InventoryDetails> InventoryDetails { get; set; }
        public string ReceiptStatus
        {
            get
            {
                if(Active == 0)
                {
                    return "SALE";
                }
                else
                {
                    return "VOID";
                }
            }
        }
        public string StatusColor
        {
            get
            {
                if (Active == 0)
                {
                    return "ForestGreen";
                }
                else
                {
                    return "Red";
                }
            }
        }

        public string TransactionDate
        {
            get
            {
                DateTime dt;

                if(DateTime.TryParse($"{TranDate} {TranTime}", out dt))
                {
                    return dt.ToString("dddd, dd MMMM yyyy h:mm tt");
                }
                else
                {
                    return $"{TranDate} {TranTime}";
                }
            }
        }
    }
}
