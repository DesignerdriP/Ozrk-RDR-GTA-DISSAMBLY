using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020006E0 RID: 1760
	internal sealed class TextParagraphResult : ParagraphResult
	{
		// Token: 0x06007173 RID: 29043 RVA: 0x0020724B File Offset: 0x0020544B
		internal TextParagraphResult(TextParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x06007174 RID: 29044 RVA: 0x002072CA File Offset: 0x002054CA
		internal Rect GetRectangleFromTextPosition(ITextPointer position)
		{
			return ((TextParaClient)this._paraClient).GetRectangleFromTextPosition(position);
		}

		// Token: 0x06007175 RID: 29045 RVA: 0x002072DD File Offset: 0x002054DD
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, double paragraphTopSpace, Rect visibleRect)
		{
			return ((TextParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(startPosition, endPosition, paragraphTopSpace, visibleRect);
		}

		// Token: 0x06007176 RID: 29046 RVA: 0x002072F4 File Offset: 0x002054F4
		internal bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			return ((TextParaClient)this._paraClient).IsAtCaretUnitBoundary(position);
		}

		// Token: 0x06007177 RID: 29047 RVA: 0x00207307 File Offset: 0x00205507
		internal ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			return ((TextParaClient)this._paraClient).GetNextCaretUnitPosition(position, direction);
		}

		// Token: 0x06007178 RID: 29048 RVA: 0x0020731B File Offset: 0x0020551B
		internal ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			return ((TextParaClient)this._paraClient).GetBackspaceCaretUnitPosition(position);
		}

		// Token: 0x06007179 RID: 29049 RVA: 0x0020732E File Offset: 0x0020552E
		internal void GetGlyphRuns(List<GlyphRun> glyphRuns, ITextPointer start, ITextPointer end)
		{
			((TextParaClient)this._paraClient).GetGlyphRuns(glyphRuns, start, end);
		}

		// Token: 0x0600717A RID: 29050 RVA: 0x00207344 File Offset: 0x00205544
		internal override bool Contains(ITextPointer position, bool strict)
		{
			bool flag = base.Contains(position, strict);
			if (!flag && strict)
			{
				flag = (position.CompareTo(base.EndPosition) == 0);
			}
			return flag;
		}

		// Token: 0x17001AF1 RID: 6897
		// (get) Token: 0x0600717B RID: 29051 RVA: 0x00207373 File Offset: 0x00205573
		internal ReadOnlyCollection<LineResult> Lines
		{
			get
			{
				if (this._lines == null)
				{
					this._lines = ((TextParaClient)this._paraClient).GetLineResults();
				}
				Invariant.Assert(this._lines != null, "Lines collection is null");
				return this._lines;
			}
		}

		// Token: 0x17001AF2 RID: 6898
		// (get) Token: 0x0600717C RID: 29052 RVA: 0x002073AC File Offset: 0x002055AC
		internal ReadOnlyCollection<ParagraphResult> Floaters
		{
			get
			{
				if (this._floaters == null)
				{
					this._floaters = ((TextParaClient)this._paraClient).GetFloaters();
				}
				return this._floaters;
			}
		}

		// Token: 0x17001AF3 RID: 6899
		// (get) Token: 0x0600717D RID: 29053 RVA: 0x002073D2 File Offset: 0x002055D2
		internal ReadOnlyCollection<ParagraphResult> Figures
		{
			get
			{
				if (this._figures == null)
				{
					this._figures = ((TextParaClient)this._paraClient).GetFigures();
				}
				return this._figures;
			}
		}

		// Token: 0x17001AF4 RID: 6900
		// (get) Token: 0x0600717E RID: 29054 RVA: 0x002073F8 File Offset: 0x002055F8
		internal override bool HasTextContent
		{
			get
			{
				return this.Lines.Count > 0 && !this.ContainsOnlyFloatingElements;
			}
		}

		// Token: 0x17001AF5 RID: 6901
		// (get) Token: 0x0600717F RID: 29055 RVA: 0x00207414 File Offset: 0x00205614
		private bool ContainsOnlyFloatingElements
		{
			get
			{
				bool result = false;
				TextParagraph textParagraph = this._paraClient.Paragraph as TextParagraph;
				Invariant.Assert(textParagraph != null);
				if (textParagraph.HasFiguresOrFloaters())
				{
					if (this.Lines.Count == 0)
					{
						result = true;
					}
					else if (this.Lines.Count == 1)
					{
						int lastDcpAttachedObjectBeforeLine = textParagraph.GetLastDcpAttachedObjectBeforeLine(0);
						if (lastDcpAttachedObjectBeforeLine + textParagraph.ParagraphStartCharacterPosition == textParagraph.ParagraphEndCharacterPosition)
						{
							result = true;
						}
					}
				}
				return result;
			}
		}

		// Token: 0x04003725 RID: 14117
		private ReadOnlyCollection<LineResult> _lines;

		// Token: 0x04003726 RID: 14118
		private ReadOnlyCollection<ParagraphResult> _floaters;

		// Token: 0x04003727 RID: 14119
		private ReadOnlyCollection<ParagraphResult> _figures;
	}
}
