using X.PagedList;
using System.Collections.Generic;
using System.Linq;
using BedeSlots.DataModels;

namespace BedeSlots.Areas.Admin.Models
{
    public class IndexViewModel
    {
        public IndexViewModel(IEnumerable<User> users, int pageNumber, int pageSize)
        {
            this.Users = users.Select(u => new UserViewModel(u)).ToPagedList(pageNumber, pageSize);
        }
        public string StatusMessage { get; set; }
        public IPagedList<UserViewModel> Users { get; set; } 
    }
}
