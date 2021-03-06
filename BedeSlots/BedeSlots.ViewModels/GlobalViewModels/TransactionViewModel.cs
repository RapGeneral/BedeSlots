﻿using BedeSlots.DataModels;
using BedeSlots.GlobalData.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace BedeSlots.GlobalData.GlobalViewModels
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