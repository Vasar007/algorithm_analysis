using System;
using System.Windows;
using System.Windows.Input;
using Acolyte.Assertions;
using MaterialDesignThemes.Wpf;
using Prism.Events;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DesktopApp.Domain.Messages;
using AlgorithmAnalysis.DesktopApp.ViewModels;
using AlgorithmAnalysis.Logging;
using AlgorithmAnalysis.DesktopApp.Models;
using AlgorithmAnalysis.DesktopApp.Properties;

namespace AlgorithmAnalysis.DesktopApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private static readonly ILogger _logger = LoggerFactory.CreateLoggerFor<MainWindow>();

        private readonly IEventAggregator _eventAggregator;

        private readonly SnackbarMessageQueue _messageQueue;

        public MainWindow(
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator.ThrowIfNull(nameof(eventAggregator));

            InitializeComponent();

            _messageQueue = MainSnackbar.MessageQueue;

            _logger.Info("Main window was created.");

            SubscribeOnConfigReload();
        }

        private void SubscribeOnConfigReload()
        {
            var rootChangeToken = ConfigOptions.GetReloadToken();

            rootChangeToken.RegisterChangeCallback(
                ShowMessageOnConfigReload, state: _messageQueue
            );
        }

        private void ShowMessageOnConfigReload(object state)
        {
            if (!(state is SnackbarMessageQueue messageQueue))
            {
                throw new ArgumentException("Argument must be message queue.", nameof(state));
            }

            // Use the message queue to send a message.
            // The message queue can be called from any thread.
            messageQueue.Enqueue(
                content: DesktopAppStrings.SnackBarOnConfigReloadMessage,
                actionContent: DesktopAppStrings.SnackBarOnConfigReloadActionText,
                actionHandler: actionArgument => ReloadParameters(),
                actionArgument: null,
                promote: true,
                neverConsiderToBeDuplicate: false
            );

            // Callback is called only one time, need to resubscribe.
            SubscribeOnConfigReload();
        }

        private void ReloadParameters()
        {
            _eventAggregator
                .GetEvent<ConfigOptionsWereChangedMessage>()
                .Publish();
        }

        private void OnCopy(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            if (!(eventArgs.Parameter is string stringValue)) return;

            try
            {
                Clipboard.SetDataObject(stringValue);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Data could not be copied to clipboard.");
            }
        }

        private void Settings_DialogOpened(object sender, DialogOpenedEventArgs eventArgs)
        {
            if (!(eventArgs.Session.Content is SettingsView _)) return;
        }

        private void Settings_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            // Use different types to publish different events.
            switch (eventArgs.Parameter)
            {
                case SettingsModel _:
                    // Save all settings to config file and close dialog.
                    SaveAllSettings();
                    break;

                case SettingsViewModel _:
                    // Reset all settings and close dialog.
                    ResetAllSettings();
                    break;

                default:
                    string typeName = eventArgs.Parameter?.GetType().Name ?? "NULL";
                    throw new ArgumentOutOfRangeException(
                        nameof(eventArgs), eventArgs.Parameter,
                        $"Unknwon parameter type: '{typeName}'."
                    );
            }
        }

        private void SaveAllSettings()
        {
            _eventAggregator
                .GetEvent<SaveAllSettingsMessage>()
                .Publish();
        }

        private void ResetAllSettings()
        {
            _eventAggregator
                .GetEvent<ResetAllSettingsMessage>()
                .Publish();
        }
    }
}
