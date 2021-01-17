using System;
using System.Collections;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Media;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020007D6 RID: 2006
	internal sealed class PathNode
	{
		// Token: 0x06007C16 RID: 31766 RVA: 0x0022E752 File Offset: 0x0022C952
		internal PathNode(DependencyObject node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			this._node = node;
		}

		// Token: 0x06007C17 RID: 31767 RVA: 0x0022E77C File Offset: 0x0022C97C
		public override bool Equals(object obj)
		{
			PathNode pathNode = obj as PathNode;
			return pathNode != null && this._node.Equals(pathNode.Node);
		}

		// Token: 0x06007C18 RID: 31768 RVA: 0x0022E7A6 File Offset: 0x0022C9A6
		public override int GetHashCode()
		{
			if (this._node == null)
			{
				return base.GetHashCode();
			}
			return this._node.GetHashCode();
		}

		// Token: 0x17001CE8 RID: 7400
		// (get) Token: 0x06007C19 RID: 31769 RVA: 0x0022E7C2 File Offset: 0x0022C9C2
		public DependencyObject Node
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x17001CE9 RID: 7401
		// (get) Token: 0x06007C1A RID: 31770 RVA: 0x0022E7CA File Offset: 0x0022C9CA
		public IList Children
		{
			get
			{
				return this._children;
			}
		}

		// Token: 0x06007C1B RID: 31771 RVA: 0x0022E7D4 File Offset: 0x0022C9D4
		internal static PathNode BuildPathForElements(ICollection nodes)
		{
			if (nodes == null)
			{
				throw new ArgumentNullException("nodes");
			}
			PathNode pathNode = null;
			foreach (object obj in nodes)
			{
				DependencyObject node = (DependencyObject)obj;
				PathNode pathNode2 = PathNode.BuildPathForElement(node);
				if (pathNode == null)
				{
					pathNode = pathNode2;
				}
				else
				{
					PathNode.AddBranchToPath(pathNode, pathNode2);
				}
			}
			if (pathNode != null)
			{
				pathNode.FreezeChildren();
			}
			return pathNode;
		}

		// Token: 0x06007C1C RID: 31772 RVA: 0x0022E854 File Offset: 0x0022CA54
		internal static DependencyObject GetParent(DependencyObject node)
		{
			DependencyObject dependencyObject = node;
			DependencyObject dependencyObject2;
			for (;;)
			{
				dependencyObject2 = (DependencyObject)dependencyObject.GetValue(PathNode.HiddenParentProperty);
				if (dependencyObject2 == null)
				{
					Visual visual = dependencyObject as Visual;
					if (visual != null)
					{
						dependencyObject2 = VisualTreeHelper.GetParent(visual);
					}
				}
				if (dependencyObject2 == null)
				{
					dependencyObject2 = LogicalTreeHelper.GetParent(dependencyObject);
				}
				if (dependencyObject2 == null || FrameworkElement.DType.IsInstanceOfType(dependencyObject2) || FrameworkContentElement.DType.IsInstanceOfType(dependencyObject2))
				{
					break;
				}
				dependencyObject = dependencyObject2;
			}
			return dependencyObject2;
		}

		// Token: 0x06007C1D RID: 31773 RVA: 0x0022E8B8 File Offset: 0x0022CAB8
		private static PathNode BuildPathForElement(DependencyObject node)
		{
			PathNode pathNode = null;
			while (node != null)
			{
				PathNode pathNode2 = new PathNode(node);
				if (pathNode != null)
				{
					pathNode2.AddChild(pathNode);
				}
				pathNode = pathNode2;
				if (node.ReadLocalValue(AnnotationService.ServiceProperty) != DependencyProperty.UnsetValue)
				{
					break;
				}
				node = PathNode.GetParent(node);
			}
			return pathNode;
		}

		// Token: 0x06007C1E RID: 31774 RVA: 0x0022E8FC File Offset: 0x0022CAFC
		private static PathNode AddBranchToPath(PathNode path, PathNode branch)
		{
			PathNode pathNode = path;
			PathNode pathNode2 = branch;
			while (pathNode.Node.Equals(pathNode2.Node) && pathNode2._children.Count > 0)
			{
				bool flag = false;
				PathNode pathNode3 = (PathNode)pathNode2._children[0];
				foreach (object obj in pathNode._children)
				{
					PathNode pathNode4 = (PathNode)obj;
					if (pathNode4.Equals(pathNode3))
					{
						flag = true;
						pathNode2 = pathNode3;
						pathNode = pathNode4;
						break;
					}
				}
				if (!flag)
				{
					pathNode.AddChild(pathNode3);
					break;
				}
			}
			return path;
		}

		// Token: 0x06007C1F RID: 31775 RVA: 0x0022E9B4 File Offset: 0x0022CBB4
		private void AddChild(object child)
		{
			this._children.Add(child);
		}

		// Token: 0x06007C20 RID: 31776 RVA: 0x0022E9C4 File Offset: 0x0022CBC4
		private void FreezeChildren()
		{
			foreach (object obj in this._children)
			{
				PathNode pathNode = (PathNode)obj;
				pathNode.FreezeChildren();
			}
			this._children = ArrayList.ReadOnly(this._children);
		}

		// Token: 0x04003A54 RID: 14932
		internal static readonly DependencyProperty HiddenParentProperty = DependencyProperty.RegisterAttached("HiddenParent", typeof(DependencyObject), typeof(PathNode));

		// Token: 0x04003A55 RID: 14933
		private DependencyObject _node;

		// Token: 0x04003A56 RID: 14934
		private ArrayList _children = new ArrayList(1);
	}
}
