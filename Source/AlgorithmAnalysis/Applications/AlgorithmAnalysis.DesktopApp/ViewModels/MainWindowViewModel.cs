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

namespace AlgorithmAnalysis.DesktopApp.ViewModels
{
    internal sealed class MainWindowViewModel : BindableBase
    {
        private readonly ResultWrapper _result;

        private readonly AnalysisPerformer _performer;

        public string Title { get; }

        public AnalysisSpecificModel AnalysisSpecific { get; }

        public RawParametersPack RawParameters { get; }

        public SelectiveParametersModel SelectiveParameters { get; }

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
            _result = ResultWrapper.Create(ConfigOptions.Excel);
            _performer = new AnalysisPerformer();

            Title = DesktopOptions.Title;

            AnalysisSpecific = new AnalysisSpecificModel();
            RawParameters = new RawParametersPack();
            SelectiveParameters = new SelectiveParametersModel();
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

                AnalysisContext context = RawParameters.CreateContext(
                    _result.OutputExcelFile, SelectiveParameters
                );

                CheckOutputFile();

                // TODO: add cancellation button to interupt analysis.
                AnalysisResult result = await Task.Run(() => _performer.PerformAnalysisAsync(context))
                    .ConfigureAwait(false);

                ProcessResult(result);
            }
            catch (Exception ex)
            {
                MessageBoxProvider.ShowError($"Analysis failed. Exception occurred: {ex.Message}");
            }
            finally
            {
                CanExecuteAnalysis = true;
            }
        }

        private void ResetFields()
        {
            AnalysisSpecific.Reset();
            RawParameters.Reset();
            SelectiveParameters.Reset();
        }

        private void CheckOutputFile()
        {
            // Use static File methods because FileInfo can have out-of-date state.
            if (!File.Exists(_result.OutputExcelFile.FullName)) return;

            // TODO: check final excel file and ASK user to delete file or change output name.
            string message =
                "There are file with the same name as output analysis file " +
                $"('{_result}'). " +
                "This file will be removed.";

            MessageBoxProvider.ShowInfo(message);

            File.Delete(_result.OutputExcelFile.FullName);
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
            if (AnalysisSpecific.OpenAnalysisResults)
            {
                _ = ProcessManager.OpenFileWithAssociatedAppAsync(_result.OutputExcelFile);
            }
        }
    }
}
