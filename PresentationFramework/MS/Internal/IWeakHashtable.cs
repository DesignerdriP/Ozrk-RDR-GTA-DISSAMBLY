using System;
using System.Collections;

namespace MS.Internal
{
	// Token: 0x020005EA RID: 1514
	internal interface IWeakHashtable
	{
		// Token: 0x17001841 RID: 6209
		object this[object key]
		{
			get;
		}

		// Token: 0x17001842 RID: 6210
		// (get) Token: 0x060064F2 RID: 25842
		ICollection Keys { get; }

		// Token: 0x17001843 RID: 6211
		// (get) Token: 0x060064F3 RID: 25843
		int Count { get; }

		// Token: 0x060064F4 RID: 25844
		bool ContainsKey(object key);

		// Token: 0x060064F5 RID: 25845
		void Remove(object key);

		// Token: 0x060064F6 RID: 25846
		void Clear();

		// Token: 0x060064F7 RID: 25847
		void SetWeak(object key, object value);

		// Token: 0x060064F8 RID: 25848
		object UnwrapKey(object key);
	}
}
