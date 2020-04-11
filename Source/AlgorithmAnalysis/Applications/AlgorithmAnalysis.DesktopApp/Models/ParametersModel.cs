using System.IO;
using Acolyte.Assertions;
using Prism.Mvvm;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DomainLogic;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class ParametersModel : BindableBase, IChangeable
    {
        public ParametersAlgorithmModel Algorithm { get; }

        public ParametersAnalysisModel Analysis { get; }

        public ParametersAdvancedModel Advanced { get; }


        public ParametersModel()
        {
            Algorithm = new ParametersAlgorithmModel();
            Analysis = new ParametersAnalysisModel();
            Advanced = new ParametersAdvancedModel();

            // Internal model should call Reset method in ctors themself.
        }

        #region IChangeableModel Implementation

        public void Reset()
        {
            Algorithm.Reset();
            Analysis.Reset();
            Advanced.Reset();
        }

        public void Validate()
        {
            Algorithm.Validate();
            Analysis.Validate();
            Advanced.Validate();
        }

        #endregion

        public AnalysisContext CreateContext(FileInfo outputExcelFile)
        {
            outputExcelFile.ThrowIfNull(nameof(outputExcelFile));

            Validate();

            return new AnalysisContext(
                args: ConvertArgs(),
                launchContext: CreateLaunchContext(),
                outputExcelFile: outputExcelFile,
                phaseOnePartOne: Analysis.SelectedPhaseOnePartOne!,
                phaseOnePartTwo: Analysis.SelectedPhaseOnePartTwo!,
                phaseTwo: Analysis.SelectedPhaseTwo!,
                goodnessOfFit: Analysis.SelectedGoodnessOfFitKind!
            );
        }

        private ParametersPack ConvertArgs()
        {
            return ParametersPack.Create(
                analysisOptions: ConfigOptions.Analysis,
                algorithmType: Algorithm.SelectedAlgorithmType!,
                startValue: int.Parse(Algorithm.StartValue),
                endValue: int.Parse(Algorithm.EndValue),
                extrapolationSegmentValue: int.Parse(Algorithm.ExtrapolationSegmentValue),
                launchesNumber: int.Parse(Algorithm.LaunchesNumber),
                step: int.Parse(Algorithm.Step)
            );
        }

        private AnalysisLaunchContext CreateLaunchContext()
        {
            return new AnalysisLaunchContext(
                showAnalysisWindow: Advanced.ShowAnalysisWindow,
                maxDegreeOfParallelism: Analysis.SelectedMaxDegreeOfParallelism
            );
        }
    }
}
