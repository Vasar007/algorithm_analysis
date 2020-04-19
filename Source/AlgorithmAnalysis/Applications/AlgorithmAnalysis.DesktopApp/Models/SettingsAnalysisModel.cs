using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Acolyte.Assertions;
using Acolyte.Collections;
using Prism.Mvvm;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DesktopApp.Properties;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class SettingsAnalysisModel : BindableBase, ISettingsModel
    {
        #region Algorithms

        public ObservableCollection<AlgorithmTypeValueModel> SpecifiedAlgorithms { get; }

        /// <summary>
        /// Shows how many algorithms specified for analysis.
        /// </summary>
        public int SpecifiedAlgorithmsNumber => SpecifiedAlgorithms.Count;

        // Initializes through Reset method in ctor.
        private string _specifiedAlgorithmsStatusText = default!;
        public string SpecifiedAlgorithmsStatusText
        {
            get => _specifiedAlgorithmsStatusText;
            set => SetProperty(ref _specifiedAlgorithmsStatusText, value.ThrowIfNull(nameof(value)));
        }

        public bool IsAnyAlgorithmSpecified => SpecifiedAlgorithms.IsNotEmpty();

        /// <summary>
        /// Shows warning about no availbale algorithms to analyze.
        /// </summary>
        private bool _isHintForAlgorithmVisible;
        public bool IsHintForAlgorithmVisible
        {
            get => _isHintForAlgorithmVisible;
            set => SetProperty(ref _isHintForAlgorithmVisible, value);
        }

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
            SpecifiedAlgorithms = new ObservableCollection<AlgorithmTypeValueModel>();

            Reset();
        }

        #region IChangeable Implementation

        public void Reset()
        {
            AnalysisOptions analysisOptions = ConfigOptions.Analysis;

            ResetAlgorithmSettings(analysisOptions);
            CommonAnalysisFilenameSuffix = analysisOptions.CommonAnalysisFilenameSuffix;
            OutputFileExtension = analysisOptions.OutputFileExtension;
        }

        public void Validate()
        {
            // TODO: implement settings parameters validation:
            // SpecifiedAlgorithms
            // CommonAnalysisFilenameSuffix
            // OutputFileExtension
        }

        #endregion

        #region ISaveable Implementation

        public void SaveToConfigFile()
        {
            Validate();

            AnalysisOptions analysisOptions = ConfigOptions.Analysis;

            analysisOptions.AvailableAlgorithms = SpecifiedAlgorithms
                .Select(model => model.Convert())
                .ToList();

            analysisOptions.CommonAnalysisFilenameSuffix = CommonAnalysisFilenameSuffix;
            analysisOptions.OutputFileExtension = OutputFileExtension;

            ConfigOptions.SetOptions(analysisOptions);
        }

        #endregion

        public void ResetAlgorithmSettings(AnalysisOptions analysisOptions)
        {
            SpecifiedAlgorithms.Clear();

            var algorithmTypes = analysisOptions.AvailableAlgorithms
                .Select(AlgorithmTypeValueModel.Create);
            SpecifiedAlgorithms.AddRange(algorithmTypes);

            UpdateAlgorithmsStatus();
        }

        public void UpdateAlgorithmsStatus()
        {
            SpecifiedAlgorithmsStatusText = string.Format(
                SpecifiedAlgorithmsStatusTextFormat(), SpecifiedAlgorithmsNumber
            );

            IsHintForAlgorithmVisible = !IsAnyAlgorithmSpecified;
        }

        public void AddNewAlgorithm()
        {
            // Add empty model to the end of collection.
            var newEmptyModel = AlgorithmTypeValueModel.CreateEmpty(SpecifiedAlgorithms.Count);
            SpecifiedAlgorithms.Add(newEmptyModel);
        }

        public void RemoveAlgorithm(IReadOnlyList<AlgorithmTypeValueModel> selectedItems)
        {
            if (SpecifiedAlgorithms.IsEmpty()) return;

            // When there are no selected items, remove the last value.
            if (selectedItems.IsEmpty())
            {
                SpecifiedAlgorithms.RemoveAt(SpecifiedAlgorithms.Count - 1);
                return;
            }

            // Otherwise, remove selected item because user can select only one row.
            AlgorithmTypeValueModel algorithmTypeValue = selectedItems.Single();

            SpecifiedAlgorithms.Remove(algorithmTypeValue);

            UpdateAlgorithmIndecies();
        }

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

        private void UpdateAlgorithmIndecies()
        {
            for (int index = 0; index < SpecifiedAlgorithms.Count; ++index)
            {
                // Start indexing for UI with 1 instead of 0.
                SpecifiedAlgorithms[index].Index = index.UseOneBasedIndexing();
            }
        }
    }
}
