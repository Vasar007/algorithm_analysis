using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class ParametersAdvancedModel : BindableBase, IChangeable
    {
        private bool _showAnalysisWindow;
        public bool ShowAnalysisWindow
        {
            get => _showAnalysisWindow;
            set => SetProperty(ref _showAnalysisWindow, value);
        }

        private bool _openAnalysisResults;
        public bool OpenAnalysisResults
        {
            get => _openAnalysisResults;
            set => SetProperty(ref _openAnalysisResults, value);
        }


        public ParametersAdvancedModel()
        {
            Reset();
        }

        #region IChangeableModel Implementation

        public void Reset()
        {
            ShowAnalysisWindow = false;
            OpenAnalysisResults = false;
        }

        public void Validate()
        {
            // Nothing to validate.
        }

        #endregion
    }
}
