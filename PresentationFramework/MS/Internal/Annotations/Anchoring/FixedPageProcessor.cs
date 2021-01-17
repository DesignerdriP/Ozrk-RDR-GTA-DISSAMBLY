using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020007D1 RID: 2001
	internal class FixedPageProcessor : SubTreeProcessor
	{
		// Token: 0x06007BC5 RID: 31685 RVA: 0x0022C531 File Offset: 0x0022A731
		public FixedPageProcessor(LocatorManager manager) : base(manager)
		{
		}

		// Token: 0x06007BC6 RID: 31686 RVA: 0x0022C53C File Offset: 0x0022A73C
		public override IList<IAttachedAnnotation> PreProcessNode(DependencyObject node, out bool calledProcessAnnotations)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			DocumentPageView documentPageView = node as DocumentPageView;
			if (documentPageView != null && (documentPageView.DocumentPage is FixedDocumentPage || documentPageView.DocumentPage is FixedDocumentSequenceDocumentPage))
			{
				calledProcessAnnotations = true;
				return base.Manager.ProcessAnnotations(documentPageView);
			}
			calledProcessAnnotations = false;
			return null;
		}

		// Token: 0x06007BC7 RID: 31687 RVA: 0x0022C590 File Offset: 0x0022A790
		public override ContentLocator GenerateLocator(PathNode node, out bool continueGenerating)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			continueGenerating = true;
			ContentLocator contentLocator = null;
			DocumentPageView documentPageView = node.Node as DocumentPageView;
			int num = -1;
			if (documentPageView != null)
			{
				if (documentPageView.DocumentPage is FixedDocumentPage || documentPageView.DocumentPage is FixedDocumentSequenceDocumentPage)
				{
					num = documentPageView.PageNumber;
				}
			}
			else
			{
				FixedTextSelectionProcessor.FixedPageProxy fixedPageProxy = node.Node as FixedTextSelectionProcessor.FixedPageProxy;
				if (fixedPageProxy != null)
				{
					num = fixedPageProxy.Page;
				}
			}
			if (num >= 0)
			{
				contentLocator = new ContentLocator();
				ContentLocatorPart item = FixedPageProcessor.CreateLocatorPart(num);
				contentLocator.Parts.Add(item);
			}
			return contentLocator;
		}

		// Token: 0x06007BC8 RID: 31688 RVA: 0x0022C61C File Offset: 0x0022A81C
		public override DependencyObject ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out bool continueResolving)
		{
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			if (FixedPageProcessor.PageNumberElementName != locatorPart.PartType)
			{
				throw new ArgumentException(SR.Get("IncorrectLocatorPartType", new object[]
				{
					locatorPart.PartType.Namespace + ":" + locatorPart.PartType.Name
				}), "locatorPart");
			}
			continueResolving = true;
			string text = locatorPart.NameValuePairs[FixedPageProcessor.ValueAttributeName];
			if (text == null)
			{
				throw new ArgumentException(SR.Get("IncorrectLocatorPartType", new object[]
				{
					locatorPart.PartType.Namespace + ":" + locatorPart.PartType.Name
				}), "locatorPart");
			}
			int num = int.Parse(text, NumberFormatInfo.InvariantInfo);
			FixedDocumentPage fixedDocumentPage = null;
			if (this._useLogicalTree)
			{
				IDocumentPaginatorSource documentPaginatorSource = startNode as FixedDocument;
				if (documentPaginatorSource != null)
				{
					fixedDocumentPage = (documentPaginatorSource.DocumentPaginator.GetPage(num) as FixedDocumentPage);
				}
				else
				{
					documentPaginatorSource = (startNode as FixedDocumentSequence);
					if (documentPaginatorSource != null)
					{
						FixedDocumentSequenceDocumentPage fixedDocumentSequenceDocumentPage = documentPaginatorSource.DocumentPaginator.GetPage(num) as FixedDocumentSequenceDocumentPage;
						if (fixedDocumentSequenceDocumentPage != null)
						{
							fixedDocumentPage = (fixedDocumentSequenceDocumentPage.ChildDocumentPage as FixedDocumentPage);
						}
					}
				}
			}
			else
			{
				DocumentPageView documentPageView = startNode as DocumentPageView;
				if (documentPageView != null)
				{
					fixedDocumentPage = (documentPageView.DocumentPage as FixedDocumentPage);
					if (fixedDocumentPage == null)
					{
						FixedDocumentSequenceDocumentPage fixedDocumentSequenceDocumentPage2 = documentPageView.DocumentPage as FixedDocumentSequenceDocumentPage;
						if (fixedDocumentSequenceDocumentPage2 != null)
						{
							fixedDocumentPage = (fixedDocumentSequenceDocumentPage2.ChildDocumentPage as FixedDocumentPage);
						}
					}
					if (fixedDocumentPage != null && documentPageView.PageNumber != num)
					{
						continueResolving = false;
						fixedDocumentPage = null;
					}
				}
			}
			if (fixedDocumentPage != null)
			{
				return fixedDocumentPage.FixedPage;
			}
			return null;
		}

		// Token: 0x06007BC9 RID: 31689 RVA: 0x0022C7AF File Offset: 0x0022A9AF
		public override XmlQualifiedName[] GetLocatorPartTypes()
		{
			return (XmlQualifiedName[])FixedPageProcessor.LocatorPartTypeNames.Clone();
		}

		// Token: 0x17001CDE RID: 7390
		// (set) Token: 0x06007BCA RID: 31690 RVA: 0x0022C7C0 File Offset: 0x0022A9C0
		internal bool UseLogicalTree
		{
			set
			{
				this._useLogicalTree = value;
			}
		}

		// Token: 0x06007BCB RID: 31691 RVA: 0x0022C7CC File Offset: 0x0022A9CC
		internal static ContentLocatorPart CreateLocatorPart(int page)
		{
			return new ContentLocatorPart(FixedPageProcessor.PageNumberElementName)
			{
				NameValuePairs = 
				{
					{
						FixedPageProcessor.ValueAttributeName,
						page.ToString(NumberFormatInfo.InvariantInfo)
					}
				}
			};
		}

		// Token: 0x04003A38 RID: 14904
		public static readonly string Id = "FixedPage";

		// Token: 0x04003A39 RID: 14905
		private static readonly string ValueAttributeName = "Value";

		// Token: 0x04003A3A RID: 14906
		private static readonly XmlQualifiedName PageNumberElementName = new XmlQualifiedName("PageNumber", "http://schemas.microsoft.com/windows/annotations/2003/11/base");

		// Token: 0x04003A3B RID: 14907
		private static readonly XmlQualifiedName[] LocatorPartTypeNames = new XmlQualifiedName[]
		{
			FixedPageProcessor.PageNumberElementName
		};

		// Token: 0x04003A3C RID: 14908
		private bool _useLogicalTree;
	}
}
