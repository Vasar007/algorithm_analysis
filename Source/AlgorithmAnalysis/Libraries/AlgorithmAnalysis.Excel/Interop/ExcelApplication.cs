using Acolyte.Common;
using AlgorithmAnalysis.Common;
using Microsoft.Office.Interop.Excel;

namespace AlgorithmAnalysis.Excel.Interop
{
    internal static class ExcelApplication
    {
        private static readonly ResetableLazy<Application> _application =
            new ResetableLazy<Application>(() => new Application());

        public static Application Instance => _application.Value;

        public static void Dispose()
        {
            if (!_application.IsValueCreated)
                return;

            ComObjectHelper.ReleaseComObjectSafe(Instance.WorksheetFunction);
            Instance.Quit();
            ComObjectHelper.ReleaseComObjectSafe(Instance);
            _application.Reset();
        }
    }
}
