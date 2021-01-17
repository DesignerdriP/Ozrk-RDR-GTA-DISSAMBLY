﻿using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020003E1 RID: 993
	internal struct StaticTextPointer
	{
		// Token: 0x060035E5 RID: 13797 RVA: 0x000F4A34 File Offset: 0x000F2C34
		internal StaticTextPointer(ITextContainer textContainer, object handle0)
		{
			this = new StaticTextPointer(textContainer, handle0, 0);
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x000F4A3F File Offset: 0x000F2C3F
		internal StaticTextPointer(ITextContainer textContainer, object handle0, int handle1)
		{
			this._textContainer = textContainer;
			this._generation = ((textContainer != null) ? textContainer.Generation : 0U);
			this._handle0 = handle0;
			this._handle1 = handle1;
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x000F4A68 File Offset: 0x000F2C68
		internal ITextPointer CreateDynamicTextPointer(LogicalDirection direction)
		{
			this.AssertGeneration();
			return this._textContainer.CreateDynamicTextPointer(this, direction);
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x000F4A82 File Offset: 0x000F2C82
		internal TextPointerContext GetPointerContext(LogicalDirection direction)
		{
			this.AssertGeneration();
			return this._textContainer.GetPointerContext(this, direction);
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x000F4A9C File Offset: 0x000F2C9C
		internal int GetOffsetToPosition(StaticTextPointer position)
		{
			this.AssertGeneration();
			return this._textContainer.GetOffsetToPosition(this, position);
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x000F4AB6 File Offset: 0x000F2CB6
		internal int GetTextInRun(LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			this.AssertGeneration();
			return this._textContainer.GetTextInRun(this, direction, textBuffer, startIndex, count);
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x000F4AD4 File Offset: 0x000F2CD4
		internal object GetAdjacentElement(LogicalDirection direction)
		{
			this.AssertGeneration();
			return this._textContainer.GetAdjacentElement(this, direction);
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x000F4AEE File Offset: 0x000F2CEE
		internal StaticTextPointer CreatePointer(int offset)
		{
			this.AssertGeneration();
			return this._textContainer.CreatePointer(this, offset);
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x000F4B08 File Offset: 0x000F2D08
		internal StaticTextPointer GetNextContextPosition(LogicalDirection direction)
		{
			this.AssertGeneration();
			return this._textContainer.GetNextContextPosition(this, direction);
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x000F4B22 File Offset: 0x000F2D22
		internal int CompareTo(StaticTextPointer position)
		{
			this.AssertGeneration();
			return this._textContainer.CompareTo(this, position);
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x000F4B3C File Offset: 0x000F2D3C
		internal int CompareTo(ITextPointer position)
		{
			this.AssertGeneration();
			return this._textContainer.CompareTo(this, position);
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x000F4B56 File Offset: 0x000F2D56
		internal object GetValue(DependencyProperty formattingProperty)
		{
			this.AssertGeneration();
			return this._textContainer.GetValue(this, formattingProperty);
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x000F4B70 File Offset: 0x000F2D70
		internal static StaticTextPointer Min(StaticTextPointer position1, StaticTextPointer position2)
		{
			position2.AssertGeneration();
			if (position1.CompareTo(position2) > 0)
			{
				return position2;
			}
			return position1;
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x000F4B87 File Offset: 0x000F2D87
		internal static StaticTextPointer Max(StaticTextPointer position1, StaticTextPointer position2)
		{
			position2.AssertGeneration();
			if (position1.CompareTo(position2) < 0)
			{
				return position2;
			}
			return position1;
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x000F4B9E File Offset: 0x000F2D9E
		internal void AssertGeneration()
		{
			if (this._textContainer != null)
			{
				Invariant.Assert(this._generation == this._textContainer.Generation, "StaticTextPointer not synchronized to tree generation!");
			}
		}

		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x060035F4 RID: 13812 RVA: 0x000F4BC5 File Offset: 0x000F2DC5
		internal ITextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x060035F5 RID: 13813 RVA: 0x000F4BCD File Offset: 0x000F2DCD
		internal DependencyObject Parent
		{
			get
			{
				return this._textContainer.GetParent(this);
			}
		}

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x060035F6 RID: 13814 RVA: 0x000F4BE0 File Offset: 0x000F2DE0
		internal bool IsNull
		{
			get
			{
				return this._textContainer == null;
			}
		}

		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x060035F7 RID: 13815 RVA: 0x000F4BEB File Offset: 0x000F2DEB
		internal object Handle0
		{
			get
			{
				return this._handle0;
			}
		}

		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x060035F8 RID: 13816 RVA: 0x000F4BF3 File Offset: 0x000F2DF3
		internal int Handle1
		{
			get
			{
				return this._handle1;
			}
		}

		// Token: 0x0400252D RID: 9517
		internal static StaticTextPointer Null = new StaticTextPointer(null, null, 0);

		// Token: 0x0400252E RID: 9518
		private readonly ITextContainer _textContainer;

		// Token: 0x0400252F RID: 9519
		private readonly uint _generation;

		// Token: 0x04002530 RID: 9520
		private readonly object _handle0;

		// Token: 0x04002531 RID: 9521
		private readonly int _handle1;
	}
}
