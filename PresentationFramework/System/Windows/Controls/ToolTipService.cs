using System;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Represents a service that provides properties and events to control the display and behavior of tooltips.</summary>
	// Token: 0x02000547 RID: 1351
	public static class ToolTipService
	{
		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.ToolTipService.ToolTip" /> attached property for an object.</summary>
		/// <param name="element">The object from which the property value is read.</param>
		/// <returns>The object's <see cref="P:System.Windows.Controls.ToolTipService.ToolTip" /> property value.</returns>
		// Token: 0x06005896 RID: 22678 RVA: 0x001886A8 File Offset: 0x001868A8
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static object GetToolTip(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return element.GetValue(ToolTipService.ToolTipProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.ToolTipService.ToolTip" /> attached property for an object.</summary>
		/// <param name="element">The object to which the attached property is written.</param>
		/// <param name="value">The value to set.</param>
		// Token: 0x06005897 RID: 22679 RVA: 0x001886C3 File Offset: 0x001868C3
		public static void SetToolTip(DependencyObject element, object value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.ToolTipProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.ToolTipService.HorizontalOffset" /> attached property for an object.</summary>
		/// <param name="element">The object from which the property value is read.</param>
		/// <returns>The object's <see cref="P:System.Windows.Controls.ToolTipService.HorizontalOffset" /> property value.</returns>
		// Token: 0x06005898 RID: 22680 RVA: 0x001886DF File Offset: 0x001868DF
		[TypeConverter(typeof(LengthConverter))]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static double GetHorizontalOffset(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(ToolTipService.HorizontalOffsetProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.ToolTipService.HorizontalOffset" /> attached property for an object.</summary>
		/// <param name="element">The object to which the attached property is written.</param>
		/// <param name="value">The value to set.</param>
		// Token: 0x06005899 RID: 22681 RVA: 0x001886FF File Offset: 0x001868FF
		public static void SetHorizontalOffset(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.HorizontalOffsetProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.ToolTipService.VerticalOffset" /> attached property for an object.</summary>
		/// <param name="element">The object from which the property value is read.</param>
		/// <returns>The object's <see cref="P:System.Windows.Controls.ToolTipService.VerticalOffset" /> property value.</returns>
		// Token: 0x0600589A RID: 22682 RVA: 0x00188720 File Offset: 0x00186920
		[TypeConverter(typeof(LengthConverter))]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static double GetVerticalOffset(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(ToolTipService.VerticalOffsetProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.ToolTipService.VerticalOffset" /> attached property for an object.</summary>
		/// <param name="element">The object to which the attached property is written.</param>
		/// <param name="value">The desired value.</param>
		// Token: 0x0600589B RID: 22683 RVA: 0x00188740 File Offset: 0x00186940
		public static void SetVerticalOffset(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.VerticalOffsetProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.ToolTipService.HasDropShadow" /> attached property for an object. </summary>
		/// <param name="element">The object from which the property value is read.</param>
		/// <returns>The object's <see cref="P:System.Windows.Controls.ToolTipService.HasDropShadow" /> property value.</returns>
		// Token: 0x0600589C RID: 22684 RVA: 0x00188761 File Offset: 0x00186961
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetHasDropShadow(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ToolTipService.HasDropShadowProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.ToolTipService.HasDropShadow" /> attached property for an object.</summary>
		/// <param name="element">The object to which the attached property is written.</param>
		/// <param name="value">The value to set.</param>
		// Token: 0x0600589D RID: 22685 RVA: 0x00188781 File Offset: 0x00186981
		public static void SetHasDropShadow(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.HasDropShadowProperty, BooleanBoxes.Box(value));
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.ToolTipService.PlacementTarget" /> attached property for an object. </summary>
		/// <param name="element">The object from which the property value is read.</param>
		/// <returns>The object's <see cref="P:System.Windows.Controls.ToolTipService.PlacementTarget" /> property value.</returns>
		// Token: 0x0600589E RID: 22686 RVA: 0x001887A2 File Offset: 0x001869A2
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static UIElement GetPlacementTarget(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (UIElement)element.GetValue(ToolTipService.PlacementTargetProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.ToolTipService.PlacementTarget" /> attached property for an object. </summary>
		/// <param name="element">The object to which the attached property is written.</param>
		/// <param name="value">The value to set.</param>
		// Token: 0x0600589F RID: 22687 RVA: 0x001887C2 File Offset: 0x001869C2
		public static void SetPlacementTarget(DependencyObject element, UIElement value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.PlacementTargetProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.ToolTipService.PlacementRectangle" /> attached property for an object.</summary>
		/// <param name="element">The object from which the property value is read.</param>
		/// <returns>The object's <see cref="P:System.Windows.Controls.ToolTipService.PlacementRectangle" /> property value.</returns>
		// Token: 0x060058A0 RID: 22688 RVA: 0x001887DE File Offset: 0x001869DE
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static Rect GetPlacementRectangle(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Rect)element.GetValue(ToolTipService.PlacementRectangleProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.ToolTipService.PlacementRectangle" /> attached property for an object. </summary>
		/// <param name="element">The object to which the attached property is written.</param>
		/// <param name="value">The value to set.</param>
		// Token: 0x060058A1 RID: 22689 RVA: 0x001887FE File Offset: 0x001869FE
		public static void SetPlacementRectangle(DependencyObject element, Rect value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.PlacementRectangleProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.ToolTipService.Placement" /> attached property for an object.</summary>
		/// <param name="element">The object from which the property value is read.</param>
		/// <returns>The object's <see cref="P:System.Windows.Controls.ToolTipService.Placement" /> property value.</returns>
		// Token: 0x060058A2 RID: 22690 RVA: 0x0018881F File Offset: 0x00186A1F
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static PlacementMode GetPlacement(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (PlacementMode)element.GetValue(ToolTipService.PlacementProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.ToolTipService.Placement" /> attached property for an object. </summary>
		/// <param name="element">The object to which the attached property is written.</param>
		/// <param name="value">The value to set.</param>
		// Token: 0x060058A3 RID: 22691 RVA: 0x0018883F File Offset: 0x00186A3F
		public static void SetPlacement(DependencyObject element, PlacementMode value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.PlacementProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.ToolTipService.ShowOnDisabled" /> attached property for an object. </summary>
		/// <param name="element">The object from which the property value is read.</param>
		/// <returns>The object's <see cref="P:System.Windows.Controls.ToolTipService.ShowOnDisabled" /> property value.</returns>
		// Token: 0x060058A4 RID: 22692 RVA: 0x00188860 File Offset: 0x00186A60
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetShowOnDisabled(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ToolTipService.ShowOnDisabledProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.ToolTipService.ShowOnDisabled" /> attached property for an object. </summary>
		/// <param name="element">The object to which the attached property is written.</param>
		/// <param name="value">The value to set.</param>
		// Token: 0x060058A5 RID: 22693 RVA: 0x00188880 File Offset: 0x00186A80
		public static void SetShowOnDisabled(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.ShowOnDisabledProperty, BooleanBoxes.Box(value));
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.ToolTipService.IsOpen" /> attached property for an object.</summary>
		/// <param name="element">The object from which the property value is read.</param>
		/// <returns>The object's <see cref="P:System.Windows.Controls.ToolTipService.IsOpen" /> property value.</returns>
		// Token: 0x060058A6 RID: 22694 RVA: 0x001888A1 File Offset: 0x00186AA1
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetIsOpen(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ToolTipService.IsOpenProperty);
		}

		// Token: 0x060058A7 RID: 22695 RVA: 0x001888C1 File Offset: 0x00186AC1
		private static void SetIsOpen(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.IsOpenPropertyKey, BooleanBoxes.Box(value));
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.ToolTipService.IsEnabled" /> attached property for an object.</summary>
		/// <param name="element">The object from which the property value is read.</param>
		/// <returns>The object's <see cref="P:System.Windows.Controls.ToolTipService.IsEnabled" /> property value.</returns>
		// Token: 0x060058A8 RID: 22696 RVA: 0x001888E2 File Offset: 0x00186AE2
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetIsEnabled(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ToolTipService.IsEnabledProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.ToolTipService.IsEnabled" /> attached property for an object. </summary>
		/// <param name="element">The object to which the attached property is written.</param>
		/// <param name="value">The value to set.</param>
		// Token: 0x060058A9 RID: 22697 RVA: 0x00188902 File Offset: 0x00186B02
		public static void SetIsEnabled(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.IsEnabledProperty, BooleanBoxes.Box(value));
		}

		// Token: 0x060058AA RID: 22698 RVA: 0x0015A2F7 File Offset: 0x001584F7
		private static bool PositiveValueValidation(object o)
		{
			return (int)o >= 0;
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.ToolTipService.ShowDuration" /> attached property for an object. </summary>
		/// <param name="element">The object from which the property value is read.</param>
		/// <returns>The object's <see cref="P:System.Windows.Controls.ToolTipService.ShowDuration" /> property value.</returns>
		// Token: 0x060058AB RID: 22699 RVA: 0x00188923 File Offset: 0x00186B23
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static int GetShowDuration(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(ToolTipService.ShowDurationProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.ToolTipService.ShowDuration" /> attached property for an object. </summary>
		/// <param name="element">The object to which the attached property is written.</param>
		/// <param name="value">The value to set.</param>
		// Token: 0x060058AC RID: 22700 RVA: 0x00188943 File Offset: 0x00186B43
		public static void SetShowDuration(DependencyObject element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.ShowDurationProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.ToolTipService.InitialShowDelay" /> attached property for an object.</summary>
		/// <param name="element">The object from which the property value is read.</param>
		/// <returns>The object's <see cref="P:System.Windows.Controls.ToolTipService.InitialShowDelay" /> property value.</returns>
		// Token: 0x060058AD RID: 22701 RVA: 0x00188964 File Offset: 0x00186B64
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static int GetInitialShowDelay(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(ToolTipService.InitialShowDelayProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.ToolTipService.InitialShowDelay" /> attached property for an object.</summary>
		/// <param name="element">The object to which the attached property is written.</param>
		/// <param name="value">The value to set.</param>
		// Token: 0x060058AE RID: 22702 RVA: 0x00188984 File Offset: 0x00186B84
		public static void SetInitialShowDelay(DependencyObject element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.InitialShowDelayProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.ToolTipService.BetweenShowDelay" /> attached property for an object.</summary>
		/// <param name="element">The object from which the property value is read.</param>
		/// <returns>The object's <see cref="P:System.Windows.Controls.ToolTipService.BetweenShowDelay" /> property value.</returns>
		// Token: 0x060058AF RID: 22703 RVA: 0x001889A5 File Offset: 0x00186BA5
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static int GetBetweenShowDelay(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(ToolTipService.BetweenShowDelayProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.ToolTipService.BetweenShowDelay" /> attached property for an object. </summary>
		/// <param name="element">The object to which the attached property is written.</param>
		/// <param name="value">The value to set.</param>
		// Token: 0x060058B0 RID: 22704 RVA: 0x001889C5 File Offset: 0x00186BC5
		public static void SetBetweenShowDelay(DependencyObject element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.BetweenShowDelayProperty, value);
		}

		/// <summary>Adds a handler for the <see cref="E:System.Windows.Controls.ToolTipService.ToolTipOpening" /> attached event.</summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> that listens to this event.</param>
		/// <param name="handler">The event handler to be added.</param>
		// Token: 0x060058B1 RID: 22705 RVA: 0x001889E6 File Offset: 0x00186BE6
		public static void AddToolTipOpeningHandler(DependencyObject element, ToolTipEventHandler handler)
		{
			UIElement.AddHandler(element, ToolTipService.ToolTipOpeningEvent, handler);
		}

		/// <summary>Removes a handler for the <see cref="E:System.Windows.Controls.ToolTipService.ToolTipOpening" /> attached event.</summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> that listens to this event.</param>
		/// <param name="handler">The event handler to be removed.</param>
		// Token: 0x060058B2 RID: 22706 RVA: 0x001889F4 File Offset: 0x00186BF4
		public static void RemoveToolTipOpeningHandler(DependencyObject element, ToolTipEventHandler handler)
		{
			UIElement.RemoveHandler(element, ToolTipService.ToolTipOpeningEvent, handler);
		}

		/// <summary>Adds a handler for the <see cref="E:System.Windows.Controls.ToolTipService.ToolTipClosing" /> attached event.</summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> that listens to this event.</param>
		/// <param name="handler">The event handler to be added.</param>
		// Token: 0x060058B3 RID: 22707 RVA: 0x00188A02 File Offset: 0x00186C02
		public static void AddToolTipClosingHandler(DependencyObject element, ToolTipEventHandler handler)
		{
			UIElement.AddHandler(element, ToolTipService.ToolTipClosingEvent, handler);
		}

		/// <summary>Removes a handler for the <see cref="E:System.Windows.Controls.ToolTipService.ToolTipClosing" /> attached event.</summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> that listens to this event.</param>
		/// <param name="handler">The event handler to be removed.</param>
		// Token: 0x060058B4 RID: 22708 RVA: 0x00188A10 File Offset: 0x00186C10
		public static void RemoveToolTipClosingHandler(DependencyObject element, ToolTipEventHandler handler)
		{
			UIElement.RemoveHandler(element, ToolTipService.ToolTipClosingEvent, handler);
		}

		// Token: 0x060058B5 RID: 22709 RVA: 0x00188A20 File Offset: 0x00186C20
		static ToolTipService()
		{
			EventManager.RegisterClassHandler(typeof(UIElement), ToolTipService.FindToolTipEvent, new FindToolTipEventHandler(ToolTipService.OnFindToolTip));
			EventManager.RegisterClassHandler(typeof(ContentElement), ToolTipService.FindToolTipEvent, new FindToolTipEventHandler(ToolTipService.OnFindToolTip));
			EventManager.RegisterClassHandler(typeof(UIElement3D), ToolTipService.FindToolTipEvent, new FindToolTipEventHandler(ToolTipService.OnFindToolTip));
		}

		// Token: 0x060058B6 RID: 22710 RVA: 0x00188D94 File Offset: 0x00186F94
		private static void OnFindToolTip(object sender, FindToolTipEventArgs e)
		{
			if (e.TargetElement == null)
			{
				DependencyObject dependencyObject = sender as DependencyObject;
				if (dependencyObject != null)
				{
					if (e.TriggerAction != ToolTip.ToolTipTrigger.KeyboardShortcut && PopupControlService.Current.StopLookingForToolTip(dependencyObject))
					{
						e.Handled = true;
						e.KeepCurrentActive = true;
						return;
					}
					if (ToolTipService.ToolTipIsEnabled(dependencyObject, e.TriggerAction))
					{
						e.TargetElement = dependencyObject;
						e.Handled = true;
					}
				}
			}
		}

		// Token: 0x060058B7 RID: 22711 RVA: 0x00188DF4 File Offset: 0x00186FF4
		private static bool ToolTipIsEnabled(DependencyObject o, ToolTip.ToolTipTrigger triggerAction)
		{
			object toolTip = ToolTipService.GetToolTip(o);
			if (toolTip != null && ToolTipService.GetIsEnabled(o))
			{
				ToolTip toolTip2 = toolTip as ToolTip;
				bool flag = toolTip2 == null || toolTip2.ShouldShowOnKeyboardFocus;
				if ((PopupControlService.IsElementEnabled(o) || ToolTipService.GetShowOnDisabled(o)) && (triggerAction != ToolTip.ToolTipTrigger.KeyboardFocus || flag))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTipService.ToolTip" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTipService.ToolTip" /> attached property.</returns>
		// Token: 0x04002ED1 RID: 11985
		public static readonly DependencyProperty ToolTipProperty = DependencyProperty.RegisterAttached("ToolTip", typeof(object), typeof(ToolTipService), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTipService.HorizontalOffset" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTipService.HorizontalOffset" /> attached property.</returns>
		// Token: 0x04002ED2 RID: 11986
		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ToolTipService), new FrameworkPropertyMetadata(0.0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTipService.VerticalOffset" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTipService.VerticalOffset" /> attached property.</returns>
		// Token: 0x04002ED3 RID: 11987
		public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(ToolTipService), new FrameworkPropertyMetadata(0.0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTipService.HasDropShadow" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTipService.HasDropShadow" /> attached property.</returns>
		// Token: 0x04002ED4 RID: 11988
		public static readonly DependencyProperty HasDropShadowProperty = DependencyProperty.RegisterAttached("HasDropShadow", typeof(bool), typeof(ToolTipService), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTipService.PlacementTarget" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTipService.PlacementTarget" /> attached property.</returns>
		// Token: 0x04002ED5 RID: 11989
		public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.RegisterAttached("PlacementTarget", typeof(UIElement), typeof(ToolTipService), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTipService.PlacementRectangle" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTipService.PlacementRectangle" /> attached property.</returns>
		// Token: 0x04002ED6 RID: 11990
		public static readonly DependencyProperty PlacementRectangleProperty = DependencyProperty.RegisterAttached("PlacementRectangle", typeof(Rect), typeof(ToolTipService), new FrameworkPropertyMetadata(Rect.Empty));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTipService.Placement" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTipService.Placement" /> attached property.</returns>
		// Token: 0x04002ED7 RID: 11991
		public static readonly DependencyProperty PlacementProperty = DependencyProperty.RegisterAttached("Placement", typeof(PlacementMode), typeof(ToolTipService), new FrameworkPropertyMetadata(PlacementMode.Mouse));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTipService.ShowOnDisabled" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTipService.ShowOnDisabled" /> attached property.</returns>
		// Token: 0x04002ED8 RID: 11992
		public static readonly DependencyProperty ShowOnDisabledProperty = DependencyProperty.RegisterAttached("ShowOnDisabled", typeof(bool), typeof(ToolTipService), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04002ED9 RID: 11993
		private static readonly DependencyPropertyKey IsOpenPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsOpen", typeof(bool), typeof(ToolTipService), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTipService.IsOpen" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTipService.IsOpen" /> attached property.</returns>
		// Token: 0x04002EDA RID: 11994
		public static readonly DependencyProperty IsOpenProperty = ToolTipService.IsOpenPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTipService.IsEnabled" /> attached property. </summary>
		/// <returns>Identifies the <see cref="P:System.Windows.Controls.ToolTipService.IsEnabled" /> attached property.</returns>
		// Token: 0x04002EDB RID: 11995
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ToolTipService), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTipService.ShowDuration" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTipService.ShowDuration" /> attached property.</returns>
		// Token: 0x04002EDC RID: 11996
		public static readonly DependencyProperty ShowDurationProperty = DependencyProperty.RegisterAttached("ShowDuration", typeof(int), typeof(ToolTipService), new FrameworkPropertyMetadata(5000), new ValidateValueCallback(ToolTipService.PositiveValueValidation));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTipService.InitialShowDelay" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTipService.InitialShowDelay" /> attached property.</returns>
		// Token: 0x04002EDD RID: 11997
		public static readonly DependencyProperty InitialShowDelayProperty = DependencyProperty.RegisterAttached("InitialShowDelay", typeof(int), typeof(ToolTipService), new FrameworkPropertyMetadata(SystemParameters.MouseHoverTimeMilliseconds), new ValidateValueCallback(ToolTipService.PositiveValueValidation));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTipService.BetweenShowDelay" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTipService.BetweenShowDelay" /> attached property.</returns>
		// Token: 0x04002EDE RID: 11998
		public static readonly DependencyProperty BetweenShowDelayProperty = DependencyProperty.RegisterAttached("BetweenShowDelay", typeof(int), typeof(ToolTipService), new FrameworkPropertyMetadata(100), new ValidateValueCallback(ToolTipService.PositiveValueValidation));

		/// <summary>Identifies the <see cref="E:System.Windows.Controls.ToolTipService.ToolTipOpening" /> event that is exposed by objects that use the <see cref="T:System.Windows.Controls.ToolTipService" /> service to display tooltips. </summary>
		/// <returns>The identifier for the <see cref="E:System.Windows.Controls.ToolTipService.ToolTipOpening" /> event.</returns>
		// Token: 0x04002EDF RID: 11999
		public static readonly RoutedEvent ToolTipOpeningEvent = EventManager.RegisterRoutedEvent("ToolTipOpening", RoutingStrategy.Direct, typeof(ToolTipEventHandler), typeof(ToolTipService));

		/// <summary>Identifies the <see cref="E:System.Windows.Controls.ToolTipService.ToolTipClosing" /> event that is exposed by objects that use the <see cref="T:System.Windows.Controls.ToolTipService" /> service to display tooltips. </summary>
		/// <returns>The identifier for the <see cref="E:System.Windows.Controls.ToolTipService.ToolTipClosing" /> event.</returns>
		// Token: 0x04002EE0 RID: 12000
		public static readonly RoutedEvent ToolTipClosingEvent = EventManager.RegisterRoutedEvent("ToolTipClosing", RoutingStrategy.Direct, typeof(ToolTipEventHandler), typeof(ToolTipService));

		// Token: 0x04002EE1 RID: 12001
		internal static readonly RoutedEvent FindToolTipEvent = EventManager.RegisterRoutedEvent("FindToolTip", RoutingStrategy.Bubble, typeof(FindToolTipEventHandler), typeof(ToolTipService));
	}
}
