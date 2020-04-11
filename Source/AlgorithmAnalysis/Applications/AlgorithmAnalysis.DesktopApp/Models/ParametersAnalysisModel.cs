using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DesktopApp.Domain.Validation;
using AlgorithmAnalysis.DomainLogic;
using AlgorithmAnalysis.Math;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class ParametersAnalysisModel : BindableBase, IChangeableModel
    {
        #region Analysis Kind Phase One Part One

        public IReadOnlyList<PhaseOnePartOneAnalysisKind> AvailableAnalysisKindForPhaseOnePartOne { get; }

        private PhaseOnePartOneAnalysisKind? _selectedPhaseOnePartOne;
        public PhaseOnePartOneAnalysisKind? SelectedPhaseOnePartOne
        {
            get => _selectedPhaseOnePartOne;
            set => SetProperty(ref _selectedPhaseOnePartOne, value);
        }

        public bool IsAnalysisKindForPhaseOnePartOneSelectable =>
            AvailableAnalysisKindForPhaseOnePartOne.Count > CommonConstants.EmptyCollectionCount;

        /// <summary>
        /// Show warning about no availbale analysis kind of phase one part one.
        /// </summary>
        public bool IsHintForAnalysisKindForPhaseOnePartOneVisible =>
            !IsAnalysisKindForPhaseOnePartOneSelectable &&
            SelectedPhaseOnePartOne is null;

        #endregion

        #region Analysis Kind Phase One Part Two

        public IReadOnlyList<PhaseOnePartTwoAnalysisKind> AvailableAnalysisKindForPhaseOnePartTwo { get; }

        private PhaseOnePartTwoAnalysisKind? _selectedPhaseOnePartTwo;
        public PhaseOnePartTwoAnalysisKind? SelectedPhaseOnePartTwo
        {
            get => _selectedPhaseOnePartTwo;
            set => SetProperty(ref _selectedPhaseOnePartTwo, value);
        }

        public bool IsAnalysisKindForPhaseOnePartTwoSelectable =>
            AvailableAnalysisKindForPhaseOnePartTwo.Count > CommonConstants.EmptyCollectionCount;

        /// <summary>
        /// Show warning about no availbale analysis kind of phase one part two.
        /// </summary>
        public bool IsHintForAnalysisKindForPhaseOnePartTwoVisible =>
            !IsAnalysisKindForPhaseOnePartTwoSelectable &&
            SelectedPhaseOnePartTwo is null;

        #endregion

        #region Analysis Kind Phase Two

        public IReadOnlyList<PhaseTwoAnalysisKind> AvailableAnalysisKindForPhaseTwo { get; }

        private PhaseTwoAnalysisKind? _selectedPhaseTwo;
        public PhaseTwoAnalysisKind? SelectedPhaseTwo
        {
            get => _selectedPhaseTwo;
            set => SetProperty(ref _selectedPhaseTwo, value);
        }

        public bool IsAnalysisKindForPhaseTwoSelectable =>
            AvailableAnalysisKindForPhaseTwo.Count > CommonConstants.EmptyCollectionCount;

        /// <summary>
        /// Show warning about no availbale analysis kind of phase two.
        /// </summary>
        public bool IsHintForAnalysisKindForPhaseTwoVisible =>
            !IsAnalysisKindForPhaseTwoSelectable &&
            SelectedPhaseTwo is null;

        #endregion

        #region Goodness Of Fit

        public IReadOnlyList<GoodnessOfFitKind> AvailableGoodnessOfFitKinds { get; }

        private GoodnessOfFitKind? _selectedGoodnessOfFitKind;
        public GoodnessOfFitKind? SelectedGoodnessOfFitKind
        {
            get => _selectedGoodnessOfFitKind;
            set => SetProperty(ref _selectedGoodnessOfFitKind, value);
        }

        public bool IsGoodnessOfFitSelectable =>
            AvailableGoodnessOfFitKinds.Count > CommonConstants.EmptyCollectionCount;

        /// <summary>
        /// Show warning about no availbale goodness of fit methods.
        /// </summary>
        public bool IsHintForGoodnessOfFitVisible =>
            !IsGoodnessOfFitSelectable &&
            SelectedGoodnessOfFitKind is null;

        #endregion

        #region Degree Of Parallerism

        public int MinDegreeOfParallelism { get; }

        public int MaxDegreeOfParallelism { get; }

        public bool IsDegreeOfParallelismSelectable =>
            MinDegreeOfParallelism != MaxDegreeOfParallelism;

        /// <summary>
        /// Shows warning about restriction to select degree of parallelism when PC has only one
        /// CPU. May be later we change minimum value of parallelism degree.
        /// </summary>
        public bool IsHintForDegreeOfParallelismVisible =>
            !IsDegreeOfParallelismSelectable &&
            MinDegreeOfParallelism == CommonConstants.MinimumProcessorCount;

        private int _selectedMaxDegreeOfParallelism;
        public int SelectedMaxDegreeOfParallelism
        {
            get => _selectedMaxDegreeOfParallelism;
            set => SetProperty(ref _selectedMaxDegreeOfParallelism, value);
        }

        #endregion


        public ParametersAnalysisModel()
        {
            AvailableAnalysisKindForPhaseOnePartOne = DesktopOptions.AvailableAnalysisKindForPhaseOnePartOne;
            AvailableAnalysisKindForPhaseOnePartTwo = DesktopOptions.AvailableAnalysisKindForPhaseOnePartTwo;
            AvailableAnalysisKindForPhaseTwo = DesktopOptions.AvailableAnalysisKindForPhaseTwo;
            AvailableGoodnessOfFitKinds = DesktopOptions.AvailableGoodnessOfFitKinds;

            MinDegreeOfParallelism = DesktopOptions.MinDegreeOfParallelism;
            MaxDegreeOfParallelism = DesktopOptions.MaxDegreeOfParallelism;

            Reset();
            Validate();
        }

        #region IChangeableModel Implementation

        public void Reset()
        {
            SelectedPhaseOnePartOne = AvailableAnalysisKindForPhaseOnePartOne.FirstOrDefault();
            SelectedPhaseOnePartTwo = AvailableAnalysisKindForPhaseOnePartTwo.FirstOrDefault();
            // TODO: use first value instead of last when implement normal distribution.
            SelectedPhaseTwo = AvailableAnalysisKindForPhaseTwo.LastOrDefault();
            SelectedGoodnessOfFitKind = AvailableGoodnessOfFitKinds.FirstOrDefault();
            SelectedMaxDegreeOfParallelism = MinDegreeOfParallelism;
        }


        public void Validate()
        {
            ValidationHelper.AssertIfGotNullValueFromCollection(
                SelectedPhaseOnePartOne, nameof(AvailableAnalysisKindForPhaseOnePartOne)
            );
            ValidationHelper.AssertIfGotNullValueFromCollection(
                SelectedPhaseOnePartTwo, nameof(AvailableAnalysisKindForPhaseOnePartTwo)
            );
            ValidationHelper.AssertIfGotNullValueFromCollection(
                SelectedPhaseTwo, nameof(AvailableAnalysisKindForPhaseTwo)
            );
            ValidationHelper.AssertIfGotNullValueFromCollection(
                SelectedGoodnessOfFitKind, nameof(AvailableGoodnessOfFitKinds)
            );

            string minDegreeAssertMessage =
                "Invalid min values for degree of parallelism: " +
                $"{MinDegreeOfParallelism.ToString()} <= 0.";
            ValidationHelper.AssertIf(MinDegreeOfParallelism <= 0, minDegreeAssertMessage);

            string maxDegreeAssertMessage =
                "Invalid max values for degree of parallelism: " +
                $"{MaxDegreeOfParallelism.ToString()} <= 0.";
            ValidationHelper.AssertIf(MaxDegreeOfParallelism <= 0, maxDegreeAssertMessage);

            string invalidDegreeValuesAssertMessage =
                "Invalid min and max values for degree of parallelism: " +
                $"{MinDegreeOfParallelism.ToString()} > {MaxDegreeOfParallelism.ToString()}.";
            ValidationHelper.AssertIf(
                MinDegreeOfParallelism > MaxDegreeOfParallelism,
                invalidDegreeValuesAssertMessage
            );
            
        }

        #endregion
    }
}
