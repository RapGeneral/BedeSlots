using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSlots.DataModels
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid TypeId { get; set; }

        public Guid BalanceId { get; set; }
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }        

        public decimal OpeningBalance { get; set; }

        public Balance Balance { get; set; }

        public TransactionType Type { get; set; }
    }
}
