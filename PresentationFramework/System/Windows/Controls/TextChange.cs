using System;

namespace System.Windows.Controls
{
	/// <summary>Contains information about the changes that occur in the <see cref="E:System.Windows.Controls.Primitives.TextBoxBase.TextChanged" /> event.</summary>
	// Token: 0x0200053E RID: 1342
	public class TextChange
	{
		// Token: 0x060057DD RID: 22493 RVA: 0x0000326D File Offset: 0x0000146D
		internal TextChange()
		{
		}

		/// <summary>Gets or sets the position at which the change occurred.</summary>
		/// <returns>The position at which the change occurred.</returns>
		// Token: 0x17001568 RID: 5480
		// (get) Token: 0x060057DE RID: 22494 RVA: 0x00185983 File Offset: 0x00183B83
		// (set) Token: 0x060057DF RID: 22495 RVA: 0x0018598B File Offset: 0x00183B8B
		public int Offset
		{
			get
			{
				return this._offset;
			}
			internal set
			{
				this._offset = value;
			}
		}

		/// <summary>Gets or sets the number of symbols that have been added to the control.</summary>
		/// <returns>The number of symbols that have been added to the control.</returns>
		// Token: 0x17001569 RID: 5481
		// (get) Token: 0x060057E0 RID: 22496 RVA: 0x00185994 File Offset: 0x00183B94
		// (set) Token: 0x060057E1 RID: 22497 RVA: 0x0018599C File Offset: 0x00183B9C
		public int AddedLength
		{
			get
			{
				return this._addedLength;
			}
			internal set
			{
				this._addedLength = value;
			}
		}

		/// <summary>Gets or sets the number of symbols that have been removed from the control.</summary>
		/// <returns>The number of symbols that have been removed from the control.</returns>
		// Token: 0x1700156A RID: 5482
		// (get) Token: 0x060057E2 RID: 22498 RVA: 0x001859A5 File Offset: 0x00183BA5
		// (set) Token: 0x060057E3 RID: 22499 RVA: 0x001859AD File Offset: 0x00183BAD
		public int RemovedLength
		{
			get
			{
				return this._removedLength;
			}
			internal set
			{
				this._removedLength = value;
			}
		}

		// Token: 0x04002E8E RID: 11918
		private int _offset;

		// Token: 0x04002E8F RID: 11919
		private int _addedLength;

		// Token: 0x04002E90 RID: 11920
		private int _removedLength;
	}
}
