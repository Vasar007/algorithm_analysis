using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AlgorithmAnalysis.DesktopApp.Domain
{
    public static class TaskExtension
    {
        public static async void FireAndForgetSafeAsync(this Task task,
            IErrorHandler? handler = null)
        {
            handler ??= new CommonErrorHandler();

            try
            {
                await task;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occured during async execution: {ex}.");
                handler.HandleError(ex);
            }
        }
    }
}
