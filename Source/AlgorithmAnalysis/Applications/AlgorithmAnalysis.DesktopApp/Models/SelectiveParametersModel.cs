using Acolyte.Assertions;
using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DomainLogic;
using AlgorithmAnalysis.Models;
using System.Collections.Generic;
using AlgorithmAnalysis.Math;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class SelectiveParametersModel : BindableBase, IResetable
    {
        public IReadOnlyList<PhaseOnePartOneAnalysisKind> AvailableAnalysisKindForPhaseOnePartOne { get; }

        // Initializes through Reset method in ctor.
        private PhaseOnePartOneAnalysisKind _selectedPhaseOnePartOne = default!;
        public PhaseOnePartOneAnalysisKind SelectedPhaseOnePartOne
        {
            get => _selectedPhaseOnePartOne;
            set => SetProperty(ref _selectedPhaseOnePartOne, value.ThrowIfNull(nameof(value)));
        }

        public IReadOnlyList<PhaseOnePartTwoAnalysisKind> AvailableAnalysisKindForPhaseOnePartTwo { get; }

        // Initializes through Reset method in ctor.
        private PhaseOnePartTwoAnalysisKind _selectedPhaseOnePartTwo = default!;
        public PhaseOnePartTwoAnalysisKind SelectedPhaseOnePartTwo
        {
            get => _selectedPhaseOnePartTwo;
            set => SetProperty(ref _selectedPhaseOnePartTwo, value.ThrowIfNull(nameof(value)));
        }

        public IReadOnlyList<PhaseTwoAnalysisKind> AvailableAnalysisKindForPhaseTwo { get; }

        // Initializes through Reset method in ctor.
        private PhaseTwoAnalysisKind _selectedPhaseTwo = default!;
        public PhaseTwoAnalysisKind SelectedPhaseTwo
        {
            get => _selectedPhaseTwo;
            set => SetProperty(ref _selectedPhaseTwo, value.ThrowIfNull(nameof(value)));
        }

        public IReadOnlyList<AlgorithmType> AvailableAlgorithms { get; }

        // Initializes through Reset method in ctor.
        private AlgorithmType _selectedAlgorithmType = default!;
        public AlgorithmType SelectedAlgorithmType
        {
            get => _selectedAlgorithmType;
            set => SetProperty(ref _selectedAlgorithmType, value.ThrowIfNull(nameof(value)));
        }

        public IReadOnlyList<GoodnessOfFitKind> AvailableGoodnessOfFitKinds { get; }

        // Initializes through Reset method in ctor.
        private GoodnessOfFitKind _selectedGoodnessOfFitKind = default!;
        public GoodnessOfFitKind SelectedGoodnessOfFitKind
        {
            get => _selectedGoodnessOfFitKind;
            set => SetProperty(ref _selectedGoodnessOfFitKind, value.ThrowIfNull(nameof(value)));
        }


        public SelectiveParametersModel()
        {
            AvailableAnalysisKindForPhaseOnePartOne = DesktopOptions.AvailableAnalysisKindForPhaseOnePartOne;
            AvailableAnalysisKindForPhaseOnePartTwo = DesktopOptions.AvailableAnalysisKindForPhaseOnePartTwo;
            AvailableAnalysisKindForPhaseTwo = DesktopOptions.AvailableAnalysisKindForPhaseTwo;
            AvailableAlgorithms = DesktopOptions.AvailableAlgorithms;
            AvailableGoodnessOfFitKinds = DesktopOptions.AvailableGoodnessOfFitKinds;

            Reset();
        }

        #region IResetable Implementation

        public void Reset()
        {
            SelectedPhaseOnePartOne = AvailableAnalysisKindForPhaseOnePartOne[0];
            SelectedPhaseOnePartTwo = AvailableAnalysisKindForPhaseOnePartTwo[0];
            // TODO: reset selected index when implement normal distribution.
            SelectedPhaseTwo = AvailableAnalysisKindForPhaseTwo[1];
            SelectedAlgorithmType = AvailableAlgorithms[0];
            SelectedGoodnessOfFitKind = AvailableGoodnessOfFitKinds[0];
        }

        #endregion
    }
}
