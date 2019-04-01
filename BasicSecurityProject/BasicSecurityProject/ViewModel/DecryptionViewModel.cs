using System.Security.Cryptography;

namespace BasicSecurityProject.ViewModel
{
    public class DecryptionViewModel
    {
        public byte[] AesKey { get; set; }
        public RSAParameters PrivateKey { get; set; }
        public RSAParameters PublicKey { get; set; }
        public byte[] File { get; set; }
        public string FromUserUsername { get; set; }
    }
}