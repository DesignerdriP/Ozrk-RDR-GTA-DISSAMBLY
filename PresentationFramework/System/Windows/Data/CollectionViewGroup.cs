using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace System.Windows.Data
{
	/// <summary>Represents a group created by a <see cref="T:System.Windows.Data.CollectionView" /> object based on the <see cref="P:System.Windows.Data.CollectionView.GroupDescriptions" />.</summary>
	// Token: 0x020001A8 RID: 424
	public abstract class CollectionViewGroup : INotifyPropertyChanged
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.CollectionViewGroup" /> class with the name of the group.</summary>
		/// <param name="name">The name of this group.</param>
		// Token: 0x06001AD1 RID: 6865 RVA: 0x0007F2E4 File Offset: 0x0007D4E4
		protected CollectionViewGroup(object name)
		{
			this._name = name;
			this._itemsRW = new ObservableCollection<object>();
			this._itemsRO = new ReadOnlyObservableCollection<object>(this._itemsRW);
		}

		/// <summary>Gets the name of this group.</summary>
		/// <returns>The name of this group which is the common value of the property used to divide items into groups.</returns>
		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001AD2 RID: 6866 RVA: 0x0007F30F File Offset: 0x0007D50F
		public object Name
		{
			get
			{
				return this._name;
			}
		}

		/// <summary>Gets the immediate items contained in this group.</summary>
		/// <returns>A read-only collection of the immediate items in this group. This is either a collection of subgroups or a collection of items if this group does not have any subgroups.</returns>
		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06001AD3 RID: 6867 RVA: 0x0007F317 File Offset: 0x0007D517
		public ReadOnlyObservableCollection<object> Items
		{
			get
			{
				return this._itemsRO;
			}
		}

		/// <summary>Gets the number of items in the subtree under this group.</summary>
		/// <returns>The number of items (leaves) in the subtree under this group.</returns>
		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06001AD4 RID: 6868 RVA: 0x0007F31F File Offset: 0x0007D51F
		public int ItemCount
		{
			get
			{
				return this._itemCount;
			}
		}

		/// <summary>Gets a value that indicates whether this group has any subgroups.</summary>
		/// <returns>
		///     <see langword="true" /> if this group is at the bottom level and does not have any subgroups; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06001AD5 RID: 6869
		public abstract bool IsBottomLevel { get; }

		/// <summary>Occurs when a property value changes.</summary>
		// Token: 0x14000050 RID: 80
		// (add) Token: 0x06001AD6 RID: 6870 RVA: 0x0007F327 File Offset: 0x0007D527
		// (remove) Token: 0x06001AD7 RID: 6871 RVA: 0x0007F330 File Offset: 0x0007D530
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this.PropertyChanged += value;
			}
			remove
			{
				this.PropertyChanged -= value;
			}
		}

		/// <summary>Occurs when a property value changes.</summary>
		// Token: 0x14000051 RID: 81
		// (add) Token: 0x06001AD8 RID: 6872 RVA: 0x0007F33C File Offset: 0x0007D53C
		// (remove) Token: 0x06001AD9 RID: 6873 RVA: 0x0007F374 File Offset: 0x0007D574
		protected virtual event PropertyChangedEventHandler PropertyChanged;

		/// <summary>Raises the <see cref="E:System.Windows.Data.CollectionViewGroup.PropertyChanged" /> event using the provided arguments.</summary>
		/// <param name="e">Arguments of the event being raised.</param>
		// Token: 0x06001ADA RID: 6874 RVA: 0x0007F3A9 File Offset: 0x0007D5A9
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, e);
			}
		}

		/// <summary>Gets the immediate items contained in this group.</summary>
		/// <returns>A collection of immediate items in this group. This is either a collection of subgroups or a collection of items if this group does not have any subgroups.</returns>
		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06001ADB RID: 6875 RVA: 0x0007F3C0 File Offset: 0x0007D5C0
		protected ObservableCollection<object> ProtectedItems
		{
			get
			{
				return this._itemsRW;
			}
		}

		/// <summary>Gets and sets the number of items in the subtree under this group.</summary>
		/// <returns>The number of items (leaves) in the subtree under this group</returns>
		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06001ADC RID: 6876 RVA: 0x0007F31F File Offset: 0x0007D51F
		// (set) Token: 0x06001ADD RID: 6877 RVA: 0x0007F3C8 File Offset: 0x0007D5C8
		protected int ProtectedItemCount
		{
			get
			{
				return this._itemCount;
			}
			set
			{
				this._itemCount = value;
				this.OnPropertyChanged(new PropertyChangedEventArgs("ItemCount"));
			}
		}

		// Token: 0x04001375 RID: 4981
		private object _name;

		// Token: 0x04001376 RID: 4982
		private ObservableCollection<object> _itemsRW;

		// Token: 0x04001377 RID: 4983
		private ReadOnlyObservableCollection<object> _itemsRO;

		// Token: 0x04001378 RID: 4984
		private int _itemCount;
	}
}
