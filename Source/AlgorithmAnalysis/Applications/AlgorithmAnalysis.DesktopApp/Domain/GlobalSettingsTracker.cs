namespace AlgorithmAnalysis.DesktopApp.Domain
{
    internal static class GlobalSettingsTracker
    {
        private static readonly object _syncRoot = new object();

        private static bool _hasSettingsChanged = false;
        public static bool HasSettingsChanged
        {
            get
            {
                lock (_syncRoot)
                {
                    return _hasSettingsChanged;
                }
            }

            set
            {
                lock (_syncRoot)
                {
                    _hasSettingsChanged = value;
                }
            }
        }
    }
}
