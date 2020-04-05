using System;
using System.IO;
using System.Runtime.InteropServices;
using AlgorithmAnalysis.Common;
using Microsoft.Extensions.Configuration;

namespace AlgorithmAnalysis.Configuration
{
    public static class ConfigOptions
    {
        private static readonly Lazy<IConfigurationRoot> Root =
            new Lazy<IConfigurationRoot>(LoadOptions);

        public static string ConfigFilename => CommonConstants.ConfigFilename;

        public static string AlternativeOptionsPath => PredefinedPaths.AlternativeOptionsPath;

        public static AnalysisOptions Analysis => GetOptions<AnalysisOptions>();

        public static ReportOptions Report => GetOptions<ReportOptions>();

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
                : AlternativeOptionsPath;

            configurationBuilder.AddJsonFile(configPath, optional: true, reloadOnChange: true);
            return configurationBuilder.Build();
        }
    }
}
