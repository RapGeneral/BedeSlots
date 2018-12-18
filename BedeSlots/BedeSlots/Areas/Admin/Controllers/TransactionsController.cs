using BedeSlots.Areas.Admin.Models;
using BedeSlots.DataModels;
using BedeSlots.GlobalData.GlobalViewModels;
using BedeSlots.Infrastructure.Providers.Interfaces;
using BedeSlots.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BedeSlots.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
    [Authorize(Roles = "Administrator")]
    public class TransactionsController : Controller
    {
        private readonly IMemoryCache cache;
        private readonly IUserManager<User> userManager;
        private readonly ITransactionServices transcationServices;
        private readonly int PAGE_SIZE = 15;

        public TransactionsController(IUserManager<User> userManager, ITransactionServices transcationServices, IMemoryCache cache)
        {
            this.cache = cache;
            this.userManager = userManager;
            this.transcationServices = transcationServices;
        }

        public async Task<IActionResult> Index(int? page, string username, int? min, int? max, string types, string sortBy, bool descending)
        {
            var resultTransactions = await transcationServices.SearchTransactionAsync(username, min, max, types?.Split(','), sortBy, descending);

            var pagedTransactions = await resultTransactions.ToPagedListAsync(page ?? 1, PAGE_SIZE);

            var cachedSelectListTypes = await cache.GetOrCreateAsync("TransactionTypes", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(4);
                var transactionTypes = await transcationServices.GetTypesAsync();
                return transactionTypes.Select(t => new SelectListItem(t, t));
            });

            var cachedSelectListSortProps = cache.GetOrCreate("SortProperties", entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(4);
                return typeof(TransactionViewModel).GetProperties().Select(p => new SelectListItem(p.Name, p.Name));
            });

            var model = new IndexViewModel
            {
                TransactionTypes = cachedSelectListTypes,
                PagedTransactions = pagedTransactions,
                SortProp = cachedSelectListSortProps
            };

            return View(model);
        }

        public async Task<IActionResult> TransactionGrid(int? page, string username, int? min, int? max, string types, string sortBy, bool? descending)
        {
            var resultTransactions = await transcationServices.SearchTransactionAsync(username, min, max, types?.Split(','), sortBy, descending ?? false);

            var pagedTransactions = await resultTransactions
                .OrderBy(u => u.Username)
                .ToPagedListAsync(page ?? 1, PAGE_SIZE);

            return PartialView("_TransactionGrid", pagedTransactions);
        }
    }
}