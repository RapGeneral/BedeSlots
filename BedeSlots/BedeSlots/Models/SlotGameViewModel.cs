using BedeSlots.ViewModels.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Models
{
    public class SlotGameViewModel
    {
        public ICollection<List<GameItemChanceOutOf100>> GameMatrix { get; set; }

        [Required(ErrorMessage = "Stake is required!")]
        [Range(1,1000, ErrorMessage ="Your stakes can be minimum 1 and maximum 1000!")]
        public int Stake { get; set; }
        public int N { get; set; }
        public int M { get; set; }
    }
}