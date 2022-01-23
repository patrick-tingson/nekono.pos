using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nekono.App.Pos.Models;
using Xamarin.Forms;

namespace Nekono.App.Pos.Services
{
    public class ItemsStore : IDataStore<ItemInPOSDetails>
    {
        List<ItemInPOSDetails> items;
        IItemsServices ItemsServices => DependencyService.Get<IItemsServices>();

        public ItemsStore()
        {
            items = new List<ItemInPOSDetails>();
        }

        public Task<bool> AddItemAsync(ItemInPOSDetails item)
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

        public async Task<ItemInPOSDetails> GetItemAsync(string id)
        {
            if (items.Count == 0)
            {
                items = await ItemsServices.GetItemInPOSDetails();
            }

            return await Task.FromResult(items.FirstOrDefault(s => s.ItemNo == id));
        }

        public async Task<IEnumerable<ItemInPOSDetails>> GetItemsAsync(bool forceRefresh = false)
        {
            if(items.Count == 0)
            {
                items = await ItemsServices.GetItemInPOSDetails();
            }

            return null;
        }

        public Task<bool> UpdateItemAsync(ItemInPOSDetails item)
        {
            throw new NotImplementedException();
        }
    }
}