using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Nekono.App.Pos.Models;
using Nekono.App.Pos.Views;
using Nekono.App.Pos.ViewModels;

namespace Nekono.App.Pos.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel(this);
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (Item)layout.BindingContext;
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.IsBusy = true;

            await Task.Delay(500);
            await Task.Run(() =>
            {
                eBarcode.Focus();
            });
        }

        //void OnTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    Console.WriteLine("Was: " + e.OldTextValue);
        //    Console.WriteLine("Entered " + e.NewTextValue);

        //    // you could use this to do some parsing or whatever you need
        //}

        async void OnCompleted(object sender, EventArgs e)
        {
            // No need to distinguish btw scanned vs typed in search value, <Enter> handles it for you same way
            //DisplayAlert("Barcode", "You have scanned " + ((Entry)sender).Text, "OK");

            await viewModel.ExecuteScannedCompletedCommand();

            // clean up and refocus if you need to do multiple scans
            ((Entry)sender).Text = string.Empty;
            ((Entry)sender).Focus();

            // initiate some operation such as search database for scanned item or so
            // SearchItem((Entry)sender).Text);
        }
    }
}