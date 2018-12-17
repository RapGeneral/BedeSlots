using BedeSlots.DataModels;
using BedeSlots.ViewModels.Providers;
using BedeSlots.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using Microsoft.AspNetCore.Identity;

namespace BedeSlots.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("[area]/[controller]/[action]")]
	//[Authorize(Roles = "Administrator")]
	public class UsersController : Controller
	{
        private readonly IUserServices userServices;
        private readonly IUserManager<User> userManager;
        private readonly int PAGE_SIZE = 15;

		public UsersController(IUserManager<User> userManager, IUserServices userServices)
		{
            this.userServices = userServices;
			this.userManager = userManager;
		}
        public async Task<IActionResult> Index(int? page, string username)
        {
            var resultUsers = await userServices.SearchByUsernameAsync(username);

            var users = await resultUsers.ToPagedListAsync(page ?? 1, PAGE_SIZE);

			return View(users);
		}
		public async Task<IActionResult> UserGrid(int? page, string username)
		{
            var resultUsers = await userServices.SearchByUsernameAsync(username);

            var pagedUsers = await resultUsers
								.OrderBy(u => u.UserName)
								.ToPagedListAsync(page ?? 1, PAGE_SIZE);

			return PartialView("_UserGrid", pagedUsers);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LockUser(string userId, int durationInDays)
		{
            if(durationInDays < 1)
            {
                return this.PartialView("_StatusMessage", "Error: How do you expect to lock the user back in time?");
            }
            if (durationInDays > 36000)
            {
                return this.PartialView("_StatusMessage", "Error: Do you even know that 36000 days are nearly 100 years?");
            }
            var user = userManager.Users.Where(u => u.Id == userId).FirstOrDefault();
			if (user is null)
			{
                return this.PartialView("_StatusMessage", "Error: User not found!");
			}

			var enableLockOutResult = await userManager.SetLockoutEnabledAsync(user, true);
			if (!enableLockOutResult.Succeeded)
			{
                return this.PartialView("_StatusMessage", "Error: Could enable the lockout on the user!");
            }
			var lockoutTimeResult = await userManager.SetLockoutEndDateAsync(user, DateTime.Today.AddDays(durationInDays));
			if (!lockoutTimeResult.Succeeded)
			{
                return this.PartialView("_StatusMessage", "Error: Could not add time to user's lockout!");
			}
            return this.PartialView("_StatusMessage", $"The user has been successfully locked for {durationInDays} days!");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UnlockUser(string userId)
		{
			var user = userManager.Users.Where(u => u.Id == userId).FirstOrDefault();
			if (user is null)
			{
                return this.PartialView("_StatusMessage", "Error: User not found!");
            }

			var lockoutTimeResult = await userManager.SetLockoutEndDateAsync(user, DateTime.Now);
			if (!lockoutTimeResult.Succeeded)
			{
                return this.PartialView("_StatusMessage", "Error: Could not add time to user's lockout!");
			}
            return this.PartialView("_StatusMessage", "The user has been successfully unlocked!");
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PromoteUser(string userId)
        {
            var user = userManager.Users.Where(u => u.Id == userId).FirstOrDefault();
            if (user is null)
            {
                return this.PartialView("_StatusMessage", "Error: User not found!");
            }

            var result = await userManager.AddToRoleAsync(user, "Administrator");
            if (!result.Succeeded)
            {
                return this.PartialView("_StatusMessage", "Error: Could not promote user!");
            }
            return this.PartialView("_StatusMessage", "The user has been successfully promoted!");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DemoteUser(string userId)
        {
            var user = userManager.Users.Where(u => u.Id == userId).FirstOrDefault();
            if (user is null)
            {
                return this.PartialView("_StatusMessage", "Error: User not found!");
            }

            var result = await userManager.RemoveFromRoleAsync(user, "Administrator");
            if (!result.Succeeded)
            {
                return this.PartialView("_StatusMessage", "Error: Could not demote user!");
            }
            return this.PartialView("_StatusMessage", "The user has been successfully demoted!");
        }
    }
}