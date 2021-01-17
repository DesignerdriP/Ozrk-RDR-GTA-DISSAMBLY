using System;

namespace System.Windows.Controls
{
	/// <summary>Describes a change in the scrolling state and contains the required arguments for a <see cref="E:System.Windows.Controls.ScrollViewer.ScrollChanged" /> event. </summary>
	// Token: 0x02000522 RID: 1314
	public class ScrollChangedEventArgs : RoutedEventArgs
	{
		// Token: 0x060054D7 RID: 21719 RVA: 0x00177A84 File Offset: 0x00175C84
		internal ScrollChangedEventArgs(Vector offset, Vector offsetChange, Size extent, Vector extentChange, Size viewport, Vector viewportChange)
		{
			this._offset = offset;
			this._offsetChange = offsetChange;
			this._extent = extent;
			this._extentChange = extentChange;
			this._viewport = viewport;
			this._viewportChange = viewportChange;
		}

		/// <summary>Gets the updated horizontal offset value for a <see cref="T:System.Windows.Controls.ScrollViewer" />.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the updated value of the horizontal offset for a <see cref="T:System.Windows.Controls.ScrollViewer" />.</returns>
		// Token: 0x1700149E RID: 5278
		// (get) Token: 0x060054D8 RID: 21720 RVA: 0x00177AB9 File Offset: 0x00175CB9
		public double HorizontalOffset
		{
			get
			{
				return this._offset.X;
			}
		}

		/// <summary>Gets the updated value of the vertical offset for a <see cref="T:System.Windows.Controls.ScrollViewer" />.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the updated vertical offset of a <see cref="T:System.Windows.Controls.ScrollViewer" />.</returns>
		// Token: 0x1700149F RID: 5279
		// (get) Token: 0x060054D9 RID: 21721 RVA: 0x00177AC6 File Offset: 0x00175CC6
		public double VerticalOffset
		{
			get
			{
				return this._offset.Y;
			}
		}

		/// <summary>Gets a value that indicates the change in horizontal offset for a <see cref="T:System.Windows.Controls.ScrollViewer" />.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the change in horizontal offset for a <see cref="T:System.Windows.Controls.ScrollViewer" />.</returns>
		// Token: 0x170014A0 RID: 5280
		// (get) Token: 0x060054DA RID: 21722 RVA: 0x00177AD3 File Offset: 0x00175CD3
		public double HorizontalChange
		{
			get
			{
				return this._offsetChange.X;
			}
		}

		/// <summary>Gets a value that indicates the change in vertical offset of a <see cref="T:System.Windows.Controls.ScrollViewer" />.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the change in vertical offset of a <see cref="T:System.Windows.Controls.ScrollViewer" />.</returns>
		// Token: 0x170014A1 RID: 5281
		// (get) Token: 0x060054DB RID: 21723 RVA: 0x00177AE0 File Offset: 0x00175CE0
		public double VerticalChange
		{
			get
			{
				return this._offsetChange.Y;
			}
		}

		/// <summary>Gets the updated value of the viewport width for a <see cref="T:System.Windows.Controls.ScrollViewer" />.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the updated value of the viewport width for a <see cref="T:System.Windows.Controls.ScrollViewer" />.</returns>
		// Token: 0x170014A2 RID: 5282
		// (get) Token: 0x060054DC RID: 21724 RVA: 0x00177AED File Offset: 0x00175CED
		public double ViewportWidth
		{
			get
			{
				return this._viewport.Width;
			}
		}

		/// <summary>Gets the updated value of the viewport height for a <see cref="T:System.Windows.Controls.ScrollViewer" />.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the updated viewport height of a <see cref="T:System.Windows.Controls.ScrollViewer" />.</returns>
		// Token: 0x170014A3 RID: 5283
		// (get) Token: 0x060054DD RID: 21725 RVA: 0x00177AFA File Offset: 0x00175CFA
		public double ViewportHeight
		{
			get
			{
				return this._viewport.Height;
			}
		}

