using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class SettingsAnalysisModel : BindableBase, IChangeableModel
    {
        public SettingsAnalysisModel()
        {
            Reset();
            Validate();
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
