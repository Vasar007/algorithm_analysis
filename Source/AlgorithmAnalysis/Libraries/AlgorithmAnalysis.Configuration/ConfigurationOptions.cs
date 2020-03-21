using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;

namespace AlgorithmAnalysis.Configuration
{
    public static partial class ConfigOptions
    {
        private static readonly Lazy<IConfigurationRoot> Root =
            new Lazy<IConfigurationRoot>(LoadOptions);

        public static string ConfigFilename { get; } = "config.json";

        public static AnalysisOptions Analysis => GetOptions<AnalysisOptions>();

        public static ExcelOptions Excel => GetOptions<ExcelOptions>();

        public static LoggerOptions Logger => GetOptions<LoggerOptions>();


        public static T GetOptions<T>()
            where T : IOptions, new()
        {
            T section = Root.Value.GetSection(typeof(T).Name).Get<T>();

            if (section == null) return new T();

            return section;
        }

        private static IConfigurationRoot LoadOptions()
        {
            var configurationBuilder = new ConfigurationBuilder();

            string configPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? Path.Combine(Directory.GetCurrentDirectory(), ConfigFilename)
                : $"/etc/algorithm_analysis/{ConfigFilename}";

            configurationBuilder.AddJsonFile(configPath, optional: false, reloadOnChange: true);
            return configurationBuilder.Build();
        }
    }
}
