using System.IO;
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
            string originalPathUnified = PathHelper.UnifyDirectorySeparatorChars(originalPath);
            string originalRelativePath = TransformPathToRelative(originalPath);

            string remainingOriginalPath = originalPathUnified.Replace(
                originalRelativePath, string.Empty
            );

            string finalNewPath = Path.Combine(remainingOriginalPath, newPath);

            return finalNewPath;
        }
    }
}
