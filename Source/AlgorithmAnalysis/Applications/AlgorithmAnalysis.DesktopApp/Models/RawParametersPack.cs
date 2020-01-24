using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.DesktopApp.Domain;
using Prism.Mvvm;

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
                algorythmType: TransformAlgorithmValue(AlgorythmType),
                startValue: int.Parse(StartValue),
                endValue: int.Parse(EndValue),
                launchesNumber: int.Parse(LaunchesNumber),
                step: int.Parse(Step)
            );
        }

        private static int TransformAlgorithmValue(string value)
        {
            return value switch
            {
                "A1" => 0,

                "A2" => 1,

                _ => throw new ArgumentOutOfRangeException(
                         nameof(value), value,$"Unknown value to transform: '{value}'."
                     )
            };
        }
    }
}
