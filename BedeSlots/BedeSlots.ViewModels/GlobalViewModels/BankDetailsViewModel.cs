using BedeSlots.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BedeSlots.ViewModels.GlobalViewModels
{
    public class BankDetailsViewModel
    {
        public BankDetailsViewModel(BankDetails bankDetails)
        {
            Number = bankDetails.Number;
            Cvv = bankDetails.Cvv;
            ExpiryDate = bankDetails.ExpiryDate;
            CreatedOn = bankDetails.CreatedOn;
            IsDeleted = bankDetails.IsDeleted;
        }

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
