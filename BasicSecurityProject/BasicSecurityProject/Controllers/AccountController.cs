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
        private readonly SHA256 _sha256 = SHA256.Create();

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
            return View();
        }

        [HttpGet]
        public IActionResult Encryption()
        {
            return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> Encryption(EncryptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                //we zoeken de user naar wie het verstuurd meot worden
                Account toAccount = _accountRepository.GetAll().First(x => x.Username == model.ToUserUsername);

                //****de sleutels worden gegenereerd in een tekstdocument -> het XML formaat wordt gebruikt****
                //programma genereert een public en private key voor alice (fromAccount)
                _rsa.GenerateKeysInFile("GeneratedKeys", "Public_A", "Private_A");
                //programma genereert een public en private key voor bob (toUser)
                _rsa.GenerateKeysInFile("GeneratedKeys", "Public_B", "Private_B");

                //programma vraagt input + gebruikt symmetric key om dit te versleutelen
                //OUTPUT: File_1
                //1)inladen van de key en de iv
                using (var memoryStream = new MemoryStream())
                {
                    await model.AesKey.CopyToAsync(memoryStream);
                    _aes.Key = memoryStream.ToArray();
                }
                using (var memoryStream = new MemoryStream())
                {
                    await model.Iv.CopyToAsync(memoryStream);
                    _aes.Iv = memoryStream.ToArray();
                }
                /*  voorbeeldcode toont dat de IV van een key altijd de helft zo groot is
                _aes.Key = Encoding.ASCII.GetBytes("r5u8x/A?D*G-KaPdSgVkYp3s6v9y$B&E"); //LETTERLIJK HARDGECODEERD --> INLADEN + 32 tekens = 256 bits
                _aes.Iv = Encoding.ASCII.GetBytes("p3s5v8y/B?E(H+Mb"); //LETTERLIJK HARDGECODEERD --> INLADEN + 16 tekens = 128 bits
                */
                //2) wegscrijven file1
                using (var memoryStream = new MemoryStream())
                {
                    await model.File.CopyToAsync(memoryStream);
                    _aes.Encrypt("GeneratedFiles", "File_1", memoryStream.ToArray());
                }

                //programma encrypteerd de symmetric key met public key v bob
                //OUTPUT: File_2
                //****de xml string wordt omgezet naar een bytearray**** --> WAAROM ? op stackoverflow wordt aangeraden keys als bytearrays op te slaan
                //****de bytearray wordt omgezet naar een string**** --> WAAROM ? ik heb geen idee hoe een bytearray direct naar RSAparameters om te zetten
                //****deze string kan omgezet worden naar een key**** --> WAAROM ? zie hierboven
                //****met deze key kan er geencrypteerd worden****
                //****de encryptie geeft een bytearray terug die opgeslagen wordt****
                System.IO.File.WriteAllBytes("GeneratedFiles/File_2", _rsa.EncryptData(_aes.Key, RSAEncryption.convertStringToKey(Encoding.Default.GetString(System.IO.File.ReadAllBytes("GeneratedKeys/Public_B")))));

                //programma maakt een hash van het oorspronkelijk boodschap + encrypteerd met de private key van alice + saved
                //OUTPUT: File_3

                byte[] hashedOriginalMessage;
                using (var memoryStream = new MemoryStream())
                {
                    await model.File.CopyToAsync(memoryStream);
                    hashedOriginalMessage = _sha256.ComputeHash(memoryStream.ToArray());
                }
                //hetzelfde wordt gedaan als bij die block comments bij file2
                System.IO.File.WriteAllBytes("GeneratedFiles/File_3", _rsa.SignData(hashedOriginalMessage, RSAEncryption.convertStringToKey(Encoding.Default.GetString(System.IO.File.ReadAllBytes("GeneratedKeys/Private_A")))));

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