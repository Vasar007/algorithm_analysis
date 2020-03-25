using AlgorithmAnalysis.Common;

namespace AlgorithmAnalysis.Configuration
{
    public sealed class LoggerOptions : IOptions
    {
        public string LogFolderPath { get; set; } = CommonConstants.DefaultLogFolderPath;

        public bool EnableLogForExcelLibrary { get; set; } = false;

        public string LogFilesExtension { get; set; } = ".log";

        public string LogNameSeparator { get; set; } = "-";


        public LoggerOptions()
        {
        }
    }
}
