using System;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x0200078B RID: 1931
	internal class JournalEntryPageFunctionKeepAlive : JournalEntryPageFunction
	{
		// Token: 0x06007962 RID: 31074 RVA: 0x002270FA File Offset: 0x002252FA
		internal JournalEntryPageFunctionKeepAlive(JournalEntryGroupState jeGroupState, PageFunctionBase pageFunction) : base(jeGroupState, pageFunction)
		{
			this._keepAlivePageFunction = pageFunction;
		}

		// Token: 0x06007963 RID: 31075 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool IsPageFunction()
		{
			return true;
		}

		// Token: 0x06007964 RID: 31076 RVA: 0x0022710B File Offset: 0x0022530B
		internal override bool IsAlive()
		{
			return this.KeepAlivePageFunction != null;
		}

		// Token: 0x17001CAD RID: 7341
		// (get) Token: 0x06007965 RID: 31077 RVA: 0x00227116 File Offset: 0x00225316
		internal PageFunctionBase KeepAlivePageFunction
		{
			get
			{
				return this._keepAlivePageFunction;
			}
		}

		// Token: 0x06007966 RID: 31078 RVA: 0x00227120 File Offset: 0x00225320
		internal override PageFunctionBase ResumePageFunction()
		{
			PageFunctionBase keepAlivePageFunction = this.KeepAlivePageFunction;
			keepAlivePageFunction._Resume = true;
			return keepAlivePageFunction;
		}

		// Token: 0x06007967 RID: 31079 RVA: 0x0022713C File Offset: 0x0022533C
		internal override void SaveState(object contentObject)
		{
			Invariant.Assert(this._keepAlivePageFunction == contentObject);
		}

		// Token: 0x06007968 RID: 31080 RVA: 0x0022714C File Offset: 0x0022534C
		internal override bool Navigate(INavigator navigator, NavigationMode navMode)
		{
			PageFunctionBase content = (navigator.Content == this._keepAlivePageFunction) ? this._keepAlivePageFunction : this.ResumePageFunction();
			return navigator.Navigate(content, new NavigateInfo(base.Source, navMode, this));
		}

		// Token: 0x04003981 RID: 14721
		private PageFunctionBase _keepAlivePageFunction;
	}
}
