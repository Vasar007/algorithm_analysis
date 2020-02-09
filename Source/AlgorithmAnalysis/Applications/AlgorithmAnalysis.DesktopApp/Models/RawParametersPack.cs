using Acolyte.Assertions;
using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DomainLogic;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class RawParametersPack : BindableBase
    {
        // Initializes through Reset method in ctor.
        private string _selectedPhaseOnePartOne = default!;
        public string SelectedPhaseOnePartOne
        {
            get => _selectedPhaseOnePartOne;
            set => SetProperty(ref _selectedPhaseOnePartOne, value.ThrowIfNull(nameof(value)));
        }

        // Initializes through Reset method in ctor.
        private string _selectedPhaseOnePartTwo = default!;
        public string SelectedPhaseOnePartTwo
        {
            get => _selectedPhaseOnePartTwo;
            set => SetProperty(ref _selectedPhaseOnePartTwo, value.ThrowIfNull(nameof(value)));
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
            SelectedPhaseOnePartOne = DesktopOptions.AvailableAnalysisKindForPhaseOne[0];
            SelectedPhaseOnePartTwo = DesktopOptions.AvailableAnalysisKindForPhaseTwo[0];
            SelectedAlgorithmType = DesktopOptions.AvailableAlgorithms[0];
            StartValue = "80";
            EndValue = "320";
            LaunchesNumber = "200";
            Step = "10";
            ShowAnalysisWindow = false;
        }

        public AnalysisContext CreateContext()
        {
            var phaseOnePartOne = AnalysisHelper
                .GetEnumValueByDescription<PhaseOnePartOneAnalysisKind>(SelectedPhaseOnePartOne);
            var phaseOnePartTwo = AnalysisHelper
                .GetEnumValueByDescription<PhaseOnePartTwoAnalysisKind>(SelectedPhaseOnePartTwo);

            return new AnalysisContext(
                args: ConvertArgs(),
                showAnalysisWindow: ShowAnalysisWindow,
                phaseOnePartOne: phaseOnePartOne,
                phaseOnePartTwo: phaseOnePartTwo
            );
        }

        private ParametersPack ConvertArgs()
        {
            return new ParametersPack(
                analysisProgramName: DesktopOptions.DefaultAnalysisProgramName,
                algorithmType: SelectedAlgorithmType,
                startValue: int.Parse(StartValue),
                endValue: int.Parse(EndValue),
                launchesNumber: int.Parse(LaunchesNumber),
                step: int.Parse(Step),
                outputFilenamePattern: DesktopOptions.DefaultOutputFilenamePattern
            );
        }
    }
}
