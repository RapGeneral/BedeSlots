using BedeSlots.DataModels;
using BedeSlots.GlobalData.Enums;
using BedeSlots.Infrastructure.Providers.Interfaces;
using BedeSlots.Models;
using BedeSlots.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJsonConverter jsonConverter;
        private readonly IUserManager<User> userManager;
        private readonly IUserServices userServices;
        private readonly ITransactionServices transactionServices;
        private readonly ISlotGamesServices slotGameServices;

        public HomeController(
            IUserManager<User> userManager,
            ITransactionServices transactionServices,
            IUserServices userServices,
            ISlotGamesServices slotGameServices,
            IJsonConverter jsonConverter)
        {
            this.jsonConverter = jsonConverter;
            this.userManager = userManager;
            this.userServices = userServices ?? throw new System.ArgumentNullException(nameof(userServices));
            this.transactionServices = transactionServices;
            this.slotGameServices = slotGameServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult SlotGame(int N, int M)
        {
            var model = new SlotGameViewModel { N = N, M = M };
            return View("SlotGame", model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SlotGame(SlotGameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return PartialView("_StatusMessage", "Error: Incorrect format!");
            }
            var userId = userManager.GetUserId(User);
            var balanceInfo = await userServices.GetBalanceInformation(userId);
            if (balanceInfo.Amount - model.Stake < 0)
            {
                Response.StatusCode = 400;
                return PartialView("_StatusMessage", "Error: You cant bet more than what you have!");
            }
            var usdChangeOfStake = await userServices.UpdateUserBalanceByAmount(-model.Stake, userId);
            await transactionServices.CreateTransactionAsync(TypeOfTransaction.Stake, "Stake", usdChangeOfStake, userId);
            var gameMatrix = slotGameServices.Run(model.N, model.M);
            var coef = slotGameServices.Evaluate(gameMatrix);
            var earnings = model.Stake * coef;
            if (coef != 0)
            {
                decimal usdChangeOfEarnings;
                lock (userServices)
                {
                    usdChangeOfEarnings = userServices.UpdateUserBalanceByAmount(earnings, userId).Result;
                }
                lock (transactionServices)
                {
                    transactionServices.CreateTransactionAsync(TypeOfTransaction.Win, "Win", usdChangeOfEarnings, userId);
                }
            }
            //serialize matrix to json
            string result = jsonConverter.SerializeObject(gameMatrix, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            });
            return Json(result);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Money()
        {
            return ViewComponent(nameof(Money));
        }
    }
}
