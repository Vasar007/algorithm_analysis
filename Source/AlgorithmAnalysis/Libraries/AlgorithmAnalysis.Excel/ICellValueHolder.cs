using System;

namespace AlgorithmAnalysis.Excel
{
    public interface ICellValueHolder
    {
        ActiveCellType ActiveType { get; }

        string StringValue { get; }

        double NumericValue { get; }

        bool BooleanValue { get; }

        DateTime DateTimeValue { get; }

        byte ErrorValue { get; }

        string Formula { get; }
    }
}
