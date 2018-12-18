using BedeSlots.Areas.Identity.Models.AccountViewModels.Validation;
using BedeSlots.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Areas.Identity.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(25, ErrorMessage = "The username must be max 25 charactes long.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        [Required(ErrorMessage = "Currency is required!")]
        [Display(Name = "Choose currency")]        
        public string CurrencyName { get; set; }

        public IEnumerable<SelectListItem> Currencies { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }
    }
}