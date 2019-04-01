using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BasicSecurityProject.ViewModel
{
    public class EncryptionViewModel
    {
        public IFormFile AesKey { get; set; }
        public IFormFile Iv { get; set; }
        public IFormFile File { get; set; }
        public string ToUserUsername { get; set; }
    }
}
