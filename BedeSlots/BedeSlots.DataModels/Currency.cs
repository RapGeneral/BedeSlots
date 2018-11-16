using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSlots.DataModels
{
    public  class Currency
    {
        public Guid Id { get; set; }

        public string CurrencyName { get; set; }

        public ICollection<Rate> Rates { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
