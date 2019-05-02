using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicSecurityProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicSecurityProject.Context
{
    public class AccountContext : DbContext
    {
        public AccountContext()
        {
        }

        public AccountContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

        }

    }
}
