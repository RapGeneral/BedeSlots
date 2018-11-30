using BedeSlots.DataModels.Abstract;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BedeSlots.DataModels
{
    public class User : IdentityUser, IAuditable
    {
        public Guid UserCurrencyId { get; set; }

        public DateTime DateOfBirth { get; set; }

        public ICollection<UserBankDetails> UserBankDetails { get; set; }

        public ICollection<Balance> Balances { get; set; }
        
        public Currency Currency { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
