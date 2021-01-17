﻿using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace MS.Internal.Ink
{
	// Token: 0x0200068B RID: 1675
	internal sealed class InkCollectionBehavior : StylusEditingBehavior
	{
		// Token: 0x06006D99 RID: 28057 RVA: 0x001F7AAD File Offset: 0x001F5CAD
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal InkCollectionBehavior(EditingCoordinator editingCoordinator, InkCanvas inkCanvas) : base(editingCoordinator, inkCanvas)
		{
			this._stylusPoints = null;
			this._userInitiated = false;
		}

		// Token: 0x06006D9A RID: 28058 RVA: 0x001F7AC5 File Offset: 0x001F5CC5
		internal void ResetDynamicRenderer()
		{
			this._resetDynamicRenderer = true;
		}

		// Token: 0x06006D9B RID: 28059 RVA: 0x001F7AD0 File Offset: 0x001F5CD0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnSwitchToMode(InkCanvasEditingMode mode)
		{
			switch (mode)
			{
			case InkCanvasEditingMode.None:
				base.Commit(false);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				break;
			case InkCanvasEditingMode.Ink:
			case InkCanvasEditingMode.GestureOnly:
			case InkCanvasEditingMode.InkAndGesture:
				base.InkCanvas.RaiseActiveEditingModeChanged(new RoutedEventArgs(InkCanvas.ActiveEditingModeChangedEvent, base.InkCanvas));
				return;
			case InkCanvasEditingMode.Select:
			{
				StylusPointCollection stylusPointCollection = (this._stylusPoints != null) ? this._stylusPoints.Clone() : null;
				base.Commit(false);
				IStylusEditing stylusEditing = base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				if (stylusPointCollection != null && stylusEditing != null)
				{
					stylusEditing.AddStylusPoints(stylusPointCollection, false);
					return;
				}
				break;
			}
			case InkCanvasEditingMode.EraseByPoint:
			case InkCanvasEditingMode.EraseByStroke:
				base.Commit(false);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				return;
			default:
				return;
			}
		}

		// Token: 0x06006D9C RID: 28060 RVA: 0x001F7B84 File Offset: 0x001F5D84
		protected override void OnActivate()
		{
			base.OnActivate();
			if (base.InkCanvas.InternalDynamicRenderer != null)
			{
				base.InkCanvas.InternalDynamicRenderer.Enabled = true;
				base.InkCanvas.UpdateDynamicRenderer();
			}
			this._resetDynamicRenderer = base.EditingCoordinator.StylusOrMouseIsDown;
		}

		// Token: 0x06006D9D RID: 28061 RVA: 0x001F7BD1 File Offset: 0x001F5DD1
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			if (base.InkCanvas.InternalDynamicRenderer != null)
			{
				base.InkCanvas.InternalDynamicRenderer.Enabled = false;
				base.InkCanvas.UpdateDynamicRenderer();
			}
		}

		// Token: 0x06006D9E RID: 28062 RVA: 0x001F7C02 File Offset: 0x001F5E02
		protected override Cursor GetCurrentCursor()
		{
			if (base.EditingCoordinator.UserIsEditing)
			{
				return Cursors.None;
			}
			return this.PenCursor;
		}

		// Token: 0x06006D9F RID: 28063 RVA: 0x001F7C20 File Offset: 0x001F5E20
		[SecurityCritical]
		protected override void StylusInputBegin(StylusPointCollection stylusPoints, bool userInitiated)
		{
			this._userInitiated = false;
			if (userInitiated)
			{
				this._userInitiated = true;
			}
			this._stylusPoints = new StylusPointCollection(stylusPoints.Description, 100);
			this._stylusPoints.Add(stylusPoints);
			this._strokeDrawingAttributes = base.InkCanvas.DefaultDrawingAttributes.Clone();
			if (this._resetDynamicRenderer)
			{
				InputDevice inputDeviceForReset = base.EditingCoordinator.GetInputDeviceForReset();
				if (base.InkCanvas.InternalDynamicRenderer != null && inputDeviceForReset != null)
				{
					StylusDevice stylusDevice = inputDeviceForReset as StylusDevice;
					base.InkCanvas.InternalDynamicRenderer.Reset(stylusDevice, stylusPoints);
				}
				this._resetDynamicRenderer = false;
			}
			base.EditingCoordinator.InvalidateBehaviorCursor(this);
		}

		// Token: 0x06006DA0 RID: 28064 RVA: 0x001F7CC3 File Offset: 0x001F5EC3
		[SecurityCritical]
		protected override void StylusInputContinue(StylusPointCollection stylusPoints, bool userInitiated)
		{
			if (!userInitiated)
			{
				this._userInitiated = false;
			}
			this._stylusPoints.Add(stylusPoints);
		}

		// Token: 0x06006DA1 RID: 28065 RVA: 0x001F7CDC File Offset: 0x001F5EDC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void StylusInputEnd(bool commit)
		{
			try
			{
				if (commit && this._stylusPoints != null)
				{
					Stroke stroke = new Stroke(this._stylusPoints, this._strokeDrawingAttributes);
					InkCanvasStrokeCollectedEventArgs e = new InkCanvasStrokeCollectedEventArgs(stroke);
					base.InkCanvas.RaiseGestureOrStrokeCollected(e, this._userInitiated);
				}
			}
			finally
			{
				this._stylusPoints = null;
				this._strokeDrawingAttributes = null;
				this._userInitiated = false;
				base.EditingCoordinator.InvalidateBehaviorCursor(this);
			}
		}

		// Token: 0x06006DA2 RID: 28066 RVA: 0x001F7D54 File Offset: 0x001F5F54
		protected override void OnTransformChanged()
		{
			this._cachedPenCursor = null;
		}

		// Token: 0x17001A20 RID: 6688
		// (get) Token: 0x06006DA3 RID: 28067 RVA: 0x001F7D60 File Offset: 0x001F5F60
		private Cursor PenCursor
		{
			get
			{
				if (this._cachedPenCursor == null || this._cursorDrawingAttributes != base.InkCanvas.DefaultDrawingAttributes)
				{
					Matrix matrix = base.GetElementTransformMatrix();
					DrawingAttributes drawingAttributes = base.InkCanvas.DefaultDrawingAttributes;
					if (!matrix.IsIdentity)
					{
						matrix *= drawingAttributes.StylusTipTransform;
						matrix.OffsetX = 0.0;
						matrix.OffsetY = 0.0;
						if (matrix.HasInverse)
						{
							drawingAttributes = drawingAttributes.Clone();
							drawingAttributes.StylusTipTransform = matrix;
						}
					}
					this._cursorDrawingAttributes = base.InkCanvas.DefaultDrawingAttributes.Clone();
					DpiScale dpi = base.InkCanvas.GetDpi();
					this._cachedPenCursor = PenCursorManager.GetPenCursor(drawingAttributes, false, base.InkCanvas.FlowDirection == FlowDirection.RightToLeft, dpi.DpiScaleX, dpi.DpiScaleY);
				}
				return this._cachedPenCursor;
			}
		}

		// Token: 0x040035F4 RID: 13812
		private bool _resetDynamicRenderer;

		// Token: 0x040035F5 RID: 13813
		[SecurityCritical]
		private StylusPointCollection _stylusPoints;

		// Token: 0x040035F6 RID: 13814
		[SecurityCritical]
		private bool _userInitiated;

		// Token: 0x040035F7 RID: 13815
		private DrawingAttributes _strokeDrawingAttributes;

		// Token: 0x040035F8 RID: 13816
		private DrawingAttributes _cursorDrawingAttributes;

		// Token: 0x040035F9 RID: 13817
		private Cursor _cachedPenCursor;
	}
}
