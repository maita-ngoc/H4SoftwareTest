using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography;

namespace H4SoftwareTest.Code
{
    public class EncryptionHandler
    {
        private readonly IDataProtector _dataProtector;
        private readonly HttpClient _httpClient;
        private string _publicKey;
        private string _privateKey;
        public EncryptionHandler(IDataProtectionProvider dataProtector, HttpClient httpClient)
        {
            _dataProtector = dataProtector.CreateProtector("ThisIsKey");
            // RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //// true = public and private, false = public
            //_privateKey = rsa.ToXmlString(true);
            //_publicKey = rsa.ToXmlString(false);

            if (!File.Exists("privateKey.txt"))
                using (System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider())
                {
                    _privateKey = rsa.ToXmlString(true);
                    _publicKey = rsa.ToXmlString(false);

                    File.WriteAllText("privateKey.txt", _privateKey);
                    File.WriteAllText("publicKey.txt", _publicKey);

                }
            else
            {
                _privateKey = File.ReadAllText("privateKey.txt");
                _publicKey = File.ReadAllText("publicKey.txt");
            }
            _httpClient = httpClient;


        }

        #region Symetric encryption
        public string EncryptSymetrisc(string txtToEncrypt)
        {

            return _dataProtector.Protect(txtToEncrypt);
        }
        public string DecryptSymetrisc(string txtToDecrypt) => _dataProtector.Unprotect(txtToDecrypt);
        #endregion Symetric encryption

        #region Asymetric encryption 

        public async Task<string> EncryptAsymetriscParent(string txtToEncrypt)
        {
            string[] data = new string[2] { txtToEncrypt, _publicKey };
            //string serializedValue = JsonConvert.SerializeObject(data);
            //StringContent content = new StringContent(serializedValue,System.Text.Encoding.UTF8,"application/json");
            ////var response = await _httpClient.PostAsync("https://localhost:7035/api/Encrypt", content);
            //string encrypted= await response.Content.ReadAsStringAsync();
            //return encrypted;

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(_publicKey);
            byte[] txtToEncryptAsByteArray = System.Text.Encoding.UTF8.GetBytes(txtToEncrypt);
            byte[] encryptedValue = rsa.Encrypt(txtToEncryptAsByteArray, true);
            string encryptedValueAsString = Convert.ToBase64String(encryptedValue);
            return encryptedValueAsString;
        }

        public string DecryptAsymetrisc(string txtToDecrypt)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(_privateKey);
            byte[] txtToDecryptAsByteArray = Convert.FromBase64String(txtToDecrypt);
            byte[] encryptedValue = rsa.Decrypt(txtToDecryptAsByteArray, true);
            string encryptedValueAsString = System.Text.Encoding.UTF8.GetString(encryptedValue);
            return encryptedValueAsString;
        }
        //public string EncryptAsymetrisc(string[] value)
        //{
        //    string txtToEncrypt = value[0];
        //    string publicKey = value[1];
        //    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        //    rsa.FromXmlString(publicKey);
        //    byte[] txtToEncryptAsByteArray = System.Text.Encoding.UTF8.GetBytes(txtToEncrypt);
        //    byte[] encryptedValue = rsa.Encrypt(txtToEncryptAsByteArray, true);
        //    string encryptedValueAsString = Convert.ToBase64String(encryptedValue);
        //    return encryptedValueAsString;
        //}
        #endregion Asymetric encryption
    }
}
