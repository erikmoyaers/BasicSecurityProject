using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BasicSecurityProject.Models;
using BasicSecurityProject.Services;
using BasicSecurityProject.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BasicSecurityProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISaltGenerator _saltGenerator = new SaltGenerator();//TO DO : in constructor zetten

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel loginCredentials)
        {
            if(ModelState.IsValid)
            {
                if(_accountRepository.GetAll().Any(x => x.Username == loginCredentials.Username))
                {
                    var accountToLoginTo = _accountRepository.GetAll().First(x => x.Username == loginCredentials.Username);
                    if (accountToLoginTo.Hash.Equals(_saltGenerator.getHashOfPasswordAndSalt(loginCredentials.Password, accountToLoginTo.Salt)))
                    {
                        return RedirectToAction(nameof(EncryptOrDecryptChoice), accountToLoginTo.ID);
                    }
                    else
                    {
                        return View("Wrong password");
                    }
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(AccountViewModel account)
        {
            if(ModelState.IsValid)
            {
                if(_accountRepository.GetAll().Any(x => x.Username == account.Username))
                {
                    if (!account.Password.Equals(account.PasswordAgain))
                    {
                        var newAccount = new Account();
                        newAccount.Username = account.Username;
                        newAccount.Salt = _saltGenerator.getSalt();
                        newAccount.Hash = _saltGenerator.getHashOfPasswordAndSalt(account.Password, newAccount.Salt);
                        _accountRepository.CreateAccount(newAccount);
                        return RedirectToAction(nameof(EncryptOrDecryptChoice), new { id = newAccount.ID });//potentiele fout: hoe weet die het ID ?
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }
                
            }
            else
            {
                return View();
            }
        }

        public IActionResult EncryptOrDecryptChoice(int id)
        {
            var model = _accountRepository.FindById(id);
            if(model == null)
            {
                return View("Not found"); //mss beter een of andere fancy niet gevonden pagina
            }
            return View(model);
        }

        

        /*
        public IActionResult Index()
        {
            var model = _accountRepository.GetAll();
            return View(model);
        }

        [HttpPost]
        public IActionResult View()
        {
            
            return View();
        }
        */

    }
}