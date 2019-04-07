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
        public IFormFile PrivateKey { get; set; }
        public IFormFile File { get; set; }
        public string ToUserUsername { get; set; }
        public string FolderToSaveFile1 { get; set; }
        public string FolderToSaveFile2 { get; set; }
        public string FolderToSaveFile3 { get; set; }
    }
}
