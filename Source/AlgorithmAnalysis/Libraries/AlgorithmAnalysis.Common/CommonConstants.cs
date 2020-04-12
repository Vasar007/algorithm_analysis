using System.IO;

namespace AlgorithmAnalysis.Common
{
    public static class CommonConstants
    {
        public static string ApplicationName { get; } = "AlgorithmAnalysis";

        public static string TextLogTraceListenerName { get; } = "TextLogTraceListener";

        public static string ConfigFilename { get; } = "config.json";

        public static int EmptyCollectionCount { get; } = 0;

        public static int MinimumProcessorCount { get; } = 1;

        public static char SpecificStartingChar { get; } = '{';

        public static char SpecificEndingChar { get; } = '}';

        public static string FormulaSymbol { get; } = "x";

        public static string StringFormatZero { get; } = "{0}";

        public static string CommonApplicationData { get; } = "SpecialFolder.CommonApplicationData";

        public static string DefaultLogFilenameExtensions { get; } = ".log";

        public static string DefaultLogFilenameSeparator { get; } = "-";

        public static string DefaultResultFilename { get; } = "results.xlsx";
    }
}
