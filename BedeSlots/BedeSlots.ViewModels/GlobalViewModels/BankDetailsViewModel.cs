using System;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.ViewModels.GlobalViewModels
{
    public class BankDetailsViewModel
    {
        [CreditCard]
        public string Number { get; set; }

        [Range(100, 999, ErrorMessage = "The number is not valid!")]
        public int Cvv { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ExpiryDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}