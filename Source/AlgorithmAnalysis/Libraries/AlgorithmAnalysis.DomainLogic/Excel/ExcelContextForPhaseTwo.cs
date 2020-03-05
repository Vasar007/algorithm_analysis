using Acolyte.Assertions;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal sealed class ExcelContextForPhaseTwo
    {
        public ParametersPack Args { get; }


        public ExcelContextForPhaseTwo(
            ParametersPack args)
        {
            Args = args.ThrowIfNull(nameof(args));
        }
    }
}
