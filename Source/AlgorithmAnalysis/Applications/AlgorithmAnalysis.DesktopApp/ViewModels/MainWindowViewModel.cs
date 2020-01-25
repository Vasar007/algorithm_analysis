using System;
using System.Collections.Generic;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Commands;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DesktopApp.Models;
using AlgorithmAnalysis.DomainLogic;

namespace AlgorithmAnalysis.DesktopApp.ViewModels
{
    internal sealed class MainWindowViewModel : BindableBase
    {
        private readonly AnalysisPerformer _performer;

        public string Title { get; }

        public IReadOnlyList<string> AvailableAlgorithms { get; }

        public RawParametersPack Parameters { get; }

        public ICommand RunCommand { get; }

        public ICommand ResetCommand { get; }


        public MainWindowViewModel()
        {
            _performer = new AnalysisPerformer();

            Title = DesktopOptions.Title;
            AvailableAlgorithms = DesktopOptions.AvailableAlgorithms;
            Parameters = new RawParametersPack();

            RunCommand = new DelegateCommand(LaunchAnalysis);
            ResetCommand = new DelegateCommand(ResetFields);
        }

        private void LaunchAnalysis()
        {
            try
            {
                // TODO: check that all text boxes has a valid content.

                // TODO: disable controls on main window when analysis started.
                // TODO: display waiting message (and progress bar, if it's possible).

                _performer.PerformAnalysis(Parameters.Convert());

                MessageBoxProvider.ShowInfo("Analysis finished.");

                // TODO: enable controls on main window when analysis finished.
            }
            catch (Exception ex)
            {
                MessageBoxProvider.ShowError($"Exception occurred: {ex.Message}");
            }
        }

        private void ResetFields()
        {
            Parameters.Reset();
        }
    }
}
