using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Commands;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DesktopApp.Domain.Commands;
using AlgorithmAnalysis.DesktopApp.Models;
using AlgorithmAnalysis.DomainLogic;

namespace AlgorithmAnalysis.DesktopApp.ViewModels
{
    internal sealed class MainWindowViewModel : BindableBase
    {
        private readonly AnalysisPerformer _performer;

        private readonly string _finalExcelFilename;

        public string Title { get; }

        public IReadOnlyList<string> AvailableAnalysisKindForPhaseOne { get; }

        public IReadOnlyList<string> AvailableAlgorithms { get; }

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
            _finalExcelFilename = DesktopOptions.FinalExcelFilename;
            _performer = new AnalysisPerformer(_finalExcelFilename);

            Title = DesktopOptions.Title;
            AvailableAnalysisKindForPhaseOne = DesktopOptions.AvailableAnalysisKindForPhaseOne;
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

                CheckOutputFile();

                AnalysisContext context = Parameters.CreateContext();
                await Task.Run(() => _performer.PerformAnalysis(context)).ConfigureAwait(false);

                MessageBoxProvider.ShowInfo("Analysis finished.");
            }
            catch (Exception ex)
            {
                MessageBoxProvider.ShowError($"Exception occurred: {ex.Message}");
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
            if (!File.Exists(_finalExcelFilename)) return;

            // TODO: check final excel file and ASK user to delete file or change output name.
            string message =
                "There are file with the same name as output analysis file " +
                $"('{_finalExcelFilename}'). " +
                "This file will be removed.";

            MessageBoxProvider.ShowInfo(message);

            File.Delete(_finalExcelFilename);
        }
    }
}
