using System;
using System.Globalization;
using System.Windows.Data;

namespace System.Windows.Controls
{
	/// <summary>Provides a way to create a custom rule in order to check the validity of user input. </summary>
	// Token: 0x02000555 RID: 1365
	public abstract class ValidationRule
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ValidationRule" /> class.</summary>
		// Token: 0x0600599C RID: 22940 RVA: 0x0018B84D File Offset: 0x00189A4D
		protected ValidationRule() : this(ValidationStep.RawProposedValue, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ValidationRule" /> class with the specified validation step and a value that indicates whether the validation rule runs when the target is updated. </summary>
		/// <param name="validationStep">One of the enumeration values that specifies when the validation rule runs.</param>
		/// <param name="validatesOnTargetUpdated">
		///       <see langword="true" /> to have the validation rule run when the target of the <see cref="T:System.Windows.Data.Binding" /> is updated; otherwise, <see langword="false" />.</param>
		// Token: 0x0600599D RID: 22941 RVA: 0x0018B857 File Offset: 0x00189A57
		protected ValidationRule(ValidationStep validationStep, bool validatesOnTargetUpdated)
		{
			this._validationStep = validationStep;
			this._validatesOnTargetUpdated = validatesOnTargetUpdated;
		}

		/// <summary>When overridden in a derived class, performs validation checks on a value.</summary>
		/// <param name="value">The value from the binding target to check.</param>
		/// <param name="cultureInfo">The culture to use in this rule.</param>
		/// <returns>A <see cref="T:System.Windows.Controls.ValidationResult" /> object.</returns>
		// Token: 0x0600599E RID: 22942
		public abstract ValidationResult Validate(object value, CultureInfo cultureInfo);

		/// <summary>Performs validation checks on a value.</summary>
		/// <param name="value">The value from the binding target to check.</param>
		/// <param name="cultureInfo">The culture to use in this rule.</param>
		/// <param name="owner">The binding expression that uses the validation rule.</param>
		/// <returns>A <see cref="T:System.Windows.Controls.ValidationResult" /> object.</returns>
		// Token: 0x0600599F RID: 22943 RVA: 0x0018B870 File Offset: 0x00189A70
		public virtual ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
		{
			ValidationStep validationStep = this._validationStep;
			if (validationStep - ValidationStep.UpdatedValue <= 1)
			{
				value = owner;
			}
			return this.Validate(value, cultureInfo);
		}

		/// <summary>Performs validation checks on a value.</summary>
		/// <param name="value">The value from the binding target to check.</param>
		/// <param name="cultureInfo">The culture to use in this rule.</param>
		/// <param name="owner">The binding group that uses the validation rule.</param>
		/// <returns>A <see cref="T:System.Windows.Controls.ValidationResult" /> object.</returns>
		// Token: 0x060059A0 RID: 22944 RVA: 0x0018B895 File Offset: 0x00189A95
		public virtual ValidationResult Validate(object value, CultureInfo cultureInfo, BindingGroup owner)
		{
			return this.Validate(owner, cultureInfo);
		}

		/// <summary>Gets or sets when the validation rule runs.</summary>
		/// <returns>One of the enumeration values.  The default is <see cref="F:System.Windows.Controls.ValidationStep.RawProposedValue" />.</returns>
		// Token: 0x170015CC RID: 5580
		// (get) Token: 0x060059A1 RID: 22945 RVA: 0x0018B89F File Offset: 0x00189A9F
		// (set) Token: 0x060059A2 RID: 22946 RVA: 0x0018B8A7 File Offset: 0x00189AA7
		public ValidationStep ValidationStep
		{
			get
			{
				return this._validationStep;
			}
			set
			{
				this._validationStep = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the validation rule runs when the target of the <see cref="T:System.Windows.Data.Binding" /> is updated. </summary>
		/// <returns>
		///     <see langword="true" /> if the validation rule runs when the target of the <see cref="T:System.Windows.Data.Binding" /> is updated; otherwise, <see langword="false" />.</returns>
		// Token: 0x170015CD RID: 5581
		// (get) Token: 0x060059A3 RID: 22947 RVA: 0x0018B8B0 File Offset: 0x00189AB0
		// (set) Token: 0x060059A4 RID: 22948 RVA: 0x0018B8B8 File Offset: 0x00189AB8
		public bool ValidatesOnTargetUpdated
		{
			get
			{
				return this._validatesOnTargetUpdated;
			}
			set
			{
				this._validatesOnTargetUpdated = value;
			}
		}

		// Token: 0x04002F14 RID: 12052
		private ValidationStep _validationStep;

		// Token: 0x04002F15 RID: 12053
		private bool _validatesOnTargetUpdated;
	}
}
