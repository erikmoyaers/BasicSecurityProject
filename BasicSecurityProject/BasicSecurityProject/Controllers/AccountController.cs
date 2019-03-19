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
        private readonly SHA256 _sha256 = SHA256.Create();

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //PROBLEEM: user komt niet in de databank 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(AccountViewModel account)
        {
            if(ModelState.IsValid)
            {
                if(account.Password.Equals(account.PasswordAgain))
                {
                    var newAccount = new Account();
                    newAccount.Username = account.Username;
                    newAccount.Salt = getSalt();
                    //van wat ik kan verstaan: wordt gewoon achter elkaar bijgevoegd. klopt dit?
                    newAccount.Hash = Encoding.UTF8.GetString(_sha256.ComputeHash(Encoding.ASCII.GetBytes(account.Password + newAccount.Salt)));
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

        public IActionResult EncryptOrDecryptChoice(int id)
        {
            var model = _accountRepository.FindById(id);
            if(model == null)
            {
                return View("Not found"); //mss beter een of andere fancy niet gevonden pagina
            }
            return View(model);
        }

        //https://codereview.stackexchange.com/questions/93614/salt-generation-in-c
        //STEEKT MOMENTEEL GEWOON HIER. NIEUWE KLASSE MAKEN ?
        public static string getSalt()
        {
            var random = new RNGCryptoServiceProvider();

            // Maximum length of salt
            int max_length = 32;

            // Empty salt array
            byte[] salt = new byte[max_length];

            // Build the random bytes
            random.GetNonZeroBytes(salt);

            // Return the string encoded salt
            return Convert.ToBase64String(salt);
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