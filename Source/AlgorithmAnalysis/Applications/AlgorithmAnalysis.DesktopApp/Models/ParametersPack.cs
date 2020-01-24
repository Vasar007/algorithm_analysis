using Acolyte.Assertions;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class ParametersPack
    {
        public int AlgorythmType { get; }

        public int StartValue { get; }

        public int EndValue { get; }

        public int LaunchesNumber { get; }

        public int Step { get; }


        public ParametersPack(
            int algorythmType,
            int startValue,
            int endValue,
            int launchesNumber,
            int step)
        {
            AlgorythmType = algorythmType.ThrowIfValueIsOutOfRange(nameof(algorythmType), 0, 1);
            StartValue = startValue.ThrowIfValueIsOutOfRange(nameof(algorythmType), 1, int.MaxValue);
            EndValue = endValue.ThrowIfValueIsOutOfRange(nameof(algorythmType), StartValue, int.MaxValue);
            LaunchesNumber = launchesNumber.ThrowIfValueIsOutOfRange(nameof(algorythmType), 1, int.MaxValue);
            Step = step.ThrowIfValueIsOutOfRange(nameof(algorythmType), 1, int.MaxValue);
        }

        public string GetPack()
        {
            return $"{AlgorythmType.ToString()} {StartValue.ToString()} {EndValue.ToString()} " +
                   $"{LaunchesNumber.ToString()} {Step.ToString()}";
        }
    }
}
