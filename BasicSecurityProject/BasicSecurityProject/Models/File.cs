using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace BasicSecurityProject.Models
{
    public class File
    {
        public int ID { get; set; }
        public Account FromAccount { get; set; }
        public Account ToAccount { get; set; }
        public byte[] FileByteArray { get; set; }
        

    }
}
