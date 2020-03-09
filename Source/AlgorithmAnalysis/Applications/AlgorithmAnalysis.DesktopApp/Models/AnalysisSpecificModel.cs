using System.Collections.Generic;
using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DomainLogic;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class AnalysisSpecificModel : BindableBase, IResetable
    {
        public int MinDegreeOfParallerism { get; }

        public int MaxDegreeOfParallerism { get; }

        public IReadOnlyList<PhaseOnePartOneAnalysisKind> AvailableAnalysisKindForPhaseOnePartOne { get; }

        public IReadOnlyList<PhaseOnePartTwoAnalysisKind> AvailableAnalysisKindForPhaseOnePartTwo { get; }

        public IReadOnlyList<PhaseTwoAnalysisKind> AvailableAnalysisKindForPhaseTwo { get; }

        public IReadOnlyList<AlgorithmType> AvailableAlgorithms { get; }

        private bool _openAnalysisResults;
        public bool OpenAnalysisResults
        {
            get => _openAnalysisResults;
            set => SetProperty(ref _openAnalysisResults, value);
        }


        public AnalysisSpecificModel()
        {
            MinDegreeOfParallerism = DesktopOptions.MinDegreeOfParallerism;
            MaxDegreeOfParallerism = DesktopOptions.MaxDegreeOfParallerism;

            AvailableAnalysisKindForPhaseOnePartOne = DesktopOptions.AvailableAnalysisKindForPhaseOnePartOne;
            AvailableAnalysisKindForPhaseOnePartTwo = DesktopOptions.AvailableAnalysisKindForPhaseOnePartTwo;
            AvailableAnalysisKindForPhaseTwo = DesktopOptions.AvailableAnalysisKindForPhaseTwo;
            AvailableAlgorithms = DesktopOptions.AvailableAlgorithms;

            Reset();
        }

        #region IResetable Implementation

        public void Reset()
        {
            OpenAnalysisResults = false;
        }

        #endregion
    }
}
