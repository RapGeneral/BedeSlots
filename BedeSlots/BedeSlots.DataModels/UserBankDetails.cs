using BedeSlots.DataModels.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSlots.DataModels
{
    public class UserBankDetails : IDeletable
    {
        public string UserId { get; set; }

        public bool IsDeleted { get; set; }

        public Guid BankDetailsId { get; set; }

        public User User { get; set; }

        public BankDetails BankDetails { get; set; }

        public DateTime? DeletedOn { get; set; }
    }

}
