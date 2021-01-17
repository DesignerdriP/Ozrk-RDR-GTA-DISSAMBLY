using System;
using System.Security;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000610 RID: 1552
	internal sealed class CellParagraph : SubpageParagraph
	{
		// Token: 0x06006762 RID: 26466 RVA: 0x001CE704 File Offset: 0x001CC904
		internal CellParagraph(DependencyObject element, StructuralCache structuralCache) : base(element, structuralCache)
		{
			this._isInterruptible = false;
		}

		// Token: 0x170018FC RID: 6396
		// (get) Token: 0x06006763 RID: 26467 RVA: 0x001CE715 File Offset: 0x001CC915
		internal TableCell Cell
		{
			get
			{
				return (TableCell)base.Element;
			}
		}

		// Token: 0x06006764 RID: 26468 RVA: 0x001CE724 File Offset: 0x001CC924
		[SecurityCritical]
		internal void FormatCellFinite(TableParaClient tableParaClient, IntPtr pfsbrkcellIn, IntPtr pfsFtnRejector, int fEmptyOK, uint fswdirTable, int dvrAvailable, out PTS.FSFMTR pfmtr, out IntPtr ppfscell, out IntPtr pfsbrkcellOut, out int dvrUsed)
		{
			CellParaClient cellParaClient = new CellParaClient(this, tableParaClient);
			Size subpageSize = new Size(cellParaClient.CalculateCellWidth(tableParaClient), Math.Max(TextDpi.FromTextDpi(dvrAvailable), 0.0));
			cellParaClient.FormatCellFinite(subpageSize, pfsbrkcellIn, PTS.ToBoolean(fEmptyOK), fswdirTable, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA.fsksuppresshardbreakbeforefirstparaNone, out pfmtr, out dvrUsed, out pfsbrkcellOut);
			ppfscell = cellParaClient.Handle;
			if (pfmtr.kstop == PTS.FSFMTRKSTOP.fmtrNoProgressOutOfSpace)
			{
				cellParaClient.Dispose();
				ppfscell = IntPtr.Zero;
				dvrUsed = 0;
			}
			if (dvrAvailable < dvrUsed)
			{
				if (PTS.ToBoolean(fEmptyOK))
				{
					if (cellParaClient != null)
					{
						cellParaClient.Dispose();
					}
					if (pfsbrkcellOut != IntPtr.Zero)
					{
						PTS.Validate(PTS.FsDestroySubpageBreakRecord(cellParaClient.PtsContext.Context, pfsbrkcellOut), cellParaClient.PtsContext);
						pfsbrkcellOut = IntPtr.Zero;
					}
					ppfscell = IntPtr.Zero;
					pfmtr.kstop = PTS.FSFMTRKSTOP.fmtrNoProgressOutOfSpace;
					dvrUsed = 0;
					return;
				}
				pfmtr.fForcedProgress = 1;
			}
		}

		// Token: 0x06006765 RID: 26469 RVA: 0x001CE804 File Offset: 0x001CCA04
		internal void FormatCellBottomless(TableParaClient tableParaClient, uint fswdirTable, out PTS.FSFMTRBL fmtrbl, out IntPtr ppfscell, out int dvrUsed)
		{
			CellParaClient cellParaClient = new CellParaClient(this, tableParaClient);
			cellParaClient.FormatCellBottomless(fswdirTable, cellParaClient.CalculateCellWidth(tableParaClient), out fmtrbl, out dvrUsed);
			ppfscell = cellParaClient.Handle;
		}

		// Token: 0x06006766 RID: 26470 RVA: 0x001CE833 File Offset: 0x001CCA33
		internal void UpdateBottomlessCell(CellParaClient cellParaClient, TableParaClient tableParaClient, uint fswdirTable, out PTS.FSFMTRBL fmtrbl, out int dvrUsed)
		{
			cellParaClient.UpdateBottomlessCell(fswdirTable, cellParaClient.CalculateCellWidth(tableParaClient), out fmtrbl, out dvrUsed);
		}

		// Token: 0x06006767 RID: 26471 RVA: 0x001CE847 File Offset: 0x001CCA47
		internal void SetCellHeight(CellParaClient cellParaClient, TableParaClient tableParaClient, IntPtr subpageBreakRecord, int fBrokenHere, uint fswdirTable, int dvrActual)
		{
			cellParaClient.ArrangeHeight = TextDpi.FromTextDpi(dvrActual);
		}

		// Token: 0x06006768 RID: 26472 RVA: 0x001CE856 File Offset: 0x001CCA56
		internal void UpdGetCellChange(out int fWidthChanged, out PTS.FSKCHANGE fskchCell)
		{
			fWidthChanged = 1;
			fskchCell = PTS.FSKCHANGE.fskchNew;
		}
	}
}
