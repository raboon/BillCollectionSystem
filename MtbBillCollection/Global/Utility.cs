using System;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System.IO;



namespace MtbBillCollection.Global
{
    public sealed class Utility
    {

        static string _key = "ABCDEFFEDCBAABCDEFFEDCBAABCDEFFEDCBAABCDEFFEDCBA";
        static string _vector = "ABCDEFFEDCBABCDE";


        public static string EncryptString(string encryptValue)
        {
            TripleDESCryptoServiceProvider _cryptoProvider = new TripleDESCryptoServiceProvider();
            byte[] valBytes = Encoding.Unicode.GetBytes(encryptValue);
            ICryptoTransform transform = _cryptoProvider.CreateEncryptor();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
            cs.Write(valBytes, 0, valBytes.Length);
            cs.FlushFinalBlock();
            byte[] returnBytes = ms.ToArray();
            cs.Close();
            return Convert.ToBase64String(returnBytes);
        }

        public static string DecryptString(string encryptedValue)
        {
            TripleDESCryptoServiceProvider _cryptoProvider = new TripleDESCryptoServiceProvider();

            _cryptoProvider.Key = HexToByte(_key);
            _cryptoProvider.IV = HexToByte(_vector);

            byte[] valBytes = Convert.FromBase64String(encryptedValue);
            ICryptoTransform transform = _cryptoProvider.CreateDecryptor();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
            cs.Write(valBytes, 0, valBytes.Length);
            cs.FlushFinalBlock();
            byte[] returnBytes = ms.ToArray();
            cs.Close();
            return Encoding.Unicode.GetString(returnBytes);
        }


        /// <summary>
        /// Converts a hexadecimal string to a byte array
        /// </summary>
        /// <param name="hexString">hex value</param>
        /// <returns>byte array</returns>
        private static byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] =
                Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

    }


}