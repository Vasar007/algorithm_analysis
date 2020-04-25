using Prism.Mvvm;
using Acolyte.Assertions;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DesktopApp.Domain;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class SettingsLoggerModel : BindableBase, ISettingsModel
    {
        // Initializes through Reset method in ctor.
        private string _originalLogFolderPath = default!;

        // Initializes through Reset method in ctor.
        private string _logFolderPath = default!;
        public string LogFolderPath
        {
            get => _logFolderPath;
            set => SetProperty(ref _logFolderPath, value.ThrowIfNull(nameof(value)));
        }

        private bool _enableLogForExcelLibrary;
        public bool EnableLogForExcelLibrary
        {
            get => _enableLogForExcelLibrary;
            set => SetProperty(ref _enableLogForExcelLibrary, value);
        }

        // Initializes through Reset method in ctor.
        private string _logFilesExtension = default!;
        public string LogFilesExtension
        {
            get => _logFilesExtension;
            set => SetProperty(ref _logFilesExtension, value.ThrowIfNull(nameof(value)));
        }

        // Initializes through Reset method in ctor.
        private string _logFilenameSeparator = default!;
        public string LogFilenameSeparator
        {
            get => _logFilenameSeparator;
            set => SetProperty(ref _logFilenameSeparator, value.ThrowIfNull(nameof(value)));
        }

        private bool _useFullyQualifiedEntityNames;
        public bool UseFullyQualifiedEntityNames
        {
            get => _useFullyQualifiedEntityNames;
            set => SetProperty(ref _useFullyQualifiedEntityNames, value);
        }


        public SettingsLoggerModel()
        {
            Reset();
        }

        #region IResetable Implementation

        public void Reset()
        {
            LoggerOptions loggerOptions = ConfigOptions.Logger;

            _originalLogFolderPath = loggerOptions.LogFolderPath;
            LogFolderPath = ModelPathTransformer.TransformPathToRelative(
                loggerOptions.LogFolderPath
            );
            EnableLogForExcelLibrary = loggerOptions.EnableLogForExcelLibrary;
            LogFilesExtension = loggerOptions.LogFilesExtension;
            LogFilenameSeparator = loggerOptions.LogFilenameSeparator;
        }

        public void Validate()
        {
            // TODO: implement settings parameters validation:
            // LogFolderPath
            // LogFilesExtension
            // LogFilenameSeparator
        }

        #endregion

        #region ISaveable Implementation

        public void SaveToConfigFile()
        {
            Validate();

            LoggerOptions loggerOptions = ConfigOptions.Logger;

            loggerOptions.LogFolderPath = ModelPathTransformer.TransformPathToOriginal(
                _originalLogFolderPath, LogFolderPath
            );
            loggerOptions.EnableLogForExcelLibrary = EnableLogForExcelLibrary;
            loggerOptions.LogFilesExtension = LogFilesExtension;
            loggerOptions.LogFilenameSeparator = LogFilenameSeparator;
            loggerOptions.UseFullyQualifiedEntityNames = UseFullyQualifiedEntityNames;

            ConfigOptions.SetOptions(loggerOptions);
        }

        #endregion
    }
}
