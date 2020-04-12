using Acolyte.Assertions;
using Prism.Mvvm;
using AlgorithmAnalysis.Models;
using AlgorithmAnalysis.DesktopApp.Domain;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class AlgorithmTypeValueModel : BindableBase
    {
        private string _description;
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

        private string _minFormula;
        public string MinFormula
        {
            get => _minFormula;
            set => SetProperty(ref _minFormula, value.ThrowIfNull(nameof(value)));
        }

        private string _averageFormula;
        public string AverageFormula
        {
            get => _averageFormula;
            set => SetProperty(ref _averageFormula, value.ThrowIfNull(nameof(value)));
        }

        private string _maxFormula;
        public string MaxFormula
        {
            get => _maxFormula;
            set => SetProperty(ref _maxFormula, value.ThrowIfNull(nameof(value)));
        }

        private string _analysisProgramName;
        public string AnalysisProgramName
        {
            get => _analysisProgramName;
            set => SetProperty(ref _analysisProgramName, value.ThrowIfNull(nameof(value)));
        }

        private readonly string _originalOutputFilenamePattern;

        private string _relativeOutputFilenamePattern;
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
            string relativeOutputFilenamePattern,
            string originalOutputFilenamePattern)
        {
            _description = description.ThrowIfNull(nameof(description));
            _index = index.ThrowIfValueIsOutOfRange(nameof(index), 1, int.MaxValue);
            _minFormula = minFormula.ThrowIfNull(nameof(minFormula));
            _averageFormula = averageFormula.ThrowIfNull(nameof(averageFormula));
            _maxFormula = maxFormula.ThrowIfNull(nameof(maxFormula));
            _analysisProgramName = analysisProgramName.ThrowIfNull(nameof(analysisProgramName));
            _relativeOutputFilenamePattern =
                relativeOutputFilenamePattern.ThrowIfNull(nameof(relativeOutputFilenamePattern));
            _originalOutputFilenamePattern =
                originalOutputFilenamePattern.ThrowIfNull(nameof(originalOutputFilenamePattern));
        }

        public static AlgorithmTypeValueModel Create(
            AlgorithmTypeValue algorithmTypeValue, int index)
        {
            algorithmTypeValue.ThrowIfNull(nameof(algorithmTypeValue));

            string relativeOutputFilenamePattern = ModelPathTransformer.TransformPathToRelative(
                algorithmTypeValue.OutputFilenamePattern
            );

            return new AlgorithmTypeValueModel(
                description: algorithmTypeValue.Description,
                index: index + 1, // Start indexing for UI with 1 instead of 0.
                minFormula: algorithmTypeValue.MinFormula,
                averageFormula: algorithmTypeValue.AverageFormula,
                maxFormula: algorithmTypeValue.MaxFormula,
                analysisProgramName: algorithmTypeValue.AnalysisProgramName,
                relativeOutputFilenamePattern: relativeOutputFilenamePattern,
                originalOutputFilenamePattern: algorithmTypeValue.OutputFilenamePattern
            );
        }

        public AlgorithmTypeValue Convert()
        {
            string newOutputFilenamePattern = ModelPathTransformer.TransformPathToOriginal(
                _originalOutputFilenamePattern, RelativeOutputFilenamePattern
            );

            return new AlgorithmTypeValue
            {
                Description = Description,
                MinFormula = MinFormula,
                AverageFormula = AverageFormula,
                MaxFormula = MaxFormula,
                AnalysisProgramName = AnalysisProgramName,
                OutputFilenamePattern = newOutputFilenamePattern
            };
        }
    }
}
