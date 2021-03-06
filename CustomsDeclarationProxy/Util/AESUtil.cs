﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace CustomsDeclarationProxy.Util
{
    public class AESUtil
    {

        /// AES encryption
        public static string AesEncoding(string data, string key, Encoding encoding)
        {
            var hashMd5 = new MD5CryptoServiceProvider();
            byte[] keyArray = hashMd5.ComputeHash(encoding.GetBytes(key));
            byte[] toEncryptArray = encoding.GetBytes(data);
            RijndaelManaged rijndaelManaged = new System.Security.Cryptography.RijndaelManaged();
            rijndaelManaged.Key = keyArray;
            rijndaelManaged.Mode = System.Security.Cryptography.CipherMode.ECB;
            rijndaelManaged.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ICryptoTransform cTransform = rijndaelManaged.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }


        /// AES decode

        public static string AesDecoding(string data, string key, Encoding encoding)
        {
            var hashMd5 = new MD5CryptoServiceProvider();
            byte[] keyArray = hashMd5.ComputeHash(encoding.GetBytes(key));
            byte[] toEncryptArray = Convert.FromBase64String(data);
            RijndaelManaged rijndaelManaged = new System.Security.Cryptography.RijndaelManaged();
            rijndaelManaged.Key = keyArray;
            rijndaelManaged.Mode = CipherMode.ECB;
            rijndaelManaged.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rijndaelManaged.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return encoding.GetString(resultArray, 0, resultArray.Length);
        }
    }

}
