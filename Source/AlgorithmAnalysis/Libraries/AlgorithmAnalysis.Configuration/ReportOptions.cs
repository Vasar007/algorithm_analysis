using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Common.Files;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Configuration
{
    public sealed class ReportOptions : IOptions
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ExcelCellCreationMode CellCreationMode { get; set; } = ExcelCellCreationMode.Centerized;

        [JsonConverter(typeof(StringEnumConverter))]
        public ExcelLibraryProvider LibraryProvider { get; set; } = ExcelLibraryProvider.EPPlus;

        [JsonConverter(typeof(StringEnumConverter))]
        public ExcelVersion ExcelVersion { get; set; } = ExcelVersion.V2007;

        public string OutputReportFilePath { get; set; } =
             Path.Combine(PredefinedPaths.DefaultResultFolderPath, CommonConstants.DefaultResultFilename);


        public ReportOptions()
        {
        }
    }
}
