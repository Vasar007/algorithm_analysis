using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Acolyte.Assertions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using AlgorithmAnalysis.Common.Processes;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DesktopApp.Domain.Commands;
using AlgorithmAnalysis.DesktopApp.Domain.Messages;
using AlgorithmAnalysis.DesktopApp.Models;
using AlgorithmAnalysis.DomainLogic;
using AlgorithmAnalysis.DomainLogic.Analysis;
using AlgorithmAnalysis.Logging;

namespace AlgorithmAnalysis.DesktopApp.ViewModels
{
    internal sealed class MainWindowViewModel : BindableBase
    {
        private static readonly ILogger _logger =
            LoggerFactory.CreateLoggerFor<MainWindowViewModel>();

        private readonly IEventAggregator _eventAggregator;

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

        public ICommand RunAnalysisCommand { get; }

        public ICommand ResetParametersCommand { get; }


        public MainWindowViewModel(
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator.ThrowIfNull(nameof(eventAggregator));

            _result = ResultWrapper.Create(ConfigOptions.Report);
            _performer = new AnalysisPerformer();

            Title = DesktopOptions.Title;

            Parameters = new ParametersModel();
            CanExecuteAnalysis = true;

            AppCloseCommand = new DelegateCommand(
                ApplicationCloseCommand.Execute, ApplicationCloseCommand.CanExecute
            );

            RunAnalysisCommand = new AsyncRelayCommand(LaunchAnalysisSafeAsync);
            ResetParametersCommand = new DelegateCommand(ResetParameters);

            SubscribeOnEvents();
        }

        private void SubscribeOnEvents()
        {
            _eventAggregator
               .GetEvent<SaveAllSettingsMessage>()
               .Subscribe(ReloadParameters);
        }

        private async Task LaunchAnalysisSafeAsync()
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

                _ = ProcessResultAsync(context, result);
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

        private void ReloadParameters()
        {
            Parameters.Reload();
        }

        private void ResetParameters()
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

        private async Task ProcessResultAsync(AnalysisContext context, AnalysisResult result)
        {
            if (result.Success)
            {
                MessageBoxProvider.ShowInfo(result.Message);
            }
            else
            {
                MessageBoxProvider.ShowError(result.Message);
            }

            await OpenResultsIfNeededAsync(context);
        }

        private async Task OpenResultsIfNeededAsync(AnalysisContext context)
        {
            if (context.LaunchContext.ShowResults)
            {
                await ProcessManager.OpenFileWithAssociatedAppAsync(_result.OutputReportFile);
            }
        }
    }
}
