using System;

namespace System.Windows.Documents
{
	// Token: 0x02000366 RID: 870
	internal sealed class FixedSOMTableRow : FixedSOMContainer
	{
		// Token: 0x06002E32 RID: 11826 RVA: 0x000CE42C File Offset: 0x000CC62C
		public void AddCell(FixedSOMTableCell cell)
		{
			base.Add(cell);
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06002E33 RID: 11827 RVA: 0x000D0C39 File Offset: 0x000CEE39
		internal override FixedElement.ElementType[] ElementTypes
		{
			get
			{
				return new FixedElement.ElementType[]
				{
					FixedElement.ElementType.TableRow
				};
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06002E34 RID: 11828 RVA: 0x000D0C48 File Offset: 0x000CEE48
		internal bool IsEmpty
		{
			get
			{
				foreach (FixedSOMSemanticBox fixedSOMSemanticBox in base.SemanticBoxes)
				{
					FixedSOMTableCell fixedSOMTableCell = (FixedSOMTableCell)fixedSOMSemanticBox;
					if (!fixedSOMTableCell.IsEmpty)
					{
						return false;
					}
				}
				return true;
			}
		}
	}
}
