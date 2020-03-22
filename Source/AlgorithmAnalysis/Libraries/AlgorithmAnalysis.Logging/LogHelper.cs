using System.IO;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Configuration;

namespace AlgorithmAnalysis.Logging
{
    public static class LogHelper
    {
        public static string GetLogFolderPath(LoggerOptions loggerOptions)
        {
            loggerOptions.ThrowIfNull(nameof(loggerOptions));

            var parser = new LogFolderPathParser();
            return parser.Parse(loggerOptions);
        }

        public static string GetOrCreateLogFolder(LoggerOptions loggerOptions)
        {
            loggerOptions.ThrowIfNull(nameof(loggerOptions));

            string logFolderPath = GetLogFolderPath(loggerOptions);

            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
            }

            return logFolderPath;
        }

        public static string GetLogFilePath(LoggerOptions loggerOptions, string logFilename)
        {
            loggerOptions.ThrowIfNull(nameof(loggerOptions));
            logFilename.ThrowIfNullOrWhiteSpace(nameof(logFilename));

            string logFolderPath = GetOrCreateLogFolder(loggerOptions);
            return Path.Combine(logFolderPath, logFilename);
        }

        public static string CreateLogFilename(string logName, LoggerOptions loggerOptions)
        {
            logName.ThrowIfNull(nameof(logName));
            loggerOptions.ThrowIfNull(nameof(loggerOptions));

            string fullLogFilename =
                $"{logName}{loggerOptions.LogNameSeparator}{Utils.GetLocalShortDate()}";

            return $"{fullLogFilename}{loggerOptions.LogFilesExtension}";
        }

        public static string CreateLogFilename(string logName)
        {
            return CreateLogFilename(logName, ConfigOptions.Logger);
        }
    }
}
