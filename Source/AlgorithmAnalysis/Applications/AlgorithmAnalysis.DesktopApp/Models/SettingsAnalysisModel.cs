using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acolyte.Assertions;
using Acolyte.Collections;
using Prism.Mvvm;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class SettingsAnalysisModel : BindableBase, IChangeable, ISaveable
    {
        #region Algorithms

        public ObservableCollection<AlgorithmType> SpecifiedAlgorithms { get; }

        public int SpecifiedAlgorithmsCount => SpecifiedAlgorithms.Count;

        public bool IsAnyAlgorithmExists => SpecifiedAlgorithms.IsNotEmpty();

        /// <summary>
        /// Shows warning about no availbale algorithms to analyze.
        /// </summary>
        public bool IsHintForAlgorithmVisible => !IsAnyAlgorithmExists;

        #endregion

        // Initializes through Reset method in ctor.
        private string _commonAnalysisFilenameSuffix = default!;
        public string CommonAnalysisFilenameSuffix
        {
            get => _commonAnalysisFilenameSuffix;
            set => SetProperty(ref _commonAnalysisFilenameSuffix, value.ThrowIfNull(nameof(value)));
        }

        // Initializes through Reset method in ctor.
        private string _outputFileExtension = default!;
        public string OutputFileExtension
        {
            get => _outputFileExtension;
            set => SetProperty(ref _outputFileExtension, value.ThrowIfNull(nameof(value)));
        }


        public SettingsAnalysisModel()
        {
            SpecifiedAlgorithms = new ObservableCollection<AlgorithmType>();

            Reset();
        }

        #region IChangeableModel Implementation

        public void Reset()
        {
            AnalysisOptions analysisOptions = ConfigOptions.Analysis;

            CommonAnalysisFilenameSuffix = analysisOptions.CommonAnalysisFilenameSuffix;
            OutputFileExtension = analysisOptions.OutputFileExtension;
        }

        public void Validate()
        {
            // TODO: implement settings parameters validation:
            // CommonAnalysisFilenameSuffix
            // OutputFileExtension
        }

        #endregion

        #region ISaveable Implementation

        public void SaveToConfigFile()
        {
            Validate();

            AnalysisOptions analysisOptions = ConfigOptions.Analysis;

            // TODO: allow to configure algorithms.
            analysisOptions.AvailableAlgorithms = SpecifiedAlgorithms.GetAlgorithmTypeValues();
            analysisOptions.CommonAnalysisFilenameSuffix = CommonAnalysisFilenameSuffix;
            analysisOptions.OutputFileExtension = OutputFileExtension;

            ConfigOptions.SetOptions(analysisOptions);
        }

        #endregion
    }
}
