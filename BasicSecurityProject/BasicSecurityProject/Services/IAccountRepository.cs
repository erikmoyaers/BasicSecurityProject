﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicSecurityProject.Models;

namespace BasicSecurityProject.Services
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAll();
    }
}