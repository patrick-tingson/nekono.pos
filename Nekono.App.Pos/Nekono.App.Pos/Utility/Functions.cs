using Nekono.App.Pos.Dependency;
using Nekono.App.Pos.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Nekono.App.Pos.Utility
{
    public class Functions
    {
        public static IPrinterSDK PrinterSDK { get => DependencyService.Get<IPrinterSDK>(); }

        public static string WriteSingleLn(string value1)
        {
            return string.Format("{0, -90}", string.Format("{0," + ((90 + value1.Length) / 2).ToString() + "}\n", value1));
        }

        public static string WriteDoubleLn(string value1, string value2)
        {
            return string.Format("{0}{1," + (90 - value1.Length) + "}\n", value1, value2);
        }

        public static string WriteLineCutLn()
        {
            return "--------------------------------------------------------------------------------------------------------------\n";
        }

        public static string WriteLineDoubleCutLn()
        {
            return "=====================================================\n";
        }

        public static void GoToLogin()
        {
            SecureStorage.RemoveAll();
            App.Current.MainPage = new LoginPage();
        }
    }
}
