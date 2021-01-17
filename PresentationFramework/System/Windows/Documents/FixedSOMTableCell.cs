using System;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x02000365 RID: 869
	internal sealed class FixedSOMTableCell : FixedSOMContainer
	{
		// Token: 0x06002E29 RID: 11817 RVA: 0x000D0A7D File Offset: 0x000CEC7D
		public FixedSOMTableCell(double left, double top, double right, double bottom)
		{
			this._boundingRect = new Rect(new Point(left, top), new Point(right, bottom));
			this._containsTable = false;
			this._columnSpan = 1;
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x000D0AAD File Offset: 0x000CECAD
		public void AddContainer(FixedSOMContainer container)
		{
			if (!this._containsTable || !this._AddToInnerTable(container))
			{
				base.Add(container);
			}
			if (container is FixedSOMTable)
			{
				this._containsTable = true;
			}
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x000D0AD8 File Offset: 0x000CECD8
		public override void SetRTFProperties(FixedElement element)
		{
			element.SetValue(Block.BorderThicknessProperty, new Thickness(1.0));
			element.SetValue(Block.BorderBrushProperty, Brushes.Black);
			element.SetValue(TableCell.ColumnSpanProperty, this._columnSpan);
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x000D0B2C File Offset: 0x000CED2C
		private bool _AddToInnerTable(FixedSOMContainer container)
		{
			foreach (FixedSOMSemanticBox fixedSOMSemanticBox in this._semanticBoxes)
			{
				FixedSOMTable fixedSOMTable = fixedSOMSemanticBox as FixedSOMTable;
				if (fixedSOMTable != null && fixedSOMTable.AddContainer(container))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06002E2D RID: 11821 RVA: 0x000D0B94 File Offset: 0x000CED94
		internal override FixedElement.ElementType[] ElementTypes
		{
			get
			{
				return new FixedElement.ElementType[]
				{
					FixedElement.ElementType.TableCell
				};
			}
		}

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x06002E2E RID: 11822 RVA: 0x000D0BA4 File Offset: 0x000CEDA4
		internal bool IsEmpty
		{
			get
			{
				foreach (FixedSOMSemanticBox fixedSOMSemanticBox in base.SemanticBoxes)
				{
					FixedSOMContainer fixedSOMContainer = (FixedSOMContainer)fixedSOMSemanticBox;
					FixedSOMTable fixedSOMTable = fixedSOMContainer as FixedSOMTable;
					if (fixedSOMTable != null && !fixedSOMTable.IsEmpty)
					{
						return false;
					}
					FixedSOMFixedBlock fixedSOMFixedBlock = fixedSOMContainer as FixedSOMFixedBlock;
					if (fixedSOMFixedBlock != null && !fixedSOMFixedBlock.IsWhiteSpace)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x06002E2F RID: 11823 RVA: 0x000D0C28 File Offset: 0x000CEE28
		// (set) Token: 0x06002E30 RID: 11824 RVA: 0x000D0C30 File Offset: 0x000CEE30
		internal int ColumnSpan
		{
			get
			{
				return this._columnSpan;
			}
			set
			{
				this._columnSpan = value;
			}
		}

		// Token: 0x04001DF0 RID: 7664
		private bool _containsTable;

		// Token: 0x04001DF1 RID: 7665
		private int _columnSpan;
	}
}
