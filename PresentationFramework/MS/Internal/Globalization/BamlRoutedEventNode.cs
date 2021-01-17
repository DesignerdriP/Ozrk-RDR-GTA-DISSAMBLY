using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006AC RID: 1708
	internal sealed class BamlRoutedEventNode : BamlTreeNode
	{
		// Token: 0x06006E92 RID: 28306 RVA: 0x001FC2C0 File Offset: 0x001FA4C0
		internal BamlRoutedEventNode(string assemblyName, string ownerTypeFullName, string eventIdName, string handlerName) : base(BamlNodeType.RoutedEvent)
		{
			this._assemblyName = assemblyName;
			this._ownerTypeFullName = ownerTypeFullName;
			this._eventIdName = eventIdName;
			this._handlerName = handlerName;
		}

		// Token: 0x06006E93 RID: 28307 RVA: 0x001FC2E7 File Offset: 0x001FA4E7
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteRoutedEvent(this._assemblyName, this._ownerTypeFullName, this._eventIdName, this._handlerName);
		}

		// Token: 0x06006E94 RID: 28308 RVA: 0x001FC307 File Offset: 0x001FA507
		internal override BamlTreeNode Copy()
		{
			return new BamlRoutedEventNode(this._assemblyName, this._ownerTypeFullName, this._eventIdName, this._handlerName);
		}

		// Token: 0x04003660 RID: 13920
		private string _assemblyName;

		// Token: 0x04003661 RID: 13921
		private string _ownerTypeFullName;

		// Token: 0x04003662 RID: 13922
		private string _eventIdName;

		// Token: 0x04003663 RID: 13923
		private string _handlerName;
	}
}
