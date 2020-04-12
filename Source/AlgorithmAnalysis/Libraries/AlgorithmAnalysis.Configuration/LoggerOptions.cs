using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Common.Files;

namespace AlgorithmAnalysis.Configuration
{
    public sealed class LoggerOptions : IOptions
    {
        public string LogFolderPath { get; set; } = PredefinedPaths.DefaultLogFolderPath;

        public bool EnableLogForExcelLibrary { get; set; } = false;

        public string LogFilesExtension { get; set; } = CommonConstants.DefaultLogFilenameExtensions;

        public string LogFilenameSeparator { get; set; } = CommonConstants.DefaultLogFilenameSeparator;


        public LoggerOptions()
        {
        }
    }
}
