using System;
using System.Windows.Threading;
using MS.Internal.Documents;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200060C RID: 1548
	internal sealed class BackgroundFormatInfo
	{
		// Token: 0x0600671F RID: 26399 RVA: 0x001CD9D1 File Offset: 0x001CBBD1
		internal BackgroundFormatInfo(StructuralCache structuralCache)
		{
			this._structuralCache = structuralCache;
		}

		// Token: 0x06006720 RID: 26400 RVA: 0x001CD9EC File Offset: 0x001CBBEC
		internal void UpdateBackgroundFormatInfo()
		{
			this._cpInterrupted = -1;
			this._lastCPUninterruptible = 0;
			this._doesFinalDTRCoverRestOfText = false;
			this._cchAllText = this._structuralCache.TextContainer.SymbolCount;
			if (this._structuralCache.DtrList != null)
			{
				int num = 0;
				for (int i = 0; i < this._structuralCache.DtrList.Length - 1; i++)
				{
					num += this._structuralCache.DtrList[i].PositionsAdded - this._structuralCache.DtrList[i].PositionsRemoved;
				}
				DirtyTextRange dirtyTextRange = this._structuralCache.DtrList[this._structuralCache.DtrList.Length - 1];
				if (dirtyTextRange.StartIndex + num + dirtyTextRange.PositionsAdded >= this._cchAllText)
				{
					this._doesFinalDTRCoverRestOfText = true;
					this._lastCPUninterruptible = dirtyTextRange.StartIndex + num;
				}
			}
			else
			{
				this._doesFinalDTRCoverRestOfText = true;
			}
			this._backgroundFormatStopTime = DateTime.UtcNow.AddMilliseconds(200.0);
		}

		// Token: 0x06006721 RID: 26401 RVA: 0x001CDB04 File Offset: 0x001CBD04
		internal void ThrottleBackgroundFormatting()
		{
			if (this._throttleBackgroundTimer == null)
			{
				this._throttleBackgroundTimer = new DispatcherTimer(DispatcherPriority.Background);
				this._throttleBackgroundTimer.Interval = new TimeSpan(0, 0, 2);
				this._throttleBackgroundTimer.Tick += this.OnThrottleBackgroundTimeout;
			}
			else
			{
				this._throttleBackgroundTimer.Stop();
			}
			this._throttleBackgroundTimer.Start();
		}

		// Token: 0x06006722 RID: 26402 RVA: 0x001CDB67 File Offset: 0x001CBD67
		internal void BackgroundFormat(IFlowDocumentFormatter formatter, bool ignoreThrottle)
		{
			if (this._throttleBackgroundTimer == null)
			{
				formatter.OnContentInvalidated(true);
				return;
			}
			if (ignoreThrottle)
			{
				this.OnThrottleBackgroundTimeout(null, EventArgs.Empty);
				return;
			}
			this._pendingBackgroundFormatter = formatter;
		}

		// Token: 0x170018E5 RID: 6373
		// (get) Token: 0x06006723 RID: 26403 RVA: 0x001CDB90 File Offset: 0x001CBD90
		internal int LastCPUninterruptible
		{
			get
			{
				return this._lastCPUninterruptible;
			}
		}

		// Token: 0x170018E6 RID: 6374
		// (get) Token: 0x06006724 RID: 26404 RVA: 0x001CDB98 File Offset: 0x001CBD98
		internal DateTime BackgroundFormatStopTime
		{
			get
			{
				return this._backgroundFormatStopTime;
			}
		}

		// Token: 0x170018E7 RID: 6375
		// (get) Token: 0x06006725 RID: 26405 RVA: 0x001CDBA0 File Offset: 0x001CBDA0
		internal int CchAllText
		{
			get
			{
				return this._cchAllText;
			}
		}

		// Token: 0x170018E8 RID: 6376
		// (get) Token: 0x06006726 RID: 26406 RVA: 0x001CDBA8 File Offset: 0x001CBDA8
		internal static bool IsBackgroundFormatEnabled
		{
			get
			{
				return BackgroundFormatInfo._isBackgroundFormatEnabled;
			}
		}

		// Token: 0x170018E9 RID: 6377
		// (get) Token: 0x06006727 RID: 26407 RVA: 0x001CDBAF File Offset: 0x001CBDAF
		internal bool DoesFinalDTRCoverRestOfText
		{
			get
			{
				return this._doesFinalDTRCoverRestOfText;
			}
		}

		// Token: 0x170018EA RID: 6378
		// (get) Token: 0x06006728 RID: 26408 RVA: 0x001CDBB7 File Offset: 0x001CBDB7
		// (set) Token: 0x06006729 RID: 26409 RVA: 0x001CDBBF File Offset: 0x001CBDBF
		internal int CPInterrupted
		{
			get
			{
				return this._cpInterrupted;
			}
			set
			{
				this._cpInterrupted = value;
			}
		}

		// Token: 0x170018EB RID: 6379
		// (get) Token: 0x0600672A RID: 26410 RVA: 0x001CDBC8 File Offset: 0x001CBDC8
		// (set) Token: 0x0600672B RID: 26411 RVA: 0x001CDBD0 File Offset: 0x001CBDD0
		internal double ViewportHeight
		{
			get
			{
				return this._viewportHeight;
			}
			set
			{
				this._viewportHeight = value;
			}
		}

		// Token: 0x0600672C RID: 26412 RVA: 0x001CDBD9 File Offset: 0x001CBDD9
		private void OnThrottleBackgroundTimeout(object sender, EventArgs e)
		{
			this._throttleBackgroundTimer.Stop();
			this._throttleBackgroundTimer = null;
			if (this._pendingBackgroundFormatter != null)
			{
				this.BackgroundFormat(this._pendingBackgroundFormatter, true);
				this._pendingBackgroundFormatter = null;
			}
		}

		// Token: 0x0400334E RID: 13134
		private double _viewportHeight;

		// Token: 0x0400334F RID: 13135
		private bool _doesFinalDTRCoverRestOfText;

		// Token: 0x04003350 RID: 13136
		private int _lastCPUninterruptible;

		// Token: 0x04003351 RID: 13137
		private DateTime _backgroundFormatStopTime;

		// Token: 0x04003352 RID: 13138
		private int _cchAllText;

		// Token: 0x04003353 RID: 13139
		private int _cpInterrupted;

		// Token: 0x04003354 RID: 13140
		private static bool _isBackgroundFormatEnabled = true;

		// Token: 0x04003355 RID: 13141
		private StructuralCache _structuralCache;

		// Token: 0x04003356 RID: 13142
		private DateTime _throttleTimeout = DateTime.UtcNow;

		// Token: 0x04003357 RID: 13143
		private DispatcherTimer _throttleBackgroundTimer;

		// Token: 0x04003358 RID: 13144
		private IFlowDocumentFormatter _pendingBackgroundFormatter;

		// Token: 0x04003359 RID: 13145
		private const uint _throttleBackgroundSeconds = 2U;

		// Token: 0x0400335A RID: 13146
		private const uint _stopTimeDelta = 200U;
	}
}
