using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Properties;
using AlgorithmAnalysis.Excel;
using AlgorithmAnalysis.Logging;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo
{
    internal sealed class ScottFrequencyHistogramBuilder : IFrequencyHistogramBuilder
    {
        private static readonly ILogger _logger =
            LoggerFactory.CreateLoggerFor<ScottFrequencyHistogramBuilder>();

        private readonly ParametersPack _args;


        public ScottFrequencyHistogramBuilder(ParametersPack args)
        {
            _args = args.ThrowIfNull(nameof(args));
        }

        #region IFrequencyHistogramBuilder Implementation

        public void CreateHistogramData(IExcelSheet sheet)
        {
            sheet[ExcelColumnIndex.D, 1].SetValue(ExcelStringsPhaseOnePartTwo.PocketColumnName);
            sheet[ExcelColumnIndex.E, 1].SetValue(ExcelStringsPhaseOnePartTwo.EmpiricalFrequencyColumnName);
            sheet[ExcelColumnIndex.F, 1].SetValue(ExcelStringsPhaseOnePartTwo.RelativeFrequencyColumnName);
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

            string scottFormula = ManualFormulaProvider.ScottFormula(
                sheet.FormulaProvider, normalizedValueRange, "$J$6"
            );
            sheet[ExcelColumnIndex.J, 12].SetFormula(scottFormula);
            sheet[ExcelColumnIndex.J, 13].SetFormula(
                sheet.FormulaProvider.RoundUp("($J$11 - $J$10) / $J$12", "0")
            );

            IExcelCellValueHolder histogramSegmentsNumber = sheet.EvaluateCell(ExcelColumnIndex.J, 13);
            int histogramSegmentsNumberInt = Convert.ToInt32(histogramSegmentsNumber.NumericValue);
            string histogramSegmentsNumberIndex =
                histogramSegmentsNumberInt.UseOneBasedIndexing().SkipHeader().ToString();

            CreateIntervalData(sheet, histogramSegmentsNumberInt, normalizedValueRange);

            string chi2Formula = ManualFormulaProvider.Chi2(
                sheet.FormulaProvider, $"$H$2:$H${histogramSegmentsNumberIndex}", "$J$6"
            );
            sheet[ExcelColumnIndex.J, 14].SetFormula(chi2Formula);
            sheet[ExcelColumnIndex.J, 15].SetFormula("$J$13 - 1 - 2");
            sheet[ExcelColumnIndex.J, 16].SetFormula(sheet.FormulaProvider.ChiInv("$J$8", "$J$15"));

            string testFormula = sheet.FormulaProvider.ChiTest(
                $"$F$2:$F${histogramSegmentsNumberIndex}",
                $"$G$2:$G${histogramSegmentsNumberIndex}"
            );
            sheet[ExcelColumnIndex.J, 17].SetFormula(testFormula);

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

            double chi2ObservableValue = chi2Observable.NumericValue;
            double chi2CriticalValue = chi2Critical.NumericValue;

            _logger.Info($"Chi2 observable: '{chi2ObservableValue.ToString()}'.");
            _logger.Info($"Chi2 critical: '{chi2CriticalValue.ToString()}'.");

            // TODO: remove this dirty hack.
            const int dirtyhack = 100;
            return (chi2ObservableValue / dirtyhack) < chi2CriticalValue;
        }

        #endregion

        private void CreateIntervalData(IExcelSheet sheet, int histogramSegmentsNumber,
            string normalizedValueRange)
        {
            histogramSegmentsNumber.ThrowIfValueIsOutOfRange(nameof(histogramSegmentsNumber), 1, int.MaxValue);

            for (int i = 0; i <= histogramSegmentsNumber; ++i)
            {
                int currentRow = i.UseOneBasedIndexing().SkipHeader();
                string currentRowStr = currentRow.ToString();
                string previousRowStr = (currentRow - 1).ToString();

                string pocketFormula = $"$J$10 + ($J$12 * {i.ToString()})";
                sheet[ExcelColumnIndex.D, currentRow].SetFormula(pocketFormula);

                if (i == 0)
                {
                    CreateFirstValueInInterval(
                        sheet, currentRow, currentRowStr, normalizedValueRange
                    );
                }
                else if (i == histogramSegmentsNumber)
                {
                    CreateLastValueInInterval(
                        sheet, currentRow, previousRowStr, normalizedValueRange
                    );
                }
                else
                {
                    CreateMediumValueInInterval(
                        sheet, currentRow, currentRowStr, previousRowStr, normalizedValueRange
                    );
                }

                string relativeFrequencyFormula = $"$E{currentRowStr} / $J$6";
                sheet[ExcelColumnIndex.F, currentRow].SetFormula(relativeFrequencyFormula);

                string chi2SingleFormula = ManualFormulaProvider.Chi2Single(
                    $"$F{currentRowStr}", $"$G{currentRowStr}"
                );
                sheet[ExcelColumnIndex.H, currentRow].SetFormula(chi2SingleFormula);
            }

            string lastRowIndexStr = histogramSegmentsNumber.UseOneBasedIndexing().SkipHeader().ToString();
            int checkSumRowIndex = (histogramSegmentsNumber + 2).UseOneBasedIndexing().SkipHeader();

            sheet[ExcelColumnIndex.D, checkSumRowIndex].SetValue(ExcelStringsPhaseOnePartTwo.Sum);
            sheet[ExcelColumnIndex.E, checkSumRowIndex].SetFormula(
                sheet.FormulaProvider.Sum($"$E2:$E${lastRowIndexStr}")
            );
            sheet[ExcelColumnIndex.F, checkSumRowIndex].SetFormula(
                sheet.FormulaProvider.Sum($"$F2:$F${lastRowIndexStr}")
            );
            sheet[ExcelColumnIndex.G, checkSumRowIndex].SetFormula(
                sheet.FormulaProvider.Sum($"$G2:$G${lastRowIndexStr}")
            );
            sheet[ExcelColumnIndex.H, checkSumRowIndex].SetFormula(
                sheet.FormulaProvider.Sum($"$H2:$H${lastRowIndexStr}")
            );
        }

        private static void CreateFirstValueInInterval(IExcelSheet sheet, int currentRow,
            string currentRowStr, string normalizedValueRange)
        {
            string empericalFrequencyFormula = sheet.FormulaProvider.CountIfS(
                normalizedValueRange, $"\"<\" & $D{currentRowStr}"
            );
            sheet[ExcelColumnIndex.E, currentRow].SetFormula(empericalFrequencyFormula);

            string theoreticalFrequencyFormula = sheet.FormulaProvider.BetaDist(
                $"$D{currentRowStr}", "$M$11", "$M$12", cumulative: true
            );
            sheet[ExcelColumnIndex.G, currentRow].SetFormula(theoreticalFrequencyFormula);
        }

        private static void CreateLastValueInInterval(IExcelSheet sheet, int currentRow,
            string previousRowStr, string normalizedValueRange)
        {
            string empericalFrequencyFormula = sheet.FormulaProvider.CountIfS(
                normalizedValueRange, $"\">=\" & $D{previousRowStr}"
            );
            sheet[ExcelColumnIndex.E, currentRow].SetFormula(empericalFrequencyFormula);

            string betaDistFormula = sheet.FormulaProvider.BetaDist(
                $"$D{previousRowStr}", "$M$11", "$M$12", cumulative: true
            );
            string theoreticalFrequencyFormula = $"1 - {betaDistFormula}";
            sheet[ExcelColumnIndex.G, currentRow].SetFormula(theoreticalFrequencyFormula);
        }

        private static void CreateMediumValueInInterval(IExcelSheet sheet, int currentRow,
            string currentRowStr, string previousRowStr, string normalizedValueRange)
        {
            string empericalFrequencyFormula = sheet.FormulaProvider.CountIfS(
                normalizedValueRange, $"\">=\" & $D{previousRowStr}",
                normalizedValueRange, $"\"<\" & $D{currentRowStr}"
            );
            sheet[ExcelColumnIndex.E, currentRow].SetFormula(empericalFrequencyFormula);

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
