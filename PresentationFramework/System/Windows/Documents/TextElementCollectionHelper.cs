using System;

namespace System.Windows.Documents
{
	// Token: 0x02000402 RID: 1026
	internal static class TextElementCollectionHelper
	{
		// Token: 0x060039A1 RID: 14753 RVA: 0x001055B0 File Offset: 0x001037B0
		internal static void MarkDirty(DependencyObject parent)
		{
			if (parent == null)
			{
				return;
			}
			WeakReference[] cleanParentList = TextElementCollectionHelper._cleanParentList;
			lock (cleanParentList)
			{
				for (int i = 0; i < TextElementCollectionHelper._cleanParentList.Length; i++)
				{
					if (TextElementCollectionHelper._cleanParentList[i] != null)
					{
						TextElementCollectionHelper.ParentCollectionPair parentCollectionPair = (TextElementCollectionHelper.ParentCollectionPair)TextElementCollectionHelper._cleanParentList[i].Target;
						if (parentCollectionPair == null || parentCollectionPair.Parent == parent)
						{
							TextElementCollectionHelper._cleanParentList[i] = null;
						}
					}
				}
			}
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x00105630 File Offset: 0x00103830
		internal static void MarkClean(DependencyObject parent, object collection)
		{
			WeakReference[] cleanParentList = TextElementCollectionHelper._cleanParentList;
			lock (cleanParentList)
			{
				int num2;
				int num = TextElementCollectionHelper.GetCleanParentIndex(parent, collection, out num2);
				if (num == -1)
				{
					num = ((num2 >= 0) ? num2 : (TextElementCollectionHelper._cleanParentList.Length - 1));
					TextElementCollectionHelper._cleanParentList[num] = new WeakReference(new TextElementCollectionHelper.ParentCollectionPair(parent, collection));
				}
				TextElementCollectionHelper.TouchCleanParent(num);
			}
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x001056A4 File Offset: 0x001038A4
		internal static bool IsCleanParent(DependencyObject parent, object collection)
		{
			int num = -1;
			WeakReference[] cleanParentList = TextElementCollectionHelper._cleanParentList;
			lock (cleanParentList)
			{
				int num2;
				num = TextElementCollectionHelper.GetCleanParentIndex(parent, collection, out num2);
				if (num >= 0)
				{
					TextElementCollectionHelper.TouchCleanParent(num);
				}
			}
			return num >= 0;
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x001056FC File Offset: 0x001038FC
		private static void TouchCleanParent(int index)
		{
			WeakReference weakReference = TextElementCollectionHelper._cleanParentList[index];
			Array.Copy(TextElementCollectionHelper._cleanParentList, 0, TextElementCollectionHelper._cleanParentList, 1, index);
			TextElementCollectionHelper._cleanParentList[0] = weakReference;
		}

		// Token: 0x060039A5 RID: 14757 RVA: 0x0010572C File Offset: 0x0010392C
		private static int GetCleanParentIndex(DependencyObject parent, object collection, out int firstFreeIndex)
		{
			int result = -1;
			firstFreeIndex = -1;
			for (int i = 0; i < TextElementCollectionHelper._cleanParentList.Length; i++)
			{
				if (TextElementCollectionHelper._cleanParentList[i] == null)
				{
					if (firstFreeIndex == -1)
					{
						firstFreeIndex = i;
					}
				}
				else
				{
					TextElementCollectionHelper.ParentCollectionPair parentCollectionPair = (TextElementCollectionHelper.ParentCollectionPair)TextElementCollectionHelper._cleanParentList[i].Target;
					if (parentCollectionPair == null)
					{
						TextElementCollectionHelper._cleanParentList[i] = null;
						if (firstFreeIndex == -1)
						{
							firstFreeIndex = i;
						}
					}
					else if (parentCollectionPair.Parent == parent && parentCollectionPair.Collection == collection)
					{
						result = i;
					}
				}
			}
			return result;
		}

		// Token: 0x040025B1 RID: 9649
		private static readonly WeakReference[] _cleanParentList = new WeakReference[10];

		// Token: 0x02000905 RID: 2309
		private class ParentCollectionPair
		{
			// Token: 0x060085CF RID: 34255 RVA: 0x0024AFDE File Offset: 0x002491DE
			internal ParentCollectionPair(DependencyObject parent, object collection)
			{
				this._parent = parent;
				this._collection = collection;
			}

			// Token: 0x17001E36 RID: 7734
			// (get) Token: 0x060085D0 RID: 34256 RVA: 0x0024AFF4 File Offset: 0x002491F4
			internal DependencyObject Parent
			{
				get
				{
					return this._parent;
				}
			}

			// Token: 0x17001E37 RID: 7735
			// (get) Token: 0x060085D1 RID: 34257 RVA: 0x0024AFFC File Offset: 0x002491FC
			internal object Collection
			{
				get
				{
					return this._collection;
				}
			}

			// Token: 0x04004302 RID: 17154
			private readonly DependencyObject _parent;

			// Token: 0x04004303 RID: 17155
			private readonly object _collection;
		}
	}
}
