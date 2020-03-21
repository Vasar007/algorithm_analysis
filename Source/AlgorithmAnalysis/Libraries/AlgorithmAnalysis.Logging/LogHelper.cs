using Acolyte.Assertions;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Configuration;

namespace AlgorithmAnalysis.Logging
{
    public static class LogHelper
    {
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
