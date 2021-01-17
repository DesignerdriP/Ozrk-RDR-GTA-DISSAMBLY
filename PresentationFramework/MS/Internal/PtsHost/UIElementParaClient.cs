﻿using System;
using System.Collections.Generic;
using System.Security;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000651 RID: 1617
	internal sealed class UIElementParaClient : FloaterBaseParaClient
	{
		// Token: 0x06006B6B RID: 27499 RVA: 0x001F0813 File Offset: 0x001EEA13
		internal UIElementParaClient(FloaterBaseParagraph paragraph) : base(paragraph)
		{
		}

		// Token: 0x06006B6C RID: 27500 RVA: 0x001F081C File Offset: 0x001EEA1C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnArrange()
		{
			base.OnArrange();
			PTS.FSFLOATERDETAILS fsfloaterdetails;
			PTS.Validate(PTS.FsQueryFloaterDetails(base.PtsContext.Context, this._paraHandle.Value, out fsfloaterdetails));
			this._rect = fsfloaterdetails.fsrcFloater;
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Paragraph.Element, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (base.ParentFlowDirection != base.PageFlowDirection)
			{
				mbpInfo.MirrorMargin();
				PTS.FSRECT pageRect = this._pageContext.PageRect;
				PTS.Validate(PTS.FsTransformRectangle(PTS.FlowDirectionToFswdir(base.ParentFlowDirection), ref pageRect, ref this._rect, PTS.FlowDirectionToFswdir(base.PageFlowDirection), out this._rect));
			}
			this._rect.u = this._rect.u + mbpInfo.MarginLeft;
			this._rect.du = this._rect.du - (mbpInfo.MarginLeft + mbpInfo.MarginRight);
			this._rect.du = Math.Max(TextDpi.ToTextDpi(TextDpi.MinWidth), this._rect.du);
			this._rect.dv = Math.Max(TextDpi.ToTextDpi(TextDpi.MinWidth), this._rect.dv);
		}

		// Token: 0x06006B6D RID: 27501 RVA: 0x001F094C File Offset: 0x001EEB4C
		internal override List<Rect> GetRectangles(ContentElement e, int start, int length)
		{
			List<Rect> result = new List<Rect>();
			if (base.Paragraph.Element == e)
			{
				this.GetRectanglesForParagraphElement(out result);
			}
			return result;
		}

		// Token: 0x06006B6E RID: 27502 RVA: 0x001F0978 File Offset: 0x001EEB78
		internal override void ValidateVisual(PTS.FSKUPDATE fskupdInherited)
		{
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Paragraph.Element, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			PtsHelper.UpdateMirroringTransform(base.PageFlowDirection, base.ThisFlowDirection, this._visual, TextDpi.FromTextDpi(2 * this._rect.u + this._rect.du));
			UIElementIsland uielementIsland = ((UIElementParagraph)base.Paragraph).UIElementIsland;
			if (uielementIsland != null)
			{
				if (this._visual.Children.Count != 1 || this._visual.Children[0] != uielementIsland)
				{
					Visual visual = VisualTreeHelper.GetParent(uielementIsland) as Visual;
					if (visual != null)
					{
						ContainerVisual containerVisual = visual as ContainerVisual;
						Invariant.Assert(containerVisual != null, "Parent should always derives from ContainerVisual.");
						containerVisual.Children.Remove(uielementIsland);
					}
					this._visual.Children.Clear();
					this._visual.Children.Add(uielementIsland);
				}
				uielementIsland.Offset = new PTS.FSVECTOR(this._rect.u + mbpInfo.BPLeft, this._rect.v + mbpInfo.BPTop).FromTextDpi();
			}
			else
			{
				this._visual.Children.Clear();
			}
			Brush backgroundBrush = (Brush)base.Paragraph.Element.GetValue(TextElement.BackgroundProperty);
			this._visual.DrawBackgroundAndBorder(backgroundBrush, mbpInfo.BorderBrush, mbpInfo.Border, this._rect.FromTextDpi(), this.IsFirstChunk, this.IsLastChunk);
		}

		// Token: 0x06006B6F RID: 27503 RVA: 0x001F0B08 File Offset: 0x001EED08
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			if (startPosition.CompareTo(((BlockUIContainer)base.Paragraph.Element).ContentEnd) < 0 && endPosition.CompareTo(((BlockUIContainer)base.Paragraph.Element).ContentStart) > 0)
			{
				return new RectangleGeometry(this._rect.FromTextDpi());
			}
			return null;
		}

		// Token: 0x06006B70 RID: 27504 RVA: 0x001F0B63 File Offset: 0x001EED63
		internal override ParagraphResult CreateParagraphResult()
		{
			return new UIElementParagraphResult(this);
		}

		// Token: 0x06006B71 RID: 27505 RVA: 0x001F0B6B File Offset: 0x001EED6B
		internal override IInputElement InputHitTest(PTS.FSPOINT pt)
		{
			if (this._rect.Contains(pt))
			{
				return base.Paragraph.Element as IInputElement;
			}
			return null;
		}

		// Token: 0x06006B72 RID: 27506 RVA: 0x001F0B90 File Offset: 0x001EED90
		internal override TextContentRange GetTextContentRange()
		{
			BlockUIContainer textElement = (BlockUIContainer)base.Paragraph.Element;
			return TextContainerHelper.GetTextContentRangeForTextElement(textElement);
		}
	}
}
