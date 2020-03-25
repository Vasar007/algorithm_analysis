using System;
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

        public int MinDegreeOfParallelism { get; }

        public int MaxDegreeOfParallelism { get; }

        public bool IsDegreeOfParallelismSelectable =>
            MinDegreeOfParallelism != MaxDegreeOfParallelism;

        /// <summary>
        /// Shows warning about restriction to select degree of parallelism when PC has only one
        /// CPU. May be later we change minimum value of parallelism degree.
        /// </summary>
        public bool IsHintForDegreeOfParallelismVisible =>
            !IsDegreeOfParallelismSelectable &&
            MinDegreeOfParallelism == DesktopOptions.MinDegreeOfParallelism;

        private int _selectedMaxDegreeOfParallelism;
        public int SelectedMaxDegreeOfParallelism
        {
            get => _selectedMaxDegreeOfParallelism;
            set => SetProperty(ref _selectedMaxDegreeOfParallelism, value);
        }


        public RawParametersPack()
        {
            MinDegreeOfParallelism = DesktopOptions.MinDegreeOfParallelism;
            MaxDegreeOfParallelism = DesktopOptions.MaxDegreeOfParallelism;

            Reset();

            VerifyParameters();
        }

        private void VerifyParameters()
        {
            if (MinDegreeOfParallelism <= 0)
            {
                string message =
                    "Invalid min values for degree of parallelism: " +
                    $"{MinDegreeOfParallelism.ToString()} <= 0.";
                throw new ApplicationException(message);
            }

            if (MaxDegreeOfParallelism <= 0)
            {
                string message =
                    "Invalid max values for degree of parallelism: " +
                    $"{MaxDegreeOfParallelism.ToString()} <= 0.";
                throw new ApplicationException(message);
            }

            if (MinDegreeOfParallelism > MaxDegreeOfParallelism)
            {
                string message =
                    "Invalid min and max values for degree of parallelism: " +
                    $"{MinDegreeOfParallelism.ToString()} > {MaxDegreeOfParallelism.ToString()}.";
                throw new ApplicationException(message);
            }
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
            SelectedMaxDegreeOfParallelism = MinDegreeOfParallelism;
        }

        #endregion

        public AnalysisContext CreateContext(FileInfo outputExcelFile,
            SelectiveParametersModel selectiveParameters)
        {
            outputExcelFile.ThrowIfNull(nameof(outputExcelFile));
            selectiveParameters.ThrowIfNull(nameof(selectiveParameters));

            selectiveParameters.VerifyParameters();

            return new AnalysisContext(
                args: ConvertArgs(selectiveParameters),
                launchContext: CreateLaunchContext(),
                outputExcelFile: outputExcelFile,
                phaseOnePartOne: selectiveParameters.SelectedPhaseOnePartOne!,
                phaseOnePartTwo: selectiveParameters.SelectedPhaseOnePartTwo!,
                phaseTwo: selectiveParameters.SelectedPhaseTwo!,
                goodnessOfFit: selectiveParameters.SelectedGoodnessOfFitKind!
            );
        }

        private ParametersPack ConvertArgs(SelectiveParametersModel selectiveParameters)
        {
            return ParametersPack.Create(
                analysisOptions: ConfigOptions.Analysis,
                algorithmType: selectiveParameters.SelectedAlgorithmType!,
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
                maxDegreeOfParallelism: SelectedMaxDegreeOfParallelism
            );
        }
    }
}
