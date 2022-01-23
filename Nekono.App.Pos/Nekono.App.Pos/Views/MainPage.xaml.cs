using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Nekono.App.Pos.Models;
using Nekono.App.Pos.Models;
using Nekono.App.Pos.Utility;
using Xamarin.Essentials;

namespace Nekono.App.Pos.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.Sales, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Sales:
                        MenuPages.Add(id, new NavigationPage(new SalesPage()));
                        break;
                    case (int)MenuItemType.Receipts:
                        MenuPages.Add(id, new NavigationPage(new ReceiptsPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }

        private Task TestPrintCash()
        {
            var collectionReceiptDetails = new CollectionReceiptDetails();
            collectionReceiptDetails.CollectionReceiptNo = "CR00000001";
            collectionReceiptDetails.Bank = "";
            collectionReceiptDetails.BranchCode = "GEN. T VAL";
            collectionReceiptDetails.CreatedBy = "patrick.tingson";
            collectionReceiptDetails.TotalAmount = 6001.00M;
            collectionReceiptDetails.TranDate = "2021/02/13";
            collectionReceiptDetails.TranTime = "12:57:00";
            collectionReceiptDetails.Type = "CASH";
            collectionReceiptDetails.InventoryDetails = new List<InventoryDetails> {
            new InventoryDetails
            {
                ItemNo = "IN00001",
                ItemName = "EVO Renegade BL XXL",
                Qty = 1,
                AmountPerQty = 3000.50M,
                TotalAmount = 1 * 3000.50M,
                Discount = 0
            },
            new InventoryDetails
            {
                ItemNo = "IN00002",
                ItemName = "KYT Spyder RD LG",
                Qty = 1,
                AmountPerQty = 3000.50M,
                TotalAmount = 1 * 3000.50M,
                Discount = 0.20M
            }
        };

            Functions.PrinterSDK.PrintReceipt(collectionReceiptDetails);

            return Task.CompletedTask;
        }

        private Task TestPrintCard()
        {
            var collectionReceiptDetails = new CollectionReceiptDetails();
            collectionReceiptDetails.CollectionReceiptNo = "CR00000001";
            collectionReceiptDetails.BranchCode = "GEN. T VAL";
            collectionReceiptDetails.CreatedBy = "patrick.tingson";
            collectionReceiptDetails.TotalAmount = 6001.00M;
            collectionReceiptDetails.TranDate = "2021/02/13";
            collectionReceiptDetails.TranTime = "12:57:00";
            collectionReceiptDetails.Type = "CARD";
            collectionReceiptDetails.Bank = "BDO CREDIT";
            collectionReceiptDetails.RefNo = "1234567890";
            collectionReceiptDetails.InventoryDetails = new List<InventoryDetails> {
            new InventoryDetails
            {
                ItemNo = "IN00001",
                ItemName = "EVO Renegade BL XXL",
                Qty = 1,
                AmountPerQty = 3000.50M,
                TotalAmount = 1 * 3000.50M,
                Discount = 0
            },
            new InventoryDetails
            {
                ItemNo = "IN00002",
                ItemName = "KYT Spyder RD LG",
                Qty = 1,
                AmountPerQty = 3000.50M,
                TotalAmount = 1 * 3000.50M,
                Discount = 0.20M
            }
        };

            Functions.PrinterSDK.PrintReceipt(collectionReceiptDetails);

            return Task.CompletedTask;
        }
    }
}