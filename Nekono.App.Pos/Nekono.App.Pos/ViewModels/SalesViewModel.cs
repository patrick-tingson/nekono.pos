using Nekono.App.Pos.Models;
using Nekono.App.Pos.Utility;
using Nekono.App.Pos.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Nekono.App.Pos.ViewModels
{
    public class SalesViewModel : BaseViewModel
    {
        public ObservableCollection<InventoryDetails> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command ClearListCommand { get; set; }
        public Command SubmitSalesCommand { get; set; }
        public ContentPage View { get; set; }

        string bank = string.Empty;
        public string Bank
        {
            get { return bank; }
            set { SetProperty(ref bank, value); }
        }

        string bankRefNo = string.Empty;
        public string BankRefNo
        {
            get { return bankRefNo; }
            set { SetProperty(ref bankRefNo, value); }
        }

        string barcodeScannedText = string.Empty;
        public string BarcodeScannedText
        {
            get { return barcodeScannedText; }
            set { SetProperty(ref barcodeScannedText, value); }
        }

        decimal totalSalesAmount = 0;
        public decimal TotalSalesAmount
        {
            get { return totalSalesAmount; }
            set { SetProperty(ref totalSalesAmount, value); }
        }

        bool isCardPayment = false;
        public bool IsCardPayment
        {
            get { return isCardPayment; }
            set { SetProperty(ref isCardPayment, value); }
        }

        bool isCashPayment = true;
        public bool IsCashPayment
        {
            get { return isCashPayment; }
            set { SetProperty(ref isCashPayment, value); }
        }

        private string BRANCH_CODE = "GEN. T VAL";

        public SalesViewModel(ContentPage view)
        {
            this.View = view; 
            
            Title = "Sales";
            Items = new ObservableCollection<InventoryDetails>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ClearListCommand = new Command(async () => await ExecuteClearListCommand());
            SubmitSalesCommand = new Command(async () => await ExecuteSubmitSalesCommand());

            //MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            //{
            //    var newItem = item as Item;
            //    Items.Add(newItem);
            //    await DataStore.AddItemAsync(newItem);
            //});
        }

        private async Task ExecuteSubmitSalesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (IsCardPayment)
                {
                    if (Bank.Length == 0)
                    {
                        await App.Current.MainPage.DisplayAlert("Error Encountered", "Please enter the card bank", "Ok");
                        await TextSetFocus("txtBank");

                        return;

                    }
                    else if (BankRefNo.Length == 0)
                    {
                        await App.Current.MainPage.DisplayAlert("Error Encountered", "Please enter the bank transaction ref.#", "Ok");
                        await TextSetFocus("txtBankRefNo");

                        return;
                    }
                }

                var confirmItems = new StringBuilder();
                var items = await ItemSalesStore.GetItemsAsync(true);
                var itemsCount = 0;

                var collectionReceipt = new CollectionReceiptDetails
                {
                    CreatedBy = await SecureStorage.GetAsync("username"),
                    Type = IsCashPayment ? "CASH" : "CARD",
                    Bank = IsCardPayment ? Bank : "",
                    RefNo = IsCardPayment ? BankRefNo : "",
                    TotalAmount = TotalSalesAmount,
                    BranchCode = BRANCH_CODE,
                    TranDate = Variables.TranDate(),
                    TranTime = Variables.TranTime(),
                    InventoryDetails = items
                };

                foreach (var item in items)
                {
                    confirmItems.Append(Functions.WriteDoubleLn(item.ItemName.PadRight(20, ' '), $"PHP {item.TotalAmount:N}".PadLeft(50, ' ')));
                    confirmItems.Append(Functions.WriteDoubleLn($"{item.Qty} X {item.AmountPerQty:N}", ""));
                    itemsCount++;
                }

                if(itemsCount == 0)
                {
                    return;
                }

                if (await App.Current.MainPage.DisplayAlert("Please confirm the item(s)", confirmItems.ToString(), "Confirm", "Cancel"))
                {
                    var salesReceiptNoResult = await CollectionReceiptServices.Sale(collectionReceipt);

                    if(salesReceiptNoResult.Length > 0)
                    {
                        collectionReceipt.CollectionReceiptNo = salesReceiptNoResult;
                        
                        try
                        {
                            Functions.PrinterSDK.PrintReceipt(collectionReceipt, true);

                            await App.Current.MainPage.DisplayAlert("NEKONOMOTO", $"Sales request has been successfully saved! \nReceipt #: {salesReceiptNoResult}", "Ok");

                            await Functions.PrinterSDK.PrintReceipt(collectionReceipt);
                        }
                        catch (Exception ex)
                        {
                            await App.Current.MainPage.DisplayAlert("NEKONOMOTO", $"Sales request has been successfully saved! \nReceipt #: {salesReceiptNoResult}\nUnable to print receipt. Please turn on or check your printer.", "Ok");
                        }

                        await ExecuteClearListCommand();
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error Encountered", "Sales request failed. Please contact your system admin.", "Ok");
                    }
                }

                await TextSetFocus();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error Encountered", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteDeleteItemCommand(string itemNo)
        {
            IsBusy = true;

            try
            {
                await ItemSalesStore.DeleteItemAsync(itemNo);

                await ExecuteLoadItemsCommand();

                await TextSetFocus();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error Encountered", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                TotalSalesAmount = 0;
                var items = await ItemSalesStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                    TotalSalesAmount += item.TotalAmount;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error Encountered", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteUpdatePriceTypeCommand(string itemNo, string priceType, decimal priceEntered = 0, bool isPriceEntry = false)
        {
            IsBusy = true;

            try
            {
                var searchItem = await ItemsStore.GetItemAsync(itemNo);

                if (searchItem != null)
                {
                    var existedItem = await ItemSalesStore.GetItemAsync(itemNo);

                    if (existedItem != null)
                    {
                        var amountPerQty = 0M;
                        var discount = 0M;

                        if (priceType == "WSAL")
                        {
                            amountPerQty = isPriceEntry ? priceEntered : searchItem.FinalWholesalePrice;
                            discount = searchItem.WholesaleDiscount;
                        }
                        else if(priceType == "RSAL")
                        {
                            amountPerQty = searchItem.FinalRetailPrice;
                            discount = searchItem.RetailDiscount;
                        }
                        else
                        {
                            return;
                        }

                        await ItemSalesStore.UpdateItemAsync(new InventoryDetails
                        {
                            ItemNo = existedItem.ItemNo,
                            ItemName = existedItem.ItemName,
                            Type = priceType,
                            Qty = existedItem.Qty,
                            AmountPerQty = amountPerQty,
                            Discount = discount,
                            TotalAmount = amountPerQty * existedItem.Qty,
                            TranDate = existedItem.TranDate,
                            TranTime = existedItem.TranTime,
                            VendorCode = existedItem.VendorCode,
                            BranchCode = existedItem.BranchCode
                        }); 

                        await ExecuteLoadItemsCommand();
                    }
                }

                await TextSetFocus();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error Encountered", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteUpdateQtyCommand(string itemNo, int qty)
        {
            IsBusy = true;

            try
            {
                var existedItem = await ItemSalesStore.GetItemAsync(itemNo);

                if (existedItem != null)
                {
                    var newQty = existedItem.Qty + qty;

                    if(newQty != 0)
                    {
                        await ItemSalesStore.UpdateItemAsync(new InventoryDetails
                        {
                            ItemNo = existedItem.ItemNo,
                            ItemName = existedItem.ItemName,
                            Type = existedItem.Type,
                            Qty = newQty,
                            AmountPerQty = existedItem.AmountPerQty,
                            Discount = existedItem.Discount,
                            TotalAmount = existedItem.AmountPerQty * newQty,
                            TranDate = existedItem.TranDate,
                            TranTime = existedItem.TranTime,
                            VendorCode = existedItem.VendorCode,
                            BranchCode = existedItem.BranchCode
                        });
                    }
                    else
                    {
                        await ItemSalesStore.DeleteItemAsync(itemNo);
                    }
                }

                await ExecuteLoadItemsCommand();

                await TextSetFocus();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error Encountered", ex.Message, "Ok");
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
                var searchItem = await ItemsStore.GetItemAsync(BarcodeScannedText);

                if(searchItem != null)
                {
                    var existedItem = await ItemSalesStore.GetItemAsync(BarcodeScannedText);

                    if(existedItem != null)
                    {
                        await ItemSalesStore.UpdateItemAsync(new InventoryDetails
                        {
                            ItemNo = searchItem.ItemNo,
                            ItemName = searchItem.ItemName,
                            Type = existedItem.Type,
                            Qty = existedItem.Qty + 1,
                            AmountPerQty = existedItem.AmountPerQty,
                            Discount = existedItem.Discount,
                            TotalAmount = existedItem.AmountPerQty * (existedItem.Qty + 1),
                            TranDate = existedItem.TranDate,
                            TranTime = existedItem.TranTime,
                            VendorCode = existedItem.VendorCode,
                            BranchCode = existedItem.BranchCode
                        });
                    }
                    else
                    {
                        await ItemSalesStore.AddItemAsync(new InventoryDetails
                        {
                            ItemNo = searchItem.ItemNo,
                            ItemName = searchItem.ItemName,
                            Type = "RSAL",
                            Qty = 1,
                            AmountPerQty = searchItem.FinalRetailPrice,
                            Discount = searchItem.RetailDiscount,
                            TotalAmount = searchItem.FinalRetailPrice,
                            TranDate = Variables.TranDate(),
                            TranTime = Variables.TranTime(),
                            VendorCode = searchItem.VendorCode,
                            BranchCode = BRANCH_CODE
                        });
                    }

                    await ExecuteLoadItemsCommand();
                }

                await TextSetFocus();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error Encountered", ex.Message, "Ok");
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
                IsCashPayment = true;
                IsCardPayment = false;
                Bank = string.Empty;
                BankRefNo = string.Empty;

                await ItemSalesStore.ClearItemAsync();

                await ExecuteLoadItemsCommand();

                await TextSetFocus();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error Encountered", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }

            return;
        }

        public async Task ExecuteItemsInitialization()
        {
            IsBusy = true;

            try
            {
                await ItemsStore.GetItemsAsync();
                await TextSetFocus();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error Encountered", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }

            return;
        }

        public Task TextSetFocus(string entryName = "eBarcode")
        {
            var entry = View.FindByName(entryName) as Entry;
            entry.Text = string.Empty;
            entry.Focus();

            return Task.CompletedTask;
        }
    }
}
