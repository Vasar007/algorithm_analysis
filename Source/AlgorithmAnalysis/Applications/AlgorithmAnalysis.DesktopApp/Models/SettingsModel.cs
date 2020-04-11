using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class SettingsModel : BindableBase, IChangeableModel
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

            Reset();
            Validate();
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
    }
}
