using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BadeSlots.Areas.Identity.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string ImageUrl { get; set; }

        public string StatusMessage { get; set; }
    }
}
