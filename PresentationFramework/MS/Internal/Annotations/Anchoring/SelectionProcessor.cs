using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Annotations;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020007D7 RID: 2007
	internal abstract class SelectionProcessor
	{
		// Token: 0x06007C23 RID: 31779
		public abstract bool MergeSelections(object selection1, object selection2, out object newSelection);

		// Token: 0x06007C24 RID: 31780
		public abstract IList<DependencyObject> GetSelectedNodes(object selection);

		// Token: 0x06007C25 RID: 31781
		public abstract UIElement GetParent(object selection);

		// Token: 0x06007C26 RID: 31782
		public abstract Point GetAnchorPoint(object selection);

		// Token: 0x06007C27 RID: 31783
		public abstract IList<ContentLocatorPart> GenerateLocatorParts(object selection, DependencyObject startNode);

		// Token: 0x06007C28 RID: 31784
		public abstract object ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out AttachmentLevel attachmentLevel);

		// Token: 0x06007C29 RID: 31785
		public abstract XmlQualifiedName[] GetLocatorPartTypes();
	}
}
