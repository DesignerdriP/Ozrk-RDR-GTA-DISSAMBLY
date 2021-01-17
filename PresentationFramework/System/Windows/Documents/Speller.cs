using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Internal.Utility;

namespace System.Windows.Documents
{
	// Token: 0x020003DA RID: 986
	internal class Speller
	{
		// Token: 0x06003547 RID: 13639 RVA: 0x000F1480 File Offset: 0x000EF680
		internal Speller(TextEditor textEditor)
		{
			this._textEditor = textEditor;
			this._textEditor.TextContainer.Change += this.OnTextContainerChange;
			if (this._textEditor.TextContainer.SymbolCount > 0)
			{
				this.ScheduleIdleCallback();
			}
			this._defaultCulture = ((InputLanguageManager.Current != null) ? InputLanguageManager.Current.CurrentInputLanguage : Thread.CurrentThread.CurrentCulture);
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x000F14F4 File Offset: 0x000EF6F4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void Detach()
		{
			Invariant.Assert(this._textEditor != null);
			this._textEditor.TextContainer.Change -= this.OnTextContainerChange;
			if (this._pendingCaretMovedCallback)
			{
				this._textEditor.Selection.Changed -= this.OnCaretMoved;
				this._textEditor.UiScope.LostFocus -= this.OnLostFocus;
				this._pendingCaretMovedCallback = false;
			}
			if (this._highlightLayer != null)
			{
				this._textEditor.TextContainer.Highlights.RemoveLayer(this._highlightLayer);
				this._highlightLayer = null;
			}
			this._statusTable = null;
			if (this._spellerInterop != null)
			{
				this._spellerInterop.Dispose();
				this._spellerInterop = null;
			}
			this._textEditor = null;
		}

		// Token: 0x06003549 RID: 13641 RVA: 0x000F15C4 File Offset: 0x000EF7C4
		internal SpellingError GetError(ITextPointer position, LogicalDirection direction, bool forceEvaluation)
		{
			if (forceEvaluation && this.EnsureInitialized() && this._statusTable.IsRunType(position.CreateStaticPointer(), direction, SpellerStatusTable.RunType.Dirty))
			{
				this.ScanPosition(position, direction);
			}
			ITextPointer start;
			ITextPointer end;
			SpellingError result;
			if (this._statusTable != null && this._statusTable.GetError(position.CreateStaticPointer(), direction, out start, out end))
			{
				result = new SpellingError(this, start, end);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x000F1628 File Offset: 0x000EF828
		internal ITextPointer GetNextSpellingErrorPosition(ITextPointer position, LogicalDirection direction)
		{
			if (!this.EnsureInitialized())
			{
				return null;
			}
			SpellerStatusTable.RunType runType;
			StaticTextPointer staticTextPointer;
			while (this._statusTable.GetRun(position.CreateStaticPointer(), direction, out runType, out staticTextPointer) && runType != SpellerStatusTable.RunType.Error)
			{
				if (runType == SpellerStatusTable.RunType.Dirty)
				{
					this.ScanPosition(position, direction);
					this._statusTable.GetRun(position.CreateStaticPointer(), direction, out runType, out staticTextPointer);
					Invariant.Assert(runType != SpellerStatusTable.RunType.Dirty);
					if (runType == SpellerStatusTable.RunType.Error)
					{
						break;
					}
				}
				position = staticTextPointer.CreateDynamicTextPointer(direction);
			}
			SpellingError error = this.GetError(position, direction, false);
			if (error != null)
			{
				return error.Start;
			}
			return null;
		}

		// Token: 0x0600354B RID: 13643 RVA: 0x000F16B0 File Offset: 0x000EF8B0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal IList GetSuggestionsForError(SpellingError error)
		{
			ArrayList arrayList = new ArrayList(1);
			XmlLanguage language;
			CultureInfo currentCultureAndLanguage = this.GetCurrentCultureAndLanguage(error.Start, out language);
			if (currentCultureAndLanguage != null && this._spellerInterop.CanSpellCheck(currentCultureAndLanguage))
			{
				ITextPointer contentStart;
				ITextPointer contextStart;
				this.ExpandToWordBreakAndContext(error.Start, LogicalDirection.Backward, language, out contentStart, out contextStart);
				ITextPointer contentEnd;
				ITextPointer contextEnd;
				this.ExpandToWordBreakAndContext(error.End, LogicalDirection.Forward, language, out contentEnd, out contextEnd);
				Speller.TextMap textMap = new Speller.TextMap(contextStart, contextEnd, contentStart, contentEnd);
				this.SetCulture(currentCultureAndLanguage);
				this._spellerInterop.Mode = SpellerInteropBase.SpellerMode.SpellingErrorsWithSuggestions;
				this._spellerInterop.EnumTextSegments(textMap.Text, textMap.TextLength, null, new SpellerInteropBase.EnumTextSegmentsCallback(this.ScanErrorTextSegment), new Speller.TextMapCallbackData(textMap, arrayList));
			}
			return arrayList;
		}

		// Token: 0x0600354C RID: 13644 RVA: 0x000F1764 File Offset: 0x000EF964
		internal void IgnoreAll(string word)
		{
			if (this._ignoredWordsList == null)
			{
				this._ignoredWordsList = new ArrayList(1);
			}
			int num = this._ignoredWordsList.BinarySearch(word, new CaseInsensitiveComparer(this._defaultCulture));
			if (num < 0)
			{
				this._ignoredWordsList.Insert(~num, word);
				if (this._statusTable != null)
				{
					StaticTextPointer textPosition = this._textEditor.TextContainer.CreateStaticPointerAtOffset(0);
					char[] array = null;
					while (!textPosition.IsNull)
					{
						ITextPointer textPointer;
						ITextPointer textPointer2;
						if (this._statusTable.GetError(textPosition, LogicalDirection.Forward, out textPointer, out textPointer2))
						{
							string textInternal = TextRangeBase.GetTextInternal(textPointer, textPointer2, ref array);
							if (string.Compare(word, textInternal, true, this._defaultCulture) == 0)
							{
								this._statusTable.MarkCleanRange(textPointer, textPointer2);
							}
						}
						textPosition = this._statusTable.GetNextErrorTransition(textPosition, LogicalDirection.Forward);
					}
				}
			}
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x000F1824 File Offset: 0x000EFA24
		internal void SetSpellingReform(SpellingReform spellingReform)
		{
			if (this._spellingReform != spellingReform)
			{
				this._spellingReform = spellingReform;
				this.ResetErrors();
			}
		}

		// Token: 0x0600354E RID: 13646 RVA: 0x000F183C File Offset: 0x000EFA3C
		internal void SetCustomDictionaries(CustomDictionarySources dictionaryLocations, bool add)
		{
			if (!this.EnsureInitialized())
			{
				return;
			}
			if (add)
			{
				using (IEnumerator<Uri> enumerator = dictionaryLocations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Uri uri = enumerator.Current;
						this.OnDictionaryUriAdded(uri);
					}
					return;
				}
			}
			this.OnDictionaryUriCollectionCleared();
		}

		// Token: 0x0600354F RID: 13647 RVA: 0x000F1898 File Offset: 0x000EFA98
		internal void ResetErrors()
		{
			if (this._statusTable != null)
			{
				this._statusTable.MarkDirtyRange(this._textEditor.TextContainer.Start, this._textEditor.TextContainer.End);
				if (this._textEditor.TextContainer.SymbolCount > 0)
				{
					this.ScheduleIdleCallback();
				}
			}
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x000F18F1 File Offset: 0x000EFAF1
		internal static bool IsSpellerAffectingProperty(DependencyProperty property)
		{
			return property == FrameworkElement.LanguageProperty || property == SpellCheck.SpellingReformProperty;
		}

		// Token: 0x06003551 RID: 13649 RVA: 0x000F1908 File Offset: 0x000EFB08
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnDictionaryUriAdded(Uri uri)
		{
			if (!this.EnsureInitialized())
			{
				return;
			}
			if (this.UriMap.ContainsKey(uri))
			{
				this.OnDictionaryUriRemoved(uri);
			}
			if (!uri.IsAbsoluteUri || uri.IsFile)
			{
				Uri uri2 = Speller.ResolvePathUri(uri);
				object lexicon = this._spellerInterop.LoadDictionary(uri2.LocalPath);
				this.UriMap.Add(uri, new Speller.DictionaryInfo(uri2, lexicon));
			}
			else
			{
				this.LoadDictionaryFromPackUri(uri);
			}
			this.ResetErrors();
		}

		// Token: 0x06003552 RID: 13650 RVA: 0x000F1980 File Offset: 0x000EFB80
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnDictionaryUriRemoved(Uri uri)
		{
			if (!this.EnsureInitialized())
			{
				return;
			}
			if (!this.UriMap.ContainsKey(uri))
			{
				return;
			}
			Speller.DictionaryInfo dictionaryInfo = this.UriMap[uri];
			try
			{
				this._spellerInterop.UnloadDictionary(dictionaryInfo.Lexicon);
			}
			catch (Exception ex)
			{
				if (SecurityHelper.CheckUnmanagedCodePermission())
				{
					Trace.Write(string.Format(CultureInfo.InvariantCulture, "Unloading dictionary failed. Original Uri:{0}, file Uri:{1}, exception:{2}", new object[]
					{
						uri.ToString(),
						dictionaryInfo.PathUri.ToString(),
						ex.ToString()
					}));
				}
				throw;
			}
			this.UriMap.Remove(uri);
			this.ResetErrors();
		}

		// Token: 0x06003553 RID: 13651 RVA: 0x000F1A2C File Offset: 0x000EFC2C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnDictionaryUriCollectionCleared()
		{
			if (!this.EnsureInitialized())
			{
				return;
			}
			this._spellerInterop.ReleaseAllLexicons();
			this.UriMap.Clear();
			this.ResetErrors();
		}

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06003554 RID: 13652 RVA: 0x000F1A53 File Offset: 0x000EFC53
		internal SpellerStatusTable StatusTable
		{
			get
			{
				return this._statusTable;
			}
		}

		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06003555 RID: 13653 RVA: 0x000F1A5B File Offset: 0x000EFC5B
		private Dictionary<Uri, Speller.DictionaryInfo> UriMap
		{
			[SecurityCritical]
			get
			{
				if (this._uriMap == null)
				{
					this._uriMap = new Dictionary<Uri, Speller.DictionaryInfo>();
				}
				return this._uriMap;
			}
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x000F1A78 File Offset: 0x000EFC78
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool EnsureInitialized()
		{
			if (this._spellerInterop != null)
			{
				return true;
			}
			if (this._failedToInit)
			{
				return false;
			}
			Invariant.Assert(this._highlightLayer == null);
			Invariant.Assert(this._statusTable == null);
			this._spellerInterop = SpellerInteropBase.CreateInstance();
			this._failedToInit = (this._spellerInterop == null);
			if (this._failedToInit)
			{
				return false;
			}
			this._highlightLayer = new SpellerHighlightLayer(this);
			this._statusTable = new SpellerStatusTable(this._textEditor.TextContainer.Start, this._highlightLayer);
			this._textEditor.TextContainer.Highlights.AddLayer(this._highlightLayer);
			this._spellingReform = (SpellingReform)this._textEditor.UiScope.GetValue(SpellCheck.SpellingReformProperty);
			return true;
		}

		// Token: 0x06003557 RID: 13655 RVA: 0x000F1B42 File Offset: 0x000EFD42
		private void ScheduleIdleCallback()
		{
			if (!this._pendingIdleCallback)
			{
				Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new DispatcherOperationCallback(this.OnIdle), null);
				this._pendingIdleCallback = true;
			}
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x000F1B6C File Offset: 0x000EFD6C
		private void ScheduleCaretMovedCallback()
		{
			if (!this._pendingCaretMovedCallback)
			{
				this._textEditor.Selection.Changed += this.OnCaretMoved;
				this._textEditor.UiScope.LostFocus += this.OnLostFocus;
				this._pendingCaretMovedCallback = true;
			}
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x000F1BC0 File Offset: 0x000EFDC0
		private void OnTextContainerChange(object sender, TextContainerChangeEventArgs e)
		{
			Invariant.Assert(sender == this._textEditor.TextContainer);
			if (e.Count == 0 || (e.TextChange == TextChangeType.PropertyModified && !Speller.IsSpellerAffectingProperty(e.Property)))
			{
				return;
			}
			if (this._failedToInit)
			{
				return;
			}
			if (this._statusTable != null)
			{
				this._statusTable.OnTextChange(e);
			}
			this.ScheduleIdleCallback();
		}

		// Token: 0x0600355A RID: 13658 RVA: 0x000F1C24 File Offset: 0x000EFE24
		private object OnIdle(object unused)
		{
			Invariant.Assert(this._pendingIdleCallback);
			this._pendingIdleCallback = false;
			if (this._textEditor != null && this.EnsureInitialized())
			{
				long timeLimit = DateTime.Now.Ticks + 200000L;
				ITextPointer textPointer = null;
				Speller.ScanStatus scanStatus = null;
				ITextPointer start;
				while (this.GetNextScanRange(textPointer, out start, out textPointer))
				{
					scanStatus = this.ScanRange(start, textPointer, timeLimit);
					if (scanStatus.HasExceededTimeLimit)
					{
						break;
					}
				}
				if (scanStatus != null && scanStatus.HasExceededTimeLimit)
				{
					this.ScheduleIdleCallback();
				}
			}
			return null;
		}

		// Token: 0x0600355B RID: 13659 RVA: 0x000F1C9E File Offset: 0x000EFE9E
		private void OnCaretMoved(object sender, EventArgs e)
		{
			this.OnCaretMovedWorker();
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x000F1C9E File Offset: 0x000EFE9E
		private void OnLostFocus(object sender, RoutedEventArgs e)
		{
			this.OnCaretMovedWorker();
		}

		// Token: 0x0600355D RID: 13661 RVA: 0x000F1CA8 File Offset: 0x000EFEA8
		private void OnCaretMovedWorker()
		{
			if (!this._pendingCaretMovedCallback || this._textEditor == null)
			{
				return;
			}
			this._textEditor.Selection.Changed -= this.OnCaretMoved;
			this._textEditor.UiScope.LostFocus -= this.OnLostFocus;
			this._pendingCaretMovedCallback = false;
			this.ScheduleIdleCallback();
		}

		// Token: 0x0600355E RID: 13662 RVA: 0x000F1D0C File Offset: 0x000EFF0C
		private bool GetNextScanRange(ITextPointer searchStart, out ITextPointer start, out ITextPointer end)
		{
			start = null;
			end = null;
			if (searchStart == null)
			{
				searchStart = this._textEditor.TextContainer.Start;
			}
			ITextPointer textPointer;
			ITextPointer rawEnd;
			this.GetNextScanRangeRaw(searchStart, out textPointer, out rawEnd);
			if (textPointer != null)
			{
				this.AdjustScanRangeAroundComposition(textPointer, rawEnd, out start, out end);
			}
			return start != null;
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x000F1D54 File Offset: 0x000EFF54
		private void GetNextScanRangeRaw(ITextPointer searchStart, out ITextPointer start, out ITextPointer end)
		{
			Invariant.Assert(searchStart != null);
			start = null;
			end = null;
			this._statusTable.GetFirstDirtyRange(searchStart, out start, out end);
			if (start != null)
			{
				Invariant.Assert(start.CompareTo(end) < 0);
				if (start.GetOffsetToPosition(end) > 64)
				{
					end = start.CreatePointer(64);
				}
				XmlLanguage currentLanguage = this.GetCurrentLanguage(start);
				end = this.GetNextLanguageTransition(start, LogicalDirection.Forward, currentLanguage, end);
				Invariant.Assert(start.CompareTo(end) < 0);
			}
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x000F1DD4 File Offset: 0x000EFFD4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void AdjustScanRangeAroundComposition(ITextPointer rawStart, ITextPointer rawEnd, out ITextPointer start, out ITextPointer end)
		{
			start = rawStart;
			end = rawEnd;
			if (!this._textEditor.Selection.IsEmpty)
			{
				return;
			}
			if (!this._textEditor.UiScope.IsKeyboardFocused)
			{
				return;
			}
			ITextPointer start2 = this._textEditor.Selection.Start;
			this._spellerInterop.Mode = SpellerInteropBase.SpellerMode.WordBreaking;
			XmlLanguage currentLanguage = this.GetCurrentLanguage(start2);
			ITextPointer textPointer = this.SearchForWordBreaks(start2, LogicalDirection.Backward, currentLanguage, 1, false);
			ITextPointer textPointer2 = this.SearchForWordBreaks(start2, LogicalDirection.Forward, currentLanguage, 1, false);
			Speller.TextMap textMap = new Speller.TextMap(textPointer, textPointer2, start2, start2);
			ArrayList arrayList = new ArrayList(2);
			this._spellerInterop.EnumTextSegments(textMap.Text, textMap.TextLength, null, new SpellerInteropBase.EnumTextSegmentsCallback(this.ExpandToWordBreakCallback), arrayList);
			if (arrayList.Count != 0)
			{
				int offset;
				int offset2;
				this.FindPositionInSegmentList(textMap, LogicalDirection.Backward, arrayList, out offset, out offset2);
				textPointer = textMap.MapOffsetToPosition(offset);
				textPointer2 = textMap.MapOffsetToPosition(offset2);
			}
			if (textPointer.CompareTo(rawEnd) < 0 && textPointer2.CompareTo(rawStart) > 0)
			{
				if (textPointer.CompareTo(rawStart) > 0)
				{
					end = textPointer;
				}
				else if (textPointer2.CompareTo(rawEnd) < 0)
				{
					start = textPointer2;
				}
				else
				{
					this.GetNextScanRangeRaw(textPointer2, out start, out end);
				}
				this.ScheduleCaretMovedCallback();
			}
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x000F1EF4 File Offset: 0x000F00F4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private Speller.ScanStatus ScanRange(ITextPointer start, ITextPointer end, long timeLimit)
		{
			Speller.ScanStatus scanStatus = new Speller.ScanStatus(timeLimit, start);
			XmlLanguage language;
			CultureInfo currentCultureAndLanguage = this.GetCurrentCultureAndLanguage(start, out language);
			if (currentCultureAndLanguage == null)
			{
				this._statusTable.MarkCleanRange(start, end);
			}
			else
			{
				this.SetCulture(currentCultureAndLanguage);
				ITextPointer textPointer;
				ITextPointer textPointer2;
				this.ExpandToWordBreakAndContext(start, LogicalDirection.Backward, language, out textPointer, out textPointer2);
				ITextPointer textPointer3;
				ITextPointer textPointer4;
				this.ExpandToWordBreakAndContext(end, LogicalDirection.Forward, language, out textPointer3, out textPointer4);
				Invariant.Assert(textPointer.CompareTo(textPointer3) < 0);
				Invariant.Assert(textPointer2.CompareTo(textPointer4) < 0);
				Invariant.Assert(textPointer.CompareTo(textPointer2) >= 0);
				Invariant.Assert(textPointer3.CompareTo(textPointer4) <= 0);
				this._statusTable.MarkCleanRange(textPointer, textPointer3);
				if (this._spellerInterop.CanSpellCheck(currentCultureAndLanguage))
				{
					this._spellerInterop.Mode = SpellerInteropBase.SpellerMode.SpellingErrors;
					Speller.TextMap textMap = new Speller.TextMap(textPointer2, textPointer4, textPointer, textPointer3);
					this._spellerInterop.EnumTextSegments(textMap.Text, textMap.TextLength, new SpellerInteropBase.EnumSentencesCallback(this.ScanRangeCheckTimeLimitCallback), new SpellerInteropBase.EnumTextSegmentsCallback(this.ScanTextSegment), new Speller.TextMapCallbackData(textMap, scanStatus));
					if (scanStatus.TimeoutPosition != null)
					{
						if (scanStatus.TimeoutPosition.CompareTo(end) < 0)
						{
							this._statusTable.MarkDirtyRange(scanStatus.TimeoutPosition, end);
							if (scanStatus.TimeoutPosition.CompareTo(start) <= 0)
							{
								string text = string.Concat(new object[]
								{
									"Speller is not advancing! \nCulture = ",
									currentCultureAndLanguage,
									"\nStart offset = ",
									start.Offset,
									" parent = ",
									start.ParentType.Name,
									"\nContextStart offset = ",
									textPointer2.Offset,
									" parent = ",
									textPointer2.ParentType.Name,
									"\nContentStart offset = ",
									textPointer.Offset,
									" parent = ",
									textPointer.ParentType.Name,
									"\nContentEnd offset = ",
									textPointer3.Offset,
									" parent = ",
									textPointer3.ParentType.Name,
									"\nContextEnd offset = ",
									textPointer4.Offset,
									" parent = ",
									textPointer4.ParentType.Name,
									"\nTimeout offset = ",
									scanStatus.TimeoutPosition.Offset,
									" parent = ",
									scanStatus.TimeoutPosition.ParentType.Name,
									"\ntextMap TextLength = ",
									textMap.TextLength,
									" text = ",
									new string(textMap.Text),
									"\nDocument = ",
									start.TextContainer.Parent.GetType().Name,
									"\n"
								});
								if (start is TextPointer)
								{
									text = text + "Xml = " + new TextRange((TextPointer)start.TextContainer.Start, (TextPointer)start.TextContainer.End).Xml;
								}
								Invariant.Assert(false, text);
							}
						}
						else
						{
							Invariant.Assert(scanStatus.TimeoutPosition.CompareTo(textPointer3) <= 0);
						}
					}
				}
			}
			return scanStatus;
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x000F2248 File Offset: 0x000F0448
		private bool ScanErrorTextSegment(SpellerInteropBase.ISpellerSegment textSegment, object o)
		{
			Speller.TextMapCallbackData textMapCallbackData = (Speller.TextMapCallbackData)o;
			SpellerInteropBase.ITextRange textRange = textSegment.TextRange;
			if (textRange.Start + textRange.Length <= textMapCallbackData.TextMap.ContentStartOffset)
			{
				return true;
			}
			if (textRange.Start >= textMapCallbackData.TextMap.ContentEndOffset)
			{
				return false;
			}
			if (textRange.Length > 1)
			{
				if (textSegment.SubSegments.Count == 0)
				{
					ArrayList arrayList = (ArrayList)textMapCallbackData.Data;
					if (textSegment.Suggestions.Count <= 0)
					{
						return false;
					}
					using (IEnumerator<string> enumerator = textSegment.Suggestions.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string value = enumerator.Current;
							arrayList.Add(value);
						}
						return false;
					}
				}
				textSegment.EnumSubSegments(new SpellerInteropBase.EnumTextSegmentsCallback(this.ScanErrorTextSegment), textMapCallbackData);
			}
			return false;
		}

		// Token: 0x06003563 RID: 13667 RVA: 0x000F231C File Offset: 0x000F051C
		private bool ScanTextSegment(SpellerInteropBase.ISpellerSegment textSegment, object o)
		{
			Speller.TextMapCallbackData textMapCallbackData = (Speller.TextMapCallbackData)o;
			SpellerInteropBase.ITextRange textRange = textSegment.TextRange;
			if (textRange.Start + textRange.Length <= textMapCallbackData.TextMap.ContentStartOffset)
			{
				return true;
			}
			if (textRange.Start >= textMapCallbackData.TextMap.ContentEndOffset)
			{
				return false;
			}
			if (textRange.Length > 1)
			{
				char[] array = new char[textRange.Length];
				Array.Copy(textMapCallbackData.TextMap.Text, textRange.Start, array, 0, textRange.Length);
				if (!this.IsIgnoredWord(array) && !textSegment.IsClean)
				{
					if (textSegment.SubSegments.Count == 0)
					{
						this.MarkErrorRange(textMapCallbackData.TextMap, textRange);
					}
					else
					{
						textSegment.EnumSubSegments(new SpellerInteropBase.EnumTextSegmentsCallback(this.ScanTextSegment), textMapCallbackData);
					}
				}
			}
			return true;
		}

		// Token: 0x06003564 RID: 13668 RVA: 0x000F23DC File Offset: 0x000F05DC
		private bool ScanRangeCheckTimeLimitCallback(SpellerInteropBase.ISpellerSentence sentence, object o)
		{
			Speller.TextMapCallbackData textMapCallbackData = (Speller.TextMapCallbackData)o;
			Speller.ScanStatus scanStatus = (Speller.ScanStatus)textMapCallbackData.Data;
			if (scanStatus.HasExceededTimeLimit)
			{
				Invariant.Assert(scanStatus.TimeoutPosition == null);
				int endOffset = sentence.EndOffset;
				if (endOffset >= 0)
				{
					int num = Math.Min(textMapCallbackData.TextMap.ContentEndOffset, endOffset);
					if (num > textMapCallbackData.TextMap.ContentStartOffset)
					{
						ITextPointer textPointer = textMapCallbackData.TextMap.MapOffsetToPosition(num);
						if (textPointer.CompareTo(scanStatus.StartPosition) > 0)
						{
							scanStatus.TimeoutPosition = textPointer;
						}
					}
				}
			}
			return scanStatus.TimeoutPosition == null;
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x000F246C File Offset: 0x000F066C
		private void MarkErrorRange(Speller.TextMap textMap, SpellerInteropBase.ITextRange sTextRange)
		{
			if (sTextRange.Start + sTextRange.Length > textMap.ContentEndOffset)
			{
				return;
			}
			ITextPointer start = textMap.MapOffsetToPosition(sTextRange.Start);
			ITextPointer end = textMap.MapOffsetToPosition(sTextRange.Start + sTextRange.Length);
			if (sTextRange.Start < textMap.ContentStartOffset)
			{
				Invariant.Assert(sTextRange.Start + sTextRange.Length > textMap.ContentStartOffset);
				this._statusTable.MarkCleanRange(start, end);
			}
			this._statusTable.MarkErrorRange(start, end);
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x000F24F4 File Offset: 0x000F06F4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void ExpandToWordBreakAndContext(ITextPointer position, LogicalDirection direction, XmlLanguage language, out ITextPointer contentPosition, out ITextPointer contextPosition)
		{
			contentPosition = position;
			contextPosition = position;
			if (position.GetPointerContext(direction) == TextPointerContext.None)
			{
				return;
			}
			this._spellerInterop.Mode = SpellerInteropBase.SpellerMode.WordBreaking;
			ITextPointer textPointer = this.SearchForWordBreaks(position, direction, language, 4, true);
			LogicalDirection direction2 = (direction == LogicalDirection.Forward) ? LogicalDirection.Backward : LogicalDirection.Forward;
			ITextPointer textPointer2 = this.SearchForWordBreaks(position, direction2, language, 1, false);
			ITextPointer contextStart;
			ITextPointer contextEnd;
			if (direction == LogicalDirection.Backward)
			{
				contextStart = textPointer;
				contextEnd = textPointer2;
			}
			else
			{
				contextStart = textPointer2;
				contextEnd = textPointer;
			}
			Speller.TextMap textMap = new Speller.TextMap(contextStart, contextEnd, position, position);
			ArrayList arrayList = new ArrayList(5);
			this._spellerInterop.EnumTextSegments(textMap.Text, textMap.TextLength, null, new SpellerInteropBase.EnumTextSegmentsCallback(this.ExpandToWordBreakCallback), arrayList);
			if (arrayList.Count != 0)
			{
				int num2;
				int num3;
				int num = this.FindPositionInSegmentList(textMap, direction, arrayList, out num2, out num3);
				int num4;
				if (direction == LogicalDirection.Backward)
				{
					num4 = ((textMap.ContentStartOffset == num3) ? num3 : num2);
				}
				else
				{
					num4 = ((textMap.ContentStartOffset == num2) ? num2 : num3);
				}
				Speller.TextMapOffsetErrorLogger textMapOffsetErrorLogger = new Speller.TextMapOffsetErrorLogger(direction, textMap, arrayList, num, num2, num3, num4);
				textMapOffsetErrorLogger.LogDebugInfo();
				contentPosition = textMap.MapOffsetToPosition(num4);
				int num5;
				if (direction == LogicalDirection.Backward)
				{
					num -= 3;
					SpellerInteropBase.ITextRange textRange = (SpellerInteropBase.ITextRange)arrayList[Math.Max(num, 0)];
					num5 = Math.Min(textRange.Start, num4);
				}
				else
				{
					num += 4;
					SpellerInteropBase.ITextRange textRange = (SpellerInteropBase.ITextRange)arrayList[Math.Min(num, arrayList.Count - 1)];
					num5 = Math.Max(textRange.Start + textRange.Length, num4);
				}
				textMapOffsetErrorLogger.ContextOffset = num5;
				textMapOffsetErrorLogger.LogDebugInfo();
				contextPosition = textMap.MapOffsetToPosition(num5);
			}
			if (direction == LogicalDirection.Backward)
			{
				if (position.CompareTo(contentPosition) < 0)
				{
					contentPosition = position;
				}
				if (position.CompareTo(contextPosition) < 0)
				{
					contextPosition = position;
					return;
				}
			}
			else
			{
				if (position.CompareTo(contentPosition) > 0)
				{
					contentPosition = position;
				}
				if (position.CompareTo(contextPosition) > 0)
				{
					contextPosition = position;
				}
			}
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x000F26C8 File Offset: 0x000F08C8
		private int FindPositionInSegmentList(Speller.TextMap textMap, LogicalDirection direction, ArrayList segments, out int leftWordBreak, out int rightWordBreak)
		{
			leftWordBreak = int.MaxValue;
			rightWordBreak = -1;
			SpellerInteropBase.ITextRange textRange = (SpellerInteropBase.ITextRange)segments[0];
			int i;
			if (textMap.ContentStartOffset < textRange.Start)
			{
				leftWordBreak = 0;
				rightWordBreak = textRange.Start;
				i = -1;
			}
			else
			{
				textRange = (SpellerInteropBase.ITextRange)segments[segments.Count - 1];
				if (textMap.ContentStartOffset > textRange.Start + textRange.Length)
				{
					leftWordBreak = textRange.Start + textRange.Length;
					rightWordBreak = textMap.TextLength;
					i = segments.Count;
				}
				else
				{
					for (i = 0; i < segments.Count; i++)
					{
						textRange = (SpellerInteropBase.ITextRange)segments[i];
						leftWordBreak = textRange.Start;
						rightWordBreak = textRange.Start + textRange.Length;
						if (leftWordBreak <= textMap.ContentStartOffset && rightWordBreak >= textMap.ContentStartOffset)
						{
							break;
						}
						if (i < segments.Count - 1 && rightWordBreak < textMap.ContentStartOffset)
						{
							textRange = (SpellerInteropBase.ITextRange)segments[i + 1];
							leftWordBreak = rightWordBreak;
							rightWordBreak = textRange.Start;
							if (rightWordBreak > textMap.ContentStartOffset)
							{
								if (direction == LogicalDirection.Backward)
								{
									i++;
									break;
								}
								break;
							}
						}
					}
				}
			}
			Invariant.Assert(leftWordBreak <= textMap.ContentStartOffset && textMap.ContentStartOffset <= rightWordBreak);
			return i;
		}

		// Token: 0x06003568 RID: 13672 RVA: 0x000F281C File Offset: 0x000F0A1C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private ITextPointer SearchForWordBreaks(ITextPointer position, LogicalDirection direction, XmlLanguage language, int minWordCount, bool stopOnError)
		{
			ITextPointer textPointer = position.CreatePointer();
			ITextPointer textPointer2 = null;
			if (stopOnError)
			{
				StaticTextPointer nextErrorTransition = this._statusTable.GetNextErrorTransition(position.CreateStaticPointer(), direction);
				if (!nextErrorTransition.IsNull)
				{
					textPointer2 = nextErrorTransition.CreateDynamicTextPointer(LogicalDirection.Forward);
				}
			}
			bool flag = false;
			int num;
			do
			{
				textPointer.MoveByOffset((direction == LogicalDirection.Backward) ? -32 : 32);
				if (textPointer2 != null && ((direction == LogicalDirection.Backward && textPointer2.CompareTo(textPointer) > 0) || (direction == LogicalDirection.Forward && textPointer2.CompareTo(textPointer) < 0)))
				{
					textPointer.MoveToPosition(textPointer2);
					flag = true;
				}
				ITextPointer nextLanguageTransition = this.GetNextLanguageTransition(position, direction, language, textPointer);
				if ((direction == LogicalDirection.Backward && nextLanguageTransition.CompareTo(textPointer) > 0) || (direction == LogicalDirection.Forward && nextLanguageTransition.CompareTo(textPointer) < 0))
				{
					textPointer.MoveToPosition(nextLanguageTransition);
					flag = true;
				}
				ITextPointer textPointer3;
				ITextPointer textPointer4;
				if (direction == LogicalDirection.Backward)
				{
					textPointer3 = textPointer;
					textPointer4 = position;
				}
				else
				{
					textPointer3 = position;
					textPointer4 = textPointer;
				}
				Speller.TextMap textMap = new Speller.TextMap(textPointer3, textPointer4, textPointer3, textPointer4);
				num = this._spellerInterop.EnumTextSegments(textMap.Text, textMap.TextLength, null, null, null);
			}
			while (!flag && num < minWordCount + 1 && textPointer.GetPointerContext(direction) != TextPointerContext.None);
			return textPointer;
		}

		// Token: 0x06003569 RID: 13673 RVA: 0x000F291C File Offset: 0x000F0B1C
		private ITextPointer GetNextLanguageTransition(ITextPointer position, LogicalDirection direction, XmlLanguage language, ITextPointer haltPosition)
		{
			ITextPointer textPointer = position.CreatePointer();
			while (((direction == LogicalDirection.Forward && textPointer.CompareTo(haltPosition) < 0) || (direction == LogicalDirection.Backward && textPointer.CompareTo(haltPosition) > 0)) && this.GetCurrentLanguage(textPointer) == language)
			{
				textPointer.MoveToNextContextPosition(direction);
			}
			if ((direction == LogicalDirection.Forward && textPointer.CompareTo(haltPosition) > 0) || (direction == LogicalDirection.Backward && textPointer.CompareTo(haltPosition) < 0))
			{
				textPointer.MoveToPosition(haltPosition);
			}
			return textPointer;
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x000F2988 File Offset: 0x000F0B88
		private bool ExpandToWordBreakCallback(SpellerInteropBase.ISpellerSegment textSegment, object o)
		{
			ArrayList arrayList = (ArrayList)o;
			arrayList.Add(textSegment.TextRange);
			return true;
		}

		// Token: 0x0600356B RID: 13675 RVA: 0x000F29AC File Offset: 0x000F0BAC
		private bool IsIgnoredWord(char[] word)
		{
			bool result = false;
			if (this._ignoredWordsList != null)
			{
				result = (this._ignoredWordsList.BinarySearch(new string(word), new CaseInsensitiveComparer(this._defaultCulture)) >= 0);
			}
			return result;
		}

		// Token: 0x0600356C RID: 13676 RVA: 0x000F29E8 File Offset: 0x000F0BE8
		private static bool CanSpellCheck(CultureInfo culture)
		{
			string twoLetterISOLanguageName = culture.TwoLetterISOLanguageName;
			return twoLetterISOLanguageName == "en" || twoLetterISOLanguageName == "de" || twoLetterISOLanguageName == "fr" || twoLetterISOLanguageName == "es";
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x000F2A37 File Offset: 0x000F0C37
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void SetCulture(CultureInfo culture)
		{
			this._spellerInterop.SetLocale(culture);
			this._spellerInterop.SetReformMode(culture, this._spellingReform);
		}

		// Token: 0x0600356E RID: 13678 RVA: 0x000F2A58 File Offset: 0x000F0C58
		private void ScanPosition(ITextPointer position, LogicalDirection direction)
		{
			ITextPointer start;
			ITextPointer end;
			if (direction == LogicalDirection.Forward)
			{
				start = position;
				end = position.CreatePointer(1);
			}
			else
			{
				start = position.CreatePointer(-1);
				end = position;
			}
			this.ScanRange(start, end, long.MaxValue);
		}

		// Token: 0x0600356F RID: 13679 RVA: 0x000F2A94 File Offset: 0x000F0C94
		private XmlLanguage GetCurrentLanguage(ITextPointer position)
		{
			XmlLanguage result;
			this.GetCurrentCultureAndLanguage(position, out result);
			return result;
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x000F2AAC File Offset: 0x000F0CAC
		private CultureInfo GetCurrentCultureAndLanguage(ITextPointer position, out XmlLanguage language)
		{
			bool flag;
			CultureInfo cultureInfo;
			if (!this._textEditor.AcceptsRichContent && this._textEditor.UiScope.GetValueSource(FrameworkElement.LanguageProperty, null, out flag) == BaseValueSourceInternal.Default)
			{
				cultureInfo = this._defaultCulture;
				language = XmlLanguage.GetLanguage(cultureInfo.IetfLanguageTag);
			}
			else
			{
				language = (XmlLanguage)position.GetValue(FrameworkElement.LanguageProperty);
				if (language == null)
				{
					cultureInfo = null;
				}
				else
				{
					try
					{
						cultureInfo = language.GetSpecificCulture();
					}
					catch (InvalidOperationException)
					{
						cultureInfo = null;
					}
				}
			}
			return cultureInfo;
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x000F2B34 File Offset: 0x000F0D34
		private static Uri ResolvePathUri(Uri uri)
		{
			Uri result;
			if (!uri.IsAbsoluteUri)
			{
				result = new Uri(new Uri(Directory.GetCurrentDirectory() + "/"), uri);
			}
			else
			{
				result = uri;
			}
			return result;
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x000F2B6C File Offset: 0x000F0D6C
		[SecurityCritical]
		private void LoadDictionaryFromPackUri(Uri item)
		{
			new EnvironmentPermission(PermissionState.Unrestricted).Assert();
			Uri uri;
			string tempPath;
			try
			{
				uri = Speller.LoadPackFile(item);
				tempPath = Path.GetTempPath();
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			try
			{
				object lexicon = this._spellerInterop.LoadDictionary(uri, tempPath);
				this.UriMap.Add(item, new Speller.DictionaryInfo(uri, lexicon));
			}
			finally
			{
				this.CleanupDictionaryTempFile(uri);
			}
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x000F2BE4 File Offset: 0x000F0DE4
		[SecurityCritical]
		private void CleanupDictionaryTempFile(Uri tempLocationUri)
		{
			if (tempLocationUri != null)
			{
				new FileIOPermission(PermissionState.Unrestricted).Assert();
				try
				{
					File.Delete(tempLocationUri.LocalPath);
				}
				catch (Exception ex)
				{
					if (SecurityHelper.CheckUnmanagedCodePermission())
					{
						Trace.Write(string.Format(CultureInfo.InvariantCulture, "Failure to delete temporary file with custom dictionary data. file Uri:{0},exception:{1}", new object[]
						{
							tempLocationUri.ToString(),
							ex.ToString()
						}));
					}
					throw;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x000F2C6C File Offset: 0x000F0E6C
		[SecurityCritical]
		private static Uri LoadPackFile(Uri uri)
		{
			Invariant.Assert(PackUriHelper.IsPackUri(uri));
			Uri resolvedUri = BindUriHelper.GetResolvedUri(BaseUriHelper.PackAppBaseUri, uri);
			string uriString;
			using (Stream stream = WpfWebRequestHelper.CreateRequestAndGetResponseStream(resolvedUri))
			{
				using (FileStream fileStream = FileHelper.CreateAndOpenTemporaryFile(out uriString, FileAccess.ReadWrite, FileOptions.None, null, "WPF"))
				{
					stream.CopyTo(fileStream);
				}
			}
			return new Uri(uriString);
		}

		// Token: 0x0400250A RID: 9482
		private const int MaxIdleTimeSliceMs = 20;

		// Token: 0x0400250B RID: 9483
		private const long MaxIdleTimeSliceNs = 200000L;

		// Token: 0x0400250C RID: 9484
		private const int MaxScanBlockSize = 64;

		// Token: 0x0400250D RID: 9485
		private const int ContextBlockSize = 32;

		// Token: 0x0400250E RID: 9486
		private const int MinWordBreaksForContext = 4;

		// Token: 0x0400250F RID: 9487
		private TextEditor _textEditor;

		// Token: 0x04002510 RID: 9488
		private SpellerStatusTable _statusTable;

		// Token: 0x04002511 RID: 9489
		private SpellerHighlightLayer _highlightLayer;

		// Token: 0x04002512 RID: 9490
		[SecurityCritical]
		private SpellerInteropBase _spellerInterop;

		// Token: 0x04002513 RID: 9491
		private SpellingReform _spellingReform;

		// Token: 0x04002514 RID: 9492
		private bool _pendingIdleCallback;

		// Token: 0x04002515 RID: 9493
		private bool _pendingCaretMovedCallback;

		// Token: 0x04002516 RID: 9494
		private ArrayList _ignoredWordsList;

		// Token: 0x04002517 RID: 9495
		private readonly CultureInfo _defaultCulture;

		// Token: 0x04002518 RID: 9496
		private bool _failedToInit;

		// Token: 0x04002519 RID: 9497
		[SecurityCritical]
		private Dictionary<Uri, Speller.DictionaryInfo> _uriMap;

		// Token: 0x020008DE RID: 2270
		private class TextMap
		{
			// Token: 0x060084A8 RID: 33960 RVA: 0x002492D8 File Offset: 0x002474D8
			internal TextMap(ITextPointer contextStart, ITextPointer contextEnd, ITextPointer contentStart, ITextPointer contentEnd)
			{
				Invariant.Assert(contextStart.CompareTo(contentStart) <= 0);
				Invariant.Assert(contextEnd.CompareTo(contentEnd) >= 0);
				this._basePosition = contextStart.GetFrozenPointer(LogicalDirection.Backward);
				ITextPointer textPointer = contextStart.CreatePointer();
				int offsetToPosition = contextStart.GetOffsetToPosition(contextEnd);
				this._text = new char[offsetToPosition];
				this._positionMap = new int[offsetToPosition + 1];
				this._textLength = 0;
				int num = 0;
				this._contentStartOffset = 0;
				this._contentEndOffset = 0;
				while (textPointer.CompareTo(contextEnd) < 0)
				{
					if (textPointer.CompareTo(contentStart) == 0)
					{
						this._contentStartOffset = this._textLength;
					}
					if (textPointer.CompareTo(contentEnd) == 0)
					{
						this._contentEndOffset = this._textLength;
					}
					switch (textPointer.GetPointerContext(LogicalDirection.Forward))
					{
					case TextPointerContext.Text:
					{
						int num2 = textPointer.GetTextRunLength(LogicalDirection.Forward);
						num2 = Math.Min(num2, this._text.Length - this._textLength);
						num2 = Math.Min(num2, textPointer.GetOffsetToPosition(contextEnd));
						textPointer.GetTextInRun(LogicalDirection.Forward, this._text, this._textLength, num2);
						for (int i = this._textLength; i < this._textLength + num2; i++)
						{
							this._positionMap[i] = i + num;
						}
						int offsetToPosition2 = textPointer.GetOffsetToPosition(contentStart);
						if (offsetToPosition2 >= 0 && offsetToPosition2 <= num2)
						{
							this._contentStartOffset = this._textLength + textPointer.GetOffsetToPosition(contentStart);
						}
						offsetToPosition2 = textPointer.GetOffsetToPosition(contentEnd);
						if (offsetToPosition2 >= 0 && offsetToPosition2 <= num2)
						{
							this._contentEndOffset = this._textLength + textPointer.GetOffsetToPosition(contentEnd);
						}
						textPointer.MoveByOffset(num2);
						this._textLength += num2;
						break;
					}
					case TextPointerContext.EmbeddedElement:
						this._text[this._textLength] = '';
						this._positionMap[this._textLength] = this._textLength + num;
						this._textLength++;
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
						break;
					case TextPointerContext.ElementStart:
					case TextPointerContext.ElementEnd:
						if (this.IsAdjacentToFormatElement(textPointer))
						{
							num++;
						}
						else
						{
							this._text[this._textLength] = ' ';
							this._positionMap[this._textLength] = this._textLength + num;
							this._textLength++;
						}
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
						break;
					}
				}
				if (textPointer.CompareTo(contentEnd) == 0)
				{
					this._contentEndOffset = this._textLength;
				}
				if (this._textLength > 0)
				{
					this._positionMap[this._textLength] = this._positionMap[this._textLength - 1] + 1;
				}
				else
				{
					this._positionMap[0] = 0;
				}
				Invariant.Assert(this._contentStartOffset <= this._contentEndOffset);
			}

			// Token: 0x060084A9 RID: 33961 RVA: 0x0024957C File Offset: 0x0024777C
			internal ITextPointer MapOffsetToPosition(int offset)
			{
				Invariant.Assert(offset >= 0 && offset <= this._textLength);
				return this._basePosition.CreatePointer(this._positionMap[offset]);
			}

			// Token: 0x17001E09 RID: 7689
			// (get) Token: 0x060084AA RID: 33962 RVA: 0x002495A9 File Offset: 0x002477A9
			internal int ContentStartOffset
			{
				get
				{
					return this._contentStartOffset;
				}
			}

			// Token: 0x17001E0A RID: 7690
			// (get) Token: 0x060084AB RID: 33963 RVA: 0x002495B1 File Offset: 0x002477B1
			internal int ContentEndOffset
			{
				get
				{
					return this._contentEndOffset;
				}
			}

			// Token: 0x17001E0B RID: 7691
			// (get) Token: 0x060084AC RID: 33964 RVA: 0x002495B9 File Offset: 0x002477B9
			internal char[] Text
			{
				get
				{
					return this._text;
				}
			}

			// Token: 0x17001E0C RID: 7692
			// (get) Token: 0x060084AD RID: 33965 RVA: 0x002495C1 File Offset: 0x002477C1
			internal int TextLength
			{
				get
				{
					return this._textLength;
				}
			}

			// Token: 0x060084AE RID: 33966 RVA: 0x002495CC File Offset: 0x002477CC
			private bool IsAdjacentToFormatElement(ITextPointer pointer)
			{
				bool result = false;
				TextPointerContext pointerContext = pointer.GetPointerContext(LogicalDirection.Forward);
				if (pointerContext == TextPointerContext.ElementStart && TextSchema.IsFormattingType(pointer.GetElementType(LogicalDirection.Forward)))
				{
					result = true;
				}
				else if (pointerContext == TextPointerContext.ElementEnd && TextSchema.IsFormattingType(pointer.ParentType))
				{
					result = true;
				}
				return result;
			}

			// Token: 0x0400429B RID: 17051
			private readonly ITextPointer _basePosition;

			// Token: 0x0400429C RID: 17052
			private readonly char[] _text;

			// Token: 0x0400429D RID: 17053
			private readonly int[] _positionMap;

			// Token: 0x0400429E RID: 17054
			private readonly int _textLength;

			// Token: 0x0400429F RID: 17055
			private readonly int _contentStartOffset;

			// Token: 0x040042A0 RID: 17056
			private readonly int _contentEndOffset;
		}

		// Token: 0x020008DF RID: 2271
		private class ScanStatus
		{
			// Token: 0x060084AF RID: 33967 RVA: 0x0024960D File Offset: 0x0024780D
			internal ScanStatus(long timeLimit, ITextPointer startPosition)
			{
				this._timeLimit = timeLimit;
				this._startPosition = startPosition;
			}

			// Token: 0x17001E0D RID: 7693
			// (get) Token: 0x060084B0 RID: 33968 RVA: 0x00249624 File Offset: 0x00247824
			internal bool HasExceededTimeLimit
			{
				get
				{
					long ticks = DateTime.Now.Ticks;
					return ticks >= this._timeLimit;
				}
			}

			// Token: 0x17001E0E RID: 7694
			// (get) Token: 0x060084B1 RID: 33969 RVA: 0x0024964B File Offset: 0x0024784B
			// (set) Token: 0x060084B2 RID: 33970 RVA: 0x00249653 File Offset: 0x00247853
			internal ITextPointer TimeoutPosition
			{
				get
				{
					return this._timeoutPosition;
				}
				set
				{
					this._timeoutPosition = value;
				}
			}

			// Token: 0x17001E0F RID: 7695
			// (get) Token: 0x060084B3 RID: 33971 RVA: 0x0024965C File Offset: 0x0024785C
			internal ITextPointer StartPosition
			{
				get
				{
					return this._startPosition;
				}
			}

			// Token: 0x040042A1 RID: 17057
			private readonly long _timeLimit;

			// Token: 0x040042A2 RID: 17058
			private readonly ITextPointer _startPosition;

			// Token: 0x040042A3 RID: 17059
			private ITextPointer _timeoutPosition;
		}

		// Token: 0x020008E0 RID: 2272
		private class TextMapCallbackData
		{
			// Token: 0x060084B4 RID: 33972 RVA: 0x00249664 File Offset: 0x00247864
			internal TextMapCallbackData(Speller.TextMap textmap, object data)
			{
				this._textmap = textmap;
				this._data = data;
			}

			// Token: 0x17001E10 RID: 7696
			// (get) Token: 0x060084B5 RID: 33973 RVA: 0x0024967A File Offset: 0x0024787A
			internal Speller.TextMap TextMap
			{
				get
				{
					return this._textmap;
				}
			}

			// Token: 0x17001E11 RID: 7697
			// (get) Token: 0x060084B6 RID: 33974 RVA: 0x00249682 File Offset: 0x00247882
			internal object Data
			{
				get
				{
					return this._data;
				}
			}

			// Token: 0x040042A4 RID: 17060
			private readonly Speller.TextMap _textmap;

			// Token: 0x040042A5 RID: 17061
			private readonly object _data;
		}

		// Token: 0x020008E1 RID: 2273
		private class DictionaryInfo
		{
			// Token: 0x060084B7 RID: 33975 RVA: 0x0024968A File Offset: 0x0024788A
			[SecurityCritical]
			internal DictionaryInfo(Uri pathUri, object lexicon)
			{
				this._pathUri = pathUri;
				this._lexicon = lexicon;
			}

			// Token: 0x17001E12 RID: 7698
			// (get) Token: 0x060084B8 RID: 33976 RVA: 0x002496A0 File Offset: 0x002478A0
			internal Uri PathUri
			{
				[SecurityCritical]
				get
				{
					return this._pathUri;
				}
			}

			// Token: 0x17001E13 RID: 7699
			// (get) Token: 0x060084B9 RID: 33977 RVA: 0x002496A8 File Offset: 0x002478A8
			internal object Lexicon
			{
				[SecurityCritical]
				get
				{
					return this._lexicon;
				}
			}

			// Token: 0x040042A6 RID: 17062
			[SecurityCritical]
			private readonly object _lexicon;

			// Token: 0x040042A7 RID: 17063
			[SecurityCritical]
			private readonly Uri _pathUri;
		}

		// Token: 0x020008E2 RID: 2274
		private class TextMapOffsetErrorLogger
		{
			// Token: 0x060084BA RID: 33978 RVA: 0x002496B0 File Offset: 0x002478B0
			public TextMapOffsetErrorLogger(LogicalDirection direction, Speller.TextMap textMap, ArrayList segments, int positionInSegmentList, int leftWordBreak, int rightWordBreak, int contentOffset)
			{
				this._debugInfo = new Speller.TextMapOffsetErrorLogger.DebugInfo
				{
					Direction = direction.ToString(),
					SegmentCount = segments.Count,
					SegmentStartsAndLengths = new Speller.TextMapOffsetErrorLogger.SegmentInfo[segments.Count],
					PositionInSegmentList = positionInSegmentList,
					LeftWordBreak = leftWordBreak,
					RightWordBreak = rightWordBreak,
					ContentOffSet = contentOffset,
					ContextOffset = Speller.TextMapOffsetErrorLogger.UnsetValue,
					CalculationMode = Speller.TextMapOffsetErrorLogger.CalculationModes.ContentPosition,
					TextMapText = string.Join<char>(string.Empty, textMap.Text),
					TextMapTextLength = textMap.TextLength,
					TextMapContentStartOffset = textMap.ContentStartOffset,
					TextMapContentEndOffset = textMap.ContentEndOffset
				};
				for (int i = 0; i < segments.Count; i++)
				{
					SpellerInteropBase.ITextRange textRange = segments[i] as SpellerInteropBase.ITextRange;
					if (textRange != null)
					{
						this._debugInfo.SegmentStartsAndLengths[i] = new Speller.TextMapOffsetErrorLogger.SegmentInfo
						{
							Start = textRange.Start,
							Length = textRange.Length
						};
					}
				}
			}

			// Token: 0x17001E14 RID: 7700
			// (set) Token: 0x060084BB RID: 33979 RVA: 0x002497D0 File Offset: 0x002479D0
			public int ContextOffset
			{
				set
				{
					this._debugInfo.ContextOffset = value;
					this._debugInfo.CalculationMode = Speller.TextMapOffsetErrorLogger.CalculationModes.ContextPosition;
				}
			}

			// Token: 0x060084BC RID: 33980 RVA: 0x002497EC File Offset: 0x002479EC
			public void LogDebugInfo()
			{
				int num = (this._debugInfo.CalculationMode == Speller.TextMapOffsetErrorLogger.CalculationModes.ContentPosition) ? this._debugInfo.ContentOffSet : this._debugInfo.ContextOffset;
				if (num < 0 || num > this._debugInfo.TextMapTextLength)
				{
					EventSource provider = TraceLoggingProvider.GetProvider();
					EventSourceOptions options = new EventSourceOptions
					{
						Keywords = (EventKeywords)70368744177664L,
						Tags = (EventTags)33554432
					};
					if (provider != null)
					{
						provider.Write<Speller.TextMapOffsetErrorLogger.DebugInfo>(Speller.TextMapOffsetErrorLogger.TextMapOffsetError, options, this._debugInfo);
					}
				}
			}

			// Token: 0x040042A8 RID: 17064
			private static readonly string TextMapOffsetError = "TextMapOffsetError";

			// Token: 0x040042A9 RID: 17065
			private Speller.TextMapOffsetErrorLogger.DebugInfo _debugInfo;

			// Token: 0x040042AA RID: 17066
			private static readonly int UnsetValue = -2;

			// Token: 0x02000BA5 RID: 2981
			public enum CalculationModes
			{
				// Token: 0x04004EBE RID: 20158
				ContentPosition,
				// Token: 0x04004EBF RID: 20159
				ContextPosition
			}

			// Token: 0x02000BA6 RID: 2982
			[EventData]
			private struct DebugInfo
			{
				// Token: 0x17001FC2 RID: 8130
				// (get) Token: 0x060091AD RID: 37293 RVA: 0x0025F03A File Offset: 0x0025D23A
				// (set) Token: 0x060091AE RID: 37294 RVA: 0x0025F042 File Offset: 0x0025D242
				public string Direction { get; set; }

				// Token: 0x17001FC3 RID: 8131
				// (get) Token: 0x060091AF RID: 37295 RVA: 0x0025F04B File Offset: 0x0025D24B
				// (set) Token: 0x060091B0 RID: 37296 RVA: 0x0025F053 File Offset: 0x0025D253
				public int SegmentCount { get; set; }

				// Token: 0x17001FC4 RID: 8132
				// (get) Token: 0x060091B1 RID: 37297 RVA: 0x0025F05C File Offset: 0x0025D25C
				// (set) Token: 0x060091B2 RID: 37298 RVA: 0x0025F064 File Offset: 0x0025D264
				public Speller.TextMapOffsetErrorLogger.SegmentInfo[] SegmentStartsAndLengths { get; set; }

				// Token: 0x17001FC5 RID: 8133
				// (get) Token: 0x060091B3 RID: 37299 RVA: 0x0025F06D File Offset: 0x0025D26D
				// (set) Token: 0x060091B4 RID: 37300 RVA: 0x0025F075 File Offset: 0x0025D275
				public int PositionInSegmentList { get; set; }

				// Token: 0x17001FC6 RID: 8134
				// (get) Token: 0x060091B5 RID: 37301 RVA: 0x0025F07E File Offset: 0x0025D27E
				// (set) Token: 0x060091B6 RID: 37302 RVA: 0x0025F086 File Offset: 0x0025D286
				public int LeftWordBreak { get; set; }

				// Token: 0x17001FC7 RID: 8135
				// (get) Token: 0x060091B7 RID: 37303 RVA: 0x0025F08F File Offset: 0x0025D28F
				// (set) Token: 0x060091B8 RID: 37304 RVA: 0x0025F097 File Offset: 0x0025D297
				public int RightWordBreak { get; set; }

				// Token: 0x17001FC8 RID: 8136
				// (get) Token: 0x060091B9 RID: 37305 RVA: 0x0025F0A0 File Offset: 0x0025D2A0
				// (set) Token: 0x060091BA RID: 37306 RVA: 0x0025F0A8 File Offset: 0x0025D2A8
				public int ContentOffSet { get; set; }

				// Token: 0x17001FC9 RID: 8137
				// (get) Token: 0x060091BB RID: 37307 RVA: 0x0025F0B1 File Offset: 0x0025D2B1
				// (set) Token: 0x060091BC RID: 37308 RVA: 0x0025F0B9 File Offset: 0x0025D2B9
				public int ContextOffset { get; set; }

				// Token: 0x17001FCA RID: 8138
				// (get) Token: 0x060091BD RID: 37309 RVA: 0x0025F0C2 File Offset: 0x0025D2C2
				// (set) Token: 0x060091BE RID: 37310 RVA: 0x0025F0CA File Offset: 0x0025D2CA
				public Speller.TextMapOffsetErrorLogger.CalculationModes CalculationMode { get; set; }

				// Token: 0x17001FCB RID: 8139
				// (get) Token: 0x060091BF RID: 37311 RVA: 0x0025F0D3 File Offset: 0x0025D2D3
				// (set) Token: 0x060091C0 RID: 37312 RVA: 0x0025F0DB File Offset: 0x0025D2DB
				public string TextMapText { get; set; }

				// Token: 0x17001FCC RID: 8140
				// (get) Token: 0x060091C1 RID: 37313 RVA: 0x0025F0E4 File Offset: 0x0025D2E4
				// (set) Token: 0x060091C2 RID: 37314 RVA: 0x0025F0EC File Offset: 0x0025D2EC
				public int TextMapTextLength { get; set; }

				// Token: 0x17001FCD RID: 8141
				// (get) Token: 0x060091C3 RID: 37315 RVA: 0x0025F0F5 File Offset: 0x0025D2F5
				// (set) Token: 0x060091C4 RID: 37316 RVA: 0x0025F0FD File Offset: 0x0025D2FD
				public int TextMapContentStartOffset { get; set; }

				// Token: 0x17001FCE RID: 8142
				// (get) Token: 0x060091C5 RID: 37317 RVA: 0x0025F106 File Offset: 0x0025D306
				// (set) Token: 0x060091C6 RID: 37318 RVA: 0x0025F10E File Offset: 0x0025D30E
				public int TextMapContentEndOffset { get; set; }
			}

			// Token: 0x02000BA7 RID: 2983
			[EventData]
			private struct SegmentInfo
			{
				// Token: 0x17001FCF RID: 8143
				// (get) Token: 0x060091C7 RID: 37319 RVA: 0x0025F117 File Offset: 0x0025D317
				// (set) Token: 0x060091C8 RID: 37320 RVA: 0x0025F11F File Offset: 0x0025D31F
				public int Start { get; set; }

				// Token: 0x17001FD0 RID: 8144
				// (get) Token: 0x060091C9 RID: 37321 RVA: 0x0025F128 File Offset: 0x0025D328
				// (set) Token: 0x060091CA RID: 37322 RVA: 0x0025F130 File Offset: 0x0025D330
				public int Length { get; set; }
			}
		}
	}
}
