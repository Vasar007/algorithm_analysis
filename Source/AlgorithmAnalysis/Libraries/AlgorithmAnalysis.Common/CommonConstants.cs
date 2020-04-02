using System.IO;

namespace AlgorithmAnalysis.Common
{
    public static class CommonConstants
    {
        public static string ApplicationName { get; } = "AlgorithmAnalysis";

        public static string TextLogTraceListenerName { get; } = "TextLogTraceListener";

        public static int EmptyCollectionCount { get; } = 0;

        public static int MinimumProcessorCount { get; } = 1;

        public static char SpecificStartingChar { get; } = '{';

        public static char SpecificEndingChar { get; } = '}';

        public static string CommonApplicationData { get; } = "SpecialFolder.CommonApplicationData";

        public static string DefaultLogFilenameExtensions { get; } = ".log";

        public static string DefaultLogFilenameSeparator { get; } = "-";

        public static string DefaultResultFilename { get; } = "results.xlsx";

        public static string DefaultWorkingPath { get; } =
            Utils.CreateSpecificPath(CommonApplicationData, appendAdppFolder: true);

        public static string DefaultLogFolderPath { get; } =
            Path.Combine(DefaultWorkingPath, "logs");

        public static string DefaultResultFolderPath { get; } =
            Path.Combine(DefaultWorkingPath, "results");

        public static string DefaultDataFolderPath { get; } =
            Path.Combine(DefaultWorkingPath, "data");
    }
}
