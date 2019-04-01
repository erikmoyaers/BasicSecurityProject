using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicSecurityProject.Services
{
    public interface IDecryption
    {
        byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv);
    }
}
