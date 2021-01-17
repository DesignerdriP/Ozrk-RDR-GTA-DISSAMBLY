using System;
using System.Collections.ObjectModel;
using System.Security;
using System.Security.Permissions;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	/// <summary>An abstract base class that provides functionality for text editing controls, including <see cref="T:System.Windows.Controls.TextBox" /> and <see cref="T:System.Windows.Controls.RichTextBox" />.</summary>
	// Token: 0x020005AC RID: 1452
	[Localizability(LocalizationCategory.Text)]
	[TemplatePart(Name = "PART_ContentHost", Type = typeof(FrameworkElement))]
	public abstract class TextBoxBase : Control
	{
		// Token: 0x06006030 RID: 24624 RVA: 0x001AFFB4 File Offset: 0x001AE1B4
		static TextBoxBase()
		{
			TextBoxBase.TextChangedEvent = EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble, typeof(TextChangedEventHandler), typeof(TextBoxBase));
			TextBoxBase.SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TextBoxBase));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxBase), new FrameworkPropertyMetadata(typeof(TextBoxBase)));
			TextBoxBase._dType = DependencyObjectType.FromSystemTypeInternal(typeof(TextBoxBase));
			Control.PaddingProperty.OverrideMetadata(typeof(TextBoxBase), new FrameworkPropertyMetadata(new PropertyChangedCallback(TextBoxBase.OnScrollViewerPropertyChanged)));
			InputMethod.IsInputMethodEnabledProperty.OverrideMetadata(typeof(TextBoxBase), new FrameworkPropertyMetadata(new PropertyChangedCallback(TextBoxBase.OnInputMethodEnabledPropertyChanged)));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(TextBoxBase), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(TextBoxBase), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
		}

		// Token: 0x06006031 RID: 24625 RVA: 0x001B03FD File Offset: 0x001AE5FD
		internal TextBoxBase()
		{
			base.CoerceValue(TextBoxBase.HorizontalScrollBarVisibilityProperty);
			if (!SecurityHelper.CallerHasPermissionWithAppDomainOptimization(new IPermission[]
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode)
			}))
			{
				base.AllowDrop = false;
			}
		}

		/// <summary>Appends a string to the contents of a text control.</summary>
		/// <param name="textData">A string that specifies the text to append to the current contents of the text control.</param>
		// Token: 0x06006032 RID: 24626 RVA: 0x001B0430 File Offset: 0x001AE630
		public void AppendText(string textData)
		{
			if (textData == null)
			{
				return;
			}
			TextRange textRange = new TextRange(this._textContainer.End, this._textContainer.End);
			textRange.Text = textData;
		}

		/// <summary>Is called when a control template is applied.</summary>
		// Token: 0x06006033 RID: 24627 RVA: 0x001B0464 File Offset: 0x001AE664
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.AttachToVisualTree();
		}

		/// <summary>Copies the current selection of the text editing control to the <see cref="T:System.Windows.Clipboard" />.</summary>
		// Token: 0x06006034 RID: 24628 RVA: 0x001B0472 File Offset: 0x001AE672
		[SecurityCritical]
		public void Copy()
		{
			TextEditorCopyPaste.Copy(this.TextEditor, false);
		}

		/// <summary>Removes the current selection from the text editing control and copies it to the <see cref="T:System.Windows.Clipboard" />.</summary>
		// Token: 0x06006035 RID: 24629 RVA: 0x001B0480 File Offset: 0x001AE680
		[SecurityCritical]
		public void Cut()
		{
			TextEditorCopyPaste.Cut(this.TextEditor, false);
		}

		/// <summary>Pastes the contents of the Clipboard over the current selection in the text editing control.</summary>
		// Token: 0x06006036 RID: 24630 RVA: 0x001B048E File Offset: 0x001AE68E
		public void Paste()
		{
			TextEditorCopyPaste.Paste(this.TextEditor);
		}

		/// <summary>Selects all the contents of the text editing control.</summary>
		// Token: 0x06006037 RID: 24631 RVA: 0x001B049C File Offset: 0x001AE69C
		public void SelectAll()
		{
			using (this.TextSelectionInternal.DeclareChangeBlock())
			{
				this.TextSelectionInternal.Select(this._textContainer.Start, this._textContainer.End);
			}
		}

		/// <summary>Scrolls the contents of the control to the left by one line.</summary>
		// Token: 0x06006038 RID: 24632 RVA: 0x001B04F4 File Offset: 0x001AE6F4
		public void LineLeft()
		{
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.LineLeft();
			}
		}

		/// <summary>Scrolls the contents of the control to the right by one line.</summary>
		// Token: 0x06006039 RID: 24633 RVA: 0x001B050F File Offset: 0x001AE70F
		public void LineRight()
		{
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.LineRight();
			}
		}

		/// <summary>Scrolls the contents of the control to the left by one page.</summary>
		// Token: 0x0600603A RID: 24634 RVA: 0x001B052A File Offset: 0x001AE72A
		public void PageLeft()
		{
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.PageLeft();
			}
		}

		/// <summary>Scrolls the contents of the control to the right by one page.</summary>
		// Token: 0x0600603B RID: 24635 RVA: 0x001B0545 File Offset: 0x001AE745
		public void PageRight()
		{
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.PageRight();
			}
		}

		/// <summary>Scrolls the contents of the control upward by one line. </summary>
		// Token: 0x0600603C RID: 24636 RVA: 0x001B0560 File Offset: 0x001AE760
		public void LineUp()
		{
			base.UpdateLayout();
			this.DoLineUp();
		}

		/// <summary>Scrolls the contents of the control down by one line.</summary>
		// Token: 0x0600603D RID: 24637 RVA: 0x001B056E File Offset: 0x001AE76E
		public void LineDown()
		{
			base.UpdateLayout();
			this.DoLineDown();
		}

		/// <summary>Scrolls the contents of the control up by one page.</summary>
		// Token: 0x0600603E RID: 24638 RVA: 0x001B057C File Offset: 0x001AE77C
		public void PageUp()
		{
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.PageUp();
			}
		}

		/// <summary>Scrolls the contents of the control down by one page.</summary>
		// Token: 0x0600603F RID: 24639 RVA: 0x001B0597 File Offset: 0x001AE797
		public void PageDown()
		{
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.PageDown();
			}
		}

		/// <summary>Scrolls the view of the editing control to the beginning of the viewport.</summary>
		// Token: 0x06006040 RID: 24640 RVA: 0x001B05B2 File Offset: 0x001AE7B2
		public void ScrollToHome()
		{
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.ScrollToHome();
			}
		}

		/// <summary>Scrolls the view of the editing control to the end of the content.</summary>
		// Token: 0x06006041 RID: 24641 RVA: 0x001B05CD File Offset: 0x001AE7CD
		public void ScrollToEnd()
		{
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.ScrollToEnd();
			}
		}

		/// <summary>Scrolls the contents of the editing control to the specified horizontal offset.</summary>
		/// <param name="offset">A double value that specifies the horizontal offset to scroll to.</param>
		// Token: 0x06006042 RID: 24642 RVA: 0x001B05E8 File Offset: 0x001AE7E8
		public void ScrollToHorizontalOffset(double offset)
		{
			if (double.IsNaN(offset))
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.ScrollToHorizontalOffset(offset);
			}
		}

		/// <summary>Scrolls the contents of the editing control to the specified vertical offset.</summary>
		/// <param name="offset">A double value that specifies the vertical offset to scroll to.</param>
		// Token: 0x06006043 RID: 24643 RVA: 0x001B0617 File Offset: 0x001AE817
		public void ScrollToVerticalOffset(double offset)
		{
			if (double.IsNaN(offset))
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.ScrollToVerticalOffset(offset);
			}
		}

		/// <summary>Undoes the most recent undo command. In other words, undoes the most recent undo unit on the undo stack.</summary>
		/// <returns>
		///     <see langword="true" /> if the undo operation was successful; otherwise, <see langword="false" />. This method returns <see langword="false" /> if the undo stack is empty.</returns>
		// Token: 0x06006044 RID: 24644 RVA: 0x001B0648 File Offset: 0x001AE848
		public bool Undo()
		{
			UndoManager undoManager = UndoManager.GetUndoManager(this);
			if (undoManager != null && undoManager.UndoCount > undoManager.MinUndoStackCount)
			{
				this.TextEditor.Undo();
				return true;
			}
			return false;
		}

		/// <summary>Undoes the most recent undo command. In other words, redoes the most recent undo unit on the undo stack.</summary>
		/// <returns>
		///     <see langword="true" /> if the redo operation was successful; otherwise, <see langword="false" />. This method returns <see langword="false" /> if there is no undo command available (the undo stack is empty).</returns>
		// Token: 0x06006045 RID: 24645 RVA: 0x001B067C File Offset: 0x001AE87C
		public bool Redo()
		{
			UndoManager undoManager = UndoManager.GetUndoManager(this);
			if (undoManager != null && undoManager.RedoCount > 0)
			{
				this.TextEditor.Redo();
				return true;
			}
			return false;
		}

		/// <summary>Locks the most recent undo unit of the undo stack of the application. This prevents the locked unit from being merged with undo units that are added subsequently.</summary>
		// Token: 0x06006046 RID: 24646 RVA: 0x001B06AC File Offset: 0x001AE8AC
		public void LockCurrentUndoUnit()
		{
			UndoManager undoManager = UndoManager.GetUndoManager(this);
			if (undoManager != null)
			{
				IParentUndoUnit openedUnit = undoManager.OpenedUnit;
				if (openedUnit != null)
				{
					while (openedUnit.OpenedUnit != null)
					{
						openedUnit = openedUnit.OpenedUnit;
					}
					if (openedUnit.LastUnit is IParentUndoUnit)
					{
						openedUnit.OnNextAdd();
						return;
					}
				}
				else if (undoManager.LastUnit is IParentUndoUnit)
				{
					((IParentUndoUnit)undoManager.LastUnit).OnNextAdd();
				}
			}
		}

		/// <summary>Begins a change block.</summary>
		// Token: 0x06006047 RID: 24647 RVA: 0x001B070F File Offset: 0x001AE90F
		public void BeginChange()
		{
			this.TextEditor.Selection.BeginChange();
		}

		/// <summary>Ends a change block.</summary>
		// Token: 0x06006048 RID: 24648 RVA: 0x001B0721 File Offset: 0x001AE921
		public void EndChange()
		{
			if (this.TextEditor.Selection.ChangeBlockLevel == 0)
			{
				throw new InvalidOperationException(SR.Get("TextBoxBase_UnmatchedEndChange"));
			}
			this.TextEditor.Selection.EndChange();
		}

		/// <summary>Creates a change block.</summary>
		/// <returns>An <see cref="T:System.IDisposable" /> object that refers to a new change block.</returns>
		// Token: 0x06006049 RID: 24649 RVA: 0x001B0755 File Offset: 0x001AE955
		public IDisposable DeclareChangeBlock()
		{
			return this.TextEditor.Selection.DeclareChangeBlock();
		}

		/// <summary>Gets or sets a value that indicates whether the text editing control is read-only to a user interacting with the control. </summary>
		/// <returns>
		///     <see langword="true" /> if the contents of the text editing control are read-only to a user; otherwise, the contents of the text editing control can be modified by the user. The default value is <see langword="false" />.</returns>
		// Token: 0x17001723 RID: 5923
		// (get) Token: 0x0600604A RID: 24650 RVA: 0x001B0767 File Offset: 0x001AE967
		// (set) Token: 0x0600604B RID: 24651 RVA: 0x001B0779 File Offset: 0x001AE979
		public bool IsReadOnly
		{
			get
			{
				return (bool)base.GetValue(TextEditor.IsReadOnlyProperty);
			}
			set
			{
				base.SetValue(TextEditor.IsReadOnlyProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a read-only text box displays a caret.</summary>
		/// <returns>
		///     <see langword="true" /> if a read-only text box displays a caret; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001724 RID: 5924
		// (get) Token: 0x0600604C RID: 24652 RVA: 0x001B0787 File Offset: 0x001AE987
		// (set) Token: 0x0600604D RID: 24653 RVA: 0x001B0799 File Offset: 0x001AE999
		public bool IsReadOnlyCaretVisible
		{
			get
			{
				return (bool)base.GetValue(TextBoxBase.IsReadOnlyCaretVisibleProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.IsReadOnlyCaretVisibleProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates how the text editing control responds when the user presses the ENTER key.</summary>
		/// <returns>
		///     <see langword="true" /> if pressing the ENTER key inserts a new line at the current cursor position; otherwise, the ENTER key is ignored. The default value is <see langword="false" /> for <see cref="T:System.Windows.Controls.TextBox" /> and <see langword="true" /> for <see cref="T:System.Windows.Controls.RichTextBox" />.</returns>
		// Token: 0x17001725 RID: 5925
		// (get) Token: 0x0600604E RID: 24654 RVA: 0x001B07A7 File Offset: 0x001AE9A7
		// (set) Token: 0x0600604F RID: 24655 RVA: 0x001B07B9 File Offset: 0x001AE9B9
		public bool AcceptsReturn
		{
			get
			{
				return (bool)base.GetValue(TextBoxBase.AcceptsReturnProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.AcceptsReturnProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates how the text editing control responds when the user presses the TAB key.  </summary>
		/// <returns>
		///     <see langword="true" /> if pressing the TAB key inserts a tab character at the current cursor position; <see langword="false" /> if pressing the TAB key moves the focus to the next control that is marked as a tab stop and does not insert a tab character.The default value is <see langword="false" />.</returns>
		// Token: 0x17001726 RID: 5926
		// (get) Token: 0x06006050 RID: 24656 RVA: 0x001B07C7 File Offset: 0x001AE9C7
		// (set) Token: 0x06006051 RID: 24657 RVA: 0x001B07D9 File Offset: 0x001AE9D9
		public bool AcceptsTab
		{
			get
			{
				return (bool)base.GetValue(TextBoxBase.AcceptsTabProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.AcceptsTabProperty, value);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Controls.SpellCheck" /> object that provides access to spelling errors in the text contents of a <see cref="T:System.Windows.Controls.Primitives.TextBoxBase" /> or <see cref="T:System.Windows.Controls.RichTextBox" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Controls.SpellCheck" /> object that provides access to spelling errors in the text contents of a <see cref="T:System.Windows.Controls.Primitives.TextBoxBase" /> or <see cref="T:System.Windows.Controls.RichTextBox" />.This property has no default value.</returns>
		// Token: 0x17001727 RID: 5927
		// (get) Token: 0x06006052 RID: 24658 RVA: 0x001B07E7 File Offset: 0x001AE9E7
		public SpellCheck SpellCheck
		{
			get
			{
				return new SpellCheck(this);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a horizontal scroll bar is shown. </summary>
		/// <returns>A value that is defined by the <see cref="T:System.Windows.Controls.ScrollBarVisibility" /> enumeration.The default value is <see cref="F:System.Windows.Visibility.Hidden" />.</returns>
		// Token: 0x17001728 RID: 5928
		// (get) Token: 0x06006053 RID: 24659 RVA: 0x001B07EF File Offset: 0x001AE9EF
		// (set) Token: 0x06006054 RID: 24660 RVA: 0x001B0801 File Offset: 0x001AEA01
		public ScrollBarVisibility HorizontalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(TextBoxBase.HorizontalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.HorizontalScrollBarVisibilityProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a vertical scroll bar is shown. </summary>
		/// <returns>A value that is defined by the <see cref="T:System.Windows.Controls.ScrollBarVisibility" /> enumeration. The default value is <see cref="F:System.Windows.Visibility.Hidden" />.</returns>
		// Token: 0x17001729 RID: 5929
		// (get) Token: 0x06006055 RID: 24661 RVA: 0x001B0814 File Offset: 0x001AEA14
		// (set) Token: 0x06006056 RID: 24662 RVA: 0x001B0826 File Offset: 0x001AEA26
		public ScrollBarVisibility VerticalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(TextBoxBase.VerticalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.VerticalScrollBarVisibilityProperty, value);
			}
		}

		/// <summary>Gets the horizontal size of the visible content area.</summary>
		/// <returns>A floating-point value that specifies the horizontal size of the visible content area, in device-independent units (1/96th inch per unit).The value of this property is 0.0 if the text editing control is not configured to support scrolling.This property has no default value.</returns>
		// Token: 0x1700172A RID: 5930
		// (get) Token: 0x06006057 RID: 24663 RVA: 0x001B0839 File Offset: 0x001AEA39
		public double ExtentWidth
		{
			get
			{
				if (this.ScrollViewer == null)
				{
					return 0.0;
				}
				return this.ScrollViewer.ExtentWidth;
			}
		}

		/// <summary>Gets the vertical size of the visible content area.</summary>
		/// <returns>A floating-point value that specifies the vertical size of the visible content area, in device-independent units (1/96th inch per unit).The value of this property is 0.0 if the text-editing control is not configured to support scrolling.This property has no default value.</returns>
		// Token: 0x1700172B RID: 5931
		// (get) Token: 0x06006058 RID: 24664 RVA: 0x001B0858 File Offset: 0x001AEA58
		public double ExtentHeight
		{
			get
			{
				if (this.ScrollViewer == null)
				{
					return 0.0;
				}
				return this.ScrollViewer.ExtentHeight;
			}
		}

		/// <summary>Gets the horizontal size of the scrollable content area.</summary>
		/// <returns>A floating-point value that specifies the horizontal size of the scrollable content area, in device-independent units (1/96th inch per unit).The value of this property is 0.0 if the text editing control is not configured to support scrolling.This property has no default value.</returns>
		// Token: 0x1700172C RID: 5932
		// (get) Token: 0x06006059 RID: 24665 RVA: 0x001B0877 File Offset: 0x001AEA77
		public double ViewportWidth
		{
			get
			{
				if (this.ScrollViewer == null)
				{
					return 0.0;
				}
				return this.ScrollViewer.ViewportWidth;
			}
		}

		/// <summary>Gets the vertical size of the scrollable content area.</summary>
		/// <returns>A floating-point value that specifies the vertical size of the scrollable content area, in device-independent units (1/96th inch per unit).The value of this property is 0.0 if the text editing control is not configured to support scrolling.This property has no default value.</returns>
		// Token: 0x1700172D RID: 5933
		// (get) Token: 0x0600605A RID: 24666 RVA: 0x001B0896 File Offset: 0x001AEA96
		public double ViewportHeight
		{
			get
			{
				if (this.ScrollViewer == null)
				{
					return 0.0;
				}
				return this.ScrollViewer.ViewportHeight;
			}
		}

		/// <summary>Gets or sets the horizontal scroll position.</summary>
		/// <returns>A floating-point value that specifies the horizontal scroll position, in device-independent units (1/96th inch per unit). Setting this property causes the text editing control to scroll to the specified horizontal offset. Reading this property returns the current horizontal offset.The value of this property is 0.0 if the text editing control is not configured to support scrolling.This property has no default value.</returns>
		/// <exception cref="T:System.ArgumentException">An attempt is made to set this property to a negative value.</exception>
		// Token: 0x1700172E RID: 5934
		// (get) Token: 0x0600605B RID: 24667 RVA: 0x001B08B5 File Offset: 0x001AEAB5
		public double HorizontalOffset
		{
			get
			{
				if (this.ScrollViewer == null)
				{
					return 0.0;
				}
				return this.ScrollViewer.HorizontalOffset;
			}
		}

		/// <summary>Gets or sets the vertical scroll position.</summary>
		/// <returns>A floating-point value that specifies the vertical scroll position, in device-independent units (1/96th inch per unit).Setting this property causes the text editing control to scroll to the specified vertical offset. Reading this property returns the current vertical offset.The value of this property is 0.0 if the text editing control is not configured to support scrolling.This property has no default value.</returns>
		/// <exception cref="T:System.ArgumentException">An attempt is made to set this property to a negative value.</exception>
		// Token: 0x1700172F RID: 5935
		// (get) Token: 0x0600605C RID: 24668 RVA: 0x001B08D4 File Offset: 0x001AEAD4
		public double VerticalOffset
		{
			get
			{
				if (this.ScrollViewer == null)
				{
					return 0.0;
				}
				return this.ScrollViewer.VerticalOffset;
			}
		}

		/// <summary>Gets a value that indicates whether the most recent action can be undone.</summary>
		/// <returns>
		///     <see langword="true " />if the most recent action can be undone; otherwise, <see langword="false" />.This property has no default value.</returns>
		// Token: 0x17001730 RID: 5936
		// (get) Token: 0x0600605D RID: 24669 RVA: 0x001B08F4 File Offset: 0x001AEAF4
		public bool CanUndo
		{
			get
			{
				UndoManager undoManager = UndoManager.GetUndoManager(this);
				return undoManager != null && this._pendingUndoAction != UndoAction.Clear && (undoManager.UndoCount > undoManager.MinUndoStackCount || (undoManager.State != UndoState.Undo && this._pendingUndoAction == UndoAction.Create));
			}
		}

		/// <summary>Gets a value that indicates whether the most recent undo action can be redone.</summary>
		/// <returns>
		///     <see langword="true" /> if the most recent undo action can be redone; otherwise, <see langword="false" />.This property has no default value.</returns>
		// Token: 0x17001731 RID: 5937
		// (get) Token: 0x0600605E RID: 24670 RVA: 0x001B0938 File Offset: 0x001AEB38
		public bool CanRedo
		{
			get
			{
				UndoManager undoManager = UndoManager.GetUndoManager(this);
				return undoManager != null && this._pendingUndoAction != UndoAction.Clear && (undoManager.RedoCount > 0 || (undoManager.State == UndoState.Undo && this._pendingUndoAction == UndoAction.Create));
			}
		}

		/// <summary>Gets or sets a value that indicates whether undo support is enabled for the text-editing control.  </summary>
		/// <returns>
		///     <see langword="true" /> if undo support is enabled; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17001732 RID: 5938
		// (get) Token: 0x0600605F RID: 24671 RVA: 0x001B0976 File Offset: 0x001AEB76
		// (set) Token: 0x06006060 RID: 24672 RVA: 0x001B0988 File Offset: 0x001AEB88
		public bool IsUndoEnabled
		{
			get
			{
				return (bool)base.GetValue(TextBoxBase.IsUndoEnabledProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.IsUndoEnabledProperty, value);
			}
		}

		/// <summary>Gets or sets the number of actions stored in the undo queue.</summary>
		/// <returns>The number of actions stored in the undo queue. The default is –1, which means the undo queue is limited to the memory that is available.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.UndoLimit" /> is set after calling <see cref="M:System.Windows.Controls.Primitives.TextBoxBase.BeginChange" /> and before calling <see cref="M:System.Windows.Controls.Primitives.TextBoxBase.EndChange" />.</exception>
		// Token: 0x17001733 RID: 5939
		// (get) Token: 0x06006061 RID: 24673 RVA: 0x001B0996 File Offset: 0x001AEB96
		// (set) Token: 0x06006062 RID: 24674 RVA: 0x001B09A8 File Offset: 0x001AEBA8
		public int UndoLimit
		{
			get
			{
				return (int)base.GetValue(TextBoxBase.UndoLimitProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.UndoLimitProperty, value);
			}
		}

		/// <summary>Gets or sets a value that determines whether when a user selects part of a word by dragging across it with the mouse, the rest of the word is selected.</summary>
		/// <returns>
		///     <see langword="true" /> if automatic word selection is enabled; otherwise, <see langword="false" />.The default value is <see langword="false" />.</returns>
		// Token: 0x17001734 RID: 5940
		// (get) Token: 0x06006063 RID: 24675 RVA: 0x001B09BB File Offset: 0x001AEBBB
		// (set) Token: 0x06006064 RID: 24676 RVA: 0x001B09CD File Offset: 0x001AEBCD
		public bool AutoWordSelection
		{
			get
			{
				return (bool)base.GetValue(TextBoxBase.AutoWordSelectionProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.AutoWordSelectionProperty, value);
			}
		}

		/// <summary>Gets or sets the brush that highlights selected text.</summary>
		/// <returns>The brush that highlights selected text.</returns>
		// Token: 0x17001735 RID: 5941
		// (get) Token: 0x06006065 RID: 24677 RVA: 0x001B09DB File Offset: 0x001AEBDB
		// (set) Token: 0x06006066 RID: 24678 RVA: 0x001B09ED File Offset: 0x001AEBED
		public Brush SelectionBrush
		{
			get
			{
				return (Brush)base.GetValue(TextBoxBase.SelectionBrushProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.SelectionBrushProperty, value);
			}
		}

		// Token: 0x17001736 RID: 5942
		// (get) Token: 0x06006067 RID: 24679 RVA: 0x001B09FB File Offset: 0x001AEBFB
		// (set) Token: 0x06006068 RID: 24680 RVA: 0x001B0A0D File Offset: 0x001AEC0D
		public Brush SelectionTextBrush
		{
			get
			{
				return (Brush)base.GetValue(TextBoxBase.SelectionTextBrushProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.SelectionTextBrushProperty, value);
			}
		}

		/// <summary>Gets or sets the opacity of the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.SelectionBrush" />.</summary>
		/// <returns>The opacity of the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.SelectionBrush" />. The default is 0.4.</returns>
		// Token: 0x17001737 RID: 5943
		// (get) Token: 0x06006069 RID: 24681 RVA: 0x001B0A1B File Offset: 0x001AEC1B
		// (set) Token: 0x0600606A RID: 24682 RVA: 0x001B0A2D File Offset: 0x001AEC2D
		public double SelectionOpacity
		{
			get
			{
				return (double)base.GetValue(TextBoxBase.SelectionOpacityProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.SelectionOpacityProperty, value);
			}
		}

		/// <summary>Gets or sets the brush that is used to paint the caret of the text box.</summary>
		/// <returns>The brush that is used to paint the caret of the text box.</returns>
		// Token: 0x17001738 RID: 5944
		// (get) Token: 0x0600606B RID: 24683 RVA: 0x001B0A40 File Offset: 0x001AEC40
		// (set) Token: 0x0600606C RID: 24684 RVA: 0x001B0A52 File Offset: 0x001AEC52
		public Brush CaretBrush
		{
			get
			{
				return (Brush)base.GetValue(TextBoxBase.CaretBrushProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.CaretBrushProperty, value);
			}
		}

		/// <summary>Gets a value that indicates whether the text box has focus and selected text.</summary>
		/// <returns>
		///     <see langword="true" /> if the text box has focus and selected text; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x17001739 RID: 5945
		// (get) Token: 0x0600606D RID: 24685 RVA: 0x001B0A60 File Offset: 0x001AEC60
		public bool IsSelectionActive
		{
			get
			{
				return (bool)base.GetValue(TextBoxBase.IsSelectionActiveProperty);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the text box displays selected text when the text box does not have focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the text box displays selected text when the text box does not have focus; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x1700173A RID: 5946
		// (get) Token: 0x0600606E RID: 24686 RVA: 0x001B0A72 File Offset: 0x001AEC72
		// (set) Token: 0x0600606F RID: 24687 RVA: 0x001B0A84 File Offset: 0x001AEC84
		public bool IsInactiveSelectionHighlightEnabled
		{
			get
			{
				return (bool)base.GetValue(TextBoxBase.IsInactiveSelectionHighlightEnabledProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.IsInactiveSelectionHighlightEnabledProperty, value);
			}
		}

		/// <summary>Occurs when content changes in the text element.</summary>
		// Token: 0x1400011D RID: 285
		// (add) Token: 0x06006070 RID: 24688 RVA: 0x001B0A92 File Offset: 0x001AEC92
		// (remove) Token: 0x06006071 RID: 24689 RVA: 0x001B0AA0 File Offset: 0x001AECA0
		public event TextChangedEventHandler TextChanged
		{
			add
			{
				base.AddHandler(TextBoxBase.TextChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(TextBoxBase.TextChangedEvent, value);
			}
		}

		/// <summary>Occurs when the text selection has changed.</summary>
		// Token: 0x1400011E RID: 286
		// (add) Token: 0x06006072 RID: 24690 RVA: 0x001B0AAE File Offset: 0x001AECAE
		// (remove) Token: 0x06006073 RID: 24691 RVA: 0x001B0ABC File Offset: 0x001AECBC
		public event RoutedEventHandler SelectionChanged
		{
			add
			{
				base.AddHandler(TextBoxBase.SelectionChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(TextBoxBase.SelectionChangedEvent, value);
			}
		}

		// Token: 0x06006074 RID: 24692 RVA: 0x001B0ACC File Offset: 0x001AECCC
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Disabled", useTransitions);
			}
			else if (this.IsReadOnly)
			{
				VisualStateManager.GoToState(this, "ReadOnly", useTransitions);
			}
			else if (base.IsMouseOver)
			{
				VisualStateManager.GoToState(this, "MouseOver", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			if (base.IsKeyboardFocused)
			{
				VisualStateManager.GoToState(this, "Focused", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		/// <summary>Is called when content in this editing control changes.</summary>
		/// <param name="e">The arguments that are associated with the <see cref="E:System.Windows.Controls.Primitives.TextBoxBase.TextChanged" /> event.</param>
		// Token: 0x06006075 RID: 24693 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnTextChanged(TextChangedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Is called when the caret or current selection changes position.</summary>
		/// <param name="e">The arguments that are associated with the <see cref="E:System.Windows.Controls.Primitives.TextBoxBase.SelectionChanged" /> event.</param>
		// Token: 0x06006076 RID: 24694 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnSelectionChanged(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Is called when the control template changes.</summary>
		/// <param name="oldTemplate">A <see cref="T:System.Windows.Controls.ControlTemplate" /> object that specifies the control template that is currently active.</param>
		/// <param name="newTemplate">A <see cref="T:System.Windows.Controls.ControlTemplate" /> object that specifies a new control template to use.</param>
		// Token: 0x06006077 RID: 24695 RVA: 0x001B0B56 File Offset: 0x001AED56
		protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
			base.OnTemplateChanged(oldTemplate, newTemplate);
			if (oldTemplate != null && newTemplate != null && oldTemplate.VisualTree != newTemplate.VisualTree)
			{
				this.DetachFromVisualTree();
			}
		}

		/// <summary>Is called when a <see cref="E:System.Windows.UIElement.MouseWheel" /> event is routed to this class (or to a class that inherits from this class).</summary>
		/// <param name="e">The mouse wheel arguments that are associated with this event.</param>
		// Token: 0x06006078 RID: 24696 RVA: 0x001B0B7C File Offset: 0x001AED7C
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.ScrollViewer != null && ((e.Delta > 0 && this.VerticalOffset != 0.0) || (e.Delta < 0 && this.VerticalOffset < this.ScrollViewer.ScrollableHeight)))
			{
				Invariant.Assert(this.RenderScope is IScrollInfo);
				if (e.Delta > 0)
				{
					((IScrollInfo)this.RenderScope).MouseWheelUp();
				}
				else
				{
					((IScrollInfo)this.RenderScope).MouseWheelDown();
				}
				e.Handled = true;
			}
			base.OnMouseWheel(e);
		}

		/// <summary>Called when the <see cref="E:System.Windows.UIElement.KeyDown" /> occurs.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06006079 RID: 24697 RVA: 0x001B0C21 File Offset: 0x001AEE21
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnPreviewKeyDown(e);
			}
		}

		/// <summary>Invoked whenever an unhandled <see cref="E:System.Windows.Input.Keyboard.KeyDown" /> attached routed event reaches an element derived from this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x0600607A RID: 24698 RVA: 0x001B0C47 File Offset: 0x001AEE47
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnKeyDown(e);
			}
		}

		/// <summary>Invoked whenever an unhandled <see cref="E:System.Windows.Input.Keyboard.KeyUp" /> attached routed event reaches an element derived from this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x0600607B RID: 24699 RVA: 0x001B0C6D File Offset: 0x001AEE6D
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnKeyUp(e);
			}
		}

		/// <summary>Invoked whenever an unhandled <see cref="E:System.Windows.Input.TextCompositionManager.TextInput" /> attached routed event reaches an element derived from this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x0600607C RID: 24700 RVA: 0x001B0C93 File Offset: 0x001AEE93
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			base.OnTextInput(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnTextInput(e);
			}
		}

		/// <summary>Invoked whenever an unhandled <see cref="E:System.Windows.Input.Mouse.MouseDown" /> attached routed event reaches an element derived from this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x0600607D RID: 24701 RVA: 0x001B0CB9 File Offset: 0x001AEEB9
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnMouseDown(e);
			}
		}

		/// <summary>Invoked whenever an unhandled <see cref="E:System.Windows.Input.Mouse.MouseMove" /> attached routed event reaches an element derived from this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x0600607E RID: 24702 RVA: 0x001B0CDF File Offset: 0x001AEEDF
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnMouseMove(e);
			}
		}

		/// <summary>Invoked whenever an unhandled <see cref="E:System.Windows.Input.Mouse.MouseUp" /> event reaches an element derived from this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Arguments of the event. These arguments will include details about which mouse button was depressed, and the handled state.</param>
		// Token: 0x0600607F RID: 24703 RVA: 0x001B0D05 File Offset: 0x001AEF05
		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			base.OnMouseUp(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnMouseUp(e);
			}
		}

		/// <summary>Invoked whenever an unhandled <see cref="E:System.Windows.Input.Mouse.QueryCursor" /> attached routed event reaches an element derived from this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x06006080 RID: 24704 RVA: 0x001B0D2B File Offset: 0x001AEF2B
		protected override void OnQueryCursor(QueryCursorEventArgs e)
		{
			base.OnQueryCursor(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnQueryCursor(e);
			}
		}

		/// <summary>  Invoked whenever an unhandled <see cref="E:System.Windows.DragDrop.QueryContinueDrag" /> attached  routed event reaches an element derived from this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x06006081 RID: 24705 RVA: 0x001B0D51 File Offset: 0x001AEF51
		protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
		{
			base.OnQueryContinueDrag(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnQueryContinueDrag(e);
			}
		}

		/// <summary>Invoked whenever an unhandled <see cref="E:System.Windows.DragDrop.GiveFeedback" /> attached routed event reaches an element derived from this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x06006082 RID: 24706 RVA: 0x001B0D77 File Offset: 0x001AEF77
		protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
			base.OnGiveFeedback(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnGiveFeedback(e);
			}
		}

		/// <summary>Invoked whenever an unhandled <see cref="E:System.Windows.DragDrop.DragEnter" /> attached routed event reaches an element derived from this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x06006083 RID: 24707 RVA: 0x001B0D9D File Offset: 0x001AEF9D
		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnDragEnter(e);
			}
		}

		/// <summary>Invoked whenever an unhandled <see cref="E:System.Windows.DragDrop.DragOver" /> attached routed event reaches an element derived from this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x06006084 RID: 24708 RVA: 0x001B0DC3 File Offset: 0x001AEFC3
		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnDragOver(e);
			}
		}

		/// <summary>Invoked whenever an unhandled <see cref="E:System.Windows.DragDrop.DragLeave" /> attached routed event reaches an element derived from this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x06006085 RID: 24709 RVA: 0x001B0DE9 File Offset: 0x001AEFE9
		protected override void OnDragLeave(DragEventArgs e)
		{
			base.OnDragLeave(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnDragLeave(e);
			}
		}

		/// <summary>Invoked whenever an unhandled <see cref="E:System.Windows.DragDrop.DragEnter" /> attached routed event reaches an element derived from this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x06006086 RID: 24710 RVA: 0x001B0E0F File Offset: 0x001AF00F
		protected override void OnDrop(DragEventArgs e)
		{
			base.OnDrop(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnDrop(e);
			}
		}

		/// <summary>Called whenever an unhandled <see cref="E:System.Windows.FrameworkElement.ContextMenuOpening" /> routed event reaches this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Arguments of the event.</param>
		// Token: 0x06006087 RID: 24711 RVA: 0x001B0E35 File Offset: 0x001AF035
		protected override void OnContextMenuOpening(ContextMenuEventArgs e)
		{
			base.OnContextMenuOpening(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnContextMenuOpening(e);
			}
		}

		/// <summary>Invoked whenever an unhandled <see cref="E:System.Windows.Input.Keyboard.GotKeyboardFocus" /> attached routed event reaches an element derived from this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x06006088 RID: 24712 RVA: 0x001B0E5B File Offset: 0x001AF05B
		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnGotKeyboardFocus(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnGotKeyboardFocus(e);
			}
		}

		/// <summary>Invoked whenever an unhandled <see cref="E:System.Windows.Input.Keyboard.LostKeyboardFocus" /> attached routed event reaches an element derived from this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x06006089 RID: 24713 RVA: 0x001B0E81 File Offset: 0x001AF081
		protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnLostKeyboardFocus(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnLostKeyboardFocus(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.UIElement.LostFocus" /> event (using the provided arguments).</summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x0600608A RID: 24714 RVA: 0x001B0EA7 File Offset: 0x001AF0A7
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnLostFocus(e);
			}
		}

		// Token: 0x0600608B RID: 24715
		internal abstract FrameworkElement CreateRenderScope();

		// Token: 0x0600608C RID: 24716 RVA: 0x001B0ED0 File Offset: 0x001AF0D0
		internal virtual void OnTextContainerChanged(object sender, TextContainerChangedEventArgs e)
		{
			if (!e.HasContentAddedOrRemoved && !e.HasLocalPropertyValueChange)
			{
				return;
			}
			UndoManager undoManager = UndoManager.GetUndoManager(this);
			UndoAction undoAction;
			if (undoManager != null)
			{
				if (this._textEditor.UndoState == UndoState.Redo)
				{
					undoAction = UndoAction.Redo;
				}
				else if (this._textEditor.UndoState == UndoState.Undo)
				{
					undoAction = UndoAction.Undo;
				}
				else if (undoManager.OpenedUnit == null)
				{
					undoAction = UndoAction.Clear;
				}
				else if (undoManager.LastReopenedUnit == undoManager.OpenedUnit)
				{
					undoAction = UndoAction.Merge;
				}
				else
				{
					undoAction = UndoAction.Create;
				}
			}
			else
			{
				undoAction = UndoAction.Create;
			}
			this._pendingUndoAction = undoAction;
			try
			{
				this.OnTextChanged(new TextChangedEventArgs(TextBoxBase.TextChangedEvent, undoAction, new ReadOnlyCollection<TextChange>(e.Changes.Values)));
			}
			finally
			{
				this._pendingUndoAction = UndoAction.None;
			}
		}

		// Token: 0x0600608D RID: 24717 RVA: 0x001B0F84 File Offset: 0x001AF184
		internal void InitializeTextContainer(TextContainer textContainer)
		{
			Invariant.Assert(textContainer != null);
			Invariant.Assert(textContainer.TextSelection == null);
			if (this._textContainer != null)
			{
				Invariant.Assert(this._textEditor != null);
				Invariant.Assert(this._textEditor.TextContainer == this._textContainer);
				Invariant.Assert(this._textEditor.TextContainer.TextSelection == this._textEditor.Selection);
				this.DetachFromVisualTree();
				this._textEditor.OnDetach();
			}
			this._textContainer = textContainer;
			this._textContainer.Changed += this.OnTextContainerChanged;
			this._textEditor = new TextEditor(this._textContainer, this, true);
			this._textEditor.Selection.Changed += this.OnSelectionChangedInternal;
			UndoManager undoManager = UndoManager.GetUndoManager(this);
			if (undoManager != null)
			{
				undoManager.UndoLimit = this.UndoLimit;
			}
		}

		// Token: 0x0600608E RID: 24718 RVA: 0x001B1070 File Offset: 0x001AF270
		internal TextPointer GetTextPositionFromPointInternal(Point point, bool snapToText)
		{
			GeneralTransform generalTransform = base.TransformToDescendant(this.RenderScope);
			if (generalTransform != null)
			{
				generalTransform.TryTransform(point, out point);
			}
			TextPointer result;
			if (TextEditor.GetTextView(this.RenderScope).Validate(point))
			{
				result = (TextPointer)TextEditor.GetTextView(this.RenderScope).GetTextPositionFromPoint(point, snapToText);
			}
			else
			{
				result = (snapToText ? this.TextContainer.Start : null);
			}
			return result;
		}

		// Token: 0x0600608F RID: 24719 RVA: 0x001B10D8 File Offset: 0x001AF2D8
		internal bool GetRectangleFromTextPosition(TextPointer position, out Rect rect)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			if (TextEditor.GetTextView(this.RenderScope).Validate(position))
			{
				rect = TextEditor.GetTextView(this.RenderScope).GetRectangleFromTextPosition(position);
				Point inPoint = new Point(0.0, 0.0);
				GeneralTransform generalTransform = base.TransformToDescendant(this.RenderScope);
				if (generalTransform != null)
				{
					generalTransform.TryTransform(inPoint, out inPoint);
				}
				rect.X -= inPoint.X;
				rect.Y -= inPoint.Y;
			}
			else
			{
				rect = Rect.Empty;
			}
			return rect != Rect.Empty;
		}

		// Token: 0x06006090 RID: 24720 RVA: 0x001B1198 File Offset: 0x001AF398
		internal virtual void AttachToVisualTree()
		{
			this.DetachFromVisualTree();
			this.SetRenderScopeToContentHost();
			if (this.ScrollViewer != null)
			{
				this.ScrollViewer.ScrollChanged += this.OnScrollChanged;
				base.SetValue(TextEditor.PageHeightProperty, this.ScrollViewer.ViewportHeight);
				this.ScrollViewer.Focusable = false;
				this.ScrollViewer.HandlesMouseWheelScrolling = false;
				if (this.ScrollViewer.Background == null)
				{
					this.ScrollViewer.Background = Brushes.Transparent;
				}
				TextBoxBase.OnScrollViewerPropertyChanged(this, new DependencyPropertyChangedEventArgs(ScrollViewer.HorizontalScrollBarVisibilityProperty, null, base.GetValue(TextBoxBase.HorizontalScrollBarVisibilityProperty)));
				TextBoxBase.OnScrollViewerPropertyChanged(this, new DependencyPropertyChangedEventArgs(ScrollViewer.VerticalScrollBarVisibilityProperty, null, base.GetValue(TextBoxBase.VerticalScrollBarVisibilityProperty)));
				TextBoxBase.OnScrollViewerPropertyChanged(this, new DependencyPropertyChangedEventArgs(Control.PaddingProperty, null, base.GetValue(Control.PaddingProperty)));
				return;
			}
			base.ClearValue(TextEditor.PageHeightProperty);
		}

		// Token: 0x06006091 RID: 24721 RVA: 0x001B1284 File Offset: 0x001AF484
		internal virtual void DoLineUp()
		{
			if (this.ScrollViewer != null)
			{
				this.ScrollViewer.LineUp();
			}
		}

		// Token: 0x06006092 RID: 24722 RVA: 0x001B1299 File Offset: 0x001AF499
		internal virtual void DoLineDown()
		{
			if (this.ScrollViewer != null)
			{
				this.ScrollViewer.LineDown();
			}
		}

		// Token: 0x06006093 RID: 24723 RVA: 0x001B12B0 File Offset: 0x001AF4B0
		internal override void AddToEventRouteCore(EventRoute route, RoutedEventArgs args)
		{
			base.AddToEventRouteCore(route, args);
			Visual visual = this.RenderScope;
			while (visual != this && visual != null)
			{
				if (visual is UIElement)
				{
					((UIElement)visual).AddToEventRoute(route, args);
				}
				visual = (VisualTreeHelper.GetParent(visual) as Visual);
			}
		}

		// Token: 0x06006094 RID: 24724 RVA: 0x001B12F8 File Offset: 0x001AF4F8
		internal void ChangeUndoEnabled(bool value)
		{
			if (this.TextSelectionInternal.ChangeBlockLevel > 0)
			{
				throw new InvalidOperationException(SR.Get("TextBoxBase_CantSetIsUndoEnabledInsideChangeBlock"));
			}
			UndoManager undoManager = UndoManager.GetUndoManager(this);
			if (undoManager != null)
			{
				if (!value && undoManager.IsEnabled)
				{
					undoManager.Clear();
				}
				undoManager.IsEnabled = value;
			}
		}

		// Token: 0x06006095 RID: 24725 RVA: 0x001B1348 File Offset: 0x001AF548
		internal void ChangeUndoLimit(object value)
		{
			UndoManager undoManager = UndoManager.GetUndoManager(this);
			if (undoManager != null)
			{
				if (undoManager.OpenedUnit != null)
				{
					throw new InvalidOperationException(SR.Get("TextBoxBase_CantSetIsUndoEnabledInsideChangeBlock"));
				}
				int undoLimit;
				if (value == DependencyProperty.UnsetValue)
				{
					undoLimit = UndoManager.UndoLimitDefaultValue;
				}
				else
				{
					undoLimit = (int)value;
				}
				undoManager.UndoLimit = undoLimit;
			}
		}

		// Token: 0x1700173B RID: 5947
		// (get) Token: 0x06006096 RID: 24726 RVA: 0x001B1395 File Offset: 0x001AF595
		internal ScrollViewer ScrollViewer
		{
			get
			{
				if (this._scrollViewer == null && this._textEditor != null)
				{
					this._scrollViewer = (this._textEditor._Scroller as ScrollViewer);
				}
				return this._scrollViewer;
			}
		}

		// Token: 0x1700173C RID: 5948
		// (get) Token: 0x06006097 RID: 24727 RVA: 0x001B13C3 File Offset: 0x001AF5C3
		internal TextSelection TextSelectionInternal
		{
			get
			{
				return (TextSelection)this._textEditor.Selection;
			}
		}

		// Token: 0x1700173D RID: 5949
		// (get) Token: 0x06006098 RID: 24728 RVA: 0x001B13D5 File Offset: 0x001AF5D5
		internal TextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x1700173E RID: 5950
		// (get) Token: 0x06006099 RID: 24729 RVA: 0x001B13DD File Offset: 0x001AF5DD
		internal FrameworkElement RenderScope
		{
			get
			{
				return this._renderScope;
			}
		}

		// Token: 0x1700173F RID: 5951
		// (get) Token: 0x0600609A RID: 24730 RVA: 0x001B13E5 File Offset: 0x001AF5E5
		// (set) Token: 0x0600609B RID: 24731 RVA: 0x001B13ED File Offset: 0x001AF5ED
		internal UndoAction PendingUndoAction
		{
			get
			{
				return this._pendingUndoAction;
			}
			set
			{
				this._pendingUndoAction = value;
			}
		}

		// Token: 0x17001740 RID: 5952
		// (get) Token: 0x0600609C RID: 24732 RVA: 0x001B13F6 File Offset: 0x001AF5F6
		internal TextEditor TextEditor
		{
			get
			{
				return this._textEditor;
			}
		}

		// Token: 0x17001741 RID: 5953
		// (get) Token: 0x0600609D RID: 24733 RVA: 0x001B13FE File Offset: 0x001AF5FE
		internal bool IsContentHostAvailable
		{
			get
			{
				return this._textBoxContentHost != null;
			}
		}

		// Token: 0x0600609E RID: 24734 RVA: 0x001B140C File Offset: 0x001AF60C
		private void DetachFromVisualTree()
		{
			if (this._textEditor != null)
			{
				this._textEditor.Selection.DetachFromVisualTree();
			}
			if (this.ScrollViewer != null)
			{
				this.ScrollViewer.ScrollChanged -= this.OnScrollChanged;
			}
			this._scrollViewer = null;
			this.ClearContentHost();
		}

		// Token: 0x0600609F RID: 24735 RVA: 0x001B1460 File Offset: 0x001AF660
		private void InitializeRenderScope()
		{
			if (this._renderScope == null)
			{
				return;
			}
			ITextView textView = (ITextView)((IServiceProvider)this._renderScope).GetService(typeof(ITextView));
			this.TextContainer.TextView = textView;
			this._textEditor.TextView = textView;
			if (this.ScrollViewer != null)
			{
				this.ScrollViewer.CanContentScroll = true;
			}
		}

		// Token: 0x060060A0 RID: 24736 RVA: 0x001B14C4 File Offset: 0x001AF6C4
		private void UninitializeRenderScope()
		{
			this._textEditor.TextView = null;
			TextBoxView textBoxView;
			if ((textBoxView = (this._renderScope as TextBoxView)) != null)
			{
				textBoxView.RemoveTextContainerListeners();
				return;
			}
			FlowDocumentView flowDocumentView;
			if ((flowDocumentView = (this._renderScope as FlowDocumentView)) != null)
			{
				if (flowDocumentView.Document != null)
				{
					flowDocumentView.Document.Uninitialize();
					flowDocumentView.Document = null;
					return;
				}
			}
			else
			{
				Invariant.Assert(this._renderScope == null, "_renderScope must be null here");
			}
		}

		// Token: 0x060060A1 RID: 24737 RVA: 0x001B1530 File Offset: 0x001AF730
		private static Brush GetDefaultSelectionBrush()
		{
			Brush brush = new SolidColorBrush(SystemColors.HighlightColor);
			brush.Freeze();
			return brush;
		}

		// Token: 0x060060A2 RID: 24738 RVA: 0x001B1550 File Offset: 0x001AF750
		private static Brush GetDefaultSelectionTextBrush()
		{
			Brush brush = new SolidColorBrush(SystemColors.HighlightTextColor);
			brush.Freeze();
			return brush;
		}

		// Token: 0x060060A3 RID: 24739 RVA: 0x001B156F File Offset: 0x001AF76F
		private static object OnPageHeightGetValue(DependencyObject d)
		{
			return ((TextBoxBase)d).ViewportHeight;
		}

		// Token: 0x060060A4 RID: 24740 RVA: 0x001B1584 File Offset: 0x001AF784
		private void SetRenderScopeToContentHost()
		{
			FrameworkElement renderScope = this.CreateRenderScope();
			this.ClearContentHost();
			this._textBoxContentHost = (base.GetTemplateChild("PART_ContentHost") as FrameworkElement);
			this._renderScope = renderScope;
			if (this._textBoxContentHost is ScrollViewer)
			{
				ScrollViewer scrollViewer = (ScrollViewer)this._textBoxContentHost;
				if (scrollViewer.Content != null)
				{
					this._renderScope = null;
					this._textBoxContentHost = null;
					throw new NotSupportedException(SR.Get("TextBoxScrollViewerMarkedAsTextBoxContentMustHaveNoContent"));
				}
				scrollViewer.Content = this._renderScope;
			}
			else if (this._textBoxContentHost is Decorator)
			{
				Decorator decorator = (Decorator)this._textBoxContentHost;
				if (decorator.Child != null)
				{
					this._renderScope = null;
					this._textBoxContentHost = null;
					throw new NotSupportedException(SR.Get("TextBoxDecoratorMarkedAsTextBoxContentMustHaveNoContent"));
				}
				decorator.Child = this._renderScope;
			}
			else
			{
				this._renderScope = null;
				if (this._textBoxContentHost != null)
				{
					this._textBoxContentHost = null;
					throw new NotSupportedException(SR.Get("TextBoxInvalidTextContainer"));
				}
			}
			this.InitializeRenderScope();
		}

		// Token: 0x060060A5 RID: 24741 RVA: 0x001B1684 File Offset: 0x001AF884
		private void ClearContentHost()
		{
			this.UninitializeRenderScope();
			if (this._textBoxContentHost is ScrollViewer)
			{
				((ScrollViewer)this._textBoxContentHost).Content = null;
			}
			else if (this._textBoxContentHost is Decorator)
			{
				((Decorator)this._textBoxContentHost).Child = null;
			}
			else
			{
				Invariant.Assert(this._textBoxContentHost == null, "_textBoxContentHost must be null here");
			}
			this._textBoxContentHost = null;
		}

		// Token: 0x060060A6 RID: 24742 RVA: 0x001B16F4 File Offset: 0x001AF8F4
		private static void OnIsReadOnlyCaretVisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase textBoxBase = (TextBoxBase)d;
			textBoxBase.TextSelectionInternal.UpdateCaretState(CaretScrollMethod.None);
			((ITextSelection)textBoxBase.TextSelectionInternal).RefreshCaret();
		}

		// Token: 0x060060A7 RID: 24743 RVA: 0x001B171F File Offset: 0x001AF91F
		internal virtual void OnScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (e.ViewportHeightChange != 0.0)
			{
				base.SetValue(TextEditor.PageHeightProperty, e.ViewportHeight);
			}
		}

		// Token: 0x060060A8 RID: 24744 RVA: 0x001B1748 File Offset: 0x001AF948
		private void OnSelectionChangedInternal(object sender, EventArgs e)
		{
			this.OnSelectionChanged(new RoutedEventArgs(TextBoxBase.SelectionChangedEvent));
		}

		// Token: 0x17001742 RID: 5954
		// (get) Token: 0x060060A9 RID: 24745 RVA: 0x001B175A File Offset: 0x001AF95A
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return TextBoxBase._dType;
			}
		}

		// Token: 0x060060AA RID: 24746 RVA: 0x001B1764 File Offset: 0x001AF964
		internal static void OnScrollViewerPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase textBoxBase = d as TextBoxBase;
			if (textBoxBase != null && textBoxBase.ScrollViewer != null)
			{
				object newValue = e.NewValue;
				if (newValue == DependencyProperty.UnsetValue)
				{
					textBoxBase.ScrollViewer.ClearValue(e.Property);
					return;
				}
				textBoxBase.ScrollViewer.SetValue(e.Property, newValue);
			}
		}

		// Token: 0x060060AB RID: 24747 RVA: 0x001B17BC File Offset: 0x001AF9BC
		private static void OnIsUndoEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase textBoxBase = (TextBoxBase)d;
			textBoxBase.ChangeUndoEnabled((bool)e.NewValue);
		}

		// Token: 0x060060AC RID: 24748 RVA: 0x001AD7D1 File Offset: 0x001AB9D1
		private static bool UndoLimitValidateValue(object value)
		{
			return (int)value >= -1;
		}

		// Token: 0x060060AD RID: 24749 RVA: 0x001B17E4 File Offset: 0x001AF9E4
		private static void OnUndoLimitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase textBoxBase = (TextBoxBase)d;
			textBoxBase.ChangeUndoLimit(e.NewValue);
		}

		// Token: 0x060060AE RID: 24750 RVA: 0x001B1808 File Offset: 0x001AFA08
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void OnInputMethodEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase textBoxBase = (TextBoxBase)d;
			if (textBoxBase.TextEditor != null && textBoxBase.TextEditor.TextStore != null)
			{
				bool flag = (bool)e.NewValue;
				if (flag && Keyboard.FocusedElement == textBoxBase)
				{
					textBoxBase.TextEditor.TextStore.OnGotFocus();
				}
			}
		}

		// Token: 0x060060AF RID: 24751 RVA: 0x001B185C File Offset: 0x001AFA5C
		private static void UpdateCaretElement(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase textBoxBase = (TextBoxBase)d;
			if (textBoxBase.TextSelectionInternal != null)
			{
				CaretElement caretElement = textBoxBase.TextSelectionInternal.CaretElement;
				if (caretElement != null)
				{
					if (e.Property == TextBoxBase.CaretBrushProperty)
					{
						caretElement.UpdateCaretBrush(TextSelection.GetCaretBrush(textBoxBase.TextEditor));
					}
					caretElement.InvalidateVisual();
				}
				TextBoxView textBoxView = ((textBoxBase != null) ? textBoxBase.RenderScope : null) as TextBoxView;
				TextBoxView textBoxView2 = textBoxView;
				if (textBoxView2 != null && ((ITextView)textBoxView2).RendersOwnSelection)
				{
					textBoxView.InvalidateArrange();
				}
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.IsReadOnly" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.IsReadOnly" /> dependency property.</returns>
		// Token: 0x040030F2 RID: 12530
		public static readonly DependencyProperty IsReadOnlyProperty = TextEditor.IsReadOnlyProperty.AddOwner(typeof(TextBoxBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.IsReadOnlyCaretVisible" /> dependency property.</summary>
		// Token: 0x040030F3 RID: 12531
		public static readonly DependencyProperty IsReadOnlyCaretVisibleProperty = DependencyProperty.Register("IsReadOnlyCaretVisible", typeof(bool), typeof(TextBoxBase), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(TextBoxBase.OnIsReadOnlyCaretVisiblePropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.AcceptsReturn" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.AcceptsReturn" /> dependency property.</returns>
		// Token: 0x040030F4 RID: 12532
		public static readonly DependencyProperty AcceptsReturnProperty = KeyboardNavigation.AcceptsReturnProperty.AddOwner(typeof(TextBoxBase));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.AcceptsTab" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.AcceptsTab" /> dependency property.</returns>
		// Token: 0x040030F5 RID: 12533
		public static readonly DependencyProperty AcceptsTabProperty = DependencyProperty.Register("AcceptsTab", typeof(bool), typeof(TextBoxBase), new FrameworkPropertyMetadata(false));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.HorizontalScrollBarVisibility" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.HorizontalScrollBarVisibility" /> dependency property.</returns>
		// Token: 0x040030F6 RID: 12534
		public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = ScrollViewer.HorizontalScrollBarVisibilityProperty.AddOwner(typeof(TextBoxBase), new FrameworkPropertyMetadata(ScrollBarVisibility.Hidden, new PropertyChangedCallback(TextBoxBase.OnScrollViewerPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.VerticalScrollBarVisibility" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.VerticalScrollBarVisibility" /> dependency property.</returns>
		// Token: 0x040030F7 RID: 12535
		public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = ScrollViewer.VerticalScrollBarVisibilityProperty.AddOwner(typeof(TextBoxBase), new FrameworkPropertyMetadata(ScrollBarVisibility.Hidden, new PropertyChangedCallback(TextBoxBase.OnScrollViewerPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.IsUndoEnabled" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.IsUndoEnabled" /> dependency property.</returns>
		// Token: 0x040030F8 RID: 12536
		public static readonly DependencyProperty IsUndoEnabledProperty = DependencyProperty.Register("IsUndoEnabled", typeof(bool), typeof(TextBoxBase), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(TextBoxBase.OnIsUndoEnabledChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.UndoLimit" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.UndoLimit" /> dependency property.</returns>
		// Token: 0x040030F9 RID: 12537
		public static readonly DependencyProperty UndoLimitProperty = DependencyProperty.Register("UndoLimit", typeof(int), typeof(TextBoxBase), new FrameworkPropertyMetadata(UndoManager.UndoLimitDefaultValue, new PropertyChangedCallback(TextBoxBase.OnUndoLimitChanged)), new ValidateValueCallback(TextBoxBase.UndoLimitValidateValue));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.AutoWordSelection" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.AutoWordSelection" /> dependency property.</returns>
		// Token: 0x040030FA RID: 12538
		public static readonly DependencyProperty AutoWordSelectionProperty = DependencyProperty.Register("AutoWordSelection", typeof(bool), typeof(TextBoxBase), new FrameworkPropertyMetadata(false));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.SelectionBrush" /> dependency property.</summary>
		// Token: 0x040030FB RID: 12539
		public static readonly DependencyProperty SelectionBrushProperty = DependencyProperty.Register("SelectionBrush", typeof(Brush), typeof(TextBoxBase), new FrameworkPropertyMetadata(TextBoxBase.GetDefaultSelectionBrush(), new PropertyChangedCallback(TextBoxBase.UpdateCaretElement)));

		// Token: 0x040030FC RID: 12540
		public static readonly DependencyProperty SelectionTextBrushProperty = DependencyProperty.Register("SelectionTextBrush", typeof(Brush), typeof(TextBoxBase), new FrameworkPropertyMetadata(TextBoxBase.GetDefaultSelectionTextBrush(), new PropertyChangedCallback(TextBoxBase.UpdateCaretElement)));

		// Token: 0x040030FD RID: 12541
		internal const double AdornerSelectionOpacityDefaultValue = 0.4;

		// Token: 0x040030FE RID: 12542
		internal const double NonAdornerSelectionOpacityDefaultValue = 1.0;

		// Token: 0x040030FF RID: 12543
		private static double SelectionOpacityDefaultValue = FrameworkAppContextSwitches.UseAdornerForTextboxSelectionRendering ? 0.4 : 1.0;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.SelectionOpacity" /> dependency property.</summary>
		// Token: 0x04003100 RID: 12544
		public static readonly DependencyProperty SelectionOpacityProperty = DependencyProperty.Register("SelectionOpacity", typeof(double), typeof(TextBoxBase), new FrameworkPropertyMetadata(TextBoxBase.SelectionOpacityDefaultValue, new PropertyChangedCallback(TextBoxBase.UpdateCaretElement)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.CaretBrush" /> dependency property.</summary>
		// Token: 0x04003101 RID: 12545
		public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register("CaretBrush", typeof(Brush), typeof(TextBoxBase), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(TextBoxBase.UpdateCaretElement)));

		// Token: 0x04003102 RID: 12546
		internal static readonly DependencyPropertyKey IsSelectionActivePropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsSelectionActive", typeof(bool), typeof(TextBoxBase), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.IsSelectionActive" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.IsSelectionActive" /> dependency property.</returns>
		// Token: 0x04003103 RID: 12547
		public static readonly DependencyProperty IsSelectionActiveProperty = TextBoxBase.IsSelectionActivePropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.IsInactiveSelectionHighlightEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TextBoxBase.IsInactiveSelectionHighlightEnabled" /> dependency property.</returns>
		// Token: 0x04003104 RID: 12548
		public static readonly DependencyProperty IsInactiveSelectionHighlightEnabledProperty = DependencyProperty.Register("IsInactiveSelectionHighlightEnabled", typeof(bool), typeof(TextBoxBase));

		// Token: 0x04003107 RID: 12551
		private static DependencyObjectType _dType;

		// Token: 0x04003108 RID: 12552
		private TextContainer _textContainer;

		// Token: 0x04003109 RID: 12553
		private TextEditor _textEditor;

		// Token: 0x0400310A RID: 12554
		private FrameworkElement _textBoxContentHost;

		// Token: 0x0400310B RID: 12555
		private FrameworkElement _renderScope;

		// Token: 0x0400310C RID: 12556
		private ScrollViewer _scrollViewer;

		// Token: 0x0400310D RID: 12557
		private UndoAction _pendingUndoAction;

		// Token: 0x0400310E RID: 12558
		internal const string ContentHostTemplateName = "PART_ContentHost";
	}
}
