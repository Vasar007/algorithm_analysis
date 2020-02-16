﻿using System;

namespace AlgorithmAnalysis.Excel
{
    public interface ICellHolder
    {
        ActiveCellType ActiveType { get; }

        string StringValue { get; }
        
        double NumericValue { get; }
        
        bool BooleanValue { get; }
        
        DateTime DateTimeValue { get; }
        
        byte ErrorValue { get; }
        
        string Formula { get; }


        void SetValue(string value);

        void SetValue(double value);

        void SetValue(bool value);

        void SetValue(DateTime value);

        void SetErrorValue(byte value);

        void SetFormula(string formula);
    }
}
