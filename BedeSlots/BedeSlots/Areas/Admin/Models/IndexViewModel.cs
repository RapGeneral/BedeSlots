using BedeSlots.GlobalData.GlobalViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BedeSlots.Areas.Admin.Models
{
    public class IndexViewModel
    {
        public IPagedList<TransactionViewModel> PagedTransactions { get; set; }
        public IEnumerable<SelectListItem> TransactionTypes { get; set; }
        public IEnumerable<SelectListItem> SortProp { get; set; }
    }
}
