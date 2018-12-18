using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Areas.Identity.Models.ManageViewModels
{
    public class DepositViewModel
    {
        [Range(5, 5000, ErrorMessage = "You can deposit minimum 5 and maximum 5000 at a time!")]
        [Required(ErrorMessage = "You should enter an amount to deposit!")]
        public int DepositAmount { get; set; }
        [Required(ErrorMessage ="You need to select a credit card!")]
        public string SelectedCard { get; set; }
        public ICollection<SelectListItem> UserCards { get; set; }
        public string StatusMessage { get; set; }
    }
}
