using System.ComponentModel.DataAnnotations;

namespace BedeSlots.GlobalData.GlobalViewModels
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