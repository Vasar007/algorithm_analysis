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

                var starterInfo = new ProcessStartInfo("algorithm_analysis.exe", args.GetPack())
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                };

                // TODO: disable controls on main window when analysis started.
                using (Process algorithmApp = Process.Start(starterInfo))
                {
                    algorithmApp.WaitForExit();
                }

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
