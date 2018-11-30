using System;

namespace BedeSlots.DataModels
{
    public class Rate
    {
        public Guid Id { get; set; }

        public Guid BaseCurrencyId { get; set; }

        public Guid ToCurrencyId { get; set; }

        public decimal Coeff { get; set; }

        public DateTime CreatedAt { get; set; }

        public Currency BaseCurrency { get; set; }

        public Currency ToCurrency { get; set; }
    }
}
