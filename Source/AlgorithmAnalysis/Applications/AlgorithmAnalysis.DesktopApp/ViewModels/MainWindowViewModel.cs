using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Commands;
using AlgorithmAnalysis.Common.Processes;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DesktopApp.Domain.Commands;
using AlgorithmAnalysis.DesktopApp.Models;
using AlgorithmAnalysis.DomainLogic;
using AlgorithmAnalysis.DomainLogic.Analysis;
using AlgorithmAnalysis.Logging;

namespace AlgorithmAnalysis.DesktopApp.ViewModels
{
    internal sealed class MainWindowViewModel : BindableBase
    {
        private static readonly ILogger _logger =
            LoggerFactory.CreateLoggerFor<SettingsViewModel>();

        private readonly ResultWrapper _result;

        private readonly AnalysisPerformer _performer;

        public string Title { get; }

        public ParametersModel Parameters { get; }

        private bool _canExecuteAnalysis;
        public bool CanExecuteAnalysis
        {
            get => _canExecuteAnalysis;
            set => SetProperty(ref _canExecuteAnalysis, value);
        }

        public ICommand AppCloseCommand { get; }

        public ICommand RunCommand { get; }

        public ICommand ResetCommand { get; }


        public MainWindowViewModel()
        {
            _result = ResultWrapper.Create(ConfigOptions.Report);
            _performer = new AnalysisPerformer();

            Title = DesktopOptions.Title;

            Parameters = new ParametersModel();
            CanExecuteAnalysis = true;

            AppCloseCommand = new DelegateCommand(
                ApplicationCloseCommand.Execute, ApplicationCloseCommand.CanExecute
            );

            RunCommand = new AsyncRelayCommand(LaunchAnalysis);
            ResetCommand = new DelegateCommand(ResetFields);
        }

        private async Task LaunchAnalysis()
        {
            CanExecuteAnalysis = false;

            try
            {
                // TODO: display waiting message (and progress bar, if it's possible).

                AnalysisContext context = Parameters.CreateContext(_result.OutputReportFile);

                CheckOutputFile();

                // TODO: add cancellation button to interupt analysis.
                AnalysisResult result = await Task.Run(() => _performer.PerformAnalysisAsync(context))
                    .ConfigureAwait(false);

                ProcessResult(result);
            }
            catch (Exception ex)
            {
                string message = $"Failed to perform analysis: {ex.Message}";

                _logger.Error(ex, message);
                MessageBoxProvider.ShowError(message);
            }
            finally
            {
                CanExecuteAnalysis = true;
            }
        }

        private void ResetFields()
        {
            Parameters.Reset();
        }

        private void CheckOutputFile()
        {
            // Use static File methods because FileInfo can have out-of-date state.
            if (!File.Exists(_result.OutputReportFile.FullName)) return;

            // TODO: check final report file and ASK user to delete file or change output name.
            string message =
                "There are file with the same name as output analysis file " +
                $"('{_result}'). " +
                "This file will be removed.";

            MessageBoxProvider.ShowInfo(message);

            File.Delete(_result.OutputReportFile.FullName);
        }

        private void ProcessResult(AnalysisResult result)
        {
            if (result.Success)
            {
                MessageBoxProvider.ShowInfo(result.Message);
            }
            else
            {
                MessageBoxProvider.ShowError(result.Message);
            }

            OpenResultsIfNeeded();
        }

        private void OpenResultsIfNeeded()
        {
            if (Parameters.Advanced.OpenAnalysisResults)
            {
                _ = ProcessManager.OpenFileWithAssociatedAppAsync(_result.OutputReportFile);
            }
        }
    }
}
