using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicSecurityProject.Models
{
    public class Account
    {
        public int ID { get; set; }
        public String Username { get; set; }
        public String Hash { get; set; }
        public String Salt { get; set; }
    }
}
