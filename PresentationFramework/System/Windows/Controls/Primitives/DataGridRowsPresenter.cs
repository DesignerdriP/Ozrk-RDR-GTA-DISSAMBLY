using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Used within the template of a <see cref="T:System.Windows.Controls.DataGrid" /> to specify the location in the control's visual tree where the rows are to be added. </summary>
	// Token: 0x02000581 RID: 1409
	public class DataGridRowsPresenter : VirtualizingStackPanel
	{
		// Token: 0x06005D52 RID: 23890 RVA: 0x00144757 File Offset: 0x00142957
		internal void InternalBringIndexIntoView(int index)
		{
			this.BringIndexIntoView(index);
		}

		/// <summary>Indicates that the <see cref="P:System.Windows.Controls.Panel.IsItemsHost" /> property value has changed.</summary>
		/// <param name="oldIsItemsHost">The old property value.</param>
		/// <param name="newIsItemsHost">The new property value.</param>
		// Token: 0x06005D53 RID: 23891 RVA: 0x001A453C File Offset: 0x001A273C
		protected override void OnIsItemsHostChanged(bool oldIsItemsHost, bool newIsItemsHost)
		{
			base.OnIsItemsHostChanged(oldIsItemsHost, newIsItemsHost);
			if (newIsItemsHost)
			{
				DataGrid owner = this.Owner;
				if (owner != null)
				{
					IItemContainerGenerator itemContainerGenerator = owner.ItemContainerGenerator;
					if (itemContainerGenerator != null && itemContainerGenerator == itemContainerGenerator.GetItemContainerGeneratorForPanel(this))
					{
						owner.InternalItemsHost = this;
						return;
					}
				}
			}
			else
			{
				if (this._owner != null && this._owner.InternalItemsHost == this)
				{
					this._owner.InternalItemsHost = null;
				}
				this._owner = null;
			}
		}

		/// <summary>Called when the size of the viewport changes.</summary>
		/// <param name="oldViewportSize">The old size of the viewport.</param>
		/// <param name="newViewportSize">The new size of the viewport.</param>
		// Token: 0x06005D54 RID: 23892 RVA: 0x001A45A4 File Offset: 0x001A27A4
		protected override void OnViewportSizeChanged(Size oldViewportSize, Size newViewportSize)
		{
			DataGrid owner = this.Owner;
			if (owner != null)
			{
				ScrollContentPresenter internalScrollContentPresenter = owner.InternalScrollContentPresenter;
				if (internalScrollContentPresenter == null || internalScrollContentPresenter.CanContentScroll)
				{
					owner.OnViewportSizeChanged(oldViewportSize, newViewportSize);
				}
			}
		}

		/// <summary>Measures the child elements of a <see cref="T:System.Windows.Controls.VirtualizingStackPanel" /> in anticipation of arranging them during the <see cref="M:System.Windows.Controls.VirtualizingStackPanel.ArrangeOverride(System.Windows.Size)" /> pass.</summary>
		/// <param name="constraint">An upper limit <see cref="T:System.Windows.Size" /> that should not be exceeded.</param>
		/// <returns>The <see cref="T:System.Windows.Size" /> that represents the desired size of the element.</returns>
		// Token: 0x06005D55 RID: 23893 RVA: 0x001A45D5 File Offset: 0x001A27D5
		protected override Size MeasureOverride(Size constraint)
		{
			this._availableSize = constraint;
			return base.MeasureOverride(constraint);
		}

		// Token: 0x17001693 RID: 5779
		// (get) Token: 0x06005D56 RID: 23894 RVA: 0x001A45E5 File Offset: 0x001A27E5
		internal Size AvailableSize
		{
			get
			{
				return this._availableSize;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.VirtualizingStackPanel.CleanUpVirtualizedItem" /> event for rows that have scrolled out of view. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005D57 RID: 23895 RVA: 0x001A45ED File Offset: 0x001A27ED
		protected override void OnCleanUpVirtualizedItem(CleanUpVirtualizedItemEventArgs e)
		{
			base.OnCleanUpVirtualizedItem(e);
			if (e.UIElement != null && Validation.GetHasError(e.UIElement))
			{
				e.Cancel = true;
			}
		}

		// Token: 0x17001694 RID: 5780
		// (get) Token: 0x06005D58 RID: 23896 RVA: 0x001A4612 File Offset: 0x001A2812
		internal DataGrid Owner
		{
			get
			{
				if (this._owner == null)
				{
					this._owner = (ItemsControl.GetItemsOwner(this) as DataGrid);
				}
				return this._owner;
			}
		}

		// Token: 0x04003011 RID: 12305
		private DataGrid _owner;

		// Token: 0x04003012 RID: 12306
		private Size _availableSize;
	}
}
