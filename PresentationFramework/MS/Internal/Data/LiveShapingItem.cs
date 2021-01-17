using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200072D RID: 1837
	internal class LiveShapingItem : DependencyObject
	{
		// Token: 0x06007583 RID: 30083 RVA: 0x002192F4 File Offset: 0x002174F4
		internal LiveShapingItem(object item, LiveShapingList list, bool filtered = false, LiveShapingBlock block = null, bool oneTime = false)
		{
			this._block = block;
			list.InitializeItem(this, item, filtered, oneTime);
			this.ForwardChanges = !oneTime;
		}

		// Token: 0x17001BF9 RID: 7161
		// (get) Token: 0x06007584 RID: 30084 RVA: 0x0021931A File Offset: 0x0021751A
		// (set) Token: 0x06007585 RID: 30085 RVA: 0x00219322 File Offset: 0x00217522
		internal object Item
		{
			get
			{
				return this._item;
			}
			set
			{
				this._item = value;
			}
		}

		// Token: 0x17001BFA RID: 7162
		// (get) Token: 0x06007586 RID: 30086 RVA: 0x0021932B File Offset: 0x0021752B
		// (set) Token: 0x06007587 RID: 30087 RVA: 0x00219333 File Offset: 0x00217533
		internal LiveShapingBlock Block
		{
			get
			{
				return this._block;
			}
			set
			{
				this._block = value;
			}
		}

		// Token: 0x17001BFB RID: 7163
		// (get) Token: 0x06007588 RID: 30088 RVA: 0x0021933C File Offset: 0x0021753C
		private LiveShapingList List
		{
			get
			{
				return this.Block.List;
			}
		}

		// Token: 0x17001BFC RID: 7164
		// (get) Token: 0x06007589 RID: 30089 RVA: 0x00219349 File Offset: 0x00217549
		// (set) Token: 0x0600758A RID: 30090 RVA: 0x00219352 File Offset: 0x00217552
		internal bool IsSortDirty
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.IsSortDirty);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.IsSortDirty, value);
			}
		}

		// Token: 0x17001BFD RID: 7165
		// (get) Token: 0x0600758B RID: 30091 RVA: 0x0021935C File Offset: 0x0021755C
		// (set) Token: 0x0600758C RID: 30092 RVA: 0x00219365 File Offset: 0x00217565
		internal bool IsSortPendingClean
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.IsSortPendingClean);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.IsSortPendingClean, value);
			}
		}

		// Token: 0x17001BFE RID: 7166
		// (get) Token: 0x0600758D RID: 30093 RVA: 0x0021936F File Offset: 0x0021756F
		// (set) Token: 0x0600758E RID: 30094 RVA: 0x00219378 File Offset: 0x00217578
		internal bool IsFilterDirty
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.IsFilterDirty);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.IsFilterDirty, value);
			}
		}

		// Token: 0x17001BFF RID: 7167
		// (get) Token: 0x0600758F RID: 30095 RVA: 0x00219382 File Offset: 0x00217582
		// (set) Token: 0x06007590 RID: 30096 RVA: 0x0021938B File Offset: 0x0021758B
		internal bool IsGroupDirty
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.IsGroupDirty);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.IsGroupDirty, value);
			}
		}

		// Token: 0x17001C00 RID: 7168
		// (get) Token: 0x06007591 RID: 30097 RVA: 0x00219395 File Offset: 0x00217595
		// (set) Token: 0x06007592 RID: 30098 RVA: 0x0021939F File Offset: 0x0021759F
		internal bool FailsFilter
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.FailsFilter);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.FailsFilter, value);
			}
		}

		// Token: 0x17001C01 RID: 7169
		// (get) Token: 0x06007593 RID: 30099 RVA: 0x002193AA File Offset: 0x002175AA
		// (set) Token: 0x06007594 RID: 30100 RVA: 0x002193B4 File Offset: 0x002175B4
		internal bool ForwardChanges
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.ForwardChanges);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.ForwardChanges, value);
			}
		}

		// Token: 0x17001C02 RID: 7170
		// (get) Token: 0x06007595 RID: 30101 RVA: 0x002193BF File Offset: 0x002175BF
		// (set) Token: 0x06007596 RID: 30102 RVA: 0x002193C9 File Offset: 0x002175C9
		internal bool IsDeleted
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.IsDeleted);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.IsDeleted, value);
			}
		}

		// Token: 0x06007597 RID: 30103 RVA: 0x002193D4 File Offset: 0x002175D4
		internal void FindPosition(out RBFinger<LiveShapingItem> oldFinger, out RBFinger<LiveShapingItem> newFinger, Comparison<LiveShapingItem> comparison)
		{
			this.Block.FindPosition(this, out oldFinger, out newFinger, comparison);
		}

		// Token: 0x06007598 RID: 30104 RVA: 0x002193E5 File Offset: 0x002175E5
		internal RBFinger<LiveShapingItem> GetFinger()
		{
			return this.Block.GetFinger(this);
		}

		// Token: 0x17001C03 RID: 7171
		// (get) Token: 0x06007599 RID: 30105 RVA: 0x002193F3 File Offset: 0x002175F3
		// (set) Token: 0x0600759A RID: 30106 RVA: 0x00219405 File Offset: 0x00217605
		internal int StartingIndex
		{
			get
			{
				return (int)base.GetValue(LiveShapingItem.StartingIndexProperty);
			}
			set
			{
				base.SetValue(LiveShapingItem.StartingIndexProperty, value);
			}
		}

		// Token: 0x0600759B RID: 30107 RVA: 0x00219418 File Offset: 0x00217618
		internal int GetAndClearStartingIndex()
		{
			int startingIndex = this.StartingIndex;
			base.ClearValue(LiveShapingItem.StartingIndexProperty);
			return startingIndex;
		}

		// Token: 0x0600759C RID: 30108 RVA: 0x00219438 File Offset: 0x00217638
		internal void SetBinding(string path, DependencyProperty dp, bool oneTime = false, bool enableXT = false)
		{
			if (enableXT && oneTime)
			{
				enableXT = false;
			}
			if (!base.LookupEntry(dp.GlobalIndex).Found)
			{
				if (!string.IsNullOrEmpty(path))
				{
					Binding binding;
					if (SystemXmlHelper.IsXmlNode(this._item))
					{
						binding = new Binding();
						binding.XPath = path;
					}
					else
					{
						binding = new Binding(path);
					}
					binding.Source = this._item;
					if (oneTime)
					{
						binding.Mode = BindingMode.OneTime;
					}
					BindingExpressionBase bindingExpressionBase = binding.CreateBindingExpression(this, dp);
					if (enableXT)
					{
						bindingExpressionBase.TargetWantsCrossThreadNotifications = true;
					}
					base.SetValue(dp, bindingExpressionBase);
					return;
				}
				if (!oneTime)
				{
					INotifyPropertyChanged notifyPropertyChanged = this.Item as INotifyPropertyChanged;
					if (notifyPropertyChanged != null)
					{
						PropertyChangedEventManager.AddHandler(notifyPropertyChanged, new EventHandler<PropertyChangedEventArgs>(this.OnPropertyChanged), string.Empty);
					}
				}
			}
		}

		// Token: 0x0600759D RID: 30109 RVA: 0x002194EE File Offset: 0x002176EE
		internal object GetValue(string path, DependencyProperty dp)
		{
			if (!string.IsNullOrEmpty(path))
			{
				this.SetBinding(path, dp, false, false);
				return base.GetValue(dp);
			}
			return this.Item;
		}

		// Token: 0x0600759E RID: 30110 RVA: 0x00219510 File Offset: 0x00217710
		internal void Clear()
		{
			this.List.ClearItem(this);
		}

		// Token: 0x0600759F RID: 30111 RVA: 0x0021951E File Offset: 0x0021771E
		internal void OnCrossThreadPropertyChange(DependencyProperty dp)
		{
			this.List.OnItemPropertyChangedCrossThread(this, dp);
		}

		// Token: 0x060075A0 RID: 30112 RVA: 0x00219530 File Offset: 0x00217730
		internal void AddParentGroup(CollectionViewGroupInternal group)
		{
			object value = base.GetValue(LiveShapingItem.ParentGroupsProperty);
			if (value == null)
			{
				base.SetValue(LiveShapingItem.ParentGroupsProperty, group);
				return;
			}
			List<CollectionViewGroupInternal> list;
			if ((list = (value as List<CollectionViewGroupInternal>)) == null)
			{
				list = new List<CollectionViewGroupInternal>(2);
				list.Add(value as CollectionViewGroupInternal);
				list.Add(group);
				base.SetValue(LiveShapingItem.ParentGroupsProperty, list);
				return;
			}
			list.Add(group);
		}

		// Token: 0x060075A1 RID: 30113 RVA: 0x00219594 File Offset: 0x00217794
		internal void RemoveParentGroup(CollectionViewGroupInternal group)
		{
			object value = base.GetValue(LiveShapingItem.ParentGroupsProperty);
			List<CollectionViewGroupInternal> list = value as List<CollectionViewGroupInternal>;
			if (list == null)
			{
				if (value == group)
				{
					base.ClearValue(LiveShapingItem.ParentGroupsProperty);
					return;
				}
			}
			else
			{
				list.Remove(group);
				if (list.Count == 1)
				{
					base.SetValue(LiveShapingItem.ParentGroupsProperty, list[0]);
				}
			}
		}

		// Token: 0x17001C04 RID: 7172
		// (get) Token: 0x060075A2 RID: 30114 RVA: 0x002195EA File Offset: 0x002177EA
		internal List<CollectionViewGroupInternal> ParentGroups
		{
			get
			{
				return base.GetValue(LiveShapingItem.ParentGroupsProperty) as List<CollectionViewGroupInternal>;
			}
		}

		// Token: 0x17001C05 RID: 7173
		// (get) Token: 0x060075A3 RID: 30115 RVA: 0x002195FC File Offset: 0x002177FC
		internal CollectionViewGroupInternal ParentGroup
		{
			get
			{
				return base.GetValue(LiveShapingItem.ParentGroupsProperty) as CollectionViewGroupInternal;
			}
		}

		// Token: 0x060075A4 RID: 30116 RVA: 0x0021960E File Offset: 0x0021780E
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			if (this.ForwardChanges)
			{
				this.List.OnItemPropertyChanged(this, e.Property);
			}
		}

		// Token: 0x060075A5 RID: 30117 RVA: 0x0021962B File Offset: 0x0021782B
		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.List.OnItemPropertyChanged(this, null);
		}

		// Token: 0x060075A6 RID: 30118 RVA: 0x0021963A File Offset: 0x0021783A
		private bool TestFlag(LiveShapingItem.PrivateFlags flag)
		{
			return (this._flags & flag) > (LiveShapingItem.PrivateFlags)0;
		}

		// Token: 0x060075A7 RID: 30119 RVA: 0x00219647 File Offset: 0x00217847
		private void ChangeFlag(LiveShapingItem.PrivateFlags flag, bool value)
		{
			if (value)
			{
				this._flags |= flag;
				return;
			}
			this._flags &= ~flag;
		}

		// Token: 0x04003830 RID: 14384
		private static readonly DependencyProperty StartingIndexProperty = DependencyProperty.Register("StartingIndex", typeof(int), typeof(LiveShapingItem));

		// Token: 0x04003831 RID: 14385
		private static readonly DependencyProperty ParentGroupsProperty = DependencyProperty.Register("ParentGroups", typeof(object), typeof(LiveShapingItem));

		// Token: 0x04003832 RID: 14386
		private LiveShapingBlock _block;

		// Token: 0x04003833 RID: 14387
		private object _item;

		// Token: 0x04003834 RID: 14388
		private LiveShapingItem.PrivateFlags _flags;

		// Token: 0x02000B4F RID: 2895
		[Flags]
		private enum PrivateFlags
		{
			// Token: 0x04004AEA RID: 19178
			IsSortDirty = 1,
			// Token: 0x04004AEB RID: 19179
			IsSortPendingClean = 2,
			// Token: 0x04004AEC RID: 19180
			IsFilterDirty = 4,
			// Token: 0x04004AED RID: 19181
			IsGroupDirty = 8,
			// Token: 0x04004AEE RID: 19182
			FailsFilter = 16,
			// Token: 0x04004AEF RID: 19183
			ForwardChanges = 32,
			// Token: 0x04004AF0 RID: 19184
			IsDeleted = 64
		}
	}
}
