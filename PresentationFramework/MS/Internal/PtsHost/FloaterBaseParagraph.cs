using System;
using System.Security;
using System.Windows.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000621 RID: 1569
	internal abstract class FloaterBaseParagraph : BaseParagraph
	{
		// Token: 0x060067F7 RID: 26615 RVA: 0x001CF787 File Offset: 0x001CD987
		protected FloaterBaseParagraph(TextElement element, StructuralCache structuralCache) : base(element, structuralCache)
		{
		}

		// Token: 0x060067F8 RID: 26616 RVA: 0x001D38EA File Offset: 0x001D1AEA
		public override void Dispose()
		{
			base.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x060067F9 RID: 26617 RVA: 0x001D38F8 File Offset: 0x001D1AF8
		internal override void UpdGetParaChange(out PTS.FSKCHANGE fskch, out int fNoFurtherChanges)
		{
			fskch = PTS.FSKCHANGE.fskchNew;
			fNoFurtherChanges = PTS.FromBoolean(this._stopAsking);
		}

		// Token: 0x060067FA RID: 26618 RVA: 0x001D390A File Offset: 0x001D1B0A
		internal override void GetParaProperties(ref PTS.FSPAP fspap)
		{
			base.GetParaProperties(ref fspap, false);
			fspap.idobj = PtsHost.FloaterParagraphId;
		}

		// Token: 0x060067FB RID: 26619
		internal abstract override void CreateParaclient(out IntPtr paraClientHandle);

		// Token: 0x060067FC RID: 26620
		internal abstract override void CollapseMargin(BaseParaClient paraClient, MarginCollapsingState mcs, uint fswdir, bool suppressTopSpace, out int dvr);

		// Token: 0x060067FD RID: 26621
		internal abstract void GetFloaterProperties(uint fswdirTrack, out PTS.FSFLOATERPROPS fsfloaterprops);

		// Token: 0x060067FE RID: 26622 RVA: 0x001D3920 File Offset: 0x001D1B20
		[SecurityCritical]
		internal unsafe virtual void GetFloaterPolygons(FloaterBaseParaClient paraClient, uint fswdirTrack, int ncVertices, int nfspt, int* rgcVertices, out int ccVertices, PTS.FSPOINT* rgfspt, out int cfspt, out int fWrapThrough)
		{
			ccVertices = (cfspt = (fWrapThrough = 0));
		}

		// Token: 0x060067FF RID: 26623
		internal abstract void FormatFloaterContentFinite(FloaterBaseParaClient paraClient, IntPtr pbrkrecIn, int fBRFromPreviousPage, IntPtr footnoteRejector, int fEmptyOk, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out IntPtr pfsFloatContent, out IntPtr pbrkrecOut, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices);

		// Token: 0x06006800 RID: 26624
		internal abstract void FormatFloaterContentBottomless(FloaterBaseParaClient paraClient, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfsFloatContent, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices);

		// Token: 0x06006801 RID: 26625
		internal abstract void UpdateBottomlessFloaterContent(FloaterBaseParaClient paraClient, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, IntPtr pfsFloatContent, out PTS.FSFMTRBL fsfmtrbl, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices);

		// Token: 0x06006802 RID: 26626
		internal abstract void GetMCSClientAfterFloater(uint fswdirTrack, MarginCollapsingState mcs, out IntPtr pmcsclientOut);

		// Token: 0x06006803 RID: 26627 RVA: 0x001D393D File Offset: 0x001D1B3D
		internal virtual void GetDvrUsedForFloater(uint fswdirTrack, MarginCollapsingState mcs, int dvrDisplaced, out int dvrUsed)
		{
			dvrUsed = 0;
		}
	}
}
