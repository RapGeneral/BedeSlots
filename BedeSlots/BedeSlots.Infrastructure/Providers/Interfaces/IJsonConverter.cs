using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSlots.Infrastructure.Providers.Interfaces
{
    public interface IJsonConverter
    {
        string SerializeObject(object value, JsonSerializerSettings settings);
    }
}
