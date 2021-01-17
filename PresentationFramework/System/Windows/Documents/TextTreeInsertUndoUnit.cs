using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200041E RID: 1054
	internal class TextTreeInsertUndoUnit : TextTreeUndoUnit
	{
		// Token: 0x06003D3E RID: 15678 RVA: 0x0011BDE8 File Offset: 0x00119FE8
		internal TextTreeInsertUndoUnit(TextContainer tree, int symbolOffset, int symbolCount) : base(tree, symbolOffset)
		{
			Invariant.Assert(symbolCount > 0, "Creating no-op insert undo unit!");
			this._symbolCount = symbolCount;
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x0011BE08 File Offset: 0x0011A008
		public override void DoCore()
		{
			base.VerifyTreeContentHashCode();
			TextPointer startPosition = new TextPointer(base.TextContainer, base.SymbolOffset, LogicalDirection.Forward);
			TextPointer endPosition = new TextPointer(base.TextContainer, base.SymbolOffset + this._symbolCount, LogicalDirection.Forward);
			base.TextContainer.DeleteContentInternal(startPosition, endPosition);
		}

		// Token: 0x0400265E RID: 9822
		private readonly int _symbolCount;
	}
}
