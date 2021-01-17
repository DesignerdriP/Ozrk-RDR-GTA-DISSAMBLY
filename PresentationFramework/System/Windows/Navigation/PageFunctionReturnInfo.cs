using System;

namespace System.Windows.Navigation
{
	// Token: 0x0200031B RID: 795
	internal class PageFunctionReturnInfo : NavigateInfo
	{
		// Token: 0x060029FF RID: 10751 RVA: 0x000C1A81 File Offset: 0x000BFC81
		internal PageFunctionReturnInfo(PageFunctionBase finishingChildPageFunction, Uri source, NavigationMode navigationMode, JournalEntry journalEntry, object returnEventArgs) : base(source, navigationMode, journalEntry)
		{
			this._returnEventArgs = returnEventArgs;
			this._finishingChildPageFunction = finishingChildPageFunction;
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06002A00 RID: 10752 RVA: 0x000C1A9C File Offset: 0x000BFC9C
		internal object ReturnEventArgs
		{
			get
			{
				return this._returnEventArgs;
			}
		}

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06002A01 RID: 10753 RVA: 0x000C1AA4 File Offset: 0x000BFCA4
		internal PageFunctionBase FinishingChildPageFunction
		{
			get
			{
				return this._finishingChildPageFunction;
			}
		}

		// Token: 0x04001C23 RID: 7203
		private object _returnEventArgs;

		// Token: 0x04001C24 RID: 7204
		private PageFunctionBase _finishingChildPageFunction;
	}
}
