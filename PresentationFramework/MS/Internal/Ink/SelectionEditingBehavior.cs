using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MS.Internal.Ink
{
	// Token: 0x02000690 RID: 1680
	internal sealed class SelectionEditingBehavior : EditingBehavior
	{
		// Token: 0x06006DD2 RID: 28114 RVA: 0x001F990B File Offset: 0x001F7B0B
		internal SelectionEditingBehavior(EditingCoordinator editingCoordinator, InkCanvas inkCanvas) : base(editingCoordinator, inkCanvas)
		{
		}

		// Token: 0x06006DD3 RID: 28115 RVA: 0x001F9918 File Offset: 0x001F7B18
		protected override void OnActivate()
		{
			this._actionStarted = false;
			this.InitializeCapture();
			MouseDevice primaryDevice = Mouse.PrimaryDevice;
			this._hitResult = base.InkCanvas.SelectionAdorner.SelectionHandleHitTest(primaryDevice.GetPosition(base.InkCanvas.SelectionAdorner));
			base.EditingCoordinator.InvalidateBehaviorCursor(this);
			this._selectionRect = base.InkCanvas.GetSelectionBounds();
			this._previousLocation = primaryDevice.GetPosition(base.InkCanvas.SelectionAdorner);
			this._previousRect = this._selectionRect;
			base.InkCanvas.InkCanvasSelection.StartFeedbackAdorner(this._selectionRect, this._hitResult);
			base.InkCanvas.SelectionAdorner.AddHandler(Mouse.MouseUpEvent, new MouseButtonEventHandler(this.OnMouseUp));
			base.InkCanvas.SelectionAdorner.AddHandler(Mouse.MouseMoveEvent, new MouseEventHandler(this.OnMouseMove));
			base.InkCanvas.SelectionAdorner.AddHandler(UIElement.LostMouseCaptureEvent, new MouseEventHandler(this.OnLostMouseCapture));
		}

		// Token: 0x06006DD4 RID: 28116 RVA: 0x001F9A20 File Offset: 0x001F7C20
		protected override void OnDeactivate()
		{
			base.InkCanvas.SelectionAdorner.RemoveHandler(Mouse.MouseUpEvent, new MouseButtonEventHandler(this.OnMouseUp));
			base.InkCanvas.SelectionAdorner.RemoveHandler(Mouse.MouseMoveEvent, new MouseEventHandler(this.OnMouseMove));
			base.InkCanvas.SelectionAdorner.RemoveHandler(UIElement.LostMouseCaptureEvent, new MouseEventHandler(this.OnLostMouseCapture));
		}

		// Token: 0x06006DD5 RID: 28117 RVA: 0x001F9A90 File Offset: 0x001F7C90
		protected override void OnCommit(bool commit)
		{
			this.ReleaseCapture(true, commit);
		}

		// Token: 0x06006DD6 RID: 28118 RVA: 0x001F9A9A File Offset: 0x001F7C9A
		protected override Cursor GetCurrentCursor()
		{
			return PenCursorManager.GetSelectionCursor(this._hitResult, base.InkCanvas.FlowDirection == FlowDirection.RightToLeft);
		}

		// Token: 0x06006DD7 RID: 28119 RVA: 0x001F9AB8 File Offset: 0x001F7CB8
		private void OnMouseMove(object sender, MouseEventArgs args)
		{
			Point position = args.GetPosition(base.InkCanvas.SelectionAdorner);
			if (!DoubleUtil.AreClose(position.X, this._previousLocation.X) || !DoubleUtil.AreClose(position.Y, this._previousLocation.Y))
			{
				if (!this._actionStarted)
				{
					this._actionStarted = true;
				}
				Rect rect = this.ChangeFeedbackRectangle(position);
				base.InkCanvas.InkCanvasSelection.UpdateFeedbackAdorner(rect);
				this._previousRect = rect;
			}
		}

		// Token: 0x06006DD8 RID: 28120 RVA: 0x001F9B38 File Offset: 0x001F7D38
		private void OnMouseUp(object sender, MouseButtonEventArgs args)
		{
			if (this._actionStarted)
			{
				this._previousRect = this.ChangeFeedbackRectangle(args.GetPosition(base.InkCanvas.SelectionAdorner));
			}
			base.Commit(true);
		}

		// Token: 0x06006DD9 RID: 28121 RVA: 0x001F9B66 File Offset: 0x001F7D66
		private void OnLostMouseCapture(object sender, MouseEventArgs args)
		{
			if (base.EditingCoordinator.UserIsEditing)
			{
				this.ReleaseCapture(false, true);
			}
		}

		// Token: 0x06006DDA RID: 28122 RVA: 0x001F9B80 File Offset: 0x001F7D80
		private Rect ChangeFeedbackRectangle(Point newPoint)
		{
			if ((this._hitResult == InkCanvasSelectionHitResult.TopLeft || this._hitResult == InkCanvasSelectionHitResult.BottomLeft || this._hitResult == InkCanvasSelectionHitResult.Left) && newPoint.X > this._selectionRect.Right - 16.0)
			{
				newPoint.X = this._selectionRect.Right - 16.0;
			}
			if ((this._hitResult == InkCanvasSelectionHitResult.TopRight || this._hitResult == InkCanvasSelectionHitResult.BottomRight || this._hitResult == InkCanvasSelectionHitResult.Right) && newPoint.X < this._selectionRect.Left + 16.0)
			{
				newPoint.X = this._selectionRect.Left + 16.0;
			}
			if ((this._hitResult == InkCanvasSelectionHitResult.TopLeft || this._hitResult == InkCanvasSelectionHitResult.TopRight || this._hitResult == InkCanvasSelectionHitResult.Top) && newPoint.Y > this._selectionRect.Bottom - 16.0)
			{
				newPoint.Y = this._selectionRect.Bottom - 16.0;
			}
			if ((this._hitResult == InkCanvasSelectionHitResult.BottomLeft || this._hitResult == InkCanvasSelectionHitResult.BottomRight || this._hitResult == InkCanvasSelectionHitResult.Bottom) && newPoint.Y < this._selectionRect.Top + 16.0)
			{
				newPoint.Y = this._selectionRect.Top + 16.0;
			}
			Rect result = this.CalculateRect(newPoint.X - this._previousLocation.X, newPoint.Y - this._previousLocation.Y);
			if (this._hitResult == InkCanvasSelectionHitResult.BottomRight || this._hitResult == InkCanvasSelectionHitResult.BottomLeft || this._hitResult == InkCanvasSelectionHitResult.TopRight || this._hitResult == InkCanvasSelectionHitResult.TopLeft || this._hitResult == InkCanvasSelectionHitResult.Selection)
			{
				this._previousLocation.X = newPoint.X;
				this._previousLocation.Y = newPoint.Y;
			}
			else if (this._hitResult == InkCanvasSelectionHitResult.Left || this._hitResult == InkCanvasSelectionHitResult.Right)
			{
				this._previousLocation.X = newPoint.X;
			}
			else if (this._hitResult == InkCanvasSelectionHitResult.Top || this._hitResult == InkCanvasSelectionHitResult.Bottom)
			{
				this._previousLocation.Y = newPoint.Y;
			}
			return result;
		}

		// Token: 0x06006DDB RID: 28123 RVA: 0x001F9DB0 File Offset: 0x001F7FB0
		private Rect CalculateRect(double x, double y)
		{
			Rect rect = this._previousRect;
			switch (this._hitResult)
			{
			case InkCanvasSelectionHitResult.TopLeft:
				rect = SelectionEditingBehavior.ExtendSelectionTop(rect, y);
				rect = SelectionEditingBehavior.ExtendSelectionLeft(rect, x);
				break;
			case InkCanvasSelectionHitResult.Top:
				rect = SelectionEditingBehavior.ExtendSelectionTop(rect, y);
				break;
			case InkCanvasSelectionHitResult.TopRight:
				rect = SelectionEditingBehavior.ExtendSelectionTop(rect, y);
				rect = SelectionEditingBehavior.ExtendSelectionRight(rect, x);
				break;
			case InkCanvasSelectionHitResult.Right:
				rect = SelectionEditingBehavior.ExtendSelectionRight(rect, x);
				break;
			case InkCanvasSelectionHitResult.BottomRight:
				rect = SelectionEditingBehavior.ExtendSelectionRight(rect, x);
				rect = SelectionEditingBehavior.ExtendSelectionBottom(rect, y);
				break;
			case InkCanvasSelectionHitResult.Bottom:
				rect = SelectionEditingBehavior.ExtendSelectionBottom(rect, y);
				break;
			case InkCanvasSelectionHitResult.BottomLeft:
				rect = SelectionEditingBehavior.ExtendSelectionLeft(rect, x);
				rect = SelectionEditingBehavior.ExtendSelectionBottom(rect, y);
				break;
			case InkCanvasSelectionHitResult.Left:
				rect = SelectionEditingBehavior.ExtendSelectionLeft(rect, x);
				break;
			case InkCanvasSelectionHitResult.Selection:
				rect.Offset(x, y);
				break;
			}
			return rect;
		}

		// Token: 0x06006DDC RID: 28124 RVA: 0x001F9E74 File Offset: 0x001F8074
		private static Rect ExtendSelectionLeft(Rect rect, double extendBy)
		{
			Rect result = rect;
			result.X += extendBy;
			result.Width -= extendBy;
			return result;
		}

		// Token: 0x06006DDD RID: 28125 RVA: 0x001F9EA4 File Offset: 0x001F80A4
		private static Rect ExtendSelectionTop(Rect rect, double extendBy)
		{
			Rect result = rect;
			result.Y += extendBy;
			result.Height -= extendBy;
			return result;
		}

		// Token: 0x06006DDE RID: 28126 RVA: 0x001F9ED4 File Offset: 0x001F80D4
		private static Rect ExtendSelectionRight(Rect rect, double extendBy)
		{
			Rect result = rect;
			result.Width += extendBy;
			return result;
		}

		// Token: 0x06006DDF RID: 28127 RVA: 0x001F9EF4 File Offset: 0x001F80F4
		private static Rect ExtendSelectionBottom(Rect rect, double extendBy)
		{
			Rect result = rect;
			result.Height += extendBy;
			return result;
		}

		// Token: 0x06006DE0 RID: 28128 RVA: 0x001F9F13 File Offset: 0x001F8113
		private void InitializeCapture()
		{
			base.EditingCoordinator.UserIsEditing = true;
			base.InkCanvas.SelectionAdorner.CaptureMouse();
		}

		// Token: 0x06006DE1 RID: 28129 RVA: 0x001F9F34 File Offset: 0x001F8134
		private void ReleaseCapture(bool releaseDevice, bool commit)
		{
			if (base.EditingCoordinator.UserIsEditing)
			{
				base.EditingCoordinator.UserIsEditing = false;
				if (releaseDevice)
				{
					base.InkCanvas.SelectionAdorner.ReleaseMouseCapture();
				}
				base.SelfDeactivate();
				base.InkCanvas.InkCanvasSelection.EndFeedbackAdorner(commit ? this._previousRect : this._selectionRect);
			}
		}

		// Token: 0x04003616 RID: 13846
		private const double MinimumHeightWidthSize = 16.0;

		// Token: 0x04003617 RID: 13847
		private Point _previousLocation;

		// Token: 0x04003618 RID: 13848
		private Rect _previousRect;

		// Token: 0x04003619 RID: 13849
		private Rect _selectionRect;

		// Token: 0x0400361A RID: 13850
		private InkCanvasSelectionHitResult _hitResult;

		// Token: 0x0400361B RID: 13851
		private bool _actionStarted;
	}
}
