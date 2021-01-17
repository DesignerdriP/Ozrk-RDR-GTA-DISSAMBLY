using System;
using System.Globalization;

namespace System.Windows.Controls
{
	// Token: 0x0200048F RID: 1167
	internal sealed class ConversionValidationRule : ValidationRule
	{
		// Token: 0x060044BF RID: 17599 RVA: 0x0013827E File Offset: 0x0013647E
		internal ConversionValidationRule() : base(ValidationStep.ConvertedProposedValue, false)
		{
		}

		// Token: 0x060044C0 RID: 17600 RVA: 0x00138288 File Offset: 0x00136488
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			return ValidationResult.ValidResult;
		}

		// Token: 0x040028AF RID: 10415
		internal static readonly ConversionValidationRule Instance = new ConversionValidationRule();
	}
}
