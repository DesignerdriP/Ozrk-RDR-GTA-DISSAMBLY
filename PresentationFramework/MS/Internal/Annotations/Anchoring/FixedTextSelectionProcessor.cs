using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020007D2 RID: 2002
	internal class FixedTextSelectionProcessor : SelectionProcessor
	{
		// Token: 0x06007BCE RID: 31694 RVA: 0x0022C846 File Offset: 0x0022AA46
		public override bool MergeSelections(object anchor1, object anchor2, out object newAnchor)
		{
			return TextSelectionHelper.MergeSelections(anchor1, anchor2, out newAnchor);
		}

		// Token: 0x06007BCF RID: 31695 RVA: 0x0022C850 File Offset: 0x0022AA50
		public override IList<DependencyObject> GetSelectedNodes(object selection)
		{
			IList<TextSegment> list = this.CheckSelection(selection);
			IList<DependencyObject> list2 = new List<DependencyObject>();
			foreach (TextSegment textSegment in list)
			{
				int minValue = int.MinValue;
				ITextPointer pointer = textSegment.Start.CreatePointer(LogicalDirection.Forward);
				TextSelectionHelper.GetPointerPage(pointer, out minValue);
				Point pointForPointer = TextSelectionHelper.GetPointForPointer(pointer);
				if (minValue == -2147483648)
				{
					throw new ArgumentException(SR.Get("SelectionDoesNotResolveToAPage", new object[]
					{
						"start"
					}), "selection");
				}
				int minValue2 = int.MinValue;
				ITextPointer pointer2 = textSegment.End.CreatePointer(LogicalDirection.Backward);
				TextSelectionHelper.GetPointerPage(pointer2, out minValue2);
				Point pointForPointer2 = TextSelectionHelper.GetPointForPointer(pointer2);
				if (minValue2 == -2147483648)
				{
					throw new ArgumentException(SR.Get("SelectionDoesNotResolveToAPage", new object[]
					{
						"end"
					}), "selection");
				}
				int num = list2.Count;
				int num2 = minValue2 - minValue;
				int i = 0;
				if (list2.Count > 0 && ((FixedTextSelectionProcessor.FixedPageProxy)list2[list2.Count - 1]).Page == minValue)
				{
					num--;
					i++;
				}
				while (i <= num2)
				{
					list2.Add(new FixedTextSelectionProcessor.FixedPageProxy(textSegment.Start.TextContainer.Parent, minValue + i));
					i++;
				}
				if (num2 == 0)
				{
					((FixedTextSelectionProcessor.FixedPageProxy)list2[num]).Segments.Add(new FixedTextSelectionProcessor.PointSegment(pointForPointer, pointForPointer2));
				}
				else
				{
					((FixedTextSelectionProcessor.FixedPageProxy)list2[num]).Segments.Add(new FixedTextSelectionProcessor.PointSegment(pointForPointer, FixedTextSelectionProcessor.PointSegment.NotAPoint));
					((FixedTextSelectionProcessor.FixedPageProxy)list2[num + num2]).Segments.Add(new FixedTextSelectionProcessor.PointSegment(FixedTextSelectionProcessor.PointSegment.NotAPoint, pointForPointer2));
				}
			}
			return list2;
		}

		// Token: 0x06007BD0 RID: 31696 RVA: 0x0022CA40 File Offset: 0x0022AC40
		public override UIElement GetParent(object selection)
		{
			this.CheckAnchor(selection);
			return TextSelectionHelper.GetParent(selection);
		}

		// Token: 0x06007BD1 RID: 31697 RVA: 0x0022CA50 File Offset: 0x0022AC50
		public override Point GetAnchorPoint(object selection)
		{
			this.CheckAnchor(selection);
			return TextSelectionHelper.GetAnchorPoint(selection);
		}

		// Token: 0x06007BD2 RID: 31698 RVA: 0x0022CA60 File Offset: 0x0022AC60
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
			this.CheckSelection(selection);
			FixedTextSelectionProcessor.FixedPageProxy fixedPageProxy = startNode as FixedTextSelectionProcessor.FixedPageProxy;
			if (fixedPageProxy == null)
			{
				throw new ArgumentException(SR.Get("StartNodeMustBeFixedPageProxy"), "startNode");
			}
			ContentLocatorPart contentLocatorPart = new ContentLocatorPart(FixedTextSelectionProcessor.FixedTextElementName);
			if (fixedPageProxy.Segments.Count == 0)
			{
				contentLocatorPart.NameValuePairs.Add("Count", 1.ToString(NumberFormatInfo.InvariantInfo));
				contentLocatorPart.NameValuePairs.Add("Segment" + 0.ToString(NumberFormatInfo.InvariantInfo), ",,,");
			}
			else
			{
				contentLocatorPart.NameValuePairs.Add("Count", fixedPageProxy.Segments.Count.ToString(NumberFormatInfo.InvariantInfo));
				for (int i = 0; i < fixedPageProxy.Segments.Count; i++)
				{
					string text = "";
					if (!double.IsNaN(fixedPageProxy.Segments[i].Start.X))
					{
						text = text + fixedPageProxy.Segments[i].Start.X.ToString(NumberFormatInfo.InvariantInfo) + TextSelectionProcessor.Separator[0].ToString() + fixedPageProxy.Segments[i].Start.Y.ToString(NumberFormatInfo.InvariantInfo);
					}
					else
					{
						text += TextSelectionProcessor.Separator[0].ToString();
					}
					text += TextSelectionProcessor.Separator[0].ToString();
					if (!double.IsNaN(fixedPageProxy.Segments[i].End.X))
					{
						text = text + fixedPageProxy.Segments[i].End.X.ToString(NumberFormatInfo.InvariantInfo) + TextSelectionProcessor.Separator[0].ToString() + fixedPageProxy.Segments[i].End.Y.ToString(NumberFormatInfo.InvariantInfo);
					}
					else
					{
						text += TextSelectionProcessor.Separator[0].ToString();
					}
					contentLocatorPart.NameValuePairs.Add("Segment" + i.ToString(NumberFormatInfo.InvariantInfo), text);
				}
			}
			return new List<ContentLocatorPart>(1)
			{
				contentLocatorPart
			};
		}

		// Token: 0x06007BD3 RID: 31699 RVA: 0x0022CD00 File Offset: 0x0022AF00
		public override object ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out AttachmentLevel attachmentLevel)
		{
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			DocumentPage documentPage = null;
			FixedPage fixedPage = startNode as FixedPage;
			if (fixedPage != null)
			{
				documentPage = this.GetDocumentPage(fixedPage);
			}
			else
			{
				DocumentPageView documentPageView = startNode as DocumentPageView;
				if (documentPageView != null)
				{
					documentPage = (documentPageView.DocumentPage as FixedDocumentPage);
					if (documentPage == null)
					{
						documentPage = (documentPageView.DocumentPage as FixedDocumentSequenceDocumentPage);
					}
				}
			}
			if (documentPage == null)
			{
				throw new ArgumentException(SR.Get("StartNodeMustBeDocumentPageViewOrFixedPage"), "startNode");
			}
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			attachmentLevel = AttachmentLevel.Unresolved;
			ITextView textView = (ITextView)((IServiceProvider)documentPage).GetService(typeof(ITextView));
			ReadOnlyCollection<TextSegment> textSegments = textView.TextSegments;
			if (textSegments == null || textSegments.Count <= 0)
			{
				return null;
			}
			TextAnchor textAnchor = new TextAnchor();
			if (documentPage != null)
			{
				string text = locatorPart.NameValuePairs["Count"];
				if (text == null)
				{
					throw new ArgumentException(SR.Get("InvalidLocatorPart", new object[]
					{
						"Count"
					}));
				}
				int num = int.Parse(text, NumberFormatInfo.InvariantInfo);
				for (int i = 0; i < num; i++)
				{
					Point point;
					Point point2;
					this.GetLocatorPartSegmentValues(locatorPart, i, out point, out point2);
					ITextPointer textPointer;
					if (double.IsNaN(point.X) || double.IsNaN(point.Y))
					{
						textPointer = FixedTextSelectionProcessor.FindStartVisibleTextPointer(documentPage);
					}
					else
					{
						textPointer = textView.GetTextPositionFromPoint(point, true);
					}
					if (textPointer != null)
					{
						ITextPointer textPointer2;
						if (double.IsNaN(point2.X) || double.IsNaN(point2.Y))
						{
							textPointer2 = FixedTextSelectionProcessor.FindEndVisibleTextPointer(documentPage);
						}
						else
						{
							textPointer2 = textView.GetTextPositionFromPoint(point2, true);
						}
						Invariant.Assert(textPointer2 != null, "end TP is null when start TP is not");
						attachmentLevel = AttachmentLevel.Full;
						textAnchor.AddTextSegment(textPointer, textPointer2);
					}
				}
			}
			if (textAnchor.TextSegments.Count > 0)
			{
				return textAnchor;
			}
			return null;
		}

		// Token: 0x06007BD4 RID: 31700 RVA: 0x0022CEBC File Offset: 0x0022B0BC
		public override XmlQualifiedName[] GetLocatorPartTypes()
		{
			return (XmlQualifiedName[])FixedTextSelectionProcessor.LocatorPartTypeNames.Clone();
		}

		// Token: 0x06007BD5 RID: 31701 RVA: 0x0022CED0 File Offset: 0x0022B0D0
		private DocumentPage GetDocumentPage(FixedPage page)
		{
			Invariant.Assert(page != null);
			DocumentPage result = null;
			PageContent pageContent = page.Parent as PageContent;
			if (pageContent != null)
			{
				FixedDocument fixedDocument = pageContent.Parent as FixedDocument;
				FixedDocumentSequence fixedDocumentSequence = fixedDocument.Parent as FixedDocumentSequence;
				if (fixedDocumentSequence != null)
				{
					result = fixedDocumentSequence.GetPage(fixedDocument, fixedDocument.GetIndexOfPage(page));
				}
				else
				{
					result = fixedDocument.GetPage(fixedDocument.GetIndexOfPage(page));
				}
			}
			return result;
		}

		// Token: 0x06007BD6 RID: 31702 RVA: 0x0022CF34 File Offset: 0x0022B134
		private IList<TextSegment> CheckSelection(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			ITextRange textRange = selection as ITextRange;
			ITextPointer start;
			IList<TextSegment> textSegments;
			if (textRange != null)
			{
				start = textRange.Start;
				textSegments = textRange.TextSegments;
			}
			else
			{
				TextAnchor textAnchor = selection as TextAnchor;
				if (textAnchor == null)
				{
					throw new ArgumentException(SR.Get("WrongSelectionType"), "selection: type=" + selection.GetType().ToString());
				}
				start = textAnchor.Start;
				textSegments = textAnchor.TextSegments;
			}
			if (!(start.TextContainer is FixedTextContainer) && !(start.TextContainer is DocumentSequenceTextContainer))
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "selection: type=" + selection.GetType().ToString());
			}
			return textSegments;
		}

		// Token: 0x06007BD7 RID: 31703 RVA: 0x0022CFEC File Offset: 0x0022B1EC
		private TextAnchor CheckAnchor(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			TextAnchor textAnchor = selection as TextAnchor;
			if (textAnchor == null || (!(textAnchor.Start.TextContainer is FixedTextContainer) && !(textAnchor.Start.TextContainer is DocumentSequenceTextContainer)))
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "selection: type=" + selection.GetType().ToString());
			}
			return textAnchor;
		}

		// Token: 0x06007BD8 RID: 31704 RVA: 0x0022D05C File Offset: 0x0022B25C
		private void GetLocatorPartSegmentValues(ContentLocatorPart locatorPart, int segmentNumber, out Point start, out Point end)
		{
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			if (FixedTextSelectionProcessor.FixedTextElementName != locatorPart.PartType)
			{
				throw new ArgumentException(SR.Get("IncorrectLocatorPartType", new object[]
				{
					locatorPart.PartType.Namespace + ":" + locatorPart.PartType.Name
				}), "locatorPart");
			}
			string text = locatorPart.NameValuePairs["Segment" + segmentNumber.ToString(NumberFormatInfo.InvariantInfo)];
			if (text == null)
			{
				throw new ArgumentException(SR.Get("InvalidLocatorPart", new object[]
				{
					"Segment" + segmentNumber.ToString(NumberFormatInfo.InvariantInfo)
				}));
			}
			string[] array = text.Split(TextSelectionProcessor.Separator);
			if (array.Length != 4)
			{
				throw new ArgumentException(SR.Get("InvalidLocatorPart", new object[]
				{
					"Segment" + segmentNumber.ToString(NumberFormatInfo.InvariantInfo)
				}));
			}
			start = this.GetPoint(array[0], array[1]);
			end = this.GetPoint(array[2], array[3]);
		}

		// Token: 0x06007BD9 RID: 31705 RVA: 0x0022D184 File Offset: 0x0022B384
		private Point GetPoint(string xstr, string ystr)
		{
			Point result;
			if (xstr != null && !string.IsNullOrEmpty(xstr.Trim()) && ystr != null && !string.IsNullOrEmpty(ystr.Trim()))
			{
				double x = double.Parse(xstr, NumberFormatInfo.InvariantInfo);
				double y = double.Parse(ystr, NumberFormatInfo.InvariantInfo);
				result = new Point(x, y);
			}
			else
			{
				result = new Point(double.NaN, double.NaN);
			}
			return result;
		}

		// Token: 0x06007BDA RID: 31706 RVA: 0x0022D1F0 File Offset: 0x0022B3F0
		private static ITextPointer FindStartVisibleTextPointer(DocumentPage documentPage)
		{
			ITextPointer textPointer;
			ITextPointer position;
			if (!FixedTextSelectionProcessor.GetTextViewRange(documentPage, out textPointer, out position))
			{
				return null;
			}
			if (!textPointer.IsAtInsertionPosition && !textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward))
			{
				return null;
			}
			if (textPointer.CompareTo(position) > 0)
			{
				return null;
			}
			return textPointer;
		}

		// Token: 0x06007BDB RID: 31707 RVA: 0x0022D22C File Offset: 0x0022B42C
		private static ITextPointer FindEndVisibleTextPointer(DocumentPage documentPage)
		{
			ITextPointer textPointer;
			ITextPointer textPointer2;
			if (!FixedTextSelectionProcessor.GetTextViewRange(documentPage, out textPointer, out textPointer2))
			{
				return null;
			}
			if (!textPointer2.IsAtInsertionPosition && !textPointer2.MoveToNextInsertionPosition(LogicalDirection.Backward))
			{
				return null;
			}
			if (textPointer.CompareTo(textPointer2) > 0)
			{
				return null;
			}
			return textPointer2;
		}

		// Token: 0x06007BDC RID: 31708 RVA: 0x0022D268 File Offset: 0x0022B468
		private static bool GetTextViewRange(DocumentPage documentPage, out ITextPointer start, out ITextPointer end)
		{
			ITextPointer textPointer;
			end = (textPointer = null);
			start = textPointer;
			Invariant.Assert(documentPage != DocumentPage.Missing);
			ITextView textView = ((IServiceProvider)documentPage).GetService(typeof(ITextView)) as ITextView;
			Invariant.Assert(textView != null, "DocumentPage didn't provide a TextView.");
			if (textView.TextSegments == null || textView.TextSegments.Count == 0)
			{
				return false;
			}
			start = textView.TextSegments[0].Start.CreatePointer(LogicalDirection.Forward);
			end = textView.TextSegments[textView.TextSegments.Count - 1].End.CreatePointer(LogicalDirection.Backward);
			return true;
		}

		// Token: 0x04003A3D RID: 14909
		private static readonly XmlQualifiedName FixedTextElementName = new XmlQualifiedName("FixedTextRange", "http://schemas.microsoft.com/windows/annotations/2003/11/base");

		// Token: 0x04003A3E RID: 14910
		private static readonly XmlQualifiedName[] LocatorPartTypeNames = new XmlQualifiedName[]
		{
			FixedTextSelectionProcessor.FixedTextElementName
		};

		// Token: 0x02000B7D RID: 2941
		internal sealed class FixedPageProxy : DependencyObject
		{
			// Token: 0x06008E38 RID: 36408 RVA: 0x0025B630 File Offset: 0x00259830
			public FixedPageProxy(DependencyObject parent, int page)
			{
				base.SetValue(PathNode.HiddenParentProperty, parent);
				this._page = page;
			}

			// Token: 0x17001FA1 RID: 8097
			// (get) Token: 0x06008E39 RID: 36409 RVA: 0x0025B657 File Offset: 0x00259857
			public int Page
			{
				get
				{
					return this._page;
				}
			}

			// Token: 0x17001FA2 RID: 8098
			// (get) Token: 0x06008E3A RID: 36410 RVA: 0x0025B65F File Offset: 0x0025985F
			public IList<FixedTextSelectionProcessor.PointSegment> Segments
			{
				get
				{
					return this._segments;
				}
			}

			// Token: 0x04004B76 RID: 19318
			private int _page;

			// Token: 0x04004B77 RID: 19319
			private IList<FixedTextSelectionProcessor.PointSegment> _segments = new List<FixedTextSelectionProcessor.PointSegment>(1);
		}

		// Token: 0x02000B7E RID: 2942
		internal sealed class PointSegment
		{
			// Token: 0x06008E3B RID: 36411 RVA: 0x0025B667 File Offset: 0x00259867
			internal PointSegment(Point start, Point end)
			{
				this._start = start;
				this._end = end;
			}

			// Token: 0x17001FA3 RID: 8099
			// (get) Token: 0x06008E3C RID: 36412 RVA: 0x0025B67D File Offset: 0x0025987D
			public Point Start
			{
				get
				{
					return this._start;
				}
			}

			// Token: 0x17001FA4 RID: 8100
			// (get) Token: 0x06008E3D RID: 36413 RVA: 0x0025B685 File Offset: 0x00259885
			public Point End
			{
				get
				{
					return this._end;
				}
			}

			// Token: 0x04004B78 RID: 19320
			public static readonly Point NotAPoint = new Point(double.NaN, double.NaN);

			// Token: 0x04004B79 RID: 19321
			private Point _start;

			// Token: 0x04004B7A RID: 19322
			private Point _end;
		}
	}
}
