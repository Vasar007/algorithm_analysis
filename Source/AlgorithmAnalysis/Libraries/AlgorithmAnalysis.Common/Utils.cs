using System;
using System.IO;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common.Files;

namespace AlgorithmAnalysis.Common
{
    public static class Utils
    {
        public static string GetLocalShortDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static string ResolvePath(string unresolvedPath)
        {
            unresolvedPath.ThrowIfNullOrWhiteSpace(nameof(unresolvedPath));

            var pathResolver = new FolderPathResolver();
            return pathResolver.Resolve(unresolvedPath);
        }

        public static string GetOrCreateFolder(string potentialFolderPath)
        {
            potentialFolderPath.ThrowIfNullOrWhiteSpace(nameof(potentialFolderPath));

            string folderPath = ResolvePath(potentialFolderPath);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }
    }
}
