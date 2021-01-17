using System;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200065D RID: 1629
	internal class ElementTableKey
	{
		// Token: 0x06006C17 RID: 27671 RVA: 0x001F1AA1 File Offset: 0x001EFCA1
		internal ElementTableKey(string xmlNamespace, string baseName)
		{
			if (xmlNamespace == null)
			{
				throw new ArgumentNullException("xmlNamespace");
			}
			if (baseName == null)
			{
				throw new ArgumentNullException("baseName");
			}
			this._xmlNamespace = xmlNamespace;
			this._baseName = baseName;
		}

		// Token: 0x06006C18 RID: 27672 RVA: 0x001F1AD4 File Offset: 0x001EFCD4
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			if (other.GetType() != base.GetType())
			{
				return false;
			}
			ElementTableKey elementTableKey = (ElementTableKey)other;
			return string.CompareOrdinal(this.BaseName, elementTableKey.BaseName) == 0 && string.CompareOrdinal(this.XmlNamespace, elementTableKey.XmlNamespace) == 0;
		}

		// Token: 0x06006C19 RID: 27673 RVA: 0x001F1B2B File Offset: 0x001EFD2B
		public override int GetHashCode()
		{
			return this.XmlNamespace.GetHashCode() ^ this.BaseName.GetHashCode();
		}

		// Token: 0x170019D9 RID: 6617
		// (get) Token: 0x06006C1A RID: 27674 RVA: 0x001F1B44 File Offset: 0x001EFD44
		internal string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
		}

		// Token: 0x170019DA RID: 6618
		// (get) Token: 0x06006C1B RID: 27675 RVA: 0x001F1B4C File Offset: 0x001EFD4C
		internal string BaseName
		{
			get
			{
				return this._baseName;
			}
		}

		// Token: 0x0400350B RID: 13579
		private string _baseName;

		// Token: 0x0400350C RID: 13580
		private string _xmlNamespace;

		// Token: 0x0400350D RID: 13581
		public static readonly string XamlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

		// Token: 0x0400350E RID: 13582
		public static readonly string FixedMarkupNamespace = "http://schemas.microsoft.com/xps/2005/06";
	}
}
