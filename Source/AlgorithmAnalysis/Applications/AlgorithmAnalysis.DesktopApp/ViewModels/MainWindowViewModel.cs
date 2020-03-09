using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Commands;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DesktopApp.Domain.Commands;
using AlgorithmAnalysis.DesktopApp.Models;
using AlgorithmAnalysis.DomainLogic;
using AlgorithmAnalysis.DomainLogic.Analysis;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DesktopApp.ViewModels
{
    internal sealed class MainWindowViewModel : BindableBase
    {
        private readonly AnalysisPerformer _performer;

        private readonly FileInfo _outputExcelFile;

        public string Title { get; }

        public IReadOnlyList<PhaseOnePartOneAnalysisKind> AvailableAnalysisKindForPhaseOnePartOne { get; }

        public IReadOnlyList<PhaseOnePartTwoAnalysisKind> AvailableAnalysisKindForPhaseOnePartTwo { get; }

        public IReadOnlyList<PhaseTwoAnalysisKind> AvailableAnalysisKindForPhaseTwo { get; }

        public IReadOnlyList<AlgorithmType> AvailableAlgorithms { get; }

        public RawParametersPack Parameters { get; }

        private bool _canExecuteAnalysis;
        public bool CanExecuteAnalysis
        {
            get => _canExecuteAnalysis;
            set => SetProperty(ref _canExecuteAnalysis, value);
        }

        public ICommand RunCommand { get; }

        public ICommand ResetCommand { get; }


        public MainWindowViewModel()
        {
            _outputExcelFile = new FileInfo(ConfigOptions.Excel.OutputExcelFilename);
            _performer = new AnalysisPerformer();

            Title = DesktopOptions.Title;
            AvailableAnalysisKindForPhaseOnePartOne = DesktopOptions.AvailableAnalysisKindForPhaseOnePartOne;
            AvailableAnalysisKindForPhaseOnePartTwo = DesktopOptions.AvailableAnalysisKindForPhaseOnePartTwo;
            AvailableAnalysisKindForPhaseTwo = DesktopOptions.AvailableAnalysisKindForPhaseTwo;
            AvailableAlgorithms = DesktopOptions.AvailableAlgorithms;
            Parameters = new RawParametersPack();
            CanExecuteAnalysis = true;

            RunCommand = new AsyncRelayCommand(LaunchAnalysis);
            ResetCommand = new DelegateCommand(ResetFields);
        }

        private async Task LaunchAnalysis()
        {
            CanExecuteAnalysis = false;

            try
            {
                // TODO: check that all text boxes has a valid content (now exception will be 
                // thrown during ParametersPack validation in ctor).

                // TODO: display waiting message (and progress bar, if it's possible).

                AnalysisContext context = Parameters.CreateContext(_outputExcelFile);

                CheckOutputFile();
                
                // TODO: add cancellation button to interupt analysis.
                AnalysisResult result = await Task
                    .Run(() => _performer.PerformAnalysis(context))
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
            Parameters.Reset();
        }

        private void CheckOutputFile()
        {
            // Use static File methods because FileInfo can have out-of-date state.
            if (!File.Exists(_outputExcelFile.FullName)) return;

            // TODO: check final excel file and ASK user to delete file or change output name.
            string message =
                "There are file with the same name as output analysis file " +
                $"('{_outputExcelFile}'). " +
                "This file will be removed.";

            MessageBoxProvider.ShowInfo(message);

            File.Delete(_outputExcelFile.FullName);
        }

        private static void ProcessResult(AnalysisResult result)
        {
            if (result.Success)
            {
                MessageBoxProvider.ShowInfo(result.Message);
            }
            else
            {
                MessageBoxProvider.ShowError(result.Message);
            }
        }
    }
}
