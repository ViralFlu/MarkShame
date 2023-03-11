using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MarkShame
{
    public class PathValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string path = value as string;

            if (string.IsNullOrEmpty(path))
            {
                return new ValidationResult(false, "Path cannot be empty.");
            }

            try
            {
                // Check if the file exists
                if (!File.Exists(path))
                {
                    return new ValidationResult(false, "File does not exist.");
                }

                // Get the full path of the file
                Path.GetFullPath(path);

                // Return a valid result
                return ValidationResult.ValidResult;
            }
            catch
            {
                // Return an invalid result with an appropriate error message
                return new ValidationResult(false, "Invalid path.");
            }
        }
    }
}
