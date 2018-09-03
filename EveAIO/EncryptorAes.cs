namespace EveAIO
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;

    internal static class EncryptorAes
    {
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
                provider.Key = Encoding.GetEncoding("windows-1250").GetBytes(<Key>k__BackingField);
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

        public static string Encrypt(string plainText)
        {
            byte[] buffer;
            if (string.IsNullOrEmpty(plainText))
            {
                throw new ArgumentNullException("plainText");
            }
            using (AesCryptoServiceProvider provider = new AesCryptoServiceProvider())
            {
                provider.Key = Encoding.GetEncoding("windows-1250").GetBytes(<Key>k__BackingField);
                provider.IV = Encoding.GetEncoding("windows-1250").GetBytes("CGgwK9WpEtVQxm9Y");
                ICryptoTransform transform = provider.CreateEncryptor(provider.Key, provider.IV);
                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(stream2))
                        {
                            writer.Write(plainText);
                        }
                        buffer = stream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(Encoding.GetEncoding("windows-1250").GetBytes(Encoding.GetEncoding("windows-1250").GetString(buffer)));
        }

        public static string Key
        {
            [CompilerGenerated]
            get => 
                <Key>k__BackingField;
            [CompilerGenerated]
            set => 
                (<Key>k__BackingField = value);
        }
    }
}

