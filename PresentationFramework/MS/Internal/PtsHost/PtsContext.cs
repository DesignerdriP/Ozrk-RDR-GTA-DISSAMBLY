using System;
using System.Collections;
using System.Security;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000639 RID: 1593
	internal sealed class PtsContext : DispatcherObject, IDisposable
	{
		// Token: 0x06006912 RID: 26898 RVA: 0x001DBA1C File Offset: 0x001D9C1C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal PtsContext(bool isOptimalParagraphEnabled, TextFormattingMode textFormattingMode)
		{
			this._pages = new ArrayList(1);
			this._pageBreakRecords = new ArrayList(1);
			this._unmanagedHandles = new PtsContext.HandleIndex[16];
			this._isOptimalParagraphEnabled = isOptimalParagraphEnabled;
			this.BuildFreeList(1);
			this._ptsHost = PtsCache.AcquireContext(this, textFormattingMode);
		}

		// Token: 0x06006913 RID: 26899 RVA: 0x001DBA70 File Offset: 0x001D9C70
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void Dispose()
		{
			if (Interlocked.CompareExchange(ref this._disposed, 1, 0) == 0)
			{
				try
				{
					this.Enter();
					for (int i = 0; i < this._pageBreakRecords.Count; i++)
					{
						Invariant.Assert((IntPtr)this._pageBreakRecords[i] != IntPtr.Zero, "Invalid break record object");
						PTS.Validate(PTS.FsDestroyPageBreakRecord(this._ptsHost.Context, (IntPtr)this._pageBreakRecords[i]));
					}
				}
				finally
				{
					this.Leave();
					this._pageBreakRecords = null;
				}
				try
				{
					this.Enter();
					for (int i = 0; i < this._pages.Count; i++)
					{
						Invariant.Assert((IntPtr)this._pages[i] != IntPtr.Zero, "Invalid break record object");
						PTS.Validate(PTS.FsDestroyPage(this._ptsHost.Context, (IntPtr)this._pages[i]));
					}
				}
				finally
				{
					this.Leave();
					this._pages = null;
				}
				if (Invariant.Strict && this._unmanagedHandles != null)
				{
					for (int i = 0; i < this._unmanagedHandles.Length; i++)
					{
						object obj = this._unmanagedHandles[i].Obj;
						if (obj != null)
						{
							Invariant.Assert(obj is BaseParagraph || obj is Section || obj is LineBreakRecord, "One of PTS Client objects is not properly disposed.");
						}
					}
				}
				this._ptsHost = null;
				this._unmanagedHandles = null;
				this._callbackException = null;
				this._disposeCompleted = true;
			}
		}

		// Token: 0x06006914 RID: 26900 RVA: 0x001DBC14 File Offset: 0x001D9E14
		internal IntPtr CreateHandle(object obj)
		{
			Invariant.Assert(obj != null, "Cannot create handle for non-existing object.");
			Invariant.Assert(!this.Disposed, "PtsContext is already disposed.");
			if (this._unmanagedHandles[0].Index == 0L)
			{
				this.Resize();
			}
			long index = this._unmanagedHandles[0].Index;
			checked
			{
				this._unmanagedHandles[0].Index = this._unmanagedHandles[(int)((IntPtr)index)].Index;
				this._unmanagedHandles[(int)((IntPtr)index)].Obj = obj;
				this._unmanagedHandles[(int)((IntPtr)index)].Index = 0L;
				return (IntPtr)index;
			}
		}

		// Token: 0x06006915 RID: 26901 RVA: 0x001DBCC0 File Offset: 0x001D9EC0
		internal void ReleaseHandle(IntPtr handle)
		{
			long num = (long)handle;
			Invariant.Assert(!this._disposeCompleted, "PtsContext is already disposed.");
			Invariant.Assert(num > 0L && num < (long)this._unmanagedHandles.Length, "Invalid object handle.");
			checked
			{
				Invariant.Assert(this._unmanagedHandles[(int)((IntPtr)num)].IsHandle(), "Handle has been already released.");
				this._unmanagedHandles[(int)((IntPtr)num)].Obj = null;
				this._unmanagedHandles[(int)((IntPtr)num)].Index = this._unmanagedHandles[0].Index;
				this._unmanagedHandles[0].Index = num;
			}
		}

		// Token: 0x06006916 RID: 26902 RVA: 0x001DBD6C File Offset: 0x001D9F6C
		internal bool IsValidHandle(IntPtr handle)
		{
			long num = (long)handle;
			Invariant.Assert(!this._disposeCompleted, "PtsContext is already disposed.");
			return num >= 0L && num < (long)this._unmanagedHandles.Length && this._unmanagedHandles[(int)(checked((IntPtr)num))].IsHandle();
		}

		// Token: 0x06006917 RID: 26903 RVA: 0x001DBDB8 File Offset: 0x001D9FB8
		internal object HandleToObject(IntPtr handle)
		{
			long num = (long)handle;
			Invariant.Assert(!this._disposeCompleted, "PtsContext is already disposed.");
			Invariant.Assert(num > 0L && num < (long)this._unmanagedHandles.Length, "Invalid object handle.");
			checked
			{
				Invariant.Assert(this._unmanagedHandles[(int)((IntPtr)num)].IsHandle(), "Handle has been already released.");
				return this._unmanagedHandles[(int)((IntPtr)num)].Obj;
			}
		}

		// Token: 0x06006918 RID: 26904 RVA: 0x001DBE2B File Offset: 0x001DA02B
		internal void Enter()
		{
			Invariant.Assert(!this._disposeCompleted, "PtsContext is already disposed.");
			this._ptsHost.EnterContext(this);
		}

		// Token: 0x06006919 RID: 26905 RVA: 0x001DBE4C File Offset: 0x001DA04C
		internal void Leave()
		{
			Invariant.Assert(!this._disposeCompleted, "PtsContext is already disposed.");
			this._ptsHost.LeaveContext(this);
		}

		// Token: 0x0600691A RID: 26906 RVA: 0x001DBE70 File Offset: 0x001DA070
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnPageCreated(SecurityCriticalDataForSet<IntPtr> ptsPage)
		{
			Invariant.Assert(ptsPage.Value != IntPtr.Zero, "Invalid page object.");
			Invariant.Assert(!this.Disposed, "PtsContext is already disposed.");
			Invariant.Assert(!this._pages.Contains(ptsPage.Value), "Page already exists.");
			this._pages.Add(ptsPage.Value);
		}

		// Token: 0x0600691B RID: 26907 RVA: 0x001DBEE8 File Offset: 0x001DA0E8
		internal void OnPageDisposed(SecurityCriticalDataForSet<IntPtr> ptsPage, bool disposing, bool enterContext)
		{
			Invariant.Assert(ptsPage.Value != IntPtr.Zero, "Invalid page object.");
			if (disposing)
			{
				this.OnDestroyPage(ptsPage, enterContext);
				return;
			}
			if (!this.Disposed && !base.Dispatcher.HasShutdownStarted)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.OnDestroyPage), ptsPage);
			}
		}

		// Token: 0x0600691C RID: 26908 RVA: 0x001DBF50 File Offset: 0x001DA150
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnPageBreakRecordCreated(SecurityCriticalDataForSet<IntPtr> br)
		{
			Invariant.Assert(br.Value != IntPtr.Zero, "Invalid break record object.");
			Invariant.Assert(!this.Disposed, "PtsContext is already disposed.");
			Invariant.Assert(!this._pageBreakRecords.Contains(br.Value), "Break record already exists.");
			this._pageBreakRecords.Add(br.Value);
		}

		// Token: 0x0600691D RID: 26909 RVA: 0x001DBFC8 File Offset: 0x001DA1C8
		internal void OnPageBreakRecordDisposed(SecurityCriticalDataForSet<IntPtr> br, bool disposing)
		{
			Invariant.Assert(br.Value != IntPtr.Zero, "Invalid break record object.");
			if (disposing)
			{
				this.OnDestroyBreakRecord(br);
				return;
			}
			if (!this.Disposed && !base.Dispatcher.HasShutdownStarted)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.OnDestroyBreakRecord), br);
			}
		}

		// Token: 0x17001968 RID: 6504
		// (get) Token: 0x0600691E RID: 26910 RVA: 0x001DC035 File Offset: 0x001DA235
		internal bool Disposed
		{
			get
			{
				return this._disposed != 0;
			}
		}

		// Token: 0x17001969 RID: 6505
		// (get) Token: 0x0600691F RID: 26911 RVA: 0x001DC040 File Offset: 0x001DA240
		internal IntPtr Context
		{
			get
			{
				return this._ptsHost.Context;
			}
		}

		// Token: 0x1700196A RID: 6506
		// (get) Token: 0x06006920 RID: 26912 RVA: 0x001DC04D File Offset: 0x001DA24D
		internal bool IsOptimalParagraphEnabled
		{
			get
			{
				return this._isOptimalParagraphEnabled;
			}
		}

		// Token: 0x1700196B RID: 6507
		// (get) Token: 0x06006921 RID: 26913 RVA: 0x001DC055 File Offset: 0x001DA255
		// (set) Token: 0x06006922 RID: 26914 RVA: 0x001DC05D File Offset: 0x001DA25D
		internal TextFormatter TextFormatter
		{
			get
			{
				return this._textFormatter;
			}
			set
			{
				this._textFormatter = value;
			}
		}

		// Token: 0x1700196C RID: 6508
		// (get) Token: 0x06006923 RID: 26915 RVA: 0x001DC066 File Offset: 0x001DA266
		// (set) Token: 0x06006924 RID: 26916 RVA: 0x001DC06E File Offset: 0x001DA26E
		internal Exception CallbackException
		{
			get
			{
				return this._callbackException;
			}
			set
			{
				this._callbackException = value;
			}
		}

		// Token: 0x06006925 RID: 26917 RVA: 0x001DC078 File Offset: 0x001DA278
		private void BuildFreeList(int freeIndex)
		{
			this._unmanagedHandles[0].Index = (long)freeIndex;
			while (freeIndex < this._unmanagedHandles.Length)
			{
				this._unmanagedHandles[freeIndex].Index = (long)(++freeIndex);
			}
			this._unmanagedHandles[freeIndex - 1].Index = 0L;
		}

		// Token: 0x06006926 RID: 26918 RVA: 0x001DC0D4 File Offset: 0x001DA2D4
		private void Resize()
		{
			int freeIndex = this._unmanagedHandles.Length;
			PtsContext.HandleIndex[] array = new PtsContext.HandleIndex[this._unmanagedHandles.Length * 2];
			Array.Copy(this._unmanagedHandles, array, this._unmanagedHandles.Length);
			this._unmanagedHandles = array;
			this.BuildFreeList(freeIndex);
		}

		// Token: 0x06006927 RID: 26919 RVA: 0x001DC11C File Offset: 0x001DA31C
		private object OnDestroyPage(object args)
		{
			SecurityCriticalDataForSet<IntPtr> ptsPage = (SecurityCriticalDataForSet<IntPtr>)args;
			this.OnDestroyPage(ptsPage, true);
			return null;
		}

		// Token: 0x06006928 RID: 26920 RVA: 0x001DC13C File Offset: 0x001DA33C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnDestroyPage(SecurityCriticalDataForSet<IntPtr> ptsPage, bool enterContext)
		{
			Invariant.Assert(ptsPage.Value != IntPtr.Zero, "Invalid page object.");
			if (!this.Disposed)
			{
				Invariant.Assert(this._pages != null, "Collection of pages does not exist.");
				Invariant.Assert(this._pages.Contains(ptsPage.Value), "Page does not exist.");
				try
				{
					if (enterContext)
					{
						this.Enter();
					}
					PTS.Validate(PTS.FsDestroyPage(this._ptsHost.Context, ptsPage.Value));
				}
				finally
				{
					if (enterContext)
					{
						this.Leave();
					}
					this._pages.Remove(ptsPage.Value);
				}
			}
		}

		// Token: 0x06006929 RID: 26921 RVA: 0x001DC1F8 File Offset: 0x001DA3F8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private object OnDestroyBreakRecord(object args)
		{
			SecurityCriticalDataForSet<IntPtr> securityCriticalDataForSet = (SecurityCriticalDataForSet<IntPtr>)args;
			Invariant.Assert(securityCriticalDataForSet.Value != IntPtr.Zero, "Invalid break record object.");
			if (!this.Disposed)
			{
				Invariant.Assert(this._pageBreakRecords != null, "Collection of break records does not exist.");
				Invariant.Assert(this._pageBreakRecords.Contains(securityCriticalDataForSet.Value), "Break record does not exist.");
				try
				{
					this.Enter();
					PTS.Validate(PTS.FsDestroyPageBreakRecord(this._ptsHost.Context, securityCriticalDataForSet.Value));
				}
				finally
				{
					this.Leave();
					this._pageBreakRecords.Remove(securityCriticalDataForSet.Value);
				}
			}
			return null;
		}

		// Token: 0x0400340B RID: 13323
		private PtsContext.HandleIndex[] _unmanagedHandles;

		// Token: 0x0400340C RID: 13324
		[SecurityCritical]
		private ArrayList _pages;

		// Token: 0x0400340D RID: 13325
		[SecurityCritical]
		private ArrayList _pageBreakRecords;

		// Token: 0x0400340E RID: 13326
		private Exception _callbackException;

		// Token: 0x0400340F RID: 13327
		private PtsHost _ptsHost;

		// Token: 0x04003410 RID: 13328
		private bool _isOptimalParagraphEnabled;

		// Token: 0x04003411 RID: 13329
		private TextFormatter _textFormatter;

		// Token: 0x04003412 RID: 13330
		private int _disposed;

		// Token: 0x04003413 RID: 13331
		private bool _disposeCompleted;

		// Token: 0x04003414 RID: 13332
		private const int _defaultHandlesCapacity = 16;

		// Token: 0x02000A20 RID: 2592
		private struct HandleIndex
		{
			// Token: 0x06008AAF RID: 35503 RVA: 0x002576D6 File Offset: 0x002558D6
			internal bool IsHandle()
			{
				return this.Obj != null && this.Index == 0L;
			}

			// Token: 0x0400470B RID: 18187
			internal long Index;

			// Token: 0x0400470C RID: 18188
			internal object Obj;
		}
	}
}
