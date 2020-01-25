using System.Collections.Generic;
using System.Windows.Input;
using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DesktopApp.Models;
using Prism.Commands;
using System;
using System.Diagnostics;

namespace AlgorithmAnalysis.DesktopApp.ViewModels
{
    internal sealed class MainWindowViewModel : BindableBase
    {
        public string Title { get; }

        public IReadOnlyList<string> AvailableAlgorithms { get; }

        public RawParametersPack Parameters { get; }

        public ICommand RunCommand { get; }

        public ICommand ResetCommand { get; }


        public MainWindowViewModel()
        {
            Title = DesktopOptions.Title;
            AvailableAlgorithms = DesktopOptions.AvailableAlgorithms;
            Parameters = new RawParametersPack();

            RunCommand = new DelegateCommand(StartAnalysis);
            ResetCommand = new DelegateCommand(ResetFields);
        }

        private void StartAnalysis()
        {
            try
            {
                ParametersPack args = Parameters.Convert();

                var starterInfo = new ProcessStartInfo(
                    DesktopOptions.AnalysisProgramName,
                    args.PackAsInputArguments()
                )
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                };

                // TODO: disable controls on main window when analysis started.
                // TODO: display waiting message (and progress bar, if it's possible).
                using (Process algorithmApp = Process.Start(starterInfo))
                {
                    algorithmApp.WaitForExit();
                }
                
                // TODO: find output files with data and parse them.
                // TODO: save output data to the Excel tables and apply formulas.
                // TODO: delete output files with data.

                MessageBoxProvider.ShowInfo($"Analysis finished.");
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
