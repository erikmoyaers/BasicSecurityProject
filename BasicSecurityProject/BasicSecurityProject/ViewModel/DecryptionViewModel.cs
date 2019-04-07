using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace BasicSecurityProject.ViewModel
{
    public class DecryptionViewModel
    {
        public IFormFile File_1 { get; set; }
        public IFormFile File_2 { get; set; }
        public IFormFile File_2_IV { get; set; }
        public IFormFile File_3 { get; set; }
        public IFormFile PrivateKey { get; set; }
        public string FromUserUsername { get; set; }
    }
}