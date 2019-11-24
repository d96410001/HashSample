using System;
using System.Collections.Generic;
using System.Text;

namespace HashLib.Utility
{
    public class Cryptography
    {
        public static class EncryptUtil
        {
            public static string EncryptAES(string text, string key, string iv)
            {
                var sourceBytes = System.Text.Encoding.UTF8.GetBytes(text);
                var aes = System.Security.Cryptography.Aes.Create();
                aes.Mode = System.Security.Cryptography.CipherMode.CBC;
                aes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                aes.Key = System.Text.Encoding.UTF8.GetBytes(key);
                aes.IV = System.Text.Encoding.UTF8.GetBytes(iv);
                var transform = aes.CreateEncryptor();
                return Convert.ToBase64String(transform.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length));
            }

            public static string DecryptAES(string text, string key, string iv)
            {
                var encryptBytes = Convert.FromBase64String(text);
                var aes = System.Security.Cryptography.Aes.Create();
                aes.Mode = System.Security.Cryptography.CipherMode.CBC;
                aes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                aes.Key = System.Text.Encoding.UTF8.GetBytes(key);
                aes.IV = System.Text.Encoding.UTF8.GetBytes(iv);
                var transform = aes.CreateDecryptor();
                return System.Text.Encoding.UTF8.GetString(transform.TransformFinalBlock(encryptBytes, 0, encryptBytes.Length));
            }
        }
    }
}
