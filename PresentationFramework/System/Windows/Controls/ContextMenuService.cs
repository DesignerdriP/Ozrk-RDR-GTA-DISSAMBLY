using System;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Provides the system implementation for displaying a <see cref="T:System.Windows.Controls.ContextMenu" />. </summary>
	// Token: 0x0200048B RID: 1163
	public static class ContextMenuService
	{
		/// <summary> Gets the value of the <see cref="P:System.Windows.Controls.ContextMenuService.ContextMenu" /> property of the specified object. </summary>
		/// <param name="element">Object to query concerning the <see cref="P:System.Windows.FrameworkElement.ContextMenu" /> property.</param>
		/// <returns>Value of the <see cref="P:System.Windows.Controls.ContextMenuService.ContextMenu" /> property.</returns>
		// Token: 0x0600445D RID: 17501 RVA: 0x001372C8 File Offset: 0x001354C8
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static ContextMenu GetContextMenu(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			ContextMenu contextMenu = (ContextMenu)element.GetValue(ContextMenuService.ContextMenuProperty);
			if (contextMenu != null && element.Dispatcher != contextMenu.Dispatcher)
			{
				throw new ArgumentException(SR.Get("ContextMenuInDifferentDispatcher"));
			}
			return contextMenu;
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.ContextMenuService.ContextMenu" /> property of the specified object.</summary>
		/// <param name="element">Object to set the property on.</param>
		/// <param name="value">Value to set.</param>
		// Token: 0x0600445E RID: 17502 RVA: 0x00137316 File Offset: 0x00135516
		public static void SetContextMenu(DependencyObject element, ContextMenu value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.ContextMenuProperty, value);
		}

		/// <summary> Gets the value of the <see cref="P:System.Windows.Controls.ContextMenuService.HorizontalOffset" /> property of the specified object. </summary>
		/// <param name="element">Object to query concerning the <see cref="P:System.Windows.Controls.ContextMenuService.HorizontalOffset" /> property.</param>
		/// <returns>Value of the <see cref="P:System.Windows.Controls.ContextMenuService.HorizontalOffset" /> property.</returns>
		// Token: 0x0600445F RID: 17503 RVA: 0x00137332 File Offset: 0x00135532
		[TypeConverter(typeof(LengthConverter))]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static double GetHorizontalOffset(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(ContextMenuService.HorizontalOffsetProperty);
		}

		/// <summary> Sets the value of the <see cref="P:System.Windows.Controls.ContextMenuService.HorizontalOffset" /> property of the specified object. </summary>
		/// <param name="element">Object to set the value on.</param>
		/// <param name="value">Value to set.</param>
		// Token: 0x06004460 RID: 17504 RVA: 0x00137352 File Offset: 0x00135552
		public static void SetHorizontalOffset(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.HorizontalOffsetProperty, value);
		}

		/// <summary> Gets the value of the <see cref="P:System.Windows.Controls.ContextMenuService.VerticalOffset" /> property of the specified object. </summary>
		/// <param name="element">Object to query concerning the <see cref="P:System.Windows.Controls.ContextMenuService.VerticalOffset" /> property.</param>
		/// <returns>Value of the <see cref="P:System.Windows.Controls.ContextMenuService.VerticalOffset" /> property.</returns>
		// Token: 0x06004461 RID: 17505 RVA: 0x00137373 File Offset: 0x00135573
		[TypeConverter(typeof(LengthConverter))]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static double GetVerticalOffset(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(ContextMenuService.VerticalOffsetProperty);
		}

		/// <summary> Sets the value of the <see cref="P:System.Windows.Controls.ContextMenuService.VerticalOffset" /> property of the specified object. </summary>
		/// <param name="element">Object to set value on.</param>
		/// <param name="value">Value to set.</param>
		// Token: 0x06004462 RID: 17506 RVA: 0x00137393 File Offset: 0x00135593
		public static void SetVerticalOffset(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.VerticalOffsetProperty, value);
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.ContextMenu" /> has a drop shadow. </summary>
		/// <param name="element">Object to query concerning whether it has a drop shadow.</param>
		/// <returns>A Boolean value, true if the <see cref="T:System.Windows.Controls.ContextMenu" /> has a drop shadow; false otherwise.</returns>
		// Token: 0x06004463 RID: 17507 RVA: 0x001373B4 File Offset: 0x001355B4
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetHasDropShadow(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ContextMenuService.HasDropShadowProperty);
		}

		/// <summary>Sets a value that indicates whether the <see cref="T:System.Windows.Controls.ContextMenu" /> has a drop shadow.</summary>
		/// <param name="element">Object to set the property on.</param>
		/// <param name="value">Boolean value to set, <see langword="true" /> if the <see cref="T:System.Windows.Controls.ContextMenu" /> has a drop shadow; <see langword="false" /> otherwise.</param>
		// Token: 0x06004464 RID: 17508 RVA: 0x001373D4 File Offset: 0x001355D4
		public static void SetHasDropShadow(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.HasDropShadowProperty, BooleanBoxes.Box(value));
		}

		/// <summary> Gets the value of the <see cref="P:System.Windows.Controls.ContextMenuService.PlacementTarget" /> property of the specified object. </summary>
		/// <param name="element">Object to query concerning the <see cref="P:System.Windows.Controls.ContextMenuService.PlacementTarget" /> property.</param>
		/// <returns>Value of the <see cref="P:System.Windows.Controls.ContextMenuService.PlacementTarget" /> property.</returns>
		// Token: 0x06004465 RID: 17509 RVA: 0x001373F5 File Offset: 0x001355F5
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static UIElement GetPlacementTarget(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (UIElement)element.GetValue(ContextMenuService.PlacementTargetProperty);
		}

		/// <summary> Sets the value of the <see cref="P:System.Windows.Controls.ContextMenuService.PlacementTarget" /> property of the specified object. </summary>
		/// <param name="element">Object to set value on.</param>
		/// <param name="value">Value to set.</param>
		// Token: 0x06004466 RID: 17510 RVA: 0x00137415 File Offset: 0x00135615
		public static void SetPlacementTarget(DependencyObject element, UIElement value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.PlacementTargetProperty, value);
		}

		/// <summary> Gets the value of the <see cref="P:System.Windows.Controls.ContextMenuService.PlacementRectangle" /> property of the specified object. </summary>
		/// <param name="element">Object to query concerning the <see cref="P:System.Windows.Controls.ContextMenuService.PlacementRectangle" /> property.</param>
		/// <returns>Value of the <see cref="P:System.Windows.Controls.ContextMenuService.PlacementRectangle" /> property.</returns>
		// Token: 0x06004467 RID: 17511 RVA: 0x00137431 File Offset: 0x00135631
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static Rect GetPlacementRectangle(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Rect)element.GetValue(ContextMenuService.PlacementRectangleProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.ContextMenuService.PlacementRectangle" /> property of the specified object. </summary>
		/// <param name="element">Object to set the value on.</param>
		/// <param name="value">Value to set.</param>
		// Token: 0x06004468 RID: 17512 RVA: 0x00137451 File Offset: 0x00135651
		public static void SetPlacementRectangle(DependencyObject element, Rect value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.PlacementRectangleProperty, value);
		}

		/// <summary> Gets the value of the <see cref="P:System.Windows.Controls.ContextMenuService.Placement" /> property of the specified object. </summary>
		/// <param name="element">Object to query concerning the <see cref="P:System.Windows.Controls.ContextMenuService.Placement" />  property.</param>
		/// <returns>Value of the <see cref="P:System.Windows.Controls.ContextMenuService.Placement" /> property.</returns>
		// Token: 0x06004469 RID: 17513 RVA: 0x00137472 File Offset: 0x00135672
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static PlacementMode GetPlacement(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (PlacementMode)element.GetValue(ContextMenuService.PlacementProperty);
		}

		/// <summary> Sets the value of the <see cref="P:System.Windows.Controls.ContextMenuService.Placement" />  property of the specified object. </summary>
		/// <param name="element">Object to set the value on.</param>
		/// <param name="value">Value to set.</param>
		// Token: 0x0600446A RID: 17514 RVA: 0x00137492 File Offset: 0x00135692
		public static void SetPlacement(DependencyObject element, PlacementMode value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.PlacementProperty, value);
		}

		/// <summary> Gets the value of the <see cref="P:System.Windows.Controls.ContextMenuService.ShowOnDisabled" /> property of the specified object. </summary>
		/// <param name="element">Object to query concerning the <see cref="P:System.Windows.Controls.ContextMenuService.ShowOnDisabled" /> property.</param>
		/// <returns>Value of the <see cref="P:System.Windows.Controls.ContextMenuService.ShowOnDisabled" /> property.</returns>
		// Token: 0x0600446B RID: 17515 RVA: 0x001374B3 File Offset: 0x001356B3
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetShowOnDisabled(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ContextMenuService.ShowOnDisabledProperty);
		}

		/// <summary> Sets the value of the <see cref="P:System.Windows.Controls.ContextMenuService.ShowOnDisabled" /> property of the specified object. </summary>
		/// <param name="element">Object to set value on.</param>
		/// <param name="value">Value to set.</param>
		// Token: 0x0600446C RID: 17516 RVA: 0x001374D3 File Offset: 0x001356D3
		public static void SetShowOnDisabled(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.ShowOnDisabledProperty, BooleanBoxes.Box(value));
		}

		/// <summary> Gets the value of the <see cref="P:System.Windows.Controls.ContextMenuService.IsEnabled" /> property of the specified object. </summary>
		/// <param name="element">Object to query concerning the <see cref="P:System.Windows.Controls.ContextMenuService.IsEnabled" /> property.</param>
		/// <returns>Value of the <see cref="P:System.Windows.Controls.ContextMenuService.IsEnabled" /> property.</returns>
		// Token: 0x0600446D RID: 17517 RVA: 0x001374F4 File Offset: 0x001356F4
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetIsEnabled(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ContextMenuService.IsEnabledProperty);
		}

		/// <summary> Sets the value of the <see cref="P:System.Windows.Controls.ContextMenuService.IsEnabled" /> property of the specified object. </summary>
		/// <param name="element">Object to set value on.</param>
		/// <param name="value">Value to set.</param>
		// Token: 0x0600446E RID: 17518 RVA: 0x00137514 File Offset: 0x00135714
		public static void SetIsEnabled(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.IsEnabledProperty, BooleanBoxes.Box(value));
		}

		/// <summary>Adds a handler for the <see cref="E:System.Windows.Controls.ContextMenuService.ContextMenuOpening" /> attached event.</summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> that listens to this event.</param>
		/// <param name="handler">The event handler to be added.</param>
		// Token: 0x0600446F RID: 17519 RVA: 0x00137535 File Offset: 0x00135735
		public static void AddContextMenuOpeningHandler(DependencyObject element, ContextMenuEventHandler handler)
		{
			UIElement.AddHandler(element, ContextMenuService.ContextMenuOpeningEvent, handler);
		}

		/// <summary>Removes a handler for the <see cref="E:System.Windows.Controls.ContextMenuService.ContextMenuOpening" /> attached event.</summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> that listens to this event.</param>
		/// <param name="handler">The event handler to be removed.</param>
		// Token: 0x06004470 RID: 17520 RVA: 0x00137543 File Offset: 0x00135743
		public static void RemoveContextMenuOpeningHandler(DependencyObject element, ContextMenuEventHandler handler)
		{
			UIElement.RemoveHandler(element, ContextMenuService.ContextMenuOpeningEvent, handler);
		}

		/// <summary>Adds a handler for the <see cref="E:System.Windows.Controls.ContextMenuService.ContextMenuClosing" /> attached event.</summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> that listens to this event.</param>
		/// <param name="handler">The event handler to be added.</param>
		// Token: 0x06004471 RID: 17521 RVA: 0x00137551 File Offset: 0x00135751
		public static void AddContextMenuClosingHandler(DependencyObject element, ContextMenuEventHandler handler)
		{
			UIElement.AddHandler(element, ContextMenuService.ContextMenuClosingEvent, handler);
		}

		/// <summary>Removes a handler for the <see cref="E:System.Windows.Controls.ContextMenuService.ContextMenuClosing" /> attached event.</summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> that listens to this event.</param>
		/// <param name="handler">The event handler to be removed.</param>
		// Token: 0x06004472 RID: 17522 RVA: 0x0013755F File Offset: 0x0013575F
		public static void RemoveContextMenuClosingHandler(DependencyObject element, ContextMenuEventHandler handler)
		{
			UIElement.RemoveHandler(element, ContextMenuService.ContextMenuClosingEvent, handler);
		}

		// Token: 0x06004473 RID: 17523 RVA: 0x00137570 File Offset: 0x00135770
		static ContextMenuService()
		{
			EventManager.RegisterClassHandler(typeof(UIElement), ContextMenuService.ContextMenuOpeningEvent, new ContextMenuEventHandler(ContextMenuService.OnContextMenuOpening));
			EventManager.RegisterClassHandler(typeof(ContentElement), ContextMenuService.ContextMenuOpeningEvent, new ContextMenuEventHandler(ContextMenuService.OnContextMenuOpening));
			EventManager.RegisterClassHandler(typeof(UIElement3D), ContextMenuService.ContextMenuOpeningEvent, new ContextMenuEventHandler(ContextMenuService.OnContextMenuOpening));
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x001377CC File Offset: 0x001359CC
		private static void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			if (e.TargetElement == null)
			{
				DependencyObject dependencyObject = sender as DependencyObject;
				if (dependencyObject != null && ContextMenuService.ContextMenuIsEnabled(dependencyObject))
				{
					e.TargetElement = dependencyObject;
				}
			}
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x001377FC File Offset: 0x001359FC
		internal static bool ContextMenuIsEnabled(DependencyObject o)
		{
			bool result = false;
			object contextMenu = ContextMenuService.GetContextMenu(o);
			if (contextMenu != null && ContextMenuService.GetIsEnabled(o) && (PopupControlService.IsElementEnabled(o) || ContextMenuService.GetShowOnDisabled(o)))
			{
				result = true;
			}
			return result;
		}

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.ContextMenuService.ContextMenu" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenuService.ContextMenu" /> attached property.</returns>
		// Token: 0x0400288E RID: 10382
		public static readonly DependencyProperty ContextMenuProperty = DependencyProperty.RegisterAttached("ContextMenu", typeof(ContextMenu), typeof(ContextMenuService), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.ContextMenuService.HorizontalOffset" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenuService.HorizontalOffset" /> attached property.</returns>
		// Token: 0x0400288F RID: 10383
		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ContextMenuService), new FrameworkPropertyMetadata(0.0));

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.ContextMenuService.VerticalOffset" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenuService.VerticalOffset" /> attached property.</returns>
		// Token: 0x04002890 RID: 10384
		public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(ContextMenuService), new FrameworkPropertyMetadata(0.0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ContextMenuService.HasDropShadow" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenuService.HasDropShadow" /> attached property.</returns>
		// Token: 0x04002891 RID: 10385
		public static readonly DependencyProperty HasDropShadowProperty = DependencyProperty.RegisterAttached("HasDropShadow", typeof(bool), typeof(ContextMenuService), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.ContextMenuService.PlacementTarget" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenuService.PlacementTarget" /> attached property.</returns>
		// Token: 0x04002892 RID: 10386
		public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.RegisterAttached("PlacementTarget", typeof(UIElement), typeof(ContextMenuService), new FrameworkPropertyMetadata(null));

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.ContextMenuService.PlacementRectangle" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenuService.PlacementRectangle" /> attached property.</returns>
		// Token: 0x04002893 RID: 10387
		public static readonly DependencyProperty PlacementRectangleProperty = DependencyProperty.RegisterAttached("PlacementRectangle", typeof(Rect), typeof(ContextMenuService), new FrameworkPropertyMetadata(Rect.Empty));

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.ContextMenuService.Placement" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenuService.Placement" /> attached property.</returns>
		// Token: 0x04002894 RID: 10388
		public static readonly DependencyProperty PlacementProperty = DependencyProperty.RegisterAttached("Placement", typeof(PlacementMode), typeof(ContextMenuService), new FrameworkPropertyMetadata(PlacementMode.MousePoint));

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.ContextMenuService.ShowOnDisabled" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenuService.ShowOnDisabled" /> attached property.</returns>
		// Token: 0x04002895 RID: 10389
		public static readonly DependencyProperty ShowOnDisabledProperty = DependencyProperty.RegisterAttached("ShowOnDisabled", typeof(bool), typeof(ContextMenuService), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.ContextMenuService.IsEnabled" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenuService.IsEnabled" /> attached property.</returns>
		// Token: 0x04002896 RID: 10390
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ContextMenuService), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		/// <summary>Identifies the <see cref="E:System.Windows.Controls.ContextMenuService.ContextMenuOpening" /> attached event.</summary>
		/// <returns>The identifier for the <see cref="E:System.Windows.Controls.ContextMenuService.ContextMenuOpening" />  attached event.</returns>
		// Token: 0x04002897 RID: 10391
		public static readonly RoutedEvent ContextMenuOpeningEvent = EventManager.RegisterRoutedEvent("ContextMenuOpening", RoutingStrategy.Bubble, typeof(ContextMenuEventHandler), typeof(ContextMenuService));

		/// <summary> Identifies the <see cref="E:System.Windows.Controls.ContextMenuService.ContextMenuClosing" /> routed event. </summary>
		/// <returns>The identifier for the <see cref="E:System.Windows.Controls.ContextMenuService.ContextMenuClosing" /> routed event.</returns>
		// Token: 0x04002898 RID: 10392
		public static readonly RoutedEvent ContextMenuClosingEvent = EventManager.RegisterRoutedEvent("ContextMenuClosing", RoutingStrategy.Bubble, typeof(ContextMenuEventHandler), typeof(ContextMenuService));
	}
}
