using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020003DB RID: 987
	internal class SpellerHighlightLayer : HighlightLayer
	{
		// Token: 0x06003576 RID: 13686 RVA: 0x000F2CF4 File Offset: 0x000F0EF4
		internal SpellerHighlightLayer(Speller speller)
		{
			this._speller = speller;
		}

		// Token: 0x06003577 RID: 13687 RVA: 0x000F2D04 File Offset: 0x000F0F04
		internal override object GetHighlightValue(StaticTextPointer textPosition, LogicalDirection direction)
		{
			object result;
			if (this.IsContentHighlighted(textPosition, direction))
			{
				result = SpellerHighlightLayer._errorTextDecorations;
			}
			else
			{
				result = DependencyProperty.UnsetValue;
			}
			return result;
		}

		// Token: 0x06003578 RID: 13688 RVA: 0x000F2D2A File Offset: 0x000F0F2A
		internal override bool IsContentHighlighted(StaticTextPointer textPosition, LogicalDirection direction)
		{
			return this._speller.StatusTable.IsRunType(textPosition, direction, SpellerStatusTable.RunType.Error);
		}

		// Token: 0x06003579 RID: 13689 RVA: 0x000F2D3F File Offset: 0x000F0F3F
		internal override StaticTextPointer GetNextChangePosition(StaticTextPointer textPosition, LogicalDirection direction)
		{
			return this._speller.StatusTable.GetNextErrorTransition(textPosition, direction);
		}

		// Token: 0x0600357A RID: 13690 RVA: 0x000F2D53 File Offset: 0x000F0F53
		internal void FireChangedEvent(ITextPointer start, ITextPointer end)
		{
			if (this.Changed != null)
			{
				this.Changed(this, new SpellerHighlightLayer.SpellerHighlightChangedEventArgs(start, end));
			}
		}

		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x0600357B RID: 13691 RVA: 0x000F2D70 File Offset: 0x000F0F70
		internal override Type OwnerType
		{
			get
			{
				return typeof(SpellerHighlightLayer);
			}
		}

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x0600357C RID: 13692 RVA: 0x000F2D7C File Offset: 0x000F0F7C
		// (remove) Token: 0x0600357D RID: 13693 RVA: 0x000F2DB4 File Offset: 0x000F0FB4
		internal override event HighlightChangedEventHandler Changed;

		// Token: 0x0600357E RID: 13694 RVA: 0x000F2DEC File Offset: 0x000F0FEC
		private static TextDecorationCollection GetErrorTextDecorations()
		{
			DrawingGroup drawingGroup = new DrawingGroup();
			DrawingContext drawingContext = drawingGroup.Open();
			Pen pen = new Pen(Brushes.Red, 0.33);
			drawingContext.DrawLine(pen, new Point(0.0, 0.0), new Point(0.5, 1.0));
			drawingContext.DrawLine(pen, new Point(0.5, 1.0), new Point(1.0, 0.0));
			drawingContext.Close();
			TextDecoration value = new TextDecoration(TextDecorationLocation.Underline, new Pen(new DrawingBrush(drawingGroup)
			{
				TileMode = TileMode.Tile,
				Viewport = new Rect(0.0, 0.0, 3.0, 3.0),
				ViewportUnits = BrushMappingMode.Absolute
			}, 3.0), 0.0, TextDecorationUnit.FontRecommended, TextDecorationUnit.Pixel);
			TextDecorationCollection textDecorationCollection = new TextDecorationCollection();
			textDecorationCollection.Add(value);
			textDecorationCollection.Freeze();
			return textDecorationCollection;
		}

		// Token: 0x0400251B RID: 9499
		private readonly Speller _speller;

		// Token: 0x0400251C RID: 9500
		private static readonly TextDecorationCollection _errorTextDecorations = SpellerHighlightLayer.GetErrorTextDecorations();

		// Token: 0x020008E3 RID: 2275
		private class SpellerHighlightChangedEventArgs : HighlightChangedEventArgs
		{
			// Token: 0x060084BE RID: 33982 RVA: 0x00249888 File Offset: 0x00247A88
			internal SpellerHighlightChangedEventArgs(ITextPointer start, ITextPointer end)
			{
				Invariant.Assert(start.CompareTo(end) < 0, "Bogus start/end combination!");
				this._ranges = new ReadOnlyCollection<TextSegment>(new List<TextSegment>(1)
				{
					new TextSegment(start, end)
				});
			}

			// Token: 0x17001E15 RID: 7701
			// (get) Token: 0x060084BF RID: 33983 RVA: 0x002498CF File Offset: 0x00247ACF
			internal override IList Ranges
			{
				get
				{
					return this._ranges;
				}
			}

			// Token: 0x17001E16 RID: 7702
			// (get) Token: 0x060084C0 RID: 33984 RVA: 0x000F2D70 File Offset: 0x000F0F70
			internal override Type OwnerType
			{
				get
				{
					return typeof(SpellerHighlightLayer);
				}
			}

			// Token: 0x040042AB RID: 17067
			private readonly ReadOnlyCollection<TextSegment> _ranges;
		}
	}
}
