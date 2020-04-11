using System;
using Acolyte.Assertions;
using Prism.Events;
using Prism.Mvvm;
using AlgorithmAnalysis.Logging;
using AlgorithmAnalysis.DesktopApp.Models;
using AlgorithmAnalysis.DesktopApp.Domain.Messages;
using AlgorithmAnalysis.DesktopApp.Domain;

namespace AlgorithmAnalysis.DesktopApp.ViewModels
{
    internal sealed class SettingsViewModel : BindableBase
    {
        // TODO: allow to save into config all setting values.

        private static readonly ILogger _logger =
            LoggerFactory.CreateLoggerFor<SettingsViewModel>();

        private readonly IEventAggregator _eventAggregator;

        public SettingsModel Settings { get; }


        public SettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator.ThrowIfNull(nameof(eventAggregator));

            Settings = new SettingsModel();

            SubscribeOnEvents();
        }

        private void SubscribeOnEvents()
        {
            _eventAggregator
               .GetEvent<SaveSettingsMessage>()
               .Subscribe(SaveSettings);
        }

        private void SaveSettings(SettingsModel settingsModel)
        {
            settingsModel.ThrowIfNull(nameof(settingsModel));

            _logger.Info("Saving settings to configuration file.");

            try
            {
                settingsModel.SaveToConfigFile();
            }
            catch (Exception ex)
            {
                string message = $"Failed to save settings to configuration file: {ex.Message}";

                _logger.Error(ex, message);
                MessageBoxProvider.ShowError(message);
            }
        }

        private void ResetSettings(SettingsModel settingsModel)
        {
            settingsModel.ThrowIfNull(nameof(settingsModel));

            _logger.Info("Saving settings to configuration file.");

            try
            {
                settingsModel.Reset();
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
