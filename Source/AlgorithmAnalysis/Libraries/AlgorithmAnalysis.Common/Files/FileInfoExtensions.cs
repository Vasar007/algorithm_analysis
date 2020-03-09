using System;
using System.Diagnostics;
using System.IO;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Files
{
    public static class FileInfoExtensions
    {
        public static bool IsFileLocked(this FileInfo file)
        {
            file.ThrowIfNull(nameof(file));

            try
            {
                using FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                stream.Close();
            }
            catch (IOException ex)
            {
                // The file is unavailable because it is:
                // - still being written to;
                // - being processed by another thread;
                // - does not exist (has already been processed).

                string message = $"File '{file}' is unavailable:{Environment.NewLine}{ex}";

                Debug.WriteLine(message);
                Trace.WriteLine(message);

                return true;
            }

            // File is not locked.
            return false;
        }
    }
}
