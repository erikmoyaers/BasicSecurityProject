using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BasicSecurityProject.ViewModel
{
    public class AccountViewModel
    {
        //zet hier logica omtrent lengte bij. in databank staat er VARCHAR(MAX) maar test dit eens de applicatie bolt
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PasswordAgain { get; set; }
    }
}
