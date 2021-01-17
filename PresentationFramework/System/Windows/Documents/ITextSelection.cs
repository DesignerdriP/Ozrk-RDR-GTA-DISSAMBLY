using System;

namespace System.Windows.Documents
{
	// Token: 0x02000384 RID: 900
	internal interface ITextSelection : ITextRange
	{
		// Token: 0x06003130 RID: 12592
		void SetCaretToPosition(ITextPointer caretPosition, LogicalDirection direction, bool allowStopAtLineEnd, bool allowStopNearSpace);

		// Token: 0x06003131 RID: 12593
		void ExtendToPosition(ITextPointer textPosition);

		// Token: 0x06003132 RID: 12594
		bool ExtendToNextInsertionPosition(LogicalDirection direction);

		// Token: 0x06003133 RID: 12595
		bool Contains(Point point);

		// Token: 0x06003134 RID: 12596
		void OnDetach();

		// Token: 0x06003135 RID: 12597
		void UpdateCaretAndHighlight();

		// Token: 0x06003136 RID: 12598
		void OnTextViewUpdated();

		// Token: 0x06003137 RID: 12599
		void DetachFromVisualTree();

		// Token: 0x06003138 RID: 12600
		void RefreshCaret();

		// Token: 0x06003139 RID: 12601
		void OnInterimSelectionChanged(bool interimSelection);

		// Token: 0x0600313A RID: 12602
		void SetSelectionByMouse(ITextPointer cursorPosition, Point cursorMousePoint);

		// Token: 0x0600313B RID: 12603
		void ExtendSelectionByMouse(ITextPointer cursorPosition, bool forceWordSelection, bool forceParagraphSelection);

		// Token: 0x0600313C RID: 12604
		bool ExtendToNextTableRow(LogicalDirection direction);

		// Token: 0x0600313D RID: 12605
		void OnCaretNavigation();

		// Token: 0x0600313E RID: 12606
		void ValidateLayout();

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x0600313F RID: 12607
		TextEditor TextEditor { get; }

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x06003140 RID: 12608
		ITextView TextView { get; }

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x06003141 RID: 12609
		bool IsInterimSelection { get; }

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x06003142 RID: 12610
		ITextPointer AnchorPosition { get; }

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06003143 RID: 12611
		ITextPointer MovingPosition { get; }

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06003144 RID: 12612
		CaretElement CaretElement { get; }

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06003145 RID: 12613
		bool CoversEntireContent { get; }
	}
}
