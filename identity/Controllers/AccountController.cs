using identity.Data.Repositorys;
using identity.Models.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identity.Controllers
{
    public class AccountController : Controller
    {
        UserManager<IdentityUser> _user;
        SignInManager<IdentityUser> _signIn;
        IMessageSender _messageSender;
        public AccountController(UserManager<IdentityUser> user, SignInManager<IdentityUser> signInManager, IMessageSender messageSender)
        {
            _user = user;
            _signIn = signInManager;
            _messageSender = messageSender;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (_signIn.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (_signIn.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    Email = model.Email,
                    UserName = model.UserName,
                };
                var result = await _user.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var emailConfirmationToken =
                        await _user.GenerateEmailConfirmationTokenAsync(user);
                    var emailMessage =
                        Url.Action("ConfirmEmail", "Account",
                            new { username = user.UserName, token = emailConfirmationToken },
                            Request.Scheme);
                    await _messageSender.SendEmailAsync(model.Email, "Email confirmation", emailMessage);

                    return View("SucceededRegister", model.UserName);
                }
                foreach (var eror in result.Errors)
                {
                    ModelState.AddModelError("", eror.Description);
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (_signIn.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (_signIn.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            ViewData["returnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signIn.PasswordSignInAsync(model.UserName, model.Password, model.RemmberMe, true);


                if (result.Succeeded)
                {
                    if (returnUrl != null && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut)
                {
                    ViewData["ModelEror"] = "اکانت شما به دلیل ورود پسورد اشتباه به مدت پنج دقیقه قفل میباشد";
                    return View(model);
                }

                else
                {
                    ModelState.AddModelError("", "رمز عبور یا نام کاربری اشتباه است");

                }
            }

            return View(model);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signIn.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> IsEmailInUse(string Email)
        {
            var user = await _user.FindByEmailAsync(Email);

            if (user == null) return Json(true);
            return Json("ایمیل وارد شده از قبل ثبت نام شده است");
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> IsNameInUse(string UserName)
        {
            var user = await _user.FindByNameAsync(UserName);

            if (user == null) return Json(true);
            return Json("نام کاربری وارد شده از قبل ثبت نام شده است");
        }
    }
}
