﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Internal.Utility;

namespace System.Windows.Controls
{
	/// <summary>Frame is a content control that supports navigation.</summary>
	// Token: 0x020004D3 RID: 1235
	[DefaultProperty("Source")]
	[DefaultEvent("Navigated")]
	[Localizability(LocalizationCategory.Ignore)]
	[ContentProperty]
	[TemplatePart(Name = "PART_FrameCP", Type = typeof(ContentPresenter))]
	public class Frame : ContentControl, INavigator, INavigatorBase, INavigatorImpl, IJournalNavigationScopeHost, IDownloader, IJournalState, IAddChild, IUriContext
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Frame" /> class.</summary>
		// Token: 0x06004BE2 RID: 19426 RVA: 0x00156304 File Offset: 0x00154504
		public Frame()
		{
			this.Init();
		}

		// Token: 0x06004BE3 RID: 19427 RVA: 0x00156314 File Offset: 0x00154514
		static Frame()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Frame), new FrameworkPropertyMetadata(typeof(Frame)));
			Frame._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Frame));
			ContentControl.ContentProperty.OverrideMetadata(typeof(Frame), new FrameworkPropertyMetadata(null, new CoerceValueCallback(Frame.CoerceContent)));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(Frame), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
			KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeof(Frame), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			NavigationService.NavigationServiceProperty.OverrideMetadata(typeof(Frame), new FrameworkPropertyMetadata(new PropertyChangedCallback(Frame.OnParentNavigationServiceChanged)));
			ControlsTraceLogger.AddControl(TelemetryControls.Frame);
		}

		// Token: 0x06004BE4 RID: 19428 RVA: 0x0015655C File Offset: 0x0015475C
		private static object CoerceContent(DependencyObject d, object value)
		{
			Frame frame = (Frame)d;
			if (frame._navigationService.Content == value)
			{
				return value;
			}
			frame.Navigate(value);
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06004BE5 RID: 19429 RVA: 0x0015658D File Offset: 0x0015478D
		private void Init()
		{
			base.InheritanceBehavior = InheritanceBehavior.SkipToAppNow;
			base.ContentIsNotLogical = true;
			this._navigationService = new NavigationService(this);
			this._navigationService.BPReady += this._OnBPReady;
		}

		/// <summary>For a description of this member, see <see cref="P:System.Windows.Markup.IUriContext.BaseUri" />.</summary>
		/// <returns>The base URI of the current context.</returns>
		// Token: 0x17001288 RID: 4744
		// (get) Token: 0x06004BE6 RID: 19430 RVA: 0x001565C0 File Offset: 0x001547C0
		// (set) Token: 0x06004BE7 RID: 19431 RVA: 0x001565C8 File Offset: 0x001547C8
		Uri IUriContext.BaseUri
		{
			get
			{
				return this.BaseUri;
			}
			set
			{
				this.BaseUri = value;
			}
		}

		/// <summary>Gets or sets the base uniform resource identifier (URI) for a <see cref="T:System.Windows.Controls.Frame" />.</summary>
		/// <returns>The base uniform resource identifier (URI) of the <see cref="T:System.Windows.Controls.Frame" /> control.</returns>
		// Token: 0x17001289 RID: 4745
		// (get) Token: 0x06004BE8 RID: 19432 RVA: 0x000C204F File Offset: 0x000C024F
		// (set) Token: 0x06004BE9 RID: 19433 RVA: 0x000C2061 File Offset: 0x000C0261
		protected virtual Uri BaseUri
		{
			get
			{
				return (Uri)base.GetValue(BaseUriHelper.BaseUriProperty);
			}
			set
			{
				base.SetValue(BaseUriHelper.BaseUriProperty, value);
			}
		}

		// Token: 0x1700128A RID: 4746
		// (get) Token: 0x06004BEA RID: 19434 RVA: 0x001565D1 File Offset: 0x001547D1
		NavigationService IDownloader.Downloader
		{
			get
			{
				return this._navigationService;
			}
		}

		/// <summary>Occurs after <see cref="T:System.Windows.Controls.Frame" /> content has been rendered.</summary>
		// Token: 0x140000D1 RID: 209
		// (add) Token: 0x06004BEB RID: 19435 RVA: 0x001565DC File Offset: 0x001547DC
		// (remove) Token: 0x06004BEC RID: 19436 RVA: 0x00156614 File Offset: 0x00154814
		public event EventHandler ContentRendered;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.Frame.ContentRendered" /> event.</summary>
		/// <param name="args">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06004BED RID: 19437 RVA: 0x0015664C File Offset: 0x0015484C
		protected virtual void OnContentRendered(EventArgs args)
		{
			DependencyObject dependencyObject = base.Content as DependencyObject;
			if (dependencyObject != null)
			{
				IInputElement focusedElement = FocusManager.GetFocusedElement(dependencyObject);
				if (focusedElement != null)
				{
					focusedElement.Focus();
				}
			}
			if (this.ContentRendered != null)
			{
				this.ContentRendered(this, args);
			}
		}

		// Token: 0x06004BEE RID: 19438 RVA: 0x00156690 File Offset: 0x00154890
		private static object CoerceSource(DependencyObject d, object value)
		{
			Frame frame = (Frame)d;
			if (frame._sourceUpdatedFromNavService)
			{
				Invariant.Assert(frame._navigationService != null, "_navigationService should never be null here");
				return frame._navigationService.Source;
			}
			return value;
		}

		// Token: 0x06004BEF RID: 19439 RVA: 0x001566CC File Offset: 0x001548CC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Frame frame = (Frame)d;
			if (!frame._sourceUpdatedFromNavService)
			{
				Uri uriToNavigate = BindUriHelper.GetUriToNavigate(frame, ((IUriContext)frame).BaseUri, (Uri)e.NewValue);
				frame._navigationService.Navigate(uriToNavigate, null, false, true);
			}
		}

		// Token: 0x06004BF0 RID: 19440 RVA: 0x00156714 File Offset: 0x00154914
		void INavigatorImpl.OnSourceUpdatedFromNavService(bool journalOrCancel)
		{
			try
			{
				this._sourceUpdatedFromNavService = true;
				base.SetCurrentValueInternal(Frame.SourceProperty, this._navigationService.Source);
			}
			finally
			{
				this._sourceUpdatedFromNavService = false;
			}
		}

		/// <summary>Gets or sets the uniform resource identifier (URI) of the current content, or the URI of new content that is currently being navigated to. </summary>
		/// <returns>A <see cref="T:System.Uri" /> that contains the URI for the current content, or the content that is currently being navigated to.</returns>
		// Token: 0x1700128B RID: 4747
		// (get) Token: 0x06004BF1 RID: 19441 RVA: 0x00156758 File Offset: 0x00154958
		// (set) Token: 0x06004BF2 RID: 19442 RVA: 0x0015676A File Offset: 0x0015496A
		[Bindable(true)]
		[CustomCategory("Navigation")]
		public Uri Source
		{
			get
			{
				return (Uri)base.GetValue(Frame.SourceProperty);
			}
			set
			{
				base.SetValue(Frame.SourceProperty, value);
			}
		}

		/// <summary>Gets or sets when the <see cref="T:System.Windows.Controls.Frame" /> can show its navigation UI. </summary>
		/// <returns>A <see cref="T:System.Windows.Navigation.NavigationUIVisibility" /> value that specifies when the <see cref="T:System.Windows.Controls.Frame" /> can show its navigation UI. The default value is <see cref="F:System.Windows.Navigation.NavigationUIVisibility.Automatic" />.</returns>
		// Token: 0x1700128C RID: 4748
		// (get) Token: 0x06004BF3 RID: 19443 RVA: 0x00156778 File Offset: 0x00154978
		// (set) Token: 0x06004BF4 RID: 19444 RVA: 0x0015678A File Offset: 0x0015498A
		public NavigationUIVisibility NavigationUIVisibility
		{
			get
			{
				return (NavigationUIVisibility)base.GetValue(Frame.NavigationUIVisibilityProperty);
			}
			set
			{
				base.SetValue(Frame.NavigationUIVisibilityProperty, value);
			}
		}

		/// <summary>Gets or sets whether a <see cref="T:System.Windows.Controls.Frame" /> isolates external Extensible Application Markup Language (XAML) content within a partial trust security sandbox (with the default <see langword="Internet " />permission set). </summary>
		/// <returns>
		///     <see langword="true" /> if content is isolated within a partial trust security sandbox; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.Security.SecurityException">
		///         <see cref="P:System.Windows.Controls.Frame.SandboxExternalContent" /> is set when an application is executing in partial trust.</exception>
		// Token: 0x1700128D RID: 4749
		// (get) Token: 0x06004BF5 RID: 19445 RVA: 0x0015679D File Offset: 0x0015499D
		// (set) Token: 0x06004BF6 RID: 19446 RVA: 0x001567B0 File Offset: 0x001549B0
		public bool SandboxExternalContent
		{
			get
			{
				return (bool)base.GetValue(Frame.SandboxExternalContentProperty);
			}
			set
			{
				bool value2 = value;
				SecurityHelper.ThrowExceptionIfSettingTrueInPartialTrust(ref value2);
				base.SetValue(Frame.SandboxExternalContentProperty, value2);
			}
		}

		// Token: 0x06004BF7 RID: 19447 RVA: 0x001567D4 File Offset: 0x001549D4
		private static void OnSandboxExternalContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Frame frame = (Frame)d;
			bool flag = (bool)e.NewValue;
			SecurityHelper.ThrowExceptionIfSettingTrueInPartialTrust(ref flag);
			if (flag && !(bool)e.OldValue)
			{
				frame.NavigationService.Refresh();
			}
		}

		// Token: 0x06004BF8 RID: 19448 RVA: 0x00156818 File Offset: 0x00154A18
		private static object CoerceSandBoxExternalContentValue(DependencyObject d, object value)
		{
			bool flag = (bool)value;
			SecurityHelper.ThrowExceptionIfSettingTrueInPartialTrust(ref flag);
			return flag;
		}

		// Token: 0x06004BF9 RID: 19449 RVA: 0x0015683C File Offset: 0x00154A3C
		private static bool ValidateJournalOwnershipValue(object value)
		{
			JournalOwnership journalOwnership = (JournalOwnership)value;
			return journalOwnership == JournalOwnership.Automatic || journalOwnership == JournalOwnership.UsesParentJournal || journalOwnership == JournalOwnership.OwnsJournal;
		}

		// Token: 0x06004BFA RID: 19450 RVA: 0x0015685D File Offset: 0x00154A5D
		private static void OnJournalOwnershipPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Frame)d).OnJournalOwnershipPropertyChanged((JournalOwnership)e.NewValue);
		}

		// Token: 0x06004BFB RID: 19451 RVA: 0x00156878 File Offset: 0x00154A78
		private void OnJournalOwnershipPropertyChanged(JournalOwnership newValue)
		{
			switch (this._journalOwnership)
			{
			case JournalOwnership.Automatic:
				if (newValue != JournalOwnership.OwnsJournal)
				{
					if (newValue == JournalOwnership.UsesParentJournal)
					{
						this.SwitchToParentJournal();
					}
				}
				else
				{
					this.SwitchToOwnJournal();
				}
				break;
			case JournalOwnership.OwnsJournal:
				if (newValue != JournalOwnership.Automatic && newValue == JournalOwnership.UsesParentJournal)
				{
					this.SwitchToParentJournal();
				}
				break;
			case JournalOwnership.UsesParentJournal:
				if (newValue != JournalOwnership.Automatic)
				{
					if (newValue == JournalOwnership.OwnsJournal)
					{
						this.SwitchToOwnJournal();
					}
				}
				else
				{
					this._navigationService.InvalidateJournalNavigationScope();
				}
				break;
			}
			this._journalOwnership = newValue;
		}

		// Token: 0x06004BFC RID: 19452 RVA: 0x001568EC File Offset: 0x00154AEC
		private static object CoerceJournalOwnership(DependencyObject d, object newValue)
		{
			JournalOwnership journalOwnership = ((Frame)d)._journalOwnership;
			if (journalOwnership == JournalOwnership.OwnsJournal && (JournalOwnership)newValue == JournalOwnership.Automatic)
			{
				return JournalOwnership.OwnsJournal;
			}
			return newValue;
		}

		/// <summary>Gets or sets whether a <see cref="T:System.Windows.Controls.Frame" /> is responsible for managing its own navigation history, or yields navigation history management to a parent navigator (<see cref="T:System.Windows.Navigation.NavigationWindow" />, <see cref="T:System.Windows.Controls.Frame" />).</summary>
		/// <returns>A <see cref="T:System.Windows.Navigation.JournalOwnership" /> value that specifies whether <see cref="T:System.Windows.Controls.Frame" /> manages its own journal. The default value is <see cref="F:System.Windows.Navigation.JournalOwnership.Automatic" />.</returns>
		// Token: 0x1700128E RID: 4750
		// (get) Token: 0x06004BFD RID: 19453 RVA: 0x00156919 File Offset: 0x00154B19
		// (set) Token: 0x06004BFE RID: 19454 RVA: 0x00156921 File Offset: 0x00154B21
		public JournalOwnership JournalOwnership
		{
			get
			{
				return this._journalOwnership;
			}
			set
			{
				if (value != this._journalOwnership)
				{
					base.SetValue(Frame.JournalOwnershipProperty, value);
				}
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Navigation.NavigationService" /> that is used by this <see cref="T:System.Windows.Controls.Frame" /> to provide navigation services.</summary>
		/// <returns>A <see cref="T:System.Windows.Controls.Frame" /> object that represents the <see cref="T:System.Windows.Navigation.NavigationService" /> used by this <see cref="T:System.Windows.Controls.Frame" />, if one is available. Otherwise, <see langword="null" /> is returned.</returns>
		// Token: 0x1700128F RID: 4751
		// (get) Token: 0x06004BFF RID: 19455 RVA: 0x0015693D File Offset: 0x00154B3D
		public NavigationService NavigationService
		{
			get
			{
				base.VerifyAccess();
				return this._navigationService;
			}
		}

		/// <summary>Creates and returns a <see cref="T:System.Windows.Automation.Peers.NavigationWindowAutomationPeer" /> object for this <see cref="T:System.Windows.Controls.Frame" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.NavigationWindowAutomationPeer" /> object for this <see cref="T:System.Windows.Controls.Frame" />.</returns>
		// Token: 0x06004C00 RID: 19456 RVA: 0x0015694B File Offset: 0x00154B4B
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new FrameAutomationPeer(this);
		}

		/// <summary>Adds a child object. </summary>
		/// <param name="value">The child object to add.</param>
		// Token: 0x06004C01 RID: 19457 RVA: 0x00156953 File Offset: 0x00154B53
		protected override void AddChild(object value)
		{
			throw new InvalidOperationException(SR.Get("FrameNoAddChild"));
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x06004C02 RID: 19458 RVA: 0x0000B31C File Offset: 0x0000951C
		protected override void AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06004C03 RID: 19459 RVA: 0x00156964 File Offset: 0x00154B64
		private void _OnBPReady(object o, BPReadyEventArgs e)
		{
			base.SetCurrentValueInternal(ContentControl.ContentProperty, e.Content);
			base.InvalidateMeasure();
			if (base.IsLoaded)
			{
				this.PostContentRendered();
				return;
			}
			if (!this._postContentRenderedFromLoadedHandler)
			{
				base.Loaded += this.LoadedHandler;
				this._postContentRenderedFromLoadedHandler = true;
			}
		}

		// Token: 0x06004C04 RID: 19460 RVA: 0x001569B8 File Offset: 0x00154BB8
		private void LoadedHandler(object sender, RoutedEventArgs args)
		{
			if (this._postContentRenderedFromLoadedHandler)
			{
				this.PostContentRendered();
				this._postContentRenderedFromLoadedHandler = false;
				base.Loaded -= this.LoadedHandler;
			}
		}

		// Token: 0x06004C05 RID: 19461 RVA: 0x001569E1 File Offset: 0x00154BE1
		private void PostContentRendered()
		{
			if (this._contentRenderedCallback != null)
			{
				this._contentRenderedCallback.Abort();
			}
			this._contentRenderedCallback = base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object unused)
			{
				this._contentRenderedCallback = null;
				this.OnContentRendered(EventArgs.Empty);
				return null;
			}), this);
		}

		// Token: 0x06004C06 RID: 19462 RVA: 0x00156A16 File Offset: 0x00154C16
		private void OnQueryGoBack(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this._ownJournalScope.CanGoBack;
			e.Handled = true;
		}

		// Token: 0x06004C07 RID: 19463 RVA: 0x00156A30 File Offset: 0x00154C30
		private void OnGoBack(object sender, ExecutedRoutedEventArgs e)
		{
			this._ownJournalScope.GoBack();
			e.Handled = true;
		}

		// Token: 0x06004C08 RID: 19464 RVA: 0x00156A44 File Offset: 0x00154C44
		private void OnQueryGoForward(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this._ownJournalScope.CanGoForward;
			e.Handled = true;
		}

		// Token: 0x06004C09 RID: 19465 RVA: 0x00156A5E File Offset: 0x00154C5E
		private void OnGoForward(object sender, ExecutedRoutedEventArgs e)
		{
			this._ownJournalScope.GoForward();
			e.Handled = true;
		}

		// Token: 0x06004C0A RID: 19466 RVA: 0x00156A74 File Offset: 0x00154C74
		private void OnNavigateJournal(object sender, ExecutedRoutedEventArgs e)
		{
			FrameworkElement frameworkElement = e.Parameter as FrameworkElement;
			if (frameworkElement != null)
			{
				JournalEntry journalEntry = frameworkElement.DataContext as JournalEntry;
				if (journalEntry != null && this._ownJournalScope.NavigateToEntry(journalEntry))
				{
					e.Handled = true;
				}
			}
		}

		// Token: 0x06004C0B RID: 19467 RVA: 0x00156AB4 File Offset: 0x00154CB4
		private void OnQueryRefresh(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (base.Content != null);
		}

		// Token: 0x06004C0C RID: 19468 RVA: 0x00156AC5 File Offset: 0x00154CC5
		private void OnRefresh(object sender, ExecutedRoutedEventArgs e)
		{
			this._navigationService.Refresh();
			e.Handled = true;
		}

		// Token: 0x06004C0D RID: 19469 RVA: 0x00156AD9 File Offset: 0x00154CD9
		private void OnBrowseStop(object sender, ExecutedRoutedEventArgs e)
		{
			this._ownJournalScope.StopLoading();
			e.Handled = true;
		}

		// Token: 0x06004C0E RID: 19470 RVA: 0x00156AED File Offset: 0x00154CED
		internal override object AdjustEventSource(RoutedEventArgs e)
		{
			e.Source = this;
			return this;
		}

		// Token: 0x06004C0F RID: 19471 RVA: 0x00156AF7 File Offset: 0x00154CF7
		internal override string GetPlainText()
		{
			if (this.Source != null)
			{
				return this.Source.ToString();
			}
			return string.Empty;
		}

		/// <summary>Allows derived classes to determine the serialization behavior of the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if the content should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004C10 RID: 19472 RVA: 0x00156B18 File Offset: 0x00154D18
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool ShouldSerializeContent()
		{
			Invariant.Assert(this._navigationService != null, "_navigationService should never be null here");
			return !this._navigationService.CanReloadFromUri && base.Content != null;
		}

		// Token: 0x06004C11 RID: 19473 RVA: 0x00156B45 File Offset: 0x00154D45
		private static void OnParentNavigationServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Frame)d).NavigationService.OnParentNavigationServiceChanged();
		}

		/// <summary>Called when the template generation for the visual tree is created.</summary>
		// Token: 0x06004C12 RID: 19474 RVA: 0x00156B58 File Offset: 0x00154D58
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			Visual templateChild = this.TemplateChild;
			if (templateChild != null)
			{
				this.NavigationService.VisualTreeAvailable(templateChild);
			}
		}

		// Token: 0x06004C13 RID: 19475 RVA: 0x00156B81 File Offset: 0x00154D81
		Visual INavigatorImpl.FindRootViewer()
		{
			return NavigationHelper.FindRootViewer(this, "PART_FrameCP");
		}

		// Token: 0x06004C14 RID: 19476 RVA: 0x00156B8E File Offset: 0x00154D8E
		JournalNavigationScope INavigator.GetJournal(bool create)
		{
			return this.GetJournal(create);
		}

		// Token: 0x06004C15 RID: 19477 RVA: 0x00156B98 File Offset: 0x00154D98
		private JournalNavigationScope GetJournal(bool create)
		{
			Invariant.Assert(this._ownJournalScope != null ^ this._journalOwnership != JournalOwnership.OwnsJournal);
			if (this._ownJournalScope != null)
			{
				return this._ownJournalScope;
			}
			JournalNavigationScope parentJournal = this.GetParentJournal(create);
			if (parentJournal != null)
			{
				base.SetCurrentValueInternal(Frame.JournalOwnershipProperty, JournalOwnership.UsesParentJournal);
				return parentJournal;
			}
			if (create && this._journalOwnership == JournalOwnership.Automatic)
			{
				base.SetCurrentValueInternal(Frame.JournalOwnershipProperty, JournalOwnership.OwnsJournal);
			}
			return this._ownJournalScope;
		}

		/// <summary>Gets a value that indicates whether there is at least one entry in forward navigation history. </summary>
		/// <returns>
		///     <see langword="true" /> if there is at least one entry in forward navigation history; <see langword="false" /> if there are no entries in forward navigation history or the <see cref="T:System.Windows.Controls.Frame" /> does not own its own navigation history.</returns>
		// Token: 0x17001290 RID: 4752
		// (get) Token: 0x06004C16 RID: 19478 RVA: 0x00156C10 File Offset: 0x00154E10
		public bool CanGoForward
		{
			get
			{
				return this._ownJournalScope != null && this._ownJournalScope.CanGoForward;
			}
		}

		/// <summary>Gets a value that indicates whether there is at least one entry in back navigation history. </summary>
		/// <returns>
		///     <see langword="true" /> if there is at least one entry in back navigation history; <see langword="false" /> if there are no entries in back navigation history or the <see cref="T:System.Windows.Controls.Frame" /> does not own its own navigation history.</returns>
		// Token: 0x17001291 RID: 4753
		// (get) Token: 0x06004C17 RID: 19479 RVA: 0x00156C38 File Offset: 0x00154E38
		public bool CanGoBack
		{
			get
			{
				return this._ownJournalScope != null && this._ownJournalScope.CanGoBack;
			}
		}

		/// <summary>Adds an entry to back navigation history that contains a <see cref="T:System.Windows.Navigation.CustomContentState" /> object.</summary>
		/// <param name="state">A <see cref="T:System.Windows.Navigation.CustomContentState" /> object that represents application-defined state that is associated with a specific piece of content.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="state" /> is <see langword="null" />, and a <see cref="T:System.Windows.Navigation.CustomContentState" /> object is not returned from <see cref="M:System.Windows.Navigation.IProvideCustomContentState.GetContentState" />.</exception>
		// Token: 0x06004C18 RID: 19480 RVA: 0x00156C5D File Offset: 0x00154E5D
		public void AddBackEntry(CustomContentState state)
		{
			base.VerifyAccess();
			this._navigationService.AddBackEntry(state);
		}

		/// <summary>Removes the most recent journal entry from back history.</summary>
		/// <returns>The most recent <see cref="T:System.Windows.Navigation.JournalEntry" /> in back navigation history, if there is one.</returns>
		// Token: 0x06004C19 RID: 19481 RVA: 0x00156C71 File Offset: 0x00154E71
		public JournalEntry RemoveBackEntry()
		{
			if (this._ownJournalScope == null)
			{
				throw new InvalidOperationException(SR.Get("InvalidOperation_NoJournal"));
			}
			return this._ownJournalScope.RemoveBackEntry();
		}

		/// <summary>Navigates asynchronously to content that is specified by a uniform resource identifier (URI).</summary>
		/// <param name="source">A <see cref="T:System.Uri" /> object initialized with the URI for the desired content.</param>
		/// <returns>
		///     <see langword="true" /> if navigation is not canceled; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004C1A RID: 19482 RVA: 0x00156C96 File Offset: 0x00154E96
		public bool Navigate(Uri source)
		{
			base.VerifyAccess();
			return this._navigationService.Navigate(source);
		}

		/// <summary>Navigates asynchronously to source content located at a uniform resource identifier (URI), and passes an object that contains data to be used for processing during navigation.</summary>
		/// <param name="source">A <see cref="T:System.Uri" /> object initialized with the URI for the desired content.</param>
		/// <param name="extraData">A <see cref="T:System.Object" /> that contains data to be used for processing during navigation.</param>
		/// <returns>
		///     <see langword="true" /> if navigation is not canceled; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004C1B RID: 19483 RVA: 0x00156CAA File Offset: 0x00154EAA
		public bool Navigate(Uri source, object extraData)
		{
			base.VerifyAccess();
			return this._navigationService.Navigate(source, extraData);
		}

		/// <summary>Navigates asynchronously to content that is contained by an object.</summary>
		/// <param name="content">An <see cref="T:System.Object" /> that contains the content to navigate to.</param>
		/// <returns>
		///     <see langword="true" /> if navigation is not canceled; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004C1C RID: 19484 RVA: 0x00156CBF File Offset: 0x00154EBF
		public bool Navigate(object content)
		{
			base.VerifyAccess();
			return this._navigationService.Navigate(content);
		}

		/// <summary>Navigates asynchronously to content that is contained by an object, and passes an object that contains data to be used for processing during navigation.</summary>
		/// <param name="content">An <see cref="T:System.Object" /> that contains the content to navigate to.</param>
		/// <param name="extraData">A <see cref="T:System.Object" /> that contains data to be used for processing during navigation.</param>
		/// <returns>
		///     <see langword="true" /> if navigation is not canceled; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004C1D RID: 19485 RVA: 0x00156CD3 File Offset: 0x00154ED3
		public bool Navigate(object content, object extraData)
		{
			base.VerifyAccess();
			return this._navigationService.Navigate(content, extraData);
		}

		/// <summary>Navigates to the most recent item in forward navigation history, if a <see cref="T:System.Windows.Controls.Frame" /> manages its own navigation history.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="M:System.Windows.Controls.Frame.GoForward" /> is called when there are no entries in back navigation history.</exception>
		// Token: 0x06004C1E RID: 19486 RVA: 0x00156CE8 File Offset: 0x00154EE8
		public void GoForward()
		{
			if (this._ownJournalScope == null)
			{
				throw new InvalidOperationException(SR.Get("InvalidOperation_NoJournal"));
			}
			this._ownJournalScope.GoForward();
		}

		/// <summary>Navigates to the most recent item in back navigation history, if a <see cref="T:System.Windows.Controls.Frame" /> manages its own navigation history.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="M:System.Windows.Controls.Frame.GoBack" /> is called when there are no entries in back navigation history.</exception>
		// Token: 0x06004C1F RID: 19487 RVA: 0x00156D0D File Offset: 0x00154F0D
		public void GoBack()
		{
			if (this._ownJournalScope == null)
			{
				throw new InvalidOperationException(SR.Get("InvalidOperation_NoJournal"));
			}
			this._ownJournalScope.GoBack();
		}

		/// <summary>Stops further downloading of content for the current navigation request.</summary>
		// Token: 0x06004C20 RID: 19488 RVA: 0x00156D32 File Offset: 0x00154F32
		public void StopLoading()
		{
			base.VerifyAccess();
			this._navigationService.StopLoading();
		}

		/// <summary>Reloads the current content.</summary>
		// Token: 0x06004C21 RID: 19489 RVA: 0x00156D45 File Offset: 0x00154F45
		public void Refresh()
		{
			base.VerifyAccess();
			this._navigationService.Refresh();
		}

		/// <summary>Gets the uniform resource identifier (URI) of the content that was last navigated to.</summary>
		/// <returns>A <see cref="T:System.Uri" /> for the content that was last navigated to, if navigated to by using a URI; otherwise, <see langword="null" />.</returns>
		// Token: 0x17001292 RID: 4754
		// (get) Token: 0x06004C22 RID: 19490 RVA: 0x00156D58 File Offset: 0x00154F58
		public Uri CurrentSource
		{
			get
			{
				return this._navigationService.CurrentSource;
			}
		}

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerable" /> that you use to enumerate the entries in back navigation history for a <see cref="T:System.Windows.Controls.Frame" />. </summary>
		/// <returns>
		///     <see cref="T:System.Collections.IEnumerable" /> if at least one entry has been added to back navigation history. If there are not entries, or the <see cref="T:System.Windows.Controls.Frame" /> does not own its own navigation history, <see cref="P:System.Windows.Controls.Frame.BackStack" /> is <see langword="null" />.</returns>
		// Token: 0x17001293 RID: 4755
		// (get) Token: 0x06004C23 RID: 19491 RVA: 0x00156D68 File Offset: 0x00154F68
		public IEnumerable BackStack
		{
			get
			{
				return (this._ownJournalScope == null) ? null : this._ownJournalScope.BackStack;
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.IEnumerable" /> that you use to enumerate the entries in forward navigation history for a <see cref="T:System.Windows.Controls.Frame" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerable" /> object if at least one entry has been added to forward navigation history, or <see langword="null" /> if there are no entries or the <see cref="T:System.Windows.Controls.Frame" /> does not own its own navigation history.</returns>
		// Token: 0x17001294 RID: 4756
		// (get) Token: 0x06004C24 RID: 19492 RVA: 0x00156D90 File Offset: 0x00154F90
		public IEnumerable ForwardStack
		{
			get
			{
				return (this._ownJournalScope == null) ? null : this._ownJournalScope.ForwardStack;
			}
		}

		/// <summary>Occurs when a new navigation is requested.</summary>
		// Token: 0x140000D2 RID: 210
		// (add) Token: 0x06004C25 RID: 19493 RVA: 0x00156DB5 File Offset: 0x00154FB5
		// (remove) Token: 0x06004C26 RID: 19494 RVA: 0x00156DC3 File Offset: 0x00154FC3
		public event NavigatingCancelEventHandler Navigating
		{
			add
			{
				this._navigationService.Navigating += value;
			}
			remove
			{
				this._navigationService.Navigating -= value;
			}
		}

		/// <summary>Occurs periodically during a download to provide navigation progress information.</summary>
		// Token: 0x140000D3 RID: 211
		// (add) Token: 0x06004C27 RID: 19495 RVA: 0x00156DD1 File Offset: 0x00154FD1
		// (remove) Token: 0x06004C28 RID: 19496 RVA: 0x00156DDF File Offset: 0x00154FDF
		public event NavigationProgressEventHandler NavigationProgress
		{
			add
			{
				this._navigationService.NavigationProgress += value;
			}
			remove
			{
				this._navigationService.NavigationProgress -= value;
			}
		}

		/// <summary>Occurs when an error is raised while navigating to the requested content.</summary>
		// Token: 0x140000D4 RID: 212
		// (add) Token: 0x06004C29 RID: 19497 RVA: 0x00156DED File Offset: 0x00154FED
		// (remove) Token: 0x06004C2A RID: 19498 RVA: 0x00156DFB File Offset: 0x00154FFB
		public event NavigationFailedEventHandler NavigationFailed
		{
			add
			{
				this._navigationService.NavigationFailed += value;
			}
			remove
			{
				this._navigationService.NavigationFailed -= value;
			}
		}

		/// <summary>Occurs when the content that is being navigated to has been found, and is available from the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property, although it may not have completed loading.</summary>
		// Token: 0x140000D5 RID: 213
		// (add) Token: 0x06004C2B RID: 19499 RVA: 0x00156E09 File Offset: 0x00155009
		// (remove) Token: 0x06004C2C RID: 19500 RVA: 0x00156E17 File Offset: 0x00155017
		public event NavigatedEventHandler Navigated
		{
			add
			{
				this._navigationService.Navigated += value;
			}
			remove
			{
				this._navigationService.Navigated -= value;
			}
		}

		/// <summary>Occurs when content that was navigated to has been loaded, parsed, and has begun rendering.</summary>
		// Token: 0x140000D6 RID: 214
		// (add) Token: 0x06004C2D RID: 19501 RVA: 0x00156E25 File Offset: 0x00155025
		// (remove) Token: 0x06004C2E RID: 19502 RVA: 0x00156E33 File Offset: 0x00155033
		public event LoadCompletedEventHandler LoadCompleted
		{
			add
			{
				this._navigationService.LoadCompleted += value;
			}
			remove
			{
				this._navigationService.LoadCompleted -= value;
			}
		}

		/// <summary>Occurs when the <see cref="M:System.Windows.Controls.Frame.StopLoading" /> method is called, or when a new navigation is requested while a current navigation is in progress.</summary>
		// Token: 0x140000D7 RID: 215
		// (add) Token: 0x06004C2F RID: 19503 RVA: 0x00156E41 File Offset: 0x00155041
		// (remove) Token: 0x06004C30 RID: 19504 RVA: 0x00156E4F File Offset: 0x0015504F
		public event NavigationStoppedEventHandler NavigationStopped
		{
			add
			{
				this._navigationService.NavigationStopped += value;
			}
			remove
			{
				this._navigationService.NavigationStopped -= value;
			}
		}

		/// <summary>Occurs when navigation to a content fragment begins, which occurs immediately, if the desired fragment is in the current content, or after the source XAML content has been loaded, if the desired fragment is in different content.</summary>
		// Token: 0x140000D8 RID: 216
		// (add) Token: 0x06004C31 RID: 19505 RVA: 0x00156E5D File Offset: 0x0015505D
		// (remove) Token: 0x06004C32 RID: 19506 RVA: 0x00156E6B File Offset: 0x0015506B
		public event FragmentNavigationEventHandler FragmentNavigation
		{
			add
			{
				this._navigationService.FragmentNavigation += value;
			}
			remove
			{
				this._navigationService.FragmentNavigation -= value;
			}
		}

		// Token: 0x06004C33 RID: 19507 RVA: 0x000337BD File Offset: 0x000319BD
		void IJournalNavigationScopeHost.VerifyContextAndObjectState()
		{
			base.VerifyAccess();
		}

		// Token: 0x06004C34 RID: 19508 RVA: 0x00002137 File Offset: 0x00000337
		void IJournalNavigationScopeHost.OnJournalAvailable()
		{
		}

		// Token: 0x06004C35 RID: 19509 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IJournalNavigationScopeHost.GoBackOverride()
		{
			return false;
		}

		// Token: 0x06004C36 RID: 19510 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IJournalNavigationScopeHost.GoForwardOverride()
		{
			return false;
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x00156E7C File Offset: 0x0015507C
		CustomJournalStateInternal IJournalState.GetJournalState(JournalReason journalReason)
		{
			if (journalReason != JournalReason.NewContentNavigation)
			{
				return null;
			}
			Frame.FramePersistState framePersistState = new Frame.FramePersistState();
			framePersistState.JournalEntry = this._navigationService.MakeJournalEntry(JournalReason.NewContentNavigation);
			framePersistState.NavSvcGuid = this._navigationService.GuidId;
			framePersistState.JournalOwnership = this._journalOwnership;
			if (this._ownJournalScope != null)
			{
				framePersistState.Journal = this._ownJournalScope.Journal;
			}
			return framePersistState;
		}

		// Token: 0x06004C38 RID: 19512 RVA: 0x00156EE0 File Offset: 0x001550E0
		void IJournalState.RestoreJournalState(CustomJournalStateInternal cjs)
		{
			Frame.FramePersistState framePersistState = (Frame.FramePersistState)cjs;
			this._navigationService.GuidId = framePersistState.NavSvcGuid;
			this.JournalOwnership = framePersistState.JournalOwnership;
			if (this._journalOwnership == JournalOwnership.OwnsJournal)
			{
				Invariant.Assert(framePersistState.Journal != null);
				this._ownJournalScope.Journal = framePersistState.Journal;
			}
			if (framePersistState.JournalEntry != null)
			{
				framePersistState.JournalEntry.Navigate(this, NavigationMode.Back);
			}
		}

		// Token: 0x06004C39 RID: 19513 RVA: 0x00156F4F File Offset: 0x0015514F
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			if (this._ownJournalScope != null)
			{
				this._ownJournalScope.EnsureJournal();
			}
		}

		// Token: 0x06004C3A RID: 19514 RVA: 0x00156F6C File Offset: 0x0015516C
		internal override void OnThemeChanged()
		{
			DependencyObject d;
			if (!base.HasTemplateGeneratedSubTree && (d = (base.Content as DependencyObject)) != null)
			{
				FrameworkElement frameworkElement;
				FrameworkContentElement frameworkContentElement;
				Helper.DowncastToFEorFCE(d, out frameworkElement, out frameworkContentElement, false);
				if (frameworkElement != null || frameworkContentElement != null)
				{
					TreeWalkHelper.InvalidateOnResourcesChange(frameworkElement, frameworkContentElement, ResourcesChangeInfo.ThemeChangeInfo);
				}
			}
		}

		// Token: 0x06004C3B RID: 19515 RVA: 0x00156FB0 File Offset: 0x001551B0
		private JournalNavigationScope GetParentJournal(bool create)
		{
			JournalNavigationScope result = null;
			NavigationService parentNavigationService = this._navigationService.ParentNavigationService;
			if (parentNavigationService != null)
			{
				result = parentNavigationService.INavigatorHost.GetJournal(create);
			}
			return result;
		}

		// Token: 0x06004C3C RID: 19516 RVA: 0x00156FDC File Offset: 0x001551DC
		private void SwitchToOwnJournal()
		{
			if (this._ownJournalScope == null)
			{
				JournalNavigationScope parentJournal = this.GetParentJournal(false);
				if (parentJournal != null)
				{
					parentJournal.Journal.RemoveEntries(this._navigationService.GuidId);
				}
				this._ownJournalScope = new JournalNavigationScope(this);
				this._navigationService.InvalidateJournalNavigationScope();
				if (base.IsLoaded)
				{
					this._ownJournalScope.EnsureJournal();
				}
				this.AddCommandBinding(new CommandBinding(NavigationCommands.BrowseBack, new ExecutedRoutedEventHandler(this.OnGoBack), new CanExecuteRoutedEventHandler(this.OnQueryGoBack)));
				this.AddCommandBinding(new CommandBinding(NavigationCommands.BrowseForward, new ExecutedRoutedEventHandler(this.OnGoForward), new CanExecuteRoutedEventHandler(this.OnQueryGoForward)));
				this.AddCommandBinding(new CommandBinding(NavigationCommands.NavigateJournal, new ExecutedRoutedEventHandler(this.OnNavigateJournal)));
				this.AddCommandBinding(new CommandBinding(NavigationCommands.Refresh, new ExecutedRoutedEventHandler(this.OnRefresh), new CanExecuteRoutedEventHandler(this.OnQueryRefresh)));
				this.AddCommandBinding(new CommandBinding(NavigationCommands.BrowseStop, new ExecutedRoutedEventHandler(this.OnBrowseStop)));
			}
			this._journalOwnership = JournalOwnership.OwnsJournal;
		}

		// Token: 0x06004C3D RID: 19517 RVA: 0x001570F8 File Offset: 0x001552F8
		private void SwitchToParentJournal()
		{
			if (this._ownJournalScope != null)
			{
				this._ownJournalScope = null;
				this._navigationService.InvalidateJournalNavigationScope();
				JournalNavigationScope.ClearDPValues(this);
				foreach (CommandBinding commandBinding in this._commandBindings)
				{
					base.CommandBindings.Remove(commandBinding);
				}
				this._commandBindings = null;
			}
			this._journalOwnership = JournalOwnership.UsesParentJournal;
		}

		// Token: 0x06004C3E RID: 19518 RVA: 0x00157180 File Offset: 0x00155380
		private void AddCommandBinding(CommandBinding b)
		{
			base.CommandBindings.Add(b);
			if (this._commandBindings == null)
			{
				this._commandBindings = new List<CommandBinding>(6);
			}
			this._commandBindings.Add(b);
		}

		// Token: 0x17001295 RID: 4757
		// (get) Token: 0x06004C3F RID: 19519 RVA: 0x001571AF File Offset: 0x001553AF
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Frame._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Frame.Source" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Frame.Source" /> dependency property.</returns>
		// Token: 0x04002B02 RID: 11010
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(Frame), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(Frame.OnSourcePropertyChanged), new CoerceValueCallback(Frame.CoerceSource)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Frame.CanGoBack" /> dependency property.</summary>
		// Token: 0x04002B03 RID: 11011
		public static readonly DependencyProperty CanGoBackProperty = JournalNavigationScope.CanGoBackProperty.AddOwner(typeof(Frame));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Frame.CanGoForward" /> dependency property.</summary>
		// Token: 0x04002B04 RID: 11012
		public static readonly DependencyProperty CanGoForwardProperty = JournalNavigationScope.CanGoForwardProperty.AddOwner(typeof(Frame));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Frame.BackStack" /> dependency property.</summary>
		// Token: 0x04002B05 RID: 11013
		public static readonly DependencyProperty BackStackProperty = JournalNavigationScope.BackStackProperty.AddOwner(typeof(Frame));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Frame.ForwardStack" /> dependency property.</summary>
		// Token: 0x04002B06 RID: 11014
		public static readonly DependencyProperty ForwardStackProperty = JournalNavigationScope.ForwardStackProperty.AddOwner(typeof(Frame));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Frame.NavigationUIVisibility" /> dependency property.</summary>
		// Token: 0x04002B07 RID: 11015
		public static readonly DependencyProperty NavigationUIVisibilityProperty = DependencyProperty.Register("NavigationUIVisibility", typeof(NavigationUIVisibility), typeof(Frame), new PropertyMetadata(NavigationUIVisibility.Automatic));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Frame.SandboxExternalContent" /> dependency property.</summary>
		// Token: 0x04002B08 RID: 11016
		public static readonly DependencyProperty SandboxExternalContentProperty = DependencyProperty.Register("SandboxExternalContent", typeof(bool), typeof(Frame), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Frame.OnSandboxExternalContentPropertyChanged), new CoerceValueCallback(Frame.CoerceSandBoxExternalContentValue)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Frame.JournalOwnership" /> dependency property.</summary>
		// Token: 0x04002B09 RID: 11017
		public static readonly DependencyProperty JournalOwnershipProperty = DependencyProperty.Register("JournalOwnership", typeof(JournalOwnership), typeof(Frame), new FrameworkPropertyMetadata(JournalOwnership.Automatic, new PropertyChangedCallback(Frame.OnJournalOwnershipPropertyChanged), new CoerceValueCallback(Frame.CoerceJournalOwnership)), new ValidateValueCallback(Frame.ValidateJournalOwnershipValue));

		// Token: 0x04002B0A RID: 11018
		private bool _postContentRenderedFromLoadedHandler;

		// Token: 0x04002B0B RID: 11019
		private DispatcherOperation _contentRenderedCallback;

		// Token: 0x04002B0C RID: 11020
		private NavigationService _navigationService;

		// Token: 0x04002B0D RID: 11021
		private bool _sourceUpdatedFromNavService;

		// Token: 0x04002B0E RID: 11022
		private JournalOwnership _journalOwnership;

		// Token: 0x04002B0F RID: 11023
		private JournalNavigationScope _ownJournalScope;

		// Token: 0x04002B10 RID: 11024
		private List<CommandBinding> _commandBindings;

		// Token: 0x04002B11 RID: 11025
		private static DependencyObjectType _dType;

		// Token: 0x02000971 RID: 2417
		[Serializable]
		private class FramePersistState : CustomJournalStateInternal
		{
			// Token: 0x06008774 RID: 34676 RVA: 0x0024FE8C File Offset: 0x0024E08C
			internal override void PrepareForSerialization()
			{
				if (this.JournalEntry != null && this.JournalEntry.IsAlive())
				{
					this.JournalEntry = null;
				}
				if (this.Journal != null)
				{
					this.Journal.PruneKeepAliveEntries();
				}
			}

			// Token: 0x0400444B RID: 17483
			internal JournalEntry JournalEntry;

			// Token: 0x0400444C RID: 17484
			internal Guid NavSvcGuid;

			// Token: 0x0400444D RID: 17485
			internal JournalOwnership JournalOwnership;

			// Token: 0x0400444E RID: 17486
			internal Journal Journal;
		}
	}
}
