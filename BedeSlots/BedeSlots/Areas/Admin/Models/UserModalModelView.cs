using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Areas.Admin.Models
{
    public class UserModalModelView
    {
        [Required]
        public string ID { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}