using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using MS.Internal.Annotations.Anchoring;
using MS.Utility;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020007E3 RID: 2019
	internal sealed class MarkedHighlightComponent : Canvas, IAnnotationComponent
	{
		// Token: 0x06007CCC RID: 31948 RVA: 0x002319EC File Offset: 0x0022FBEC
		public MarkedHighlightComponent(XmlQualifiedName type, DependencyObject host)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this._DPHost = ((host == null) ? this : host);
			base.ClipToBounds = false;
			this.HighlightAnchor = new HighlightComponent(1, true, type);
			base.Children.Add(this.HighlightAnchor);
			this._leftMarker = null;
			this._rightMarker = null;
			this._state = 0;
			this.SetState();
		}

		// Token: 0x17001D05 RID: 7429
		// (get) Token: 0x06007CCD RID: 31949 RVA: 0x00231A6C File Offset: 0x0022FC6C
		public IList AttachedAnnotations
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				if (this._attachedAnnotation != null)
				{
					arrayList.Add(this._attachedAnnotation);
				}
				return arrayList;
			}
		}

		// Token: 0x17001D06 RID: 7430
		// (get) Token: 0x06007CCE RID: 31950 RVA: 0x00231A95 File Offset: 0x0022FC95
		// (set) Token: 0x06007CCF RID: 31951 RVA: 0x00231A9D File Offset: 0x0022FC9D
		public PresentationContext PresentationContext
		{
			get
			{
				return this._presentationContext;
			}
			set
			{
				this._presentationContext = value;
			}
		}

		// Token: 0x17001D07 RID: 7431
		// (get) Token: 0x06007CD0 RID: 31952 RVA: 0x0011BDA8 File Offset: 0x00119FA8
		// (set) Token: 0x06007CD1 RID: 31953 RVA: 0x00002137 File Offset: 0x00000337
		public int ZOrder
		{
			get
			{
				return -1;
			}
			set
			{
			}
		}

		// Token: 0x17001D08 RID: 7432
		// (get) Token: 0x06007CD2 RID: 31954 RVA: 0x00231AA6 File Offset: 0x0022FCA6
		public UIElement AnnotatedElement
		{
			get
			{
				if (this._attachedAnnotation == null)
				{
					return null;
				}
				return this._attachedAnnotation.Parent as UIElement;
			}
		}

		// Token: 0x17001D09 RID: 7433
		// (get) Token: 0x06007CD3 RID: 31955 RVA: 0x00231AC2 File Offset: 0x0022FCC2
		// (set) Token: 0x06007CD4 RID: 31956 RVA: 0x00231ACA File Offset: 0x0022FCCA
		public bool IsDirty
		{
			get
			{
				return this._isDirty;
			}
			set
			{
				this._isDirty = value;
				if (value)
				{
					this.UpdateGeometry();
				}
			}
		}

		// Token: 0x06007CD5 RID: 31957 RVA: 0x00231ADC File Offset: 0x0022FCDC
		public GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			if (this._attachedAnnotation == null)
			{
				throw new InvalidOperationException(SR.Get("InvalidAttachedAnnotation"));
			}
			this.HighlightAnchor.GetDesiredTransform(transform);
			return transform;
		}

		// Token: 0x06007CD6 RID: 31958 RVA: 0x00231B04 File Offset: 0x0022FD04
		public void AddAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			if (this._attachedAnnotation != null)
			{
				throw new ArgumentException(SR.Get("MoreThanOneAttachedAnnotation"));
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAttachedMHBegin);
			this._attachedAnnotation = attachedAnnotation;
			if ((attachedAnnotation.AttachmentLevel & AttachmentLevel.StartPortion) != AttachmentLevel.Unresolved)
			{
				this._leftMarker = this.CreateMarker(this.GetMarkerGeometry());
			}
			if ((attachedAnnotation.AttachmentLevel & AttachmentLevel.EndPortion) != AttachmentLevel.Unresolved)
			{
				this._rightMarker = this.CreateMarker(this.GetMarkerGeometry());
			}
			this.RegisterAnchor();
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAttachedMHEnd);
		}

		// Token: 0x06007CD7 RID: 31959 RVA: 0x00231B88 File Offset: 0x0022FD88
		public void RemoveAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			if (attachedAnnotation == null)
			{
				throw new ArgumentNullException("attachedAnnotation");
			}
			if (attachedAnnotation != this._attachedAnnotation)
			{
				throw new ArgumentException(SR.Get("InvalidAttachedAnnotation"), "attachedAnnotation");
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.RemoveAttachedMHBegin);
			this.CleanUpAnchor();
			this._attachedAnnotation = null;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.RemoveAttachedMHEnd);
		}

		// Token: 0x06007CD8 RID: 31960 RVA: 0x001279B5 File Offset: 0x00125BB5
		public void ModifyAttachedAnnotation(IAttachedAnnotation attachedAnnotation, object previousAttachedAnchor, AttachmentLevel previousAttachmentLevel)
		{
			throw new NotSupportedException(SR.Get("NotSupported"));
		}

		// Token: 0x17001D0A RID: 7434
		// (set) Token: 0x06007CD9 RID: 31961 RVA: 0x00231BE8 File Offset: 0x0022FDE8
		public bool Focused
		{
			set
			{
				byte state = this._state;
				if (value)
				{
					this._state |= 1;
				}
				else
				{
					this._state &= 126;
				}
				if (this._state == 0 != (state == 0))
				{
					this.SetState();
				}
			}
		}

		// Token: 0x17001D0B RID: 7435
		// (set) Token: 0x06007CDA RID: 31962 RVA: 0x00231C35 File Offset: 0x0022FE35
		public Brush MarkerBrush
		{
			set
			{
				base.SetValue(MarkedHighlightComponent.MarkerBrushProperty, value);
			}
		}

		// Token: 0x17001D0C RID: 7436
		// (set) Token: 0x06007CDB RID: 31963 RVA: 0x00231C43 File Offset: 0x0022FE43
		public double StrokeThickness
		{
			set
			{
				base.SetValue(MarkedHighlightComponent.StrokeThicknessProperty, value);
			}
		}

		// Token: 0x06007CDC RID: 31964 RVA: 0x00231C56 File Offset: 0x0022FE56
		internal void SetTabIndex(int index)
		{
			if (this._DPHost != null)
			{
				KeyboardNavigation.SetTabIndex(this._DPHost, index);
			}
		}

		// Token: 0x06007CDD RID: 31965 RVA: 0x00231C6C File Offset: 0x0022FE6C
		private void SetMarkerTransform(Path marker, ITextPointer anchor, ITextPointer baseAnchor, int xScaleFactor)
		{
			if (marker == null)
			{
				return;
			}
			GeometryGroup geometryGroup = marker.Data as GeometryGroup;
			Rect anchorRectangle = TextSelectionHelper.GetAnchorRectangle(anchor);
			if (anchorRectangle == Rect.Empty)
			{
				return;
			}
			double num = anchorRectangle.Height - MarkedHighlightComponent.MarkerVerticalSpace - this._bottomTailHeight - this._topTailHeight;
			double scaleY = 0.0;
			double scaleY2 = 0.0;
			if (num > 0.0)
			{
				scaleY = num / this._bodyHeight;
				scaleY2 = 1.0;
			}
			ScaleTransform value = new ScaleTransform(1.0, scaleY);
			TranslateTransform value2 = new TranslateTransform(anchorRectangle.X, anchorRectangle.Y + this._topTailHeight + MarkedHighlightComponent.MarkerVerticalSpace);
			TransformGroup transformGroup = new TransformGroup();
			transformGroup.Children.Add(value);
			transformGroup.Children.Add(value2);
			ScaleTransform value3 = new ScaleTransform((double)xScaleFactor, scaleY2);
			TranslateTransform value4 = new TranslateTransform(anchorRectangle.X, anchorRectangle.Bottom - this._bottomTailHeight);
			TranslateTransform value5 = new TranslateTransform(anchorRectangle.X, anchorRectangle.Top + MarkedHighlightComponent.MarkerVerticalSpace);
			TransformGroup transformGroup2 = new TransformGroup();
			transformGroup2.Children.Add(value3);
			transformGroup2.Children.Add(value4);
			TransformGroup transformGroup3 = new TransformGroup();
			transformGroup3.Children.Add(value3);
			transformGroup3.Children.Add(value5);
			if (geometryGroup.Children[0] != null)
			{
				geometryGroup.Children[0].Transform = transformGroup3;
			}
			if (geometryGroup.Children[1] != null)
			{
				geometryGroup.Children[1].Transform = transformGroup;
			}
			if (geometryGroup.Children[2] != null)
			{
				geometryGroup.Children[2].Transform = transformGroup2;
			}
			if (baseAnchor != null)
			{
				ITextView documentPageTextView = TextSelectionHelper.GetDocumentPageTextView(baseAnchor);
				ITextView documentPageTextView2 = TextSelectionHelper.GetDocumentPageTextView(anchor);
				if (documentPageTextView != documentPageTextView2 && documentPageTextView.RenderScope != null && documentPageTextView2.RenderScope != null)
				{
					geometryGroup.Transform = (Transform)documentPageTextView2.RenderScope.TransformToVisual(documentPageTextView.RenderScope);
				}
			}
		}

		// Token: 0x06007CDE RID: 31966 RVA: 0x00231E7C File Offset: 0x0023007C
		private void SetSelected(bool selected)
		{
			byte state = this._state;
			if (selected && this._uiParent.IsFocused)
			{
				this._state |= 2;
			}
			else
			{
				this._state &= 125;
			}
			if (this._state == 0 != (state == 0))
			{
				this.SetState();
			}
		}

		// Token: 0x06007CDF RID: 31967 RVA: 0x00231ED8 File Offset: 0x002300D8
		private void RemoveHighlightMarkers()
		{
			if (this._leftMarker != null)
			{
				base.Children.Remove(this._leftMarker);
			}
			if (this._rightMarker != null)
			{
				base.Children.Remove(this._rightMarker);
			}
			this._leftMarker = null;
			this._rightMarker = null;
		}

		// Token: 0x06007CE0 RID: 31968 RVA: 0x00231F28 File Offset: 0x00230128
		private void RegisterAnchor()
		{
			TextAnchor textAnchor = this._attachedAnnotation.AttachedAnchor as TextAnchor;
			if (textAnchor == null)
			{
				throw new ArgumentException(SR.Get("InvalidAttachedAnchor"));
			}
			ITextContainer textContainer = textAnchor.Start.TextContainer;
			this.HighlightAnchor.AddAttachedAnnotation(this._attachedAnnotation);
			this.UpdateGeometry();
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.AnnotatedElement);
			if (adornerLayer == null)
			{
				throw new InvalidOperationException(SR.Get("NoPresentationContextForGivenElement", new object[]
				{
					this.AnnotatedElement
				}));
			}
			AdornerPresentationContext.HostComponent(adornerLayer, this, this.AnnotatedElement, false);
			this._selection = textContainer.TextSelection;
			if (this._selection != null)
			{
				this._uiParent = (PathNode.GetParent(textContainer.Parent) as UIElement);
				this.RegisterComponent();
				if (this._uiParent != null)
				{
					this._uiParent.GotKeyboardFocus += this.OnContainerGotFocus;
					this._uiParent.LostKeyboardFocus += this.OnContainerLostFocus;
					if (this.HighlightAnchor.IsSelected(this._selection))
					{
						this.SetSelected(true);
					}
				}
			}
		}

		// Token: 0x06007CE1 RID: 31969 RVA: 0x00232038 File Offset: 0x00230238
		private void CleanUpAnchor()
		{
			if (this._selection != null)
			{
				this.UnregisterComponent();
				if (this._uiParent != null)
				{
					this._uiParent.GotKeyboardFocus -= this.OnContainerGotFocus;
					this._uiParent.LostKeyboardFocus -= this.OnContainerLostFocus;
				}
			}
			this._presentationContext.RemoveFromHost(this, false);
			if (this.HighlightAnchor != null)
			{
				this.HighlightAnchor.RemoveAttachedAnnotation(this._attachedAnnotation);
				base.Children.Remove(this.HighlightAnchor);
				this.HighlightAnchor = null;
				this.RemoveHighlightMarkers();
			}
			this._attachedAnnotation = null;
		}

		// Token: 0x06007CE2 RID: 31970 RVA: 0x002320D4 File Offset: 0x002302D4
		private void SetState()
		{
			if (this._state == 0)
			{
				if (this._highlightAnchor != null)
				{
					this._highlightAnchor.Activate(false);
				}
				this.MarkerBrush = new SolidColorBrush(MarkedHighlightComponent.DefaultMarkerColor);
				this.StrokeThickness = MarkedHighlightComponent.MarkerStrokeThickness;
				this._DPHost.SetValue(StickyNoteControl.IsActiveProperty, false);
				return;
			}
			if (this._highlightAnchor != null)
			{
				this._highlightAnchor.Activate(true);
			}
			this.MarkerBrush = new SolidColorBrush(MarkedHighlightComponent.DefaultActiveMarkerColor);
			this.StrokeThickness = MarkedHighlightComponent.ActiveMarkerStrokeThickness;
			this._DPHost.SetValue(StickyNoteControl.IsActiveProperty, true);
		}

		// Token: 0x06007CE3 RID: 31971 RVA: 0x0023216C File Offset: 0x0023036C
		private Path CreateMarker(Geometry geometry)
		{
			Path path = new Path();
			path.Data = geometry;
			Binding binding = new Binding("MarkerBrushProperty");
			binding.Source = this;
			path.SetBinding(Shape.StrokeProperty, binding);
			Binding binding2 = new Binding("StrokeThicknessProperty");
			binding2.Source = this;
			path.SetBinding(Shape.StrokeThicknessProperty, binding2);
			path.StrokeEndLineCap = PenLineCap.Round;
			path.StrokeStartLineCap = PenLineCap.Round;
			base.Children.Add(path);
			return path;
		}

		// Token: 0x06007CE4 RID: 31972 RVA: 0x002321E0 File Offset: 0x002303E0
		private void RegisterComponent()
		{
			MarkedHighlightComponent.ComponentsRegister componentsRegister = (MarkedHighlightComponent.ComponentsRegister)MarkedHighlightComponent._documentHandlers[this._selection];
			if (componentsRegister == null)
			{
				componentsRegister = new MarkedHighlightComponent.ComponentsRegister(new EventHandler(MarkedHighlightComponent.OnSelectionChanged), new MouseEventHandler(MarkedHighlightComponent.OnMouseMove));
				MarkedHighlightComponent._documentHandlers.Add(this._selection, componentsRegister);
				this._selection.Changed += componentsRegister.SelectionHandler;
				if (this._uiParent != null)
				{
					this._uiParent.MouseMove += componentsRegister.MouseMoveHandler;
				}
			}
			componentsRegister.Add(this);
		}

		// Token: 0x06007CE5 RID: 31973 RVA: 0x00232268 File Offset: 0x00230468
		private void UnregisterComponent()
		{
			MarkedHighlightComponent.ComponentsRegister componentsRegister = (MarkedHighlightComponent.ComponentsRegister)MarkedHighlightComponent._documentHandlers[this._selection];
			componentsRegister.Remove(this);
			if (componentsRegister.Components.Count == 0)
			{
				MarkedHighlightComponent._documentHandlers.Remove(this._selection);
				this._selection.Changed -= componentsRegister.SelectionHandler;
				if (this._uiParent != null)
				{
					this._uiParent.MouseMove -= componentsRegister.MouseMoveHandler;
				}
			}
		}

		// Token: 0x06007CE6 RID: 31974 RVA: 0x002322DC File Offset: 0x002304DC
		private void UpdateGeometry()
		{
			if (this.HighlightAnchor == null || this.HighlightAnchor == null)
			{
				throw new Exception(SR.Get("UndefinedHighlightAnchor"));
			}
			TextAnchor range = ((IHighlightRange)this.HighlightAnchor).Range;
			ITextPointer textPointer = range.Start.CreatePointer(LogicalDirection.Forward);
			ITextPointer textPointer2 = range.End.CreatePointer(LogicalDirection.Backward);
			FlowDirection textFlowDirection = MarkedHighlightComponent.GetTextFlowDirection(textPointer);
			FlowDirection textFlowDirection2 = MarkedHighlightComponent.GetTextFlowDirection(textPointer2);
			this.SetMarkerTransform(this._leftMarker, textPointer, null, (textFlowDirection == FlowDirection.LeftToRight) ? 1 : -1);
			this.SetMarkerTransform(this._rightMarker, textPointer2, textPointer, (textFlowDirection2 == FlowDirection.LeftToRight) ? -1 : 1);
			this.HighlightAnchor.IsDirty = true;
			this.IsDirty = false;
		}

		// Token: 0x06007CE7 RID: 31975 RVA: 0x0023237C File Offset: 0x0023057C
		private Geometry GetMarkerGeometry()
		{
			GeometryGroup geometryGroup = new GeometryGroup();
			geometryGroup.Children.Add(new LineGeometry(new Point(0.0, 1.0), new Point(1.0, 0.0)));
			geometryGroup.Children.Add(new LineGeometry(new Point(0.0, 0.0), new Point(0.0, 50.0)));
			geometryGroup.Children.Add(new LineGeometry(new Point(0.0, 0.0), new Point(1.0, 1.0)));
			this._bodyHeight = geometryGroup.Children[1].Bounds.Height;
			this._topTailHeight = geometryGroup.Children[0].Bounds.Height;
			this._bottomTailHeight = geometryGroup.Children[2].Bounds.Height;
			return geometryGroup;
		}

		// Token: 0x06007CE8 RID: 31976 RVA: 0x002324A8 File Offset: 0x002306A8
		private void CheckPosition(ITextPointer position)
		{
			IHighlightRange highlightAnchor = this._highlightAnchor;
			bool flag = highlightAnchor.Range.Contains(position);
			bool flag2 = (bool)this._DPHost.GetValue(StickyNoteControl.IsMouseOverAnchorProperty);
			if (flag != flag2)
			{
				this._DPHost.SetValue(StickyNoteControl.IsMouseOverAnchorPropertyKey, flag);
			}
		}

		// Token: 0x06007CE9 RID: 31977 RVA: 0x002324F4 File Offset: 0x002306F4
		private void OnContainerGotFocus(object sender, KeyboardFocusChangedEventArgs args)
		{
			if (this.HighlightAnchor != null && this.HighlightAnchor.IsSelected(this._selection))
			{
				this.SetSelected(true);
			}
		}

		// Token: 0x06007CEA RID: 31978 RVA: 0x00232518 File Offset: 0x00230718
		private void OnContainerLostFocus(object sender, KeyboardFocusChangedEventArgs args)
		{
			this.SetSelected(false);
		}

		// Token: 0x17001D0D RID: 7437
		// (get) Token: 0x06007CEB RID: 31979 RVA: 0x00232521 File Offset: 0x00230721
		// (set) Token: 0x06007CEC RID: 31980 RVA: 0x00232529 File Offset: 0x00230729
		internal HighlightComponent HighlightAnchor
		{
			get
			{
				return this._highlightAnchor;
			}
			set
			{
				this._highlightAnchor = value;
				if (this._highlightAnchor != null)
				{
					this._highlightAnchor.DefaultBackground = MarkedHighlightComponent.DefaultAnchorBackground;
					this._highlightAnchor.DefaultActiveBackground = MarkedHighlightComponent.DefaultActiveAnchorBackground;
				}
			}
		}

		// Token: 0x06007CED RID: 31981 RVA: 0x0023255C File Offset: 0x0023075C
		private static FlowDirection GetTextFlowDirection(ITextPointer pointer)
		{
			Invariant.Assert(pointer != null, "Null pointer passed.");
			Invariant.Assert(pointer.IsAtInsertionPosition, "Pointer is not an insertion position");
			int num = 0;
			LogicalDirection logicalDirection = pointer.LogicalDirection;
			TextPointerContext pointerContext = pointer.GetPointerContext(logicalDirection);
			FlowDirection result;
			if ((pointerContext == TextPointerContext.ElementEnd || pointerContext == TextPointerContext.ElementStart) && !TextSchema.IsFormattingType(pointer.ParentType))
			{
				result = (FlowDirection)pointer.GetValue(FrameworkElement.FlowDirectionProperty);
			}
			else
			{
				Rect anchorRectangle = TextSelectionHelper.GetAnchorRectangle(pointer);
				ITextPointer textPointer = pointer.GetNextInsertionPosition(logicalDirection);
				if (textPointer != null)
				{
					textPointer = textPointer.CreatePointer((logicalDirection == LogicalDirection.Backward) ? LogicalDirection.Forward : LogicalDirection.Backward);
					if (logicalDirection == LogicalDirection.Forward)
					{
						if (pointerContext == TextPointerContext.ElementEnd && textPointer.GetPointerContext(textPointer.LogicalDirection) == TextPointerContext.ElementStart)
						{
							return (FlowDirection)pointer.GetValue(FrameworkElement.FlowDirectionProperty);
						}
					}
					else if (pointerContext == TextPointerContext.ElementStart && textPointer.GetPointerContext(textPointer.LogicalDirection) == TextPointerContext.ElementEnd)
					{
						return (FlowDirection)pointer.GetValue(FrameworkElement.FlowDirectionProperty);
					}
					Rect anchorRectangle2 = TextSelectionHelper.GetAnchorRectangle(textPointer);
					if (anchorRectangle2 != Rect.Empty && anchorRectangle != Rect.Empty)
					{
						num = Math.Sign(anchorRectangle2.Left - anchorRectangle.Left);
						if (logicalDirection == LogicalDirection.Backward)
						{
							num = -num;
						}
					}
				}
				if (num == 0)
				{
					result = (FlowDirection)pointer.GetValue(FrameworkElement.FlowDirectionProperty);
				}
				else
				{
					result = ((num > 0) ? FlowDirection.LeftToRight : FlowDirection.RightToLeft);
				}
			}
			return result;
		}

		// Token: 0x06007CEE RID: 31982 RVA: 0x0023269C File Offset: 0x0023089C
		private static void OnSelectionChanged(object sender, EventArgs args)
		{
			ITextRange textRange = sender as ITextRange;
			MarkedHighlightComponent.ComponentsRegister componentsRegister = (MarkedHighlightComponent.ComponentsRegister)MarkedHighlightComponent._documentHandlers[textRange];
			if (componentsRegister == null)
			{
				return;
			}
			List<MarkedHighlightComponent> components = componentsRegister.Components;
			bool[] array = new bool[components.Count];
			for (int i = 0; i < components.Count; i++)
			{
				array[i] = components[i].HighlightAnchor.IsSelected(textRange);
				if (!array[i])
				{
					components[i].SetSelected(false);
				}
			}
			for (int j = 0; j < components.Count; j++)
			{
				if (array[j])
				{
					components[j].SetSelected(true);
				}
			}
		}

		// Token: 0x06007CEF RID: 31983 RVA: 0x00232744 File Offset: 0x00230944
		private static void OnMouseMove(object sender, MouseEventArgs args)
		{
			IServiceProvider serviceProvider = sender as IServiceProvider;
			if (serviceProvider == null)
			{
				return;
			}
			ITextView textView = (ITextView)serviceProvider.GetService(typeof(ITextView));
			if (textView == null || !textView.IsValid)
			{
				return;
			}
			Point position = Mouse.PrimaryDevice.GetPosition(textView.RenderScope);
			ITextPointer textPositionFromPoint = textView.GetTextPositionFromPoint(position, false);
			if (textPositionFromPoint != null)
			{
				MarkedHighlightComponent.CheckAllHighlightRanges(textPositionFromPoint);
			}
		}

		// Token: 0x06007CF0 RID: 31984 RVA: 0x002327A4 File Offset: 0x002309A4
		private static void CheckAllHighlightRanges(ITextPointer pos)
		{
			ITextContainer textContainer = pos.TextContainer;
			ITextRange textSelection = textContainer.TextSelection;
			if (textSelection == null)
			{
				return;
			}
			MarkedHighlightComponent.ComponentsRegister componentsRegister = (MarkedHighlightComponent.ComponentsRegister)MarkedHighlightComponent._documentHandlers[textSelection];
			if (componentsRegister == null)
			{
				return;
			}
			List<MarkedHighlightComponent> components = componentsRegister.Components;
			for (int i = 0; i < components.Count; i++)
			{
				components[i].CheckPosition(pos);
			}
		}

		// Token: 0x04003A7E RID: 14974
		public static DependencyProperty MarkerBrushProperty = DependencyProperty.Register("MarkerBrushProperty", typeof(Brush), typeof(MarkedHighlightComponent));

		// Token: 0x04003A7F RID: 14975
		public static DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThicknessProperty", typeof(double), typeof(MarkedHighlightComponent));

		// Token: 0x04003A80 RID: 14976
		internal static Color DefaultAnchorBackground = (Color)ColorConverter.ConvertFromString("#3380FF80");

		// Token: 0x04003A81 RID: 14977
		internal static Color DefaultMarkerColor = (Color)ColorConverter.ConvertFromString("#FF008000");

		// Token: 0x04003A82 RID: 14978
		internal static Color DefaultActiveAnchorBackground = (Color)ColorConverter.ConvertFromString("#3300FF00");

		// Token: 0x04003A83 RID: 14979
		internal static Color DefaultActiveMarkerColor = (Color)ColorConverter.ConvertFromString("#FF008000");

		// Token: 0x04003A84 RID: 14980
		internal static double MarkerStrokeThickness = 1.0;

		// Token: 0x04003A85 RID: 14981
		internal static double ActiveMarkerStrokeThickness = 2.0;

		// Token: 0x04003A86 RID: 14982
		internal static double MarkerVerticalSpace = 2.0;

		// Token: 0x04003A87 RID: 14983
		private static Hashtable _documentHandlers = new Hashtable();

		// Token: 0x04003A88 RID: 14984
		private byte _state;

		// Token: 0x04003A89 RID: 14985
		private HighlightComponent _highlightAnchor;

		// Token: 0x04003A8A RID: 14986
		private double _bodyHeight;

		// Token: 0x04003A8B RID: 14987
		private double _bottomTailHeight;

		// Token: 0x04003A8C RID: 14988
		private double _topTailHeight;

		// Token: 0x04003A8D RID: 14989
		private Path _leftMarker;

		// Token: 0x04003A8E RID: 14990
		private Path _rightMarker;

		// Token: 0x04003A8F RID: 14991
		private DependencyObject _DPHost;

		// Token: 0x04003A90 RID: 14992
		private const byte FocusFlag = 1;

		// Token: 0x04003A91 RID: 14993
		private const byte FocusFlagComplement = 126;

		// Token: 0x04003A92 RID: 14994
		private const byte SelectedFlag = 2;

		// Token: 0x04003A93 RID: 14995
		private const byte SelectedFlagComplement = 125;

		// Token: 0x04003A94 RID: 14996
		private IAttachedAnnotation _attachedAnnotation;

		// Token: 0x04003A95 RID: 14997
		private PresentationContext _presentationContext;

		// Token: 0x04003A96 RID: 14998
		private bool _isDirty = true;

		// Token: 0x04003A97 RID: 14999
		private ITextRange _selection;

		// Token: 0x04003A98 RID: 15000
		private UIElement _uiParent;

		// Token: 0x02000B84 RID: 2948
		private class ComponentsRegister
		{
			// Token: 0x06008E62 RID: 36450 RVA: 0x0025C1FB File Offset: 0x0025A3FB
			public ComponentsRegister(EventHandler selectionHandler, MouseEventHandler mouseMoveHandler)
			{
				this._components = new List<MarkedHighlightComponent>();
				this._selectionHandler = selectionHandler;
				this._mouseMoveHandler = mouseMoveHandler;
			}

			// Token: 0x06008E63 RID: 36451 RVA: 0x0025C21C File Offset: 0x0025A41C
			public void Add(MarkedHighlightComponent component)
			{
				if (this._components.Count == 0)
				{
					UIElement host = component.PresentationContext.Host;
					if (host != null)
					{
						KeyboardNavigation.SetTabNavigation(host, KeyboardNavigationMode.Local);
						KeyboardNavigation.SetControlTabNavigation(host, KeyboardNavigationMode.Local);
					}
				}
				int i = 0;
				while (i < this._components.Count && this.Compare(this._components[i], component) <= 0)
				{
					i++;
				}
				this._components.Insert(i, component);
				while (i < this._components.Count)
				{
					this._components[i].SetTabIndex(i);
					i++;
				}
			}

			// Token: 0x06008E64 RID: 36452 RVA: 0x0025C2B4 File Offset: 0x0025A4B4
			public void Remove(MarkedHighlightComponent component)
			{
				int i = 0;
				while (i < this._components.Count && this._components[i] != component)
				{
					i++;
				}
				if (i < this._components.Count)
				{
					this._components.RemoveAt(i);
					while (i < this._components.Count)
					{
						this._components[i].SetTabIndex(i);
						i++;
					}
				}
			}

			// Token: 0x17001FAF RID: 8111
			// (get) Token: 0x06008E65 RID: 36453 RVA: 0x0025C326 File Offset: 0x0025A526
			public List<MarkedHighlightComponent> Components
			{
				get
				{
					return this._components;
				}
			}

			// Token: 0x17001FB0 RID: 8112
			// (get) Token: 0x06008E66 RID: 36454 RVA: 0x0025C32E File Offset: 0x0025A52E
			public EventHandler SelectionHandler
			{
				get
				{
					return this._selectionHandler;
				}
			}

			// Token: 0x17001FB1 RID: 8113
			// (get) Token: 0x06008E67 RID: 36455 RVA: 0x0025C336 File Offset: 0x0025A536
			public MouseEventHandler MouseMoveHandler
			{
				get
				{
					return this._mouseMoveHandler;
				}
			}

			// Token: 0x06008E68 RID: 36456 RVA: 0x0025C340 File Offset: 0x0025A540
			private int Compare(IAnnotationComponent first, IAnnotationComponent second)
			{
				TextAnchor textAnchor = ((IAttachedAnnotation)first.AttachedAnnotations[0]).FullyAttachedAnchor as TextAnchor;
				TextAnchor textAnchor2 = ((IAttachedAnnotation)second.AttachedAnnotations[0]).FullyAttachedAnchor as TextAnchor;
				int num = textAnchor.Start.CompareTo(textAnchor2.Start);
				if (num == 0)
				{
					num = textAnchor.End.CompareTo(textAnchor2.End);
				}
				return num;
			}

			// Token: 0x04004B8C RID: 19340
			private List<MarkedHighlightComponent> _components;

			// Token: 0x04004B8D RID: 19341
			private EventHandler _selectionHandler;

			// Token: 0x04004B8E RID: 19342
			private MouseEventHandler _mouseMoveHandler;
		}
	}
}
