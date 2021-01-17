using System;
using System.Collections.Specialized;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.ItemContainerGenerator.ItemsChanged" /> event.</summary>
	// Token: 0x02000594 RID: 1428
	public class ItemsChangedEventArgs : EventArgs
	{
		// Token: 0x06005E4F RID: 24143 RVA: 0x001A7404 File Offset: 0x001A5604
		internal ItemsChangedEventArgs(NotifyCollectionChangedAction action, GeneratorPosition position, GeneratorPosition oldPosition, int itemCount, int itemUICount)
		{
			this._action = action;
			this._position = position;
			this._oldPosition = oldPosition;
			this._itemCount = itemCount;
			this._itemUICount = itemUICount;
		}

		// Token: 0x06005E50 RID: 24144 RVA: 0x001A7431 File Offset: 0x001A5631
		internal ItemsChangedEventArgs(NotifyCollectionChangedAction action, GeneratorPosition position, int itemCount, int itemUICount) : this(action, position, new GeneratorPosition(-1, 0), itemCount, itemUICount)
		{
		}

		/// <summary>Gets the action that occurred on the items collection.</summary>
		/// <returns>Returns the action that occurred.</returns>
		// Token: 0x170016C4 RID: 5828
		// (get) Token: 0x06005E51 RID: 24145 RVA: 0x001A7445 File Offset: 0x001A5645
		public NotifyCollectionChangedAction Action
		{
			get
			{
				return this._action;
			}
		}

		/// <summary>Gets the position in the collection where the change occurred.</summary>
		/// <returns>Returns a <see cref="T:System.Windows.Controls.Primitives.GeneratorPosition" />.</returns>
		// Token: 0x170016C5 RID: 5829
		// (get) Token: 0x06005E52 RID: 24146 RVA: 0x001A744D File Offset: 0x001A564D
		public GeneratorPosition Position
		{
			get
			{
				return this._position;
			}
		}

		/// <summary>Gets the position in the collection before the change occurred.</summary>
		/// <returns>Returns a <see cref="T:System.Windows.Controls.Primitives.GeneratorPosition" />.</returns>
		// Token: 0x170016C6 RID: 5830
		// (get) Token: 0x06005E53 RID: 24147 RVA: 0x001A7455 File Offset: 0x001A5655
		public GeneratorPosition OldPosition
		{
			get
			{
				return this._oldPosition;
			}
		}

		/// <summary>Gets the number of items that were involved in the change.</summary>
		/// <returns>Integer that represents the number of items involved in the change.</returns>
		// Token: 0x170016C7 RID: 5831
		// (get) Token: 0x06005E54 RID: 24148 RVA: 0x001A745D File Offset: 0x001A565D
		public int ItemCount
		{
			get
			{
				return this._itemCount;
			}
		}

		/// <summary>Gets the number of user interface (UI) elements involved in the change.</summary>
		/// <returns>Integer that represents the number of UI elements involved in the change.</returns>
		// Token: 0x170016C8 RID: 5832
		// (get) Token: 0x06005E55 RID: 24149 RVA: 0x001A7465 File Offset: 0x001A5665
		public int ItemUICount
		{
			get
			{
				return this._itemUICount;
			}
		}

		// Token: 0x04003050 RID: 12368
		private NotifyCollectionChangedAction _action;

		// Token: 0x04003051 RID: 12369
		private GeneratorPosition _position;

		// Token: 0x04003052 RID: 12370
		private GeneratorPosition _oldPosition;

		// Token: 0x04003053 RID: 12371
		private int _itemCount;

		// Token: 0x04003054 RID: 12372
		private int _itemUICount;
	}
}
