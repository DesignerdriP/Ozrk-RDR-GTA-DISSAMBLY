using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using MS.Internal.PtsHost;

namespace MS.Internal.Text
{
	// Token: 0x02000602 RID: 1538
	internal class LineProperties : TextParagraphProperties
	{
		// Token: 0x1700188E RID: 6286
		// (get) Token: 0x06006656 RID: 26198 RVA: 0x001CC09D File Offset: 0x001CA29D
		public override FlowDirection FlowDirection
		{
			get
			{
				return this._flowDirection;
			}
		}

		// Token: 0x1700188F RID: 6287
		// (get) Token: 0x06006657 RID: 26199 RVA: 0x001CC0A5 File Offset: 0x001CA2A5
		public override TextAlignment TextAlignment
		{
			get
			{
				if (!this.IgnoreTextAlignment)
				{
					return this._textAlignment;
				}
				return TextAlignment.Left;
			}
		}

		// Token: 0x17001890 RID: 6288
		// (get) Token: 0x06006658 RID: 26200 RVA: 0x001CC0B7 File Offset: 0x001CA2B7
		public override double LineHeight
		{
			get
			{
				if (this.LineStackingStrategy == LineStackingStrategy.BlockLineHeight && !double.IsNaN(this._lineHeight))
				{
					return this._lineHeight;
				}
				return 0.0;
			}
		}

		// Token: 0x17001891 RID: 6289
		// (get) Token: 0x06006659 RID: 26201 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool FirstLineInParagraph
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001892 RID: 6290
		// (get) Token: 0x0600665A RID: 26202 RVA: 0x001CC0DE File Offset: 0x001CA2DE
		public override TextRunProperties DefaultTextRunProperties
		{
			get
			{
				return this._defaultTextProperties;
			}
		}

		// Token: 0x17001893 RID: 6291
		// (get) Token: 0x0600665B RID: 26203 RVA: 0x001CC0E6 File Offset: 0x001CA2E6
		public override TextDecorationCollection TextDecorations
		{
			get
			{
				return this._defaultTextProperties.TextDecorations;
			}
		}

		// Token: 0x17001894 RID: 6292
		// (get) Token: 0x0600665C RID: 26204 RVA: 0x001CC0F3 File Offset: 0x001CA2F3
		public override TextWrapping TextWrapping
		{
			get
			{
				return this._textWrapping;
			}
		}

		// Token: 0x17001895 RID: 6293
		// (get) Token: 0x0600665D RID: 26205 RVA: 0x001CC0FB File Offset: 0x001CA2FB
		public override TextMarkerProperties TextMarkerProperties
		{
			get
			{
				return this._markerProperties;
			}
		}

		// Token: 0x17001896 RID: 6294
		// (get) Token: 0x0600665E RID: 26206 RVA: 0x0018D432 File Offset: 0x0018B632
		public override double Indent
		{
			get
			{
				return 0.0;
			}
		}

		// Token: 0x0600665F RID: 26207 RVA: 0x001CC103 File Offset: 0x001CA303
		internal LineProperties(DependencyObject element, DependencyObject contentHost, TextProperties defaultTextProperties, MarkerProperties markerProperties) : this(element, contentHost, defaultTextProperties, markerProperties, (TextAlignment)element.GetValue(Block.TextAlignmentProperty))
		{
		}

		// Token: 0x06006660 RID: 26208 RVA: 0x001CC120 File Offset: 0x001CA320
		internal LineProperties(DependencyObject element, DependencyObject contentHost, TextProperties defaultTextProperties, MarkerProperties markerProperties, TextAlignment textAlignment)
		{
			this._defaultTextProperties = defaultTextProperties;
			this._markerProperties = ((markerProperties != null) ? markerProperties.GetTextMarkerProperties(this) : null);
			this._flowDirection = (FlowDirection)element.GetValue(Block.FlowDirectionProperty);
			this._textAlignment = textAlignment;
			this._lineHeight = (double)element.GetValue(Block.LineHeightProperty);
			this._textIndent = (double)element.GetValue(Paragraph.TextIndentProperty);
			this._lineStackingStrategy = (LineStackingStrategy)element.GetValue(Block.LineStackingStrategyProperty);
			this._textWrapping = TextWrapping.Wrap;
			this._textTrimming = TextTrimming.None;
			if (contentHost is TextBlock || contentHost is ITextBoxViewHost)
			{
				this._textWrapping = (TextWrapping)contentHost.GetValue(TextBlock.TextWrappingProperty);
				this._textTrimming = (TextTrimming)contentHost.GetValue(TextBlock.TextTrimmingProperty);
				return;
			}
			if (contentHost is FlowDocument)
			{
				this._textWrapping = ((FlowDocument)contentHost).TextWrapping;
			}
		}

