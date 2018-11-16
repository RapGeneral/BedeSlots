using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSlots.DataModels
{
    public class BankDetails
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public int Cvv { get; set; }

        public DateTime ExpiryDate { get; set; }

        public ICollection<UserBankDetails> UserBankDetails { get; set; }
        
    }
}
