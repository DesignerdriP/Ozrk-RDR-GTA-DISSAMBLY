﻿using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Shapes;
using MS.Internal;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Defines an area within which you can explicitly position child elements by using coordinates that are relative to the <see cref="T:System.Windows.Controls.Canvas" /> area.</summary>
	// Token: 0x0200047F RID: 1151
	public class Canvas : Panel
	{
		// Token: 0x06004329 RID: 17193 RVA: 0x00133370 File Offset: 0x00131570
		static Canvas()
		{
			ControlsTraceLogger.AddControl(TelemetryControls.Canvas);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.Canvas.Left" /> attached property for a given dependency object. </summary>
		/// <param name="element">The element from which the property value is read.</param>
		/// <returns>The <see cref="P:System.Windows.Controls.Canvas.Left" /> coordinate of the specified element.</returns>
		// Token: 0x0600432B RID: 17195 RVA: 0x001334C4 File Offset: 0x001316C4
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetLeft(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(Canvas.LeftProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.Canvas.Left" /> attached property for a given dependency object. </summary>
		/// <param name="element">The element to which the property value is written.</param>
		/// <param name="length">Sets the <see cref="P:System.Windows.Controls.Canvas.Left" /> coordinate of the specified element.</param>
		// Token: 0x0600432C RID: 17196 RVA: 0x001334E4 File Offset: 0x001316E4
		public static void SetLeft(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Canvas.LeftProperty, length);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.Canvas.Top" /> attached property for a given dependency object. </summary>
		/// <param name="element">The element from which the property value is read.</param>
		/// <returns>The <see cref="P:System.Windows.Controls.Canvas.Top" /> coordinate of the specified element.</returns>
		// Token: 0x0600432D RID: 17197 RVA: 0x00133505 File Offset: 0x00131705
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetTop(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(Canvas.TopProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.Canvas.Top" /> attached property for a given dependency object. </summary>
		/// <param name="element">The element to which the property value is written.</param>
		/// <param name="length">Sets the <see cref="P:System.Windows.Controls.Canvas.Top" /> coordinate of the specified element.</param>
		// Token: 0x0600432E RID: 17198 RVA: 0x00133525 File Offset: 0x00131725
		public static void SetTop(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Canvas.TopProperty, length);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.Canvas.Right" /> attached property for a given dependency object. </summary>
		/// <param name="element">The element from which the property value is read.</param>
		/// <returns>The <see cref="P:System.Windows.Controls.Canvas.Right" /> coordinate of the specified element.</returns>
		// Token: 0x0600432F RID: 17199 RVA: 0x00133546 File Offset: 0x00131746
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetRight(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(Canvas.RightProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.Canvas.Right" /> attached property for a given dependency object. </summary>
		/// <param name="element">The element to which the property value is written.</param>
		/// <param name="length">Sets the <see cref="P:System.Windows.Controls.Canvas.Right" /> coordinate of the specified element.</param>
		// Token: 0x06004330 RID: 17200 RVA: 0x00133566 File Offset: 0x00131766
		public static void SetRight(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Canvas.RightProperty, length);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Controls.Canvas.Bottom" /> attached property for a given dependency object. </summary>
		/// <param name="element">The element from which the property value is read.</param>
		/// <returns>The <see cref="P:System.Windows.Controls.Canvas.Bottom" /> coordinate of the specified element.</returns>
		// Token: 0x06004331 RID: 17201 RVA: 0x00133587 File Offset: 0x00131787
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetBottom(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(Canvas.BottomProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.Canvas.Bottom" /> attached property for a given dependency object. </summary>
		/// <param name="element">The element to which the property value is written.</param>
		/// <param name="length">Sets the <see cref="P:System.Windows.Controls.Canvas.Bottom" /> coordinate of the specified element.</param>
		// Token: 0x06004332 RID: 17202 RVA: 0x001335A7 File Offset: 0x001317A7
		public static void SetBottom(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Canvas.BottomProperty, length);
		}

		// Token: 0x06004333 RID: 17203 RVA: 0x001335C8 File Offset: 0x001317C8
		private static void OnPositioningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			UIElement uielement = d as UIElement;
			if (uielement != null)
			{
				Canvas canvas = VisualTreeHelper.GetParent(uielement) as Canvas;
				if (canvas != null)
				{
					canvas.InvalidateArrange();
				}
			}
		}

		/// <summary>Measures the child elements of a <see cref="T:System.Windows.Controls.Canvas" /> in anticipation of arranging them during the <see cref="M:System.Windows.Controls.Canvas.ArrangeOverride(System.Windows.Size)" /> pass.</summary>
		/// <param name="constraint">An upper limit <see cref="T:System.Windows.Size" /> that should not be exceeded.</param>
		/// <returns>A <see cref="T:System.Windows.Size" /> that represents the size that is required to arrange child content.</returns>
		// Token: 0x06004334 RID: 17204 RVA: 0x001335F4 File Offset: 0x001317F4
		protected override Size MeasureOverride(Size constraint)
		{
			Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				if (uielement != null)
				{
					uielement.Measure(availableSize);
				}
			}
			return default(Size);
		}

		/// <summary>Arranges the content of a <see cref="T:System.Windows.Controls.Canvas" /> element.</summary>
		/// <param name="arrangeSize">The size that this <see cref="T:System.Windows.Controls.Canvas" /> element should use to arrange its child elements.</param>
		/// <returns>A <see cref="T:System.Windows.Size" /> that represents the arranged size of this <see cref="T:System.Windows.Controls.Canvas" /> element and its descendants.</returns>
		// Token: 0x06004335 RID: 17205 RVA: 0x00133674 File Offset: 0x00131874
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				if (uielement != null)
				{
					double x = 0.0;
					double y = 0.0;
					double left = Canvas.GetLeft(uielement);
					if (!DoubleUtil.IsNaN(left))
					{
						x = left;
					}
					else
					{
						double right = Canvas.GetRight(uielement);
						if (!DoubleUtil.IsNaN(right))
						{
							x = arrangeSize.Width - uielement.DesiredSize.Width - right;
						}
					}
					double top = Canvas.GetTop(uielement);
					if (!DoubleUtil.IsNaN(top))
					{
						y = top;
					}
					else
					{
						double bottom = Canvas.GetBottom(uielement);
						if (!DoubleUtil.IsNaN(bottom))
						{
							y = arrangeSize.Height - uielement.DesiredSize.Height - bottom;
						}
					}
					uielement.Arrange(new Rect(new Point(x, y), uielement.DesiredSize));
				}
			}
			return arrangeSize;
		}

		/// <summary>Returns a clipping geometry that indicates the area that will be clipped if the <see cref="P:System.Windows.UIElement.ClipToBounds" /> property is set to <see langword="true" />. </summary>
		/// <param name="layoutSlotSize">The available size of the element.</param>
		/// <returns>A <see cref="T:System.Windows.Media.Geometry" /> that represents the area that is clipped if <see cref="P:System.Windows.UIElement.ClipToBounds" /> is <see langword="true" />.</returns>
		// Token: 0x06004336 RID: 17206 RVA: 0x00133788 File Offset: 0x00131988
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			if (base.ClipToBounds)
			{
				return new RectangleGeometry(new Rect(base.RenderSize));
			}
			return null;
		}

		// Token: 0x1700107E RID: 4222
		// (get) Token: 0x06004337 RID: 17207 RVA: 0x000956EF File Offset: 0x000938EF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 9;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Canvas.Left" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Canvas.Left" /> attached property.</returns>
		// Token: 0x04002832 RID: 10290
		public static readonly DependencyProperty LeftProperty = DependencyProperty.RegisterAttached("Left", typeof(double), typeof(Canvas), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(Canvas.OnPositioningChanged)), new ValidateValueCallback(Shape.IsDoubleFiniteOrNaN));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Canvas.Top" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Canvas.Top" /> attached property.</returns>
		// Token: 0x04002833 RID: 10291
		public static readonly DependencyProperty TopProperty = DependencyProperty.RegisterAttached("Top", typeof(double), typeof(Canvas), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(Canvas.OnPositioningChanged)), new ValidateValueCallback(Shape.IsDoubleFiniteOrNaN));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Canvas.Right" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Canvas.Right" /> attached property.</returns>
		// Token: 0x04002834 RID: 10292
		public static readonly DependencyProperty RightProperty = DependencyProperty.RegisterAttached("Right", typeof(double), typeof(Canvas), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(Canvas.OnPositioningChanged)), new ValidateValueCallback(Shape.IsDoubleFiniteOrNaN));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Canvas.Bottom" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Canvas.Bottom" /> attached property.</returns>
		// Token: 0x04002835 RID: 10293
		public static readonly DependencyProperty BottomProperty = DependencyProperty.RegisterAttached("Bottom", typeof(double), typeof(Canvas), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(Canvas.OnPositioningChanged)), new ValidateValueCallback(Shape.IsDoubleFiniteOrNaN));
	}
}
