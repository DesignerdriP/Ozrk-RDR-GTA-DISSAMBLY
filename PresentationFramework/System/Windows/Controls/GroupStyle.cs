using System;
using System.ComponentModel;

namespace System.Windows.Controls
{
	/// <summary>Defines how you want the group to look at each level.</summary>
	// Token: 0x020004E1 RID: 1249
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class GroupStyle : INotifyPropertyChanged
	{
		// Token: 0x06004DE3 RID: 19939 RVA: 0x0015F798 File Offset: 0x0015D998
		static GroupStyle()
		{
			ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(StackPanel)));
			itemsPanelTemplate.Seal();
			GroupStyle.DefaultGroupPanel = itemsPanelTemplate;
			GroupStyle.DefaultStackPanel = itemsPanelTemplate;
			itemsPanelTemplate = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(VirtualizingStackPanel)));
			itemsPanelTemplate.Seal();
			GroupStyle.DefaultVirtualizingStackPanel = itemsPanelTemplate;
			GroupStyle.s_DefaultGroupStyle = new GroupStyle();
		}

		/// <summary>Occurs when a property value changes.</summary>
		// Token: 0x140000DD RID: 221
		// (add) Token: 0x06004DE4 RID: 19940 RVA: 0x0015F7F7 File Offset: 0x0015D9F7
		// (remove) Token: 0x06004DE5 RID: 19941 RVA: 0x0015F800 File Offset: 0x0015DA00
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this.PropertyChanged += value;
			}
			remove
			{
				this.PropertyChanged -= value;
			}
		}

		/// <summary>Occurs when a property value changes.</summary>
		// Token: 0x140000DE RID: 222
		// (add) Token: 0x06004DE6 RID: 19942 RVA: 0x0015F80C File Offset: 0x0015DA0C
		// (remove) Token: 0x06004DE7 RID: 19943 RVA: 0x0015F844 File Offset: 0x0015DA44
		protected virtual event PropertyChangedEventHandler PropertyChanged;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.GroupStyle.PropertyChanged" /> event using the provided arguments.</summary>
		/// <param name="e">Arguments of the event being raised.</param>
		// Token: 0x06004DE8 RID: 19944 RVA: 0x0015F879 File Offset: 0x0015DA79
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, e);
			}
		}

		/// <summary>Gets or sets a template that creates the panel used to layout the items.</summary>
		/// <returns>An <see cref="T:System.Windows.Controls.ItemsPanelTemplate" /> object that creates the panel used to layout the items.</returns>
		// Token: 0x170012F1 RID: 4849
		// (get) Token: 0x06004DE9 RID: 19945 RVA: 0x0015F890 File Offset: 0x0015DA90
		// (set) Token: 0x06004DEA RID: 19946 RVA: 0x0015F898 File Offset: 0x0015DA98
		public ItemsPanelTemplate Panel
		{
			get
			{
				return this._panel;
			}
			set
			{
				this._panel = value;
				this.OnPropertyChanged("Panel");
			}
		}

		/// <summary>Gets or sets the style that is applied to the <see cref="T:System.Windows.Controls.GroupItem" /> generated for each item.</summary>
		/// <returns>The style that is applied to the <see cref="T:System.Windows.Controls.GroupItem" /> generated for each item. The default is <see langword="null" />.</returns>
		// Token: 0x170012F2 RID: 4850
		// (get) Token: 0x06004DEB RID: 19947 RVA: 0x0015F8AC File Offset: 0x0015DAAC
		// (set) Token: 0x06004DEC RID: 19948 RVA: 0x0015F8B4 File Offset: 0x0015DAB4
		[DefaultValue(null)]
		public Style ContainerStyle
		{
			get
			{
				return this._containerStyle;
			}
			set
			{
				this._containerStyle = value;
				this.OnPropertyChanged("ContainerStyle");
			}
		}

		/// <summary>Enables the application writer to provide custom selection logic for a style to apply to each generated <see cref="T:System.Windows.Controls.GroupItem" />.</summary>
		/// <returns>An object that derives from <see cref="T:System.Windows.Controls.StyleSelector" />. The default is <see langword="null" />.</returns>
		// Token: 0x170012F3 RID: 4851
		// (get) Token: 0x06004DED RID: 19949 RVA: 0x0015F8C8 File Offset: 0x0015DAC8
		// (set) Token: 0x06004DEE RID: 19950 RVA: 0x0015F8D0 File Offset: 0x0015DAD0
		[DefaultValue(null)]
		public StyleSelector ContainerStyleSelector
		{
			get
			{
				return this._containerStyleSelector;
			}
			set
			{
				this._containerStyleSelector = value;
				this.OnPropertyChanged("ContainerStyleSelector");
			}
		}

		/// <summary>Gets or sets the template that is used to display the group header.</summary>
		/// <returns>A <see cref="T:System.Windows.DataTemplate" /> object that is used to display the group header. The default is <see langword="null" />.</returns>
		// Token: 0x170012F4 RID: 4852
		// (get) Token: 0x06004DEF RID: 19951 RVA: 0x0015F8E4 File Offset: 0x0015DAE4
		// (set) Token: 0x06004DF0 RID: 19952 RVA: 0x0015F8EC File Offset: 0x0015DAEC
		[DefaultValue(null)]
		public DataTemplate HeaderTemplate
		{
			get
			{
				return this._headerTemplate;
			}
			set
			{
				this._headerTemplate = value;
				this.OnPropertyChanged("HeaderTemplate");
			}
		}

		/// <summary>Enables the application writer to provide custom selection logic for a template that is used to display the group header.</summary>
		/// <returns>An object that derives from <see cref="T:System.Windows.Controls.DataTemplateSelector" />. The default is <see langword="null" />.</returns>
		// Token: 0x170012F5 RID: 4853
		// (get) Token: 0x06004DF1 RID: 19953 RVA: 0x0015F900 File Offset: 0x0015DB00
		// (set) Token: 0x06004DF2 RID: 19954 RVA: 0x0015F908 File Offset: 0x0015DB08
		[DefaultValue(null)]
		public DataTemplateSelector HeaderTemplateSelector
		{
			get
			{
				return this._headerTemplateSelector;
			}
			set
			{
				this._headerTemplateSelector = value;
				this.OnPropertyChanged("HeaderTemplateSelector");
			}
		}

		/// <summary>Gets or sets a composite string that specifies how to format the header if it is displayed as a string.</summary>
		/// <returns>A composite string that specifies how to format the header if it is displayed as a string.</returns>
		// Token: 0x170012F6 RID: 4854
		// (get) Token: 0x06004DF3 RID: 19955 RVA: 0x0015F91C File Offset: 0x0015DB1C
		// (set) Token: 0x06004DF4 RID: 19956 RVA: 0x0015F924 File Offset: 0x0015DB24
		[DefaultValue(null)]
		public string HeaderStringFormat
		{
			get
			{
				return this._headerStringFormat;
			}
			set
			{
				this._headerStringFormat = value;
				this.OnPropertyChanged("HeaderStringFormat");
			}
		}

		/// <summary>Gets or sets a value that indicates whether items corresponding to empty groups should be displayed.</summary>
		/// <returns>
		///     <see langword="true" /> to not display empty groups; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170012F7 RID: 4855
		// (get) Token: 0x06004DF5 RID: 19957 RVA: 0x0015F938 File Offset: 0x0015DB38
		// (set) Token: 0x06004DF6 RID: 19958 RVA: 0x0015F940 File Offset: 0x0015DB40
		[DefaultValue(false)]
		public bool HidesIfEmpty
		{
			get
			{
				return this._hidesIfEmpty;
			}
			set
			{
				this._hidesIfEmpty = value;
				this.OnPropertyChanged("HidesIfEmpty");
			}
		}

		/// <summary>Gets or sets the number of alternating <see cref="T:System.Windows.Controls.GroupItem" /> objects.</summary>
		/// <returns>The number of alternating <see cref="T:System.Windows.Controls.GroupItem" /> objects.</returns>
		// Token: 0x170012F8 RID: 4856
		// (get) Token: 0x06004DF7 RID: 19959 RVA: 0x0015F954 File Offset: 0x0015DB54
		// (set) Token: 0x06004DF8 RID: 19960 RVA: 0x0015F95C File Offset: 0x0015DB5C
		[DefaultValue(0)]
		public int AlternationCount
		{
			get
			{
				return this._alternationCount;
			}
			set
			{
				this._alternationCount = value;
				this._isAlternationCountSet = true;
				this.OnPropertyChanged("AlternationCount");
			}
		}

		/// <summary>Gets the default style of the group.</summary>
		/// <returns>The default style of the group.</returns>
		// Token: 0x170012F9 RID: 4857
		// (get) Token: 0x06004DF9 RID: 19961 RVA: 0x0015F977 File Offset: 0x0015DB77
		public static GroupStyle Default
		{
			get
			{
				return GroupStyle.s_DefaultGroupStyle;
			}
		}

		// Token: 0x06004DFA RID: 19962 RVA: 0x0015F97E File Offset: 0x0015DB7E
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x170012FA RID: 4858
		// (get) Token: 0x06004DFB RID: 19963 RVA: 0x0015F98C File Offset: 0x0015DB8C
		internal bool IsAlternationCountSet
		{
			get
			{
				return this._isAlternationCountSet;
			}
		}

		/// <summary>Identifies the default <see cref="T:System.Windows.Controls.ItemsPanelTemplate" /> that creates the panel used to layout the items.</summary>
		// Token: 0x04002B9D RID: 11165
		public static readonly ItemsPanelTemplate DefaultGroupPanel;

		// Token: 0x04002B9E RID: 11166
		private ItemsPanelTemplate _panel;

		// Token: 0x04002B9F RID: 11167
		private Style _containerStyle;

		// Token: 0x04002BA0 RID: 11168
		private StyleSelector _containerStyleSelector;

		// Token: 0x04002BA1 RID: 11169
		private DataTemplate _headerTemplate;

		// Token: 0x04002BA2 RID: 11170
		private DataTemplateSelector _headerTemplateSelector;

		// Token: 0x04002BA3 RID: 11171
		private string _headerStringFormat;

		// Token: 0x04002BA4 RID: 11172
		private bool _hidesIfEmpty;

		// Token: 0x04002BA5 RID: 11173
		private bool _isAlternationCountSet;

		// Token: 0x04002BA6 RID: 11174
		private int _alternationCount;

		// Token: 0x04002BA7 RID: 11175
		private static GroupStyle s_DefaultGroupStyle;

		// Token: 0x04002BA8 RID: 11176
		internal static ItemsPanelTemplate DefaultStackPanel;

		// Token: 0x04002BA9 RID: 11177
		internal static ItemsPanelTemplate DefaultVirtualizingStackPanel;
	}
}
