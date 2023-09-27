
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Tiktok.API.Domain.Services;

namespace Tiktok.API.Infrastructure.Services;

public class SerializeService : ISerializeService
{
    public string Serialize<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            }
        });
    }

    public string Serialize<T>(T obj, Type type)
    {
        return JsonConvert.SerializeObject(obj, type, new JsonSerializerSettings());
    }

    public T? Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}