using System;

namespace AlgorithmAnalysis.Excel
{
    public interface IExcelCellValueHolder
    {
        ActiveExcelCellType ActiveType { get; }

        string StringValue { get; }

        double NumericValue { get; }

        bool BooleanValue { get; }

        DateTime DateTimeValue { get; }

        byte ErrorValue { get; }

        string Formula { get; }
    }
}
