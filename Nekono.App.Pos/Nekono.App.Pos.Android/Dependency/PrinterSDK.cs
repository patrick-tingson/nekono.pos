//----------------------------------------------------------------------------------------------------------------
//Dependency Class to Native Android App to Xamarin.Forms
//Hashing, Encryption and Decryption of data
//----------------------------------------------------------------------------------------------------------------

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Content.PM;
using Android.Net;
using Android.Telephony;
using Com.Imagpay;
using Java.Lang;
using Nekono.App.Pos.Dependency;
using Nekono.App.Pos.Droid;
using Nekono.App.Pos.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(PrinterSDK))]
namespace Nekono.App.Pos.Droid
{
    public class PrinterSDK : IPrinterSDK
    {
        int[] status = new int[1];
        int[] battery = new int[1];

        BTPrinter printerBT = new BTPrinter();

        public bool Connect()
        {
            BluetoothDevice bluetoothDevice = BluetoothPrinterDevice();

            if (bluetoothDevice?.Name != "xp-58iih Printer")
                throw new InvalidOperationException("Error : Please connect your bluetooth printer.");

            try
            {
                if (!printerBT.IsConnected)
                {
                    printerBT.Connect(bluetoothDevice);
                }
            }
            catch(Java.Lang.Exception ex)
            {
                throw ex;
            }


            return printerBT.IsConnected;
        }

        public bool Close()
        {
            try
            {
                printerBT.Close();
            }
            catch (Java.Lang.Exception ex)
            {
                throw ex;
            }

            return printerBT.IsConnected;
        }

        public async Task<bool> PrintReceipt(CollectionReceiptDetails collectionReceiptDetails, bool storeCopy = false, bool rePrint = false)
        {
            bool printStatus = false;
            decimal totalAmount = 0;
            int totalQty = 0;

            try
            {
                if(!printerBT.IsConnected)
                {
                    Connect();
                }

                printerBT.Reset();

                printerBT.Write(ALIGN_CENTER);
                printerBT.Write(BOLD_LARGE_FONT);
                printerBT.Println("NEKONOMOTO");
                printerBT.Write(BOLD_NORMAL_FONT);
                printerBT.Println(WriteSingle("MOTORCYCLE PARTS AND ACCESSORIES"));
                printerBT.Reset();
                printerBT.Println(WriteSingle("78 Gen. T. de Leon, Valenzuela"));
                printerBT.Println(WriteSingle("1440, Metro Manila"));
                printerBT.Println("");

                if (rePrint)
                {
                    printerBT.Println(WriteSingle("***RE-PRINT***"));
                    printerBT.Println("");
                }
                else
                {
                    if (storeCopy)
                    {
                        printerBT.Write(KICK_DRAWER);
                    }
                }

                printerBT.Println(WriteDouble(collectionReceiptDetails.CollectionReceiptNo, DateTime.Now.ToString("MM/dd/yyyy HH:mm")));
                printerBT.Println(WriteDouble("POS Id", collectionReceiptDetails.CreatedBy));
                printerBT.Println(WriteDouble("Branch", collectionReceiptDetails.BranchCode));
                printerBT.Println(WriteLineCut());

                foreach (var itemDetails in collectionReceiptDetails.InventoryDetails)
                {
                    printerBT.Println(WriteDouble($"{itemDetails.ItemName}", ""));
                    printerBT.Println(WriteDouble($"{itemDetails.ItemNo} - {itemDetails.VendorCode}", ""));
                    printerBT.Println(WriteDouble($"{itemDetails.Qty} x {itemDetails.AmountPerQty:N}", $"{itemDetails.TotalAmount:N}"));
                    totalAmount += itemDetails.TotalAmount;
                    totalQty += itemDetails.Qty;
                }

                printerBT.Println(WriteLineCut());

                printerBT.Println(WriteDouble("Item Purchased", $"{totalQty}"));
                printerBT.Write(BOLD_NORMAL_FONT);
                printerBT.Println(WriteDouble("Total Amount", $"PHP {totalAmount:N}"));
                printerBT.Reset();
                printerBT.Println(WriteDouble("Payment Method", collectionReceiptDetails.Type));

                if (collectionReceiptDetails.Type == "CARD")
                {
                    printerBT.Println(WriteDouble("Bank", collectionReceiptDetails.Bank));
                    printerBT.Println(WriteDouble("Ref. No.", collectionReceiptDetails.RefNo));
                }

                printerBT.Println(WriteLineDoubleCut());

                if (storeCopy)
                {
                    printerBT.Println(WriteSingle($"***STORE COPY***"));
                }
                else
                {
                    printerBT.Println(WriteSingle($"***CUSTOMER COPY***"));
                }

                printerBT.Println(WriteLineDoubleCut());

                printerBT.Write(ALIGN_CENTER);
                printerBT.Write(BOLD_NORMAL_FONT);
                printerBT.Println("Customer Details");
                printerBT.Reset();
                printerBT.Println(WriteDouble("Name", ""));
                printerBT.Println(WriteUnderline());
                printerBT.Println(WriteDouble("Address", ""));
                printerBT.Println(WriteUnderline());
                printerBT.Println(WriteDouble("Signature", ""));
                printerBT.Println(WriteUnderline());
                printerBT.Println("");
                printerBT.Println("");

                //if (storeCopy)
                //{
                //    await PrintCustomerCopy(collectionReceiptDetails, printerBT, totalQty, totalAmount);
                //}

                printerBT.Finish();
                //printerBT.Close();

                printStatus = true;
            }
            catch (Java.Lang.Exception ex)
            {
                throw ex;
            }

            return printStatus;
        }

