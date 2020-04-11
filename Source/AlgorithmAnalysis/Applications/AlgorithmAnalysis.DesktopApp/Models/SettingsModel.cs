using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class SettingsModel : BindableBase, IResetable
    {
        public SettingsLoggerModel Logger { get; }


        public SettingsModel()
        {
            Logger = new SettingsLoggerModel();

            Reset();
        }

        #region IResetable Implementation

        public void Reset()
        {
            Logger.Reset();
        }

        #endregion
    }
}
