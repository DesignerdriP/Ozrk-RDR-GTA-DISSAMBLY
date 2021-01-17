using System;
using MS.Internal.Text;

namespace System.Windows.Documents
{
	/// <summary>Provides access to a rich set of OpenType typography properties.</summary>
	// Token: 0x0200042B RID: 1067
	public sealed class Typography
	{
		// Token: 0x06003E39 RID: 15929 RVA: 0x0011D869 File Offset: 0x0011BA69
		internal Typography(DependencyObject owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this._owner = owner;
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x0011D888 File Offset: 0x0011BA88
		static Typography()
		{
			Typography.Default.SetStandardLigatures(true);
			Typography.Default.SetContextualAlternates(true);
			Typography.Default.SetContextualLigatures(true);
			Typography.Default.SetKerning(true);
		}

		/// <summary>Gets or sets a value that indicates whether standard ligatures are enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if standard ligatures are enabled; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x06003E3B RID: 15931 RVA: 0x0011E108 File Offset: 0x0011C308
		// (set) Token: 0x06003E3C RID: 15932 RVA: 0x0011E11F File Offset: 0x0011C31F
		public bool StandardLigatures
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StandardLigaturesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.StandardLigaturesProperty, value);
			}
		}

		/// <summary>Gets or sets a value that determines whether contextual ligatures are enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if contextual ligatures are enabled; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x06003E3D RID: 15933 RVA: 0x0011E132 File Offset: 0x0011C332
		// (set) Token: 0x06003E3E RID: 15934 RVA: 0x0011E149 File Offset: 0x0011C349
		public bool ContextualLigatures
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.ContextualLigaturesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.ContextualLigaturesProperty, value);
			}
		}

		/// <summary>Gets or sets a value that determines whether discretionary ligatures are enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if discretionary ligatures are enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x06003E3F RID: 15935 RVA: 0x0011E15C File Offset: 0x0011C35C
		// (set) Token: 0x06003E40 RID: 15936 RVA: 0x0011E173 File Offset: 0x0011C373
		public bool DiscretionaryLigatures
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.DiscretionaryLigaturesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.DiscretionaryLigaturesProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether historical ligatures are enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if historical ligatures are enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x06003E41 RID: 15937 RVA: 0x0011E186 File Offset: 0x0011C386
		// (set) Token: 0x06003E42 RID: 15938 RVA: 0x0011E19D File Offset: 0x0011C39D
		public bool HistoricalLigatures
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.HistoricalLigaturesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.HistoricalLigaturesProperty, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the index of an alternate annotation form. </summary>
		/// <returns>The index of the alternate annotation form. The default value is 0 (zero).</returns>
		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x06003E43 RID: 15939 RVA: 0x0011E1B0 File Offset: 0x0011C3B0
		// (set) Token: 0x06003E44 RID: 15940 RVA: 0x0011E1C7 File Offset: 0x0011C3C7
		public int AnnotationAlternates
		{
			get
			{
				return (int)this._owner.GetValue(Typography.AnnotationAlternatesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.AnnotationAlternatesProperty, value);
			}
		}

		/// <summary>Gets or sets a value that determines whether custom glyph forms can be used based upon the context of the text being rendered. </summary>
		/// <returns>
		///     <see langword="true" /> if custom glyph forms can be used; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x06003E45 RID: 15941 RVA: 0x0011E1DF File Offset: 0x0011C3DF
		// (set) Token: 0x06003E46 RID: 15942 RVA: 0x0011E1F6 File Offset: 0x0011C3F6
		public bool ContextualAlternates
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.ContextualAlternatesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.ContextualAlternatesProperty, value);
			}
		}

		/// <summary>Gets or sets a value that determines whether historical forms are enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if historical forms are enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x06003E47 RID: 15943 RVA: 0x0011E209 File Offset: 0x0011C409
		// (set) Token: 0x06003E48 RID: 15944 RVA: 0x0011E220 File Offset: 0x0011C420
		public bool HistoricalForms
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.HistoricalFormsProperty);
			}
			set
			{
				this._owner.SetValue(Typography.HistoricalFormsProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether kerning is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if kerning is enabled; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x06003E49 RID: 15945 RVA: 0x0011E233 File Offset: 0x0011C433
		// (set) Token: 0x06003E4A RID: 15946 RVA: 0x0011E24A File Offset: 0x0011C44A
		public bool Kerning
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.KerningProperty);
			}
			set
			{
				this._owner.SetValue(Typography.KerningProperty, value);
			}
		}

		/// <summary>Gets or sets a value that determines whether inter-glyph spacing for all-capital text is globally adjusted to improve readability. </summary>
		/// <returns>
		///     <see langword="true" /> if spacing is adjusted; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F8F RID: 3983
		// (get) Token: 0x06003E4B RID: 15947 RVA: 0x0011E25D File Offset: 0x0011C45D
		// (set) Token: 0x06003E4C RID: 15948 RVA: 0x0011E274 File Offset: 0x0011C474
		public bool CapitalSpacing
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.CapitalSpacingProperty);
			}
			set
			{
				this._owner.SetValue(Typography.CapitalSpacingProperty, value);
			}
		}

		/// <summary>Gets or sets a value that determines whether glyphs adjust their vertical position to better align with uppercase glyphs. </summary>
		/// <returns>
		///     <see langword="true" /> if the vertical position is adjusted; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F90 RID: 3984
		// (get) Token: 0x06003E4D RID: 15949 RVA: 0x0011E287 File Offset: 0x0011C487
		// (set) Token: 0x06003E4E RID: 15950 RVA: 0x0011E29E File Offset: 0x0011C49E
		public bool CaseSensitiveForms
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.CaseSensitiveFormsProperty);
			}
			set
			{
				this._owner.SetValue(Typography.CaseSensitiveFormsProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x06003E4F RID: 15951 RVA: 0x0011E2B1 File Offset: 0x0011C4B1
		// (set) Token: 0x06003E50 RID: 15952 RVA: 0x0011E2C8 File Offset: 0x0011C4C8
		public bool StylisticSet1
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet1Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet1Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x06003E51 RID: 15953 RVA: 0x0011E2DB File Offset: 0x0011C4DB
		// (set) Token: 0x06003E52 RID: 15954 RVA: 0x0011E2F2 File Offset: 0x0011C4F2
		public bool StylisticSet2
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet2Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet2Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x06003E53 RID: 15955 RVA: 0x0011E305 File Offset: 0x0011C505
		// (set) Token: 0x06003E54 RID: 15956 RVA: 0x0011E31C File Offset: 0x0011C51C
		public bool StylisticSet3
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet3Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet3Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x06003E55 RID: 15957 RVA: 0x0011E32F File Offset: 0x0011C52F
		// (set) Token: 0x06003E56 RID: 15958 RVA: 0x0011E346 File Offset: 0x0011C546
		public bool StylisticSet4
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet4Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet4Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x06003E57 RID: 15959 RVA: 0x0011E359 File Offset: 0x0011C559
		// (set) Token: 0x06003E58 RID: 15960 RVA: 0x0011E370 File Offset: 0x0011C570
		public bool StylisticSet5
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet5Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet5Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x06003E59 RID: 15961 RVA: 0x0011E383 File Offset: 0x0011C583
		// (set) Token: 0x06003E5A RID: 15962 RVA: 0x0011E39A File Offset: 0x0011C59A
		public bool StylisticSet6
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet6Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet6Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x06003E5B RID: 15963 RVA: 0x0011E3AD File Offset: 0x0011C5AD
		// (set) Token: 0x06003E5C RID: 15964 RVA: 0x0011E3C4 File Offset: 0x0011C5C4
		public bool StylisticSet7
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet7Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet7Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x06003E5D RID: 15965 RVA: 0x0011E3D7 File Offset: 0x0011C5D7
		// (set) Token: 0x06003E5E RID: 15966 RVA: 0x0011E3EE File Offset: 0x0011C5EE
		public bool StylisticSet8
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet8Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet8Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x06003E5F RID: 15967 RVA: 0x0011E401 File Offset: 0x0011C601
		// (set) Token: 0x06003E60 RID: 15968 RVA: 0x0011E418 File Offset: 0x0011C618
		public bool StylisticSet9
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet9Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet9Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x06003E61 RID: 15969 RVA: 0x0011E42B File Offset: 0x0011C62B
		// (set) Token: 0x06003E62 RID: 15970 RVA: 0x0011E442 File Offset: 0x0011C642
		public bool StylisticSet10
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet10Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet10Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x06003E63 RID: 15971 RVA: 0x0011E455 File Offset: 0x0011C655
		// (set) Token: 0x06003E64 RID: 15972 RVA: 0x0011E46C File Offset: 0x0011C66C
		public bool StylisticSet11
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet11Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet11Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x06003E65 RID: 15973 RVA: 0x0011E47F File Offset: 0x0011C67F
		// (set) Token: 0x06003E66 RID: 15974 RVA: 0x0011E496 File Offset: 0x0011C696
		public bool StylisticSet12
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet12Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet12Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x06003E67 RID: 15975 RVA: 0x0011E4A9 File Offset: 0x0011C6A9
		// (set) Token: 0x06003E68 RID: 15976 RVA: 0x0011E4C0 File Offset: 0x0011C6C0
		public bool StylisticSet13
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet13Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet13Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x06003E69 RID: 15977 RVA: 0x0011E4D3 File Offset: 0x0011C6D3
		// (set) Token: 0x06003E6A RID: 15978 RVA: 0x0011E4EA File Offset: 0x0011C6EA
		public bool StylisticSet14
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet14Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet14Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x06003E6B RID: 15979 RVA: 0x0011E4FD File Offset: 0x0011C6FD
		// (set) Token: 0x06003E6C RID: 15980 RVA: 0x0011E514 File Offset: 0x0011C714
		public bool StylisticSet15
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet15Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet15Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x06003E6D RID: 15981 RVA: 0x0011E527 File Offset: 0x0011C727
		// (set) Token: 0x06003E6E RID: 15982 RVA: 0x0011E53E File Offset: 0x0011C73E
		public bool StylisticSet16
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet16Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet16Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x06003E6F RID: 15983 RVA: 0x0011E551 File Offset: 0x0011C751
		// (set) Token: 0x06003E70 RID: 15984 RVA: 0x0011E568 File Offset: 0x0011C768
		public bool StylisticSet17
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet17Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet17Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x06003E71 RID: 15985 RVA: 0x0011E57B File Offset: 0x0011C77B
		// (set) Token: 0x06003E72 RID: 15986 RVA: 0x0011E592 File Offset: 0x0011C792
		public bool StylisticSet18
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet18Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet18Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x06003E73 RID: 15987 RVA: 0x0011E5A5 File Offset: 0x0011C7A5
		// (set) Token: 0x06003E74 RID: 15988 RVA: 0x0011E5BC File Offset: 0x0011C7BC
		public bool StylisticSet19
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet19Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet19Property, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a stylistic set of a font form is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the stylistic set of the font form is enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x06003E75 RID: 15989 RVA: 0x0011E5CF File Offset: 0x0011C7CF
		// (set) Token: 0x06003E76 RID: 15990 RVA: 0x0011E5E6 File Offset: 0x0011C7E6
		public bool StylisticSet20
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet20Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet20Property, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.FontFraction" /> enumerated value that indicates the fraction style. </summary>
		/// <returns>A <see cref="T:System.Windows.FontFraction" /> enumerated value. The default value is <see cref="F:System.Windows.FontFraction.Normal" />.</returns>
		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x06003E77 RID: 15991 RVA: 0x0011E5F9 File Offset: 0x0011C7F9
		// (set) Token: 0x06003E78 RID: 15992 RVA: 0x0011E610 File Offset: 0x0011C810
		public FontFraction Fraction
		{
			get
			{
				return (FontFraction)this._owner.GetValue(Typography.FractionProperty);
			}
			set
			{
				this._owner.SetValue(Typography.FractionProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a nominal zero font form should be replaced with a slashed zero. </summary>
		/// <returns>
		///     <see langword="true" /> if slashed zero forms are enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000FA6 RID: 4006
		// (get) Token: 0x06003E79 RID: 15993 RVA: 0x0011E628 File Offset: 0x0011C828
		// (set) Token: 0x06003E7A RID: 15994 RVA: 0x0011E63F File Offset: 0x0011C83F
		public bool SlashedZero
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.SlashedZeroProperty);
			}
			set
			{
				this._owner.SetValue(Typography.SlashedZeroProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether standard typographic font forms of Greek glyphs have been replaced with corresponding font forms commonly used in mathematical notation. </summary>
		/// <returns>
		///     <see langword="true" /> if mathematical Greek forms are enabled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x06003E7B RID: 15995 RVA: 0x0011E652 File Offset: 0x0011C852
		// (set) Token: 0x06003E7C RID: 15996 RVA: 0x0011E669 File Offset: 0x0011C869
		public bool MathematicalGreek
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.MathematicalGreekProperty);
			}
			set
			{
				this._owner.SetValue(Typography.MathematicalGreekProperty, value);
			}
		}

		/// <summary>Gets or sets a value that determines whether the standard Japanese font forms have been replaced with the corresponding preferred typographic forms. </summary>
		/// <returns>
		///     <see langword="true" /> if standard Japanese font forms have been replaced with the corresponding preferred typographic forms; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x06003E7D RID: 15997 RVA: 0x0011E67C File Offset: 0x0011C87C
		// (set) Token: 0x06003E7E RID: 15998 RVA: 0x0011E693 File Offset: 0x0011C893
		public bool EastAsianExpertForms
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.EastAsianExpertFormsProperty);
			}
			set
			{
				this._owner.SetValue(Typography.EastAsianExpertFormsProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.FontVariants" /> enumerated value that indicates a variation of the standard typographic form to be used. </summary>
		/// <returns>A <see cref="T:System.Windows.FontVariants" /> enumerated value. The default value is <see cref="F:System.Windows.FontVariants.Normal" />.</returns>
		// Token: 0x17000FA9 RID: 4009
		// (get) Token: 0x06003E7F RID: 15999 RVA: 0x0011E6A6 File Offset: 0x0011C8A6
		// (set) Token: 0x06003E80 RID: 16000 RVA: 0x0011E6BD File Offset: 0x0011C8BD
		public FontVariants Variants
		{
			get
			{
				return (FontVariants)this._owner.GetValue(Typography.VariantsProperty);
			}
			set
			{
				this._owner.SetValue(Typography.VariantsProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.FontCapitals" /> enumerated value that indicates the capital form of the selected font. </summary>
		/// <returns>A <see cref="T:System.Windows.FontCapitals" /> enumerated value. The default value is <see cref="F:System.Windows.FontCapitals.Normal" />.</returns>
		// Token: 0x17000FAA RID: 4010
		// (get) Token: 0x06003E81 RID: 16001 RVA: 0x0011E6D5 File Offset: 0x0011C8D5
		// (set) Token: 0x06003E82 RID: 16002 RVA: 0x0011E6EC File Offset: 0x0011C8EC
		public FontCapitals Capitals
		{
			get
			{
				return (FontCapitals)this._owner.GetValue(Typography.CapitalsProperty);
			}
			set
			{
				this._owner.SetValue(Typography.CapitalsProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.FontNumeralStyle" /> enumerated value that determines the set of glyphs that are used to render numeric alternate font forms. </summary>
		/// <returns>A <see cref="T:System.Windows.FontNumeralStyle" /> enumerated value. The default value is <see cref="F:System.Windows.FontNumeralStyle.Normal" />.</returns>
		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x06003E83 RID: 16003 RVA: 0x0011E704 File Offset: 0x0011C904
		// (set) Token: 0x06003E84 RID: 16004 RVA: 0x0011E71B File Offset: 0x0011C91B
		public FontNumeralStyle NumeralStyle
		{
			get
			{
				return (FontNumeralStyle)this._owner.GetValue(Typography.NumeralStyleProperty);
			}
			set
			{
				this._owner.SetValue(Typography.NumeralStyleProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.FontNumeralAlignment" /> enumerated value that indicates the alighnment of widths when using numerals. </summary>
		/// <returns>A <see cref="T:System.Windows.FontNumeralAlignment" /> enumerated value. The default value is <see cref="F:System.Windows.FontNumeralAlignment.Normal" />.</returns>
		// Token: 0x17000FAC RID: 4012
		// (get) Token: 0x06003E85 RID: 16005 RVA: 0x0011E733 File Offset: 0x0011C933
		// (set) Token: 0x06003E86 RID: 16006 RVA: 0x0011E74A File Offset: 0x0011C94A
		public FontNumeralAlignment NumeralAlignment
		{
			get
			{
				return (FontNumeralAlignment)this._owner.GetValue(Typography.NumeralAlignmentProperty);
			}
			set
			{
				this._owner.SetValue(Typography.NumeralAlignmentProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.FontEastAsianWidths" /> enumerated value that indicates the proportional width to be used for Latin characters in an East Asian font. </summary>
		/// <returns>A <see cref="T:System.Windows.FontEastAsianWidths" /> enumerated value. The default value is <see cref="F:System.Windows.FontEastAsianWidths.Normal" />.</returns>
		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x06003E87 RID: 16007 RVA: 0x0011E762 File Offset: 0x0011C962
		// (set) Token: 0x06003E88 RID: 16008 RVA: 0x0011E779 File Offset: 0x0011C979
		public FontEastAsianWidths EastAsianWidths
		{
			get
			{
				return (FontEastAsianWidths)this._owner.GetValue(Typography.EastAsianWidthsProperty);
			}
			set
			{
				this._owner.SetValue(Typography.EastAsianWidthsProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.FontEastAsianLanguage" /> enumerated value that indicates the version of glyphs to be used for a specific writing system or language. </summary>
		/// <returns>A <see cref="T:System.Windows.FontEastAsianLanguage" /> enumerated value. The default value is <see cref="F:System.Windows.FontEastAsianLanguage.Normal" />.</returns>
		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x06003E89 RID: 16009 RVA: 0x0011E791 File Offset: 0x0011C991
		// (set) Token: 0x06003E8A RID: 16010 RVA: 0x0011E7A8 File Offset: 0x0011C9A8
		public FontEastAsianLanguage EastAsianLanguage
		{
			get
			{
				return (FontEastAsianLanguage)this._owner.GetValue(Typography.EastAsianLanguageProperty);
			}
			set
			{
				this._owner.SetValue(Typography.EastAsianLanguageProperty, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the index of a standard swashes form. </summary>
		/// <returns>The index of the standard swashes form. The default value is 0 (zero).</returns>
		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x06003E8B RID: 16011 RVA: 0x0011E7C0 File Offset: 0x0011C9C0
		// (set) Token: 0x06003E8C RID: 16012 RVA: 0x0011E7D7 File Offset: 0x0011C9D7
		public int StandardSwashes
		{
			get
			{
				return (int)this._owner.GetValue(Typography.StandardSwashesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.StandardSwashesProperty, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the index of a contextual swashes form. </summary>
		/// <returns>The index of the standard swashes form. The default value is 0 (zero).</returns>
		// Token: 0x17000FB0 RID: 4016
		// (get) Token: 0x06003E8D RID: 16013 RVA: 0x0011E7EF File Offset: 0x0011C9EF
		// (set) Token: 0x06003E8E RID: 16014 RVA: 0x0011E806 File Offset: 0x0011CA06
		public int ContextualSwashes
		{
			get
			{
				return (int)this._owner.GetValue(Typography.ContextualSwashesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.ContextualSwashesProperty, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the index of a stylistic alternates form. </summary>
		/// <returns>The index of the stylistic alternates form. The default value is 0 (zero).</returns>
		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x06003E8F RID: 16015 RVA: 0x0011E81E File Offset: 0x0011CA1E
		// (set) Token: 0x06003E90 RID: 16016 RVA: 0x0011E835 File Offset: 0x0011CA35
		public int StylisticAlternates
		{
			get
			{
				return (int)this._owner.GetValue(Typography.StylisticAlternatesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticAlternatesProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StandardLigatures" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StandardLigatures" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003E91 RID: 16017 RVA: 0x0011E84D File Offset: 0x0011CA4D
		public static void SetStandardLigatures(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StandardLigaturesProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StandardLigatures" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StandardLigatures" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StandardLigatures" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003E92 RID: 16018 RVA: 0x0011E869 File Offset: 0x0011CA69
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStandardLigatures(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StandardLigaturesProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.ContextualLigatures" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.ContextualLigatures" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003E93 RID: 16019 RVA: 0x0011E889 File Offset: 0x0011CA89
		public static void SetContextualLigatures(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.ContextualLigaturesProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.ContextualLigatures" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.ContextualLigatures" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.ContextualLigatures" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003E94 RID: 16020 RVA: 0x0011E8A5 File Offset: 0x0011CAA5
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetContextualLigatures(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.ContextualLigaturesProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.DiscretionaryLigatures" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.DiscretionaryLigatures" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003E95 RID: 16021 RVA: 0x0011E8C5 File Offset: 0x0011CAC5
		public static void SetDiscretionaryLigatures(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.DiscretionaryLigaturesProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.DiscretionaryLigatures" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.DiscretionaryLigatures" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.DiscretionaryLigatures" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003E96 RID: 16022 RVA: 0x0011E8E1 File Offset: 0x0011CAE1
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetDiscretionaryLigatures(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.DiscretionaryLigaturesProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.HistoricalLigatures" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.HistoricalLigatures" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003E97 RID: 16023 RVA: 0x0011E901 File Offset: 0x0011CB01
		public static void SetHistoricalLigatures(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.HistoricalLigaturesProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.HistoricalLigatures" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.HistoricalLigatures" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.HistoricalLigatures" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003E98 RID: 16024 RVA: 0x0011E91D File Offset: 0x0011CB1D
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetHistoricalLigatures(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.HistoricalLigaturesProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.AnnotationAlternates" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.AnnotationAlternates" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003E99 RID: 16025 RVA: 0x0011E93D File Offset: 0x0011CB3D
		public static void SetAnnotationAlternates(DependencyObject element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.AnnotationAlternatesProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.AnnotationAlternates" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.AnnotationAlternates" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.TextElement.FontFamily" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003E9A RID: 16026 RVA: 0x0011E95E File Offset: 0x0011CB5E
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static int GetAnnotationAlternates(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Typography.AnnotationAlternatesProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.ContextualAlternates" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.ContextualAlternates" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003E9B RID: 16027 RVA: 0x0011E97E File Offset: 0x0011CB7E
		public static void SetContextualAlternates(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.ContextualAlternatesProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.ContextualAlternates" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.ContextualAlternates" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.ContextualAlternates" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003E9C RID: 16028 RVA: 0x0011E99A File Offset: 0x0011CB9A
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetContextualAlternates(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.ContextualAlternatesProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.HistoricalForms" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.HistoricalForms" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003E9D RID: 16029 RVA: 0x0011E9BA File Offset: 0x0011CBBA
		public static void SetHistoricalForms(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.HistoricalFormsProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.HistoricalForms" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.HistoricalForms" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.HistoricalForms" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003E9E RID: 16030 RVA: 0x0011E9D6 File Offset: 0x0011CBD6
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetHistoricalForms(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.HistoricalFormsProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.Kerning" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.Kerning" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003E9F RID: 16031 RVA: 0x0011E9F6 File Offset: 0x0011CBF6
		public static void SetKerning(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.KerningProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.Kerning" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.Kerning" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.Kerning" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EA0 RID: 16032 RVA: 0x0011EA12 File Offset: 0x0011CC12
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetKerning(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.KerningProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.CapitalSpacing" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.CapitalSpacing" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EA1 RID: 16033 RVA: 0x0011EA32 File Offset: 0x0011CC32
		public static void SetCapitalSpacing(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.CapitalSpacingProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.CapitalSpacing" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.CapitalSpacing" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.CapitalSpacing" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EA2 RID: 16034 RVA: 0x0011EA4E File Offset: 0x0011CC4E
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetCapitalSpacing(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.CapitalSpacingProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.CaseSensitiveForms" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.CaseSensitiveForms" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EA3 RID: 16035 RVA: 0x0011EA6E File Offset: 0x0011CC6E
		public static void SetCaseSensitiveForms(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.CaseSensitiveFormsProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.CaseSensitiveForms" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.CaseSensitiveForms" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.CaseSensitiveForms" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EA4 RID: 16036 RVA: 0x0011EA8A File Offset: 0x0011CC8A
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetCaseSensitiveForms(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.CaseSensitiveFormsProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet1" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet1" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EA5 RID: 16037 RVA: 0x0011EAAA File Offset: 0x0011CCAA
		public static void SetStylisticSet1(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet1Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet1" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet1" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet1" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EA6 RID: 16038 RVA: 0x0011EAC6 File Offset: 0x0011CCC6
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet1(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet1Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet2" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet2" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EA7 RID: 16039 RVA: 0x0011EAE6 File Offset: 0x0011CCE6
		public static void SetStylisticSet2(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet2Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet2" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet2" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet2" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EA8 RID: 16040 RVA: 0x0011EB02 File Offset: 0x0011CD02
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet2(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet2Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet3" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet3" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EA9 RID: 16041 RVA: 0x0011EB22 File Offset: 0x0011CD22
		public static void SetStylisticSet3(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet3Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet3" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet3" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet3" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EAA RID: 16042 RVA: 0x0011EB3E File Offset: 0x0011CD3E
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet3(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet3Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet4" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet4" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EAB RID: 16043 RVA: 0x0011EB5E File Offset: 0x0011CD5E
		public static void SetStylisticSet4(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet4Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet4" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet4" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet4" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EAC RID: 16044 RVA: 0x0011EB7A File Offset: 0x0011CD7A
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet4(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet4Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet5" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet5" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EAD RID: 16045 RVA: 0x0011EB9A File Offset: 0x0011CD9A
		public static void SetStylisticSet5(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet5Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet5" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet5" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet5" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EAE RID: 16046 RVA: 0x0011EBB6 File Offset: 0x0011CDB6
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet5(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet5Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet6" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet6" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EAF RID: 16047 RVA: 0x0011EBD6 File Offset: 0x0011CDD6
		public static void SetStylisticSet6(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet6Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet6" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet6" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet6" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EB0 RID: 16048 RVA: 0x0011EBF2 File Offset: 0x0011CDF2
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet6(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet6Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet7" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet7" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EB1 RID: 16049 RVA: 0x0011EC12 File Offset: 0x0011CE12
		public static void SetStylisticSet7(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet7Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet7" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet7" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet7" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EB2 RID: 16050 RVA: 0x0011EC2E File Offset: 0x0011CE2E
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet7(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet7Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet8" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet8" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EB3 RID: 16051 RVA: 0x0011EC4E File Offset: 0x0011CE4E
		public static void SetStylisticSet8(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet8Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet8" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet8" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet8" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EB4 RID: 16052 RVA: 0x0011EC6A File Offset: 0x0011CE6A
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet8(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet8Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet9" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet9" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EB5 RID: 16053 RVA: 0x0011EC8A File Offset: 0x0011CE8A
		public static void SetStylisticSet9(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet9Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet8" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet8" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet8" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EB6 RID: 16054 RVA: 0x0011ECA6 File Offset: 0x0011CEA6
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet9(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet9Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet10" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet10" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EB7 RID: 16055 RVA: 0x0011ECC6 File Offset: 0x0011CEC6
		public static void SetStylisticSet10(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet10Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet10" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet10" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet10" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EB8 RID: 16056 RVA: 0x0011ECE2 File Offset: 0x0011CEE2
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet10(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet10Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet11" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet11" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EB9 RID: 16057 RVA: 0x0011ED02 File Offset: 0x0011CF02
		public static void SetStylisticSet11(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet11Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet11" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet11" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet11" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EBA RID: 16058 RVA: 0x0011ED1E File Offset: 0x0011CF1E
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet11(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet11Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet12" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet12" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EBB RID: 16059 RVA: 0x0011ED3E File Offset: 0x0011CF3E
		public static void SetStylisticSet12(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet12Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet12" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet12" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet12" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EBC RID: 16060 RVA: 0x0011ED5A File Offset: 0x0011CF5A
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet12(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet12Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet13" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet13" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EBD RID: 16061 RVA: 0x0011ED7A File Offset: 0x0011CF7A
		public static void SetStylisticSet13(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet13Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet13" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet13" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet13" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EBE RID: 16062 RVA: 0x0011ED96 File Offset: 0x0011CF96
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet13(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet13Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet14" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet14" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EBF RID: 16063 RVA: 0x0011EDB6 File Offset: 0x0011CFB6
		public static void SetStylisticSet14(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet14Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet14" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet14" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet14" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EC0 RID: 16064 RVA: 0x0011EDD2 File Offset: 0x0011CFD2
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet14(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet14Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet15" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet15" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EC1 RID: 16065 RVA: 0x0011EDF2 File Offset: 0x0011CFF2
		public static void SetStylisticSet15(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet15Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet15" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet15" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet15" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EC2 RID: 16066 RVA: 0x0011EE0E File Offset: 0x0011D00E
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet15(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet15Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet16" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet16" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EC3 RID: 16067 RVA: 0x0011EE2E File Offset: 0x0011D02E
		public static void SetStylisticSet16(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet16Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet16" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet16" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet16" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EC4 RID: 16068 RVA: 0x0011EE4A File Offset: 0x0011D04A
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet16(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet16Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet17" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet17" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EC5 RID: 16069 RVA: 0x0011EE6A File Offset: 0x0011D06A
		public static void SetStylisticSet17(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet17Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet17" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet17" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet17" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EC6 RID: 16070 RVA: 0x0011EE86 File Offset: 0x0011D086
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet17(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet17Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet18" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet18" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EC7 RID: 16071 RVA: 0x0011EEA6 File Offset: 0x0011D0A6
		public static void SetStylisticSet18(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet18Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet18" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet18" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet18" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EC8 RID: 16072 RVA: 0x0011EEC2 File Offset: 0x0011D0C2
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet18(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet18Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet19" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet19" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EC9 RID: 16073 RVA: 0x0011EEE2 File Offset: 0x0011D0E2
		public static void SetStylisticSet19(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet19Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet19" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet19" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet19" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003ECA RID: 16074 RVA: 0x0011EEFE File Offset: 0x0011D0FE
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet19(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet19Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet20" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet20" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003ECB RID: 16075 RVA: 0x0011EF1E File Offset: 0x0011D11E
		public static void SetStylisticSet20(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet20Property, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet20" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet20" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticSet20" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003ECC RID: 16076 RVA: 0x0011EF3A File Offset: 0x0011D13A
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet20(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet20Property);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.Fraction" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.Fraction" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003ECD RID: 16077 RVA: 0x0011EF5A File Offset: 0x0011D15A
		public static void SetFraction(DependencyObject element, FontFraction value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.FractionProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.Fraction" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.Fraction" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.Fraction" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003ECE RID: 16078 RVA: 0x0011EF7B File Offset: 0x0011D17B
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static FontFraction GetFraction(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontFraction)element.GetValue(Typography.FractionProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.SlashedZero" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.SlashedZero" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003ECF RID: 16079 RVA: 0x0011EF9B File Offset: 0x0011D19B
		public static void SetSlashedZero(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.SlashedZeroProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.SlashedZero" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.SlashedZero" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.SlashedZero" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003ED0 RID: 16080 RVA: 0x0011EFB7 File Offset: 0x0011D1B7
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetSlashedZero(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.SlashedZeroProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.MathematicalGreek" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.MathematicalGreek" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003ED1 RID: 16081 RVA: 0x0011EFD7 File Offset: 0x0011D1D7
		public static void SetMathematicalGreek(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.MathematicalGreekProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.MathematicalGreek" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.MathematicalGreek" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.MathematicalGreek" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003ED2 RID: 16082 RVA: 0x0011EFF3 File Offset: 0x0011D1F3
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetMathematicalGreek(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.MathematicalGreekProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.EastAsianExpertForms" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.EastAsianExpertForms" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003ED3 RID: 16083 RVA: 0x0011F013 File Offset: 0x0011D213
		public static void SetEastAsianExpertForms(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.EastAsianExpertFormsProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.EastAsianExpertForms" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.EastAsianExpertForms" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.EastAsianExpertForms" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003ED4 RID: 16084 RVA: 0x0011F02F File Offset: 0x0011D22F
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetEastAsianExpertForms(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.EastAsianExpertFormsProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.Variants" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.Variants" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003ED5 RID: 16085 RVA: 0x0011F04F File Offset: 0x0011D24F
		public static void SetVariants(DependencyObject element, FontVariants value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.VariantsProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.Variants" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.Variants" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.Variants" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003ED6 RID: 16086 RVA: 0x0011F070 File Offset: 0x0011D270
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static FontVariants GetVariants(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontVariants)element.GetValue(Typography.VariantsProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.Capitals" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.Capitals" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003ED7 RID: 16087 RVA: 0x0011F090 File Offset: 0x0011D290
		public static void SetCapitals(DependencyObject element, FontCapitals value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.CapitalsProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.Capitals" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.Capitals" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.Capitals" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003ED8 RID: 16088 RVA: 0x0011F0B1 File Offset: 0x0011D2B1
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static FontCapitals GetCapitals(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontCapitals)element.GetValue(Typography.CapitalsProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.NumeralStyle" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.NumeralStyle" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003ED9 RID: 16089 RVA: 0x0011F0D1 File Offset: 0x0011D2D1
		public static void SetNumeralStyle(DependencyObject element, FontNumeralStyle value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.NumeralStyleProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.NumeralStyle" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.NumeralStyle" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.NumeralStyle" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EDA RID: 16090 RVA: 0x0011F0F2 File Offset: 0x0011D2F2
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static FontNumeralStyle GetNumeralStyle(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontNumeralStyle)element.GetValue(Typography.NumeralStyleProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.NumeralAlignment" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.NumeralAlignment" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EDB RID: 16091 RVA: 0x0011F112 File Offset: 0x0011D312
		public static void SetNumeralAlignment(DependencyObject element, FontNumeralAlignment value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.NumeralAlignmentProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.NumeralAlignment" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.NumeralAlignment" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.NumeralAlignment" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EDC RID: 16092 RVA: 0x0011F133 File Offset: 0x0011D333
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static FontNumeralAlignment GetNumeralAlignment(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontNumeralAlignment)element.GetValue(Typography.NumeralAlignmentProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.EastAsianWidths" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.EastAsianWidths" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EDD RID: 16093 RVA: 0x0011F153 File Offset: 0x0011D353
		public static void SetEastAsianWidths(DependencyObject element, FontEastAsianWidths value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.EastAsianWidthsProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.EastAsianWidths" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.EastAsianWidths" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.EastAsianWidths" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EDE RID: 16094 RVA: 0x0011F174 File Offset: 0x0011D374
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static FontEastAsianWidths GetEastAsianWidths(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontEastAsianWidths)element.GetValue(Typography.EastAsianWidthsProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.EastAsianLanguage" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.EastAsianLanguage" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EDF RID: 16095 RVA: 0x0011F194 File Offset: 0x0011D394
		public static void SetEastAsianLanguage(DependencyObject element, FontEastAsianLanguage value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.EastAsianLanguageProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.EastAsianLanguage" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.EastAsianLanguage" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.EastAsianLanguage" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EE0 RID: 16096 RVA: 0x0011F1B5 File Offset: 0x0011D3B5
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static FontEastAsianLanguage GetEastAsianLanguage(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontEastAsianLanguage)element.GetValue(Typography.EastAsianLanguageProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StandardSwashes" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StandardSwashes" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EE1 RID: 16097 RVA: 0x0011F1D5 File Offset: 0x0011D3D5
		public static void SetStandardSwashes(DependencyObject element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StandardSwashesProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StandardSwashes" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StandardSwashes" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StandardSwashes" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EE2 RID: 16098 RVA: 0x0011F1F6 File Offset: 0x0011D3F6
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static int GetStandardSwashes(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Typography.StandardSwashesProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.ContextualSwashes" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.ContextualSwashes" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EE3 RID: 16099 RVA: 0x0011F216 File Offset: 0x0011D416
		public static void SetContextualSwashes(DependencyObject element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.ContextualSwashesProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.ContextualSwashes" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.ContextualSwashes" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.ContextualSwashes" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EE4 RID: 16100 RVA: 0x0011F237 File Offset: 0x0011D437
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static int GetContextualSwashes(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Typography.ContextualSwashesProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Typography.StylisticAlternates" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.Typography.StylisticAlternates" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06003EE5 RID: 16101 RVA: 0x0011F257 File Offset: 0x0011D457
		public static void SetStylisticAlternates(DependencyObject element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticAlternatesProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Typography.StylisticAlternates" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.Typography.StylisticAlternates" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Typography.StylisticAlternates" /> attached property on the specified dependency object.</returns>
		// Token: 0x06003EE6 RID: 16102 RVA: 0x0011F278 File Offset: 0x0011D478
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static int GetStylisticAlternates(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Typography.StylisticAlternatesProperty);
		}

		// Token: 0x04002697 RID: 9879
		private static readonly Type _typeofThis = typeof(Typography);

		// Token: 0x04002698 RID: 9880
		private static readonly Type _typeofBool = typeof(bool);

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StandardLigatures" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StandardLigatures" /> attached property.</returns>
		// Token: 0x04002699 RID: 9881
		public static readonly DependencyProperty StandardLigaturesProperty = DependencyProperty.RegisterAttached("StandardLigatures", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.ContextualLigatures" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.ContextualLigatures" /> attached property.</returns>
		// Token: 0x0400269A RID: 9882
		public static readonly DependencyProperty ContextualLigaturesProperty = DependencyProperty.RegisterAttached("ContextualLigatures", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.DiscretionaryLigatures" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.DiscretionaryLigatures" /> attached property.</returns>
		// Token: 0x0400269B RID: 9883
		public static readonly DependencyProperty DiscretionaryLigaturesProperty = DependencyProperty.RegisterAttached("DiscretionaryLigatures", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.HistoricalLigatures" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.HistoricalLigatures" /> attached property.</returns>
		// Token: 0x0400269C RID: 9884
		public static readonly DependencyProperty HistoricalLigaturesProperty = DependencyProperty.RegisterAttached("HistoricalLigatures", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.AnnotationAlternates" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Typography.AnnotationAlternates" /> attached property.</returns>
		// Token: 0x0400269D RID: 9885
		public static readonly DependencyProperty AnnotationAlternatesProperty = DependencyProperty.RegisterAttached("AnnotationAlternates", typeof(int), Typography._typeofThis, new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.ContextualAlternates" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.ContextualAlternates" /> attached property.</returns>
		// Token: 0x0400269E RID: 9886
		public static readonly DependencyProperty ContextualAlternatesProperty = DependencyProperty.RegisterAttached("ContextualAlternates", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.HistoricalForms" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.HistoricalForms" /> attached property.</returns>
		// Token: 0x0400269F RID: 9887
		public static readonly DependencyProperty HistoricalFormsProperty = DependencyProperty.RegisterAttached("HistoricalForms", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.Kerning" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.Kerning" /> attached property.</returns>
		// Token: 0x040026A0 RID: 9888
		public static readonly DependencyProperty KerningProperty = DependencyProperty.RegisterAttached("Kerning", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.CapitalSpacing" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.CapitalSpacing" /> attached property.</returns>
		// Token: 0x040026A1 RID: 9889
		public static readonly DependencyProperty CapitalSpacingProperty = DependencyProperty.RegisterAttached("CapitalSpacing", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.CaseSensitiveForms" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.CaseSensitiveForms" /> attached property.</returns>
		// Token: 0x040026A2 RID: 9890
		public static readonly DependencyProperty CaseSensitiveFormsProperty = DependencyProperty.RegisterAttached("CaseSensitiveForms", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet1" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet1" /> attached property.</returns>
		// Token: 0x040026A3 RID: 9891
		public static readonly DependencyProperty StylisticSet1Property = DependencyProperty.RegisterAttached("StylisticSet1", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet2" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet2" /> attached property.</returns>
		// Token: 0x040026A4 RID: 9892
		public static readonly DependencyProperty StylisticSet2Property = DependencyProperty.RegisterAttached("StylisticSet2", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet3" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet3" /> attached property.</returns>
		// Token: 0x040026A5 RID: 9893
		public static readonly DependencyProperty StylisticSet3Property = DependencyProperty.RegisterAttached("StylisticSet3", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet4" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet4" /> attached property.</returns>
		// Token: 0x040026A6 RID: 9894
		public static readonly DependencyProperty StylisticSet4Property = DependencyProperty.RegisterAttached("StylisticSet4", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet5" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet4" /> attached property.</returns>
		// Token: 0x040026A7 RID: 9895
		public static readonly DependencyProperty StylisticSet5Property = DependencyProperty.RegisterAttached("StylisticSet5", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet6" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet6" /> attached property.</returns>
		// Token: 0x040026A8 RID: 9896
		public static readonly DependencyProperty StylisticSet6Property = DependencyProperty.RegisterAttached("StylisticSet6", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet7" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet7" /> attached property.</returns>
		// Token: 0x040026A9 RID: 9897
		public static readonly DependencyProperty StylisticSet7Property = DependencyProperty.RegisterAttached("StylisticSet7", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet8" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet8" /> attached property.</returns>
		// Token: 0x040026AA RID: 9898
		public static readonly DependencyProperty StylisticSet8Property = DependencyProperty.RegisterAttached("StylisticSet8", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet9" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet9" /> attached property.</returns>
		// Token: 0x040026AB RID: 9899
		public static readonly DependencyProperty StylisticSet9Property = DependencyProperty.RegisterAttached("StylisticSet9", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet10" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet10" /> attached property.</returns>
		// Token: 0x040026AC RID: 9900
		public static readonly DependencyProperty StylisticSet10Property = DependencyProperty.RegisterAttached("StylisticSet10", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet11" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet11" /> attached property.</returns>
		// Token: 0x040026AD RID: 9901
		public static readonly DependencyProperty StylisticSet11Property = DependencyProperty.RegisterAttached("StylisticSet11", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet12" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet12" /> attached property.</returns>
		// Token: 0x040026AE RID: 9902
		public static readonly DependencyProperty StylisticSet12Property = DependencyProperty.RegisterAttached("StylisticSet12", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet13" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet13" /> attached property.</returns>
		// Token: 0x040026AF RID: 9903
		public static readonly DependencyProperty StylisticSet13Property = DependencyProperty.RegisterAttached("StylisticSet13", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet14" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet14" /> attached property.</returns>
		// Token: 0x040026B0 RID: 9904
		public static readonly DependencyProperty StylisticSet14Property = DependencyProperty.RegisterAttached("StylisticSet14", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet15" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet15" /> attached property.</returns>
		// Token: 0x040026B1 RID: 9905
		public static readonly DependencyProperty StylisticSet15Property = DependencyProperty.RegisterAttached("StylisticSet15", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet16" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet16" /> attached property.</returns>
		// Token: 0x040026B2 RID: 9906
		public static readonly DependencyProperty StylisticSet16Property = DependencyProperty.RegisterAttached("StylisticSet16", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet17" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet17" /> attached property.</returns>
		// Token: 0x040026B3 RID: 9907
		public static readonly DependencyProperty StylisticSet17Property = DependencyProperty.RegisterAttached("StylisticSet17", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet18" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet18" /> attached property.</returns>
		// Token: 0x040026B4 RID: 9908
		public static readonly DependencyProperty StylisticSet18Property = DependencyProperty.RegisterAttached("StylisticSet18", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet19" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet19" /> attached property.</returns>
		// Token: 0x040026B5 RID: 9909
		public static readonly DependencyProperty StylisticSet19Property = DependencyProperty.RegisterAttached("StylisticSet19", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticSet20" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticSet20" /> attached property.</returns>
		// Token: 0x040026B6 RID: 9910
		public static readonly DependencyProperty StylisticSet20Property = DependencyProperty.RegisterAttached("StylisticSet20", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.Fraction" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.Fraction" /> attached property.</returns>
		// Token: 0x040026B7 RID: 9911
		public static readonly DependencyProperty FractionProperty = DependencyProperty.RegisterAttached("Fraction", typeof(FontFraction), Typography._typeofThis, new FrameworkPropertyMetadata(FontFraction.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.SlashedZero" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.SlashedZero" /> attached property.</returns>
		// Token: 0x040026B8 RID: 9912
		public static readonly DependencyProperty SlashedZeroProperty = DependencyProperty.RegisterAttached("SlashedZero", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.MathematicalGreek" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.MathematicalGreek" /> attached property.</returns>
		// Token: 0x040026B9 RID: 9913
		public static readonly DependencyProperty MathematicalGreekProperty = DependencyProperty.RegisterAttached("MathematicalGreek", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.EastAsianExpertForms" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.EastAsianExpertForms" /> attached property.</returns>
		// Token: 0x040026BA RID: 9914
		public static readonly DependencyProperty EastAsianExpertFormsProperty = DependencyProperty.RegisterAttached("EastAsianExpertForms", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.Variants" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.Variants" /> attached property.</returns>
		// Token: 0x040026BB RID: 9915
		public static readonly DependencyProperty VariantsProperty = DependencyProperty.RegisterAttached("Variants", typeof(FontVariants), Typography._typeofThis, new FrameworkPropertyMetadata(FontVariants.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.Capitals" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.Capitals" /> attached property.</returns>
		// Token: 0x040026BC RID: 9916
		public static readonly DependencyProperty CapitalsProperty = DependencyProperty.RegisterAttached("Capitals", typeof(FontCapitals), Typography._typeofThis, new FrameworkPropertyMetadata(FontCapitals.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.NumeralStyle" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.NumeralStyle" /> attached property.</returns>
		// Token: 0x040026BD RID: 9917
		public static readonly DependencyProperty NumeralStyleProperty = DependencyProperty.RegisterAttached("NumeralStyle", typeof(FontNumeralStyle), Typography._typeofThis, new FrameworkPropertyMetadata(FontNumeralStyle.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.NumeralAlignment" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.NumeralAlignment" /> attached property.</returns>
		// Token: 0x040026BE RID: 9918
		public static readonly DependencyProperty NumeralAlignmentProperty = DependencyProperty.RegisterAttached("NumeralAlignment", typeof(FontNumeralAlignment), Typography._typeofThis, new FrameworkPropertyMetadata(FontNumeralAlignment.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.EastAsianWidths" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.EastAsianWidths" /> attached property.</returns>
		// Token: 0x040026BF RID: 9919
		public static readonly DependencyProperty EastAsianWidthsProperty = DependencyProperty.RegisterAttached("EastAsianWidths", typeof(FontEastAsianWidths), Typography._typeofThis, new FrameworkPropertyMetadata(FontEastAsianWidths.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.EastAsianLanguage" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.EastAsianLanguage" /> attached property.</returns>
		// Token: 0x040026C0 RID: 9920
		public static readonly DependencyProperty EastAsianLanguageProperty = DependencyProperty.RegisterAttached("EastAsianLanguage", typeof(FontEastAsianLanguage), Typography._typeofThis, new FrameworkPropertyMetadata(FontEastAsianLanguage.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StandardSwashes" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StandardSwashes" /> attached property.</returns>
		// Token: 0x040026C1 RID: 9921
		public static readonly DependencyProperty StandardSwashesProperty = DependencyProperty.RegisterAttached("StandardSwashes", typeof(int), Typography._typeofThis, new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.ContextualSwashes" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.ContextualSwashes" /> attached property.</returns>
		// Token: 0x040026C2 RID: 9922
		public static readonly DependencyProperty ContextualSwashesProperty = DependencyProperty.RegisterAttached("ContextualSwashes", typeof(int), Typography._typeofThis, new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Typography.StylisticAlternates" /> attached property.</summary>
		/// <returns>The identifier for <see cref="P:System.Windows.Documents.Typography.StylisticAlternates" /> attached property.</returns>
		// Token: 0x040026C3 RID: 9923
		public static readonly DependencyProperty StylisticAlternatesProperty = DependencyProperty.RegisterAttached("StylisticAlternates", typeof(int), Typography._typeofThis, new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040026C4 RID: 9924
		internal static DependencyProperty[] TypographyPropertiesList = new DependencyProperty[]
		{
			Typography.StandardLigaturesProperty,
			Typography.ContextualLigaturesProperty,
			Typography.DiscretionaryLigaturesProperty,
			Typography.HistoricalLigaturesProperty,
			Typography.AnnotationAlternatesProperty,
			Typography.ContextualAlternatesProperty,
			Typography.HistoricalFormsProperty,
			Typography.KerningProperty,
			Typography.CapitalSpacingProperty,
			Typography.CaseSensitiveFormsProperty,
			Typography.StylisticSet1Property,
			Typography.StylisticSet2Property,
			Typography.StylisticSet3Property,
			Typography.StylisticSet4Property,
			Typography.StylisticSet5Property,
			Typography.StylisticSet6Property,
			Typography.StylisticSet7Property,
			Typography.StylisticSet8Property,
			Typography.StylisticSet9Property,
			Typography.StylisticSet10Property,
			Typography.StylisticSet11Property,
			Typography.StylisticSet12Property,
			Typography.StylisticSet13Property,
			Typography.StylisticSet14Property,
			Typography.StylisticSet15Property,
			Typography.StylisticSet16Property,
			Typography.StylisticSet17Property,
			Typography.StylisticSet18Property,
			Typography.StylisticSet19Property,
			Typography.StylisticSet20Property,
			Typography.FractionProperty,
			Typography.SlashedZeroProperty,
			Typography.MathematicalGreekProperty,
			Typography.EastAsianExpertFormsProperty,
			Typography.VariantsProperty,
			Typography.CapitalsProperty,
			Typography.NumeralStyleProperty,
			Typography.NumeralAlignmentProperty,
			Typography.EastAsianWidthsProperty,
			Typography.EastAsianLanguageProperty,
			Typography.StandardSwashesProperty,
			Typography.ContextualSwashesProperty,
			Typography.StylisticAlternatesProperty
		};

		// Token: 0x040026C5 RID: 9925
		internal static readonly TypographyProperties Default = new TypographyProperties();

		// Token: 0x040026C6 RID: 9926
		private DependencyObject _owner;
	}
}
