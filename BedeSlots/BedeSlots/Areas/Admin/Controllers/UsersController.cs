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
		public async Task<IActionResult> LockUser(UserModalModelView input)
		{
			var user = _userManager.Users.Where(u => u.Id == input.ID).FirstOrDefault();
			if (user is null)
			{
				this.StatusMessage = "Error: User not found!";
				return this.RedirectToAction(nameof(Index));
			}

			var enableLockOutResult = await _userManager.SetLockoutEnabledAsync(user, true);
			if (!enableLockOutResult.Succeeded)
			{
				this.StatusMessage = "Error: Could enable the lockout on the user!";
				return this.RedirectToAction(nameof(Index));
			}
			var lockoutTimeResult = await _userManager.SetLockoutEndDateAsync(user, DateTime.Today.AddYears(10));
			if (!lockoutTimeResult.Succeeded)
			{
				this.StatusMessage = "Error: Could not add time to user's lockout!";
				return this.RedirectToAction(nameof(Index));
			}
			this.StatusMessage = "The user has been successfully locked for 10 years!";
			return this.RedirectToAction(nameof(Index));
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UnlockUser(UserModalModelView input)
		{
			var user = _userManager.Users.Where(u => u.Id == input.ID).FirstOrDefault();
			if (user is null)
			{
				this.StatusMessage = "Error: User not found!";
				return this.RedirectToAction(nameof(Index));
			}

			var lockoutTimeResult = await _userManager.SetLockoutEndDateAsync(user, DateTime.Now);
			if (!lockoutTimeResult.Succeeded)
			{
				this.StatusMessage = "Error: Could not add time to user's lockout!";
				return this.RedirectToAction(nameof(Index));
			}
			this.StatusMessage = "The user has been successfully unlocked!";
			return this.RedirectToAction(nameof(Index));
		}
	}
}