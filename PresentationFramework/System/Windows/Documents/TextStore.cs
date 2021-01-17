using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Documents;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x0200041A RID: 1050
	internal class TextStore : UnsafeNativeMethods.ITextStoreACP, UnsafeNativeMethods.ITfThreadFocusSink, UnsafeNativeMethods.ITfContextOwnerCompositionSink, UnsafeNativeMethods.ITfTextEditSink, UnsafeNativeMethods.ITfTransitoryExtensionSink, UnsafeNativeMethods.ITfMouseTrackerACP
	{
		// Token: 0x06003C97 RID: 15511 RVA: 0x00118088 File Offset: 0x00116288
		internal TextStore(TextEditor textEditor)
		{
			this._weakTextEditor = new TextStore.ScopeWeakReference(textEditor);
			this._threadFocusCookie = -1;
			this._editSinkCookie = -1;
			this._editCookie = -1;
			this._transitoryExtensionSinkCookie = -1;
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x001180D8 File Offset: 0x001162D8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void AdviseSink(ref Guid riid, object obj, UnsafeNativeMethods.AdviseFlags flags)
		{
			if (riid != UnsafeNativeMethods.IID_ITextStoreACPSink)
			{
				throw new COMException(SR.Get("TextStore_CONNECT_E_CANNOTCONNECT"), -2147220990);
			}
			UnsafeNativeMethods.ITextStoreACPSink textStoreACPSink = obj as UnsafeNativeMethods.ITextStoreACPSink;
			if (textStoreACPSink == null)
			{
				throw new COMException(SR.Get("TextStore_E_NOINTERFACE"), -2147467262);
			}
			if (this.HasSink)
			{
				Marshal.ReleaseComObject(this._sink);
			}
			else
			{
				this._textservicesHost.RegisterWinEventSink(this);
			}
			this._sink = textStoreACPSink;
		}

		// Token: 0x06003C99 RID: 15513 RVA: 0x00118154 File Offset: 0x00116354
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void UnadviseSink(object obj)
		{
			if (obj != this._sink)
			{
				throw new COMException(SR.Get("TextStore_CONNECT_E_NOCONNECTION"), -2147220992);
			}
			Marshal.ReleaseComObject(this._sink);
			this._sink = null;
			this._textservicesHost.UnregisterWinEventSink(this);
		}

		// Token: 0x06003C9A RID: 15514 RVA: 0x00118194 File Offset: 0x00116394
		public void RequestLock(UnsafeNativeMethods.LockFlags flags, out int hrSession)
		{
			if (!this.HasSink)
			{
				throw new COMException(SR.Get("TextStore_NoSink"));
			}
			if (flags == (UnsafeNativeMethods.LockFlags)0)
			{
				throw new COMException(SR.Get("TextStore_BadLockFlags"));
			}
			if (this._lockFlags != (UnsafeNativeMethods.LockFlags)0)
			{
				if ((this._lockFlags & UnsafeNativeMethods.LockFlags.TS_LF_WRITE) == UnsafeNativeMethods.LockFlags.TS_LF_WRITE || (flags & UnsafeNativeMethods.LockFlags.TS_LF_WRITE) == (UnsafeNativeMethods.LockFlags)0 || (flags & UnsafeNativeMethods.LockFlags.TS_LF_SYNC) == UnsafeNativeMethods.LockFlags.TS_LF_SYNC)
				{
					throw new COMException(SR.Get("TextStore_ReentrantRequestLock"));
				}
				this._pendingWriteReq = true;
				hrSession = 262912;
				return;
			}
			else
			{
				if (this._textChangeReentrencyCount == 0)
				{
					hrSession = this.GrantLockWorker(flags);
					return;
				}
				if ((flags & UnsafeNativeMethods.LockFlags.TS_LF_SYNC) == (UnsafeNativeMethods.LockFlags)0)
				{
					if (this._pendingAsyncLockFlags == (UnsafeNativeMethods.LockFlags)0)
					{
						this._pendingAsyncLockFlags = flags;
						Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.GrantLockHandler), null);
					}
					else if ((flags & UnsafeNativeMethods.LockFlags.TS_LF_READWRITE & this._pendingAsyncLockFlags) != (flags & UnsafeNativeMethods.LockFlags.TS_LF_READWRITE))
					{
						this._pendingAsyncLockFlags = flags;
					}
					hrSession = 262912;
					return;
				}
				hrSession = -2147220984;
				return;
			}
		}

		// Token: 0x06003C9B RID: 15515 RVA: 0x00118270 File Offset: 0x00116470
		public void GetStatus(out UnsafeNativeMethods.TS_STATUS status)
		{
			if (this.IsTextEditorValid && this.IsReadOnly)
			{
				status.dynamicFlags = UnsafeNativeMethods.DynamicStatusFlags.TS_SD_READONLY;
			}
			else
			{
				status.dynamicFlags = (UnsafeNativeMethods.DynamicStatusFlags)0;
			}
			status.staticFlags = UnsafeNativeMethods.StaticStatusFlags.TS_SS_REGIONS;
		}

		// Token: 0x06003C9C RID: 15516 RVA: 0x00118299 File Offset: 0x00116499
		public void QueryInsert(int startIndex, int endIndex, int cch, out int startResultIndex, out int endResultIndex)
		{
			startResultIndex = startIndex;
			endResultIndex = endIndex;
		}

		// Token: 0x06003C9D RID: 15517 RVA: 0x001182A4 File Offset: 0x001164A4
		public void GetSelection(int index, int count, UnsafeNativeMethods.TS_SELECTION_ACP[] selection, out int fetched)
		{
			fetched = 0;
			if (count > 0 && (index == 0 || index == -1))
			{
				selection[0].start = this.TextSelection.Start.CharOffset;
				selection[0].end = this.TextSelection.End.CharOffset;
				selection[0].style.ase = ((this.TextSelection.MovingPosition.CompareTo(this.TextSelection.Start) == 0) ? UnsafeNativeMethods.TsActiveSelEnd.TS_AE_START : UnsafeNativeMethods.TsActiveSelEnd.TS_AE_END);
				selection[0].style.interimChar = this._interimSelection;
				fetched = 1;
			}
		}

		// Token: 0x06003C9E RID: 15518 RVA: 0x0011834C File Offset: 0x0011654C
		public void SetSelection(int count, UnsafeNativeMethods.TS_SELECTION_ACP[] selection)
		{
			if (count == 1)
			{
				ITextPointer textPointer;
				ITextPointer textPointer2;
				this.GetNormalizedRange(selection[0].start, selection[0].end, out textPointer, out textPointer2);
				if (selection[0].start == selection[0].end)
				{
					this.TextSelection.SetCaretToPosition(textPointer, LogicalDirection.Backward, true, true);
				}
				else if (selection[0].style.ase == UnsafeNativeMethods.TsActiveSelEnd.TS_AE_START)
				{
					this.TextSelection.Select(textPointer2, textPointer);
				}
				else
				{
					this.TextSelection.Select(textPointer, textPointer2);
				}
				bool interimSelection = this._interimSelection;
				this._interimSelection = selection[0].style.interimChar;
				if (interimSelection != this._interimSelection)
				{
					this.TextSelection.OnInterimSelectionChanged(this._interimSelection);
				}
			}
		}

		// Token: 0x06003C9F RID: 15519 RVA: 0x00118418 File Offset: 0x00116618
		public void GetText(int startIndex, int endIndex, char[] text, int cchReq, out int charsCopied, UnsafeNativeMethods.TS_RUNINFO[] runInfo, int cRunInfoReq, out int cRunInfoRcv, out int nextIndex)
		{
			charsCopied = 0;
			cRunInfoRcv = 0;
			nextIndex = startIndex;
			if (cchReq == 0 && cRunInfoReq == 0)
			{
				return;
			}
			if (startIndex == endIndex)
			{
				return;
			}
			ITextPointer textPointer = this.CreatePointerAtCharOffset(startIndex, LogicalDirection.Forward);
			ITextPointer textPointer2 = (endIndex >= 0) ? this.CreatePointerAtCharOffset(endIndex, LogicalDirection.Forward) : null;
			bool flag = false;
			while (!flag && (cchReq == 0 || cchReq > charsCopied) && (cRunInfoReq == 0 || cRunInfoReq > cRunInfoRcv))
			{
				switch (textPointer.GetPointerContext(LogicalDirection.Forward))
				{
				case TextPointerContext.None:
					flag = true;
					break;
				case TextPointerContext.Text:
					flag = TextStore.WalkTextRun(textPointer, textPointer2, text, cchReq, ref charsCopied, runInfo, cRunInfoReq, ref cRunInfoRcv);
					break;
				case TextPointerContext.EmbeddedElement:
					flag = TextStore.WalkObjectRun(textPointer, textPointer2, text, cchReq, ref charsCopied, runInfo, cRunInfoReq, ref cRunInfoRcv);
					break;
				case TextPointerContext.ElementStart:
				{
					Invariant.Assert(textPointer is TextPointer);
					TextElement textElement = (TextElement)((TextPointer)textPointer).GetAdjacentElement(LogicalDirection.Forward);
					if (textElement.IMELeftEdgeCharCount > 0)
					{
						Invariant.Assert(textElement.IMELeftEdgeCharCount == 1);
						flag = TextStore.WalkRegionBoundary(textPointer, textPointer2, text, cchReq, ref charsCopied, runInfo, cRunInfoReq, ref cRunInfoRcv);
					}
					else
					{
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
						flag = (textPointer2 != null && textPointer.CompareTo(textPointer2) >= 0);
					}
					break;
				}
				case TextPointerContext.ElementEnd:
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					flag = (textPointer2 != null && textPointer.CompareTo(textPointer2) >= 0);
					break;
				default:
					Invariant.Assert(false, "Bogus TextPointerContext!");
					break;
				}
			}
			nextIndex = textPointer.CharOffset;
		}

		// Token: 0x06003CA0 RID: 15520 RVA: 0x00118578 File Offset: 0x00116778
		public void SetText(UnsafeNativeMethods.SetTextFlags flags, int startIndex, int endIndex, char[] text, int cch, out UnsafeNativeMethods.TS_TEXTCHANGE change)
		{
			if (this.IsReadOnly)
			{
				throw new COMException(SR.Get("TextStore_TS_E_READONLY"), -2147220983);
			}
			ITextPointer nextInsertionPosition;
			ITextPointer textPointer;
			this.GetNormalizedRange(startIndex, endIndex, out nextInsertionPosition, out textPointer);
			while (nextInsertionPosition != null && TextPointerBase.IsBeforeFirstTable(nextInsertionPosition))
			{
				nextInsertionPosition = nextInsertionPosition.GetNextInsertionPosition(LogicalDirection.Forward);
			}
			if (nextInsertionPosition == null)
			{
				throw new COMException(SR.Get("TextStore_CompositionRejected"), -2147467259);
			}
			if (nextInsertionPosition.CompareTo(textPointer) > 0)
			{
				textPointer = nextInsertionPosition;
			}
			string text2 = this.FilterCompositionString(new string(text), nextInsertionPosition.GetOffsetToPosition(textPointer));
			if (text2 == null)
			{
				throw new COMException(SR.Get("TextStore_CompositionRejected"), -2147467259);
			}
			TextStore.CompositionParentUndoUnit textParentUndoUnit = this.OpenCompositionUndoUnit();
			UndoCloseAction undoCloseAction = UndoCloseAction.Rollback;
			try
			{
				ITextRange range = new TextRange(nextInsertionPosition, textPointer, true);
				this.TextEditor.SetText(range, text2, InputLanguageManager.Current.CurrentInputLanguage);
				change.start = startIndex;
				change.oldEnd = endIndex;
				change.newEnd = endIndex + text.Length - (endIndex - startIndex);
				this.ValidateChange(change);
				this.VerifyTextStoreConsistency();
				undoCloseAction = UndoCloseAction.Commit;
			}
			finally
			{
				this.CloseTextParentUndoUnit(textParentUndoUnit, undoCloseAction);
			}
		}

		// Token: 0x06003CA1 RID: 15521 RVA: 0x00118694 File Offset: 0x00116894
		public void GetFormattedText(int startIndex, int endIndex, out object obj)
		{
			obj = null;
			throw new COMException(SR.Get("TextStore_E_NOTIMPL"), -2147467263);
		}

		// Token: 0x06003CA2 RID: 15522 RVA: 0x001186AD File Offset: 0x001168AD
		public void GetEmbedded(int index, ref Guid guidService, ref Guid riid, out object obj)
		{
			obj = null;
		}

		// Token: 0x06003CA3 RID: 15523 RVA: 0x001186B3 File Offset: 0x001168B3
		public void QueryInsertEmbedded(ref Guid guidService, int formatEtc, out bool insertable)
		{
			insertable = false;
		}

		// Token: 0x06003CA4 RID: 15524 RVA: 0x001186B8 File Offset: 0x001168B8
		public void InsertEmbedded(UnsafeNativeMethods.InsertEmbeddedFlags flags, int startIndex, int endIndex, object obj, out UnsafeNativeMethods.TS_TEXTCHANGE change)
		{
			if (this.IsReadOnly)
			{
				throw new COMException(SR.Get("TextStore_TS_E_READONLY"), -2147220983);
			}
			throw new COMException(SR.Get("TextStore_TS_E_FORMAT"), -2147220982);
		}

		// Token: 0x06003CA5 RID: 15525 RVA: 0x001186EC File Offset: 0x001168EC
		public void InsertTextAtSelection(UnsafeNativeMethods.InsertAtSelectionFlags flags, char[] text, int cch, out int startIndex, out int endIndex, out UnsafeNativeMethods.TS_TEXTCHANGE change)
		{
			startIndex = -1;
			endIndex = -1;
			change.start = 0;
			change.oldEnd = 0;
			change.newEnd = 0;
			if (this.IsReadOnly)
			{
				throw new COMException(SR.Get("TextStore_TS_E_READONLY"), -2147220983);
			}
			ITextRange textRange = new TextRange(this.TextSelection.AnchorPosition, this.TextSelection.MovingPosition);
			textRange.ApplyTypingHeuristics(false);
			ITextPointer textPointer;
			ITextPointer textPointer2;
			TextStore.GetAdjustedSelection(textRange.Start, textRange.End, out textPointer, out textPointer2);
			ITextPointer textPointer3 = textPointer.CreatePointer();
			textPointer3.SetLogicalDirection(LogicalDirection.Backward);
			ITextPointer textPointer4 = textPointer2.CreatePointer();
			textPointer4.SetLogicalDirection(LogicalDirection.Forward);
			int charOffset = textPointer3.CharOffset;
			int charOffset2 = textPointer4.CharOffset;
			if ((flags & UnsafeNativeMethods.InsertAtSelectionFlags.TS_IAS_QUERYONLY) == (UnsafeNativeMethods.InsertAtSelectionFlags)0)
			{
				TextStore.CompositionParentUndoUnit textParentUndoUnit = this.OpenCompositionUndoUnit();
				UndoCloseAction undoCloseAction = UndoCloseAction.Rollback;
				try
				{
					this.VerifyTextStoreConsistency();
					change.oldEnd = charOffset2;
					string text2 = this.FilterCompositionString(new string(text), textRange.Start.GetOffsetToPosition(textRange.End));
					if (text2 == null)
					{
						throw new COMException(SR.Get("TextStore_CompositionRejected"), -2147467259);
					}
					this.TextSelection.ApplyTypingHeuristics(false);
					if (textPointer.CompareTo(this.TextSelection.Start) != 0 || textPointer2.CompareTo(this.TextSelection.End) != 0)
					{
						this.TextSelection.Select(textPointer, textPointer2);
					}
					if (!this._isComposing && this._previousCompositionStartOffset == -1)
					{
						this._previousCompositionStartOffset = this.TextSelection.Start.Offset;
						this._previousCompositionEndOffset = this.TextSelection.End.Offset;
					}
					this.TextEditor.SetSelectedText(text2, InputLanguageManager.Current.CurrentInputLanguage);
					change.start = textPointer3.CharOffset;
					change.newEnd = textPointer4.CharOffset;
					this.ValidateChange(change);
					this.VerifyTextStoreConsistency();
					undoCloseAction = UndoCloseAction.Commit;
				}
				finally
				{
					this.CloseTextParentUndoUnit(textParentUndoUnit, undoCloseAction);
				}
			}
			if ((flags & UnsafeNativeMethods.InsertAtSelectionFlags.TS_IAS_NOQUERY) == (UnsafeNativeMethods.InsertAtSelectionFlags)0)
			{
				startIndex = charOffset;
				endIndex = textPointer4.CharOffset;
			}
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x001188FC File Offset: 0x00116AFC
		public void InsertEmbeddedAtSelection(UnsafeNativeMethods.InsertAtSelectionFlags flags, object obj, out int startIndex, out int endIndex, out UnsafeNativeMethods.TS_TEXTCHANGE change)
		{
			startIndex = -1;
			endIndex = -1;
			change.start = 0;
			change.oldEnd = 0;
			change.newEnd = 0;
			if (this.IsReadOnly)
			{
				throw new COMException(SR.Get("TextStore_TS_E_READONLY"), -2147220983);
			}
			throw new COMException(SR.Get("TextStore_TS_E_FORMAT"), -2147220982);
		}

		// Token: 0x06003CA7 RID: 15527 RVA: 0x0011895C File Offset: 0x00116B5C
		public int RequestSupportedAttrs(UnsafeNativeMethods.AttributeFlags flags, int count, Guid[] filterAttributes)
		{
			this.PrepareAttributes((InputScope)this.UiScope.GetValue(InputMethod.InputScopeProperty), (double)this.UiScope.GetValue(TextElement.FontSizeProperty), (FontFamily)this.UiScope.GetValue(TextElement.FontFamilyProperty), (XmlLanguage)this.UiScope.GetValue(FrameworkContentElement.LanguageProperty), this.UiScope, count, filterAttributes);
			if (this._preparedattributes.Count == 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x001189DC File Offset: 0x00116BDC
		public int RequestAttrsAtPosition(int index, int count, Guid[] filterAttributes, UnsafeNativeMethods.AttributeFlags flags)
		{
			ITextPointer textPointer = this.CreatePointerAtCharOffset(index, LogicalDirection.Forward);
			this.PrepareAttributes((InputScope)textPointer.GetValue(InputMethod.InputScopeProperty), (double)textPointer.GetValue(TextElement.FontSizeProperty), (FontFamily)textPointer.GetValue(TextElement.FontFamilyProperty), (XmlLanguage)textPointer.GetValue(FrameworkContentElement.LanguageProperty), null, count, filterAttributes);
			if (this._preparedattributes.Count == 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x00118A4B File Offset: 0x00116C4B
		public void RequestAttrsTransitioningAtPosition(int position, int count, Guid[] filterAttributes, UnsafeNativeMethods.AttributeFlags flags)
		{
			throw new COMException(SR.Get("TextStore_E_NOTIMPL"), -2147467263);
		}

		// Token: 0x06003CAA RID: 15530 RVA: 0x00118A61 File Offset: 0x00116C61
		public void FindNextAttrTransition(int startIndex, int haltIndex, int count, Guid[] filterAttributes, UnsafeNativeMethods.AttributeFlags flags, out int acpNext, out bool found, out int foundOffset)
		{
			acpNext = 0;
			found = false;
			foundOffset = 0;
		}

		// Token: 0x06003CAB RID: 15531 RVA: 0x00118A70 File Offset: 0x00116C70
		public void RetrieveRequestedAttrs(int count, UnsafeNativeMethods.TS_ATTRVAL[] attributeVals, out int fetched)
		{
			fetched = 0;
			int num = 0;
			while (num < count && num < this._preparedattributes.Count)
			{
				attributeVals[num] = (UnsafeNativeMethods.TS_ATTRVAL)this._preparedattributes[num];
				fetched++;
				num++;
			}
			this._preparedattributes.Clear();
			this._preparedattributes = null;
		}

		// Token: 0x06003CAC RID: 15532 RVA: 0x00118ACA File Offset: 0x00116CCA
		public void GetEnd(out int end)
		{
			end = this.TextContainer.IMECharCount;
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x00118AD9 File Offset: 0x00116CD9
		public void GetActiveView(out int viewCookie)
		{
			viewCookie = 0;
		}

		// Token: 0x06003CAE RID: 15534 RVA: 0x00118AE0 File Offset: 0x00116CE0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void GetACPFromPoint(int viewCookie, ref UnsafeNativeMethods.POINT tsfPoint, UnsafeNativeMethods.GetPositionFromPointFlags flags, out int positionCP)
		{
			SecurityHelper.DemandUnmanagedCode();
			NativeMethods.POINT point = new NativeMethods.POINT(tsfPoint.x, tsfPoint.y);
			PresentationSource presentationSource;
			IWin32Window win32Window;
			ITextView textView;
			this.GetVisualInfo(out presentationSource, out win32Window, out textView);
			CompositionTarget compositionTarget = presentationSource.CompositionTarget;
			SafeNativeMethods.ScreenToClient(new HandleRef(null, win32Window.Handle), point);
			Point point2 = new Point((double)point.x, (double)point.y);
			point2 = compositionTarget.TransformFromDevice.Transform(point2);
			GeneralTransform generalTransform = compositionTarget.RootVisual.TransformToDescendant(this.RenderScope);
			if (generalTransform != null)
			{
				generalTransform.TryTransform(point2, out point2);
			}
			if (!textView.Validate(point2))
			{
				throw new COMException(SR.Get("TextStore_TS_E_NOLAYOUT"), -2147220986);
			}
			ITextPointer textPositionFromPoint = textView.GetTextPositionFromPoint(point2, (flags & UnsafeNativeMethods.GetPositionFromPointFlags.GXFPF_NEAREST) > (UnsafeNativeMethods.GetPositionFromPointFlags)0);
			if (textPositionFromPoint == null)
			{
				throw new COMException(SR.Get("TextStore_TS_E_INVALIDPOINT"), -2147220985);
			}
			positionCP = textPositionFromPoint.CharOffset;
			if ((flags & UnsafeNativeMethods.GetPositionFromPointFlags.GXFPF_ROUND_NEAREST) == (UnsafeNativeMethods.GetPositionFromPointFlags)0)
			{
				ITextPointer position = textPositionFromPoint.CreatePointer(LogicalDirection.Backward);
				ITextPointer textPointer = textPositionFromPoint.CreatePointer(LogicalDirection.Forward);
				textPointer.MoveToNextInsertionPosition(LogicalDirection.Backward);
				Rect rectangleFromTextPosition = textView.GetRectangleFromTextPosition(position);
				Rect rectangleFromTextPosition2 = textView.GetRectangleFromTextPosition(textPointer);
				Point point3 = new Point(Math.Min(rectangleFromTextPosition2.Left, rectangleFromTextPosition.Left), Math.Min(rectangleFromTextPosition2.Top, rectangleFromTextPosition.Top));
				Point point4 = new Point(Math.Max(rectangleFromTextPosition2.Left, rectangleFromTextPosition.Left), Math.Max(rectangleFromTextPosition2.Bottom, rectangleFromTextPosition.Bottom));
				Rect rect = new Rect(point3, point4);
				if (rect.Contains(point2))
				{
					positionCP--;
				}
			}
		}

		// Token: 0x06003CAF RID: 15535 RVA: 0x00118C7C File Offset: 0x00116E7C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		void UnsafeNativeMethods.ITextStoreACP.GetTextExt(int viewCookie, int startIndex, int endIndex, out UnsafeNativeMethods.RECT rect, out bool clipped)
		{
			this._isInUpdateLayout = true;
			this.UiScope.UpdateLayout();
			this._isInUpdateLayout = false;
			if (this._hasTextChangedInUpdateLayout)
			{
				this._netCharCount = this.TextContainer.IMECharCount;
				throw new COMException(SR.Get("TextStore_TS_E_NOLAYOUT"), -2147220986);
			}
			rect = default(UnsafeNativeMethods.RECT);
			clipped = false;
			PresentationSource presentationSource;
			IWin32Window win32Window;
			ITextView textView;
			this.GetVisualInfo(out presentationSource, out win32Window, out textView);
			CompositionTarget compositionTarget = presentationSource.CompositionTarget;
			ITextPointer textPointer = this.CreatePointerAtCharOffset(startIndex, LogicalDirection.Forward);
			textPointer.MoveToInsertionPosition(LogicalDirection.Forward);
			if (!this.TextView.IsValid)
			{
				throw new COMException(SR.Get("TextStore_TS_E_NOLAYOUT"), -2147220986);
			}
			Point topLeft;
			Point bottomRight;
			if (startIndex == endIndex)
			{
				Rect characterRect = textPointer.GetCharacterRect(LogicalDirection.Forward);
				topLeft = characterRect.TopLeft;
				bottomRight = characterRect.BottomRight;
			}
			else
			{
				Rect rect2 = new Rect(Size.Empty);
				ITextPointer textPointer2 = textPointer.CreatePointer();
				ITextPointer textPointer3 = this.CreatePointerAtCharOffset(endIndex, LogicalDirection.Backward);
				textPointer3.MoveToInsertionPosition(LogicalDirection.Backward);
				ITextPointer textPointer4;
				bool flag;
				do
				{
					TextSegment lineRange = this.TextView.GetLineRange(textPointer2);
					Rect rect3;
					if (!lineRange.IsNull)
					{
						ITextPointer start = (lineRange.Start.CompareTo(textPointer) <= 0) ? textPointer : lineRange.Start;
						textPointer4 = ((lineRange.End.CompareTo(textPointer3) >= 0) ? textPointer3 : lineRange.End);
						rect3 = TextStore.GetLineBounds(start, textPointer4);
						flag = (textPointer2.MoveToLineBoundary(1) != 0);
					}
					else
					{
						rect3 = textPointer2.GetCharacterRect(LogicalDirection.Forward);
						flag = textPointer2.MoveToNextInsertionPosition(LogicalDirection.Forward);
						textPointer4 = textPointer2;
					}
					if (!rect3.IsEmpty)
					{
						rect2.Union(rect3);
					}
				}
				while (textPointer4.CompareTo(textPointer3) != 0 && flag);
				topLeft = rect2.TopLeft;
				bottomRight = rect2.BottomRight;
			}
			GeneralTransform generalTransform = this.UiScope.TransformToAncestor(compositionTarget.RootVisual);
			generalTransform.TryTransform(topLeft, out topLeft);
			generalTransform.TryTransform(bottomRight, out bottomRight);
			rect = TextStore.TransformRootRectToScreenCoordinates(topLeft, bottomRight, win32Window, presentationSource);
		}

		// Token: 0x06003CB0 RID: 15536 RVA: 0x00118E70 File Offset: 0x00117070
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void GetScreenExt(int viewCookie, out UnsafeNativeMethods.RECT rect)
		{
			Rect visualContentBounds = this.UiScope.VisualContentBounds;
			Rect visualDescendantBounds = this.UiScope.VisualDescendantBounds;
			visualContentBounds.Union(visualDescendantBounds);
			PresentationSource presentationSource;
			IWin32Window win32Window;
			ITextView textView;
			this.GetVisualInfo(out presentationSource, out win32Window, out textView);
			CompositionTarget compositionTarget = presentationSource.CompositionTarget;
			Point point = new Point(visualContentBounds.Left, visualContentBounds.Top);
			Point point2 = new Point(visualContentBounds.Right, visualContentBounds.Bottom);
			GeneralTransform generalTransform = this.UiScope.TransformToAncestor(compositionTarget.RootVisual);
			generalTransform.TryTransform(point, out point);
			generalTransform.TryTransform(point2, out point2);
			rect = TextStore.TransformRootRectToScreenCoordinates(point, point2, win32Window, presentationSource);
		}

		// Token: 0x06003CB1 RID: 15537 RVA: 0x00118F19 File Offset: 0x00117119
		[SecurityCritical]
		void UnsafeNativeMethods.ITextStoreACP.GetWnd(int viewCookie, out IntPtr hwnd)
		{
			hwnd = IntPtr.Zero;
			hwnd = this.CriticalSourceWnd;
		}

		// Token: 0x06003CB2 RID: 15538 RVA: 0x00118F2A File Offset: 0x0011712A
		[SecurityCritical]
		void UnsafeNativeMethods.ITfThreadFocusSink.OnSetThreadFocus()
		{
			if (!this.IsTextEditorValid)
			{
				return;
			}
			if (Keyboard.FocusedElement == this.UiScope)
			{
				this.OnGotFocus();
			}
		}

		// Token: 0x06003CB3 RID: 15539 RVA: 0x00002137 File Offset: 0x00000337
		public void OnKillThreadFocus()
		{
		}

		// Token: 0x06003CB4 RID: 15540 RVA: 0x00118F48 File Offset: 0x00117148
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void OnStartComposition(UnsafeNativeMethods.ITfCompositionView view, out bool ok)
		{
			if (this._isComposing)
			{
				ok = false;
				return;
			}
			ITextPointer textPointer;
			ITextPointer textPointer2;
			this.GetCompositionPositions(view, out textPointer, out textPointer2);
			int startOffsetBefore = textPointer.Offset;
			int endOffsetBefore = textPointer2.Offset;
			this._lastCompositionText = TextRangeBase.GetTextInternal(textPointer, textPointer2);
			if (this._previousCompositionStartOffset != -1)
			{
				startOffsetBefore = this._previousCompositionStartOffset;
				endOffsetBefore = this._previousCompositionEndOffset;
			}
			else if (this.TextEditor.AcceptsRichContent && textPointer.CompareTo(textPointer2) != 0)
			{
				TextElement element = (TextElement)((TextPointer)textPointer).Parent;
				TextElement element2 = (TextElement)((TextPointer)textPointer2).Parent;
				TextElement commonAncestor = TextElement.GetCommonAncestor(element, element2);
				int imecharCount = this.TextContainer.IMECharCount;
				TextRange textRange = new TextRange(textPointer, textPointer2);
				string text = textRange.Text;
				if (commonAncestor is Run)
				{
					this.TextEditor.MarkCultureProperty(textRange, InputLanguageManager.Current.CurrentInputLanguage);
				}
				else if (commonAncestor is Paragraph || commonAncestor is Span)
				{
					this.TextEditor.SetText(textRange, text, InputLanguageManager.Current.CurrentInputLanguage);
				}
				Invariant.Assert(textRange.Text == text);
				Invariant.Assert(imecharCount == this.TextContainer.IMECharCount);
			}
			this.CompositionEventList.Add(new TextStore.CompositionEventRecord(TextStore.CompositionStage.StartComposition, startOffsetBefore, endOffsetBefore, this._lastCompositionText));
			this._previousCompositionStartOffset = textPointer.Offset;
			this._previousCompositionEndOffset = textPointer2.Offset;
			this._isComposing = true;
			this.BreakTypingSequence(textPointer2);
			ok = true;
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x001190C4 File Offset: 0x001172C4
		[SecurityCritical]
		public void OnUpdateComposition(UnsafeNativeMethods.ITfCompositionView view, UnsafeNativeMethods.ITfRange rangeNew)
		{
			this.TextEditor.CloseToolTip();
			Invariant.Assert(this._isComposing);
			Invariant.Assert(this._previousCompositionStartOffset != -1);
			ITextPointer textPointer;
			ITextPointer textPointer2;
			this.GetCompositionPositions(view, out textPointer, out textPointer2);
			ITextPointer textPointer3 = null;
			ITextPointer textPointer4 = null;
			bool flag = false;
			if (rangeNew != null)
			{
				this.TextPositionsFromITfRange(rangeNew, out textPointer3, out textPointer4);
				flag = (textPointer3.Offset != textPointer.Offset || textPointer4.Offset != textPointer2.Offset);
			}
			string textInternal = TextRangeBase.GetTextInternal(textPointer, textPointer2);
			if (flag)
			{
				TextStore.CompositionEventRecord item = new TextStore.CompositionEventRecord(TextStore.CompositionStage.UpdateComposition, this._previousCompositionStartOffset, this._previousCompositionEndOffset, textInternal, true);
				this.CompositionEventList.Add(item);
				this._previousCompositionStartOffset = textPointer3.Offset;
				this._previousCompositionEndOffset = textPointer4.Offset;
				this._lastCompositionText = null;
			}
			else
			{
				TextStore.CompositionEventRecord item2 = new TextStore.CompositionEventRecord(TextStore.CompositionStage.UpdateComposition, this._previousCompositionStartOffset, this._previousCompositionEndOffset, textInternal);
				object obj = (this.CompositionEventList.Count == 0) ? null : this.CompositionEventList[this.CompositionEventList.Count - 1];
				if (this._lastCompositionText == null || string.CompareOrdinal(textInternal, this._lastCompositionText) != 0)
				{
					this.CompositionEventList.Add(item2);
				}
				this._previousCompositionStartOffset = textPointer.Offset;
				this._previousCompositionEndOffset = textPointer2.Offset;
				this._lastCompositionText = textInternal;
			}
			this.BreakTypingSequence(textPointer2);
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x0011921C File Offset: 0x0011741C
		[SecurityCritical]
		public void OnEndComposition(UnsafeNativeMethods.ITfCompositionView view)
		{
			Invariant.Assert(this._isComposing);
			Invariant.Assert(this._previousCompositionStartOffset != -1);
			ITextPointer textPointer;
			ITextPointer textPointer2;
			this.GetCompositionPositions(view, out textPointer, out textPointer2);
			if (this._compositionEventState == TextStore.CompositionEventState.NotRaisingEvents)
			{
				this.CompositionEventList.Add(new TextStore.CompositionEventRecord(TextStore.CompositionStage.EndComposition, textPointer.Offset, textPointer2.Offset, TextRangeBase.GetTextInternal(textPointer, textPointer2)));
				TextStore.CompositionParentUndoUnit compositionParentUndoUnit = this.PeekCompositionParentUndoUnit();
				if (compositionParentUndoUnit != null)
				{
					compositionParentUndoUnit.IsLastCompositionUnit = true;
				}
			}
			this._nextUndoUnitIsFirstCompositionUnit = true;
			this._isComposing = false;
			this._previousCompositionStartOffset = -1;
			this._previousCompositionEndOffset = -1;
			if (this._interimSelection)
			{
				this._interimSelection = false;
				this.TextSelection.OnInterimSelectionChanged(this._interimSelection);
			}
			this.BreakTypingSequence(textPointer2);
		}

		// Token: 0x06003CB7 RID: 15543 RVA: 0x001192D0 File Offset: 0x001174D0
		[SecurityCritical]
		void UnsafeNativeMethods.ITfTextEditSink.OnEndEdit(UnsafeNativeMethods.ITfContext context, int ecReadOnly, UnsafeNativeMethods.ITfEditRecord editRecord)
		{
			this._textservicesproperty.OnEndEdit(context, ecReadOnly, editRecord);
			Marshal.ReleaseComObject(editRecord);
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x001192E8 File Offset: 0x001174E8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void OnTransitoryExtensionUpdated(UnsafeNativeMethods.ITfContext context, int ecReadOnly, UnsafeNativeMethods.ITfRange rangeResult, UnsafeNativeMethods.ITfRange rangeComposition, out bool fDeleteResultRange)
		{
			fDeleteResultRange = true;
			if (rangeResult != null)
			{
				string text = TextStore.StringFromITfRange(rangeResult, ecReadOnly);
				if (text.Length > 0)
				{
					if (this.TextEditor.AllowOvertype && this.TextEditor._OvertypeMode && this.TextSelection.IsEmpty)
					{
						ITextPointer textPointer = this.TextSelection.End.CreatePointer();
						textPointer.MoveToInsertionPosition(LogicalDirection.Forward);
						if (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
						{
							char[] array = new char[2];
							textPointer.GetTextInRun(LogicalDirection.Forward, array, 0, array.Length);
							if (array[0] != Environment.NewLine[0] || array[1] != Environment.NewLine[1])
							{
								int length = text.Length;
								while (length-- > 0)
								{
									this.TextSelection.ExtendToNextInsertionPosition(LogicalDirection.Forward);
								}
							}
						}
					}
					string text2 = this.FilterCompositionString(text, this.TextSelection.Start.GetOffsetToPosition(this.TextSelection.End));
					if (text2 == null)
					{
						throw new COMException(SR.Get("TextStore_CompositionRejected"), -2147467259);
					}
					this.TextEditor.SetText(this.TextSelection, text2, InputLanguageManager.Current.CurrentInputLanguage);
					this.TextSelection.Select(this.TextSelection.End, this.TextSelection.End);
				}
			}
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x00119430 File Offset: 0x00117630
		[SecuritySafeCritical]
		public int AdviceMouseSink(UnsafeNativeMethods.ITfRangeACP range, UnsafeNativeMethods.ITfMouseSink sink, out int dwCookie)
		{
			if (this._mouseSinks == null)
			{
				this._mouseSinks = new ArrayList(1);
			}
			this._mouseSinks.Sort();
			dwCookie = 0;
			while (dwCookie < this._mouseSinks.Count && ((TextStore.MouseSink)this._mouseSinks[dwCookie]).Cookie == dwCookie)
			{
				dwCookie++;
			}
			Invariant.Assert(dwCookie != -1);
			this._mouseSinks.Add(new TextStore.MouseSink(range, sink, dwCookie));
			if (this._mouseSinks.Count == 1)
			{
				this.UiScope.PreviewMouseLeftButtonDown += this.OnMouseButtonEvent;
				this.UiScope.PreviewMouseLeftButtonUp += this.OnMouseButtonEvent;
				this.UiScope.PreviewMouseRightButtonDown += this.OnMouseButtonEvent;
				this.UiScope.PreviewMouseRightButtonUp += this.OnMouseButtonEvent;
				this.UiScope.PreviewMouseMove += this.OnMouseEvent;
			}
			return 0;
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x00119538 File Offset: 0x00117738
		public int UnadviceMouseSink(int dwCookie)
		{
			int result = -2147024809;
			for (int i = 0; i < this._mouseSinks.Count; i++)
			{
				TextStore.MouseSink mouseSink = (TextStore.MouseSink)this._mouseSinks[i];
				if (mouseSink.Cookie == dwCookie)
				{
					this._mouseSinks.RemoveAt(i);
					if (this._mouseSinks.Count == 0)
					{
						this.UiScope.PreviewMouseLeftButtonDown -= this.OnMouseButtonEvent;
						this.UiScope.PreviewMouseLeftButtonUp -= this.OnMouseButtonEvent;
						this.UiScope.PreviewMouseRightButtonDown -= this.OnMouseButtonEvent;
						this.UiScope.PreviewMouseRightButtonUp -= this.OnMouseButtonEvent;
						this.UiScope.PreviewMouseMove -= this.OnMouseEvent;
					}
					if (mouseSink.Locked)
					{
						mouseSink.PendingDispose = true;
					}
					else
					{
						mouseSink.Dispose();
					}
					result = 0;
					break;
				}
			}
			return result;
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x00119630 File Offset: 0x00117830
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnAttach()
		{
			this._netCharCount = this.TextContainer.IMECharCount;
			this._textservicesHost = TextServicesHost.Current;
			this._textservicesHost.RegisterTextStore(this);
			this.TextContainer.Change += this.OnTextContainerChange;
			this._textservicesproperty = new TextServicesProperty(this);
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x00119688 File Offset: 0x00117888
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnDetach(bool finalizer)
		{
			if (this.IsTextEditorValid)
			{
				this.TextContainer.Change -= this.OnTextContainerChange;
			}
			this._textservicesHost.UnregisterTextStore(this, finalizer);
			this._textservicesproperty = null;
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x001196C0 File Offset: 0x001178C0
		[SecurityCritical]
		internal void OnGotFocus()
		{
			if ((bool)this.UiScope.GetValue(InputMethod.IsInputMethodEnabledProperty))
			{
				this._textservicesHost.ThreadManager.SetFocus(this.DocumentManager);
			}
			if (this._makeLayoutChangeOnGotFocus)
			{
				this.OnLayoutUpdated();
				this._makeLayoutChangeOnGotFocus = false;
			}
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x0011970F File Offset: 0x0011790F
		internal void OnLostFocus()
		{
			this.CompleteComposition();
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x00119717 File Offset: 0x00117917
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnLayoutUpdated()
		{
			if (this.HasSink)
			{
				this._sink.OnLayoutChange(UnsafeNativeMethods.TsLayoutCode.TS_LC_CHANGE, 0);
			}
			if (this._textservicesproperty != null)
			{
				this._textservicesproperty.OnLayoutUpdated();
			}
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x00119741 File Offset: 0x00117941
		internal void OnSelectionChange()
		{
			this._compositionModifiedByEventListener = true;
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x0011974A File Offset: 0x0011794A
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnSelectionChanged()
		{
			if (this._compositionEventState == TextStore.CompositionEventState.RaisingEvents)
			{
				return;
			}
			if (this._ignoreNextSelectionChange)
			{
				this._ignoreNextSelectionChange = false;
				return;
			}
			if (this.HasSink)
			{
				this._sink.OnSelectionChange();
			}
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x0011977C File Offset: 0x0011797C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal bool QueryRangeOrReconvertSelection(bool fDoReconvert)
		{
			if (this._isComposing && !fDoReconvert)
			{
				ITextPointer textPointer;
				ITextPointer textPointer2;
				this.GetCompositionPositions(out textPointer, out textPointer2);
				if (textPointer != null && textPointer2 != null && textPointer.CompareTo(this.TextSelection.Start) <= 0 && textPointer.CompareTo(this.TextSelection.End) <= 0 && textPointer2.CompareTo(this.TextSelection.Start) >= 0 && textPointer2.CompareTo(this.TextSelection.End) >= 0)
				{
					return true;
				}
			}
			UnsafeNativeMethods.ITfFnReconversion tfFnReconversion;
			UnsafeNativeMethods.ITfRange tfRange;
			bool fnReconv = this.GetFnReconv(this.TextSelection.Start, this.TextSelection.End, out tfFnReconversion, out tfRange);
			if (tfFnReconversion != null)
			{
				if (fDoReconvert)
				{
					tfFnReconversion.Reconvert(tfRange);
				}
				Marshal.ReleaseComObject(tfFnReconversion);
			}
			if (tfRange != null)
			{
				Marshal.ReleaseComObject(tfRange);
			}
			return fnReconv;
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x0011983C File Offset: 0x00117A3C
		[SecurityCritical]
		internal UnsafeNativeMethods.ITfCandidateList GetReconversionCandidateList()
		{
			UnsafeNativeMethods.ITfCandidateList result = null;
			UnsafeNativeMethods.ITfFnReconversion tfFnReconversion;
			UnsafeNativeMethods.ITfRange tfRange;
			bool fnReconv = this.GetFnReconv(this.TextSelection.Start, this.TextSelection.End, out tfFnReconversion, out tfRange);
			if (tfFnReconversion != null)
			{
				tfFnReconversion.GetReconversion(tfRange, out result);
				Marshal.ReleaseComObject(tfFnReconversion);
			}
			if (tfRange != null)
			{
				Marshal.ReleaseComObject(tfRange);
			}
			return result;
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x00119890 File Offset: 0x00117A90
		[SecurityCritical]
		private bool GetFnReconv(ITextPointer textStart, ITextPointer textEnd, out UnsafeNativeMethods.ITfFnReconversion funcReconv, out UnsafeNativeMethods.ITfRange rangeNew)
		{
			bool flag = false;
			funcReconv = null;
			rangeNew = null;
			UnsafeNativeMethods.ITfContext tfContext;
			this.DocumentManager.GetBase(out tfContext);
			UnsafeNativeMethods.ITfRange tfRange;
			tfContext.GetStart(this.EditCookie, out tfRange);
			UnsafeNativeMethods.ITfRangeACP tfRangeACP = tfRange as UnsafeNativeMethods.ITfRangeACP;
			int charOffset = textStart.CharOffset;
			int charOffset2 = textEnd.CharOffset;
			tfRangeACP.SetExtent(charOffset, charOffset2 - charOffset);
			Guid guid_SYSTEM_FUNCTIONPROVIDER = UnsafeNativeMethods.GUID_SYSTEM_FUNCTIONPROVIDER;
			Guid guid_Null = UnsafeNativeMethods.Guid_Null;
			Guid iid_ITfFnReconversion = UnsafeNativeMethods.IID_ITfFnReconversion;
			UnsafeNativeMethods.ITfFunctionProvider tfFunctionProvider;
			this._textservicesHost.ThreadManager.GetFunctionProvider(ref guid_SYSTEM_FUNCTIONPROVIDER, out tfFunctionProvider);
			object obj;
			tfFunctionProvider.GetFunction(ref guid_Null, ref iid_ITfFnReconversion, out obj);
			funcReconv = (obj as UnsafeNativeMethods.ITfFnReconversion);
			funcReconv.QueryRange(tfRange, out rangeNew, out flag);
			Marshal.ReleaseComObject(tfFunctionProvider);
			if (!flag)
			{
				Marshal.ReleaseComObject(funcReconv);
				funcReconv = null;
			}
			Marshal.ReleaseComObject(tfRange);
			Marshal.ReleaseComObject(tfContext);
			return flag;
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x00119958 File Offset: 0x00117B58
		internal void CompleteCompositionAsync()
		{
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.CompleteCompositionHandler), null);
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x00119974 File Offset: 0x00117B74
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void CompleteComposition()
		{
			if (this._isComposing)
			{
				FrameworkTextComposition.CompleteCurrentComposition(this.DocumentManager);
			}
			this._previousCompositionStartOffset = -1;
			this._previousCompositionEndOffset = -1;
			this._previousCompositionStart = null;
			this._previousCompositionEnd = null;
		}

		// Token: 0x06003CC7 RID: 15559 RVA: 0x001199A8 File Offset: 0x00117BA8
		internal ITextPointer CreatePointerAtCharOffset(int charOffset, LogicalDirection direction)
		{
			this.ValidateCharOffset(charOffset);
			ITextPointer textPointer = this.TextContainer.CreatePointerAtCharOffset(charOffset, direction);
			if (textPointer == null)
			{
				textPointer = this.TextSelection.Start.CreatePointer(direction);
			}
			return textPointer;
		}

		// Token: 0x06003CC8 RID: 15560 RVA: 0x001199E0 File Offset: 0x00117BE0
		internal void MakeLayoutChangeOnGotFocus()
		{
			if (this._isComposing)
			{
				this._makeLayoutChangeOnGotFocus = true;
			}
		}

		// Token: 0x06003CC9 RID: 15561 RVA: 0x001199F4 File Offset: 0x00117BF4
		internal void UpdateCompositionText(FrameworkTextComposition composition)
		{
			if (this._compositionModifiedByEventListener)
			{
				return;
			}
			this._handledByTextStoreListener = true;
			bool compositionModifiedByEventListener = false;
			ITextRange textRange;
			string text;
			if (composition._ResultStart != null)
			{
				textRange = new TextRange(composition._ResultStart, composition._ResultEnd, true);
				text = this.TextEditor._FilterText(composition.Text, textRange);
				if (text.Length != composition.Text.Length)
				{
					compositionModifiedByEventListener = true;
				}
			}
			else
			{
				textRange = new TextRange(composition._CompositionStart, composition._CompositionEnd, true);
				text = this.TextEditor._FilterText(composition.CompositionText, textRange, false);
				Invariant.Assert(text.Length == composition.CompositionText.Length);
			}
			this._nextUndoUnitIsFirstCompositionUnit = false;
			TextStore.CompositionParentUndoUnit compositionParentUndoUnit = this.PeekCompositionParentUndoUnit();
			if (compositionParentUndoUnit != null)
			{
				compositionParentUndoUnit.IsLastCompositionUnit = false;
			}
			TextStore.CompositionParentUndoUnit compositionParentUndoUnit2 = this.OpenCompositionUndoUnit(textRange.Start, textRange.End);
			UndoCloseAction undoCloseAction = UndoCloseAction.Rollback;
			if (composition._ResultStart != null)
			{
				this._nextUndoUnitIsFirstCompositionUnit = true;
				compositionParentUndoUnit2.IsLastCompositionUnit = true;
			}
			this.TextSelection.BeginChange();
			try
			{
				this.TextEditor.SetText(textRange, text, InputLanguageManager.Current.CurrentInputLanguage);
				if (this._interimSelection)
				{
					this.TextSelection.Select(textRange.Start, textRange.End);
				}
				else
				{
					this.TextSelection.SetCaretToPosition(textRange.End, LogicalDirection.Backward, true, true);
				}
				compositionParentUndoUnit2.RecordRedoSelectionState(textRange.End, textRange.End);
				undoCloseAction = UndoCloseAction.Commit;
			}
			finally
			{
				this._compositionModifiedByEventListener = compositionModifiedByEventListener;
				this.TextSelection.EndChange();
				this.CloseTextParentUndoUnit(compositionParentUndoUnit2, undoCloseAction);
			}
		}

		// Token: 0x06003CCA RID: 15562 RVA: 0x00119B7C File Offset: 0x00117D7C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static FrameworkTextComposition CreateComposition(TextEditor editor, object owner)
		{
			FrameworkTextComposition result;
			if (editor.AcceptsRichContent)
			{
				result = new FrameworkRichTextComposition(InputManager.UnsecureCurrent, editor.UiScope, owner);
			}
			else
			{
				result = new FrameworkTextComposition(InputManager.Current, editor.UiScope, owner);
			}
			return result;
		}

		// Token: 0x17000F00 RID: 3840
		// (get) Token: 0x06003CCB RID: 15563 RVA: 0x00119BB8 File Offset: 0x00117DB8
		internal UIElement RenderScope
		{
			get
			{
				if (this.TextEditor == null)
				{
					return null;
				}
				if (this.TextEditor.TextView == null)
				{
					return null;
				}
				return this.TextEditor.TextView.RenderScope;
			}
		}

		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x06003CCC RID: 15564 RVA: 0x00119BE3 File Offset: 0x00117DE3
		internal FrameworkElement UiScope
		{
			get
			{
				return this.TextEditor.UiScope;
			}
		}

		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x06003CCD RID: 15565 RVA: 0x00119BF0 File Offset: 0x00117DF0
		internal ITextContainer TextContainer
		{
			get
			{
				return this.TextEditor.TextContainer;
			}
		}

		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x06003CCE RID: 15566 RVA: 0x00119BFD File Offset: 0x00117DFD
		internal ITextView TextView
		{
			get
			{
				return this.TextEditor.TextView;
			}
		}

		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x06003CCF RID: 15567 RVA: 0x00119C0A File Offset: 0x00117E0A
		// (set) Token: 0x06003CD0 RID: 15568 RVA: 0x00119C21 File Offset: 0x00117E21
		internal UnsafeNativeMethods.ITfDocumentMgr DocumentManager
		{
			[SecurityCritical]
			get
			{
				if (this._documentmanager == null)
				{
					return null;
				}
				return this._documentmanager.Value;
			}
			[SecurityCritical]
			set
			{
				this._documentmanager = new SecurityCriticalDataClass<UnsafeNativeMethods.ITfDocumentMgr>(value);
			}
		}

		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x06003CD1 RID: 15569 RVA: 0x00119C2F File Offset: 0x00117E2F
		// (set) Token: 0x06003CD2 RID: 15570 RVA: 0x00119C37 File Offset: 0x00117E37
		internal int ThreadFocusCookie
		{
			get
			{
				return this._threadFocusCookie;
			}
			set
			{
				this._threadFocusCookie = value;
			}
		}

		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x06003CD3 RID: 15571 RVA: 0x00119C40 File Offset: 0x00117E40
		// (set) Token: 0x06003CD4 RID: 15572 RVA: 0x00119C48 File Offset: 0x00117E48
		internal int EditSinkCookie
		{
			get
			{
				return this._editSinkCookie;
			}
			set
			{
				this._editSinkCookie = value;
			}
		}

		// Token: 0x17000F07 RID: 3847
		// (get) Token: 0x06003CD5 RID: 15573 RVA: 0x00119C51 File Offset: 0x00117E51
		// (set) Token: 0x06003CD6 RID: 15574 RVA: 0x00119C59 File Offset: 0x00117E59
		internal int EditCookie
		{
			get
			{
				return this._editCookie;
			}
			set
			{
				this._editCookie = value;
			}
		}

		// Token: 0x17000F08 RID: 3848
		// (get) Token: 0x06003CD7 RID: 15575 RVA: 0x00119C62 File Offset: 0x00117E62
		internal bool IsInterimSelection
		{
			get
			{
				return this._interimSelection;
			}
		}

		// Token: 0x17000F09 RID: 3849
		// (get) Token: 0x06003CD8 RID: 15576 RVA: 0x00119C6A File Offset: 0x00117E6A
		internal bool IsComposing
		{
			get
			{
				return this._isComposing;
			}
		}

		// Token: 0x17000F0A RID: 3850
		// (get) Token: 0x06003CD9 RID: 15577 RVA: 0x00119C72 File Offset: 0x00117E72
		internal bool IsEffectivelyComposing
		{
			get
			{
				return this._isComposing && !this._isEffectivelyNotComposing;
			}
		}

		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x06003CDA RID: 15578 RVA: 0x00119C87 File Offset: 0x00117E87
		// (set) Token: 0x06003CDB RID: 15579 RVA: 0x00119C8F File Offset: 0x00117E8F
		internal int TransitoryExtensionSinkCookie
		{
			get
			{
				return this._transitoryExtensionSinkCookie;
			}
			set
			{
				this._transitoryExtensionSinkCookie = value;
			}
		}

		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x06003CDC RID: 15580 RVA: 0x00119C98 File Offset: 0x00117E98
		internal IntPtr CriticalSourceWnd
		{
			[SecurityCritical]
			get
			{
				bool callerIsTrusted = true;
				return this.GetSourceWnd(callerIsTrusted);
			}
		}

		// Token: 0x06003CDD RID: 15581 RVA: 0x00119CB0 File Offset: 0x00117EB0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnTextContainerChange(object sender, TextContainerChangeEventArgs args)
		{
			if (args.IMECharCount > 0 && (args.TextChange == TextChangeType.ContentAdded || args.TextChange == TextChangeType.ContentRemoved))
			{
				this._compositionModifiedByEventListener = true;
			}
			if (this._compositionEventState == TextStore.CompositionEventState.RaisingEvents)
			{
				return;
			}
			Invariant.Assert(sender == this.TextContainer);
			if (this._lockFlags == (UnsafeNativeMethods.LockFlags)0 && this.HasSink)
			{
				int num = 0;
				int num2 = 0;
				if (args.TextChange == TextChangeType.ContentAdded)
				{
					num = args.IMECharCount;
				}
				else if (args.TextChange == TextChangeType.ContentRemoved)
				{
					num2 = args.IMECharCount;
				}
				if (num <= 0 && num2 <= 0)
				{
					return;
				}
				UnsafeNativeMethods.TS_TEXTCHANGE ts_TEXTCHANGE;
				ts_TEXTCHANGE.start = args.ITextPosition.CharOffset;
				ts_TEXTCHANGE.oldEnd = ts_TEXTCHANGE.start + num2;
				ts_TEXTCHANGE.newEnd = ts_TEXTCHANGE.start + num;
				this.ValidateChange(ts_TEXTCHANGE);
				try
				{
					this._textChangeReentrencyCount++;
					this._sink.OnTextChange((UnsafeNativeMethods.OnTextChangeFlags)0, ref ts_TEXTCHANGE);
					return;
				}
				finally
				{
					this._textChangeReentrencyCount--;
				}
			}
			if (this._isInUpdateLayout)
			{
				this._hasTextChangedInUpdateLayout = true;
			}
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x00119DBC File Offset: 0x00117FBC
		private object GrantLockHandler(object o)
		{
			if (this._textservicesHost != null && this.HasSink)
			{
				this.GrantLockWorker(this._pendingAsyncLockFlags);
			}
			this._pendingAsyncLockFlags = (UnsafeNativeMethods.LockFlags)0;
			return null;
		}

		// Token: 0x17000F0D RID: 3853
		// (get) Token: 0x06003CDF RID: 15583 RVA: 0x00119DE3 File Offset: 0x00117FE3
		private bool HasSink
		{
			[SecuritySafeCritical]
			get
			{
				return this._sink != null;
			}
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x00119DF0 File Offset: 0x00117FF0
		private int GrantLockWorker(UnsafeNativeMethods.LockFlags flags)
		{
			TextEditor textEditor = this.TextEditor;
			int result;
			if (textEditor == null)
			{
				result = -2147467259;
			}
			else
			{
				this._lockFlags = flags;
				this._hasTextChangedInUpdateLayout = false;
				UndoManager undoManager = UndoManager.GetUndoManager(textEditor.TextContainer.Parent);
				int previousUndoCount = 0;
				bool isImeSupportModeEnabled = false;
				if (undoManager != null)
				{
					previousUndoCount = undoManager.UndoCount;
					isImeSupportModeEnabled = undoManager.IsImeSupportModeEnabled;
					undoManager.IsImeSupportModeEnabled = true;
				}
				this._previousCompositionStartOffset = ((this._previousCompositionStart == null) ? -1 : this._previousCompositionStart.Offset);
				this._previousCompositionEndOffset = ((this._previousCompositionEnd == null) ? -1 : this._previousCompositionEnd.Offset);
				try
				{
					textEditor.Selection.BeginChangeNoUndo();
					try
					{
						result = this.GrantLock();
						if (this._pendingWriteReq)
						{
							this._lockFlags = UnsafeNativeMethods.LockFlags.TS_LF_READWRITE;
							this.GrantLock();
						}
					}
					finally
					{
						this._pendingWriteReq = false;
						this._lockFlags = (UnsafeNativeMethods.LockFlags)0;
						this._ignoreNextSelectionChange = textEditor.Selection._IsChanged;
						try
						{
							textEditor.Selection.EndChange(false, true);
						}
						finally
						{
							this._ignoreNextSelectionChange = false;
							this._previousCompositionStart = ((this._previousCompositionStartOffset == -1) ? null : textEditor.TextContainer.CreatePointerAtOffset(this._previousCompositionStartOffset, LogicalDirection.Backward));
							this._previousCompositionEnd = ((this._previousCompositionEndOffset == -1) ? null : textEditor.TextContainer.CreatePointerAtOffset(this._previousCompositionEndOffset, LogicalDirection.Forward));
						}
					}
					if (undoManager != null)
					{
						this.HandleCompositionEvents(previousUndoCount);
					}
				}
				finally
				{
					if (undoManager != null)
					{
						undoManager.IsImeSupportModeEnabled = isImeSupportModeEnabled;
					}
					this._previousCompositionStart = ((this._previousCompositionStartOffset == -1) ? null : textEditor.TextContainer.CreatePointerAtOffset(this._previousCompositionStartOffset, LogicalDirection.Backward));
					this._previousCompositionEnd = ((this._previousCompositionEndOffset == -1) ? null : textEditor.TextContainer.CreatePointerAtOffset(this._previousCompositionEndOffset, LogicalDirection.Forward));
				}
			}
			return result;
		}

		// Token: 0x06003CE1 RID: 15585 RVA: 0x00119FBC File Offset: 0x001181BC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private int GrantLock()
		{
			Invariant.Assert(Thread.CurrentThread == this._textservicesHost.Dispatcher.Thread, "GrantLock called on bad thread!");
			this.VerifyTextStoreConsistency();
			int result = this._sink.OnLockGranted(this._lockFlags);
			this.VerifyTextStoreConsistency();
			return result;
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x0011A00C File Offset: 0x0011820C
		private static bool WalkTextRun(ITextPointer navigator, ITextPointer limit, char[] text, int cchReq, ref int charsCopied, UnsafeNativeMethods.TS_RUNINFO[] runInfo, int cRunInfoReq, ref int cRunInfoRcv)
		{
			Invariant.Assert(navigator.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text);
			Invariant.Assert(limit == null || navigator.CompareTo(limit) <= 0);
			bool result = false;
			int num;
			if (cchReq > 0)
			{
				num = TextPointerBase.GetTextWithLimit(navigator, LogicalDirection.Forward, text, charsCopied, Math.Min(cchReq, text.Length - charsCopied), limit);
				navigator.MoveByOffset(num);
				charsCopied += num;
				result = (text.Length == charsCopied || (limit != null && navigator.CompareTo(limit) == 0));
			}
			else
			{
				num = navigator.GetTextRunLength(LogicalDirection.Forward);
				navigator.MoveToNextContextPosition(LogicalDirection.Forward);
				if (limit != null && navigator.CompareTo(limit) >= 0)
				{
					int offsetToPosition = limit.GetOffsetToPosition(navigator);
					Invariant.Assert(offsetToPosition >= 0 && offsetToPosition <= num, "Bogus offset -- extends past run!");
					num -= offsetToPosition;
					navigator.MoveToPosition(limit);
					result = true;
				}
			}
			if (cRunInfoReq > 0 && num > 0)
			{
				if (cRunInfoRcv > 0 && runInfo[cRunInfoRcv - 1].type == UnsafeNativeMethods.TsRunType.TS_RT_PLAIN)
				{
					int num2 = cRunInfoRcv - 1;
					runInfo[num2].count = runInfo[num2].count + num;
				}
				else
				{
					runInfo[cRunInfoRcv].count = num;
					runInfo[cRunInfoRcv].type = UnsafeNativeMethods.TsRunType.TS_RT_PLAIN;
					cRunInfoRcv++;
				}
			}
			return result;
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x0011A13C File Offset: 0x0011833C
		private static bool WalkObjectRun(ITextPointer navigator, ITextPointer limit, char[] text, int cchReq, ref int charsCopied, UnsafeNativeMethods.TS_RUNINFO[] runInfo, int cRunInfoReq, ref int cRunInfoRcv)
		{
			Invariant.Assert(navigator.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.EmbeddedElement);
			Invariant.Assert(limit == null || navigator.CompareTo(limit) <= 0);
			if (limit != null && navigator.CompareTo(limit) == 0)
			{
				return true;
			}
			bool result = false;
			navigator.MoveToNextContextPosition(LogicalDirection.Forward);
			if (cchReq >= 1)
			{
				text[charsCopied] = '￼';
				charsCopied++;
			}
			if (cRunInfoReq > 0)
			{
				if (cRunInfoRcv > 0 && runInfo[cRunInfoRcv - 1].type == UnsafeNativeMethods.TsRunType.TS_RT_PLAIN)
				{
					int num = cRunInfoRcv - 1;
					runInfo[num].count = runInfo[num].count + 1;
				}
				else
				{
					runInfo[cRunInfoRcv].count = 1;
					runInfo[cRunInfoRcv].type = UnsafeNativeMethods.TsRunType.TS_RT_PLAIN;
					cRunInfoRcv++;
				}
			}
			return result;
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x0011A200 File Offset: 0x00118400
		private static bool WalkRegionBoundary(ITextPointer navigator, ITextPointer limit, char[] text, int cchReq, ref int charsCopied, UnsafeNativeMethods.TS_RUNINFO[] runInfo, int cRunInfoReq, ref int cRunInfoRcv)
		{
			Invariant.Assert(navigator.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart || navigator.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd);
			Invariant.Assert(limit == null || navigator.CompareTo(limit) <= 0);
			if (limit != null && navigator.CompareTo(limit) >= 0)
			{
				return true;
			}
			bool result = false;
			if (cchReq > 0)
			{
				char c = (navigator.GetAdjacentElement(LogicalDirection.Forward) is TableCell) ? '\0' : '\n';
				text[charsCopied] = c;
				navigator.MoveByOffset(1);
				charsCopied++;
				result = (text.Length == charsCopied || (limit != null && navigator.CompareTo(limit) == 0));
			}
			else
			{
				navigator.MoveByOffset(1);
			}
			if (cRunInfoReq > 0)
			{
				if (cRunInfoRcv > 0 && runInfo[cRunInfoRcv - 1].type == UnsafeNativeMethods.TsRunType.TS_RT_PLAIN)
				{
					int num = cRunInfoRcv - 1;
					runInfo[num].count = runInfo[num].count + 1;
				}
				else
				{
					runInfo[cRunInfoRcv].count = 1;
					runInfo[cRunInfoRcv].type = UnsafeNativeMethods.TsRunType.TS_RT_PLAIN;
					cRunInfoRcv++;
				}
			}
			return result;
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x0011A305 File Offset: 0x00118505
		[SecurityCritical]
		private void GetVisualInfo(out PresentationSource source, out IWin32Window win32Window, out ITextView view)
		{
			source = PresentationSource.CriticalFromVisual(this.RenderScope);
			win32Window = (source as IWin32Window);
			if (win32Window == null)
			{
				throw new COMException(SR.Get("TextStore_TS_E_NOLAYOUT"), -2147220986);
			}
			view = this.TextView;
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x0011A340 File Offset: 0x00118540
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static UnsafeNativeMethods.RECT TransformRootRectToScreenCoordinates(Point milPointTopLeft, Point milPointBottomRight, IWin32Window win32Window, PresentationSource source)
		{
			UnsafeNativeMethods.RECT result = default(UnsafeNativeMethods.RECT);
			CompositionTarget compositionTarget = source.CompositionTarget;
			milPointTopLeft = compositionTarget.TransformToDevice.Transform(milPointTopLeft);
			milPointBottomRight = compositionTarget.TransformToDevice.Transform(milPointBottomRight);
			IntPtr handle = IntPtr.Zero;
			new UIPermission(UIPermissionWindow.AllWindows).Assert();
			try
			{
				handle = win32Window.Handle;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			NativeMethods.POINT point = new NativeMethods.POINT();
			UnsafeNativeMethods.ClientToScreen(new HandleRef(null, handle), point);
			result.left = (int)((double)point.x + milPointTopLeft.X);
			result.right = (int)((double)point.x + milPointBottomRight.X);
			result.top = (int)((double)point.y + milPointTopLeft.Y);
			result.bottom = (int)((double)point.y + milPointBottomRight.Y);
			return result;
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x0011A420 File Offset: 0x00118620
		private static string GetFontFamilyName(FontFamily fontFamily, XmlLanguage language)
		{
			if (fontFamily == null)
			{
				return null;
			}
			if (fontFamily.Source != null)
			{
				return fontFamily.Source;
			}
			LanguageSpecificStringDictionary familyNames = fontFamily.FamilyNames;
			if (familyNames == null)
			{
				return null;
			}
			foreach (XmlLanguage key in language.MatchingLanguages)
			{
				string text = familyNames[key];
				if (text != null)
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x0011A4A8 File Offset: 0x001186A8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void PrepareAttributes(InputScope inputScope, double fontSize, FontFamily fontFamily, XmlLanguage language, Visual visual, int count, Guid[] filterAttributes)
		{
			if (this._preparedattributes == null)
			{
				this._preparedattributes = new ArrayList(count);
			}
			else
			{
				this._preparedattributes.Clear();
			}
			int i = 0;
			while (i < TextStore._supportingattributes.Length)
			{
				if (count == 0)
				{
					goto IL_67;
				}
				bool flag = false;
				for (int j = 0; j < count; j++)
				{
					if (TextStore._supportingattributes[i].Guid.Equals(filterAttributes[j]))
					{
						flag = true;
					}
				}
				if (flag)
				{
					goto IL_67;
				}
				IL_386:
				i++;
				continue;
				IL_67:
				UnsafeNativeMethods.TS_ATTRVAL ts_ATTRVAL = new UnsafeNativeMethods.TS_ATTRVAL
				{
					attributeId = TextStore._supportingattributes[i].Guid,
					overlappedId = (int)TextStore._supportingattributes[i].Style,
					val = new NativeMethods.VARIANT()
				};
				ts_ATTRVAL.val.SuppressFinalize();
				switch (TextStore._supportingattributes[i].Style)
				{
				case TextStore.AttributeStyle.InputScope:
				{
					object o = new InputScopeAttribute(inputScope);
					ts_ATTRVAL.val.vt = 13;
					ts_ATTRVAL.val.data1.Value = Marshal.GetIUnknownForObject(o);
					break;
				}
				case TextStore.AttributeStyle.Font_Style_Height:
					ts_ATTRVAL.val.vt = 3;
					ts_ATTRVAL.val.data1.Value = (IntPtr)((int)fontSize);
					break;
				case TextStore.AttributeStyle.Font_FaceName:
				{
					string fontFamilyName = TextStore.GetFontFamilyName(fontFamily, language);
					if (fontFamilyName != null)
					{
						ts_ATTRVAL.val.vt = 8;
						ts_ATTRVAL.val.data1.Value = Marshal.StringToBSTR(fontFamilyName);
					}
					break;
				}
				case TextStore.AttributeStyle.Font_SizePts:
					ts_ATTRVAL.val.vt = 3;
					ts_ATTRVAL.val.data1.Value = (IntPtr)((int)(fontSize / 96.0 * 72.0));
					break;
				case TextStore.AttributeStyle.Text_ReadOnly:
					ts_ATTRVAL.val.vt = 11;
					ts_ATTRVAL.val.data1.Value = (this.IsReadOnly ? ((IntPtr)1) : ((IntPtr)0));
					break;
				case TextStore.AttributeStyle.Text_Orientation:
				{
					ts_ATTRVAL.val.vt = 3;
					ts_ATTRVAL.val.data1.Value = (IntPtr)0;
					PresentationSource presentationSource = PresentationSource.CriticalFromVisual(this.RenderScope);
					if (presentationSource != null)
					{
						Visual rootVisual = presentationSource.RootVisual;
						if (rootVisual != null && visual != null)
						{
							GeneralTransform generalTransform = visual.TransformToAncestor(rootVisual);
							Transform affineTransform = generalTransform.AffineTransform;
							if (affineTransform != null)
							{
								Matrix value = affineTransform.Value;
								if (value.M11 != 0.0 || value.M12 != 0.0)
								{
									double num = Math.Asin(value.M12 / Math.Sqrt(value.M11 * value.M11 + value.M12 * value.M12));
									double num2 = Math.Acos(value.M11 / Math.Sqrt(value.M11 * value.M11 + value.M12 * value.M12));
									double num3 = Math.Round(num2 * 180.0 / 3.141592653589793, 0);
									double num4;
									if (num <= 0.0)
									{
										num4 = num3;
									}
									else
									{
										num4 = 360.0 - num3;
									}
									ts_ATTRVAL.val.data1.Value = (IntPtr)((int)num4 * 10);
								}
							}
						}
					}
					break;
				}
				case TextStore.AttributeStyle.Text_VerticalWriting:
					ts_ATTRVAL.val.vt = 11;
					ts_ATTRVAL.val.data1.Value = (IntPtr)0;
					break;
				}
				this._preparedattributes.Add(ts_ATTRVAL);
				goto IL_386;
			}
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x0011A84C File Offset: 0x00118A4C
		[SecurityCritical]
		private void TextPositionsFromITfRange(UnsafeNativeMethods.ITfRange range, out ITextPointer start, out ITextPointer end)
		{
			UnsafeNativeMethods.ITfRangeACP tfRangeACP = range as UnsafeNativeMethods.ITfRangeACP;
			int num;
			int num2;
			tfRangeACP.GetExtent(out num, out num2);
			start = this.CreatePointerAtCharOffset(num, LogicalDirection.Backward);
			end = this.CreatePointerAtCharOffset(num + num2, LogicalDirection.Forward);
			while (start.CompareTo(end) < 0 && start.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
			{
				start.MoveToNextContextPosition(LogicalDirection.Forward);
			}
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x0011A8A4 File Offset: 0x00118AA4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void GetCompositionPositions(out ITextPointer start, out ITextPointer end)
		{
			start = null;
			end = null;
			if (this._isComposing)
			{
				UnsafeNativeMethods.ITfCompositionView currentCompositionView = FrameworkTextComposition.GetCurrentCompositionView(this.DocumentManager);
				if (currentCompositionView != null)
				{
					this.GetCompositionPositions(currentCompositionView, out start, out end);
				}
			}
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x0011A8D8 File Offset: 0x00118AD8
		[SecurityCritical]
		private void GetCompositionPositions(UnsafeNativeMethods.ITfCompositionView view, out ITextPointer start, out ITextPointer end)
		{
			UnsafeNativeMethods.ITfRange tfRange;
			view.GetRange(out tfRange);
			this.TextPositionsFromITfRange(tfRange, out start, out end);
			Marshal.ReleaseComObject(tfRange);
			Marshal.ReleaseComObject(view);
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x0011A904 File Offset: 0x00118B04
		[SecurityCritical]
		private static string StringFromITfRange(UnsafeNativeMethods.ITfRange range, int ecReadOnly)
		{
			UnsafeNativeMethods.ITfRangeACP tfRangeACP = (UnsafeNativeMethods.ITfRangeACP)range;
			int num;
			int num2;
			tfRangeACP.GetExtent(out num, out num2);
			char[] array = new char[num2];
			int num3;
			tfRangeACP.GetText(ecReadOnly, 0, array, num2, out num3);
			return new string(array);
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x0011A93E File Offset: 0x00118B3E
		private void OnMouseButtonEvent(object sender, MouseButtonEventArgs e)
		{
			e.Handled = this.InternalMouseEventHandler();
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x0011A93E File Offset: 0x00118B3E
		private void OnMouseEvent(object sender, MouseEventArgs e)
		{
			e.Handled = this.InternalMouseEventHandler();
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x0011A94C File Offset: 0x00118B4C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool InternalMouseEventHandler()
		{
			int num = 0;
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				num |= 1;
			}
			if (Mouse.RightButton == MouseButtonState.Pressed)
			{
				num |= 2;
			}
			if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
			{
				num |= 4;
			}
			if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
			{
				num |= 8;
			}
			Point position = Mouse.GetPosition(this.RenderScope);
			ITextView textView = this.TextView;
			if (textView == null)
			{
				return false;
			}
			if (!textView.Validate(position))
			{
				return false;
			}
			ITextPointer textPositionFromPoint = textView.GetTextPositionFromPoint(position, false);
			if (textPositionFromPoint == null)
			{
				return false;
			}
			Rect rectangleFromTextPosition = textView.GetRectangleFromTextPosition(textPositionFromPoint);
			ITextPointer textPointer = textPositionFromPoint.CreatePointer();
			if (textPointer == null)
			{
				return false;
			}
			if (position.X - rectangleFromTextPosition.Left >= 0.0)
			{
				textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward);
			}
			else
			{
				textPointer.MoveToNextInsertionPosition(LogicalDirection.Backward);
			}
			Rect rectangleFromTextPosition2 = textView.GetRectangleFromTextPosition(textPointer);
			int charOffset = textPositionFromPoint.CharOffset;
			int num2;
			if (position.X - rectangleFromTextPosition.Left >= 0.0)
			{
				if ((position.X - rectangleFromTextPosition.Left) * 4.0 / (rectangleFromTextPosition2.Left - rectangleFromTextPosition.Left) <= 1.0)
				{
					num2 = 2;
				}
				else
				{
					num2 = 3;
				}
			}
			else if ((position.X - rectangleFromTextPosition2.Left) * 4.0 / (rectangleFromTextPosition.Left - rectangleFromTextPosition2.Left) <= 3.0)
			{
				num2 = 0;
			}
			else
			{
				num2 = 1;
			}
			bool flag = false;
			int num3 = 0;
			while (num3 < this._mouseSinks.Count && !flag)
			{
				TextStore.MouseSink mouseSink = (TextStore.MouseSink)this._mouseSinks[num3];
				int num4;
				int num5;
				mouseSink.Range.GetExtent(out num4, out num5);
				if (charOffset >= num4 && charOffset <= num4 + num5 && (charOffset != num4 || num2 > 1) && (charOffset != num4 + num5 || num2 < 2))
				{
					mouseSink.Locked = true;
					try
					{
						mouseSink.Sink.OnMouseEvent(charOffset - num4, num2, num, out flag);
					}
					finally
					{
						mouseSink.Locked = false;
					}
				}
				num3++;
			}
			return flag;
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x0011AB58 File Offset: 0x00118D58
		private TextStore.CompositionParentUndoUnit OpenCompositionUndoUnit()
		{
			return this.OpenCompositionUndoUnit(null, null);
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x0011AB64 File Offset: 0x00118D64
		private TextStore.CompositionParentUndoUnit OpenCompositionUndoUnit(ITextPointer compositionStart, ITextPointer compositionEnd)
		{
			UndoManager undoManager = UndoManager.GetUndoManager(this.TextContainer.Parent);
			if (undoManager == null || !undoManager.IsEnabled)
			{
				return null;
			}
			if (compositionStart == null)
			{
				Invariant.Assert(compositionEnd == null);
				this.GetCompositionPositions(out compositionStart, out compositionEnd);
			}
			ITextPointer textPointer;
			if (compositionStart != null && compositionStart.CompareTo(this.TextSelection.Start) > 0)
			{
				textPointer = compositionStart;
			}
			else
			{
				textPointer = this.TextSelection.Start;
			}
			TextStore.CompositionParentUndoUnit compositionParentUndoUnit = new TextStore.CompositionParentUndoUnit(this.TextSelection, textPointer, textPointer, this._nextUndoUnitIsFirstCompositionUnit);
			this._nextUndoUnitIsFirstCompositionUnit = false;
			undoManager.Open(compositionParentUndoUnit);
			return compositionParentUndoUnit;
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x0011ABF0 File Offset: 0x00118DF0
		private static Rect GetLineBounds(ITextPointer start, ITextPointer end)
		{
			if (!start.HasValidLayout || !end.HasValidLayout)
			{
				return Rect.Empty;
			}
			Rect characterRect = start.GetCharacterRect(LogicalDirection.Forward);
			characterRect.Union(end.GetCharacterRect(LogicalDirection.Backward));
			ITextPointer textPointer = start.CreatePointer(LogicalDirection.Forward);
			while (textPointer.MoveToNextContextPosition(LogicalDirection.Forward) && textPointer.CompareTo(end) < 0)
			{
				TextPointerContext pointerContext = textPointer.GetPointerContext(LogicalDirection.Backward);
				switch (pointerContext)
				{
				case TextPointerContext.Text:
					break;
				case TextPointerContext.EmbeddedElement:
				case TextPointerContext.ElementEnd:
					characterRect.Union(textPointer.GetCharacterRect(LogicalDirection.Backward));
					break;
				case TextPointerContext.ElementStart:
					characterRect.Union(textPointer.GetCharacterRect(LogicalDirection.Backward));
					textPointer.MoveToElementEdge(ElementEdge.AfterEnd);
					break;
				default:
					Invariant.Assert(pointerContext > TextPointerContext.None);
					break;
				}
			}
			return characterRect;
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x0011AC9C File Offset: 0x00118E9C
		private string FilterCompositionString(string text, int charsToReplaceCount)
		{
			string text2 = this.TextEditor._FilterText(text, charsToReplaceCount, false);
			if (text2.Length != text.Length)
			{
				this.CompleteCompositionAsync();
				return null;
			}
			return text2;
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x0011ACCF File Offset: 0x00118ECF
		private object CompleteCompositionHandler(object o)
		{
			this.CompleteComposition();
			return null;
		}

		// Token: 0x06003CF5 RID: 15605 RVA: 0x0011ACD8 File Offset: 0x00118ED8
		[SecurityCritical]
		private IntPtr GetSourceWnd(bool callerIsTrusted)
		{
			IntPtr result = IntPtr.Zero;
			if (this.RenderScope != null)
			{
				IWin32Window win32Window;
				if (callerIsTrusted)
				{
					win32Window = (PresentationSource.CriticalFromVisual(this.RenderScope) as IWin32Window);
				}
				else
				{
					win32Window = (PresentationSource.FromVisual(this.RenderScope) as IWin32Window);
				}
				if (win32Window != null)
				{
					new UIPermission(UIPermissionWindow.AllWindows).Assert();
					try
					{
						result = win32Window.Handle;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			return result;
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x0011AD48 File Offset: 0x00118F48
		private void ValidateChange(UnsafeNativeMethods.TS_TEXTCHANGE change)
		{
			Invariant.Assert(change.start >= 0, "Bad StartIndex");
			Invariant.Assert(change.start <= change.oldEnd, "Bad oldEnd index");
			Invariant.Assert(change.start <= change.newEnd, "Bad newEnd index");
			this._netCharCount += change.newEnd - change.oldEnd;
			Invariant.Assert(this._netCharCount >= 0, "Negative _netCharCount!");
		}

		// Token: 0x06003CF7 RID: 15607 RVA: 0x0011ADD1 File Offset: 0x00118FD1
		private void VerifyTextStoreConsistency()
		{
			if (this._netCharCount != this.TextContainer.IMECharCount)
			{
				Invariant.Assert(false, "TextContainer/TextStore have inconsistent char counts!");
			}
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x0011ADF4 File Offset: 0x00118FF4
		private void ValidateCharOffset(int offset)
		{
			if (offset < 0 || offset > this.TextContainer.IMECharCount)
			{
				throw new ArgumentException(SR.Get("TextStore_BadIMECharOffset", new object[]
				{
					offset,
					this.TextContainer.IMECharCount
				}));
			}
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x0011AE48 File Offset: 0x00119048
		private void BreakTypingSequence(ITextPointer caretPosition)
		{
			TextStore.CompositionParentUndoUnit compositionParentUndoUnit = this.PeekCompositionParentUndoUnit();
			if (compositionParentUndoUnit != null)
			{
				compositionParentUndoUnit.RecordRedoSelectionState(caretPosition, caretPosition);
			}
		}

		// Token: 0x06003CFA RID: 15610 RVA: 0x0011AE68 File Offset: 0x00119068
		private static void GetAdjustedSelection(ITextPointer startIn, ITextPointer endIn, out ITextPointer startOut, out ITextPointer endOut)
		{
			startOut = startIn;
			endOut = endIn;
			TextPointer textPointer = startOut as TextPointer;
			if (textPointer == null)
			{
				return;
			}
			TextPointer position = (TextPointer)endOut;
			if (startIn.CompareTo(endIn) != 0)
			{
				bool flag = TextPointerBase.IsInBlockUIContainer(textPointer) || TextPointerBase.IsInBlockUIContainer(position);
				TableCell tableCellFromPosition = TextRangeEditTables.GetTableCellFromPosition(textPointer);
				TableCell tableCellFromPosition2 = TextRangeEditTables.GetTableCellFromPosition(position);
				bool flag2 = tableCellFromPosition != null && tableCellFromPosition == tableCellFromPosition2;
				bool flag3 = TextRangeEditTables.GetTableFromPosition(textPointer) != null || TextRangeEditTables.GetTableFromPosition(position) != null;
				if (!flag && (flag2 || !flag3))
				{
					return;
				}
			}
			if (textPointer.IsAtRowEnd)
			{
				TextPointer nextInsertionPosition = textPointer.GetNextInsertionPosition(LogicalDirection.Backward);
				Table tableFromPosition = TextRangeEditTables.GetTableFromPosition(textPointer);
				textPointer = TextRangeEditTables.GetAdjustedRowEndPosition(tableFromPosition, textPointer);
				if (!textPointer.IsAtInsertionPosition)
				{
					textPointer = nextInsertionPosition;
				}
			}
			else if (TextPointerBase.IsInBlockUIContainer(textPointer))
			{
				if (textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
				{
					textPointer = textPointer.GetNextInsertionPosition(LogicalDirection.Backward);
				}
				else
				{
					textPointer = textPointer.GetNextInsertionPosition(LogicalDirection.Forward);
				}
			}
			while (textPointer != null && TextPointerBase.IsBeforeFirstTable(textPointer))
			{
				textPointer = textPointer.GetNextInsertionPosition(LogicalDirection.Forward);
			}
			if (textPointer == null || textPointer.IsAtRowEnd || TextPointerBase.IsInBlockUIContainer(textPointer))
			{
				throw new COMException(SR.Get("TextStore_CompositionRejected"), -2147467259);
			}
			startOut = textPointer;
			endOut = textPointer;
		}

		// Token: 0x06003CFB RID: 15611 RVA: 0x0011AF80 File Offset: 0x00119180
		private void GetNormalizedRange(int startCharOffset, int endCharOffset, out ITextPointer start, out ITextPointer end)
		{
			start = this.CreatePointerAtCharOffset(startCharOffset, LogicalDirection.Forward);
			end = ((startCharOffset == endCharOffset) ? start : this.CreatePointerAtCharOffset(endCharOffset, LogicalDirection.Backward));
			while (start.CompareTo(end) < 0)
			{
				TextPointerContext pointerContext = start.GetPointerContext(LogicalDirection.Forward);
				if (pointerContext == TextPointerContext.ElementStart)
				{
					TextElement textElement = start.GetAdjacentElement(LogicalDirection.Forward) as TextElement;
					if (textElement == null)
					{
						break;
					}
					if (textElement.IMELeftEdgeCharCount != 0)
					{
						break;
					}
				}
				else if (pointerContext != TextPointerContext.ElementEnd)
				{
					break;
				}
				start.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			if (start.CompareTo(end) == 0)
			{
				start = start.GetFormatNormalizedPosition(LogicalDirection.Backward);
				end = start;
				return;
			}
			start = start.GetFormatNormalizedPosition(LogicalDirection.Backward);
			end = end.GetFormatNormalizedPosition(LogicalDirection.Backward);
		}

		// Token: 0x06003CFC RID: 15612 RVA: 0x0011B020 File Offset: 0x00119220
		private void HandleCompositionEvents(int previousUndoCount)
		{
			if (this.CompositionEventList.Count == 0 || this._compositionEventState != TextStore.CompositionEventState.NotRaisingEvents)
			{
				return;
			}
			this._compositionEventState = TextStore.CompositionEventState.RaisingEvents;
			try
			{
				int offset = this.TextSelection.AnchorPosition.Offset;
				int offset2 = this.TextSelection.MovingPosition.Offset;
				UndoManager undoManager = UndoManager.GetUndoManager(this.TextContainer.Parent);
				undoManager.SetRedoStack(null);
				this.UndoQuietly(undoManager.UndoCount - previousUndoCount);
				Stack imeChangeStack = undoManager.SetRedoStack(null);
				int undoCount = undoManager.UndoCount;
				int appSelectionAnchorOffset;
				int appSelectionMovingOffset;
				this.RaiseCompositionEvents(out appSelectionAnchorOffset, out appSelectionMovingOffset);
				this.SetFinalDocumentState(undoManager, imeChangeStack, undoManager.UndoCount - undoCount, offset, offset2, appSelectionAnchorOffset, appSelectionMovingOffset);
			}
			finally
			{
				this.CompositionEventList.Clear();
				this._compositionEventState = TextStore.CompositionEventState.NotRaisingEvents;
			}
		}

		// Token: 0x06003CFD RID: 15613 RVA: 0x0011B0EC File Offset: 0x001192EC
		private TextParentUndoUnit OpenTextParentUndoUnit()
		{
			UndoManager undoManager = UndoManager.GetUndoManager(this.TextContainer.Parent);
			TextParentUndoUnit textParentUndoUnit = new TextParentUndoUnit(this.TextSelection, this.TextSelection.Start, this.TextSelection.Start);
			undoManager.Open(textParentUndoUnit);
			return textParentUndoUnit;
		}

		// Token: 0x06003CFE RID: 15614 RVA: 0x0011B134 File Offset: 0x00119334
		private void CloseTextParentUndoUnit(TextParentUndoUnit textParentUndoUnit, UndoCloseAction undoCloseAction)
		{
			if (textParentUndoUnit != null)
			{
				UndoManager undoManager = UndoManager.GetUndoManager(this.TextContainer.Parent);
				undoManager.Close(textParentUndoUnit, undoCloseAction);
			}
		}

		// Token: 0x06003CFF RID: 15615 RVA: 0x0011B160 File Offset: 0x00119360
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void RaiseCompositionEvents(out int appSelectionAnchorOffset, out int appSelectionMovingOffset)
		{
			appSelectionAnchorOffset = -1;
			appSelectionMovingOffset = -1;
			UndoManager undoManager = UndoManager.GetUndoManager(this.TextContainer.Parent);
			int i = 0;
			while (i < this.CompositionEventList.Count)
			{
				TextStore.CompositionEventRecord compositionEventRecord = this.CompositionEventList[i];
				FrameworkTextComposition frameworkTextComposition = TextStore.CreateComposition(this.TextEditor, this);
				ITextPointer start = this.TextContainer.CreatePointerAtOffset(compositionEventRecord.StartOffsetBefore, LogicalDirection.Backward);
				ITextPointer textPointer = this.TextContainer.CreatePointerAtOffset(compositionEventRecord.EndOffsetBefore, LogicalDirection.Forward);
				bool flag = false;
				this._handledByTextStoreListener = false;
				this._compositionModifiedByEventListener = false;
				switch (compositionEventRecord.Stage)
				{
				case TextStore.CompositionStage.StartComposition:
					frameworkTextComposition.Stage = TextCompositionStage.None;
					frameworkTextComposition.SetCompositionPositions(start, textPointer, compositionEventRecord.Text);
					undoManager.MinUndoStackCount = undoManager.UndoCount;
					try
					{
						flag = TextCompositionManager.StartComposition(frameworkTextComposition);
						break;
					}
					finally
					{
						undoManager.MinUndoStackCount = 0;
					}
					goto IL_CB;
				case TextStore.CompositionStage.UpdateComposition:
					goto IL_CB;
				case TextStore.CompositionStage.EndComposition:
					goto IL_14D;
				default:
					goto IL_190;
				}
				IL_19B:
				if ((compositionEventRecord.Stage == TextStore.CompositionStage.EndComposition && !this._handledByTextStoreListener) || (compositionEventRecord.Stage != TextStore.CompositionStage.EndComposition && flag) || frameworkTextComposition.PendingComplete)
				{
					this._compositionModifiedByEventListener = true;
				}
				if (this._compositionModifiedByEventListener)
				{
					appSelectionAnchorOffset = this.TextSelection.AnchorPosition.Offset;
					appSelectionMovingOffset = this.TextSelection.MovingPosition.Offset;
					return;
				}
				if (compositionEventRecord.Stage != TextStore.CompositionStage.EndComposition && !compositionEventRecord.IsShiftUpdate)
				{
					this.UpdateCompositionText(frameworkTextComposition);
				}
				if (compositionEventRecord.Stage == TextStore.CompositionStage.EndComposition)
				{
					start = textPointer.GetFrozenPointer(LogicalDirection.Backward);
				}
				if (this._compositionModifiedByEventListener)
				{
					appSelectionAnchorOffset = this.TextSelection.AnchorPosition.Offset;
					appSelectionMovingOffset = this.TextSelection.MovingPosition.Offset;
					return;
				}
				i++;
				continue;
				IL_CB:
				frameworkTextComposition.Stage = TextCompositionStage.Started;
				frameworkTextComposition.SetCompositionPositions(start, textPointer, compositionEventRecord.Text);
				undoManager.MinUndoStackCount = undoManager.UndoCount;
				try
				{
					if (this.IsCompositionRecordShifted(compositionEventRecord) && this.IsMaxLengthExceeded(frameworkTextComposition.CompositionText, compositionEventRecord.EndOffsetBefore - compositionEventRecord.StartOffsetBefore))
					{
						frameworkTextComposition.SetResultPositions(start, textPointer, compositionEventRecord.Text);
						flag = TextCompositionManager.CompleteComposition(frameworkTextComposition);
						this._compositionModifiedByEventListener = true;
						goto IL_19B;
					}
					if (!compositionEventRecord.IsShiftUpdate)
					{
						flag = TextCompositionManager.UpdateComposition(frameworkTextComposition);
					}
					goto IL_19B;
				}
				finally
				{
					undoManager.MinUndoStackCount = 0;
				}
				IL_14D:
				frameworkTextComposition.Stage = TextCompositionStage.Started;
				frameworkTextComposition.SetResultPositions(start, textPointer, compositionEventRecord.Text);
				undoManager.MinUndoStackCount = undoManager.UndoCount;
				try
				{
					this._isEffectivelyNotComposing = true;
					flag = TextCompositionManager.CompleteComposition(frameworkTextComposition);
					goto IL_19B;
				}
				finally
				{
					undoManager.MinUndoStackCount = 0;
					this._isEffectivelyNotComposing = false;
				}
				IL_190:
				Invariant.Assert(false, "Invalid composition stage!");
				goto IL_19B;
			}
		}

		// Token: 0x06003D00 RID: 15616 RVA: 0x0011B3FC File Offset: 0x001195FC
		private bool IsMaxLengthExceeded(string textData, int charsToReplaceCount)
		{
			if (!this.TextEditor.AcceptsRichContent && this.TextEditor.MaxLength > 0)
			{
				ITextContainer textContainer = this.TextContainer;
				int num = textContainer.SymbolCount - charsToReplaceCount;
				int num2 = Math.Max(0, this.TextEditor.MaxLength - num);
				if (textData.Length > num2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003D01 RID: 15617 RVA: 0x0011B454 File Offset: 0x00119654
		private bool IsCompositionRecordShifted(TextStore.CompositionEventRecord record)
		{
			return (0 <= record.StartOffsetBefore && 0 <= this._previousCompositionStartOffset && record.StartOffsetBefore < this._previousCompositionStartOffset) || record.IsShiftUpdate;
		}

		// Token: 0x06003D02 RID: 15618 RVA: 0x0011B484 File Offset: 0x00119684
		private void SetFinalDocumentState(UndoManager undoManager, Stack imeChangeStack, int appChangeCount, int imeSelectionAnchorOffset, int imeSelectionMovingOffset, int appSelectionAnchorOffset, int appSelectionMovingOffset)
		{
			this.TextSelection.BeginChangeNoUndo();
			try
			{
				bool compositionModifiedByEventListener = this._compositionModifiedByEventListener;
				this.UndoQuietly(appChangeCount);
				Stack redoStack = undoManager.SetRedoStack(imeChangeStack);
				int count = imeChangeStack.Count;
				this.RedoQuietly(count);
				Invariant.Assert(this._netCharCount == this.TextContainer.IMECharCount);
				if (compositionModifiedByEventListener)
				{
					int num = undoManager.UndoCount;
					if (this._isComposing)
					{
						TextParentUndoUnit textParentUndoUnit = this.OpenTextParentUndoUnit();
						try
						{
							this.CompleteComposition();
						}
						finally
						{
							this.CloseTextParentUndoUnit(textParentUndoUnit, (textParentUndoUnit.LastUnit != null) ? UndoCloseAction.Commit : UndoCloseAction.Discard);
						}
					}
					num = undoManager.UndoCount - num;
					this._compositionEventState = TextStore.CompositionEventState.ApplyingApplicationChange;
					try
					{
						this.UndoQuietly(num);
						this.UndoQuietly(count);
						undoManager.SetRedoStack(redoStack);
						this.RedoQuietly(appChangeCount);
						Invariant.Assert(this._netCharCount == this.TextContainer.IMECharCount);
						ITextPointer position = this.TextContainer.CreatePointerAtOffset(appSelectionAnchorOffset, LogicalDirection.Forward);
						ITextPointer position2 = this.TextContainer.CreatePointerAtOffset(appSelectionMovingOffset, LogicalDirection.Forward);
						this.TextSelection.Select(position, position2);
						this.MergeCompositionUndoUnits();
						return;
					}
					finally
					{
						this._compositionEventState = TextStore.CompositionEventState.RaisingEvents;
					}
				}
				ITextPointer position3 = this.TextContainer.CreatePointerAtOffset(imeSelectionAnchorOffset, LogicalDirection.Backward);
				ITextPointer position4 = this.TextContainer.CreatePointerAtOffset(imeSelectionMovingOffset, LogicalDirection.Backward);
				this.TextSelection.Select(position3, position4);
				this.MergeCompositionUndoUnits();
			}
			finally
			{
				this.TextSelection.EndChange(false, true);
			}
		}

		// Token: 0x06003D03 RID: 15619 RVA: 0x0011B628 File Offset: 0x00119828
		private void UndoQuietly(int count)
		{
			if (count > 0)
			{
				UndoManager undoManager = UndoManager.GetUndoManager(this.TextContainer.Parent);
				this.TextSelection.BeginChangeNoUndo();
				try
				{
					undoManager.Undo(count);
				}
				finally
				{
					this.TextSelection.EndChange(false, true);
				}
			}
		}

		// Token: 0x06003D04 RID: 15620 RVA: 0x0011B67C File Offset: 0x0011987C
		private void RedoQuietly(int count)
		{
			if (count > 0)
			{
				UndoManager undoManager = UndoManager.GetUndoManager(this.TextContainer.Parent);
				this.TextSelection.BeginChangeNoUndo();
				try
				{
					undoManager.Redo(count);
				}
				finally
				{
					this.TextSelection.EndChange(false, true);
				}
			}
		}

		// Token: 0x06003D05 RID: 15621 RVA: 0x0011B6D0 File Offset: 0x001198D0
		private void MergeCompositionUndoUnits()
		{
			UndoManager undoManager = UndoManager.GetUndoManager(this.TextContainer.Parent);
			if (undoManager == null || !undoManager.IsEnabled)
			{
				return;
			}
			int i = undoManager.UndoCount - 1;
			int num = undoManager.UndoCount - 1;
			while (i >= 0)
			{
				TextStore.CompositionParentUndoUnit compositionParentUndoUnit = undoManager.GetUndoUnit(i) as TextStore.CompositionParentUndoUnit;
				if (compositionParentUndoUnit == null || (compositionParentUndoUnit.IsFirstCompositionUnit && compositionParentUndoUnit.IsLastCompositionUnit))
				{
					break;
				}
				if (!compositionParentUndoUnit.IsFirstCompositionUnit)
				{
					i--;
				}
				else
				{
					int num2 = num - i;
					for (int j = i + 1; j <= i + num2; j++)
					{
						TextStore.CompositionParentUndoUnit unit = (TextStore.CompositionParentUndoUnit)undoManager.GetUndoUnit(j);
						compositionParentUndoUnit.MergeCompositionUnit(unit);
					}
					undoManager.RemoveUndoRange(i + 1, num2);
					i--;
					num = i;
				}
			}
		}

		// Token: 0x06003D06 RID: 15622 RVA: 0x0011B784 File Offset: 0x00119984
		private TextStore.CompositionParentUndoUnit PeekCompositionParentUndoUnit()
		{
			UndoManager undoManager = UndoManager.GetUndoManager(this.TextContainer.Parent);
			if (undoManager == null || !undoManager.IsEnabled)
			{
				return null;
			}
			return undoManager.PeekUndoStack() as TextStore.CompositionParentUndoUnit;
		}

		// Token: 0x17000F0E RID: 3854
		// (get) Token: 0x06003D07 RID: 15623 RVA: 0x0011B7BA File Offset: 0x001199BA
		private bool IsTextEditorValid
		{
			get
			{
				return this._weakTextEditor.IsValid;
			}
		}

		// Token: 0x17000F0F RID: 3855
		// (get) Token: 0x06003D08 RID: 15624 RVA: 0x0011B7C7 File Offset: 0x001199C7
		private TextEditor TextEditor
		{
			get
			{
				return this._weakTextEditor.TextEditor;
			}
		}

		// Token: 0x17000F10 RID: 3856
		// (get) Token: 0x06003D09 RID: 15625 RVA: 0x0011B7D4 File Offset: 0x001199D4
		private ITextSelection TextSelection
		{
			get
			{
				return this.TextEditor.Selection;
			}
		}

		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x06003D0A RID: 15626 RVA: 0x0011B7E1 File Offset: 0x001199E1
		private bool IsReadOnly
		{
			get
			{
				return (bool)this.UiScope.GetValue(TextEditor.IsReadOnlyProperty) || this.TextEditor.IsReadOnly;
			}
		}

		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x06003D0B RID: 15627 RVA: 0x0011B807 File Offset: 0x00119A07
		private List<TextStore.CompositionEventRecord> CompositionEventList
		{
			get
			{
				if (this._compositionEventList == null)
				{
					this._compositionEventList = new List<TextStore.CompositionEventRecord>();
				}
				return this._compositionEventList;
			}
		}

		// Token: 0x0400262F RID: 9775
		private readonly TextStore.ScopeWeakReference _weakTextEditor;

		// Token: 0x04002630 RID: 9776
		private TextServicesHost _textservicesHost;

		// Token: 0x04002631 RID: 9777
		[SecurityCritical]
		private UnsafeNativeMethods.ITextStoreACPSink _sink;

		// Token: 0x04002632 RID: 9778
		private bool _pendingWriteReq;

		// Token: 0x04002633 RID: 9779
		private UnsafeNativeMethods.LockFlags _lockFlags;

		// Token: 0x04002634 RID: 9780
		private UnsafeNativeMethods.LockFlags _pendingAsyncLockFlags;

		// Token: 0x04002635 RID: 9781
		private int _textChangeReentrencyCount;

		// Token: 0x04002636 RID: 9782
		private bool _isComposing;

		// Token: 0x04002637 RID: 9783
		private bool _isEffectivelyNotComposing;

		// Token: 0x04002638 RID: 9784
		private int _previousCompositionStartOffset = -1;

		// Token: 0x04002639 RID: 9785
		private int _previousCompositionEndOffset = -1;

		// Token: 0x0400263A RID: 9786
		private ITextPointer _previousCompositionStart;

		// Token: 0x0400263B RID: 9787
		private ITextPointer _previousCompositionEnd;

		// Token: 0x0400263C RID: 9788
		private TextServicesProperty _textservicesproperty;

		// Token: 0x0400263D RID: 9789
		private const int _viewCookie = 0;

		// Token: 0x0400263E RID: 9790
		[SecurityCritical]
		private SecurityCriticalDataClass<UnsafeNativeMethods.ITfDocumentMgr> _documentmanager;

		// Token: 0x0400263F RID: 9791
		private int _threadFocusCookie;

		// Token: 0x04002640 RID: 9792
		private int _editSinkCookie;

		// Token: 0x04002641 RID: 9793
		private int _editCookie;

		// Token: 0x04002642 RID: 9794
		private int _transitoryExtensionSinkCookie;

		// Token: 0x04002643 RID: 9795
		private ArrayList _preparedattributes;

		// Token: 0x04002644 RID: 9796
		private ArrayList _mouseSinks;

		// Token: 0x04002645 RID: 9797
		private static readonly TextStore.TextServicesAttribute[] _supportingattributes = new TextStore.TextServicesAttribute[]
		{
			new TextStore.TextServicesAttribute(UnsafeNativeMethods.GUID_PROP_INPUTSCOPE, TextStore.AttributeStyle.InputScope),
			new TextStore.TextServicesAttribute(UnsafeNativeMethods.TSATTRID_Font_Style_Height, TextStore.AttributeStyle.Font_Style_Height),
			new TextStore.TextServicesAttribute(UnsafeNativeMethods.TSATTRID_Font_FaceName, TextStore.AttributeStyle.Font_FaceName),
			new TextStore.TextServicesAttribute(UnsafeNativeMethods.TSATTRID_Font_SizePts, TextStore.AttributeStyle.Font_SizePts),
			new TextStore.TextServicesAttribute(UnsafeNativeMethods.TSATTRID_Text_ReadOnly, TextStore.AttributeStyle.Text_ReadOnly),
			new TextStore.TextServicesAttribute(UnsafeNativeMethods.TSATTRID_Text_Orientation, TextStore.AttributeStyle.Text_Orientation),
			new TextStore.TextServicesAttribute(UnsafeNativeMethods.TSATTRID_Text_VerticalWriting, TextStore.AttributeStyle.Text_VerticalWriting)
		};

		// Token: 0x04002646 RID: 9798
		private bool _interimSelection;

		// Token: 0x04002647 RID: 9799
		private bool _ignoreNextSelectionChange;

		// Token: 0x04002648 RID: 9800
		private int _netCharCount;

		// Token: 0x04002649 RID: 9801
		private bool _makeLayoutChangeOnGotFocus;

		// Token: 0x0400264A RID: 9802
		private TextStore.CompositionEventState _compositionEventState;

		// Token: 0x0400264B RID: 9803
		private bool _compositionModifiedByEventListener;

		// Token: 0x0400264C RID: 9804
		private List<TextStore.CompositionEventRecord> _compositionEventList;

		// Token: 0x0400264D RID: 9805
		private bool _nextUndoUnitIsFirstCompositionUnit = true;

		// Token: 0x0400264E RID: 9806
		private string _lastCompositionText;

		// Token: 0x0400264F RID: 9807
		private bool _handledByTextStoreListener;

		// Token: 0x04002650 RID: 9808
		private bool _isInUpdateLayout;

		// Token: 0x04002651 RID: 9809
		private bool _hasTextChangedInUpdateLayout;

		// Token: 0x0200090E RID: 2318
		private enum AttributeStyle
		{
			// Token: 0x04004324 RID: 17188
			InputScope,
			// Token: 0x04004325 RID: 17189
			Font_Style_Height,
			// Token: 0x04004326 RID: 17190
			Font_FaceName,
			// Token: 0x04004327 RID: 17191
			Font_SizePts,
			// Token: 0x04004328 RID: 17192
			Text_ReadOnly,
			// Token: 0x04004329 RID: 17193
			Text_Orientation,
			// Token: 0x0400432A RID: 17194
			Text_VerticalWriting
		}

		// Token: 0x0200090F RID: 2319
		private struct TextServicesAttribute
		{
			// Token: 0x060085E2 RID: 34274 RVA: 0x0024B482 File Offset: 0x00249682
			internal TextServicesAttribute(Guid guid, TextStore.AttributeStyle style)
			{
				this._guid = guid;
				this._style = style;
			}

			// Token: 0x17001E3E RID: 7742
			// (get) Token: 0x060085E3 RID: 34275 RVA: 0x0024B492 File Offset: 0x00249692
			internal Guid Guid
			{
				get
				{
					return this._guid;
				}
			}

			// Token: 0x17001E3F RID: 7743
			// (get) Token: 0x060085E4 RID: 34276 RVA: 0x0024B49A File Offset: 0x0024969A
			internal TextStore.AttributeStyle Style
			{
				get
				{
					return this._style;
				}
			}

			// Token: 0x0400432B RID: 17195
			private Guid _guid;

			// Token: 0x0400432C RID: 17196
			private TextStore.AttributeStyle _style;
		}

		// Token: 0x02000910 RID: 2320
		private class ScopeWeakReference : WeakReference
		{
			// Token: 0x060085E5 RID: 34277 RVA: 0x0023595C File Offset: 0x00233B5C
			internal ScopeWeakReference(object obj) : base(obj)
			{
			}

			// Token: 0x17001E40 RID: 7744
			// (get) Token: 0x060085E6 RID: 34278 RVA: 0x0024B4A4 File Offset: 0x002496A4
			internal bool IsValid
			{
				get
				{
					bool result;
					try
					{
						result = this.IsAlive;
					}
					catch (InvalidOperationException)
					{
						result = false;
					}
					return result;
				}
			}

			// Token: 0x17001E41 RID: 7745
			// (get) Token: 0x060085E7 RID: 34279 RVA: 0x0024B4D0 File Offset: 0x002496D0
			internal TextEditor TextEditor
			{
				get
				{
					TextEditor result;
					try
					{
						result = (TextEditor)this.Target;
					}
					catch (InvalidOperationException)
					{
						result = null;
					}
					return result;
				}
			}
		}

		// Token: 0x02000911 RID: 2321
		private class MouseSink : IDisposable, IComparer
		{
			// Token: 0x060085E8 RID: 34280 RVA: 0x0024B504 File Offset: 0x00249704
			[SecurityCritical]
			internal MouseSink(UnsafeNativeMethods.ITfRangeACP range, UnsafeNativeMethods.ITfMouseSink sink, int cookie)
			{
				this._range = new SecurityCriticalDataClass<UnsafeNativeMethods.ITfRangeACP>(range);
				this._sink = new SecurityCriticalDataClass<UnsafeNativeMethods.ITfMouseSink>(sink);
				this._cookie = cookie;
			}

			// Token: 0x060085E9 RID: 34281 RVA: 0x0024B52C File Offset: 0x0024972C
			[SecurityCritical]
			[SecurityTreatAsSafe]
			public void Dispose()
			{
				Invariant.Assert(!this._locked);
				if (this._range != null)
				{
					Marshal.ReleaseComObject(this._range.Value);
					this._range = null;
				}
				if (this._sink != null)
				{
					Marshal.ReleaseComObject(this._sink.Value);
					this._sink = null;
				}
				this._cookie = -1;
				GC.SuppressFinalize(this);
			}

			// Token: 0x060085EA RID: 34282 RVA: 0x0024B594 File Offset: 0x00249794
			public int Compare(object x, object y)
			{
				return ((TextStore.MouseSink)x)._cookie - ((TextStore.MouseSink)y)._cookie;
			}

			// Token: 0x17001E42 RID: 7746
			// (get) Token: 0x060085EB RID: 34283 RVA: 0x0024B5AD File Offset: 0x002497AD
			// (set) Token: 0x060085EC RID: 34284 RVA: 0x0024B5B5 File Offset: 0x002497B5
			internal bool Locked
			{
				get
				{
					return this._locked;
				}
				set
				{
					this._locked = value;
					if (!this._locked && this._pendingDispose)
					{
						this.Dispose();
					}
				}
			}

			// Token: 0x17001E43 RID: 7747
			// (set) Token: 0x060085ED RID: 34285 RVA: 0x0024B5D4 File Offset: 0x002497D4
			internal bool PendingDispose
			{
				set
				{
					this._pendingDispose = value;
				}
			}

			// Token: 0x17001E44 RID: 7748
			// (get) Token: 0x060085EE RID: 34286 RVA: 0x0024B5DD File Offset: 0x002497DD
			internal UnsafeNativeMethods.ITfRangeACP Range
			{
				[SecurityCritical]
				get
				{
					return this._range.Value;
				}
			}

			// Token: 0x17001E45 RID: 7749
			// (get) Token: 0x060085EF RID: 34287 RVA: 0x0024B5EA File Offset: 0x002497EA
			internal UnsafeNativeMethods.ITfMouseSink Sink
			{
				[SecurityCritical]
				get
				{
					return this._sink.Value;
				}
			}

			// Token: 0x17001E46 RID: 7750
			// (get) Token: 0x060085F0 RID: 34288 RVA: 0x0024B5F7 File Offset: 0x002497F7
			internal int Cookie
			{
				get
				{
					return this._cookie;
				}
			}

			// Token: 0x0400432D RID: 17197
			[SecurityCritical]
			private SecurityCriticalDataClass<UnsafeNativeMethods.ITfRangeACP> _range;

			// Token: 0x0400432E RID: 17198
			[SecurityCritical]
			private SecurityCriticalDataClass<UnsafeNativeMethods.ITfMouseSink> _sink;

			// Token: 0x0400432F RID: 17199
			private int _cookie;

			// Token: 0x04004330 RID: 17200
			private bool _locked;

			// Token: 0x04004331 RID: 17201
			private bool _pendingDispose;
		}

		// Token: 0x02000912 RID: 2322
		private class CompositionParentUndoUnit : TextParentUndoUnit
		{
			// Token: 0x060085F1 RID: 34289 RVA: 0x0024B5FF File Offset: 0x002497FF
			internal CompositionParentUndoUnit(ITextSelection selection, ITextPointer anchorPosition, ITextPointer movingPosition, bool isFirstCompositionUnit) : base(selection, anchorPosition, movingPosition)
			{
				this._isFirstCompositionUnit = isFirstCompositionUnit;
			}

			// Token: 0x060085F2 RID: 34290 RVA: 0x0024B612 File Offset: 0x00249812
			private CompositionParentUndoUnit(TextStore.CompositionParentUndoUnit undoUnit) : base(undoUnit)
			{
				this._isFirstCompositionUnit = undoUnit._isFirstCompositionUnit;
				this._isLastCompositionUnit = undoUnit._isLastCompositionUnit;
			}

			// Token: 0x060085F3 RID: 34291 RVA: 0x0024B633 File Offset: 0x00249833
			protected override TextParentUndoUnit CreateRedoUnit()
			{
				return new TextStore.CompositionParentUndoUnit(this);
			}

			// Token: 0x060085F4 RID: 34292 RVA: 0x0024B63C File Offset: 0x0024983C
			internal void MergeCompositionUnit(TextStore.CompositionParentUndoUnit unit)
			{
				object[] array = unit.CopyUnits();
				Invariant.Assert(this.Locked);
				this.Locked = false;
				for (int i = array.Length - 1; i >= 0; i--)
				{
					this.Add((IUndoUnit)array[i]);
				}
				this.Locked = true;
				base.MergeRedoSelectionState(unit);
				this._isLastCompositionUnit |= unit.IsLastCompositionUnit;
			}

			// Token: 0x17001E47 RID: 7751
			// (get) Token: 0x060085F5 RID: 34293 RVA: 0x0024B6A1 File Offset: 0x002498A1
			internal bool IsFirstCompositionUnit
			{
				get
				{
					return this._isFirstCompositionUnit;
				}
			}

			// Token: 0x17001E48 RID: 7752
			// (get) Token: 0x060085F6 RID: 34294 RVA: 0x0024B6A9 File Offset: 0x002498A9
			// (set) Token: 0x060085F7 RID: 34295 RVA: 0x0024B6B1 File Offset: 0x002498B1
			internal bool IsLastCompositionUnit
			{
				get
				{
					return this._isLastCompositionUnit;
				}
				set
				{
					this._isLastCompositionUnit = value;
				}
			}

			// Token: 0x060085F8 RID: 34296 RVA: 0x0024B6BA File Offset: 0x002498BA
			private object[] CopyUnits()
			{
				return base.Units.ToArray();
			}

			// Token: 0x04004332 RID: 17202
			private readonly bool _isFirstCompositionUnit;

			// Token: 0x04004333 RID: 17203
			private bool _isLastCompositionUnit;
		}

		// Token: 0x02000913 RID: 2323
		private enum CompositionEventState
		{
			// Token: 0x04004335 RID: 17205
			NotRaisingEvents,
			// Token: 0x04004336 RID: 17206
			RaisingEvents,
			// Token: 0x04004337 RID: 17207
			ApplyingApplicationChange
		}

		// Token: 0x02000914 RID: 2324
		private enum CompositionStage
		{
			// Token: 0x04004339 RID: 17209
			StartComposition = 1,
			// Token: 0x0400433A RID: 17210
			UpdateComposition,
			// Token: 0x0400433B RID: 17211
			EndComposition
		}

		// Token: 0x02000915 RID: 2325
		private class CompositionEventRecord
		{
			// Token: 0x060085F9 RID: 34297 RVA: 0x0024B6C7 File Offset: 0x002498C7
			internal CompositionEventRecord(TextStore.CompositionStage stage, int startOffsetBefore, int endOffsetBefore, string text) : this(stage, startOffsetBefore, endOffsetBefore, text, false)
			{
			}

			// Token: 0x060085FA RID: 34298 RVA: 0x0024B6D5 File Offset: 0x002498D5
			internal CompositionEventRecord(TextStore.CompositionStage stage, int startOffsetBefore, int endOffsetBefore, string text, bool isShiftUpdate)
			{
				this._stage = stage;
				this._startOffsetBefore = startOffsetBefore;
				this._endOffsetBefore = endOffsetBefore;
				this._text = text;
				this._isShiftUpdate = isShiftUpdate;
			}

			// Token: 0x17001E49 RID: 7753
			// (get) Token: 0x060085FB RID: 34299 RVA: 0x0024B702 File Offset: 0x00249902
			internal TextStore.CompositionStage Stage
			{
				get
				{
					return this._stage;
				}
			}

			// Token: 0x17001E4A RID: 7754
			// (get) Token: 0x060085FC RID: 34300 RVA: 0x0024B70A File Offset: 0x0024990A
			internal int StartOffsetBefore
			{
				get
				{
					return this._startOffsetBefore;
				}
			}

			// Token: 0x17001E4B RID: 7755
			// (get) Token: 0x060085FD RID: 34301 RVA: 0x0024B712 File Offset: 0x00249912
			internal int EndOffsetBefore
			{
				get
				{
					return this._endOffsetBefore;
				}
			}

			// Token: 0x17001E4C RID: 7756
			// (get) Token: 0x060085FE RID: 34302 RVA: 0x0024B71A File Offset: 0x0024991A
			internal string Text
			{
				get
				{
					return this._text;
				}
			}

			// Token: 0x17001E4D RID: 7757
			// (get) Token: 0x060085FF RID: 34303 RVA: 0x0024B722 File Offset: 0x00249922
			internal bool IsShiftUpdate
			{
				get
				{
					return this._isShiftUpdate;
				}
			}

			// Token: 0x0400433C RID: 17212
			private readonly TextStore.CompositionStage _stage;

			// Token: 0x0400433D RID: 17213
			private readonly int _startOffsetBefore;

			// Token: 0x0400433E RID: 17214
			private readonly int _endOffsetBefore;

			// Token: 0x0400433F RID: 17215
			private readonly string _text;

			// Token: 0x04004340 RID: 17216
			private readonly bool _isShiftUpdate;
		}
	}
}
