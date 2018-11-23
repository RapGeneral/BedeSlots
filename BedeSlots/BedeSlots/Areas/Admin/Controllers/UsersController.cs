using BedeSlots.Areas.Admin.Models;
using BedeSlots.DataModels;
using BedeSlots.Providers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BedeSlots.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("[area]/[controller]/[action]")]
	//[Authorize(Roles = "Administrator")]
	public class UsersController : Controller
	{
		private readonly IUserManager<User> _userManager;
		private readonly int PAGE_SIZE = 1;

		[TempData]
		public string StatusMessage { get; set; }

		public UsersController(IUserManager<User> userManager)
		{
			_userManager = userManager;
		}
		public IActionResult Index(int? page, string username)
		{
			IEnumerable<User> users;
			if (username is null)
			{
				users = _userManager.Users;
			}
			else
			{
				users = _userManager.Users.Where(u => u.UserName.ToLower()
									.Contains(username.ToLower()))
									.ToList();
			}

			var indexViewModel = new IndexViewModel(users, page ?? 1, PAGE_SIZE)
			{
				StatusMessage = StatusMessage
			};

			return View(indexViewModel);
		}
		public IActionResult UserGrid(int? page, string username)
		{
			IEnumerable<User> users;
			if (username is null)
			{
				users = _userManager.Users;
			}
			else
			{
				users = _userManager.Users.Where(u => u.UserName.ToLower()
									.Contains(username.ToLower()))
									.ToList();
			}

			var pagedUsers = users
								.Select(u => new UserViewModel(u))
								.OrderBy(u => u.UserName)
								.ToPagedList(page ?? 1, PAGE_SIZE);
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
            var user = _userManager.Users.Where(u => u.Id == userId).FirstOrDefault();
			if (user is null)
			{
                return this.PartialView("_StatusMessage", "Error: User not found!");
			}

			var enableLockOutResult = await _userManager.SetLockoutEnabledAsync(user, true);
			if (!enableLockOutResult.Succeeded)
			{
                return this.PartialView("_StatusMessage", "Error: Could enable the lockout on the user!");
            }
			var lockoutTimeResult = await _userManager.SetLockoutEndDateAsync(user, DateTime.Today.AddDays(durationInDays));
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
			var user = _userManager.Users.Where(u => u.Id == userId).FirstOrDefault();
			if (user is null)
			{
                return this.PartialView("_StatusMessage", "Error: User not found!");
            }

			var lockoutTimeResult = await _userManager.SetLockoutEndDateAsync(user, DateTime.Now);
			if (!lockoutTimeResult.Succeeded)
			{
                return this.PartialView("_StatusMessage", "Error: Could not add time to user's lockout!");
			}
            return this.PartialView("_StatusMessage", "The user has been successfully unlocked!");
		}
	}
}