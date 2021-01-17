using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.VirtualizingStackPanel.CleanUpVirtualizedItem" /> event.</summary>
	// Token: 0x02000482 RID: 1154
	public class CleanUpVirtualizedItemEventArgs : RoutedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.CleanUpVirtualizedItemEventArgs" /> class.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> that represents the original data value.</param>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> that represents the data value.</param>
		// Token: 0x0600433E RID: 17214 RVA: 0x001338CE File Offset: 0x00131ACE
		public CleanUpVirtualizedItemEventArgs(object value, UIElement element) : base(VirtualizingStackPanel.CleanUpVirtualizedItemEvent)
		{
			this._value = value;
			this._element = element;
		}

		/// <summary>Gets an <see cref="T:System.Object" /> that represents the original data value.</summary>
		/// <returns>The <see cref="T:System.Object" /> that represents the original data value.</returns>
		// Token: 0x17001080 RID: 4224
		// (get) Token: 0x0600433F RID: 17215 RVA: 0x001338E9 File Offset: 0x00131AE9
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		/// <summary>Gets an instance of the visual element that represents the data value.</summary>
		/// <returns>The <see cref="T:System.Windows.UIElement" /> that represents the data value.</returns>
		// Token: 0x17001081 RID: 4225
		// (get) Token: 0x06004340 RID: 17216 RVA: 0x001338F1 File Offset: 0x00131AF1
		public UIElement UIElement
		{
			get
			{
				return this._element;
			}
		}

		/// <summary>Gets or sets a value that indicates whether this item should not be re-virtualized.</summary>
		/// <returns>
		///     <see langword="true" /> if you want to prevent revirtualization of this item; otherwise <see langword="false" />.</returns>
		// Token: 0x17001082 RID: 4226
		// (get) Token: 0x06004341 RID: 17217 RVA: 0x001338F9 File Offset: 0x00131AF9
		// (set) Token: 0x06004342 RID: 17218 RVA: 0x00133901 File Offset: 0x00131B01
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = value;
			}
		}

		// Token: 0x0400283B RID: 10299
		private object _value;

		// Token: 0x0400283C RID: 10300
		private UIElement _element;

		// Token: 0x0400283D RID: 10301
		private bool _cancel;
	}
}
