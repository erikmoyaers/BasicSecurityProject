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
        public String FromAccount { get; set; }
        public String ToAccount { get; set; }
        public byte[] FileByteArray { get; set; }
        

    }
}
