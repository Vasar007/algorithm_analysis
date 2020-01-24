using System.Globalization;
using System.Windows.Controls;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class NumberValidation : ValidationRule
    {
        public NumberValidation()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!(value is string str))
            {
                return new ValidationResult(false, "Text box content value is not a string.");
            }

            if (str.Length == 0)
            {
                return new ValidationResult(false, "Text box is empty.");
            }

            if (!int.TryParse(str, out int result))
            {
                return new ValidationResult(false, "Illegal characters in text box.");
            }

            if (result <= 0)
            {
                return new ValidationResult(false, "Parameter should be positive.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
