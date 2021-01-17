using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using MS.Internal.Annotations;
using MS.Internal.Annotations.Anchoring;

namespace System.Windows.Annotations
{
	/// <summary>Represents a set of name/value pairs that identify an item of content.</summary>
	// Token: 0x020005D0 RID: 1488
	public sealed class ContentLocatorPart : INotifyPropertyChanged2, INotifyPropertyChanged, IOwnedObject
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Annotations.ContentLocatorPart" /> class with a specified type name and namespace.</summary>
		/// <param name="partType">The type name and namespace for the <see cref="T:System.Windows.Annotations.ContentLocatorPart" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="partType" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The strings <paramref name="partType" />.<see cref="P:System.Xml.XmlQualifiedName.Name" /> or <paramref name="partType" />.<see cref="P:System.Xml.XmlQualifiedName.Namespace" /> (or both) are <see langword="null" /> or empty.</exception>
		// Token: 0x06006325 RID: 25381 RVA: 0x001BDF7C File Offset: 0x001BC17C
		public ContentLocatorPart(XmlQualifiedName partType)
		{
			if (partType == null)
			{
				throw new ArgumentNullException("partType");
			}
			if (string.IsNullOrEmpty(partType.Name))
			{
				throw new ArgumentException(SR.Get("TypeNameMustBeSpecified"), "partType.Name");
			}
			if (string.IsNullOrEmpty(partType.Namespace))
			{
				throw new ArgumentException(SR.Get("TypeNameMustBeSpecified"), "partType.Namespace");
			}
			this._type = partType;
			this._nameValues = new ObservableDictionary();
			this._nameValues.PropertyChanged += this.OnPropertyChanged;
		}

		/// <summary>Returns a value that indicates whether a given <see cref="T:System.Windows.Annotations.ContentLocatorPart" /> is identical to this <see cref="T:System.Windows.Annotations.ContentLocatorPart" />.</summary>
		/// <param name="obj">The part to compare for equality.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Annotations.ContentLocatorPart.NameValuePairs" /> within both parts are identical; otherwise, <see langword="false" />.</returns>
		// Token: 0x06006326 RID: 25382 RVA: 0x001BE010 File Offset: 0x001BC210
		public override bool Equals(object obj)
		{
			ContentLocatorPart contentLocatorPart = obj as ContentLocatorPart;
			if (contentLocatorPart == this)
			{
				return true;
			}
			if (contentLocatorPart == null)
			{
				return false;
			}
			if (!this._type.Equals(contentLocatorPart.PartType))
			{
				return false;
			}
			if (contentLocatorPart.NameValuePairs.Count != this._nameValues.Count)
			{
				return false;
			}
			foreach (KeyValuePair<string, string> keyValuePair in this._nameValues)
			{
				string b;
				if (!contentLocatorPart._nameValues.TryGetValue(keyValuePair.Key, out b))
				{
					return false;
				}
				if (keyValuePair.Value != b)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Returns the hash code for this part.</summary>
		/// <returns>The hash code for this part.</returns>
		// Token: 0x06006327 RID: 25383 RVA: 0x001B8BAC File Offset: 0x001B6DAC
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>Creates a modifiable deep copy clone of this <see cref="T:System.Windows.Annotations.ContentLocatorPart" />.</summary>
		/// <returns>A modifiable deep copy clone of this <see cref="T:System.Windows.Annotations.ContentLocatorPart" />.</returns>
		// Token: 0x06006328 RID: 25384 RVA: 0x001BE0CC File Offset: 0x001BC2CC
		public object Clone()
		{
			ContentLocatorPart contentLocatorPart = new ContentLocatorPart(this._type);
			foreach (KeyValuePair<string, string> keyValuePair in this._nameValues)
			{
				contentLocatorPart.NameValuePairs.Add(keyValuePair.Key, keyValuePair.Value);
			}
			return contentLocatorPart;
		}

		/// <summary>Gets a collection of the name/value pairs that define this part.</summary>
		/// <returns>The collection of the name/value pairs that define this <see cref="T:System.Windows.Annotations.ContentLocatorPart" />.</returns>
		// Token: 0x170017CA RID: 6090
		// (get) Token: 0x06006329 RID: 25385 RVA: 0x001BE138 File Offset: 0x001BC338
		public IDictionary<string, string> NameValuePairs
		{
			get
			{
				return this._nameValues;
			}
		}

		/// <summary>Gets the type name and namespace of the part.</summary>
		/// <returns>The type name and namespace of the part.</returns>
		// Token: 0x170017CB RID: 6091
		// (get) Token: 0x0600632A RID: 25386 RVA: 0x001BE140 File Offset: 0x001BC340
		public XmlQualifiedName PartType
		{
			get
			{
				return this._type;
			}
		}

		/// <summary>This event supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. </summary>
		// Token: 0x1400012E RID: 302
		// (add) Token: 0x0600632B RID: 25387 RVA: 0x001BE148 File Offset: 0x001BC348
		// (remove) Token: 0x0600632C RID: 25388 RVA: 0x001BE151 File Offset: 0x001BC351
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChanged += value;
			}
			remove
			{
				this._propertyChanged -= value;
			}
		}

