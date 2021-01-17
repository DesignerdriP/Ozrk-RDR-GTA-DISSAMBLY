using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using MS.Internal;

namespace System.Windows.Data
{
	/// <summary>Describes the grouping of items using a property name as the criteria.</summary>
	// Token: 0x020001B9 RID: 441
	public class PropertyGroupDescription : GroupDescription
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.PropertyGroupDescription" /> class.</summary>
		// Token: 0x06001C89 RID: 7305 RVA: 0x00086264 File Offset: 0x00084464
		public PropertyGroupDescription()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.PropertyGroupDescription" /> class with the specified property name.</summary>
		/// <param name="propertyName">The name of the property that specifies which group an item belongs to.</param>
		// Token: 0x06001C8A RID: 7306 RVA: 0x00086273 File Offset: 0x00084473
		public PropertyGroupDescription(string propertyName)
		{
			this.UpdatePropertyName(propertyName);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.PropertyGroupDescription" /> class with the specified property name and converter.</summary>
		/// <param name="propertyName">The name of the property that specifies which group an item belongs to. If this is <see langword="null" />, the item itself is passed to the value converter.</param>
		/// <param name="converter">An <see cref="T:System.Windows.Data.IValueConverter" /> object to apply to the property value or the item to produce the final value that is used to determine which group(s) an item belongs to. The converter may return a collection, which indicates the items can appear in more than one group.</param>
		// Token: 0x06001C8B RID: 7307 RVA: 0x00086289 File Offset: 0x00084489
		public PropertyGroupDescription(string propertyName, IValueConverter converter)
		{
			this.UpdatePropertyName(propertyName);
			this._converter = converter;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.PropertyGroupDescription" /> class with the specified parameters.</summary>
		/// <param name="propertyName">The name of the property that specifies which group an item belongs to. If this is <see langword="null" />, the item itself is passed to the value converter.</param>
		/// <param name="converter">An <see cref="T:System.Windows.Data.IValueConverter" /> object to apply to the property value or the item to produce the final value that is used to determine which group(s) an item belongs to. The converter may return a collection, which indicates the items can appear in more than one group.</param>
		/// <param name="stringComparison">A <see cref="T:System.StringComparison" /> value that specifies the comparison between the value of an item and the name of a group.</param>
		// Token: 0x06001C8C RID: 7308 RVA: 0x000862A6 File Offset: 0x000844A6
		public PropertyGroupDescription(string propertyName, IValueConverter converter, StringComparison stringComparison)
		{
			this.UpdatePropertyName(propertyName);
			this._converter = converter;
			this._stringComparison = stringComparison;
		}

		/// <summary>Gets or sets the name of the property that is used to determine which group(s) an item belongs to.</summary>
		/// <returns>The default value is <see langword="null" />.</returns>
		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06001C8D RID: 7309 RVA: 0x000862CA File Offset: 0x000844CA
		// (set) Token: 0x06001C8E RID: 7310 RVA: 0x000862D2 File Offset: 0x000844D2
		[DefaultValue(null)]
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
			set
			{
				this.UpdatePropertyName(value);
				this.OnPropertyChanged("PropertyName");
			}
		}

		/// <summary>Gets or sets a converter to apply to the property value or the item to produce the final value that is used to determine which group(s) an item belongs to.</summary>
		/// <returns>The default value is <see langword="null" />.</returns>
		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06001C8F RID: 7311 RVA: 0x000862E6 File Offset: 0x000844E6
		// (set) Token: 0x06001C90 RID: 7312 RVA: 0x000862EE File Offset: 0x000844EE
		[DefaultValue(null)]
		public IValueConverter Converter
		{
			get
			{
				return this._converter;
			}
			set
			{
				this._converter = value;
				this.OnPropertyChanged("Converter");
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.StringComparison" /> value that specifies the comparison between the value of an item (as determined by <see cref="P:System.Windows.Data.PropertyGroupDescription.PropertyName" /> and <see cref="P:System.Windows.Data.PropertyGroupDescription.Converter" />) and the name of a group.</summary>
		/// <returns>The default value is <see cref="T:System.StringComparison" />.<see langword="Ordinal" />.</returns>
		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06001C91 RID: 7313 RVA: 0x00086302 File Offset: 0x00084502
		// (set) Token: 0x06001C92 RID: 7314 RVA: 0x0008630A File Offset: 0x0008450A
		[DefaultValue(StringComparison.Ordinal)]
		public StringComparison StringComparison
		{
			get
			{
				return this._stringComparison;
			}
			set
			{
				this._stringComparison = value;
				this.OnPropertyChanged("StringComparison");
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.IComparer" /> value that orders groups in ascending order of name.</summary>
		/// <returns>An <see cref="T:System.Collections.IComparer" /> value that orders groups in ascending order of name.</returns>
		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06001C93 RID: 7315 RVA: 0x0008631E File Offset: 0x0008451E
		public static IComparer CompareNameAscending
		{
			get
			{
				return PropertyGroupDescription._compareNameAscending;
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.IComparer" /> value that orders groups in descending order of name.</summary>
		/// <returns>An <see cref="T:System.Collections.IComparer" /> value that orders groups in descending order of name.</returns>
		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06001C94 RID: 7316 RVA: 0x00086325 File Offset: 0x00084525
		public static IComparer CompareNameDescending
		{
			get
			{
				return PropertyGroupDescription._compareNameDescending;
			}
		}

		/// <summary>Returns the group name(s) for the given item. </summary>
		/// <param name="item">The item to return group names for.</param>
		/// <param name="level">The level of grouping.</param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to supply to the converter.</param>
		/// <returns>The group name(s) for the given item.</returns>
		// Token: 0x06001C95 RID: 7317 RVA: 0x0008632C File Offset: 0x0008452C
		public override object GroupNameFromItem(object item, int level, CultureInfo culture)
		{
			object obj;
			object obj2;
			if (string.IsNullOrEmpty(this.PropertyName))
			{
				obj = item;
			}
			else if (SystemXmlHelper.TryGetValueFromXmlNode(item, this.PropertyName, out obj2))
			{
				obj = obj2;
			}
			else
			{
				if (item != null)
				{
					using (this._propertyPath.SetContext(item))
					{
						obj = this._propertyPath.GetValue();
						goto IL_4F;
					}
				}
				obj = null;
			}
			IL_4F:
			if (this.Converter != null)
			{
				obj = this.Converter.Convert(obj, typeof(object), level, culture);
			}
			return obj;
		}

		/// <summary>Returns a value that indicates whether the group name and the item name match so that the item belongs to the group.</summary>
		/// <param name="groupName">The name of the group to check.</param>
		/// <param name="itemName">The name of the item to check.</param>
		/// <returns>
		///     <see langword="true" /> if the names match and the item belongs to the group; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001C96 RID: 7318 RVA: 0x000863C0 File Offset: 0x000845C0
		public override bool NamesMatch(object groupName, object itemName)
		{
			string text = groupName as string;
			string text2 = itemName as string;
			if (text != null && text2 != null)
			{
				return string.Equals(text, text2, this.StringComparison);
			}
			return object.Equals(groupName, itemName);
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x000863F6 File Offset: 0x000845F6
		private void UpdatePropertyName(string propertyName)
		{
			this._propertyName = propertyName;
			this._propertyPath = ((!string.IsNullOrEmpty(propertyName)) ? new PropertyPath(propertyName, new object[0]) : null);
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x0008641C File Offset: 0x0008461C
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x040013D9 RID: 5081
		private string _propertyName;

		// Token: 0x040013DA RID: 5082
		private PropertyPath _propertyPath;

		// Token: 0x040013DB RID: 5083
		private IValueConverter _converter;

		// Token: 0x040013DC RID: 5084
		private StringComparison _stringComparison = StringComparison.Ordinal;

		// Token: 0x040013DD RID: 5085
		private static readonly IComparer _compareNameAscending = new PropertyGroupDescription.NameComparer(ListSortDirection.Ascending);

		// Token: 0x040013DE RID: 5086
		private static readonly IComparer _compareNameDescending = new PropertyGroupDescription.NameComparer(ListSortDirection.Descending);

		// Token: 0x02000884 RID: 2180
		private class NameComparer : IComparer
		{
			// Token: 0x0600832F RID: 33583 RVA: 0x00244BE8 File Offset: 0x00242DE8
			public NameComparer(ListSortDirection direction)
			{
				this._direction = direction;
			}

			// Token: 0x06008330 RID: 33584 RVA: 0x00244BF8 File Offset: 0x00242DF8
			int IComparer.Compare(object x, object y)
			{
				CollectionViewGroup collectionViewGroup = x as CollectionViewGroup;
				object a = ((collectionViewGroup != null) ? collectionViewGroup.Name : null) ?? x;
				collectionViewGroup = (y as CollectionViewGroup);
				object b = ((collectionViewGroup != null) ? collectionViewGroup.Name : null) ?? y;
				int num = Comparer.DefaultInvariant.Compare(a, b);
				if (this._direction != ListSortDirection.Ascending)
				{
					return -num;
				}
				return num;
			}

			// Token: 0x0400415A RID: 16730
			private ListSortDirection _direction;
		}
	}
}
