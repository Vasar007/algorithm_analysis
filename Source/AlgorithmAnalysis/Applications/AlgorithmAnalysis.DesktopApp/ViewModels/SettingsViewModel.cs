using System;
using System.Windows.Input;
using Acolyte.Assertions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DesktopApp.Domain.Messages;
using AlgorithmAnalysis.DesktopApp.Models;
using AlgorithmAnalysis.Logging;

namespace AlgorithmAnalysis.DesktopApp.ViewModels
{
    internal sealed class SettingsViewModel : BindableBase
    {
        // TODO: allow to save into config all setting values.

        private static readonly ILogger _logger =
            LoggerFactory.CreateLoggerFor<SettingsViewModel>();

        private readonly IEventAggregator _eventAggregator;

        public SettingsModel Settings { get; }

        public ICommand ResetSettingsCommand { get; }


        public SettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator.ThrowIfNull(nameof(eventAggregator));

            Settings = new SettingsModel();

            ResetSettingsCommand = new DelegateCommand(ResetSettings);

            SubscribeOnEvents();
        }

        private void SubscribeOnEvents()
        {
            _eventAggregator
               .GetEvent<SaveSettingsMessage>()
               .Subscribe(SaveSettings);

            _eventAggregator
               .GetEvent<ResetSettingsMessage>()
               .Subscribe(ResetSettings);
        }

        private void SaveSettings()
        {
            _logger.Info("Saving settings to configuration file.");

            try
            {
                Settings.SaveToConfigFile();
            }
            catch (Exception ex)
            {
                string message = $"Failed to save settings to configuration file: {ex.Message}";

                _logger.Error(ex, message);
                MessageBoxProvider.ShowError(message);
            }
        }

        private void ResetSettings()
        {
            _logger.Info("Resetting settings to default values (as in config file).");

            try
            {
                Settings.Reset();
            }
            catch (Exception ex)
            {
                string message = $"Failed to reset settings: {ex.Message}";

                _logger.Error(ex, message);
                MessageBoxProvider.ShowError(message);
            }
        }
    }
}
