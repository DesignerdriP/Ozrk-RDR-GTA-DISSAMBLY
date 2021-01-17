using System;
using System.Windows;
using System.Windows.Documents;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200062E RID: 1582
	internal sealed class ListParagraph : ContainerParagraph
	{
		// Token: 0x060068AE RID: 26798 RVA: 0x001D81FA File Offset: 0x001D63FA
		internal ListParagraph(DependencyObject element, StructuralCache structuralCache) : base(element, structuralCache)
		{
		}

		// Token: 0x060068AF RID: 26799 RVA: 0x001D8BC8 File Offset: 0x001D6DC8
		internal override void CreateParaclient(out IntPtr paraClientHandle)
		{
			ListParaClient listParaClient = new ListParaClient(this);
			paraClientHandle = listParaClient.Handle;
		}

		// Token: 0x060068B0 RID: 26800 RVA: 0x001D8BE4 File Offset: 0x001D6DE4
		protected override BaseParagraph GetParagraph(ITextPointer textPointer, bool fEmptyOk)
		{
			Invariant.Assert(textPointer is TextPointer);
			BaseParagraph baseParagraph = null;
			while (baseParagraph == null)
			{
				TextPointerContext pointerContext = textPointer.GetPointerContext(LogicalDirection.Forward);
				if (pointerContext == TextPointerContext.ElementStart)
				{
					TextElement adjacentElementFromOuterPosition = ((TextPointer)textPointer).GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
					if (adjacentElementFromOuterPosition is ListItem)
					{
						baseParagraph = new ListItemParagraph(adjacentElementFromOuterPosition, base.StructuralCache);
						break;
					}
					if (adjacentElementFromOuterPosition is List)
					{
						baseParagraph = new ListParagraph(adjacentElementFromOuterPosition, base.StructuralCache);
						break;
					}
					if (((TextPointer)textPointer).IsFrozen)
					{
						textPointer = textPointer.CreatePointer();
					}
					textPointer.MoveToPosition(adjacentElementFromOuterPosition.ElementEnd);
				}
				else if (pointerContext == TextPointerContext.ElementEnd)
				{
					if (base.Element == ((TextPointer)textPointer).Parent)
					{
						break;
					}
					if (((TextPointer)textPointer).IsFrozen)
					{
						textPointer = textPointer.CreatePointer();
					}
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
				}
				else
				{
					if (((TextPointer)textPointer).IsFrozen)
					{
						textPointer = textPointer.CreatePointer();
					}
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
				}
			}
			if (baseParagraph != null)
			{
				base.StructuralCache.CurrentFormatContext.DependentMax = (TextPointer)textPointer;
			}
			return baseParagraph;
		}
	}
}
