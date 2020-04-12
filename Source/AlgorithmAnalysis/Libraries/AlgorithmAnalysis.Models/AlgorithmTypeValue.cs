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
            return new AlgorithmTypeValue
            {
                Description = algorithmValue.Description,
                Value = algorithmValue.Value,
                //MinFormulaFormat = TransformRawFormulaToFormulaFormat(algorithmValue.MinFormula),
                //AverageFormulaFormat = TransformRawFormulaToFormulaFormat(algorithmValue.AverageFormula),
                //MaxFormulaFormat = TransformRawFormulaToFormulaFormat(algorithmValue.MaxFormula),
                AnalysisProgramName = algorithmValue.AnalysisProgramName,
                OutputFilenamePattern = algorithmValue.OutputFilenamePattern
            };
        }
    }
}
