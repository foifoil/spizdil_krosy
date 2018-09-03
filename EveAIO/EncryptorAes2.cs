namespace EveAIO
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    internal static class EncryptorAes2
    {
        public static string Key;

        static EncryptorAes2()
        {
            Class7.RIuqtBYzWxthF();
            Key = "FGRSDFGBVDCFGRET";
        }

        public static string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
            {
                throw new ArgumentNullException("encryptedText");
            }
            string str = null;
            byte[] bytes = Convert.FromBase64String(encryptedText);
            encryptedText = Encoding.GetEncoding("windows-1250").GetString(bytes);
            using (AesCryptoServiceProvider provider = new AesCryptoServiceProvider())
            {
                provider.Key = Encoding.GetEncoding("windows-1250").GetBytes(Key);
                provider.IV = Encoding.GetEncoding("windows-1250").GetBytes("CGgwK9WpEtVQxm9Y");
                ICryptoTransform transform = provider.CreateDecryptor(provider.Key, provider.IV);
                using (MemoryStream stream = new MemoryStream(Encoding.GetEncoding("windows-1250").GetBytes(encryptedText)))
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(stream2))
                        {
                            str = reader.ReadToEnd();
                        }
                    }
                }
            }
            return str;
        }
    }
}

