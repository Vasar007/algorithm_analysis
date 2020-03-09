using System.IO;
using System.Threading.Tasks;

namespace AlgorithmAnalysis.Common.Processes
{
    public static class ProcessManager
    {
        public async static Task OpenFileWithAssociatedAppAsync(FileInfo file)
        {
            var launchContext = ProcessLaunchContext.Create(
                file: file,
                args: null,
                showWindow: true,
                useShellExecute: true
            );

            using var analysisRunner = ProgramRunner.RunProgram(launchContext);
            await analysisRunner.WaitAsync();
        }
    }
}
