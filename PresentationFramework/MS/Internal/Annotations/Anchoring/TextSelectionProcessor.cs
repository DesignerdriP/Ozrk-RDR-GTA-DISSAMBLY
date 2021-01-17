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
	// Token: 0x020007DA RID: 2010
	internal sealed class TextSelectionProcessor : SelectionProcessor
	{
		// Token: 0x06007C41 RID: 31809 RVA: 0x0022C846 File Offset: 0x0022AA46
		public override bool MergeSelections(object anchor1, object anchor2, out object newAnchor)
		{
			return TextSelectionHelper.MergeSelections(anchor1, anchor2, out newAnchor);
		}

		// Token: 0x06007C42 RID: 31810 RVA: 0x0022F225 File Offset: 0x0022D425
		public override IList<DependencyObject> GetSelectedNodes(object selection)
		{
			return TextSelectionHelper.GetSelectedNodes(selection);
		}

		// Token: 0x06007C43 RID: 31811 RVA: 0x0022F22D File Offset: 0x0022D42D
		public override UIElement GetParent(object selection)
		{
			return TextSelectionHelper.GetParent(selection);
		}

		// Token: 0x06007C44 RID: 31812 RVA: 0x0022F235 File Offset: 0x0022D435
		public override Point GetAnchorPoint(object selection)
		{
			return TextSelectionHelper.GetAnchorPoint(selection);
		}

		// Token: 0x06007C45 RID: 31813 RVA: 0x0022F240 File Offset: 0x0022D440
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
			IList<TextSegment> list = null;
			ITextPointer textPointer;
			ITextPointer position;
			TextSelectionHelper.CheckSelection(selection, out textPointer, out position, out list);
			if (!(textPointer is TextPointer))
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "selection");
			}
			ITextPointer textPointer2;
			ITextPointer textPointer3;
			if (!this.GetNodesStartAndEnd(startNode, out textPointer2, out textPointer3))
			{
				return null;
			}
			if (textPointer2.CompareTo(position) > 0)
			{
				throw new ArgumentException(SR.Get("InvalidStartNodeForTextSelection"), "startNode");
			}
			if (textPointer3.CompareTo(textPointer) < 0)
			{
				throw new ArgumentException(SR.Get("InvalidStartNodeForTextSelection"), "startNode");
			}
			ContentLocatorPart contentLocatorPart = new ContentLocatorPart(TextSelectionProcessor.CharacterRangeElementName);
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < list.Count; i++)
			{
				this.GetTextSegmentValues(list[i], textPointer2, textPointer3, out num, out num2);
				contentLocatorPart.NameValuePairs.Add("Segment" + i.ToString(NumberFormatInfo.InvariantInfo), num.ToString(NumberFormatInfo.InvariantInfo) + TextSelectionProcessor.Separator[0].ToString() + num2.ToString(NumberFormatInfo.InvariantInfo));
			}
			contentLocatorPart.NameValuePairs.Add("Count", list.Count.ToString(NumberFormatInfo.InvariantInfo));
			return new List<ContentLocatorPart>(1)
			{
				contentLocatorPart
			};
		}

		// Token: 0x06007C46 RID: 31814 RVA: 0x0022F3A8 File Offset: 0x0022D5A8
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
			if (TextSelectionProcessor.CharacterRangeElementName != locatorPart.PartType)
			{
				throw new ArgumentException(SR.Get("IncorrectLocatorPartType", new object[]
				{
					locatorPart.PartType.Namespace + ":" + locatorPart.PartType.Name
				}), "locatorPart");
			}
			int num = 0;
			int num2 = 0;
			string text = locatorPart.NameValuePairs["Count"];
			if (text == null)
			{
				throw new ArgumentException(SR.Get("InvalidLocatorPart", new object[]
				{
					"Count"
				}));
			}
			int num3 = int.Parse(text, NumberFormatInfo.InvariantInfo);
			TextAnchor textAnchor = new TextAnchor();
			attachmentLevel = AttachmentLevel.Unresolved;
			for (int i = 0; i < num3; i++)
			{
				TextSelectionProcessor.GetLocatorPartSegmentValues(locatorPart, i, out num, out num2);
				ITextPointer textPointer;
				ITextPointer textPointer2;
				if (!this.GetNodesStartAndEnd(startNode, out textPointer, out textPointer2))
				{
					return null;
				}
				int offsetToPosition = textPointer.GetOffsetToPosition(textPointer2);
				if (num > offsetToPosition)
				{
					return null;
				}
				ITextPointer textPointer3 = textPointer.CreatePointer(num);
				ITextPointer textPointer4 = (offsetToPosition <= num2) ? textPointer2.CreatePointer() : textPointer.CreatePointer(num2);
				if (textPointer3.CompareTo(textPointer4) >= 0)
				{
					return null;
				}
				textAnchor.AddTextSegment(textPointer3, textPointer4);
			}
			if (textAnchor.IsEmpty)
			{
				throw new ArgumentException(SR.Get("IncorrectAnchorLength"), "locatorPart");
			}
			attachmentLevel = AttachmentLevel.Full;
			if (this._clamping)
			{
				ITextPointer start = textAnchor.Start;
				ITextPointer end = textAnchor.End;
				IServiceProvider serviceProvider;
				if (this._targetPage != null)
				{
					serviceProvider = this._targetPage;
				}
				else
				{
					FlowDocument node = start.TextContainer.Parent as FlowDocument;
					serviceProvider = (PathNode.GetParent(node) as IServiceProvider);
				}
				Invariant.Assert(serviceProvider != null, "No ServiceProvider found to get TextView from.");
				ITextView textView = serviceProvider.GetService(typeof(ITextView)) as ITextView;
				Invariant.Assert(textView != null, "Null TextView provided by ServiceProvider.");
				textAnchor = TextAnchor.TrimToIntersectionWith(textAnchor, textView.TextSegments);
				if (textAnchor == null)
				{
					attachmentLevel = AttachmentLevel.Unresolved;
				}
				else
				{
					if (textAnchor.Start.CompareTo(start) != 0)
					{
						attachmentLevel &= ~AttachmentLevel.StartPortion;
					}
					if (textAnchor.End.CompareTo(end) != 0)
					{
						attachmentLevel &= ~AttachmentLevel.EndPortion;
					}
				}
			}
			return textAnchor;
		}

		// Token: 0x06007C47 RID: 31815 RVA: 0x0022F5DB File Offset: 0x0022D7DB
		public override XmlQualifiedName[] GetLocatorPartTypes()
		{
			return (XmlQualifiedName[])TextSelectionProcessor.LocatorPartTypeNames.Clone();
		}

		// Token: 0x17001CEB RID: 7403
		// (set) Token: 0x06007C48 RID: 31816 RVA: 0x0022F5EC File Offset: 0x0022D7EC
		internal bool Clamping
		{
			set
			{
				this._clamping = value;
			}
		}

		// Token: 0x06007C49 RID: 31817 RVA: 0x0022F5F8 File Offset: 0x0022D7F8
		internal static void GetMaxMinLocatorPartValues(ContentLocatorPart locatorPart, out int startOffset, out int endOffset)
		{
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			string text = locatorPart.NameValuePairs["Count"];
			if (text == null)
			{
				throw new ArgumentException(SR.Get("InvalidLocatorPart", new object[]
				{
					"Count"
				}));
			}
			int num = int.Parse(text, NumberFormatInfo.InvariantInfo);
			startOffset = int.MaxValue;
			endOffset = 0;
			for (int i = 0; i < num; i++)
			{
				int num2;
				int num3;
				TextSelectionProcessor.GetLocatorPartSegmentValues(locatorPart, i, out num2, out num3);
				if (num2 < startOffset)
				{
					startOffset = num2;
				}
				if (num3 > endOffset)
				{
					endOffset = num3;
				}
			}
		}

		// Token: 0x06007C4A RID: 31818 RVA: 0x0022F687 File Offset: 0x0022D887
		internal void SetTargetDocumentPageView(DocumentPageView target)
		{
			this._targetPage = target;
		}

		// Token: 0x06007C4B RID: 31819 RVA: 0x0022F690 File Offset: 0x0022D890
		private static void GetLocatorPartSegmentValues(ContentLocatorPart locatorPart, int segmentNumber, out int startOffset, out int endOffset)
		{
			if (segmentNumber < 0)
			{
				throw new ArgumentException("segmentNumber");
			}
			string text = locatorPart.NameValuePairs["Segment" + segmentNumber.ToString(NumberFormatInfo.InvariantInfo)];
			string[] array = text.Split(TextSelectionProcessor.Separator);
			if (array.Length != 2)
			{
				throw new ArgumentException(SR.Get("InvalidLocatorPart", new object[]
				{
					"Segment" + segmentNumber.ToString(NumberFormatInfo.InvariantInfo)
				}));
			}
			startOffset = int.Parse(array[0], NumberFormatInfo.InvariantInfo);
			endOffset = int.Parse(array[1], NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06007C4C RID: 31820 RVA: 0x0022F730 File Offset: 0x0022D930
		private ITextContainer GetTextContainer(DependencyObject startNode)
		{
			ITextContainer textContainer = null;
			IServiceProvider serviceProvider = startNode as IServiceProvider;
			if (serviceProvider != null)
			{
				textContainer = (serviceProvider.GetService(typeof(ITextContainer)) as ITextContainer);
			}
			if (textContainer == null)
			{
				TextBoxBase textBoxBase = startNode as TextBoxBase;
				if (textBoxBase != null)
				{
					textContainer = textBoxBase.TextContainer;
				}
			}
			return textContainer;
		}

		// Token: 0x06007C4D RID: 31821 RVA: 0x0022F774 File Offset: 0x0022D974
		private bool GetNodesStartAndEnd(DependencyObject startNode, out ITextPointer start, out ITextPointer end)
		{
			start = null;
			end = null;
			ITextContainer textContainer = this.GetTextContainer(startNode);
			if (textContainer != null)
			{
				start = textContainer.Start;
				end = textContainer.End;
			}
			else
			{
				TextElement textElement = startNode as TextElement;
				if (textElement == null)
				{
					return false;
				}
				start = textElement.ContentStart;
				end = textElement.ContentEnd;
			}
			return true;
		}

		// Token: 0x06007C4E RID: 31822 RVA: 0x0022F7C4 File Offset: 0x0022D9C4
		private void GetTextSegmentValues(TextSegment segment, ITextPointer elementStart, ITextPointer elementEnd, out int startOffset, out int endOffset)
		{
			startOffset = 0;
			endOffset = 0;
			if (elementStart.CompareTo(segment.Start) >= 0)
			{
				startOffset = 0;
			}
			else
			{
				startOffset = elementStart.GetOffsetToPosition(segment.Start);
			}
			if (elementEnd.CompareTo(segment.End) >= 0)
			{
				endOffset = elementStart.GetOffsetToPosition(segment.End);
				return;
			}
			endOffset = elementStart.GetOffsetToPosition(elementEnd);
		}

		// Token: 0x04003A58 RID: 14936
		internal const string SegmentAttribute = "Segment";

		// Token: 0x04003A59 RID: 14937
		internal const string CountAttribute = "Count";

		// Token: 0x04003A5A RID: 14938
		internal const string IncludeOverlaps = "IncludeOverlaps";

		// Token: 0x04003A5B RID: 14939
		internal static readonly char[] Separator = new char[]
		{
			','
		};

		// Token: 0x04003A5C RID: 14940
		internal static readonly XmlQualifiedName CharacterRangeElementName = new XmlQualifiedName("CharacterRange", "http://schemas.microsoft.com/windows/annotations/2003/11/base");

		// Token: 0x04003A5D RID: 14941
		private static readonly XmlQualifiedName[] LocatorPartTypeNames = new XmlQualifiedName[]
		{
			TextSelectionProcessor.CharacterRangeElementName
		};

		// Token: 0x04003A5E RID: 14942
		private DocumentPageView _targetPage;

		// Token: 0x04003A5F RID: 14943
		private bool _clamping = true;
	}
}
