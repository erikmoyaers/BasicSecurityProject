using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BasicSecurityProject.Services
{
    public class SaltGenerator : ISaltGenerator
    {
        private readonly SHA256 _sha256 = SHA256.Create();
        public string getHashOfPasswordAndSalt(string password, string salt)
        {
            //wordt naar hex vertaald omdat UTF8 rommel als output geeft
            return BitConverter.ToString(_sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt))).Replace("-", String.Empty);
        }


        //https://codereview.stackexchange.com/questions/93614/salt-generation-in-c
        public string getSalt()
        {
            var random = new RNGCryptoServiceProvider();

            // Maximum length of salt
            int max_length = 32;

            // Empty salt array
            byte[] salt = new byte[max_length];

            // Build the random bytes
            random.GetBytes(salt);

            // Return the string encoded salt
            return Convert.ToBase64String(salt);
        }
    }
}
