using System.Windows.Controls;

namespace AlgorithmAnalysis.DesktopApp.Views
{
    /// <summary>
    /// Interaction logic for AlgorithmParametersView.xaml
    /// </summary>
    public sealed partial class AlgorithmParametersView : UserControl
    {
        #region Proxy Properties

        public ComboBox AlgorithmComboBoxProxy => AlgorithmComboBox;

        public TextBox FirstValueTextBoxProxy => FirstValueTextBox;

        public TextBox LastValueTextBoxProxy => LastValueTextBox;

        public TextBox LastExtrapolationValueTextBoxProxy => LastExtrapolationValueTextBox;

        public TextBox LaunchesNumberTextBoxProxy => LaunchesNumberTextBox;

        public TextBox StepValueTextBoxProxy => StepValueTextBox;

        #endregion


        public AlgorithmParametersView()
        {
            InitializeComponent();
        }
    }
}
