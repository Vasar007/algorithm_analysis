using System.IO;

namespace AlgorithmAnalysis.Common.Files
{
    public static class PredefinedPaths
    {
        public static string DefaultOptionsPath { get; } =
            Path.Combine(Directory.GetCurrentDirectory(), CommonConstants.ConfigFilename);

        public static string AlternativeOptionsPath { get; } =
            Path.Combine("/etc", "algorithm_analysis", CommonConstants.ConfigFilename);

        public static string DefaultWorkingPath { get; } =
            PathHelper.CreateSpecificResolvedPath(
                CommonConstants.CommonApplicationData, appendAppFolder: true
            );

        public static string DefaultLogFolderPath { get; } =
            Path.Combine(DefaultWorkingPath, "logs");

        public static string DefaultResultFolderPath { get; } =
            Path.Combine(DefaultWorkingPath, "results");

        public static string DefaultDataFolderPath { get; } =
            Path.Combine(DefaultWorkingPath, "data");
    }
}
