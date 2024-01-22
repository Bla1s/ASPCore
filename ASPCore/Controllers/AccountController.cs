using ASPCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASPCore.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private UserManager<AppUser> userManager;
		private SignInManager<AppUser> signInManager;

		public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signinMgr)
		{
			userManager = userMgr;
			signInManager = signinMgr;
		}

		[AllowAnonymous]
		public IActionResult Login(string returnUrl)
		{
			Login login = new Login();
			login.ReturnUrl = returnUrl;
			return View(login);
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(Login login)
		{
			if (ModelState.IsValid)
			{
				AppUser appUser = await userManager.FindByEmailAsync(login.Email);
				if (appUser != null)
				{
					await signInManager.SignOutAsync();
					Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, login.Password, login.Remember, false);
					if (result.Succeeded)
						return Redirect(login.ReturnUrl ?? "/");
				}
				ModelState.AddModelError(nameof(login.Email), "Login Failed: Invalid Email or password");
			}
			return View(login);
		}
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

        [AllowAnonymous]
        public IActionResult Register(string returnUrl)
        {
            Register register = new Register();
            register.ReturnUrl = returnUrl;
            return View(register);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register register)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = register.Name,
                    Email = register.Email
                   
                };

                var result = await userManager.CreateAsync(user, register.Password);

                if (result.Succeeded)
                {
                    
                    return Redirect(register.ReturnUrl ?? "/");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            // If we got this far, something failed, redisplay the form
            return View(register);
        }

    }
}
