using System;
using System.Collections;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents the control that redistributes space between columns or rows of a <see cref="T:System.Windows.Controls.Grid" /> control. </summary>
	// Token: 0x020004E0 RID: 1248
	[StyleTypedProperty(Property = "PreviewStyle", StyleTargetType = typeof(Control))]
	public class GridSplitter : Thumb
	{
		// Token: 0x06004DB5 RID: 19893 RVA: 0x0015E864 File Offset: 0x0015CA64
		static GridSplitter()
		{
			EventManager.RegisterClassHandler(typeof(GridSplitter), Thumb.DragStartedEvent, new DragStartedEventHandler(GridSplitter.OnDragStarted));
			EventManager.RegisterClassHandler(typeof(GridSplitter), Thumb.DragDeltaEvent, new DragDeltaEventHandler(GridSplitter.OnDragDelta));
			EventManager.RegisterClassHandler(typeof(GridSplitter), Thumb.DragCompletedEvent, new DragCompletedEventHandler(GridSplitter.OnDragCompleted));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(GridSplitter), new FrameworkPropertyMetadata(typeof(GridSplitter)));
			GridSplitter._dType = DependencyObjectType.FromSystemTypeInternal(typeof(GridSplitter));
			UIElement.FocusableProperty.OverrideMetadata(typeof(GridSplitter), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			FrameworkElement.HorizontalAlignmentProperty.OverrideMetadata(typeof(GridSplitter), new FrameworkPropertyMetadata(HorizontalAlignment.Right));
			FrameworkElement.CursorProperty.OverrideMetadata(typeof(GridSplitter), new FrameworkPropertyMetadata(null, new CoerceValueCallback(GridSplitter.CoerceCursor)));
			ControlsTraceLogger.AddControl(TelemetryControls.GridSplitter);
		}

		// Token: 0x06004DB7 RID: 19895 RVA: 0x0015EAD8 File Offset: 0x0015CCD8
		private static void UpdateCursor(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			o.CoerceValue(FrameworkElement.CursorProperty);
		}

		// Token: 0x06004DB8 RID: 19896 RVA: 0x0015EAE8 File Offset: 0x0015CCE8
		private static object CoerceCursor(DependencyObject o, object value)
		{
			GridSplitter gridSplitter = (GridSplitter)o;
			bool flag;
			BaseValueSourceInternal valueSource = gridSplitter.GetValueSource(FrameworkElement.CursorProperty, null, out flag);
			if (value == null && valueSource == BaseValueSourceInternal.Default)
			{
				GridResizeDirection effectiveResizeDirection = gridSplitter.GetEffectiveResizeDirection();
				if (effectiveResizeDirection == GridResizeDirection.Columns)
				{
					return Cursors.SizeWE;
				}
				if (effectiveResizeDirection == GridResizeDirection.Rows)
				{
					return Cursors.SizeNS;
				}
			}
			return value;
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.GridSplitter" /> control resizes rows or columns.  </summary>
		/// <returns>One of the enumeration values that specifies whether to resize rows or columns. The default is <see cref="F:System.Windows.Controls.GridResizeDirection.Auto" />.</returns>
		// Token: 0x170012EA RID: 4842
		// (get) Token: 0x06004DB9 RID: 19897 RVA: 0x0015EB30 File Offset: 0x0015CD30
		// (set) Token: 0x06004DBA RID: 19898 RVA: 0x0015EB42 File Offset: 0x0015CD42
		public GridResizeDirection ResizeDirection
		{
			get
			{
				return (GridResizeDirection)base.GetValue(GridSplitter.ResizeDirectionProperty);
			}
			set
			{
				base.SetValue(GridSplitter.ResizeDirectionProperty, value);
			}
		}

		// Token: 0x06004DBB RID: 19899 RVA: 0x0015EB58 File Offset: 0x0015CD58
		private static bool IsValidResizeDirection(object o)
		{
			GridResizeDirection gridResizeDirection = (GridResizeDirection)o;
			return gridResizeDirection == GridResizeDirection.Auto || gridResizeDirection == GridResizeDirection.Columns || gridResizeDirection == GridResizeDirection.Rows;
		}

		/// <summary>Gets or sets which columns or rows are resized relative to the column or row for which the <see cref="T:System.Windows.Controls.GridSplitter" /> control is defined. </summary>
		/// <returns>One of the enumeration values that indicates which columns or rows are resized by this <see cref="T:System.Windows.Controls.GridSplitter" /> control. The default is <see cref="F:System.Windows.Controls.GridResizeBehavior.BasedOnAlignment" />.</returns>
		// Token: 0x170012EB RID: 4843
		// (get) Token: 0x06004DBC RID: 19900 RVA: 0x0015EB79 File Offset: 0x0015CD79
		// (set) Token: 0x06004DBD RID: 19901 RVA: 0x0015EB8B File Offset: 0x0015CD8B
		public GridResizeBehavior ResizeBehavior
		{
			get
			{
				return (GridResizeBehavior)base.GetValue(GridSplitter.ResizeBehaviorProperty);
			}
			set
			{
				base.SetValue(GridSplitter.ResizeBehaviorProperty, value);
			}
		}

		// Token: 0x06004DBE RID: 19902 RVA: 0x0015EBA0 File Offset: 0x0015CDA0
		private static bool IsValidResizeBehavior(object o)
		{
			GridResizeBehavior gridResizeBehavior = (GridResizeBehavior)o;
			return gridResizeBehavior == GridResizeBehavior.BasedOnAlignment || gridResizeBehavior == GridResizeBehavior.CurrentAndNext || gridResizeBehavior == GridResizeBehavior.PreviousAndCurrent || gridResizeBehavior == GridResizeBehavior.PreviousAndNext;
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.GridSplitter" /> control updates the column or row size as the user drags the control. </summary>
		/// <returns>
		///     <see langword="true" /> if a <see cref="T:System.Windows.Controls.GridSplitter" /> preview is displayed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170012EC RID: 4844
		// (get) Token: 0x06004DBF RID: 19903 RVA: 0x0015EBC5 File Offset: 0x0015CDC5
		// (set) Token: 0x06004DC0 RID: 19904 RVA: 0x0015EBD7 File Offset: 0x0015CDD7
		public bool ShowsPreview
		{
			get
			{
				return (bool)base.GetValue(GridSplitter.ShowsPreviewProperty);
			}
			set
			{
				base.SetValue(GridSplitter.ShowsPreviewProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets or sets the style that customizes the appearance, effects, or other style characteristics for the <see cref="T:System.Windows.Controls.GridSplitter" /> control preview indicator that is displayed when the <see cref="P:System.Windows.Controls.GridSplitter.ShowsPreview" /> property is set to <see langword="true" />. </summary>
		/// <returns>Returns the <see cref="T:System.Windows.Style" /> for the preview indicator that shows the potential change in <see cref="T:System.Windows.Controls.Grid" /> dimensions as you move the <see cref="T:System.Windows.Controls.GridSplitter" /> control. The default is the style that the current theme supplies.</returns>
		// Token: 0x170012ED RID: 4845
		// (get) Token: 0x06004DC1 RID: 19905 RVA: 0x0015EBEA File Offset: 0x0015CDEA
		// (set) Token: 0x06004DC2 RID: 19906 RVA: 0x0015EBFC File Offset: 0x0015CDFC
		public Style PreviewStyle
		{
			get
			{
				return (Style)base.GetValue(GridSplitter.PreviewStyleProperty);
			}
			set
			{
				base.SetValue(GridSplitter.PreviewStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the distance that each press of an arrow key moves a <see cref="T:System.Windows.Controls.GridSplitter" /> control. </summary>
		/// <returns>The distance that the <see cref="T:System.Windows.Controls.GridSplitter" /> moves for each press of an arrow key. The default is 10. </returns>
		// Token: 0x170012EE RID: 4846
		// (get) Token: 0x06004DC3 RID: 19907 RVA: 0x0015EC0A File Offset: 0x0015CE0A
		// (set) Token: 0x06004DC4 RID: 19908 RVA: 0x0015EC1C File Offset: 0x0015CE1C
		public double KeyboardIncrement
		{
			get
			{
				return (double)base.GetValue(GridSplitter.KeyboardIncrementProperty);
			}
			set
			{
				base.SetValue(GridSplitter.KeyboardIncrementProperty, value);
			}
		}

		// Token: 0x06004DC5 RID: 19909 RVA: 0x0015EC30 File Offset: 0x0015CE30
		private static bool IsValidDelta(object o)
		{
			double num = (double)o;
			return num > 0.0 && !double.IsPositiveInfinity(num);
		}

		/// <summary>Gets or sets the minimum distance that a user must drag a mouse to resize rows or columns with a <see cref="T:System.Windows.Controls.GridSplitter" /> control. </summary>
		/// <returns>A value that represents the minimum distance that a user must use the mouse to drag a <see cref="T:System.Windows.Controls.GridSplitter" /> to resize rows or columns. The default is 1.</returns>
		// Token: 0x170012EF RID: 4847
		// (get) Token: 0x06004DC6 RID: 19910 RVA: 0x0015EC5B File Offset: 0x0015CE5B
		// (set) Token: 0x06004DC7 RID: 19911 RVA: 0x0015EC6D File Offset: 0x0015CE6D
		public double DragIncrement
		{
			get
			{
				return (double)base.GetValue(GridSplitter.DragIncrementProperty);
			}
			set
			{
				base.SetValue(GridSplitter.DragIncrementProperty, value);
			}
		}

		/// <summary>Creates the implementation of <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the <see cref="T:System.Windows.Controls.GridSplitter" /> control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Automation.Peers.GridSplitterAutomationPeer" /> for this <see cref="T:System.Windows.Controls.ToolTip" /> control.</returns>
		// Token: 0x06004DC8 RID: 19912 RVA: 0x0015EC80 File Offset: 0x0015CE80
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new GridSplitterAutomationPeer(this);
		}

		// Token: 0x06004DC9 RID: 19913 RVA: 0x0015EC88 File Offset: 0x0015CE88
		private GridResizeDirection GetEffectiveResizeDirection()
		{
			GridResizeDirection gridResizeDirection = this.ResizeDirection;
			if (gridResizeDirection == GridResizeDirection.Auto)
			{
				if (base.HorizontalAlignment != HorizontalAlignment.Stretch)
				{
					gridResizeDirection = GridResizeDirection.Columns;
				}
				else if (base.VerticalAlignment != VerticalAlignment.Stretch)
				{
					gridResizeDirection = GridResizeDirection.Rows;
				}
				else if (base.ActualWidth <= base.ActualHeight)
				{
					gridResizeDirection = GridResizeDirection.Columns;
				}
				else
				{
					gridResizeDirection = GridResizeDirection.Rows;
				}
			}
			return gridResizeDirection;
		}

		// Token: 0x06004DCA RID: 19914 RVA: 0x0015ECD0 File Offset: 0x0015CED0
		private GridResizeBehavior GetEffectiveResizeBehavior(GridResizeDirection direction)
		{
			GridResizeBehavior gridResizeBehavior = this.ResizeBehavior;
			if (gridResizeBehavior == GridResizeBehavior.BasedOnAlignment)
			{
				if (direction == GridResizeDirection.Columns)
				{
					HorizontalAlignment horizontalAlignment = base.HorizontalAlignment;
					if (horizontalAlignment != HorizontalAlignment.Left)
					{
						if (horizontalAlignment != HorizontalAlignment.Right)
						{
							gridResizeBehavior = GridResizeBehavior.PreviousAndNext;
						}
						else
						{
							gridResizeBehavior = GridResizeBehavior.CurrentAndNext;
						}
					}
					else
					{
						gridResizeBehavior = GridResizeBehavior.PreviousAndCurrent;
					}
				}
				else
				{
					VerticalAlignment verticalAlignment = base.VerticalAlignment;
					if (verticalAlignment != VerticalAlignment.Top)
					{
						if (verticalAlignment != VerticalAlignment.Bottom)
						{
							gridResizeBehavior = GridResizeBehavior.PreviousAndNext;
						}
						else
						{
							gridResizeBehavior = GridResizeBehavior.CurrentAndNext;
						}
					}
					else
					{
						gridResizeBehavior = GridResizeBehavior.PreviousAndCurrent;
					}
				}
			}
			return gridResizeBehavior;
		}

		/// <summary>Responds to a change in the dimensions of the <see cref="T:System.Windows.Controls.GridSplitter" /> control.</summary>
		/// <param name="sizeInfo">Information about the change in size of the <see cref="T:System.Windows.Controls.GridSplitter" />.</param>
		// Token: 0x06004DCB RID: 19915 RVA: 0x0015ED22 File Offset: 0x0015CF22
		protected internal override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);
			base.CoerceValue(FrameworkElement.CursorProperty);
		}

		// Token: 0x06004DCC RID: 19916 RVA: 0x0015ED38 File Offset: 0x0015CF38
		private void RemovePreviewAdorner()
		{
			if (this._resizeData.Adorner != null)
			{
				AdornerLayer adornerLayer = VisualTreeHelper.GetParent(this._resizeData.Adorner) as AdornerLayer;
				adornerLayer.Remove(this._resizeData.Adorner);
			}
		}

		// Token: 0x06004DCD RID: 19917 RVA: 0x0015ED7C File Offset: 0x0015CF7C
		private void InitializeData(bool ShowsPreview)
		{
			Grid grid = base.Parent as Grid;
			if (grid != null)
			{
				this._resizeData = new GridSplitter.ResizeData();
				this._resizeData.Grid = grid;
				this._resizeData.ShowsPreview = ShowsPreview;
				this._resizeData.ResizeDirection = this.GetEffectiveResizeDirection();
				this._resizeData.ResizeBehavior = this.GetEffectiveResizeBehavior(this._resizeData.ResizeDirection);
				this._resizeData.SplitterLength = Math.Min(base.ActualWidth, base.ActualHeight);
				if (!this.SetupDefinitionsToResize())
				{
					this._resizeData = null;
					return;
				}
				this.SetupPreview();
			}
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x0015EE20 File Offset: 0x0015D020
		private bool SetupDefinitionsToResize()
		{
			int num = (int)base.GetValue((this._resizeData.ResizeDirection == GridResizeDirection.Columns) ? Grid.ColumnSpanProperty : Grid.RowSpanProperty);
			if (num == 1)
			{
				int num2 = (int)base.GetValue((this._resizeData.ResizeDirection == GridResizeDirection.Columns) ? Grid.ColumnProperty : Grid.RowProperty);
				GridResizeBehavior resizeBehavior = this._resizeData.ResizeBehavior;
				int num3;
				int num4;
				if (resizeBehavior != GridResizeBehavior.CurrentAndNext)
				{
					if (resizeBehavior == GridResizeBehavior.PreviousAndCurrent)
					{
						num3 = num2 - 1;
						num4 = num2;
					}
					else
					{
						num3 = num2 - 1;
						num4 = num2 + 1;
					}
				}
				else
				{
					num3 = num2;
					num4 = num2 + 1;
				}
				int num5 = (this._resizeData.ResizeDirection == GridResizeDirection.Columns) ? this._resizeData.Grid.ColumnDefinitions.Count : this._resizeData.Grid.RowDefinitions.Count;
				if (num3 >= 0 && num4 < num5)
				{
					this._resizeData.SplitterIndex = num2;
					this._resizeData.Definition1Index = num3;
					this._resizeData.Definition1 = GridSplitter.GetGridDefinition(this._resizeData.Grid, num3, this._resizeData.ResizeDirection);
					this._resizeData.OriginalDefinition1Length = this._resizeData.Definition1.UserSizeValueCache;
					this._resizeData.OriginalDefinition1ActualLength = this.GetActualLength(this._resizeData.Definition1);
					this._resizeData.Definition2Index = num4;
					this._resizeData.Definition2 = GridSplitter.GetGridDefinition(this._resizeData.Grid, num4, this._resizeData.ResizeDirection);
					this._resizeData.OriginalDefinition2Length = this._resizeData.Definition2.UserSizeValueCache;
					this._resizeData.OriginalDefinition2ActualLength = this.GetActualLength(this._resizeData.Definition2);
					bool flag = GridSplitter.IsStar(this._resizeData.Definition1);
					bool flag2 = GridSplitter.IsStar(this._resizeData.Definition2);
					if (flag && flag2)
					{
						this._resizeData.SplitBehavior = GridSplitter.SplitBehavior.Split;
					}
					else
					{
						this._resizeData.SplitBehavior = ((!flag) ? GridSplitter.SplitBehavior.Resize1 : GridSplitter.SplitBehavior.Resize2);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004DCF RID: 19919 RVA: 0x0015F02C File Offset: 0x0015D22C
		private void SetupPreview()
		{
			if (this._resizeData.ShowsPreview)
			{
				AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._resizeData.Grid);
				if (adornerLayer == null)
				{
					return;
				}
				this._resizeData.Adorner = new GridSplitter.PreviewAdorner(this, this.PreviewStyle);
				adornerLayer.Add(this._resizeData.Adorner);
				this.GetDeltaConstraints(out this._resizeData.MinChange, out this._resizeData.MaxChange);
			}
		}

		/// <summary>Called when the <see cref="T:System.Windows.Controls.GridSplitter" /> control loses keyboard focus.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Input.KeyboardFocusChangedEventArgs" /> that contains the event data. </param>
		// Token: 0x06004DD0 RID: 19920 RVA: 0x0015F09F File Offset: 0x0015D29F
		protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnLostKeyboardFocus(e);
			if (this._resizeData != null)
			{
				this.CancelResize();
			}
		}

		// Token: 0x06004DD1 RID: 19921 RVA: 0x0015F0B8 File Offset: 0x0015D2B8
		private static void OnDragStarted(object sender, DragStartedEventArgs e)
		{
			GridSplitter gridSplitter = sender as GridSplitter;
			gridSplitter.OnDragStarted(e);
		}

		// Token: 0x06004DD2 RID: 19922 RVA: 0x0015F0D3 File Offset: 0x0015D2D3
		private void OnDragStarted(DragStartedEventArgs e)
		{
			this.InitializeData(this.ShowsPreview);
		}

		// Token: 0x06004DD3 RID: 19923 RVA: 0x0015F0E4 File Offset: 0x0015D2E4
		private static void OnDragDelta(object sender, DragDeltaEventArgs e)
		{
			GridSplitter gridSplitter = sender as GridSplitter;
			gridSplitter.OnDragDelta(e);
		}

		// Token: 0x06004DD4 RID: 19924 RVA: 0x0015F100 File Offset: 0x0015D300
		private void OnDragDelta(DragDeltaEventArgs e)
		{
			if (this._resizeData != null)
			{
				double num = e.HorizontalChange;
				double num2 = e.VerticalChange;
				double dragIncrement = this.DragIncrement;
				num = Math.Round(num / dragIncrement) * dragIncrement;
				num2 = Math.Round(num2 / dragIncrement) * dragIncrement;
				if (this._resizeData.ShowsPreview)
				{
					if (this._resizeData.ResizeDirection == GridResizeDirection.Columns)
					{
						this._resizeData.Adorner.OffsetX = Math.Min(Math.Max(num, this._resizeData.MinChange), this._resizeData.MaxChange);
						return;
					}
					this._resizeData.Adorner.OffsetY = Math.Min(Math.Max(num2, this._resizeData.MinChange), this._resizeData.MaxChange);
					return;
				}
				else
				{
					this.MoveSplitter(num, num2);
				}
			}
		}

		// Token: 0x06004DD5 RID: 19925 RVA: 0x0015F1CC File Offset: 0x0015D3CC
		private static void OnDragCompleted(object sender, DragCompletedEventArgs e)
		{
			GridSplitter gridSplitter = sender as GridSplitter;
			gridSplitter.OnDragCompleted(e);
		}

		// Token: 0x06004DD6 RID: 19926 RVA: 0x0015F1E8 File Offset: 0x0015D3E8
		private void OnDragCompleted(DragCompletedEventArgs e)
		{
			if (this._resizeData != null)
			{
				if (this._resizeData.ShowsPreview)
				{
					this.MoveSplitter(this._resizeData.Adorner.OffsetX, this._resizeData.Adorner.OffsetY);
					this.RemovePreviewAdorner();
				}
				this._resizeData = null;
			}
		}

		/// <summary>Called when a key is pressed.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Input.KeyEventArgs" /> that contains the event data. </param>
		// Token: 0x06004DD7 RID: 19927 RVA: 0x0015F240 File Offset: 0x0015D440
		protected override void OnKeyDown(KeyEventArgs e)
		{
			Key key = e.Key;
			if (key != Key.Escape)
			{
				switch (key)
				{
				case Key.Left:
					e.Handled = this.KeyboardMoveSplitter(-this.KeyboardIncrement, 0.0);
					return;
				case Key.Up:
					e.Handled = this.KeyboardMoveSplitter(0.0, -this.KeyboardIncrement);
					return;
				case Key.Right:
					e.Handled = this.KeyboardMoveSplitter(this.KeyboardIncrement, 0.0);
					return;
				case Key.Down:
					e.Handled = this.KeyboardMoveSplitter(0.0, this.KeyboardIncrement);
					break;
				default:
					return;
				}
			}
			else if (this._resizeData != null)
			{
				this.CancelResize();
				e.Handled = true;
				return;
			}
		}

		// Token: 0x06004DD8 RID: 19928 RVA: 0x0015F2FC File Offset: 0x0015D4FC
		private void CancelResize()
		{
			Grid grid = base.Parent as Grid;
			if (this._resizeData.ShowsPreview)
			{
				this.RemovePreviewAdorner();
			}
			else
			{
				GridSplitter.SetDefinitionLength(this._resizeData.Definition1, this._resizeData.OriginalDefinition1Length);
				GridSplitter.SetDefinitionLength(this._resizeData.Definition2, this._resizeData.OriginalDefinition2Length);
			}
			this._resizeData = null;
		}

		// Token: 0x06004DD9 RID: 19929 RVA: 0x0015F368 File Offset: 0x0015D568
		private static bool IsStar(DefinitionBase definition)
		{
			return definition.UserSizeValueCache.IsStar;
		}

		// Token: 0x06004DDA RID: 19930 RVA: 0x0015F383 File Offset: 0x0015D583
		private static DefinitionBase GetGridDefinition(Grid grid, int index, GridResizeDirection direction)
		{
			if (direction != GridResizeDirection.Columns)
			{
				return grid.RowDefinitions[index];
			}
			return grid.ColumnDefinitions[index];
		}

		// Token: 0x06004DDB RID: 19931 RVA: 0x0015F3A4 File Offset: 0x0015D5A4
		private double GetActualLength(DefinitionBase definition)
		{
			ColumnDefinition columnDefinition = definition as ColumnDefinition;
			if (columnDefinition != null)
			{
				return columnDefinition.ActualWidth;
			}
			return ((RowDefinition)definition).ActualHeight;
		}

		// Token: 0x06004DDC RID: 19932 RVA: 0x0015F3CD File Offset: 0x0015D5CD
		private static void SetDefinitionLength(DefinitionBase definition, GridLength length)
		{
			definition.SetValue((definition is ColumnDefinition) ? ColumnDefinition.WidthProperty : RowDefinition.HeightProperty, length);
		}

		// Token: 0x06004DDD RID: 19933 RVA: 0x0015F3F0 File Offset: 0x0015D5F0
		private void GetDeltaConstraints(out double minDelta, out double maxDelta)
		{
			double actualLength = this.GetActualLength(this._resizeData.Definition1);
			double num = this._resizeData.Definition1.UserMinSizeValueCache;
			double userMaxSizeValueCache = this._resizeData.Definition1.UserMaxSizeValueCache;
			double actualLength2 = this.GetActualLength(this._resizeData.Definition2);
			double num2 = this._resizeData.Definition2.UserMinSizeValueCache;
			double userMaxSizeValueCache2 = this._resizeData.Definition2.UserMaxSizeValueCache;
			if (this._resizeData.SplitterIndex == this._resizeData.Definition1Index)
			{
				num = Math.Max(num, this._resizeData.SplitterLength);
			}
			else if (this._resizeData.SplitterIndex == this._resizeData.Definition2Index)
			{
				num2 = Math.Max(num2, this._resizeData.SplitterLength);
			}
			if (this._resizeData.SplitBehavior == GridSplitter.SplitBehavior.Split)
			{
				minDelta = -Math.Min(actualLength - num, userMaxSizeValueCache2 - actualLength2);
				maxDelta = Math.Min(userMaxSizeValueCache - actualLength, actualLength2 - num2);
				return;
			}
			if (this._resizeData.SplitBehavior == GridSplitter.SplitBehavior.Resize1)
			{
				minDelta = num - actualLength;
				maxDelta = userMaxSizeValueCache - actualLength;
				return;
			}
			minDelta = actualLength2 - userMaxSizeValueCache2;
			maxDelta = actualLength2 - num2;
		}

		// Token: 0x06004DDE RID: 19934 RVA: 0x0015F510 File Offset: 0x0015D710
		private void SetLengths(double definition1Pixels, double definition2Pixels)
		{
			if (this._resizeData.SplitBehavior == GridSplitter.SplitBehavior.Split)
			{
				IEnumerable enumerable2;
				if (this._resizeData.ResizeDirection != GridResizeDirection.Columns)
				{
					IEnumerable enumerable = this._resizeData.Grid.RowDefinitions;
					enumerable2 = enumerable;
				}
				else
				{
					IEnumerable enumerable = this._resizeData.Grid.ColumnDefinitions;
					enumerable2 = enumerable;
				}
				IEnumerable enumerable3 = enumerable2;
				int num = 0;
				using (IEnumerator enumerator = enumerable3.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DefinitionBase definition = (DefinitionBase)obj;
						if (num == this._resizeData.Definition1Index)
						{
							GridSplitter.SetDefinitionLength(definition, new GridLength(definition1Pixels, GridUnitType.Star));
						}
						else if (num == this._resizeData.Definition2Index)
						{
							GridSplitter.SetDefinitionLength(definition, new GridLength(definition2Pixels, GridUnitType.Star));
						}
						else if (GridSplitter.IsStar(definition))
						{
							GridSplitter.SetDefinitionLength(definition, new GridLength(this.GetActualLength(definition), GridUnitType.Star));
						}
						num++;
					}
					return;
				}
			}
			if (this._resizeData.SplitBehavior == GridSplitter.SplitBehavior.Resize1)
			{
				GridSplitter.SetDefinitionLength(this._resizeData.Definition1, new GridLength(definition1Pixels));
				return;
			}
			GridSplitter.SetDefinitionLength(this._resizeData.Definition2, new GridLength(definition2Pixels));
		}

		// Token: 0x06004DDF RID: 19935 RVA: 0x0015F644 File Offset: 0x0015D844
		private void MoveSplitter(double horizontalChange, double verticalChange)
		{
			DpiScale dpi = base.GetDpi();
			double num;
			if (this._resizeData.ResizeDirection == GridResizeDirection.Columns)
			{
				num = horizontalChange;
				if (base.UseLayoutRounding)
				{
					num = UIElement.RoundLayoutValue(num, dpi.DpiScaleX);
				}
			}
			else
			{
				num = verticalChange;
				if (base.UseLayoutRounding)
				{
					num = UIElement.RoundLayoutValue(num, dpi.DpiScaleY);
				}
			}
			DefinitionBase definition = this._resizeData.Definition1;
			DefinitionBase definition2 = this._resizeData.Definition2;
			if (definition != null && definition2 != null)
			{
				double actualLength = this.GetActualLength(definition);
				double actualLength2 = this.GetActualLength(definition2);
				if (this._resizeData.SplitBehavior == GridSplitter.SplitBehavior.Split && !LayoutDoubleUtil.AreClose(actualLength + actualLength2, this._resizeData.OriginalDefinition1ActualLength + this._resizeData.OriginalDefinition2ActualLength))
				{
					this.CancelResize();
					return;
				}
				double val;
				double val2;
				this.GetDeltaConstraints(out val, out val2);
				if (base.FlowDirection != this._resizeData.Grid.FlowDirection)
				{
					num = -num;
				}
				num = Math.Min(Math.Max(num, val), val2);
				double num2 = actualLength + num;
				double definition2Pixels = actualLength + actualLength2 - num2;
				this.SetLengths(num2, definition2Pixels);
			}
		}

		// Token: 0x06004DE0 RID: 19936 RVA: 0x0015F754 File Offset: 0x0015D954
		internal bool KeyboardMoveSplitter(double horizontalChange, double verticalChange)
		{
			if (this._resizeData != null)
			{
				return false;
			}
			this.InitializeData(false);
			if (this._resizeData == null)
			{
				return false;
			}
			if (base.FlowDirection == FlowDirection.RightToLeft)
			{
				horizontalChange = -horizontalChange;
			}
			this.MoveSplitter(horizontalChange, verticalChange);
			this._resizeData = null;
			return true;
		}

		// Token: 0x170012F0 RID: 4848
		// (get) Token: 0x06004DE1 RID: 19937 RVA: 0x0015F78E File Offset: 0x0015D98E
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return GridSplitter._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridSplitter.ResizeDirection" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.GridSplitter.ResizeDirection" /> dependency property.</returns>
		// Token: 0x04002B94 RID: 11156
		public static readonly DependencyProperty ResizeDirectionProperty = DependencyProperty.Register("ResizeDirection", typeof(GridResizeDirection), typeof(GridSplitter), new FrameworkPropertyMetadata(GridResizeDirection.Auto, new PropertyChangedCallback(GridSplitter.UpdateCursor)), new ValidateValueCallback(GridSplitter.IsValidResizeDirection));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridSplitter.ResizeBehavior" /> dependency property. </summary>
		// Token: 0x04002B95 RID: 11157
		public static readonly DependencyProperty ResizeBehaviorProperty = DependencyProperty.Register("ResizeBehavior", typeof(GridResizeBehavior), typeof(GridSplitter), new FrameworkPropertyMetadata(GridResizeBehavior.BasedOnAlignment), new ValidateValueCallback(GridSplitter.IsValidResizeBehavior));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridSplitter.ShowsPreview" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.GridSplitter.ShowsPreview" /> dependency property.</returns>
		// Token: 0x04002B96 RID: 11158
		public static readonly DependencyProperty ShowsPreviewProperty = DependencyProperty.Register("ShowsPreview", typeof(bool), typeof(GridSplitter), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridSplitter.PreviewStyle" /> dependency property. </summary>
		// Token: 0x04002B97 RID: 11159
		public static readonly DependencyProperty PreviewStyleProperty = DependencyProperty.Register("PreviewStyle", typeof(Style), typeof(GridSplitter), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridSplitter.KeyboardIncrement" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.GridSplitter.KeyboardIncrement" /> dependency property.</returns>
		// Token: 0x04002B98 RID: 11160
		public static readonly DependencyProperty KeyboardIncrementProperty = DependencyProperty.Register("KeyboardIncrement", typeof(double), typeof(GridSplitter), new FrameworkPropertyMetadata(10.0), new ValidateValueCallback(GridSplitter.IsValidDelta));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridSplitter.DragIncrement" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.GridSplitter.DragIncrement" /> dependency property.</returns>
		// Token: 0x04002B99 RID: 11161
		public static readonly DependencyProperty DragIncrementProperty = DependencyProperty.Register("DragIncrement", typeof(double), typeof(GridSplitter), new FrameworkPropertyMetadata(1.0), new ValidateValueCallback(GridSplitter.IsValidDelta));

		// Token: 0x04002B9A RID: 11162
		private GridSplitter.ResizeData _resizeData;

		// Token: 0x04002B9B RID: 11163
		private static DependencyObjectType _dType;

		// Token: 0x02000988 RID: 2440
		private sealed class PreviewAdorner : Adorner
		{
			// Token: 0x060087A3 RID: 34723 RVA: 0x00250964 File Offset: 0x0024EB64
			public PreviewAdorner(GridSplitter gridSplitter, Style previewStyle) : base(gridSplitter)
			{
				Control control = new Control();
				control.Style = previewStyle;
				control.IsEnabled = false;
				this.Translation = new TranslateTransform();
				this._decorator = new Decorator();
				this._decorator.Child = control;
				this._decorator.RenderTransform = this.Translation;
				base.AddVisualChild(this._decorator);
			}

			// Token: 0x060087A4 RID: 34724 RVA: 0x002509CB File Offset: 0x0024EBCB
			protected override Visual GetVisualChild(int index)
			{
				if (index != 0)
				{
					throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
				}
				return this._decorator;
			}

			// Token: 0x17001EA2 RID: 7842
			// (get) Token: 0x060087A5 RID: 34725 RVA: 0x00016748 File Offset: 0x00014948
			protected override int VisualChildrenCount
			{
				get
				{
					return 1;
				}
			}

			// Token: 0x060087A6 RID: 34726 RVA: 0x002509F4 File Offset: 0x0024EBF4
			protected override Size ArrangeOverride(Size finalSize)
			{
				this._decorator.Arrange(new Rect(default(Point), finalSize));
				return finalSize;
			}

			// Token: 0x17001EA3 RID: 7843
			// (get) Token: 0x060087A7 RID: 34727 RVA: 0x00250A1C File Offset: 0x0024EC1C
			// (set) Token: 0x060087A8 RID: 34728 RVA: 0x00250A29 File Offset: 0x0024EC29
			public double OffsetX
			{
				get
				{
					return this.Translation.X;
				}
				set
				{
					this.Translation.X = value;
				}
			}

			// Token: 0x17001EA4 RID: 7844
			// (get) Token: 0x060087A9 RID: 34729 RVA: 0x00250A37 File Offset: 0x0024EC37
			// (set) Token: 0x060087AA RID: 34730 RVA: 0x00250A44 File Offset: 0x0024EC44
			public double OffsetY
			{
				get
				{
					return this.Translation.Y;
				}
				set
				{
					this.Translation.Y = value;
				}
			}

			// Token: 0x040044A6 RID: 17574
			private TranslateTransform Translation;

			// Token: 0x040044A7 RID: 17575
			private Decorator _decorator;
		}

		// Token: 0x02000989 RID: 2441
		private enum SplitBehavior
		{
			// Token: 0x040044A9 RID: 17577
			Split,
			// Token: 0x040044AA RID: 17578
			Resize1,
			// Token: 0x040044AB RID: 17579
			Resize2
		}

		// Token: 0x0200098A RID: 2442
		private class ResizeData
		{
			// Token: 0x040044AC RID: 17580
			public bool ShowsPreview;

			// Token: 0x040044AD RID: 17581
			public GridSplitter.PreviewAdorner Adorner;

			// Token: 0x040044AE RID: 17582
			public double MinChange;

			// Token: 0x040044AF RID: 17583
			public double MaxChange;

			// Token: 0x040044B0 RID: 17584
			public Grid Grid;

			// Token: 0x040044B1 RID: 17585
			public GridResizeDirection ResizeDirection;

			// Token: 0x040044B2 RID: 17586
			public GridResizeBehavior ResizeBehavior;

			// Token: 0x040044B3 RID: 17587
			public DefinitionBase Definition1;

			// Token: 0x040044B4 RID: 17588
			public DefinitionBase Definition2;

			// Token: 0x040044B5 RID: 17589
			public GridSplitter.SplitBehavior SplitBehavior;

			// Token: 0x040044B6 RID: 17590
			public int SplitterIndex;

			// Token: 0x040044B7 RID: 17591
			public int Definition1Index;

			// Token: 0x040044B8 RID: 17592
			public int Definition2Index;

			// Token: 0x040044B9 RID: 17593
			public GridLength OriginalDefinition1Length;

			// Token: 0x040044BA RID: 17594
			public GridLength OriginalDefinition2Length;

			// Token: 0x040044BB RID: 17595
			public double OriginalDefinition1ActualLength;

			// Token: 0x040044BC RID: 17596
			public double OriginalDefinition2ActualLength;

			// Token: 0x040044BD RID: 17597
			public double SplitterLength;
		}
	}
}
