using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSlots.DataModels
{
    public class UserBankDetails
    {
        public string UserId { get; set; }

        public Guid BankDetailsId { get; set; }

        public User User { get; set; }

        public BankDetails BankDetails { get; set; }
    }

}
