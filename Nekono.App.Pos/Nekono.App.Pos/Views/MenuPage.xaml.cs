using Nekono.App.Pos.Models;
using Nekono.App.Pos.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nekono.App.Pos.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Sales, Title="Sales" },
                new HomeMenuItem {Id = MenuItemType.Receipts, Title="Receipts" }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            LoggedInUsername.Text = await SecureStorage.GetAsync("username");
        }

        async void OnLogoutCommand(object sender, EventArgs args)
        {
            if (await DisplayAlert("NEKONOMOTO", "Are you sure do you want to logout?", "Yes", "Cancel"))
            {
                SecureStorage.RemoveAll();
                App.Current.MainPage = new LoginPage();
            }
        }

        async void OnKickDrawer(object sender, EventArgs args)
        {
            try
            {
                await Functions.PrinterSDK.KickDrawer();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("NEKONOMOTO", $"Unable to open drawer. Please turn on or check your printer.", "Ok");
            }
        }
    }
}