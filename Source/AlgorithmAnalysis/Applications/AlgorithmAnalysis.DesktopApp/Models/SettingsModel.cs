using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class SettingsModel : BindableBase, IChangeable, ISaveable
    {
        public SettingsAppearenceModel Appearence { get; }

        public SettingsAnalysisModel Analysis { get; }

        public SettingsReportModel Report { get; }

        public SettingsLoggerModel Logger { get; }


        public SettingsModel()
        {
            Appearence = new SettingsAppearenceModel();
            Analysis = new SettingsAnalysisModel();
            Report = new SettingsReportModel();
            Logger = new SettingsLoggerModel();

            // Internal model should call Reset method in ctors themself.
        }

        #region IChangeableModel Implementation

        public void Reset()
        {
            Appearence.Reset();
            Analysis.Reset();
            Report.Reset();
            Logger.Reset();
        }

        public void Validate()
        {
            Appearence.Validate();
            Analysis.Validate();
            Report.Validate();
            Logger.Validate();
        }

        #endregion

        #region ISaveable Implementation

        public void SaveToConfigFile()
        {
            // Internal models should call Validate method themself
            // because SaveToConfigFile can be called for separate model too.

            Appearence.SaveToConfigFile();
            Analysis.SaveToConfigFile();
            Report.SaveToConfigFile();
            Logger.SaveToConfigFile();
        }

        #endregion
    }
}
