using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nekono.App.Pos.Utility
{
    public class Signature
    {
        public static string Create(string userName, string password)
        {
            return SHA1($"{userName}{DateTimeOffset.UtcNow:yyyyMMdd}{password}");
        }

        public static string SHA1(string stringToHash)
        {
            System.Security.Cryptography.SHA1Managed sha1Obj = new System.Security.Cryptography.SHA1Managed();
            byte[] bytesToHash = System.Text.Encoding.UTF8.GetBytes(stringToHash);
            bytesToHash = sha1Obj.ComputeHash(bytesToHash);
            return BitConverter.ToString(bytesToHash).Replace("-", "").ToUpper();
        }
    }
}
