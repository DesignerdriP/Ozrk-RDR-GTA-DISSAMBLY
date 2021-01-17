﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MS.Internal;
using MS.Internal.Annotations;

namespace System.Windows.Annotations
{
	/// <summary>Represents an ordered set of <see cref="T:System.Windows.Annotations.ContentLocator" /> elements that identify an item of content.</summary>
	// Token: 0x020005D2 RID: 1490
	[XmlRoot(Namespace = "http://schemas.microsoft.com/windows/annotations/2003/11/core", ElementName = "ContentLocatorGroup")]
	public sealed class ContentLocatorGroup : ContentLocatorBase, IXmlSerializable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Annotations.ContentLocatorGroup" /> class.</summary>
		// Token: 0x06006341 RID: 25409 RVA: 0x001BEB8D File Offset: 0x001BCD8D
		public ContentLocatorGroup()
		{
			this._locators = new AnnotationObservableCollection<ContentLocator>();
			this._locators.CollectionChanged += this.OnCollectionChanged;
		}

		/// <summary>Creates a modifiable deep copy clone of this <see cref="T:System.Windows.Annotations.ContentLocatorGroup" />.</summary>
		/// <returns>A modifiable deep copy clone of this <see cref="T:System.Windows.Annotations.ContentLocatorGroup" />.</returns>
		// Token: 0x06006342 RID: 25410 RVA: 0x001BEBB8 File Offset: 0x001BCDB8
		public override object Clone()
		{
			ContentLocatorGroup contentLocatorGroup = new ContentLocatorGroup();
			foreach (ContentLocator contentLocator in this._locators)
			{
				contentLocatorGroup.Locators.Add((ContentLocator)contentLocator.Clone());
			}
			return contentLocatorGroup;
		}

		/// <summary>Always returns <see langword="null" />.  See Annotations Schema for schema details.</summary>
		/// <returns>Always <see langword="null" />.  See Annotations Schema for schema details.</returns>
		// Token: 0x06006343 RID: 25411 RVA: 0x0000C238 File Offset: 0x0000A438
		public XmlSchema GetSchema()
		{
			return null;
		}

		/// <summary>Serializes the <see cref="T:System.Windows.Annotations.ContentLocatorGroup" /> to a specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="writer">The XML writer to use to serialize the <see cref="T:System.Windows.Annotations.ContentLocatorGroup" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="writer" /> is <see langword="null" />.</exception>
		// Token: 0x06006344 RID: 25412 RVA: 0x001BEC1C File Offset: 0x001BCE1C
		public void WriteXml(XmlWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (writer.LookupPrefix("http://schemas.microsoft.com/windows/annotations/2003/11/core") == null)
			{
				writer.WriteAttributeString("xmlns", "anc", null, "http://schemas.microsoft.com/windows/annotations/2003/11/core");
			}
			foreach (ContentLocatorBase contentLocatorBase in this._locators)
			{
				if (contentLocatorBase != null)
				{
					AnnotationResource.ListSerializer.Serialize(writer, contentLocatorBase);
				}
			}
		}

		/// <summary>Deserializes the <see cref="T:System.Windows.Annotations.ContentLocatorGroup" /> from a specified <see cref="T:System.Xml.XmlReader" />.</summary>
		/// <param name="reader">The XML reader to use to deserialize the <see cref="T:System.Windows.Annotations.ContentLocatorGroup" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="reader" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Xml.XmlException">The serialized XML for the <see cref="T:System.Windows.Annotations.ContentLocatorGroup" /> is not valid.</exception>
		// Token: 0x06006345 RID: 25413 RVA: 0x001BECA4 File Offset: 0x001BCEA4
		public void ReadXml(XmlReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			Annotation.CheckForNonNamespaceAttribute(reader, "ContentLocatorGroup");
			if (!reader.IsEmptyElement)
			{
				reader.Read();
				while (!("ContentLocatorGroup" == reader.LocalName) || XmlNodeType.EndElement != reader.NodeType)
				{
					if (!("ContentLocator" == reader.LocalName))
					{
						throw new XmlException(SR.Get("InvalidXmlContent", new object[]
						{
							"ContentLocatorGroup"
						}));
					}
					ContentLocator item = (ContentLocator)AnnotationResource.ListSerializer.Deserialize(reader);
					this._locators.Add(item);
				}
			}
			reader.Read();
		}

		/// <summary>Gets the collection of the <see cref="T:System.Windows.Annotations.ContentLocator" /> elements that make up this <see cref="T:System.Windows.Annotations.ContentLocatorGroup" />.</summary>
		/// <returns>The collection of <see cref="T:System.Windows.Annotations.ContentLocator" /> elements that make up this <see cref="T:System.Windows.Annotations.ContentLocatorGroup" />.</returns>
		// Token: 0x170017CE RID: 6094
		// (get) Token: 0x06006346 RID: 25414 RVA: 0x001BED4D File Offset: 0x001BCF4D
		public Collection<ContentLocator> Locators
		{
			get
			{
				return this._locators;
			}
		}

		// Token: 0x06006347 RID: 25415 RVA: 0x001BED58 File Offset: 0x001BCF58
		internal override ContentLocatorBase Merge(ContentLocatorBase other)
		{
			if (other == null)
			{
				return this;
			}
			ContentLocator contentLocator = null;
			ContentLocatorGroup contentLocatorGroup = other as ContentLocatorGroup;
			if (contentLocatorGroup != null)
			{
				List<ContentLocatorBase> list = new List<ContentLocatorBase>(contentLocatorGroup.Locators.Count * (this.Locators.Count - 1));
				foreach (ContentLocator contentLocator2 in this.Locators)
				{
					foreach (ContentLocator contentLocator3 in contentLocatorGroup.Locators)
					{
						if (contentLocator == null)
						{
							contentLocator = contentLocator3;
						}
						else
						{
							ContentLocator contentLocator4 = (ContentLocator)contentLocator2.Clone();
							contentLocator4.Append(contentLocator3);
							list.Add(contentLocator4);
						}
					}
					contentLocator2.Append(contentLocator);
					contentLocator = null;
				}
				using (List<ContentLocatorBase>.Enumerator enumerator3 = list.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						ContentLocatorBase contentLocatorBase = enumerator3.Current;
						ContentLocator item = (ContentLocator)contentLocatorBase;
						this.Locators.Add(item);
					}
					return this;
				}
			}
			ContentLocator contentLocator5 = other as ContentLocator;
			Invariant.Assert(contentLocator5 != null, "other should be of type ContentLocator");
			foreach (ContentLocator contentLocator6 in this.Locators)
			{
				contentLocator6.Append(contentLocator5);
			}
			return this;
		}

		// Token: 0x06006348 RID: 25416 RVA: 0x001BEEE4 File Offset: 0x001BD0E4
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			base.FireLocatorChanged("Locators");
		}

		// Token: 0x040031CC RID: 12748
		private AnnotationObservableCollection<ContentLocator> _locators;
	}
}
