using BedeSlots.DataModels;
using BedeSlots.ViewModels.Enums;
using System;

namespace BedeSlots.ViewModels
{
    public class TransactionViewModel
    {
        public DateTime Date { get; set; }

        public TypeOfTransaction Type { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public string Username { get; set; }
    }
}