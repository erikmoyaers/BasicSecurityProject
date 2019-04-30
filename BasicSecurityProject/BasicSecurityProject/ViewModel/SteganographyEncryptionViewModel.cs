using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicSecurityProject.ViewModel
{
    public class SteganographyEncryptionViewModel
    {
        public IFormFile ImageToEncryptWith { get; set; }
        public String TextToEncrypt { get; set; }
        public String SaveFilePath { get; set; }
        public String SaveFileName { get; set; }
    }
}
