using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x020005FF RID: 1535
	internal sealed class InlineObject : TextEmbeddedObject
	{
		// Token: 0x0600661F RID: 26143 RVA: 0x001CB699 File Offset: 0x001C9899
		internal InlineObject(int dcp, int cch, UIElement element, TextRunProperties textProps, TextBlock host)
		{
			this._dcp = dcp;
			this._cch = cch;
			this._element = element;
			this._textProps = textProps;
			this._host = host;
		}

		// Token: 0x06006620 RID: 26144 RVA: 0x001CB6C8 File Offset: 0x001C98C8
		public override TextEmbeddedObjectMetrics Format(double remainingParagraphWidth)
		{
			Size size = this._host.MeasureChild(this);
			TextDpi.EnsureValidObjSize(ref size);
			double baseline = size.Height;
			double num = (double)this.Element.GetValue(TextBlock.BaselineOffsetProperty);
			if (!DoubleUtil.IsNaN(num))
			{
				baseline = num;
			}
			return new TextEmbeddedObjectMetrics(size.Width, size.Height, baseline);
		}

		// Token: 0x06006621 RID: 26145 RVA: 0x001CB728 File Offset: 0x001C9928
		public override Rect ComputeBoundingBox(bool rightToLeft, bool sideways)
		{
			if (this._element.IsArrangeValid)
			{
				Size desiredSize = this._element.DesiredSize;
				double num = (!sideways) ? desiredSize.Height : desiredSize.Width;
				double num2 = (double)this.Element.GetValue(TextBlock.BaselineOffsetProperty);
				if (!sideways && !DoubleUtil.IsNaN(num2))
				{
					num = num2;
				}
				return new Rect(0.0, -num, sideways ? desiredSize.Height : desiredSize.Width, sideways ? desiredSize.Width : desiredSize.Height);
			}
			return Rect.Empty;
		}

		// Token: 0x06006622 RID: 26146 RVA: 0x00002137 File Offset: 0x00000337
		public override void Draw(DrawingContext drawingContext, Point origin, bool rightToLeft, bool sideways)
		{
		}

		// Token: 0x17001874 RID: 6260
		// (get) Token: 0x06006623 RID: 26147 RVA: 0x001CB7C2 File Offset: 0x001C99C2
		public override CharacterBufferReference CharacterBufferReference
		{
			get
			{
				return new CharacterBufferReference(string.Empty, 0);
			}
		}

		// Token: 0x17001875 RID: 6261
		// (get) Token: 0x06006624 RID: 26148 RVA: 0x001CB7CF File Offset: 0x001C99CF
		public override int Length
		{
			get
			{
				return this._cch;
			}
		}

		// Token: 0x17001876 RID: 6262
		// (get) Token: 0x06006625 RID: 26149 RVA: 0x001CB7D7 File Offset: 0x001C99D7
		public override TextRunProperties Properties
		{
			get
			{
				return this._textProps;
			}
		}

		// Token: 0x17001877 RID: 6263
		// (get) Token: 0x06006626 RID: 26150 RVA: 0x0000B02A File Offset: 0x0000922A
		public override LineBreakCondition BreakBefore
		{
			get
			{
				return LineBreakCondition.BreakDesired;
			}
		}

		// Token: 0x17001878 RID: 6264
		// (get) Token: 0x06006627 RID: 26151 RVA: 0x0000B02A File Offset: 0x0000922A
		public override LineBreakCondition BreakAfter
		{
			get
			{
				return LineBreakCondition.BreakDesired;
			}
		}

		// Token: 0x17001879 RID: 6265
		// (get) Token: 0x06006628 RID: 26152 RVA: 0x00016748 File Offset: 0x00014948
		public override bool HasFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700187A RID: 6266
		// (get) Token: 0x06006629 RID: 26153 RVA: 0x001CB7DF File Offset: 0x001C99DF
		internal int Dcp
		{
			get
			{
				return this._dcp;
			}
		}

		// Token: 0x1700187B RID: 6267
		// (get) Token: 0x0600662A RID: 26154 RVA: 0x001CB7E7 File Offset: 0x001C99E7
		internal UIElement Element
		{
			get
			{
				return this._element;
			}
		}

		// Token: 0x040032F2 RID: 13042
		private readonly int _dcp;

		// Token: 0x040032F3 RID: 13043
		private readonly int _cch;

		// Token: 0x040032F4 RID: 13044
		private readonly UIElement _element;

		// Token: 0x040032F5 RID: 13045
		private readonly TextRunProperties _textProps;

		// Token: 0x040032F6 RID: 13046
		private readonly TextBlock _host;
	}
}
