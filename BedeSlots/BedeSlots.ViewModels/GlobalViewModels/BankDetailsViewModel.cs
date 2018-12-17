using System;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.ViewModels.GlobalViewModels
{
    public class BankDetailsViewModel
    {
        public string Id { get; set; }
        [Required]
        [CreditCard]
        public string Number { get; set; }

        [Required]
        [Range(100, 9999, ErrorMessage = "The number is not valid!")]
        public int Cvv { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{mm/yy}")]
        public DateTime ExpiryDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}