﻿using System;

namespace System.Windows.Documents
{
	// Token: 0x020003EF RID: 1007
	internal class TextContainerChangeEventArgs : EventArgs
	{
		// Token: 0x060037C1 RID: 14273 RVA: 0x000F9037 File Offset: 0x000F7237
		internal TextContainerChangeEventArgs(ITextPointer textPosition, int count, int charCount, TextChangeType textChange) : this(textPosition, count, charCount, textChange, null, false)
		{
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x000F9046 File Offset: 0x000F7246
		internal TextContainerChangeEventArgs(ITextPointer textPosition, int count, int charCount, TextChangeType textChange, DependencyProperty property, bool affectsRenderOnly)
		{
			this._textPosition = textPosition.GetFrozenPointer(LogicalDirection.Forward);
			this._count = count;
			this._charCount = charCount;
			this._textChange = textChange;
			this._property = property;
			this._affectsRenderOnly = affectsRenderOnly;
		}

		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x060037C3 RID: 14275 RVA: 0x000F9081 File Offset: 0x000F7281
		internal ITextPointer ITextPosition
		{
			get
			{
				return this._textPosition;
			}
		}

		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x060037C4 RID: 14276 RVA: 0x000F9089 File Offset: 0x000F7289
		internal int IMECharCount
		{
			get
			{
				return this._charCount;
			}
		}

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x060037C5 RID: 14277 RVA: 0x000F9091 File Offset: 0x000F7291
		internal bool AffectsRenderOnly
		{
			get
			{
				return this._affectsRenderOnly;
			}
		}

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x060037C6 RID: 14278 RVA: 0x000F9099 File Offset: 0x000F7299
		internal int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x060037C7 RID: 14279 RVA: 0x000F90A1 File Offset: 0x000F72A1
		internal TextChangeType TextChange
		{
			get
			{
				return this._textChange;
			}
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x060037C8 RID: 14280 RVA: 0x000F90A9 File Offset: 0x000F72A9
		internal DependencyProperty Property
		{
			get
			{
				return this._property;
			}
		}

		// Token: 0x0400256C RID: 9580
		private readonly ITextPointer _textPosition;

		// Token: 0x0400256D RID: 9581
		private readonly int _count;

		// Token: 0x0400256E RID: 9582
		private readonly int _charCount;

		// Token: 0x0400256F RID: 9583
		private readonly TextChangeType _textChange;

		// Token: 0x04002570 RID: 9584
		private readonly DependencyProperty _property;

		// Token: 0x04002571 RID: 9585
		private readonly bool _affectsRenderOnly;
	}
}
