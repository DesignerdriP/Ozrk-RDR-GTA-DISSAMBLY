using System;

namespace System.Windows.Markup
{
	// Token: 0x02000251 RID: 593
	internal class XamlRoutedEventNode : XamlAttributeNode
	{
		// Token: 0x060022EC RID: 8940 RVA: 0x000AC4BE File Offset: 0x000AA6BE
		internal XamlRoutedEventNode(int lineNumber, int linePosition, int depth, RoutedEvent routedEvent, string assemblyName, string typeFullName, string routedEventName, string value) : base(XamlNodeType.RoutedEvent, lineNumber, linePosition, depth, value)
		{
			this._routedEvent = routedEvent;
			this._assemblyName = assemblyName;
			this._typeFullName = typeFullName;
			this._routedEventName = routedEventName;
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x060022ED RID: 8941 RVA: 0x000AC4ED File Offset: 0x000AA6ED
		internal RoutedEvent Event
		{
			get
			{
				return this._routedEvent;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x060022EE RID: 8942 RVA: 0x000AC4F5 File Offset: 0x000AA6F5
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x060022EF RID: 8943 RVA: 0x000AC4FD File Offset: 0x000AA6FD
		internal string TypeFullName
		{
			get
			{
				return this._typeFullName;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x060022F0 RID: 8944 RVA: 0x000AC505 File Offset: 0x000AA705
		internal string EventName
		{
			get
			{
				return this._routedEventName;
			}
		}

		// Token: 0x04001A55 RID: 6741
		private RoutedEvent _routedEvent;

		// Token: 0x04001A56 RID: 6742
		private string _assemblyName;

		// Token: 0x04001A57 RID: 6743
		private string _typeFullName;

		// Token: 0x04001A58 RID: 6744
		private string _routedEventName;
	}
}
