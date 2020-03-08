using System;
using System.IO;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common
{
    public static class Utils
    {
        public static string GetLocalShortDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static string GetLogFilePath(string logFolderPath, string logFilename)
        {
            logFolderPath.ThrowIfNullOrWhiteSpace(nameof(logFolderPath));
            logFilename.ThrowIfNullOrWhiteSpace(nameof(logFilename));

            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
            }

            return Path.Combine(logFolderPath, logFilename);
        }
    }
}
