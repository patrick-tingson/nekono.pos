using Nekono.App.Pos.Models;
using Nekono.App.Pos.Utility;
using Nekono.App.Pos.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nekono.App.Pos.ViewModels
{
    public class ReceiptsViewModel : BaseViewModel
    {
        public ObservableCollection<CollectionReceiptDetails> CollectionReceipts { get; set; }
        public ObservableCollection<InventoryDetails> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command ReprintCommand { get; set; }
        public Command VoidCommand { get; set; }

        string transactionDate = "-";
        public string TransactionDate
        {
            get { return transactionDate; }
            set { SetProperty(ref transactionDate, value); }
        }

        string paymentMethod = "-";
        public string PaymentMethod
        {
            get { return paymentMethod; }
            set { SetProperty(ref paymentMethod, value); }
        }

        string receiptStatus = "-";
        public string ReceiptStatus
        {
            get { return receiptStatus; }
            set { SetProperty(ref receiptStatus, value); }
        }

        string receiptNo = "-";
        public string ReceiptNo
        {
            get { return receiptNo; }
            set { SetProperty(ref receiptNo, value); }
        }

        string receiptTotalAmount = "-";
        public string ReceiptTotalAmount
        {
            get { return receiptTotalAmount; }
            set { SetProperty(ref receiptTotalAmount, value); }
        }

        string remarks = "-";
        public string Remarks
        {
            get { return remarks; }
            set { SetProperty(ref remarks, value); }
        }

        string textStatusColor = "-";
        public string TextStatusColor
        {
            get { return textStatusColor; }
            set { SetProperty(ref textStatusColor, value); }
        }

        IEnumerable<InventoryDetails> itemsIncluded = null;
        public IEnumerable<InventoryDetails> ItemsIncluded
        {
            get { return itemsIncluded; }
            set { SetProperty(ref itemsIncluded, value); }
        }

        CollectionReceiptDetails receiptDetails = new CollectionReceiptDetails();
        public CollectionReceiptDetails ReceiptDetails
        {
            get { return receiptDetails; }
            set { SetProperty(ref receiptDetails, value); }
        }

        public ReceiptsViewModel()
        {
            Title = "Receipts";
            CollectionReceipts = new ObservableCollection<CollectionReceiptDetails>();
            Items = new ObservableCollection<InventoryDetails>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ReprintCommand = new Command(async () => await ExecuteReprintReceiptCommand());
            VoidCommand = new Command(async () => await ExecuteVoidReceiptCommand());
            //MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            //{
            //    var newItem = item as Item;
            //    Items.Add(newItem);
            //    await DataStore.AddItemAsync(newItem);
            //});
        }

        public async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                CollectionReceipts.Clear();
                var receipts = await CollectionReceiptsStore.GetItemsAsync(true);
                foreach (var receipt in receipts)
                {
                    CollectionReceipts.Add(receipt);
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

        public async Task ExecuteLoadMoreReceiptDetailsCommand(CollectionReceiptDetails collectionReceiptDetails)
        {

            IsBusy = true;

            try
            {
                TransactionDate = collectionReceiptDetails.TransactionDate;
                ReceiptNo = collectionReceiptDetails.CollectionReceiptNo;
                ReceiptStatus = collectionReceiptDetails.ReceiptStatus;
                Remarks = collectionReceiptDetails.Remarks;
                ReceiptTotalAmount = $"PHP {collectionReceiptDetails.TotalAmount:N}";
                TextStatusColor = collectionReceiptDetails.StatusColor;
                var refNo = string.Empty;

                if(collectionReceiptDetails.RefNo != null && collectionReceiptDetails.RefNo?.Length > 0)
                {
                    refNo = $"Ref# {collectionReceiptDetails.RefNo}";
                }

                PaymentMethod = $"{collectionReceiptDetails.Type} {collectionReceiptDetails.Bank} {refNo}";

                collectionReceiptDetails.InventoryDetails = await ItemsServices.GetItemsInReceipt(ReceiptNo);

                receiptDetails = collectionReceiptDetails;

                Items.Clear();
                foreach (var item in collectionReceiptDetails.InventoryDetails)
                {
                    Items.Add(item);
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

        public async Task ExecuteReprintReceiptCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if(ReceiptDetails.CollectionReceiptNo != null)
                {
                    if(await App.Current.MainPage.DisplayAlert("NEKONOMOTO", "Please confirm your re-print receipt request.", "Confirm", "Cancel"))
                    {
                        try
                        {
                            await Functions.PrinterSDK.PrintReceipt(ReceiptDetails, true, true);
                            await App.Current.MainPage.DisplayAlert("NEKONOMOTO", "Re-print receipt successful!", "Ok");
                        }
                        catch (Exception ex)
                        {
                            await App.Current.MainPage.DisplayAlert("NEKONOMOTO", $"Unable to print receipt. Please turn on or check your printer.", "Ok");
                        }
                    }
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

        public async Task ExecuteVoidReceiptCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (ReceiptDetails.CollectionReceiptNo != null && ReceiptDetails.Active == 0)
                {

                    if (await App.Current.MainPage.DisplayAlert("NEKONOMOTO", $"Are you sure do you want to void this receipt {ReceiptDetails.CollectionReceiptNo} with Total Amount: PHP {ReceiptDetails.TotalAmount:N}?"
                        , "Confirm", "Cancel"))
                    {
                        var reasonToVoid = "";

                        var prePopulateReason = await App.Current.MainPage.DisplayActionSheet($"Please select your reason to void this receipt {ReceiptDetails.CollectionReceiptNo} with Total Amount: PHP {ReceiptDetails.TotalAmount:N}",
                            "Cancel", null, "Duplicate Receipt", "Wrong Item(s)", "Customer Issue", "Others");

                        if (prePopulateReason != "Cancel")
                        {
                            reasonToVoid = prePopulateReason;

                            if (prePopulateReason == "Others")
                            {
                                var remarks = await App.Current.MainPage.DisplayPromptAsync("NEKONOMOTO", $"Please enter your reason to void this receipt {ReceiptDetails.CollectionReceiptNo} with Total Amount: PHP {ReceiptDetails.TotalAmount:N}",
                                                    "Confirm", "Cancel", "Reason...", 50);

                                if (remarks == null || remarks?.Length == 0)
                                {
                                    return;
                                }

                                reasonToVoid = remarks;
                            }

                            var voidRequest = new DeleteRequest
                            {
                                Id = ReceiptDetails.CollectionReceiptNo,
                                Remarks = reasonToVoid
                            };

                            var voidResponse = await CollectionReceiptServices.Void(voidRequest);

                            if (voidResponse)
                            {
                                await App.Current.MainPage.DisplayAlert("NEKONOMOTO", $"Successful Receipt Void! \nReceipt # {ReceiptDetails.CollectionReceiptNo}\nTotal Amount of PHP {ReceiptDetails.TotalAmount:N}", "Ok");
                                ReceiptStatus = "VOID";
                                await ExecuteLoadItemsCommand();
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Error Encountered", "Void receipt failed! Please contact POS admin.", "Ok");
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
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
    }
}
