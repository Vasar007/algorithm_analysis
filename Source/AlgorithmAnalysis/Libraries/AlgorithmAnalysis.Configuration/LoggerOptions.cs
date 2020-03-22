namespace AlgorithmAnalysis.Configuration
{
    public sealed class LoggerOptions : IOptions
    {
        public string LogFolderPath { get; set; } = "logs";

        public bool EnableLogForExcelLibrary { get; set; } = true;

        public string LogFilesExtension { get; set; } = ".log";

        public string LogNameSeparator { get; set; } = "-";


        public LoggerOptions()
        {
        }
    }
}
