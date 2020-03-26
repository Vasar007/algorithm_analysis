using System.IO;
using Acolyte.Assertions;
using OfficeOpenXml;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.Logging;

namespace AlgorithmAnalysis.Excel.EPPlus
{
    internal sealed class EpplusLogger
    {
        private static readonly string EpplusLogFilename =
            LogHelper.CreateLogFilename(logName: "epplus");

        private readonly ExcelPackage _package;

        private readonly LoggerOptions _loggerOptions;

        private readonly string _logFilePath;


        public EpplusLogger(
            ExcelPackage package,
            LoggerOptions loggerOptions)
        {
            _package = package.ThrowIfNull(nameof(package));
            _loggerOptions = loggerOptions.ThrowIfNull(nameof(loggerOptions));

            _logFilePath = LogHelper.GetLogFilePath(_loggerOptions, EpplusLogFilename);
        }

        public void AttachLogger()
        {
            if (!_loggerOptions.EnableLogForExcelLibrary) return;

            var logfile = new FileInfo(_logFilePath);
            _package.Workbook.FormulaParserManager.AttachLogger(logfile);
        }

        public void DetachLogger()
        {
            if (!_loggerOptions.EnableLogForExcelLibrary) return;

            _package.Workbook.FormulaParserManager.DetachLogger();
        }
    }
}
