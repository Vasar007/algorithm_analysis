using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AlgorithmAnalysis.Common
{
    public static class ComObjectHelper
    {
        public static void ReleaseComObjectSafe(object obj)
        {
            try
            {
                Marshal.ReleaseComObject(obj);
            }
            catch (Exception ex)
            {
                string message = $"Unable to release the COM object:{Environment.NewLine}{ex}";

                Debug.WriteLine(message);
                Trace.WriteLine(message);
            }
        }
    }
}
