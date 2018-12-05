using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BedeSlots.DataModels;
using BedeSlots.Infrastructure.Providers;
using BedeSlots.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace BedeSlots.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
    public class TransactionsController : Controller
    {
        private readonly IUserManager<User> userManager;
        private readonly ITransactionServices transcationServices;
        private readonly int PAGE_SIZE = 1;

        public TransactionsController(IUserManager<User> userManager, ITransactionServices transcationServices)
        {
            this.userManager = userManager;
            this.transcationServices = transcationServices;
        }

        public async Task<IActionResult> Index(int? page, string username, int? min, int? max, ICollection<string> types)
        {
            var resultTransactions = await transcationServices.SearchTransactionAsync(username, min, max, new List<string>());

            var transactions = await resultTransactions.ToPagedListAsync(page ?? 1, PAGE_SIZE);

            return View(transactions);
        }

        public async Task<IActionResult> TransactionGrid(int? page, string username, int? min, int? max, ICollection<string> types)
        {
            var resultTransactions = await transcationServices.SearchTransactionAsync(username, min, max, new List<string>());

            var pagedTransactions = await resultTransactions
                .OrderBy(u => u.Username)
                .ToPagedListAsync(page ?? 1, PAGE_SIZE);

            return PartialView("_TransactionGrid", pagedTransactions);
        }
    }
}