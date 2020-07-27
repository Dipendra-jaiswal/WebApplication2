using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Models.AccountModel;

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<CustomIdentityUser> userManager;
        private readonly SignInManager<CustomIdentityUser> signInManager;

        public AccountController(UserManager<CustomIdentityUser> userManager,SignInManager<CustomIdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
       
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new CustomIdentityUser { UserName = model.UserName, Email = model.Email, UserProfession = "Software Engineer" };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);                
                   
                    return RedirectToAction("Index", "Student");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("userLogin",$"Error occured during user registration.following errors - {error.Code} - {error.Description}");
                }
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    bool isValid = true;
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                        //return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        isValid = false;
                        ModelState.AddModelError("Local Url", $"Invalid Url return.");
                    }
                    if(isValid)
                    {
                        return RedirectToAction("Index", "Student");
                    }
                }

                ModelState.AddModelError("userLogin", $"Invalid Login Attempt.");
            }
            return View(model);
        }

        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> IsUserNameInUse(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if(user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"UserName {userName} is already in use.");
            }
        }


        public async Task<IActionResult> Logout(RegisterViewModel model)
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}