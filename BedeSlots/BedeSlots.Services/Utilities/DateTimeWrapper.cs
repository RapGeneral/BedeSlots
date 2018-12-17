using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSlots.Services.Utilities
{
    public class DateTimeWrapper : IDateTimeWrapper
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}
