using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Areas.Identity.Models.AccountViewModels.Validation
{
    public class MinimumAgeAttribute : ValidationAttribute
    {
        int minimumAge;

        public MinimumAgeAttribute(int minValue)
        {
            minimumAge = minValue;
        }

        public override bool IsValid(object value)
        {
            DateTime date;
            if (DateTime.TryParse(value.ToString(), out date))
            {
                return date.AddYears(minimumAge) < DateTime.Now;
            }

            return false;
        }
    }
}