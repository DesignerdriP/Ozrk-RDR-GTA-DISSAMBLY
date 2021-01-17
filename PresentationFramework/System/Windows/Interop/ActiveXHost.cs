using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using MS.Internal;
using MS.Internal.Controls;
using MS.Win32;

namespace System.Windows.Interop
{
	/// <summary>Hosts an ActiveX control as an element within Windows Presentation Foundation (WPF) content. </summary>
	// Token: 0x020005B9 RID: 1465
	public class ActiveXHost : HwndHost
	{
		// Token: 0x06006165 RID: 24933 RVA: 0x001B54F0 File Offset: 0x001B36F0
		static ActiveXHost()
		{
			ActiveXHost.invalidatorMap[UIElement.VisibilityProperty] = new ActiveXHost.PropertyInvalidator(ActiveXHost.OnVisibilityInvalidated);
			ActiveXHost.invalidatorMap[UIElement.IsEnabledProperty] = new ActiveXHost.PropertyInvalidator(ActiveXHost.OnIsEnabledInvalidated);
			EventManager.RegisterClassHandler(typeof(ActiveXHost), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(ActiveXHost.OnAccessKeyPressed));
			Control.IsTabStopProperty.OverrideMetadata(typeof(ActiveXHost), new FrameworkPropertyMetadata(true));
			UIElement.FocusableProperty.OverrideMetadata(typeof(ActiveXHost), new FrameworkPropertyMetadata(true));
			EventManager.RegisterClassHandler(typeof(ActiveXHost), Keyboard.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(ActiveXHost.OnGotFocus));
			EventManager.RegisterClassHandler(typeof(ActiveXHost), Keyboard.LostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(ActiveXHost.OnLostFocus));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ActiveXHost), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
		}

		// Token: 0x06006166 RID: 24934 RVA: 0x001B5614 File Offset: 0x001B3814
		[SecurityCritical]
		internal ActiveXHost(Guid clsid, bool fTrusted) : base(fTrusted)
		{
			if (Thread.CurrentThread.ApartmentState != ApartmentState.STA)
			{
				throw new ThreadStateException(SR.Get("AxRequiresApartmentThread", new object[]
				{
					clsid.ToString()
				}));
			}
			this._clsid.Value = clsid;
			base.Initialized += this.OnInitialized;
		}

