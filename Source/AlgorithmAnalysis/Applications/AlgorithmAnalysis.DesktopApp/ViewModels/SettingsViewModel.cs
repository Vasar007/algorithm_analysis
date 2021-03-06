﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acolyte.Assertions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using AlgorithmAnalysis.Common.Files;
using AlgorithmAnalysis.Common.Processes;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DesktopApp.Domain.Commands;
using AlgorithmAnalysis.DesktopApp.Domain.Messages;
using AlgorithmAnalysis.DesktopApp.Models;
using AlgorithmAnalysis.Logging;
using Acolyte.Collections;

namespace AlgorithmAnalysis.DesktopApp.ViewModels
{
    internal sealed class SettingsViewModel : BindableBase
    {
        private static readonly ILogger _logger =
            LoggerFactory.CreateLoggerFor<SettingsViewModel>();

        private readonly IEventAggregator _eventAggregator;

        public SettingsModel Settings { get; }

        public ICommand OpenConfigFileCommand { get; }

        public ICommand ResetSettingsCommand { get; }

        public ICommand AddNewAlgorithmCommand { get; }

        public ICommand RemoveAlgorithmCommand { get; }


        public SettingsViewModel(
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator.ThrowIfNull(nameof(eventAggregator));

            Settings = new SettingsModel();

            OpenConfigFileCommand = new AsyncRelayCommand(OpenConfigFileAsync);
            ResetSettingsCommand = new DelegateCommand(ResetAllSettingsSafe);

            AddNewAlgorithmCommand = new DelegateCommand(AddNewAlgorithmSafe);
            RemoveAlgorithmCommand = new DelegateCommand<IList<object>>(RemoveAlgorithmSafe);

            SubscribeOnEvents();
        }

        private void SubscribeOnEvents()
        {
            _eventAggregator
               .GetEvent<SaveAllSettingsMessage>()
               .Subscribe(SaveAllSettingsSafe);

            _eventAggregator
               .GetEvent<ResetAllSettingsMessage>()
               .Subscribe(ResetAllSettingsSafe);

            _eventAggregator
               .GetEvent<PartiallySaveAlgorithmSettingsMessage>()
               .Subscribe(PartiallySaveAlgorithmSettingsSafe);

            _eventAggregator
               .GetEvent<ResetAlgorithmSettingsMessage>()
               .Subscribe(ResetAlgorithmSettingsSafe);

            _eventAggregator
              .GetEvent<ConfigOptionsWereChangedManuallyMessage>()
              .Subscribe(ResetAppearenceSettingsSafe);
        }

        private void SaveAllSettingsSafe()
        {
            _logger.Info("Saving all settings to configuration file.");

            try
            {
                GlobalSettingsTracker.HasSettingsChanged = true;

                Settings.SaveToConfigFile();
                // Theme has already changed.
            }
            catch (Exception ex)
            {
                string message = $"Failed to save all settings to configuration file: {ex.Message}";

                _logger.Error(ex, message);
                MessageBoxProvider.ShowError(message);
            }

            _eventAggregator
                .GetEvent<ConfigOptionsWereChangedThroughSettingsMessage>()
                .Publish();
        }

        private void ResetAllSettingsSafe()
        {
            _logger.Info("Resetting all settings to default values (as in config file).");

            try
            {
                Settings.Reset();
            }
            catch (Exception ex)
            {
                string message = $"Failed to reset all settings: {ex.Message}";

                _logger.Error(ex, message);
                MessageBoxProvider.ShowError(message);
            }
        }

        private void PartiallySaveAlgorithmSettingsSafe()
        {
            _logger.Info("Partially saving algorithm settings (just update some data).");

            try
            {
                Settings.Analysis.UpdateAlgorithmsStatus();
            }
            catch (Exception ex)
            {
                string message = $"Failed to partially save algorithm settings: {ex.Message}";

                _logger.Error(ex, message);
                MessageBoxProvider.ShowError(message);
            }
        }

        private void ResetAlgorithmSettingsSafe()
        {
            _logger.Info("Resetting algorithm settings to default values (as in config file).");

            try
            {
                AnalysisOptions analysisOptions = ConfigOptions.Analysis;

                Settings.Analysis.ResetAlgorithmSettings(analysisOptions);
            }
            catch (Exception ex)
            {
                string message = $"Failed to reset algorithm settings: {ex.Message}";

                _logger.Error(ex, message);
                MessageBoxProvider.ShowError(message);
            }
        }

        private void ResetAppearenceSettingsSafe()
        {
            _logger.Info("Resetting appearence settings to default values (as in config file).");

            try
            {
                Settings.Appearence.Reset();
            }
            catch (Exception ex)
            {
                string message = $"Failed to reset appearence settings: {ex.Message}";

                _logger.Error(ex, message);
                MessageBoxProvider.ShowError(message);
            }
        }

        private void AddNewAlgorithmSafe()
        {
            _logger.Info("Addiing new algorithm value to table.");

            try
            {
                Settings.Analysis.AddNewAlgorithm();
            }
            catch (Exception ex)
            {
                string message = $"Failed to add new algorithm value: {ex.Message}";

                _logger.Error(ex, message);
                MessageBoxProvider.ShowError(message);
            }
        }

        private void RemoveAlgorithmSafe(IList<object> selectedItems)
        {
            _logger.Info("Remove algorithm value from table.");

            try
            {
                var convertedItems = selectedItems
                    .Cast<AlgorithmTypeValueModel>()
                    .ToReadOnlyList();

                Settings.Analysis.RemoveAlgorithm(convertedItems);
            }
            catch (Exception ex)
            {
                string message = $"Failed to remove algorithm value: {ex.Message}";

                _logger.Error(ex, message);
                MessageBoxProvider.ShowError(message);
            }
        }

        private Task OpenConfigFileAsync()
        {
            string configFilePath = PredefinedPaths.DefaultOptionsPath;
            var configFile = new FileInfo(configFilePath);

            if (!configFile.Exists)
            {
                MessageBoxProvider.ShowInfo("Configuration file was not found on disk.");
                return Task.CompletedTask;
            }

            _ = ProcessManager.OpenFileWithAssociatedAppAsync(configFile);
            return Task.CompletedTask;
        }
    }
}
