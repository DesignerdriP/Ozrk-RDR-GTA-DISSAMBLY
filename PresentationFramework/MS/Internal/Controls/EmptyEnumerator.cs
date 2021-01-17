using System;
using System.Collections;

namespace MS.Internal.Controls
{
	// Token: 0x0200075D RID: 1885
	internal class EmptyEnumerator : IEnumerator
	{
		// Token: 0x060077F8 RID: 30712 RVA: 0x0000326D File Offset: 0x0000146D
		private EmptyEnumerator()
		{
		}

		// Token: 0x17001C65 RID: 7269
		// (get) Token: 0x060077F9 RID: 30713 RVA: 0x00223578 File Offset: 0x00221778
		public static IEnumerator Instance
		{
			get
			{
				if (EmptyEnumerator._instance == null)
				{
					EmptyEnumerator._instance = new EmptyEnumerator();
				}
				return EmptyEnumerator._instance;
			}
		}

		// Token: 0x060077FA RID: 30714 RVA: 0x00002137 File Offset: 0x00000337
		public void Reset()
		{
		}

		// Token: 0x060077FB RID: 30715 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool MoveNext()
		{
			return false;
		}

		// Token: 0x17001C66 RID: 7270
		// (get) Token: 0x060077FC RID: 30716 RVA: 0x001707AE File Offset: 0x0016E9AE
		public object Current
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x040038E0 RID: 14560
		private static IEnumerator _instance;
	}
}
