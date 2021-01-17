using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Provides information about the <see cref="E:System.Windows.Controls.Primitives.Thumb.DragStarted" /> event that occurs when a user drags a <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control with the mouse..</summary>
	// Token: 0x02000589 RID: 1417
	public class DragStartedEventArgs : RoutedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Primitives.DragStartedEventArgs" /> class.</summary>
		/// <param name="horizontalOffset">The horizontal offset of the mouse click with respect to the screen coordinates of the <see cref="T:System.Windows.Controls.Primitives.Thumb" />.</param>
		/// <param name="verticalOffset">The vertical offset of the mouse click with respect to the screen coordinates of the <see cref="T:System.Windows.Controls.Primitives.Thumb" />.</param>
		// Token: 0x06005DF3 RID: 24051 RVA: 0x001A6E74 File Offset: 0x001A5074
		public DragStartedEventArgs(double horizontalOffset, double verticalOffset)
		{
			this._horizontalOffset = horizontalOffset;
			this._verticalOffset = verticalOffset;
			base.RoutedEvent = Thumb.DragStartedEvent;
		}

		/// <summary>Gets the horizontal offset of the mouse click relative to the screen coordinates of the <see cref="T:System.Windows.Controls.Primitives.Thumb" />.</summary>
		/// <returns>The horizontal offset of the mouse click with respect to the upper-left corner of the bounding box of the <see cref="T:System.Windows.Controls.Primitives.Thumb" />. There is no default value.</returns>
		// Token: 0x170016AF RID: 5807
		// (get) Token: 0x06005DF4 RID: 24052 RVA: 0x001A6E95 File Offset: 0x001A5095
		public double HorizontalOffset
		{
			get
			{
				return this._horizontalOffset;
			}
		}

		/// <summary>Gets the vertical offset of the mouse click relative to the screen coordinates of the <see cref="T:System.Windows.Controls.Primitives.Thumb" />.</summary>
		/// <returns>The horizontal offset of the mouse click with respect to the upper-left corner of the bounding box of the <see cref="T:System.Windows.Controls.Primitives.Thumb" />. There is no default value.</returns>
		// Token: 0x170016B0 RID: 5808
		// (get) Token: 0x06005DF5 RID: 24053 RVA: 0x001A6E9D File Offset: 0x001A509D
		public double VerticalOffset
		{
			get
			{
				return this._verticalOffset;
			}
		}

		/// <summary>Converts a method that handles the <see cref="E:System.Windows.Controls.Primitives.Thumb.DragStarted" /> event to the <see cref="T:System.Windows.Controls.Primitives.DragStartedEventHandler" /> type.</summary>
		/// <param name="genericHandler">The event handler delegate.</param>
		/// <param name="genericTarget">The <see cref="T:System.Windows.Controls.Primitives.Thumb" /> that uses the handler.</param>
		// Token: 0x06005DF6 RID: 24054 RVA: 0x001A6EA8 File Offset: 0x001A50A8
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			DragStartedEventHandler dragStartedEventHandler = (DragStartedEventHandler)genericHandler;
			dragStartedEventHandler(genericTarget, this);
		}

		// Token: 0x0400303F RID: 12351
		private double _horizontalOffset;

		// Token: 0x04003040 RID: 12352
		private double _verticalOffset;
	}
}
