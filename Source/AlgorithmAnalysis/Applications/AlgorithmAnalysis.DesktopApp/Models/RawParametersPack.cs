using System.IO;
using Acolyte.Assertions;
using Prism.Mvvm;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DomainLogic;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class RawParametersPack : BindableBase, IResetable
    {
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
        private string _extrapolationSegmentValue = default!;
        public string ExtrapolationSegmentValue
        {
            get => _extrapolationSegmentValue;
            set => SetProperty(ref _extrapolationSegmentValue, value.ThrowIfNull(nameof(value)));
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

        public int MinDegreeOfParallerism { get; }

        public int MaxDegreeOfParallerism { get; }

        private int _maxDegreeOfParallelism;
        public int MaxDegreeOfParallelism
        {
            get => _maxDegreeOfParallelism;
            set => SetProperty(ref _maxDegreeOfParallelism, value);
        }


        public RawParametersPack()
        {
            MinDegreeOfParallerism = DesktopOptions.MinDegreeOfParallerism;
            MaxDegreeOfParallerism = DesktopOptions.MaxDegreeOfParallerism;

            Reset();
        }

        #region IResetable Implementation

        public void Reset()
        {
            StartValue = "80";
            EndValue = "320";
            ExtrapolationSegmentValue = "2560";
            LaunchesNumber = "200";
            Step = "10";
            ShowAnalysisWindow = false;
            MaxDegreeOfParallelism = 1;
        }

        #endregion

        public AnalysisContext CreateContext(FileInfo outputExcelFile,
            SelectiveParametersModel selectiveParameters)
        {
            outputExcelFile.ThrowIfNull(nameof(outputExcelFile));
            selectiveParameters.ThrowIfNull(nameof(selectiveParameters));

            return new AnalysisContext(
                args: ConvertArgs(selectiveParameters),
                launchContext: CreateLaunchContext(),
                outputExcelFile: outputExcelFile,
                phaseOnePartOne: selectiveParameters.SelectedPhaseOnePartOne,
                phaseOnePartTwo: selectiveParameters.SelectedPhaseOnePartTwo,
                phaseTwo: selectiveParameters.SelectedPhaseTwo,
                goodnessOfFit: selectiveParameters.SelectedGoodnessOfFitKind
            );
        }

        private ParametersPack ConvertArgs(SelectiveParametersModel selectiveParameters)
        {
            return ParametersPack.Create(
                analysisOptions: ConfigOptions.Analysis,
                algorithmType: selectiveParameters.SelectedAlgorithmType,
                startValue: int.Parse(StartValue),
                endValue: int.Parse(EndValue),
                extrapolationSegmentValue: int.Parse(ExtrapolationSegmentValue),
                launchesNumber: int.Parse(LaunchesNumber),
                step: int.Parse(Step)
            );
        }

        private AnalysisLaunchContext CreateLaunchContext()
        {
            return new AnalysisLaunchContext(
                showAnalysisWindow: ShowAnalysisWindow,
                maxDegreeOfParallelism: MaxDegreeOfParallelism
            );
        }
    }
}
