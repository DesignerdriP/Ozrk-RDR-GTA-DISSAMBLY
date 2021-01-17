using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Xml;
using MS.Internal;
using MS.Internal.Commands;

namespace System.Windows.Documents
{
	// Token: 0x020003F4 RID: 1012
	internal static class TextEditorCopyPaste
	{
		// Token: 0x0600384D RID: 14413 RVA: 0x000FB758 File Offset: 0x000F9958
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void _RegisterClassHandlers(Type controlType, bool acceptsRichContent, bool readOnly, bool registerEventListeners)
		{
			CommandHelpers.RegisterCommandHandler(controlType, ApplicationCommands.Copy, new ExecutedRoutedEventHandler(TextEditorCopyPaste.OnCopy), new CanExecuteRoutedEventHandler(TextEditorCopyPaste.OnQueryStatusCopy), KeyGesture.CreateFromResourceStrings(SR.Get("KeyCopy"), SR.Get("KeyCopyDisplayString")), KeyGesture.CreateFromResourceStrings(SR.Get("KeyCtrlInsert"), SR.Get("KeyCtrlInsertDisplayString")));
			if (acceptsRichContent)
			{
				CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.CopyFormat, new ExecutedRoutedEventHandler(TextEditorCopyPaste.OnCopyFormat), new CanExecuteRoutedEventHandler(TextEditorCopyPaste.OnQueryStatusCopyFormat), "KeyCopyFormat", "KeyCopyFormatDisplayString");
			}
			if (!readOnly)
			{
				CommandHelpers.RegisterCommandHandler(controlType, ApplicationCommands.Cut, new ExecutedRoutedEventHandler(TextEditorCopyPaste.OnCut), new CanExecuteRoutedEventHandler(TextEditorCopyPaste.OnQueryStatusCut), KeyGesture.CreateFromResourceStrings(SR.Get("KeyCut"), SR.Get("KeyCutDisplayString")), KeyGesture.CreateFromResourceStrings(SR.Get("KeyShiftDelete"), SR.Get("KeyShiftDeleteDisplayString")));
				ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(TextEditorCopyPaste.OnPaste);
				CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(TextEditorCopyPaste.OnQueryStatusPaste);
				InputGesture inputGesture = KeyGesture.CreateFromResourceStrings(SR.Get("KeyShiftInsert"), SR.Get("KeyShiftInsertDisplayString"));
				new UIPermission(UIPermissionClipboard.AllClipboard).Assert();
				try
				{
					CommandHelpers.RegisterCommandHandler(controlType, ApplicationCommands.Paste, executedRoutedEventHandler, canExecuteRoutedEventHandler, inputGesture);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				if (acceptsRichContent)
				{
					CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.PasteFormat, new ExecutedRoutedEventHandler(TextEditorCopyPaste.OnPasteFormat), new CanExecuteRoutedEventHandler(TextEditorCopyPaste.OnQueryStatusPasteFormat), "KeyPasteFormat", "KeyPasteFormatDisplayString");
				}
			}
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x000FB8DC File Offset: 0x000F9ADC
		[SecurityCritical]
		internal static DataObject _CreateDataObject(TextEditor This, bool isDragDrop)
		{
			new UIPermission(UIPermissionClipboard.AllClipboard).Assert();
			DataObject dataObject;
			try
			{
				dataObject = new DataObject();
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			string text = This.Selection.Text;
			if (text != string.Empty)
			{
				if (TextEditorCopyPaste.ConfirmDataFormatSetting(This.UiScope, dataObject, DataFormats.Text))
				{
					TextEditorCopyPaste.CriticalSetDataWrapper(dataObject, DataFormats.Text, text);
				}
				if (TextEditorCopyPaste.ConfirmDataFormatSetting(This.UiScope, dataObject, DataFormats.UnicodeText))
				{
					TextEditorCopyPaste.CriticalSetDataWrapper(dataObject, DataFormats.UnicodeText, text);
				}
			}
			if (This.AcceptsRichContent)
			{
				if (SecurityHelper.CheckUnmanagedCodePermission())
				{
					Stream stream = null;
					string text2 = WpfPayload.SaveRange(This.Selection, ref stream, false);
					if (text2.Length > 0)
					{
						if (stream != null && TextEditorCopyPaste.ConfirmDataFormatSetting(This.UiScope, dataObject, DataFormats.XamlPackage))
						{
							dataObject.SetData(DataFormats.XamlPackage, stream);
						}
						if (TextEditorCopyPaste.ConfirmDataFormatSetting(This.UiScope, dataObject, DataFormats.Rtf))
						{
							string text3 = TextEditorCopyPaste.ConvertXamlToRtf(text2, stream);
							if (text3 != string.Empty)
							{
								dataObject.SetData(DataFormats.Rtf, text3, true);
							}
						}
					}
					Image image = This.Selection.GetUIElementSelected() as Image;
					if (image != null && image.Source is BitmapSource)
					{
						dataObject.SetImage((BitmapSource)image.Source);
					}
				}
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
				TextRangeSerialization.WriteXaml(xmlWriter, This.Selection, false, null);
				string text4 = stringWriter.ToString();
				if (text4.Length > 0 && TextEditorCopyPaste.ConfirmDataFormatSetting(This.UiScope, dataObject, DataFormats.Xaml))
				{
					TextEditorCopyPaste.CriticalSetDataWrapper(dataObject, DataFormats.Xaml, text4);
					PermissionSet permissionSet = SecurityHelper.ExtractAppDomainPermissionSetMinusSiteOfOrigin();
					string content = permissionSet.ToString();
					TextEditorCopyPaste.CriticalSetDataWrapper(dataObject, DataFormats.ApplicationTrust, content);
				}
			}
			DataObjectCopyingEventArgs dataObjectCopyingEventArgs = new DataObjectCopyingEventArgs(dataObject, isDragDrop);
			This.UiScope.RaiseEvent(dataObjectCopyingEventArgs);
			if (dataObjectCopyingEventArgs.CommandCancelled)
			{
				dataObject = null;
			}
			return dataObject;
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x000FBAC0 File Offset: 0x000F9CC0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static bool _DoPaste(TextEditor This, IDataObject dataObject, bool isDragDrop)
		{
			if (!SecurityHelper.CallerHasAllClipboardPermission())
			{
				return false;
			}
			Invariant.Assert(dataObject != null);
			bool result = false;
			string formatToApply = TextEditorCopyPaste.GetPasteApplyFormat(This, dataObject);
			DataObjectPastingEventArgs dataObjectPastingEventArgs;
			try
			{
				dataObjectPastingEventArgs = new DataObjectPastingEventArgs(dataObject, isDragDrop, formatToApply);
			}
			catch (ArgumentException)
			{
				return result;
			}
			This.UiScope.RaiseEvent(dataObjectPastingEventArgs);
			if (!dataObjectPastingEventArgs.CommandCancelled)
			{
				IDataObject dataObject2 = dataObjectPastingEventArgs.DataObject;
				formatToApply = dataObjectPastingEventArgs.FormatToApply;
				result = TextEditorCopyPaste.PasteContentData(This, dataObject, dataObject2, formatToApply);
			}
			return result;
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x000FBB3C File Offset: 0x000F9D3C
		internal static string GetPasteApplyFormat(TextEditor This, IDataObject dataObject)
		{
			bool flag = SecurityHelper.CheckUnmanagedCodePermission();
			string result;
			if (This.AcceptsRichContent && flag && dataObject.GetDataPresent(DataFormats.XamlPackage))
			{
				result = DataFormats.XamlPackage;
			}
			else if (This.AcceptsRichContent && dataObject.GetDataPresent(DataFormats.Xaml))
			{
				result = DataFormats.Xaml;
			}
			else if (This.AcceptsRichContent && flag && dataObject.GetDataPresent(DataFormats.Rtf))
			{
				result = DataFormats.Rtf;
			}
			else if (dataObject.GetDataPresent(DataFormats.UnicodeText))
			{
				result = DataFormats.UnicodeText;
			}
			else if (dataObject.GetDataPresent(DataFormats.Text))
			{
				result = DataFormats.Text;
			}
			else if (This.AcceptsRichContent && flag && dataObject is DataObject && ((DataObject)dataObject).ContainsImage())
			{
				result = DataFormats.Bitmap;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x000FBC08 File Offset: 0x000F9E08
		[SecurityCritical]
		internal static void Cut(TextEditor This, bool userInitiated)
		{
			if (userInitiated)
			{
				try
				{
					new UIPermission(UIPermissionClipboard.OwnClipboard).Demand();
					goto IL_1E;
				}
				catch (SecurityException)
				{
					return;
				}
			}
			if (!SecurityHelper.CallerHasAllClipboardPermission())
			{
				return;
			}
			IL_1E:
			TextEditorTyping._FlushPendingInputItems(This);
			TextEditorTyping._BreakTypingSequence(This);
			if (This.Selection != null && !This.Selection.IsEmpty)
			{
				DataObject dataObject = TextEditorCopyPaste._CreateDataObject(This, false);
				if (dataObject != null)
				{
					try
					{
						Clipboard.CriticalSetDataObject(dataObject, true);
					}
					catch (ExternalException obj) when (!FrameworkCompatibilityPreferences.ShouldThrowOnCopyOrCutFailure)
					{
						return;
					}
					using (This.Selection.DeclareChangeBlock())
					{
						TextEditorSelection._ClearSuggestedX(This);
						This.Selection.Text = string.Empty;
						if (This.Selection is TextSelection)
						{
							((TextSelection)This.Selection).ClearSpringloadFormatting();
						}
					}
				}
			}
		}

		// Token: 0x06003852 RID: 14418 RVA: 0x000FBCFC File Offset: 0x000F9EFC
		[SecurityCritical]
		internal static void Copy(TextEditor This, bool userInitiated)
		{
			if (userInitiated)
			{
				try
				{
					new UIPermission(UIPermissionClipboard.OwnClipboard).Demand();
					goto IL_1B;
				}
				catch (SecurityException)
				{
					return;
				}
			}
			if (!SecurityHelper.CallerHasAllClipboardPermission())
			{
				return;
			}
			IL_1B:
			TextEditorTyping._FlushPendingInputItems(This);
			TextEditorTyping._BreakTypingSequence(This);
			if (This.Selection != null && !This.Selection.IsEmpty)
			{
				DataObject dataObject = TextEditorCopyPaste._CreateDataObject(This, false);
				if (dataObject != null)
				{
					try
					{
						Clipboard.CriticalSetDataObject(dataObject, true);
					}
					catch (ExternalException obj) when (!FrameworkCompatibilityPreferences.ShouldThrowOnCopyOrCutFailure)
					{
					}
				}
			}
		}

		// Token: 0x06003853 RID: 14419 RVA: 0x000FBD94 File Offset: 0x000F9F94
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void Paste(TextEditor This)
		{
			if (!SecurityHelper.CallerHasAllClipboardPermission())
			{
				return;
			}
			if (This.Selection.IsTableCellRange)
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(This);
			TextEditorTyping._BreakTypingSequence(This);
			IDataObject dataObject;
			try
			{
				dataObject = Clipboard.GetDataObject();
			}
			catch (ExternalException)
			{
				dataObject = null;
			}
			bool coversEntireContent = This.Selection.CoversEntireContent;
			if (dataObject != null)
			{
				using (This.Selection.DeclareChangeBlock())
				{
					TextEditorSelection._ClearSuggestedX(This);
					if (TextEditorCopyPaste._DoPaste(This, dataObject, false))
					{
						This.Selection.SetCaretToPosition(This.Selection.End, LogicalDirection.Backward, false, true);
						if (This.Selection is TextSelection)
						{
							((TextSelection)This.Selection).ClearSpringloadFormatting();
						}
					}
				}
			}
			if (coversEntireContent)
			{
				This.Selection.ValidateLayout();
			}
		}

		// Token: 0x06003854 RID: 14420 RVA: 0x000FBE68 File Offset: 0x000FA068
		internal static string ConvertXamlToRtf(string xamlContent, Stream wpfContainerMemory)
		{
			XamlRtfConverter xamlRtfConverter = new XamlRtfConverter();
			if (wpfContainerMemory != null)
			{
				xamlRtfConverter.WpfPayload = WpfPayload.OpenWpfPayload(wpfContainerMemory);
			}
			return xamlRtfConverter.ConvertXamlToRtf(xamlContent);
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x000FBE94 File Offset: 0x000FA094
		internal static MemoryStream ConvertRtfToXaml(string rtfContent)
		{
			MemoryStream memoryStream = new MemoryStream();
			WpfPayload wpfPayload = WpfPayload.CreateWpfPayload(memoryStream);
			using (wpfPayload.Package)
			{
				using (Stream stream = wpfPayload.CreateXamlStream())
				{
					string text = new XamlRtfConverter
					{
						WpfPayload = wpfPayload
					}.ConvertRtfToXaml(rtfContent);
					if (text != string.Empty)
					{
						StreamWriter streamWriter = new StreamWriter(stream);
						using (streamWriter)
						{
							streamWriter.Write(text);
							return memoryStream;
						}
					}
					memoryStream = null;
				}
			}
			return memoryStream;
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x000FBF48 File Offset: 0x000FA148
		private static void OnQueryStatusCut(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			if (textEditor.UiScope is PasswordBox)
			{
				args.CanExecute = false;
				args.Handled = true;
				return;
			}
			args.CanExecute = !textEditor.Selection.IsEmpty;
			args.Handled = true;
		}

		// Token: 0x06003857 RID: 14423 RVA: 0x000FBFA8 File Offset: 0x000FA1A8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void OnCut(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			if (textEditor.UiScope is PasswordBox)
			{
				return;
			}
			TextEditorCopyPaste.Cut(textEditor, args.UserInitiated);
		}

		// Token: 0x06003858 RID: 14424 RVA: 0x000FBFEC File Offset: 0x000FA1EC
		private static void OnQueryStatusCopy(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled)
			{
				return;
			}
			if (textEditor.UiScope is PasswordBox)
			{
				args.CanExecute = false;
				args.Handled = true;
				return;
			}
			args.CanExecute = !textEditor.Selection.IsEmpty;
			args.Handled = true;
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x000FC044 File Offset: 0x000FA244
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void OnCopy(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled)
			{
				return;
			}
			if (textEditor.UiScope is PasswordBox)
			{
				return;
			}
			TextEditorCopyPaste.Copy(textEditor, args.UserInitiated);
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x000FC080 File Offset: 0x000FA280
		private static void OnQueryStatusPaste(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			args.Handled = true;
			try
			{
				if (SecurityHelper.CallerHasAllClipboardPermission())
				{
					string pasteApplyFormat = TextEditorCopyPaste.GetPasteApplyFormat(textEditor, Clipboard.GetDataObject());
					args.CanExecute = (pasteApplyFormat.Length > 0);
				}
				else
				{
					args.CanExecute = Clipboard.IsClipboardPopulated();
				}
			}
			catch (ExternalException)
			{
				args.CanExecute = false;
			}
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x000FC0FC File Offset: 0x000FA2FC
		private static void OnPaste(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			TextEditorCopyPaste.Paste(textEditor);
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x000FC12C File Offset: 0x000FA32C
		private static void OnQueryStatusCopyFormat(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled)
			{
				return;
			}
			args.CanExecute = false;
			args.Handled = true;
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x00002137 File Offset: 0x00000337
		private static void OnCopyFormat(object sender, ExecutedRoutedEventArgs args)
		{
		}

		// Token: 0x0600385E RID: 14430 RVA: 0x000FC15C File Offset: 0x000FA35C
		private static void OnQueryStatusPasteFormat(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			args.CanExecute = false;
			args.Handled = true;
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x00002137 File Offset: 0x00000337
		private static void OnPasteFormat(object sender, ExecutedRoutedEventArgs args)
		{
		}

		// Token: 0x06003860 RID: 14432 RVA: 0x000FC192 File Offset: 0x000FA392
		[SecurityCritical]
		private static void CriticalSetDataWrapper(IDataObject dataObjectValue, string format, string content)
		{
			if (dataObjectValue is DataObject)
			{
				((DataObject)dataObjectValue).CriticalSetData(format, content, !(format == DataFormats.ApplicationTrust));
			}
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x000FC1BC File Offset: 0x000FA3BC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static bool PasteContentData(TextEditor This, IDataObject dataObject, IDataObject dataObjectToApply, string formatToApply)
		{
			if (formatToApply == DataFormats.Bitmap && dataObjectToApply is DataObject && This.AcceptsRichContent && This.Selection is TextSelection && SecurityHelper.CheckUnmanagedCodePermission())
			{
				BitmapSource bitmapSource = TextEditorCopyPaste.GetPasteData(dataObjectToApply, DataFormats.Bitmap) as BitmapSource;
				if (bitmapSource != null)
				{
					MemoryStream data = WpfPayload.SaveImage(bitmapSource, "image/bmp");
					dataObjectToApply = new DataObject();
					formatToApply = DataFormats.XamlPackage;
					dataObjectToApply.SetData(DataFormats.XamlPackage, data);
				}
			}
			if (formatToApply == DataFormats.XamlPackage)
			{
				if (This.AcceptsRichContent && This.Selection is TextSelection && SecurityHelper.CheckUnmanagedCodePermission())
				{
					object pasteData = TextEditorCopyPaste.GetPasteData(dataObjectToApply, DataFormats.XamlPackage);
					MemoryStream memoryStream = pasteData as MemoryStream;
					if (memoryStream != null)
					{
						object obj = WpfPayload.LoadElement(memoryStream);
						if ((obj is Section || obj is Span) && TextEditorCopyPaste.PasteTextElement(This, (TextElement)obj))
						{
							return true;
						}
						if (obj is FrameworkElement)
						{
							((TextSelection)This.Selection).InsertEmbeddedUIElement((FrameworkElement)obj);
							return true;
						}
					}
				}
				dataObjectToApply = dataObject;
				if (dataObjectToApply.GetDataPresent(DataFormats.Xaml))
				{
					formatToApply = DataFormats.Xaml;
				}
				else if (SecurityHelper.CheckUnmanagedCodePermission() && dataObjectToApply.GetDataPresent(DataFormats.Rtf))
				{
					formatToApply = DataFormats.Rtf;
				}
				else if (dataObjectToApply.GetDataPresent(DataFormats.UnicodeText))
				{
					formatToApply = DataFormats.UnicodeText;
				}
				else if (dataObjectToApply.GetDataPresent(DataFormats.Text))
				{
					formatToApply = DataFormats.Text;
				}
			}
			if (formatToApply == DataFormats.Xaml)
			{
				if (This.AcceptsRichContent && This.Selection is TextSelection)
				{
					object pasteData2 = TextEditorCopyPaste.GetPasteData(dataObjectToApply, DataFormats.Xaml);
					if (pasteData2 != null && TextEditorCopyPaste.PasteXaml(This, pasteData2.ToString()))
					{
						return true;
					}
				}
				dataObjectToApply = dataObject;
				if (SecurityHelper.CheckUnmanagedCodePermission() && dataObjectToApply.GetDataPresent(DataFormats.Rtf))
				{
					formatToApply = DataFormats.Rtf;
				}
				else if (dataObjectToApply.GetDataPresent(DataFormats.UnicodeText))
				{
					formatToApply = DataFormats.UnicodeText;
				}
				else if (dataObjectToApply.GetDataPresent(DataFormats.Text))
				{
					formatToApply = DataFormats.Text;
				}
			}
			if (formatToApply == DataFormats.Rtf)
			{
				if (This.AcceptsRichContent && SecurityHelper.CheckUnmanagedCodePermission())
				{
					object pasteData3 = TextEditorCopyPaste.GetPasteData(dataObjectToApply, DataFormats.Rtf);
					if (pasteData3 != null)
					{
						MemoryStream memoryStream2 = TextEditorCopyPaste.ConvertRtfToXaml(pasteData3.ToString());
						if (memoryStream2 != null)
						{
							TextElement textElement = WpfPayload.LoadElement(memoryStream2) as TextElement;
							if ((textElement is Section || textElement is Span) && TextEditorCopyPaste.PasteTextElement(This, textElement))
							{
								return true;
							}
						}
					}
				}
				dataObjectToApply = dataObject;
				if (dataObjectToApply.GetDataPresent(DataFormats.UnicodeText))
				{
					formatToApply = DataFormats.UnicodeText;
				}
				else if (dataObjectToApply.GetDataPresent(DataFormats.Text))
				{
					formatToApply = DataFormats.Text;
				}
			}
			if (formatToApply == DataFormats.UnicodeText)
			{
				object pasteData4 = TextEditorCopyPaste.GetPasteData(dataObjectToApply, DataFormats.UnicodeText);
				if (pasteData4 != null)
				{
					return TextEditorCopyPaste.PastePlainText(This, pasteData4.ToString());
				}
				if (dataObjectToApply.GetDataPresent(DataFormats.Text))
				{
					formatToApply = DataFormats.Text;
					dataObjectToApply = dataObject;
				}
			}
			if (formatToApply == DataFormats.Text)
			{
				object pasteData5 = TextEditorCopyPaste.GetPasteData(dataObjectToApply, DataFormats.Text);
				if (pasteData5 != null && TextEditorCopyPaste.PastePlainText(This, pasteData5.ToString()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003862 RID: 14434 RVA: 0x000FC4C4 File Offset: 0x000FA6C4
		[SecurityCritical]
		private static object GetPasteData(IDataObject dataObject, string dataFormat)
		{
			object result;
			try
			{
				result = dataObject.GetData(dataFormat, true);
			}
			catch (OutOfMemoryException)
			{
				result = null;
			}
			catch (ExternalException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003863 RID: 14435 RVA: 0x000FC504 File Offset: 0x000FA704
		private static bool PasteTextElement(TextEditor This, TextElement sectionOrSpan)
		{
			bool result = false;
			This.Selection.BeginChange();
			try
			{
				((TextRange)This.Selection).SetXmlVirtual(sectionOrSpan);
				TextRangeEditLists.MergeListsAroundNormalizedPosition((TextPointer)This.Selection.Start);
				TextRangeEditLists.MergeListsAroundNormalizedPosition((TextPointer)This.Selection.End);
				TextRangeEdit.MergeFlowDirection((TextPointer)This.Selection.Start);
				TextRangeEdit.MergeFlowDirection((TextPointer)This.Selection.End);
				result = true;
			}
			finally
			{
				This.Selection.EndChange();
			}
			return result;
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x000FC5A8 File Offset: 0x000FA7A8
		private static bool PasteXaml(TextEditor This, string pasteXaml)
		{
			bool result;
			if (pasteXaml.Length == 0)
			{
				result = false;
			}
			else
			{
				try
				{
					bool useRestrictiveXamlReader = !Clipboard.UseLegacyDangerousClipboardDeserializationMode();
					object obj = XamlReader.Load(new XmlTextReader(new StringReader(pasteXaml)), useRestrictiveXamlReader);
					TextElement textElement = obj as TextElement;
					result = (textElement != null && TextEditorCopyPaste.PasteTextElement(This, textElement));
				}
				catch (XamlParseException ex)
				{
					Invariant.Assert(ex != null);
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06003865 RID: 14437 RVA: 0x000FC618 File Offset: 0x000FA818
		private static bool PastePlainText(TextEditor This, string pastedText)
		{
			pastedText = This._FilterText(pastedText, This.Selection);
			if (pastedText.Length > 0)
			{
				if (This.AcceptsRichContent && This.Selection.Start is TextPointer)
				{
					This.Selection.Text = string.Empty;
					TextPointer textPointer = TextRangeEditTables.EnsureInsertionPosition((TextPointer)This.Selection.Start);
					textPointer = textPointer.GetPositionAtOffset(0, LogicalDirection.Backward);
					TextPointer textPointer2 = textPointer.GetPositionAtOffset(0, LogicalDirection.Forward);
					int num = 0;
					for (int i = 0; i < pastedText.Length; i++)
					{
						if (pastedText[i] == '\r' || pastedText[i] == '\n')
						{
							textPointer2.InsertTextInRun(pastedText.Substring(num, i - num));
							if (!This.AcceptsReturn)
							{
								return true;
							}
							if (textPointer2.HasNonMergeableInlineAncestor)
							{
								textPointer2.InsertTextInRun(" ");
							}
							else
							{
								textPointer2 = textPointer2.InsertParagraphBreak();
							}
							if (pastedText[i] == '\r' && i + 1 < pastedText.Length && pastedText[i + 1] == '\n')
							{
								i++;
							}
							num = i + 1;
						}
					}
					textPointer2.InsertTextInRun(pastedText.Substring(num, pastedText.Length - num));
					This.Selection.Select(textPointer, textPointer2);
				}
				else
				{
					This.Selection.Text = pastedText;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003866 RID: 14438 RVA: 0x000FC758 File Offset: 0x000FA958
		private static bool ConfirmDataFormatSetting(FrameworkElement uiScope, IDataObject dataObject, string format)
		{
			DataObjectSettingDataEventArgs dataObjectSettingDataEventArgs = new DataObjectSettingDataEventArgs(dataObject, format);
			uiScope.RaiseEvent(dataObjectSettingDataEventArgs);
			return !dataObjectSettingDataEventArgs.CommandCancelled;
		}
	}
}
