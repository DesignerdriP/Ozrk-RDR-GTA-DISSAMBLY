using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000378 RID: 888
	internal class Highlights
	{
		// Token: 0x06003003 RID: 12291 RVA: 0x000D7F7B File Offset: 0x000D617B
		internal Highlights(ITextContainer textContainer)
		{
			this._textContainer = textContainer;
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x000D7F8C File Offset: 0x000D618C
		internal virtual object GetHighlightValue(StaticTextPointer textPosition, LogicalDirection direction, Type highlightLayerOwnerType)
		{
			object obj = DependencyProperty.UnsetValue;
			for (int i = 0; i < this.LayerCount; i++)
			{
				HighlightLayer layer = this.GetLayer(i);
				if (layer.OwnerType == highlightLayerOwnerType)
				{
					obj = layer.GetHighlightValue(textPosition, direction);
					if (obj != DependencyProperty.UnsetValue)
					{
						break;
					}
				}
			}
			return obj;
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x000D7FD8 File Offset: 0x000D61D8
		internal virtual bool IsContentHighlighted(StaticTextPointer textPosition, LogicalDirection direction)
		{
			int num = 0;
			while (num < this.LayerCount && !this.GetLayer(num).IsContentHighlighted(textPosition, direction))
			{
				num++;
			}
			return num < this.LayerCount;
		}

		// Token: 0x06003006 RID: 12294 RVA: 0x000D8010 File Offset: 0x000D6210
		internal virtual StaticTextPointer GetNextHighlightChangePosition(StaticTextPointer textPosition, LogicalDirection direction)
		{
			StaticTextPointer staticTextPointer = StaticTextPointer.Null;
			for (int i = 0; i < this.LayerCount; i++)
			{
				StaticTextPointer nextChangePosition = this.GetLayer(i).GetNextChangePosition(textPosition, direction);
				if (!nextChangePosition.IsNull)
				{
					if (staticTextPointer.IsNull)
					{
						staticTextPointer = nextChangePosition;
					}
					else if (direction == LogicalDirection.Forward)
					{
						staticTextPointer = StaticTextPointer.Min(staticTextPointer, nextChangePosition);
					}
					else
					{
						staticTextPointer = StaticTextPointer.Max(staticTextPointer, nextChangePosition);
					}
				}
			}
			return staticTextPointer;
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x000D8070 File Offset: 0x000D6270
		internal virtual StaticTextPointer GetNextPropertyChangePosition(StaticTextPointer textPosition, LogicalDirection direction)
		{
			StaticTextPointer staticTextPointer;
			switch (textPosition.GetPointerContext(direction))
			{
			case TextPointerContext.None:
				return StaticTextPointer.Null;
			case TextPointerContext.Text:
			{
				staticTextPointer = this.GetNextHighlightChangePosition(textPosition, direction);
				StaticTextPointer nextContextPosition = textPosition.GetNextContextPosition(LogicalDirection.Forward);
				if (staticTextPointer.IsNull || nextContextPosition.CompareTo(staticTextPointer) < 0)
				{
					return nextContextPosition;
				}
				return staticTextPointer;
			}
			}
			staticTextPointer = textPosition.CreatePointer(1);
			return staticTextPointer;
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x000D80E0 File Offset: 0x000D62E0
		internal void AddLayer(HighlightLayer highlightLayer)
		{
			if (this._layers == null)
			{
				this._layers = new ArrayList(1);
			}
			Invariant.Assert(!this._layers.Contains(highlightLayer));
			this._layers.Add(highlightLayer);
			highlightLayer.Changed += this.OnLayerChanged;
			this.RaiseChangedEventForLayerContent(highlightLayer);
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x000D813C File Offset: 0x000D633C
		internal void RemoveLayer(HighlightLayer highlightLayer)
		{
			Invariant.Assert(this._layers != null && this._layers.Contains(highlightLayer));
			this.RaiseChangedEventForLayerContent(highlightLayer);
			highlightLayer.Changed -= this.OnLayerChanged;
			this._layers.Remove(highlightLayer);
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x000D818C File Offset: 0x000D638C
		internal HighlightLayer GetLayer(Type highlightLayerType)
		{
			for (int i = 0; i < this.LayerCount; i++)
			{
				if (highlightLayerType == this.GetLayer(i).OwnerType)
				{
					return this.GetLayer(i);
				}
			}
			return null;
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x0600300B RID: 12299 RVA: 0x000D81C7 File Offset: 0x000D63C7
		protected ITextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x0600300C RID: 12300 RVA: 0x000D81D0 File Offset: 0x000D63D0
		// (remove) Token: 0x0600300D RID: 12301 RVA: 0x000D8208 File Offset: 0x000D6408
		internal event HighlightChangedEventHandler Changed;

		// Token: 0x0600300E RID: 12302 RVA: 0x000D823D File Offset: 0x000D643D
		private HighlightLayer GetLayer(int index)
		{
			return (HighlightLayer)this._layers[index];
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x000D8250 File Offset: 0x000D6450
		private void OnLayerChanged(object sender, HighlightChangedEventArgs args)
		{
			if (this.Changed != null)
			{
				this.Changed(this, args);
			}
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x000D8268 File Offset: 0x000D6468
		private void RaiseChangedEventForLayerContent(HighlightLayer highlightLayer)
		{
			if (this.Changed != null)
			{
				List<TextSegment> list = new List<TextSegment>();
				StaticTextPointer staticTextPointer = this._textContainer.CreateStaticPointerAtOffset(0);
				for (;;)
				{
					if (!highlightLayer.IsContentHighlighted(staticTextPointer, LogicalDirection.Forward))
					{
						staticTextPointer = highlightLayer.GetNextChangePosition(staticTextPointer, LogicalDirection.Forward);
						if (staticTextPointer.IsNull)
						{
							break;
						}
					}
					StaticTextPointer staticTextPointer2 = staticTextPointer;
					staticTextPointer = highlightLayer.GetNextChangePosition(staticTextPointer, LogicalDirection.Forward);
					Invariant.Assert(!staticTextPointer.IsNull, "Highlight start not followed by highlight end!");
					list.Add(new TextSegment(staticTextPointer2.CreateDynamicTextPointer(LogicalDirection.Forward), staticTextPointer.CreateDynamicTextPointer(LogicalDirection.Forward)));
				}
				if (list.Count > 0)
				{
					this.Changed(this, new Highlights.LayerHighlightChangedEventArgs(new ReadOnlyCollection<TextSegment>(list), highlightLayer.OwnerType));
				}
			}
		}

		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x06003011 RID: 12305 RVA: 0x000D8311 File Offset: 0x000D6511
		private int LayerCount
		{
			get
			{
				if (this._layers != null)
				{
					return this._layers.Count;
				}
				return 0;
			}
		}

		// Token: 0x04001E63 RID: 7779
		private readonly ITextContainer _textContainer;

		// Token: 0x04001E64 RID: 7780
		private ArrayList _layers;

		// Token: 0x020008D7 RID: 2263
		private class LayerHighlightChangedEventArgs : HighlightChangedEventArgs
		{
			// Token: 0x0600849C RID: 33948 RVA: 0x002491F9 File Offset: 0x002473F9
			internal LayerHighlightChangedEventArgs(ReadOnlyCollection<TextSegment> ranges, Type ownerType)
			{
				this._ranges = ranges;
				this._ownerType = ownerType;
			}

			// Token: 0x17001E05 RID: 7685
			// (get) Token: 0x0600849D RID: 33949 RVA: 0x0024920F File Offset: 0x0024740F
			internal override IList Ranges
			{
				get
				{
					return this._ranges;
				}
			}

			// Token: 0x17001E06 RID: 7686
			// (get) Token: 0x0600849E RID: 33950 RVA: 0x00249217 File Offset: 0x00247417
			internal override Type OwnerType
			{
				get
				{
					return this._ownerType;
				}
			}

			// Token: 0x04004285 RID: 17029
			private readonly ReadOnlyCollection<TextSegment> _ranges;

			// Token: 0x04004286 RID: 17030
			private readonly Type _ownerType;
		}
	}
}
