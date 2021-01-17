using System;
using System.Collections;

namespace MS.Internal.Controls
{
	// Token: 0x0200075C RID: 1884
	internal class EmptyEnumerable : IEnumerable
	{
		// Token: 0x060077F5 RID: 30709 RVA: 0x0000326D File Offset: 0x0000146D
		private EmptyEnumerable()
		{
		}

		// Token: 0x060077F6 RID: 30710 RVA: 0x00222849 File Offset: 0x00220A49
		IEnumerator IEnumerable.GetEnumerator()
		{
			return EmptyEnumerator.Instance;
		}

		// Token: 0x17001C64 RID: 7268
		// (get) Token: 0x060077F7 RID: 30711 RVA: 0x00223560 File Offset: 0x00221760
		public static IEnumerable Instance
		{
			get
			{
				if (EmptyEnumerable._instance == null)
				{
					EmptyEnumerable._instance = new EmptyEnumerable();
				}
				return EmptyEnumerable._instance;
			}
		}

		// Token: 0x040038DF RID: 14559
		private static IEnumerable _instance;
	}
}
