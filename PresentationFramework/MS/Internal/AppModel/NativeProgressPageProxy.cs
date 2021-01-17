using System;
using System.Security;
using System.Windows.Interop;
using System.Windows.Threading;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x02000796 RID: 1942
	internal class NativeProgressPageProxy : IProgressPage2, IProgressPage
	{
		// Token: 0x060079C7 RID: 31175 RVA: 0x0022858B File Offset: 0x0022678B
		[SecurityCritical]
		internal NativeProgressPageProxy(INativeProgressPage npp)
		{
			this._npp = npp;
		}

		// Token: 0x060079C8 RID: 31176 RVA: 0x0022859C File Offset: 0x0022679C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void ShowProgressMessage(string message)
		{
			HRESULT hresult = this._npp.ShowProgressMessage(message);
		}

		// Token: 0x17001CBB RID: 7355
		// (get) Token: 0x060079CA RID: 31178 RVA: 0x0003E264 File Offset: 0x0003C464
		// (set) Token: 0x060079C9 RID: 31177 RVA: 0x00002137 File Offset: 0x00000337
		public Uri DeploymentPath
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
			}
		}

		// Token: 0x17001CBC RID: 7356
		// (get) Token: 0x060079CC RID: 31180 RVA: 0x0003E264 File Offset: 0x0003C464
		// (set) Token: 0x060079CB RID: 31179 RVA: 0x00002137 File Offset: 0x00000337
		public DispatcherOperationCallback StopCallback
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
			}
		}

		// Token: 0x17001CBD RID: 7357
		// (get) Token: 0x060079CE RID: 31182 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x060079CD RID: 31181 RVA: 0x00002137 File Offset: 0x00000337
		public DispatcherOperationCallback RefreshCallback
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17001CBE RID: 7358
		// (get) Token: 0x060079D0 RID: 31184 RVA: 0x0003E264 File Offset: 0x0003C464
		// (set) Token: 0x060079CF RID: 31183 RVA: 0x002285B8 File Offset: 0x002267B8
		public string ApplicationName
		{
			get
			{
				throw new NotImplementedException();
			}
			[SecurityCritical]
			[SecurityTreatAsSafe]
			set
			{
				HRESULT hresult = this._npp.SetApplicationName(value);
			}
		}

		// Token: 0x17001CBF RID: 7359
		// (get) Token: 0x060079D2 RID: 31186 RVA: 0x0003E264 File Offset: 0x0003C464
		// (set) Token: 0x060079D1 RID: 31185 RVA: 0x002285D4 File Offset: 0x002267D4
		public string PublisherName
		{
			get
			{
				throw new NotImplementedException();
			}
			[SecurityCritical]
			[SecurityTreatAsSafe]
			set
			{
				HRESULT hresult = this._npp.SetPublisherName(value);
			}
		}

		// Token: 0x060079D3 RID: 31187 RVA: 0x002285F0 File Offset: 0x002267F0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void UpdateProgress(long bytesDownloaded, long bytesTotal)
		{
			HRESULT hresult = this._npp.OnDownloadProgress((ulong)bytesDownloaded, (ulong)bytesTotal);
		}

		// Token: 0x0400399D RID: 14749
		[SecurityCritical]
		private INativeProgressPage _npp;
	}
}
