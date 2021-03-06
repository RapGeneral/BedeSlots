﻿using System;
using System.Collections.Generic;

namespace BedeSlots.DataModels
{
	public class Balance
	{
		public Guid Id { get; set; }

		public string UserId { get; set; }

		public Guid CurrencyId { get; set; }
		public Guid TypeID { get; set; }

        public decimal Money { get; set; }

        public User User { get; set; }

        public Currency Currency { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public BalanceType Type { get; set; }
    }
}