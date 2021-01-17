using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DatePicker.DateValidationError" /> event.</summary>
	// Token: 0x020004C2 RID: 1218
	public class DatePickerDateValidationErrorEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DatePickerDateValidationErrorEventArgs" /> class. </summary>
		/// <param name="exception">The initial exception from the <see cref="E:System.Windows.Controls.DatePicker.DateValidationError" /> event.</param>
		/// <param name="text">The text that caused the <see cref="E:System.Windows.Controls.DatePicker.DateValidationError" /> event.</param>
		// Token: 0x06004A1E RID: 18974 RVA: 0x0014F222 File Offset: 0x0014D422
		public DatePickerDateValidationErrorEventArgs(Exception exception, string text)
		{
			this.Text = text;
			this.Exception = exception;
		}

		/// <summary>Gets the initial exception associated with the <see cref="E:System.Windows.Controls.DatePicker.DateValidationError" /> event.</summary>
		/// <returns>The exception associated with the validation failure.</returns>
		// Token: 0x17001215 RID: 4629
		// (get) Token: 0x06004A1F RID: 18975 RVA: 0x0014F238 File Offset: 0x0014D438
		// (set) Token: 0x06004A20 RID: 18976 RVA: 0x0014F240 File Offset: 0x0014D440
		public Exception Exception { get; private set; }

		/// <summary>Gets or sets the text that caused the <see cref="E:System.Windows.Controls.DatePicker.DateValidationError" /> event.</summary>
		/// <returns>The text that caused the validation failure.</returns>
		// Token: 0x17001216 RID: 4630
		// (get) Token: 0x06004A21 RID: 18977 RVA: 0x0014F249 File Offset: 0x0014D449
		// (set) Token: 0x06004A22 RID: 18978 RVA: 0x0014F251 File Offset: 0x0014D451
		public string Text { get; private set; }

		/// <summary>Gets or sets a value that indicates whether <see cref="P:System.Windows.Controls.DatePickerDateValidationErrorEventArgs.Exception" /> should be thrown.</summary>
		/// <returns>
		///     <see langword="true" /> if the exception should be thrown; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001217 RID: 4631
		// (get) Token: 0x06004A23 RID: 18979 RVA: 0x0014F25A File Offset: 0x0014D45A
		// (set) Token: 0x06004A24 RID: 18980 RVA: 0x0014F262 File Offset: 0x0014D462
		public bool ThrowException
		{
			get
			{
				return this._throwException;
			}
			set
			{
				this._throwException = value;
			}
		}

		// Token: 0x04002A48 RID: 10824
		private bool _throwException;
	}
}
