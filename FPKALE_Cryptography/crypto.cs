using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FPKALE_Cryptography
{
    class crypto
    {
        #region encryption
        public string Encrypt(string toEncrypt, string getKey, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);


            

            string key = getKey;
            
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
               

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            
            tdes.Key = keyArray;
            
            
            tdes.Mode = CipherMode.ECB;
            

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            
            tdes.Clear();
            
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        #endregion

        #region decryption
        public string Decrypt(string cipherString, string getKey, bool useHashing)
        {
            byte[] keyArray;
            

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);


            
            string key = getKey;

            if (useHashing)
            {
                
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                

                hashmd5.Clear();
            }
            else
            {
                
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            
            tdes.Key = keyArray;
            
            

            tdes.Mode = CipherMode.ECB;
            
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            
            tdes.Clear();
            
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        #endregion
        #region rendom#
        private Random random = new Random();
        public string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion
    }
}
