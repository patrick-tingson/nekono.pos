using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Nekono.App.Pos.Services;
using Nekono.App.Pos.Views;

namespace Nekono.App.Pos
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<ItemSalesStore>();
            DependencyService.Register<ItemsStore>();
            DependencyService.Register<CollectionReceiptsStore>();
            DependencyService.Register<LoginServices>();
            DependencyService.Register<ItemsServices>();
            DependencyService.Register<CollectionReceiptServices>();

            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