		// Token: 0x0600632D RID: 25389 RVA: 0x001BE15C File Offset: 0x001BC35C
		internal bool Matches(ContentLocatorPart part)
		{
			bool flag = false;
			string value;
			this._nameValues.TryGetValue("IncludeOverlaps", out value);
			if (!bool.TryParse(value, out flag) || !flag)
			{
				return this.Equals(part);
			}
			if (part == this)
			{
				return true;
			}
			if (!this._type.Equals(part.PartType))
			{
				return false;
			}
			int num;
			int num2;
			TextSelectionProcessor.GetMaxMinLocatorPartValues(this, out num, out num2);
			int num3;
			int num4;
			TextSelectionProcessor.GetMaxMinLocatorPartValues(part, out num3, out num4);
			return (num == num3 && num2 == num4) || (num != int.MinValue && ((num3 >= num && num3 <= num2) || (num3 < num && num4 >= num)));
		}

		// Token: 0x0600632E RID: 25390 RVA: 0x001BE1F0 File Offset: 0x001BC3F0
		internal string GetQueryFragment(XmlNamespaceManager namespaceManager)
		{
			bool flag = false;
			string value;
			this._nameValues.TryGetValue("IncludeOverlaps", out value);
			if (bool.TryParse(value, out flag) && flag)
			{
				return this.GetOverlapQueryFragment(namespaceManager);
			}
			return this.GetExactQueryFragment(namespaceManager);
		}

		// Token: 0x170017CC RID: 6092
		// (get) Token: 0x0600632F RID: 25391 RVA: 0x001BE22D File Offset: 0x001BC42D
		// (set) Token: 0x06006330 RID: 25392 RVA: 0x001BE235 File Offset: 0x001BC435
		bool IOwnedObject.Owned
		{
			get
			{
				return this._owned;
			}
			set
			{
				this._owned = value;
			}
		}

		// Token: 0x06006331 RID: 25393 RVA: 0x001BE23E File Offset: 0x001BC43E
		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this._propertyChanged != null)
			{
				this._propertyChanged(this, new PropertyChangedEventArgs("NameValuePairs"));
			}
		}

		// Token: 0x06006332 RID: 25394 RVA: 0x001BE260 File Offset: 0x001BC460
		private string GetOverlapQueryFragment(XmlNamespaceManager namespaceManager)
		{
			string text = namespaceManager.LookupPrefix("http://schemas.microsoft.com/windows/annotations/2003/11/core");
			string text2 = namespaceManager.LookupPrefix(this.PartType.Namespace);
			string text3 = (text2 == null) ? "" : (text2 + ":");
			text3 = string.Concat(new string[]
			{
				text3,
				TextSelectionProcessor.CharacterRangeElementName.Name,
				"/",
				text,
				":Item"
			});
			int num;
			int num2;
			TextSelectionProcessor.GetMaxMinLocatorPartValues(this, out num, out num2);
			string text4 = num.ToString(NumberFormatInfo.InvariantInfo);
			string text5 = num2.ToString(NumberFormatInfo.InvariantInfo);
			return string.Concat(new string[]
			{
				text3,
				"[starts-with(@Name, \"Segment\") and  ((substring-before(@Value,\",\") >= ",
				text4,
				" and substring-before(@Value,\",\") <= ",
				text5,
				") or   (substring-before(@Value,\",\") < ",
				text4,
				" and substring-after(@Value,\",\") >= ",
				text4,
				"))]"
			});
		}

		// Token: 0x06006333 RID: 25395 RVA: 0x001BE348 File Offset: 0x001BC548
		private string GetExactQueryFragment(XmlNamespaceManager namespaceManager)
		{
			string str = namespaceManager.LookupPrefix("http://schemas.microsoft.com/windows/annotations/2003/11/core");
			string text = namespaceManager.LookupPrefix(this.PartType.Namespace);
			string text2 = (text == null) ? "" : (text + ":");
			text2 += this.PartType.Name;
			bool flag = false;
			foreach (KeyValuePair<string, string> keyValuePair in this.NameValuePairs)
			{
				if (flag)
				{
					text2 = text2 + "/parent::*/" + str + ":Item[";
				}
				else
				{
					flag = true;
					text2 = text2 + "/" + str + ":Item[";
				}
				text2 = string.Concat(new string[]
				{
					text2,
					"@Name=\"",
					keyValuePair.Key,
					"\" and @Value=\"",
					keyValuePair.Value,
					"\"]"
				});
			}
			if (flag)
			{
				text2 += "/parent::*";
			}
			return text2;
		}

		// Token: 0x1400012F RID: 303
		// (add) Token: 0x06006334 RID: 25396 RVA: 0x001BE454 File Offset: 0x001BC654
		// (remove) Token: 0x06006335 RID: 25397 RVA: 0x001BE48C File Offset: 0x001BC68C
		private event PropertyChangedEventHandler _propertyChanged;

		// Token: 0x040031C7 RID: 12743
		private bool _owned;

		// Token: 0x040031C8 RID: 12744
		private XmlQualifiedName _type;

		// Token: 0x040031C9 RID: 12745
		private ObservableDictionary _nameValues;
	}
}
