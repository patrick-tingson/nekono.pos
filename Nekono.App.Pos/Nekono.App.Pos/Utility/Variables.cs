using System;
using System.Collections.Generic;
using System.Text;

namespace Nekono.App.Pos.Utility
{
    public class Variables
    {
        public static string TranDate () => DateTime.Now.ToString("yyyy/MM/dd");
        public static string TranTime () => DateTime.Now.ToString("HH:mm:ss");

        public static string APIEndpoint() => "http://localhost:2021/api/";
    }
}
