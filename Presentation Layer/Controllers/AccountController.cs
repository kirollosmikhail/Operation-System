using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.Helpers;
using Presentation_Layer.ViewModels;
using System.Threading.Tasks;

namespace Presentation_Layer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Register
        // baseurl/account/register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    Fname = model.FName,
                    Lname = model.LName,
                    IsAgree = model.IsAgree
                };
                var Result = await _userManager.CreateAsync(User, model.Password);
                if (Result.Succeeded)
                    return RedirectToAction(nameof(Login));
                else
                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        // Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    var Flag = await _userManager.CheckPasswordAsync(User, model.Password);

                    if (Flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Incorrect Password");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email is not Exists");
                }
            }
            return View(model);
        }



        // Sign Out
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }





        // Forget Password
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    // valid for only one time for this user
                    var token = await _userManager.GeneratePasswordResetTokenAsync(User);
                    // https://localhost:44307/account/resetpassword?email=.....@gmail.com?Token=asdasdasdasdasfasd
                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = User.Email, Token = token }, Request.Scheme);

                    var email = new Email()
                    {
                        Subject = "Reset Password",
                        To = model.Email,
                        Body = ResetPasswordLink
                    };
                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourInbox));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email is not exists");
                }
            }
            return View(nameof(ForgetPassword), model);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }
        // Reset Password

        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var User = await _userManager.FindByEmailAsync(email);
                var Result = await _userManager.ResetPasswordAsync(User, token, model.NewPassword);
                if (Result.Succeeded)
                    return RedirectToAction(nameof(Login));
                else
                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }


            return View(model);
        }




    }
}
