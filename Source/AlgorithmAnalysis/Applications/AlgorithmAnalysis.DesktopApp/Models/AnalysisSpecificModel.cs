using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class AnalysisSpecificModel : BindableBase, IResetable
    {
        private bool _openAnalysisResults;
        public bool OpenAnalysisResults
        {
            get => _openAnalysisResults;
            set => SetProperty(ref _openAnalysisResults, value);
        }


        public AnalysisSpecificModel()
        {
            Reset();
        }

        #region IResetable Implementation

        public void Reset()
        {
            OpenAnalysisResults = false;
        }

        #endregion
    }
}
