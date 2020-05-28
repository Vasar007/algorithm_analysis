using System.IO;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Common.Files;

namespace AlgorithmAnalysis.DesktopApp.Domain
{
    internal static class ModelPathTransformer
    {
        public static string TransformPathToRelative(string originalPath)
        {
            return PathHelper.ResolveRelativePath(originalPath);
        }

        public static string TransformPathToOriginal(string originalPath, string newPath)
        {
            // Unify directory separator chars at the beginning and at the end.
            string originalPathUnified =
                PathHelper.UnifyDirectorySeparatorCharsPlatformIndependent(originalPath);

            // Convert original path with specfic value to relative path.
            // input: "{SpecialFolder.CommonApplicationData}/AlgorithmAnalysis/results/results.xlsx"
            // output: "AlgorithmAnalysis/results/results.xlsx"
            string originalRelativePath = TransformPathToRelative(originalPath);

            // Determine commom path at the beginning.
            // At least this variable should contain specific value.
            // input: "{SpecialFolder.CommonApplicationData}/AlgorithmAnalysis/results/results.xlsx"
            // output: "{SpecialFolder.CommonApplicationData}/"
            string remainingOriginalPath = originalPathUnified.Replace(
                originalRelativePath, string.Empty
            );

            // Unify directory separator chars at the beginning and at the end.
            string newPathUnified =
                PathHelper.UnifyDirectorySeparatorCharsPlatformIndependent(newPath);

            // Create new final path
            // input1: "AlgorithmAnalysis/results.xlsx"
            // input2: "{SpecialFolder.CommonApplicationData}/"
            // output: "{SpecialFolder.CommonApplicationData}/AlgorithmAnalysis/results.xlsx"
            string finalNewPath = Path.Combine(remainingOriginalPath, newPathUnified);

            // Unify directory separator chars at the beginning and at the end.
            return PathHelper.UnifyDirectorySeparatorCharsPlatformIndependent(finalNewPath);
        }

        public static string TransformPathToOriginal(string newPath)
        {
            string newPathUnified =
                PathHelper.UnifyDirectorySeparatorCharsPlatformIndependent(newPath);

            var options = new PathCreationOptions
            {
                AppendAppFolder = false,
                ShouldResolvePath = false
            };
            string specificPath = PathHelper.CreateSpecificPath(
                CommonConstants.CommonApplicationData, options, SpecificPathCreator.CreateDefault()
            );

            string finalNewPath = Path.Combine(specificPath, newPathUnified);

            return PathHelper.UnifyDirectorySeparatorCharsPlatformIndependent(finalNewPath);
        }
    }
}
