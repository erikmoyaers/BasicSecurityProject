using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicSecurityProject.Models;
using BasicSecurityProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace BasicSecurityProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            
        }

        public IActionResult Index()
        {
            var model = _accountRepository.GetAll();
            return View(model);
        }
    }
}