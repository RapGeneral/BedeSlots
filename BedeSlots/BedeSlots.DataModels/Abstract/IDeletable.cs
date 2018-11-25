using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSlots.DataModels.Abstract
{
    public interface IDeletable
    {
        DateTime? DeletedOn { get; set; }

        bool IsDeleted { get; set; }
    }
}
