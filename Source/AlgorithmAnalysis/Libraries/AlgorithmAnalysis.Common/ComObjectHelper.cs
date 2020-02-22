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
                Debug.WriteLine($"Unable to release the COM object: {ex}");
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
