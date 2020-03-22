using System;
using System.Linq;
using Acolyte.Assertions;
using AlgorithmAnalysis.Configuration;

namespace AlgorithmAnalysis.Logging
{
    public sealed class LogFolderPathParser
    {
        public LogFolderPathParser()
        {
        }

        public string Parse(LoggerOptions loggerOptions)
        {
            loggerOptions.ThrowIfNull(nameof(loggerOptions));

            string logFolderPath = loggerOptions.LogFolderPath;
            logFolderPath.ThrowIfNullOrWhiteSpace(nameof(logFolderPath));

            if (ShouldBeParsed(logFolderPath))
            {
                logFolderPath = ParseSpecialFolder(logFolderPath);
            }

            return logFolderPath;
        }

        private static bool ShouldBeParsed(string logFolderPath)
        {
            int startingCharsNumber = logFolderPath.Count(
                ch => ch.Equals(LoggingConstants.SpecialStartingChar)
            );
            int endingCharsNumber = logFolderPath.Count(
                ch => ch.Equals(LoggingConstants.SpecialEndingChar)
            );

            if (startingCharsNumber == 0 && endingCharsNumber == 0) return false;

            // Now supported only one special expression.
            if (startingCharsNumber == 1 && endingCharsNumber == 1) return true;

            string message = "Failed to parse log folder path: invalid path format.";
            throw new ArgumentException(message, nameof(logFolderPath));
        }

        public static string ParseSpecialFolder(string logFolderPath)
        {
            if (logFolderPath.Contains(LoggingConstants.CommonApplicationData))
            {
                string valueToReplace =
                    LoggingConstants.SpecialStartingChar +
                    LoggingConstants.CommonApplicationData +
                    LoggingConstants.SpecialEndingChar;

                string newValue = Environment.GetFolderPath(
                    Environment.SpecialFolder.CommonApplicationData
                );

                return logFolderPath.Replace(valueToReplace, newValue);
            }

            string message = "Failed to parse log folder path: invalid special folder value.";
            throw new ArgumentException(message, nameof(logFolderPath));
        }
    }
}
