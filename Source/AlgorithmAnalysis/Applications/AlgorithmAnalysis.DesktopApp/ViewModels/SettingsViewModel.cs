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

        public ICommand OpenConfigFileCommand { get; }

        public ICommand ResetSettingsCommand { get; }


        public SettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator.ThrowIfNull(nameof(eventAggregator));

            Settings = new SettingsModel();

            OpenConfigFileCommand = new DelegateCommand(ResetSettingsSafe);
            ResetSettingsCommand = new DelegateCommand(ResetSettingsSafe);

            SubscribeOnEvents();
        }

        private void SubscribeOnEvents()
        {
            _eventAggregator
               .GetEvent<SaveSettingsMessage>()
               .Subscribe(SaveSettingsSafe);

            _eventAggregator
               .GetEvent<ResetSettingsMessage>()
               .Subscribe(ResetSettingsSafe);
        }

        private void SaveSettingsSafe()
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

        private void ResetSettingsSafe()
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
