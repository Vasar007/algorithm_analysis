using System;

namespace AlgorithmAnalysis.DesktopApp.Domain
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
