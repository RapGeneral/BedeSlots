using System;
using System.Collections.Generic;

namespace BedeSlots.DataModels
{
    public class Currency
    {
        public Guid Id { get; set; }

        public string CurrencyName { get; set; }

        public ICollection<Rate> Rates { get; set; }
    }
}
