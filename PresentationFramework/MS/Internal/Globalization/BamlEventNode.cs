using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006AD RID: 1709
	internal sealed class BamlEventNode : BamlTreeNode
	{
		// Token: 0x06006E95 RID: 28309 RVA: 0x001FC326 File Offset: 0x001FA526
		internal BamlEventNode(string eventName, string handlerName) : base(BamlNodeType.Event)
		{
			this._eventName = eventName;
			this._handlerName = handlerName;
		}

		// Token: 0x06006E96 RID: 28310 RVA: 0x001FC33E File Offset: 0x001FA53E
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteEvent(this._eventName, this._handlerName);
		}

		// Token: 0x06006E97 RID: 28311 RVA: 0x001FC352 File Offset: 0x001FA552
		internal override BamlTreeNode Copy()
		{
			return new BamlEventNode(this._eventName, this._handlerName);
		}

		// Token: 0x04003664 RID: 13924
		private string _eventName;

		// Token: 0x04003665 RID: 13925
		private string _handlerName;
	}
}
