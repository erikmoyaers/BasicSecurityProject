using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicSecurityProject.Services
{
    public interface ISaltGenerator
    {
        String getSalt();
        String getHashOfPasswordAndSalt(String password, String salt);
    }
}
