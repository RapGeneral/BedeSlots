using BedeSlots.DataModels;
using BedeSlots.Models;
using BedeSlots.Services.Contracts;
using BedeSlots.GlobalData.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BedeSlots.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signinManager;
        private readonly IUserServices userServices;
        private readonly ITransactionServices transactionServices;
        private readonly ISlotGamesServices slotGameServices;

        public HomeController(
            UserManager<User> userManager,
            SignInManager<User> signinManager,
            ITransactionServices transactionServices,
            IUserServices userServices,
            ISlotGamesServices slotGameServices)
        {
            this.userManager = userManager;
            this.signinManager = signinManager;
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
            //Also check if the games are exactly 3x3, 5x5 and 8x5
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return this.PartialView("_StatusMessage", "Error: Incorrect format!");
            }
            var userId = userManager.GetUserId(User);
            var balanceInfo = await userServices.GetBalanceInformation(userId);
            if(balanceInfo.Amount - model.Stake < 0)
            {
                Response.StatusCode = 400;
                return this.PartialView("_StatusMessage", "Error: You cant bet more than what you have!");
            }
            var usdChangeOfStake = await userServices.UpdateUserBalanceByAmount(-model.Stake, userId);
            await transactionServices.CreateTransactionAsync(TypeOfTransaction.Stake, "Stake", usdChangeOfStake, userId);
            var gameMatrix = slotGameServices.Run(model.N, model.M);
            var coef = slotGameServices.Evaluate(gameMatrix);
            var earnings = model.Stake * coef;
            if(coef != 0)
            {
                var usdChangeOfEarnings = await userServices.UpdateUserBalanceByAmount(earnings, userId);
                await transactionServices.CreateTransactionAsync(TypeOfTransaction.Win, "Win", usdChangeOfEarnings, userId);
            }
            //serialize matrix to json
            string result = JsonConvert.SerializeObject(gameMatrix, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            });
            return this.Json(result);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Money()
        {
            return ViewComponent(nameof(Money));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
