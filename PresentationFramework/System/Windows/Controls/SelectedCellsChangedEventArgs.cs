using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.SelectedCellsChanged" /> event. </summary>
	// Token: 0x02000529 RID: 1321
	public class SelectedCellsChangedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.SelectedCellsChangedEventArgs" /> class with the specified cells added to and removed from the selection. </summary>
		/// <param name="addedCells">The cells added to the selection.</param>
		/// <param name="removedCells">The cells removed from the selection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="addedCells" /> or <paramref name="removedCells" /> is <see langword="null" />.</exception>
		// Token: 0x060055B9 RID: 21945 RVA: 0x0017C113 File Offset: 0x0017A313
		public SelectedCellsChangedEventArgs(List<DataGridCellInfo> addedCells, List<DataGridCellInfo> removedCells)
		{
			if (addedCells == null)
			{
				throw new ArgumentNullException("addedCells");
			}
			if (removedCells == null)
			{
				throw new ArgumentNullException("removedCells");
			}
			this._addedCells = addedCells.AsReadOnly();
			this._removedCells = removedCells.AsReadOnly();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.SelectedCellsChangedEventArgs" /> class with the specified cells added to and removed from the selection.</summary>
		/// <param name="addedCells">The cells added to the selection.</param>
		/// <param name="removedCells">The cells removed from the selection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="addedCells" /> or <paramref name="removedCells" /> is <see langword="null" />.</exception>
		// Token: 0x060055BA RID: 21946 RVA: 0x0017C14F File Offset: 0x0017A34F
		public SelectedCellsChangedEventArgs(ReadOnlyCollection<DataGridCellInfo> addedCells, ReadOnlyCollection<DataGridCellInfo> removedCells)
		{
			if (addedCells == null)
			{
				throw new ArgumentNullException("addedCells");
			}
			if (removedCells == null)
			{
				throw new ArgumentNullException("removedCells");
			}
			this._addedCells = addedCells;
			this._removedCells = removedCells;
		}

		// Token: 0x060055BB RID: 21947 RVA: 0x0017C181 File Offset: 0x0017A381
		internal SelectedCellsChangedEventArgs(DataGrid owner, VirtualizedCellInfoCollection addedCells, VirtualizedCellInfoCollection removedCells)
		{
			this._addedCells = ((addedCells != null) ? addedCells : VirtualizedCellInfoCollection.MakeEmptyCollection(owner));
			this._removedCells = ((removedCells != null) ? removedCells : VirtualizedCellInfoCollection.MakeEmptyCollection(owner));
		}

		/// <summary>Gets the cells that were added to the selection.</summary>
		/// <returns>The added cells.</returns>
		// Token: 0x170014DA RID: 5338
		// (get) Token: 0x060055BC RID: 21948 RVA: 0x0017C1AD File Offset: 0x0017A3AD
		public IList<DataGridCellInfo> AddedCells
		{
			get
			{
				return this._addedCells;
			}
		}

		/// <summary>Gets the list of cells removed from the selection.</summary>
		/// <returns>The list of cells removed.</returns>
		// Token: 0x170014DB RID: 5339
		// (get) Token: 0x060055BD RID: 21949 RVA: 0x0017C1B5 File Offset: 0x0017A3B5
		public IList<DataGridCellInfo> RemovedCells
		{
			get
			{
				return this._removedCells;
			}
		}

		// Token: 0x04002E0A RID: 11786
		private IList<DataGridCellInfo> _addedCells;

		// Token: 0x04002E0B RID: 11787
		private IList<DataGridCellInfo> _removedCells;
	}
}