		// Token: 0x06006661 RID: 26209 RVA: 0x001CC214 File Offset: 0x001CA414
		internal double CalcLineAdvanceForTextParagraph(TextParagraph textParagraph, int dcp, double lineAdvance)
		{
			if (!DoubleUtil.IsNaN(this._lineHeight))
			{
				LineStackingStrategy lineStackingStrategy = this.LineStackingStrategy;
				if (lineStackingStrategy != LineStackingStrategy.BlockLineHeight)
				{
					if (lineStackingStrategy != LineStackingStrategy.MaxHeight)
					{
					}
					if (dcp == 0 && textParagraph.HasFiguresOrFloaters() && textParagraph.GetLastDcpAttachedObjectBeforeLine(0) + textParagraph.ParagraphStartCharacterPosition == textParagraph.ParagraphEndCharacterPosition)
					{
						lineAdvance = this._lineHeight;
					}
					else
					{
						lineAdvance = Math.Max(lineAdvance, this._lineHeight);
					}
				}
				else
				{
					lineAdvance = this._lineHeight;
				}
			}
			return lineAdvance;
		}

		// Token: 0x06006662 RID: 26210 RVA: 0x001CC284 File Offset: 0x001CA484
		internal double CalcLineAdvance(double lineAdvance)
		{
			if (!DoubleUtil.IsNaN(this._lineHeight))
			{
				LineStackingStrategy lineStackingStrategy = this.LineStackingStrategy;
				if (lineStackingStrategy != LineStackingStrategy.BlockLineHeight)
				{
					if (lineStackingStrategy != LineStackingStrategy.MaxHeight)
					{
					}
					lineAdvance = Math.Max(lineAdvance, this._lineHeight);
				}
				else
				{
					lineAdvance = this._lineHeight;
				}
			}
			return lineAdvance;
		}

		// Token: 0x17001897 RID: 6295
		// (get) Token: 0x06006663 RID: 26211 RVA: 0x001CC2C7 File Offset: 0x001CA4C7
		internal TextAlignment TextAlignmentInternal
		{
			get
			{
				return this._textAlignment;
			}
		}

		// Token: 0x17001898 RID: 6296
		// (get) Token: 0x06006664 RID: 26212 RVA: 0x001CC2CF File Offset: 0x001CA4CF
		// (set) Token: 0x06006665 RID: 26213 RVA: 0x001CC2D7 File Offset: 0x001CA4D7
		internal bool IgnoreTextAlignment
		{
			get
			{
				return this._ignoreTextAlignment;
			}
			set
			{
				this._ignoreTextAlignment = value;
			}
		}

