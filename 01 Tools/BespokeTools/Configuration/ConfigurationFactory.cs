using System;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace Protime.Bespoke.Tools.Configuration
{
    public static class ConfigurationFactory
    {
        public static T Create<T>(string configurationFile) where T : class
        {
            if (!File.Exists(configurationFile))
                throw new FileNotFoundException($"Configuration file '{configurationFile}' not found");

            var jsonConfiguration = File.ReadAllText(configurationFile);
            if (string.IsNullOrEmpty(jsonConfiguration))
                throw new ConfigurationErrorsException("No configuration found");

            try {
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
                return JsonConvert.DeserializeObject<T>(jsonConfiguration, settings); }
            catch (Exception e)
            {
                throw new ConfigurationErrorsException($"Incorrect configuration: {e.Message}");
            }
        }

        public static string SerializeObject<T>(T @object) where T : class
        {
            try
            {
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects};
                settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                return JsonConvert.SerializeObject(@object, settings);
            }
            catch (Exception e)
            {
                throw new ConfigurationErrorsException($"Cannot serialize object: {e.Message}");
            }
        }
    }
}
