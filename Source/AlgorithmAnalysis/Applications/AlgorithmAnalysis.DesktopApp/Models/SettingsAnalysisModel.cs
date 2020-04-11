﻿using Acolyte.Assertions;
using Prism.Mvvm;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DesktopApp.Domain;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class SettingsAnalysisModel : BindableBase, IChangeableModel
    {
        // TODO: allow to configure algorithms.

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
            Validate();
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
    }
}
