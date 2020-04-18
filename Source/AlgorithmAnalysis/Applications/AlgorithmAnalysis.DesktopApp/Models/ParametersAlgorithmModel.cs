using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Acolyte.Assertions;
using Acolyte.Collections;
using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DesktopApp.Domain.Validation;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class ParametersAlgorithmModel : BindableBase, IParametersModel
    {
        #region Algorithms

        public ObservableCollection<AlgorithmType> AvailableAlgorithms { get; }

        private AlgorithmType? _selectedAlgorithmType;
        public AlgorithmType? SelectedAlgorithmType
        {
            get => _selectedAlgorithmType;
            set => SetProperty(ref _selectedAlgorithmType, value);
        }

        public bool IsAlgorithmSelectable => AvailableAlgorithms.IsNotEmpty();

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
            AvailableAlgorithms = new ObservableCollection<AlgorithmType>(
                DesktopOptions.AvailableAlgorithms
            );

            Reset();
        }

        #region IChangeable Implementation

        public void Reset()
        {
            ResetAlgorithmType();
            StartValue = "80";
            EndValue = "320";
            ExtrapolationSegmentValue = "2560";
            LaunchesNumber = "200";
            Step = "10";
        }

        public void Validate()
        {
            ValidationHelper.AssertIfGotNullValueFromCollection(
                SelectedAlgorithmType, nameof(AvailableAlgorithms)
            );
        }

        #endregion

        #region IRealoadable Implementation

        public void Reload()
        {
            AvailableAlgorithms.Clear();
            AvailableAlgorithms.AddRange(DesktopOptions.AvailableAlgorithms);

            ResetAlgorithmType();
        }

        #endregion

        private void ResetAlgorithmType()
        {
            SelectedAlgorithmType = AvailableAlgorithms.FirstOrDefault();
        }
    }
}
