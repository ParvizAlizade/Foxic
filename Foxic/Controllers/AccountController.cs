using Foxic.Entities;
using Foxic.Utilities.Roles;
using Foxic.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Data;

namespace Foxic.Controllers
{
    public class AccountController : Controller
	{
		private readonly UserManager<User> _userManager;

		public SignInManager<User> _signInManager { get; }
		public RoleManager<IdentityRole> _roleManager { get; }

		public AccountController(UserManager<User>userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
		}

        public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterVM account)
		{
			if (!ModelState.IsValid) return View();
			if (account.ConfirmPassword!=account.Password)
			{
				ModelState.AddModelError("","Username or Password is incorrect");
			}
			if (!account.Terms) return View();
			User user = new User
			{
				Fullname = string.Concat(account.Firstname, " ", account.Lastname),
				UserName = account.Username,
				Email = account.Email
			};
			IdentityResult result = await _userManager.CreateAsync(user, account.Password);

			if (!result.Succeeded)
			{
				foreach (IdentityError message in result.Errors)
				{
					ModelState.AddModelError("", message.Description);
				}
				return View();
			}
			await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
			return RedirectToAction("Login", "Account"); 
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginVM login)
		{
			if (!ModelState.IsValid) return View();
			if (login == null) return View();


			User user = await _userManager.FindByNameAsync(login.Username);
			Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);
			if (!result.Succeeded)
			{
				if (result.IsLockedOut)
				{
					ModelState.AddModelError("", "You blocked for 5 minute");
					return View();
				}
				ModelState.AddModelError("", "Username or password is incorrect");
				return View();
			}
			return RedirectToAction("Index", "Home");
		}

		
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

		//public async Task CreateRoles()
		//{
		//	await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
		//	await _roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
		//	await _roleManager.CreateAsync(new IdentityRole(Roles.Member.ToString()));
		//}


		public async Task<IActionResult> UserDetails()
        {
            if (!ModelState.IsValid) return View();
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
			UserVM userVM = new()
			{
				Email = user.Email,
				Fullname=user.Fullname,
				Username = user.UserName
			};
            return View(userVM);
        }

		public async Task<IActionResult> Checkout()
		{
			return View();
		}
		


    }
}
