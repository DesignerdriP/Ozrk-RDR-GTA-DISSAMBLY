﻿using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200041F RID: 1055
	internal class TextTreeInsertElementUndoUnit : TextTreeUndoUnit
	{
		// Token: 0x06003D40 RID: 15680 RVA: 0x0011BE55 File Offset: 0x0011A055
		internal TextTreeInsertElementUndoUnit(TextContainer tree, int symbolOffset, bool deep) : base(tree, symbolOffset)
		{
			this._deep = deep;
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x0011BE68 File Offset: 0x0011A068
		public override void DoCore()
		{
			base.VerifyTreeContentHashCode();
			TextPointer textPointer = new TextPointer(base.TextContainer, base.SymbolOffset, LogicalDirection.Forward);
			Invariant.Assert(textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart, "TextTree undo unit out of sync with TextTree.");
			TextElement adjacentElementFromOuterPosition = textPointer.GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
			if (this._deep)
			{
				TextPointer endPosition = new TextPointer(base.TextContainer, adjacentElementFromOuterPosition.TextElementNode, ElementEdge.AfterEnd);
				base.TextContainer.DeleteContentInternal(textPointer, endPosition);
				return;
			}
			base.TextContainer.ExtractElementInternal(adjacentElementFromOuterPosition);
		}

		// Token: 0x0400265F RID: 9823
		private readonly bool _deep;
	}
}
