namespace AlgorithmAnalysis.Configuration
{
    public sealed class LoggerOptions : IOptions
    {
        public string RelativeLogFolderPath { get; set; } = "logs";

        public bool EnableLogForExcelLibrary { get; set; } = true;


        public LoggerOptions()
        {
        }
    }
}
