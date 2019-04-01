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
    /*
     * 
     * ZEER GROTE ERROR --> MEN KAN IN URL NOG INGEVEN OP ENCRYPTORDECRYPT SCHERM!!!!!!
     * 
     * 
     * 
     */

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
        public IActionResult Index(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                if(_accountRepository.GetAll().Any(x => x.Username == model.Username))
                {
                    var accountToLoginTo = _accountRepository.GetAll().First(x => x.Username == model.Username);
                    if (accountToLoginTo.Hash.Equals(_saltGenerator.getHashOfPasswordAndSalt(model.Password, accountToLoginTo.Salt)))
                    {
                        return RedirectToAction(nameof(EncryptOrDecryptChoice), new { id = accountToLoginTo.ID });
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Wrong password/account combination");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Wrong password/account combination");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Invalid modelstate");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(AccountViewModel model)
        {
            if(ModelState.IsValid)
            {
                if(!_accountRepository.GetAll().Any(x => x.Username == model.Username))
                {
                    if (model.Password.Equals(model.PasswordAgain))
                    {
                        var newAccount = new Account();
                        newAccount.Username = model.Username;
                        newAccount.Salt = _saltGenerator.getSalt();
                        newAccount.Hash = _saltGenerator.getHashOfPasswordAndSalt(model.Password, newAccount.Salt);
                        _accountRepository.CreateAccount(newAccount);
                        return RedirectToAction(nameof(EncryptOrDecryptChoice), new { id = newAccount.ID });//potentiele fout: hoe weet die het ID ?
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Passwords did not match, please try again");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Username already taken, please try again");
                    return View(model);
                }
                
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Invalid modelstate");
                return View(model);
            }
        }

        public IActionResult EncryptOrDecryptChoice(int id)
        {
            var model = _accountRepository.FindById(id);
            if(model == null)
            {
                ModelState.AddModelError(String.Empty, "returned model was null");
                return View(model);
            }
            else
            {
                return View(model);
            }
           
        }

        [HttpGet]
        public IActionResult Encryption()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Decryption()
        {
            return View();
        }

        /*
        [HttpPost]
        public IActionResult Encryption()
        {
            return View();
        }
        */
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