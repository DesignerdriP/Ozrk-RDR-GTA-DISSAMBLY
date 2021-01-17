using System;
using System.Collections;

namespace MS.Internal.Data
{
	// Token: 0x02000717 RID: 1815
	internal static class DataExtensionMethods
	{
		// Token: 0x060074C9 RID: 29897 RVA: 0x002167D8 File Offset: 0x002149D8
		internal static int Search(this IList list, int index, int count, object value, IComparer comparer)
		{
			ArrayList arrayList;
			if ((arrayList = (list as ArrayList)) != null)
			{
				return arrayList.BinarySearch(index, count, value, comparer);
			}
			LiveShapingList liveShapingList;
			if ((liveShapingList = (list as LiveShapingList)) != null)
			{
				return liveShapingList.Search(index, count, value);
			}
			return 0;
		}

		// Token: 0x060074CA RID: 29898 RVA: 0x00216810 File Offset: 0x00214A10
		internal static int Search(this IList list, object value, IComparer comparer)
		{
			return list.Search(0, list.Count, value, comparer);
		}

		// Token: 0x060074CB RID: 29899 RVA: 0x00216824 File Offset: 0x00214A24
		internal static void Move(this IList list, int oldIndex, int newIndex)
		{
			ArrayList arrayList;
			if ((arrayList = (list as ArrayList)) != null)
			{
				object value = arrayList[oldIndex];
				arrayList.RemoveAt(oldIndex);
				arrayList.Insert(newIndex, value);
				return;
			}
			LiveShapingList liveShapingList;
			if ((liveShapingList = (list as LiveShapingList)) != null)
			{
				liveShapingList.Move(oldIndex, newIndex);
			}
		}

		// Token: 0x060074CC RID: 29900 RVA: 0x00216868 File Offset: 0x00214A68
		internal static void Sort(this IList list, IComparer comparer)
		{
			ArrayList al;
			if ((al = (list as ArrayList)) != null)
			{
				SortFieldComparer.SortHelper(al, comparer);
				return;
			}
			LiveShapingList liveShapingList;
			if ((liveShapingList = (list as LiveShapingList)) != null)
			{
				liveShapingList.Sort();
			}
		}
	}
}
