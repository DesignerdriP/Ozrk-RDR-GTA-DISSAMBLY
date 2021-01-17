using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000635 RID: 1589
	internal class PageVisual : DrawingVisual, IContentHost
	{
		// Token: 0x060068F1 RID: 26865 RVA: 0x001D9A76 File Offset: 0x001D7C76
		internal PageVisual(FlowDocumentPage owner)
		{
			this._owner = new WeakReference(owner);
		}

		// Token: 0x060068F2 RID: 26866 RVA: 0x001D9A8C File Offset: 0x001D7C8C
		internal void DrawBackground(Brush backgroundBrush, Rect renderBounds)
		{
			if (this._backgroundBrush != backgroundBrush || this._renderBounds != renderBounds)
			{
				this._backgroundBrush = backgroundBrush;
				this._renderBounds = renderBounds;
				using (DrawingContext drawingContext = base.RenderOpen())
				{
					if (this._backgroundBrush != null)
					{
						drawingContext.DrawRectangle(this._backgroundBrush, null, this._renderBounds);
					}
					else
					{
						drawingContext.DrawRectangle(Brushes.Transparent, null, this._renderBounds);
					}
				}
			}
		}

		// Token: 0x17001966 RID: 6502
		// (get) Token: 0x060068F3 RID: 26867 RVA: 0x001D9B10 File Offset: 0x001D7D10
		// (set) Token: 0x060068F4 RID: 26868 RVA: 0x001D9B38 File Offset: 0x001D7D38
		internal Visual Child
		{
			get
			{
				VisualCollection children = base.Children;
				if (children.Count != 0)
				{
					return children[0];
				}
				return null;
			}
			set
			{
				VisualCollection children = base.Children;
				if (children.Count == 0)
				{
					children.Add(value);
					return;
				}
				if (children[0] != value)
				{
					children[0] = value;
				}
			}
		}

		// Token: 0x060068F5 RID: 26869 RVA: 0x001D9B70 File Offset: 0x001D7D70
		internal void ClearDrawingContext()
		{
			DrawingContext drawingContext = base.RenderOpen();
			if (drawingContext != null)
			{
				drawingContext.Close();
			}
		}

		// Token: 0x060068F6 RID: 26870 RVA: 0x001D9B90 File Offset: 0x001D7D90
		IInputElement IContentHost.InputHitTest(Point point)
		{
			IContentHost contentHost = this._owner.Target as IContentHost;
			if (contentHost != null)
			{
				return contentHost.InputHitTest(point);
			}
			return null;
		}

		// Token: 0x060068F7 RID: 26871 RVA: 0x001D9BBC File Offset: 0x001D7DBC
		ReadOnlyCollection<Rect> IContentHost.GetRectangles(ContentElement child)
		{
			IContentHost contentHost = this._owner.Target as IContentHost;
			if (contentHost != null)
			{
				return contentHost.GetRectangles(child);
			}
			return new ReadOnlyCollection<Rect>(new List<Rect>(0));
		}

		// Token: 0x17001967 RID: 6503
		// (get) Token: 0x060068F8 RID: 26872 RVA: 0x001D9BF0 File Offset: 0x001D7DF0
		IEnumerator<IInputElement> IContentHost.HostedElements
		{
			get
			{
				IContentHost contentHost = this._owner.Target as IContentHost;
				if (contentHost != null)
				{
					return contentHost.HostedElements;
				}
				return null;
			}
		}

		// Token: 0x060068F9 RID: 26873 RVA: 0x001D9C1C File Offset: 0x001D7E1C
		void IContentHost.OnChildDesiredSizeChanged(UIElement child)
		{
			IContentHost contentHost = this._owner.Target as IContentHost;
			if (contentHost != null)
			{
				contentHost.OnChildDesiredSizeChanged(child);
			}
		}

		// Token: 0x04003400 RID: 13312
		private readonly WeakReference _owner;

		// Token: 0x04003401 RID: 13313
		private Brush _backgroundBrush;

		// Token: 0x04003402 RID: 13314
		private Rect _renderBounds;
	}
}
