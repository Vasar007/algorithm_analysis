using System.IO;

namespace AlgorithmAnalysis.Common
{
    public static class PredefinedPaths
    {
        public static string DefaultOptionsPath { get; } =
            Path.Combine(Directory.GetCurrentDirectory(), CommonConstants.ConfigFilename);

        public static string AlternativeOptionsPath { get; } =
            Path.Combine("/etc", "algorithm_analysis", CommonConstants.ConfigFilename);

        public static string DefaultWorkingPath { get; } =
            Utils.CreateSpecificPath(CommonConstants.CommonApplicationData, appendAdppFolder: true);

        public static string DefaultLogFolderPath { get; } =
            Path.Combine(DefaultWorkingPath, "logs");

        public static string DefaultResultFolderPath { get; } =
            Path.Combine(DefaultWorkingPath, "results");

        public static string DefaultDataFolderPath { get; } =
            Path.Combine(DefaultWorkingPath, "data");
    }
}
