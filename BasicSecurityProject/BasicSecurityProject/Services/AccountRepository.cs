using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicSecurityProject.Context;
using BasicSecurityProject.Models;

namespace BasicSecurityProject.Services
{

    public class AccountRepository : IAccountRepository
    {
        private readonly AccountContext _context;

        public AccountRepository(AccountContext context)
        {
            _context = context;
        }

        public IEnumerable<Account> GetAll()
        {
            
            return _context.Accounts.ToList(); ;
        }
    }
}
