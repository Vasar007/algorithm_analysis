namespace AlgorithmAnalysis.Configuration
{
    public sealed class LoggerOptions : IOptions
    {
        public string RelativeLogFolderPath { get; set; } = "logs";


        public LoggerOptions()
        {
        }
    }
}
