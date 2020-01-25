using System;
using Acolyte.Assertions;
using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DomainLogic;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class RawParametersPack : BindableBase
    {
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


        public RawParametersPack()
        {
            Reset();
        }

        public void Reset()
        {
            AlgorythmType = DesktopOptions.AvailableAlgorithms[0];
            StartValue = "80";
            EndValue = "80";
            LaunchesNumber = "200";
            Step = "10";
        }

        public ParametersPack Convert()
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
