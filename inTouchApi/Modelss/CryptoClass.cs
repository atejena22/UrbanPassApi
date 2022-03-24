using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace inTouchApi.Models
{
    public sealed class CryptoClass
    {

        String prm_key = ConfigurationManager.AppSettings("appSettings:Encriptkey"); 
        String prm_iv = ConfigurationManager.AppSettings("appSettings:Encriptiv"); 

        // Singleton pattern used here with ensured thread safety
        protected static readonly CryptoClass _instance = new CryptoClass();
        public static CryptoClass Instance
        {
            get { return _instance; }
        }

        public CryptoClass()
        {

        }

        public string decryptAES(string prm_text_to_decrypt)
        {

            var sEncryptedString = prm_text_to_decrypt;

            var myRijndael = new RijndaelManaged()
            {
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.CBC,
                KeySize = 256,
                BlockSize = 256
            };

            var key = Encoding.ASCII.GetBytes(prm_key);
            var IV = Encoding.ASCII.GetBytes(prm_iv);

            var decryptor = myRijndael.CreateDecryptor(key, IV);

            var sEncrypted = Convert.FromBase64String(sEncryptedString);

            var fromEncrypt = new byte[sEncrypted.Length];

            var msDecrypt = new MemoryStream(sEncrypted);
            var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

            csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

            return (Encoding.UTF8.GetString(fromEncrypt)); // Encoding.ASCII.GetString( fromEncrypt )
        }

        public string encryptAES(string prm_text_to_encrypt)
        {

            var sToEncrypt = prm_text_to_encrypt;

            var myRijndael = new RijndaelManaged()
            {
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                KeySize = 128, 
                BlockSize = 128
            };

            var key = Encoding.ASCII.GetBytes(prm_key);
            var IV = Encoding.ASCII.GetBytes(prm_iv);

            var encryptor = myRijndael.CreateEncryptor(key, IV);

            var msEncrypt = new MemoryStream();
            var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

            var toEncrypt = Encoding.ASCII.GetBytes(sToEncrypt);

            csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
            csEncrypt.FlushFinalBlock();

            var encrypted = msEncrypt.ToArray();

            return (Convert.ToBase64String(encrypted));
        }


        public byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }

        public string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

        public static void GenerateKeyAndIV()
        {
            // This code is only here for an example
            RijndaelManaged myRijndaelManaged = new RijndaelManaged();
            myRijndaelManaged.Mode = CipherMode.CBC;
            myRijndaelManaged.Padding = PaddingMode.PKCS7;

            myRijndaelManaged.GenerateIV();
            myRijndaelManaged.GenerateKey();
            string newKey = ByteArrayToHexString(myRijndaelManaged.Key);
            string newinitVector = ByteArrayToHexString(myRijndaelManaged.IV);


        }

        public static byte[] HexStringToByte(string hexString)
        {
            try
            {
                int bytesCount = (hexString.Length) / 2;
                byte[] bytes = new byte[bytesCount];
                for (int x = 0; x < bytesCount; ++x)
                {
                    bytes[x] = Convert.ToByte(hexString.Substring(x * 2, 2), 16);
                }
                return bytes;
            }
            catch
            {
                throw;
            }
        }

        public static string stringToHex(string values)
        {
            string hexOutput = "";

            foreach (char letter in values)
            {
                // Get the integral value of the character. 
                int value = Convert.ToInt32(letter);
                // Convert the decimal value to a hexadecimal value in string form. 
                hexOutput += String.Format("{0:X}", value);

            }
            return hexOutput;
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
