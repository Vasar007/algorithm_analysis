using System;
using System.Linq;
using Acolyte.Assertions;
using Acolyte.Common;
using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DomainLogic;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class RawParametersPack : BindableBase
    {
        // Initializes through Reset method in ctor.
        private string _analysisKind = default!;
        public string AnalysisKind
        {
            get => _analysisKind;
            set => SetProperty(ref _analysisKind, value.ThrowIfNull(nameof(value)));
        }

        // Initializes through Reset method in ctor.
        private string _algorythmType = default!;
        public string AlgorythmType
        {
            get => _algorythmType;
            set => SetProperty(ref _algorythmType, value.ThrowIfNull(nameof(value)));
        }

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

        private bool _showAnalysisWindow;
        public bool ShowAnalysisWindow
        {
            get => _showAnalysisWindow;
            set => SetProperty(ref _showAnalysisWindow, value);
        }


        public RawParametersPack()
        {
            Reset();
        }

        public void Reset()
        {
            AnalysisKind = DesktopOptions.AvailableAnalysisKindForPhaseOne[0];
            AlgorythmType = DesktopOptions.AvailableAlgorithms[0];
            StartValue = "80";
            EndValue = "80";
            LaunchesNumber = "200";
            Step = "10";
            ShowAnalysisWindow = false;
        }

        public AnalysisContext CreateContext()
        {
            return new AnalysisContext(
                args: ConvertArgs(),
                analysisKind: GetAnalysisKind(),
                showAnalysisWindow: ShowAnalysisWindow
            );
        }

        private ParametersPack ConvertArgs()
        {
            return new ParametersPack(
                analysisProgramName: DesktopOptions.DefaultAnalysisProgramName,
                algorythmType: TransformAlgorithmValue(AlgorythmType),
                startValue: int.Parse(StartValue),
                endValue: int.Parse(EndValue),
                launchesNumber: int.Parse(LaunchesNumber),
                step: int.Parse(Step),
                outputFilenamePattern: DesktopOptions.DefaultOutputFilenamePattern
            );
        }

        private PhaseOnePartOneAnalysisKind GetAnalysisKind()
        {
            PhaseOnePartOneAnalysisKind analysisKind = EnumHelper.GetValues<PhaseOnePartOneAnalysisKind>()
                .Select(enumValue => (enumValue: enumValue, description: enumValue.GetDescription()))
                .First(pair => StringComparer.OrdinalIgnoreCase.Equals(pair.description, AnalysisKind))
                .enumValue;

            return analysisKind;
        }

        private static int TransformAlgorithmValue(string value)
        {
            value.ThrowIfNullOrEmpty(nameof(value));

            for (int i = 0; i < DesktopOptions.AvailableAlgorithms.Count; ++i)
            {
                bool equality = StringComparer.OrdinalIgnoreCase.Equals(
                    value,
                    DesktopOptions.AvailableAlgorithms[i]
                );

                if (equality) return i;
            }

            throw new ArgumentOutOfRangeException(
                nameof(value), value, $"Unknown value to transform: '{value}'."
            );
        }
    }
}
