using Nekono.App.Pos.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nekono.App.Pos.Dependency
{
    public interface IPrinterSDK
    {
        bool Connect();

        bool Close();

        Task<bool> PrintReceipt(CollectionReceiptDetails collectionReceiptDetails, bool storeCopy = false, bool rePrint = false);

        Task KickDrawer();

        Task<bool> PrintAllReceipt(IEnumerable<CollectionReceiptDetails> collectionReceiptDetails, IEnumerable<InventoryDetails> inventoryDetails);
    }
}
