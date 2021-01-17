using System;
using System.Globalization;

namespace System.Windows.Controls
{
	/// <summary>Represents a rule that checks for exceptions that are thrown during the update of the binding source property.</summary>
	// Token: 0x020004CD RID: 1229
	public sealed class ExceptionValidationRule : ValidationRule
	{
		/// <summary>Performs validation checks on a value.</summary>
		/// <param name="value">The value (from the binding target) to check.</param>
		/// <param name="cultureInfo">The culture to use in this rule.</param>
		/// <returns>A <see cref="T:System.Windows.Controls.ValidationResult" /> object.</returns>
		// Token: 0x06004B00 RID: 19200 RVA: 0x00138288 File Offset: 0x00136488
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			return ValidationResult.ValidResult;
		}

		// Token: 0x04002AA5 RID: 10917
		internal static readonly ExceptionValidationRule Instance = new ExceptionValidationRule();
	}
}
