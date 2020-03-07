using System;

namespace AlgorithmAnalysis.Excel
{
    public interface IExcelCellHolder
    {
        ActiveExcelCellType ActiveType { get; }

        string StringValue { get; }
        
        double NumericValue { get; }
        
        bool BooleanValue { get; }
        
        DateTime DateTimeValue { get; }
        
        byte ErrorValue { get; }
        
        string Formula { get; }

        string FullAddress { get; }

        string Address { get; }


        void SetValue(string value);

        void SetValue(double value);

        void SetValue(bool value);

        void SetValue(DateTime value);

        void SetErrorValue(byte value);

        void SetFormula(string formula);

        IExcelCellValueHolder Evaluate();
    }
}
