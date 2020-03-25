using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DomainLogic;
using AlgorithmAnalysis.Models;
using AlgorithmAnalysis.Math;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class SelectiveParametersModel : BindableBase, IResetable
    {
        public IReadOnlyList<AlgorithmType> AvailableAlgorithms { get; }

        private AlgorithmType? _selectedAlgorithmType;
        public AlgorithmType? SelectedAlgorithmType
        {
            get => _selectedAlgorithmType;
            set => SetProperty(ref _selectedAlgorithmType, value);
        }

        public bool IsAlgorithmSelectable => AvailableAlgorithms.Count > 0;

        /// <summary>
        /// Shows warning about no availbale algorithms to analyze.
        /// </summary>
        public bool IsHintForAlgorithmVisible =>
            !IsAlgorithmSelectable &&
            SelectedAlgorithmType is null;

        public IReadOnlyList<PhaseOnePartOneAnalysisKind> AvailableAnalysisKindForPhaseOnePartOne { get; }

        private PhaseOnePartOneAnalysisKind? _selectedPhaseOnePartOne;
        public PhaseOnePartOneAnalysisKind? SelectedPhaseOnePartOne
        {
            get => _selectedPhaseOnePartOne;
            set => SetProperty(ref _selectedPhaseOnePartOne, value);
        }

        public IReadOnlyList<PhaseOnePartTwoAnalysisKind> AvailableAnalysisKindForPhaseOnePartTwo { get; }

        private PhaseOnePartTwoAnalysisKind? _selectedPhaseOnePartTwo;
        public PhaseOnePartTwoAnalysisKind? SelectedPhaseOnePartTwo
        {
            get => _selectedPhaseOnePartTwo;
            set => SetProperty(ref _selectedPhaseOnePartTwo, value);
        }

        public IReadOnlyList<PhaseTwoAnalysisKind> AvailableAnalysisKindForPhaseTwo { get; }

        private PhaseTwoAnalysisKind? _selectedPhaseTwo;
        public PhaseTwoAnalysisKind? SelectedPhaseTwo
        {
            get => _selectedPhaseTwo;
            set => SetProperty(ref _selectedPhaseTwo, value);
        }

        public IReadOnlyList<GoodnessOfFitKind> AvailableGoodnessOfFitKinds { get; }

        private GoodnessOfFitKind? _selectedGoodnessOfFitKind;
        public GoodnessOfFitKind? SelectedGoodnessOfFitKind
        {
            get => _selectedGoodnessOfFitKind;
            set => SetProperty(ref _selectedGoodnessOfFitKind, value);
        }


        public SelectiveParametersModel()
        {
            AvailableAlgorithms = DesktopOptions.AvailableAlgorithms;
            AvailableAnalysisKindForPhaseOnePartOne = DesktopOptions.AvailableAnalysisKindForPhaseOnePartOne;
            AvailableAnalysisKindForPhaseOnePartTwo = DesktopOptions.AvailableAnalysisKindForPhaseOnePartTwo;
            AvailableAnalysisKindForPhaseTwo = DesktopOptions.AvailableAnalysisKindForPhaseTwo;
            AvailableGoodnessOfFitKinds = DesktopOptions.AvailableGoodnessOfFitKinds;

            Reset();
        }

        #region IResetable Implementation

        public void Reset()
        {
            SelectedAlgorithmType = AvailableAlgorithms.FirstOrDefault();

            SelectedPhaseOnePartOne = AvailableAnalysisKindForPhaseOnePartOne.FirstOrDefault();

            SelectedPhaseOnePartTwo = AvailableAnalysisKindForPhaseOnePartTwo.FirstOrDefault();

            // TODO: use first value instead of last when implement normal distribution.
            SelectedPhaseTwo = AvailableAnalysisKindForPhaseTwo.LastOrDefault();

            SelectedGoodnessOfFitKind = AvailableGoodnessOfFitKinds.FirstOrDefault();
        }

        #endregion

        public void VerifyParameters()
        {
            Assert(SelectedAlgorithmType, nameof(AvailableAlgorithms));
            Assert(SelectedPhaseOnePartOne, nameof(AvailableAnalysisKindForPhaseOnePartOne));
            Assert(SelectedPhaseOnePartTwo, nameof(AvailableAnalysisKindForPhaseOnePartTwo));
            Assert(SelectedPhaseTwo, nameof(AvailableAnalysisKindForPhaseTwo));
            Assert(SelectedGoodnessOfFitKind, nameof(AvailableGoodnessOfFitKinds));
        }

        private static void Assert<T>(T valueToCheck, string collectionName)
        {
            if (valueToCheck is null)
            {
                string message =
                    "Failed to retrive value from collection. " +
                    $"Collection: {collectionName}";
                throw new InvalidOperationException(message);
            }
        }
    }
}
