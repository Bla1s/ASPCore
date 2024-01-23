using System.ComponentModel.DataAnnotations;

namespace ASPCore.Validators
{
	public class NoNumbersAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var str = value as string;

			if (str == null)
			{
				return new ValidationResult("Value must be a string.");
			}

			if (str.Any(char.IsDigit))
			{
				return new ValidationResult("Value must not contain any numbers.");
			}

			return ValidationResult.Success;
		}
	}
}
