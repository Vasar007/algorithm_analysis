using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class SettingsReportModel : BindableBase, IChangeableModel
    {
        public SettingsReportModel()
        {
            Reset();
        }

        #region IChangeableModel Implementation

        public void Reset()
        {
        }

        public void Validate()
        {
        }

        #endregion
    }
}
