using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebISB.ExtensionMethods
{
    public static class Encryption
    {
        public static string PasswordEncryption(this string pass)
        {
            string newPass = "";
            byte[] arrByte1 = ASCIIEncoding.ASCII.GetBytes(pass);
            byte[] arrByte2 = new MD5CryptoServiceProvider().ComputeHash(arrByte1);
            arrByte2.ToList().ForEach(x => newPass += x);
            return newPass;
        }
    }
}