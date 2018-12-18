using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSlots.Infrastructure.Providers.Interfaces
{
    public interface IFileReader
    {
        string ReadAllFrom(string path);
    }
}
