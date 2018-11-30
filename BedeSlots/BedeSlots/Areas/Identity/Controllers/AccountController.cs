using BedeSlots.Areas.Identity.Models.AccountViewModels;
using BedeSlots.DataModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BedeSlots.Areas.Identity.Controllers
{
	[Area("Identity")]
	[Route("[controller]/[action]")]
	public class AccountController : Controller
	{
		private readonly UserManager<User> userManager;
		private readonly SignInManager<User> signinManager;

		public AccountController(
			UserManager<User> userManager,
			SignInManager<User> signInManager)
		{
			this.userManager = userManager;
			this.signinManager = signInManager;
		}

		[TempData]
		public string ErrorMessage { get; set; }

		public async Task<IActionResult> Login()
		{
			// Clear the existing external cookie to ensure a clean login process
			await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			return this.View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (this.ModelState.IsValid)
			{
				// This doesn't count login failures towards account lockout
				// To enable password failures to trigger account lockout, set lockoutOnFailure: true
				var result = await this.signinManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
				if (result.Succeeded)
				{
					return this.RedirectToAction("", "", new { @area = "" });
				}
				else
				{
					this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
					return this.View(model);
				}
			}

			// If we got this far, something failed, redisplay form
			return this.View(model);
		}

		public IActionResult Register()
		{
			return this.View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (this.ModelState.IsValid)
			{
				var user = new User { UserName = model.Email, Email = model.Email };
				var result = await this.userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					await this.signinManager.SignInAsync(user, isPersistent: false);

					return this.RedirectToAction("", "", new { @area = "" });
				}
				this.AddErrors(result);
			}

			// If we got this far, something failed, redisplay form
			return this.View(model);
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await this.signinManager.SignOutAsync();
			return this.RedirectToAction("", "", new { @area = "" });
		}

		[Authorize]
		public IActionResult AccessDenied()
		{
			return this.View();
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				this.ModelState.AddModelError(string.Empty, error.Description);
			}
		}
	}
}
