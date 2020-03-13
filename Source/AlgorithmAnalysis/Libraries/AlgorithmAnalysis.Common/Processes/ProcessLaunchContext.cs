using System.Diagnostics;
using System.IO;
using System.Text;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Processes
{
    public sealed class ProcessLaunchContext
    {
        public FileInfo File { get; }

        public string? Args { get; }

        public bool ShowWindow { get; }

        public bool UseShellExecute { get; }


        private ProcessLaunchContext(
            FileInfo file,
            string? args,
            bool showWindow,
            bool useShellExecute)
        {
            File = file.ThrowIfNull(nameof(file));
            Args = args;
            ShowWindow = showWindow;
            UseShellExecute = useShellExecute;
        }

        public static ProcessLaunchContext Create(
            FileInfo file,
            string? args,
            bool showWindow,
            bool useShellExecute)
        {
            return new ProcessLaunchContext(
                file: file,
                args: args,
                showWindow: showWindow,
                useShellExecute: useShellExecute
            );
        }

        public static ProcessLaunchContext Create(
            FileInfo file,
            string? args,
            bool showWindow)
        {
            return new ProcessLaunchContext(
                file: file,
                args: args,
                showWindow: showWindow,
                useShellExecute: false
            );
        }

        public ProcessStartInfo CreateStartInfo()
        {
            bool redirectStandardError = !UseShellExecute;

            var starterInfo = new ProcessStartInfo(File.FullName, Args)
            {
                WorkingDirectory = File.Directory.FullName,
                RedirectStandardError = redirectStandardError,
                UseShellExecute = UseShellExecute
            };

            if (ShowWindow)
            {
                starterInfo.WindowStyle = ProcessWindowStyle.Normal;
                starterInfo.CreateNoWindow = false;
                return starterInfo;
            }

            starterInfo.WindowStyle = ProcessWindowStyle.Hidden;
            starterInfo.CreateNoWindow = true;
            return starterInfo;
        }

        public string ToLogString()
        {
            var sb = new StringBuilder()
                .AppendLine($"[{nameof(ProcessLaunchContext)}]")
                .AppendLine($"File: '{File}'")
                .AppendLine($"Args: {Args ?? "NULL"}")
                .AppendLine($"ShowWindow: '{ShowWindow.ToString()}'")
                .AppendLine($"UseShellExecute: '{UseShellExecute.ToString()}'");

            return sb.ToString();
        }
    }
}
