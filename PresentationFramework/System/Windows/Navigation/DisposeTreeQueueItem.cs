using System;
using System.Collections;
using System.Windows.Controls;

namespace System.Windows.Navigation
{
	// Token: 0x0200031D RID: 797
	internal class DisposeTreeQueueItem
	{
		// Token: 0x06002A08 RID: 10760 RVA: 0x000C1B8F File Offset: 0x000BFD8F
		internal object Dispatch(object o)
		{
			this.DisposeElement(this._root);
			return null;
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x000C1BA0 File Offset: 0x000BFDA0
		internal void DisposeElement(object node)
		{
			DependencyObject dependencyObject = node as DependencyObject;
			if (dependencyObject != null)
			{
				bool flag = false;
				IEnumerator logicalChildren = LogicalTreeHelper.GetLogicalChildren(dependencyObject);
				if (logicalChildren != null)
				{
					while (logicalChildren.MoveNext())
					{
						flag = true;
						object node2 = logicalChildren.Current;
						this.DisposeElement(node2);
					}
				}
				if (!flag)
				{
					ContentControl contentControl = dependencyObject as ContentControl;
					if (contentControl != null && contentControl.ContentIsNotLogical && contentControl.Content != null)
					{
						this.DisposeElement(contentControl.Content);
					}
				}
			}
			IDisposable disposable = node as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}

		// Token: 0x06002A0A RID: 10762 RVA: 0x000C1C1D File Offset: 0x000BFE1D
		internal DisposeTreeQueueItem(object node)
		{
			this._root = node;
		}

		// Token: 0x04001C2B RID: 7211
		private object _root;
	}
}
