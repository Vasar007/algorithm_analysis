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
    }
}
