using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using MS.Internal;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Encapsulates a page of content that can be navigated to and hosted by Windows Internet Explorer, <see cref="T:System.Windows.Navigation.NavigationWindow" />, and <see cref="T:System.Windows.Controls.Frame" />.</summary>
	// Token: 0x02000508 RID: 1288
	[ContentProperty("Content")]
	public class Page : FrameworkElement, IWindowService, IAddChild
	{
		// Token: 0x0600528C RID: 21132 RVA: 0x0017090C File Offset: 0x0016EB0C
		static Page()
		{
			Window.IWindowServiceProperty.OverrideMetadata(typeof(Page), new FrameworkPropertyMetadata(new PropertyChangedCallback(Page._OnWindowServiceChanged)));
			UIElement.FocusableProperty.OverrideMetadata(typeof(Page), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Page), new FrameworkPropertyMetadata(typeof(Page)));
			Page._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Page));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Page" /> class.</summary>
		// Token: 0x0600528D RID: 21133 RVA: 0x00170AB8 File Offset: 0x0016ECB8
		public Page()
		{
			PropertyMetadata metadata = Page.TemplateProperty.GetMetadata(base.DependencyObjectType);
			ControlTemplate controlTemplate = (ControlTemplate)metadata.DefaultValue;
			if (controlTemplate != null)
			{
				Page.OnTemplateChanged(this, new DependencyPropertyChangedEventArgs(Page.TemplateProperty, metadata, null, controlTemplate));
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Windows.Markup.IAddChild.AddChild(System.Object)" />.</summary>
		/// <param name="obj">The child object to add.</param>
		// Token: 0x0600528E RID: 21134 RVA: 0x00170AFE File Offset: 0x0016ECFE
		void IAddChild.AddChild(object obj)
		{
			base.VerifyAccess();
			if (this.Content == null || obj == null)
			{
				this.Content = obj;
				return;
			}
			throw new InvalidOperationException(SR.Get("PageCannotHaveMultipleContent"));
		}

		/// <summary>For a description of this member, see <see cref="M:System.Windows.Markup.IAddChild.AddText(System.String)" />.</summary>
		/// <param name="str">The text to add to the object.</param>
		// Token: 0x0600528F RID: 21135 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string str)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(str, this);
		}

		/// <summary>Returns an enumerator for the logical child elements of a <see cref="T:System.Windows.Controls.Page" />.</summary>
		/// <returns>The <see cref="T:System.Collections.IEnumerator" /> for the logical child elements of a <see cref="T:System.Windows.Controls.Page" />.</returns>
		// Token: 0x17001401 RID: 5121
		// (get) Token: 0x06005290 RID: 21136 RVA: 0x00170B28 File Offset: 0x0016ED28
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				base.VerifyAccess();
				return new SingleChildEnumerator(this.Content);
			}
		}

		/// <summary>Gets or sets the content of a <see cref="T:System.Windows.Controls.Page" />.  </summary>
		/// <returns>An object that contains the content of a <see cref="T:System.Windows.Controls.Page" />. The default is <see cref="P:System.Windows.SystemFonts.MessageFontFamily" />.</returns>
		// Token: 0x17001402 RID: 5122
		// (get) Token: 0x06005291 RID: 21137 RVA: 0x00170B3B File Offset: 0x0016ED3B
		// (set) Token: 0x06005292 RID: 21138 RVA: 0x00170B4E File Offset: 0x0016ED4E
		public object Content
		{
			get
			{
				base.VerifyAccess();
				return base.GetValue(Page.ContentProperty);
			}
			set
			{
				base.VerifyAccess();
				base.SetValue(Page.ContentProperty, value);
			}
		}

		// Token: 0x17001403 RID: 5123
		// (get) Token: 0x06005293 RID: 21139 RVA: 0x00170B62 File Offset: 0x0016ED62
		// (set) Token: 0x06005294 RID: 21140 RVA: 0x00170B8D File Offset: 0x0016ED8D
		string IWindowService.Title
		{
			get
			{
				base.VerifyAccess();
				if (this.WindowService == null)
				{
					throw new InvalidOperationException(SR.Get("CannotQueryPropertiesWhenPageNotInTreeWithWindow"));
				}
				return this.WindowService.Title;
			}
			set
			{
				base.VerifyAccess();
				if (this.WindowService == null)
				{
					this.PageHelperObject._windowTitle = value;
					this.PropertyIsSet(SetPropertyFlags.WindowTitle);
					return;
				}
				if (this._isTopLevel)
				{
					this.WindowService.Title = value;
					this.PropertyIsSet(SetPropertyFlags.WindowTitle);
				}
			}
		}

		/// <summary>Gets or sets the title of the host <see cref="T:System.Windows.Window" /> or <see cref="T:System.Windows.Navigation.NavigationWindow" /> of a <see cref="T:System.Windows.Controls.Page" />.</summary>
		/// <returns>The title of a window that directly hosts the <see cref="T:System.Windows.Controls.Page" />.</returns>
		// Token: 0x17001404 RID: 5124
		// (get) Token: 0x06005295 RID: 21141 RVA: 0x00170BCC File Offset: 0x0016EDCC
		// (set) Token: 0x06005296 RID: 21142 RVA: 0x00170BDA File Offset: 0x0016EDDA
		[Localizability(LocalizationCategory.Title)]
		public string WindowTitle
		{
			get
			{
				base.VerifyAccess();
				return ((IWindowService)this).Title;
			}
			set
			{
				base.VerifyAccess();
				((IWindowService)this).Title = value;
			}
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x00170BE9 File Offset: 0x0016EDE9
		internal bool ShouldJournalWindowTitle()
		{
			return this.IsPropertySet(SetPropertyFlags.WindowTitle);
		}

		// Token: 0x17001405 RID: 5125
		// (get) Token: 0x06005298 RID: 21144 RVA: 0x00170BF2 File Offset: 0x0016EDF2
		// (set) Token: 0x06005299 RID: 21145 RVA: 0x00170C20 File Offset: 0x0016EE20
		double IWindowService.Height
		{
			get
			{
				base.VerifyAccess();
				if (this.WindowService == null)
				{
					throw new InvalidOperationException(SR.Get("CannotQueryPropertiesWhenPageNotInTreeWithWindow"));
				}
				return this.WindowService.Height;
			}
			set
			{
				base.VerifyAccess();
				if (this.WindowService == null)
				{
					this.PageHelperObject._windowHeight = value;
					this.PropertyIsSet(SetPropertyFlags.WindowHeight);
					return;
				}
				if (this._isTopLevel)
				{
					if (!this.WindowService.UserResized)
					{
						this.WindowService.Height = value;
					}
					this.PropertyIsSet(SetPropertyFlags.WindowHeight);
				}
			}
		}

		/// <summary>Gets or sets the height of the host <see cref="T:System.Windows.Window" /> or <see cref="T:System.Windows.Navigation.NavigationWindow" /> of a <see cref="T:System.Windows.Controls.Page" />.</summary>
		/// <returns>The height of a window that directly hosts a <see cref="T:System.Windows.Controls.Page" />.</returns>
		// Token: 0x17001406 RID: 5126
		// (get) Token: 0x0600529A RID: 21146 RVA: 0x00170C77 File Offset: 0x0016EE77
		// (set) Token: 0x0600529B RID: 21147 RVA: 0x00170C85 File Offset: 0x0016EE85
		public double WindowHeight
		{
			get
			{
				base.VerifyAccess();
				return ((IWindowService)this).Height;
			}
			set
			{
				base.VerifyAccess();
				((IWindowService)this).Height = value;
			}
		}

		// Token: 0x17001407 RID: 5127
		// (get) Token: 0x0600529C RID: 21148 RVA: 0x00170C94 File Offset: 0x0016EE94
		// (set) Token: 0x0600529D RID: 21149 RVA: 0x00170CC0 File Offset: 0x0016EEC0
		double IWindowService.Width
		{
			get
			{
				base.VerifyAccess();
				if (this.WindowService == null)
				{
					throw new InvalidOperationException(SR.Get("CannotQueryPropertiesWhenPageNotInTreeWithWindow"));
				}
				return this.WindowService.Width;
			}
			set
			{
				base.VerifyAccess();
				if (this.WindowService == null)
				{
					this.PageHelperObject._windowWidth = value;
					this.PropertyIsSet(SetPropertyFlags.WindowWidth);
					return;
				}
				if (this._isTopLevel)
				{
					if (!this.WindowService.UserResized)
					{
						this.WindowService.Width = value;
					}
					this.PropertyIsSet(SetPropertyFlags.WindowWidth);
				}
			}
		}

		/// <summary>Gets or sets the width of the host <see cref="T:System.Windows.Window" /> or <see cref="T:System.Windows.Navigation.NavigationWindow" /> of a <see cref="T:System.Windows.Controls.Page" />.</summary>
		/// <returns>The width of a window that directly hosts a <see cref="T:System.Windows.Controls.Page" />.</returns>
		// Token: 0x17001408 RID: 5128
		// (get) Token: 0x0600529E RID: 21150 RVA: 0x00170D17 File Offset: 0x0016EF17
		// (set) Token: 0x0600529F RID: 21151 RVA: 0x00170D25 File Offset: 0x0016EF25
		public double WindowWidth
		{
			get
			{
				base.VerifyAccess();
				return ((IWindowService)this).Width;
			}
			set
			{
				base.VerifyAccess();
				((IWindowService)this).Width = value;
			}
		}

		/// <summary>Gets or sets the background for a <see cref="T:System.Windows.Controls.Page" />.  </summary>
		/// <returns>The <see cref="T:System.Windows.Media.Brush" /> that <see cref="T:System.Windows.Controls.Page" /> uses to paint its background.</returns>
		// Token: 0x17001409 RID: 5129
		// (get) Token: 0x060052A0 RID: 21152 RVA: 0x00170D34 File Offset: 0x0016EF34
		// (set) Token: 0x060052A1 RID: 21153 RVA: 0x00170D46 File Offset: 0x0016EF46
		[Category("Appearance")]
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(Page.BackgroundProperty);
			}
			set
			{
				base.SetValue(Page.BackgroundProperty, value);
			}
		}

		/// <summary>Gets or sets the title of the <see cref="T:System.Windows.Controls.Page" />.  </summary>
		/// <returns>The title of the <see cref="T:System.Windows.Controls.Page" />.</returns>
		// Token: 0x1700140A RID: 5130
		// (get) Token: 0x060052A2 RID: 21154 RVA: 0x00170D54 File Offset: 0x0016EF54
		// (set) Token: 0x060052A3 RID: 21155 RVA: 0x00170D66 File Offset: 0x0016EF66
		public string Title
		{
			get
			{
				return (string)base.GetValue(Page.TitleProperty);
			}
			set
			{
				base.SetValue(Page.TitleProperty, value);
			}
		}

		// Token: 0x060052A4 RID: 21156 RVA: 0x00170D74 File Offset: 0x0016EF74
		private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Page)d).PropertyIsSet(SetPropertyFlags.Title);
		}

		/// <summary>Gets or sets a value that indicates whether the navigation UI of a <see cref="T:System.Windows.Navigation.NavigationWindow" /> on Microsoft Internet Explorer 6 is visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the navigation UI of a host <see cref="T:System.Windows.Navigation.NavigationWindow" /> is visible; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Controls.Page.ShowsNavigationUI" /> property is inspected on a <see cref="T:System.Windows.Controls.Page" /> instance that is not hosted by a <see cref="T:System.Windows.Window" />, <see cref="T:System.Windows.Navigation.NavigationWindow" />, or a browser.</exception>
		// Token: 0x1700140B RID: 5131
		// (get) Token: 0x060052A5 RID: 21157 RVA: 0x00170D84 File Offset: 0x0016EF84
		// (set) Token: 0x060052A6 RID: 21158 RVA: 0x00170DC6 File Offset: 0x0016EFC6
		public bool ShowsNavigationUI
		{
			get
			{
				base.VerifyAccess();
				if (this.WindowService == null)
				{
					throw new InvalidOperationException(SR.Get("CannotQueryPropertiesWhenPageNotInTreeWithWindow"));
				}
				NavigationWindow navigationWindow = this.WindowService as NavigationWindow;
				return navigationWindow != null && navigationWindow.ShowsNavigationUI;
			}
			set
			{
				base.VerifyAccess();
				if (this.WindowService == null)
				{
					this.PageHelperObject._showsNavigationUI = value;
					this.PropertyIsSet(SetPropertyFlags.ShowsNavigationUI);
					return;
				}
				if (this._isTopLevel)
				{
					this.SetShowsNavigationUI(value);
					this.PropertyIsSet(SetPropertyFlags.ShowsNavigationUI);
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.Page" /> instance is retained in navigation history.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.Page" /> instance is retained in navigation history; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700140C RID: 5132
		// (get) Token: 0x060052A7 RID: 21159 RVA: 0x00170E02 File Offset: 0x0016F002
		// (set) Token: 0x060052A8 RID: 21160 RVA: 0x00170E0A File Offset: 0x0016F00A
		public bool KeepAlive
		{
			get
			{
				return JournalEntry.GetKeepAlive(this);
			}
			set
			{
				JournalEntry.SetKeepAlive(this, value);
			}
		}

		/// <summary>Gets the navigation service that the host of the page is using to manage navigation.</summary>
		/// <returns>The <see cref="T:System.Windows.Navigation.NavigationService" /> object that the host of the page is using to manage navigation, or <see langword="null" /> if the host does not support navigation.</returns>
		// Token: 0x1700140D RID: 5133
		// (get) Token: 0x060052A9 RID: 21161 RVA: 0x00170E13 File Offset: 0x0016F013
		public NavigationService NavigationService
		{
			get
			{
				return NavigationService.GetNavigationService(this);
			}
		}

		/// <summary>Gets or sets the foreground for a <see cref="T:System.Windows.Controls.Page" />.  </summary>
		/// <returns>The <see cref="T:System.Windows.Media.Brush" /> that <see cref="T:System.Windows.Controls.Page" /> uses to paint its foreground. The default is <see cref="P:System.Windows.Media.Brushes.Black" />.</returns>
		// Token: 0x1700140E RID: 5134
		// (get) Token: 0x060052AA RID: 21162 RVA: 0x00170E1B File Offset: 0x0016F01B
		// (set) Token: 0x060052AB RID: 21163 RVA: 0x00170E2D File Offset: 0x0016F02D
		[Bindable(true)]
		[Category("Appearance")]
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(Page.ForegroundProperty);
			}
			set
			{
				base.SetValue(Page.ForegroundProperty, value);
			}
		}

		/// <summary>Gets or sets the name of the specified font family.  </summary>
		/// <returns>A <see cref="T:System.Windows.Media.FontFamily" /> that is the font family for the content of a <see cref="T:System.Windows.Controls.Page" />. The default is <see cref="P:System.Windows.SystemFonts.MessageFontFamily" />.</returns>
		// Token: 0x1700140F RID: 5135
		// (get) Token: 0x060052AC RID: 21164 RVA: 0x00170E3B File Offset: 0x0016F03B
		// (set) Token: 0x060052AD RID: 21165 RVA: 0x00170E4D File Offset: 0x0016F04D
		[Bindable(true)]
		[Category("Appearance")]
		[Localizability(LocalizationCategory.Font, Modifiability = Modifiability.Unmodifiable)]
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(Page.FontFamilyProperty);
			}
			set
			{
				base.SetValue(Page.FontFamilyProperty, value);
			}
		}

		/// <summary>Gets or sets the font size.  </summary>
		/// <returns>The font size for the content of a <see cref="T:System.Windows.Controls.Page" />. The default is <see cref="P:System.Windows.SystemFonts.MessageFontSize" />.</returns>
		// Token: 0x17001410 RID: 5136
		// (get) Token: 0x060052AE RID: 21166 RVA: 0x00170E5B File Offset: 0x0016F05B
		// (set) Token: 0x060052AF RID: 21167 RVA: 0x00170E6D File Offset: 0x0016F06D
		[TypeConverter(typeof(FontSizeConverter))]
		[Bindable(true)]
		[Category("Appearance")]
		[Localizability(LocalizationCategory.None)]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(Page.FontSizeProperty);
			}
			set
			{
				base.SetValue(Page.FontSizeProperty, value);
			}
		}

		/// <summary>Gets or sets the control template for a <see cref="T:System.Windows.Controls.Page" />.  </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.ControlTemplate" /> for a <see cref="T:System.Windows.Controls.Page" />.</returns>
		// Token: 0x17001411 RID: 5137
		// (get) Token: 0x060052B0 RID: 21168 RVA: 0x00170E80 File Offset: 0x0016F080
		// (set) Token: 0x060052B1 RID: 21169 RVA: 0x00170E88 File Offset: 0x0016F088
		public ControlTemplate Template
		{
			get
			{
				return this._templateCache;
			}
			set
			{
				base.SetValue(Page.TemplateProperty, value);
			}
		}

		// Token: 0x17001412 RID: 5138
		// (get) Token: 0x060052B2 RID: 21170 RVA: 0x00170E96 File Offset: 0x0016F096
		internal override FrameworkTemplate TemplateInternal
		{
			get
			{
				return this.Template;
			}
		}

		// Token: 0x17001413 RID: 5139
		// (get) Token: 0x060052B3 RID: 21171 RVA: 0x00170E80 File Offset: 0x0016F080
		// (set) Token: 0x060052B4 RID: 21172 RVA: 0x00170E9E File Offset: 0x0016F09E
		internal override FrameworkTemplate TemplateCache
		{
			get
			{
				return this._templateCache;
			}
			set
			{
				this._templateCache = (ControlTemplate)value;
			}
		}

		// Token: 0x060052B5 RID: 21173 RVA: 0x00170EAC File Offset: 0x0016F0AC
		internal override void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			this.OnTemplateChanged((ControlTemplate)oldTemplate, (ControlTemplate)newTemplate);
		}

		// Token: 0x060052B6 RID: 21174 RVA: 0x00170EC0 File Offset: 0x0016F0C0
		private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Page fe = (Page)d;
			StyleHelper.UpdateTemplateCache(fe, (FrameworkTemplate)e.OldValue, (FrameworkTemplate)e.NewValue, Page.TemplateProperty);
		}

		/// <summary>Called when the template for a <see cref="T:System.Windows.Controls.Page" /> changes.</summary>
		/// <param name="oldTemplate">The old template.</param>
		/// <param name="newTemplate">The new template. </param>
		// Token: 0x060052B7 RID: 21175 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
		}

		/// <summary>Measures the child elements of the <see cref="T:System.Windows.Controls.Page" />.</summary>
		/// <param name="constraint">The available area that the window can give to its children.</param>
		/// <returns>A <see cref="T:System.Windows.Size" /> that is the actual size of the window. The method may return a larger value, in which case the parent may need to add scroll bars.</returns>
		// Token: 0x060052B8 RID: 21176 RVA: 0x00170EF8 File Offset: 0x0016F0F8
		protected override Size MeasureOverride(Size constraint)
		{
			base.VerifyAccess();
			int visualChildrenCount = this.VisualChildrenCount;
			if (visualChildrenCount > 0)
			{
				UIElement uielement = this.GetVisualChild(0) as UIElement;
				if (uielement != null)
				{
					uielement.Measure(constraint);
					return uielement.DesiredSize;
				}
			}
			return new Size(0.0, 0.0);
		}

		/// <summary>Arranges the content (child elements) of the <see cref="T:System.Windows.Controls.Page" />. </summary>
		/// <param name="arrangeBounds">The size to use to arrange the child elements.</param>
		/// <returns>A <see cref="T:System.Windows.Size" /> that represents the arranged size of the page.</returns>
		// Token: 0x060052B9 RID: 21177 RVA: 0x00170F4C File Offset: 0x0016F14C
		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			base.VerifyAccess();
			int visualChildrenCount = this.VisualChildrenCount;
			if (visualChildrenCount > 0)
			{
				UIElement uielement = this.GetVisualChild(0) as UIElement;
				if (uielement != null)
				{
					uielement.Arrange(new Rect(default(Point), arrangeBounds));
				}
			}
			return arrangeBounds;
		}

		/// <summary>Called when the parent of the <see cref="T:System.Windows.Controls.Page" /> is changed.</summary>
		/// <param name="oldParent">The previous parent. Set to <see langword="null" /> if the <see cref="T:System.Windows.DependencyObject" /> did not have a previous parent.</param>
		/// <exception cref="T:System.InvalidOperationException">The new parent is neither a <see cref="T:System.Windows.Window" /> nor a <see cref="T:System.Windows.Controls.Frame" />.</exception>
		// Token: 0x060052BA RID: 21178 RVA: 0x00170F90 File Offset: 0x0016F190
		protected internal sealed override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.VerifyAccess();
			base.OnVisualParentChanged(oldParent);
			Visual visual = VisualTreeHelper.GetParent(this) as Visual;
			if (visual == null || base.Parent is Window || (this.NavigationService != null && this.NavigationService.Content == this))
			{
				return;
			}
			bool flag = false;
			FrameworkElement frameworkElement = visual as FrameworkElement;
			if (frameworkElement != null)
			{
				DependencyObject dependencyObject = frameworkElement;
				while (frameworkElement != null && frameworkElement.TemplatedParent != null)
				{
					dependencyObject = frameworkElement.TemplatedParent;
					frameworkElement = (dependencyObject as FrameworkElement);
					if (frameworkElement is Frame)
					{
						break;
					}
				}
				if (dependencyObject is Window || dependencyObject is Frame)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				throw new InvalidOperationException(SR.Get("ParentOfPageMustBeWindowOrFrame"));
			}
		}

		// Token: 0x060052BB RID: 21179 RVA: 0x00171034 File Offset: 0x0016F234
		private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Page page = (Page)d;
			page.OnContentChanged(e.OldValue, e.NewValue);
		}

		// Token: 0x060052BC RID: 21180 RVA: 0x001602A4 File Offset: 0x0015E4A4
		private void OnContentChanged(object oldContent, object newContent)
		{
			base.RemoveLogicalChild(oldContent);
			base.AddLogicalChild(newContent);
		}

		// Token: 0x060052BD RID: 21181 RVA: 0x0017105C File Offset: 0x0016F25C
		private static void _OnWindowServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Page page = d as Page;
			page.OnWindowServiceChanged(e.NewValue as IWindowService);
		}

		// Token: 0x060052BE RID: 21182 RVA: 0x00171082 File Offset: 0x0016F282
		private void OnWindowServiceChanged(IWindowService iws)
		{
			this._currentIws = iws;
			this.DetermineTopLevel();
			if (this._currentIws != null && this._isTopLevel)
			{
				this.PropagateProperties();
			}
		}

		// Token: 0x060052BF RID: 21183 RVA: 0x001710A8 File Offset: 0x0016F2A8
		private void DetermineTopLevel()
		{
			FrameworkElement frameworkElement = base.Parent as FrameworkElement;
			if (frameworkElement != null && frameworkElement.InheritanceBehavior == InheritanceBehavior.Default)
			{
				this._isTopLevel = true;
				return;
			}
			this._isTopLevel = false;
		}

		// Token: 0x060052C0 RID: 21184 RVA: 0x001710DC File Offset: 0x0016F2DC
		private void PropagateProperties()
		{
			if (this._pho == null)
			{
				return;
			}
			if (this.IsPropertySet(SetPropertyFlags.WindowTitle))
			{
				this._currentIws.Title = this.PageHelperObject._windowTitle;
			}
			if (this.IsPropertySet(SetPropertyFlags.WindowHeight) && !this._currentIws.UserResized)
			{
				this._currentIws.Height = this.PageHelperObject._windowHeight;
			}
			if (this.IsPropertySet(SetPropertyFlags.WindowWidth) && !this._currentIws.UserResized)
			{
				this._currentIws.Width = this.PageHelperObject._windowWidth;
			}
			if (this.IsPropertySet(SetPropertyFlags.ShowsNavigationUI))
			{
				this.SetShowsNavigationUI(this.PageHelperObject._showsNavigationUI);
			}
		}

		// Token: 0x17001414 RID: 5140
		// (get) Token: 0x060052C1 RID: 21185 RVA: 0x00171184 File Offset: 0x0016F384
		bool IWindowService.UserResized
		{
			get
			{
				Invariant.Assert(this._currentIws != null, "_currentIws cannot be null here.");
				return this._currentIws.UserResized;
			}
		}

		// Token: 0x060052C2 RID: 21186 RVA: 0x001711A4 File Offset: 0x0016F3A4
		private void SetShowsNavigationUI(bool showsNavigationUI)
		{
			NavigationWindow navigationWindow = this._currentIws as NavigationWindow;
			if (navigationWindow != null)
			{
				navigationWindow.ShowsNavigationUI = showsNavigationUI;
			}
		}

		// Token: 0x060052C3 RID: 21187 RVA: 0x001711C7 File Offset: 0x0016F3C7
		private bool IsPropertySet(SetPropertyFlags property)
		{
			return (this._setPropertyFlags & property) > SetPropertyFlags.None;
		}

		// Token: 0x060052C4 RID: 21188 RVA: 0x001711D4 File Offset: 0x0016F3D4
		private void PropertyIsSet(SetPropertyFlags property)
		{
			this._setPropertyFlags |= property;
		}

		/// <summary>Allows derived classes to determine the serialization behavior of the <see cref="P:System.Windows.Controls.Page.WindowTitle" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if the content should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x060052C5 RID: 21189 RVA: 0x00170BE9 File Offset: 0x0016EDE9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeWindowTitle()
		{
			return this.IsPropertySet(SetPropertyFlags.WindowTitle);
		}

		/// <summary>Allows derived classes to determine the serialization behavior of the <see cref="P:System.Windows.Controls.Page.WindowHeight" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if the content should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x060052C6 RID: 21190 RVA: 0x001711E4 File Offset: 0x0016F3E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeWindowHeight()
		{
			return this.IsPropertySet(SetPropertyFlags.WindowHeight);
		}

		/// <summary>Allows derived classes to determine the serialization behavior of the <see cref="P:System.Windows.Controls.Page.WindowWidth" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if the content should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x060052C7 RID: 21191 RVA: 0x001711ED File Offset: 0x0016F3ED
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeWindowWidth()
		{
			return this.IsPropertySet(SetPropertyFlags.WindowWidth);
		}

		/// <summary>Allows derived classes to determine the serialization behavior of the <see cref="P:System.Windows.Controls.Page.Title" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if the content should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x060052C8 RID: 21192 RVA: 0x001711F6 File Offset: 0x0016F3F6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeTitle()
		{
			return this.IsPropertySet(SetPropertyFlags.Title);
		}

		/// <summary>Allows derived classes to determine the serialization behavior of the <see cref="P:System.Windows.Controls.Page.ShowsNavigationUI" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if the content should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x060052C9 RID: 21193 RVA: 0x001711FF File Offset: 0x0016F3FF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeShowsNavigationUI()
		{
			return this.IsPropertySet(SetPropertyFlags.ShowsNavigationUI);
		}

		// Token: 0x17001415 RID: 5141
		// (get) Token: 0x060052CA RID: 21194 RVA: 0x00171209 File Offset: 0x0016F409
		private IWindowService WindowService
		{
			get
			{
				return this._currentIws;
			}
		}

		// Token: 0x17001416 RID: 5142
		// (get) Token: 0x060052CB RID: 21195 RVA: 0x00171211 File Offset: 0x0016F411
		private PageHelperObject PageHelperObject
		{
			get
			{
				if (this._pho == null)
				{
					this._pho = new PageHelperObject();
				}
				return this._pho;
			}
		}

		// Token: 0x17001417 RID: 5143
		// (get) Token: 0x060052CC RID: 21196 RVA: 0x0003BBDF File Offset: 0x00039DDF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 19;
			}
		}

		// Token: 0x17001418 RID: 5144
		// (get) Token: 0x060052CD RID: 21197 RVA: 0x0017122C File Offset: 0x0016F42C
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Page._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Page.Content" /> dependency property.</summary>
		/// <returns>The identifier the <see cref="P:System.Windows.Controls.Page.Content" /> dependency property.</returns>
		// Token: 0x04002CB7 RID: 11447
		public static readonly DependencyProperty ContentProperty = ContentControl.ContentProperty.AddOwner(typeof(Page), new FrameworkPropertyMetadata(new PropertyChangedCallback(Page.OnContentChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Page.Background" /> dependency property.</summary>
		/// <returns>The identifier the <see cref="P:System.Windows.Controls.Page.Background" /> dependency property.</returns>
		// Token: 0x04002CB8 RID: 11448
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(Page), new FrameworkPropertyMetadata(Panel.BackgroundProperty.GetDefaultValue(typeof(Panel)), FrameworkPropertyMetadataOptions.None));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Page.Title" /> dependency property.</summary>
		// Token: 0x04002CB9 RID: 11449
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Page), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Page.OnTitleChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Page.KeepAlive" /> dependency property.</summary>
		// Token: 0x04002CBA RID: 11450
		public static readonly DependencyProperty KeepAliveProperty = JournalEntry.KeepAliveProperty.AddOwner(typeof(Page));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Page.Foreground" /> dependency property.</summary>
		// Token: 0x04002CBB RID: 11451
		public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(Page));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Page.FontFamily" /> dependency property.</summary>
		// Token: 0x04002CBC RID: 11452
		public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(Page));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Page.FontSize" /> dependency property.</summary>
		// Token: 0x04002CBD RID: 11453
		public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(Page));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Page.Template" /> dependency property.</summary>
		/// <returns>The identifier the <see cref="P:System.Windows.Controls.Page.Template" /> dependency property.</returns>
		// Token: 0x04002CBE RID: 11454
		public static readonly DependencyProperty TemplateProperty = Control.TemplateProperty.AddOwner(typeof(Page), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(Page.OnTemplateChanged)));

		// Token: 0x04002CBF RID: 11455
		private IWindowService _currentIws;

		// Token: 0x04002CC0 RID: 11456
		private PageHelperObject _pho;

		// Token: 0x04002CC1 RID: 11457
		private SetPropertyFlags _setPropertyFlags;

		// Token: 0x04002CC2 RID: 11458
		private bool _isTopLevel;

		// Token: 0x04002CC3 RID: 11459
		private ControlTemplate _templateCache;

		// Token: 0x04002CC4 RID: 11460
		private static DependencyObjectType _dType;
	}
}
