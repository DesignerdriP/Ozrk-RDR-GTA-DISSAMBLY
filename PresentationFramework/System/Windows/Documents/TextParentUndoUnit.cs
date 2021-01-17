using System;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000407 RID: 1031
	internal class TextParentUndoUnit : ParentUndoUnit
	{
		// Token: 0x060039C2 RID: 14786 RVA: 0x00106538 File Offset: 0x00104738
		internal TextParentUndoUnit(ITextSelection selection) : this(selection, selection.AnchorPosition, selection.MovingPosition)
		{
		}

		// Token: 0x060039C3 RID: 14787 RVA: 0x00106550 File Offset: 0x00104750
		internal TextParentUndoUnit(ITextSelection selection, ITextPointer anchorPosition, ITextPointer movingPosition) : base(string.Empty)
		{
			this._selection = selection;
			this._undoAnchorPositionOffset = anchorPosition.Offset;
			this._undoAnchorPositionDirection = anchorPosition.LogicalDirection;
			this._undoMovingPositionOffset = movingPosition.Offset;
			this._undoMovingPositionDirection = movingPosition.LogicalDirection;
			this._redoAnchorPositionOffset = 0;
			this._redoMovingPositionOffset = 0;
		}

		// Token: 0x060039C4 RID: 14788 RVA: 0x001065B0 File Offset: 0x001047B0
		protected TextParentUndoUnit(TextParentUndoUnit undoUnit) : base(string.Empty)
		{
			this._selection = undoUnit._selection;
			this._undoAnchorPositionOffset = undoUnit._redoAnchorPositionOffset;
			this._undoAnchorPositionDirection = undoUnit._redoAnchorPositionDirection;
			this._undoMovingPositionOffset = undoUnit._redoMovingPositionOffset;
			this._undoMovingPositionDirection = undoUnit._redoMovingPositionDirection;
			this._redoAnchorPositionOffset = 0;
			this._redoMovingPositionOffset = 0;
		}

		// Token: 0x060039C5 RID: 14789 RVA: 0x00106614 File Offset: 0x00104814
		public override void Do()
		{
			base.Do();
			ITextContainer textContainer = this._selection.Start.TextContainer;
			ITextPointer position = textContainer.CreatePointerAtOffset(this._undoAnchorPositionOffset, this._undoAnchorPositionDirection);
			ITextPointer position2 = textContainer.CreatePointerAtOffset(this._undoMovingPositionOffset, this._undoMovingPositionDirection);
			this._selection.Select(position, position2);
			this._redoUnit.RecordRedoSelectionState();
		}

		// Token: 0x060039C6 RID: 14790 RVA: 0x00106676 File Offset: 0x00104876
		protected override IParentUndoUnit CreateParentUndoUnitForSelf()
		{
			this._redoUnit = this.CreateRedoUnit();
			return this._redoUnit;
		}

		// Token: 0x060039C7 RID: 14791 RVA: 0x0010668A File Offset: 0x0010488A
		protected virtual TextParentUndoUnit CreateRedoUnit()
		{
			return new TextParentUndoUnit(this);
		}

		// Token: 0x060039C8 RID: 14792 RVA: 0x00106692 File Offset: 0x00104892
		protected void MergeRedoSelectionState(TextParentUndoUnit undoUnit)
		{
			this._redoAnchorPositionOffset = undoUnit._redoAnchorPositionOffset;
			this._redoAnchorPositionDirection = undoUnit._redoAnchorPositionDirection;
			this._redoMovingPositionOffset = undoUnit._redoMovingPositionOffset;
			this._redoMovingPositionDirection = undoUnit._redoMovingPositionDirection;
		}

		// Token: 0x060039C9 RID: 14793 RVA: 0x001066C4 File Offset: 0x001048C4
		internal void RecordRedoSelectionState()
		{
			this.RecordRedoSelectionState(this._selection.AnchorPosition, this._selection.MovingPosition);
		}

		// Token: 0x060039CA RID: 14794 RVA: 0x001066E2 File Offset: 0x001048E2
		internal void RecordRedoSelectionState(ITextPointer anchorPosition, ITextPointer movingPosition)
		{
			this._redoAnchorPositionOffset = anchorPosition.Offset;
			this._redoAnchorPositionDirection = anchorPosition.LogicalDirection;
			this._redoMovingPositionOffset = movingPosition.Offset;
			this._redoMovingPositionDirection = movingPosition.LogicalDirection;
		}

		// Token: 0x040025C9 RID: 9673
		private readonly ITextSelection _selection;

		// Token: 0x040025CA RID: 9674
		private readonly int _undoAnchorPositionOffset;

		// Token: 0x040025CB RID: 9675
		private readonly LogicalDirection _undoAnchorPositionDirection;

		// Token: 0x040025CC RID: 9676
		private readonly int _undoMovingPositionOffset;

		// Token: 0x040025CD RID: 9677
		private readonly LogicalDirection _undoMovingPositionDirection;

		// Token: 0x040025CE RID: 9678
		private int _redoAnchorPositionOffset;

		// Token: 0x040025CF RID: 9679
		private LogicalDirection _redoAnchorPositionDirection;

		// Token: 0x040025D0 RID: 9680
		private int _redoMovingPositionOffset;

		// Token: 0x040025D1 RID: 9681
		private LogicalDirection _redoMovingPositionDirection;

		// Token: 0x040025D2 RID: 9682
		private TextParentUndoUnit _redoUnit;
	}
}
