using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.DataGridCell" /> types to UI Automation.</summary>
	// Token: 0x020002A1 RID: 673
	public sealed class DataGridCellItemAutomationPeer : AutomationPeer, IGridItemProvider, ITableItemProvider, IInvokeProvider, IScrollItemProvider, ISelectionItemProvider, IValueProvider, IVirtualizedItemProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.DataGridCellItemAutomationPeer" /> class. </summary>
		/// <param name="item">The element that is associated with this automation peer.</param>
		/// <param name="dataGridColumn">The <see cref="T:System.Windows.Controls.DataGrid" /> column that <paramref name="item" /> is in. </param>
		// Token: 0x06002581 RID: 9601 RVA: 0x000B46B3 File Offset: 0x000B28B3
		public DataGridCellItemAutomationPeer(object item, DataGridColumn dataGridColumn)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (dataGridColumn == null)
			{
				throw new ArgumentNullException("dataGridColumn");
			}
			this._item = new WeakReference(item);
			this._column = dataGridColumn;
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x000B46EC File Offset: 0x000B28EC
		protected override string GetAcceleratorKeyCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetAcceleratorKey();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x000B4718 File Offset: 0x000B2918
		protected override string GetAccessKeyCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetAccessKey();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x00094967 File Offset: 0x00092B67
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x000B4744 File Offset: 0x000B2944
		protected override string GetAutomationIdCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetAutomationId();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x000B4770 File Offset: 0x000B2970
		protected override Rect GetBoundingRectangleCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetBoundingRectangle();
			}
			this.ThrowElementNotAvailableException();
			return default(Rect);
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x000B47A0 File Offset: 0x000B29A0
		protected override List<AutomationPeer> GetChildrenCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				owningCellPeer.ForceEnsureChildren();
				return owningCellPeer.GetChildren();
			}
			return null;
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x000B47C8 File Offset: 0x000B29C8
		protected override string GetClassNameCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetClassName();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x000B47F4 File Offset: 0x000B29F4
		protected override Point GetClickablePointCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetClickablePoint();
			}
			this.ThrowElementNotAvailableException();
			return new Point(double.NaN, double.NaN);
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x000B4830 File Offset: 0x000B2A30
		protected override string GetHelpTextCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetHelpText();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x000B485C File Offset: 0x000B2A5C
		protected override string GetItemStatusCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetItemStatus();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x000B4888 File Offset: 0x000B2A88
		protected override string GetItemTypeCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetItemType();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x000B48B4 File Offset: 0x000B2AB4
		protected override AutomationPeer GetLabeledByCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetLabeledBy();
			}
			this.ThrowElementNotAvailableException();
			return null;
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x000B48D9 File Offset: 0x000B2AD9
		protected override string GetLocalizedControlTypeCore()
		{
			if (!AccessibilitySwitches.UseNetFx47CompatibleAccessibilityFeatures)
			{
				return SR.Get("DataGridCellItemAutomationPeer_LocalizedControlType");
			}
			return base.GetLocalizedControlTypeCore();
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x000B48F4 File Offset: 0x000B2AF4
		protected override AutomationLiveSetting GetLiveSettingCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			AutomationLiveSetting result = AutomationLiveSetting.Off;
			if (owningCellPeer != null)
			{
				result = owningCellPeer.GetLiveSetting();
			}
			else
			{
				this.ThrowElementNotAvailableException();
			}
			return result;
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x000B4920 File Offset: 0x000B2B20
		protected override string GetNameCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			string text = null;
			if (owningCellPeer != null)
			{
				text = owningCellPeer.GetName();
			}
			if (string.IsNullOrEmpty(text))
			{
				text = SR.Get("DataGridCellItemAutomationPeer_NameCoreFormat", new object[]
				{
					this.Item,
					this._column.DisplayIndex
				});
			}
			return text;
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x000B4978 File Offset: 0x000B2B78
		protected override AutomationOrientation GetOrientationCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetOrientation();
			}
			this.ThrowElementNotAvailableException();
			return AutomationOrientation.None;
		}

		/// <summary>Returns the object that supports the specified control pattern of the element that is associated with this automation peer.</summary>
		/// <param name="patternInterface">An enumeration that specifies the control pattern.</param>
		/// <returns>The current <see cref="T:System.Windows.Automation.Peers.DataGridCellItemAutomationPeer" /> object, if <paramref name="patternInterface" /> is a supported value; otherwise, <see langword="null" />. For more information, see Remarks.</returns>
		// Token: 0x06002592 RID: 9618 RVA: 0x000B49A0 File Offset: 0x000B2BA0
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface <= PatternInterface.ScrollItem)
			{
				if (patternInterface != PatternInterface.Invoke)
				{
					if (patternInterface != PatternInterface.Value)
					{
						if (patternInterface != PatternInterface.ScrollItem)
						{
							goto IL_8C;
						}
					}
					else
					{
						if (!this.IsNewItemPlaceholder)
						{
							return this;
						}
						goto IL_8C;
					}
				}
				else
				{
					if (!this.OwningDataGrid.IsReadOnly && !this._column.IsReadOnly)
					{
						return this;
					}
					goto IL_8C;
				}
			}
			else if (patternInterface <= PatternInterface.SelectionItem)
			{
				if (patternInterface != PatternInterface.GridItem)
				{
					if (patternInterface != PatternInterface.SelectionItem)
					{
						goto IL_8C;
					}
					if (this.IsCellSelectionUnit)
					{
						return this;
					}
					goto IL_8C;
				}
			}
			else if (patternInterface != PatternInterface.TableItem)
			{
				if (patternInterface != PatternInterface.VirtualizedItem)
				{
					goto IL_8C;
				}
				if (VirtualizedItemPatternIdentifiers.Pattern == null)
				{
					goto IL_8C;
				}
				if (this.OwningCellPeer == null)
				{
					return this;
				}
				if (this.OwningItemPeer != null && !this.IsItemInAutomationTree())
				{
					return this;
				}
				if (this.OwningItemPeer == null)
				{
					return this;
				}
				goto IL_8C;
			}
			return this;
			IL_8C:
			return null;
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x000B4A3C File Offset: 0x000B2C3C
		protected override int GetPositionInSetCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			int result = -1;
			if (owningCellPeer != null)
			{
				result = owningCellPeer.GetPositionInSet();
			}
			else
			{
				this.ThrowElementNotAvailableException();
			}
			return result;
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x000B4A68 File Offset: 0x000B2C68
		protected override int GetSizeOfSetCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			int result = -1;
			if (owningCellPeer != null)
			{
				result = owningCellPeer.GetSizeOfSet();
			}
			else
			{
				this.ThrowElementNotAvailableException();
			}
			return result;
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x000B4A94 File Offset: 0x000B2C94
		internal override Rect GetVisibleBoundingRectCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetVisibleBoundingRectCore();
			}
			return base.GetBoundingRectangle();
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x000B4AB8 File Offset: 0x000B2CB8
		protected override bool HasKeyboardFocusCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.HasKeyboardFocus();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x000B4AE0 File Offset: 0x000B2CE0
		protected override bool IsContentElementCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			return owningCellPeer == null || owningCellPeer.IsContentElement();
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x000B4B00 File Offset: 0x000B2D00
		protected override bool IsControlElementCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			return owningCellPeer == null || owningCellPeer.IsControlElement();
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x000B4B20 File Offset: 0x000B2D20
		protected override bool IsEnabledCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.IsEnabled();
			}
			this.ThrowElementNotAvailableException();
			return true;
		}

		// Token: 0x0600259A RID: 9626 RVA: 0x000B4B48 File Offset: 0x000B2D48
		protected override bool IsKeyboardFocusableCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.IsKeyboardFocusable();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x000B4B70 File Offset: 0x000B2D70
		protected override bool IsOffscreenCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.IsOffscreen();
			}
			this.ThrowElementNotAvailableException();
			return true;
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x000B4B98 File Offset: 0x000B2D98
		protected override bool IsPasswordCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.IsPassword();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x000B4BC0 File Offset: 0x000B2DC0
		protected override bool IsRequiredForFormCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.IsRequiredForForm();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x0600259E RID: 9630 RVA: 0x000B4BE8 File Offset: 0x000B2DE8
		protected override void SetFocusCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				owningCellPeer.SetFocus();
				return;
			}
			this.ThrowElementNotAvailableException();
		}

		// Token: 0x0600259F RID: 9631 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool IsDataItemAutomationPeer()
		{
			return true;
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x000B4C0C File Offset: 0x000B2E0C
		internal override void AddToParentProxyWeakRefCache()
		{
			DataGridItemAutomationPeer owningItemPeer = this.OwningItemPeer;
			if (owningItemPeer != null)
			{
				owningItemPeer.AddProxyToWeakRefStorage(base.ElementProxyWeakReference, this);
			}
		}

		/// <summary>Gets the ordinal number of the column that contains the cell or item.</summary>
		/// <returns>A zero-based ordinal number that identifies the column containing the cell or item.</returns>
		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x060025A1 RID: 9633 RVA: 0x000B4C30 File Offset: 0x000B2E30
		int IGridItemProvider.Column
		{
			get
			{
				return this.OwningDataGrid.Columns.IndexOf(this._column);
			}
		}

		/// <summary>Gets the number of columns spanned by a cell or item.</summary>
		/// <returns>The number of columns spanned. </returns>
		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x060025A2 RID: 9634 RVA: 0x00016748 File Offset: 0x00014948
		int IGridItemProvider.ColumnSpan
		{
			get
			{
				return 1;
			}
		}

		/// <summary>Gets a UI Automation provider that implements <see cref="T:System.Windows.Automation.Provider.IGridProvider" /> and represents the container of the cell or item.</summary>
		/// <returns>A UI Automation provider that implements the <see cref="T:System.Windows.Automation.GridPattern" /> and represents the cell or item container. </returns>
		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060025A3 RID: 9635 RVA: 0x000B4C48 File Offset: 0x000B2E48
		IRawElementProviderSimple IGridItemProvider.ContainingGrid
		{
			get
			{
				return this.ContainingGrid;
			}
		}

		/// <summary>Gets the ordinal number of the row that contains the cell or item.</summary>
		/// <returns>A zero-based ordinal number that identifies the row containing the cell or item. </returns>
		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060025A4 RID: 9636 RVA: 0x000B4C50 File Offset: 0x000B2E50
		int IGridItemProvider.Row
		{
			get
			{
				return this.OwningDataGrid.Items.IndexOf(this.Item);
			}
		}

		/// <summary>Gets the number of rows spanned by a cell or item.</summary>
		/// <returns>The number of rows spanned. </returns>
		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060025A5 RID: 9637 RVA: 0x00016748 File Offset: 0x00014948
		int IGridItemProvider.RowSpan
		{
			get
			{
				return 1;
			}
		}

		/// <summary>Retrieves a collection of UI Automation providers representing all the column headers associated with a table item or cell.</summary>
		/// <returns>A collection of UI Automation providers. </returns>
		// Token: 0x060025A6 RID: 9638 RVA: 0x000B4C68 File Offset: 0x000B2E68
		IRawElementProviderSimple[] ITableItemProvider.GetColumnHeaderItems()
		{
			if (this.OwningDataGrid != null && (this.OwningDataGrid.HeadersVisibility & DataGridHeadersVisibility.Column) == DataGridHeadersVisibility.Column && this.OwningDataGrid.ColumnHeadersPresenter != null)
			{
				DataGridColumnHeadersPresenterAutomationPeer dataGridColumnHeadersPresenterAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningDataGrid.ColumnHeadersPresenter) as DataGridColumnHeadersPresenterAutomationPeer;
				if (dataGridColumnHeadersPresenterAutomationPeer != null)
				{
					AutomationPeer automationPeer = dataGridColumnHeadersPresenterAutomationPeer.FindOrCreateItemAutomationPeer(this._column);
					if (automationPeer != null)
					{
						return new List<IRawElementProviderSimple>(1)
						{
							base.ProviderFromPeer(automationPeer)
						}.ToArray();
					}
				}
			}
			return null;
		}

		/// <summary>Retrieves a collection of UI Automation providers representing all the row headers associated with a table item or cell.</summary>
		/// <returns>A collection of UI Automation providers. </returns>
		// Token: 0x060025A7 RID: 9639 RVA: 0x000B4CE0 File Offset: 0x000B2EE0
		IRawElementProviderSimple[] ITableItemProvider.GetRowHeaderItems()
		{
			if (this.OwningDataGrid != null && (this.OwningDataGrid.HeadersVisibility & DataGridHeadersVisibility.Row) == DataGridHeadersVisibility.Row)
			{
				DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningDataGrid) as DataGridAutomationPeer;
				DataGridItemAutomationPeer dataGridItemAutomationPeer = dataGridAutomationPeer.FindOrCreateItemAutomationPeer(this.Item) as DataGridItemAutomationPeer;
				if (dataGridItemAutomationPeer != null)
				{
					AutomationPeer rowHeaderAutomationPeer = dataGridItemAutomationPeer.RowHeaderAutomationPeer;
					if (rowHeaderAutomationPeer != null)
					{
						return new List<IRawElementProviderSimple>(1)
						{
							base.ProviderFromPeer(rowHeaderAutomationPeer)
						}.ToArray();
					}
				}
			}
			return null;
		}

		/// <summary>Sends a request to activate a control and initiate its single, unambiguous action.</summary>
		// Token: 0x060025A8 RID: 9640 RVA: 0x000B4D54 File Offset: 0x000B2F54
		void IInvokeProvider.Invoke()
		{
			if (this.OwningDataGrid.IsReadOnly || this._column.IsReadOnly)
			{
				return;
			}
			this.EnsureEnabled();
			bool flag = false;
			if (this.OwningCell == null)
			{
				this.OwningDataGrid.ScrollIntoView(this.Item, this._column);
			}
			DataGridCell owningCell = this.OwningCell;
			if (owningCell != null)
			{
				if (!owningCell.IsEditing)
				{
					if (!owningCell.IsKeyboardFocusWithin)
					{
						owningCell.Focus();
					}
					this.OwningDataGrid.HandleSelectionForCellInput(owningCell, false, false, false);
					flag = this.OwningDataGrid.BeginEdit();
				}
				else
				{
					flag = true;
				}
			}
			if (!flag && !this.IsNewItemPlaceholder)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_AutomationInvokeFailed"));
			}
		}

		/// <summary>Scrolls the content area of a container object in order to display the control within the visible region (viewport) of the container.</summary>
		// Token: 0x060025A9 RID: 9641 RVA: 0x000B4DFE File Offset: 0x000B2FFE
		void IScrollItemProvider.ScrollIntoView()
		{
			this.OwningDataGrid.ScrollIntoView(this.Item, this._column);
		}

		/// <summary>Gets a value that indicates whether an item is selected. </summary>
		/// <returns>
		///     <see langword="true" /> if the element is selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060025AA RID: 9642 RVA: 0x000B4E17 File Offset: 0x000B3017
		bool ISelectionItemProvider.IsSelected
		{
			get
			{
				return this.OwningDataGrid.SelectedCellsInternal.Contains(new DataGridCellInfo(this.Item, this._column));
			}
		}

		/// <summary>Gets the UI Automation provider that implements <see cref="T:System.Windows.Automation.Provider.ISelectionProvider" /> and acts as the container for the calling object.</summary>
		/// <returns>The provider that supports <see cref="T:System.Windows.Automation.Provider.ISelectionProvider" />. </returns>
		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060025AB RID: 9643 RVA: 0x000B4C48 File Offset: 0x000B2E48
		IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
		{
			get
			{
				return this.ContainingGrid;
			}
		}

		/// <summary>Adds the current element to the collection of selected items.</summary>
		// Token: 0x060025AC RID: 9644 RVA: 0x000B4E3C File Offset: 0x000B303C
		void ISelectionItemProvider.AddToSelection()
		{
			if (!this.IsCellSelectionUnit)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_CannotSelectCell"));
			}
			DataGridCellInfo cell = new DataGridCellInfo(this.Item, this._column);
			if (this.OwningDataGrid.SelectedCellsInternal.Contains(cell))
			{
				return;
			}
			this.EnsureEnabled();
			if (this.OwningDataGrid.SelectionMode == DataGridSelectionMode.Single && this.OwningDataGrid.SelectedCells.Count > 0)
			{
				throw new InvalidOperationException();
			}
			this.OwningDataGrid.SelectedCellsInternal.Add(cell);
		}

		/// <summary>Removes the current element from the collection of selected items.</summary>
		// Token: 0x060025AD RID: 9645 RVA: 0x000B4EC8 File Offset: 0x000B30C8
		void ISelectionItemProvider.RemoveFromSelection()
		{
			if (!this.IsCellSelectionUnit)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_CannotSelectCell"));
			}
			this.EnsureEnabled();
			DataGridCellInfo cell = new DataGridCellInfo(this.Item, this._column);
			if (this.OwningDataGrid.SelectedCellsInternal.Contains(cell))
			{
				this.OwningDataGrid.SelectedCellsInternal.Remove(cell);
			}
		}

		/// <summary>Deselects any selected items and then selects the current element.</summary>
		// Token: 0x060025AE RID: 9646 RVA: 0x000B4F2C File Offset: 0x000B312C
		void ISelectionItemProvider.Select()
		{
			if (!this.IsCellSelectionUnit)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_CannotSelectCell"));
			}
			this.EnsureEnabled();
			DataGridCellInfo currentCellInfo = new DataGridCellInfo(this.Item, this._column);
			this.OwningDataGrid.SelectOnlyThisCell(currentCellInfo);
		}

		/// <summary>Gets a value that specifies whether the value of a control is read-only. </summary>
		/// <returns>
		///     <see langword="true" /> if the value is read-only; <see langword="false" /> if it can be modified. </returns>
		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060025AF RID: 9647 RVA: 0x000B4F76 File Offset: 0x000B3176
		bool IValueProvider.IsReadOnly
		{
			get
			{
				return this._column.IsReadOnly;
			}
		}

		/// <summary>Sets the value of a control.</summary>
		/// <param name="value">The value to set. The provider is responsible for converting the value to the appropriate data type.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Controls.DataGridCell" /> object that is associated with this <see cref="T:System.Windows.Automation.Peers.DataGridCellItemAutomationPeer" /> object is read-only.</exception>
		// Token: 0x060025B0 RID: 9648 RVA: 0x000B4F83 File Offset: 0x000B3183
		void IValueProvider.SetValue(string value)
		{
			if (this._column.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_ColumnIsReadOnly"));
			}
			if (this.OwningDataGrid != null)
			{
				this.OwningDataGrid.SetCellAutomationValue(this.Item, this._column, value);
			}
		}

		/// <summary>Gets the value of the control.</summary>
		/// <returns>The value of the control as a string. </returns>
		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x060025B1 RID: 9649 RVA: 0x000B4FC2 File Offset: 0x000B31C2
		string IValueProvider.Value
		{
			get
			{
				if (this.OwningDataGrid != null)
				{
					return this.OwningDataGrid.GetCellAutomationValue(this.Item, this._column);
				}
				return null;
			}
		}

		/// <summary>Makes the virtual item fully accessible as a UI Automation element.</summary>
		// Token: 0x060025B2 RID: 9650 RVA: 0x000B4DFE File Offset: 0x000B2FFE
		void IVirtualizedItemProvider.Realize()
		{
			this.OwningDataGrid.ScrollIntoView(this.Item, this._column);
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x000B4FE5 File Offset: 0x000B31E5
		private void EnsureEnabled()
		{
			if (!this.OwningDataGrid.IsEnabled)
			{
				throw new ElementNotEnabledException();
			}
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x000B4FFA File Offset: 0x000B31FA
		private void ThrowElementNotAvailableException()
		{
			if (VirtualizedItemPatternIdentifiers.Pattern != null && !this.IsItemInAutomationTree())
			{
				throw new ElementNotAvailableException(SR.Get("VirtualizedElement"));
			}
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x000B501C File Offset: 0x000B321C
		private bool IsItemInAutomationTree()
		{
			AutomationPeer parent = base.GetParent();
			return base.Index != -1 && parent != null && parent.Children != null && base.Index < parent.Children.Count && parent.Children[base.Index] == this;
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x060025B6 RID: 9654 RVA: 0x000B506E File Offset: 0x000B326E
		private bool IsCellSelectionUnit
		{
			get
			{
				return this.OwningDataGrid != null && (this.OwningDataGrid.SelectionUnit == DataGridSelectionUnit.Cell || this.OwningDataGrid.SelectionUnit == DataGridSelectionUnit.CellOrRowHeader);
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x060025B7 RID: 9655 RVA: 0x000B5098 File Offset: 0x000B3298
		private bool IsNewItemPlaceholder
		{
			get
			{
				object item = this.Item;
				return item == CollectionView.NewItemPlaceholder || item == DataGrid.NewItemPlaceholder;
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x060025B8 RID: 9656 RVA: 0x000B50BE File Offset: 0x000B32BE
		private DataGrid OwningDataGrid
		{
			get
			{
				return this._column.DataGridOwner;
			}
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x060025B9 RID: 9657 RVA: 0x000B50CC File Offset: 0x000B32CC
		private DataGridCell OwningCell
		{
			get
			{
				DataGrid owningDataGrid = this.OwningDataGrid;
				if (owningDataGrid == null)
				{
					return null;
				}
				return owningDataGrid.TryFindCell(this.Item, this._column);
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x060025BA RID: 9658 RVA: 0x000B50F8 File Offset: 0x000B32F8
		internal DataGridCellAutomationPeer OwningCellPeer
		{
			get
			{
				DataGridCellAutomationPeer dataGridCellAutomationPeer = null;
				DataGridCell owningCell = this.OwningCell;
				if (owningCell != null)
				{
					dataGridCellAutomationPeer = (UIElementAutomationPeer.CreatePeerForElement(owningCell) as DataGridCellAutomationPeer);
					dataGridCellAutomationPeer.EventsSource = this;
				}
				return dataGridCellAutomationPeer;
			}
		}

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x060025BB RID: 9659 RVA: 0x000B5128 File Offset: 0x000B3328
		private IRawElementProviderSimple ContainingGrid
		{
			get
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningDataGrid);
				if (automationPeer != null)
				{
					return base.ProviderFromPeer(automationPeer);
				}
				return null;
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x060025BC RID: 9660 RVA: 0x000B514D File Offset: 0x000B334D
		internal DataGridColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x060025BD RID: 9661 RVA: 0x000B5155 File Offset: 0x000B3355
		internal object Item
		{
			get
			{
				if (this._item != null)
				{
					return this._item.Target;
				}
				return null;
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x060025BE RID: 9662 RVA: 0x000B516C File Offset: 0x000B336C
		private DataGridItemAutomationPeer OwningItemPeer
		{
			get
			{
				if (this.OwningDataGrid != null)
				{
					DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningDataGrid) as DataGridAutomationPeer;
					if (dataGridAutomationPeer != null)
					{
						return dataGridAutomationPeer.GetExistingPeerByItem(this.Item, true) as DataGridItemAutomationPeer;
					}
				}
				return null;
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x060025BF RID: 9663 RVA: 0x000B51A9 File Offset: 0x000B33A9
		// (set) Token: 0x060025C0 RID: 9664 RVA: 0x000B51B4 File Offset: 0x000B33B4
		internal override bool AncestorsInvalid
		{
			get
			{
				return base.AncestorsInvalid;
			}
			set
			{
				base.AncestorsInvalid = value;
				if (value)
				{
					return;
				}
				AutomationPeer owningCellPeer = this.OwningCellPeer;
				if (owningCellPeer != null)
				{
					owningCellPeer.AncestorsInvalid = false;
				}
			}
		}

		// Token: 0x04001B68 RID: 7016
		private WeakReference _item;

		// Token: 0x04001B69 RID: 7017
		private DataGridColumn _column;
	}
}