		/// <summary>Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.FrameworkElement" /> has been updated. The specific dependency property that changed is reported in the arguments parameter. Overrides <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)" />.</summary>
		/// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
		// Token: 0x06006167 RID: 24935 RVA: 0x001B56C4 File Offset: 0x001B38C4
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.IsAValueChange || e.IsASubPropertyChange)
			{
				DependencyProperty property = e.Property;
				if (property != null && ActiveXHost.invalidatorMap.ContainsKey(property))
				{
					ActiveXHost.PropertyInvalidator propertyInvalidator = (ActiveXHost.PropertyInvalidator)ActiveXHost.invalidatorMap[property];
					propertyInvalidator(this);
				}
			}
		}

		/// <summary>Creates the <see cref="T:System.Windows.Interop.ActiveXHost" /> window and assigns it to a parent.</summary>
		/// <param name="hwndParent">The parent window.</param>
		/// <returns>A <see cref="T:System.Runtime.InteropServices.HandleRef" /> to the <see cref="T:System.Windows.Interop.ActiveXHost" /> window.</returns>
		// Token: 0x06006168 RID: 24936 RVA: 0x001B571C File Offset: 0x001B391C
		[SecurityCritical]
		protected override HandleRef BuildWindowCore(HandleRef hwndParent)
		{
			this.ParentHandle = hwndParent;
			this.TransitionUpTo(ActiveXHelper.ActiveXState.InPlaceActive);
			Invariant.Assert(this._axOleInPlaceActiveObject != null, "InPlace activation of ActiveX control failed");
			if (this.ControlHandle.Handle == IntPtr.Zero)
			{
				IntPtr zero = IntPtr.Zero;
				this._axOleInPlaceActiveObject.GetWindow(out zero);
				this.AttachWindow(zero);
			}
			return this._axWindow;
		}

		/// <summary>Called when the hosted window's position changes. </summary>
		/// <param name="bounds">The window's position.</param>
		// Token: 0x06006169 RID: 24937 RVA: 0x001B5788 File Offset: 0x001B3988
		[SecurityCritical]
		protected override void OnWindowPositionChanged(Rect bounds)
		{
			this._boundRect = bounds;
			this._bounds.left = (int)bounds.X;
			this._bounds.top = (int)bounds.Y;
			this._bounds.right = (int)(bounds.Width + bounds.X);
			this._bounds.bottom = (int)(bounds.Height + bounds.Y);
			this.ActiveXSite.OnActiveXRectChange(this._bounds);
		}

		/// <summary>Destroys the hosted window.</summary>
		/// <param name="hwnd">A structure that contains the window handle.</param>
		// Token: 0x0600616A RID: 24938 RVA: 0x00002137 File Offset: 0x00000337
		protected override void DestroyWindowCore(HandleRef hwnd)
		{
		}

		/// <summary>Returns the size of the window represented by the <see cref="T:System.Windows.Interop.HwndHost" /> object, as requested by layout engine operations. </summary>
		/// <param name="swConstraint">The size of the <see cref="T:System.Windows.Interop.HwndHost" /> object.</param>
		/// <returns>The size of the <see cref="T:System.Windows.Interop.HwndHost" /> object.</returns>
		// Token: 0x0600616B RID: 24939 RVA: 0x001B580C File Offset: 0x001B3A0C
		protected override Size MeasureOverride(Size swConstraint)
		{
			base.MeasureOverride(swConstraint);
			double width;
			if (double.IsPositiveInfinity(swConstraint.Width))
			{
				width = 150.0;
			}
			else
			{
				width = swConstraint.Width;
			}
			double height;
			if (double.IsPositiveInfinity(swConstraint.Height))
			{
				height = 150.0;
			}
			else
			{
				height = swConstraint.Height;
			}
			return new Size(width, height);
		}

		/// <summary>Provides class handling for when an access key that is meaningful for this element is invoked. </summary>
		/// <param name="args">The event data to the access key event. The event data reports which key was invoked, and indicate whether the <see cref="T:System.Windows.Input.AccessKeyManager" /> object that controls the sending of these events also sent this access key invocation to other elements.</param>
		// Token: 0x0600616C RID: 24940 RVA: 0x00002137 File Offset: 0x00000337
		protected override void OnAccessKey(AccessKeyEventArgs args)
		{
		}

		/// <summary>Releases the unmanaged resources that are used by the <see cref="T:System.Windows.Interop.ActiveXHost" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x0600616D RID: 24941 RVA: 0x001B586C File Offset: 0x001B3A6C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && !this._isDisposed)
				{
					this.TransitionDownTo(ActiveXHelper.ActiveXState.Passive);
					this._isDisposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x0600616E RID: 24942 RVA: 0x001B58AC File Offset: 0x001B3AAC
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		internal virtual ActiveXSite CreateActiveXSite()
		{
			return new ActiveXSite(this);
		}

		// Token: 0x0600616F RID: 24943 RVA: 0x001B58B4 File Offset: 0x001B3AB4
		[SecurityCritical]
		internal virtual object CreateActiveXObject(Guid clsid)
		{
			return Activator.CreateInstance(Type.GetTypeFromCLSID(clsid));
		}

		// Token: 0x06006170 RID: 24944 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void AttachInterfaces(object nativeActiveXObject)
		{
		}

		// Token: 0x06006171 RID: 24945 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void DetachInterfaces()
		{
		}

		// Token: 0x06006172 RID: 24946 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void CreateSink()
		{
		}

		// Token: 0x06006173 RID: 24947 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		internal virtual void DetachSink()
		{
		}

		// Token: 0x06006174 RID: 24948 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void OnActiveXStateChange(int oldState, int newState)
		{
		}

		/// <summary>Gets a value that indicates whether the <see cref="M:System.Windows.Interop.ActiveXHost.Dispose(System.Boolean)" /> method has been called on the <see cref="T:System.Windows.Interop.ActiveXHost" /> instance. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Interop.ActiveXHost" /> instance has been disposed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001773 RID: 6003
		// (get) Token: 0x06006175 RID: 24949 RVA: 0x001B58C1 File Offset: 0x001B3AC1
		protected bool IsDisposed
		{
			get
			{
				return this._isDisposed;
			}
		}

		// Token: 0x06006176 RID: 24950 RVA: 0x001B58C9 File Offset: 0x001B3AC9
		internal void RegisterAccessKey(char key)
		{
			AccessKeyManager.Register(key.ToString(), this);
		}

		// Token: 0x17001774 RID: 6004
		// (get) Token: 0x06006177 RID: 24951 RVA: 0x001B58D8 File Offset: 0x001B3AD8
		internal ActiveXSite ActiveXSite
		{
			[SecurityCritical]
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				if (this._axSite == null)
				{
					this._axSite = this.CreateActiveXSite();
				}
				return this._axSite;
			}
		}

		// Token: 0x17001775 RID: 6005
		// (get) Token: 0x06006178 RID: 24952 RVA: 0x001B58F4 File Offset: 0x001B3AF4
		internal ActiveXContainer Container
		{
			[SecurityCritical]
			get
			{
				if (this._axContainer == null)
				{
					this._axContainer = new ActiveXContainer(this);
				}
				return this._axContainer;
			}
		}

		// Token: 0x17001776 RID: 6006
		// (get) Token: 0x06006179 RID: 24953 RVA: 0x001B5910 File Offset: 0x001B3B10
		// (set) Token: 0x0600617A RID: 24954 RVA: 0x001B5918 File Offset: 0x001B3B18
		internal ActiveXHelper.ActiveXState ActiveXState
		{
			get
			{
				return this._axState;
			}
			set
			{
				this._axState = value;
			}
		}

		// Token: 0x0600617B RID: 24955 RVA: 0x001B5921 File Offset: 0x001B3B21
		internal bool GetAxHostState(int mask)
		{
			return this._axHostState[mask];
		}

		// Token: 0x0600617C RID: 24956 RVA: 0x001B592F File Offset: 0x001B3B2F
		internal void SetAxHostState(int mask, bool value)
		{
			this._axHostState[mask] = value;
		}

		// Token: 0x0600617D RID: 24957 RVA: 0x001B5940 File Offset: 0x001B3B40
		internal void TransitionUpTo(ActiveXHelper.ActiveXState state)
		{
			if (!this.GetAxHostState(ActiveXHelper.inTransition))
			{
				this.SetAxHostState(ActiveXHelper.inTransition, true);
				try
				{
					while (state > this.ActiveXState)
					{
						ActiveXHelper.ActiveXState activeXState = this.ActiveXState;
						switch (this.ActiveXState)
						{
						case ActiveXHelper.ActiveXState.Passive:
							this.TransitionFromPassiveToLoaded();
							this.ActiveXState = ActiveXHelper.ActiveXState.Loaded;
							break;
						case ActiveXHelper.ActiveXState.Loaded:
							this.TransitionFromLoadedToRunning();
							this.ActiveXState = ActiveXHelper.ActiveXState.Running;
							break;
						case ActiveXHelper.ActiveXState.Running:
							this.TransitionFromRunningToInPlaceActive();
							this.ActiveXState = ActiveXHelper.ActiveXState.InPlaceActive;
							break;
						case (ActiveXHelper.ActiveXState)3:
							goto IL_87;
						case ActiveXHelper.ActiveXState.InPlaceActive:
							this.TransitionFromInPlaceActiveToUIActive();
							this.ActiveXState = ActiveXHelper.ActiveXState.UIActive;
							break;
						default:
							goto IL_87;
						}
						IL_95:
						this.OnActiveXStateChange((int)activeXState, (int)this.ActiveXState);
						continue;
						IL_87:
						this.ActiveXState++;
						goto IL_95;
					}
				}
				finally
				{
					this.SetAxHostState(ActiveXHelper.inTransition, false);
				}
			}
		}

		// Token: 0x0600617E RID: 24958 RVA: 0x001B5A1C File Offset: 0x001B3C1C
		internal void TransitionDownTo(ActiveXHelper.ActiveXState state)
		{
			if (!this.GetAxHostState(ActiveXHelper.inTransition))
			{
				this.SetAxHostState(ActiveXHelper.inTransition, true);
				try
				{
					while (state < this.ActiveXState)
					{
						ActiveXHelper.ActiveXState activeXState = this.ActiveXState;
						ActiveXHelper.ActiveXState activeXState2 = this.ActiveXState;
						switch (activeXState2)
						{
						case ActiveXHelper.ActiveXState.Loaded:
							this.TransitionFromLoadedToPassive();
							this.ActiveXState = ActiveXHelper.ActiveXState.Passive;
							break;
						case ActiveXHelper.ActiveXState.Running:
							this.TransitionFromRunningToLoaded();
							this.ActiveXState = ActiveXHelper.ActiveXState.Loaded;
							break;
						case (ActiveXHelper.ActiveXState)3:
							goto IL_95;
						case ActiveXHelper.ActiveXState.InPlaceActive:
							this.TransitionFromInPlaceActiveToRunning();
							this.ActiveXState = ActiveXHelper.ActiveXState.Running;
							break;
						default:
							if (activeXState2 != ActiveXHelper.ActiveXState.UIActive)
							{
								if (activeXState2 != ActiveXHelper.ActiveXState.Open)
								{
									goto IL_95;
								}
								this.ActiveXState = ActiveXHelper.ActiveXState.UIActive;
							}
							else
							{
								this.TransitionFromUIActiveToInPlaceActive();
								this.ActiveXState = ActiveXHelper.ActiveXState.InPlaceActive;
							}
							break;
						}
						IL_A3:
						this.OnActiveXStateChange((int)activeXState, (int)this.ActiveXState);
						continue;
						IL_95:
						this.ActiveXState--;
						goto IL_A3;
					}
				}
				finally
				{
					this.SetAxHostState(ActiveXHelper.inTransition, false);
				}
			}
		}

		// Token: 0x0600617F RID: 24959 RVA: 0x001B5B04 File Offset: 0x001B3D04
		[SecurityCritical]
		internal bool DoVerb(int verb)
		{
			int num = this._axOleObject.DoVerb(verb, IntPtr.Zero, this.ActiveXSite, 0, this.ParentHandle.Handle, this._bounds);
			return num == 0;
		}

		// Token: 0x06006180 RID: 24960 RVA: 0x001B5B44 File Offset: 0x001B3D44
		[SecurityCritical]
		internal void AttachWindow(IntPtr hwnd)
		{
			if (this._axWindow.Handle == hwnd)
			{
				return;
			}
			this._axWindow = new HandleRef(this, hwnd);
			if (this.ParentHandle.Handle != IntPtr.Zero)
			{
				UnsafeNativeMethods.SetParent(this._axWindow, this.ParentHandle);
			}
		}

		// Token: 0x06006181 RID: 24961 RVA: 0x001B5B9E File Offset: 0x001B3D9E
		[SecurityCritical]
		private void StartEvents()
		{
			if (!this.GetAxHostState(ActiveXHelper.sinkAttached))
			{
				this.SetAxHostState(ActiveXHelper.sinkAttached, true);
				this.CreateSink();
			}
			this.ActiveXSite.StartEvents();
		}

		// Token: 0x06006182 RID: 24962 RVA: 0x001B5BCA File Offset: 0x001B3DCA
		[SecurityCritical]
		private void StopEvents()
		{
			if (this.GetAxHostState(ActiveXHelper.sinkAttached))
			{
				this.SetAxHostState(ActiveXHelper.sinkAttached, false);
				this.DetachSink();
			}
			this.ActiveXSite.StopEvents();
		}

		// Token: 0x06006183 RID: 24963 RVA: 0x001B5BF6 File Offset: 0x001B3DF6
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void TransitionFromPassiveToLoaded()
		{
			if (this.ActiveXState == ActiveXHelper.ActiveXState.Passive)
			{
				this._axInstance = this.CreateActiveXObject(this._clsid.Value);
				this.ActiveXState = ActiveXHelper.ActiveXState.Loaded;
				this.AttachInterfacesInternal();
			}
		}

		// Token: 0x06006184 RID: 24964 RVA: 0x001B5C24 File Offset: 0x001B3E24
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void TransitionFromLoadedToPassive()
		{
			if (this.ActiveXState == ActiveXHelper.ActiveXState.Loaded)
			{
				if (this._axInstance != null)
				{
					this.DetachInterfacesInternal();
					Marshal.FinalReleaseComObject(this._axInstance);
					this._axInstance = null;
				}
				this.ActiveXState = ActiveXHelper.ActiveXState.Passive;
			}
		}

		// Token: 0x06006185 RID: 24965 RVA: 0x001B5C58 File Offset: 0x001B3E58
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void TransitionFromLoadedToRunning()
		{
			if (this.ActiveXState == ActiveXHelper.ActiveXState.Loaded)
			{
				int num = 0;
				int miscStatus = this._axOleObject.GetMiscStatus(1, out num);
				if (NativeMethods.Succeeded(miscStatus) && (num & 131072) != 0)
				{
					this._axOleObject.SetClientSite(this.ActiveXSite);
				}
				this.StartEvents();
				this.ActiveXState = ActiveXHelper.ActiveXState.Running;
			}
		}

		// Token: 0x06006186 RID: 24966 RVA: 0x001B5CAF File Offset: 0x001B3EAF
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void TransitionFromRunningToLoaded()
		{
			if (this.ActiveXState == ActiveXHelper.ActiveXState.Running)
			{
				this.StopEvents();
				this._axOleObject.SetClientSite(null);
				this.ActiveXState = ActiveXHelper.ActiveXState.Loaded;
			}
		}

		// Token: 0x06006187 RID: 24967 RVA: 0x001B5CD4 File Offset: 0x001B3ED4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void TransitionFromRunningToInPlaceActive()
		{
			if (this.ActiveXState == ActiveXHelper.ActiveXState.Running)
			{
				try
				{
					this.DoVerb(-5);
				}
				catch (Exception ex)
				{
					if (CriticalExceptions.IsCriticalException(ex))
					{
						throw;
					}
					throw new TargetInvocationException(SR.Get("AXNohWnd", new object[]
					{
						base.GetType().Name
					}), ex);
				}
				this.ActiveXState = ActiveXHelper.ActiveXState.InPlaceActive;
			}
		}

		// Token: 0x06006188 RID: 24968 RVA: 0x001B5D3C File Offset: 0x001B3F3C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void TransitionFromInPlaceActiveToRunning()
		{
			if (this.ActiveXState == ActiveXHelper.ActiveXState.InPlaceActive)
			{
				this._axOleInPlaceObject.InPlaceDeactivate();
				this.ActiveXState = ActiveXHelper.ActiveXState.Running;
			}
		}

		// Token: 0x06006189 RID: 24969 RVA: 0x001B5D59 File Offset: 0x001B3F59
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void TransitionFromInPlaceActiveToUIActive()
		{
			if (this.ActiveXState == ActiveXHelper.ActiveXState.InPlaceActive)
			{
				this.DoVerb(-4);
				this.ActiveXState = ActiveXHelper.ActiveXState.UIActive;
			}
		}

		// Token: 0x0600618A RID: 24970 RVA: 0x001B5D74 File Offset: 0x001B3F74
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void TransitionFromUIActiveToInPlaceActive()
		{
			if (this.ActiveXState == ActiveXHelper.ActiveXState.UIActive)
			{
				int num = this._axOleInPlaceObject.UIDeactivate();
				this.ActiveXState = ActiveXHelper.ActiveXState.InPlaceActive;
			}
		}

		// Token: 0x17001777 RID: 6007
		// (get) Token: 0x0600618B RID: 24971 RVA: 0x001B5D9D File Offset: 0x001B3F9D
		// (set) Token: 0x0600618C RID: 24972 RVA: 0x001B5DAF File Offset: 0x001B3FAF
		internal int TabIndex
		{
			get
			{
				return (int)base.GetValue(ActiveXHost.TabIndexProperty);
			}
			set
			{
				base.SetValue(ActiveXHost.TabIndexProperty, value);
			}
		}

		// Token: 0x17001778 RID: 6008
		// (get) Token: 0x0600618D RID: 24973 RVA: 0x001B5DC2 File Offset: 0x001B3FC2
		// (set) Token: 0x0600618E RID: 24974 RVA: 0x001B5DCA File Offset: 0x001B3FCA
		internal HandleRef ParentHandle
		{
			[SecurityCritical]
			get
			{
				return this._hwndParent;
			}
			[SecurityCritical]
			set
			{
				this._hwndParent = value;
			}
		}

		// Token: 0x17001779 RID: 6009
		// (get) Token: 0x0600618F RID: 24975 RVA: 0x001B5DD3 File Offset: 0x001B3FD3
		// (set) Token: 0x06006190 RID: 24976 RVA: 0x001B5DDB File Offset: 0x001B3FDB
		internal NativeMethods.COMRECT Bounds
		{
			get
			{
				return this._bounds;
			}
			set
			{
				this._bounds = value;
			}
		}

		// Token: 0x1700177A RID: 6010
		// (get) Token: 0x06006191 RID: 24977 RVA: 0x001B5DE4 File Offset: 0x001B3FE4
		internal Rect BoundRect
		{
			get
			{
				return this._boundRect;
			}
		}

		// Token: 0x1700177B RID: 6011
		// (get) Token: 0x06006192 RID: 24978 RVA: 0x001B5DEC File Offset: 0x001B3FEC
		internal HandleRef ControlHandle
		{
			[SecurityCritical]
			get
			{
				return this._axWindow;
			}
		}

		// Token: 0x1700177C RID: 6012
		// (get) Token: 0x06006193 RID: 24979 RVA: 0x001B5DF4 File Offset: 0x001B3FF4
		internal object ActiveXInstance
		{
			[SecurityCritical]
			get
			{
				return this._axInstance;
			}
		}

		// Token: 0x1700177D RID: 6013
		// (get) Token: 0x06006194 RID: 24980 RVA: 0x001B5DFC File Offset: 0x001B3FFC
		internal UnsafeNativeMethods.IOleInPlaceObject ActiveXInPlaceObject
		{
			[SecurityCritical]
			get
			{
				return this._axOleInPlaceObject;
			}
		}

		// Token: 0x1700177E RID: 6014
		// (get) Token: 0x06006195 RID: 24981 RVA: 0x001B5E04 File Offset: 0x001B4004
		internal UnsafeNativeMethods.IOleInPlaceActiveObject ActiveXInPlaceActiveObject
		{
			[SecurityCritical]
			get
			{
				return this._axOleInPlaceActiveObject;
			}
		}

		// Token: 0x06006196 RID: 24982 RVA: 0x001B5E0C File Offset: 0x001B400C
		private void OnInitialized(object sender, EventArgs e)
		{
			base.Initialized -= this.OnInitialized;
		}

		// Token: 0x06006197 RID: 24983 RVA: 0x00002137 File Offset: 0x00000337
		private static void OnIsEnabledInvalidated(ActiveXHost axHost)
		{
		}

		// Token: 0x06006198 RID: 24984 RVA: 0x001B5E20 File Offset: 0x001B4020
		private static void OnVisibilityInvalidated(ActiveXHost axHost)
		{
			if (axHost != null)
			{
				switch (axHost.Visibility)
				{
				}
			}
		}

		// Token: 0x06006199 RID: 24985 RVA: 0x001B5E4C File Offset: 0x001B404C
		private static void OnGotFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			ActiveXHost activeXHost = sender as ActiveXHost;
			if (activeXHost != null)
			{
				Invariant.Assert(activeXHost.ActiveXState >= ActiveXHelper.ActiveXState.InPlaceActive, "Should at least be InPlaceActive when getting focus");
				if (activeXHost.ActiveXState < ActiveXHelper.ActiveXState.UIActive)
				{
					activeXHost.TransitionUpTo(ActiveXHelper.ActiveXState.UIActive);
				}
			}
		}

		// Token: 0x0600619A RID: 24986 RVA: 0x001B5E8C File Offset: 0x001B408C
		private static void OnLostFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			ActiveXHost activeXHost = sender as ActiveXHost;
			if (activeXHost != null)
			{
				Invariant.Assert(activeXHost.ActiveXState >= ActiveXHelper.ActiveXState.UIActive, "Should at least be UIActive when losing focus");
				bool flag = !activeXHost.IsKeyboardFocusWithin;
				if (flag)
				{
					activeXHost.TransitionDownTo(ActiveXHelper.ActiveXState.InPlaceActive);
				}
			}
		}

		// Token: 0x0600619B RID: 24987 RVA: 0x0019E023 File Offset: 0x0019C223
		private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs args)
		{
			if (!args.Handled && args.Scope == null && args.Target == null)
			{
				args.Target = (UIElement)sender;
			}
		}

		// Token: 0x0600619C RID: 24988 RVA: 0x001B5ED0 File Offset: 0x001B40D0
		[SecurityCritical]
		private void AttachInterfacesInternal()
		{
			this._axOleObject = (UnsafeNativeMethods.IOleObject)this._axInstance;
			this._axOleInPlaceObject = (UnsafeNativeMethods.IOleInPlaceObject)this._axInstance;
			this._axOleInPlaceActiveObject = (UnsafeNativeMethods.IOleInPlaceActiveObject)this._axInstance;
			this.AttachInterfaces(this._axInstance);
		}

		// Token: 0x0600619D RID: 24989 RVA: 0x001B5F1C File Offset: 0x001B411C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void DetachInterfacesInternal()
		{
			this._axOleObject = null;
			this._axOleInPlaceObject = null;
			this._axOleInPlaceActiveObject = null;
			this.DetachInterfaces();
		}

		// Token: 0x0600619E RID: 24990 RVA: 0x001B5F3C File Offset: 0x001B413C
		[SecurityCritical]
		private NativeMethods.SIZE SetExtent(int width, int height)
		{
			NativeMethods.SIZE size = new NativeMethods.SIZE();
			size.cx = width;
			size.cy = height;
			bool flag = false;
			try
			{
				this._axOleObject.SetExtent(1, size);
			}
			catch (COMException)
			{
				flag = true;
			}
			if (flag)
			{
				this._axOleObject.GetExtent(1, size);
				try
				{
					this._axOleObject.SetExtent(1, size);
				}
				catch (COMException ex)
				{
				}
			}
			return this.GetExtent();
		}

		// Token: 0x0600619F RID: 24991 RVA: 0x001B5FBC File Offset: 0x001B41BC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private NativeMethods.SIZE GetExtent()
		{
			NativeMethods.SIZE size = new NativeMethods.SIZE();
			this._axOleObject.GetExtent(1, size);
			return size;
		}

		// Token: 0x04003153 RID: 12627
		internal static readonly DependencyProperty TabIndexProperty = Control.TabIndexProperty.AddOwner(typeof(ActiveXHost));

		// Token: 0x04003154 RID: 12628
		private static Hashtable invalidatorMap = new Hashtable();

		// Token: 0x04003155 RID: 12629
		private NativeMethods.COMRECT _bounds = new NativeMethods.COMRECT(0, 0, 0, 0);

		// Token: 0x04003156 RID: 12630
		private Rect _boundRect = new Rect(0.0, 0.0, 0.0, 0.0);

		// Token: 0x04003157 RID: 12631
		private Size _cachedSize = Size.Empty;

		// Token: 0x04003158 RID: 12632
		[SecurityCritical]
		private HandleRef _hwndParent;

		// Token: 0x04003159 RID: 12633
		private bool _isDisposed;

		// Token: 0x0400315A RID: 12634
		private SecurityCriticalDataForSet<Guid> _clsid;

		// Token: 0x0400315B RID: 12635
		[SecurityCritical]
		private HandleRef _axWindow;

		// Token: 0x0400315C RID: 12636
		private BitVector32 _axHostState;

		// Token: 0x0400315D RID: 12637
		private ActiveXHelper.ActiveXState _axState;

		// Token: 0x0400315E RID: 12638
		[SecurityCritical]
		private ActiveXSite _axSite;

		// Token: 0x0400315F RID: 12639
		[SecurityCritical]
		private ActiveXContainer _axContainer;

		// Token: 0x04003160 RID: 12640
		[SecurityCritical]
		private object _axInstance;

		// Token: 0x04003161 RID: 12641
		[SecurityCritical]
		private UnsafeNativeMethods.IOleObject _axOleObject;

		// Token: 0x04003162 RID: 12642
		[SecurityCritical]
		private UnsafeNativeMethods.IOleInPlaceObject _axOleInPlaceObject;

		// Token: 0x04003163 RID: 12643
		[SecurityCritical]
		private UnsafeNativeMethods.IOleInPlaceActiveObject _axOleInPlaceActiveObject;

		// Token: 0x020009F7 RID: 2551
		// (Invoke) Token: 0x060089BF RID: 35263
		private delegate void PropertyInvalidator(ActiveXHost axhost);
	}
}
