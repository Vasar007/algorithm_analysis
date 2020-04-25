namespace AlgorithmAnalysis.DesktopApp.Domain.Converters
{
    internal static class ConverterHelper
    {
        public static bool ToBoolean(object value)
        {
            bool flag = false;
            if (value is bool valueConverted)
            {
                flag = valueConverted;
            }
            else if (value is bool?)
            {
                var nullable = (bool?) value;
                flag = nullable.GetValueOrDefault(false);
            }

            return flag;
        }
    }
}
