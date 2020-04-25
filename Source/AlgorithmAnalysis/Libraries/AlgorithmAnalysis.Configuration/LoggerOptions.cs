using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Common.Files;

namespace AlgorithmAnalysis.Configuration
{
    public sealed class LoggerOptions : IOptions
    {
        // TODO: hide folder path from external usages.
        public string LogFolderPath { get; set; } = PredefinedPaths.DefaultLogFolderPath;

        public bool EnableLogForExcelLibrary { get; set; } = false;

        public string LogFilesExtension { get; set; } = CommonConstants.DefaultLogFilenameExtensions;

        public string LogFilenameSeparator { get; set; } = CommonConstants.DefaultLogFilenameSeparator;

        public bool UseFullyQualifiedEntityNames { get; set; } = false;


        public LoggerOptions()
        {
        }
    }
}
