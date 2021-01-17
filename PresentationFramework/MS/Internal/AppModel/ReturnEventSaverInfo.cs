using System;

namespace MS.Internal.AppModel
{
	// Token: 0x02000798 RID: 1944
	[Serializable]
	internal struct ReturnEventSaverInfo
	{
		// Token: 0x060079D8 RID: 31192 RVA: 0x00228682 File Offset: 0x00226882
		internal ReturnEventSaverInfo(string delegateTypeName, string targetTypeName, string delegateMethodName, bool fSamePf)
		{
			this._delegateTypeName = delegateTypeName;
			this._targetTypeName = targetTypeName;
			this._delegateMethodName = delegateMethodName;
			this._delegateInSamePF = fSamePf;
		}

		// Token: 0x0400399F RID: 14751
		internal string _delegateTypeName;

		// Token: 0x040039A0 RID: 14752
		internal string _targetTypeName;

		// Token: 0x040039A1 RID: 14753
		internal string _delegateMethodName;

		// Token: 0x040039A2 RID: 14754
		internal bool _delegateInSamePF;
	}
}
