using System;

namespace System.Windows.Documents
{
	// Token: 0x02000382 RID: 898
	internal interface ITextPointer
	{
		// Token: 0x060030D8 RID: 12504
		ITextPointer CreatePointer();

		// Token: 0x060030D9 RID: 12505
		StaticTextPointer CreateStaticPointer();

		// Token: 0x060030DA RID: 12506
		ITextPointer CreatePointer(int offset);

		// Token: 0x060030DB RID: 12507
		ITextPointer CreatePointer(LogicalDirection gravity);

		// Token: 0x060030DC RID: 12508
		ITextPointer CreatePointer(int offset, LogicalDirection gravity);

		// Token: 0x060030DD RID: 12509
		void SetLogicalDirection(LogicalDirection direction);

		// Token: 0x060030DE RID: 12510
		int CompareTo(ITextPointer position);

		// Token: 0x060030DF RID: 12511
		int CompareTo(StaticTextPointer position);

		// Token: 0x060030E0 RID: 12512
		bool HasEqualScope(ITextPointer position);

		// Token: 0x060030E1 RID: 12513
		TextPointerContext GetPointerContext(LogicalDirection direction);

		// Token: 0x060030E2 RID: 12514
		int GetOffsetToPosition(ITextPointer position);

		// Token: 0x060030E3 RID: 12515
		int GetTextRunLength(LogicalDirection direction);

		// Token: 0x060030E4 RID: 12516
		string GetTextInRun(LogicalDirection direction);

		// Token: 0x060030E5 RID: 12517
		int GetTextInRun(LogicalDirection direction, char[] textBuffer, int startIndex, int count);

		// Token: 0x060030E6 RID: 12518
		object GetAdjacentElement(LogicalDirection direction);

		// Token: 0x060030E7 RID: 12519
		void MoveToPosition(ITextPointer position);

		// Token: 0x060030E8 RID: 12520
		int MoveByOffset(int offset);

		// Token: 0x060030E9 RID: 12521
		bool MoveToNextContextPosition(LogicalDirection direction);

		// Token: 0x060030EA RID: 12522
		ITextPointer GetNextContextPosition(LogicalDirection direction);

		// Token: 0x060030EB RID: 12523
		bool MoveToInsertionPosition(LogicalDirection direction);

		// Token: 0x060030EC RID: 12524
		ITextPointer GetInsertionPosition(LogicalDirection direction);

		// Token: 0x060030ED RID: 12525
		ITextPointer GetFormatNormalizedPosition(LogicalDirection direction);

		// Token: 0x060030EE RID: 12526
		bool MoveToNextInsertionPosition(LogicalDirection direction);

		// Token: 0x060030EF RID: 12527
		ITextPointer GetNextInsertionPosition(LogicalDirection direction);

		// Token: 0x060030F0 RID: 12528
		void MoveToElementEdge(ElementEdge edge);

		// Token: 0x060030F1 RID: 12529
		int MoveToLineBoundary(int count);

		// Token: 0x060030F2 RID: 12530
		Rect GetCharacterRect(LogicalDirection direction);

		// Token: 0x060030F3 RID: 12531
		void Freeze();

		// Token: 0x060030F4 RID: 12532
		ITextPointer GetFrozenPointer(LogicalDirection logicalDirection);

		// Token: 0x060030F5 RID: 12533
		void InsertTextInRun(string textData);

		// Token: 0x060030F6 RID: 12534
		void DeleteContentToPosition(ITextPointer limit);

		// Token: 0x060030F7 RID: 12535
		Type GetElementType(LogicalDirection direction);

		// Token: 0x060030F8 RID: 12536
		object GetValue(DependencyProperty formattingProperty);

		// Token: 0x060030F9 RID: 12537
		object ReadLocalValue(DependencyProperty formattingProperty);

		// Token: 0x060030FA RID: 12538
		LocalValueEnumerator GetLocalValueEnumerator();

		// Token: 0x060030FB RID: 12539
		bool ValidateLayout();

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x060030FC RID: 12540
		ITextContainer TextContainer { get; }

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x060030FD RID: 12541
		bool HasValidLayout { get; }

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x060030FE RID: 12542
		bool IsAtCaretUnitBoundary { get; }

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x060030FF RID: 12543
		LogicalDirection LogicalDirection { get; }

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x06003100 RID: 12544
		Type ParentType { get; }

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x06003101 RID: 12545
		bool IsAtInsertionPosition { get; }

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x06003102 RID: 12546
		bool IsFrozen { get; }

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x06003103 RID: 12547
		int Offset { get; }

		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x06003104 RID: 12548
		int CharOffset { get; }
	}
}
