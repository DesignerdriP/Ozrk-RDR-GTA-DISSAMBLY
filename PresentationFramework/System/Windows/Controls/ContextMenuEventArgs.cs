using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the context menu event. </summary>
	// Token: 0x0200048D RID: 1165
	public sealed class ContextMenuEventArgs : RoutedEventArgs
	{
		// Token: 0x0600447A RID: 17530 RVA: 0x00137830 File Offset: 0x00135A30
		internal ContextMenuEventArgs(object source, bool opening) : this(source, opening, -1.0, -1.0)
		{
		}

		// Token: 0x0600447B RID: 17531 RVA: 0x0013784C File Offset: 0x00135A4C
		internal ContextMenuEventArgs(object source, bool opening, double left, double top)
		{
			this._left = left;
			this._top = top;
			base.RoutedEvent = (opening ? ContextMenuService.ContextMenuOpeningEvent : ContextMenuService.ContextMenuClosingEvent);
			base.Source = source;
		}

		/// <summary> Gets the horizontal position of the mouse.  </summary>
		/// <returns>The horizontal position of the mouse.</returns>
		// Token: 0x170010CC RID: 4300
		// (get) Token: 0x0600447C RID: 17532 RVA: 0x0013787F File Offset: 0x00135A7F
		public double CursorLeft
		{
			get
			{
				return this._left;
			}
		}

		/// <summary>Gets the vertical position of the mouse.  </summary>
		/// <returns>The vertical position of the mouse. </returns>
		// Token: 0x170010CD RID: 4301
		// (get) Token: 0x0600447D RID: 17533 RVA: 0x00137887 File Offset: 0x00135A87
		public double CursorTop
		{
			get
			{
				return this._top;
			}
		}

		// Token: 0x170010CE RID: 4302
		// (get) Token: 0x0600447E RID: 17534 RVA: 0x0013788F File Offset: 0x00135A8F
		// (set) Token: 0x0600447F RID: 17535 RVA: 0x00137897 File Offset: 0x00135A97
		internal DependencyObject TargetElement
		{
			get
			{
				return this._targetElement;
			}
			set
			{
				this._targetElement = value;
			}
		}

		// Token: 0x06004480 RID: 17536 RVA: 0x001378A0 File Offset: 0x00135AA0
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			ContextMenuEventHandler contextMenuEventHandler = (ContextMenuEventHandler)genericHandler;
			contextMenuEventHandler(genericTarget, this);
		}

		// Token: 0x04002899 RID: 10393
		private double _left;

		// Token: 0x0400289A RID: 10394
		private double _top;

		// Token: 0x0400289B RID: 10395
		private DependencyObject _targetElement;
	}
}
