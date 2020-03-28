using System.Windows.Controls;

namespace AlgorithmAnalysis.DesktopApp.Views
{
    /// <summary>
    /// Interaction logic for AnalysisParametersView.xaml
    /// </summary>
    public sealed partial class AnalysisParametersView : UserControl
    {
        #region Proxy Properties

        public ComboBox AnalysisPhaseOnePartOneComboBoxProxy => AnalysisPhaseOnePartOneComboBox;

        public ComboBox AnalysisPhaseOnePartTwoComboBoxProxy => AnalysisPhaseOnePartTwoComboBox;

        public ComboBox AnalysisPhaseTwoComboBoxProxy => AnalysisPhaseTwoComboBox;

        public ComboBox GoodnessOfFitComboBoxProxy => GoodnessOfFitComboBox;

        #endregion

        public AnalysisParametersView()
        {
            InitializeComponent();
        }
    }
}
