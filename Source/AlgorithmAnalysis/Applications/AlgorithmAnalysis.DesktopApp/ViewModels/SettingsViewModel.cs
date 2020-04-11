using System;
using Acolyte.Assertions;
using Prism.Events;
using Prism.Mvvm;
using AlgorithmAnalysis.Logging;
using AlgorithmAnalysis.DesktopApp.Models;
using AlgorithmAnalysis.DesktopApp.Domain.Messages;

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
                settingsModel.Validate();


            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to save settings to configuration file.");
            }
        }
    }
}
