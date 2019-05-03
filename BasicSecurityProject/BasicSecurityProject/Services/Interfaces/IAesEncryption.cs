using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicSecurityProject.Services
{
    public interface IAesEncryption
    {
        //byte[] GenerateRandomNumber(int length); --> DEPRECATED: keys worden niet automatisch gegenereerd, maar ingegeven met IFormFile
        byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv);
        byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv);
    }
}
