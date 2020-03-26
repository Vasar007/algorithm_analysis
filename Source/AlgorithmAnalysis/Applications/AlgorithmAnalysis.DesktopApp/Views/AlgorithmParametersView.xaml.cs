using System.Windows;
using System.Windows.Controls;

namespace AlgorithmAnalysis.DesktopApp.Views
{
    /// <summary>
    /// Interaction logic for AlgorithmParametersView.xaml
    /// </summary>
    public sealed partial class AlgorithmParametersView : UserControl
    {
        #region AlgorithmComboBox

        public static readonly DependencyProperty AlgorithmComboBoxProxyProperty =
            DependencyProperty.Register(
                nameof(AlgorithmComboBoxProxy),
                typeof(ComboBox),
                typeof(AlgorithmParametersView),
                new PropertyMetadata(default(ComboBox))
            );

        public ComboBox AlgorithmComboBoxProxy
        {
            get => (ComboBox) GetValue(AlgorithmComboBoxProxyProperty);
            set => SetValue(AlgorithmComboBoxProxyProperty, value);
        }

        #endregion

        #region FirstValueTextBox

        public static readonly DependencyProperty FirstValueTextBoxProxyProperty =
            DependencyProperty.Register(
                nameof(FirstValueTextBoxProxy),
                typeof(TextBox),
                typeof(AlgorithmParametersView),
                new PropertyMetadata(default(TextBox))
            );

        public TextBox FirstValueTextBoxProxy
        {
            get => (TextBox) GetValue(FirstValueTextBoxProxyProperty);
            set => SetValue(FirstValueTextBoxProxyProperty, value);
        }

        #endregion

        #region LastValueTextBox

        public static readonly DependencyProperty LastValueTextBoxProxyProperty =
            DependencyProperty.Register(
                nameof(LastValueTextBoxProxy),
                typeof(TextBox),
                typeof(AlgorithmParametersView),
                new PropertyMetadata(default(TextBox))
            );

        public TextBox LastValueTextBoxProxy
        {
            get => (TextBox) GetValue(LastValueTextBoxProxyProperty);
            set => SetValue(LastValueTextBoxProxyProperty, value);
        }

        #endregion

        #region LastExtrapolationValueTextBox

        public static readonly DependencyProperty LastExtrapolationValueTextBoxProxyProperty =
            DependencyProperty.Register(
                nameof(LastExtrapolationValueTextBoxProxy),
                typeof(TextBox),
                typeof(AlgorithmParametersView),
                new PropertyMetadata(default(TextBox))
            );

        public TextBox LastExtrapolationValueTextBoxProxy
        {
            get => (TextBox) GetValue(LastExtrapolationValueTextBoxProxyProperty);
            set => SetValue(LastExtrapolationValueTextBoxProxyProperty, value);
        }

        #endregion

        #region LaunchesNumberTextBox

        public static readonly DependencyProperty LaunchesNumberTextBoxProxyProperty =
            DependencyProperty.Register(
                nameof(LaunchesNumberTextBoxProxy),
                typeof(TextBox),
                typeof(AlgorithmParametersView),
                new PropertyMetadata(default(TextBox))
            );

        public TextBox LaunchesNumberTextBoxProxy
        {
            get => (TextBox) GetValue(LaunchesNumberTextBoxProxyProperty);
            set => SetValue(LaunchesNumberTextBoxProxyProperty, value);
        }

        #endregion

        #region StepValueTextBox

        public static readonly DependencyProperty StepValueTextBoxProxyProperty =
            DependencyProperty.Register(
                nameof(StepValueTextBoxProxy),
                typeof(TextBox),
                typeof(AlgorithmParametersView),
                new PropertyMetadata(default(TextBox))
            );

        public TextBox StepValueTextBoxProxy
        {
            get => (TextBox) GetValue(StepValueTextBoxProxyProperty);
            set => SetValue(StepValueTextBoxProxyProperty, value);
        }

        #endregion


        public AlgorithmParametersView()
        {
            InitializeComponent();

            AlgorithmComboBoxProxy = AlgorithmComboBox;
            FirstValueTextBoxProxy = FirstValueTextBox;
            LastValueTextBoxProxy = LastValueTextBox;
            LastExtrapolationValueTextBoxProxy = LastExtrapolationValueTextBox;
            LaunchesNumberTextBoxProxy = LaunchesNumberTextBox;
            StepValueTextBoxProxy = StepValueTextBox;
        }
    }
}
