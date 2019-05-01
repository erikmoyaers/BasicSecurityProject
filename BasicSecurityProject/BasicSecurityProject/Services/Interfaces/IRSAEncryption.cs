using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BasicSecurityProject.Services.Interfaces
{
    public interface IRSAEncryption
    {
        void GenerateKeysInFile(string folder, string publicKeyName, string privateKeyName);
        byte[] EncryptData(byte[] dataToEncrypt, RSAParameters publicKey);
        byte[] DecryptData(byte[] dataToDecrypt, RSAParameters privateKey);
        byte[] SignData(byte[] hashOfDataToSign, RSAParameters privateKey);
        bool VerifySignature(byte[] hashOfSignedData, byte[] signature, RSAParameters publicKey);
    }
}
