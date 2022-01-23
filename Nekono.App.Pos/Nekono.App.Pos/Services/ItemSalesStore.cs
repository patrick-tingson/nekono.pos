using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nekono.App.Pos.Models;

namespace Nekono.App.Pos.Services
{
    public class ItemSalesStore : IDataStore<InventoryDetails>
    {
        readonly List<InventoryDetails> items;

        public ItemSalesStore()
        {
            items = new List<InventoryDetails>();
        }

        public async Task<bool> AddItemAsync(InventoryDetails item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public Task ClearItemAsync()
        {
            items.Clear();

            return Task.CompletedTask;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((InventoryDetails arg) => arg.ItemNo == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<InventoryDetails> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.ItemNo == id));
        }

        public async Task<IEnumerable<InventoryDetails>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items.OrderByDescending(s => s.TranTime));
        }

        public async Task<bool> UpdateItemAsync(InventoryDetails item)
        {
            var oldItem = items.Where((InventoryDetails arg) => arg.ItemNo == item.ItemNo).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }
    }
}