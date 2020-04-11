using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class SettingsModel : BindableBase, IChangeableModel
    {
        public SettingsLoggerModel Logger { get; }


        public SettingsModel()
        {
            Logger = new SettingsLoggerModel();

            Reset();
        }

        #region IChangeableModel Implementation

        public void Reset()
        {
            Logger.Reset();
        }

        public void Validate()
        {
            Logger.Validate();
        }

        #endregion
    }
}
