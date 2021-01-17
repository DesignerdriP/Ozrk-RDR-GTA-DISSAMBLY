using System;
using System.Windows;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A4 RID: 1700
	internal sealed class BamlStartElementNode : BamlTreeNode, ILocalizabilityInheritable
	{
		// Token: 0x06006E5D RID: 28253 RVA: 0x001FBEC2 File Offset: 0x001FA0C2
		internal BamlStartElementNode(string assemblyName, string typeFullName, bool isInjected, bool useTypeConverter) : base(BamlNodeType.StartElement)
		{
			this._assemblyName = assemblyName;
			this._typeFullName = typeFullName;
			this._isInjected = isInjected;
			this._useTypeConverter = useTypeConverter;
		}

		// Token: 0x06006E5E RID: 28254 RVA: 0x001FBEE8 File Offset: 0x001FA0E8
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteStartElement(this._assemblyName, this._typeFullName, this._isInjected, this._useTypeConverter);
		}

		// Token: 0x06006E5F RID: 28255 RVA: 0x001FBF08 File Offset: 0x001FA108
		internal override BamlTreeNode Copy()
		{
			return new BamlStartElementNode(this._assemblyName, this._typeFullName, this._isInjected, this._useTypeConverter)
			{
				_content = this._content,
				_uid = this._uid,
				_inheritableAttribute = this._inheritableAttribute
			};
		}

		// Token: 0x06006E60 RID: 28256 RVA: 0x001FBF58 File Offset: 0x001FA158
		internal void InsertProperty(BamlTreeNode child)
		{
			if (this._children == null)
			{
				base.AddChild(child);
				return;
			}
			int index = 0;
			for (int i = 0; i < this._children.Count; i++)
			{
				if (this._children[i].NodeType == BamlNodeType.Property)
				{
					index = i;
				}
			}
			this._children.Insert(index, child);
			child.Parent = this;
		}

		// Token: 0x17001A36 RID: 6710
		// (get) Token: 0x06006E61 RID: 28257 RVA: 0x001FBFB7 File Offset: 0x001FA1B7
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17001A37 RID: 6711
		// (get) Token: 0x06006E62 RID: 28258 RVA: 0x001FBFBF File Offset: 0x001FA1BF
		internal string TypeFullName
		{
			get
			{
				return this._typeFullName;
			}
		}

		// Token: 0x17001A38 RID: 6712
		// (get) Token: 0x06006E63 RID: 28259 RVA: 0x001FBFC7 File Offset: 0x001FA1C7
		// (set) Token: 0x06006E64 RID: 28260 RVA: 0x001FBFCF File Offset: 0x001FA1CF
		internal string Content
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

		// Token: 0x17001A39 RID: 6713
		// (get) Token: 0x06006E65 RID: 28261 RVA: 0x001FBFD8 File Offset: 0x001FA1D8
		// (set) Token: 0x06006E66 RID: 28262 RVA: 0x001FBFE0 File Offset: 0x001FA1E0
		internal string Uid
		{
			get
			{
				return this._uid;
			}
			set
			{
				this._uid = value;
			}
		}

		// Token: 0x17001A3A RID: 6714
		// (get) Token: 0x06006E67 RID: 28263 RVA: 0x001FBFEC File Offset: 0x001FA1EC
		public ILocalizabilityInheritable LocalizabilityAncestor
		{
			get
			{
				if (this._localizabilityAncestor == null)
				{
					BamlTreeNode parent = base.Parent;
					while (this._localizabilityAncestor == null && parent != null)
					{
						this._localizabilityAncestor = (parent as ILocalizabilityInheritable);
						parent = parent.Parent;
					}
				}
				return this._localizabilityAncestor;
			}
		}

		// Token: 0x17001A3B RID: 6715
		// (get) Token: 0x06006E68 RID: 28264 RVA: 0x001FC02E File Offset: 0x001FA22E
		// (set) Token: 0x06006E69 RID: 28265 RVA: 0x001FC036 File Offset: 0x001FA236
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

		// Token: 0x17001A3C RID: 6716
		// (get) Token: 0x06006E6A RID: 28266 RVA: 0x001FC03F File Offset: 0x001FA23F
		// (set) Token: 0x06006E6B RID: 28267 RVA: 0x001FC047 File Offset: 0x001FA247
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

		// Token: 0x04003648 RID: 13896
		private string _assemblyName;

		// Token: 0x04003649 RID: 13897
		private string _typeFullName;

		// Token: 0x0400364A RID: 13898
		private string _content;

		// Token: 0x0400364B RID: 13899
		private string _uid;

		// Token: 0x0400364C RID: 13900
		private LocalizabilityAttribute _inheritableAttribute;

		// Token: 0x0400364D RID: 13901
		private ILocalizabilityInheritable _localizabilityAncestor;

		// Token: 0x0400364E RID: 13902
		private bool _isIgnored;

		// Token: 0x0400364F RID: 13903
		private bool _isInjected;

		// Token: 0x04003650 RID: 13904
		private bool _useTypeConverter;
	}
}
