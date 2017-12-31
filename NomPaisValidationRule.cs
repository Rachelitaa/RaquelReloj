using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Rellotge
{
    public class NomPaisValidationRule : ValidationRule
    {
        private Regex RExp;

        public NomPaisValidationRule()
        {
            RExp = new Regex("^[a-zA-Z][a-zA-Z ]*[a-zA-Z]$", RegexOptions.IgnoreCase);
        }

        public override ValidationResult Validate(object value, CultureInfo ultureInfo)
        {
            if (value == null || !RExp.Match(value.ToString()).Success)
            {
                return new ValidationResult(false, "The value is not a valid name");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
