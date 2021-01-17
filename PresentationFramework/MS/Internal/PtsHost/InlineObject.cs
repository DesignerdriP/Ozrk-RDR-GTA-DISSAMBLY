using System;
using System.Windows;
using MS.Internal.Documents;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200061A RID: 1562
	internal sealed class InlineObject : EmbeddedObject
	{
		// Token: 0x060067C2 RID: 26562 RVA: 0x001D159B File Offset: 0x001CF79B
		internal InlineObject(int dcp, UIElementIsland uiElementIsland, TextParagraph para) : base(dcp)
		{
			this._para = para;
			this._uiElementIsland = uiElementIsland;
			this._uiElementIsland.DesiredSizeChanged += this._para.OnUIElementDesiredSizeChanged;
		}

		// Token: 0x060067C3 RID: 26563 RVA: 0x001D15CE File Offset: 0x001CF7CE
		internal override void Dispose()
		{
			if (this._uiElementIsland != null)
			{
				this._uiElementIsland.DesiredSizeChanged -= this._para.OnUIElementDesiredSizeChanged;
			}
			base.Dispose();
		}

		// Token: 0x060067C4 RID: 26564 RVA: 0x001D15FC File Offset: 0x001CF7FC
		internal override void Update(EmbeddedObject newObject)
		{
			InlineObject inlineObject = newObject as InlineObject;
			ErrorHandler.Assert(inlineObject != null, ErrorHandler.EmbeddedObjectTypeMismatch);
			ErrorHandler.Assert(inlineObject._uiElementIsland == this._uiElementIsland, ErrorHandler.EmbeddedObjectOwnerMismatch);
			inlineObject._uiElementIsland = null;
		}

		// Token: 0x1700191A RID: 6426
		// (get) Token: 0x060067C5 RID: 26565 RVA: 0x001D163D File Offset: 0x001CF83D
		internal override DependencyObject Element
		{
			get
			{
				return this._uiElementIsland.Root;
			}
		}

		// Token: 0x04003387 RID: 13191
		private UIElementIsland _uiElementIsland;

		// Token: 0x04003388 RID: 13192
		private TextParagraph _para;
	}
}
