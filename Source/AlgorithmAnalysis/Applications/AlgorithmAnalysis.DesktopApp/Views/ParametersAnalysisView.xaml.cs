using System.Windows.Controls;

namespace AlgorithmAnalysis.DesktopApp.Views
{
    /// <summary>
    /// Interaction logic for ParametersAnalysisView.xaml
    /// </summary>
    public sealed partial class ParametersAnalysisView : UserControl
    {
        #region Proxy Properties

        public ComboBox AnalysisPhaseOnePartOneComboBoxProxy => AnalysisPhaseOnePartOneComboBox;

        public ComboBox AnalysisPhaseOnePartTwoComboBoxProxy => AnalysisPhaseOnePartTwoComboBox;

        public ComboBox AnalysisPhaseTwoComboBoxProxy => AnalysisPhaseTwoComboBox;

        public ComboBox GoodnessOfFitComboBoxProxy => GoodnessOfFitComboBox;

        #endregion

        public ParametersAnalysisView()
        {
            InitializeComponent();
        }
    }
}
