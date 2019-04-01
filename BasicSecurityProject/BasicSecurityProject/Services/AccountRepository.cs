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

        public Account FindById(int id)
        {
            return _context.Accounts.FirstOrDefault(a => a.ID == id);
        }

        public void CreateAccount(Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();
        }

        /*
        public void SetPrivateKey(int userId, byte[] privateKey)
        {
            _context.Accounts.First(a => a.ID == userId).PrivateKey = privateKey;
        }

        public void SetPublicKey(int userId, byte[] publicKey)
        {
            _context.Accounts.First(a => a.ID == userId).PrivateKey = publicKey;
        }
        */

    }
}
