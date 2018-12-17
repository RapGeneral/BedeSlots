using BedeSlots.Infrastructure.Providers.Interfaces;
using Newtonsoft.Json;

namespace BedeSlots.Infrastructure.Providers
{
    public class JsonConvertor : IJsonConverter
    {
        public string SerializeObject(object value, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(value, settings);
        }
    }
}
