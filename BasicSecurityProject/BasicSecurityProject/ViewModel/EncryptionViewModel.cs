using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BasicSecurityProject.ViewModel
{
    public class EncryptionViewModel
    {
        public byte[] AesKey { get; set; }
        public RSAParameters PrivateKey { get; set; }
        public RSAParameters PublicKey { get; set; }
        public byte[] File { get; set; }
        public string ToUserUsername { get; set; }
    }
}
