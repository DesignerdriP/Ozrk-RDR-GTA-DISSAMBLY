﻿using System;
using System.ComponentModel;
using System.Security;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal.Commands;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents the base class for all <see cref="T:System.Windows.Controls.Button" /> controls. </summary>
	// Token: 0x02000576 RID: 1398
	[DefaultEvent("Click")]
	[Localizability(LocalizationCategory.Button)]
	public abstract class ButtonBase : ContentControl, ICommandSource
	{
		// Token: 0x06005BFE RID: 23550 RVA: 0x0019DD74 File Offset: 0x0019BF74
		static ButtonBase()
		{
			ButtonBase.ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ButtonBase));
			ButtonBase.CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ButtonBase), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ButtonBase.OnCommandChanged)));
			ButtonBase.CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(ButtonBase), new FrameworkPropertyMetadata(null));
			ButtonBase.CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(ButtonBase), new FrameworkPropertyMetadata(null));
			ButtonBase.IsPressedPropertyKey = DependencyProperty.RegisterReadOnly("IsPressed", typeof(bool), typeof(ButtonBase), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(ButtonBase.OnIsPressedChanged)));
			ButtonBase.IsPressedProperty = ButtonBase.IsPressedPropertyKey.DependencyProperty;
			ButtonBase.ClickModeProperty = DependencyProperty.Register("ClickMode", typeof(ClickMode), typeof(ButtonBase), new FrameworkPropertyMetadata(ClickMode.Release), new ValidateValueCallback(ButtonBase.IsValidClickMode));
			EventManager.RegisterClassHandler(typeof(ButtonBase), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(ButtonBase.OnAccessKeyPressed));
			KeyboardNavigation.AcceptsReturnProperty.OverrideMetadata(typeof(ButtonBase), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			InputMethod.IsInputMethodEnabledProperty.OverrideMetadata(typeof(ButtonBase), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(ButtonBase), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(ButtonBase), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> routed event. </summary>
		// Token: 0x06005C00 RID: 23552 RVA: 0x0019DF58 File Offset: 0x0019C158
		protected virtual void OnClick()
		{
			RoutedEventArgs e = new RoutedEventArgs(ButtonBase.ClickEvent, this);
			base.RaiseEvent(e);
			CommandHelpers.ExecuteCommandSource(this);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.IsPressed" /> property changes.</summary>
		/// <param name="e">The data for <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" />.</param>
		// Token: 0x06005C01 RID: 23553 RVA: 0x0019DF7E File Offset: 0x0019C17E
		protected virtual void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
		{
			Control.OnVisualStatePropertyChanged(this, e);
		}

		// Token: 0x1700164D RID: 5709
		// (get) Token: 0x06005C02 RID: 23554 RVA: 0x0019DF88 File Offset: 0x0019C188
		private bool IsInMainFocusScope
		{
			get
			{
				Visual visual = FocusManager.GetFocusScope(this) as Visual;
				return visual == null || VisualTreeHelper.GetParent(visual) == null;
			}
		}

		// Token: 0x06005C03 RID: 23555 RVA: 0x0019DFAF File Offset: 0x0019C1AF
		internal void AutomationButtonBaseClick()
		{
			this.OnClick();
		}

		// Token: 0x06005C04 RID: 23556 RVA: 0x0019DFB8 File Offset: 0x0019C1B8
		private static bool IsValidClickMode(object o)
		{
			ClickMode clickMode = (ClickMode)o;
			return clickMode == ClickMode.Press || clickMode == ClickMode.Release || clickMode == ClickMode.Hover;
		}

		/// <summary> Called when the rendered size of a control changes. </summary>
		/// <param name="sizeInfo">Specifies the size changes.</param>
		// Token: 0x06005C05 RID: 23557 RVA: 0x0019DFD9 File Offset: 0x0019C1D9
		protected internal override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);
			if (base.IsMouseCaptured && Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed && !this.IsSpaceKeyDown)
			{
				this.UpdateIsPressed();
			}
		}

		// Token: 0x06005C06 RID: 23558 RVA: 0x0019E008 File Offset: 0x0019C208
		private static void OnIsPressedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ButtonBase buttonBase = (ButtonBase)d;
			buttonBase.OnIsPressedChanged(e);
		}

		// Token: 0x06005C07 RID: 23559 RVA: 0x0019E023 File Offset: 0x0019C223
		private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
		{
			if (!e.Handled && e.Scope == null && e.Target == null)
			{
				e.Target = (UIElement)sender;
			}
		}

		// Token: 0x06005C08 RID: 23560 RVA: 0x0019E04C File Offset: 0x0019C24C
		private void UpdateIsPressed()
		{
			Point position = Mouse.PrimaryDevice.GetPosition(this);
			if (position.X >= 0.0 && position.X <= base.ActualWidth && position.Y >= 0.0 && position.Y <= base.ActualHeight)
			{
				if (!this.IsPressed)
				{
					this.SetIsPressed(true);
					return;
				}
			}
			else if (this.IsPressed)
			{
				this.SetIsPressed(false);
			}
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Controls.Button" /> is clicked. </summary>
		// Token: 0x14000112 RID: 274
		// (add) Token: 0x06005C09 RID: 23561 RVA: 0x0019E0C6 File Offset: 0x0019C2C6
		// (remove) Token: 0x06005C0A RID: 23562 RVA: 0x0019E0D4 File Offset: 0x0019C2D4
		[Category("Behavior")]
		public event RoutedEventHandler Click
		{
			add
			{
				base.AddHandler(ButtonBase.ClickEvent, value);
			}
			remove
			{
				base.RemoveHandler(ButtonBase.ClickEvent, value);
			}
		}

		/// <summary>Gets a value that indicates whether a <see cref="T:System.Windows.Controls.Primitives.ButtonBase" /> is currently activated.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.Primitives.ButtonBase" /> is activated; otherwise <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700164E RID: 5710
		// (get) Token: 0x06005C0B RID: 23563 RVA: 0x0019E0E2 File Offset: 0x0019C2E2
		// (set) Token: 0x06005C0C RID: 23564 RVA: 0x0019E0F4 File Offset: 0x0019C2F4
		[Browsable(false)]
		[Category("Appearance")]
		[ReadOnly(true)]
		public bool IsPressed
		{
			get
			{
				return (bool)base.GetValue(ButtonBase.IsPressedProperty);
			}
			protected set
			{
				base.SetValue(ButtonBase.IsPressedPropertyKey, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06005C0D RID: 23565 RVA: 0x0019E107 File Offset: 0x0019C307
		private void SetIsPressed(bool pressed)
		{
			if (pressed)
			{
				base.SetValue(ButtonBase.IsPressedPropertyKey, BooleanBoxes.Box(pressed));
				return;
			}
			base.ClearValue(ButtonBase.IsPressedPropertyKey);
		}

		/// <summary>Gets or sets the command to invoke when this button is pressed.  </summary>
		/// <returns>A command to invoke when this button is pressed. The default value is <see langword="null" />.</returns>
		// Token: 0x1700164F RID: 5711
		// (get) Token: 0x06005C0E RID: 23566 RVA: 0x0019E129 File Offset: 0x0019C329
		// (set) Token: 0x06005C0F RID: 23567 RVA: 0x0019E13B File Offset: 0x0019C33B
		[Bindable(true)]
		[Category("Action")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(ButtonBase.CommandProperty);
			}
			set
			{
				base.SetValue(ButtonBase.CommandProperty, value);
			}
		}

		// Token: 0x06005C10 RID: 23568 RVA: 0x0019E14C File Offset: 0x0019C34C
		private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ButtonBase buttonBase = (ButtonBase)d;
			buttonBase.OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
		}

		// Token: 0x06005C11 RID: 23569 RVA: 0x0019E17E File Offset: 0x0019C37E
		private void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
		{
			if (oldCommand != null)
			{
				this.UnhookCommand(oldCommand);
			}
			if (newCommand != null)
			{
				this.HookCommand(newCommand);
			}
		}

		// Token: 0x06005C12 RID: 23570 RVA: 0x0019E194 File Offset: 0x0019C394
		private void UnhookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.RemoveHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
			this.UpdateCanExecute();
		}

		// Token: 0x06005C13 RID: 23571 RVA: 0x0019E1AE File Offset: 0x0019C3AE
		private void HookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.AddHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
			this.UpdateCanExecute();
		}

		// Token: 0x06005C14 RID: 23572 RVA: 0x0019E1C8 File Offset: 0x0019C3C8
		private void OnCanExecuteChanged(object sender, EventArgs e)
		{
			this.UpdateCanExecute();
		}

		// Token: 0x06005C15 RID: 23573 RVA: 0x0019E1D0 File Offset: 0x0019C3D0
		private void UpdateCanExecute()
		{
			if (this.Command != null)
			{
				this.CanExecute = CommandHelpers.CanExecuteCommandSource(this);
				return;
			}
			this.CanExecute = true;
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.ContentElement.IsEnabled" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if the control is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001650 RID: 5712
		// (get) Token: 0x06005C16 RID: 23574 RVA: 0x0019E1EE File Offset: 0x0019C3EE
		protected override bool IsEnabledCore
		{
			get
			{
				return base.IsEnabledCore && this.CanExecute;
			}
		}

		/// <summary>Gets or sets the parameter to pass to the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.Command" /> property.  </summary>
		/// <returns>Parameter to pass to the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.Command" /> property.</returns>
		// Token: 0x17001651 RID: 5713
		// (get) Token: 0x06005C17 RID: 23575 RVA: 0x0019E200 File Offset: 0x0019C400
		// (set) Token: 0x06005C18 RID: 23576 RVA: 0x0019E20D File Offset: 0x0019C40D
		[Bindable(true)]
		[Category("Action")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public object CommandParameter
		{
			get
			{
				return base.GetValue(ButtonBase.CommandParameterProperty);
			}
			set
			{
				base.SetValue(ButtonBase.CommandParameterProperty, value);
			}
		}

		/// <summary>Gets or sets the element on which to raise the specified command.  </summary>
		/// <returns>Element on which to raise a command.</returns>
		// Token: 0x17001652 RID: 5714
		// (get) Token: 0x06005C19 RID: 23577 RVA: 0x0019E21B File Offset: 0x0019C41B
		// (set) Token: 0x06005C1A RID: 23578 RVA: 0x0019E22D File Offset: 0x0019C42D
		[Bindable(true)]
		[Category("Action")]
		public IInputElement CommandTarget
		{
			get
			{
				return (IInputElement)base.GetValue(ButtonBase.CommandTargetProperty);
			}
			set
			{
				base.SetValue(ButtonBase.CommandTargetProperty, value);
			}
		}

		/// <summary>Gets or sets when the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event occurs.  </summary>
		/// <returns>When the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event occurs. The default value is <see cref="F:System.Windows.Controls.ClickMode.Release" />. </returns>
		// Token: 0x17001653 RID: 5715
		// (get) Token: 0x06005C1B RID: 23579 RVA: 0x0019E23B File Offset: 0x0019C43B
		// (set) Token: 0x06005C1C RID: 23580 RVA: 0x0019E24D File Offset: 0x0019C44D
		[Bindable(true)]
		[Category("Behavior")]
		public ClickMode ClickMode
		{
			get
			{
				return (ClickMode)base.GetValue(ButtonBase.ClickModeProperty);
			}
			set
			{
				base.SetValue(ButtonBase.ClickModeProperty, value);
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> routed event that occurs when the left mouse button is pressed while the mouse pointer is over this control.</summary>
		/// <param name="e">The event data. </param>
		// Token: 0x06005C1D RID: 23581 RVA: 0x0019E260 File Offset: 0x0019C460
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (this.ClickMode != ClickMode.Hover)
			{
				e.Handled = true;
				base.Focus();
				if (e.ButtonState == MouseButtonState.Pressed)
				{
					base.CaptureMouse();
					if (base.IsMouseCaptured)
					{
						if (e.ButtonState == MouseButtonState.Pressed)
						{
							if (!this.IsPressed)
							{
								this.SetIsPressed(true);
							}
						}
						else
						{
							base.ReleaseMouseCapture();
						}
					}
				}
				if (this.ClickMode == ClickMode.Press)
				{
					bool flag = true;
					try
					{
						this.OnClick();
						flag = false;
					}
					finally
					{
						if (flag)
						{
							this.SetIsPressed(false);
							base.ReleaseMouseCapture();
						}
					}
				}
			}
			base.OnMouseLeftButtonDown(e);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeftButtonUp" /> routed event that occurs when the left mouse button is released while the mouse pointer is over this control. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005C1E RID: 23582 RVA: 0x0019E2FC File Offset: 0x0019C4FC
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (this.ClickMode != ClickMode.Hover)
			{
				e.Handled = true;
				bool flag = !this.IsSpaceKeyDown && this.IsPressed && this.ClickMode == ClickMode.Release;
				if (base.IsMouseCaptured && !this.IsSpaceKeyDown)
				{
					base.ReleaseMouseCapture();
				}
				if (flag)
				{
					this.OnClick();
				}
			}
			base.OnMouseLeftButtonUp(e);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseMove" /> routed event that occurs when the mouse pointer moves while over this element.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005C1F RID: 23583 RVA: 0x0019E35C File Offset: 0x0019C55C
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (this.ClickMode != ClickMode.Hover && base.IsMouseCaptured && Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed && !this.IsSpaceKeyDown)
			{
				this.UpdateIsPressed();
				e.Handled = true;
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.LostMouseCapture" /> routed event that occurs when this control is no longer receiving mouse event messages. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Input.Mouse.LostMouseCapture" /> event.</param>
		// Token: 0x06005C20 RID: 23584 RVA: 0x0019E398 File Offset: 0x0019C598
		protected override void OnLostMouseCapture(MouseEventArgs e)
		{
			base.OnLostMouseCapture(e);
			if (e.OriginalSource == this && this.ClickMode != ClickMode.Hover && !this.IsSpaceKeyDown)
			{
				if (base.IsKeyboardFocused && !this.IsInMainFocusScope)
				{
					Keyboard.Focus(null);
				}
				this.SetIsPressed(false);
			}
		}

		/// <summary>Provides class handling for the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.ClickMode" /> routed event that occurs when the mouse enters this control. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Input.Mouse.MouseEnter" /> event.</param>
		// Token: 0x06005C21 RID: 23585 RVA: 0x0019E3E4 File Offset: 0x0019C5E4
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			base.OnMouseEnter(e);
			if (this.HandleIsMouseOverChanged())
			{
				e.Handled = true;
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeave" /> routed event that occurs when the mouse leaves an element. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Input.Mouse.MouseLeave" /> event.</param>
		// Token: 0x06005C22 RID: 23586 RVA: 0x0019E3FC File Offset: 0x0019C5FC
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			if (this.HandleIsMouseOverChanged())
			{
				e.Handled = true;
			}
		}

		// Token: 0x06005C23 RID: 23587 RVA: 0x0019E414 File Offset: 0x0019C614
		private bool HandleIsMouseOverChanged()
		{
			if (this.ClickMode == ClickMode.Hover)
			{
				if (base.IsMouseOver)
				{
					this.SetIsPressed(true);
					this.OnClick();
				}
				else
				{
					this.SetIsPressed(false);
				}
				return true;
			}
			return false;
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.KeyDown" /> routed event that occurs when the user presses a key while this control has focus.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005C24 RID: 23588 RVA: 0x0019E440 File Offset: 0x0019C640
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (this.ClickMode == ClickMode.Hover)
			{
				return;
			}
			if (e.Key == Key.Space)
			{
				if ((Keyboard.Modifiers & (ModifierKeys.Alt | ModifierKeys.Control)) != ModifierKeys.Alt && !base.IsMouseCaptured && e.OriginalSource == this)
				{
					this.IsSpaceKeyDown = true;
					this.SetIsPressed(true);
					base.CaptureMouse();
					if (this.ClickMode == ClickMode.Press)
					{
						this.OnClick();
					}
					e.Handled = true;
					return;
				}
			}
			else if (e.Key == Key.Return && (bool)base.GetValue(KeyboardNavigation.AcceptsReturnProperty))
			{
				if (e.OriginalSource == this)
				{
					this.IsSpaceKeyDown = false;
					this.SetIsPressed(false);
					if (base.IsMouseCaptured)
					{
						base.ReleaseMouseCapture();
					}
					this.OnClick();
					e.Handled = true;
					return;
				}
			}
			else if (this.IsSpaceKeyDown)
			{
				this.SetIsPressed(false);
				this.IsSpaceKeyDown = false;
				if (base.IsMouseCaptured)
				{
					base.ReleaseMouseCapture();
				}
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.KeyUp" /> routed event that occurs when the user releases a key while this control has focus.</summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.KeyUp" /> event.</param>
		// Token: 0x06005C25 RID: 23589 RVA: 0x0019E52C File Offset: 0x0019C72C
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (this.ClickMode == ClickMode.Hover)
			{
				return;
			}
			if (e.Key == Key.Space && this.IsSpaceKeyDown && (Keyboard.Modifiers & (ModifierKeys.Alt | ModifierKeys.Control)) != ModifierKeys.Alt)
			{
				this.IsSpaceKeyDown = false;
				if (this.GetMouseLeftButtonReleased())
				{
					bool flag = this.IsPressed && this.ClickMode == ClickMode.Release;
					if (base.IsMouseCaptured)
					{
						base.ReleaseMouseCapture();
					}
					if (flag)
					{
						this.OnClick();
					}
				}
				else if (base.IsMouseCaptured)
				{
					this.UpdateIsPressed();
				}
				e.Handled = true;
			}
		}

		/// <summary> Called when an element loses keyboard focus. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.IInputElement.LostKeyboardFocus" /> event.</param>
		// Token: 0x06005C26 RID: 23590 RVA: 0x0019E5B8 File Offset: 0x0019C7B8
		protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnLostKeyboardFocus(e);
			if (this.ClickMode == ClickMode.Hover)
			{
				return;
			}
			if (e.OriginalSource == this)
			{
				if (this.IsPressed)
				{
					this.SetIsPressed(false);
				}
				if (base.IsMouseCaptured)
				{
					base.ReleaseMouseCapture();
				}
				this.IsSpaceKeyDown = false;
			}
		}

		/// <summary>Responds when the <see cref="P:System.Windows.Controls.AccessText.AccessKey" /> for this control is called. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Input.AccessKeyManager.AccessKeyPressed" /> event.</param>
		// Token: 0x06005C27 RID: 23591 RVA: 0x0019E5F8 File Offset: 0x0019C7F8
		protected override void OnAccessKey(AccessKeyEventArgs e)
		{
			if (e.IsMultiple)
			{
				base.OnAccessKey(e);
				return;
			}
			this.OnClick();
		}

		// Token: 0x06005C28 RID: 23592 RVA: 0x0019E610 File Offset: 0x0019C810
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool GetMouseLeftButtonReleased()
		{
			return InputManager.Current.PrimaryMouseDevice.LeftButton == MouseButtonState.Released;
		}

		// Token: 0x17001654 RID: 5716
		// (get) Token: 0x06005C29 RID: 23593 RVA: 0x0019E624 File Offset: 0x0019C824
		// (set) Token: 0x06005C2A RID: 23594 RVA: 0x0019E62D File Offset: 0x0019C82D
		private bool IsSpaceKeyDown
		{
			get
			{
				return base.ReadControlFlag(Control.ControlBoolFlags.IsSpaceKeyDown);
			}
			set
			{
				base.WriteControlFlag(Control.ControlBoolFlags.IsSpaceKeyDown, value);
			}
		}

		// Token: 0x17001655 RID: 5717
		// (get) Token: 0x06005C2B RID: 23595 RVA: 0x0017033E File Offset: 0x0016E53E
		// (set) Token: 0x06005C2C RID: 23596 RVA: 0x0019E637 File Offset: 0x0019C837
		private bool CanExecute
		{
			get
			{
				return !base.ReadControlFlag(Control.ControlBoolFlags.CommandDisabled);
			}
			set
			{
				if (value != this.CanExecute)
				{
					base.WriteControlFlag(Control.ControlBoolFlags.CommandDisabled, !value);
					base.CoerceValue(UIElement.IsEnabledProperty);
				}
			}
		}

		// Token: 0x06005C2D RID: 23597 RVA: 0x0019E658 File Offset: 0x0019C858
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Disabled", useTransitions);
			}
			else if (this.IsPressed)
			{
				VisualStateManager.GoToState(this, "Pressed", useTransitions);
			}
			else if (base.IsMouseOver)
			{
				VisualStateManager.GoToState(this, "MouseOver", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
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

		/// <summary>Identifies the routed <see cref="P:System.Windows.Controls.Primitives.ButtonBase.Command" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.Command" /> dependency property.</returns>
		// Token: 0x04002FA2 RID: 12194
		[CommonDependencyProperty]
		public static readonly DependencyProperty CommandProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.CommandParameter" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.CommandParameter" /> dependency property.</returns>
		// Token: 0x04002FA3 RID: 12195
		[CommonDependencyProperty]
		public static readonly DependencyProperty CommandParameterProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.CommandTarget" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.CommandTarget" /> dependency property.</returns>
		// Token: 0x04002FA4 RID: 12196
		[CommonDependencyProperty]
		public static readonly DependencyProperty CommandTargetProperty;

		// Token: 0x04002FA5 RID: 12197
		internal static readonly DependencyPropertyKey IsPressedPropertyKey;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.IsPressed" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.IsPressed" /> dependency property.</returns>
		// Token: 0x04002FA6 RID: 12198
		[CommonDependencyProperty]
		public static readonly DependencyProperty IsPressedProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.ClickMode" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.ClickMode" /> dependency property.</returns>
		// Token: 0x04002FA7 RID: 12199
		public static readonly DependencyProperty ClickModeProperty;
	}
}
