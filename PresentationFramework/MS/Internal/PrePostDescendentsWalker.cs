using System;
using System.Windows;

namespace MS.Internal
{
	// Token: 0x020005ED RID: 1517
	internal class PrePostDescendentsWalker<T> : DescendentsWalker<T>
	{
		// Token: 0x0600652E RID: 25902 RVA: 0x001C66D8 File Offset: 0x001C48D8
		public PrePostDescendentsWalker(TreeWalkPriority priority, VisitedCallback<T> preCallback, VisitedCallback<T> postCallback, T data) : base(priority, preCallback, data)
		{
			this._postCallback = postCallback;
		}

		// Token: 0x0600652F RID: 25903 RVA: 0x001C66EC File Offset: 0x001C48EC
		public override void StartWalk(DependencyObject startNode, bool skipStartNode)
		{
			try
			{
				base.StartWalk(startNode, skipStartNode);
			}
			finally
			{
				if (!skipStartNode && this._postCallback != null && (FrameworkElement.DType.IsInstanceOfType(startNode) || FrameworkContentElement.DType.IsInstanceOfType(startNode)))
				{
					this._postCallback(startNode, base.Data, this._priority == TreeWalkPriority.VisualTree);
				}
			}
		}

		// Token: 0x06006530 RID: 25904 RVA: 0x001C6758 File Offset: 0x001C4958
		protected override void _VisitNode(DependencyObject d, bool visitedViaVisualTree)
		{
			try
			{
				base._VisitNode(d, visitedViaVisualTree);
			}
			finally
			{
				if (this._postCallback != null)
				{
					this._postCallback(d, base.Data, visitedViaVisualTree);
				}
			}
		}

		// Token: 0x040032BB RID: 12987
		private VisitedCallback<T> _postCallback;
	}
}
