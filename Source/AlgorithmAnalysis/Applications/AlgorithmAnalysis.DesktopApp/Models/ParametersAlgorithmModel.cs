using System.Collections.Generic;
using System.Linq;
using Acolyte.Assertions;
using Prism.Mvvm;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DesktopApp.Domain.Validation;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class ParametersAlgorithmModel : BindableBase, IChangeableModel
    {
        #region Algorithms

        public IReadOnlyList<AlgorithmType> AvailableAlgorithms { get; }

        private AlgorithmType? _selectedAlgorithmType;
        public AlgorithmType? SelectedAlgorithmType
        {
            get => _selectedAlgorithmType;
            set => SetProperty(ref _selectedAlgorithmType, value);
        }

        public bool IsAlgorithmSelectable =>
            AvailableAlgorithms.Count > CommonConstants.EmptyCollectionCount;

        /// <summary>
        /// Shows warning about no availbale algorithms to analyze.
        /// </summary>
        public bool IsHintForAlgorithmVisible =>
            !IsAlgorithmSelectable &&
            SelectedAlgorithmType is null;

        #endregion

        // Initializes through Reset method in ctor.
        private string _startValue = default!;
        public string StartValue
        {
            get => _startValue;
            set => SetProperty(ref _startValue, value.ThrowIfNull(nameof(value)));
        }

        // Initializes through Reset method in ctor.
        private string _endValue = default!;
        public string EndValue
        {
            get => _endValue;
            set => SetProperty(ref _endValue, value.ThrowIfNull(nameof(value)));
        }

        // Initializes through Reset method in ctor.
        private string _extrapolationSegmentValue = default!;
        public string ExtrapolationSegmentValue
        {
            get => _extrapolationSegmentValue;
            set => SetProperty(ref _extrapolationSegmentValue, value.ThrowIfNull(nameof(value)));
        }

        // Initializes through Reset method in ctor.
        private string _launchesNumber = default!;
        public string LaunchesNumber
        {
            get => _launchesNumber;
            set => SetProperty(ref _launchesNumber, value.ThrowIfNull(nameof(value)));
        }

        // Initializes through Reset method in ctor.
        private string _step = default!;
        public string Step
        {
            get => _step;
            set => SetProperty(ref _step, value.ThrowIfNull(nameof(value)));
        }


        public ParametersAlgorithmModel()
        {
            AvailableAlgorithms = DesktopOptions.AvailableAlgorithms;

            Reset();
            Validate();
        }

        #region IChangeableModel Implementation

        public void Reset()
        {
            SelectedAlgorithmType = AvailableAlgorithms.FirstOrDefault();

            StartValue = "80";
            EndValue = "320";
            ExtrapolationSegmentValue = "2560";
            LaunchesNumber = "200";
            Step = "10";
        }

        public void Validate()
        {
            ValidationHelper.AssertIfNull(SelectedAlgorithmType, nameof(AvailableAlgorithms));
        }

        #endregion
    }
}
