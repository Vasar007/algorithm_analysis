using System;
using System.IO;
using System.Runtime.InteropServices;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Files
{
    public static class PathHelper
    {
        // ResolvePath method usage examples:
        // input: "{SpecialFolder.CommonApplicationData}/AlgorithmAnalysis/logs"
        // output: "C:\ProgramData\AlgorithmAnalysis\logs"

        // CreateSpecificPath method usage examples:
        // input: "SpecialFolder.CommonApplicationData", appendAppFolder: true, shouldResolve: true
        // output: "C:\ProgramData\AlgorithmAnalysis"
        // input: "SpecialFolder.CommonApplicationData", appendAppFolder: true, shouldResolve: false
        // output: "{SpecialFolder.CommonApplicationData}\AlgorithmAnalysis"

        public static string ResolvePath(string unresolvedPath, PathResolutionOptions? options,
            IPathResolver pathResolver)
        {
            pathResolver.ThrowIfNull(nameof(pathResolver));

            return pathResolver.Resolve(unresolvedPath, options);
        }

        public static string ResolvePath(string unresolvedPath, PathResolutionOptions? options)
        {
            var pathResolver = SpecificPathResolver.CreateDefault();

            return ResolvePath(unresolvedPath, options, pathResolver);
        }

        public static string ResolveFullPath(string unresolvedPath)
        {
            var options = new PathResolutionOptions
            {
                UnifyDirectorySeparatorChars = true,
                ReturnRelativePath = false
            };
            return ResolvePath(unresolvedPath, options);
        }

        public static string ResolveRelativePath(string unresolvedPath)
        {
            var options = new PathResolutionOptions
            {
                UnifyDirectorySeparatorChars = true,
                ReturnRelativePath = true
            };
            return ResolvePath(unresolvedPath, options);
        }

        public static string CreateSpecificPath(string specificValue, PathCreationOptions? options,
            IPathCreator pathCreator)
        {
            pathCreator.ThrowIfNull(nameof(pathCreator));

            return pathCreator.CreateSpecificPath(specificValue, options);
        }

        public static string CreateSpecificResolvedPath(string specificValue, bool appendAppFolder)
        {
            var pathCreator = SpecificPathCreator.CreateDefault();

            var options = new PathCreationOptions
            {
                AppendAppFolder = appendAppFolder,
                ShouldResolvePath = true
            };
            return CreateSpecificPath(specificValue, options, pathCreator);
        }

        public static string GetOrCreateFolder(string potentialFolderPath)
        {
            potentialFolderPath.ThrowIfNullOrWhiteSpace(nameof(potentialFolderPath));

            string folderPath = ResolveFullPath(potentialFolderPath);

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

        public static bool IsStartWithDirectorySeparatorChar(string path)
        {
            path.ThrowIfNullOrWhiteSpace(nameof(path));

            var separator = Path.DirectorySeparatorChar.ToString();
            var altSeparator = Path.AltDirectorySeparatorChar.ToString();

            return path.StartsWith(separator, StringComparison.OrdinalIgnoreCase) ||
                   path.StartsWith(altSeparator, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsEndWithDirectorySeparatorChar(string path)
        {
            path.ThrowIfNullOrWhiteSpace(nameof(path));

            var separator = Path.DirectorySeparatorChar.ToString();
            var altSeparator = Path.AltDirectorySeparatorChar.ToString();

            return path.EndsWith(separator, StringComparison.OrdinalIgnoreCase) ||
                   path.EndsWith(altSeparator, StringComparison.OrdinalIgnoreCase);
        }

        public static string TrimStartDirectorySeparatorChar(string path)
        {
            path.ThrowIfNullOrWhiteSpace(nameof(path));

            string trimmedPath = path;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                trimmedPath = IsStartWithDirectorySeparatorChar(path)
                    ? path.Substring(startIndex: 1)
                    : path;
            }

            return trimmedPath;
        }

        public static string TrimEndDirectorySeparatorChar(string path)
        {
            path.ThrowIfNullOrWhiteSpace(nameof(path));

            return IsEndWithDirectorySeparatorChar(path)
                ? Path.GetDirectoryName(path)
                : path;
        }

        public static string TrimDirectorySeparatorChar(string path)
        {
            path.ThrowIfNullOrWhiteSpace(nameof(path));

            string trimmedPath = TrimStartDirectorySeparatorChar(path);
            return TrimEndDirectorySeparatorChar(trimmedPath);
        }

        public static string GetLastPartOfPath(string path)
        {
            path.ThrowIfNullOrWhiteSpace(nameof(path));

            string directoryName = TrimEndDirectorySeparatorChar(path);

            return Path.GetFileName(directoryName);
        }
    }
}
