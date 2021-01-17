using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Represents a panel that lays out cells and column headers in a data grid. </summary>
	// Token: 0x0200049A RID: 1178
	public class DataGridCellsPanel : VirtualizingPanel
	{
		// Token: 0x0600473B RID: 18235 RVA: 0x00142814 File Offset: 0x00140A14
		static DataGridCellsPanel()
		{
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(DataGridCellsPanel), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridCellsPanel" /> class.</summary>
		// Token: 0x0600473C RID: 18236 RVA: 0x00142835 File Offset: 0x00140A35
		public DataGridCellsPanel()
		{
			this.IsVirtualizing = false;
			this.InRecyclingMode = false;
		}

		/// <summary>Determines the desired size of the panel.</summary>
		/// <param name="constraint">The maximum size that the panel can occupy.</param>
		/// <returns>The desired size of the panel.</returns>
		// Token: 0x0600473D RID: 18237 RVA: 0x00142858 File Offset: 0x00140A58
		protected override Size MeasureOverride(Size constraint)
		{
			Size size = default(Size);
			this.DetermineVirtualizationState();
			this.EnsureRealizedChildren();
			IList realizedChildren = this.RealizedChildren;
			if (this.RebuildRealizedColumnsBlockList)
			{
				size = this.DetermineRealizedColumnsBlockList(constraint);
			}
			else
			{
				size = this.GenerateAndMeasureChildrenForRealizedColumns(constraint);
			}
			if (this.IsVirtualizing && this.InRecyclingMode)
			{
				this.DisconnectRecycledContainers();
			}
			if (!DoubleUtil.AreClose(base.DesiredSize, size) && base.MeasureDuringArrange)
			{
				this.ParentPresenter.InvalidateMeasure();
				UIElement uielement = VisualTreeHelper.GetParent(this) as UIElement;
				if (uielement != null)
				{
					uielement.InvalidateMeasure();
				}
			}
			return size;
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x001428E8 File Offset: 0x00140AE8
		private static void MeasureChild(UIElement child, Size constraint)
		{
			IProvideDataGridColumn provideDataGridColumn = child as IProvideDataGridColumn;
			bool flag = child is DataGridColumnHeader;
			Size availableSize = new Size(double.PositiveInfinity, constraint.Height);
			double num = 0.0;
			bool flag2 = false;
			if (provideDataGridColumn != null)
			{
				DataGridColumn column = provideDataGridColumn.Column;
				DataGridLength width = column.Width;
				if (width.IsAuto || (width.IsSizeToHeader && flag) || (width.IsSizeToCells && !flag))
				{
					child.Measure(availableSize);
					num = child.DesiredSize.Width;
					flag2 = true;
				}
				availableSize.Width = column.GetConstraintWidth(flag);
			}
			if (DoubleUtil.AreClose(num, 0.0))
			{
				child.Measure(availableSize);
			}
			Size desiredSize = child.DesiredSize;
			if (provideDataGridColumn != null)
			{
				DataGridColumn column2 = provideDataGridColumn.Column;
				column2.UpdateDesiredWidthForAutoColumn(flag, DoubleUtil.AreClose(num, 0.0) ? desiredSize.Width : num);
				DataGridLength width2 = column2.Width;
				if (flag2 && !DoubleUtil.IsNaN(width2.DisplayValue) && DoubleUtil.GreaterThan(num, width2.DisplayValue))
				{
					availableSize.Width = width2.DisplayValue;
					child.Measure(availableSize);
				}
			}
		}

		// Token: 0x0600473F RID: 18239 RVA: 0x00142A14 File Offset: 0x00140C14
		private Size GenerateAndMeasureChildrenForRealizedColumns(Size constraint)
		{
			double num = 0.0;
			double num2 = 0.0;
			DataGrid parentDataGrid = this.ParentDataGrid;
			double averageColumnWidth = parentDataGrid.InternalColumns.AverageColumnWidth;
			IItemContainerGenerator itemContainerGenerator = base.ItemContainerGenerator;
			List<RealizedColumnsBlock> realizedColumnsBlockList = this.RealizedColumnsBlockList;
			this.VirtualizeChildren(realizedColumnsBlockList, itemContainerGenerator);
			if (realizedColumnsBlockList.Count > 0)
			{
				int i = 0;
				int count = realizedColumnsBlockList.Count;
				while (i < count)
				{
					RealizedColumnsBlock realizedColumnsBlock = realizedColumnsBlockList[i];
					Size size = this.GenerateChildren(itemContainerGenerator, realizedColumnsBlock.StartIndex, realizedColumnsBlock.EndIndex, constraint);
					num += size.Width;
					num2 = Math.Max(num2, size.Height);
					if (i != count - 1)
					{
						RealizedColumnsBlock realizedColumnsBlock2 = realizedColumnsBlockList[i + 1];
						num += this.GetColumnEstimatedMeasureWidthSum(realizedColumnsBlock.EndIndex + 1, realizedColumnsBlock2.StartIndex - 1, averageColumnWidth);
					}
					i++;
				}
				num += this.GetColumnEstimatedMeasureWidthSum(0, realizedColumnsBlockList[0].StartIndex - 1, averageColumnWidth);
				num += this.GetColumnEstimatedMeasureWidthSum(realizedColumnsBlockList[realizedColumnsBlockList.Count - 1].EndIndex + 1, parentDataGrid.Columns.Count - 1, averageColumnWidth);
			}
			else
			{
				num = 0.0;
			}
			return new Size(num, num2);
		}

		// Token: 0x06004740 RID: 18240 RVA: 0x00142B5C File Offset: 0x00140D5C
		private Size DetermineRealizedColumnsBlockList(Size constraint)
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			Size result = default(Size);
			DataGrid parentDataGrid = this.ParentDataGrid;
			if (parentDataGrid == null)
			{
				return result;
			}
			double horizontalScrollOffset = parentDataGrid.HorizontalScrollOffset;
			double cellsPanelHorizontalOffset = parentDataGrid.CellsPanelHorizontalOffset;
			double num = horizontalScrollOffset;
			double num2 = -cellsPanelHorizontalOffset;
			double num3 = horizontalScrollOffset - cellsPanelHorizontalOffset;
			int num4 = -1;
			int lastVisibleNonFrozenDisplayIndex = -1;
			double num5 = this.GetViewportWidth() - cellsPanelHorizontalOffset;
			double num6 = 0.0;
			if (this.IsVirtualizing && DoubleUtil.LessThan(num5, 0.0))
			{
				return result;
			}
			bool hasVisibleStarColumns = parentDataGrid.InternalColumns.HasVisibleStarColumns;
			double averageColumnWidth = parentDataGrid.InternalColumns.AverageColumnWidth;
			bool flag = DoubleUtil.AreClose(averageColumnWidth, 0.0);
			bool flag2 = !this.IsVirtualizing;
			bool flag3 = flag || hasVisibleStarColumns || flag2;
			int frozenColumnCount = parentDataGrid.FrozenColumnCount;
			int num7 = -1;
			bool redeterminationNeeded = false;
			IItemContainerGenerator itemContainerGenerator = base.ItemContainerGenerator;
			IDisposable disposable = null;
			int num8 = 0;
			try
			{
				int i = 0;
				int count = parentDataGrid.Columns.Count;
				while (i < count)
				{
					DataGridColumn dataGridColumn = parentDataGrid.ColumnFromDisplayIndex(i);
					if (dataGridColumn.IsVisible)
					{
						int num9 = parentDataGrid.ColumnIndexFromDisplayIndex(i);
						if (num9 != num8 || num7 != num9 - 1)
						{
							num8 = num9;
							if (disposable != null)
							{
								disposable.Dispose();
								disposable = null;
							}
						}
						num7 = num9;
						Size size;
						if (flag3)
						{
							if (this.GenerateChild(itemContainerGenerator, constraint, dataGridColumn, ref disposable, ref num8, out size) == null)
							{
								break;
							}
						}
						else
						{
							size = new Size(DataGridCellsPanel.GetColumnEstimatedMeasureWidth(dataGridColumn, averageColumnWidth), 0.0);
						}
						if (flag2 || hasVisibleStarColumns || DoubleUtil.LessThan(num6, num5))
						{
							if (i < frozenColumnCount)
							{
								if (!flag3 && this.GenerateChild(itemContainerGenerator, constraint, dataGridColumn, ref disposable, ref num8, out size) == null)
								{
									break;
								}
								list.Add(num9);
								list2.Add(i);
								num6 += size.Width;
								num += size.Width;
							}
							else if (DoubleUtil.LessThanOrClose(num2, num3))
							{
								if (DoubleUtil.LessThanOrClose(num2 + size.Width, num3))
								{
									if (flag3)
									{
										if (flag2 || hasVisibleStarColumns)
										{
											list.Add(num9);
											list2.Add(i);
										}
										else if (flag)
										{
											redeterminationNeeded = true;
										}
									}
									else if (disposable != null)
									{
										disposable.Dispose();
										disposable = null;
									}
									num2 += size.Width;
								}
								else
								{
									if (!flag3 && this.GenerateChild(itemContainerGenerator, constraint, dataGridColumn, ref disposable, ref num8, out size) == null)
									{
										break;
									}
									double num10 = num3 - num2;
									if (DoubleUtil.AreClose(num10, 0.0))
									{
										num2 = num + size.Width;
										num6 += size.Width;
									}
									else
									{
										double num11 = size.Width - num10;
										num2 = num + num11;
										num6 += num11;
									}
									list.Add(num9);
									list2.Add(i);
									num4 = i;
									lastVisibleNonFrozenDisplayIndex = i;
								}
							}
							else
							{
								if (!flag3 && this.GenerateChild(itemContainerGenerator, constraint, dataGridColumn, ref disposable, ref num8, out size) == null)
								{
									break;
								}
								if (num4 < 0)
								{
									num4 = i;
								}
								lastVisibleNonFrozenDisplayIndex = i;
								num2 += size.Width;
								num6 += size.Width;
								list.Add(num9);
								list2.Add(i);
							}
						}
						result.Width += size.Width;
						result.Height = Math.Max(result.Height, size.Height);
					}
					i++;
				}
			}
			finally
			{
				if (disposable != null)
				{
					disposable.Dispose();
					disposable = null;
				}
			}
			if (!hasVisibleStarColumns && !flag2)
			{
				bool flag4 = this.ParentPresenter is DataGridColumnHeadersPresenter;
				if (flag4)
				{
					Size size2 = this.EnsureAtleastOneHeader(itemContainerGenerator, constraint, list, list2);
					result.Height = Math.Max(result.Height, size2.Height);
					redeterminationNeeded = true;
				}
				else
				{
					this.EnsureFocusTrail(list, list2, num4, lastVisibleNonFrozenDisplayIndex, constraint);
				}
			}
			this.UpdateRealizedBlockLists(list, list2, redeterminationNeeded);
			this.VirtualizeChildren(this.RealizedColumnsBlockList, itemContainerGenerator);
			return result;
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x00142F44 File Offset: 0x00141144
		private void UpdateRealizedBlockLists(List<int> realizedColumnIndices, List<int> realizedColumnDisplayIndices, bool redeterminationNeeded)
		{
			realizedColumnIndices.Sort();
			this.RealizedColumnsBlockList = DataGridCellsPanel.BuildRealizedColumnsBlockList(realizedColumnIndices);
			this.RealizedColumnsDisplayIndexBlockList = DataGridCellsPanel.BuildRealizedColumnsBlockList(realizedColumnDisplayIndices);
			if (!redeterminationNeeded)
			{
				this.RebuildRealizedColumnsBlockList = false;
			}
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x00142F70 File Offset: 0x00141170
		private static List<RealizedColumnsBlock> BuildRealizedColumnsBlockList(List<int> indexList)
		{
			List<RealizedColumnsBlock> list = new List<RealizedColumnsBlock>();
			if (indexList.Count == 1)
			{
				list.Add(new RealizedColumnsBlock(indexList[0], indexList[0], 0));
			}
			else if (indexList.Count > 0)
			{
				int startIndex = indexList[0];
				int i = 1;
				int count = indexList.Count;
				while (i < count)
				{
					if (indexList[i] != indexList[i - 1] + 1)
					{
						if (list.Count == 0)
						{
							list.Add(new RealizedColumnsBlock(startIndex, indexList[i - 1], 0));
						}
						else
						{
							RealizedColumnsBlock realizedColumnsBlock = list[list.Count - 1];
							int startIndexOffset = realizedColumnsBlock.StartIndexOffset + realizedColumnsBlock.EndIndex - realizedColumnsBlock.StartIndex + 1;
							list.Add(new RealizedColumnsBlock(startIndex, indexList[i - 1], startIndexOffset));
						}
						startIndex = indexList[i];
					}
					if (i == count - 1)
					{
						if (list.Count == 0)
						{
							list.Add(new RealizedColumnsBlock(startIndex, indexList[i], 0));
						}
						else
						{
							RealizedColumnsBlock realizedColumnsBlock2 = list[list.Count - 1];
							int startIndexOffset2 = realizedColumnsBlock2.StartIndexOffset + realizedColumnsBlock2.EndIndex - realizedColumnsBlock2.StartIndex + 1;
							list.Add(new RealizedColumnsBlock(startIndex, indexList[i], startIndexOffset2));
						}
					}
					i++;
				}
			}
			return list;
		}

		// Token: 0x06004743 RID: 18243 RVA: 0x001430BC File Offset: 0x001412BC
		private static GeneratorPosition IndexToGeneratorPositionForStart(IItemContainerGenerator generator, int index, out int childIndex)
		{
			GeneratorPosition result = (generator != null) ? generator.GeneratorPositionFromIndex(index) : new GeneratorPosition(-1, index + 1);
			childIndex = ((result.Offset == 0) ? result.Index : (result.Index + 1));
			return result;
		}

		// Token: 0x06004744 RID: 18244 RVA: 0x001430FD File Offset: 0x001412FD
		private UIElement GenerateChild(IItemContainerGenerator generator, Size constraint, DataGridColumn column, ref IDisposable generatorState, ref int childIndex, out Size childSize)
		{
			if (generatorState == null)
			{
				generatorState = generator.StartAt(DataGridCellsPanel.IndexToGeneratorPositionForStart(generator, childIndex, out childIndex), GeneratorDirection.Forward, true);
			}
			return this.GenerateChild(generator, constraint, column, ref childIndex, out childSize);
		}

		// Token: 0x06004745 RID: 18245 RVA: 0x00143128 File Offset: 0x00141328
		private UIElement GenerateChild(IItemContainerGenerator generator, Size constraint, DataGridColumn column, ref int childIndex, out Size childSize)
		{
			bool newlyRealized;
			UIElement uielement = generator.GenerateNext(out newlyRealized) as UIElement;
			if (uielement == null)
			{
				childSize = default(Size);
				return null;
			}
			this.AddContainerFromGenerator(childIndex, uielement, newlyRealized);
			childIndex++;
			DataGridCellsPanel.MeasureChild(uielement, constraint);
			DataGridLength width = column.Width;
			childSize = uielement.DesiredSize;
			if (!DoubleUtil.IsNaN(width.DisplayValue))
			{
				childSize = new Size(width.DisplayValue, childSize.Height);
			}
			return uielement;
		}

		// Token: 0x06004746 RID: 18246 RVA: 0x001431A8 File Offset: 0x001413A8
		private Size GenerateChildren(IItemContainerGenerator generator, int startIndex, int endIndex, Size constraint)
		{
			double num = 0.0;
			double num2 = 0.0;
			int num3;
			GeneratorPosition position = DataGridCellsPanel.IndexToGeneratorPositionForStart(generator, startIndex, out num3);
			DataGrid parentDataGrid = this.ParentDataGrid;
			using (generator.StartAt(position, GeneratorDirection.Forward, true))
			{
				for (int i = startIndex; i <= endIndex; i++)
				{
					if (parentDataGrid.Columns[i].IsVisible)
					{
						Size size;
						if (this.GenerateChild(generator, constraint, parentDataGrid.Columns[i], ref num3, out size) == null)
						{
							return new Size(num, num2);
						}
						num += size.Width;
						num2 = Math.Max(num2, size.Height);
					}
				}
			}
			return new Size(num, num2);
		}

		// Token: 0x06004747 RID: 18247 RVA: 0x00143274 File Offset: 0x00141474
		private void AddContainerFromGenerator(int childIndex, UIElement child, bool newlyRealized)
		{
			if (!newlyRealized)
			{
				if (this.InRecyclingMode)
				{
					IList realizedChildren = this.RealizedChildren;
					if (childIndex >= realizedChildren.Count || realizedChildren[childIndex] != child)
					{
						this.InsertRecycledContainer(childIndex, child);
						child.Measure(default(Size));
						return;
					}
				}
			}
			else
			{
				this.InsertNewContainer(childIndex, child);
			}
		}

		// Token: 0x06004748 RID: 18248 RVA: 0x001432C6 File Offset: 0x001414C6
		private void InsertRecycledContainer(int childIndex, UIElement container)
		{
			this.InsertContainer(childIndex, container, true);
		}

		// Token: 0x06004749 RID: 18249 RVA: 0x001432D1 File Offset: 0x001414D1
		private void InsertNewContainer(int childIndex, UIElement container)
		{
			this.InsertContainer(childIndex, container, false);
		}

		// Token: 0x0600474A RID: 18250 RVA: 0x001432DC File Offset: 0x001414DC
		private void InsertContainer(int childIndex, UIElement container, bool isRecycled)
		{
			UIElementCollection internalChildren = base.InternalChildren;
			int num = 0;
			if (childIndex > 0)
			{
				num = this.ChildIndexFromRealizedIndex(childIndex - 1);
				num++;
			}
			if (!isRecycled || num >= internalChildren.Count || internalChildren[num] != container)
			{
				if (num < internalChildren.Count)
				{
					int num2 = num;
					if (isRecycled && VisualTreeHelper.GetParent(container) != null)
					{
						int num3 = internalChildren.IndexOf(container);
						base.RemoveInternalChildRange(num3, 1);
						if (num3 < num2)
						{
							num2--;
						}
						base.InsertInternalChild(num2, container);
					}
					else
					{
						base.InsertInternalChild(num2, container);
					}
				}
				else if (isRecycled && VisualTreeHelper.GetParent(container) != null)
				{
					int index = internalChildren.IndexOf(container);
					base.RemoveInternalChildRange(index, 1);
					base.AddInternalChild(container);
				}
				else
				{
					base.AddInternalChild(container);
				}
			}
			if (this.IsVirtualizing && this.InRecyclingMode)
			{
				this._realizedChildren.Insert(childIndex, container);
			}
			base.ItemContainerGenerator.PrepareItemContainer(container);
		}

		// Token: 0x0600474B RID: 18251 RVA: 0x001433B4 File Offset: 0x001415B4
		private int ChildIndexFromRealizedIndex(int realizedChildIndex)
		{
			if (this.IsVirtualizing && this.InRecyclingMode && realizedChildIndex < this._realizedChildren.Count)
			{
				UIElement uielement = this._realizedChildren[realizedChildIndex];
				UIElementCollection internalChildren = base.InternalChildren;
				for (int i = realizedChildIndex; i < internalChildren.Count; i++)
				{
					if (internalChildren[i] == uielement)
					{
						return i;
					}
				}
			}
			return realizedChildIndex;
		}

		// Token: 0x0600474C RID: 18252 RVA: 0x00143414 File Offset: 0x00141614
		private static bool InBlockOrNextBlock(List<RealizedColumnsBlock> blockList, int index, ref int blockIndex, ref RealizedColumnsBlock block, out bool pastLastBlock)
		{
			pastLastBlock = false;
			bool result = true;
			if (index < block.StartIndex)
			{
				result = false;
			}
			else if (index > block.EndIndex)
			{
				if (blockIndex == blockList.Count - 1)
				{
					blockIndex++;
					pastLastBlock = true;
					result = false;
				}
				else
				{
					int num = blockIndex + 1;
					blockIndex = num;
					block = blockList[num];
					if (index < block.StartIndex || index > block.EndIndex)
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x0600474D RID: 18253 RVA: 0x00143484 File Offset: 0x00141684
		private Size EnsureAtleastOneHeader(IItemContainerGenerator generator, Size constraint, List<int> realizedColumnIndices, List<int> realizedColumnDisplayIndices)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			int count = parentDataGrid.Columns.Count;
			Size result = default(Size);
			if (this.RealizedChildren.Count == 0 && count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					DataGridColumn dataGridColumn = parentDataGrid.Columns[i];
					if (dataGridColumn.IsVisible)
					{
						int index = i;
						using (generator.StartAt(DataGridCellsPanel.IndexToGeneratorPositionForStart(generator, index, out index), GeneratorDirection.Forward, true))
						{
							UIElement uielement = this.GenerateChild(generator, constraint, dataGridColumn, ref index, out result);
							if (uielement != null)
							{
								int num = 0;
								DataGridCellsPanel.AddToIndicesListIfNeeded(realizedColumnIndices, realizedColumnDisplayIndices, i, dataGridColumn.DisplayIndex, ref num);
								return result;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x00143548 File Offset: 0x00141748
		private void EnsureFocusTrail(List<int> realizedColumnIndices, List<int> realizedColumnDisplayIndices, int firstVisibleNonFrozenDisplayIndex, int lastVisibleNonFrozenDisplayIndex, Size constraint)
		{
			if (firstVisibleNonFrozenDisplayIndex < 0)
			{
				return;
			}
			int frozenColumnCount = this.ParentDataGrid.FrozenColumnCount;
			int count = this.Columns.Count;
			ItemsControl parentPresenter = this.ParentPresenter;
			if (parentPresenter == null)
			{
				return;
			}
			ItemContainerGenerator itemContainerGenerator = parentPresenter.ItemContainerGenerator;
			int num = 0;
			int num2 = -1;
			for (int i = 0; i < firstVisibleNonFrozenDisplayIndex; i++)
			{
				if (this.GenerateChildForFocusTrail(itemContainerGenerator, realizedColumnIndices, realizedColumnDisplayIndices, constraint, i, ref num))
				{
					num2 = i;
					break;
				}
			}
			if (num2 < frozenColumnCount)
			{
				for (int j = frozenColumnCount; j < count; j++)
				{
					if (this.GenerateChildForFocusTrail(itemContainerGenerator, realizedColumnIndices, realizedColumnDisplayIndices, constraint, j, ref num))
					{
						num2 = j;
						break;
					}
				}
			}
			for (int k = firstVisibleNonFrozenDisplayIndex - 1; k > num2; k--)
			{
				if (this.GenerateChildForFocusTrail(itemContainerGenerator, realizedColumnIndices, realizedColumnDisplayIndices, constraint, k, ref num))
				{
					num2 = k;
					break;
				}
			}
			for (int l = lastVisibleNonFrozenDisplayIndex + 1; l < count; l++)
			{
				if (this.GenerateChildForFocusTrail(itemContainerGenerator, realizedColumnIndices, realizedColumnDisplayIndices, constraint, l, ref num))
				{
					num2 = l;
					break;
				}
			}
			int num3 = count - 1;
			while (num3 > num2 && !this.GenerateChildForFocusTrail(itemContainerGenerator, realizedColumnIndices, realizedColumnDisplayIndices, constraint, num3, ref num))
			{
				num3--;
			}
		}

		// Token: 0x0600474F RID: 18255 RVA: 0x00143658 File Offset: 0x00141858
		private bool GenerateChildForFocusTrail(ItemContainerGenerator generator, List<int> realizedColumnIndices, List<int> realizedColumnDisplayIndices, Size constraint, int displayIndex, ref int displayIndexListIterator)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			DataGridColumn dataGridColumn = parentDataGrid.ColumnFromDisplayIndex(displayIndex);
			if (dataGridColumn.IsVisible)
			{
				int num = parentDataGrid.ColumnIndexFromDisplayIndex(displayIndex);
				UIElement uielement = generator.ContainerFromIndex(num) as UIElement;
				if (uielement == null)
				{
					int index = num;
					using (((IItemContainerGenerator)generator).StartAt(DataGridCellsPanel.IndexToGeneratorPositionForStart(generator, index, out index), GeneratorDirection.Forward, true))
					{
						Size size;
						uielement = this.GenerateChild(generator, constraint, dataGridColumn, ref index, out size);
					}
				}
				if (uielement != null && DataGridHelper.TreeHasFocusAndTabStop(uielement))
				{
					DataGridCellsPanel.AddToIndicesListIfNeeded(realizedColumnIndices, realizedColumnDisplayIndices, num, displayIndex, ref displayIndexListIterator);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004750 RID: 18256 RVA: 0x001436F4 File Offset: 0x001418F4
		private static void AddToIndicesListIfNeeded(List<int> realizedColumnIndices, List<int> realizedColumnDisplayIndices, int columnIndex, int displayIndex, ref int displayIndexListIterator)
		{
			int count = realizedColumnDisplayIndices.Count;
			while (displayIndexListIterator < count)
			{
				if (realizedColumnDisplayIndices[displayIndexListIterator] == displayIndex)
				{
					return;
				}
				if (realizedColumnDisplayIndices[displayIndexListIterator] > displayIndex)
				{
					realizedColumnDisplayIndices.Insert(displayIndexListIterator, displayIndex);
					realizedColumnIndices.Add(columnIndex);
					return;
				}
				displayIndexListIterator++;
			}
			realizedColumnIndices.Add(columnIndex);
			realizedColumnDisplayIndices.Add(displayIndex);
		}

		// Token: 0x06004751 RID: 18257 RVA: 0x00143754 File Offset: 0x00141954
		private void VirtualizeChildren(List<RealizedColumnsBlock> blockList, IItemContainerGenerator generator)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			ObservableCollection<DataGridColumn> columns = parentDataGrid.Columns;
			int count = columns.Count;
			int num = 0;
			IList realizedChildren = this.RealizedChildren;
			int num2 = realizedChildren.Count;
			if (num2 == 0)
			{
				return;
			}
			int index = 0;
			int count2 = blockList.Count;
			RealizedColumnsBlock realizedColumnsBlock = (count2 > 0) ? blockList[index] : new RealizedColumnsBlock(-1, -1, -1);
			bool flag = count2 <= 0;
			int num3 = -1;
			int num4 = 0;
			int num5 = -1;
			ItemsControl parentPresenter = this.ParentPresenter;
			DataGridCellsPresenter dataGridCellsPresenter = parentPresenter as DataGridCellsPresenter;
			DataGridColumnHeadersPresenter dataGridColumnHeadersPresenter = parentPresenter as DataGridColumnHeadersPresenter;
			for (int i = 0; i < num2; i++)
			{
				int num6 = i;
				UIElement uielement = realizedChildren[i] as UIElement;
				IProvideDataGridColumn provideDataGridColumn = uielement as IProvideDataGridColumn;
				if (provideDataGridColumn != null)
				{
					DataGridColumn column = provideDataGridColumn.Column;
					while (num < count && column != columns[num])
					{
						num++;
					}
					num6 = num++;
				}
				bool flag2 = flag || !DataGridCellsPanel.InBlockOrNextBlock(blockList, num6, ref index, ref realizedColumnsBlock, out flag);
				DataGridCell dataGridCell = uielement as DataGridCell;
				if ((dataGridCell != null && (dataGridCell.IsEditing || dataGridCell.IsKeyboardFocusWithin || dataGridCell == parentDataGrid.FocusedCell)) || (dataGridCellsPresenter != null && dataGridCellsPresenter.IsItemItsOwnContainerInternal(dataGridCellsPresenter.Items[num6])) || (dataGridColumnHeadersPresenter != null && dataGridColumnHeadersPresenter.IsItemItsOwnContainerInternal(dataGridColumnHeadersPresenter.Items[num6])))
				{
					flag2 = false;
				}
				if (!columns[num6].IsVisible)
				{
					flag2 = true;
				}
				if (flag2)
				{
					if (num3 == -1)
					{
						num3 = i;
						num4 = 1;
					}
					else if (num5 == num6 - 1)
					{
						num4++;
					}
					else
					{
						this.CleanupRange(realizedChildren, generator, num3, num4);
						num2 -= num4;
						i -= num4;
						num4 = 1;
						num3 = i;
					}
					num5 = num6;
				}
				else if (num4 > 0)
				{
					this.CleanupRange(realizedChildren, generator, num3, num4);
					num2 -= num4;
					i -= num4;
					num4 = 0;
					num3 = -1;
				}
			}
			if (num4 > 0)
			{
				this.CleanupRange(realizedChildren, generator, num3, num4);
			}
		}

		// Token: 0x06004752 RID: 18258 RVA: 0x00143950 File Offset: 0x00141B50
		private void CleanupRange(IList children, IItemContainerGenerator generator, int startIndex, int count)
		{
			if (count <= 0)
			{
				return;
			}
			if (this.IsVirtualizing && this.InRecyclingMode)
			{
				GeneratorPosition position = new GeneratorPosition(startIndex, 0);
				((IRecyclingItemContainerGenerator)generator).Recycle(position, count);
				this._realizedChildren.RemoveRange(startIndex, count);
				return;
			}
			base.RemoveInternalChildRange(startIndex, count);
			generator.Remove(new GeneratorPosition(startIndex, 0), count);
		}

		// Token: 0x06004753 RID: 18259 RVA: 0x001439B4 File Offset: 0x00141BB4
		private void DisconnectRecycledContainers()
		{
			int num = 0;
			UIElement uielement = (this._realizedChildren.Count > 0) ? this._realizedChildren[0] : null;
			UIElementCollection internalChildren = base.InternalChildren;
			int num2 = -1;
			int num3 = 0;
			for (int i = 0; i < internalChildren.Count; i++)
			{
				UIElement uielement2 = internalChildren[i];
				if (uielement2 == uielement)
				{
					if (num3 > 0)
					{
						base.RemoveInternalChildRange(num2, num3);
						i -= num3;
						num3 = 0;
						num2 = -1;
					}
					num++;
					if (num < this._realizedChildren.Count)
					{
						uielement = this._realizedChildren[num];
					}
					else
					{
						uielement = null;
					}
				}
				else
				{
					if (num2 == -1)
					{
						num2 = i;
					}
					num3++;
				}
			}
			if (num3 > 0)
			{
				base.RemoveInternalChildRange(num2, num3);
			}
		}

		// Token: 0x06004754 RID: 18260 RVA: 0x00143A70 File Offset: 0x00141C70
		private void InitializeArrangeState(DataGridCellsPanel.ArrangeState arrangeState)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			double horizontalScrollOffset = parentDataGrid.HorizontalScrollOffset;
			double cellsPanelHorizontalOffset = parentDataGrid.CellsPanelHorizontalOffset;
			arrangeState.NextFrozenCellStart = horizontalScrollOffset;
			arrangeState.NextNonFrozenCellStart -= cellsPanelHorizontalOffset;
			arrangeState.ViewportStartX = horizontalScrollOffset - cellsPanelHorizontalOffset;
			arrangeState.FrozenColumnCount = parentDataGrid.FrozenColumnCount;
		}

		// Token: 0x06004755 RID: 18261 RVA: 0x00143ABC File Offset: 0x00141CBC
		private void FinishArrange(DataGridCellsPanel.ArrangeState arrangeState)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			if (parentDataGrid != null)
			{
				parentDataGrid.NonFrozenColumnsViewportHorizontalOffset = arrangeState.DataGridHorizontalScrollStartX;
			}
			if (arrangeState.OldClippedChild != null)
			{
				arrangeState.OldClippedChild.CoerceValue(UIElement.ClipProperty);
			}
			this._clippedChildForFrozenBehaviour = arrangeState.NewClippedChild;
			if (this._clippedChildForFrozenBehaviour != null)
			{
				this._clippedChildForFrozenBehaviour.CoerceValue(UIElement.ClipProperty);
			}
		}

		// Token: 0x06004756 RID: 18262 RVA: 0x00143B1B File Offset: 0x00141D1B
		private void SetDataGridCellPanelWidth(IList children, double newWidth)
		{
			if (children.Count != 0 && children[0] is DataGridColumnHeader && !DoubleUtil.AreClose(this.ParentDataGrid.CellsPanelActualWidth, newWidth))
			{
				this.ParentDataGrid.CellsPanelActualWidth = newWidth;
			}
		}

		// Token: 0x06004757 RID: 18263 RVA: 0x00143B54 File Offset: 0x00141D54
		[Conditional("DEBUG")]
		private static void Debug_VerifyRealizedIndexCountVsDisplayIndexCount(List<RealizedColumnsBlock> blockList, List<RealizedColumnsBlock> displayIndexBlockList)
		{
			RealizedColumnsBlock realizedColumnsBlock = blockList[blockList.Count - 1];
			RealizedColumnsBlock realizedColumnsBlock2 = displayIndexBlockList[displayIndexBlockList.Count - 1];
		}

		/// <summary>Determines the final size and placement of the panel.</summary>
		/// <param name="arrangeSize">The maximum size that the panel can occupy.</param>
		/// <returns>The final size and placement of the panel.</returns>
		// Token: 0x06004758 RID: 18264 RVA: 0x00143B80 File Offset: 0x00141D80
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			IList realizedChildren = this.RealizedChildren;
			DataGridCellsPanel.ArrangeState arrangeState = new DataGridCellsPanel.ArrangeState();
			arrangeState.ChildHeight = arrangeSize.Height;
			DataGrid parentDataGrid = this.ParentDataGrid;
			if (parentDataGrid != null)
			{
				parentDataGrid.QueueInvalidateCellsPanelHorizontalOffset();
				this.SetDataGridCellPanelWidth(realizedChildren, arrangeSize.Width);
				this.InitializeArrangeState(arrangeState);
			}
			List<RealizedColumnsBlock> realizedColumnsDisplayIndexBlockList = this.RealizedColumnsDisplayIndexBlockList;
			if (realizedColumnsDisplayIndexBlockList != null && realizedColumnsDisplayIndexBlockList.Count > 0)
			{
				double averageColumnWidth = parentDataGrid.InternalColumns.AverageColumnWidth;
				List<RealizedColumnsBlock> realizedColumnsBlockList = this.RealizedColumnsBlockList;
				List<int> realizedChildrenNotInBlockList = this.GetRealizedChildrenNotInBlockList(realizedColumnsBlockList, realizedChildren);
				int num = -1;
				RealizedColumnsBlock realizedColumnsBlock = realizedColumnsDisplayIndexBlockList[++num];
				bool flag = false;
				int i = 0;
				int count = parentDataGrid.Columns.Count;
				while (i < count)
				{
					bool flag2 = DataGridCellsPanel.InBlockOrNextBlock(realizedColumnsDisplayIndexBlockList, i, ref num, ref realizedColumnsBlock, out flag);
					if (flag)
					{
						break;
					}
					if (flag2)
					{
						int num2 = parentDataGrid.ColumnIndexFromDisplayIndex(i);
						RealizedColumnsBlock realizedBlockForColumn = DataGridCellsPanel.GetRealizedBlockForColumn(realizedColumnsBlockList, num2);
						int num3 = realizedBlockForColumn.StartIndexOffset + num2 - realizedBlockForColumn.StartIndex;
						if (realizedChildrenNotInBlockList != null)
						{
							int num4 = 0;
							int count2 = realizedChildrenNotInBlockList.Count;
							while (num4 < count2 && realizedChildrenNotInBlockList[num4] <= num3)
							{
								num3++;
								num4++;
							}
						}
						this.ArrangeChild(realizedChildren[num3] as UIElement, i, arrangeState);
					}
					else
					{
						DataGridColumn dataGridColumn = parentDataGrid.ColumnFromDisplayIndex(i);
						if (dataGridColumn.IsVisible)
						{
							double columnEstimatedMeasureWidth = DataGridCellsPanel.GetColumnEstimatedMeasureWidth(dataGridColumn, averageColumnWidth);
							arrangeState.NextNonFrozenCellStart += columnEstimatedMeasureWidth;
						}
					}
					i++;
				}
				if (realizedChildrenNotInBlockList != null)
				{
					int j = 0;
					int count3 = realizedChildrenNotInBlockList.Count;
					while (j < count3)
					{
						UIElement uielement = realizedChildren[realizedChildrenNotInBlockList[j]] as UIElement;
						uielement.Arrange(default(Rect));
						j++;
					}
				}
			}
			this.FinishArrange(arrangeState);
			return arrangeSize;
		}

		// Token: 0x06004759 RID: 18265 RVA: 0x00143D48 File Offset: 0x00141F48
		private void ArrangeChild(UIElement child, int displayIndex, DataGridCellsPanel.ArrangeState arrangeState)
		{
			IProvideDataGridColumn provideDataGridColumn = child as IProvideDataGridColumn;
			if (child == this._clippedChildForFrozenBehaviour)
			{
				arrangeState.OldClippedChild = child;
				this._clippedChildForFrozenBehaviour = null;
			}
			double num;
			if (provideDataGridColumn != null)
			{
				num = provideDataGridColumn.Column.Width.DisplayValue;
				if (DoubleUtil.IsNaN(num))
				{
					num = provideDataGridColumn.Column.ActualWidth;
				}
			}
			else
			{
				num = child.DesiredSize.Width;
			}
			Rect finalRect = new Rect(new Size(num, arrangeState.ChildHeight));
			if (displayIndex < arrangeState.FrozenColumnCount)
			{
				finalRect.X = arrangeState.NextFrozenCellStart;
				arrangeState.NextFrozenCellStart += num;
				arrangeState.DataGridHorizontalScrollStartX += num;
			}
			else if (DoubleUtil.LessThanOrClose(arrangeState.NextNonFrozenCellStart, arrangeState.ViewportStartX))
			{
				if (DoubleUtil.LessThanOrClose(arrangeState.NextNonFrozenCellStart + num, arrangeState.ViewportStartX))
				{
					finalRect.X = arrangeState.NextNonFrozenCellStart;
					arrangeState.NextNonFrozenCellStart += num;
				}
				else
				{
					double num2 = arrangeState.ViewportStartX - arrangeState.NextNonFrozenCellStart;
					if (DoubleUtil.AreClose(num2, 0.0))
					{
						finalRect.X = arrangeState.NextFrozenCellStart;
						arrangeState.NextNonFrozenCellStart = arrangeState.NextFrozenCellStart + num;
					}
					else
					{
						finalRect.X = arrangeState.NextFrozenCellStart - num2;
						double num3 = num - num2;
						arrangeState.NewClippedChild = child;
						this._childClipForFrozenBehavior.Rect = new Rect(num2, 0.0, num3, finalRect.Height);
						arrangeState.NextNonFrozenCellStart = arrangeState.NextFrozenCellStart + num3;
					}
				}
			}
			else
			{
				finalRect.X = arrangeState.NextNonFrozenCellStart;
				arrangeState.NextNonFrozenCellStart += num;
			}
			child.Arrange(finalRect);
		}

		// Token: 0x0600475A RID: 18266 RVA: 0x00143F04 File Offset: 0x00142104
		private static RealizedColumnsBlock GetRealizedBlockForColumn(List<RealizedColumnsBlock> blockList, int columnIndex)
		{
			int i = 0;
			int count = blockList.Count;
			while (i < count)
			{
				RealizedColumnsBlock result = blockList[i];
				if (columnIndex >= result.StartIndex && columnIndex <= result.EndIndex)
				{
					return result;
				}
				i++;
			}
			return new RealizedColumnsBlock(-1, -1, -1);
		}

		// Token: 0x0600475B RID: 18267 RVA: 0x00143F4C File Offset: 0x0014214C
		private List<int> GetRealizedChildrenNotInBlockList(List<RealizedColumnsBlock> blockList, IList children)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			RealizedColumnsBlock realizedColumnsBlock = blockList[blockList.Count - 1];
			int num = realizedColumnsBlock.StartIndexOffset + realizedColumnsBlock.EndIndex - realizedColumnsBlock.StartIndex + 1;
			if (children.Count == num)
			{
				return null;
			}
			List<int> list = new List<int>();
			if (blockList.Count == 0)
			{
				int i = 0;
				int count = children.Count;
				while (i < count)
				{
					list.Add(i);
					i++;
				}
			}
			else
			{
				int num2 = 0;
				RealizedColumnsBlock realizedColumnsBlock2 = blockList[num2++];
				int j = 0;
				int count2 = children.Count;
				while (j < count2)
				{
					IProvideDataGridColumn provideDataGridColumn = children[j] as IProvideDataGridColumn;
					int num3 = j;
					if (provideDataGridColumn != null)
					{
						num3 = parentDataGrid.Columns.IndexOf(provideDataGridColumn.Column);
					}
					if (num3 < realizedColumnsBlock2.StartIndex)
					{
						list.Add(j);
					}
					else if (num3 > realizedColumnsBlock2.EndIndex)
					{
						if (num2 >= blockList.Count)
						{
							for (int k = j; k < count2; k++)
							{
								list.Add(k);
							}
							break;
						}
						realizedColumnsBlock2 = blockList[num2++];
						if (num3 < realizedColumnsBlock2.StartIndex)
						{
							list.Add(j);
						}
					}
					j++;
				}
			}
			return list;
		}

		// Token: 0x17001178 RID: 4472
		// (get) Token: 0x0600475C RID: 18268 RVA: 0x00144090 File Offset: 0x00142290
		internal bool HasCorrectRealizedColumns
		{
			get
			{
				DataGridColumnCollection dataGridColumnCollection = (DataGridColumnCollection)this.ParentDataGrid.Columns;
				this.EnsureRealizedChildren();
				IList realizedChildren = this.RealizedChildren;
				if (realizedChildren.Count == dataGridColumnCollection.Count)
				{
					return true;
				}
				List<int> displayIndexMap = dataGridColumnCollection.DisplayIndexMap;
				List<RealizedColumnsBlock> realizedColumnsBlockList = this.RealizedColumnsBlockList;
				int i = 0;
				int count = realizedChildren.Count;
				for (int j = 0; j < realizedColumnsBlockList.Count; j++)
				{
					RealizedColumnsBlock realizedColumnsBlock = realizedColumnsBlockList[j];
					for (int k = realizedColumnsBlock.StartIndex; k <= realizedColumnsBlock.EndIndex; k++)
					{
						while (i < count)
						{
							IProvideDataGridColumn provideDataGridColumn = realizedChildren[i] as IProvideDataGridColumn;
							if (provideDataGridColumn != null)
							{
								int displayIndex = provideDataGridColumn.Column.DisplayIndex;
								int num = (displayIndex < 0) ? -1 : displayIndexMap[displayIndex];
								if (k < num)
								{
									return false;
								}
								if (k == num)
								{
									break;
								}
							}
							i++;
						}
						if (i == count)
						{
							return false;
						}
						i++;
					}
				}
				return true;
			}
		}

		// Token: 0x17001179 RID: 4473
		// (get) Token: 0x0600475D RID: 18269 RVA: 0x00144184 File Offset: 0x00142384
		// (set) Token: 0x0600475E RID: 18270 RVA: 0x001441BC File Offset: 0x001423BC
		private bool RebuildRealizedColumnsBlockList
		{
			get
			{
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid == null)
				{
					return true;
				}
				DataGridColumnCollection internalColumns = parentDataGrid.InternalColumns;
				if (!this.IsVirtualizing)
				{
					return internalColumns.RebuildRealizedColumnsBlockListForNonVirtualizedRows;
				}
				return internalColumns.RebuildRealizedColumnsBlockListForVirtualizedRows;
			}
			set
			{
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid != null)
				{
					if (this.IsVirtualizing)
					{
						parentDataGrid.InternalColumns.RebuildRealizedColumnsBlockListForVirtualizedRows = value;
						return;
					}
					parentDataGrid.InternalColumns.RebuildRealizedColumnsBlockListForNonVirtualizedRows = value;
				}
			}
		}

		// Token: 0x1700117A RID: 4474
		// (get) Token: 0x0600475F RID: 18271 RVA: 0x001441F4 File Offset: 0x001423F4
		// (set) Token: 0x06004760 RID: 18272 RVA: 0x0014422C File Offset: 0x0014242C
		private List<RealizedColumnsBlock> RealizedColumnsBlockList
		{
			get
			{
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid == null)
				{
					return null;
				}
				DataGridColumnCollection internalColumns = parentDataGrid.InternalColumns;
				if (!this.IsVirtualizing)
				{
					return internalColumns.RealizedColumnsBlockListForNonVirtualizedRows;
				}
				return internalColumns.RealizedColumnsBlockListForVirtualizedRows;
			}
			set
			{
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid != null)
				{
					if (this.IsVirtualizing)
					{
						parentDataGrid.InternalColumns.RealizedColumnsBlockListForVirtualizedRows = value;
						return;
					}
					parentDataGrid.InternalColumns.RealizedColumnsBlockListForNonVirtualizedRows = value;
				}
			}
		}

		// Token: 0x1700117B RID: 4475
		// (get) Token: 0x06004761 RID: 18273 RVA: 0x00144264 File Offset: 0x00142464
		// (set) Token: 0x06004762 RID: 18274 RVA: 0x0014429C File Offset: 0x0014249C
		private List<RealizedColumnsBlock> RealizedColumnsDisplayIndexBlockList
		{
			get
			{
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid == null)
				{
					return null;
				}
				DataGridColumnCollection internalColumns = parentDataGrid.InternalColumns;
				if (!this.IsVirtualizing)
				{
					return internalColumns.RealizedColumnsDisplayIndexBlockListForNonVirtualizedRows;
				}
				return internalColumns.RealizedColumnsDisplayIndexBlockListForVirtualizedRows;
			}
			set
			{
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid != null)
				{
					if (this.IsVirtualizing)
					{
						parentDataGrid.InternalColumns.RealizedColumnsDisplayIndexBlockListForVirtualizedRows = value;
						return;
					}
					parentDataGrid.InternalColumns.RealizedColumnsDisplayIndexBlockListForNonVirtualizedRows = value;
				}
			}
		}

		/// <summary>Indicates that the <see cref="P:System.Windows.Controls.Panel.IsItemsHost" /> property value has changed.</summary>
		/// <param name="oldIsItemsHost">The old value of the <see cref="P:System.Windows.Controls.Panel.IsItemsHost" /> property.</param>
		/// <param name="newIsItemsHost">The new value of the <see cref="P:System.Windows.Controls.Panel.IsItemsHost" /> property.</param>
		// Token: 0x06004763 RID: 18275 RVA: 0x001442D4 File Offset: 0x001424D4
		protected override void OnIsItemsHostChanged(bool oldIsItemsHost, bool newIsItemsHost)
		{
			base.OnIsItemsHostChanged(oldIsItemsHost, newIsItemsHost);
			if (newIsItemsHost)
			{
				ItemsControl parentPresenter = this.ParentPresenter;
				if (parentPresenter != null)
				{
					IItemContainerGenerator itemContainerGenerator = parentPresenter.ItemContainerGenerator;
					if (itemContainerGenerator != null && itemContainerGenerator == itemContainerGenerator.GetItemContainerGeneratorForPanel(this))
					{
						DataGridCellsPresenter dataGridCellsPresenter = parentPresenter as DataGridCellsPresenter;
						if (dataGridCellsPresenter != null)
						{
							dataGridCellsPresenter.InternalItemsHost = this;
							return;
						}
						DataGridColumnHeadersPresenter dataGridColumnHeadersPresenter = parentPresenter as DataGridColumnHeadersPresenter;
						if (dataGridColumnHeadersPresenter != null)
						{
							dataGridColumnHeadersPresenter.InternalItemsHost = this;
							return;
						}
					}
				}
			}
			else
			{
				ItemsControl parentPresenter2 = this.ParentPresenter;
				if (parentPresenter2 != null)
				{
					DataGridCellsPresenter dataGridCellsPresenter2 = parentPresenter2 as DataGridCellsPresenter;
					if (dataGridCellsPresenter2 != null)
					{
						if (dataGridCellsPresenter2.InternalItemsHost == this)
						{
							dataGridCellsPresenter2.InternalItemsHost = null;
							return;
						}
					}
					else
					{
						DataGridColumnHeadersPresenter dataGridColumnHeadersPresenter2 = parentPresenter2 as DataGridColumnHeadersPresenter;
						if (dataGridColumnHeadersPresenter2 != null && dataGridColumnHeadersPresenter2.InternalItemsHost == this)
						{
							dataGridColumnHeadersPresenter2.InternalItemsHost = null;
						}
					}
				}
			}
		}

		// Token: 0x1700117C RID: 4476
		// (get) Token: 0x06004764 RID: 18276 RVA: 0x0014437C File Offset: 0x0014257C
		private DataGridRowsPresenter ParentRowsPresenter
		{
			get
			{
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid == null)
				{
					return null;
				}
				if (!parentDataGrid.IsGrouping)
				{
					return parentDataGrid.InternalItemsHost as DataGridRowsPresenter;
				}
				DataGridCellsPresenter dataGridCellsPresenter = this.ParentPresenter as DataGridCellsPresenter;
				if (dataGridCellsPresenter != null)
				{
					DataGridRow dataGridRowOwner = dataGridCellsPresenter.DataGridRowOwner;
					if (dataGridRowOwner != null)
					{
						return VisualTreeHelper.GetParent(dataGridRowOwner) as DataGridRowsPresenter;
					}
				}
				return null;
			}
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x001443D0 File Offset: 0x001425D0
		private void DetermineVirtualizationState()
		{
			ItemsControl parentPresenter = this.ParentPresenter;
			if (parentPresenter != null)
			{
				this.IsVirtualizing = VirtualizingPanel.GetIsVirtualizing(parentPresenter);
				this.InRecyclingMode = (VirtualizingPanel.GetVirtualizationMode(parentPresenter) == VirtualizationMode.Recycling);
			}
		}

		// Token: 0x1700117D RID: 4477
		// (get) Token: 0x06004766 RID: 18278 RVA: 0x00144402 File Offset: 0x00142602
		// (set) Token: 0x06004767 RID: 18279 RVA: 0x0014440A File Offset: 0x0014260A
		private bool IsVirtualizing { get; set; }

		// Token: 0x1700117E RID: 4478
		// (get) Token: 0x06004768 RID: 18280 RVA: 0x00144413 File Offset: 0x00142613
		// (set) Token: 0x06004769 RID: 18281 RVA: 0x0014441B File Offset: 0x0014261B
		private bool InRecyclingMode { get; set; }

		// Token: 0x0600476A RID: 18282 RVA: 0x00144424 File Offset: 0x00142624
		private static double GetColumnEstimatedMeasureWidth(DataGridColumn column, double averageColumnWidth)
		{
			if (!column.IsVisible)
			{
				return 0.0;
			}
			double num = column.Width.DisplayValue;
			if (DoubleUtil.IsNaN(num))
			{
				num = Math.Max(averageColumnWidth, column.MinWidth);
				num = Math.Min(num, column.MaxWidth);
			}
			return num;
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x00144478 File Offset: 0x00142678
		private double GetColumnEstimatedMeasureWidthSum(int startIndex, int endIndex, double averageColumnWidth)
		{
			double num = 0.0;
			DataGrid parentDataGrid = this.ParentDataGrid;
			for (int i = startIndex; i <= endIndex; i++)
			{
				num += DataGridCellsPanel.GetColumnEstimatedMeasureWidth(parentDataGrid.Columns[i], averageColumnWidth);
			}
			return num;
		}

		// Token: 0x1700117F RID: 4479
		// (get) Token: 0x0600476C RID: 18284 RVA: 0x001444B8 File Offset: 0x001426B8
		private IList RealizedChildren
		{
			get
			{
				if (this.IsVirtualizing && this.InRecyclingMode)
				{
					return this._realizedChildren;
				}
				return base.InternalChildren;
			}
		}

		// Token: 0x0600476D RID: 18285 RVA: 0x001444D8 File Offset: 0x001426D8
		private void EnsureRealizedChildren()
		{
			if (this.IsVirtualizing && this.InRecyclingMode)
			{
				if (this._realizedChildren == null)
				{
					UIElementCollection internalChildren = base.InternalChildren;
					this._realizedChildren = new List<UIElement>(internalChildren.Count);
					for (int i = 0; i < internalChildren.Count; i++)
					{
						this._realizedChildren.Add(internalChildren[i]);
					}
					return;
				}
			}
			else
			{
				this._realizedChildren = null;
			}
		}

		// Token: 0x0600476E RID: 18286 RVA: 0x00144540 File Offset: 0x00142740
		internal double ComputeCellsPanelHorizontalOffset()
		{
			double result = 0.0;
			DataGrid parentDataGrid = this.ParentDataGrid;
			double horizontalScrollOffset = parentDataGrid.HorizontalScrollOffset;
			ScrollViewer internalScrollHost = parentDataGrid.InternalScrollHost;
			if (internalScrollHost != null)
			{
				result = horizontalScrollOffset + base.TransformToAncestor(internalScrollHost).Transform(default(Point)).X;
			}
			return result;
		}

		// Token: 0x0600476F RID: 18287 RVA: 0x00144594 File Offset: 0x00142794
		private double GetViewportWidth()
		{
			double num = 0.0;
			DataGrid parentDataGrid = this.ParentDataGrid;
			if (parentDataGrid != null)
			{
				ScrollContentPresenter internalScrollContentPresenter = parentDataGrid.InternalScrollContentPresenter;
				if (internalScrollContentPresenter != null && !internalScrollContentPresenter.CanContentScroll)
				{
					num = internalScrollContentPresenter.ViewportWidth;
				}
				else
				{
					IScrollInfo scrollInfo = parentDataGrid.InternalItemsHost as IScrollInfo;
					if (scrollInfo != null)
					{
						num = scrollInfo.ViewportWidth;
					}
				}
			}
			DataGridRowsPresenter parentRowsPresenter = this.ParentRowsPresenter;
			if (DoubleUtil.AreClose(num, 0.0) && parentRowsPresenter != null)
			{
				Size availableSize = parentRowsPresenter.AvailableSize;
				if (!DoubleUtil.IsNaN(availableSize.Width) && !double.IsInfinity(availableSize.Width))
				{
					num = availableSize.Width;
				}
				else if (parentDataGrid.IsGrouping)
				{
					IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = DataGridHelper.FindParent<GroupItem>(parentRowsPresenter);
					if (hierarchicalVirtualizationAndScrollInfo != null)
					{
						num = hierarchicalVirtualizationAndScrollInfo.Constraints.Viewport.Width;
					}
				}
			}
			return num;
		}

		/// <summary>Updates the visible cells when an item in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection has changed.</summary>
		/// <param name="sender">The object that raised the <see cref="E:System.Windows.Controls.ItemContainerGenerator.ItemsChanged" /> event.</param>
		/// <param name="args">The data for the event.</param>
		// Token: 0x06004770 RID: 18288 RVA: 0x00144664 File Offset: 0x00142864
		protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
		{
			base.OnItemsChanged(sender, args);
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Remove:
				this.OnItemsRemove(args);
				return;
			case NotifyCollectionChangedAction.Replace:
				this.OnItemsReplace(args);
				return;
			case NotifyCollectionChangedAction.Move:
				this.OnItemsMove(args);
				break;
			case NotifyCollectionChangedAction.Reset:
				break;
			default:
				return;
			}
		}

		// Token: 0x06004771 RID: 18289 RVA: 0x001446B0 File Offset: 0x001428B0
		private void OnItemsRemove(ItemsChangedEventArgs args)
		{
			this.RemoveChildRange(args.Position, args.ItemCount, args.ItemUICount);
		}

		// Token: 0x06004772 RID: 18290 RVA: 0x001446B0 File Offset: 0x001428B0
		private void OnItemsReplace(ItemsChangedEventArgs args)
		{
			this.RemoveChildRange(args.Position, args.ItemCount, args.ItemUICount);
		}

		// Token: 0x06004773 RID: 18291 RVA: 0x001446CA File Offset: 0x001428CA
		private void OnItemsMove(ItemsChangedEventArgs args)
		{
			this.RemoveChildRange(args.OldPosition, args.ItemCount, args.ItemUICount);
		}

		// Token: 0x06004774 RID: 18292 RVA: 0x001446E4 File Offset: 0x001428E4
		private void RemoveChildRange(GeneratorPosition position, int itemCount, int itemUICount)
		{
			if (base.IsItemsHost)
			{
				UIElementCollection internalChildren = base.InternalChildren;
				int num = position.Index;
				if (position.Offset > 0)
				{
					num++;
				}
				if (num < internalChildren.Count && itemUICount > 0)
				{
					base.RemoveInternalChildRange(num, itemUICount);
					if (this.IsVirtualizing && this.InRecyclingMode)
					{
						this._realizedChildren.RemoveRange(num, itemUICount);
					}
				}
			}
		}

		/// <summary>Called when the collection of children in the <see cref="T:System.Windows.Controls.DataGrid" /> is cleared.</summary>
		// Token: 0x06004775 RID: 18293 RVA: 0x00144748 File Offset: 0x00142948
		protected override void OnClearChildren()
		{
			base.OnClearChildren();
			this._realizedChildren = null;
		}

		// Token: 0x06004776 RID: 18294 RVA: 0x00144757 File Offset: 0x00142957
		internal void InternalBringIndexIntoView(int index)
		{
			this.BringIndexIntoView(index);
		}

		/// <summary>Scrolls the viewport to the item at the specified index.</summary>
		/// <param name="index">The index of the item that should become visible.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index is out of range.</exception>
		// Token: 0x06004777 RID: 18295 RVA: 0x00144760 File Offset: 0x00142960
		protected internal override void BringIndexIntoView(int index)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			if (parentDataGrid == null)
			{
				base.BringIndexIntoView(index);
				return;
			}
			if (index < 0 || index >= parentDataGrid.Columns.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (parentDataGrid.InternalColumns.ColumnWidthsComputationPending)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action<int>(this.RetryBringIndexIntoView), index);
				return;
			}
			ScrollContentPresenter internalScrollContentPresenter = parentDataGrid.InternalScrollContentPresenter;
			IScrollInfo scrollInfo = null;
			if (internalScrollContentPresenter != null && !internalScrollContentPresenter.CanContentScroll)
			{
				scrollInfo = internalScrollContentPresenter;
			}
			else
			{
				ScrollViewer internalScrollHost = parentDataGrid.InternalScrollHost;
				if (internalScrollHost != null)
				{
					scrollInfo = internalScrollHost.ScrollInfo;
				}
			}
			if (scrollInfo == null)
			{
				base.BringIndexIntoView(index);
				return;
			}
			bool measureDirty = base.MeasureDirty;
			bool retryRequested = measureDirty;
			double num = 0.0;
			double value = parentDataGrid.HorizontalScrollOffset;
			while (!this.IsChildInView(index, out num) && !DoubleUtil.AreClose(value, num))
			{
				retryRequested = true;
				scrollInfo.SetHorizontalOffset(num);
				base.UpdateLayout();
				value = num;
			}
			if (parentDataGrid.RetryBringColumnIntoView(retryRequested))
			{
				DispatcherPriority priority = measureDirty ? DispatcherPriority.Background : DispatcherPriority.Loaded;
				base.Dispatcher.BeginInvoke(priority, new Action<int>(this.RetryBringIndexIntoView), index);
				base.InvalidateMeasure();
			}
		}

		// Token: 0x06004778 RID: 18296 RVA: 0x00144884 File Offset: 0x00142A84
		private void RetryBringIndexIntoView(int index)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			if (parentDataGrid != null && 0 <= index && index < parentDataGrid.Columns.Count)
			{
				this.BringIndexIntoView(index);
			}
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x001448B4 File Offset: 0x00142AB4
		private bool IsChildInView(int index, out double newHorizontalOffset)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			double horizontalScrollOffset = parentDataGrid.HorizontalScrollOffset;
			newHorizontalOffset = horizontalScrollOffset;
			double averageColumnWidth = parentDataGrid.InternalColumns.AverageColumnWidth;
			int frozenColumnCount = parentDataGrid.FrozenColumnCount;
			double cellsPanelHorizontalOffset = parentDataGrid.CellsPanelHorizontalOffset;
			double viewportWidth = this.GetViewportWidth();
			double num = horizontalScrollOffset;
			double num2 = -cellsPanelHorizontalOffset;
			double num3 = horizontalScrollOffset - cellsPanelHorizontalOffset;
			int displayIndex = this.Columns[index].DisplayIndex;
			double num4 = 0.0;
			double num5 = 0.0;
			for (int i = 0; i <= displayIndex; i++)
			{
				DataGridColumn dataGridColumn = parentDataGrid.ColumnFromDisplayIndex(i);
				if (dataGridColumn.IsVisible)
				{
					double columnEstimatedMeasureWidth = DataGridCellsPanel.GetColumnEstimatedMeasureWidth(dataGridColumn, averageColumnWidth);
					if (i < frozenColumnCount)
					{
						num4 = num;
						num5 = num4 + columnEstimatedMeasureWidth;
						num += columnEstimatedMeasureWidth;
					}
					else if (DoubleUtil.LessThanOrClose(num2, num3))
					{
						if (DoubleUtil.LessThanOrClose(num2 + columnEstimatedMeasureWidth, num3))
						{
							num4 = num2;
							num5 = num4 + columnEstimatedMeasureWidth;
							num2 += columnEstimatedMeasureWidth;
						}
						else
						{
							num4 = num;
							double num6 = num3 - num2;
							if (DoubleUtil.AreClose(num6, 0.0))
							{
								num5 = num4 + columnEstimatedMeasureWidth;
								num2 = num + columnEstimatedMeasureWidth;
							}
							else
							{
								double num7 = columnEstimatedMeasureWidth - num6;
								num5 = num4 + num7;
								num2 = num + num7;
								if (i == displayIndex)
								{
									newHorizontalOffset = horizontalScrollOffset - num6;
									return false;
								}
							}
						}
					}
					else
					{
						num4 = num2;
						num5 = num4 + columnEstimatedMeasureWidth;
						num2 += columnEstimatedMeasureWidth;
					}
				}
			}
			double num8 = num3 + viewportWidth;
			if (DoubleUtil.LessThan(num4, num3))
			{
				newHorizontalOffset = num4 + cellsPanelHorizontalOffset;
			}
			else
			{
				if (!DoubleUtil.GreaterThan(num5, num8))
				{
					return true;
				}
				double num9 = num5 - num8;
				if (displayIndex < frozenColumnCount)
				{
					num -= num5 - num4;
				}
				if (DoubleUtil.LessThan(num4 - num9, num))
				{
					num9 = num4 - num;
				}
				if (DoubleUtil.AreClose(num9, 0.0))
				{
					return true;
				}
				newHorizontalOffset = horizontalScrollOffset + num9;
			}
			return false;
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x00144A7E File Offset: 0x00142C7E
		internal Geometry GetFrozenClipForChild(UIElement child)
		{
			if (child == this._clippedChildForFrozenBehaviour)
			{
				return this._childClipForFrozenBehavior;
			}
			return null;
		}

		// Token: 0x17001180 RID: 4480
		// (get) Token: 0x0600477B RID: 18299 RVA: 0x00144A94 File Offset: 0x00142C94
		private ObservableCollection<DataGridColumn> Columns
		{
			get
			{
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid != null)
				{
					return parentDataGrid.Columns;
				}
				return null;
			}
		}

		// Token: 0x17001181 RID: 4481
		// (get) Token: 0x0600477C RID: 18300 RVA: 0x00144AB4 File Offset: 0x00142CB4
		private DataGrid ParentDataGrid
		{
			get
			{
				if (this._parentDataGrid == null)
				{
					DataGridCellsPresenter dataGridCellsPresenter = this.ParentPresenter as DataGridCellsPresenter;
					if (dataGridCellsPresenter != null)
					{
						DataGridRow dataGridRowOwner = dataGridCellsPresenter.DataGridRowOwner;
						if (dataGridRowOwner != null)
						{
							this._parentDataGrid = dataGridRowOwner.DataGridOwner;
						}
					}
					else
					{
						DataGridColumnHeadersPresenter dataGridColumnHeadersPresenter = this.ParentPresenter as DataGridColumnHeadersPresenter;
						if (dataGridColumnHeadersPresenter != null)
						{
							this._parentDataGrid = dataGridColumnHeadersPresenter.ParentDataGrid;
						}
					}
				}
				return this._parentDataGrid;
			}
		}

		// Token: 0x17001182 RID: 4482
		// (get) Token: 0x0600477D RID: 18301 RVA: 0x00144B14 File Offset: 0x00142D14
		private ItemsControl ParentPresenter
		{
			get
			{
				FrameworkElement frameworkElement = base.TemplatedParent as FrameworkElement;
				if (frameworkElement != null)
				{
					return frameworkElement.TemplatedParent as ItemsControl;
				}
				return null;
			}
		}

		// Token: 0x04002956 RID: 10582
		private DataGrid _parentDataGrid;

		// Token: 0x04002957 RID: 10583
		private UIElement _clippedChildForFrozenBehaviour;

		// Token: 0x04002958 RID: 10584
		private RectangleGeometry _childClipForFrozenBehavior = new RectangleGeometry();

		// Token: 0x04002959 RID: 10585
		private List<UIElement> _realizedChildren;

		// Token: 0x02000966 RID: 2406
		private class ArrangeState
		{
			// Token: 0x06008741 RID: 34625 RVA: 0x0024F6F4 File Offset: 0x0024D8F4
			public ArrangeState()
			{
				this.FrozenColumnCount = 0;
				this.ChildHeight = 0.0;
				this.NextFrozenCellStart = 0.0;
				this.NextNonFrozenCellStart = 0.0;
				this.ViewportStartX = 0.0;
				this.DataGridHorizontalScrollStartX = 0.0;
				this.OldClippedChild = null;
				this.NewClippedChild = null;
			}

			// Token: 0x17001E8D RID: 7821
			// (get) Token: 0x06008742 RID: 34626 RVA: 0x0024F767 File Offset: 0x0024D967
			// (set) Token: 0x06008743 RID: 34627 RVA: 0x0024F76F File Offset: 0x0024D96F
			public int FrozenColumnCount { get; set; }

			// Token: 0x17001E8E RID: 7822
			// (get) Token: 0x06008744 RID: 34628 RVA: 0x0024F778 File Offset: 0x0024D978
			// (set) Token: 0x06008745 RID: 34629 RVA: 0x0024F780 File Offset: 0x0024D980
			public double ChildHeight { get; set; }

			// Token: 0x17001E8F RID: 7823
			// (get) Token: 0x06008746 RID: 34630 RVA: 0x0024F789 File Offset: 0x0024D989
			// (set) Token: 0x06008747 RID: 34631 RVA: 0x0024F791 File Offset: 0x0024D991
			public double NextFrozenCellStart { get; set; }

			// Token: 0x17001E90 RID: 7824
			// (get) Token: 0x06008748 RID: 34632 RVA: 0x0024F79A File Offset: 0x0024D99A
			// (set) Token: 0x06008749 RID: 34633 RVA: 0x0024F7A2 File Offset: 0x0024D9A2
			public double NextNonFrozenCellStart { get; set; }

			// Token: 0x17001E91 RID: 7825
			// (get) Token: 0x0600874A RID: 34634 RVA: 0x0024F7AB File Offset: 0x0024D9AB
			// (set) Token: 0x0600874B RID: 34635 RVA: 0x0024F7B3 File Offset: 0x0024D9B3
			public double ViewportStartX { get; set; }

			// Token: 0x17001E92 RID: 7826
			// (get) Token: 0x0600874C RID: 34636 RVA: 0x0024F7BC File Offset: 0x0024D9BC
			// (set) Token: 0x0600874D RID: 34637 RVA: 0x0024F7C4 File Offset: 0x0024D9C4
			public double DataGridHorizontalScrollStartX { get; set; }

			// Token: 0x17001E93 RID: 7827
			// (get) Token: 0x0600874E RID: 34638 RVA: 0x0024F7CD File Offset: 0x0024D9CD
			// (set) Token: 0x0600874F RID: 34639 RVA: 0x0024F7D5 File Offset: 0x0024D9D5
			public UIElement OldClippedChild { get; set; }

			// Token: 0x17001E94 RID: 7828
			// (get) Token: 0x06008750 RID: 34640 RVA: 0x0024F7DE File Offset: 0x0024D9DE
			// (set) Token: 0x06008751 RID: 34641 RVA: 0x0024F7E6 File Offset: 0x0024D9E6
			public UIElement NewClippedChild { get; set; }
		}
	}
}
