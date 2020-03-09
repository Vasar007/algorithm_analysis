using Acolyte.Assertions;
using Prism.Mvvm;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DomainLogic;
using AlgorithmAnalysis.Models;
using System.IO;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class RawParametersPack : BindableBase
    {
        // Initializes through Reset method in ctor.
        private PhaseOnePartOneAnalysisKind _selectedPhaseOnePartOne = default!;
        public PhaseOnePartOneAnalysisKind SelectedPhaseOnePartOne
        {
            get => _selectedPhaseOnePartOne;
            set => SetProperty(ref _selectedPhaseOnePartOne, value.ThrowIfNull(nameof(value)));
        }

        // Initializes through Reset method in ctor.
        private PhaseOnePartTwoAnalysisKind _selectedPhaseOnePartTwo = default!;
        public PhaseOnePartTwoAnalysisKind SelectedPhaseOnePartTwo
        {
            get => _selectedPhaseOnePartTwo;
            set => SetProperty(ref _selectedPhaseOnePartTwo, value.ThrowIfNull(nameof(value)));
        }

        // Initializes through Reset method in ctor.
        private PhaseTwoAnalysisKind _selectedPhaseTwo = default!;
        public PhaseTwoAnalysisKind SelectedPhaseTwo
        {
            get => _selectedPhaseTwo;
            set => SetProperty(ref _selectedPhaseTwo, value.ThrowIfNull(nameof(value)));
        }

        // Initializes through Reset method in ctor.
        private AlgorithmType _selectedAlgorithmType = default!;
        public AlgorithmType SelectedAlgorithmType
        {
            get => _selectedAlgorithmType;
            set => SetProperty(ref _selectedAlgorithmType, value.ThrowIfNull(nameof(value)));
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


        public RawParametersPack()
        {
            Reset();
        }

        public void Reset()
        {
            SelectedPhaseOnePartOne = DesktopOptions.AvailableAnalysisKindForPhaseOnePartOne[0];
            SelectedPhaseOnePartTwo = DesktopOptions.AvailableAnalysisKindForPhaseOnePartTwo[0];
            SelectedAlgorithmType = DesktopOptions.AvailableAlgorithms[0];
            StartValue = "80";
            EndValue = "320";
            ExtrapolationSegmentValue = "2560";
            LaunchesNumber = "200";
            Step = "10";
            ShowAnalysisWindow = false;
        }

        public AnalysisContext CreateContext(FileInfo outputExcelFile)
        {
            return new AnalysisContext(
                args: ConvertArgs(),
                showAnalysisWindow: ShowAnalysisWindow,
                outputExcelFile: outputExcelFile,
                phaseOnePartOne: SelectedPhaseOnePartOne,
                phaseOnePartTwo: SelectedPhaseOnePartTwo,
                phaseTwo: SelectedPhaseTwo
            );
        }

        private ParametersPack ConvertArgs()
        {
            return ParametersPack.Create(
                analysisOptions: ConfigOptions.Analysis,
                algorithmType: SelectedAlgorithmType,
                startValue: int.Parse(StartValue),
                endValue: int.Parse(EndValue),
                extrapolationSegmentValue: int.Parse(ExtrapolationSegmentValue),
                launchesNumber: int.Parse(LaunchesNumber),
                step: int.Parse(Step)
            );
        }
    }
}