        public Task KickDrawer()
        {
            try
            {
                if (!printerBT.IsConnected)
                {
                    Connect();
                }

                printerBT.Write(KICK_DRAWER);
            }
            catch (Java.Lang.Exception ex)
            {
                throw ex;
            }

            return Task.CompletedTask;
        }

        private string KickDrawerEscapeCommand()
        {
            const string ESC = "\u001B";
            const string p = "\u0070";
            const string m = "\u0000";
            const string t1 = "\u0025";
            const string t2 = "\u0250";

            return ESC + p + m + t1 + t2;
        }

        BluetoothDevice BluetoothPrinterDevice()
        {
            BluetoothDevice bluetoothDevice = null;
            var adapter = BluetoothAdapter.DefaultAdapter;
            foreach (var bd in adapter.BondedDevices)
            {
                //if (bd.Name == "SZZCS")
                //{
                //    GetExtra = bd;
                //    break;
                //}
                if (bd.Name == "xp-58iih Printer")
                {
                    bluetoothDevice = bd;
                }
            }
            return bluetoothDevice;
        }

        public class BTRecv : BTReceiver
        {
            public BTRecv(Android.Content.Context p0)
                : base(p0)
            {

            }

            public BTRecv(Android.Content.Context p0, string p1)
                : base(p0, p1)
            {

            }

            public override bool IsPrinter(BluetoothDevice p0)
            {
                return (p0.Name == "xp-58iih Printer");
            }

            public override void FinishedDiscovery()
            {
                //throw new NotImplementedException();
            }

            public override void Print(BluetoothDevice p0)
            {
                var printerBT = new BTPrinter();
                printerBT.Connect(p0);
                printerBT.Reset();
                //Print title                
                printerBT.Write(ALIGN_CENTER);
                printerBT.Write(DEFAULT_BIG_FONT);
                printerBT.Println("MEGABANKER");
                printerBT.Write(DEFAULT_NORMAL_FONT);
                printerBT.Println("Banking Agent");
                printerBT.Println("");
                printerBT.Println(WriteDouble("Hello World", "Hello Din World!"));
                printerBT.Finish();
                printerBT.Close();
            }

            public override void StartedDiscovery()
            {
                //throw new NotImplementedException();
            }
        }


        public static byte[] ALIGN_LEFT = { 27, 97 };
        public static byte[] ALIGN_CENTER = { 27, 97, 1 };
        public static byte[] ALIGN_RIGHT = { 27, 97, 2 };
        public static byte[] DEFAULT_NORMAL_FONT = { 29, 33 };
        public static byte[] BOLD_NORMAL_FONT = new byte[] { 0x1B, 0x21, 0x08 };
        public static byte[] BOLD_LARGE_FONT = new byte[] { 0x1B, 0x21, 0x20 };
        public static byte[] DEFAULT_BIG_FONT = { 27, 33, 1 };
        public static byte[] KICK_DRAWER = { 27, 112, 0, 25, 250 };

        public static string WriteSingle(string value1)
        {
            return string.Format("{0, -32}", string.Format("{0," + ((32 + value1.Length) / 2).ToString() + "}", value1));
        }

        public static string WriteDouble(string value1, string value2)
        {
            return string.Format("{0}{1," + (32 - value1.Length) + "}", value1, value2);
        }

        public static string WriteLineCut()
        {
            return "--------------------------------";
        }

        public static string WriteUnderline()
        {
            return "________________________________";
        }

        public static string WriteLineDoubleCut()
        {
            return "================================";
        }

        public Task<bool> PrintAllReceipt(IEnumerable<CollectionReceiptDetails> collectionReceiptDetails, IEnumerable<InventoryDetails> inventoryDetails)
        {
            throw new NotImplementedException();
        }
    }
}