using System;
using System.Diagnostics;

namespace AlgorithmAnalysis.DesktopApp.Domain
{
    public sealed class CommonErrorHandler : IErrorHandler
    {
        public CommonErrorHandler()
        {
        }

        #region IErrorHandler Implementation

        public void HandleError(Exception ex)
        {
            Debug.WriteLine($"Exception occured during task execution: {ex}");
        }

        #endregion
    }
}
