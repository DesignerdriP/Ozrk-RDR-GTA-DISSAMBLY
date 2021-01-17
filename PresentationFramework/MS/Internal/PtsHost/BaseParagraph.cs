using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200060E RID: 1550
	internal abstract class BaseParagraph : UnmanagedHandle
	{
		// Token: 0x06006742 RID: 26434 RVA: 0x001CDD66 File Offset: 0x001CBF66
		protected BaseParagraph(DependencyObject element, StructuralCache structuralCache) : base(structuralCache.PtsContext)
		{
			this._element = element;
			this._structuralCache = structuralCache;
			this._changeType = PTS.FSKCHANGE.fskchNone;
			this._stopAsking = false;
			this.UpdateLastFormatPositions();
		}

		// Token: 0x06006743 RID: 26435 RVA: 0x001CDD96 File Offset: 0x001CBF96
		internal virtual void UpdGetParaChange(out PTS.FSKCHANGE fskch, out int fNoFurtherChanges)
		{
			fskch = this._changeType;
			fNoFurtherChanges = PTS.FromBoolean(this._stopAsking);
		}

		// Token: 0x06006744 RID: 26436 RVA: 0x001CDDAD File Offset: 0x001CBFAD
		internal virtual void CollapseMargin(BaseParaClient paraClient, MarginCollapsingState mcs, uint fswdir, bool suppressTopSpace, out int dvr)
		{
			dvr = ((mcs == null || suppressTopSpace) ? 0 : mcs.Margin);
		}

		// Token: 0x06006745 RID: 26437
		internal abstract void GetParaProperties(ref PTS.FSPAP fspap);

		// Token: 0x06006746 RID: 26438
		internal abstract void CreateParaclient(out IntPtr pfsparaclient);

		// Token: 0x06006747 RID: 26439 RVA: 0x001CDDC4 File Offset: 0x001CBFC4
		internal virtual void SetUpdateInfo(PTS.FSKCHANGE fskch, bool stopAsking)
		{
			this._changeType = fskch;
			this._stopAsking = stopAsking;
		}

		// Token: 0x06006748 RID: 26440 RVA: 0x001CDDD4 File Offset: 0x001CBFD4
		internal virtual void ClearUpdateInfo()
		{
			this._changeType = PTS.FSKCHANGE.fskchNone;
			this._stopAsking = true;
		}

		// Token: 0x06006749 RID: 26441 RVA: 0x001CDDE4 File Offset: 0x001CBFE4
		internal virtual bool InvalidateStructure(int startPosition)
		{
			int cpfromElement = TextContainerHelper.GetCPFromElement(this.StructuralCache.TextContainer, this.Element, ElementEdge.BeforeStart);
			return cpfromElement == startPosition;
		}

		// Token: 0x0600674A RID: 26442 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void InvalidateFormatCache()
		{
		}

		// Token: 0x0600674B RID: 26443 RVA: 0x001CDE0D File Offset: 0x001CC00D
		internal void UpdateLastFormatPositions()
		{
			this._lastFormatCch = this.Cch;
		}

		// Token: 0x0600674C RID: 26444 RVA: 0x001CDE1C File Offset: 0x001CC01C
		protected void GetParaProperties(ref PTS.FSPAP fspap, bool ignoreElementProps)
		{
			if (!ignoreElementProps)
			{
				fspap.fKeepWithNext = PTS.FromBoolean(DynamicPropertyReader.GetKeepWithNext(this._element));
				fspap.fBreakPageBefore = ((this._element is Block) ? PTS.FromBoolean(this.StructuralCache.CurrentFormatContext.FinitePage && ((Block)this._element).BreakPageBefore) : PTS.FromBoolean(false));
				fspap.fBreakColumnBefore = ((this._element is Block) ? PTS.FromBoolean(((Block)this._element).BreakColumnBefore) : PTS.FromBoolean(false));
			}
		}

		// Token: 0x170018F4 RID: 6388
		// (get) Token: 0x0600674D RID: 26445 RVA: 0x001CDEBA File Offset: 0x001CC0BA
		internal int ParagraphStartCharacterPosition
		{
			get
			{
				if (this is TextParagraph)
				{
					return TextContainerHelper.GetCPFromElement(this.StructuralCache.TextContainer, this.Element, ElementEdge.AfterStart);
				}
				return TextContainerHelper.GetCPFromElement(this.StructuralCache.TextContainer, this.Element, ElementEdge.BeforeStart);
			}
		}

		// Token: 0x170018F5 RID: 6389
		// (get) Token: 0x0600674E RID: 26446 RVA: 0x001CDEF3 File Offset: 0x001CC0F3
		internal int ParagraphEndCharacterPosition
		{
			get
			{
				if (this is TextParagraph)
				{
					return TextContainerHelper.GetCPFromElement(this.StructuralCache.TextContainer, this.Element, ElementEdge.BeforeEnd);
				}
				return TextContainerHelper.GetCPFromElement(this.StructuralCache.TextContainer, this.Element, ElementEdge.AfterEnd);
			}
		}

		// Token: 0x170018F6 RID: 6390
		// (get) Token: 0x0600674F RID: 26447 RVA: 0x001CDF2C File Offset: 0x001CC12C
		internal int Cch
		{
			get
			{
				int num = TextContainerHelper.GetCchFromElement(this.StructuralCache.TextContainer, this.Element);
				if (this is TextParagraph && this.Element is TextElement)
				{
					Invariant.Assert(num >= 2);
					num -= 2;
				}
				return num;
			}
		}

		// Token: 0x170018F7 RID: 6391
		// (get) Token: 0x06006750 RID: 26448 RVA: 0x001CDF76 File Offset: 0x001CC176
		internal int LastFormatCch
		{
			get
			{
				return this._lastFormatCch;
			}
		}

		// Token: 0x170018F8 RID: 6392
		// (get) Token: 0x06006751 RID: 26449 RVA: 0x001CDF7E File Offset: 0x001CC17E
		internal StructuralCache StructuralCache
		{
			get
			{
				return this._structuralCache;
			}
		}

		// Token: 0x170018F9 RID: 6393
		// (get) Token: 0x06006752 RID: 26450 RVA: 0x001CDF86 File Offset: 0x001CC186
		internal DependencyObject Element
		{
			get
			{
				return this._element;
			}
		}

		// Token: 0x04003363 RID: 13155
		protected PTS.FSKCHANGE _changeType;

		// Token: 0x04003364 RID: 13156
		protected bool _stopAsking;

		// Token: 0x04003365 RID: 13157
		protected int _lastFormatCch;

		// Token: 0x04003366 RID: 13158
		internal BaseParagraph Next;

		// Token: 0x04003367 RID: 13159
		internal BaseParagraph Previous;

		// Token: 0x04003368 RID: 13160
		protected readonly StructuralCache _structuralCache;

		// Token: 0x04003369 RID: 13161
		protected readonly DependencyObject _element;
	}
}
