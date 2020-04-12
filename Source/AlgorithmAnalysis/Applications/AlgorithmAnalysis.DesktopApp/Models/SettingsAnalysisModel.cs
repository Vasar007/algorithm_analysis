using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acolyte.Assertions;
using Acolyte.Collections;
using Prism.Mvvm;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.Models;
using AlgorithmAnalysis.DesktopApp.Properties;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class SettingsAnalysisModel : BindableBase, IChangeable, ISaveable
    {
        #region Algorithms

        // Initializes through Reset method in ctor.
        public ObservableCollection<AlgorithmType> SpecifiedAlgorithms { get; private set; } =
            default!;

        public int SpecifiedAlgorithmsNumber => SpecifiedAlgorithms.Count;

        public string SpecifiedAlgorithmsStatusText =>
            string.Format(SpecifiedAlgorithmsStatusTextFormat(), SpecifiedAlgorithmsNumber);

        public bool IsAnyAlgorithmSpecified => SpecifiedAlgorithms.IsNotEmpty();

        /// <summary>
        /// Shows warning about no availbale algorithms to analyze.
        /// </summary>
        public bool IsHintForAlgorithmVisible => !IsAnyAlgorithmSpecified;

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
            Reset();
        }

        #region IChangeableModel Implementation

        public void Reset()
        {
            AnalysisOptions analysisOptions = ConfigOptions.Analysis;

            var algorithmTypes = analysisOptions.AvailableAlgorithms.GetAlgorithmTypes();
            SpecifiedAlgorithms = new ObservableCollection<AlgorithmType>(algorithmTypes);
            CommonAnalysisFilenameSuffix = analysisOptions.CommonAnalysisFilenameSuffix;
            OutputFileExtension = analysisOptions.OutputFileExtension;
        }

        public void Validate()
        {
            // TODO: implement settings parameters validation:
            // AvailableAlgorithms
            // CommonAnalysisFilenameSuffix
            // OutputFileExtension
        }

        #endregion

        #region ISaveable Implementation

        public void SaveToConfigFile()
        {
            Validate();

            AnalysisOptions analysisOptions = ConfigOptions.Analysis;

            analysisOptions.AvailableAlgorithms = SpecifiedAlgorithms.GetAlgorithmTypeValues();
            analysisOptions.CommonAnalysisFilenameSuffix = CommonAnalysisFilenameSuffix;
            analysisOptions.OutputFileExtension = OutputFileExtension;

            ConfigOptions.SetOptions(analysisOptions);
        }

        #endregion

        private string SpecifiedAlgorithmsStatusTextFormat()
        {
            return SpecifiedAlgorithmsNumber switch
            {
                0 => DesktopAppStrings.SpecifiedAlgorithmsNotFoundText,

                1 => DesktopAppStrings.SpecifiedAlgorithmsSingleText,

                _ when SpecifiedAlgorithmsNumber > 1 => DesktopAppStrings.SpecifiedAlgorithmsManyText,

                _ => throw new ArgumentOutOfRangeException(
                         nameof(SpecifiedAlgorithmsNumber), SpecifiedAlgorithmsNumber,
                         $"Unexpected algorithms number: '{SpecifiedAlgorithmsNumber.ToString()}'."
                     )
            };
        }
    }
}
