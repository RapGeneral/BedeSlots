using System.ComponentModel.DataAnnotations;

namespace BedeSlots.ViewModels.GlobalViewModels
{
    public class UserViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string ID { get; set; }

    }
}