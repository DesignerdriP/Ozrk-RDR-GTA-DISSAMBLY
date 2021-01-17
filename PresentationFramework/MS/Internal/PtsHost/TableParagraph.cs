﻿using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200064B RID: 1611
	internal sealed class TableParagraph : BaseParagraph
	{
		// Token: 0x06006AC6 RID: 27334 RVA: 0x001E8FFB File Offset: 0x001E71FB
		internal TableParagraph(DependencyObject element, StructuralCache structuralCache) : base(element, structuralCache)
		{
			this.Table.TableStructureChanged += this.TableStructureChanged;
		}

		// Token: 0x06006AC7 RID: 27335 RVA: 0x001E901C File Offset: 0x001E721C
		public override void Dispose()
		{
			this.Table.TableStructureChanged -= this.TableStructureChanged;
			BaseParagraph baseParagraph = this._firstChild;
			while (baseParagraph != null)
			{
				BaseParagraph baseParagraph2 = baseParagraph;
				baseParagraph = baseParagraph.Next;
				baseParagraph2.Dispose();
				baseParagraph2.Next = null;
				baseParagraph2.Previous = null;
			}
			this._firstChild = null;
			base.Dispose();
		}

		// Token: 0x06006AC8 RID: 27336 RVA: 0x001E9078 File Offset: 0x001E7278
		internal override void CollapseMargin(BaseParaClient paraClient, MarginCollapsingState mcs, uint fswdir, bool suppressTopSpace, out int dvr)
		{
			if (suppressTopSpace && (base.StructuralCache.CurrentFormatContext.FinitePage || mcs == null))
			{
				dvr = 0;
				return;
			}
			MbpInfo mbp = MbpInfo.FromElement(this.Table, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			MarginCollapsingState marginCollapsingState = null;
			MarginCollapsingState.CollapseTopMargin(base.PtsContext, mbp, mcs, out marginCollapsingState, out dvr);
			if (marginCollapsingState != null)
			{
				dvr = marginCollapsingState.Margin;
				marginCollapsingState.Dispose();
				marginCollapsingState = null;
			}
		}

		// Token: 0x06006AC9 RID: 27337 RVA: 0x001E90E6 File Offset: 0x001E72E6
		internal override void GetParaProperties(ref PTS.FSPAP fspap)
		{
			fspap.idobj = PtsHost.TableParagraphId;
			fspap.fKeepWithNext = 0;
			fspap.fBreakPageBefore = 0;
			fspap.fBreakColumnBefore = 0;
		}

		// Token: 0x06006ACA RID: 27338 RVA: 0x001E9108 File Offset: 0x001E7308
		internal override void CreateParaclient(out IntPtr pfsparaclient)
		{
			TableParaClient tableParaClient = new TableParaClient(this);
			pfsparaclient = tableParaClient.Handle;
		}

		// Token: 0x06006ACB RID: 27339 RVA: 0x001E9124 File Offset: 0x001E7324
		internal void GetTableProperties(uint fswdirTrack, out PTS.FSTABLEOBJPROPS fstableobjprops)
		{
			fstableobjprops = default(PTS.FSTABLEOBJPROPS);
			fstableobjprops.fskclear = PTS.FSKCLEAR.fskclearNone;
			fstableobjprops.ktablealignment = PTS.FSKTABLEOBJALIGNMENT.fsktableobjAlignLeft;
			fstableobjprops.fFloat = 0;
			fstableobjprops.fskwr = PTS.FSKWRAP.fskwrBoth;
			fstableobjprops.fDelayNoProgress = 0;
			fstableobjprops.dvrCaptionTop = 0;
			fstableobjprops.dvrCaptionBottom = 0;
			fstableobjprops.durCaptionLeft = 0;
			fstableobjprops.durCaptionRight = 0;
			fstableobjprops.fswdirTable = PTS.FlowDirectionToFswdir((FlowDirection)base.Element.GetValue(FrameworkElement.FlowDirectionProperty));
		}

		// Token: 0x06006ACC RID: 27340 RVA: 0x001E9198 File Offset: 0x001E7398
		internal void GetMCSClientAfterTable(uint fswdirTrack, IntPtr pmcsclientIn, out IntPtr ppmcsclientOut)
		{
			ppmcsclientOut = IntPtr.Zero;
			MbpInfo mbp = MbpInfo.FromElement(this.Table, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			MarginCollapsingState mcsCurrent = null;
			if (pmcsclientIn != IntPtr.Zero)
			{
				mcsCurrent = (base.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
			}
			MarginCollapsingState marginCollapsingState = null;
			int num;
			MarginCollapsingState.CollapseBottomMargin(base.PtsContext, mbp, mcsCurrent, out marginCollapsingState, out num);
			if (marginCollapsingState != null)
			{
				ppmcsclientOut = marginCollapsingState.Handle;
			}
		}

		// Token: 0x06006ACD RID: 27341 RVA: 0x001E9207 File Offset: 0x001E7407
		internal void GetFirstHeaderRow(int fRepeatedHeader, out int fFound, out IntPtr pnmFirstHeaderRow)
		{
			fFound = 0;
			pnmFirstHeaderRow = IntPtr.Zero;
		}

		// Token: 0x06006ACE RID: 27342 RVA: 0x001E9213 File Offset: 0x001E7413
		internal void GetNextHeaderRow(int fRepeatedHeader, IntPtr nmHeaderRow, out int fFound, out IntPtr pnmNextHeaderRow)
		{
			fFound = 0;
			pnmNextHeaderRow = IntPtr.Zero;
		}

		// Token: 0x06006ACF RID: 27343 RVA: 0x001E9207 File Offset: 0x001E7407
		internal void GetFirstFooterRow(int fRepeatedFooter, out int fFound, out IntPtr pnmFirstFooterRow)
		{
			fFound = 0;
			pnmFirstFooterRow = IntPtr.Zero;
		}

		// Token: 0x06006AD0 RID: 27344 RVA: 0x001E9213 File Offset: 0x001E7413
		internal void GetNextFooterRow(int fRepeatedFooter, IntPtr nmFooterRow, out int fFound, out IntPtr pnmNextFooterRow)
		{
			fFound = 0;
			pnmNextFooterRow = IntPtr.Zero;
		}

		// Token: 0x06006AD1 RID: 27345 RVA: 0x001E9220 File Offset: 0x001E7420
		internal void GetFirstRow(out int fFound, out IntPtr pnmFirstRow)
		{
			if (this._firstChild == null)
			{
				TableRow tableRow = null;
				int num = 0;
				while (num < this.Table.RowGroups.Count && tableRow == null)
				{
					TableRowGroup tableRowGroup = this.Table.RowGroups[num];
					if (tableRowGroup.Rows.Count > 0)
					{
						tableRow = tableRowGroup.Rows[0];
						Invariant.Assert(tableRow.Index != -1);
					}
					num++;
				}
				if (tableRow != null)
				{
					this._firstChild = new RowParagraph(tableRow, base.StructuralCache);
					((RowParagraph)this._firstChild).CalculateRowSpans();
				}
			}
			if (this._firstChild != null)
			{
				fFound = 1;
				pnmFirstRow = this._firstChild.Handle;
				return;
			}
			fFound = 0;
			pnmFirstRow = IntPtr.Zero;
		}

		// Token: 0x06006AD2 RID: 27346 RVA: 0x001E92E0 File Offset: 0x001E74E0
		internal void GetNextRow(IntPtr nmRow, out int fFound, out IntPtr pnmNextRow)
		{
			BaseParagraph baseParagraph = (RowParagraph)base.PtsContext.HandleToObject(nmRow);
			BaseParagraph baseParagraph2 = baseParagraph.Next;
			if (baseParagraph2 == null)
			{
				TableRow row = ((RowParagraph)baseParagraph).Row;
				TableRowGroup rowGroup = row.RowGroup;
				TableRow tableRow = null;
				int num = row.Index + 1;
				int num2 = rowGroup.Index + 1;
				if (num < rowGroup.Rows.Count)
				{
					tableRow = rowGroup.Rows[num];
				}
				while (tableRow == null && num2 != this.Table.RowGroups.Count)
				{
					TableRowCollection rows = this.Table.RowGroups[num2].Rows;
					if (rows.Count > 0)
					{
						tableRow = rows[0];
					}
					num2++;
				}
				if (tableRow != null)
				{
					baseParagraph2 = new RowParagraph(tableRow, base.StructuralCache);
					baseParagraph.Next = baseParagraph2;
					baseParagraph2.Previous = baseParagraph;
					((RowParagraph)baseParagraph2).CalculateRowSpans();
				}
			}
			if (baseParagraph2 != null)
			{
				fFound = 1;
				pnmNextRow = baseParagraph2.Handle;
				return;
			}
			fFound = 0;
			pnmNextRow = IntPtr.Zero;
		}

		// Token: 0x06006AD3 RID: 27347 RVA: 0x001E93E5 File Offset: 0x001E75E5
		internal void UpdFChangeInHeaderFooter(out int fHeaderChanged, out int fFooterChanged, out int fRepeatedHeaderChanged, out int fRepeatedFooterChanged)
		{
			fHeaderChanged = 0;
			fRepeatedHeaderChanged = 0;
			fFooterChanged = 0;
			fRepeatedFooterChanged = 0;
		}

		// Token: 0x06006AD4 RID: 27348 RVA: 0x001E93F4 File Offset: 0x001E75F4
		internal void UpdGetFirstChangeInTable(out int fFound, out int fChangeFirst, out IntPtr pnmRowBeforeChange)
		{
			fFound = 1;
			fChangeFirst = 1;
			pnmRowBeforeChange = IntPtr.Zero;
		}

		// Token: 0x06006AD5 RID: 27349 RVA: 0x001E3769 File Offset: 0x001E1969
		internal void GetDistributionKind(uint fswdirTable, out PTS.FSKTABLEHEIGHTDISTRIBUTION tabledistr)
		{
			tabledistr = PTS.FSKTABLEHEIGHTDISTRIBUTION.fskdistributeUnchanged;
		}

		// Token: 0x06006AD6 RID: 27350 RVA: 0x001E3625 File Offset: 0x001E1825
		internal override void UpdGetParaChange(out PTS.FSKCHANGE fskch, out int fNoFurtherChanges)
		{
			base.UpdGetParaChange(out fskch, out fNoFurtherChanges);
			fskch = PTS.FSKCHANGE.fskchNew;
		}

		// Token: 0x06006AD7 RID: 27351 RVA: 0x001E9404 File Offset: 0x001E7604
		internal override bool InvalidateStructure(int startPosition)
		{
			bool result = true;
			for (RowParagraph rowParagraph = this._firstChild as RowParagraph; rowParagraph != null; rowParagraph = (rowParagraph.Next as RowParagraph))
			{
				if (!this.InvalidateRowStructure(rowParagraph, startPosition))
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06006AD8 RID: 27352 RVA: 0x001E9440 File Offset: 0x001E7640
		internal override void InvalidateFormatCache()
		{
			for (RowParagraph rowParagraph = this._firstChild as RowParagraph; rowParagraph != null; rowParagraph = (rowParagraph.Next as RowParagraph))
			{
				this.InvalidateRowFormatCache(rowParagraph);
			}
		}

		// Token: 0x170019AC RID: 6572
		// (get) Token: 0x06006AD9 RID: 27353 RVA: 0x001E9471 File Offset: 0x001E7671
		internal Table Table
		{
			get
			{
				return (Table)base.Element;
			}
		}

		// Token: 0x06006ADA RID: 27354 RVA: 0x001E9480 File Offset: 0x001E7680
		private bool InvalidateRowStructure(RowParagraph rowParagraph, int startPosition)
		{
			bool result = true;
			for (int i = 0; i < rowParagraph.Cells.Length; i++)
			{
				CellParagraph cellParagraph = rowParagraph.Cells[i];
				if (cellParagraph.ParagraphEndCharacterPosition < startPosition || !cellParagraph.InvalidateStructure(startPosition))
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06006ADB RID: 27355 RVA: 0x001E94C0 File Offset: 0x001E76C0
		private void InvalidateRowFormatCache(RowParagraph rowParagraph)
		{
			for (int i = 0; i < rowParagraph.Cells.Length; i++)
			{
				rowParagraph.Cells[i].InvalidateFormatCache();
			}
		}

		// Token: 0x06006ADC RID: 27356 RVA: 0x001E94F0 File Offset: 0x001E76F0
		private void TableStructureChanged(object sender, EventArgs e)
		{
			for (BaseParagraph baseParagraph = this._firstChild; baseParagraph != null; baseParagraph = baseParagraph.Next)
			{
				baseParagraph.Dispose();
			}
			this._firstChild = null;
			int num = this.Table.SymbolCount - 2;
			if (num > 0)
			{
				DirtyTextRange dtr = new DirtyTextRange(this.Table.ContentStartOffset, num, num, false);
				base.StructuralCache.AddDirtyTextRange(dtr);
			}
			if (base.StructuralCache.FormattingOwner.Formatter != null)
			{
				base.StructuralCache.FormattingOwner.Formatter.OnContentInvalidated(true, this.Table.ContentStart, this.Table.ContentEnd);
			}
		}

		// Token: 0x04003455 RID: 13397
		private BaseParagraph _firstChild;
	}
}
