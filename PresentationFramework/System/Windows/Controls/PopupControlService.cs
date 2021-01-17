using System;
using System.Security;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Win32;

namespace System.Windows.Controls
{
	// Token: 0x02000512 RID: 1298
	internal sealed class PopupControlService
	{
		// Token: 0x060053D3 RID: 21459 RVA: 0x001738FB File Offset: 0x00171AFB
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal PopupControlService()
		{
			InputManager.Current.PostProcessInput += this.OnPostProcessInput;
			this._focusChangedEventHandler = new KeyboardFocusChangedEventHandler(this.OnFocusChanged);
		}

		// Token: 0x060053D4 RID: 21460 RVA: 0x0017392C File Offset: 0x00171B2C
		[SecurityCritical]
		private void OnPostProcessInput(object sender, ProcessInputEventArgs e)
		{
			if (e.StagingItem.Input.RoutedEvent == InputManager.InputReportEvent)
			{
				InputReportEventArgs inputReportEventArgs = (InputReportEventArgs)e.StagingItem.Input;
				if (!inputReportEventArgs.Handled && inputReportEventArgs.Report.Type == InputType.Mouse)
				{
					RawMouseInputReport rawMouseInputReport = (RawMouseInputReport)inputReportEventArgs.Report;
					if ((rawMouseInputReport.Actions & RawMouseActions.AbsoluteMove) == RawMouseActions.AbsoluteMove)
					{
						if (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
						{
							this.RaiseToolTipClosingEvent(true);
							return;
						}
						IInputElement inputElement = Mouse.PrimaryDevice.RawDirectlyOver;
						if (inputElement != null)
						{
							Point position = Mouse.PrimaryDevice.GetPosition(inputElement);
							if (Mouse.CapturedMode != CaptureMode.None)
							{
								PresentationSource presentationSource = PresentationSource.CriticalFromVisual((DependencyObject)inputElement);
								UIElement uielement = (presentationSource != null) ? (presentationSource.RootVisual as UIElement) : null;
								if (uielement != null)
								{
									position = Mouse.PrimaryDevice.GetPosition(uielement);
									IInputElement inputElement2;
									uielement.InputHitTest(position, out inputElement2, out inputElement);
									position = Mouse.PrimaryDevice.GetPosition(inputElement);
								}
								else
								{
									inputElement = null;
								}
							}
							if (inputElement != null)
							{
								this.OnMouseMove(inputElement, position);
								return;
							}
						}
					}
					else if ((rawMouseInputReport.Actions & RawMouseActions.Deactivate) == RawMouseActions.Deactivate && this.LastMouseDirectlyOver != null)
					{
						this.LastMouseDirectlyOver = null;
						if (this.LastMouseOverWithToolTip != null)
						{
							this.RaiseToolTipClosingEvent(true);
							if (SafeNativeMethods.GetCapture() == IntPtr.Zero)
							{
								this.LastMouseOverWithToolTip = null;
								return;
							}
						}
					}
				}
			}
			else
			{
				if (e.StagingItem.Input.RoutedEvent == Keyboard.KeyDownEvent)
				{
					this.ProcessKeyDown(sender, (KeyEventArgs)e.StagingItem.Input);
					return;
				}
				if (e.StagingItem.Input.RoutedEvent == Keyboard.KeyUpEvent)
				{
					this.ProcessKeyUp(sender, (KeyEventArgs)e.StagingItem.Input);
					return;
				}
				if (e.StagingItem.Input.RoutedEvent == Mouse.MouseUpEvent)
				{
					this.ProcessMouseUp(sender, (MouseButtonEventArgs)e.StagingItem.Input);
					return;
				}
				if (e.StagingItem.Input.RoutedEvent == Mouse.MouseDownEvent)
				{
					this.RaiseToolTipClosingEvent(true);
				}
			}
		}

		// Token: 0x060053D5 RID: 21461 RVA: 0x00173B2F File Offset: 0x00171D2F
		private void OnMouseMove(IInputElement directlyOver, Point pt)
		{
			if (directlyOver != this.LastMouseDirectlyOver)
			{
				this.LastMouseDirectlyOver = directlyOver;
				if (directlyOver != this.LastMouseOverWithToolTip)
				{
					this.InspectElementForToolTip(directlyOver as DependencyObject, ToolTip.ToolTipTrigger.Mouse);
				}
			}
		}

		// Token: 0x060053D6 RID: 21462 RVA: 0x00173B58 File Offset: 0x00171D58
		private void OnFocusChanged(object sender, KeyboardFocusChangedEventArgs e)
		{
			IInputElement newFocus = e.NewFocus;
			if (newFocus != null)
			{
				this.InspectElementForToolTip(newFocus as DependencyObject, ToolTip.ToolTipTrigger.KeyboardFocus);
			}
		}

		// Token: 0x060053D7 RID: 21463 RVA: 0x00173B80 File Offset: 0x00171D80
		[SecurityCritical]
		private void ProcessMouseUp(object sender, MouseButtonEventArgs e)
		{
			this.RaiseToolTipClosingEvent(false);
			if (!e.Handled && e.ChangedButton == MouseButton.Right && e.RightButton == MouseButtonState.Released)
			{
				IInputElement rawDirectlyOver = Mouse.PrimaryDevice.RawDirectlyOver;
				if (rawDirectlyOver != null)
				{
					Point position = Mouse.PrimaryDevice.GetPosition(rawDirectlyOver);
					if (this.RaiseContextMenuOpeningEvent(rawDirectlyOver, position.X, position.Y, e.UserInitiated))
					{
						e.Handled = true;
					}
				}
			}
		}

		// Token: 0x060053D8 RID: 21464 RVA: 0x00173BEC File Offset: 0x00171DEC
		[SecurityCritical]
		private void ProcessKeyDown(object sender, KeyEventArgs e)
		{
			if (!e.Handled)
			{
				if (!AccessibilitySwitches.UseLegacyToolTipDisplay && e.SystemKey == Key.F10 && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
				{
					e.Handled = this.OpenOrCloseToolTipViaShortcut();
					return;
				}
				if (e.SystemKey == Key.F10 && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
				{
					this.RaiseContextMenuOpeningEvent(e);
				}
			}
		}

		// Token: 0x060053D9 RID: 21465 RVA: 0x00173C50 File Offset: 0x00171E50
		public bool OpenOrCloseToolTipViaShortcut()
		{
			bool result = false;
			if (this._lastToolTipOpen)
			{
				this.RaiseToolTipClosingEvent(true);
				this.LastObjectWithToolTip = null;
				result = true;
			}
			else
			{
				IInputElement focusedElement = Keyboard.FocusedElement;
				if (focusedElement != null)
				{
					result = this.InspectElementForToolTip(focusedElement as DependencyObject, ToolTip.ToolTipTrigger.KeyboardShortcut);
				}
			}
			return result;
		}

		// Token: 0x060053DA RID: 21466 RVA: 0x00173C91 File Offset: 0x00171E91
		[SecurityCritical]
		private void ProcessKeyUp(object sender, KeyEventArgs e)
		{
			if (!e.Handled && e.Key == Key.Apps)
			{
				this.RaiseContextMenuOpeningEvent(e);
			}
		}

		// Token: 0x060053DB RID: 21467 RVA: 0x00173CAC File Offset: 0x00171EAC
		private bool InspectElementForToolTip(DependencyObject o, ToolTip.ToolTipTrigger triggerAction)
		{
			DependencyObject lastChecked = o;
			bool flag = false;
			bool fromKeyboard = triggerAction == ToolTip.ToolTipTrigger.KeyboardFocus || triggerAction == ToolTip.ToolTipTrigger.KeyboardShortcut;
			bool result = this.LocateNearestToolTip(ref o, triggerAction, ref flag);
			if (flag)
			{
				if (o != null)
				{
					if (this.LastObjectWithToolTip != null)
					{
						this.RaiseToolTipClosingEvent(true);
						this.LastMouseOverWithToolTip = null;
					}
					this.LastChecked = lastChecked;
					this.LastObjectWithToolTip = o;
					if (!fromKeyboard)
					{
						this.LastMouseOverWithToolTip = o;
					}
					bool flag2 = !fromKeyboard && this._quickShow;
					this.ResetToolTipTimer();
					if (flag2)
					{
						this._quickShow = false;
						this.RaiseToolTipOpeningEvent(fromKeyboard);
					}
					else
					{
						this.ToolTipTimer = new DispatcherTimer(DispatcherPriority.Normal);
						this.ToolTipTimer.Interval = TimeSpan.FromMilliseconds((double)ToolTipService.GetInitialShowDelay(o));
						this.ToolTipTimer.Tag = BooleanBoxes.TrueBox;
						this.ToolTipTimer.Tick += delegate(object s, EventArgs e)
						{
							this.RaiseToolTipOpeningEvent(fromKeyboard);
						};
						this.ToolTipTimer.Start();
					}
				}
			}
			else if (this.LastMouseOverWithToolTip == null || triggerAction != ToolTip.ToolTipTrigger.KeyboardFocus)
			{
				this.RaiseToolTipClosingEvent(true);
				if (triggerAction == ToolTip.ToolTipTrigger.Mouse)
				{
					this.LastMouseOverWithToolTip = null;
				}
				this.LastObjectWithToolTip = null;
			}
			return result;
		}

		// Token: 0x060053DC RID: 21468 RVA: 0x00173DD8 File Offset: 0x00171FD8
		private bool LocateNearestToolTip(ref DependencyObject o, ToolTip.ToolTipTrigger triggerAction, ref bool showToolTip)
		{
			IInputElement inputElement = o as IInputElement;
			bool result = false;
			showToolTip = false;
			if (inputElement != null)
			{
				FindToolTipEventArgs findToolTipEventArgs = new FindToolTipEventArgs(triggerAction);
				inputElement.RaiseEvent(findToolTipEventArgs);
				result = findToolTipEventArgs.Handled;
				if (findToolTipEventArgs.TargetElement != null)
				{
					o = findToolTipEventArgs.TargetElement;
					showToolTip = true;
				}
				else if (findToolTipEventArgs.KeepCurrentActive)
				{
					o = null;
					showToolTip = true;
				}
			}
			return result;
		}

		// Token: 0x060053DD RID: 21469 RVA: 0x00173E2E File Offset: 0x0017202E
		internal bool StopLookingForToolTip(DependencyObject o)
		{
			return o == this.LastChecked || o == this.LastMouseOverWithToolTip || o == this._currentToolTip || this.WithinCurrentToolTip(o);
		}

		// Token: 0x060053DE RID: 21470 RVA: 0x00173E58 File Offset: 0x00172058
		private bool WithinCurrentToolTip(DependencyObject o)
		{
			if (this._currentToolTip == null)
			{
				return false;
			}
			DependencyObject dependencyObject = o as Visual;
			if (dependencyObject == null)
			{
				ContentElement contentElement = o as ContentElement;
				if (contentElement != null)
				{
					dependencyObject = PopupControlService.FindContentElementParent(contentElement);
				}
				else
				{
					dependencyObject = (o as Visual3D);
				}
			}
			return dependencyObject != null && ((dependencyObject is Visual && ((Visual)dependencyObject).IsDescendantOf(this._currentToolTip)) || (dependencyObject is Visual3D && ((Visual3D)dependencyObject).IsDescendantOf(this._currentToolTip)));
		}

		// Token: 0x060053DF RID: 21471 RVA: 0x00173ED0 File Offset: 0x001720D0
		private void ResetToolTipTimer()
		{
			if (this._toolTipTimer != null)
			{
				this._toolTipTimer.Stop();
				this._toolTipTimer = null;
				this._quickShow = false;
			}
		}

		// Token: 0x060053E0 RID: 21472 RVA: 0x00173EF3 File Offset: 0x001720F3
		internal void OnRaiseToolTipOpeningEvent(object sender, EventArgs e)
		{
			this.RaiseToolTipOpeningEvent(false);
		}

		// Token: 0x060053E1 RID: 21473 RVA: 0x00173EFC File Offset: 0x001720FC
		private void RaiseToolTipOpeningEvent(bool fromKeyboard = false)
		{
			this.ResetToolTipTimer();
			if (this._forceCloseTimer != null)
			{
				this.OnForceClose(null, EventArgs.Empty);
			}
			DependencyObject lastObjectWithToolTip = this.LastObjectWithToolTip;
			if (lastObjectWithToolTip != null)
			{
				bool flag = true;
				IInputElement inputElement = lastObjectWithToolTip as IInputElement;
				if (inputElement != null)
				{
					ToolTipEventArgs toolTipEventArgs = new ToolTipEventArgs(true);
					inputElement.RaiseEvent(toolTipEventArgs);
					flag = !toolTipEventArgs.Handled;
				}
				if (flag)
				{
					object toolTip = ToolTipService.GetToolTip(lastObjectWithToolTip);
					ToolTip toolTip2 = toolTip as ToolTip;
					if (toolTip2 != null)
					{
						this._currentToolTip = toolTip2;
						this._ownToolTip = false;
					}
					else if (this._currentToolTip == null || !this._ownToolTip)
					{
						this._currentToolTip = new ToolTip();
						this._ownToolTip = true;
						this._currentToolTip.SetValue(PopupControlService.ServiceOwnedProperty, BooleanBoxes.TrueBox);
						Binding binding = new Binding();
						binding.Path = new PropertyPath(ToolTipService.ToolTipProperty);
						binding.Mode = BindingMode.OneWay;
						binding.Source = lastObjectWithToolTip;
						this._currentToolTip.SetBinding(ContentControl.ContentProperty, binding);
					}
					if (!this._currentToolTip.StaysOpen)
					{
						throw new NotSupportedException(SR.Get("ToolTipStaysOpenFalseNotAllowed"));
					}
					this._currentToolTip.SetValue(PopupControlService.OwnerProperty, lastObjectWithToolTip);
					this._currentToolTip.Opened += new RoutedEventHandler(this.OnToolTipOpened);
					this._currentToolTip.Closed += new RoutedEventHandler(this.OnToolTipClosed);
					this._currentToolTip.FromKeyboard = fromKeyboard;
					this._currentToolTip.IsOpen = true;
					this.ToolTipTimer = new DispatcherTimer(DispatcherPriority.Normal);
					this.ToolTipTimer.Interval = TimeSpan.FromMilliseconds((double)ToolTipService.GetShowDuration(lastObjectWithToolTip));
					this.ToolTipTimer.Tick += this.OnRaiseToolTipClosingEvent;
					this.ToolTipTimer.Start();
				}
			}
		}

		// Token: 0x060053E2 RID: 21474 RVA: 0x001740AC File Offset: 0x001722AC
		internal void OnRaiseToolTipClosingEvent(object sender, EventArgs e)
		{
			this.RaiseToolTipClosingEvent(false);
		}

		// Token: 0x060053E3 RID: 21475 RVA: 0x001740B8 File Offset: 0x001722B8
		private void RaiseToolTipClosingEvent(bool reset)
		{
			this.ResetToolTipTimer();
			if (reset)
			{
				this.LastChecked = null;
			}
			DependencyObject lastObjectWithToolTip = this.LastObjectWithToolTip;
			if (lastObjectWithToolTip != null && this._currentToolTip != null)
			{
				bool isOpen = this._currentToolTip.IsOpen;
				try
				{
					if (isOpen)
					{
						IInputElement inputElement = lastObjectWithToolTip as IInputElement;
						if (inputElement != null)
						{
							inputElement.RaiseEvent(new ToolTipEventArgs(false));
						}
					}
				}
				finally
				{
					if (this._currentToolTip != null)
					{
						if (isOpen)
						{
							this._currentToolTip.IsOpen = false;
							if (this._currentToolTip != null)
							{
								this._forceCloseTimer = new DispatcherTimer(DispatcherPriority.Normal);
								this._forceCloseTimer.Interval = Popup.AnimationDelayTime;
								this._forceCloseTimer.Tick += this.OnForceClose;
								this._forceCloseTimer.Tag = this._currentToolTip;
								this._forceCloseTimer.Start();
							}
							this._quickShow = true;
							this.ToolTipTimer = new DispatcherTimer(DispatcherPriority.Normal);
							this.ToolTipTimer.Interval = TimeSpan.FromMilliseconds((double)ToolTipService.GetBetweenShowDelay(lastObjectWithToolTip));
							this.ToolTipTimer.Tick += this.OnBetweenShowDelay;
							this.ToolTipTimer.Start();
						}
						else
						{
							this._currentToolTip.ClearValue(PopupControlService.OwnerProperty);
							if (this._ownToolTip)
							{
								BindingOperations.ClearBinding(this._currentToolTip, ContentControl.ContentProperty);
							}
						}
						if (this._currentToolTip != null)
						{
							this._currentToolTip.FromKeyboard = false;
							this._currentToolTip = null;
						}
					}
				}
			}
		}

		// Token: 0x060053E4 RID: 21476 RVA: 0x0017423C File Offset: 0x0017243C
		private void OnToolTipOpened(object sender, EventArgs e)
		{
			ToolTip toolTip = (ToolTip)sender;
			toolTip.Opened -= new RoutedEventHandler(this.OnToolTipOpened);
			this._lastToolTipOpen = true;
		}

		// Token: 0x060053E5 RID: 21477 RVA: 0x0017426C File Offset: 0x0017246C
		private void OnToolTipClosed(object sender, EventArgs e)
		{
			ToolTip toolTip = (ToolTip)sender;
			toolTip.Closed -= new RoutedEventHandler(this.OnToolTipClosed);
			toolTip.ClearValue(PopupControlService.OwnerProperty);
			this._lastToolTipOpen = false;
			if ((bool)toolTip.GetValue(PopupControlService.ServiceOwnedProperty))
			{
				BindingOperations.ClearBinding(toolTip, ContentControl.ContentProperty);
			}
		}

		// Token: 0x060053E6 RID: 21478 RVA: 0x001742C4 File Offset: 0x001724C4
		private void OnForceClose(object sender, EventArgs e)
		{
			this._forceCloseTimer.Stop();
			ToolTip toolTip = (ToolTip)this._forceCloseTimer.Tag;
			toolTip.ForceClose();
			this._forceCloseTimer = null;
		}

		// Token: 0x060053E7 RID: 21479 RVA: 0x001742FA File Offset: 0x001724FA
		private void OnBetweenShowDelay(object source, EventArgs e)
		{
			this.ResetToolTipTimer();
		}

		// Token: 0x1700145E RID: 5214
		// (get) Token: 0x060053E8 RID: 21480 RVA: 0x00174304 File Offset: 0x00172504
		// (set) Token: 0x060053E9 RID: 21481 RVA: 0x00174337 File Offset: 0x00172537
		private IInputElement LastMouseDirectlyOver
		{
			get
			{
				if (this._lastMouseDirectlyOver != null)
				{
					IInputElement inputElement = (IInputElement)this._lastMouseDirectlyOver.Target;
					if (inputElement != null)
					{
						return inputElement;
					}
					this._lastMouseDirectlyOver = null;
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this._lastMouseDirectlyOver = null;
					return;
				}
				if (this._lastMouseDirectlyOver == null)
				{
					this._lastMouseDirectlyOver = new WeakReference(value);
					return;
				}
				this._lastMouseDirectlyOver.Target = value;
			}
		}

		// Token: 0x1700145F RID: 5215
		// (get) Token: 0x060053EA RID: 21482 RVA: 0x00174368 File Offset: 0x00172568
		// (set) Token: 0x060053EB RID: 21483 RVA: 0x0017439B File Offset: 0x0017259B
		private DependencyObject LastMouseOverWithToolTip
		{
			get
			{
				if (this._lastMouseOverWithToolTip != null)
				{
					DependencyObject dependencyObject = (DependencyObject)this._lastMouseOverWithToolTip.Target;
					if (dependencyObject != null)
					{
						return dependencyObject;
					}
					this._lastMouseOverWithToolTip = null;
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this._lastMouseOverWithToolTip = null;
					return;
				}
				if (this._lastMouseOverWithToolTip == null)
				{
					this._lastMouseOverWithToolTip = new WeakReference(value);
					return;
				}
				this._lastMouseOverWithToolTip.Target = value;
			}
		}

		// Token: 0x17001460 RID: 5216
		// (get) Token: 0x060053EC RID: 21484 RVA: 0x001743CC File Offset: 0x001725CC
		// (set) Token: 0x060053ED RID: 21485 RVA: 0x001743FF File Offset: 0x001725FF
		private DependencyObject LastObjectWithToolTip
		{
			get
			{
				if (this._lastObjectWithToolTip != null)
				{
					DependencyObject dependencyObject = (DependencyObject)this._lastObjectWithToolTip.Target;
					if (dependencyObject != null)
					{
						return dependencyObject;
					}
					this._lastObjectWithToolTip = null;
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this._lastObjectWithToolTip = null;
					return;
				}
				if (this._lastObjectWithToolTip == null)
				{
					this._lastObjectWithToolTip = new WeakReference(value);
					return;
				}
				this._lastObjectWithToolTip.Target = value;
			}
		}

		// Token: 0x17001461 RID: 5217
		// (get) Token: 0x060053EE RID: 21486 RVA: 0x00174430 File Offset: 0x00172630
		// (set) Token: 0x060053EF RID: 21487 RVA: 0x00174463 File Offset: 0x00172663
		private DependencyObject LastChecked
		{
			get
			{
				if (this._lastChecked != null)
				{
					DependencyObject dependencyObject = (DependencyObject)this._lastChecked.Target;
					if (dependencyObject != null)
					{
						return dependencyObject;
					}
					this._lastChecked = null;
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this._lastChecked = null;
					return;
				}
				if (this._lastChecked == null)
				{
					this._lastChecked = new WeakReference(value);
					return;
				}
				this._lastChecked.Target = value;
			}
		}

		// Token: 0x060053F0 RID: 21488 RVA: 0x00174494 File Offset: 0x00172694
		[SecurityCritical]
		private void RaiseContextMenuOpeningEvent(KeyEventArgs e)
		{
			IInputElement inputElement = e.OriginalSource as IInputElement;
			if (inputElement != null && this.RaiseContextMenuOpeningEvent(inputElement, -1.0, -1.0, e.UserInitiated))
			{
				e.Handled = true;
			}
		}

		// Token: 0x060053F1 RID: 21489 RVA: 0x001744D8 File Offset: 0x001726D8
		[SecurityCritical]
		private bool RaiseContextMenuOpeningEvent(IInputElement source, double x, double y, bool userInitiated)
		{
			ContextMenuEventArgs contextMenuEventArgs = new ContextMenuEventArgs(source, true, x, y);
			DependencyObject dependencyObject = source as DependencyObject;
			if (userInitiated && dependencyObject != null)
			{
				if (InputElement.IsUIElement(dependencyObject))
				{
					((UIElement)dependencyObject).RaiseEvent(contextMenuEventArgs, userInitiated);
				}
				else if (InputElement.IsContentElement(dependencyObject))
				{
					((ContentElement)dependencyObject).RaiseEvent(contextMenuEventArgs, userInitiated);
				}
				else if (InputElement.IsUIElement3D(dependencyObject))
				{
					((UIElement3D)dependencyObject).RaiseEvent(contextMenuEventArgs, userInitiated);
				}
				else
				{
					source.RaiseEvent(contextMenuEventArgs);
				}
			}
			else
			{
				source.RaiseEvent(contextMenuEventArgs);
			}
			if (contextMenuEventArgs.Handled)
			{
				this.RaiseToolTipClosingEvent(true);
				return true;
			}
			DependencyObject targetElement = contextMenuEventArgs.TargetElement;
			if (targetElement != null && ContextMenuService.ContextMenuIsEnabled(targetElement))
			{
				object contextMenu = ContextMenuService.GetContextMenu(targetElement);
				ContextMenu contextMenu2 = contextMenu as ContextMenu;
				contextMenu2.SetValue(PopupControlService.OwnerProperty, targetElement);
				contextMenu2.Closed += this.OnContextMenuClosed;
				if (x == -1.0 && y == -1.0)
				{
					contextMenu2.Placement = PlacementMode.Center;
				}
				else
				{
					contextMenu2.Placement = PlacementMode.MousePoint;
				}
				this.RaiseToolTipClosingEvent(true);
				contextMenu2.SetCurrentValueInternal(ContextMenu.IsOpenProperty, BooleanBoxes.TrueBox);
				return true;
			}
			return false;
		}

		// Token: 0x060053F2 RID: 21490 RVA: 0x001745F0 File Offset: 0x001727F0
		private void OnContextMenuClosed(object source, RoutedEventArgs e)
		{
			ContextMenu contextMenu = source as ContextMenu;
			if (contextMenu != null)
			{
				contextMenu.Closed -= this.OnContextMenuClosed;
				DependencyObject dependencyObject = (DependencyObject)contextMenu.GetValue(PopupControlService.OwnerProperty);
				if (dependencyObject != null)
				{
					contextMenu.ClearValue(PopupControlService.OwnerProperty);
					UIElement target = PopupControlService.GetTarget(dependencyObject);
					if (target != null && !PopupControlService.IsPresentationSourceNull(target))
					{
						IInputElement inputElement2;
						if (!(dependencyObject is ContentElement) && !(dependencyObject is UIElement3D))
						{
							IInputElement inputElement = target;
							inputElement2 = inputElement;
						}
						else
						{
							inputElement2 = (IInputElement)dependencyObject;
						}
						IInputElement inputElement3 = inputElement2;
						ContextMenuEventArgs e2 = new ContextMenuEventArgs(inputElement3, false);
						inputElement3.RaiseEvent(e2);
					}
				}
			}
		}

		// Token: 0x060053F3 RID: 21491 RVA: 0x00174679 File Offset: 0x00172879
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static bool IsPresentationSourceNull(DependencyObject uie)
		{
			return PresentationSource.CriticalFromVisual(uie) == null;
		}

		// Token: 0x060053F4 RID: 21492 RVA: 0x00174684 File Offset: 0x00172884
		internal static DependencyObject FindParent(DependencyObject o)
		{
			DependencyObject dependencyObject = o as Visual;
			if (dependencyObject == null)
			{
				dependencyObject = (o as Visual3D);
			}
			ContentElement contentElement = (dependencyObject == null) ? (o as ContentElement) : null;
			if (contentElement != null)
			{
				o = ContentOperations.GetParent(contentElement);
				if (o != null)
				{
					return o;
				}
				FrameworkContentElement frameworkContentElement = contentElement as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					return frameworkContentElement.Parent;
				}
			}
			else if (dependencyObject != null)
			{
				return VisualTreeHelper.GetParent(dependencyObject);
			}
			return null;
		}

		// Token: 0x060053F5 RID: 21493 RVA: 0x001746DC File Offset: 0x001728DC
		internal static DependencyObject FindContentElementParent(ContentElement ce)
		{
			DependencyObject dependencyObject = null;
			DependencyObject dependencyObject2 = ce;
			while (dependencyObject2 != null)
			{
				dependencyObject = (dependencyObject2 as Visual);
				if (dependencyObject != null)
				{
					break;
				}
				dependencyObject = (dependencyObject2 as Visual3D);
				if (dependencyObject != null)
				{
					break;
				}
				ce = (dependencyObject2 as ContentElement);
				if (ce == null)
				{
					break;
				}
				dependencyObject2 = ContentOperations.GetParent(ce);
				if (dependencyObject2 == null)
				{
					FrameworkContentElement frameworkContentElement = ce as FrameworkContentElement;
					if (frameworkContentElement != null)
					{
						dependencyObject2 = frameworkContentElement.Parent;
					}
				}
			}
			return dependencyObject;
		}

		// Token: 0x060053F6 RID: 21494 RVA: 0x00174730 File Offset: 0x00172930
		internal static bool IsElementEnabled(DependencyObject o)
		{
			bool result = true;
			UIElement uielement = o as UIElement;
			ContentElement contentElement = (uielement == null) ? (o as ContentElement) : null;
			UIElement3D uielement3D = (uielement == null && contentElement == null) ? (o as UIElement3D) : null;
			if (uielement != null)
			{
				result = uielement.IsEnabled;
			}
			else if (contentElement != null)
			{
				result = contentElement.IsEnabled;
			}
			else if (uielement3D != null)
			{
				result = uielement3D.IsEnabled;
			}
			return result;
		}

		// Token: 0x17001462 RID: 5218
		// (get) Token: 0x060053F7 RID: 21495 RVA: 0x00174786 File Offset: 0x00172986
		internal static PopupControlService Current
		{
			get
			{
				return FrameworkElement.PopupControlService;
			}
		}

		// Token: 0x17001463 RID: 5219
		// (get) Token: 0x060053F8 RID: 21496 RVA: 0x0017478D File Offset: 0x0017298D
		internal ToolTip CurrentToolTip
		{
			get
			{
				return this._currentToolTip;
			}
		}

		// Token: 0x17001464 RID: 5220
		// (get) Token: 0x060053F9 RID: 21497 RVA: 0x00174795 File Offset: 0x00172995
		// (set) Token: 0x060053FA RID: 21498 RVA: 0x0017479D File Offset: 0x0017299D
		private DispatcherTimer ToolTipTimer
		{
			get
			{
				return this._toolTipTimer;
			}
			set
			{
				this.ResetToolTipTimer();
				this._toolTipTimer = value;
			}
		}

		// Token: 0x060053FB RID: 21499 RVA: 0x001747AC File Offset: 0x001729AC
		private static UIElement GetTarget(DependencyObject o)
		{
			UIElement uielement = o as UIElement;
			if (uielement == null)
			{
				ContentElement contentElement = o as ContentElement;
				if (contentElement != null)
				{
					DependencyObject dependencyObject = PopupControlService.FindContentElementParent(contentElement);
					uielement = (dependencyObject as UIElement);
					if (uielement == null)
					{
						UIElement3D uielement3D = dependencyObject as UIElement3D;
						if (uielement3D != null)
						{
							uielement = UIElementHelper.GetContainingUIElement2D(uielement3D);
						}
					}
				}
				else
				{
					UIElement3D uielement3D2 = o as UIElement3D;
					if (uielement3D2 != null)
					{
						uielement = UIElementHelper.GetContainingUIElement2D(uielement3D2);
					}
				}
			}
			return uielement;
		}

		// Token: 0x060053FC RID: 21500 RVA: 0x00174808 File Offset: 0x00172A08
		private static void OnOwnerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			if (o is ContextMenu)
			{
				o.CoerceValue(ContextMenu.HorizontalOffsetProperty);
				o.CoerceValue(ContextMenu.VerticalOffsetProperty);
				o.CoerceValue(ContextMenu.PlacementTargetProperty);
				o.CoerceValue(ContextMenu.PlacementRectangleProperty);
				o.CoerceValue(ContextMenu.PlacementProperty);
				o.CoerceValue(ContextMenu.HasDropShadowProperty);
				return;
			}
			if (o is ToolTip)
			{
				o.CoerceValue(ToolTip.HorizontalOffsetProperty);
				o.CoerceValue(ToolTip.VerticalOffsetProperty);
				o.CoerceValue(ToolTip.PlacementTargetProperty);
				o.CoerceValue(ToolTip.PlacementRectangleProperty);
				o.CoerceValue(ToolTip.PlacementProperty);
				o.CoerceValue(ToolTip.HasDropShadowProperty);
			}
		}

		// Token: 0x060053FD RID: 21501 RVA: 0x001748AC File Offset: 0x00172AAC
		internal static object CoerceProperty(DependencyObject o, object value, DependencyProperty dp)
		{
			DependencyObject dependencyObject = (DependencyObject)o.GetValue(PopupControlService.OwnerProperty);
			if (dependencyObject != null)
			{
				bool flag;
				if (dependencyObject.GetValueSource(dp, null, out flag) != BaseValueSourceInternal.Default || flag)
				{
					return dependencyObject.GetValue(dp);
				}
				if (dp == ToolTip.PlacementTargetProperty || dp == ContextMenu.PlacementTargetProperty)
				{
					UIElement target = PopupControlService.GetTarget(dependencyObject);
					if (target != null)
					{
						return target;
					}
				}
			}
			return value;
		}

		// Token: 0x17001465 RID: 5221
		// (get) Token: 0x060053FE RID: 21502 RVA: 0x00174906 File Offset: 0x00172B06
		internal KeyboardFocusChangedEventHandler FocusChangedEventHandler
		{
			get
			{
				return this._focusChangedEventHandler;
			}
		}

		// Token: 0x04002D0C RID: 11532
		internal static readonly RoutedEvent ContextMenuOpenedEvent = EventManager.RegisterRoutedEvent("Opened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PopupControlService));

		// Token: 0x04002D0D RID: 11533
		internal static readonly RoutedEvent ContextMenuClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PopupControlService));

