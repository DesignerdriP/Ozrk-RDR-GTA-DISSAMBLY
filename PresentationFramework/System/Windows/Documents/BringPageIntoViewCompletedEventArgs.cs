﻿using System;
using System.ComponentModel;

namespace System.Windows.Documents
{
	// Token: 0x0200038D RID: 909
	internal class BringPageIntoViewCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06003183 RID: 12675 RVA: 0x000DB8BA File Offset: 0x000D9ABA
		public BringPageIntoViewCompletedEventArgs(ITextPointer position, Point suggestedOffset, int count, ITextPointer newPosition, Point newSuggestedOffset, int pagesMoved, bool succeeded, Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
		{
			this._position = position;
			this._count = count;
			this._newPosition = newPosition;
			this._newSuggestedOffset = newSuggestedOffset;
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x06003184 RID: 12676 RVA: 0x000DB8E6 File Offset: 0x000D9AE6
		public ITextPointer Position
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._position;
			}
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x06003185 RID: 12677 RVA: 0x000DB8F4 File Offset: 0x000D9AF4
		public int Count
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._count;
			}
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x06003186 RID: 12678 RVA: 0x000DB902 File Offset: 0x000D9B02
		public ITextPointer NewPosition
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._newPosition;
			}
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x06003187 RID: 12679 RVA: 0x000DB910 File Offset: 0x000D9B10
		public Point NewSuggestedOffset
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._newSuggestedOffset;
			}
		}

		// Token: 0x04001E93 RID: 7827
		private readonly ITextPointer _position;

		// Token: 0x04001E94 RID: 7828
		private readonly int _count;

		// Token: 0x04001E95 RID: 7829
		private readonly ITextPointer _newPosition;

		// Token: 0x04001E96 RID: 7830
		private readonly Point _newSuggestedOffset;
	}
}
