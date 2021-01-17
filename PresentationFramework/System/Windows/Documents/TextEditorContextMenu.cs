using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020003F3 RID: 1011
	internal static class TextEditorContextMenu
	{
		// Token: 0x06003846 RID: 14406 RVA: 0x000FB0EB File Offset: 0x000F92EB
		internal static void _RegisterClassHandlers(Type controlType, bool registerEventListeners)
		{
			if (registerEventListeners)
			{
				EventManager.RegisterClassHandler(controlType, FrameworkElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(TextEditorContextMenu.OnContextMenuOpening));
			}
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x000FB108 File Offset: 0x000F9308
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || textEditor.TextView == null)
			{
				return;
			}
			Point position = Mouse.GetPosition(textEditor.TextView.RenderScope);
			ContextMenu contextMenu = null;
			bool flag = false;
			if (textEditor.IsReadOnly)
			{
				if ((e.CursorLeft != -1.0 && !textEditor.Selection.Contains(position)) || (e.CursorLeft == -1.0 && textEditor.Selection.IsEmpty))
				{
					return;
				}
			}
			else if ((textEditor.Selection.IsEmpty || e.TargetElement is TextElement) && e.TargetElement != null)
			{
				contextMenu = (ContextMenu)e.TargetElement.GetValue(FrameworkElement.ContextMenuProperty);
			}
			else if (e.CursorLeft == -1.0)
			{
				TextPointer textPointer = TextEditorContextMenu.GetContentPosition(textEditor.Selection.Start) as TextPointer;
				if (textPointer != null)
				{
					for (TextElement textElement = textPointer.Parent as TextElement; textElement != null; textElement = (textElement.Parent as TextElement))
					{
						contextMenu = (ContextMenu)textElement.GetValue(FrameworkElement.ContextMenuProperty);
						if (contextMenu != null)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (e.CursorLeft != -1.0)
			{
				if (!TextEditorMouse.IsPointWithinInteractiveArea(textEditor, Mouse.GetPosition(textEditor.UiScope)))
				{
					return;
				}
				if (contextMenu == null || !(e.TargetElement is UIElement))
				{
					using (textEditor.Selection.DeclareChangeBlock())
					{
						if (!textEditor.Selection.Contains(position))
						{
							TextEditorMouse.SetCaretPositionOnMouseEvent(textEditor, position, MouseButton.Right, 1);
						}
					}
				}
			}
			if (contextMenu == null)
			{
				if (textEditor.UiScope.ReadLocalValue(FrameworkElement.ContextMenuProperty) == null)
				{
					return;
				}
				contextMenu = textEditor.UiScope.ContextMenu;
			}
			textEditor.IsContextMenuOpen = true;
			if (contextMenu != null && !flag)
			{
				contextMenu.HorizontalOffset = 0.0;
				contextMenu.VerticalOffset = 0.0;
				contextMenu.Closed += TextEditorContextMenu.OnContextMenuClosed;
				return;
			}
			textEditor.CompleteComposition();
			if (contextMenu == null)
			{
				contextMenu = new TextEditorContextMenu.EditorContextMenu();
				((TextEditorContextMenu.EditorContextMenu)contextMenu).AddMenuItems(textEditor, e.UserInitiated);
			}
			contextMenu.Placement = PlacementMode.RelativePoint;
			contextMenu.PlacementTarget = textEditor.UiScope;
			ITextPointer textPointer2 = null;
			SpellingError spellingError = (contextMenu is TextEditorContextMenu.EditorContextMenu) ? textEditor.GetSpellingErrorAtSelection() : null;
			LogicalDirection logicalDirection;
			if (spellingError != null)
			{
				textPointer2 = spellingError.End;
				logicalDirection = LogicalDirection.Backward;
			}
			else if (e.CursorLeft == -1.0)
			{
				textPointer2 = textEditor.Selection.Start;
				logicalDirection = LogicalDirection.Forward;
			}
			else
			{
				logicalDirection = LogicalDirection.Forward;
			}
			if (textPointer2 != null && textPointer2.CreatePointer(logicalDirection).HasValidLayout)
			{
				double horizontalOffset;
				double verticalOffset;
				TextEditorContextMenu.GetClippedPositionOffsets(textEditor, textPointer2, logicalDirection, out horizontalOffset, out verticalOffset);
				contextMenu.HorizontalOffset = horizontalOffset;
				contextMenu.VerticalOffset = verticalOffset;
			}
			else
			{
				Point position2 = Mouse.GetPosition(textEditor.UiScope);
				contextMenu.HorizontalOffset = position2.X;
				contextMenu.VerticalOffset = position2.Y;
			}
			contextMenu.Closed += TextEditorContextMenu.OnContextMenuClosed;
			contextMenu.IsOpen = true;
			e.Handled = true;
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x000FB400 File Offset: 0x000F9600
		private static void OnContextMenuClosed(object sender, RoutedEventArgs e)
		{
			UIElement placementTarget = ((ContextMenu)sender).PlacementTarget;
			if (placementTarget != null)
			{
				TextEditor textEditor = TextEditor._GetTextEditor(placementTarget);
				if (textEditor != null)
				{
					textEditor.IsContextMenuOpen = false;
					textEditor.Selection.UpdateCaretAndHighlight();
					((ContextMenu)sender).Closed -= TextEditorContextMenu.OnContextMenuClosed;
				}
			}
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x000FB450 File Offset: 0x000F9650
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void GetClippedPositionOffsets(TextEditor This, ITextPointer position, LogicalDirection direction, out double horizontalOffset, out double verticalOffset)
		{
			Rect characterRect = position.GetCharacterRect(direction);
			horizontalOffset = characterRect.X;
			verticalOffset = characterRect.Y + characterRect.Height;
			FrameworkElement frameworkElement = This.TextView.RenderScope as FrameworkElement;
			if (frameworkElement != null)
			{
				GeneralTransform generalTransform = frameworkElement.TransformToAncestor(This.UiScope);
				if (generalTransform != null)
				{
					TextEditorContextMenu.ClipToElement(frameworkElement, generalTransform, ref horizontalOffset, ref verticalOffset);
				}
			}
			for (Visual visual = This.UiScope; visual != null; visual = (VisualTreeHelper.GetParent(visual) as Visual))
			{
				frameworkElement = (visual as FrameworkElement);
				if (frameworkElement != null)
				{
					GeneralTransform generalTransform2 = visual.TransformToDescendant(This.UiScope);
					if (generalTransform2 != null)
					{
						TextEditorContextMenu.ClipToElement(frameworkElement, generalTransform2, ref horizontalOffset, ref verticalOffset);
					}
				}
			}
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(This.UiScope);
			IWin32Window win32Window = presentationSource as IWin32Window;
			if (win32Window != null)
			{
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
				NativeMethods.RECT rect = new NativeMethods.RECT(0, 0, 0, 0);
				SafeNativeMethods.GetClientRect(new HandleRef(null, handle), ref rect);
				Point point = new Point((double)rect.left, (double)rect.top);
				Point point2 = new Point((double)rect.right, (double)rect.bottom);
				CompositionTarget compositionTarget = presentationSource.CompositionTarget;
				point = compositionTarget.TransformFromDevice.Transform(point);
				point2 = compositionTarget.TransformFromDevice.Transform(point2);
				GeneralTransform generalTransform3 = compositionTarget.RootVisual.TransformToDescendant(This.UiScope);
				if (generalTransform3 != null)
				{
					generalTransform3.TryTransform(point, out point);
					generalTransform3.TryTransform(point2, out point2);
					horizontalOffset = TextEditorContextMenu.ClipToBounds(point.X, horizontalOffset, point2.X);
					verticalOffset = TextEditorContextMenu.ClipToBounds(point.Y, verticalOffset, point2.Y);
				}
			}
		}

		// Token: 0x0600384A RID: 14410 RVA: 0x000FB61C File Offset: 0x000F981C
		private static void ClipToElement(FrameworkElement element, GeneralTransform transform, ref double horizontalOffset, ref double verticalOffset)
		{
			Geometry clip = VisualTreeHelper.GetClip(element);
			Point inPoint;
			Point inPoint2;
			if (clip != null)
			{
				Rect bounds = clip.Bounds;
				inPoint = new Point(bounds.X, bounds.Y);
				inPoint2 = new Point(bounds.X + bounds.Width, bounds.Y + bounds.Height);
			}
			else
			{
				if (element.ActualWidth == 0.0 && element.ActualHeight == 0.0)
				{
					return;
				}
				inPoint = new Point(0.0, 0.0);
				inPoint2 = new Point(element.ActualWidth, element.ActualHeight);
			}
			transform.TryTransform(inPoint, out inPoint);
			transform.TryTransform(inPoint2, out inPoint2);
			horizontalOffset = TextEditorContextMenu.ClipToBounds(inPoint.X, horizontalOffset, inPoint2.X);
			verticalOffset = TextEditorContextMenu.ClipToBounds(inPoint.Y, verticalOffset, inPoint2.Y);
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x000FB708 File Offset: 0x000F9908
		private static double ClipToBounds(double min, double value, double max)
		{
			if (min > max)
			{
				double num = min;
				min = max;
				max = num;
			}
			if (value < min)
			{
				value = min;
			}
			else if (value >= max)
			{
				value = max - 1.0;
			}
			return value;
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x000FB73C File Offset: 0x000F993C
		private static ITextPointer GetContentPosition(ITextPointer position)
		{
			while (position.GetAdjacentElement(LogicalDirection.Forward) is Inline)
			{
				position = position.GetNextContextPosition(LogicalDirection.Forward);
			}
			return position;
		}

		// Token: 0x020008FD RID: 2301
		private class EditorContextMenu : ContextMenu
		{
			// Token: 0x060085A4 RID: 34212 RVA: 0x0024A05F File Offset: 0x0024825F
			[SecurityCritical]
			internal void AddMenuItems(TextEditor textEditor, bool userInitiated)
			{
				if (!userInitiated)
				{
					SecurityHelper.DemandAllClipboardPermission();
				}
				if (!textEditor.IsReadOnly && this.AddReconversionItems(textEditor))
				{
					this.AddSeparator();
				}
				if (this.AddSpellerItems(textEditor))
				{
					this.AddSeparator();
				}
				this.AddClipboardItems(textEditor, userInitiated);
			}

			// Token: 0x060085A5 RID: 34213 RVA: 0x0024A098 File Offset: 0x00248298
			~EditorContextMenu()
			{
				this.ReleaseCandidateList(null);
			}

			// Token: 0x060085A6 RID: 34214 RVA: 0x0024A0C8 File Offset: 0x002482C8
			protected override void OnClosed(RoutedEventArgs e)
			{
				base.OnClosed(e);
				this.DelayReleaseCandidateList();
			}

			// Token: 0x060085A7 RID: 34215 RVA: 0x0024A0D7 File Offset: 0x002482D7
			private void AddSeparator()
			{
				base.Items.Add(new Separator());
			}

			// Token: 0x060085A8 RID: 34216 RVA: 0x0024A0EC File Offset: 0x002482EC
			private bool AddSpellerItems(TextEditor textEditor)
			{
				SpellingError spellingErrorAtSelection = textEditor.GetSpellingErrorAtSelection();
				if (spellingErrorAtSelection == null)
				{
					return false;
				}
				bool flag = false;
				MenuItem menuItem;
				foreach (string text in spellingErrorAtSelection.Suggestions)
				{
					menuItem = new TextEditorContextMenu.EditorMenuItem();
					menuItem.Header = new TextBlock
					{
						FontWeight = FontWeights.Bold,
						Text = text
					};
					menuItem.Command = EditingCommands.CorrectSpellingError;
					menuItem.CommandParameter = text;
					base.Items.Add(menuItem);
					menuItem.CommandTarget = textEditor.UiScope;
					flag = true;
				}
				if (!flag)
				{
					menuItem = new TextEditorContextMenu.EditorMenuItem();
					menuItem.Header = SR.Get("TextBox_ContextMenu_NoSpellingSuggestions");
					menuItem.IsEnabled = false;
					base.Items.Add(menuItem);
				}
				this.AddSeparator();
				menuItem = new TextEditorContextMenu.EditorMenuItem();
				menuItem.Header = SR.Get("TextBox_ContextMenu_IgnoreAll");
				menuItem.Command = EditingCommands.IgnoreSpellingError;
				base.Items.Add(menuItem);
				menuItem.CommandTarget = textEditor.UiScope;
				return true;
			}

			// Token: 0x060085A9 RID: 34217 RVA: 0x0024A208 File Offset: 0x00248408
			private string GetMenuItemDescription(string suggestion)
			{
				if (suggestion.Length == 1)
				{
					if (suggestion[0] == ' ')
					{
						return SR.Get("TextBox_ContextMenu_Description_SBCSSpace");
					}
					if (suggestion[0] == '\u3000')
					{
						return SR.Get("TextBox_ContextMenu_Description_DBCSSpace");
					}
				}
				return null;
			}

			// Token: 0x060085AA RID: 34218 RVA: 0x0024A244 File Offset: 0x00248444
			[SecurityCritical]
			[SecurityTreatAsSafe]
			private bool AddReconversionItems(TextEditor textEditor)
			{
				TextStore textStore = textEditor.TextStore;
				if (textStore == null)
				{
					GC.SuppressFinalize(this);
					return false;
				}
				this.ReleaseCandidateList(null);
				this._candidateList = new SecurityCriticalDataClass<UnsafeNativeMethods.ITfCandidateList>(textStore.GetReconversionCandidateList());
				if (this.CandidateList == null)
				{
					GC.SuppressFinalize(this);
					return false;
				}
				int num = 0;
				this.CandidateList.GetCandidateNum(out num);
				if (num > 0)
				{
					int num2 = 0;
					while (num2 < 5 && num2 < num)
					{
						UnsafeNativeMethods.ITfCandidateString tfCandidateString;
						this.CandidateList.GetCandidate(num2, out tfCandidateString);
						string text;
						tfCandidateString.GetString(out text);
						MenuItem menuItem = new TextEditorContextMenu.ReconversionMenuItem(this, num2);
						menuItem.Header = text;
						menuItem.InputGestureText = this.GetMenuItemDescription(text);
						base.Items.Add(menuItem);
						Marshal.ReleaseComObject(tfCandidateString);
						num2++;
					}
				}
				if (num > 5)
				{
					MenuItem menuItem = new TextEditorContextMenu.EditorMenuItem();
					menuItem.Header = SR.Get("TextBox_ContextMenu_More");
					menuItem.Command = ApplicationCommands.CorrectionList;
					base.Items.Add(menuItem);
					menuItem.CommandTarget = textEditor.UiScope;
				}
				return num > 0;
			}

			// Token: 0x060085AB RID: 34219 RVA: 0x0024A340 File Offset: 0x00248540
			[SecurityCritical]
			private bool AddClipboardItems(TextEditor textEditor, bool userInitiated)
			{
				MenuItem menuItem = new TextEditorContextMenu.EditorMenuItem();
				menuItem.Header = SR.Get("TextBox_ContextMenu_Cut");
				menuItem.CommandTarget = textEditor.UiScope;
				menuItem.Command = ApplicationCommands.Cut;
				base.Items.Add(menuItem);
				menuItem = new TextEditorContextMenu.EditorMenuItem();
				menuItem.Header = SR.Get("TextBox_ContextMenu_Copy");
				menuItem.CommandTarget = textEditor.UiScope;
				menuItem.Command = ApplicationCommands.Copy;
				base.Items.Add(menuItem);
				if (!userInitiated)
				{
					SecurityHelper.DemandAllClipboardPermission();
				}
				menuItem = new TextEditorContextMenu.EditorMenuItem();
				menuItem.Header = SR.Get("TextBox_ContextMenu_Paste");
				menuItem.CommandTarget = textEditor.UiScope;
				menuItem.Command = ApplicationCommands.Paste;
				base.Items.Add(menuItem);
				return true;
			}

			// Token: 0x060085AC RID: 34220 RVA: 0x0024A404 File Offset: 0x00248604
			[SecurityCritical]
			[SecurityTreatAsSafe]
			private void DelayReleaseCandidateList()
			{
				if (this.CandidateList != null)
				{
					Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.ReleaseCandidateList), null);
				}
			}

			// Token: 0x060085AD RID: 34221 RVA: 0x0024A427 File Offset: 0x00248627
			[SecurityCritical]
			[SecurityTreatAsSafe]
			private object ReleaseCandidateList(object o)
			{
				if (this.CandidateList != null)
				{
					Marshal.ReleaseComObject(this.CandidateList);
					this._candidateList = null;
					GC.SuppressFinalize(this);
				}
				return null;
			}

			// Token: 0x17001E31 RID: 7729
			// (get) Token: 0x060085AE RID: 34222 RVA: 0x0024A44B File Offset: 0x0024864B
			internal UnsafeNativeMethods.ITfCandidateList CandidateList
			{
				[SecurityCritical]
				get
				{
					if (this._candidateList == null)
					{
						return null;
					}
					return this._candidateList.Value;
				}
			}

			// Token: 0x040042F3 RID: 17139
			[SecurityCritical]
			private SecurityCriticalDataClass<UnsafeNativeMethods.ITfCandidateList> _candidateList;
		}

		// Token: 0x020008FE RID: 2302
		private class EditorMenuItem : MenuItem
		{
			// Token: 0x060085B0 RID: 34224 RVA: 0x0024A46A File Offset: 0x0024866A
			internal EditorMenuItem()
			{
			}

			// Token: 0x060085B1 RID: 34225 RVA: 0x0024A472 File Offset: 0x00248672
			[SecurityCritical]
			internal override void OnClickCore(bool userInitiated)
			{
				base.OnClickImpl(userInitiated);
			}
		}

		// Token: 0x020008FF RID: 2303
		private class ReconversionMenuItem : TextEditorContextMenu.EditorMenuItem
		{
			// Token: 0x060085B2 RID: 34226 RVA: 0x0024A47B File Offset: 0x0024867B
			internal ReconversionMenuItem(TextEditorContextMenu.EditorContextMenu menu, int index)
			{
				this._menu = menu;
				this._index = index;
			}

			// Token: 0x060085B3 RID: 34227 RVA: 0x0024A494 File Offset: 0x00248694
			[SecurityCritical]
			internal override void OnClickCore(bool userInitiated)
			{
				Invariant.Assert(this._menu.CandidateList != null);
				try
				{
					this._menu.CandidateList.SetResult(this._index, UnsafeNativeMethods.TfCandidateResult.CAND_FINALIZED);
				}
				catch (COMException)
				{
				}
				base.OnClickCore(false);
			}

			// Token: 0x040042F4 RID: 17140
			private int _index;

			// Token: 0x040042F5 RID: 17141
			private TextEditorContextMenu.EditorContextMenu _menu;
		}
	}
}