		// Token: 0x04002D0E RID: 11534
		internal static readonly DependencyProperty ServiceOwnedProperty = DependencyProperty.RegisterAttached("ServiceOwned", typeof(bool), typeof(PopupControlService), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04002D0F RID: 11535
		internal static readonly DependencyProperty OwnerProperty = DependencyProperty.RegisterAttached("Owner", typeof(DependencyObject), typeof(PopupControlService), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(PopupControlService.OnOwnerChanged)));

		// Token: 0x04002D10 RID: 11536
		private DispatcherTimer _toolTipTimer;

		// Token: 0x04002D11 RID: 11537
		private bool _quickShow;

		// Token: 0x04002D12 RID: 11538
		private WeakReference _lastMouseDirectlyOver;

		// Token: 0x04002D13 RID: 11539
		private WeakReference _lastMouseOverWithToolTip;

		// Token: 0x04002D14 RID: 11540
		private WeakReference _lastObjectWithToolTip;

		// Token: 0x04002D15 RID: 11541
		private WeakReference _lastChecked;

		// Token: 0x04002D16 RID: 11542
		private bool _lastToolTipOpen;

		// Token: 0x04002D17 RID: 11543
		private ToolTip _currentToolTip;

		// Token: 0x04002D18 RID: 11544
		private DispatcherTimer _forceCloseTimer;

		// Token: 0x04002D19 RID: 11545
		private bool _ownToolTip;

		// Token: 0x04002D1A RID: 11546
		private KeyboardFocusChangedEventHandler _focusChangedEventHandler;
	}
}
