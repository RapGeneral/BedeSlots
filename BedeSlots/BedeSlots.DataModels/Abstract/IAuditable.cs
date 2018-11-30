using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSlots.DataModels.Abstract
{
    public interface IAuditable
    {
        DateTime CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }
    }
}
