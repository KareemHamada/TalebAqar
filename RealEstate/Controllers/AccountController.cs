﻿
namespace RealEstate.Controllers
{
    public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
        [Authorize(Roles = "Admin")]
        public IActionResult Register()
		{
			return View();
		}

        [Authorize(Roles = "Admin")]
        [HttpPost]
		public IActionResult Register(RegisterVM model)
		{
			if (!ModelState.IsValid) 
				return View(model);

			var user = new ApplicationUser
			{
				UserName = model.UserName,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
			};

			var result = _userManager.CreateAsync(user, model.Password).Result;

			if (result.Succeeded)
				return RedirectToAction("Index", "Users", new { area = "Admin" });
			//return RedirectToAction(nameof(Login));

			foreach (var error in result.Errors)
				ModelState.AddModelError(string.Empty, error.Description);

			return View();
		}

        public IActionResult Login()
		{
			return View();
		}

        [HttpPost]
		public IActionResult Login(LoginVM model)
		{
			// server side validation
			if (!ModelState.IsValid) 
				return View(model);

			// check if user exist
			var user = _userManager.FindByEmailAsync(model.Email).Result;

			if (user is not null)
			{
				//check password
				if (_userManager.CheckPasswordAsync(user, model.Password).Result)
				{
					// login if password is correct
					var result = _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false).Result;

					if (result.Succeeded)
                        //return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", string.Empty));
                        return RedirectToAction("Index", "Home", new { area = "AdminArea289" });


                    ModelState.AddModelError(string.Empty, "الاميل او الباسورد غير صحيح");

					return View();
				}
			}
            ModelState.AddModelError(string.Empty, "الاميل او الباسورد غير صحيح");

            return View(model);
		}


        public new IActionResult SignOut()
		{
			_signInManager.SignOutAsync();

			return RedirectToAction(nameof(Login));
		}



        [Authorize(Roles = "Admin")]
        public IActionResult ForgetPassword()
		{
			return View();
		}



        [Authorize(Roles = "Admin")]
        [HttpPost]
		public IActionResult ForgetPassword(ForgetPasswordVM model)
		{
			// server side validation
			if (!ModelState.IsValid) 
				return View(model);

			// check if user exist
			var user = _userManager.FindByEmailAsync(model.Email).Result;

			if (user is not null)
			{
				// create reset password token
				var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;

				//create url to reset password
				var url = Url.Action(nameof(ResetPassord), nameof(AccountController).Replace("Controller", string.Empty), new { email = model.Email, Token = token }, Request.Scheme);
				//create email object
				var email = new Utilities.Email()
				{
					Subject = "Reset Password",
					Body = url!,
					Recipient = model.Email
				};
				// send email
				MailSettings.SendEmail(email);
				// 
				// redirect to check url inbox
				return RedirectToAction(nameof(CheckYourInBox));
			}
			ModelState.AddModelError(string.Empty, "User not found");
			return View(model);
		}

        [Authorize(Roles = "Admin")]
        public IActionResult CheckYourInBox()
		{
			return View();
		}


        [Authorize(Roles = "Admin")]
        public IActionResult ResetPassord(string email, string token)
		{
			if (email is null || token is null)
				return BadRequest();

			TempData["Email"] = email;
			TempData["Token"] = token;
			return View();
		}

        [Authorize(Roles = "Admin")]

        public IActionResult ResetPassord(ResetPassordVM model)
		{
			model.Token = TempData["Token"]?.ToString() ?? string.Empty;
			model.Email = TempData["Email"]?.ToString() ?? string.Empty;


			// server side validation
			if (!ModelState.IsValid) return View(model);

			var user = _userManager.FindByEmailAsync(model.Email).Result;
			if (user != null)
			{
				var result = _userManager.ResetPasswordAsync(user, model.Token, model.Password).Result;
				if (result.Succeeded)
					return RedirectToAction(nameof(Login));

				foreach (var error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);
			}
			ModelState.AddModelError(string.Empty, "user not found");

			return View(model);
		}

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
