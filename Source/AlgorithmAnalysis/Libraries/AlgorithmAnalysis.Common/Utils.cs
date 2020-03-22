using System;

namespace AlgorithmAnalysis.Common
{
    public static class Utils
    {
        public static string GetLocalShortDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}
