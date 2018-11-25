using BedeSlots.DataModels.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSlots.DataModels
{
    public class BankDetails : IAuditable, IDeletable
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public int Cvv { get; set; }

        public DateTime ExpiryDate { get; set; }

        public ICollection<UserBankDetails> UserBankDetails { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
