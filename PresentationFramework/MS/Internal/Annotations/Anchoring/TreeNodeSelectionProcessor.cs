using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Media;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020007DC RID: 2012
	internal sealed class TreeNodeSelectionProcessor : SelectionProcessor
	{
		// Token: 0x06007C5C RID: 31836 RVA: 0x0022FAAE File Offset: 0x0022DCAE
		public override bool MergeSelections(object selection1, object selection2, out object newSelection)
		{
			if (selection1 == null)
			{
				throw new ArgumentNullException("selection1");
			}
			if (selection2 == null)
			{
				throw new ArgumentNullException("selection2");
			}
			newSelection = null;
			return false;
		}

		// Token: 0x06007C5D RID: 31837 RVA: 0x0022FAD0 File Offset: 0x0022DCD0
		public override IList<DependencyObject> GetSelectedNodes(object selection)
		{
			return new DependencyObject[]
			{
				this.GetParent(selection)
			};
		}

		// Token: 0x06007C5E RID: 31838 RVA: 0x0022FAE4 File Offset: 0x0022DCE4
		public override UIElement GetParent(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			UIElement uielement = selection as UIElement;
			if (uielement == null)
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "selection");
			}
			return uielement;
		}

		// Token: 0x06007C5F RID: 31839 RVA: 0x0022FB20 File Offset: 0x0022DD20
		public override Point GetAnchorPoint(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			Visual visual = selection as Visual;
			if (visual == null)
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "selection");
			}
			Rect visualContentBounds = visual.VisualContentBounds;
			return new Point(visualContentBounds.Left, visualContentBounds.Top);
		}

		// Token: 0x06007C60 RID: 31840 RVA: 0x0022FB74 File Offset: 0x0022DD74
		public override IList<ContentLocatorPart> GenerateLocatorParts(object selection, DependencyObject startNode)
		{
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			return new List<ContentLocatorPart>(0);
		}

		// Token: 0x06007C61 RID: 31841 RVA: 0x0022FB98 File Offset: 0x0022DD98
		public override object ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out AttachmentLevel attachmentLevel)
		{
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			attachmentLevel = AttachmentLevel.Full;
			return startNode;
		}

		// Token: 0x06007C62 RID: 31842 RVA: 0x0022FBBA File Offset: 0x0022DDBA
		public override XmlQualifiedName[] GetLocatorPartTypes()
		{
			return (XmlQualifiedName[])TreeNodeSelectionProcessor.LocatorPartTypeNames.Clone();
		}

		// Token: 0x04003A61 RID: 14945
		private static readonly XmlQualifiedName[] LocatorPartTypeNames = new XmlQualifiedName[0];
	}
}
