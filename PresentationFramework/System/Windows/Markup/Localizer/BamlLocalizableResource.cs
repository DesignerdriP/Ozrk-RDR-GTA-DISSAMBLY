using System;

namespace System.Windows.Markup.Localizer
{
	/// <summary>Represents a localizable resource in a BAML stream. </summary>
	// Token: 0x0200028E RID: 654
	public class BamlLocalizableResource
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizableResource" /> class. </summary>
		// Token: 0x060024CB RID: 9419 RVA: 0x000B2750 File Offset: 0x000B0950
		public BamlLocalizableResource() : this(null, null, LocalizationCategory.None, true, true)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizableResource" /> class, with the specified localizable value, localization comments, resource category, localization lock status, and visibility of the resource.</summary>
		/// <param name="content">The localizable value.</param>
		/// <param name="comments">Comments used for localizing.</param>
		/// <param name="category">The string category of the resource.</param>
		/// <param name="modifiable">
		///       <see langword="true" /> if the resource should be modifiable; otherwise, <see langword="false" />.</param>
		/// <param name="readable">
		///       <see langword="true" /> if the resource should be visible for translation purposes because it is potentially readable as text in the UI; otherwise, <see langword="false" />.</param>
		// Token: 0x060024CC RID: 9420 RVA: 0x000B275D File Offset: 0x000B095D
		public BamlLocalizableResource(string content, string comments, LocalizationCategory category, bool modifiable, bool readable)
		{
			this._content = content;
			this._comments = comments;
			this._category = category;
			this.Modifiable = modifiable;
			this.Readable = readable;
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x000B278A File Offset: 0x000B098A
		internal BamlLocalizableResource(BamlLocalizableResource other)
		{
			this._content = other._content;
			this._comments = other._comments;
			this._flags = other._flags;
			this._category = other._category;
		}

		/// <summary>Gets or sets the localizable content.</summary>
		/// <returns>The localizable content string.</returns>
		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x060024CE RID: 9422 RVA: 0x000B27C2 File Offset: 0x000B09C2
		// (set) Token: 0x060024CF RID: 9423 RVA: 0x000B27CA File Offset: 0x000B09CA
		public string Content
		{
			get
			{
				return this._content;
			}
			set
			{
				this._content = value;
			}
		}

		/// <summary>Gets or sets the localization comments associated with a resource. </summary>
		/// <returns>The localization comment string.</returns>
		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x060024D0 RID: 9424 RVA: 0x000B27D3 File Offset: 0x000B09D3
		// (set) Token: 0x060024D1 RID: 9425 RVA: 0x000B27DB File Offset: 0x000B09DB
		public string Comments
		{
			get
			{
				return this._comments;
			}
			set
			{
				this._comments = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the localizable resource is modifiable. </summary>
		/// <returns>
		///     <see langword="true" /> if the resource is modifiable; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x060024D2 RID: 9426 RVA: 0x000B27E4 File Offset: 0x000B09E4
		// (set) Token: 0x060024D3 RID: 9427 RVA: 0x000B27F1 File Offset: 0x000B09F1
		public bool Modifiable
		{
			get
			{
				return (this._flags & BamlLocalizableResource.LocalizationFlags.Modifiable) > (BamlLocalizableResource.LocalizationFlags)0;
			}
			set
			{
				if (value)
				{
					this._flags |= BamlLocalizableResource.LocalizationFlags.Modifiable;
					return;
				}
				this._flags &= ~BamlLocalizableResource.LocalizationFlags.Modifiable;
			}
		}

		/// <summary>Gets or sets whether the resource is visible for translation. </summary>
		/// <returns>
		///     <see langword="true" /> if the resource is visible for translation; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x060024D4 RID: 9428 RVA: 0x000B2817 File Offset: 0x000B0A17
		// (set) Token: 0x060024D5 RID: 9429 RVA: 0x000B2824 File Offset: 0x000B0A24
		public bool Readable
		{
			get
			{
				return (this._flags & BamlLocalizableResource.LocalizationFlags.Readable) > (BamlLocalizableResource.LocalizationFlags)0;
			}
			set
			{
				if (value)
				{
					this._flags |= BamlLocalizableResource.LocalizationFlags.Readable;
					return;
				}
				this._flags &= ~BamlLocalizableResource.LocalizationFlags.Readable;
			}
		}

		/// <summary>Gets or sets the localization category of a resource. </summary>
		/// <returns>The localization category, as a value of the enumeration.</returns>
		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x060024D6 RID: 9430 RVA: 0x000B284A File Offset: 0x000B0A4A
		// (set) Token: 0x060024D7 RID: 9431 RVA: 0x000B2852 File Offset: 0x000B0A52
		public LocalizationCategory Category
		{
			get
			{
				return this._category;
			}
			set
			{
				this._category = value;
			}
		}

		/// <summary>Determines whether a specified <see cref="T:System.Windows.Markup.Localizer.BamlLocalizableResource" /> object is equal to this object.</summary>
		/// <param name="other">The <see cref="T:System.Windows.Markup.Localizer.BamlLocalizableResource" /> object test for equality.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="other" /> is equal to this object; otherwise, <see langword="false" />.</returns>
		// Token: 0x060024D8 RID: 9432 RVA: 0x000B285C File Offset: 0x000B0A5C
		public override bool Equals(object other)
		{
			BamlLocalizableResource bamlLocalizableResource = other as BamlLocalizableResource;
			return bamlLocalizableResource != null && (this._content == bamlLocalizableResource._content && this._comments == bamlLocalizableResource._comments && this._flags == bamlLocalizableResource._flags) && this._category == bamlLocalizableResource._category;
		}

		/// <summary>Returns an integer hash code representing this instance. </summary>
		/// <returns>An integer hash code.</returns>
		// Token: 0x060024D9 RID: 9433 RVA: 0x000B28B9 File Offset: 0x000B0AB9
		public override int GetHashCode()
		{
			return ((this._content == null) ? 0 : this._content.GetHashCode()) ^ ((this._comments == null) ? 0 : this._comments.GetHashCode()) ^ (int)this._flags ^ (int)this._category;
		}

		// Token: 0x04001B48 RID: 6984
		private string _content;

		// Token: 0x04001B49 RID: 6985
		private string _comments;

		// Token: 0x04001B4A RID: 6986
		private BamlLocalizableResource.LocalizationFlags _flags;

		// Token: 0x04001B4B RID: 6987
		private LocalizationCategory _category;

		// Token: 0x020008B4 RID: 2228
		[Flags]
		private enum LocalizationFlags : byte
		{
			// Token: 0x04004204 RID: 16900
			Readable = 1,
			// Token: 0x04004205 RID: 16901
			Modifiable = 2
		}
	}
}
