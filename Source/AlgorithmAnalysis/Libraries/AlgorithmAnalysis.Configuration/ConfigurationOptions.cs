using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using AlgorithmAnalysis.Common;

namespace AlgorithmAnalysis.Configuration
{
    public static class ConfigOptions
    {
        private static readonly Lazy<IConfigurationRoot> Root =
            new Lazy<IConfigurationRoot>(LoadOptions);

        public static string DefaultOptionsPath => PredefinedPaths.DefaultOptionsPath;

        public static string AlternativeOptionsPath => PredefinedPaths.AlternativeOptionsPath;

        #region Options

        public static AppearanceOptions Appearence => GetOptions<AppearanceOptions>();

        public static AnalysisOptions Analysis => GetOptions<AnalysisOptions>();

        public static ReportOptions Report => GetOptions<ReportOptions>();

        public static LoggerOptions Logger => GetOptions<LoggerOptions>();

        #endregion


        [return: MaybeNull]
        public static TOptions FindOptions<TOptions>()
            where TOptions : class, IOptions, new()
        {
            return Root.Value.GetSection(typeof(TOptions).Name).Get<TOptions>();
        }

        [return: NotNull]
        public static TOptions GetOptions<TOptions>()
            where TOptions : class, IOptions, new()
        {
            TOptions? section = FindOptions<TOptions>();

            if (section is null) return new TOptions();

            return section;
        }

        public static void SetOptions<TOptions>([AllowNull] TOptions options)
            where TOptions : class, IOptions, new()
        {
            if (options is null) return;

            Root.Value.GetSection(typeof(TOptions).Name).Bind(options);
        }

        private static IConfigurationRoot LoadOptions()
        {
            var configurationBuilder = new ConfigurationBuilder();

            string configPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? DefaultOptionsPath
                : AlternativeOptionsPath;

            configurationBuilder.AddWritableJsonFile(
                path: configPath,
                optional: true,
                reloadOnChange: true
            );

            return configurationBuilder.Build();
        }
    }
}
