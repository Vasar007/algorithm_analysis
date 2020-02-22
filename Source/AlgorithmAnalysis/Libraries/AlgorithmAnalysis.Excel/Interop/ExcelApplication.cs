using AlgorithmAnalysis.Common;
using Microsoft.Office.Interop.Excel;

namespace AlgorithmAnalysis.Excel.Interop
{
    internal static class ExcelApplication
    {
        private static readonly object SyncRoot = new object();

        private static Application? _application;
        public static Application Instance
        {
            get
            {
                if (_application is null)
                {
                    lock (SyncRoot)
                    {
                        if (_application is null)
                        {
                            _application = new Application();
                        }
                    }
                }

                return _application;
            }
        }

        public static void Dispose()
        {
            if (_application is null)
                return;

            ComObjectHelper.ReleaseComObjectSafe(_application.WorksheetFunction);
            _application.Quit();
            ComObjectHelper.ReleaseComObjectSafe(_application);
            _application = null;
        }
    }
}
