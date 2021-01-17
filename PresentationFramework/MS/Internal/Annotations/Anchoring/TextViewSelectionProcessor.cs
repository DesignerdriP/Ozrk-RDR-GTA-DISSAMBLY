using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Documents;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020007DB RID: 2011
	internal class TextViewSelectionProcessor : SelectionProcessor
	{
		// Token: 0x06007C51 RID: 31825 RVA: 0x0022F863 File Offset: 0x0022DA63
		public override bool MergeSelections(object selection1, object selection2, out object newSelection)
		{
			newSelection = null;
			return false;
		}

		// Token: 0x06007C52 RID: 31826 RVA: 0x0022F869 File Offset: 0x0022DA69
		public override IList<DependencyObject> GetSelectedNodes(object selection)
		{
			this.VerifySelection(selection);
			return new DependencyObject[]
			{
				(DependencyObject)selection
			};
		}

		// Token: 0x06007C53 RID: 31827 RVA: 0x0022F882 File Offset: 0x0022DA82
		public override UIElement GetParent(object selection)
		{
			this.VerifySelection(selection);
			return (UIElement)selection;
		}

		// Token: 0x06007C54 RID: 31828 RVA: 0x0022F892 File Offset: 0x0022DA92
		public override Point GetAnchorPoint(object selection)
		{
			this.VerifySelection(selection);
			return new Point(double.NaN, double.NaN);
		}

		// Token: 0x06007C55 RID: 31829 RVA: 0x0022F8B4 File Offset: 0x0022DAB4
		public override IList<ContentLocatorPart> GenerateLocatorParts(object selection, DependencyObject startNode)
		{
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			ITextView textView = this.VerifySelection(selection);
			List<ContentLocatorPart> list = new List<ContentLocatorPart>(1);
			int num;
			int num2;
			if (textView != null && textView.IsValid)
			{
				TextViewSelectionProcessor.GetTextViewTextRange(textView, out num, out num2);
			}
			else
			{
				num = -1;
				num2 = -1;
			}
			list.Add(new ContentLocatorPart(TextSelectionProcessor.CharacterRangeElementName)
			{
				NameValuePairs = 
				{
					{
						"Count",
						1.ToString(NumberFormatInfo.InvariantInfo)
					},
					{
						"Segment" + 0.ToString(NumberFormatInfo.InvariantInfo),
						num.ToString(NumberFormatInfo.InvariantInfo) + TextSelectionProcessor.Separator[0].ToString() + num2.ToString(NumberFormatInfo.InvariantInfo)
					},
					{
						"IncludeOverlaps",
						bool.TrueString
					}
				}
			});
			return list;
		}

		// Token: 0x06007C56 RID: 31830 RVA: 0x0022F999 File Offset: 0x0022DB99
		public override object ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out AttachmentLevel attachmentLevel)
		{
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			attachmentLevel = AttachmentLevel.Unresolved;
			return null;
		}

		// Token: 0x06007C57 RID: 31831 RVA: 0x0022F9BB File Offset: 0x0022DBBB
		public override XmlQualifiedName[] GetLocatorPartTypes()
		{
			return (XmlQualifiedName[])TextViewSelectionProcessor.LocatorPartTypeNames.Clone();
		}

		// Token: 0x06007C58 RID: 31832 RVA: 0x0022F9CC File Offset: 0x0022DBCC
		internal static TextRange GetTextViewTextRange(ITextView textView, out int startOffset, out int endOffset)
		{
			startOffset = int.MinValue;
			endOffset = 0;
			TextRange result = null;
			IList<TextSegment> textSegments = textView.TextSegments;
			if (textSegments != null && textSegments.Count > 0)
			{
				ITextPointer start = textSegments[0].Start;
				ITextPointer end = textSegments[textSegments.Count - 1].End;
				startOffset = end.TextContainer.Start.GetOffsetToPosition(start);
				endOffset = end.TextContainer.Start.GetOffsetToPosition(end);
				result = new TextRange(start, end);
			}
			return result;
		}

		// Token: 0x06007C59 RID: 31833 RVA: 0x0022FA50 File Offset: 0x0022DC50
		private ITextView VerifySelection(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			IServiceProvider serviceProvider = selection as IServiceProvider;
			if (serviceProvider == null)
			{
				throw new ArgumentException(SR.Get("SelectionMustBeServiceProvider"), "selection");
			}
			return serviceProvider.GetService(typeof(ITextView)) as ITextView;
		}

		// Token: 0x04003A60 RID: 14944
		private static readonly XmlQualifiedName[] LocatorPartTypeNames = new XmlQualifiedName[0];
	}
}
