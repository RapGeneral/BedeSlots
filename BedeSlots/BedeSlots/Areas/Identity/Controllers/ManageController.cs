using BedeSlots.Areas.Identity.Models.ManageViewModels;
using BedeSlots.DataModels;
using BedeSlots.Services.Contracts;
using BedeSlots.ViewModels.Enums;
using BedeSlots.ViewModels.GlobalViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private readonly IUserBankDetailsServices userBankDetailsServices;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signinManager;
        private readonly IUserServices userServices;
        private readonly ITransactionServices transactionServices;
        private readonly IBankDetailsServices bankDetailsServices;

        public ManageController(
            UserManager<User> userManager,
            SignInManager<User> signinManager,
            ITransactionServices transactionServices,
            IUserServices userServices,
            IBankDetailsServices bankDetailsServices,
            IUserBankDetailsServices userBankDetailsServices)
        {
            this.userBankDetailsServices = userBankDetailsServices;
            this.userManager = userManager;
            this.signinManager = signinManager;
            this.userServices = userServices;
            this.transactionServices = transactionServices;
            this.bankDetailsServices = bankDetailsServices;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Deposit()
        {
            var userCards = await userServices.GetBankDetailsInformation(userManager.GetUserId(User));

            var model = new DepositViewModel
            {
                StatusMessage = StatusMessage,
                UserCards = userCards.Select(uc => new SelectListItem(uc.Number, uc.Id)).ToList()
            };
            return View(model);
        }
        [HttpDelete]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(string cardId)
        {
            var detachedCard = await userBankDetailsServices.DeleteUserBankDetailsAsync(cardId, userManager.GetUserId(User));
            return this.PartialView("_StatusMessage", $"Successfully deleted the card with #{detachedCard.Number}!");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(DepositViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = userManager.GetUserId(User);
            var depositedAmountInBaseCurrency = await userServices.UpdateUserBalanceByAmount(model.DepositAmount, userId);
            await transactionServices.CreateTransactionAsync(TypeOfTransaction.Deposit, $"Deposited with credid card #{model.SelectedCard}", depositedAmountInBaseCurrency, userId);
            StatusMessage = $"Balances updated by {model.DepositAmount}.";

            return RedirectToAction(nameof(Deposit));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await signinManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "Your password has been changed.";

            return RedirectToAction(nameof(ChangePassword));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCreditCard(BankDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.PartialView("_StatusMessage", "Error: Wrong format!");
            }

            var newBankDetails = await bankDetailsServices.AddBankDetailsAsync(model.Number, model.Cvv, model.ExpiryDate, userManager.GetUserId(User));

            return this.PartialView("_StatusMessage", "Successfully added the new credit card!");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
