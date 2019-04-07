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

        //TODO: klopt first here ?
        public Account FindById(int id)
        {
            return _context.Accounts.First(a => a.ID == id);
        }

        public Account FindByName(string name)
        {
            return _context.Accounts.First(a => a.Username.Equals(name));
        }

        public void CreateAccount(Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();
        }
    }
}
