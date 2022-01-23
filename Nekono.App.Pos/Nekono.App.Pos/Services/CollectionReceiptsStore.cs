using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nekono.App.Pos.Models;
using Nekono.App.Pos.Utility;
using Xamarin.Forms;

namespace Nekono.App.Pos.Services
{
    public class CollectionReceiptsStore : IDataStore<CollectionReceiptDetails>
    {
        List<CollectionReceiptDetails> collectionReceipt;
        ICollectionReceiptServices CollectionReceiptServices => DependencyService.Get<ICollectionReceiptServices>();

        public CollectionReceiptsStore()
        {
            #region Mock Data
            //collectionReceipt = new List<CollectionReceiptDetails>()
            //{
            //    new CollectionReceiptDetails {
            //        CollectionReceiptNo = "CR00000001",
            //        Type = "CASH",
            //        Active = 0,
            //        BranchCode = "GEN. T VAL",
            //        CreatedBy = "patrick.tingson",
            //        TotalAmount = 9999.00M,
            //        TranDate = "2021/02/14",
            //        TranTime = "01:03:55",
            //        InventoryDetails = new List<InventoryDetails>
            //        {
            //            new InventoryDetails
            //            {
            //                ItemNo = "1235678",
            //                ItemName = "EVO Renegade XXL",
            //                Type = "RSAL",
            //                Qty = 1,
            //                AmountPerQty = 123.00M,
            //                Discount = 0,
            //                TotalAmount = 123.00M,
            //                TranDate = Variables.TranDate(),
            //                TranTime = Variables.TranTime()
            //            },
            //            new InventoryDetails
            //            {
            //                ItemNo = "0987654",
            //                ItemName = "KYT Meow XXL",
            //                Type = "RSAL",
            //                Qty = 1,
            //                AmountPerQty = 777,
            //                Discount = 0,
            //                TotalAmount = 777,
            //                TranDate = Variables.TranDate(),
            //                TranTime = Variables.TranTime()
            //            },
            //            new InventoryDetails
            //            {
            //                ItemNo = "1235678",
            //                ItemName = "EVO Renegade XXL",
            //                Type = "RSAL",
            //                Qty = 1,
            //                AmountPerQty = 123.00M,
            //                Discount = 0,
            //                TotalAmount = 123.00M,
            //                TranDate = Variables.TranDate(),
            //                TranTime = Variables.TranTime()
            //            },
            //            new InventoryDetails
            //            {
            //                ItemNo = "0987654",
            //                ItemName = "KYT Meow XXL",
            //                Type = "RSAL",
            //                Qty = 1,
            //                AmountPerQty = 777,
            //                Discount = 0,
            //                TotalAmount = 777,
            //                TranDate = Variables.TranDate(),
            //                TranTime = Variables.TranTime()
            //            },
            //            new InventoryDetails
            //            {
            //                ItemNo = "1235678",
            //                ItemName = "EVO Renegade XXL",
            //                Type = "RSAL",
            //                Qty = 1,
            //                AmountPerQty = 123.00M,
            //                Discount = 0,
            //                TotalAmount = 123.00M,
            //                TranDate = Variables.TranDate(),
            //                TranTime = Variables.TranTime()
            //            },
            //            new InventoryDetails
            //            {
            //                ItemNo = "0987654",
            //                ItemName = "KYT Meow XXL",
            //                Type = "RSAL",
            //                Qty = 1,
            //                AmountPerQty = 777,
            //                Discount = 0,
            //                TotalAmount = 777,
            //                TranDate = Variables.TranDate(),
            //                TranTime = Variables.TranTime()
            //            }
            //        }
            //    },
            //    new CollectionReceiptDetails {
            //        CollectionReceiptNo = "CR00000001",
            //        Type = "CARD",
            //        Bank = "BDO CREDIT",
            //        RefNo = "123456",
            //        Active = 0,
            //        BranchCode = "GEN. T VAL",
            //        CreatedBy = "patrick.tingson",
            //        TotalAmount = 7777.00M,
            //        TranDate = "2021/02/14",
            //        TranTime = "15:03:55",
            //        InventoryDetails = new List<InventoryDetails>
            //        {
            //            new InventoryDetails
            //            {
            //                ItemNo = "0987654",
            //                ItemName = "KYT Meow XXL",
            //                Type = "RSAL",
            //                Qty = 1,
            //                AmountPerQty = 777,
            //                Discount = 0,
            //                TotalAmount = 777,
            //                TranDate = Variables.TranDate(),
            //                TranTime = Variables.TranTime()
            //            }
            //        }
            //    },
            //    new CollectionReceiptDetails {
            //        CollectionReceiptNo = "CR00000001",
            //        Type = "CARD",
            //        Bank = "BDO CREDIT",
            //        RefNo = "123456",
            //        Active = 1,
            //        Remarks = "TEST VOID",
            //        BranchCode = "GEN. T VAL",
            //        CreatedBy = "patrick.tingson",
            //        TotalAmount = 6666.00M,
            //        TranDate = "2021/02/15",
            //        TranTime = "15:03:55",
            //        InventoryDetails = new List<InventoryDetails>
            //        {
            //            new InventoryDetails
            //            {
            //                ItemNo = "0987654",
            //                ItemName = "KYT Meow XXL",
            //                Type = "WSAL",
            //                Qty = 1,
            //                AmountPerQty = 777,
            //                Discount = 0,
            //                TotalAmount = 777,
            //                TranDate = Variables.TranDate(),
            //                TranTime = Variables.TranTime()
            //            }
            //        }
            //    },
            //};
            #endregion

            collectionReceipt = new List<CollectionReceiptDetails>();
        }

        public Task<bool> AddItemAsync(CollectionReceiptDetails item)
        {
            throw new NotImplementedException();
        }

        public Task ClearItemAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<CollectionReceiptDetails> GetItemAsync(string id)
        {
            return await Task.FromResult(collectionReceipt.FirstOrDefault(s => s.CollectionReceiptNo == id));
        }

        public async Task<IEnumerable<CollectionReceiptDetails>> GetItemsAsync(bool forceRefresh = false)
        {
            collectionReceipt = await CollectionReceiptServices.GetToday();

            return await Task.FromResult(collectionReceipt.OrderByDescending(o => o.TranTime));
        }

        public Task<bool> UpdateItemAsync(CollectionReceiptDetails item)
        {
            throw new NotImplementedException();
        }
    }
}