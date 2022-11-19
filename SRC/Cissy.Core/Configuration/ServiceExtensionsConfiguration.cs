using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Cissy.Configuration
{
    public static class ServiceExtensionsConfiguration
    {
        static Dictionary<Type, IConfigurationRoot> Configs = new Dictionary<Type, IConfigurationRoot>();
        public static IConfigurationRoot GetServiceConfiguration<T>(this T Contract) where T : IConfigurableContract
        {
            Type type = typeof(T);
            if (!Configs.TryGetValue(type, out IConfigurationRoot root))
            {
                var builder = new ConfigurationBuilder();
                string s = type.Assembly.Location;
                builder.AddJsonFile(s.Substring(0, s.Length - 4) + ".json");
                root = builder.Build();
                Configs[type] = root;
            }
            return root;
        }
    }
}
