using AlgorithmAnalysis.Common.Parsing;

namespace AlgorithmAnalysis.Models
{
    public sealed class AlgorithmTypeValue
    {
        public string Description { get; set; } = string.Empty;

        public int Value { get; set; } = default;

        public string MinFormula { get; set; } = string.Empty;

        public string AverageFormula { get; set; } = string.Empty;

        public string MaxFormula { get; set; } = string.Empty;

        public string AnalysisProgramName { get; set; } = string.Empty;

        public string OutputFilenamePattern { get; set; } = string.Empty;


        public AlgorithmTypeValue()
        {
        }

        public static AlgorithmTypeValue Create(AlgorithmType algorithmValue)
        {
            string rawMinFormula = ParsingManager.TransformFormulaFormatToRawFormula(
                algorithmValue.MinFormulaFormat
            );

            string rawAverageFormula = ParsingManager.TransformFormulaFormatToRawFormula(
                algorithmValue.AverageFormulaFormat
            );

            string rawMaxFormula = ParsingManager.TransformFormulaFormatToRawFormula(
                algorithmValue.MaxFormulaFormat
            );

            return new AlgorithmTypeValue
            {
                Description = algorithmValue.Description,
                Value = algorithmValue.Value,
                MinFormula = rawMinFormula,
                AverageFormula = rawAverageFormula,
                MaxFormula = rawMaxFormula,
                AnalysisProgramName = algorithmValue.AnalysisProgramName,
                OutputFilenamePattern = algorithmValue.OutputFilenamePattern
            };
        }
    }
}