		/// <summary>Gets a value that indicates the change in viewport width of a <see cref="T:System.Windows.Controls.ScrollViewer" />.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the change in viewport width for a <see cref="T:System.Windows.Controls.ScrollViewer" />.</returns>
		// Token: 0x170014A4 RID: 5284
		// (get) Token: 0x060054DE RID: 21726 RVA: 0x00177B07 File Offset: 0x00175D07
		public double ViewportWidthChange
		{
			get
			{
				return this._viewportChange.X;
			}
		}

		/// <summary>Gets a value that indicates the change in value of the viewport height for a <see cref="T:System.Windows.Controls.ScrollViewer" />.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the change in vertical viewport height for a <see cref="T:System.Windows.Controls.ScrollViewer" />.</returns>
		// Token: 0x170014A5 RID: 5285
		// (get) Token: 0x060054DF RID: 21727 RVA: 0x00177B14 File Offset: 0x00175D14
		public double ViewportHeightChange
		{
			get
			{
				return this._viewportChange.Y;
			}
		}

		/// <summary>Gets the updated width of the <see cref="T:System.Windows.Controls.ScrollViewer" /> extent.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the updated extent width.</returns>
		// Token: 0x170014A6 RID: 5286
		// (get) Token: 0x060054E0 RID: 21728 RVA: 0x00177B21 File Offset: 0x00175D21
		public double ExtentWidth
		{
			get
			{
				return this._extent.Width;
			}
		}

		/// <summary>Gets the updated height of the <see cref="T:System.Windows.Controls.ScrollViewer" /> extent.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the updated extent height.</returns>
		// Token: 0x170014A7 RID: 5287
		// (get) Token: 0x060054E1 RID: 21729 RVA: 0x00177B2E File Offset: 0x00175D2E
		public double ExtentHeight
		{
			get
			{
				return this._extent.Height;
			}
		}

		/// <summary>Gets a value that indicates the change in width of the <see cref="T:System.Windows.Controls.ScrollViewer" /> extent.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the change in extent width.</returns>
		// Token: 0x170014A8 RID: 5288
		// (get) Token: 0x060054E2 RID: 21730 RVA: 0x00177B3B File Offset: 0x00175D3B
		public double ExtentWidthChange
		{
			get
			{
				return this._extentChange.X;
			}
		}

		/// <summary>Gets a value that indicates the change in height of the <see cref="T:System.Windows.Controls.ScrollViewer" /> extent.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the change in extent height.</returns>
		// Token: 0x170014A9 RID: 5289
		// (get) Token: 0x060054E3 RID: 21731 RVA: 0x00177B48 File Offset: 0x00175D48
		public double ExtentHeightChange
		{
			get
			{
				return this._extentChange.Y;
			}
		}

		/// <summary>Performs proper type casting before invoking the type-safe <see cref="T:System.Windows.Controls.ScrollChangedEventHandler" /> delegate </summary>
		/// <param name="genericHandler">The event handler to invoke, in this case the <see cref="T:System.Windows.Controls.ScrollChangedEventHandler" /> delegate.</param>
		/// <param name="genericTarget">The target upon which the <paramref name="genericHandler" /> is invoked.</param>
		// Token: 0x060054E4 RID: 21732 RVA: 0x00177B58 File Offset: 0x00175D58
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			ScrollChangedEventHandler scrollChangedEventHandler = (ScrollChangedEventHandler)genericHandler;
			scrollChangedEventHandler(genericTarget, this);
		}

		// Token: 0x04002DB2 RID: 11698
		private Vector _offset;

		// Token: 0x04002DB3 RID: 11699
		private Vector _offsetChange;

		// Token: 0x04002DB4 RID: 11700
		private Size _extent;

		// Token: 0x04002DB5 RID: 11701
		private Vector _extentChange;

		// Token: 0x04002DB6 RID: 11702
		private Size _viewport;

		// Token: 0x04002DB7 RID: 11703
		private Vector _viewportChange;
	}
}
