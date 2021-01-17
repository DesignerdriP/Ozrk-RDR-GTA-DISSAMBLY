using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace System.Windows.Controls
{
	// Token: 0x020004D9 RID: 1241
	internal class GridViewColumnCollectionChangedEventArgs : NotifyCollectionChangedEventArgs
	{
		// Token: 0x06004D21 RID: 19745 RVA: 0x0015B82B File Offset: 0x00159A2B
		internal GridViewColumnCollectionChangedEventArgs(GridViewColumn column, string propertyName) : base(NotifyCollectionChangedAction.Reset)
		{
			this._column = column;
			this._propertyName = propertyName;
		}

		// Token: 0x06004D22 RID: 19746 RVA: 0x0015B849 File Offset: 0x00159A49
		internal GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction action, GridViewColumn[] clearedColumns) : base(action)
		{
			this._clearedColumns = Array.AsReadOnly<GridViewColumn>(clearedColumns);
		}

		// Token: 0x06004D23 RID: 19747 RVA: 0x0015B865 File Offset: 0x00159A65
		internal GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction action, GridViewColumn changedItem, int index, int actualIndex) : base(action, changedItem, index)
		{
			this._actualIndex = actualIndex;
		}

		// Token: 0x06004D24 RID: 19748 RVA: 0x0015B87F File Offset: 0x00159A7F
		internal GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction action, GridViewColumn newItem, GridViewColumn oldItem, int index, int actualIndex) : base(action, newItem, oldItem, index)
		{
			this._actualIndex = actualIndex;
		}

		// Token: 0x06004D25 RID: 19749 RVA: 0x0015B89B File Offset: 0x00159A9B
		internal GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction action, GridViewColumn changedItem, int index, int oldIndex, int actualIndex) : base(action, changedItem, index, oldIndex)
		{
			this._actualIndex = actualIndex;
		}

		// Token: 0x170012CF RID: 4815
		// (get) Token: 0x06004D26 RID: 19750 RVA: 0x0015B8B7 File Offset: 0x00159AB7
		internal int ActualIndex
		{
			get
			{
				return this._actualIndex;
			}
		}

		// Token: 0x170012D0 RID: 4816
		// (get) Token: 0x06004D27 RID: 19751 RVA: 0x0015B8BF File Offset: 0x00159ABF
		internal ReadOnlyCollection<GridViewColumn> ClearedColumns
		{
			get
			{
				return this._clearedColumns;
			}
		}

		// Token: 0x170012D1 RID: 4817
		// (get) Token: 0x06004D28 RID: 19752 RVA: 0x0015B8C7 File Offset: 0x00159AC7
		internal GridViewColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x170012D2 RID: 4818
		// (get) Token: 0x06004D29 RID: 19753 RVA: 0x0015B8CF File Offset: 0x00159ACF
		internal string PropertyName
		{
			get
			{
				return this._propertyName;
			}
		}

		// Token: 0x04002B4F RID: 11087
		private int _actualIndex = -1;

		// Token: 0x04002B50 RID: 11088
		private ReadOnlyCollection<GridViewColumn> _clearedColumns;

		// Token: 0x04002B51 RID: 11089
		private GridViewColumn _column;

		// Token: 0x04002B52 RID: 11090
		private string _propertyName;
	}
}
