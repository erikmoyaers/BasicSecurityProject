using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BasicSecurityProject.Models;
using BasicSecurityProject.Services;
using BasicSecurityProject.ViewModel;
using Hybrid;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

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
        //TODO: in constructor bijvlammen
        private readonly AesEncryption _aes = new AesEncryption();
        private readonly RSAEncryption _rsa = new RSAEncryption();

        private Account _fromAccount;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            System.IO.File.AppendAllText("log.txt", "GET: index laten zien" + Environment.NewLine);
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
                        _fromAccount = _accountRepository.FindById(accountToLoginTo.ID);
                        System.IO.File.AppendAllText("log.txt", "POST: ingelogd in index" + Environment.NewLine);
                        return RedirectToAction(nameof(EncryptOrDecryptChoice));
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
                        _fromAccount.ID = newAccount.ID;
                        return RedirectToAction(nameof(EncryptOrDecryptChoice));//potentiele fout: hoe weet die het ID ?
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

        public IActionResult EncryptOrDecryptChoice()
        {
            System.IO.File.AppendAllText("log.txt", "GET: encrypt of decrypt page laten zien" + Environment.NewLine);
            return View();
        }

        [HttpGet]
        public IActionResult Encryption()
        {
            System.IO.File.AppendAllText("log.txt", "GET: encrypt laten zien" + Environment.NewLine);
            return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> Encryption(EncryptionViewModel model)
        {
            System.IO.File.AppendAllText("log.txt", "POST: post binnengeraakt" + Environment.NewLine);
            if (ModelState.IsValid)
            {
                System.IO.File.AppendAllText("log.txt", "POST: modelstate is valid" + Environment.NewLine);
                //bij encrypteren
                Account toAccount = _accountRepository.GetAll().First(x => x.Username == model.ToUserUsername);
                //programma genereert een public en private key voor alice (fromAccount)
                _rsa.GenerateKeysInFile("GeneratedKeys", "Public_A", "Private_A");
                //programma genereert een public en private key voor bob (toUser)
                _rsa.GenerateKeysInFile("GeneratedKeys", "Public_B", "Private_B");

                //programma vraagt input + gebruikt symmetric key om dit te versleutelen
                //OUTPUT: File_1
                _aes.Key = Encoding.ASCII.GetBytes("r5u8x/A?D*G-KaPdSgVkYp3s6v9y$B&E"); //LETTERLIJK HARDGECODEERD --> INLADEN + 32 tekens = 256 bits
                _aes.Iv = Encoding.ASCII.GetBytes("p3s5v8y/B?E(H+Mb"); //LETTERLIJK HARDGECODEERD --> INLADEN + 16 tekens = 128 bits

                System.IO.File.AppendAllText("log.txt", "POST: buiten stream" + Environment.NewLine);
                using (var memoryStream = new MemoryStream())
                {
                    System.IO.File.AppendAllText("log.txt", "POST: eerste lijn in stream" + Environment.NewLine);
                    await model.File.CopyToAsync(memoryStream);
                    System.IO.File.AppendAllText("log.txt", "POST: tweede lijn in stream" + Environment.NewLine);
                    _aes.Encrypt("GeneratedFiles", "File_1", memoryStream.ToArray());
                    System.IO.File.AppendAllText("log.txt", "POST: gedaan met stream" + Environment.NewLine);
                }

                //programma encrypteerd de symmetric key met public key v bob
                //OUTPUT: File_2
                
                //_rsa.EncryptData(_aes.Key, toAccount.PublicKey);
                
                System.IO.File.AppendAllText("log.txt", "POST: _aes versleuteling gelukt" + Environment.NewLine);
                //return View(model);
                return RedirectToAction(nameof(EncryptOrDecryptChoice));
            }
            else
            {
                //ModelState.AddModelError(String.Empty, "returned model was null from encryption screen");
                return View(model);
            } 
        }

        public IActionResult test()
        {
            System.IO.File.AppendAllText("log.txt", "GET: test pagina geentered" + Environment.NewLine);
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