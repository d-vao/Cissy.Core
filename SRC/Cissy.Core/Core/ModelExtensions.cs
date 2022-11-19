using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Cissy.Serialization.Json;

namespace Cissy
{
    public static class ModelExtensions
    {
        public static string ModelToJson(this IModel model, bool CamelCase = false)
        {
            if (CamelCase)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                settings.Formatting = Formatting.Indented;
                return JsonConvert.SerializeObject(model, settings);
            }
            return JsonConvert.SerializeObject(model);
        }
        public static T JsonToModel<T>(this string Json, bool CamelCase = false) where T : IModel
        {
            if (JsonIsEmpty(Json))
                return default;
            if (CamelCase)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                settings.Formatting = Formatting.Indented;
                return JsonConvert.DeserializeObject<T>(Json, settings);
            }
            return JsonConvert.DeserializeObject<T>(Json);
        }
        public static object JsonToModel(this string Json, Type type, bool CamelCase = false)
        {
            if (CamelCase)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                settings.Formatting = Formatting.Indented;
                return JsonConvert.DeserializeObject(Json, type, settings);
            }
            return JsonConvert.DeserializeObject(Json, type);
        }
        public static bool JsonIsEmpty(string json)
        {
            return json.IsNullOrEmpty() || json.Trim().Replace(" ", String.Empty) == "{}";
        }
    }
}