		// Token: 0x17001899 RID: 6297
		// (get) Token: 0x06006666 RID: 26214 RVA: 0x001CC2E0 File Offset: 0x001CA4E0
		internal LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return this._lineStackingStrategy;
			}
		}

		// Token: 0x1700189A RID: 6298
		// (get) Token: 0x06006667 RID: 26215 RVA: 0x001CC2E8 File Offset: 0x001CA4E8
		internal TextTrimming TextTrimming
		{
			get
			{
				return this._textTrimming;
			}
		}

		// Token: 0x1700189B RID: 6299
		// (get) Token: 0x06006668 RID: 26216 RVA: 0x001CC2F0 File Offset: 0x001CA4F0
		internal bool HasFirstLineProperties
		{
			get
			{
				return this._markerProperties != null || !DoubleUtil.IsZero(this._textIndent);
			}
		}

		// Token: 0x1700189C RID: 6300
		// (get) Token: 0x06006669 RID: 26217 RVA: 0x001CC30A File Offset: 0x001CA50A
		internal TextParagraphProperties FirstLineProps
		{
			get
			{
				if (this._firstLineProperties == null)
				{
					this._firstLineProperties = new LineProperties.FirstLineProperties(this);
				}
				return this._firstLineProperties;
			}
		}

		// Token: 0x0600666A RID: 26218 RVA: 0x001CC326 File Offset: 0x001CA526
		internal TextParagraphProperties GetParaEllipsisLineProps(bool firstLine)
		{
			return new LineProperties.ParaEllipsisLineProperties(firstLine ? this.FirstLineProps : this);
		}

		// Token: 0x04003308 RID: 13064
		private TextRunProperties _defaultTextProperties;

		// Token: 0x04003309 RID: 13065
		private TextMarkerProperties _markerProperties;

		// Token: 0x0400330A RID: 13066
		private LineProperties.FirstLineProperties _firstLineProperties;

		// Token: 0x0400330B RID: 13067
		private bool _ignoreTextAlignment;

		// Token: 0x0400330C RID: 13068
		private FlowDirection _flowDirection;

		// Token: 0x0400330D RID: 13069
		private TextAlignment _textAlignment;

		// Token: 0x0400330E RID: 13070
		private TextWrapping _textWrapping;

		// Token: 0x0400330F RID: 13071
		private TextTrimming _textTrimming;

		// Token: 0x04003310 RID: 13072
		private double _lineHeight;

		// Token: 0x04003311 RID: 13073
		private double _textIndent;

		// Token: 0x04003312 RID: 13074
		private LineStackingStrategy _lineStackingStrategy;

		// Token: 0x02000A16 RID: 2582
		private sealed class FirstLineProperties : TextParagraphProperties
		{
			// Token: 0x17001F39 RID: 7993
			// (get) Token: 0x06008A89 RID: 35465 RVA: 0x002574CE File Offset: 0x002556CE
			public override FlowDirection FlowDirection
			{
				get
				{
					return this._lp.FlowDirection;
				}
			}

			// Token: 0x17001F3A RID: 7994
			// (get) Token: 0x06008A8A RID: 35466 RVA: 0x002574DB File Offset: 0x002556DB
			public override TextAlignment TextAlignment
			{
				get
				{
					return this._lp.TextAlignment;
				}
			}

			// Token: 0x17001F3B RID: 7995
			// (get) Token: 0x06008A8B RID: 35467 RVA: 0x002574E8 File Offset: 0x002556E8
			public override double LineHeight
			{
				get
				{
					return this._lp.LineHeight;
				}
			}

			// Token: 0x17001F3C RID: 7996
			// (get) Token: 0x06008A8C RID: 35468 RVA: 0x00016748 File Offset: 0x00014948
			public override bool FirstLineInParagraph
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17001F3D RID: 7997
			// (get) Token: 0x06008A8D RID: 35469 RVA: 0x002574F5 File Offset: 0x002556F5
			public override TextRunProperties DefaultTextRunProperties
			{
				get
				{
					return this._lp.DefaultTextRunProperties;
				}
			}

			// Token: 0x17001F3E RID: 7998
			// (get) Token: 0x06008A8E RID: 35470 RVA: 0x00257502 File Offset: 0x00255702
			public override TextDecorationCollection TextDecorations
			{
				get
				{
					return this._lp.TextDecorations;
				}
			}

			// Token: 0x17001F3F RID: 7999
			// (get) Token: 0x06008A8F RID: 35471 RVA: 0x0025750F File Offset: 0x0025570F
			public override TextWrapping TextWrapping
			{
				get
				{
					return this._lp.TextWrapping;
				}
			}

			// Token: 0x17001F40 RID: 8000
			// (get) Token: 0x06008A90 RID: 35472 RVA: 0x0025751C File Offset: 0x0025571C
			public override TextMarkerProperties TextMarkerProperties
			{
				get
				{
					return this._lp.TextMarkerProperties;
				}
			}

			// Token: 0x17001F41 RID: 8001
			// (get) Token: 0x06008A91 RID: 35473 RVA: 0x00257529 File Offset: 0x00255729
			public override double Indent
			{
				get
				{
					return this._lp._textIndent;
				}
			}

			// Token: 0x06008A92 RID: 35474 RVA: 0x00257536 File Offset: 0x00255736
			internal FirstLineProperties(LineProperties lp)
			{
				this._lp = lp;
				this.Hyphenator = lp.Hyphenator;
			}

			// Token: 0x040046C9 RID: 18121
			private LineProperties _lp;
		}

		// Token: 0x02000A17 RID: 2583
		private sealed class ParaEllipsisLineProperties : TextParagraphProperties
		{
			// Token: 0x17001F42 RID: 8002
			// (get) Token: 0x06008A93 RID: 35475 RVA: 0x00257551 File Offset: 0x00255751
			public override FlowDirection FlowDirection
			{
				get
				{
					return this._lp.FlowDirection;
				}
			}

			// Token: 0x17001F43 RID: 8003
			// (get) Token: 0x06008A94 RID: 35476 RVA: 0x0025755E File Offset: 0x0025575E
			public override TextAlignment TextAlignment
			{
				get
				{
					return this._lp.TextAlignment;
				}
			}

			// Token: 0x17001F44 RID: 8004
			// (get) Token: 0x06008A95 RID: 35477 RVA: 0x0025756B File Offset: 0x0025576B
			public override double LineHeight
			{
				get
				{
					return this._lp.LineHeight;
				}
			}

			// Token: 0x17001F45 RID: 8005
			// (get) Token: 0x06008A96 RID: 35478 RVA: 0x00257578 File Offset: 0x00255778
			public override bool FirstLineInParagraph
			{
				get
				{
					return this._lp.FirstLineInParagraph;
				}
			}

			// Token: 0x17001F46 RID: 8006
			// (get) Token: 0x06008A97 RID: 35479 RVA: 0x00257585 File Offset: 0x00255785
			public override bool AlwaysCollapsible
			{
				get
				{
					return this._lp.AlwaysCollapsible;
				}
			}

			// Token: 0x17001F47 RID: 8007
			// (get) Token: 0x06008A98 RID: 35480 RVA: 0x00257592 File Offset: 0x00255792
			public override TextRunProperties DefaultTextRunProperties
			{
				get
				{
					return this._lp.DefaultTextRunProperties;
				}
			}

			// Token: 0x17001F48 RID: 8008
			// (get) Token: 0x06008A99 RID: 35481 RVA: 0x0025759F File Offset: 0x0025579F
			public override TextDecorationCollection TextDecorations
			{
				get
				{
					return this._lp.TextDecorations;
				}
			}

			// Token: 0x17001F49 RID: 8009
			// (get) Token: 0x06008A9A RID: 35482 RVA: 0x00016748 File Offset: 0x00014948
			public override TextWrapping TextWrapping
			{
				get
				{
					return TextWrapping.NoWrap;
				}
			}

			// Token: 0x17001F4A RID: 8010
			// (get) Token: 0x06008A9B RID: 35483 RVA: 0x002575AC File Offset: 0x002557AC
			public override TextMarkerProperties TextMarkerProperties
			{
				get
				{
					return this._lp.TextMarkerProperties;
				}
			}

			// Token: 0x17001F4B RID: 8011
			// (get) Token: 0x06008A9C RID: 35484 RVA: 0x002575B9 File Offset: 0x002557B9
			public override double Indent
			{
				get
				{
					return this._lp.Indent;
				}
			}

			// Token: 0x06008A9D RID: 35485 RVA: 0x002575C6 File Offset: 0x002557C6
			internal ParaEllipsisLineProperties(TextParagraphProperties lp)
			{
				this._lp = lp;
			}

			// Token: 0x040046CA RID: 18122
			private TextParagraphProperties _lp;
		}
	}
}
