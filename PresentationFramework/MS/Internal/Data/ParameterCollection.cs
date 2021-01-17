using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000737 RID: 1847
	internal class ParameterCollection : Collection<object>, IList, ICollection, IEnumerable
	{
		// Token: 0x0600760E RID: 30222 RVA: 0x0021AB01 File Offset: 0x00218D01
		public ParameterCollection(ParameterCollectionChanged parametersChanged)
		{
			this._parametersChanged = parametersChanged;
		}

		// Token: 0x17001C1E RID: 7198
		// (get) Token: 0x0600760F RID: 30223 RVA: 0x0021AB10 File Offset: 0x00218D10
		bool IList.IsReadOnly
		{
			get
			{
				return this.IsReadOnly;
			}
		}

		// Token: 0x17001C1F RID: 7199
		// (get) Token: 0x06007610 RID: 30224 RVA: 0x0021AB18 File Offset: 0x00218D18
		bool IList.IsFixedSize
		{
			get
			{
				return this.IsFixedSize;
			}
		}

		// Token: 0x06007611 RID: 30225 RVA: 0x0021AB20 File Offset: 0x00218D20
		protected override void ClearItems()
		{
			this.CheckReadOnly();
			base.ClearItems();
			this.OnCollectionChanged();
		}

		// Token: 0x06007612 RID: 30226 RVA: 0x0021AB34 File Offset: 0x00218D34
		protected override void InsertItem(int index, object value)
		{
			this.CheckReadOnly();
			base.InsertItem(index, value);
			this.OnCollectionChanged();
		}

		// Token: 0x06007613 RID: 30227 RVA: 0x0021AB4A File Offset: 0x00218D4A
		protected override void RemoveItem(int index)
		{
			this.CheckReadOnly();
			base.RemoveItem(index);
			this.OnCollectionChanged();
		}

		// Token: 0x06007614 RID: 30228 RVA: 0x0021AB5F File Offset: 0x00218D5F
		protected override void SetItem(int index, object value)
		{
			this.CheckReadOnly();
			base.SetItem(index, value);
			this.OnCollectionChanged();
		}

		// Token: 0x17001C20 RID: 7200
		// (get) Token: 0x06007615 RID: 30229 RVA: 0x0021AB75 File Offset: 0x00218D75
		// (set) Token: 0x06007616 RID: 30230 RVA: 0x0021AB7D File Offset: 0x00218D7D
		protected virtual bool IsReadOnly
		{
			get
			{
				return this._isReadOnly;
			}
			set
			{
				this._isReadOnly = value;
			}
		}

		// Token: 0x17001C21 RID: 7201
		// (get) Token: 0x06007617 RID: 30231 RVA: 0x0021AB10 File Offset: 0x00218D10
		protected bool IsFixedSize
		{
			get
			{
				return this.IsReadOnly;
			}
		}

		// Token: 0x06007618 RID: 30232 RVA: 0x0021AB86 File Offset: 0x00218D86
		internal void SetReadOnly(bool isReadOnly)
		{
			this.IsReadOnly = isReadOnly;
		}

		// Token: 0x06007619 RID: 30233 RVA: 0x0021AB8F File Offset: 0x00218D8F
		internal void ClearInternal()
		{
			base.ClearItems();
		}

		// Token: 0x0600761A RID: 30234 RVA: 0x0021AB97 File Offset: 0x00218D97
		private void CheckReadOnly()
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("ObjectDataProviderParameterCollectionIsNotInUse"));
			}
		}

		// Token: 0x0600761B RID: 30235 RVA: 0x0021ABB1 File Offset: 0x00218DB1
		private void OnCollectionChanged()
		{
			this._parametersChanged(this);
		}

		// Token: 0x04003855 RID: 14421
		private bool _isReadOnly;

		// Token: 0x04003856 RID: 14422
		private ParameterCollectionChanged _parametersChanged;
	}
}
