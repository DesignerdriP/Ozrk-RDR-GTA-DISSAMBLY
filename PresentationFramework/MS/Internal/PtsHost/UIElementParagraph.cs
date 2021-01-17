using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000650 RID: 1616
	internal sealed class UIElementParagraph : FloaterBaseParagraph
	{
		// Token: 0x06006B5A RID: 27482 RVA: 0x001D4B5F File Offset: 0x001D2D5F
		internal UIElementParagraph(TextElement element, StructuralCache structuralCache) : base(element, structuralCache)
		{
		}

		// Token: 0x06006B5B RID: 27483 RVA: 0x001F001B File Offset: 0x001EE21B
		public override void Dispose()
		{
			this.ClearUIElementIsland();
			base.Dispose();
		}

		// Token: 0x06006B5C RID: 27484 RVA: 0x001F0029 File Offset: 0x001EE229
		internal override bool InvalidateStructure(int startPosition)
		{
			if (this._uiElementIsland != null)
			{
				this._uiElementIsland.DesiredSizeChanged -= this.OnUIElementDesiredSizeChanged;
				this._uiElementIsland.Dispose();
				this._uiElementIsland = null;
			}
			return base.InvalidateStructure(startPosition);
		}

		// Token: 0x06006B5D RID: 27485 RVA: 0x001F0064 File Offset: 0x001EE264
		internal override void CreateParaclient(out IntPtr paraClientHandle)
		{
			UIElementParaClient uielementParaClient = new UIElementParaClient(this);
			paraClientHandle = uielementParaClient.Handle;
		}

		// Token: 0x06006B5E RID: 27486 RVA: 0x001F0080 File Offset: 0x001EE280
		internal override void CollapseMargin(BaseParaClient paraClient, MarginCollapsingState mcs, uint fswdir, bool suppressTopSpace, out int dvr)
		{
			MbpInfo mbp = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			MarginCollapsingState marginCollapsingState;
			int num;
			MarginCollapsingState.CollapseTopMargin(base.PtsContext, mbp, mcs, out marginCollapsingState, out num);
			if (suppressTopSpace)
			{
				dvr = 0;
			}
			else
			{
				dvr = num;
				if (marginCollapsingState != null)
				{
					dvr += marginCollapsingState.Margin;
				}
			}
			if (marginCollapsingState != null)
			{
				marginCollapsingState.Dispose();
			}
		}

		// Token: 0x06006B5F RID: 27487 RVA: 0x001F00E4 File Offset: 0x001EE2E4
		internal override void GetFloaterProperties(uint fswdirTrack, out PTS.FSFLOATERPROPS fsfloaterprops)
		{
			fsfloaterprops = default(PTS.FSFLOATERPROPS);
			fsfloaterprops.fFloat = 0;
			fsfloaterprops.fskclear = PTS.WrapDirectionToFskclear((WrapDirection)base.Element.GetValue(Block.ClearFloatersProperty));
			fsfloaterprops.fskfloatalignment = PTS.FSKFLOATALIGNMENT.fskfloatalignMin;
			fsfloaterprops.fskwr = PTS.FSKWRAP.fskwrNone;
			fsfloaterprops.fDelayNoProgress = 1;
		}

		// Token: 0x06006B60 RID: 27488 RVA: 0x001F0134 File Offset: 0x001EE334
		internal override void FormatFloaterContentFinite(FloaterBaseParaClient paraClient, IntPtr pbrkrecIn, int fBRFromPreviousPage, IntPtr footnoteRejector, int fEmptyOk, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out IntPtr pfsFloatContent, out IntPtr pbrkrecOut, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			Invariant.Assert(paraClient is UIElementParaClient);
			Invariant.Assert(base.Element is BlockUIContainer);
			if (fAtMaxWidth == 0 && fEmptyOk == 1)
			{
				durFloaterWidth = (dvrFloaterHeight = 0);
				cPolygons = (cVertices = 0);
				fsfmtr = default(PTS.FSFMTR);
				fsfmtr.kstop = PTS.FSFMTRKSTOP.fmtrNoProgressOutOfSpace;
				fsfmtr.fContainsItemThatStoppedBeforeFootnote = 0;
				fsfmtr.fForcedProgress = 0;
				fsbbox = default(PTS.FSBBOX);
				fsbbox.fDefined = 0;
				pbrkrecOut = IntPtr.Zero;
				pfsFloatContent = IntPtr.Zero;
				return;
			}
			cPolygons = (cVertices = 0);
			fsfmtr.fForcedProgress = PTS.FromBoolean(fAtMaxWidth == 0);
			if (((BlockUIContainer)base.Element).Child != null)
			{
				this.EnsureUIElementIsland();
				this.FormatUIElement(durAvailable, out fsbbox);
			}
			else
			{
				this.ClearUIElementIsland();
				MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
				fsbbox.fsrc = default(PTS.FSRECT);
				fsbbox.fsrc.du = durAvailable;
				fsbbox.fsrc.dv = mbpInfo.BPTop + mbpInfo.BPBottom;
			}
			durFloaterWidth = fsbbox.fsrc.du;
			dvrFloaterHeight = fsbbox.fsrc.dv;
			if (dvrAvailable < dvrFloaterHeight && fEmptyOk == 1)
			{
				durFloaterWidth = (dvrFloaterHeight = 0);
				fsfmtr = default(PTS.FSFMTR);
				fsfmtr.kstop = PTS.FSFMTRKSTOP.fmtrNoProgressOutOfSpace;
				fsbbox = default(PTS.FSBBOX);
				fsbbox.fDefined = 0;
				pfsFloatContent = IntPtr.Zero;
			}
			else
			{
				fsbbox.fDefined = 1;
				pfsFloatContent = paraClient.Handle;
				if (dvrAvailable < dvrFloaterHeight)
				{
					Invariant.Assert(fEmptyOk == 0);
					fsfmtr.fForcedProgress = 1;
				}
				fsfmtr.kstop = PTS.FSFMTRKSTOP.fmtrGoalReached;
			}
			pbrkrecOut = IntPtr.Zero;
			fsfmtr.fContainsItemThatStoppedBeforeFootnote = 0;
		}

		// Token: 0x06006B61 RID: 27489 RVA: 0x001F02F8 File Offset: 0x001EE4F8
		internal override void FormatFloaterContentBottomless(FloaterBaseParaClient paraClient, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfsFloatContent, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			Invariant.Assert(paraClient is UIElementParaClient);
			Invariant.Assert(base.Element is BlockUIContainer);
			if (fAtMaxWidth == 0)
			{
				durFloaterWidth = durAvailable + 1;
				dvrFloaterHeight = dvrAvailable + 1;
				cPolygons = (cVertices = 0);
				fsfmtrbl = PTS.FSFMTRBL.fmtrblInterrupted;
				fsbbox = default(PTS.FSBBOX);
				fsbbox.fDefined = 0;
				pfsFloatContent = IntPtr.Zero;
				return;
			}
			cPolygons = (cVertices = 0);
			if (((BlockUIContainer)base.Element).Child != null)
			{
				this.EnsureUIElementIsland();
				this.FormatUIElement(durAvailable, out fsbbox);
				pfsFloatContent = paraClient.Handle;
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				fsbbox.fDefined = 1;
				durFloaterWidth = fsbbox.fsrc.du;
				dvrFloaterHeight = fsbbox.fsrc.dv;
				return;
			}
			this.ClearUIElementIsland();
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			fsbbox.fsrc = default(PTS.FSRECT);
			fsbbox.fsrc.du = durAvailable;
			fsbbox.fsrc.dv = mbpInfo.BPTop + mbpInfo.BPBottom;
			fsbbox.fDefined = 1;
			pfsFloatContent = paraClient.Handle;
			fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
			durFloaterWidth = fsbbox.fsrc.du;
			dvrFloaterHeight = fsbbox.fsrc.dv;
		}

		// Token: 0x06006B62 RID: 27490 RVA: 0x001F0448 File Offset: 0x001EE648
		internal override void UpdateBottomlessFloaterContent(FloaterBaseParaClient paraClient, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, IntPtr pfsFloatContent, out PTS.FSFMTRBL fsfmtrbl, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			this.FormatFloaterContentBottomless(paraClient, fSuppressTopSpace, fswdir, fAtMaxWidth, durAvailable, dvrAvailable, out fsfmtrbl, out pfsFloatContent, out durFloaterWidth, out dvrFloaterHeight, out fsbbox, out cPolygons, out cVertices);
		}

		// Token: 0x06006B63 RID: 27491 RVA: 0x001F0474 File Offset: 0x001EE674
		internal override void GetMCSClientAfterFloater(uint fswdirTrack, MarginCollapsingState mcs, out IntPtr pmcsclientOut)
		{
			MbpInfo mbp = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			MarginCollapsingState marginCollapsingState;
			int num;
			MarginCollapsingState.CollapseBottomMargin(base.PtsContext, mbp, null, out marginCollapsingState, out num);
			if (marginCollapsingState != null)
			{
				pmcsclientOut = marginCollapsingState.Handle;
				return;
			}
			pmcsclientOut = IntPtr.Zero;
		}

		// Token: 0x06006B64 RID: 27492 RVA: 0x001F04C4 File Offset: 0x001EE6C4
		private void FormatUIElement(int durAvailable, out PTS.FSBBOX fsbbox)
		{
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			double width = TextDpi.FromTextDpi(Math.Max(1, durAvailable - (mbpInfo.MBPLeft + mbpInfo.MBPRight)));
			double num;
			if (this.SizeToFigureParent)
			{
				if (base.StructuralCache.CurrentFormatContext.FinitePage)
				{
					num = base.StructuralCache.CurrentFormatContext.PageHeight;
				}
				else
				{
					Figure figure = (Figure)((BlockUIContainer)base.Element).Parent;
					Invariant.Assert(figure.Height.IsAbsolute);
					num = figure.Height.Value;
				}
				num = Math.Max(TextDpi.FromTextDpi(1), num - TextDpi.FromTextDpi(mbpInfo.MBPTop + mbpInfo.MBPBottom));
				this.UIElementIsland.DoLayout(new Size(width, num), false, false);
				fsbbox.fsrc = default(PTS.FSRECT);
				fsbbox.fsrc.du = durAvailable;
				fsbbox.fsrc.dv = TextDpi.ToTextDpi(num) + mbpInfo.BPTop + mbpInfo.BPBottom;
				fsbbox.fDefined = 1;
				return;
			}
			if (base.StructuralCache.CurrentFormatContext.FinitePage)
			{
				Thickness documentPageMargin = base.StructuralCache.CurrentFormatContext.DocumentPageMargin;
				num = base.StructuralCache.CurrentFormatContext.DocumentPageSize.Height - documentPageMargin.Top - documentPageMargin.Bottom - TextDpi.FromTextDpi(mbpInfo.MBPTop + mbpInfo.MBPBottom);
				num = Math.Max(TextDpi.FromTextDpi(1), num);
			}
			else
			{
				num = double.PositiveInfinity;
			}
			Size size = this.UIElementIsland.DoLayout(new Size(width, num), false, true);
			fsbbox.fsrc = default(PTS.FSRECT);
			fsbbox.fsrc.du = durAvailable;
			fsbbox.fsrc.dv = TextDpi.ToTextDpi(size.Height) + mbpInfo.BPTop + mbpInfo.BPBottom;
			fsbbox.fDefined = 1;
		}

		// Token: 0x06006B65 RID: 27493 RVA: 0x001F06BA File Offset: 0x001EE8BA
		private void EnsureUIElementIsland()
		{
			if (this._uiElementIsland == null)
			{
				this._uiElementIsland = new UIElementIsland(((BlockUIContainer)base.Element).Child);
				this._uiElementIsland.DesiredSizeChanged += this.OnUIElementDesiredSizeChanged;
			}
		}

		// Token: 0x06006B66 RID: 27494 RVA: 0x001F06F8 File Offset: 0x001EE8F8
		private void ClearUIElementIsland()
		{
			try
			{
				if (this._uiElementIsland != null)
				{
					this._uiElementIsland.DesiredSizeChanged -= this.OnUIElementDesiredSizeChanged;
					this._uiElementIsland.Dispose();
				}
			}
			finally
			{
				this._uiElementIsland = null;
			}
		}

		// Token: 0x06006B67 RID: 27495 RVA: 0x001EFA2A File Offset: 0x001EDC2A
		private void OnUIElementDesiredSizeChanged(object sender, DesiredSizeChangedEventArgs e)
		{
			base.StructuralCache.FormattingOwner.OnChildDesiredSizeChanged(e.Child);
		}

		// Token: 0x170019BC RID: 6588
		// (get) Token: 0x06006B68 RID: 27496 RVA: 0x001F074C File Offset: 0x001EE94C
		internal UIElementIsland UIElementIsland
		{
			get
			{
				return this._uiElementIsland;
			}
		}

		// Token: 0x170019BD RID: 6589
		// (get) Token: 0x06006B69 RID: 27497 RVA: 0x001F0754 File Offset: 0x001EE954
		private bool SizeToFigureParent
		{
			get
			{
				if (!this.IsOnlyChildOfFigure)
				{
					return false;
				}
				Figure figure = (Figure)((BlockUIContainer)base.Element).Parent;
				return !figure.Height.IsAuto && (base.StructuralCache.CurrentFormatContext.FinitePage || figure.Height.IsAbsolute);
			}
		}

		// Token: 0x170019BE RID: 6590
		// (get) Token: 0x06006B6A RID: 27498 RVA: 0x001F07B8 File Offset: 0x001EE9B8
		private bool IsOnlyChildOfFigure
		{
			get
			{
				DependencyObject parent = ((BlockUIContainer)base.Element).Parent;
				if (parent is Figure)
				{
					Figure figure = parent as Figure;
					if (figure.Blocks.FirstChild == figure.Blocks.LastChild && figure.Blocks.FirstChild == base.Element)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x04003467 RID: 13415
		private UIElementIsland _uiElementIsland;
	}
}
