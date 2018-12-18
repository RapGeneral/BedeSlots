using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Areas.Identity.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
