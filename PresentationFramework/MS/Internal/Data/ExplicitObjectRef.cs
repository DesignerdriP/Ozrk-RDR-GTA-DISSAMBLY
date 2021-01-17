using System;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000736 RID: 1846
	internal sealed class ExplicitObjectRef : ObjectRef
	{
		// Token: 0x0600760A RID: 30218 RVA: 0x0021AABA File Offset: 0x00218CBA
		internal ExplicitObjectRef(object o)
		{
			if (o is DependencyObject)
			{
				this._element = new WeakReference(o);
				return;
			}
			this._object = o;
		}

		// Token: 0x0600760B RID: 30219 RVA: 0x0021AADE File Offset: 0x00218CDE
		internal override object GetObject(DependencyObject d, ObjectRefArgs args)
		{
			if (this._element == null)
			{
				return this._object;
			}
			return this._element.Target;
		}

		// Token: 0x17001C1D RID: 7197
		// (get) Token: 0x0600760C RID: 30220 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override bool ProtectedUsesMentor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600760D RID: 30221 RVA: 0x0021AAFA File Offset: 0x00218CFA
		internal override string Identify()
		{
			return "Source";
		}

		// Token: 0x04003853 RID: 14419
		private object _object;

		// Token: 0x04003854 RID: 14420
		private WeakReference _element;
	}
}
