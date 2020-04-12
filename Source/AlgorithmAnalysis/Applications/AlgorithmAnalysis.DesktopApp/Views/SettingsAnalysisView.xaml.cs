using System;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace AlgorithmAnalysis.DesktopApp.Views
{
    /// <summary>
    /// Interaction logic for SettingsAnalysisView.xaml
    /// </summary>
    public sealed partial class SettingsAnalysisView : UserControl
    {
        public SettingsAnalysisView()
        {
            InitializeComponent();
        }

        private void SettingsAlgorithmsView_DialogOpened(object sender,
            DialogOpenedEventArgs eventArgs)
        {
            if (!(eventArgs.Session.Content is SettingsAlgorithmsView _)) return;
        }

        private void SettingsAlgorithmsView_DialogClosing(object sender,
            DialogClosingEventArgs eventArgs)
        {
            // Use different types to publish different events.
            switch (eventArgs.Parameter)
            {
                case bool _:
                    break;

                default:
                    string typeName = eventArgs.Parameter?.GetType().Name ?? "NULL";
                    throw new ArgumentOutOfRangeException(
                              nameof(eventArgs), eventArgs.Parameter,
                              $"Unknwon parameter type: '{typeName}'."
                          );
            }
        }
    }
}
