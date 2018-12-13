using BedeSlots.DataModels;
using BedeSlots.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BedeSlots.ViewComponents
{
    [ViewComponent]
    public class Money : ViewComponent
    {
        private IUserServices userServices;
        private readonly UserManager<User> userManager;

        public Money(IUserServices userServices, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.userServices = userServices;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var balanceInfo = await userServices.GetBalanceInformation(userManager.GetUserId(this.User as ClaimsPrincipal));
            return View(balanceInfo);
        }
    }
}
