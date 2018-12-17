using BedeSlots.Areas.Identity.Models.AccountViewModels;
using BedeSlots.DataModels;
using BedeSlots.Services.Contracts;
using BedeSlots.GlobalData.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BedeSlots.Infrastructure.Providers.Interfaces;

namespace BedeSlots.Areas.Identity.Controllers
{
	[Area("Identity")]
	[Route("[controller]/[action]")]
	public class AccountController : Controller
	{
		private readonly IUserManager<User> userManager;
		private readonly ISignInManager<User> signinManager;
        private readonly IMemoryCache memoryCache;
        private readonly ICurrencyServices currencyServices;
		private readonly IUserServices userServices;

		public AccountController(
			IUserManager<User> userManager,
			ISignInManager<User> signInManager,
            IMemoryCache memoryCache,
			IUserServices userServices,
            ICurrencyServices currencyServices)
		{
			this.userManager = userManager;
			this.signinManager = signInManager;
            this.memoryCache = memoryCache;
            this.currencyServices = currencyServices;
			this.userServices = userServices;
		}

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
				var result = await this.signinManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
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

		public async Task<IActionResult> Register()
		{
            var cachedSelectListCurrencies = await GetCurrenciesSelectListItemsCached();
            var newRegister = new RegisterViewModel
            {
                Currencies = cachedSelectListCurrencies
            };
            return View(newRegister);            
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (this.ModelState.IsValid)
			{
				var user = new User { UserName = model.Username, Email = model.Email, DateOfBirth = model.DateOfBirth, CreatedOn = DateTime.Now };
				var result = await this.userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					await userServices.CreateUserInitialBalances(user.Id, model.CurrencyName);
                    await userManager.AddToRoleAsync(user, UserRoles.User.ToString());
					await this.signinManager.SignInAsync(user, isPersistent: false);

					return this.RedirectToAction("", "", new { @area = "" });
				}
				this.AddErrors(result);
			}

            // If we got this far, something failed, redisplay form
            model.Currencies = await GetCurrenciesSelectListItemsCached();
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
        
        private async Task<IEnumerable<SelectListItem>> GetCurrenciesSelectListItemsCached()
        {
            return await memoryCache.GetOrCreateAsync("Currencies", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(4);
                var currencies = await currencyServices.GetCurrenciesAsync();
                return currencies.Select(c => new SelectListItem(c, c));
            });
        }
	}
}