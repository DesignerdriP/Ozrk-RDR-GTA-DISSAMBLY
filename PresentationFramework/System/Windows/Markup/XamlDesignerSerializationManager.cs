﻿using System;
using System.ComponentModel;
using System.Xml;

namespace System.Windows.Markup
{
	/// <summary>Provides services for XAML serialization by XAML designers or other callers that require advanced serialization.</summary>
	// Token: 0x02000239 RID: 569
	public class XamlDesignerSerializationManager : ServiceProviders
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.XamlDesignerSerializationManager" /> class.</summary>
		/// <param name="xmlWriter">The XML writer implementation to use as basis for the <see cref="T:System.Windows.Markup.XamlDesignerSerializationManager" />. </param>
		// Token: 0x06002288 RID: 8840 RVA: 0x000AB85F File Offset: 0x000A9A5F
		public XamlDesignerSerializationManager(XmlWriter xmlWriter)
		{
			this._xamlWriterMode = XamlWriterMode.Value;
			this._xmlWriter = xmlWriter;
		}

		/// <summary>Gets or sets the XAML writer mode.</summary>
		/// <returns>The XAML writer mode.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">Not a valid member of the <see cref="T:System.Windows.Markup.XamlWriterMode" /> enumeration.</exception>
		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06002289 RID: 8841 RVA: 0x000AB875 File Offset: 0x000A9A75
		// (set) Token: 0x0600228A RID: 8842 RVA: 0x000AB87D File Offset: 0x000A9A7D
		public XamlWriterMode XamlWriterMode
		{
			get
			{
				return this._xamlWriterMode;
			}
			set
			{
				if (!XamlDesignerSerializationManager.IsValidXamlWriterMode(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(XamlWriterMode));
				}
				this._xamlWriterMode = value;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x0600228B RID: 8843 RVA: 0x000AB8A4 File Offset: 0x000A9AA4
		internal XmlWriter XmlWriter
		{
			get
			{
				return this._xmlWriter;
			}
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x000AB8AC File Offset: 0x000A9AAC
		internal void ClearXmlWriter()
		{
			this._xmlWriter = null;
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x000AB8B5 File Offset: 0x000A9AB5
		private static bool IsValidXamlWriterMode(XamlWriterMode value)
		{
			return value == XamlWriterMode.Value || value == XamlWriterMode.Expression;
		}

		// Token: 0x040019F1 RID: 6641
		private XamlWriterMode _xamlWriterMode;

		// Token: 0x040019F2 RID: 6642
		private XmlWriter _xmlWriter;
	}
}
