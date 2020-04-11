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

        // ResolvePath and CreateSpecificPath is paired methods: result one of them can be
        // converted back with another.

        public static string ResolvePath(string unresolvedPath)
        {
            unresolvedPath.ThrowIfNullOrWhiteSpace(nameof(unresolvedPath));

            var pathResolver = new SpecificPathResolver();
            return pathResolver.Resolve(unresolvedPath);
        }

        public static string CreateSpecificPath(string specificValue, string? path,
            bool appendAdppFolder)
        {
            specificValue.ThrowIfNullOrWhiteSpace(nameof(specificValue));

            var pathCreator = SpecificPathCreator.CreateDefault();
            return pathCreator.CreateSpecificPath(specificValue, path, appendAdppFolder);
        }

        public static string CreateSpecificPath(string specificValue, bool appendAdppFolder)
        {
            return CreateSpecificPath(specificValue, path: null, appendAdppFolder);
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

        public static string GetOrCreateFolderUsingFilePath(string filePath,
            string defaultDirectory)
        {
            filePath.ThrowIfNullOrWhiteSpace(nameof(filePath));
            defaultDirectory.ThrowIfNullOrWhiteSpace(nameof(defaultDirectory));

            string dataDirectory = Path.GetDirectoryName(filePath);
            if (string.IsNullOrWhiteSpace(dataDirectory))
            {
                dataDirectory = defaultDirectory;
            }

            return GetOrCreateFolder(dataDirectory);
        }

        public static string GetOrCreateFolderAndAppendFilePathToResult(string filePath,
            string defaultDirectory)
        {
            filePath.ThrowIfNullOrWhiteSpace(nameof(filePath));
            defaultDirectory.ThrowIfNullOrWhiteSpace(nameof(defaultDirectory));

            string folderPath = GetOrCreateFolderUsingFilePath(filePath, defaultDirectory);

            string filename = Path.GetFileName(filePath);
            return Path.Combine(folderPath, filename);
        }

        public static string UnifyDirectorySeparatorChars(string path)
        {
            path.ThrowIfNullOrWhiteSpace(nameof(path));

            return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }
    }
}
