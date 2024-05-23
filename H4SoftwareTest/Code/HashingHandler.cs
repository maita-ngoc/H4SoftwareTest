using BCrypt.Net;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography;
using System.Text;

namespace H4SoftwareTest.Code
{
    public class HashingHandler
    {
        public dynamic MD5Hashing(string txtToHash, string returnType)
        {
            MD5 md5 = MD5.Create();
            byte[] txtToHashAsByteArrayy = Encoding.ASCII.GetBytes(txtToHash);
            byte[] hashedValue = md5.ComputeHash(txtToHashAsByteArrayy);
            string hashedValueAsString = Convert.ToBase64String(hashedValue);
            if (returnType == "string")
            {
                return hashedValueAsString;
            }
            else if (returnType == "byteArray")
            {
                return hashedValue;
            }
            else return null;

        }

        public string SHA2Hashing(string txtToHash)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] txtToHashAsByteArrayy = Encoding.ASCII.GetBytes(txtToHash);
            byte[] hashedValue = sha256.ComputeHash(txtToHashAsByteArrayy);
            string hashedValueAsString = Convert.ToBase64String(hashedValue);
            return hashedValueAsString;
        }

        public string HMACHashing(string txtToHash)
        {
            var hmac = new HMACSHA256();
            byte[] mykey = Encoding.ASCII.GetBytes("This is my key");
            byte[] txtToHashAsByteArrayy = Encoding.ASCII.GetBytes(txtToHash);

            //Make self key
            hmac.Key = mykey;

            byte[] hashedValue = hmac.ComputeHash(txtToHashAsByteArrayy);
            string hashedValueAsString = Convert.ToBase64String(hashedValue);
            return hashedValueAsString;

        }

        public string PBKDF2Hashing(string txtToHash)
        {
            byte[] salt = Encoding.ASCII.GetBytes("This is my key");
            byte[] txtToHashAsByteArrayy = Encoding.ASCII.GetBytes(txtToHash);

            var hashAlgo = new System.Security.Cryptography.HashAlgorithmName("SHA256");
            int itirationer = 10;
            int outputLenght = 32;
            byte[] hashedValue = System.Security.Cryptography.Rfc2898DeriveBytes.Pbkdf2(txtToHashAsByteArrayy, salt, itirationer, hashAlgo, outputLenght);
            string hashedValueAsString = Convert.ToBase64String(hashedValue);
            return hashedValueAsString;
        }

        public string BCryptHashing(string txtToHash)
        {
            //return BCrypt.Net.BCrypt.HashPassword(txtToHash);

            //int workFactor = 10;
            //bool enhancedEntropi = true;
            //return BCrypt.Net.BCrypt.HashPassword(txtToHash,workFactor,enhancedEntropi);

            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            bool enhancedEntropi = true;
            HashType hashType = HashType.SHA256;
            return BCrypt.Net.BCrypt.HashPassword(txtToHash, salt, enhancedEntropi, hashType);
        }

        public bool BCryptHashingVerify(string txtToHash, string hashedValueAsString)
        {
            // return BCrypt.Net.BCrypt.Verify(txtToHash,hashedValueAsString);

            //return BCrypt.Net.BCrypt.Verify(txtToHash, hashedValueAsString,true);
            return BCrypt.Net.BCrypt.Verify(txtToHash, hashedValueAsString, true, HashType.SHA256);
        }
    }
}
