using System;

namespace System.Windows.Controls
{
	/// <summary>Represents the result returned by the <see cref="T:System.Windows.Controls.ValidationRule" />.<see cref="M:System.Windows.Controls.ValidationRule.Validate(System.Object,System.Globalization.CultureInfo)" /> method that indicates whether the checked value passed the <see cref="T:System.Windows.Controls.ValidationRule" />.</summary>
	// Token: 0x02000554 RID: 1364
	public class ValidationResult
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ValidationResult" /> class.</summary>
		/// <param name="isValid">Whether or not the value checked against the <see cref="T:System.Windows.Controls.ValidationRule" /> is valid.</param>
		/// <param name="errorContent">Information about the invalidity.</param>
		// Token: 0x06005993 RID: 22931 RVA: 0x0018B77C File Offset: 0x0018997C
		public ValidationResult(bool isValid, object errorContent)
		{
			this._isValid = isValid;
			this._errorContent = errorContent;
		}

		/// <summary>Gets a value that indicates whether the value checked against the <see cref="T:System.Windows.Controls.ValidationRule" /> is valid.</summary>
		/// <returns>
		///     <see langword="true" /> if the value is valid; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x170015C9 RID: 5577
		// (get) Token: 0x06005994 RID: 22932 RVA: 0x0018B792 File Offset: 0x00189992
		public bool IsValid
		{
			get
			{
				return this._isValid;
			}
		}

		/// <summary>Gets an object that provides additional information about the invalidity.</summary>
		/// <returns>An object that provides additional information about the invalidity.</returns>
		// Token: 0x170015CA RID: 5578
		// (get) Token: 0x06005995 RID: 22933 RVA: 0x0018B79A File Offset: 0x0018999A
		public object ErrorContent
		{
			get
			{
				return this._errorContent;
			}
		}

		/// <summary>Gets a valid instance of <see cref="T:System.Windows.Controls.ValidationResult" />.</summary>
		/// <returns>A valid instance of <see cref="T:System.Windows.Controls.ValidationResult" />.</returns>
		// Token: 0x170015CB RID: 5579
		// (get) Token: 0x06005996 RID: 22934 RVA: 0x0018B7A2 File Offset: 0x001899A2
		public static ValidationResult ValidResult
		{
			get
			{
				return ValidationResult.s_valid;
			}
		}

		/// <summary>Compares two <see cref="T:System.Windows.Controls.ValidationResult" /> objects for value equality.</summary>
		/// <param name="left">The first instance to compare.</param>
		/// <param name="right">The second instance to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the two objects are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005997 RID: 22935 RVA: 0x0018B7A9 File Offset: 0x001899A9
		public static bool operator ==(ValidationResult left, ValidationResult right)
		{
			return object.Equals(left, right);
		}

		/// <summary>Compares two <see cref="T:System.Windows.Controls.ValidationResult" /> objects for value inequality.</summary>
		/// <param name="left">The first instance to compare.</param>
		/// <param name="right">The second instance to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the values are not equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005998 RID: 22936 RVA: 0x0018B7B2 File Offset: 0x001899B2
		public static bool operator !=(ValidationResult left, ValidationResult right)
		{
			return !object.Equals(left, right);
		}

		/// <summary>Compares the specified instance and the current instance of <see cref="T:System.Windows.Controls.ValidationResult" /> for value equality.</summary>
		/// <param name="obj">The <see cref="T:System.Windows.Controls.ValidationResult" /> instance to compare.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="obj" /> and this instance of <see cref="T:System.Windows.Controls.ValidationResult" />.have the same values.</returns>
		// Token: 0x06005999 RID: 22937 RVA: 0x0018B7C0 File Offset: 0x001899C0
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ValidationResult validationResult = obj as ValidationResult;
			return validationResult != null && this.IsValid == validationResult.IsValid && this.ErrorContent == validationResult.ErrorContent;
		}

		/// <summary>Returns the hash code for this <see cref="T:System.Windows.Controls.ValidationResult" />.</summary>
		/// <returns>The hash code for this <see cref="T:System.Windows.Controls.ValidationResult" />.</returns>
		// Token: 0x0600599A RID: 22938 RVA: 0x0018B804 File Offset: 0x00189A04
		public override int GetHashCode()
		{
			return this.IsValid.GetHashCode() ^ ((this.ErrorContent == null) ? int.MinValue : this.ErrorContent).GetHashCode();
		}

		// Token: 0x04002F11 RID: 12049
		private bool _isValid;

		// Token: 0x04002F12 RID: 12050
		private object _errorContent;

		// Token: 0x04002F13 RID: 12051
		private static readonly ValidationResult s_valid = new ValidationResult(true, null);
	}
}
