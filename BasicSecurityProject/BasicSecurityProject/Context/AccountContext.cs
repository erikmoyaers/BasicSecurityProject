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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasData(
                    new Account() { ID = 1, Username = "John", Hash = "A8CFCD74832004951B4408CDB0A5DBCD8C7E52D43F7FE244BF720582E05241DA", Salt = "ef6e5f49d8ssd8v"},
                    new Account() { ID = 2, Username = "Chris", Hash = "9AE4BC0E32DB0E3484CD398459D20F9B4F79CCE36667428181BF037131A3C987", Salt = "ef6e5f49d8ssd8v" },
                    new Account() { ID = 3, Username = "Mukesh", Hash = "E2674AA2162A3225B5B51DD0A796C32F17679642593347BD6A24EB90EEBF912B", Salt = "ef6e5f49d8ssd8v" });
                
        }
        
    }
}
