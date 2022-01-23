using Nekono.App.Pos.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nekono.App.Pos.Services
{
    public interface IItemsServices
    {
        Task<List<ItemInPOSDetails>> GetItemInPOSDetails();
        Task<List<InventoryDetails>> GetItemsInReceipt(string collectionReceipt);
    }
}