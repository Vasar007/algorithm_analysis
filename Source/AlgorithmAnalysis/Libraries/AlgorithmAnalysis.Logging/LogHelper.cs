using System.IO;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Configuration;

namespace AlgorithmAnalysis.Logging
{
    public static class LogHelper
    {
        public static string ResolveLogFolderPath(LoggerOptions loggerOptions)
        {
            loggerOptions.ThrowIfNull(nameof(loggerOptions));

            return Utils.ResolvePath(loggerOptions.LogFolderPath);
        }

        public static string GetOrCreateLogFolder(LoggerOptions loggerOptions)
        {
            loggerOptions.ThrowIfNull(nameof(loggerOptions));

            return Utils.GetOrCreateFolder(loggerOptions.LogFolderPath);
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

        public static void SetTraceLogger(string logName, LoggerOptions loggerOptions)
        {
            string traceLogFilename = CreateLogFilename(logName, loggerOptions);
            string traceLogFilePath = GetLogFilePath(loggerOptions, traceLogFilename);

            TraceHelper.SetTraceListener(traceLogFilePath);
        }

        public static void SetTraceLogger(string logName)
        {
            SetTraceLogger(logName, ConfigOptions.Logger);
        }
    }
}
