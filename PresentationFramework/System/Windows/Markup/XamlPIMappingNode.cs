using System;
using System.Diagnostics;

namespace System.Windows.Markup
{
	// Token: 0x02000253 RID: 595
	[DebuggerDisplay("PIMap:{_xmlns}={_clrns};{_assy}")]
	internal class XamlPIMappingNode : XamlNode
	{
		// Token: 0x060022F4 RID: 8948 RVA: 0x000AC53A File Offset: 0x000AA73A
		internal XamlPIMappingNode(int lineNumber, int linePosition, int depth, string xmlNamespace, string clrNamespace, string assemblyName) : base(XamlNodeType.PIMapping, lineNumber, linePosition, depth)
		{
			this._xmlns = xmlNamespace;
			this._clrns = clrNamespace;
			this._assy = assemblyName;
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x060022F5 RID: 8949 RVA: 0x000AC55F File Offset: 0x000AA75F
		internal string XmlNamespace
		{
			get
			{
				return this._xmlns;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x060022F6 RID: 8950 RVA: 0x000AC567 File Offset: 0x000AA767
		internal string ClrNamespace
		{
			get
			{
				return this._clrns;
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x060022F7 RID: 8951 RVA: 0x000AC56F File Offset: 0x000AA76F
		internal string AssemblyName
		{
			get
			{
				return this._assy;
			}
		}

		// Token: 0x04001A5B RID: 6747
		private string _xmlns;

		// Token: 0x04001A5C RID: 6748
		private string _clrns;

		// Token: 0x04001A5D RID: 6749
		private string _assy;
	}
}
