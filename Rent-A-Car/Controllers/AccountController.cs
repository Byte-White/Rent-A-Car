using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Rent_A_Car.Models;
using Rent_A_Car.ViewsUserOperations;

namespace Rent_A_Car.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<User> signInManager;
		private readonly UserManager<User> userManager;

		public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
		}
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginView model)
		{
			if (ModelState.IsValid)
			{
				var existingUser = await userManager.FindByEmailAsync(model.Email);

				if (existingUser.Password == model.Password)
				{
					await signInManager.SignInAsync(existingUser, isPersistent: model.RememberMe);
					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", "Email or password is incorrect.");
					return View(model);
				}
			}
			return View(model);
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterView model)
		{
			if (ModelState.IsValid)
			{
				User user = new User
				{
					UserName = model.UserName,
					FirstName = model.FirstName,
					LastName = model.LastName,
					EGN = model.EGN,
					PhoneNumber = model.PhoneNumber,
					Email = model.Email,
					Password = model.Password
				};

				var result = await userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					return RedirectToAction("Login", "Account");
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
					return View(model);
				}
			}
			return View(model);
		}
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
		public IActionResult VerifyEmail()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> VerifyEmail(VerifyEmailView model)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByEmailAsync(model.Email);

				if (user == null)
				{
					ModelState.AddModelError("", "Email is incorrect!");
					return View(model);
				}
				else
				{
					return RedirectToAction("ChangePassword", "Account", new { email = user.Email });
				}
			}
			return View(model);
		}
		public IActionResult ChangePassword(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				return RedirectToAction("VerifyEmail", "Account");
			}
			return View(new ChangePasswordView { Email = email });
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordView model)
		{
			if (ModelState.IsValid)
			{
				var userToEdit = await userManager.FindByEmailAsync(model.Email);

				if (userToEdit != null)
				{
					var result = await userManager.RemovePasswordAsync(userToEdit);
					if (result.Succeeded)
					{
						result = await userManager.AddPasswordAsync(userToEdit, model.NewPassword);
						return RedirectToAction("Login", "Account");
					}
					else
					{

						foreach (var error in result.Errors)
						{
							ModelState.AddModelError("", error.Description);
						}

						return View(model);
					}
				}
				else
				{
					ModelState.AddModelError("", "Email not found!");
					return View(model);
				}
			}
			else
			{
				ModelState.AddModelError("", "Something went wrong. try again.");
				return View(model);
			}
		}
	}
}
