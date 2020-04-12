using System.IO;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Files
{
    public static class PathHelper
    {
        // ResolvePath and CreateSpecificPath is paired methods: result one of them can be
        // converted back with another.

        public static string ResolvePath(string unresolvedPath, IPathResolver pathResolver)
        {
            pathResolver.ThrowIfNull(nameof(pathResolver));

            return pathResolver.Resolve(unresolvedPath);
        }

        public static string ResolvePath(string unresolvedPath)
        {
            var pathResolver = new SpecificPathResolver();
            return ResolvePath(unresolvedPath, pathResolver);
        }

        public static string CreateSpecificPath(string specificValue, string? path,
            bool appendAdppFolder, IPathCreator pathCreator)
        {
            pathCreator.ThrowIfNull(nameof(pathCreator));

            return pathCreator.CreateSpecificPath(specificValue, path, appendAdppFolder);
        }

        public static string CreateSpecificPath(string specificValue, string? path,
            bool appendAdppFolder)
        {
            var pathCreator = SpecificPathCreator.CreateDefault();
            return CreateSpecificPath(specificValue, path, appendAdppFolder, pathCreator);
        }

        public static string CreateSpecificPath(string specificValue, bool appendAdppFolder,
            IPathCreator pathCreator)
        {
            pathCreator.ThrowIfNull(nameof(pathCreator));

            return pathCreator.CreateSpecificPath(specificValue, appendAdppFolder);
        }

        public static string CreateSpecificPath(string specificValue, bool appendAdppFolder)
        {
            var pathCreator = SpecificPathCreator.CreateDefault();
            return CreateSpecificPath(specificValue, appendAdppFolder, pathCreator);
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
