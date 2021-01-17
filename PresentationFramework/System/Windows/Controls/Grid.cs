using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Defines a flexible grid area that consists of columns and rows.</summary>
	// Token: 0x020004D4 RID: 1236
	public class Grid : Panel, IAddChild
	{
		// Token: 0x06004C41 RID: 19521 RVA: 0x001571CC File Offset: 0x001553CC
		static Grid()
		{
			ControlsTraceLogger.AddControl(TelemetryControls.Grid);
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Windows.Controls.Grid" />.</summary>
		// Token: 0x06004C42 RID: 19522 RVA: 0x001573C0 File Offset: 0x001555C0
		public Grid()
		{
			this.SetFlags((bool)Grid.ShowGridLinesProperty.GetDefaultValue(base.DependencyObjectType), Grid.Flags.ShowGridLinesPropertyValue);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value">An object to add as a child.</param>
		// Token: 0x06004C43 RID: 19523 RVA: 0x001573E8 File Offset: 0x001555E8
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			UIElement uielement = value as UIElement;
			if (uielement != null)
			{
				base.Children.Add(uielement);
				return;
			}
			throw new ArgumentException(SR.Get("Grid_UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(UIElement)
			}), "value");
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="text">A string to add to the object.</param>
		// Token: 0x06004C44 RID: 19524 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Gets an enumerator that can iterate the logical children of this <see cref="T:System.Windows.Controls.Grid" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" />. This property has no default value.</returns>
		// Token: 0x17001296 RID: 4758
		// (get) Token: 0x06004C45 RID: 19525 RVA: 0x0015744C File Offset: 0x0015564C
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				bool flag = base.VisualChildrenCount == 0 || base.IsItemsHost;
				if (flag)
				{
					Grid.ExtendedData extData = this.ExtData;
					if (extData == null || ((extData.ColumnDefinitions == null || extData.ColumnDefinitions.Count == 0) && (extData.RowDefinitions == null || extData.RowDefinitions.Count == 0)))
					{
						return EmptyEnumerator.Instance;
					}
				}
				return new Grid.GridChildrenCollectionEnumeratorSimple(this, !flag);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.Grid.Column" /> attached property to a given <see cref="T:System.Windows.UIElement" />. </summary>
		/// <param name="element">The element on which to set the <see cref="P:System.Windows.Controls.Grid.Column" /> attached property.</param>
		/// <param name="value">The property value to set.</param>
		// Token: 0x06004C46 RID: 19526 RVA: 0x001574B2 File Offset: 0x001556B2
		public static void SetColumn(UIElement element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Grid.ColumnProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.Grid.Column" /> attached property from a given <see cref="T:System.Windows.UIElement" />. </summary>
		/// <param name="element">The element from which to read the property value.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Controls.Grid.Column" /> attached property.</returns>
		// Token: 0x06004C47 RID: 19527 RVA: 0x001574D3 File Offset: 0x001556D3
		[AttachedPropertyBrowsableForChildren]
		public static int GetColumn(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Grid.ColumnProperty);
		}

		/// <summary> Sets the value of the <see cref="P:System.Windows.Controls.Grid.Row" /> attached property to a given <see cref="T:System.Windows.UIElement" />. </summary>
		/// <param name="element">The element on which to set the attached property.</param>
		/// <param name="value">The property value to set.</param>
		// Token: 0x06004C48 RID: 19528 RVA: 0x001574F3 File Offset: 0x001556F3
		public static void SetRow(UIElement element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Grid.RowProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.Grid.Row" /> attached property from a given <see cref="T:System.Windows.UIElement" />. </summary>
		/// <param name="element">The element from which to read the property value.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Controls.Grid.Row" /> attached property.</returns>
		// Token: 0x06004C49 RID: 19529 RVA: 0x00157514 File Offset: 0x00155714
		[AttachedPropertyBrowsableForChildren]
		public static int GetRow(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Grid.RowProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.Grid.ColumnSpan" /> attached property to a given <see cref="T:System.Windows.UIElement" />. </summary>
		/// <param name="element">The element on which to set the <see cref="P:System.Windows.Controls.Grid.ColumnSpan" /> attached property.</param>
		/// <param name="value">The property value to set.</param>
		// Token: 0x06004C4A RID: 19530 RVA: 0x00157534 File Offset: 0x00155734
		public static void SetColumnSpan(UIElement element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Grid.ColumnSpanProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.Grid.ColumnSpan" /> attached property from a given <see cref="T:System.Windows.UIElement" />.</summary>
		/// <param name="element">The element from which to read the property value.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Controls.Grid.ColumnSpan" /> attached property.</returns>
		// Token: 0x06004C4B RID: 19531 RVA: 0x00157555 File Offset: 0x00155755
		[AttachedPropertyBrowsableForChildren]
		public static int GetColumnSpan(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Grid.ColumnSpanProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.Grid.RowSpan" /> attached property to a given <see cref="T:System.Windows.UIElement" />. </summary>
		/// <param name="element">The element on which to set the <see cref="P:System.Windows.Controls.Grid.RowSpan" /> attached property.</param>
		/// <param name="value">The property value to set.</param>
		// Token: 0x06004C4C RID: 19532 RVA: 0x00157575 File Offset: 0x00155775
		public static void SetRowSpan(UIElement element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Grid.RowSpanProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.Grid.RowSpan" /> attached property from a given <see cref="T:System.Windows.UIElement" />. </summary>
		/// <param name="element">The element from which to read the  property value.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Controls.Grid.RowSpan" /> attached property.</returns>
		// Token: 0x06004C4D RID: 19533 RVA: 0x00157596 File Offset: 0x00155796
		[AttachedPropertyBrowsableForChildren]
		public static int GetRowSpan(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Grid.RowSpanProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.Grid.IsSharedSizeScope" /> attached property to a given <see cref="T:System.Windows.UIElement" />. </summary>
		/// <param name="element">The element on which to set the <see cref="P:System.Windows.Controls.Grid.IsSharedSizeScope" /> attached property.</param>
		/// <param name="value">The property value to set.</param>
		// Token: 0x06004C4E RID: 19534 RVA: 0x001575B6 File Offset: 0x001557B6
		public static void SetIsSharedSizeScope(UIElement element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Grid.IsSharedSizeScopeProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.Grid.IsSharedSizeScope" /> attached property from a given <see cref="T:System.Windows.UIElement" />. </summary>
		/// <param name="element">The element from which to read the property value.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Controls.Grid.IsSharedSizeScope" /> attached property.</returns>
		// Token: 0x06004C4F RID: 19535 RVA: 0x001575D2 File Offset: 0x001557D2
		public static bool GetIsSharedSizeScope(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Grid.IsSharedSizeScopeProperty);
		}

		/// <summary>Gets or sets a value that indicates whether grid lines are visible within this <see cref="T:System.Windows.Controls.Grid" />. </summary>
		/// <returns>
		///     <see langword="true" /> if grid lines are visible; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17001297 RID: 4759
		// (get) Token: 0x06004C50 RID: 19536 RVA: 0x001575F2 File Offset: 0x001557F2
		// (set) Token: 0x06004C51 RID: 19537 RVA: 0x001575FF File Offset: 0x001557FF
		public bool ShowGridLines
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.ShowGridLinesPropertyValue);
			}
			set
			{
				base.SetValue(Grid.ShowGridLinesProperty, value);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" /> defined on this instance of <see cref="T:System.Windows.Controls.Grid" />. </summary>
		/// <returns>A <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" /> defined on this instance of <see cref="T:System.Windows.Controls.Grid" /></returns>
		// Token: 0x17001298 RID: 4760
		// (get) Token: 0x06004C52 RID: 19538 RVA: 0x0015760D File Offset: 0x0015580D
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ColumnDefinitionCollection ColumnDefinitions
		{
			get
			{
				if (this._data == null)
				{
					this._data = new Grid.ExtendedData();
				}
				if (this._data.ColumnDefinitions == null)
				{
					this._data.ColumnDefinitions = new ColumnDefinitionCollection(this);
				}
				return this._data.ColumnDefinitions;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Controls.RowDefinitionCollection" /> defined on this instance of <see cref="T:System.Windows.Controls.Grid" />. </summary>
		/// <returns>A <see cref="T:System.Windows.Controls.RowDefinitionCollection" /> defined on this instance of <see cref="T:System.Windows.Controls.Grid" />.</returns>
		// Token: 0x17001299 RID: 4761
		// (get) Token: 0x06004C53 RID: 19539 RVA: 0x0015764B File Offset: 0x0015584B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public RowDefinitionCollection RowDefinitions
		{
			get
			{
				if (this._data == null)
				{
					this._data = new Grid.ExtendedData();
				}
				if (this._data.RowDefinitions == null)
				{
					this._data.RowDefinitions = new RowDefinitionCollection(this);
				}
				return this._data.RowDefinitions;
			}
		}

		/// <summary>Gets the child <see cref="T:System.Windows.Media.Visual" /> at the specified <paramref name="index" /> position.</summary>
		/// <param name="index">The zero-based index position of the desired <see cref="T:System.Windows.Media.Visual" />.</param>
		/// <returns>The child <see cref="T:System.Windows.Media.Visual" /> at the specified <paramref name="index" /> position.</returns>
		// Token: 0x06004C54 RID: 19540 RVA: 0x00157689 File Offset: 0x00155889
		protected override Visual GetVisualChild(int index)
		{
			if (index != base.VisualChildrenCount)
			{
				return base.GetVisualChild(index);
			}
			if (this._gridLinesRenderer == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._gridLinesRenderer;
		}

		/// <summary>Gets a value that represents the total number of <see cref="T:System.Windows.Media.Visual" /> children within this instance of <see cref="T:System.Windows.Controls.Grid" />.</summary>
		/// <returns>
		///     <see cref="T:System.Int32" /> that represents the total number of child <see cref="T:System.Windows.Media.Visual" /> objects. The default value is zero.</returns>
		// Token: 0x1700129A RID: 4762
		// (get) Token: 0x06004C55 RID: 19541 RVA: 0x001576C5 File Offset: 0x001558C5
		protected override int VisualChildrenCount
		{
			get
			{
				return base.VisualChildrenCount + ((this._gridLinesRenderer != null) ? 1 : 0);
			}
		}

		/// <summary>Measures the children of a <see cref="T:System.Windows.Controls.Grid" /> in anticipation of arranging them during the <see cref="M:System.Windows.Controls.Grid.ArrangeOverride(System.Windows.Size)" /> pass.</summary>
		/// <param name="constraint">Indicates an upper limit size that should not be exceeded.</param>
		/// <returns>
		///     <see cref="T:System.Windows.Size" /> that represents the required size to arrange child content.</returns>
		// Token: 0x06004C56 RID: 19542 RVA: 0x001576DC File Offset: 0x001558DC
		protected override Size MeasureOverride(Size constraint)
		{
			Grid.ExtendedData extData = this.ExtData;
			Size result;
			try
			{
				this.ListenToNotifications = true;
				this.MeasureOverrideInProgress = true;
				if (extData == null)
				{
					result = default(Size);
					UIElementCollection internalChildren = base.InternalChildren;
					int i = 0;
					int count = internalChildren.Count;
					while (i < count)
					{
						UIElement uielement = internalChildren[i];
						if (uielement != null)
						{
							uielement.Measure(constraint);
							result.Width = Math.Max(result.Width, uielement.DesiredSize.Width);
							result.Height = Math.Max(result.Height, uielement.DesiredSize.Height);
						}
						i++;
					}
				}
				else
				{
					bool flag = double.IsPositiveInfinity(constraint.Width);
					bool flag2 = double.IsPositiveInfinity(constraint.Height);
					if (this.RowDefinitionCollectionDirty || this.ColumnDefinitionCollectionDirty)
					{
						if (this._definitionIndices != null)
						{
							Array.Clear(this._definitionIndices, 0, this._definitionIndices.Length);
							this._definitionIndices = null;
						}
						if (base.UseLayoutRounding && this._roundingErrors != null)
						{
							Array.Clear(this._roundingErrors, 0, this._roundingErrors.Length);
							this._roundingErrors = null;
						}
					}
					this.ValidateDefinitionsUStructure();
					this.ValidateDefinitionsLayout(this.DefinitionsU, flag);
					this.ValidateDefinitionsVStructure();
					this.ValidateDefinitionsLayout(this.DefinitionsV, flag2);
					this.CellsStructureDirty |= (this.SizeToContentU != flag || this.SizeToContentV != flag2);
					this.SizeToContentU = flag;
					this.SizeToContentV = flag2;
					this.ValidateCells();
					this.MeasureCellsGroup(extData.CellGroup1, constraint, false, false);
					bool flag3 = !this.HasGroup3CellsInAutoRows;
					if (flag3)
					{
						if (this.HasStarCellsV)
						{
							this.ResolveStar(this.DefinitionsV, constraint.Height);
						}
						this.MeasureCellsGroup(extData.CellGroup2, constraint, false, false);
						if (this.HasStarCellsU)
						{
							this.ResolveStar(this.DefinitionsU, constraint.Width);
						}
						this.MeasureCellsGroup(extData.CellGroup3, constraint, false, false);
					}
					else
					{
						bool flag4 = extData.CellGroup2 > this.PrivateCells.Length;
						if (flag4)
						{
							if (this.HasStarCellsU)
							{
								this.ResolveStar(this.DefinitionsU, constraint.Width);
							}
							this.MeasureCellsGroup(extData.CellGroup3, constraint, false, false);
							if (this.HasStarCellsV)
							{
								this.ResolveStar(this.DefinitionsV, constraint.Height);
							}
						}
						else
						{
							bool flag5 = false;
							int num = 0;
							double[] minSizes = this.CacheMinSizes(extData.CellGroup2, false);
							double[] minSizes2 = this.CacheMinSizes(extData.CellGroup3, true);
							this.MeasureCellsGroup(extData.CellGroup2, constraint, false, true);
							do
							{
								if (flag5)
								{
									this.ApplyCachedMinSizes(minSizes2, true);
								}
								if (this.HasStarCellsU)
								{
									this.ResolveStar(this.DefinitionsU, constraint.Width);
								}
								this.MeasureCellsGroup(extData.CellGroup3, constraint, false, false);
								this.ApplyCachedMinSizes(minSizes, false);
								if (this.HasStarCellsV)
								{
									this.ResolveStar(this.DefinitionsV, constraint.Height);
								}
								this.MeasureCellsGroup(extData.CellGroup2, constraint, num == 5, false, out flag5);
							}
							while (flag5 && ++num <= 5);
						}
					}
					this.MeasureCellsGroup(extData.CellGroup4, constraint, false, false);
					result = new Size(this.CalculateDesiredSize(this.DefinitionsU), this.CalculateDesiredSize(this.DefinitionsV));
				}
			}
			finally
			{
				this.MeasureOverrideInProgress = false;
			}
			return result;
		}

		/// <summary>Arranges the content of a <see cref="T:System.Windows.Controls.Grid" /> element.</summary>
		/// <param name="arrangeSize">Specifies the size this <see cref="T:System.Windows.Controls.Grid" /> element should use to arrange its child elements.</param>
		/// <returns>
		///     <see cref="T:System.Windows.Size" /> that represents the arranged size of this Grid element and its children.</returns>
		// Token: 0x06004C57 RID: 19543 RVA: 0x00157A48 File Offset: 0x00155C48
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			try
			{
				this.ArrangeOverrideInProgress = true;
				if (this._data == null)
				{
					UIElementCollection internalChildren = base.InternalChildren;
					int i = 0;
					int count = internalChildren.Count;
					while (i < count)
					{
						UIElement uielement = internalChildren[i];
						if (uielement != null)
						{
							uielement.Arrange(new Rect(arrangeSize));
						}
						i++;
					}
				}
				else
				{
					this.SetFinalSize(this.DefinitionsU, arrangeSize.Width, true);
					this.SetFinalSize(this.DefinitionsV, arrangeSize.Height, false);
					UIElementCollection internalChildren2 = base.InternalChildren;
					for (int j = 0; j < this.PrivateCells.Length; j++)
					{
						UIElement uielement2 = internalChildren2[j];
						if (uielement2 != null)
						{
							int columnIndex = this.PrivateCells[j].ColumnIndex;
							int rowIndex = this.PrivateCells[j].RowIndex;
							int columnSpan = this.PrivateCells[j].ColumnSpan;
							int rowSpan = this.PrivateCells[j].RowSpan;
							Rect finalRect = new Rect((columnIndex == 0) ? 0.0 : this.DefinitionsU[columnIndex].FinalOffset, (rowIndex == 0) ? 0.0 : this.DefinitionsV[rowIndex].FinalOffset, this.GetFinalSizeForRange(this.DefinitionsU, columnIndex, columnSpan), this.GetFinalSizeForRange(this.DefinitionsV, rowIndex, rowSpan));
							uielement2.Arrange(finalRect);
						}
					}
					Grid.GridLinesRenderer gridLinesRenderer = this.EnsureGridLinesRenderer();
					if (gridLinesRenderer != null)
					{
						gridLinesRenderer.UpdateRenderBounds(arrangeSize);
					}
				}
			}
			finally
			{
				this.SetValid();
				this.ArrangeOverrideInProgress = false;
			}
			return arrangeSize;
		}

		/// <summary>Called when the visual children of a <see cref="T:System.Windows.Controls.Grid" /> element change.</summary>
		/// <param name="visualAdded">Identifies the visual child that's added.</param>
		/// <param name="visualRemoved">Identifies the visual child that's removed.</param>
		// Token: 0x06004C58 RID: 19544 RVA: 0x00157BF4 File Offset: 0x00155DF4
		protected internal override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			this.CellsStructureDirty = true;
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);
		}

		// Token: 0x06004C59 RID: 19545 RVA: 0x00157C05 File Offset: 0x00155E05
		internal void Invalidate()
		{
			this.CellsStructureDirty = true;
			base.InvalidateMeasure();
		}

		// Token: 0x06004C5A RID: 19546 RVA: 0x00157C14 File Offset: 0x00155E14
		internal double GetFinalColumnDefinitionWidth(int columnIndex)
		{
			double num = 0.0;
			Invariant.Assert(this._data != null);
			if (!this.ColumnDefinitionCollectionDirty)
			{
				DefinitionBase[] definitionsU = this.DefinitionsU;
				num = definitionsU[(columnIndex + 1) % definitionsU.Length].FinalOffset;
				if (columnIndex != 0)
				{
					num -= definitionsU[columnIndex].FinalOffset;
				}
			}
			return num;
		}

		// Token: 0x06004C5B RID: 19547 RVA: 0x00157C68 File Offset: 0x00155E68
		internal double GetFinalRowDefinitionHeight(int rowIndex)
		{
			double num = 0.0;
			Invariant.Assert(this._data != null);
			if (!this.RowDefinitionCollectionDirty)
			{
				DefinitionBase[] definitionsV = this.DefinitionsV;
				num = definitionsV[(rowIndex + 1) % definitionsV.Length].FinalOffset;
				if (rowIndex != 0)
				{
					num -= definitionsV[rowIndex].FinalOffset;
				}
			}
			return num;
		}

		// Token: 0x1700129B RID: 4763
		// (get) Token: 0x06004C5C RID: 19548 RVA: 0x00157CBA File Offset: 0x00155EBA
		// (set) Token: 0x06004C5D RID: 19549 RVA: 0x00157CC7 File Offset: 0x00155EC7
		internal bool MeasureOverrideInProgress
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.MeasureOverrideInProgress);
			}
			set
			{
				this.SetFlags(value, Grid.Flags.MeasureOverrideInProgress);
			}
		}

		// Token: 0x1700129C RID: 4764
		// (get) Token: 0x06004C5E RID: 19550 RVA: 0x00157CD5 File Offset: 0x00155ED5
		// (set) Token: 0x06004C5F RID: 19551 RVA: 0x00157CE2 File Offset: 0x00155EE2
		internal bool ArrangeOverrideInProgress
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.ArrangeOverrideInProgress);
			}
			set
			{
				this.SetFlags(value, Grid.Flags.ArrangeOverrideInProgress);
			}
		}

		// Token: 0x1700129D RID: 4765
		// (get) Token: 0x06004C60 RID: 19552 RVA: 0x00157CF0 File Offset: 0x00155EF0
		// (set) Token: 0x06004C61 RID: 19553 RVA: 0x00157CFC File Offset: 0x00155EFC
		internal bool ColumnDefinitionCollectionDirty
		{
			get
			{
				return !this.CheckFlagsAnd(Grid.Flags.ValidDefinitionsUStructure);
			}
			set
			{
				this.SetFlags(!value, Grid.Flags.ValidDefinitionsUStructure);
			}
		}

		// Token: 0x1700129E RID: 4766
		// (get) Token: 0x06004C62 RID: 19554 RVA: 0x00157D09 File Offset: 0x00155F09
		// (set) Token: 0x06004C63 RID: 19555 RVA: 0x00157D15 File Offset: 0x00155F15
		internal bool RowDefinitionCollectionDirty
		{
			get
			{
				return !this.CheckFlagsAnd(Grid.Flags.ValidDefinitionsVStructure);
			}
			set
			{
				this.SetFlags(!value, Grid.Flags.ValidDefinitionsVStructure);
			}
		}

		// Token: 0x06004C64 RID: 19556 RVA: 0x00157D22 File Offset: 0x00155F22
		private void ValidateCells()
		{
			if (this.CellsStructureDirty)
			{
				this.ValidateCellsCore();
				this.CellsStructureDirty = false;
			}
		}

		// Token: 0x06004C65 RID: 19557 RVA: 0x00157D3C File Offset: 0x00155F3C
		private void ValidateCellsCore()
		{
			UIElementCollection internalChildren = base.InternalChildren;
			Grid.ExtendedData extData = this.ExtData;
			extData.CellCachesCollection = new Grid.CellCache[internalChildren.Count];
			extData.CellGroup1 = int.MaxValue;
			extData.CellGroup2 = int.MaxValue;
			extData.CellGroup3 = int.MaxValue;
			extData.CellGroup4 = int.MaxValue;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int i = this.PrivateCells.Length - 1; i >= 0; i--)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					Grid.CellCache cellCache = default(Grid.CellCache);
					cellCache.ColumnIndex = Math.Min(Grid.GetColumn(uielement), this.DefinitionsU.Length - 1);
					cellCache.RowIndex = Math.Min(Grid.GetRow(uielement), this.DefinitionsV.Length - 1);
					cellCache.ColumnSpan = Math.Min(Grid.GetColumnSpan(uielement), this.DefinitionsU.Length - cellCache.ColumnIndex);
					cellCache.RowSpan = Math.Min(Grid.GetRowSpan(uielement), this.DefinitionsV.Length - cellCache.RowIndex);
					cellCache.SizeTypeU = this.GetLengthTypeForRange(this.DefinitionsU, cellCache.ColumnIndex, cellCache.ColumnSpan);
					cellCache.SizeTypeV = this.GetLengthTypeForRange(this.DefinitionsV, cellCache.RowIndex, cellCache.RowSpan);
					flag |= cellCache.IsStarU;
					flag2 |= cellCache.IsStarV;
					if (!cellCache.IsStarV)
					{
						if (!cellCache.IsStarU)
						{
							cellCache.Next = extData.CellGroup1;
							extData.CellGroup1 = i;
						}
						else
						{
							cellCache.Next = extData.CellGroup3;
							extData.CellGroup3 = i;
							flag3 |= cellCache.IsAutoV;
						}
					}
					else if (cellCache.IsAutoU && !cellCache.IsStarU)
					{
						cellCache.Next = extData.CellGroup2;
						extData.CellGroup2 = i;
					}
					else
					{
						cellCache.Next = extData.CellGroup4;
						extData.CellGroup4 = i;
					}
					this.PrivateCells[i] = cellCache;
				}
			}
			this.HasStarCellsU = flag;
			this.HasStarCellsV = flag2;
			this.HasGroup3CellsInAutoRows = flag3;
		}

		// Token: 0x06004C66 RID: 19558 RVA: 0x00157F58 File Offset: 0x00156158
		private void ValidateDefinitionsUStructure()
		{
			if (this.ColumnDefinitionCollectionDirty)
			{
				Grid.ExtendedData extData = this.ExtData;
				if (extData.ColumnDefinitions == null)
				{
					if (extData.DefinitionsU == null)
					{
						extData.DefinitionsU = new DefinitionBase[]
						{
							new ColumnDefinition()
						};
					}
				}
				else
				{
					extData.ColumnDefinitions.InternalTrimToSize();
					if (extData.ColumnDefinitions.InternalCount == 0)
					{
						extData.DefinitionsU = new DefinitionBase[]
						{
							new ColumnDefinition()
						};
					}
					else
					{
						extData.DefinitionsU = extData.ColumnDefinitions.InternalItems;
					}
				}
				this.ColumnDefinitionCollectionDirty = false;
			}
		}

		// Token: 0x06004C67 RID: 19559 RVA: 0x00157FE0 File Offset: 0x001561E0
		private void ValidateDefinitionsVStructure()
		{
			if (this.RowDefinitionCollectionDirty)
			{
				Grid.ExtendedData extData = this.ExtData;
				if (extData.RowDefinitions == null)
				{
					if (extData.DefinitionsV == null)
					{
						extData.DefinitionsV = new DefinitionBase[]
						{
							new RowDefinition()
						};
					}
				}
				else
				{
					extData.RowDefinitions.InternalTrimToSize();
					if (extData.RowDefinitions.InternalCount == 0)
					{
						extData.DefinitionsV = new DefinitionBase[]
						{
							new RowDefinition()
						};
					}
					else
					{
						extData.DefinitionsV = extData.RowDefinitions.InternalItems;
					}
				}
				this.RowDefinitionCollectionDirty = false;
			}
		}

		// Token: 0x06004C68 RID: 19560 RVA: 0x00158068 File Offset: 0x00156268
		private void ValidateDefinitionsLayout(DefinitionBase[] definitions, bool treatStarAsAuto)
		{
			for (int i = 0; i < definitions.Length; i++)
			{
				definitions[i].OnBeforeLayout(this);
				double num = definitions[i].UserMinSize;
				double userMaxSize = definitions[i].UserMaxSize;
				double val = 0.0;
				switch (definitions[i].UserSize.GridUnitType)
				{
				case GridUnitType.Auto:
					definitions[i].SizeType = Grid.LayoutTimeSizeType.Auto;
					val = double.PositiveInfinity;
					break;
				case GridUnitType.Pixel:
					definitions[i].SizeType = Grid.LayoutTimeSizeType.Pixel;
					val = definitions[i].UserSize.Value;
					num = Math.Max(num, Math.Min(val, userMaxSize));
					break;
				case GridUnitType.Star:
					if (treatStarAsAuto)
					{
						definitions[i].SizeType = Grid.LayoutTimeSizeType.Auto;
						val = double.PositiveInfinity;
					}
					else
					{
						definitions[i].SizeType = Grid.LayoutTimeSizeType.Star;
						val = double.PositiveInfinity;
					}
					break;
				}
				definitions[i].UpdateMinSize(num);
				definitions[i].MeasureSize = Math.Max(num, Math.Min(val, userMaxSize));
			}
		}

		// Token: 0x06004C69 RID: 19561 RVA: 0x00158160 File Offset: 0x00156360
		private double[] CacheMinSizes(int cellsHead, bool isRows)
		{
			double[] array = isRows ? new double[this.DefinitionsV.Length] : new double[this.DefinitionsU.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = -1.0;
			}
			bool sharedSizeGroupDoesRedundantLayout = FrameworkAppContextSwitches.SharedSizeGroupDoesRedundantLayout;
			int num = cellsHead;
			do
			{
				if (isRows)
				{
					if (sharedSizeGroupDoesRedundantLayout)
					{
						array[this.PrivateCells[num].RowIndex] = this.DefinitionsV[this.PrivateCells[num].RowIndex].MinSize;
					}
					else
					{
						array[this.PrivateCells[num].RowIndex] = this.DefinitionsV[this.PrivateCells[num].RowIndex].RawMinSize;
					}
				}
				else if (sharedSizeGroupDoesRedundantLayout)
				{
					array[this.PrivateCells[num].ColumnIndex] = this.DefinitionsU[this.PrivateCells[num].ColumnIndex].MinSize;
				}
				else
				{
					array[this.PrivateCells[num].ColumnIndex] = this.DefinitionsU[this.PrivateCells[num].ColumnIndex].RawMinSize;
				}
				num = this.PrivateCells[num].Next;
			}
			while (num < this.PrivateCells.Length);
			return array;
		}

		// Token: 0x06004C6A RID: 19562 RVA: 0x001582A4 File Offset: 0x001564A4
		private void ApplyCachedMinSizes(double[] minSizes, bool isRows)
		{
			for (int i = 0; i < minSizes.Length; i++)
			{
				if (DoubleUtil.GreaterThanOrClose(minSizes[i], 0.0))
				{
					if (isRows)
					{
						this.DefinitionsV[i].SetMinSize(minSizes[i]);
					}
					else
					{
						this.DefinitionsU[i].SetMinSize(minSizes[i]);
					}
				}
			}
		}

		// Token: 0x06004C6B RID: 19563 RVA: 0x001582F8 File Offset: 0x001564F8
		private void MeasureCellsGroup(int cellsHead, Size referenceSize, bool ignoreDesiredSizeU, bool forceInfinityV)
		{
			bool flag;
			this.MeasureCellsGroup(cellsHead, referenceSize, ignoreDesiredSizeU, forceInfinityV, out flag);
		}

		// Token: 0x06004C6C RID: 19564 RVA: 0x00158314 File Offset: 0x00156514
		private void MeasureCellsGroup(int cellsHead, Size referenceSize, bool ignoreDesiredSizeU, bool forceInfinityV, out bool hasDesiredSizeUChanged)
		{
			hasDesiredSizeUChanged = false;
			if (cellsHead >= this.PrivateCells.Length)
			{
				return;
			}
			UIElementCollection internalChildren = base.InternalChildren;
			Hashtable hashtable = null;
			int num = cellsHead;
			do
			{
				double width = internalChildren[num].DesiredSize.Width;
				this.MeasureCell(num, forceInfinityV);
				hasDesiredSizeUChanged |= !DoubleUtil.AreClose(width, internalChildren[num].DesiredSize.Width);
				if (!ignoreDesiredSizeU)
				{
					if (this.PrivateCells[num].ColumnSpan == 1)
					{
						this.DefinitionsU[this.PrivateCells[num].ColumnIndex].UpdateMinSize(Math.Min(internalChildren[num].DesiredSize.Width, this.DefinitionsU[this.PrivateCells[num].ColumnIndex].UserMaxSize));
					}
					else
					{
						Grid.RegisterSpan(ref hashtable, this.PrivateCells[num].ColumnIndex, this.PrivateCells[num].ColumnSpan, true, internalChildren[num].DesiredSize.Width);
					}
				}
				if (!forceInfinityV)
				{
					if (this.PrivateCells[num].RowSpan == 1)
					{
						this.DefinitionsV[this.PrivateCells[num].RowIndex].UpdateMinSize(Math.Min(internalChildren[num].DesiredSize.Height, this.DefinitionsV[this.PrivateCells[num].RowIndex].UserMaxSize));
					}
					else
					{
						Grid.RegisterSpan(ref hashtable, this.PrivateCells[num].RowIndex, this.PrivateCells[num].RowSpan, false, internalChildren[num].DesiredSize.Height);
					}
				}
				num = this.PrivateCells[num].Next;
			}
			while (num < this.PrivateCells.Length);
			if (hashtable != null)
			{
				foreach (object obj in hashtable)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					Grid.SpanKey spanKey = (Grid.SpanKey)dictionaryEntry.Key;
					double requestedSize = (double)dictionaryEntry.Value;
					this.EnsureMinSizeInDefinitionRange(spanKey.U ? this.DefinitionsU : this.DefinitionsV, spanKey.Start, spanKey.Count, requestedSize, spanKey.U ? referenceSize.Width : referenceSize.Height);
				}
			}
		}

		// Token: 0x06004C6D RID: 19565 RVA: 0x001585B4 File Offset: 0x001567B4
		private static void RegisterSpan(ref Hashtable store, int start, int count, bool u, double value)
		{
			if (store == null)
			{
				store = new Hashtable();
			}
			Grid.SpanKey key = new Grid.SpanKey(start, count, u);
			object obj = store[key];
			if (obj == null || value > (double)obj)
			{
				store[key] = value;
			}
		}

		// Token: 0x06004C6E RID: 19566 RVA: 0x001585FC File Offset: 0x001567FC
		private void MeasureCell(int cell, bool forceInfinityV)
		{
			double width;
			if (this.PrivateCells[cell].IsAutoU && !this.PrivateCells[cell].IsStarU)
			{
				width = double.PositiveInfinity;
			}
			else
			{
				width = this.GetMeasureSizeForRange(this.DefinitionsU, this.PrivateCells[cell].ColumnIndex, this.PrivateCells[cell].ColumnSpan);
			}
			double height;
			if (forceInfinityV)
			{
				height = double.PositiveInfinity;
			}
			else if (this.PrivateCells[cell].IsAutoV && !this.PrivateCells[cell].IsStarV)
			{
				height = double.PositiveInfinity;
			}
			else
			{
				height = this.GetMeasureSizeForRange(this.DefinitionsV, this.PrivateCells[cell].RowIndex, this.PrivateCells[cell].RowSpan);
			}
			UIElement uielement = base.InternalChildren[cell];
			if (uielement != null)
			{
				Size availableSize = new Size(width, height);
				uielement.Measure(availableSize);
			}
		}

		// Token: 0x06004C6F RID: 19567 RVA: 0x001586FC File Offset: 0x001568FC
		private double GetMeasureSizeForRange(DefinitionBase[] definitions, int start, int count)
		{
			double num = 0.0;
			int num2 = start + count - 1;
			do
			{
				num += ((definitions[num2].SizeType == Grid.LayoutTimeSizeType.Auto) ? definitions[num2].MinSize : definitions[num2].MeasureSize);
			}
			while (--num2 >= start);
			return num;
		}

		// Token: 0x06004C70 RID: 19568 RVA: 0x00158744 File Offset: 0x00156944
		private Grid.LayoutTimeSizeType GetLengthTypeForRange(DefinitionBase[] definitions, int start, int count)
		{
			Grid.LayoutTimeSizeType layoutTimeSizeType = Grid.LayoutTimeSizeType.None;
			int num = start + count - 1;
			do
			{
				layoutTimeSizeType |= definitions[num].SizeType;
			}
			while (--num >= start);
			return layoutTimeSizeType;
		}

		// Token: 0x06004C71 RID: 19569 RVA: 0x00158770 File Offset: 0x00156970
		private void EnsureMinSizeInDefinitionRange(DefinitionBase[] definitions, int start, int count, double requestedSize, double percentReferenceSize)
		{
			if (!Grid._IsZero(requestedSize))
			{
				DefinitionBase[] tempDefinitions = this.TempDefinitions;
				int num = start + count;
				int num2 = 0;
				double num3 = 0.0;
				double num4 = 0.0;
				double num5 = 0.0;
				double num6 = 0.0;
				for (int i = start; i < num; i++)
				{
					double minSize = definitions[i].MinSize;
					double preferredSize = definitions[i].PreferredSize;
					double num7 = Math.Max(definitions[i].UserMaxSize, minSize);
					num3 += minSize;
					num4 += preferredSize;
					num5 += num7;
					definitions[i].SizeCache = num7;
					if (num6 < num7)
					{
						num6 = num7;
					}
					if (definitions[i].UserSize.IsAuto)
					{
						num2++;
					}
					tempDefinitions[i - start] = definitions[i];
				}
				if (requestedSize > num3)
				{
					if (requestedSize <= num4)
					{
						Array.Sort(tempDefinitions, 0, count, Grid.s_spanPreferredDistributionOrderComparer);
						int j = 0;
						double num8 = requestedSize;
						while (j < num2)
						{
							num8 -= tempDefinitions[j].MinSize;
							j++;
						}
						while (j < count)
						{
							double num9 = Math.Min(num8 / (double)(count - j), tempDefinitions[j].PreferredSize);
							if (num9 > tempDefinitions[j].MinSize)
							{
								tempDefinitions[j].UpdateMinSize(num9);
							}
							num8 -= num9;
							j++;
						}
						return;
					}
					if (requestedSize <= num5)
					{
						Array.Sort(tempDefinitions, 0, count, Grid.s_spanMaxDistributionOrderComparer);
						int k = 0;
						double num10 = requestedSize - num4;
						while (k < count - num2)
						{
							double preferredSize2 = tempDefinitions[k].PreferredSize;
							double val = preferredSize2 + num10 / (double)(count - num2 - k);
							tempDefinitions[k].UpdateMinSize(Math.Min(val, tempDefinitions[k].SizeCache));
							num10 -= tempDefinitions[k].MinSize - preferredSize2;
							k++;
						}
						while (k < count)
						{
							double minSize2 = tempDefinitions[k].MinSize;
							double val2 = minSize2 + num10 / (double)(count - k);
							tempDefinitions[k].UpdateMinSize(Math.Min(val2, tempDefinitions[k].SizeCache));
							num10 -= tempDefinitions[k].MinSize - minSize2;
							k++;
						}
						return;
					}
					double num11 = requestedSize / (double)count;
					if (num11 < num6 && !Grid._AreClose(num11, num6))
					{
						double num12 = num6 * (double)count - num5;
						double num13 = requestedSize - num5;
						for (int l = 0; l < count; l++)
						{
							double num14 = (num6 - tempDefinitions[l].SizeCache) * num13 / num12;
							tempDefinitions[l].UpdateMinSize(tempDefinitions[l].SizeCache + num14);
						}
						return;
					}
					for (int m = 0; m < count; m++)
					{
						tempDefinitions[m].UpdateMinSize(num11);
					}
				}
			}
		}

		// Token: 0x06004C72 RID: 19570 RVA: 0x00158A06 File Offset: 0x00156C06
		private void ResolveStar(DefinitionBase[] definitions, double availableSize)
		{
			if (FrameworkAppContextSwitches.GridStarDefinitionsCanExceedAvailableSpace)
			{
				this.ResolveStarLegacy(definitions, availableSize);
				return;
			}
			this.ResolveStarMaxDiscrepancy(definitions, availableSize);
		}

		// Token: 0x06004C73 RID: 19571 RVA: 0x00158A20 File Offset: 0x00156C20
		private void ResolveStarLegacy(DefinitionBase[] definitions, double availableSize)
		{
			DefinitionBase[] tempDefinitions = this.TempDefinitions;
			int num = 0;
			double num2 = 0.0;
			for (int i = 0; i < definitions.Length; i++)
			{
				switch (definitions[i].SizeType)
				{
				case Grid.LayoutTimeSizeType.Pixel:
					num2 += definitions[i].MeasureSize;
					break;
				case Grid.LayoutTimeSizeType.Auto:
					num2 += definitions[i].MinSize;
					break;
				case Grid.LayoutTimeSizeType.Star:
				{
					tempDefinitions[num++] = definitions[i];
					double num3 = definitions[i].UserSize.Value;
					if (Grid._IsZero(num3))
					{
						definitions[i].MeasureSize = 0.0;
						definitions[i].SizeCache = 0.0;
					}
					else
					{
						num3 = Math.Min(num3, 1E+298);
						definitions[i].MeasureSize = num3;
						double num4 = Math.Max(definitions[i].MinSize, definitions[i].UserMaxSize);
						num4 = Math.Min(num4, 1E+298);
						definitions[i].SizeCache = num4 / num3;
					}
					break;
				}
				}
			}
			if (num > 0)
			{
				Array.Sort(tempDefinitions, 0, num, Grid.s_starDistributionOrderComparer);
				double num5 = 0.0;
				int num6 = num - 1;
				do
				{
					num5 += tempDefinitions[num6].MeasureSize;
					tempDefinitions[num6].SizeCache = num5;
				}
				while (--num6 >= 0);
				num6 = 0;
				do
				{
					double measureSize = tempDefinitions[num6].MeasureSize;
					double num7;
					if (Grid._IsZero(measureSize))
					{
						num7 = tempDefinitions[num6].MinSize;
					}
					else
					{
						double val = Math.Max(availableSize - num2, 0.0) * (measureSize / tempDefinitions[num6].SizeCache);
						num7 = Math.Min(val, tempDefinitions[num6].UserMaxSize);
						num7 = Math.Max(tempDefinitions[num6].MinSize, num7);
					}
					tempDefinitions[num6].MeasureSize = num7;
					num2 += num7;
				}
				while (++num6 < num);
			}
		}

		// Token: 0x06004C74 RID: 19572 RVA: 0x00158C04 File Offset: 0x00156E04
		private void ResolveStarMaxDiscrepancy(DefinitionBase[] definitions, double availableSize)
		{
			int num = definitions.Length;
			DefinitionBase[] tempDefinitions = this.TempDefinitions;
			double num2 = 0.0;
			int num3 = 0;
			double scale = 1.0;
			double num4 = 0.0;
			for (int i = 0; i < num; i++)
			{
				DefinitionBase definitionBase = definitions[i];
				if (definitionBase.SizeType == Grid.LayoutTimeSizeType.Star)
				{
					num3++;
					definitionBase.MeasureSize = 1.0;
					if (definitionBase.UserSize.Value > num4)
					{
						num4 = definitionBase.UserSize.Value;
					}
				}
			}
			if (double.IsPositiveInfinity(num4))
			{
				scale = -1.0;
			}
			else if (num3 > 0)
			{
				double num5 = Math.Floor(Math.Log(double.MaxValue / num4 / (double)num3, 2.0));
				if (num5 < 0.0)
				{
					scale = Math.Pow(2.0, num5 - 4.0);
				}
			}
			bool flag = true;
			while (flag)
			{
				double num6 = 0.0;
				num2 = 0.0;
				int j;
				int num7 = j = 0;
				for (int k = 0; k < num; k++)
				{
					DefinitionBase definitionBase2 = definitions[k];
					switch (definitionBase2.SizeType)
					{
					case Grid.LayoutTimeSizeType.Pixel:
						num2 += definitionBase2.MeasureSize;
						break;
					case Grid.LayoutTimeSizeType.Auto:
						num2 += definitions[k].MinSize;
						break;
					case Grid.LayoutTimeSizeType.Star:
						if (definitionBase2.MeasureSize < 0.0)
						{
							num2 += -definitionBase2.MeasureSize;
						}
						else
						{
							double num8 = Grid.StarWeight(definitionBase2, scale);
							num6 += num8;
							if (definitionBase2.MinSize > 0.0)
							{
								tempDefinitions[j++] = definitionBase2;
								definitionBase2.MeasureSize = num8 / definitionBase2.MinSize;
							}
							double num9 = Math.Max(definitionBase2.MinSize, definitionBase2.UserMaxSize);
							if (!double.IsPositiveInfinity(num9))
							{
								tempDefinitions[num + num7++] = definitionBase2;
								definitionBase2.SizeCache = num8 / num9;
							}
						}
						break;
					}
				}
				int num10 = j;
				int num11 = num7;
				double num12 = 0.0;
				double num13 = availableSize - num2;
				double num14 = num6 - num12;
				Array.Sort(tempDefinitions, 0, j, Grid.s_minRatioComparer);
				Array.Sort(tempDefinitions, num, num7, Grid.s_maxRatioComparer);
				while (j + num7 > 0 && num13 > 0.0)
				{
					if (num14 < num6 * 0.00390625)
					{
						num12 = 0.0;
						num6 = 0.0;
						for (int l = 0; l < num; l++)
						{
							DefinitionBase definitionBase3 = definitions[l];
							if (definitionBase3.SizeType == Grid.LayoutTimeSizeType.Star && definitionBase3.MeasureSize > 0.0)
							{
								num6 += Grid.StarWeight(definitionBase3, scale);
							}
						}
						num14 = num6 - num12;
					}
					double minRatio = (j > 0) ? tempDefinitions[j - 1].MeasureSize : double.PositiveInfinity;
					double maxRatio = (num7 > 0) ? tempDefinitions[num + num7 - 1].SizeCache : -1.0;
					double proportion = num14 / num13;
					bool? flag2 = Grid.Choose(minRatio, maxRatio, proportion);
					if (flag2 == null)
					{
						break;
					}
					DefinitionBase definitionBase4;
					double num15;
					if (flag2 == true)
					{
						definitionBase4 = tempDefinitions[j - 1];
						num15 = definitionBase4.MinSize;
						j--;
					}
					else
					{
						definitionBase4 = tempDefinitions[num + num7 - 1];
						num15 = Math.Max(definitionBase4.MinSize, definitionBase4.UserMaxSize);
						num7--;
					}
					num2 += num15;
					definitionBase4.MeasureSize = -num15;
					num12 += Grid.StarWeight(definitionBase4, scale);
					num3--;
					num13 = availableSize - num2;
					num14 = num6 - num12;
					while (j > 0)
					{
						if (tempDefinitions[j - 1].MeasureSize >= 0.0)
						{
							break;
						}
						j--;
						tempDefinitions[j] = null;
					}
					while (num7 > 0 && tempDefinitions[num + num7 - 1].MeasureSize < 0.0)
					{
						num7--;
						tempDefinitions[num + num7] = null;
					}
				}
				flag = false;
				if (num3 == 0 && num2 < availableSize)
				{
					for (int m = j; m < num10; m++)
					{
						DefinitionBase definitionBase5 = tempDefinitions[m];
						if (definitionBase5 != null)
						{
							definitionBase5.MeasureSize = 1.0;
							num3++;
							flag = true;
						}
					}
				}
				if (num2 > availableSize)
				{
					for (int n = num7; n < num11; n++)
					{
						DefinitionBase definitionBase6 = tempDefinitions[num + n];
						if (definitionBase6 != null)
						{
							definitionBase6.MeasureSize = 1.0;
							num3++;
							flag = true;
						}
					}
				}
			}
			num3 = 0;
			for (int num16 = 0; num16 < num; num16++)
			{
				DefinitionBase definitionBase7 = definitions[num16];
				if (definitionBase7.SizeType == Grid.LayoutTimeSizeType.Star)
				{
					if (definitionBase7.MeasureSize < 0.0)
					{
						definitionBase7.MeasureSize = -definitionBase7.MeasureSize;
					}
					else
					{
						tempDefinitions[num3++] = definitionBase7;
						definitionBase7.MeasureSize = Grid.StarWeight(definitionBase7, scale);
					}
				}
			}
			if (num3 > 0)
			{
				Array.Sort(tempDefinitions, 0, num3, Grid.s_starWeightComparer);
				double num6 = 0.0;
				for (int num17 = 0; num17 < num3; num17++)
				{
					DefinitionBase definitionBase8 = tempDefinitions[num17];
					num6 += definitionBase8.MeasureSize;
					definitionBase8.SizeCache = num6;
				}
				for (int num18 = num3 - 1; num18 >= 0; num18--)
				{
					DefinitionBase definitionBase9 = tempDefinitions[num18];
					double num19 = (definitionBase9.MeasureSize > 0.0) ? (Math.Max(availableSize - num2, 0.0) * (definitionBase9.MeasureSize / definitionBase9.SizeCache)) : 0.0;
					num19 = Math.Min(num19, definitionBase9.UserMaxSize);
					num19 = Math.Max(definitionBase9.MinSize, num19);
					definitionBase9.MeasureSize = num19;
					num2 += num19;
				}
			}
		}

		// Token: 0x06004C75 RID: 19573 RVA: 0x001591D8 File Offset: 0x001573D8
		private double CalculateDesiredSize(DefinitionBase[] definitions)
		{
			double num = 0.0;
			for (int i = 0; i < definitions.Length; i++)
			{
				num += definitions[i].MinSize;
			}
			return num;
		}

		// Token: 0x06004C76 RID: 19574 RVA: 0x00159209 File Offset: 0x00157409
		private void SetFinalSize(DefinitionBase[] definitions, double finalSize, bool columns)
		{
			if (FrameworkAppContextSwitches.GridStarDefinitionsCanExceedAvailableSpace)
			{
				this.SetFinalSizeLegacy(definitions, finalSize, columns);
				return;
			}
			this.SetFinalSizeMaxDiscrepancy(definitions, finalSize, columns);
		}

		// Token: 0x06004C77 RID: 19575 RVA: 0x00159228 File Offset: 0x00157428
		private void SetFinalSizeLegacy(DefinitionBase[] definitions, double finalSize, bool columns)
		{
			int num = 0;
			int num2 = definitions.Length;
			double num3 = 0.0;
			bool useLayoutRounding = base.UseLayoutRounding;
			int[] definitionIndices = this.DefinitionIndices;
			double[] array = null;
			double dpiScale = 1.0;
			if (useLayoutRounding)
			{
				DpiScale dpi = base.GetDpi();
				dpiScale = (columns ? dpi.DpiScaleX : dpi.DpiScaleY);
				array = this.RoundingErrors;
			}
			for (int i = 0; i < definitions.Length; i++)
			{
				if (definitions[i].UserSize.IsStar)
				{
					double num4 = definitions[i].UserSize.Value;
					if (Grid._IsZero(num4))
					{
						definitions[i].MeasureSize = 0.0;
						definitions[i].SizeCache = 0.0;
					}
					else
					{
						num4 = Math.Min(num4, 1E+298);
						definitions[i].MeasureSize = num4;
						double num5 = Math.Max(definitions[i].MinSizeForArrange, definitions[i].UserMaxSize);
						num5 = Math.Min(num5, 1E+298);
						definitions[i].SizeCache = num5 / num4;
						if (useLayoutRounding)
						{
							array[i] = definitions[i].SizeCache;
							definitions[i].SizeCache = UIElement.RoundLayoutValue(definitions[i].SizeCache, dpiScale);
						}
					}
					definitionIndices[num++] = i;
				}
				else
				{
					double num6 = 0.0;
					GridUnitType gridUnitType = definitions[i].UserSize.GridUnitType;
					if (gridUnitType != GridUnitType.Auto)
					{
						if (gridUnitType == GridUnitType.Pixel)
						{
							num6 = definitions[i].UserSize.Value;
						}
					}
					else
					{
						num6 = definitions[i].MinSizeForArrange;
					}
					double val;
					if (definitions[i].IsShared)
					{
						val = num6;
					}
					else
					{
						val = definitions[i].UserMaxSize;
					}
					definitions[i].SizeCache = Math.Max(definitions[i].MinSizeForArrange, Math.Min(num6, val));
					if (useLayoutRounding)
					{
						array[i] = definitions[i].SizeCache;
						definitions[i].SizeCache = UIElement.RoundLayoutValue(definitions[i].SizeCache, dpiScale);
					}
					num3 += definitions[i].SizeCache;
					definitionIndices[--num2] = i;
				}
			}
			if (num > 0)
			{
				Grid.StarDistributionOrderIndexComparer comparer = new Grid.StarDistributionOrderIndexComparer(definitions);
				Array.Sort(definitionIndices, 0, num, comparer);
				double num7 = 0.0;
				int num8 = num - 1;
				do
				{
					num7 += definitions[definitionIndices[num8]].MeasureSize;
					definitions[definitionIndices[num8]].SizeCache = num7;
				}
				while (--num8 >= 0);
				num8 = 0;
				do
				{
					double measureSize = definitions[definitionIndices[num8]].MeasureSize;
					double num9;
					if (Grid._IsZero(measureSize))
					{
						num9 = definitions[definitionIndices[num8]].MinSizeForArrange;
					}
					else
					{
						double val2 = Math.Max(finalSize - num3, 0.0) * (measureSize / definitions[definitionIndices[num8]].SizeCache);
						num9 = Math.Min(val2, definitions[definitionIndices[num8]].UserMaxSize);
						num9 = Math.Max(definitions[definitionIndices[num8]].MinSizeForArrange, num9);
					}
					definitions[definitionIndices[num8]].SizeCache = num9;
					if (useLayoutRounding)
					{
						array[definitionIndices[num8]] = definitions[definitionIndices[num8]].SizeCache;
						definitions[definitionIndices[num8]].SizeCache = UIElement.RoundLayoutValue(definitions[definitionIndices[num8]].SizeCache, dpiScale);
					}
					num3 += definitions[definitionIndices[num8]].SizeCache;
				}
				while (++num8 < num);
			}
			if (num3 > finalSize && !Grid._AreClose(num3, finalSize))
			{
				Grid.DistributionOrderIndexComparer comparer2 = new Grid.DistributionOrderIndexComparer(definitions);
				Array.Sort(definitionIndices, 0, definitions.Length, comparer2);
				double num10 = finalSize - num3;
				for (int j = 0; j < definitions.Length; j++)
				{
					int num11 = definitionIndices[j];
					double num12 = definitions[num11].SizeCache + num10 / (double)(definitions.Length - j);
					double value = num12;
					num12 = Math.Max(num12, definitions[num11].MinSizeForArrange);
					num12 = Math.Min(num12, definitions[num11].SizeCache);
					if (useLayoutRounding)
					{
						array[num11] = num12;
						num12 = UIElement.RoundLayoutValue(value, dpiScale);
						num12 = Math.Max(num12, definitions[num11].MinSizeForArrange);
						num12 = Math.Min(num12, definitions[num11].SizeCache);
					}
					num10 -= num12 - definitions[num11].SizeCache;
					definitions[num11].SizeCache = num12;
				}
				num3 = finalSize - num10;
			}
			if (useLayoutRounding && !Grid._AreClose(num3, finalSize))
			{
				for (int k = 0; k < definitions.Length; k++)
				{
					array[k] -= definitions[k].SizeCache;
					definitionIndices[k] = k;
				}
				Grid.RoundingErrorIndexComparer comparer3 = new Grid.RoundingErrorIndexComparer(array);
				Array.Sort(definitionIndices, 0, definitions.Length, comparer3);
				double num13 = num3;
				double num14 = UIElement.RoundLayoutValue(1.0, dpiScale);
				if (num3 > finalSize)
				{
					int num15 = definitions.Length - 1;
					while (num13 > finalSize && !Grid._AreClose(num13, finalSize))
					{
						if (num15 < 0)
						{
							break;
						}
						DefinitionBase definitionBase = definitions[definitionIndices[num15]];
						double num16 = definitionBase.SizeCache - num14;
						num16 = Math.Max(num16, definitionBase.MinSizeForArrange);
						if (num16 < definitionBase.SizeCache)
						{
							num13 -= num14;
						}
						definitionBase.SizeCache = num16;
						num15--;
					}
				}
				else if (num3 < finalSize)
				{
					int num17 = 0;
					while (num13 < finalSize && !Grid._AreClose(num13, finalSize) && num17 < definitions.Length)
					{
						DefinitionBase definitionBase2 = definitions[definitionIndices[num17]];
						double num18 = definitionBase2.SizeCache + num14;
						num18 = Math.Max(num18, definitionBase2.MinSizeForArrange);
						if (num18 > definitionBase2.SizeCache)
						{
							num13 += num14;
						}
						definitionBase2.SizeCache = num18;
						num17++;
					}
				}
			}
			definitions[0].FinalOffset = 0.0;
			for (int l = 0; l < definitions.Length; l++)
			{
				definitions[(l + 1) % definitions.Length].FinalOffset = definitions[l].FinalOffset + definitions[l].SizeCache;
			}
		}

		// Token: 0x06004C78 RID: 19576 RVA: 0x001597E0 File Offset: 0x001579E0
		private void SetFinalSizeMaxDiscrepancy(DefinitionBase[] definitions, double finalSize, bool columns)
		{
			int num = definitions.Length;
			int[] definitionIndices = this.DefinitionIndices;
			double num2 = 0.0;
			int num3 = 0;
			double scale = 1.0;
			double num4 = 0.0;
			for (int i = 0; i < num; i++)
			{
				DefinitionBase definitionBase = definitions[i];
				if (definitionBase.UserSize.IsStar)
				{
					num3++;
					definitionBase.MeasureSize = 1.0;
					if (definitionBase.UserSize.Value > num4)
					{
						num4 = definitionBase.UserSize.Value;
					}
				}
			}
			if (double.IsPositiveInfinity(num4))
			{
				scale = -1.0;
			}
			else if (num3 > 0)
			{
				double num5 = Math.Floor(Math.Log(double.MaxValue / num4 / (double)num3, 2.0));
				if (num5 < 0.0)
				{
					scale = Math.Pow(2.0, num5 - 4.0);
				}
			}
			bool flag = true;
			while (flag)
			{
				double num6 = 0.0;
				num2 = 0.0;
				int j;
				int num7 = j = 0;
				for (int k = 0; k < num; k++)
				{
					DefinitionBase definitionBase2 = definitions[k];
					if (definitionBase2.UserSize.IsStar)
					{
						if (definitionBase2.MeasureSize < 0.0)
						{
							num2 += -definitionBase2.MeasureSize;
						}
						else
						{
							double num8 = Grid.StarWeight(definitionBase2, scale);
							num6 += num8;
							if (definitionBase2.MinSizeForArrange > 0.0)
							{
								definitionIndices[j++] = k;
								definitionBase2.MeasureSize = num8 / definitionBase2.MinSizeForArrange;
							}
							double num9 = Math.Max(definitionBase2.MinSizeForArrange, definitionBase2.UserMaxSize);
							if (!double.IsPositiveInfinity(num9))
							{
								definitionIndices[num + num7++] = k;
								definitionBase2.SizeCache = num8 / num9;
							}
						}
					}
					else
					{
						double num10 = 0.0;
						GridUnitType gridUnitType = definitionBase2.UserSize.GridUnitType;
						if (gridUnitType != GridUnitType.Auto)
						{
							if (gridUnitType == GridUnitType.Pixel)
							{
								num10 = definitionBase2.UserSize.Value;
							}
						}
						else
						{
							num10 = definitionBase2.MinSizeForArrange;
						}
						double val;
						if (definitionBase2.IsShared)
						{
							val = num10;
						}
						else
						{
							val = definitionBase2.UserMaxSize;
						}
						definitionBase2.SizeCache = Math.Max(definitionBase2.MinSizeForArrange, Math.Min(num10, val));
						num2 += definitionBase2.SizeCache;
					}
				}
				int num11 = j;
				int num12 = num7;
				double num13 = 0.0;
				double num14 = finalSize - num2;
				double num15 = num6 - num13;
				Grid.MinRatioIndexComparer comparer = new Grid.MinRatioIndexComparer(definitions);
				Array.Sort(definitionIndices, 0, j, comparer);
				Grid.MaxRatioIndexComparer comparer2 = new Grid.MaxRatioIndexComparer(definitions);
				Array.Sort(definitionIndices, num, num7, comparer2);
				while (j + num7 > 0 && num14 > 0.0)
				{
					if (num15 < num6 * 0.00390625)
					{
						num13 = 0.0;
						num6 = 0.0;
						for (int l = 0; l < num; l++)
						{
							DefinitionBase definitionBase3 = definitions[l];
							if (definitionBase3.UserSize.IsStar && definitionBase3.MeasureSize > 0.0)
							{
								num6 += Grid.StarWeight(definitionBase3, scale);
							}
						}
						num15 = num6 - num13;
					}
					double minRatio = (j > 0) ? definitions[definitionIndices[j - 1]].MeasureSize : double.PositiveInfinity;
					double maxRatio = (num7 > 0) ? definitions[definitionIndices[num + num7 - 1]].SizeCache : -1.0;
					double proportion = num15 / num14;
					bool? flag2 = Grid.Choose(minRatio, maxRatio, proportion);
					if (flag2 == null)
					{
						break;
					}
					DefinitionBase definitionBase4;
					double num17;
					if (flag2 == true)
					{
						int num16 = definitionIndices[j - 1];
						definitionBase4 = definitions[num16];
						num17 = definitionBase4.MinSizeForArrange;
						j--;
					}
					else
					{
						int num16 = definitionIndices[num + num7 - 1];
						definitionBase4 = definitions[num16];
						num17 = Math.Max(definitionBase4.MinSizeForArrange, definitionBase4.UserMaxSize);
						num7--;
					}
					num2 += num17;
					definitionBase4.MeasureSize = -num17;
					num13 += Grid.StarWeight(definitionBase4, scale);
					num3--;
					num14 = finalSize - num2;
					num15 = num6 - num13;
					while (j > 0)
					{
						if (definitions[definitionIndices[j - 1]].MeasureSize >= 0.0)
						{
							break;
						}
						j--;
						definitionIndices[j] = -1;
					}
					while (num7 > 0 && definitions[definitionIndices[num + num7 - 1]].MeasureSize < 0.0)
					{
						num7--;
						definitionIndices[num + num7] = -1;
					}
				}
				flag = false;
				if (num3 == 0 && num2 < finalSize)
				{
					for (int m = j; m < num11; m++)
					{
						if (definitionIndices[m] >= 0)
						{
							DefinitionBase definitionBase5 = definitions[definitionIndices[m]];
							definitionBase5.MeasureSize = 1.0;
							num3++;
							flag = true;
						}
					}
				}
				if (num2 > finalSize)
				{
					for (int n = num7; n < num12; n++)
					{
						if (definitionIndices[num + n] >= 0)
						{
							DefinitionBase definitionBase6 = definitions[definitionIndices[num + n]];
							definitionBase6.MeasureSize = 1.0;
							num3++;
							flag = true;
						}
					}
				}
			}
			num3 = 0;
			for (int num18 = 0; num18 < num; num18++)
			{
				DefinitionBase definitionBase7 = definitions[num18];
				if (definitionBase7.UserSize.IsStar)
				{
					if (definitionBase7.MeasureSize < 0.0)
					{
						definitionBase7.SizeCache = -definitionBase7.MeasureSize;
					}
					else
					{
						definitionIndices[num3++] = num18;
						definitionBase7.MeasureSize = Grid.StarWeight(definitionBase7, scale);
					}
				}
			}
			if (num3 > 0)
			{
				Grid.StarWeightIndexComparer comparer3 = new Grid.StarWeightIndexComparer(definitions);
				Array.Sort(definitionIndices, 0, num3, comparer3);
				double num6 = 0.0;
				for (int num19 = 0; num19 < num3; num19++)
				{
					DefinitionBase definitionBase8 = definitions[definitionIndices[num19]];
					num6 += definitionBase8.MeasureSize;
					definitionBase8.SizeCache = num6;
				}
				for (int num20 = num3 - 1; num20 >= 0; num20--)
				{
					DefinitionBase definitionBase9 = definitions[definitionIndices[num20]];
					double num21 = (definitionBase9.MeasureSize > 0.0) ? (Math.Max(finalSize - num2, 0.0) * (definitionBase9.MeasureSize / definitionBase9.SizeCache)) : 0.0;
					num21 = Math.Min(num21, definitionBase9.UserMaxSize);
					num21 = Math.Max(definitionBase9.MinSizeForArrange, num21);
					num2 += num21;
					definitionBase9.SizeCache = num21;
				}
			}
			if (base.UseLayoutRounding)
			{
				DpiScale dpi = base.GetDpi();
				double num22 = columns ? dpi.DpiScaleX : dpi.DpiScaleY;
				double[] roundingErrors = this.RoundingErrors;
				double num23 = 0.0;
				for (int num24 = 0; num24 < definitions.Length; num24++)
				{
					DefinitionBase definitionBase10 = definitions[num24];
					double num25 = UIElement.RoundLayoutValue(definitionBase10.SizeCache, num22);
					roundingErrors[num24] = num25 - definitionBase10.SizeCache;
					definitionBase10.SizeCache = num25;
					num23 += num25;
				}
				if (!Grid._AreClose(num23, finalSize))
				{
					for (int num26 = 0; num26 < definitions.Length; num26++)
					{
						definitionIndices[num26] = num26;
					}
					Grid.RoundingErrorIndexComparer comparer4 = new Grid.RoundingErrorIndexComparer(roundingErrors);
					Array.Sort(definitionIndices, 0, definitions.Length, comparer4);
					double num27 = num23;
					double num28 = 1.0 / num22;
					if (num23 > finalSize)
					{
						int num29 = definitions.Length - 1;
						while (num27 > finalSize && !Grid._AreClose(num27, finalSize))
						{
							if (num29 < 0)
							{
								break;
							}
							DefinitionBase definitionBase11 = definitions[definitionIndices[num29]];
							double num30 = definitionBase11.SizeCache - num28;
							num30 = Math.Max(num30, definitionBase11.MinSizeForArrange);
							if (num30 < definitionBase11.SizeCache)
							{
								num27 -= num28;
							}
							definitionBase11.SizeCache = num30;
							num29--;
						}
					}
					else if (num23 < finalSize)
					{
						int num31 = 0;
						while (num27 < finalSize && !Grid._AreClose(num27, finalSize) && num31 < definitions.Length)
						{
							DefinitionBase definitionBase12 = definitions[definitionIndices[num31]];
							double num32 = definitionBase12.SizeCache + num28;
							num32 = Math.Max(num32, definitionBase12.MinSizeForArrange);
							if (num32 > definitionBase12.SizeCache)
							{
								num27 += num28;
							}
							definitionBase12.SizeCache = num32;
							num31++;
						}
					}
				}
			}
			definitions[0].FinalOffset = 0.0;
			for (int num33 = 0; num33 < definitions.Length; num33++)
			{
				definitions[(num33 + 1) % definitions.Length].FinalOffset = definitions[num33].FinalOffset + definitions[num33].SizeCache;
			}
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x0015A038 File Offset: 0x00158238
		private static bool? Choose(double minRatio, double maxRatio, double proportion)
		{
			if (minRatio < proportion)
			{
				if (maxRatio <= proportion)
				{
					return new bool?(true);
				}
				double num = Math.Floor(Math.Log(minRatio, 2.0));
				double num2 = Math.Floor(Math.Log(maxRatio, 2.0));
				double num3 = Math.Pow(2.0, Math.Floor((num + num2) / 2.0));
				if (proportion / num3 * (proportion / num3) > minRatio / num3 * (maxRatio / num3))
				{
					return new bool?(true);
				}
				return new bool?(false);
			}
			else
			{
				if (maxRatio > proportion)
				{
					return new bool?(false);
				}
				return null;
			}
		}

		// Token: 0x06004C7A RID: 19578 RVA: 0x0015A0D1 File Offset: 0x001582D1
		private static int CompareRoundingErrors(KeyValuePair<int, double> x, KeyValuePair<int, double> y)
		{
			if (x.Value < y.Value)
			{
				return -1;
			}
			if (x.Value > y.Value)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06004C7B RID: 19579 RVA: 0x0015A0F8 File Offset: 0x001582F8
		private double GetFinalSizeForRange(DefinitionBase[] definitions, int start, int count)
		{
			double num = 0.0;
			int num2 = start + count - 1;
			do
			{
				num += definitions[num2].SizeCache;
			}
			while (--num2 >= start);
			return num;
		}

		// Token: 0x06004C7C RID: 19580 RVA: 0x0015A12C File Offset: 0x0015832C
		private void SetValid()
		{
			Grid.ExtendedData extData = this.ExtData;
			if (extData != null && extData.TempDefinitions != null)
			{
				Array.Clear(extData.TempDefinitions, 0, Math.Max(this.DefinitionsU.Length, this.DefinitionsV.Length));
				extData.TempDefinitions = null;
			}
		}

		/// <summary>Returns true if <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" /> associated with this instance of <see cref="T:System.Windows.Controls.Grid" /> is not empty.</summary>
		/// <returns>true if <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" /> associated with this instance of <see cref="T:System.Windows.Controls.Grid" /> is not empty; otherwise, false.</returns>
		// Token: 0x06004C7D RID: 19581 RVA: 0x0015A174 File Offset: 0x00158374
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeColumnDefinitions()
		{
			Grid.ExtendedData extData = this.ExtData;
			return extData != null && extData.ColumnDefinitions != null && extData.ColumnDefinitions.Count > 0;
		}

		/// <summary>Returns true if <see cref="T:System.Windows.Controls.RowDefinitionCollection" /> associated with this instance of <see cref="T:System.Windows.Controls.Grid" /> is not empty.</summary>
		/// <returns>true if <see cref="T:System.Windows.Controls.RowDefinitionCollection" /> associated with this instance of <see cref="T:System.Windows.Controls.Grid" /> is not empty; otherwise, false.</returns>
		// Token: 0x06004C7E RID: 19582 RVA: 0x0015A1A4 File Offset: 0x001583A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeRowDefinitions()
		{
			Grid.ExtendedData extData = this.ExtData;
			return extData != null && extData.RowDefinitions != null && extData.RowDefinitions.Count > 0;
		}

		// Token: 0x06004C7F RID: 19583 RVA: 0x0015A1D4 File Offset: 0x001583D4
		private Grid.GridLinesRenderer EnsureGridLinesRenderer()
		{
			if (this.ShowGridLines && this._gridLinesRenderer == null)
			{
				this._gridLinesRenderer = new Grid.GridLinesRenderer();
				base.AddVisualChild(this._gridLinesRenderer);
			}
			if (!this.ShowGridLines && this._gridLinesRenderer != null)
			{
				base.RemoveVisualChild(this._gridLinesRenderer);
				this._gridLinesRenderer = null;
			}
			return this._gridLinesRenderer;
		}

		// Token: 0x06004C80 RID: 19584 RVA: 0x0015A231 File Offset: 0x00158431
		private void SetFlags(bool value, Grid.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x06004C81 RID: 19585 RVA: 0x0015A24F File Offset: 0x0015844F
		private bool CheckFlagsAnd(Grid.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x0015A25C File Offset: 0x0015845C
		private bool CheckFlagsOr(Grid.Flags flags)
		{
			return flags == (Grid.Flags)0 || (this._flags & flags) > (Grid.Flags)0;
		}

		// Token: 0x06004C83 RID: 19587 RVA: 0x0015A270 File Offset: 0x00158470
		private static void OnShowGridLinesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Grid grid = (Grid)d;
			if (grid.ExtData != null && grid.ListenToNotifications)
			{
				grid.InvalidateVisual();
			}
			grid.SetFlags((bool)e.NewValue, Grid.Flags.ShowGridLinesPropertyValue);
		}

		// Token: 0x06004C84 RID: 19588 RVA: 0x0015A2B4 File Offset: 0x001584B4
		private static void OnCellAttachedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Visual visual = d as Visual;
			if (visual != null)
			{
				Grid grid = VisualTreeHelper.GetParent(visual) as Grid;
				if (grid != null && grid.ExtData != null && grid.ListenToNotifications)
				{
					grid.CellsStructureDirty = true;
					grid.InvalidateMeasure();
				}
			}
		}

		// Token: 0x06004C85 RID: 19589 RVA: 0x0015A2F7 File Offset: 0x001584F7
		private static bool IsIntValueNotNegative(object value)
		{
			return (int)value >= 0;
		}

		// Token: 0x06004C86 RID: 19590 RVA: 0x0015A305 File Offset: 0x00158505
		private static bool IsIntValueGreaterThanZero(object value)
		{
			return (int)value > 0;
		}

		// Token: 0x06004C87 RID: 19591 RVA: 0x0015A310 File Offset: 0x00158510
		private static bool CompareNullRefs(object x, object y, out int result)
		{
			result = 2;
			if (x == null)
			{
				if (y == null)
				{
					result = 0;
				}
				else
				{
					result = -1;
				}
			}
			else if (y == null)
			{
				result = 1;
			}
			return result != 2;
		}

		// Token: 0x1700129F RID: 4767
		// (get) Token: 0x06004C88 RID: 19592 RVA: 0x0015A333 File Offset: 0x00158533
		private DefinitionBase[] DefinitionsU
		{
			get
			{
				return this.ExtData.DefinitionsU;
			}
		}

		// Token: 0x170012A0 RID: 4768
		// (get) Token: 0x06004C89 RID: 19593 RVA: 0x0015A340 File Offset: 0x00158540
		private DefinitionBase[] DefinitionsV
		{
			get
			{
				return this.ExtData.DefinitionsV;
			}
		}

		// Token: 0x170012A1 RID: 4769
		// (get) Token: 0x06004C8A RID: 19594 RVA: 0x0015A350 File Offset: 0x00158550
		private DefinitionBase[] TempDefinitions
		{
			get
			{
				Grid.ExtendedData extData = this.ExtData;
				int num = Math.Max(this.DefinitionsU.Length, this.DefinitionsV.Length) * 2;
				if (extData.TempDefinitions == null || extData.TempDefinitions.Length < num)
				{
					WeakReference weakReference = (WeakReference)Thread.GetData(Grid.s_tempDefinitionsDataSlot);
					if (weakReference == null)
					{
						extData.TempDefinitions = new DefinitionBase[num];
						Thread.SetData(Grid.s_tempDefinitionsDataSlot, new WeakReference(extData.TempDefinitions));
					}
					else
					{
						extData.TempDefinitions = (DefinitionBase[])weakReference.Target;
						if (extData.TempDefinitions == null || extData.TempDefinitions.Length < num)
						{
							extData.TempDefinitions = new DefinitionBase[num];
							weakReference.Target = extData.TempDefinitions;
						}
					}
				}
				return extData.TempDefinitions;
			}
		}

		// Token: 0x170012A2 RID: 4770
		// (get) Token: 0x06004C8B RID: 19595 RVA: 0x0015A408 File Offset: 0x00158608
		private int[] DefinitionIndices
		{
			get
			{
				int num = Math.Max(Math.Max(this.DefinitionsU.Length, this.DefinitionsV.Length), 1) * 2;
				if (this._definitionIndices == null || this._definitionIndices.Length < num)
				{
					this._definitionIndices = new int[num];
				}
				return this._definitionIndices;
			}
		}

		// Token: 0x170012A3 RID: 4771
		// (get) Token: 0x06004C8C RID: 19596 RVA: 0x0015A458 File Offset: 0x00158658
		private double[] RoundingErrors
		{
			get
			{
				int num = Math.Max(this.DefinitionsU.Length, this.DefinitionsV.Length);
				if (this._roundingErrors == null && num == 0)
				{
					this._roundingErrors = new double[1];
				}
				else if (this._roundingErrors == null || this._roundingErrors.Length < num)
				{
					this._roundingErrors = new double[num];
				}
				return this._roundingErrors;
			}
		}

		// Token: 0x170012A4 RID: 4772
		// (get) Token: 0x06004C8D RID: 19597 RVA: 0x0015A4B9 File Offset: 0x001586B9
		private Grid.CellCache[] PrivateCells
		{
			get
			{
				return this.ExtData.CellCachesCollection;
			}
		}

		// Token: 0x170012A5 RID: 4773
		// (get) Token: 0x06004C8E RID: 19598 RVA: 0x0015A4C6 File Offset: 0x001586C6
		// (set) Token: 0x06004C8F RID: 19599 RVA: 0x0015A4D2 File Offset: 0x001586D2
		private bool CellsStructureDirty
		{
			get
			{
				return !this.CheckFlagsAnd(Grid.Flags.ValidCellsStructure);
			}
			set
			{
				this.SetFlags(!value, Grid.Flags.ValidCellsStructure);
			}
		}

		// Token: 0x170012A6 RID: 4774
		// (get) Token: 0x06004C90 RID: 19600 RVA: 0x0015A4DF File Offset: 0x001586DF
		// (set) Token: 0x06004C91 RID: 19601 RVA: 0x0015A4EC File Offset: 0x001586EC
		private bool ListenToNotifications
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.ListenToNotifications);
			}
			set
			{
				this.SetFlags(value, Grid.Flags.ListenToNotifications);
			}
		}

		// Token: 0x170012A7 RID: 4775
		// (get) Token: 0x06004C92 RID: 19602 RVA: 0x0015A4FA File Offset: 0x001586FA
		// (set) Token: 0x06004C93 RID: 19603 RVA: 0x0015A507 File Offset: 0x00158707
		private bool SizeToContentU
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.SizeToContentU);
			}
			set
			{
				this.SetFlags(value, Grid.Flags.SizeToContentU);
			}
		}

		// Token: 0x170012A8 RID: 4776
		// (get) Token: 0x06004C94 RID: 19604 RVA: 0x0015A515 File Offset: 0x00158715
		// (set) Token: 0x06004C95 RID: 19605 RVA: 0x0015A522 File Offset: 0x00158722
		private bool SizeToContentV
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.SizeToContentV);
			}
			set
			{
				this.SetFlags(value, Grid.Flags.SizeToContentV);
			}
		}

		// Token: 0x170012A9 RID: 4777
		// (get) Token: 0x06004C96 RID: 19606 RVA: 0x0015A530 File Offset: 0x00158730
		// (set) Token: 0x06004C97 RID: 19607 RVA: 0x0015A53D File Offset: 0x0015873D
		private bool HasStarCellsU
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.HasStarCellsU);
			}
			set
			{
				this.SetFlags(value, Grid.Flags.HasStarCellsU);
			}
		}

		// Token: 0x170012AA RID: 4778
		// (get) Token: 0x06004C98 RID: 19608 RVA: 0x0015A54B File Offset: 0x0015874B
		// (set) Token: 0x06004C99 RID: 19609 RVA: 0x0015A558 File Offset: 0x00158758
		private bool HasStarCellsV
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.HasStarCellsV);
			}
			set
			{
				this.SetFlags(value, Grid.Flags.HasStarCellsV);
			}
		}

		// Token: 0x170012AB RID: 4779
		// (get) Token: 0x06004C9A RID: 19610 RVA: 0x0015A566 File Offset: 0x00158766
		// (set) Token: 0x06004C9B RID: 19611 RVA: 0x0015A573 File Offset: 0x00158773
		private bool HasGroup3CellsInAutoRows
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.HasGroup3CellsInAutoRows);
			}
			set
			{
				this.SetFlags(value, Grid.Flags.HasGroup3CellsInAutoRows);
			}
		}

		// Token: 0x06004C9C RID: 19612 RVA: 0x0015A581 File Offset: 0x00158781
		private static bool _IsZero(double d)
		{
			return Math.Abs(d) < 1E-05;
		}

		// Token: 0x06004C9D RID: 19613 RVA: 0x0015A594 File Offset: 0x00158794
		private static bool _AreClose(double d1, double d2)
		{
			return Math.Abs(d1 - d2) < 1E-05;
		}

		// Token: 0x170012AC RID: 4780
		// (get) Token: 0x06004C9E RID: 19614 RVA: 0x0015A5A9 File Offset: 0x001587A9
		private Grid.ExtendedData ExtData
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x06004C9F RID: 19615 RVA: 0x0015A5B4 File Offset: 0x001587B4
		private static double StarWeight(DefinitionBase def, double scale)
		{
			if (scale >= 0.0)
			{
				return def.UserSize.Value * scale;
			}
			if (!double.IsPositiveInfinity(def.UserSize.Value))
			{
				return 0.0;
			}
			return 1.0;
		}

		// Token: 0x170012AD RID: 4781
		// (get) Token: 0x06004CA0 RID: 19616 RVA: 0x000956EF File Offset: 0x000938EF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x06004CA1 RID: 19617 RVA: 0x00002137 File Offset: 0x00000337
		[Conditional("GRIDPARANOIA")]
		internal void EnterCounterScope(Grid.Counters scopeCounter)
		{
		}

		// Token: 0x06004CA2 RID: 19618 RVA: 0x00002137 File Offset: 0x00000337
		[Conditional("GRIDPARANOIA")]
		internal void ExitCounterScope(Grid.Counters scopeCounter)
		{
		}

		// Token: 0x06004CA3 RID: 19619 RVA: 0x00002137 File Offset: 0x00000337
		[Conditional("GRIDPARANOIA")]
		internal void EnterCounter(Grid.Counters counter)
		{
		}

		// Token: 0x06004CA4 RID: 19620 RVA: 0x00002137 File Offset: 0x00000337
		[Conditional("GRIDPARANOIA")]
		internal void ExitCounter(Grid.Counters counter)
		{
		}

		// Token: 0x04002B12 RID: 11026
		private Grid.ExtendedData _data;

		// Token: 0x04002B13 RID: 11027
		private Grid.Flags _flags;

		// Token: 0x04002B14 RID: 11028
		private Grid.GridLinesRenderer _gridLinesRenderer;

		// Token: 0x04002B15 RID: 11029
		private int[] _definitionIndices;

		// Token: 0x04002B16 RID: 11030
		private double[] _roundingErrors;

		// Token: 0x04002B17 RID: 11031
		private const double c_epsilon = 1E-05;

		// Token: 0x04002B18 RID: 11032
		private const double c_starClip = 1E+298;

		// Token: 0x04002B19 RID: 11033
		private const int c_layoutLoopMaxCount = 5;

		// Token: 0x04002B1A RID: 11034
		private static readonly LocalDataStoreSlot s_tempDefinitionsDataSlot = Thread.AllocateDataSlot();

		// Token: 0x04002B1B RID: 11035
		private static readonly IComparer s_spanPreferredDistributionOrderComparer = new Grid.SpanPreferredDistributionOrderComparer();

		// Token: 0x04002B1C RID: 11036
		private static readonly IComparer s_spanMaxDistributionOrderComparer = new Grid.SpanMaxDistributionOrderComparer();

		// Token: 0x04002B1D RID: 11037
		private static readonly IComparer s_starDistributionOrderComparer = new Grid.StarDistributionOrderComparer();

		// Token: 0x04002B1E RID: 11038
		private static readonly IComparer s_distributionOrderComparer = new Grid.DistributionOrderComparer();

		// Token: 0x04002B1F RID: 11039
		private static readonly IComparer s_minRatioComparer = new Grid.MinRatioComparer();

		// Token: 0x04002B20 RID: 11040
		private static readonly IComparer s_maxRatioComparer = new Grid.MaxRatioComparer();

		// Token: 0x04002B21 RID: 11041
		private static readonly IComparer s_starWeightComparer = new Grid.StarWeightComparer();

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Grid.ShowGridLines" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Grid.ShowGridLines" /> dependency property.</returns>
		// Token: 0x04002B22 RID: 11042
		public static readonly DependencyProperty ShowGridLinesProperty = DependencyProperty.Register("ShowGridLines", typeof(bool), typeof(Grid), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Grid.OnShowGridLinesPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Grid.Column" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Grid.Column" /> attached property.</returns>
		// Token: 0x04002B23 RID: 11043
		[CommonDependencyProperty]
		public static readonly DependencyProperty ColumnProperty = DependencyProperty.RegisterAttached("Column", typeof(int), typeof(Grid), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(Grid.OnCellAttachedPropertyChanged)), new ValidateValueCallback(Grid.IsIntValueNotNegative));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Grid.Row" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Grid.Row" /> attached property.</returns>
		// Token: 0x04002B24 RID: 11044
		[CommonDependencyProperty]
		public static readonly DependencyProperty RowProperty = DependencyProperty.RegisterAttached("Row", typeof(int), typeof(Grid), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(Grid.OnCellAttachedPropertyChanged)), new ValidateValueCallback(Grid.IsIntValueNotNegative));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Grid.ColumnSpan" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Grid.ColumnSpan" /> attached property.</returns>
		// Token: 0x04002B25 RID: 11045
		[CommonDependencyProperty]
		public static readonly DependencyProperty ColumnSpanProperty = DependencyProperty.RegisterAttached("ColumnSpan", typeof(int), typeof(Grid), new FrameworkPropertyMetadata(1, new PropertyChangedCallback(Grid.OnCellAttachedPropertyChanged)), new ValidateValueCallback(Grid.IsIntValueGreaterThanZero));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Grid.RowSpan" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Grid.RowSpan" /> attached property.</returns>
		// Token: 0x04002B26 RID: 11046
		[CommonDependencyProperty]
		public static readonly DependencyProperty RowSpanProperty = DependencyProperty.RegisterAttached("RowSpan", typeof(int), typeof(Grid), new FrameworkPropertyMetadata(1, new PropertyChangedCallback(Grid.OnCellAttachedPropertyChanged)), new ValidateValueCallback(Grid.IsIntValueGreaterThanZero));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Grid.IsSharedSizeScope" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Grid.IsSharedSizeScope" /> attached property.</returns>
		// Token: 0x04002B27 RID: 11047
		public static readonly DependencyProperty IsSharedSizeScopeProperty = DependencyProperty.RegisterAttached("IsSharedSizeScope", typeof(bool), typeof(Grid), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DefinitionBase.OnIsSharedSizeScopePropertyChanged)));

		// Token: 0x02000972 RID: 2418
		private class ExtendedData
		{
			// Token: 0x0400444F RID: 17487
			internal ColumnDefinitionCollection ColumnDefinitions;

			// Token: 0x04004450 RID: 17488
			internal RowDefinitionCollection RowDefinitions;

			// Token: 0x04004451 RID: 17489
			internal DefinitionBase[] DefinitionsU;

			// Token: 0x04004452 RID: 17490
			internal DefinitionBase[] DefinitionsV;

			// Token: 0x04004453 RID: 17491
			internal Grid.CellCache[] CellCachesCollection;

			// Token: 0x04004454 RID: 17492
			internal int CellGroup1;

			// Token: 0x04004455 RID: 17493
			internal int CellGroup2;

			// Token: 0x04004456 RID: 17494
			internal int CellGroup3;

			// Token: 0x04004457 RID: 17495
			internal int CellGroup4;

			// Token: 0x04004458 RID: 17496
			internal DefinitionBase[] TempDefinitions;
		}

		// Token: 0x02000973 RID: 2419
		[Flags]
		private enum Flags
		{
			// Token: 0x0400445A RID: 17498
			ValidDefinitionsUStructure = 1,
			// Token: 0x0400445B RID: 17499
			ValidDefinitionsVStructure = 2,
			// Token: 0x0400445C RID: 17500
			ValidCellsStructure = 4,
			// Token: 0x0400445D RID: 17501
			ShowGridLinesPropertyValue = 256,
			// Token: 0x0400445E RID: 17502
			ListenToNotifications = 4096,
			// Token: 0x0400445F RID: 17503
			SizeToContentU = 8192,
			// Token: 0x04004460 RID: 17504
			SizeToContentV = 16384,
			// Token: 0x04004461 RID: 17505
			HasStarCellsU = 32768,
			// Token: 0x04004462 RID: 17506
			HasStarCellsV = 65536,
			// Token: 0x04004463 RID: 17507
			HasGroup3CellsInAutoRows = 131072,
			// Token: 0x04004464 RID: 17508
			MeasureOverrideInProgress = 262144,
			// Token: 0x04004465 RID: 17509
			ArrangeOverrideInProgress = 524288
		}

		// Token: 0x02000974 RID: 2420
		[Flags]
		internal enum LayoutTimeSizeType : byte
		{
			// Token: 0x04004467 RID: 17511
			None = 0,
			// Token: 0x04004468 RID: 17512
			Pixel = 1,
			// Token: 0x04004469 RID: 17513
			Auto = 2,
			// Token: 0x0400446A RID: 17514
			Star = 4
		}

		// Token: 0x02000975 RID: 2421
		private struct CellCache
		{
			// Token: 0x17001E9A RID: 7834
			// (get) Token: 0x06008777 RID: 34679 RVA: 0x0024FEC5 File Offset: 0x0024E0C5
			internal bool IsStarU
			{
				get
				{
					return (this.SizeTypeU & Grid.LayoutTimeSizeType.Star) > Grid.LayoutTimeSizeType.None;
				}
			}

			// Token: 0x17001E9B RID: 7835
			// (get) Token: 0x06008778 RID: 34680 RVA: 0x0024FED2 File Offset: 0x0024E0D2
			internal bool IsAutoU
			{
				get
				{
					return (this.SizeTypeU & Grid.LayoutTimeSizeType.Auto) > Grid.LayoutTimeSizeType.None;
				}
			}

			// Token: 0x17001E9C RID: 7836
			// (get) Token: 0x06008779 RID: 34681 RVA: 0x0024FEDF File Offset: 0x0024E0DF
			internal bool IsStarV
			{
				get
				{
					return (this.SizeTypeV & Grid.LayoutTimeSizeType.Star) > Grid.LayoutTimeSizeType.None;
				}
			}

			// Token: 0x17001E9D RID: 7837
			// (get) Token: 0x0600877A RID: 34682 RVA: 0x0024FEEC File Offset: 0x0024E0EC
			internal bool IsAutoV
			{
				get
				{
					return (this.SizeTypeV & Grid.LayoutTimeSizeType.Auto) > Grid.LayoutTimeSizeType.None;
				}
			}

			// Token: 0x0400446B RID: 17515
			internal int ColumnIndex;

			// Token: 0x0400446C RID: 17516
			internal int RowIndex;

			// Token: 0x0400446D RID: 17517
			internal int ColumnSpan;

			// Token: 0x0400446E RID: 17518
			internal int RowSpan;

			// Token: 0x0400446F RID: 17519
			internal Grid.LayoutTimeSizeType SizeTypeU;

			// Token: 0x04004470 RID: 17520
			internal Grid.LayoutTimeSizeType SizeTypeV;

			// Token: 0x04004471 RID: 17521
			internal int Next;
		}

		// Token: 0x02000976 RID: 2422
		private class SpanKey
		{
			// Token: 0x0600877B RID: 34683 RVA: 0x0024FEF9 File Offset: 0x0024E0F9
			internal SpanKey(int start, int count, bool u)
			{
				this._start = start;
				this._count = count;
				this._u = u;
			}

			// Token: 0x0600877C RID: 34684 RVA: 0x0024FF18 File Offset: 0x0024E118
			public override int GetHashCode()
			{
				int num = this._start ^ this._count << 2;
				if (this._u)
				{
					num &= 134217727;
				}
				else
				{
					num |= 134217728;
				}
				return num;
			}

			// Token: 0x0600877D RID: 34685 RVA: 0x0024FF50 File Offset: 0x0024E150
			public override bool Equals(object obj)
			{
				Grid.SpanKey spanKey = obj as Grid.SpanKey;
				return spanKey != null && spanKey._start == this._start && spanKey._count == this._count && spanKey._u == this._u;
			}

			// Token: 0x17001E9E RID: 7838
			// (get) Token: 0x0600877E RID: 34686 RVA: 0x0024FF93 File Offset: 0x0024E193
			internal int Start
			{
				get
				{
					return this._start;
				}
			}

			// Token: 0x17001E9F RID: 7839
			// (get) Token: 0x0600877F RID: 34687 RVA: 0x0024FF9B File Offset: 0x0024E19B
			internal int Count
			{
				get
				{
					return this._count;
				}
			}

			// Token: 0x17001EA0 RID: 7840
			// (get) Token: 0x06008780 RID: 34688 RVA: 0x0024FFA3 File Offset: 0x0024E1A3
			internal bool U
			{
				get
				{
					return this._u;
				}
			}

			// Token: 0x04004472 RID: 17522
			private int _start;

			// Token: 0x04004473 RID: 17523
			private int _count;

			// Token: 0x04004474 RID: 17524
			private bool _u;
		}

		// Token: 0x02000977 RID: 2423
		private class SpanPreferredDistributionOrderComparer : IComparer
		{
			// Token: 0x06008781 RID: 34689 RVA: 0x0024FFAC File Offset: 0x0024E1AC
			public int Compare(object x, object y)
			{
				DefinitionBase definitionBase = x as DefinitionBase;
				DefinitionBase definitionBase2 = y as DefinitionBase;
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					if (definitionBase.UserSize.IsAuto)
					{
						if (definitionBase2.UserSize.IsAuto)
						{
							result = definitionBase.MinSize.CompareTo(definitionBase2.MinSize);
						}
						else
						{
							result = -1;
						}
					}
					else if (definitionBase2.UserSize.IsAuto)
					{
						result = 1;
					}
					else
					{
						result = definitionBase.PreferredSize.CompareTo(definitionBase2.PreferredSize);
					}
				}
				return result;
			}
		}

		// Token: 0x02000978 RID: 2424
		private class SpanMaxDistributionOrderComparer : IComparer
		{
			// Token: 0x06008783 RID: 34691 RVA: 0x0025003C File Offset: 0x0024E23C
			public int Compare(object x, object y)
			{
				DefinitionBase definitionBase = x as DefinitionBase;
				DefinitionBase definitionBase2 = y as DefinitionBase;
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					if (definitionBase.UserSize.IsAuto)
					{
						if (definitionBase2.UserSize.IsAuto)
						{
							result = definitionBase.SizeCache.CompareTo(definitionBase2.SizeCache);
						}
						else
						{
							result = 1;
						}
					}
					else if (definitionBase2.UserSize.IsAuto)
					{
						result = -1;
					}
					else
					{
						result = definitionBase.SizeCache.CompareTo(definitionBase2.SizeCache);
					}
				}
				return result;
			}
		}

		// Token: 0x02000979 RID: 2425
		private class StarDistributionOrderComparer : IComparer
		{
			// Token: 0x06008785 RID: 34693 RVA: 0x002500CC File Offset: 0x0024E2CC
			public int Compare(object x, object y)
			{
				DefinitionBase definitionBase = x as DefinitionBase;
				DefinitionBase definitionBase2 = y as DefinitionBase;
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					result = definitionBase.SizeCache.CompareTo(definitionBase2.SizeCache);
				}
				return result;
			}
		}

		// Token: 0x0200097A RID: 2426
		private class DistributionOrderComparer : IComparer
		{
			// Token: 0x06008787 RID: 34695 RVA: 0x00250108 File Offset: 0x0024E308
			public int Compare(object x, object y)
			{
				DefinitionBase definitionBase = x as DefinitionBase;
				DefinitionBase definitionBase2 = y as DefinitionBase;
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					double num = definitionBase.SizeCache - definitionBase.MinSizeForArrange;
					double value = definitionBase2.SizeCache - definitionBase2.MinSizeForArrange;
					result = num.CompareTo(value);
				}
				return result;
			}
		}

		// Token: 0x0200097B RID: 2427
		private class StarDistributionOrderIndexComparer : IComparer
		{
			// Token: 0x06008789 RID: 34697 RVA: 0x00250156 File Offset: 0x0024E356
			internal StarDistributionOrderIndexComparer(DefinitionBase[] definitions)
			{
				Invariant.Assert(definitions != null);
				this.definitions = definitions;
			}

			// Token: 0x0600878A RID: 34698 RVA: 0x00250170 File Offset: 0x0024E370
			public int Compare(object x, object y)
			{
				int? num = x as int?;
				int? num2 = y as int?;
				DefinitionBase definitionBase = null;
				DefinitionBase definitionBase2 = null;
				if (num != null)
				{
					definitionBase = this.definitions[num.Value];
				}
				if (num2 != null)
				{
					definitionBase2 = this.definitions[num2.Value];
				}
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					result = definitionBase.SizeCache.CompareTo(definitionBase2.SizeCache);
				}
				return result;
			}

			// Token: 0x04004475 RID: 17525
			private readonly DefinitionBase[] definitions;
		}

		// Token: 0x0200097C RID: 2428
		private class DistributionOrderIndexComparer : IComparer
		{
			// Token: 0x0600878B RID: 34699 RVA: 0x002501ED File Offset: 0x0024E3ED
			internal DistributionOrderIndexComparer(DefinitionBase[] definitions)
			{
				Invariant.Assert(definitions != null);
				this.definitions = definitions;
			}

			// Token: 0x0600878C RID: 34700 RVA: 0x00250208 File Offset: 0x0024E408
			public int Compare(object x, object y)
			{
				int? num = x as int?;
				int? num2 = y as int?;
				DefinitionBase definitionBase = null;
				DefinitionBase definitionBase2 = null;
				if (num != null)
				{
					definitionBase = this.definitions[num.Value];
				}
				if (num2 != null)
				{
					definitionBase2 = this.definitions[num2.Value];
				}
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					double num3 = definitionBase.SizeCache - definitionBase.MinSizeForArrange;
					double value = definitionBase2.SizeCache - definitionBase2.MinSizeForArrange;
					result = num3.CompareTo(value);
				}
				return result;
			}

			// Token: 0x04004476 RID: 17526
			private readonly DefinitionBase[] definitions;
		}

		// Token: 0x0200097D RID: 2429
		private class RoundingErrorIndexComparer : IComparer
		{
			// Token: 0x0600878D RID: 34701 RVA: 0x00250297 File Offset: 0x0024E497
			internal RoundingErrorIndexComparer(double[] errors)
			{
				Invariant.Assert(errors != null);
				this.errors = errors;
			}

			// Token: 0x0600878E RID: 34702 RVA: 0x002502B0 File Offset: 0x0024E4B0
			public int Compare(object x, object y)
			{
				int? num = x as int?;
				int? num2 = y as int?;
				int result;
				if (!Grid.CompareNullRefs(num, num2, out result))
				{
					double num3 = this.errors[num.Value];
					double value = this.errors[num2.Value];
					result = num3.CompareTo(value);
				}
				return result;
			}

			// Token: 0x04004477 RID: 17527
			private readonly double[] errors;
		}

		// Token: 0x0200097E RID: 2430
		private class MinRatioComparer : IComparer
		{
			// Token: 0x0600878F RID: 34703 RVA: 0x00250314 File Offset: 0x0024E514
			public int Compare(object x, object y)
			{
				DefinitionBase definitionBase = x as DefinitionBase;
				DefinitionBase definitionBase2 = y as DefinitionBase;
				int result;
				if (!Grid.CompareNullRefs(definitionBase2, definitionBase, out result))
				{
					result = definitionBase2.MeasureSize.CompareTo(definitionBase.MeasureSize);
				}
				return result;
			}
		}

		// Token: 0x0200097F RID: 2431
		private class MaxRatioComparer : IComparer
		{
			// Token: 0x06008791 RID: 34705 RVA: 0x00250350 File Offset: 0x0024E550
			public int Compare(object x, object y)
			{
				DefinitionBase definitionBase = x as DefinitionBase;
				DefinitionBase definitionBase2 = y as DefinitionBase;
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					result = definitionBase.SizeCache.CompareTo(definitionBase2.SizeCache);
				}
				return result;
			}
		}

		// Token: 0x02000980 RID: 2432
		private class StarWeightComparer : IComparer
		{
			// Token: 0x06008793 RID: 34707 RVA: 0x0025038C File Offset: 0x0024E58C
			public int Compare(object x, object y)
			{
				DefinitionBase definitionBase = x as DefinitionBase;
				DefinitionBase definitionBase2 = y as DefinitionBase;
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					result = definitionBase.MeasureSize.CompareTo(definitionBase2.MeasureSize);
				}
				return result;
			}
		}

		// Token: 0x02000981 RID: 2433
		private class MinRatioIndexComparer : IComparer
		{
			// Token: 0x06008795 RID: 34709 RVA: 0x002503C8 File Offset: 0x0024E5C8
			internal MinRatioIndexComparer(DefinitionBase[] definitions)
			{
				Invariant.Assert(definitions != null);
				this.definitions = definitions;
			}

			// Token: 0x06008796 RID: 34710 RVA: 0x002503E0 File Offset: 0x0024E5E0
			public int Compare(object x, object y)
			{
				int? num = x as int?;
				int? num2 = y as int?;
				DefinitionBase definitionBase = null;
				DefinitionBase definitionBase2 = null;
				if (num != null)
				{
					definitionBase = this.definitions[num.Value];
				}
				if (num2 != null)
				{
					definitionBase2 = this.definitions[num2.Value];
				}
				int result;
				if (!Grid.CompareNullRefs(definitionBase2, definitionBase, out result))
				{
					result = definitionBase2.MeasureSize.CompareTo(definitionBase.MeasureSize);
				}
				return result;
			}

			// Token: 0x04004478 RID: 17528
			private readonly DefinitionBase[] definitions;
		}

		// Token: 0x02000982 RID: 2434
		private class MaxRatioIndexComparer : IComparer
		{
			// Token: 0x06008797 RID: 34711 RVA: 0x0025045D File Offset: 0x0024E65D
			internal MaxRatioIndexComparer(DefinitionBase[] definitions)
			{
				Invariant.Assert(definitions != null);
				this.definitions = definitions;
			}

			// Token: 0x06008798 RID: 34712 RVA: 0x00250478 File Offset: 0x0024E678
			public int Compare(object x, object y)
			{
				int? num = x as int?;
				int? num2 = y as int?;
				DefinitionBase definitionBase = null;
				DefinitionBase definitionBase2 = null;
				if (num != null)
				{
					definitionBase = this.definitions[num.Value];
				}
				if (num2 != null)
				{
					definitionBase2 = this.definitions[num2.Value];
				}
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					result = definitionBase.SizeCache.CompareTo(definitionBase2.SizeCache);
				}
				return result;
			}

			// Token: 0x04004479 RID: 17529
			private readonly DefinitionBase[] definitions;
		}

		// Token: 0x02000983 RID: 2435
		private class StarWeightIndexComparer : IComparer
		{
			// Token: 0x06008799 RID: 34713 RVA: 0x002504F5 File Offset: 0x0024E6F5
			internal StarWeightIndexComparer(DefinitionBase[] definitions)
			{
				Invariant.Assert(definitions != null);
				this.definitions = definitions;
			}

			// Token: 0x0600879A RID: 34714 RVA: 0x00250510 File Offset: 0x0024E710
			public int Compare(object x, object y)
			{
				int? num = x as int?;
				int? num2 = y as int?;
				DefinitionBase definitionBase = null;
				DefinitionBase definitionBase2 = null;
				if (num != null)
				{
					definitionBase = this.definitions[num.Value];
				}
				if (num2 != null)
				{
					definitionBase2 = this.definitions[num2.Value];
				}
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					result = definitionBase.MeasureSize.CompareTo(definitionBase2.MeasureSize);
				}
				return result;
			}

			// Token: 0x0400447A RID: 17530
			private readonly DefinitionBase[] definitions;
		}

		// Token: 0x02000984 RID: 2436
		private class GridChildrenCollectionEnumeratorSimple : IEnumerator
		{
			// Token: 0x0600879B RID: 34715 RVA: 0x00250590 File Offset: 0x0024E790
			internal GridChildrenCollectionEnumeratorSimple(Grid grid, bool includeChildren)
			{
				this._currentEnumerator = -1;
				this._enumerator0 = new ColumnDefinitionCollection.Enumerator((grid.ExtData != null) ? grid.ExtData.ColumnDefinitions : null);
				this._enumerator1 = new RowDefinitionCollection.Enumerator((grid.ExtData != null) ? grid.ExtData.RowDefinitions : null);
				this._enumerator2Index = 0;
				if (includeChildren)
				{
					this._enumerator2Collection = grid.Children;
					this._enumerator2Count = this._enumerator2Collection.Count;
					return;
				}
				this._enumerator2Collection = null;
				this._enumerator2Count = 0;
			}

			// Token: 0x0600879C RID: 34716 RVA: 0x00250624 File Offset: 0x0024E824
			public bool MoveNext()
			{
				while (this._currentEnumerator < 3)
				{
					if (this._currentEnumerator >= 0)
					{
						switch (this._currentEnumerator)
						{
						case 0:
							if (this._enumerator0.MoveNext())
							{
								this._currentChild = this._enumerator0.Current;
								return true;
							}
							break;
						case 1:
							if (this._enumerator1.MoveNext())
							{
								this._currentChild = this._enumerator1.Current;
								return true;
							}
							break;
						case 2:
							if (this._enumerator2Index < this._enumerator2Count)
							{
								this._currentChild = this._enumerator2Collection[this._enumerator2Index];
								this._enumerator2Index++;
								return true;
							}
							break;
						}
					}
					this._currentEnumerator++;
				}
				return false;
			}

			// Token: 0x17001EA1 RID: 7841
			// (get) Token: 0x0600879D RID: 34717 RVA: 0x002506ED File Offset: 0x0024E8ED
			public object Current
			{
				get
				{
					if (this._currentEnumerator == -1)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
					}
					if (this._currentEnumerator >= 3)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
					}
					return this._currentChild;
				}
			}

			// Token: 0x0600879E RID: 34718 RVA: 0x00250727 File Offset: 0x0024E927
			public void Reset()
			{
				this._currentEnumerator = -1;
				this._currentChild = null;
				this._enumerator0.Reset();
				this._enumerator1.Reset();
				this._enumerator2Index = 0;
			}

			// Token: 0x0400447B RID: 17531
			private int _currentEnumerator;

			// Token: 0x0400447C RID: 17532
			private object _currentChild;

			// Token: 0x0400447D RID: 17533
			private ColumnDefinitionCollection.Enumerator _enumerator0;

			// Token: 0x0400447E RID: 17534
			private RowDefinitionCollection.Enumerator _enumerator1;

			// Token: 0x0400447F RID: 17535
			private UIElementCollection _enumerator2Collection;

			// Token: 0x04004480 RID: 17536
			private int _enumerator2Index;

			// Token: 0x04004481 RID: 17537
			private int _enumerator2Count;
		}

		// Token: 0x02000985 RID: 2437
		internal class GridLinesRenderer : DrawingVisual
		{
			// Token: 0x0600879F RID: 34719 RVA: 0x00250754 File Offset: 0x0024E954
			static GridLinesRenderer()
			{
				Grid.GridLinesRenderer.s_oddDashPen = new Pen(Brushes.Blue, 1.0);
				DoubleCollection doubleCollection = new DoubleCollection();
				doubleCollection.Add(4.0);
				doubleCollection.Add(4.0);
				Grid.GridLinesRenderer.s_oddDashPen.DashStyle = new DashStyle(doubleCollection, 0.0);
				Grid.GridLinesRenderer.s_oddDashPen.DashCap = PenLineCap.Flat;
				Grid.GridLinesRenderer.s_oddDashPen.Freeze();
				Grid.GridLinesRenderer.s_evenDashPen = new Pen(Brushes.Yellow, 1.0);
				DoubleCollection doubleCollection2 = new DoubleCollection();
				doubleCollection2.Add(4.0);
				doubleCollection2.Add(4.0);
				Grid.GridLinesRenderer.s_evenDashPen.DashStyle = new DashStyle(doubleCollection2, 4.0);
				Grid.GridLinesRenderer.s_evenDashPen.DashCap = PenLineCap.Flat;
				Grid.GridLinesRenderer.s_evenDashPen.Freeze();
			}

			// Token: 0x060087A0 RID: 34720 RVA: 0x00250854 File Offset: 0x0024EA54
			internal void UpdateRenderBounds(Size boundsSize)
			{
				using (DrawingContext drawingContext = base.RenderOpen())
				{
					Grid grid = VisualTreeHelper.GetParent(this) as Grid;
					if (grid != null && grid.ShowGridLines)
					{
						for (int i = 1; i < grid.DefinitionsU.Length; i++)
						{
							Grid.GridLinesRenderer.DrawGridLine(drawingContext, grid.DefinitionsU[i].FinalOffset, 0.0, grid.DefinitionsU[i].FinalOffset, boundsSize.Height);
						}
						for (int j = 1; j < grid.DefinitionsV.Length; j++)
						{
							Grid.GridLinesRenderer.DrawGridLine(drawingContext, 0.0, grid.DefinitionsV[j].FinalOffset, boundsSize.Width, grid.DefinitionsV[j].FinalOffset);
						}
					}
				}
			}

			// Token: 0x060087A1 RID: 34721 RVA: 0x00250928 File Offset: 0x0024EB28
			private static void DrawGridLine(DrawingContext drawingContext, double startX, double startY, double endX, double endY)
			{
				Point point = new Point(startX, startY);
				Point point2 = new Point(endX, endY);
				drawingContext.DrawLine(Grid.GridLinesRenderer.s_oddDashPen, point, point2);
				drawingContext.DrawLine(Grid.GridLinesRenderer.s_evenDashPen, point, point2);
			}

			// Token: 0x04004482 RID: 17538
			private const double c_dashLength = 4.0;

			// Token: 0x04004483 RID: 17539
			private const double c_penWidth = 1.0;

			// Token: 0x04004484 RID: 17540
			private static readonly Pen s_oddDashPen;

			// Token: 0x04004485 RID: 17541
			private static readonly Pen s_evenDashPen;

			// Token: 0x04004486 RID: 17542
			private static readonly Point c_zeroPoint = new Point(0.0, 0.0);
		}

		// Token: 0x02000986 RID: 2438
		internal enum Counters
		{
			// Token: 0x04004488 RID: 17544
			Default = -1,
			// Token: 0x04004489 RID: 17545
			MeasureOverride,
			// Token: 0x0400448A RID: 17546
			_ValidateColsStructure,
			// Token: 0x0400448B RID: 17547
			_ValidateRowsStructure,
			// Token: 0x0400448C RID: 17548
			_ValidateCells,
			// Token: 0x0400448D RID: 17549
			_MeasureCell,
			// Token: 0x0400448E RID: 17550
			__MeasureChild,
			// Token: 0x0400448F RID: 17551
			_CalculateDesiredSize,
			// Token: 0x04004490 RID: 17552
			ArrangeOverride,
			// Token: 0x04004491 RID: 17553
			_SetFinalSize,
			// Token: 0x04004492 RID: 17554
			_ArrangeChildHelper2,
			// Token: 0x04004493 RID: 17555
			_PositionCell,
			// Token: 0x04004494 RID: 17556
			Count
		}
	}
}
