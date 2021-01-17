using System;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Represents a selectable item in a <see cref="T:System.Windows.Controls.ListBox" />. </summary>
	// Token: 0x020004FC RID: 1276
	[DefaultEvent("Selected")]
	public class ListBoxItem : ContentControl
	{
		// Token: 0x0600514F RID: 20815 RVA: 0x0016CD5C File Offset: 0x0016AF5C
		static ListBoxItem()
		{
			ListBoxItem.SelectedEvent = Selector.SelectedEvent.AddOwner(typeof(ListBoxItem));
			ListBoxItem.UnselectedEvent = Selector.UnselectedEvent.AddOwner(typeof(ListBoxItem));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ListBoxItem), new FrameworkPropertyMetadata(typeof(ListBoxItem)));
			ListBoxItem._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ListBoxItem));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ListBoxItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ListBoxItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(ListBoxItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(ListBoxItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			Selector.IsSelectionActivePropertyKey.OverrideMetadata(typeof(ListBoxItem), new FrameworkPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(ListBoxItem), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		/// <summary>Gets or sets a value that indicates whether a <see cref="T:System.Windows.Controls.ListBoxItem" /> is selected.  </summary>
		/// <returns>
		///     <see langword="true" /> if the item is selected; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170013B9 RID: 5049
		// (get) Token: 0x06005150 RID: 20816 RVA: 0x0016CED2 File Offset: 0x0016B0D2
		// (set) Token: 0x06005151 RID: 20817 RVA: 0x0016CEE4 File Offset: 0x0016B0E4
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(ListBoxItem.IsSelectedProperty);
			}
			set
			{
				base.SetValue(ListBoxItem.IsSelectedProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06005152 RID: 20818 RVA: 0x0016CEF8 File Offset: 0x0016B0F8
		private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ListBoxItem listBoxItem = d as ListBoxItem;
			bool flag = (bool)e.NewValue;
			Selector parentSelector = listBoxItem.ParentSelector;
			if (parentSelector != null)
			{
				parentSelector.RaiseIsSelectedChangedAutomationEvent(listBoxItem, flag);
			}
			if (flag)
			{
				listBoxItem.OnSelected(new RoutedEventArgs(Selector.SelectedEvent, listBoxItem));
			}
			else
			{
				listBoxItem.OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, listBoxItem));
			}
			listBoxItem.UpdateVisualState();
		}

		/// <summary>Called when the <see cref="T:System.Windows.Controls.ListBoxItem" /> is selected in a <see cref="T:System.Windows.Controls.ListBox" />.  </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005153 RID: 20819 RVA: 0x0016CF58 File Offset: 0x0016B158
		protected virtual void OnSelected(RoutedEventArgs e)
		{
			this.HandleIsSelectedChanged(true, e);
		}

		/// <summary>Called when the <see cref="T:System.Windows.Controls.ListBoxItem" /> is unselected in a <see cref="T:System.Windows.Controls.ListBox" />. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005154 RID: 20820 RVA: 0x0016CF62 File Offset: 0x0016B162
		protected virtual void OnUnselected(RoutedEventArgs e)
		{
			this.HandleIsSelectedChanged(false, e);
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x0016CF6C File Offset: 0x0016B16C
		private void HandleIsSelectedChanged(bool newValue, RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Controls.ListBoxItem" /> is selected.</summary>
		// Token: 0x140000F4 RID: 244
		// (add) Token: 0x06005156 RID: 20822 RVA: 0x0016CF75 File Offset: 0x0016B175
		// (remove) Token: 0x06005157 RID: 20823 RVA: 0x0016CF83 File Offset: 0x0016B183
		public event RoutedEventHandler Selected
		{
			add
			{
				base.AddHandler(ListBoxItem.SelectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(ListBoxItem.SelectedEvent, value);
			}
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Controls.ListBoxItem" /> is unselected.</summary>
		// Token: 0x140000F5 RID: 245
		// (add) Token: 0x06005158 RID: 20824 RVA: 0x0016CF91 File Offset: 0x0016B191
		// (remove) Token: 0x06005159 RID: 20825 RVA: 0x0016CF9F File Offset: 0x0016B19F
		public event RoutedEventHandler Unselected
		{
			add
			{
				base.AddHandler(ListBoxItem.UnselectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(ListBoxItem.UnselectedEvent, value);
			}
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x0016CFB0 File Offset: 0x0016B1B0
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStateManager.GoToState(this, (base.Content is Control) ? "Normal" : "Disabled", useTransitions);
			}
			else if (base.IsMouseOver)
			{
				VisualStateManager.GoToState(this, "MouseOver", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			if (this.IsSelected)
			{
				if (Selector.GetIsSelectionActive(this))
				{
					VisualStateManager.GoToState(this, "Selected", useTransitions);
				}
				else
				{
					VisualStates.GoToState(this, useTransitions, new string[]
					{
						"SelectedUnfocused",
						"Selected"
					});
				}
			}
			else
			{
				VisualStateManager.GoToState(this, "Unselected", useTransitions);
			}
			if (base.IsKeyboardFocused)
			{
				VisualStateManager.GoToState(this, "Focused", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.ListBoxItemAutomationPeer" /> implementation for this control, as part of the WPF automation infrastructure.</summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
		// Token: 0x0600515B RID: 20827 RVA: 0x0016D082 File Offset: 0x0016B282
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ListBoxItemWrapperAutomationPeer(this);
		}

		/// <summary>Called when the user presses the right mouse button over the <see cref="T:System.Windows.Controls.ListBoxItem" />.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x0600515C RID: 20828 RVA: 0x0016D08A File Offset: 0x0016B28A
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = true;
				this.HandleMouseButtonDown(MouseButton.Left);
			}
			base.OnMouseLeftButtonDown(e);
		}

		/// <summary>Called when the user presses the right mouse button over a <see cref="T:System.Windows.Controls.ListBoxItem" />.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x0600515D RID: 20829 RVA: 0x0016D0A9 File Offset: 0x0016B2A9
		protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = true;
				this.HandleMouseButtonDown(MouseButton.Right);
			}
			base.OnMouseRightButtonDown(e);
		}

		// Token: 0x0600515E RID: 20830 RVA: 0x0016D0C8 File Offset: 0x0016B2C8
		private void HandleMouseButtonDown(MouseButton mouseButton)
		{
			if (Selector.UiGetIsSelectable(this) && base.Focus())
			{
				ListBox parentListBox = this.ParentListBox;
				if (parentListBox != null)
				{
					parentListBox.NotifyListItemClicked(this, mouseButton);
				}
			}
		}

		/// <summary>Called when the mouse enters a <see cref="T:System.Windows.Controls.ListBoxItem" />. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x0600515F RID: 20831 RVA: 0x0016D0F8 File Offset: 0x0016B2F8
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			if (this.parentNotifyDraggedOperation != null)
			{
				this.parentNotifyDraggedOperation.Abort();
				this.parentNotifyDraggedOperation = null;
			}
			if (base.IsMouseOver)
			{
				ListBox parentListBox = this.ParentListBox;
				if (parentListBox != null && Mouse.LeftButton == MouseButtonState.Pressed)
				{
					parentListBox.NotifyListItemMouseDragged(this);
				}
			}
			base.OnMouseEnter(e);
		}

		/// <summary>Called when the mouse leaves a <see cref="T:System.Windows.Controls.ListBoxItem" />. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005160 RID: 20832 RVA: 0x0016D148 File Offset: 0x0016B348
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			if (this.parentNotifyDraggedOperation != null)
			{
				this.parentNotifyDraggedOperation.Abort();
				this.parentNotifyDraggedOperation = null;
			}
			base.OnMouseLeave(e);
		}

		/// <summary>Called when the visual parent of a list box item changes. </summary>
		/// <param name="oldParent">The previous <see cref="P:System.Windows.FrameworkElement.Parent" /> property of the <see cref="T:System.Windows.Controls.ListBoxItem" />.</param>
		// Token: 0x06005161 RID: 20833 RVA: 0x0016D16C File Offset: 0x0016B36C
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			ItemsControl itemsControl = null;
			if (VisualTreeHelper.GetParent(this) == null && base.IsKeyboardFocusWithin)
			{
				itemsControl = ItemsControl.GetItemsOwner(oldParent);
			}
			base.OnVisualParentChanged(oldParent);
			if (itemsControl != null)
			{
				itemsControl.Focus();
			}
		}

		// Token: 0x170013BA RID: 5050
		// (get) Token: 0x06005162 RID: 20834 RVA: 0x0016D1A3 File Offset: 0x0016B3A3
		private ListBox ParentListBox
		{
			get
			{
				return this.ParentSelector as ListBox;
			}
		}

		// Token: 0x170013BB RID: 5051
		// (get) Token: 0x06005163 RID: 20835 RVA: 0x0016D1B0 File Offset: 0x0016B3B0
		internal Selector ParentSelector
		{
			get
			{
				return ItemsControl.ItemsControlFromItemContainer(this) as Selector;
			}
		}

		// Token: 0x170013BC RID: 5052
		// (get) Token: 0x06005164 RID: 20836 RVA: 0x0016D1BD File Offset: 0x0016B3BD
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ListBoxItem._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ListBoxItem.IsSelected" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ListBoxItem.IsSelected" /> dependency property.</returns>
		// Token: 0x04002C60 RID: 11360
		public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(ListBoxItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(ListBoxItem.OnIsSelectedChanged)));

		// Token: 0x04002C63 RID: 11363
		private DispatcherOperation parentNotifyDraggedOperation;

		// Token: 0x04002C64 RID: 11364
		private static DependencyObjectType _dType;
	}
}
