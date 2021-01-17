using System;
using System.Collections;
using System.Windows;

namespace MS.Internal.Controls
{
	// Token: 0x0200075F RID: 1887
	internal abstract class ModelTreeEnumerator : IEnumerator
	{
		// Token: 0x0600782B RID: 30763 RVA: 0x00223EDB File Offset: 0x002220DB
		internal ModelTreeEnumerator(object content)
		{
			this._content = content;
		}

		// Token: 0x17001C79 RID: 7289
		// (get) Token: 0x0600782C RID: 30764 RVA: 0x00223EF1 File Offset: 0x002220F1
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x0600782D RID: 30765 RVA: 0x00223EF9 File Offset: 0x002220F9
		bool IEnumerator.MoveNext()
		{
			return this.MoveNext();
		}

		// Token: 0x0600782E RID: 30766 RVA: 0x00223F01 File Offset: 0x00222101
		void IEnumerator.Reset()
		{
			this.Reset();
		}

		// Token: 0x17001C7A RID: 7290
		// (get) Token: 0x0600782F RID: 30767 RVA: 0x00223F09 File Offset: 0x00222109
		protected object Content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x17001C7B RID: 7291
		// (get) Token: 0x06007830 RID: 30768 RVA: 0x00223F11 File Offset: 0x00222111
		// (set) Token: 0x06007831 RID: 30769 RVA: 0x00223F19 File Offset: 0x00222119
		protected int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x17001C7C RID: 7292
		// (get) Token: 0x06007832 RID: 30770 RVA: 0x00223F22 File Offset: 0x00222122
		protected virtual object Current
		{
			get
			{
				if (this._index == 0)
				{
					return this._content;
				}
				throw new InvalidOperationException(SR.Get("EnumeratorInvalidOperation"));
			}
		}

		// Token: 0x06007833 RID: 30771 RVA: 0x00223F42 File Offset: 0x00222142
		protected virtual bool MoveNext()
		{
			if (this._index < 1)
			{
				this._index++;
				if (this._index == 0)
				{
					this.VerifyUnchanged();
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007834 RID: 30772 RVA: 0x00223F6C File Offset: 0x0022216C
		protected virtual void Reset()
		{
			this.VerifyUnchanged();
			this._index = -1;
		}

		// Token: 0x17001C7D RID: 7293
		// (get) Token: 0x06007835 RID: 30773
		protected abstract bool IsUnchanged { get; }

		// Token: 0x06007836 RID: 30774 RVA: 0x00223F7B File Offset: 0x0022217B
		protected void VerifyUnchanged()
		{
			if (!this.IsUnchanged)
			{
				throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
			}
		}

		// Token: 0x040038E7 RID: 14567
		private int _index = -1;

		// Token: 0x040038E8 RID: 14568
		private object _content;
	}
}
