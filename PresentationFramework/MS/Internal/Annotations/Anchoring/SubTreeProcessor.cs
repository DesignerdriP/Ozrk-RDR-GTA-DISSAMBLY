using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Annotations;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020007D8 RID: 2008
	internal abstract class SubTreeProcessor
	{
		// Token: 0x06007C2A RID: 31786 RVA: 0x0022EA55 File Offset: 0x0022CC55
		protected SubTreeProcessor(LocatorManager manager)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			this._manager = manager;
		}

		// Token: 0x06007C2B RID: 31787
		public abstract IList<IAttachedAnnotation> PreProcessNode(DependencyObject node, out bool calledProcessAnnotations);

		// Token: 0x06007C2C RID: 31788 RVA: 0x0022EA72 File Offset: 0x0022CC72
		public virtual IList<IAttachedAnnotation> PostProcessNode(DependencyObject node, bool childrenCalledProcessAnnotations, out bool calledProcessAnnotations)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			calledProcessAnnotations = false;
			return null;
		}

		// Token: 0x06007C2D RID: 31789
		public abstract ContentLocator GenerateLocator(PathNode node, out bool continueGenerating);

		// Token: 0x06007C2E RID: 31790
		public abstract DependencyObject ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out bool continueResolving);

		// Token: 0x06007C2F RID: 31791
		public abstract XmlQualifiedName[] GetLocatorPartTypes();

		// Token: 0x17001CEA RID: 7402
		// (get) Token: 0x06007C30 RID: 31792 RVA: 0x0022EA86 File Offset: 0x0022CC86
		protected LocatorManager Manager
		{
			get
			{
				return this._manager;
			}
		}

		// Token: 0x04003A57 RID: 14935
		private LocatorManager _manager;
	}
}
