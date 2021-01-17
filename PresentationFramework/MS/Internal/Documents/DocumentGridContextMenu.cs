using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MS.Internal.Documents
{
	// Token: 0x020006BD RID: 1725
	internal static class DocumentGridContextMenu
	{
		// Token: 0x06006F85 RID: 28549 RVA: 0x00200DA0 File Offset: 0x001FEFA0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void RegisterClassHandler()
		{
			EventManager.RegisterClassHandler(typeof(DocumentGrid), FrameworkElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(DocumentGridContextMenu.OnContextMenuOpening));
			EventManager.RegisterClassHandler(typeof(DocumentApplicationDocumentViewer), FrameworkElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(DocumentGridContextMenu.OnDocumentViewerContextMenuOpening));
		}

		// Token: 0x06006F86 RID: 28550 RVA: 0x00200DF0 File Offset: 0x001FEFF0
		[SecurityCritical]
		private static void OnDocumentViewerContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			if (e.CursorLeft == -1.0)
			{
				DocumentViewer documentViewer = sender as DocumentViewer;
				if (documentViewer != null && documentViewer.ScrollViewer != null)
				{
					DocumentGridContextMenu.OnContextMenuOpening(documentViewer.ScrollViewer.Content, e);
				}
			}
		}

		// Token: 0x06006F87 RID: 28551 RVA: 0x00200E34 File Offset: 0x001FF034
		[SecurityCritical]
		private static void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			DocumentGrid documentGrid = sender as DocumentGrid;
			if (documentGrid == null)
			{
				return;
			}
			if (!(documentGrid.DocumentViewerOwner is DocumentApplicationDocumentViewer))
			{
				return;
			}
			if (documentGrid.DocumentViewerOwner.ContextMenu != null || documentGrid.DocumentViewerOwner.ScrollViewer.ContextMenu != null)
			{
				return;
			}
			ContextMenu contextMenu = documentGrid.ContextMenu;
			if (documentGrid.ReadLocalValue(FrameworkElement.ContextMenuProperty) == null)
			{
				return;
			}
			if (contextMenu != null)
			{
				return;
			}
			contextMenu = new DocumentGridContextMenu.ViewerContextMenu();
			contextMenu.Placement = PlacementMode.RelativePoint;
			contextMenu.PlacementTarget = documentGrid;
			((DocumentGridContextMenu.ViewerContextMenu)contextMenu).AddMenuItems(documentGrid, e.UserInitiated);
			Point position;
			if (e.CursorLeft == -1.0)
			{
				position = new Point(0.5 * documentGrid.ViewportWidth, 0.5 * documentGrid.ViewportHeight);
			}
			else
			{
				position = Mouse.GetPosition(documentGrid);
			}
			contextMenu.HorizontalOffset = position.X;
			contextMenu.VerticalOffset = position.Y;
			contextMenu.IsOpen = true;
			e.Handled = true;
		}

		// Token: 0x040036BB RID: 14011
		private const double KeyboardInvokedSentinel = -1.0;

		// Token: 0x02000B32 RID: 2866
		private class ViewerContextMenu : ContextMenu
		{
			// Token: 0x06008D60 RID: 36192 RVA: 0x00259414 File Offset: 0x00257614
			[SecurityCritical]
			internal void AddMenuItems(DocumentGrid dg, bool userInitiated)
			{
				if (!userInitiated)
				{
					SecurityHelper.DemandAllClipboardPermission();
				}
				base.Name = "ViewerContextMenu";
				this.SetMenuProperties(new DocumentGridContextMenu.EditorMenuItem(), dg, ApplicationCommands.Copy);
				this.SetMenuProperties(new MenuItem(), dg, ApplicationCommands.SelectAll);
				this.AddSeparator();
				this.SetMenuProperties(new MenuItem(), dg, NavigationCommands.PreviousPage, SR.Get("DocumentApplicationContextMenuPreviousPageHeader"), SR.Get("DocumentApplicationContextMenuPreviousPageInputGesture"));
				this.SetMenuProperties(new MenuItem(), dg, NavigationCommands.NextPage, SR.Get("DocumentApplicationContextMenuNextPageHeader"), SR.Get("DocumentApplicationContextMenuNextPageInputGesture"));
				this.SetMenuProperties(new MenuItem(), dg, NavigationCommands.FirstPage, null, SR.Get("DocumentApplicationContextMenuFirstPageInputGesture"));
				this.SetMenuProperties(new MenuItem(), dg, NavigationCommands.LastPage, null, SR.Get("DocumentApplicationContextMenuLastPageInputGesture"));
				this.AddSeparator();
				this.SetMenuProperties(new MenuItem(), dg, ApplicationCommands.Print);
			}

			// Token: 0x06008D61 RID: 36193 RVA: 0x0024A0D7 File Offset: 0x002482D7
			private void AddSeparator()
			{
				base.Items.Add(new Separator());
			}

			// Token: 0x06008D62 RID: 36194 RVA: 0x002594F5 File Offset: 0x002576F5
			private void SetMenuProperties(MenuItem menuItem, DocumentGrid dg, RoutedUICommand command)
			{
				this.SetMenuProperties(menuItem, dg, command, null, null);
			}

			// Token: 0x06008D63 RID: 36195 RVA: 0x00259504 File Offset: 0x00257704
			private void SetMenuProperties(MenuItem menuItem, DocumentGrid dg, RoutedUICommand command, string header, string inputGestureText)
			{
				menuItem.Command = command;
				menuItem.CommandTarget = dg.DocumentViewerOwner;
				if (header == null)
				{
					menuItem.Header = command.Text;
				}
				else
				{
					menuItem.Header = header;
				}
				if (inputGestureText != null)
				{
					menuItem.InputGestureText = inputGestureText;
				}
				menuItem.Name = "ViewerContextMenu_" + command.Name;
				base.Items.Add(menuItem);
			}
		}

		// Token: 0x02000B33 RID: 2867
		private class EditorMenuItem : MenuItem
		{
			// Token: 0x06008D65 RID: 36197 RVA: 0x0024A46A File Offset: 0x0024866A
			internal EditorMenuItem()
			{
			}

			// Token: 0x06008D66 RID: 36198 RVA: 0x0024A472 File Offset: 0x00248672
			[SecurityCritical]
			internal override void OnClickCore(bool userInitiated)
			{
				base.OnClickImpl(userInitiated);
			}
		}
	}
}
