using Nekono.App.Pos.Models;
using Nekono.App.Pos.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nekono.App.Pos.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SalesPage : ContentPage
    {
        SalesViewModel viewModel;

        public SalesPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new SalesViewModel(this);
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (InventoryDetails)layout.BindingContext;
            //await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
            await DisplayAlert("Item Details", $"{item.ItemName} - {item.Qty} x {item.AmountPerQty}", "Ok");
        }

        void TapCashPayment(object sender, EventArgs args)
        {
            CashPaymentMethod.IsChecked = true;
        }

        void TapCardPayment(object sender, EventArgs args)
        {
            CardPaymentMethod.IsChecked = true;
        }

        async void TypeChangedOnSelectedItem(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (InventoryDetails)layout.BindingContext;
            var priceEntered = 0M;
            var isPriceEntry = false;

            var priceType = await DisplayActionSheet($"Do you want to change the price of this item {item.ItemName}?", "Cancel", null, "Retail Price", "Wholesale Price", "Price Entry");

            if(priceType == "Retail Price")
            {
                priceType = "RSAL";
            }
            else if(priceType == "Wholesale Price")
            {
                priceType = "WSAL";
            }
            else if (priceType == "Price Entry")
            {
                priceType = "WSAL";

                var priceEntry = await DisplayPromptAsync("NEKONOMOTO", "Please enter the price.", "Confirm", "Cancel", "0.00", 20, Keyboard.Numeric);

                if (priceEntry == null || priceEntry?.Length == 0)
                {
                    return;
                }

                priceEntered = Convert.ToDecimal(priceEntry);
                isPriceEntry = true;
            }

            if (priceType != "Cancel")
            {
                await viewModel.ExecuteUpdatePriceTypeCommand(item.ItemNo, priceType, priceEntered, isPriceEntry);
            }
            else
            {
                eBarcode.Focus();
            }
        }

        async void OnStepperQtyValueChanged(object sender, ValueChangedEventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (InventoryDetails)layout.BindingContext;
            int qty = Convert.ToInt32(args.NewValue - args.OldValue);

            await viewModel.ExecuteUpdateQtyCommand(item.ItemNo, qty);
        }

        void TapToFocusOnBarcodeEntry(object sender, EventArgs args)
        {
            eBarcode.Focus();
        }

        async void ClearItem_Clicked(object sender, EventArgs e)
        {
            if(await DisplayAlert("NEKONOMOTO", "Do you want to clear the item(s)?", "Ok", "Cancel"))
            {
                //await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
                await viewModel.ExecuteClearListCommand();
            }
            else
            {
                eBarcode.Focus();
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //if (viewModel.Items.Count == 0)
            //    viewModel.IsBusy = true;

            await Task.Delay(500);
            await viewModel.ExecuteItemsInitialization();
        }

        async void OnCompleted(object sender, EventArgs e)
        {
            // No need to distinguish btw scanned vs typed in search value, <Enter> handles it for you same way
            //DisplayAlert("Barcode", "You have scanned " + ((Entry)sender).Text, "OK");

            await viewModel.ExecuteScannedCompletedCommand();

            //// clean up and refocus if you need to do multiple scans
            //((Entry)sender).Text = string.Empty;
            //((Entry)sender).Focus();

            // initiate some operation such as search database for scanned item or so
            // SearchItem((Entry)sender).Text);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}