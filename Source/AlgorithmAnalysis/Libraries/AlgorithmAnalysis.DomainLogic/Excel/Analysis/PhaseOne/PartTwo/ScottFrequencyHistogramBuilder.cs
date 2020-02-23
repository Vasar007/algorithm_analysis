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
            sheet.SetCenterizedCellValue(ExcelColumnIndex.D, 1, ExcelStrings.PocketColumnName);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.E, 1, ExcelStrings.FrequencyColumnName);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.F, 1, ExcelStrings.EmpiricalFrequencyColumnName);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.G, 1, ExcelStrings.TheoreticalFrequencyColumnName);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.H, 1, ExcelStrings.Chi2ValueColumnName);

            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 10, ExcelStrings.MinimumValue);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 11, ExcelStrings.MaximumValue);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 12, ExcelStrings.IntervalLength);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 13, ExcelStrings.HistogramSemisegmentsNumber);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 14, ExcelStrings.Chi2Observable);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 15, ExcelStrings.FreedomDegreesNumber);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 16, ExcelStrings.Chi2Critical);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 17, ExcelStrings.CheckTestFucntion);

            string lastValueRowIndex = _args.LaunchesNumber.SkipHeader().ToString();
            string normalizedValueRange = $"$B$2:$B${lastValueRowIndex}";
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 10, $"MIN({normalizedValueRange})");
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 11, $"MAX({normalizedValueRange})");

            string scottFormula = $"(3.5 * STDEV({normalizedValueRange})) / (J6^(1/3))"; // STDEV == STDEV.S
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 12, scottFormula);
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 13, "ROUNDUP(($J$11 - $J$10) / $J$12, 0)");

            IExcelCellValueHolder histogramSegmentsNumber = sheet.EvaluateCell(ExcelColumnIndex.J, 13);
            int histogramSegmentsNumberInt = Convert.ToInt32(histogramSegmentsNumber.NumericValue);
            string histogramSegmentsNumberIndex = histogramSegmentsNumberInt.UseOneBasedIndexing().SkipHeader().ToString();

            CreateIntervalData(
                sheet,
                histogramSegmentsNumberInt, histogramSegmentsNumberIndex,
                normalizedValueRange
            );

            sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 14, $"SUM($H$2:$H${histogramSegmentsNumberIndex}) * $J$6");
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 15, "$J$13 - 1 - 2");
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 16, "CHIINV($J$8, $J$15)"); // CHIINV == CHISQ.INV.RT

            string testFormula = $"CHITEST($F$2:$F${histogramSegmentsNumberIndex}, $G$2:$G${histogramSegmentsNumberIndex})"; // CHITEST == CHISQ.TEST
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 17, testFormula);

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
                sheet.SetCenterizedCellFormula(ExcelColumnIndex.D, currentRow, pocketFormula);

                string empiricalFrequencyFormula = $"$E{currentRowStr} / $J$6";
                sheet.SetCenterizedCellFormula(ExcelColumnIndex.F, currentRow, empiricalFrequencyFormula);

                // TODO: implement proper excel formula provider.
                if (i == 0)
                {
                    string frequencyFormula = $"COUNTIFS({normalizedValueRange}, \"<\" & $D{currentRowStr})";
                    sheet.SetCenterizedCellFormula(ExcelColumnIndex.E, currentRow, frequencyFormula);

                    string theoreticalFrequencyFormula = $"BETADIST($D{currentRowStr}, $M$11, $M$12)"; // BETADIST == BETA.DIST
                    //string theoreticalFrequencyFormula = $"BETA.DIST($D{currentRowStr}, $M$11, $M$12, TRUE,)"; // BETADIST == BETA.DIST
                    sheet.SetCenterizedCellFormula(ExcelColumnIndex.G, currentRow, theoreticalFrequencyFormula);
                }
                else if (i == histogramSegmentsNumber)
                {
                    string frequencyFormula = $"COUNTIFS({normalizedValueRange}, \">=\" & $D{previousRowStr})";
                    sheet.SetCenterizedCellFormula(ExcelColumnIndex.E, currentRow, frequencyFormula);

                    string theoreticalFrequencyFormula = $"1 - BETADIST($D{previousRowStr}, $M$11, $M$12)"; // BETADIST == BETA.DIST
                    //string theoreticalFrequencyFormula = $"1 - BETA.DIST($D{previousRowStr}, $M$11, $M$12, TRUE,)"; // BETADIST == BETA.DIST
                    sheet.SetCenterizedCellFormula(ExcelColumnIndex.G, currentRow, theoreticalFrequencyFormula);
                }
                else
                {
                    string frequencyFormula = $"COUNTIFS({normalizedValueRange}, \">=\" & $D{previousRowStr}, {normalizedValueRange}, \"<\" & $D{currentRowStr})";
                    sheet.SetCenterizedCellFormula(ExcelColumnIndex.E, currentRow, frequencyFormula);

                    string theoreticalFrequencyFormula = $"BETADIST($D{currentRowStr}, $M$11, $M$12) - BETADIST($D{previousRowStr}, $M$11, $M$12)"; // BETADIST == BETA.DIST
                    //string theoreticalFrequencyFormula = $"BETA.DIST($D{currentRowStr}, $M$11, $M$12, TRUE,) - BETA.DIST($D{previousRowStr}, $M$11, $M$12, TRUE,)"; // BETADIST == BETA.DIST
                    sheet.SetCenterizedCellFormula(ExcelColumnIndex.G, currentRow, theoreticalFrequencyFormula);
                }
                

                string chi2Formula = $"($F{currentRowStr} - $G{currentRowStr})^2 / $G{currentRowStr}";
                sheet.SetCenterizedCellFormula(ExcelColumnIndex.H, currentRow, chi2Formula);
            }

            //string arrayFormula = $"FREQUENCY({normalizedValueRange}, $D$2:$D${histogramSegmentsNumberIndex})";
            //sheet.SetArrayFormula(arrayFormula, ExcelColumnIndex.C, 2, ExcelColumnIndex.C, histogramSegmentsNumber.UseOneBasedIndexing().SkipHeader());
        }
    }
}
