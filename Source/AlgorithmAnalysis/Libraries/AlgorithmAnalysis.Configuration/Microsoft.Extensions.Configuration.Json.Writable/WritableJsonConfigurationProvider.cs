using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AlgorithmAnalysis.Common.Json;

namespace Microsoft.Extensions.Configuration.Json.Writable
{
    public sealed class WritableJsonConfigurationProvider : JsonConfigurationProvider
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public WritableJsonConfigurationProvider(JsonConfigurationSource source)
            : base(source)
        {
            _jsonSerializerSettings = JsonHelper.DefaultSerializerSettings;
        }

        public override void Set(string key, string value)
        {
            base.Set(key, value);

            // Get whole JSON file and change only passed key with passed value.

            string fileFullPath = Source.FileProvider.GetFileInfo(Source.Path).PhysicalPath;
            string json = File.ReadAllText(fileFullPath);

            if (!(JsonConvert.DeserializeObject(json, _jsonSerializerSettings) is JObject jObject))
            {
                throw new InvalidOperationException("Failed to deserialize config file from JSON.");
            }

            jObject[key] = JToken.Parse(value);
            string output = JsonConvert.SerializeObject(jObject, _jsonSerializerSettings);

            File.WriteAllText(fileFullPath, output);
        }
    }
}
