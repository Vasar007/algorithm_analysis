using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Configuration
{
    public sealed class AppearanceOptions : IOptions
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ThemeKind Theme { get; set; } = ThemeKind.Light;


        public AppearanceOptions()
        {
        }
    }
}
