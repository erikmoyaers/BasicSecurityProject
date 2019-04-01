using BasicSecurityProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid
{
    class HybridEncryption
    {
        /*
        private readonly AesEncryption _aes = new AesEncryption();
        private readonly RSAEncryption _rsa = new RSAEncryption();
        private readonly SHA256 _sha256 = SHA256.Create();

        private Account _fromAccount;
        private Account _toAccount;

        public HybridEncryption(Account fromUser, Account toUser)
        {
            _fromAccount = fromUser;
            _toAccount = toUser;
        }

        public EncryptedPacket EncryptData(byte[] original)
        {

            
            //het packet waarin de geencrypteerde file, geencrypteerde key en geencrypteerde hash verstuurt zullen worden
            EncryptedPacket encryptedPacket = new EncryptedPacket();
            encryptedPacket.IvForAes = _aes.GenerateRandomNumber(16);

            //1: de data encrypteren met aes
            var sessionKey = _aes.GenerateRandomNumber(32); //256 bits?
            encryptedPacket.AesEncryptedData = _aes.Encrypt(original, sessionKey, encryptedPacket.IvForAes);

            //2: de aes key encrypteren met RSA (public key van b)
            encryptedPacket.RsaEncryptedSessionKey = _rsa.EncryptData(sessionKey, _toAccount.getPublicKey());

            //3: een hash pakken van het bestand en met RSA signeren (private key van a)
            byte[] dataHash = _sha256.ComputeHash(original);
            encryptedPacket.RsaSignedHash = _rsa.SignData(dataHash, _fromAccount.getPrivateKey());

            return encryptedPacket;
            
        }
        */
    }
}
