using System;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x0200078C RID: 1932
	[Serializable]
	internal abstract class JournalEntryPageFunctionSaver : JournalEntryPageFunction, ISerializable
	{
		// Token: 0x06007969 RID: 31081 RVA: 0x0022718A File Offset: 0x0022538A
		internal JournalEntryPageFunctionSaver(JournalEntryGroupState jeGroupState, PageFunctionBase pageFunction) : base(jeGroupState, pageFunction)
		{
		}

		// Token: 0x0600796A RID: 31082 RVA: 0x00227194 File Offset: 0x00225394
		protected JournalEntryPageFunctionSaver(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._returnEventSaver = (ReturnEventSaver)info.GetValue("_returnEventSaver", typeof(ReturnEventSaver));
		}

		// Token: 0x0600796B RID: 31083 RVA: 0x002271BE File Offset: 0x002253BE
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_returnEventSaver", this._returnEventSaver);
		}

		// Token: 0x0600796C RID: 31084 RVA: 0x002271DC File Offset: 0x002253DC
		internal override void SaveState(object contentObject)
		{
			PageFunctionBase pageFunctionBase = (PageFunctionBase)contentObject;
			this._returnEventSaver = pageFunctionBase._Saver;
			base.SaveState(contentObject);
		}

		// Token: 0x0600796D RID: 31085 RVA: 0x00227204 File Offset: 0x00225404
		internal override void RestoreState(object contentObject)
		{
			if (contentObject == null)
			{
				throw new ArgumentNullException("contentObject");
			}
			PageFunctionBase pageFunctionBase = (PageFunctionBase)contentObject;
			if (pageFunctionBase == null)
			{
				throw new Exception(SR.Get("InvalidPageFunctionType", new object[]
				{
					contentObject.GetType()
				}));
			}
			pageFunctionBase.ParentPageFunctionId = base.ParentPageFunctionId;
			pageFunctionBase.PageFunctionId = base.PageFunctionId;
			pageFunctionBase._Saver = this._returnEventSaver;
			pageFunctionBase._Resume = true;
			base.RestoreState(pageFunctionBase);
		}

		// Token: 0x0600796E RID: 31086 RVA: 0x0022727C File Offset: 0x0022547C
		internal override bool Navigate(INavigator navigator, NavigationMode navMode)
		{
			IDownloader downloader = navigator as IDownloader;
			NavigationService navigationService = (downloader != null) ? downloader.Downloader : null;
			PageFunctionBase content = (navigationService != null && navigationService.ContentId == base.ContentId) ? ((PageFunctionBase)navigationService.Content) : this.ResumePageFunction();
			return navigator.Navigate(content, new NavigateInfo(base.Source, navMode, this));
		}

		// Token: 0x04003982 RID: 14722
		private ReturnEventSaver _returnEventSaver;
	}
}
