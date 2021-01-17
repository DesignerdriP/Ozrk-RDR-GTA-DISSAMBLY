using System;

namespace MS.Internal.AppModel
{
	// Token: 0x020007C0 RID: 1984
	[Serializable]
	internal struct SubStream
	{
		// Token: 0x06007B58 RID: 31576 RVA: 0x0022AFEA File Offset: 0x002291EA
		internal SubStream(string propertyName, byte[] dataBytes)
		{
			this._propertyName = propertyName;
			this._data = dataBytes;
		}

		// Token: 0x04003A13 RID: 14867
		internal string _propertyName;

		// Token: 0x04003A14 RID: 14868
		internal byte[] _data;
	}
}
