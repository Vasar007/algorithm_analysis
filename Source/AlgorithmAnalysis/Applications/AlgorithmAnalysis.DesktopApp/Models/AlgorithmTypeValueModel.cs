using Acolyte.Assertions;
using Prism.Mvvm;
using AlgorithmAnalysis.Models;
using AlgorithmAnalysis.Common.Files;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class AlgorithmTypeValueModel : BindableBase
    {
        private string _description = default!;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value.ThrowIfNull(nameof(value)));
        }

        private int _index;
        public int Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }

        private string _minFormula = default!;
        public string MinFormula
        {
            get => _minFormula;
            set => SetProperty(ref _minFormula, value.ThrowIfNull(nameof(value)));
        }

        private string _averageFormula = default!;
        public string AverageFormula
        {
            get => _averageFormula;
            set => SetProperty(ref _averageFormula, value.ThrowIfNull(nameof(value)));
        }

        private string _maxFormula = default!;
        public string MaxFormula
        {
            get => _maxFormula;
            set => SetProperty(ref _maxFormula, value.ThrowIfNull(nameof(value)));
        }

        private string _analysisProgramName = default!;
        public string AnalysisProgramName
        {
            get => _analysisProgramName;
            set => SetProperty(ref _analysisProgramName, value.ThrowIfNull(nameof(value)));
        }

        private string _relativeOutputFilenamePattern = default!;
        public string RelativeOutputFilenamePattern
        {
            get => _relativeOutputFilenamePattern;
            set => SetProperty(ref _relativeOutputFilenamePattern, value.ThrowIfNull(nameof(value)));
        }


        public AlgorithmTypeValueModel(
            string description,
            int index,
            string minFormula,
            string averageFormula,
            string maxFormula,
            string analysisProgramName,
            string ralativeOutputFilenamePattern)
        {
            Description = description.ThrowIfNull(nameof(description));
            Index = index.ThrowIfValueIsOutOfRange(nameof(index), 1, int.MaxValue);
            MinFormula = minFormula.ThrowIfNull(nameof(minFormula));
            AverageFormula = averageFormula.ThrowIfNull(nameof(averageFormula));
            MaxFormula = maxFormula.ThrowIfNull(nameof(maxFormula));
            AnalysisProgramName = analysisProgramName.ThrowIfNull(nameof(analysisProgramName));
            RelativeOutputFilenamePattern =
                ralativeOutputFilenamePattern.ThrowIfNull(nameof(ralativeOutputFilenamePattern));
        }

        public static AlgorithmTypeValueModel Create(
            AlgorithmTypeValue algorithmTypeValue, int index)
        {
            algorithmTypeValue.ThrowIfNull(nameof(algorithmTypeValue));

            string ralativeOutputFilenamePattern = PathHelper.ResolveRelativePath(
                algorithmTypeValue.OutputFilenamePattern
            );

            return new AlgorithmTypeValueModel(
                description: algorithmTypeValue.Description,
                index: index + 1, // Start indexing for UI with 1 instead of 0.
                minFormula: algorithmTypeValue.MinFormula,
                averageFormula: algorithmTypeValue.AverageFormula,
                maxFormula: algorithmTypeValue.MaxFormula,
                analysisProgramName: algorithmTypeValue.AnalysisProgramName,
                ralativeOutputFilenamePattern: ralativeOutputFilenamePattern
            );
        }

        public AlgorithmTypeValue Convert()
        {
            return new AlgorithmTypeValue
            {
                Description = Description,
                MinFormula = MinFormula,
                AverageFormula = AverageFormula,
                MaxFormula = MaxFormula,
                AnalysisProgramName = AnalysisProgramName,
                OutputFilenamePattern = RelativeOutputFilenamePattern
            };
        }
    }
}
