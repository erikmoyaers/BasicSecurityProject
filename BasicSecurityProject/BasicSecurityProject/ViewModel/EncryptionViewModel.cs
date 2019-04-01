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
        /*
        public FormFile AesKey { get; set; }
        public FormFile Iv { get; set; }
        public FormFile PrivateKey { get; set; }
        public FormFile PublicKey { get; set; }
        */
        public IFormFile File { get; set; }
        public string ToUserUsername { get; set; }
    }
}
