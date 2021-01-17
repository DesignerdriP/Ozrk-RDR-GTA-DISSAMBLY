using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MS.Internal.Data
{
	// Token: 0x0200070F RID: 1807
	internal class CommitManager
	{
		// Token: 0x17001BAD RID: 7085
		// (get) Token: 0x06007430 RID: 29744 RVA: 0x00213599 File Offset: 0x00211799
		internal bool IsEmpty
		{
			get
			{
				return this._bindings.Count == 0 && this._bindingGroups.Count == 0;
			}
		}

		// Token: 0x06007431 RID: 29745 RVA: 0x002135B8 File Offset: 0x002117B8
		internal void AddBindingGroup(BindingGroup bindingGroup)
		{
			this._bindingGroups.Add(bindingGroup);
		}

		// Token: 0x06007432 RID: 29746 RVA: 0x002135C6 File Offset: 0x002117C6
		internal void RemoveBindingGroup(BindingGroup bindingGroup)
		{
			this._bindingGroups.Remove(bindingGroup);
		}

		// Token: 0x06007433 RID: 29747 RVA: 0x002135D5 File Offset: 0x002117D5
		internal void AddBinding(BindingExpressionBase binding)
		{
			this._bindings.Add(binding);
		}

		// Token: 0x06007434 RID: 29748 RVA: 0x002135E3 File Offset: 0x002117E3
		internal void RemoveBinding(BindingExpressionBase binding)
		{
			this._bindings.Remove(binding);
		}

		// Token: 0x06007435 RID: 29749 RVA: 0x002135F4 File Offset: 0x002117F4
		internal List<BindingGroup> GetBindingGroupsInScope(DependencyObject element)
		{
			List<BindingGroup> list = this._bindingGroups.ToList();
			List<BindingGroup> list2 = CommitManager.EmptyBindingGroupList;
			foreach (BindingGroup bindingGroup in list)
			{
				DependencyObject owner = bindingGroup.Owner;
				if (owner != null && this.IsInScope(element, owner))
				{
					if (list2 == CommitManager.EmptyBindingGroupList)
					{
						list2 = new List<BindingGroup>();
					}
					list2.Add(bindingGroup);
				}
			}
			return list2;
		}

		// Token: 0x06007436 RID: 29750 RVA: 0x0021367C File Offset: 0x0021187C
		internal List<BindingExpressionBase> GetBindingsInScope(DependencyObject element)
		{
			List<BindingExpressionBase> list = this._bindings.ToList();
			List<BindingExpressionBase> list2 = CommitManager.EmptyBindingList;
			foreach (BindingExpressionBase bindingExpressionBase in list)
			{
				DependencyObject targetElement = bindingExpressionBase.TargetElement;
				if (targetElement != null && bindingExpressionBase.IsEligibleForCommit && this.IsInScope(element, targetElement))
				{
					if (list2 == CommitManager.EmptyBindingList)
					{
						list2 = new List<BindingExpressionBase>();
					}
					list2.Add(bindingExpressionBase);
				}
			}
			return list2;
		}

		// Token: 0x06007437 RID: 29751 RVA: 0x0021370C File Offset: 0x0021190C
		internal bool Purge()
		{
			bool flag = false;
			int count = this._bindings.Count;
			if (count > 0)
			{
				List<BindingExpressionBase> list = this._bindings.ToList();
				foreach (BindingExpressionBase bindingExpressionBase in list)
				{
					DependencyObject targetElement = bindingExpressionBase.TargetElement;
				}
			}
			flag = (flag || this._bindings.Count < count);
			count = this._bindingGroups.Count;
			if (count > 0)
			{
				List<BindingGroup> list2 = this._bindingGroups.ToList();
				foreach (BindingGroup bindingGroup in list2)
				{
					DependencyObject owner = bindingGroup.Owner;
				}
			}
			flag = (flag || this._bindingGroups.Count < count);
			return flag;
		}

		// Token: 0x06007438 RID: 29752 RVA: 0x00213808 File Offset: 0x00211A08
		private bool IsInScope(DependencyObject ancestor, DependencyObject element)
		{
			return ancestor == null || VisualTreeHelper.IsAncestorOf(ancestor, element);
		}

		// Token: 0x040037C8 RID: 14280
		private CommitManager.Set<BindingGroup> _bindingGroups = new CommitManager.Set<BindingGroup>();

		// Token: 0x040037C9 RID: 14281
		private CommitManager.Set<BindingExpressionBase> _bindings = new CommitManager.Set<BindingExpressionBase>();

		// Token: 0x040037CA RID: 14282
		private static readonly List<BindingGroup> EmptyBindingGroupList = new List<BindingGroup>();

		// Token: 0x040037CB RID: 14283
		private static readonly List<BindingExpressionBase> EmptyBindingList = new List<BindingExpressionBase>();

		// Token: 0x02000B47 RID: 2887
		private class Set<T> : Dictionary<T, object>, IEnumerable<T>, IEnumerable
		{
			// Token: 0x06008D98 RID: 36248 RVA: 0x00259AA5 File Offset: 0x00257CA5
			public Set()
			{
			}

			// Token: 0x06008D99 RID: 36249 RVA: 0x00259AAD File Offset: 0x00257CAD
			public Set(IDictionary<T, object> other) : base(other)
			{
			}

			// Token: 0x06008D9A RID: 36250 RVA: 0x00259AB6 File Offset: 0x00257CB6
			public Set(IEqualityComparer<T> comparer) : base(comparer)
			{
			}

			// Token: 0x06008D9B RID: 36251 RVA: 0x00259ABF File Offset: 0x00257CBF
			public void Add(T item)
			{
				if (!base.ContainsKey(item))
				{
					base.Add(item, null);
				}
			}

			// Token: 0x06008D9C RID: 36252 RVA: 0x00259AD2 File Offset: 0x00257CD2
			IEnumerator<T> IEnumerable<!0>.GetEnumerator()
			{
				return base.Keys.GetEnumerator();
			}

			// Token: 0x06008D9D RID: 36253 RVA: 0x00259AE4 File Offset: 0x00257CE4
			public List<T> ToList()
			{
				return new List<T>(base.Keys);
			}
		}
	}
}
