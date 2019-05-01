using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BasicSecurityProject.Services.Interfaces
{
    public interface ISteganography
    {
        void Encryption(Bitmap imageToEncryptWith, String messageToEncrypt, String SaveFilePath, String SaveFileName);
        void Decryption(Bitmap imageToDecrypt, String SaveFilePath, String SaveFileName);

    }
}
