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
            string originalPathUnified =
                PathHelper.UnifyDirectorySeparatorCharsPlatformIndependent(originalPath);
            string originalRelativePath = TransformPathToRelative(originalPath);

            string remainingOriginalPath = originalPathUnified.Replace(
                originalRelativePath, string.Empty
            );

            string newPathUnified =
                PathHelper.UnifyDirectorySeparatorCharsPlatformIndependent(newPath);
            string finalNewPath = Path.Combine(remainingOriginalPath, newPathUnified);

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
