﻿using System.Security.Cryptography;
using System.Text;

namespace Divisima.UI.Tools
{
    public class GeneralTools
    {
        public static string GetMD5(string _text)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(_text));
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }

        public static string GetUrl(string _url)
        {
            return _url.ToLower().Replace(" ", "-").Replace("ı", "i").Replace("ğ", "g").Replace("ü", "u").Replace("ş", "s").Replace("ç", "c").Replace("ö", "o").Replace("/", "-");
        }
    }
}
