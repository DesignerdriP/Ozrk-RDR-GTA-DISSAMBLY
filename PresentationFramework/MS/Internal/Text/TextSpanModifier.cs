using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x02000608 RID: 1544
	internal class TextSpanModifier : TextModifier
	{
		// Token: 0x060066A5 RID: 26277 RVA: 0x001CCE58 File Offset: 0x001CB058
		public TextSpanModifier(int length, TextDecorationCollection textDecorations, Brush foregroundBrush)
		{
			this._length = length;
			this._modifierDecorations = textDecorations;
			this._modifierBrush = foregroundBrush;
		}

		// Token: 0x060066A6 RID: 26278 RVA: 0x001CCE75 File Offset: 0x001CB075
		public TextSpanModifier(int length, TextDecorationCollection textDecorations, Brush foregroundBrush, FlowDirection flowDirection) : this(length, textDecorations, foregroundBrush)
		{
			this._hasDirectionalEmbedding = true;
			this._flowDirection = flowDirection;
		}

		// Token: 0x170018B0 RID: 6320
		// (get) Token: 0x060066A7 RID: 26279 RVA: 0x001CCE8F File Offset: 0x001CB08F
		public sealed override int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x170018B1 RID: 6321
		// (get) Token: 0x060066A8 RID: 26280 RVA: 0x0000C238 File Offset: 0x0000A438
		public sealed override TextRunProperties Properties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060066A9 RID: 26281 RVA: 0x001CCE98 File Offset: 0x001CB098
		public sealed override TextRunProperties ModifyProperties(TextRunProperties properties)
		{
			if (properties == null || this._modifierDecorations == null || this._modifierDecorations.Count == 0)
			{
				return properties;
			}
			Brush brush = this._modifierBrush;
			if (brush == properties.ForegroundBrush)
			{
				brush = null;
			}
			TextDecorationCollection textDecorations = properties.TextDecorations;
			TextDecorationCollection textDecorationCollection;
			if (textDecorations == null || textDecorations.Count == 0)
			{
				if (brush == null)
				{
					textDecorationCollection = this._modifierDecorations;
				}
				else
				{
					textDecorationCollection = this.CopyTextDecorations(this._modifierDecorations, brush);
				}
			}
			else
			{
				textDecorationCollection = this.CopyTextDecorations(this._modifierDecorations, brush);
				foreach (TextDecoration value in textDecorations)
				{
					textDecorationCollection.Add(value);
				}
			}
			return new TextSpanModifier.MergedTextRunProperties(properties, textDecorationCollection);
		}

		// Token: 0x170018B2 RID: 6322
		// (get) Token: 0x060066AA RID: 26282 RVA: 0x001CCF58 File Offset: 0x001CB158
		public override bool HasDirectionalEmbedding
		{
			get
			{
				return this._hasDirectionalEmbedding;
			}
		}

		// Token: 0x170018B3 RID: 6323
		// (get) Token: 0x060066AB RID: 26283 RVA: 0x001CCF60 File Offset: 0x001CB160
		public override FlowDirection FlowDirection
		{
			get
			{
				return this._flowDirection;
			}
		}

		// Token: 0x060066AC RID: 26284 RVA: 0x001CCF68 File Offset: 0x001CB168
		private TextDecorationCollection CopyTextDecorations(TextDecorationCollection textDecorations, Brush brush)
		{
			TextDecorationCollection textDecorationCollection = new TextDecorationCollection();
			Pen pen = null;
			foreach (TextDecoration textDecoration in textDecorations)
			{
				if (textDecoration.Pen == null && brush != null)
				{
					if (pen == null)
					{
						pen = new Pen(brush, 1.0);
					}
					TextDecoration textDecoration2 = textDecoration.Clone();
					textDecoration2.Pen = pen;
					textDecorationCollection.Add(textDecoration2);
				}
				else
				{
					textDecorationCollection.Add(textDecoration);
				}
			}
			return textDecorationCollection;
		}

		// Token: 0x04003332 RID: 13106
		private int _length;

		// Token: 0x04003333 RID: 13107
		private TextDecorationCollection _modifierDecorations;

		// Token: 0x04003334 RID: 13108
		private Brush _modifierBrush;

		// Token: 0x04003335 RID: 13109
		private FlowDirection _flowDirection;

		// Token: 0x04003336 RID: 13110
		private bool _hasDirectionalEmbedding;

		// Token: 0x02000A18 RID: 2584
		private class MergedTextRunProperties : TextRunProperties
		{
			// Token: 0x06008A9E RID: 35486 RVA: 0x002575D5 File Offset: 0x002557D5
			internal MergedTextRunProperties(TextRunProperties runProperties, TextDecorationCollection textDecorations)
			{
				this._runProperties = runProperties;
				this._textDecorations = textDecorations;
				base.PixelsPerDip = this._runProperties.PixelsPerDip;
			}

			// Token: 0x17001F4C RID: 8012
			// (get) Token: 0x06008A9F RID: 35487 RVA: 0x002575FC File Offset: 0x002557FC
			public override Typeface Typeface
			{
				get
				{
					return this._runProperties.Typeface;
				}
			}

			// Token: 0x17001F4D RID: 8013
			// (get) Token: 0x06008AA0 RID: 35488 RVA: 0x00257609 File Offset: 0x00255809
			public override double FontRenderingEmSize
			{
				get
				{
					return this._runProperties.FontRenderingEmSize;
				}
			}

			// Token: 0x17001F4E RID: 8014
			// (get) Token: 0x06008AA1 RID: 35489 RVA: 0x00257616 File Offset: 0x00255816
			public override double FontHintingEmSize
			{
				get
				{
					return this._runProperties.FontHintingEmSize;
				}
			}

			// Token: 0x17001F4F RID: 8015
			// (get) Token: 0x06008AA2 RID: 35490 RVA: 0x00257623 File Offset: 0x00255823
			public override TextDecorationCollection TextDecorations
			{
				get
				{
					return this._textDecorations;
				}
			}

			// Token: 0x17001F50 RID: 8016
			// (get) Token: 0x06008AA3 RID: 35491 RVA: 0x0025762B File Offset: 0x0025582B
			public override Brush ForegroundBrush
			{
				get
				{
					return this._runProperties.ForegroundBrush;
				}
			}

			// Token: 0x17001F51 RID: 8017
			// (get) Token: 0x06008AA4 RID: 35492 RVA: 0x00257638 File Offset: 0x00255838
			public override Brush BackgroundBrush
			{
				get
				{
					return this._runProperties.BackgroundBrush;
				}
			}

			// Token: 0x17001F52 RID: 8018
			// (get) Token: 0x06008AA5 RID: 35493 RVA: 0x00257645 File Offset: 0x00255845
			public override CultureInfo CultureInfo
			{
				get
				{
					return this._runProperties.CultureInfo;
				}
			}

			// Token: 0x17001F53 RID: 8019
			// (get) Token: 0x06008AA6 RID: 35494 RVA: 0x00257652 File Offset: 0x00255852
			public override TextEffectCollection TextEffects
			{
				get
				{
					return this._runProperties.TextEffects;
				}
			}

			// Token: 0x17001F54 RID: 8020
			// (get) Token: 0x06008AA7 RID: 35495 RVA: 0x0025765F File Offset: 0x0025585F
			public override BaselineAlignment BaselineAlignment
			{
				get
				{
					return this._runProperties.BaselineAlignment;
				}
			}

			// Token: 0x17001F55 RID: 8021
			// (get) Token: 0x06008AA8 RID: 35496 RVA: 0x0025766C File Offset: 0x0025586C
			public override TextRunTypographyProperties TypographyProperties
			{
				get
				{
					return this._runProperties.TypographyProperties;
				}
			}

			// Token: 0x17001F56 RID: 8022
			// (get) Token: 0x06008AA9 RID: 35497 RVA: 0x00257679 File Offset: 0x00255879
			public override NumberSubstitution NumberSubstitution
			{
				get
				{
					return this._runProperties.NumberSubstitution;
				}
			}

			// Token: 0x040046CB RID: 18123
			private TextRunProperties _runProperties;

			// Token: 0x040046CC RID: 18124
			private TextDecorationCollection _textDecorations;
		}
	}
}
