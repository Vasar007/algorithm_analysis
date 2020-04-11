using System.Collections.Generic;
using Prism.Mvvm;
using Acolyte.Assertions;
using Acolyte.Common;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DesktopApp.Domain.Validation;
using AlgorithmAnalysis.Models;
using AlgorithmAnalysis.Configuration;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class SettingsReportModel : BindableBase, IChangeable, ISaveable
    {
        #region Cell Creation Mode

        public IReadOnlyList<ExcelCellCreationMode> AvailableCellCreationModes { get; }

        private ExcelCellCreationMode _cellCreationMode;
        public ExcelCellCreationMode CellCreationMode
        {
            get => _cellCreationMode;
            set => SetProperty(ref _cellCreationMode, value);
        }

        public bool IsCellCreationModeSelectable =>
            AvailableCellCreationModes.Count > CommonConstants.EmptyCollectionCount;

        /// <summary>
        /// Show warning about no availbale cell creation mode.
        /// </summary>
        public bool IsHintForCellCreationModeVisible =>
            !IsCellCreationModeSelectable &&
            !CellCreationMode.IsDefined();

        #endregion

        #region Library Provider

        public IReadOnlyList<ExcelLibraryProvider> AvailableLibraryProviders { get; }

        private ExcelLibraryProvider _libraryProvider;
        public ExcelLibraryProvider LibraryProvider
        {
            get => _libraryProvider;
            set => SetProperty(ref _libraryProvider, value);
        }

        public bool IsLibraryProviderSelectable =>
            AvailableLibraryProviders.Count > CommonConstants.EmptyCollectionCount;

        /// <summary>
        /// Show warning about no availbale library providers.
        /// </summary>
        public bool IsHintForLibraryProviderVisible =>
            !IsLibraryProviderSelectable &&
            !LibraryProvider.IsDefined();

        #endregion

        #region Excel Version

        public IReadOnlyList<ExcelVersion> AvailableExcelVersions { get; }

        private ExcelVersion _excelVersion;
        public ExcelVersion ExcelVersion
        {
            get => _excelVersion;
            set => SetProperty(ref _excelVersion, value);
        }

        public bool IsExcelVersionSelectable =>
            AvailableExcelVersions.Count > CommonConstants.EmptyCollectionCount;

        /// <summary>
        /// Show warning about no availbale library providers.
        /// </summary>
        public bool IsHintForExcelVersionVisible =>
            !IsExcelVersionSelectable &&
            !ExcelVersion.IsDefined();

        #endregion

        // Initializes through Reset method in ctor.
        private string _outputReportFilePath = default!;
        public string OutputReportFilePath
        {
            get => _outputReportFilePath;
            set => SetProperty(ref _outputReportFilePath, value.ThrowIfNull(nameof(value)));
        }


        public SettingsReportModel()
        {
            AvailableCellCreationModes = DesktopOptions.AvailableCellCreationModes;
            AvailableLibraryProviders = DesktopOptions.AvailableLibraryProviders;
            AvailableExcelVersions = DesktopOptions.AvailableExcelVersions;

            Reset();
        }

        #region IChangeableModel Implementation

        public void Reset()
        {
            ReportOptions reportOptions = ConfigOptions.Report;

            CellCreationMode = reportOptions.CellCreationMode;
            LibraryProvider = reportOptions.LibraryProvider;
            ExcelVersion = reportOptions.ExcelVersion;
            OutputReportFilePath = Utils.ResolvePath(reportOptions.OutputReportFilePath);
        }

        public void Validate()
        {
            ValidationHelper.AssertIfGotUnknownValue(CellCreationMode, nameof(CellCreationMode));
            ValidationHelper.AssertIfGotUnknownValue(LibraryProvider, nameof(LibraryProvider));
            ValidationHelper.AssertIfGotUnknownValue(ExcelVersion, nameof(ExcelVersion));

            // TODO: implement settings parameters validation:
            // OutputExcelFilePath
        }

        #endregion

        #region ISaveable Implementation

        public void SaveToConfigFile()
        {
            Validate();

            ReportOptions reportOptions = ConfigOptions.Report;

            reportOptions.CellCreationMode = CellCreationMode;
            reportOptions.LibraryProvider = LibraryProvider;
            reportOptions.ExcelVersion = ExcelVersion;
            reportOptions.OutputReportFilePath = OutputReportFilePath;

            ConfigOptions.SetOptions(reportOptions);
        }

        #endregion
    }
}
