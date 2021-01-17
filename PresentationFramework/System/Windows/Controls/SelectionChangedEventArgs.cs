using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.Primitives.Selector.SelectionChanged" /> event. </summary>
	// Token: 0x0200052F RID: 1327
	public class SelectionChangedEventArgs : RoutedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.SelectionChangedEventArgs" /> class. </summary>
		/// <param name="id">The event identifier (ID).</param>
		/// <param name="removedItems">The items that were unselected during this event.</param>
		/// <param name="addedItems">The items that were selected during this event.</param>
		// Token: 0x060055EC RID: 21996 RVA: 0x0017CE38 File Offset: 0x0017B038
		public SelectionChangedEventArgs(RoutedEvent id, IList removedItems, IList addedItems)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			if (removedItems == null)
			{
				throw new ArgumentNullException("removedItems");
			}
			if (addedItems == null)
			{
				throw new ArgumentNullException("addedItems");
			}
			base.RoutedEvent = id;
			this._removedItems = new object[removedItems.Count];
			removedItems.CopyTo(this._removedItems, 0);
			this._addedItems = new object[addedItems.Count];
			addedItems.CopyTo(this._addedItems, 0);
		}

		// Token: 0x060055ED RID: 21997 RVA: 0x0017CEB8 File Offset: 0x0017B0B8
		internal SelectionChangedEventArgs(List<ItemsControl.ItemInfo> unselectedInfos, List<ItemsControl.ItemInfo> selectedInfos)
		{
			base.RoutedEvent = Selector.SelectionChangedEvent;
			this._removedItems = new object[unselectedInfos.Count];
			for (int i = 0; i < unselectedInfos.Count; i++)
			{
				this._removedItems[i] = unselectedInfos[i].Item;
			}
			this._addedItems = new object[selectedInfos.Count];
			for (int j = 0; j < selectedInfos.Count; j++)
			{
				this._addedItems[j] = selectedInfos[j].Item;
			}
			this._removedInfos = unselectedInfos;
			this._addedInfos = selectedInfos;
		}

		/// <summary>Gets a list that contains the items that were unselected. </summary>
		/// <returns>The items that were unselected since the last time the <see cref="E:System.Windows.Controls.Primitives.Selector.SelectionChanged" /> event occurred.</returns>
		// Token: 0x170014E0 RID: 5344
		// (get) Token: 0x060055EE RID: 21998 RVA: 0x0017CF50 File Offset: 0x0017B150
		public IList RemovedItems
		{
			get
			{
				return this._removedItems;
			}
		}

		/// <summary>Gets a list that contains the items that were selected. </summary>
		/// <returns>The items that were selected since the last time the <see cref="E:System.Windows.Controls.Primitives.Selector.SelectionChanged" /> event occurred.</returns>
		// Token: 0x170014E1 RID: 5345
		// (get) Token: 0x060055EF RID: 21999 RVA: 0x0017CF58 File Offset: 0x0017B158
		public IList AddedItems
		{
			get
			{
				return this._addedItems;
			}
		}

		// Token: 0x170014E2 RID: 5346
		// (get) Token: 0x060055F0 RID: 22000 RVA: 0x0017CF60 File Offset: 0x0017B160
		internal List<ItemsControl.ItemInfo> RemovedInfos
		{
			get
			{
				return this._removedInfos;
			}
		}

		// Token: 0x170014E3 RID: 5347
		// (get) Token: 0x060055F1 RID: 22001 RVA: 0x0017CF68 File Offset: 0x0017B168
		internal List<ItemsControl.ItemInfo> AddedInfos
		{
			get
			{
				return this._addedInfos;
			}
		}

		/// <summary>Performs the proper type casting to call the type-safe <see cref="T:System.Windows.Controls.SelectionChangedEventHandler" /> delegate for the <see cref="E:System.Windows.Controls.Primitives.Selector.SelectionChanged" /> event. </summary>
		/// <param name="genericHandler">The handler to invoke.</param>
		/// <param name="genericTarget">The current object along the event's route.</param>
		// Token: 0x060055F2 RID: 22002 RVA: 0x0017CF70 File Offset: 0x0017B170
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			SelectionChangedEventHandler selectionChangedEventHandler = (SelectionChangedEventHandler)genericHandler;
			selectionChangedEventHandler(genericTarget, this);
		}

		// Token: 0x04002E17 RID: 11799
		private object[] _addedItems;

		// Token: 0x04002E18 RID: 11800
		private object[] _removedItems;

		// Token: 0x04002E19 RID: 11801
		private List<ItemsControl.ItemInfo> _addedInfos;

		// Token: 0x04002E1A RID: 11802
		private List<ItemsControl.ItemInfo> _removedInfos;
	}
}
