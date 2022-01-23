using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Nekono.App.Pos.Models;
using Nekono.App.Pos.Views;

namespace Nekono.App.Pos.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command ClearListCommand { get; set; }
        public ContentPage View { get; set; }

        string barcodeScannedText = string.Empty;
        public string BarcodeScannedText
        {
            get { return barcodeScannedText; }
            set { SetProperty(ref barcodeScannedText, value); }
        }

        public ItemsViewModel(ContentPage view)
        {
            View = view;

            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ClearListCommand = new Command(async () => await ExecuteClearListCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteScannedCompletedCommand()
        {
            IsBusy = true;

            try
            {
                await DataStore.AddItemAsync(new Item
                {
                    Description = BarcodeScannedText,
                    Id = BarcodeScannedText,
                    Text = BarcodeScannedText
                });

                await ExecuteLoadItemsCommand();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteClearListCommand()
        {
            IsBusy = true;

            try
            {
                await DataStore.ClearItemAsync();
                await ExecuteLoadItemsCommand();
                await View.DisplayAlert("Item Cleared", "HEHE", "OK");
                var eBarcode = View.FindByName("eBarcode") as Entry;
                eBarcode.Focus();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

            return;
        }
    }
}