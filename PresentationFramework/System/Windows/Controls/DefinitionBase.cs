using System;
using System.Collections;
using System.Collections.Generic;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Defines the functionality required to support a shared-size group that is used by the <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" /> and <see cref="T:System.Windows.Controls.RowDefinitionCollection" /> classes. This is an abstract class. </summary>
	// Token: 0x020004C9 RID: 1225
	[Localizability(LocalizationCategory.Ignore)]
	public abstract class DefinitionBase : FrameworkContentElement
	{
		// Token: 0x06004A51 RID: 19025 RVA: 0x0014F956 File Offset: 0x0014DB56
		internal DefinitionBase(bool isColumnDefinition)
		{
			this._isColumnDefinition = isColumnDefinition;
			this._parentIndex = -1;
		}

		/// <summary>Gets or sets a value that identifies a <see cref="T:System.Windows.Controls.ColumnDefinition" /> or <see cref="T:System.Windows.Controls.RowDefinition" /> as a member of a defined group that shares sizing properties.   </summary>
		/// <returns>A <see cref="T:System.String" /> that identifies a shared-size group.</returns>
		// Token: 0x1700121C RID: 4636
		// (get) Token: 0x06004A52 RID: 19026 RVA: 0x0014F96C File Offset: 0x0014DB6C
		// (set) Token: 0x06004A53 RID: 19027 RVA: 0x0014F97E File Offset: 0x0014DB7E
		public string SharedSizeGroup
		{
			get
			{
				return (string)base.GetValue(DefinitionBase.SharedSizeGroupProperty);
			}
			set
			{
				base.SetValue(DefinitionBase.SharedSizeGroupProperty, value);
			}
		}

		// Token: 0x06004A54 RID: 19028 RVA: 0x0014F98C File Offset: 0x0014DB8C
		internal void OnEnterParentTree()
		{
			if (this._sharedState == null)
			{
				string sharedSizeGroup = this.SharedSizeGroup;
				if (sharedSizeGroup != null)
				{
					DefinitionBase.SharedSizeScope privateSharedSizeScope = this.PrivateSharedSizeScope;
					if (privateSharedSizeScope != null)
					{
						this._sharedState = privateSharedSizeScope.EnsureSharedState(sharedSizeGroup);
						this._sharedState.AddMember(this);
					}
				}
			}
		}

		// Token: 0x06004A55 RID: 19029 RVA: 0x0014F9CE File Offset: 0x0014DBCE
		internal void OnExitParentTree()
		{
			this._offset = 0.0;
			if (this._sharedState != null)
			{
				this._sharedState.RemoveMember(this);
				this._sharedState = null;
			}
		}

		// Token: 0x06004A56 RID: 19030 RVA: 0x0014F9FA File Offset: 0x0014DBFA
		internal void OnBeforeLayout(Grid grid)
		{
			this._minSize = 0.0;
			this.LayoutWasUpdated = true;
			if (this._sharedState != null)
			{
				this._sharedState.EnsureDeferredValidation(grid);
			}
		}

		// Token: 0x06004A57 RID: 19031 RVA: 0x0014FA26 File Offset: 0x0014DC26
		internal void UpdateMinSize(double minSize)
		{
			this._minSize = Math.Max(this._minSize, minSize);
		}

		// Token: 0x06004A58 RID: 19032 RVA: 0x0014FA3A File Offset: 0x0014DC3A
		internal void SetMinSize(double minSize)
		{
			this._minSize = minSize;
		}

		// Token: 0x06004A59 RID: 19033 RVA: 0x0014FA44 File Offset: 0x0014DC44
		internal static void OnUserSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DefinitionBase definitionBase = (DefinitionBase)d;
			if (definitionBase.InParentLogicalTree)
			{
				if (definitionBase._sharedState != null)
				{
					definitionBase._sharedState.Invalidate();
					return;
				}
				Grid grid = (Grid)definitionBase.Parent;
				if (((GridLength)e.OldValue).GridUnitType != ((GridLength)e.NewValue).GridUnitType)
				{
					grid.Invalidate();
					return;
				}
				grid.InvalidateMeasure();
			}
		}

		// Token: 0x06004A5A RID: 19034 RVA: 0x0014FAB8 File Offset: 0x0014DCB8
		internal static bool IsUserSizePropertyValueValid(object value)
		{
			return ((GridLength)value).Value >= 0.0;
		}

		// Token: 0x06004A5B RID: 19035 RVA: 0x0014FAE4 File Offset: 0x0014DCE4
		internal static void OnUserMinSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DefinitionBase definitionBase = (DefinitionBase)d;
			if (definitionBase.InParentLogicalTree)
			{
				Grid grid = (Grid)definitionBase.Parent;
				grid.InvalidateMeasure();
			}
		}

		// Token: 0x06004A5C RID: 19036 RVA: 0x0014FB14 File Offset: 0x0014DD14
		internal static bool IsUserMinSizePropertyValueValid(object value)
		{
			double num = (double)value;
			return !DoubleUtil.IsNaN(num) && num >= 0.0 && !double.IsPositiveInfinity(num);
		}

		// Token: 0x06004A5D RID: 19037 RVA: 0x0014FB48 File Offset: 0x0014DD48
		internal static void OnUserMaxSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DefinitionBase definitionBase = (DefinitionBase)d;
			if (definitionBase.InParentLogicalTree)
			{
				Grid grid = (Grid)definitionBase.Parent;
				grid.InvalidateMeasure();
			}
		}

		// Token: 0x06004A5E RID: 19038 RVA: 0x0014FB78 File Offset: 0x0014DD78
		internal static bool IsUserMaxSizePropertyValueValid(object value)
		{
			double num = (double)value;
			return !DoubleUtil.IsNaN(num) && num >= 0.0;
		}

		// Token: 0x06004A5F RID: 19039 RVA: 0x0014FBA8 File Offset: 0x0014DDA8
		internal static void OnIsSharedSizeScopePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				DefinitionBase.SharedSizeScope value = new DefinitionBase.SharedSizeScope();
				d.SetValue(DefinitionBase.PrivateSharedSizeScopeProperty, value);
				return;
			}
			d.ClearValue(DefinitionBase.PrivateSharedSizeScopeProperty);
		}

		// Token: 0x1700121D RID: 4637
		// (get) Token: 0x06004A60 RID: 19040 RVA: 0x0014FBE1 File Offset: 0x0014DDE1
		internal bool IsShared
		{
			get
			{
				return this._sharedState != null;
			}
		}

		// Token: 0x1700121E RID: 4638
		// (get) Token: 0x06004A61 RID: 19041 RVA: 0x0014FBEC File Offset: 0x0014DDEC
		internal GridLength UserSize
		{
			get
			{
				if (this._sharedState == null)
				{
					return this.UserSizeValueCache;
				}
				return this._sharedState.UserSize;
			}
		}

		// Token: 0x1700121F RID: 4639
		// (get) Token: 0x06004A62 RID: 19042 RVA: 0x0014FC08 File Offset: 0x0014DE08
		internal double UserMinSize
		{
			get
			{
				return this.UserMinSizeValueCache;
			}
		}

		// Token: 0x17001220 RID: 4640
		// (get) Token: 0x06004A63 RID: 19043 RVA: 0x0014FC10 File Offset: 0x0014DE10
		internal double UserMaxSize
		{
			get
			{
				return this.UserMaxSizeValueCache;
			}
		}

		// Token: 0x17001221 RID: 4641
		// (get) Token: 0x06004A64 RID: 19044 RVA: 0x0014FC18 File Offset: 0x0014DE18
		// (set) Token: 0x06004A65 RID: 19045 RVA: 0x0014FC20 File Offset: 0x0014DE20
		internal int Index
		{
			get
			{
				return this._parentIndex;
			}
			set
			{
				this._parentIndex = value;
			}
		}

		// Token: 0x17001222 RID: 4642
		// (get) Token: 0x06004A66 RID: 19046 RVA: 0x0014FC29 File Offset: 0x0014DE29
		// (set) Token: 0x06004A67 RID: 19047 RVA: 0x0014FC31 File Offset: 0x0014DE31
		internal Grid.LayoutTimeSizeType SizeType
		{
			get
			{
				return this._sizeType;
			}
			set
			{
				this._sizeType = value;
			}
		}

		// Token: 0x17001223 RID: 4643
		// (get) Token: 0x06004A68 RID: 19048 RVA: 0x0014FC3A File Offset: 0x0014DE3A
		// (set) Token: 0x06004A69 RID: 19049 RVA: 0x0014FC42 File Offset: 0x0014DE42
		internal double MeasureSize
		{
			get
			{
				return this._measureSize;
			}
			set
			{
				this._measureSize = value;
			}
		}

		// Token: 0x17001224 RID: 4644
		// (get) Token: 0x06004A6A RID: 19050 RVA: 0x0014FC4C File Offset: 0x0014DE4C
		internal double PreferredSize
		{
			get
			{
				double num = this.MinSize;
				if (this._sizeType != Grid.LayoutTimeSizeType.Auto && num < this._measureSize)
				{
					num = this._measureSize;
				}
				return num;
			}
		}

		// Token: 0x17001225 RID: 4645
		// (get) Token: 0x06004A6B RID: 19051 RVA: 0x0014FC7A File Offset: 0x0014DE7A
		// (set) Token: 0x06004A6C RID: 19052 RVA: 0x0014FC82 File Offset: 0x0014DE82
		internal double SizeCache
		{
			get
			{
				return this._sizeCache;
			}
			set
			{
				this._sizeCache = value;
			}
		}

		// Token: 0x17001226 RID: 4646
		// (get) Token: 0x06004A6D RID: 19053 RVA: 0x0014FC8C File Offset: 0x0014DE8C
		internal double MinSize
		{
			get
			{
				double minSize = this._minSize;
				if (this.UseSharedMinimum && this._sharedState != null && minSize < this._sharedState.MinSize)
				{
					minSize = this._sharedState.MinSize;
				}
				return minSize;
			}
		}

		// Token: 0x17001227 RID: 4647
		// (get) Token: 0x06004A6E RID: 19054 RVA: 0x0014FCCC File Offset: 0x0014DECC
		internal double MinSizeForArrange
		{
			get
			{
				double minSize = this._minSize;
				if (this._sharedState != null && (this.UseSharedMinimum || !this.LayoutWasUpdated) && minSize < this._sharedState.MinSize)
				{
					minSize = this._sharedState.MinSize;
				}
				return minSize;
			}
		}

		// Token: 0x17001228 RID: 4648
		// (get) Token: 0x06004A6F RID: 19055 RVA: 0x0014FD13 File Offset: 0x0014DF13
		internal double RawMinSize
		{
			get
			{
				return this._minSize;
			}
		}

		// Token: 0x17001229 RID: 4649
		// (get) Token: 0x06004A70 RID: 19056 RVA: 0x0014FD1B File Offset: 0x0014DF1B
		// (set) Token: 0x06004A71 RID: 19057 RVA: 0x0014FD23 File Offset: 0x0014DF23
		internal double FinalOffset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
			}
		}

		// Token: 0x1700122A RID: 4650
		// (get) Token: 0x06004A72 RID: 19058 RVA: 0x0014FD2C File Offset: 0x0014DF2C
		internal GridLength UserSizeValueCache
		{
			get
			{
				return (GridLength)base.GetValue(this._isColumnDefinition ? ColumnDefinition.WidthProperty : RowDefinition.HeightProperty);
			}
		}

		// Token: 0x1700122B RID: 4651
		// (get) Token: 0x06004A73 RID: 19059 RVA: 0x0014FD4D File Offset: 0x0014DF4D
		internal double UserMinSizeValueCache
		{
			get
			{
				return (double)base.GetValue(this._isColumnDefinition ? ColumnDefinition.MinWidthProperty : RowDefinition.MinHeightProperty);
			}
		}

		// Token: 0x1700122C RID: 4652
		// (get) Token: 0x06004A74 RID: 19060 RVA: 0x0014FD6E File Offset: 0x0014DF6E
		internal double UserMaxSizeValueCache
		{
			get
			{
				return (double)base.GetValue(this._isColumnDefinition ? ColumnDefinition.MaxWidthProperty : RowDefinition.MaxHeightProperty);
			}
		}

		// Token: 0x1700122D RID: 4653
		// (get) Token: 0x06004A75 RID: 19061 RVA: 0x0014FD8F File Offset: 0x0014DF8F
		internal bool InParentLogicalTree
		{
			get
			{
				return this._parentIndex != -1;
			}
		}

		// Token: 0x06004A76 RID: 19062 RVA: 0x0014FD9D File Offset: 0x0014DF9D
		private void SetFlags(bool value, DefinitionBase.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x06004A77 RID: 19063 RVA: 0x0014FDBC File Offset: 0x0014DFBC
		private bool CheckFlagsAnd(DefinitionBase.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x06004A78 RID: 19064 RVA: 0x0014FDCC File Offset: 0x0014DFCC
		private static void OnSharedSizeGroupPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DefinitionBase definitionBase = (DefinitionBase)d;
			if (definitionBase.InParentLogicalTree)
			{
				string text = (string)e.NewValue;
				if (definitionBase._sharedState != null)
				{
					definitionBase._sharedState.RemoveMember(definitionBase);
					definitionBase._sharedState = null;
				}
				if (definitionBase._sharedState == null && text != null)
				{
					DefinitionBase.SharedSizeScope privateSharedSizeScope = definitionBase.PrivateSharedSizeScope;
					if (privateSharedSizeScope != null)
					{
						definitionBase._sharedState = privateSharedSizeScope.EnsureSharedState(text);
						definitionBase._sharedState.AddMember(definitionBase);
					}
				}
			}
		}

		// Token: 0x06004A79 RID: 19065 RVA: 0x0014FE40 File Offset: 0x0014E040
		private static bool SharedSizeGroupPropertyValueValid(object value)
		{
			if (value == null)
			{
				return true;
			}
			string text = (string)value;
			if (text != string.Empty)
			{
				int num = -1;
				while (++num < text.Length)
				{
					bool flag = char.IsDigit(text[num]);
					if ((num == 0 && flag) || (!flag && !char.IsLetter(text[num]) && '_' != text[num]))
					{
						break;
					}
				}
				if (num == text.Length)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004A7A RID: 19066 RVA: 0x0014FEB4 File Offset: 0x0014E0B4
		private static void OnPrivateSharedSizeScopePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DefinitionBase definitionBase = (DefinitionBase)d;
			if (definitionBase.InParentLogicalTree)
			{
				DefinitionBase.SharedSizeScope sharedSizeScope = (DefinitionBase.SharedSizeScope)e.NewValue;
				if (definitionBase._sharedState != null)
				{
					definitionBase._sharedState.RemoveMember(definitionBase);
					definitionBase._sharedState = null;
				}
				if (definitionBase._sharedState == null && sharedSizeScope != null)
				{
					string sharedSizeGroup = definitionBase.SharedSizeGroup;
					if (sharedSizeGroup != null)
					{
						definitionBase._sharedState = sharedSizeScope.EnsureSharedState(definitionBase.SharedSizeGroup);
						definitionBase._sharedState.AddMember(definitionBase);
					}
				}
			}
		}

		// Token: 0x1700122E RID: 4654
		// (get) Token: 0x06004A7B RID: 19067 RVA: 0x0014FF2B File Offset: 0x0014E12B
		private DefinitionBase.SharedSizeScope PrivateSharedSizeScope
		{
			get
			{
				return (DefinitionBase.SharedSizeScope)base.GetValue(DefinitionBase.PrivateSharedSizeScopeProperty);
			}
		}

		// Token: 0x1700122F RID: 4655
		// (get) Token: 0x06004A7C RID: 19068 RVA: 0x0014FF3D File Offset: 0x0014E13D
		// (set) Token: 0x06004A7D RID: 19069 RVA: 0x0014FF47 File Offset: 0x0014E147
		private bool UseSharedMinimum
		{
			get
			{
				return this.CheckFlagsAnd(DefinitionBase.Flags.UseSharedMinimum);
			}
			set
			{
				this.SetFlags(value, DefinitionBase.Flags.UseSharedMinimum);
			}
		}

		// Token: 0x17001230 RID: 4656
		// (get) Token: 0x06004A7E RID: 19070 RVA: 0x0014FF52 File Offset: 0x0014E152
		// (set) Token: 0x06004A7F RID: 19071 RVA: 0x0014FF5C File Offset: 0x0014E15C
		private bool LayoutWasUpdated
		{
			get
			{
				return this.CheckFlagsAnd(DefinitionBase.Flags.LayoutWasUpdated);
			}
			set
			{
				this.SetFlags(value, DefinitionBase.Flags.LayoutWasUpdated);
			}
		}

		// Token: 0x06004A80 RID: 19072 RVA: 0x0014FF68 File Offset: 0x0014E168
		static DefinitionBase()
		{
			DefinitionBase.PrivateSharedSizeScopeProperty.OverrideMetadata(typeof(DefinitionBase), new FrameworkPropertyMetadata(new PropertyChangedCallback(DefinitionBase.OnPrivateSharedSizeScopePropertyChanged)));
		}

		// Token: 0x04002A53 RID: 10835
		private readonly bool _isColumnDefinition;

		// Token: 0x04002A54 RID: 10836
		private DefinitionBase.Flags _flags;

		// Token: 0x04002A55 RID: 10837
		private int _parentIndex;

		// Token: 0x04002A56 RID: 10838
		private Grid.LayoutTimeSizeType _sizeType;

		// Token: 0x04002A57 RID: 10839
		private double _minSize;

		// Token: 0x04002A58 RID: 10840
		private double _measureSize;

		// Token: 0x04002A59 RID: 10841
		private double _sizeCache;

		// Token: 0x04002A5A RID: 10842
		private double _offset;

		// Token: 0x04002A5B RID: 10843
		private DefinitionBase.SharedSizeState _sharedState;

		// Token: 0x04002A5C RID: 10844
		internal const bool ThisIsColumnDefinition = true;

		// Token: 0x04002A5D RID: 10845
		internal const bool ThisIsRowDefinition = false;

		// Token: 0x04002A5E RID: 10846
		internal static readonly DependencyProperty PrivateSharedSizeScopeProperty = DependencyProperty.RegisterAttached("PrivateSharedSizeScope", typeof(DefinitionBase.SharedSizeScope), typeof(DefinitionBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DefinitionBase.SharedSizeGroup" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DefinitionBase.SharedSizeGroup" /> dependency property.</returns>
		// Token: 0x04002A5F RID: 10847
		public static readonly DependencyProperty SharedSizeGroupProperty = DependencyProperty.Register("SharedSizeGroup", typeof(string), typeof(DefinitionBase), new FrameworkPropertyMetadata(new PropertyChangedCallback(DefinitionBase.OnSharedSizeGroupPropertyChanged)), new ValidateValueCallback(DefinitionBase.SharedSizeGroupPropertyValueValid));

		// Token: 0x0200096C RID: 2412
		[Flags]
		private enum Flags : byte
		{
			// Token: 0x04004438 RID: 17464
			UseSharedMinimum = 32,
			// Token: 0x04004439 RID: 17465
			LayoutWasUpdated = 64
		}

		// Token: 0x0200096D RID: 2413
		private class SharedSizeScope
		{
			// Token: 0x06008764 RID: 34660 RVA: 0x0024F988 File Offset: 0x0024DB88
			internal DefinitionBase.SharedSizeState EnsureSharedState(string sharedSizeGroup)
			{
				DefinitionBase.SharedSizeState sharedSizeState = this._registry[sharedSizeGroup] as DefinitionBase.SharedSizeState;
				if (sharedSizeState == null)
				{
					sharedSizeState = new DefinitionBase.SharedSizeState(this, sharedSizeGroup);
					this._registry[sharedSizeGroup] = sharedSizeState;
				}
				return sharedSizeState;
			}

			// Token: 0x06008765 RID: 34661 RVA: 0x0024F9C0 File Offset: 0x0024DBC0
			internal void Remove(object key)
			{
				this._registry.Remove(key);
			}

			// Token: 0x0400443A RID: 17466
			private Hashtable _registry = new Hashtable();
		}

		// Token: 0x0200096E RID: 2414
		private class SharedSizeState
		{
			// Token: 0x06008767 RID: 34663 RVA: 0x0024F9E1 File Offset: 0x0024DBE1
			internal SharedSizeState(DefinitionBase.SharedSizeScope sharedSizeScope, string sharedSizeGroupId)
			{
				this._sharedSizeScope = sharedSizeScope;
				this._sharedSizeGroupId = sharedSizeGroupId;
				this._registry = new List<DefinitionBase>();
				this._layoutUpdated = new EventHandler(this.OnLayoutUpdated);
				this._broadcastInvalidation = true;
			}

			// Token: 0x06008768 RID: 34664 RVA: 0x0024FA1B File Offset: 0x0024DC1B
			internal void AddMember(DefinitionBase member)
			{
				this._registry.Add(member);
				this.Invalidate();
			}

			// Token: 0x06008769 RID: 34665 RVA: 0x0024FA2F File Offset: 0x0024DC2F
			internal void RemoveMember(DefinitionBase member)
			{
				this.Invalidate();
				this._registry.Remove(member);
				if (this._registry.Count == 0)
				{
					this._sharedSizeScope.Remove(this._sharedSizeGroupId);
				}
			}

			// Token: 0x0600876A RID: 34666 RVA: 0x0024FA64 File Offset: 0x0024DC64
			internal void Invalidate()
			{
				this._userSizeValid = false;
				if (this._broadcastInvalidation)
				{
					int i = 0;
					int count = this._registry.Count;
					while (i < count)
					{
						Grid grid = (Grid)this._registry[i].Parent;
						grid.Invalidate();
						i++;
					}
					this._broadcastInvalidation = false;
				}
			}

			// Token: 0x0600876B RID: 34667 RVA: 0x0024FABC File Offset: 0x0024DCBC
			internal void EnsureDeferredValidation(UIElement layoutUpdatedHost)
			{
				if (this._layoutUpdatedHost == null)
				{
					this._layoutUpdatedHost = layoutUpdatedHost;
					this._layoutUpdatedHost.LayoutUpdated += this._layoutUpdated;
				}
			}

			// Token: 0x17001E98 RID: 7832
			// (get) Token: 0x0600876C RID: 34668 RVA: 0x0024FADE File Offset: 0x0024DCDE
			internal double MinSize
			{
				get
				{
					if (!this._userSizeValid)
					{
						this.EnsureUserSizeValid();
					}
					return this._minSize;
				}
			}

			// Token: 0x17001E99 RID: 7833
			// (get) Token: 0x0600876D RID: 34669 RVA: 0x0024FAF4 File Offset: 0x0024DCF4
			internal GridLength UserSize
			{
				get
				{
					if (!this._userSizeValid)
					{
						this.EnsureUserSizeValid();
					}
					return this._userSize;
				}
			}

			// Token: 0x0600876E RID: 34670 RVA: 0x0024FB0C File Offset: 0x0024DD0C
			private void EnsureUserSizeValid()
			{
				this._userSize = new GridLength(1.0, GridUnitType.Auto);
				int i = 0;
				int count = this._registry.Count;
				while (i < count)
				{
					GridLength userSizeValueCache = this._registry[i].UserSizeValueCache;
					if (userSizeValueCache.GridUnitType == GridUnitType.Pixel)
					{
						if (this._userSize.GridUnitType == GridUnitType.Auto)
						{
							this._userSize = userSizeValueCache;
						}
						else if (this._userSize.Value < userSizeValueCache.Value)
						{
							this._userSize = userSizeValueCache;
						}
					}
					i++;
				}
				this._minSize = (this._userSize.IsAbsolute ? this._userSize.Value : 0.0);
				this._userSizeValid = true;
			}

			// Token: 0x0600876F RID: 34671 RVA: 0x0024FBC3 File Offset: 0x0024DDC3
			private void OnLayoutUpdated(object sender, EventArgs e)
			{
				if (!FrameworkAppContextSwitches.SharedSizeGroupDoesRedundantLayout)
				{
					this.ValidateSharedSizeGroup();
					return;
				}
				this.ValidateSharedSizeGroupLegacy();
			}

			// Token: 0x06008770 RID: 34672 RVA: 0x0024FBDC File Offset: 0x0024DDDC
			private void ValidateSharedSizeGroup()
			{
				double num = 0.0;
				int i = 0;
				int count = this._registry.Count;
				while (i < count)
				{
					num = Math.Max(num, this._registry[i]._minSize);
					i++;
				}
				bool flag = !DoubleUtil.AreClose(this._minSize, num);
				int j = 0;
				int count2 = this._registry.Count;
				while (j < count2)
				{
					DefinitionBase definitionBase = this._registry[j];
					bool flag2 = !DoubleUtil.AreClose(definitionBase._minSize, num);
					bool flag3;
					if (!definitionBase.UseSharedMinimum)
					{
						flag3 = !flag2;
					}
					else if (flag2)
					{
						flag3 = !flag;
					}
					else
					{
						flag3 = (definitionBase.LayoutWasUpdated && DoubleUtil.GreaterThanOrClose(definitionBase._minSize, this.MinSize));
					}
					if (!flag3)
					{
						Grid grid = (Grid)definitionBase.Parent;
						grid.InvalidateMeasure();
					}
					else if (!DoubleUtil.AreClose(num, definitionBase.SizeCache))
					{
						Grid grid2 = (Grid)definitionBase.Parent;
						grid2.InvalidateArrange();
					}
					definitionBase.UseSharedMinimum = flag2;
					definitionBase.LayoutWasUpdated = false;
					j++;
				}
				this._minSize = num;
				this._layoutUpdatedHost.LayoutUpdated -= this._layoutUpdated;
				this._layoutUpdatedHost = null;
				this._broadcastInvalidation = true;
			}

			// Token: 0x06008771 RID: 34673 RVA: 0x0024FD2C File Offset: 0x0024DF2C
			private void ValidateSharedSizeGroupLegacy()
			{
				double num = 0.0;
				int i = 0;
				int count = this._registry.Count;
				while (i < count)
				{
					num = Math.Max(num, this._registry[i].MinSize);
					i++;
				}
				bool flag = !DoubleUtil.AreClose(this._minSize, num);
				int j = 0;
				int count2 = this._registry.Count;
				while (j < count2)
				{
					DefinitionBase definitionBase = this._registry[j];
					if (flag || definitionBase.LayoutWasUpdated)
					{
						if (!DoubleUtil.AreClose(num, definitionBase.MinSize))
						{
							Grid grid = (Grid)definitionBase.Parent;
							grid.InvalidateMeasure();
							definitionBase.UseSharedMinimum = true;
						}
						else
						{
							definitionBase.UseSharedMinimum = false;
							if (!DoubleUtil.AreClose(num, definitionBase.SizeCache))
							{
								Grid grid2 = (Grid)definitionBase.Parent;
								grid2.InvalidateArrange();
							}
						}
						definitionBase.LayoutWasUpdated = false;
					}
					j++;
				}
				this._minSize = num;
				this._layoutUpdatedHost.LayoutUpdated -= this._layoutUpdated;
				this._layoutUpdatedHost = null;
				this._broadcastInvalidation = true;
			}

			// Token: 0x0400443B RID: 17467
			private readonly DefinitionBase.SharedSizeScope _sharedSizeScope;

			// Token: 0x0400443C RID: 17468
			private readonly string _sharedSizeGroupId;

			// Token: 0x0400443D RID: 17469
			private readonly List<DefinitionBase> _registry;

			// Token: 0x0400443E RID: 17470
			private readonly EventHandler _layoutUpdated;

			// Token: 0x0400443F RID: 17471
			private UIElement _layoutUpdatedHost;

			// Token: 0x04004440 RID: 17472
			private bool _broadcastInvalidation;

			// Token: 0x04004441 RID: 17473
			private bool _userSizeValid;

			// Token: 0x04004442 RID: 17474
			private GridLength _userSize;

			// Token: 0x04004443 RID: 17475
			private double _minSize;
		}
	}
}
