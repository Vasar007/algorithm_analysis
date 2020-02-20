using System;
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
            _application?.Quit();
            _application = null;

            GC.Collect();
        }
    }
}
