using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Properties;
using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo
{
    internal sealed class ScottFrequencyHistogramBuilder : IFrequencyHistogramBuilder
    {
        private readonly ParametersPack _args;


        public ScottFrequencyHistogramBuilder(ParametersPack args)
        {
            _args = args.ThrowIfNull(nameof(args));
        }

        #region IFrequencyHistogramBuilder Implementation

        public void CreateHistogramData(IExcelSheet sheet)
        {
            sheet[ExcelColumnIndex.D, 1].SetValue(ExcelStringsPhaseOnePartTwo.PocketColumnName);
            sheet[ExcelColumnIndex.E, 1].SetValue(ExcelStringsPhaseOnePartTwo.FrequencyColumnName);
            sheet[ExcelColumnIndex.F, 1].SetValue(ExcelStringsPhaseOnePartTwo.EmpiricalFrequencyColumnName);
            sheet[ExcelColumnIndex.G, 1].SetValue(ExcelStringsPhaseOnePartTwo.TheoreticalFrequencyColumnName);
            sheet[ExcelColumnIndex.H, 1].SetValue(ExcelStringsPhaseOnePartTwo.Chi2ValueColumnName);

            sheet[ExcelColumnIndex.I, 10].SetValue(ExcelStringsPhaseOnePartTwo.MinimumValue);
            sheet[ExcelColumnIndex.I, 11].SetValue(ExcelStringsPhaseOnePartTwo.MaximumValue);
            sheet[ExcelColumnIndex.I, 12].SetValue(ExcelStringsPhaseOnePartTwo.IntervalLength);
            sheet[ExcelColumnIndex.I, 13].SetValue(ExcelStringsPhaseOnePartTwo.HistogramSemisegmentsNumber);
            sheet[ExcelColumnIndex.I, 14].SetValue(ExcelStringsPhaseOnePartTwo.Chi2Observable);
            sheet[ExcelColumnIndex.I, 15].SetValue(ExcelStringsPhaseOnePartTwo.FreedomDegreesNumber);
            sheet[ExcelColumnIndex.I, 16].SetValue(ExcelStringsPhaseOnePartTwo.Chi2Critical);
            sheet[ExcelColumnIndex.I, 17].SetValue(ExcelStringsPhaseOnePartTwo.CheckTestFucntion);

            string lastValueRowIndex = _args.LaunchesNumber.SkipHeader().ToString();
            string normalizedValueRange = $"$B$2:$B${lastValueRowIndex}";
            sheet[ExcelColumnIndex.J, 10].SetFormula(sheet.FormulaProvider.Min(normalizedValueRange));
            sheet[ExcelColumnIndex.J, 11].SetFormula(sheet.FormulaProvider.Max(normalizedValueRange));

            string scottFormula = $"(3.5 * {sheet.FormulaProvider.StdDev(normalizedValueRange)}) / (J6^(1/3))";
            sheet[ExcelColumnIndex.J, 12].SetFormula(scottFormula);
            sheet[ExcelColumnIndex.J, 13].SetFormula(
                sheet.FormulaProvider.RoundUp("($J$11 - $J$10) / $J$12", "0")
            );

            IExcelCellValueHolder histogramSegmentsNumber = sheet.EvaluateCell(ExcelColumnIndex.J, 13);
            int histogramSegmentsNumberInt = Convert.ToInt32(histogramSegmentsNumber.NumericValue);
            string histogramSegmentsNumberIndex = histogramSegmentsNumberInt.UseOneBasedIndexing().SkipHeader().ToString();

            CreateIntervalData(
                sheet,
                histogramSegmentsNumberInt, histogramSegmentsNumberIndex,
                normalizedValueRange
            );

            sheet[ExcelColumnIndex.J, 14].SetFormula(
                $"{sheet.FormulaProvider.Sum($"$H$2:$H${histogramSegmentsNumberIndex}")} * $J$6"
            );
            sheet[ExcelColumnIndex.J, 15].SetFormula("$J$13 - 1 - 2");
            sheet[ExcelColumnIndex.J, 16].SetFormula(sheet.FormulaProvider.ChiInv("$J$8", "$J$15"));

            string testFormula = sheet.FormulaProvider.ChiTest(
                $"$F$2:$F${histogramSegmentsNumberIndex}",
                $"$G$2:$G${histogramSegmentsNumberIndex}"
            );
            sheet[ExcelColumnIndex.J, 17].SetFormula(testFormula);

            sheet.EvaluateAll();

            sheet.AutoSizeColumn(ExcelColumnIndex.D);
            sheet.AutoSizeColumn(ExcelColumnIndex.E);
            sheet.AutoSizeColumn(ExcelColumnIndex.F);
            sheet.AutoSizeColumn(ExcelColumnIndex.G);
            sheet.AutoSizeColumn(ExcelColumnIndex.H);
            sheet.AutoSizeColumn(ExcelColumnIndex.I);
            sheet.AutoSizeColumn(ExcelColumnIndex.J);
        }

        public bool CheckH0HypothesisByHistogramData(IExcelSheet sheet)
        {
            IExcelCellValueHolder chi2Observable = sheet.EvaluateCell(ExcelColumnIndex.J, 14);
            IExcelCellValueHolder chi2Critical = sheet.EvaluateCell(ExcelColumnIndex.J, 16);

            return chi2Observable.NumericValue < chi2Critical.NumericValue;
        }

        #endregion

        private void CreateIntervalData(IExcelSheet sheet, int histogramSegmentsNumber,
            string histogramSegmentsNumberIndex, string normalizedValueRange)
        {
            histogramSegmentsNumber.ThrowIfValueIsOutOfRange(nameof(histogramSegmentsNumber), 1, int.MaxValue);

            for (int i = 0; i <= histogramSegmentsNumber; ++i)
            {
                int currentRow = i.UseOneBasedIndexing().SkipHeader();
                string currentRowStr = currentRow.ToString();
                string previousRowStr = (currentRow - 1).ToString();

                string pocketFormula = $"$J$10 + ($J$12 * {i.ToString()})";
                sheet[ExcelColumnIndex.D, currentRow].SetFormula(pocketFormula);

                string empiricalFrequencyFormula = $"$E{currentRowStr} / $J$6";
                sheet[ExcelColumnIndex.F, currentRow].SetFormula(empiricalFrequencyFormula);

                if (i == 0)
                {
                    CreateFirstValueInInterval(sheet, currentRow, currentRowStr, normalizedValueRange);
                }
                else if (i == histogramSegmentsNumber)
                {
                    CreateLastValueInInterval(sheet, currentRow, previousRowStr, normalizedValueRange);
                }
                else
                {
                    CreateMediumValueInInterval(
                        sheet, currentRow, currentRowStr, previousRowStr, normalizedValueRange
                    );
                }

                string chi2Formula = $"($F{currentRowStr} - $G{currentRowStr})^2 / $G{currentRowStr}";
                sheet[ExcelColumnIndex.H, currentRow].SetFormula(chi2Formula);
            }

            //string arrayFormula = sheet.FormulaProvider.Frequency(
            //    $"{normalizedValueRange}", $"$D$2:$D${histogramSegmentsNumberIndex}"
            //);
            //sheet.SetArrayFormula(
            //    arrayFormula,
            //    ExcelColumnIndex.C, 2,
            //    ExcelColumnIndex.C, histogramSegmentsNumber.UseOneBasedIndexing().SkipHeader()
            //);
        }

        private static void CreateFirstValueInInterval(IExcelSheet sheet, int currentRow,
            string currentRowStr, string normalizedValueRange)
        {
            string frequencyFormula = sheet.FormulaProvider.CountIfS(
                normalizedValueRange, $"\"<\" & $D{currentRowStr}"
            );
            sheet[ExcelColumnIndex.E, currentRow].SetFormula(frequencyFormula);

            string theoreticalFrequencyFormula = sheet.FormulaProvider.BetaDist(
                $"$D{currentRowStr}", "$M$11", "$M$12", cumulative: true
            );
            sheet[ExcelColumnIndex.G, currentRow].SetFormula(theoreticalFrequencyFormula);
        }

        private static void CreateLastValueInInterval(IExcelSheet sheet, int currentRow,
            string previousRowStr, string normalizedValueRange)
        {
            string frequencyFormula = sheet.FormulaProvider.CountIfS(
                normalizedValueRange, $"\">=\" & $D{previousRowStr}"
            );
            sheet[ExcelColumnIndex.E, currentRow].SetFormula(frequencyFormula);

            string betaDistFormula = sheet.FormulaProvider.BetaDist(
                $"$D{previousRowStr}", "$M$11", "$M$12", cumulative: true
            );
            string theoreticalFrequencyFormula = $"1 - {betaDistFormula}";
            sheet[ExcelColumnIndex.G, currentRow].SetFormula(theoreticalFrequencyFormula);
        }

        private static void CreateMediumValueInInterval(IExcelSheet sheet, int currentRow,
            string currentRowStr, string previousRowStr, string normalizedValueRange)
        {
            string frequencyFormula = sheet.FormulaProvider.CountIfS(
                normalizedValueRange, $"\">=\" & $D{previousRowStr}",
                normalizedValueRange, $"\"<\" & $D{currentRowStr}"
            );
            sheet[ExcelColumnIndex.E, currentRow].SetFormula(frequencyFormula);

            string betaDistFormulaCurrent = sheet.FormulaProvider.BetaDist(
                $"$D{currentRowStr}", "$M$11", "$M$12", cumulative: true
            );
            string betaDistFormulaPrevious = sheet.FormulaProvider.BetaDist(
                $"$D{previousRowStr}", "$M$11", "$M$12", cumulative: true
            );
            string theoreticalFrequencyFormula = $"{betaDistFormulaCurrent} - {betaDistFormulaPrevious}";
            sheet[ExcelColumnIndex.G, currentRow].SetFormula(theoreticalFrequencyFormula);
        }
    }
}
