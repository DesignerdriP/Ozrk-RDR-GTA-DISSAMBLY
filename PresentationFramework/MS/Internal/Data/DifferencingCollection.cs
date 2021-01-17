using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Controls;

namespace MS.Internal.Data
{
	// Token: 0x02000722 RID: 1826
	internal sealed class DifferencingCollection : ObservableCollection<object>
	{
		// Token: 0x060074FF RID: 29951 RVA: 0x002174DA File Offset: 0x002156DA
		internal DifferencingCollection(IEnumerable enumerable)
		{
			this.LoadItems(enumerable);
		}

		// Token: 0x06007500 RID: 29952 RVA: 0x002174EC File Offset: 0x002156EC
		internal void Update(IEnumerable enumerable)
		{
			IList<object> items = base.Items;
			int num = -1;
			int newIndex = -1;
			int count = items.Count;
			DifferencingCollection.Change change = DifferencingCollection.Change.None;
			int num2 = 0;
			object obj = DifferencingCollection.Unset;
			foreach (object obj2 in enumerable)
			{
				if (num2 < count && ItemsControl.EqualsEx(obj2, items[num2]))
				{
					num2++;
				}
				else
				{
					switch (change)
					{
					case DifferencingCollection.Change.None:
						if (num2 + 1 < count && ItemsControl.EqualsEx(obj2, items[num2 + 1]))
						{
							change = DifferencingCollection.Change.Remove;
							num = num2;
							obj = items[num2];
							num2 += 2;
						}
						else
						{
							change = DifferencingCollection.Change.Add;
							num = num2;
							obj = obj2;
						}
						break;
					case DifferencingCollection.Change.Add:
						if (num2 + 1 < count && ItemsControl.EqualsEx(obj2, items[num2 + 1]))
						{
							if (ItemsControl.EqualsEx(obj, items[num2]))
							{
								change = DifferencingCollection.Change.Move;
								newIndex = num;
								num = num2;
							}
							else if (num2 < count && num2 == num)
							{
								change = DifferencingCollection.Change.Replace;
							}
							else
							{
								change = DifferencingCollection.Change.Reset;
							}
							num2 += 2;
						}
						else
						{
							change = DifferencingCollection.Change.Reset;
						}
						break;
					case DifferencingCollection.Change.Remove:
						if (ItemsControl.EqualsEx(obj2, obj))
						{
							change = DifferencingCollection.Change.Move;
							newIndex = num2 - 1;
						}
						else
						{
							change = DifferencingCollection.Change.Reset;
						}
						break;
					default:
						change = DifferencingCollection.Change.Reset;
						break;
					}
					if (change == DifferencingCollection.Change.Reset)
					{
						break;
					}
				}
			}
			if (num2 == count - 1)
			{
				if (change != DifferencingCollection.Change.None)
				{
					if (change != DifferencingCollection.Change.Add)
					{
						change = DifferencingCollection.Change.Reset;
					}
					else if (ItemsControl.EqualsEx(obj, items[num2]))
					{
						change = DifferencingCollection.Change.Move;
						newIndex = num;
						num = num2;
					}
					else if (num == count - 1)
					{
						change = DifferencingCollection.Change.Replace;
					}
					else
					{
						change = DifferencingCollection.Change.Reset;
					}
				}
				else
				{
					change = DifferencingCollection.Change.Remove;
					num = num2;
				}
			}
			else if (num2 != count)
			{
				change = DifferencingCollection.Change.Reset;
			}
			switch (change)
			{
			case DifferencingCollection.Change.None:
				break;
			case DifferencingCollection.Change.Add:
				Invariant.Assert(obj != DifferencingCollection.Unset);
				base.Insert(num, obj);
				return;
			case DifferencingCollection.Change.Remove:
				base.RemoveAt(num);
				return;
			case DifferencingCollection.Change.Move:
				base.Move(num, newIndex);
				return;
			case DifferencingCollection.Change.Replace:
				Invariant.Assert(obj != DifferencingCollection.Unset);
				base[num] = obj;
				return;
			case DifferencingCollection.Change.Reset:
				this.Reload(enumerable);
				break;
			default:
				return;
			}
		}

		// Token: 0x06007501 RID: 29953 RVA: 0x0021771C File Offset: 0x0021591C
		private void LoadItems(IEnumerable enumerable)
		{
			foreach (object item in enumerable)
			{
				base.Items.Add(item);
			}
		}

		// Token: 0x06007502 RID: 29954 RVA: 0x00217770 File Offset: 0x00215970
		private void Reload(IEnumerable enumerable)
		{
			base.Items.Clear();
			this.LoadItems(enumerable);
			this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
			this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x0400380D RID: 14349
		private static object Unset = new object();

		// Token: 0x02000B4C RID: 2892
		private enum Change
		{
			// Token: 0x04004ADE RID: 19166
			None,
			// Token: 0x04004ADF RID: 19167
			Add,
			// Token: 0x04004AE0 RID: 19168
			Remove,
			// Token: 0x04004AE1 RID: 19169
			Move,
			// Token: 0x04004AE2 RID: 19170
			Replace,
			// Token: 0x04004AE3 RID: 19171
			Reset
		}
	}
}
