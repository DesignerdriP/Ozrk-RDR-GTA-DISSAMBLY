using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using MS.Internal.Documents;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000640 RID: 1600
	internal sealed class InlineObjectRun : TextEmbeddedObject
	{
		// Token: 0x06006A2C RID: 27180 RVA: 0x001E3B87 File Offset: 0x001E1D87
		internal InlineObjectRun(int cch, UIElement element, TextRunProperties textProps, TextParagraph host)
		{
			this._cch = cch;
			this._textProps = textProps;
			this._host = host;
			this._inlineUIContainer = (InlineUIContainer)LogicalTreeHelper.GetParent(element);
		}

		// Token: 0x06006A2D RID: 27181 RVA: 0x001E3BB8 File Offset: 0x001E1DB8
		public override TextEmbeddedObjectMetrics Format(double remainingParagraphWidth)
		{
			Size size = this._host.MeasureChild(this);
			TextDpi.EnsureValidObjSize(ref size);
			double baseline = size.Height;
			double num = (double)this.UIElementIsland.Root.GetValue(TextBlock.BaselineOffsetProperty);
			if (!DoubleUtil.IsNaN(num))
			{
				baseline = num;
			}
			return new TextEmbeddedObjectMetrics(size.Width, size.Height, baseline);
		}

		// Token: 0x06006A2E RID: 27182 RVA: 0x001E3C1C File Offset: 0x001E1E1C
		public override Rect ComputeBoundingBox(bool rightToLeft, bool sideways)
		{
			Size desiredSize = this.UIElementIsland.Root.DesiredSize;
			double num = (!sideways) ? desiredSize.Height : desiredSize.Width;
			double num2 = (double)this.UIElementIsland.Root.GetValue(TextBlock.BaselineOffsetProperty);
			if (!sideways && !DoubleUtil.IsNaN(num2))
			{
				num = num2;
			}
			return new Rect(0.0, -num, sideways ? desiredSize.Height : desiredSize.Width, sideways ? desiredSize.Width : desiredSize.Height);
		}

		// Token: 0x06006A2F RID: 27183 RVA: 0x00002137 File Offset: 0x00000337
		public override void Draw(DrawingContext drawingContext, Point origin, bool rightToLeft, bool sideways)
		{
		}

		// Token: 0x17001983 RID: 6531
		// (get) Token: 0x06006A30 RID: 27184 RVA: 0x001CB7C2 File Offset: 0x001C99C2
		public override CharacterBufferReference CharacterBufferReference
		{
			get
			{
				return new CharacterBufferReference(string.Empty, 0);
			}
		}

		// Token: 0x17001984 RID: 6532
		// (get) Token: 0x06006A31 RID: 27185 RVA: 0x001E3CAD File Offset: 0x001E1EAD
		public override int Length
		{
			get
			{
				return this._cch;
			}
		}

		// Token: 0x17001985 RID: 6533
		// (get) Token: 0x06006A32 RID: 27186 RVA: 0x001E3CB5 File Offset: 0x001E1EB5
		public override TextRunProperties Properties
		{
			get
			{
				return this._textProps;
			}
		}

		// Token: 0x17001986 RID: 6534
		// (get) Token: 0x06006A33 RID: 27187 RVA: 0x0000B02A File Offset: 0x0000922A
		public override LineBreakCondition BreakBefore
		{
			get
			{
				return LineBreakCondition.BreakDesired;
			}
		}

		// Token: 0x17001987 RID: 6535
		// (get) Token: 0x06006A34 RID: 27188 RVA: 0x0000B02A File Offset: 0x0000922A
		public override LineBreakCondition BreakAfter
		{
			get
			{
				return LineBreakCondition.BreakDesired;
			}
		}

		// Token: 0x17001988 RID: 6536
		// (get) Token: 0x06006A35 RID: 27189 RVA: 0x00016748 File Offset: 0x00014948
		public override bool HasFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001989 RID: 6537
		// (get) Token: 0x06006A36 RID: 27190 RVA: 0x001E3CBD File Offset: 0x001E1EBD
		internal UIElementIsland UIElementIsland
		{
			get
			{
				return this._inlineUIContainer.UIElementIsland;
			}
		}

		// Token: 0x0400342B RID: 13355
		private readonly int _cch;

		// Token: 0x0400342C RID: 13356
		private readonly TextRunProperties _textProps;

		// Token: 0x0400342D RID: 13357
		private readonly TextParagraph _host;

		// Token: 0x0400342E RID: 13358
		private InlineUIContainer _inlineUIContainer;
	}
}
