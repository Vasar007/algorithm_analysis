using System;
using System.Windows.Controls;
using Acolyte.Assertions;
using AlgorithmAnalysis.DesktopApp.Domain.Messages;
using MaterialDesignThemes.Wpf;
using Prism.Events;

namespace AlgorithmAnalysis.DesktopApp.Views
{
    /// <summary>
    /// Interaction logic for SettingsAnalysisView.xaml
    /// </summary>
    public sealed partial class SettingsAnalysisView : UserControl
    {
        private readonly IEventAggregator _eventAggregator;


        public SettingsAnalysisView(
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator.ThrowIfNull(nameof(eventAggregator));

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
                case bool value when value:
                    break;

                case bool value when !value:
                    // Reset algorithm settings and close dialog.
                    ResetAlgorithmSettings();
                    break;

                default:
                    string typeName = eventArgs.Parameter?.GetType().Name ?? "NULL";
                    throw new ArgumentOutOfRangeException(
                        nameof(eventArgs), eventArgs.Parameter,
                        $"Unknwon parameter type: '{typeName}'."
                    );
            }
        }

        private void ResetAlgorithmSettings()
        {
            _eventAggregator
                .GetEvent<ResetAlgorithmSettingsMessage>()
                .Publish();
        }
    }
}
