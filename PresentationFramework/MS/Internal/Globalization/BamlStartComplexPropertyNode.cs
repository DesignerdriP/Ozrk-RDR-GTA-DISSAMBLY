using System;
using System.Windows;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A7 RID: 1703
	internal class BamlStartComplexPropertyNode : BamlTreeNode, ILocalizabilityInheritable
	{
		// Token: 0x06006E72 RID: 28274 RVA: 0x001FC0A6 File Offset: 0x001FA2A6
		internal BamlStartComplexPropertyNode(string assemblyName, string ownerTypeFullName, string propertyName) : base(BamlNodeType.StartComplexProperty)
		{
			this._assemblyName = assemblyName;
			this._ownerTypeFullName = ownerTypeFullName;
			this._propertyName = propertyName;
		}

		// Token: 0x06006E73 RID: 28275 RVA: 0x001FC0C5 File Offset: 0x001FA2C5
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteStartComplexProperty(this._assemblyName, this._ownerTypeFullName, this._propertyName);
		}

		// Token: 0x06006E74 RID: 28276 RVA: 0x001FC0DF File Offset: 0x001FA2DF
		internal override BamlTreeNode Copy()
		{
			return new BamlStartComplexPropertyNode(this._assemblyName, this._ownerTypeFullName, this._propertyName);
		}

		// Token: 0x17001A3D RID: 6717
		// (get) Token: 0x06006E75 RID: 28277 RVA: 0x001FC0F8 File Offset: 0x001FA2F8
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17001A3E RID: 6718
		// (get) Token: 0x06006E76 RID: 28278 RVA: 0x001FC100 File Offset: 0x001FA300
		internal string PropertyName
		{
			get
			{
				return this._propertyName;
			}
		}

		// Token: 0x17001A3F RID: 6719
		// (get) Token: 0x06006E77 RID: 28279 RVA: 0x001FC108 File Offset: 0x001FA308
		internal string OwnerTypeFullName
		{
			get
			{
				return this._ownerTypeFullName;
			}
		}

		// Token: 0x17001A40 RID: 6720
		// (get) Token: 0x06006E78 RID: 28280 RVA: 0x001FC110 File Offset: 0x001FA310
		// (set) Token: 0x06006E79 RID: 28281 RVA: 0x001FC118 File Offset: 0x001FA318
		public ILocalizabilityInheritable LocalizabilityAncestor
		{
			get
			{
				return this._localizabilityAncestor;
			}
			set
			{
				this._localizabilityAncestor = value;
			}
		}

		// Token: 0x17001A41 RID: 6721
		// (get) Token: 0x06006E7A RID: 28282 RVA: 0x001FC121 File Offset: 0x001FA321
		// (set) Token: 0x06006E7B RID: 28283 RVA: 0x001FC129 File Offset: 0x001FA329
		public LocalizabilityAttribute InheritableAttribute
		{
			get
			{
				return this._inheritableAttribute;
			}
			set
			{
				this._inheritableAttribute = value;
			}
		}

		// Token: 0x17001A42 RID: 6722
		// (get) Token: 0x06006E7C RID: 28284 RVA: 0x001FC132 File Offset: 0x001FA332
		// (set) Token: 0x06006E7D RID: 28285 RVA: 0x001FC13A File Offset: 0x001FA33A
		public bool IsIgnored
		{
			get
			{
				return this._isIgnored;
			}
			set
			{
				this._isIgnored = value;
			}
		}

		// Token: 0x04003653 RID: 13907
		protected string _assemblyName;

		// Token: 0x04003654 RID: 13908
		protected string _ownerTypeFullName;

		// Token: 0x04003655 RID: 13909
		protected string _propertyName;

		// Token: 0x04003656 RID: 13910
		private ILocalizabilityInheritable _localizabilityAncestor;

		// Token: 0x04003657 RID: 13911
		private LocalizabilityAttribute _inheritableAttribute;

		// Token: 0x04003658 RID: 13912
		private bool _isIgnored;
	}
}
