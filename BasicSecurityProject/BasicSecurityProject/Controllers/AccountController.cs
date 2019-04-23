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
        private readonly ISaltGenerator _saltGenerator = new SaltGenerator();
        //TODO: in constructor bijvlammen
        private readonly AesEncryption _aes = new AesEncryption();
        private readonly RSAEncryption _rsa = new RSAEncryption();
        private readonly SHA256 _sha256 = SHA256.Create();

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
        public IActionResult Register(RegisterViewModel model)
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
                        //NIET PRAKTISCH: eerst wordt een key gesaved, dan wordt deze automatisch opgehaald
                        _rsa.GenerateKeysInFile(model.PathToSaveKeysTo, model.PublicKeyName, model.PrivateKeyName);
                        newAccount.PublicKey = System.IO.File.ReadAllBytes(model.PathToSaveKeysTo + "/" + model.PublicKeyName);
                        _accountRepository.CreateAccount(newAccount);
                        
                        //_fromAccount.ID = newAccount.ID;
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

        [HttpGet]
        public IActionResult Decryption()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Encryption(EncryptionViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                byte[] key;
                byte[] encryptedKey;
                byte[] iv;
                byte[] encryptedIV;
                byte[] toUserPublicKeyAsByteArray;
                byte[] inputFile;
                byte[] hashedInputFile;
                byte[] fromUserPrivateKeyAsByteArray;
                byte[] signedHash;

                using (var memoryStream = new MemoryStream())
                {
                    await model.AesKey.CopyToAsync(memoryStream);
                    key = memoryStream.ToArray();
                }
                using (var memoryStream = new MemoryStream())
                {
                    await model.Iv.CopyToAsync(memoryStream);
                    iv = memoryStream.ToArray();
                }

                //2)file1: output = geencrypteerd file
                using (var memoryStream = new MemoryStream())
                {
                    await model.File.CopyToAsync(memoryStream);
                    inputFile = memoryStream.ToArray();
                    System.IO.File.WriteAllBytes(model.FolderToSaveFile1 + "/File_1", _aes.Encrypt(memoryStream.ToArray(), key, iv));
                }

                //file2: output = symmetric key (bij ons ook iv want AES) versleuteld met public key bob
                toUserPublicKeyAsByteArray = _accountRepository.FindByName(model.ToUserUsername).PublicKey;
                RSAParameters toUserPublicKey = RSAEncryption.convertStringToKey(Encoding.Default.GetString(toUserPublicKeyAsByteArray));
                encryptedKey = _rsa.EncryptData(key, toUserPublicKey);
                encryptedIV = _rsa.EncryptData(iv, toUserPublicKey);
                System.IO.File.WriteAllBytes(model.FolderToSaveFile2 + "/File_2", encryptedKey);
                System.IO.File.WriteAllBytes(model.FolderToSaveFile2 + "/File_2_IV", encryptedIV);

                //file3: hash originele bestand, geencrypteerd met private key van alice
                hashedInputFile = _sha256.ComputeHash(inputFile);
                using (var memoryStream = new MemoryStream())
                {
                    await model.PrivateKey.CopyToAsync(memoryStream);
                    fromUserPrivateKeyAsByteArray = memoryStream.ToArray();
                }
                signedHash = _rsa.SignData(hashedInputFile, RSAEncryption.convertStringToKey(Encoding.Default.GetString(fromUserPrivateKeyAsByteArray)));
                System.IO.File.WriteAllBytes(model.FolderToSaveFile3 + "/File_3", signedHash);

                /*
                //decrypteren

                //stap 1: aes sleutel wordt verkregen door file 2 te decrypteren met private_b
                byte[] keyToDecrypt = System.IO.File.ReadAllBytes("GeneratedFiles/File_2");
                byte[] ivToDecrypt = System.IO.File.ReadAllBytes("GeneratedFiles/File_2_IV");
                byte[] keyAfterDecryption = _rsa.DecryptData(keyToDecrypt, RSAEncryption.convertStringToKey(Encoding.Default.GetString(System.IO.File.ReadAllBytes("GeneratedKeys/Private_B"))));
                byte[] ivAfterDecryption = _rsa.DecryptData(ivToDecrypt, RSAEncryption.convertStringToKey(Encoding.Default.GetString(System.IO.File.ReadAllBytes("GeneratedKeys/Private_B"))));
                System.IO.File.AppendAllText("log.txt", "AES originele key in hex " + byteToHex(key) + Environment.NewLine);
                System.IO.File.AppendAllText("log.txt", "AES originele iv in hex  " + byteToHex(iv) + Environment.NewLine);
                System.IO.File.AppendAllText("log.txt", "AES decrypted key in hex " + byteToHex(keyAfterDecryption) + Environment.NewLine);
                System.IO.File.AppendAllText("log.txt", "AES decrypted iv in hex  " + byteToHex(ivAfterDecryption) + Environment.NewLine);

                //stap 2: file1 wordt gedecrypteerd met de sleutel van vorige stap
                byte[] file1  = System.IO.File.ReadAllBytes("GeneratedFiles/File_1");
                byte[] file1Decrypted = _aes.Decrypt(file1, keyAfterDecryption, ivAfterDecryption);

                System.IO.File.AppendAllText("log.txt", "file dat gedecrypteerd moet worden " + Encoding.UTF8.GetString(file1) + Environment.NewLine);
                System.IO.File.AppendAllText("log.txt", "file na decryptie " + Encoding.UTF8.GetString(file1Decrypted) + Environment.NewLine);

                //stap 3: hash berekenen van de originele boodschap
                byte[] hashedDecryptedFile = _sha256.ComputeHash(file1Decrypted);
                byte[] file3 = System.IO.File.ReadAllBytes("GeneratedFiles/File_3");
                if(_rsa.VerifySignature(hashedDecryptedFile, file3, RSAEncryption.convertStringToKey(Encoding.Default.GetString(System.IO.File.ReadAllBytes("GeneratedKeys/Public_A")))))
                {
                    System.IO.File.AppendAllText("log.txt", "END");
                }
                else
                {
                    System.IO.File.AppendAllText("log.txt", "FUCK");
                }
                */
                return RedirectToAction(nameof(EncryptOrDecryptChoice));
            }
            else
            {
                //ModelState.AddModelError(String.Empty, "returned model was null from encryption screen");
                return View(model);
            } 
        }

        [HttpPost]
        public async Task<IActionResult> Decryption(DecryptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                //stap 1: aes sleutel wordt verkregen door file 2 te decrypteren met private_b
                byte[] keyToDecrypt;
                byte[] ivToDecrypt;
                byte[] toUserPrivateKeyAsByteArray;
                byte[] keyAfterDecryption;
                byte[] ivAfterDecryption;
                byte[] file1;
                byte[] file1Decrypted;
                byte[] hashedDecryptedFile;
                byte[] file3;
                byte[] fromUserPublicKeyAsByteArray;

                using (var memoryStream = new MemoryStream())
                {
                    await model.File_2.CopyToAsync(memoryStream);
                    keyToDecrypt = memoryStream.ToArray();
                }
                using (var memoryStream = new MemoryStream())
                {
                    await model.File_2_IV.CopyToAsync(memoryStream);
                    ivToDecrypt = memoryStream.ToArray();
                }
                using (var memoryStream = new MemoryStream())
                {
                    await model.PrivateKey.CopyToAsync(memoryStream);
                    toUserPrivateKeyAsByteArray = memoryStream.ToArray();
                }
                RSAParameters toUserPrivateKey = RSAEncryption.convertStringToKey(Encoding.Default.GetString(toUserPrivateKeyAsByteArray));
                keyAfterDecryption = _rsa.DecryptData(keyToDecrypt, toUserPrivateKey);
                ivAfterDecryption = _rsa.DecryptData(ivToDecrypt, toUserPrivateKey);

                //stap 2: file1 wordt gedecrypteerd met de sleutel van vorige stap
                using (var memoryStream = new MemoryStream())
                {
                    await model.File_1.CopyToAsync(memoryStream);
                    file1 = memoryStream.ToArray();
                }
                file1Decrypted = _aes.Decrypt(file1, keyAfterDecryption, ivAfterDecryption);
                //TODO: zoek een manier om deze proper op het scherm te zetten (pop up bericht ofzo)
                System.IO.File.AppendAllText("log.txt", "file na decryptie " + Encoding.UTF8.GetString(file1Decrypted) + Environment.NewLine);

                //stap 3: hash berekenen van de originele boodschap
                hashedDecryptedFile = _sha256.ComputeHash(file1Decrypted);
                using (var memoryStream = new MemoryStream())
                {
                    await model.File_3.CopyToAsync(memoryStream);
                    file3 = memoryStream.ToArray();
                }

                fromUserPublicKeyAsByteArray = _accountRepository.FindByName(model.FromUserUsername).PublicKey;
                RSAParameters fromUserPublicKey = RSAEncryption.convertStringToKey(Encoding.Default.GetString(fromUserPublicKeyAsByteArray));

                if (_rsa.VerifySignature(hashedDecryptedFile, file3, fromUserPublicKey))
                {
                    return RedirectToAction(nameof(EncryptOrDecryptChoice));
                }
                else
                {
                    //TODO: errormessage
                    return View(model);
                }
            }
            else
            {
                //TODO: errormessage
                return View(model);
            }
        }

        public string byteToHex(byte[] ba)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in ba)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

    }
}