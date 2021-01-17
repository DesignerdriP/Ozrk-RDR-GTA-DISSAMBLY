using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents the container that handles the layout of a <see cref="T:System.Windows.Controls.ToolBar" />. </summary>
	// Token: 0x02000545 RID: 1349
	[ContentProperty("ToolBars")]
	public class ToolBarTray : FrameworkElement, IAddChild
	{
		// Token: 0x06005844 RID: 22596 RVA: 0x00186E38 File Offset: 0x00185038
		static ToolBarTray()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolBarTray), new FrameworkPropertyMetadata(typeof(ToolBarTray)));
			ToolBarTray._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ToolBarTray));
			EventManager.RegisterClassHandler(typeof(ToolBarTray), Thumb.DragDeltaEvent, new DragDeltaEventHandler(ToolBarTray.OnThumbDragDelta));
			KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeof(ToolBarTray), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			ControlsTraceLogger.AddControl(TelemetryControls.ToolBarTray);
		}

		/// <summary>Gets or sets a brush to use for the background color of the <see cref="T:System.Windows.Controls.ToolBarTray" />.   </summary>
		/// <returns>A brush to use for the background color of the <see cref="T:System.Windows.Controls.ToolBarTray" />.</returns>
		// Token: 0x17001584 RID: 5508
		// (get) Token: 0x06005846 RID: 22598 RVA: 0x00186F7B File Offset: 0x0018517B
		// (set) Token: 0x06005847 RID: 22599 RVA: 0x00186F8D File Offset: 0x0018518D
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(ToolBarTray.BackgroundProperty);
			}
			set
			{
				base.SetValue(ToolBarTray.BackgroundProperty, value);
			}
		}

		// Token: 0x06005848 RID: 22600 RVA: 0x00186F9C File Offset: 0x0018519C
		private static void OnOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Collection<ToolBar> toolBars = ((ToolBarTray)d).ToolBars;
			for (int i = 0; i < toolBars.Count; i++)
			{
				toolBars[i].CoerceValue(ToolBar.OrientationProperty);
			}
		}

		/// <summary>Specifies the orientation of a <see cref="T:System.Windows.Controls.ToolBarTray" />.   </summary>
		/// <returns>One of the <see cref="T:System.Windows.Controls.Orientation" /> values. The default is <see cref="F:System.Windows.Controls.Orientation.Horizontal" />.</returns>
		// Token: 0x17001585 RID: 5509
		// (get) Token: 0x06005849 RID: 22601 RVA: 0x00186FD7 File Offset: 0x001851D7
		// (set) Token: 0x0600584A RID: 22602 RVA: 0x00186FE9 File Offset: 0x001851E9
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(ToolBarTray.OrientationProperty);
			}
			set
			{
				base.SetValue(ToolBarTray.OrientationProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a <see cref="T:System.Windows.Controls.ToolBar" /> can be moved inside a <see cref="T:System.Windows.Controls.ToolBarTray" />.   </summary>
		/// <returns>
		///     <see langword="true" /> if the toolbar cannot be moved inside the toolbar tray; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001586 RID: 5510
		// (get) Token: 0x0600584B RID: 22603 RVA: 0x00186FFC File Offset: 0x001851FC
		// (set) Token: 0x0600584C RID: 22604 RVA: 0x0018700E File Offset: 0x0018520E
		public bool IsLocked
		{
			get
			{
				return (bool)base.GetValue(ToolBarTray.IsLockedProperty);
			}
			set
			{
				base.SetValue(ToolBarTray.IsLockedProperty, value);
			}
		}

		/// <summary>Writes the value of the <see cref="P:System.Windows.Controls.ToolBarTray.IsLocked" /> property to the specified element. </summary>
		/// <param name="element">The element to write the property to.</param>
		/// <param name="value">The property value to set.</param>
		// Token: 0x0600584D RID: 22605 RVA: 0x0018701C File Offset: 0x0018521C
		public static void SetIsLocked(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolBarTray.IsLockedProperty, value);
		}

		/// <summary>Reads the value of the <see cref="P:System.Windows.Controls.ToolBarTray.IsLocked" /> property from the specified element. </summary>
		/// <param name="element">The element from which to read the property.</param>
		/// <returns>
		///     <see langword="true" /> if the toolbar cannot be moved inside the toolbar tray; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x0600584E RID: 22606 RVA: 0x00187038 File Offset: 0x00185238
		public static bool GetIsLocked(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ToolBarTray.IsLockedProperty);
		}

		/// <summary>Gets the collection of <see cref="T:System.Windows.Controls.ToolBar" /> elements in the <see cref="T:System.Windows.Controls.ToolBarTray" />.</summary>
		/// <returns>A collection of <see cref="T:System.Windows.Controls.ToolBar" /> objects.</returns>
		// Token: 0x17001587 RID: 5511
		// (get) Token: 0x0600584F RID: 22607 RVA: 0x00187058 File Offset: 0x00185258
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Collection<ToolBar> ToolBars
		{
			get
			{
				if (this._toolBarsCollection == null)
				{
					this._toolBarsCollection = new ToolBarTray.ToolBarCollection(this);
				}
				return this._toolBarsCollection;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value">  An object to add as a child.</param>
		// Token: 0x06005850 RID: 22608 RVA: 0x00187074 File Offset: 0x00185274
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			ToolBar toolBar = value as ToolBar;
			if (toolBar == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(ToolBar)
				}), "value");
			}
			this.ToolBars.Add(toolBar);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="text">  A string to add to the object.</param>
		// Token: 0x06005851 RID: 22609 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Gets an enumerator to the logical child elements of a <see cref="T:System.Windows.Controls.ToolBarTray" />. </summary>
		/// <returns>An enumerator to the children of a <see cref="T:System.Windows.Controls.ToolBarTray" /> element.</returns>
		// Token: 0x17001588 RID: 5512
		// (get) Token: 0x06005852 RID: 22610 RVA: 0x001870D6 File Offset: 0x001852D6
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this.VisualChildrenCount == 0)
				{
					return EmptyEnumerator.Instance;
				}
				return this.ToolBars.GetEnumerator();
			}
		}

		/// <summary>Called when a <see cref="T:System.Windows.Controls.ToolBarTray" /> is displayed to get the Drawing Context (DC) to use to render the <see cref="T:System.Windows.Controls.ToolBarTray" />.</summary>
		/// <param name="dc">Drawing context to use to render the <see cref="T:System.Windows.Controls.ToolBarTray" />.</param>
		// Token: 0x06005853 RID: 22611 RVA: 0x001870F4 File Offset: 0x001852F4
		protected override void OnRender(DrawingContext dc)
		{
			Brush background = this.Background;
			if (background != null)
			{
				dc.DrawRectangle(background, null, new Rect(0.0, 0.0, base.RenderSize.Width, base.RenderSize.Height));
			}
		}

		/// <summary>Called to remeasure a <see cref="T:System.Windows.Controls.ToolBarTray" />. </summary>
		/// <param name="constraint">The measurement constraints; a <see cref="T:System.Windows.Controls.ToolBarTray" /> cannot return a size larger than the constraint.</param>
		/// <returns>The size of the control.</returns>
		// Token: 0x06005854 RID: 22612 RVA: 0x00187148 File Offset: 0x00185348
		protected override Size MeasureOverride(Size constraint)
		{
			this.GenerateBands();
			Size result = default(Size);
			bool flag = this.Orientation == Orientation.Horizontal;
			Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
			for (int i = 0; i < this._bands.Count; i++)
			{
				double num = flag ? constraint.Width : constraint.Height;
				List<ToolBar> band = this._bands[i].Band;
				double num2 = 0.0;
				double num3 = 0.0;
				for (int j = 0; j < band.Count; j++)
				{
					ToolBar toolBar = band[j];
					num -= toolBar.MinLength;
					if (DoubleUtil.LessThan(num, 0.0))
					{
						num = 0.0;
						break;
					}
				}
				for (int j = 0; j < band.Count; j++)
				{
					ToolBar toolBar2 = band[j];
					num += toolBar2.MinLength;
					if (flag)
					{
						availableSize.Width = num;
					}
					else
					{
						availableSize.Height = num;
					}
					toolBar2.Measure(availableSize);
					num2 = Math.Max(num2, flag ? toolBar2.DesiredSize.Height : toolBar2.DesiredSize.Width);
					num3 += (flag ? toolBar2.DesiredSize.Width : toolBar2.DesiredSize.Height);
					num -= (flag ? toolBar2.DesiredSize.Width : toolBar2.DesiredSize.Height);
					if (DoubleUtil.LessThan(num, 0.0))
					{
						num = 0.0;
					}
				}
				this._bands[i].Thickness = num2;
				if (flag)
				{
					result.Height += num2;
					result.Width = Math.Max(result.Width, num3);
				}
				else
				{
					result.Width += num2;
					result.Height = Math.Max(result.Height, num3);
				}
			}
			return result;
		}

		/// <summary> Called to arrange and size its <see cref="T:System.Windows.Controls.ToolBar" /> children. </summary>
		/// <param name="arrangeSize">The size that the <see cref="T:System.Windows.Controls.ToolBarTray" /> assumes to position its children.</param>
		/// <returns>The size of the control.</returns>
		// Token: 0x06005855 RID: 22613 RVA: 0x00187378 File Offset: 0x00185578
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			bool flag = this.Orientation == Orientation.Horizontal;
			Rect finalRect = default(Rect);
			for (int i = 0; i < this._bands.Count; i++)
			{
				List<ToolBar> band = this._bands[i].Band;
				double thickness = this._bands[i].Thickness;
				if (flag)
				{
					finalRect.X = 0.0;
				}
				else
				{
					finalRect.Y = 0.0;
				}
				for (int j = 0; j < band.Count; j++)
				{
					ToolBar toolBar = band[j];
					Size size = new Size(flag ? toolBar.DesiredSize.Width : thickness, flag ? thickness : toolBar.DesiredSize.Height);
					finalRect.Size = size;
					toolBar.Arrange(finalRect);
					if (flag)
					{
						finalRect.X += size.Width;
					}
					else
					{
						finalRect.Y += size.Height;
					}
				}
				if (flag)
				{
					finalRect.Y += thickness;
				}
				else
				{
					finalRect.X += thickness;
				}
			}
			return arrangeSize;
		}

		/// <summary>Gets the number of children that are currently visible.</summary>
		/// <returns>The number of visible <see cref="T:System.Windows.Controls.ToolBar" /> objects in the <see cref="T:System.Windows.Controls.ToolBarTray" />.</returns>
		// Token: 0x17001589 RID: 5513
		// (get) Token: 0x06005856 RID: 22614 RVA: 0x001874BB File Offset: 0x001856BB
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._toolBarsCollection == null)
				{
					return 0;
				}
				return this._toolBarsCollection.Count;
			}
		}

		/// <summary>Gets the index number of the visible child.</summary>
		/// <param name="index">Index of the visual child.</param>
		/// <returns>The index number of the visible child.</returns>
		// Token: 0x06005857 RID: 22615 RVA: 0x001874D2 File Offset: 0x001856D2
		protected override Visual GetVisualChild(int index)
		{
			if (this._toolBarsCollection == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._toolBarsCollection[index];
		}

		// Token: 0x06005858 RID: 22616 RVA: 0x00187504 File Offset: 0x00185704
		private static void OnThumbDragDelta(object sender, DragDeltaEventArgs e)
		{
			ToolBarTray toolBarTray = (ToolBarTray)sender;
			if (toolBarTray.IsLocked)
			{
				return;
			}
			toolBarTray.ProcessThumbDragDelta(e);
		}

		// Token: 0x06005859 RID: 22617 RVA: 0x00187528 File Offset: 0x00185728
		private void ProcessThumbDragDelta(DragDeltaEventArgs e)
		{
			Thumb thumb = e.OriginalSource as Thumb;
			if (thumb != null)
			{
				ToolBar toolBar = thumb.TemplatedParent as ToolBar;
				if (toolBar != null && toolBar.Parent == this)
				{
					if (this._bandsDirty)
					{
						this.GenerateBands();
					}
					bool flag = this.Orientation == Orientation.Horizontal;
					int band = toolBar.Band;
					Point position = Mouse.PrimaryDevice.GetPosition(this);
					Point point = this.TransformPointToToolBar(toolBar, position);
					int bandFromOffset = this.GetBandFromOffset(flag ? position.Y : position.X);
					double num = flag ? e.HorizontalChange : e.VerticalChange;
					double num2;
					if (flag)
					{
						num2 = position.X - point.X;
					}
					else
					{
						num2 = position.Y - point.Y;
					}
					double num3 = num2 + num;
					if (bandFromOffset == band)
					{
						List<ToolBar> band2 = this._bands[band].Band;
						int bandIndex = toolBar.BandIndex;
						if (DoubleUtil.LessThan(num, 0.0))
						{
							double num4 = this.ToolBarsTotalMinimum(band2, 0, bandIndex - 1);
							if (DoubleUtil.LessThanOrClose(num4, num3))
							{
								this.ShrinkToolBars(band2, 0, bandIndex - 1, -num);
							}
							else if (bandIndex > 0)
							{
								ToolBar toolBar2 = band2[bandIndex - 1];
								Point point2 = this.TransformPointToToolBar(toolBar2, position);
								if (DoubleUtil.LessThan(flag ? point2.X : point2.Y, 0.0))
								{
									toolBar2.BandIndex = bandIndex;
									band2[bandIndex] = toolBar2;
									toolBar.BandIndex = bandIndex - 1;
									band2[bandIndex - 1] = toolBar;
									if (bandIndex + 1 == band2.Count)
									{
										toolBar2.ClearValue(flag ? FrameworkElement.WidthProperty : FrameworkElement.HeightProperty);
									}
								}
								else if (flag)
								{
									if (DoubleUtil.LessThan(num4, position.X - point.X))
									{
										this.ShrinkToolBars(band2, 0, bandIndex - 1, position.X - point.X - num4);
									}
								}
								else if (DoubleUtil.LessThan(num4, position.Y - point.Y))
								{
									this.ShrinkToolBars(band2, 0, bandIndex - 1, position.Y - point.Y - num4);
								}
							}
						}
						else
						{
							double value = this.ToolBarsTotalMaximum(band2, 0, bandIndex - 1);
							if (DoubleUtil.GreaterThan(value, num3))
							{
								this.ExpandToolBars(band2, 0, bandIndex - 1, num);
							}
							else if (bandIndex < band2.Count - 1)
							{
								ToolBar toolBar3 = band2[bandIndex + 1];
								Point point3 = this.TransformPointToToolBar(toolBar3, position);
								if (DoubleUtil.GreaterThanOrClose(flag ? point3.X : point3.Y, 0.0))
								{
									toolBar3.BandIndex = bandIndex;
									band2[bandIndex] = toolBar3;
									toolBar.BandIndex = bandIndex + 1;
									band2[bandIndex + 1] = toolBar;
									if (bandIndex + 2 == band2.Count)
									{
										toolBar.ClearValue(flag ? FrameworkElement.WidthProperty : FrameworkElement.HeightProperty);
									}
								}
								else
								{
									this.ExpandToolBars(band2, 0, bandIndex - 1, num);
								}
							}
							else
							{
								this.ExpandToolBars(band2, 0, bandIndex - 1, num);
							}
						}
					}
					else
					{
						this._bandsDirty = true;
						toolBar.Band = bandFromOffset;
						toolBar.ClearValue(flag ? FrameworkElement.WidthProperty : FrameworkElement.HeightProperty);
						if (bandFromOffset >= 0 && bandFromOffset < this._bands.Count)
						{
							this.MoveToolBar(toolBar, bandFromOffset, num3);
						}
						List<ToolBar> band3 = this._bands[band].Band;
						for (int i = 0; i < band3.Count; i++)
						{
							ToolBar toolBar4 = band3[i];
							toolBar4.ClearValue(flag ? FrameworkElement.WidthProperty : FrameworkElement.HeightProperty);
						}
					}
					e.Handled = true;
				}
			}
		}

		// Token: 0x0600585A RID: 22618 RVA: 0x00187904 File Offset: 0x00185B04
		private Point TransformPointToToolBar(ToolBar toolBar, Point point)
		{
			Point result = point;
			GeneralTransform generalTransform = base.TransformToDescendant(toolBar);
			if (generalTransform != null)
			{
				generalTransform.TryTransform(point, out result);
			}
			return result;
		}

		// Token: 0x0600585B RID: 22619 RVA: 0x0018792C File Offset: 0x00185B2C
		private void ShrinkToolBars(List<ToolBar> band, int startIndex, int endIndex, double shrinkAmount)
		{
			if (this.Orientation == Orientation.Horizontal)
			{
				for (int i = endIndex; i >= startIndex; i--)
				{
					ToolBar toolBar = band[i];
					if (DoubleUtil.GreaterThanOrClose(toolBar.RenderSize.Width - shrinkAmount, toolBar.MinLength))
					{
						toolBar.Width = toolBar.RenderSize.Width - shrinkAmount;
						return;
					}
					toolBar.Width = toolBar.MinLength;
					shrinkAmount -= toolBar.RenderSize.Width - toolBar.MinLength;
				}
				return;
			}
			for (int j = endIndex; j >= startIndex; j--)
			{
				ToolBar toolBar2 = band[j];
				if (DoubleUtil.GreaterThanOrClose(toolBar2.RenderSize.Height - shrinkAmount, toolBar2.MinLength))
				{
					toolBar2.Height = toolBar2.RenderSize.Height - shrinkAmount;
					return;
				}
				toolBar2.Height = toolBar2.MinLength;
				shrinkAmount -= toolBar2.RenderSize.Height - toolBar2.MinLength;
			}
		}

		// Token: 0x0600585C RID: 22620 RVA: 0x00187A2C File Offset: 0x00185C2C
		private double ToolBarsTotalMinimum(List<ToolBar> band, int startIndex, int endIndex)
		{
			double num = 0.0;
			for (int i = startIndex; i <= endIndex; i++)
			{
				num += band[i].MinLength;
			}
			return num;
		}

		// Token: 0x0600585D RID: 22621 RVA: 0x00187A60 File Offset: 0x00185C60
		private void ExpandToolBars(List<ToolBar> band, int startIndex, int endIndex, double expandAmount)
		{
			if (this.Orientation == Orientation.Horizontal)
			{
				for (int i = endIndex; i >= startIndex; i--)
				{
					ToolBar toolBar = band[i];
					if (DoubleUtil.LessThanOrClose(toolBar.RenderSize.Width + expandAmount, toolBar.MaxLength))
					{
						toolBar.Width = toolBar.RenderSize.Width + expandAmount;
						return;
					}
					toolBar.Width = toolBar.MaxLength;
					expandAmount -= toolBar.MaxLength - toolBar.RenderSize.Width;
				}
				return;
			}
			for (int j = endIndex; j >= startIndex; j--)
			{
				ToolBar toolBar2 = band[j];
				if (DoubleUtil.LessThanOrClose(toolBar2.RenderSize.Height + expandAmount, toolBar2.MaxLength))
				{
					toolBar2.Height = toolBar2.RenderSize.Height + expandAmount;
					return;
				}
				toolBar2.Height = toolBar2.MaxLength;
				expandAmount -= toolBar2.MaxLength - toolBar2.RenderSize.Height;
			}
		}

		// Token: 0x0600585E RID: 22622 RVA: 0x00187B60 File Offset: 0x00185D60
		private double ToolBarsTotalMaximum(List<ToolBar> band, int startIndex, int endIndex)
		{
			double num = 0.0;
			for (int i = startIndex; i <= endIndex; i++)
			{
				num += band[i].MaxLength;
			}
			return num;
		}

		// Token: 0x0600585F RID: 22623 RVA: 0x00187B94 File Offset: 0x00185D94
		private void MoveToolBar(ToolBar toolBar, int newBandNumber, double position)
		{
			bool flag = this.Orientation == Orientation.Horizontal;
			List<ToolBar> band = this._bands[newBandNumber].Band;
			if (DoubleUtil.LessThanOrClose(position, 0.0))
			{
				toolBar.BandIndex = -1;
				return;
			}
			double num = 0.0;
			int num2 = -1;
			int i;
			for (i = 0; i < band.Count; i++)
			{
				ToolBar toolBar2 = band[i];
				if (num2 == -1)
				{
					num += (flag ? toolBar2.RenderSize.Width : toolBar2.RenderSize.Height);
					if (DoubleUtil.GreaterThan(num, position))
					{
						num2 = i + 1;
						toolBar.BandIndex = num2;
						if (flag)
						{
							toolBar2.Width = Math.Max(toolBar2.MinLength, toolBar2.RenderSize.Width - num + position);
						}
						else
						{
							toolBar2.Height = Math.Max(toolBar2.MinLength, toolBar2.RenderSize.Height - num + position);
						}
					}
				}
				else
				{
					toolBar2.BandIndex = i + 1;
				}
			}
			if (num2 == -1)
			{
				toolBar.BandIndex = i;
			}
		}

		// Token: 0x06005860 RID: 22624 RVA: 0x00187CB4 File Offset: 0x00185EB4
		private int GetBandFromOffset(double toolBarOffset)
		{
			if (DoubleUtil.LessThan(toolBarOffset, 0.0))
			{
				return -1;
			}
			double num = 0.0;
			for (int i = 0; i < this._bands.Count; i++)
			{
				num += this._bands[i].Thickness;
				if (DoubleUtil.GreaterThan(num, toolBarOffset))
				{
					return i;
				}
			}
			return this._bands.Count;
		}

		// Token: 0x06005861 RID: 22625 RVA: 0x00187D20 File Offset: 0x00185F20
		private void GenerateBands()
		{
			if (!this.IsBandsDirty())
			{
				return;
			}
			Collection<ToolBar> toolBars = this.ToolBars;
			this._bands.Clear();
			for (int i = 0; i < toolBars.Count; i++)
			{
				this.InsertBand(toolBars[i], i);
			}
			for (int j = 0; j < this._bands.Count; j++)
			{
				List<ToolBar> band = this._bands[j].Band;
				for (int k = 0; k < band.Count; k++)
				{
					ToolBar toolBar = band[k];
					toolBar.Band = j;
					toolBar.BandIndex = k;
				}
			}
			this._bandsDirty = false;
		}

		// Token: 0x06005862 RID: 22626 RVA: 0x00187DC8 File Offset: 0x00185FC8
		private bool IsBandsDirty()
		{
			if (this._bandsDirty)
			{
				return true;
			}
			int num = 0;
			Collection<ToolBar> toolBars = this.ToolBars;
			for (int i = 0; i < this._bands.Count; i++)
			{
				List<ToolBar> band = this._bands[i].Band;
				for (int j = 0; j < band.Count; j++)
				{
					ToolBar toolBar = band[j];
					if (toolBar.Band != i || toolBar.BandIndex != j || !toolBars.Contains(toolBar))
					{
						return true;
					}
				}
				num += band.Count;
			}
			return num != toolBars.Count;
		}

		// Token: 0x06005863 RID: 22627 RVA: 0x00187E68 File Offset: 0x00186068
		private void InsertBand(ToolBar toolBar, int toolBarIndex)
		{
			int band = toolBar.Band;
			for (int i = 0; i < this._bands.Count; i++)
			{
				int band2 = this._bands[i].Band[0].Band;
				if (band == band2)
				{
					return;
				}
				if (band < band2)
				{
					this._bands.Insert(i, this.CreateBand(toolBarIndex));
					return;
				}
			}
			this._bands.Add(this.CreateBand(toolBarIndex));
		}

		// Token: 0x06005864 RID: 22628 RVA: 0x00187EE0 File Offset: 0x001860E0
		private ToolBarTray.BandInfo CreateBand(int startIndex)
		{
			Collection<ToolBar> toolBars = this.ToolBars;
			ToolBarTray.BandInfo bandInfo = new ToolBarTray.BandInfo();
			ToolBar toolBar = toolBars[startIndex];
			bandInfo.Band.Add(toolBar);
			int band = toolBar.Band;
			for (int i = startIndex + 1; i < toolBars.Count; i++)
			{
				toolBar = toolBars[i];
				if (band == toolBar.Band)
				{
					this.InsertToolBar(toolBar, bandInfo.Band);
				}
			}
			return bandInfo;
		}

		// Token: 0x06005865 RID: 22629 RVA: 0x00187F4C File Offset: 0x0018614C
		private void InsertToolBar(ToolBar toolBar, List<ToolBar> band)
		{
			for (int i = 0; i < band.Count; i++)
			{
				if (toolBar.BandIndex < band[i].BandIndex)
				{
					band.Insert(i, toolBar);
					return;
				}
			}
			band.Add(toolBar);
		}

		// Token: 0x1700158A RID: 5514
		// (get) Token: 0x06005866 RID: 22630 RVA: 0x00187F8E File Offset: 0x0018618E
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ToolBarTray._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolBarTray.Background" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolBarTray.Background" /> dependency property.</returns>
		// Token: 0x04002EBC RID: 11964
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(ToolBarTray), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolBarTray.Orientation" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolBarTray.Orientation" /> dependency property.</returns>
		// Token: 0x04002EBD RID: 11965
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ToolBarTray), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsParentMeasure, new PropertyChangedCallback(ToolBarTray.OnOrientationPropertyChanged)), new ValidateValueCallback(ScrollBar.IsValidOrientation));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolBarTray.IsLocked" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolBarTray.IsLocked" /> dependency property.</returns>
		// Token: 0x04002EBE RID: 11966
		public static readonly DependencyProperty IsLockedProperty = DependencyProperty.RegisterAttached("IsLocked", typeof(bool), typeof(ToolBarTray), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04002EBF RID: 11967
		private List<ToolBarTray.BandInfo> _bands = new List<ToolBarTray.BandInfo>(0);

		// Token: 0x04002EC0 RID: 11968
		private bool _bandsDirty = true;

		// Token: 0x04002EC1 RID: 11969
		private ToolBarTray.ToolBarCollection _toolBarsCollection;

		// Token: 0x04002EC2 RID: 11970
		private static DependencyObjectType _dType;

		// Token: 0x020009C1 RID: 2497
		private class ToolBarCollection : Collection<ToolBar>
		{
			// Token: 0x06008887 RID: 34951 RVA: 0x00252431 File Offset: 0x00250631
			public ToolBarCollection(ToolBarTray parent)
			{
				this._parent = parent;
			}

			// Token: 0x06008888 RID: 34952 RVA: 0x00252440 File Offset: 0x00250640
			protected override void InsertItem(int index, ToolBar toolBar)
			{
				base.InsertItem(index, toolBar);
				this._parent.AddLogicalChild(toolBar);
				this._parent.AddVisualChild(toolBar);
				this._parent.InvalidateMeasure();
			}

			// Token: 0x06008889 RID: 34953 RVA: 0x00252470 File Offset: 0x00250670
			protected override void SetItem(int index, ToolBar toolBar)
			{
				ToolBar toolBar2 = base.Items[index];
				if (toolBar != toolBar2)
				{
					base.SetItem(index, toolBar);
					this._parent.RemoveVisualChild(toolBar2);
					this._parent.RemoveLogicalChild(toolBar2);
					this._parent.AddLogicalChild(toolBar);
					this._parent.AddVisualChild(toolBar);
					this._parent.InvalidateMeasure();
				}
			}

			// Token: 0x0600888A RID: 34954 RVA: 0x002524D4 File Offset: 0x002506D4
			protected override void RemoveItem(int index)
			{
				ToolBar child = base[index];
				base.RemoveItem(index);
				this._parent.RemoveVisualChild(child);
				this._parent.RemoveLogicalChild(child);
				this._parent.InvalidateMeasure();
			}

			// Token: 0x0600888B RID: 34955 RVA: 0x00252514 File Offset: 0x00250714
			protected override void ClearItems()
			{
				int count = base.Count;
				if (count > 0)
				{
					for (int i = 0; i < count; i++)
					{
						ToolBar child = base[i];
						this._parent.RemoveVisualChild(child);
						this._parent.RemoveLogicalChild(child);
					}
					this._parent.InvalidateMeasure();
				}
				base.ClearItems();
			}

			// Token: 0x04004581 RID: 17793
			private readonly ToolBarTray _parent;
		}

		// Token: 0x020009C2 RID: 2498
		private class BandInfo
		{
			// Token: 0x17001ED3 RID: 7891
			// (get) Token: 0x0600888D RID: 34957 RVA: 0x0025257C File Offset: 0x0025077C
			public List<ToolBar> Band
			{
				get
				{
					return this._band;
				}
			}

			// Token: 0x17001ED4 RID: 7892
			// (get) Token: 0x0600888E RID: 34958 RVA: 0x00252584 File Offset: 0x00250784
			// (set) Token: 0x0600888F RID: 34959 RVA: 0x0025258C File Offset: 0x0025078C
			public double Thickness
			{
				get
				{
					return this._thickness;
				}
				set
				{
					this._thickness = value;
				}
			}

			// Token: 0x04004582 RID: 17794
			private List<ToolBar> _band = new List<ToolBar>();

			// Token: 0x04004583 RID: 17795
			private double _thickness;
		}
	}
}
