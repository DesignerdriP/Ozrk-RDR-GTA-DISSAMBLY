using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Documents
{
	// Token: 0x020006F7 RID: 1783
	internal abstract class TextViewBase : ITextView
	{
		// Token: 0x060072BB RID: 29371
		internal abstract ITextPointer GetTextPositionFromPoint(Point point, bool snapToText);

		// Token: 0x060072BC RID: 29372 RVA: 0x0020F784 File Offset: 0x0020D984
		internal virtual Rect GetRectangleFromTextPosition(ITextPointer position)
		{
			Transform transform;
			Rect rect = this.GetRawRectangleFromTextPosition(position, out transform);
			Invariant.Assert(transform != null);
			if (rect != Rect.Empty)
			{
				rect = transform.TransformBounds(rect);
			}
			return rect;
		}

		// Token: 0x060072BD RID: 29373
		internal abstract Rect GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform);

		// Token: 0x060072BE RID: 29374
		internal abstract Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition);

		// Token: 0x060072BF RID: 29375
		internal abstract ITextPointer GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved);

		// Token: 0x060072C0 RID: 29376 RVA: 0x0020F7BA File Offset: 0x0020D9BA
		internal virtual ITextPointer GetPositionAtNextPage(ITextPointer position, Point suggestedOffset, int count, out Point newSuggestedOffset, out int pagesMoved)
		{
			newSuggestedOffset = suggestedOffset;
			pagesMoved = 0;
			return position;
		}

		// Token: 0x060072C1 RID: 29377
		internal abstract bool IsAtCaretUnitBoundary(ITextPointer position);

		// Token: 0x060072C2 RID: 29378
		internal abstract ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction);

		// Token: 0x060072C3 RID: 29379
		internal abstract ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position);

		// Token: 0x060072C4 RID: 29380
		internal abstract TextSegment GetLineRange(ITextPointer position);

		// Token: 0x060072C5 RID: 29381 RVA: 0x0020F7C9 File Offset: 0x0020D9C9
		internal virtual ReadOnlyCollection<GlyphRun> GetGlyphRuns(ITextPointer start, ITextPointer end)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			return new ReadOnlyCollection<GlyphRun>(new List<GlyphRun>());
		}

		// Token: 0x060072C6 RID: 29382
		internal abstract bool Contains(ITextPointer position);

		// Token: 0x060072C7 RID: 29383 RVA: 0x0020F7F0 File Offset: 0x0020D9F0
		internal static void BringRectIntoViewMinimally(ITextView textView, Rect rect)
		{
			IScrollInfo scrollInfo = textView.RenderScope as IScrollInfo;
			if (scrollInfo != null)
			{
				Rect rect2 = new Rect(scrollInfo.HorizontalOffset, scrollInfo.VerticalOffset, scrollInfo.ViewportWidth, scrollInfo.ViewportHeight);
				rect.X += rect2.X;
				rect.Y += rect2.Y;
				double num = ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(rect2.Left, rect2.Right, rect.Left, rect.Right);
				double num2 = ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(rect2.Top, rect2.Bottom, rect.Top, rect.Bottom);
				scrollInfo.SetHorizontalOffset(num);
				scrollInfo.SetVerticalOffset(num2);
				FrameworkElement frameworkElement = FrameworkElement.GetFrameworkParent(textView.RenderScope) as FrameworkElement;
				if (frameworkElement != null)
				{
					if (scrollInfo.ViewportWidth > 0.0)
					{
						rect.X -= num;
					}
					if (scrollInfo.ViewportHeight > 0.0)
					{
						rect.Y -= num2;
					}
					frameworkElement.BringIntoView(rect);
					return;
				}
			}
			else
			{
				((FrameworkElement)textView.RenderScope).BringIntoView(rect);
			}
		}

		// Token: 0x060072C8 RID: 29384 RVA: 0x0020F91A File Offset: 0x0020DB1A
		internal virtual void BringPositionIntoViewAsync(ITextPointer position, object userState)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			this.OnBringPositionIntoViewCompleted(new BringPositionIntoViewCompletedEventArgs(position, this.Contains(position), null, false, userState));
		}

		// Token: 0x060072C9 RID: 29385 RVA: 0x0020F94C File Offset: 0x0020DB4C
		internal virtual void BringPointIntoViewAsync(Point point, object userState)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ITextPointer textPositionFromPoint = this.GetTextPositionFromPoint(point, true);
			this.OnBringPointIntoViewCompleted(new BringPointIntoViewCompletedEventArgs(point, textPositionFromPoint, textPositionFromPoint != null, null, false, userState));
		}

		// Token: 0x060072CA RID: 29386 RVA: 0x0020F990 File Offset: 0x0020DB90
		internal virtual void BringLineIntoViewAsync(ITextPointer position, double suggestedX, int count, object userState)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			double newSuggestedX;
			int num;
			ITextPointer positionAtNextLine = this.GetPositionAtNextLine(position, suggestedX, count, out newSuggestedX, out num);
			this.OnBringLineIntoViewCompleted(new BringLineIntoViewCompletedEventArgs(position, suggestedX, count, positionAtNextLine, newSuggestedX, num, num == count, null, false, userState));
		}

		// Token: 0x060072CB RID: 29387 RVA: 0x0020F9DC File Offset: 0x0020DBDC
		internal virtual void BringPageIntoViewAsync(ITextPointer position, Point suggestedOffset, int count, object userState)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			Point newSuggestedOffset;
			int num;
			ITextPointer positionAtNextPage = this.GetPositionAtNextPage(position, suggestedOffset, count, out newSuggestedOffset, out num);
			this.OnBringPageIntoViewCompleted(new BringPageIntoViewCompletedEventArgs(position, suggestedOffset, count, positionAtNextPage, newSuggestedOffset, num, num == count, null, false, userState));
		}

		// Token: 0x060072CC RID: 29388 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void CancelAsync(object userState)
		{
		}

		// Token: 0x060072CD RID: 29389 RVA: 0x0020B67D File Offset: 0x0020987D
		internal virtual bool Validate()
		{
			return this.IsValid;
		}

		// Token: 0x060072CE RID: 29390 RVA: 0x0020FA28 File Offset: 0x0020DC28
		internal virtual bool Validate(Point point)
		{
			return this.Validate();
		}

		// Token: 0x060072CF RID: 29391 RVA: 0x0020FA30 File Offset: 0x0020DC30
		internal virtual bool Validate(ITextPointer position)
		{
			this.Validate();
			return this.IsValid && this.Contains(position);
		}

		// Token: 0x060072D0 RID: 29392 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void ThrottleBackgroundTasksForUserInput()
		{
		}

		// Token: 0x17001B3E RID: 6974
		// (get) Token: 0x060072D1 RID: 29393
		internal abstract UIElement RenderScope { get; }

		// Token: 0x17001B3F RID: 6975
		// (get) Token: 0x060072D2 RID: 29394
		internal abstract ITextContainer TextContainer { get; }

		// Token: 0x17001B40 RID: 6976
		// (get) Token: 0x060072D3 RID: 29395
		internal abstract bool IsValid { get; }

		// Token: 0x17001B41 RID: 6977
		// (get) Token: 0x060072D4 RID: 29396 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool RendersOwnSelection
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001B42 RID: 6978
		// (get) Token: 0x060072D5 RID: 29397
		internal abstract ReadOnlyCollection<TextSegment> TextSegments { get; }

		// Token: 0x1400014F RID: 335
		// (add) Token: 0x060072D6 RID: 29398 RVA: 0x0020FA4C File Offset: 0x0020DC4C
		// (remove) Token: 0x060072D7 RID: 29399 RVA: 0x0020FA84 File Offset: 0x0020DC84
		public event BringPositionIntoViewCompletedEventHandler BringPositionIntoViewCompleted;

		// Token: 0x14000150 RID: 336
		// (add) Token: 0x060072D8 RID: 29400 RVA: 0x0020FABC File Offset: 0x0020DCBC
		// (remove) Token: 0x060072D9 RID: 29401 RVA: 0x0020FAF4 File Offset: 0x0020DCF4
		public event BringPointIntoViewCompletedEventHandler BringPointIntoViewCompleted;

		// Token: 0x14000151 RID: 337
		// (add) Token: 0x060072DA RID: 29402 RVA: 0x0020FB2C File Offset: 0x0020DD2C
		// (remove) Token: 0x060072DB RID: 29403 RVA: 0x0020FB64 File Offset: 0x0020DD64
		public event BringLineIntoViewCompletedEventHandler BringLineIntoViewCompleted;

		// Token: 0x14000152 RID: 338
		// (add) Token: 0x060072DC RID: 29404 RVA: 0x0020FB9C File Offset: 0x0020DD9C
		// (remove) Token: 0x060072DD RID: 29405 RVA: 0x0020FBD4 File Offset: 0x0020DDD4
		public event BringPageIntoViewCompletedEventHandler BringPageIntoViewCompleted;

		// Token: 0x14000153 RID: 339
		// (add) Token: 0x060072DE RID: 29406 RVA: 0x0020FC0C File Offset: 0x0020DE0C
		// (remove) Token: 0x060072DF RID: 29407 RVA: 0x0020FC44 File Offset: 0x0020DE44
		public event EventHandler Updated;

		// Token: 0x060072E0 RID: 29408 RVA: 0x0020FC79 File Offset: 0x0020DE79
		protected virtual void OnBringPositionIntoViewCompleted(BringPositionIntoViewCompletedEventArgs e)
		{
			if (this.BringPositionIntoViewCompleted != null)
			{
				this.BringPositionIntoViewCompleted(this, e);
			}
		}

		// Token: 0x060072E1 RID: 29409 RVA: 0x0020FC90 File Offset: 0x0020DE90
		protected virtual void OnBringPointIntoViewCompleted(BringPointIntoViewCompletedEventArgs e)
		{
			if (this.BringPointIntoViewCompleted != null)
			{
				this.BringPointIntoViewCompleted(this, e);
			}
		}

		// Token: 0x060072E2 RID: 29410 RVA: 0x0020FCA7 File Offset: 0x0020DEA7
		protected virtual void OnBringLineIntoViewCompleted(BringLineIntoViewCompletedEventArgs e)
		{
			if (this.BringLineIntoViewCompleted != null)
			{
				this.BringLineIntoViewCompleted(this, e);
			}
		}

		// Token: 0x060072E3 RID: 29411 RVA: 0x0020FCBE File Offset: 0x0020DEBE
		protected virtual void OnBringPageIntoViewCompleted(BringPageIntoViewCompletedEventArgs e)
		{
			if (this.BringPageIntoViewCompleted != null)
			{
				this.BringPageIntoViewCompleted(this, e);
			}
		}

		// Token: 0x060072E4 RID: 29412 RVA: 0x0020FCD5 File Offset: 0x0020DED5
		protected virtual void OnUpdated(EventArgs e)
		{
			if (this.Updated != null)
			{
				this.Updated(this, e);
			}
		}

		// Token: 0x060072E5 RID: 29413 RVA: 0x0020FCEC File Offset: 0x0020DEEC
		protected virtual Transform GetAggregateTransform(Transform firstTransform, Transform secondTransform)
		{
			Invariant.Assert(firstTransform != null);
			Invariant.Assert(secondTransform != null);
			if (firstTransform.IsIdentity)
			{
				return secondTransform;
			}
			if (secondTransform.IsIdentity)
			{
				return firstTransform;
			}
			return new MatrixTransform(firstTransform.Value * secondTransform.Value);
		}

		// Token: 0x060072E6 RID: 29414 RVA: 0x0020FD37 File Offset: 0x0020DF37
		ITextPointer ITextView.GetTextPositionFromPoint(Point point, bool snapToText)
		{
			return this.GetTextPositionFromPoint(point, snapToText);
		}

		// Token: 0x060072E7 RID: 29415 RVA: 0x0020FD41 File Offset: 0x0020DF41
		Rect ITextView.GetRectangleFromTextPosition(ITextPointer position)
		{
			return this.GetRectangleFromTextPosition(position);
		}

		// Token: 0x060072E8 RID: 29416 RVA: 0x0020FD4A File Offset: 0x0020DF4A
		Rect ITextView.GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform)
		{
			return this.GetRawRectangleFromTextPosition(position, out transform);
		}

		// Token: 0x060072E9 RID: 29417 RVA: 0x0020FD54 File Offset: 0x0020DF54
		Geometry ITextView.GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			return this.GetTightBoundingGeometryFromTextPositions(startPosition, endPosition);
		}

		// Token: 0x060072EA RID: 29418 RVA: 0x0020FD5E File Offset: 0x0020DF5E
		ITextPointer ITextView.GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved)
		{
			return this.GetPositionAtNextLine(position, suggestedX, count, out newSuggestedX, out linesMoved);
		}

		// Token: 0x060072EB RID: 29419 RVA: 0x0020FD6D File Offset: 0x0020DF6D
		ITextPointer ITextView.GetPositionAtNextPage(ITextPointer position, Point suggestedOffset, int count, out Point newSuggestedOffset, out int pagesMoved)
		{
			return this.GetPositionAtNextPage(position, suggestedOffset, count, out newSuggestedOffset, out pagesMoved);
		}

		// Token: 0x060072EC RID: 29420 RVA: 0x0020FD7C File Offset: 0x0020DF7C
		bool ITextView.IsAtCaretUnitBoundary(ITextPointer position)
		{
			return this.IsAtCaretUnitBoundary(position);
		}

		// Token: 0x060072ED RID: 29421 RVA: 0x0020FD85 File Offset: 0x0020DF85
		ITextPointer ITextView.GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			return this.GetNextCaretUnitPosition(position, direction);
		}

		// Token: 0x060072EE RID: 29422 RVA: 0x0020FD8F File Offset: 0x0020DF8F
		ITextPointer ITextView.GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			return this.GetBackspaceCaretUnitPosition(position);
		}

		// Token: 0x060072EF RID: 29423 RVA: 0x0020FD98 File Offset: 0x0020DF98
		TextSegment ITextView.GetLineRange(ITextPointer position)
		{
			return this.GetLineRange(position);
		}

		// Token: 0x060072F0 RID: 29424 RVA: 0x0020FDA1 File Offset: 0x0020DFA1
		ReadOnlyCollection<GlyphRun> ITextView.GetGlyphRuns(ITextPointer start, ITextPointer end)
		{
			return this.GetGlyphRuns(start, end);
		}

		// Token: 0x060072F1 RID: 29425 RVA: 0x0020FDAB File Offset: 0x0020DFAB
		bool ITextView.Contains(ITextPointer position)
		{
			return this.Contains(position);
		}

		// Token: 0x060072F2 RID: 29426 RVA: 0x0020FDB4 File Offset: 0x0020DFB4
		void ITextView.BringPositionIntoViewAsync(ITextPointer position, object userState)
		{
			this.BringPositionIntoViewAsync(position, userState);
		}

		// Token: 0x060072F3 RID: 29427 RVA: 0x0020FDBE File Offset: 0x0020DFBE
		void ITextView.BringPointIntoViewAsync(Point point, object userState)
		{
			this.BringPointIntoViewAsync(point, userState);
		}

		// Token: 0x060072F4 RID: 29428 RVA: 0x0020FDC8 File Offset: 0x0020DFC8
		void ITextView.BringLineIntoViewAsync(ITextPointer position, double suggestedX, int count, object userState)
		{
			this.BringLineIntoViewAsync(position, suggestedX, count, userState);
		}

		// Token: 0x060072F5 RID: 29429 RVA: 0x0020FDD5 File Offset: 0x0020DFD5
		void ITextView.BringPageIntoViewAsync(ITextPointer position, Point suggestedOffset, int count, object userState)
		{
			this.BringPageIntoViewAsync(position, suggestedOffset, count, userState);
		}

		// Token: 0x060072F6 RID: 29430 RVA: 0x0020FDE2 File Offset: 0x0020DFE2
		void ITextView.CancelAsync(object userState)
		{
			this.CancelAsync(userState);
		}

		// Token: 0x060072F7 RID: 29431 RVA: 0x0020FA28 File Offset: 0x0020DC28
		bool ITextView.Validate()
		{
			return this.Validate();
		}

		// Token: 0x060072F8 RID: 29432 RVA: 0x0020FDEB File Offset: 0x0020DFEB
		bool ITextView.Validate(Point point)
		{
			return this.Validate(point);
		}

		// Token: 0x060072F9 RID: 29433 RVA: 0x0020FDF4 File Offset: 0x0020DFF4
		bool ITextView.Validate(ITextPointer position)
		{
			return this.Validate(position);
		}

		// Token: 0x060072FA RID: 29434 RVA: 0x0020FDFD File Offset: 0x0020DFFD
		void ITextView.ThrottleBackgroundTasksForUserInput()
		{
			this.ThrottleBackgroundTasksForUserInput();
		}

		// Token: 0x17001B43 RID: 6979
		// (get) Token: 0x060072FB RID: 29435 RVA: 0x0020FE05 File Offset: 0x0020E005
		UIElement ITextView.RenderScope
		{
			get
			{
				return this.RenderScope;
			}
		}

		// Token: 0x17001B44 RID: 6980
		// (get) Token: 0x060072FC RID: 29436 RVA: 0x0020FE0D File Offset: 0x0020E00D
		ITextContainer ITextView.TextContainer
		{
			get
			{
				return this.TextContainer;
			}
		}

		// Token: 0x17001B45 RID: 6981
		// (get) Token: 0x060072FD RID: 29437 RVA: 0x0020B67D File Offset: 0x0020987D
		bool ITextView.IsValid
		{
			get
			{
				return this.IsValid;
			}
		}

		// Token: 0x17001B46 RID: 6982
		// (get) Token: 0x060072FE RID: 29438 RVA: 0x0020FE15 File Offset: 0x0020E015
		bool ITextView.RendersOwnSelection
		{
			get
			{
				return this.RendersOwnSelection;
			}
		}

		// Token: 0x17001B47 RID: 6983
		// (get) Token: 0x060072FF RID: 29439 RVA: 0x0020FE1D File Offset: 0x0020E01D
		ReadOnlyCollection<TextSegment> ITextView.TextSegments
		{
			get
			{
				return this.TextSegments;
			}
		}
	}
}
