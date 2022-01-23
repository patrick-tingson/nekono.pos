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
    public partial class ReceiptsPage : ContentPage
    {
        ReceiptsViewModel viewModel;

        public ReceiptsPage()
        {
            InitializeComponent();
            
            BindingContext = viewModel = new ReceiptsViewModel();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await viewModel.ExecuteLoadItemsCommand();

            //if (viewModel.CollectionReceipts.Count == 0)
            //    viewModel.IsBusy = true;

        }

        async void OnSelectedReceipt(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (CollectionReceiptDetails)layout.BindingContext;

            await viewModel.ExecuteLoadMoreReceiptDetailsCommand(item);
        }

        async void OnLoadItemsCommand(object sender, EventArgs args)
        {
            await viewModel.ExecuteLoadItemsCommand();
        }
    }
}