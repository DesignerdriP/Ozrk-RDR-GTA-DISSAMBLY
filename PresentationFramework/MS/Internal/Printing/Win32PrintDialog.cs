using System;
using System.Printing;
using System.Printing.Interop;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using MS.Win32;

namespace MS.Internal.Printing
{
	// Token: 0x02000658 RID: 1624
	internal class Win32PrintDialog
	{
		// Token: 0x06006BCD RID: 27597 RVA: 0x001F0F40 File Offset: 0x001EF140
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public Win32PrintDialog()
		{
			this._printTicket = null;
			this._printQueue = null;
			this._minPage = 1U;
			this._maxPage = 9999U;
			this._pageRangeSelection = PageRangeSelection.AllPages;
		}

		// Token: 0x06006BCE RID: 27598 RVA: 0x001F0F70 File Offset: 0x001EF170
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal uint ShowDialog()
		{
			uint result = 0U;
			IntPtr intPtr = IntPtr.Zero;
			if (Application.Current != null && Application.Current.MainWindow != null)
			{
				WindowInteropHelper windowInteropHelper = new WindowInteropHelper(Application.Current.MainWindow);
				intPtr = windowInteropHelper.CriticalHandle;
			}
			try
			{
				if (this._printQueue == null || this._printTicket == null)
				{
					this.ProbeForPrintingSupport();
				}
				using (Win32PrintDialog.PrintDlgExMarshaler printDlgExMarshaler = new Win32PrintDialog.PrintDlgExMarshaler(intPtr, this))
				{
					printDlgExMarshaler.SyncToStruct();
					if (UnsafeNativeMethods.PrintDlgEx(printDlgExMarshaler.UnmanagedPrintDlgEx) == 0)
					{
						result = printDlgExMarshaler.SyncFromStruct();
					}
				}
			}
			catch (Exception ex)
			{
				if (!string.Equals(ex.GetType().FullName, "System.Printing.PrintingNotSupportedException", StringComparison.Ordinal))
				{
					throw;
				}
				string text = SR.Get("PrintDialogInstallPrintSupportMessageBox");
				string text2 = SR.Get("PrintDialogInstallPrintSupportCaption");
				MessageBoxOptions messageBoxOptions = (text2 != null && text2.Length > 0 && text2[0] == '‏') ? MessageBoxOptions.RtlReading : MessageBoxOptions.None;
				int type = (int)((MessageBoxOptions)64 | messageBoxOptions);
				if (intPtr == IntPtr.Zero)
				{
					intPtr = UnsafeNativeMethods.GetActiveWindow();
				}
				if (UnsafeNativeMethods.MessageBox(new HandleRef(null, intPtr), text, text2, type) != 0)
				{
					result = 0U;
				}
			}
			return result;
		}

		// Token: 0x170019C1 RID: 6593
		// (get) Token: 0x06006BCF RID: 27599 RVA: 0x001F10B4 File Offset: 0x001EF2B4
		// (set) Token: 0x06006BD0 RID: 27600 RVA: 0x001F10BC File Offset: 0x001EF2BC
		internal PrintTicket PrintTicket
		{
			[SecurityCritical]
			get
			{
				return this._printTicket;
			}
			[SecurityCritical]
			set
			{
				this._printTicket = value;
			}
		}

		// Token: 0x170019C2 RID: 6594
		// (get) Token: 0x06006BD1 RID: 27601 RVA: 0x001F10C5 File Offset: 0x001EF2C5
		// (set) Token: 0x06006BD2 RID: 27602 RVA: 0x001F10CD File Offset: 0x001EF2CD
		internal PrintQueue PrintQueue
		{
			[SecurityCritical]
			get
			{
				return this._printQueue;
			}
			[SecurityCritical]
			set
			{
				this._printQueue = value;
			}
		}

		// Token: 0x170019C3 RID: 6595
		// (get) Token: 0x06006BD3 RID: 27603 RVA: 0x001F10D6 File Offset: 0x001EF2D6
		// (set) Token: 0x06006BD4 RID: 27604 RVA: 0x001F10DE File Offset: 0x001EF2DE
		internal uint MinPage
		{
			get
			{
				return this._minPage;
			}
			set
			{
				this._minPage = value;
			}
		}

		// Token: 0x170019C4 RID: 6596
		// (get) Token: 0x06006BD5 RID: 27605 RVA: 0x001F10E7 File Offset: 0x001EF2E7
		// (set) Token: 0x06006BD6 RID: 27606 RVA: 0x001F10EF File Offset: 0x001EF2EF
		internal uint MaxPage
		{
			get
			{
				return this._maxPage;
			}
			set
			{
				this._maxPage = value;
			}
		}

		// Token: 0x170019C5 RID: 6597
		// (get) Token: 0x06006BD7 RID: 27607 RVA: 0x001F10F8 File Offset: 0x001EF2F8
		// (set) Token: 0x06006BD8 RID: 27608 RVA: 0x001F1100 File Offset: 0x001EF300
		internal PageRangeSelection PageRangeSelection
		{
			get
			{
				return this._pageRangeSelection;
			}
			set
			{
				this._pageRangeSelection = value;
			}
		}

		// Token: 0x170019C6 RID: 6598
		// (get) Token: 0x06006BD9 RID: 27609 RVA: 0x001F1109 File Offset: 0x001EF309
		// (set) Token: 0x06006BDA RID: 27610 RVA: 0x001F1111 File Offset: 0x001EF311
		internal PageRange PageRange
		{
			get
			{
				return this._pageRange;
			}
			set
			{
				this._pageRange = value;
			}
		}

		// Token: 0x170019C7 RID: 6599
		// (get) Token: 0x06006BDB RID: 27611 RVA: 0x001F111A File Offset: 0x001EF31A
		// (set) Token: 0x06006BDC RID: 27612 RVA: 0x001F1122 File Offset: 0x001EF322
		internal bool PageRangeEnabled
		{
			get
			{
				return this._pageRangeEnabled;
			}
			set
			{
				this._pageRangeEnabled = value;
			}
		}

		// Token: 0x170019C8 RID: 6600
		// (get) Token: 0x06006BDD RID: 27613 RVA: 0x001F112B File Offset: 0x001EF32B
		// (set) Token: 0x06006BDE RID: 27614 RVA: 0x001F1133 File Offset: 0x001EF333
		internal bool SelectedPagesEnabled
		{
			get
			{
				return this._selectedPagesEnabled;
			}
			set
			{
				this._selectedPagesEnabled = value;
			}
		}

		// Token: 0x170019C9 RID: 6601
		// (get) Token: 0x06006BDF RID: 27615 RVA: 0x001F113C File Offset: 0x001EF33C
		// (set) Token: 0x06006BE0 RID: 27616 RVA: 0x001F1144 File Offset: 0x001EF344
		internal bool CurrentPageEnabled
		{
			get
			{
				return this._currentPageEnabled;
			}
			set
			{
				this._currentPageEnabled = value;
			}
		}

		// Token: 0x06006BE1 RID: 27617 RVA: 0x001F1150 File Offset: 0x001EF350
		[SecurityCritical]
		private void ProbeForPrintingSupport()
		{
			string deviceName = (this._printQueue != null) ? this._printQueue.FullName : string.Empty;
			SystemDrawingHelper.NewDefaultPrintingPermission().Assert();
			try
			{
				using (new PrintTicketConverter(deviceName, 1))
				{
				}
			}
			catch (PrintQueueException)
			{
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x040034F2 RID: 13554
		[SecurityCritical]
		private PrintTicket _printTicket;

		// Token: 0x040034F3 RID: 13555
		[SecurityCritical]
		private PrintQueue _printQueue;

		// Token: 0x040034F4 RID: 13556
		private PageRangeSelection _pageRangeSelection;

		// Token: 0x040034F5 RID: 13557
		private PageRange _pageRange;

		// Token: 0x040034F6 RID: 13558
		private bool _pageRangeEnabled;

		// Token: 0x040034F7 RID: 13559
		private bool _selectedPagesEnabled;

		// Token: 0x040034F8 RID: 13560
		private bool _currentPageEnabled;

		// Token: 0x040034F9 RID: 13561
		private uint _minPage;

		// Token: 0x040034FA RID: 13562
		private uint _maxPage;

		// Token: 0x040034FB RID: 13563
		private const char RightToLeftMark = '‏';

		// Token: 0x02000B18 RID: 2840
		private sealed class PrintDlgExMarshaler : IDisposable
		{
			// Token: 0x06008D06 RID: 36102 RVA: 0x00257EAE File Offset: 0x002560AE
			[SecurityCritical]
			internal PrintDlgExMarshaler(IntPtr owner, Win32PrintDialog dialog)
			{
				this._ownerHandle = owner;
				this._dialog = dialog;
				this._unmanagedPrintDlgEx = IntPtr.Zero;
			}

			// Token: 0x06008D07 RID: 36103 RVA: 0x00257ED0 File Offset: 0x002560D0
			~PrintDlgExMarshaler()
			{
				this.Dispose(true);
			}

			// Token: 0x17001F69 RID: 8041
			// (get) Token: 0x06008D08 RID: 36104 RVA: 0x00257F00 File Offset: 0x00256100
			internal IntPtr UnmanagedPrintDlgEx
			{
				[SecurityCritical]
				get
				{
					return this._unmanagedPrintDlgEx;
				}
			}

			// Token: 0x06008D09 RID: 36105 RVA: 0x00257F08 File Offset: 0x00256108
			[SecurityCritical]
			internal uint SyncFromStruct()
			{
				if (this._unmanagedPrintDlgEx == IntPtr.Zero)
				{
					return 0U;
				}
				uint num = this.AcquireResultFromPrintDlgExStruct(this._unmanagedPrintDlgEx);
				if (num == 1U || num == 2U)
				{
					string text;
					uint num2;
					PageRange pageRange;
					IntPtr devModeHandle;
					this.ExtractPrintDataAndDevMode(this._unmanagedPrintDlgEx, out text, out num2, out pageRange, out devModeHandle);
					this._dialog.PrintQueue = this.AcquirePrintQueue(text);
					this._dialog.PrintTicket = this.AcquirePrintTicket(devModeHandle, text);
					if ((num2 & 2U) == 2U)
					{
						if (pageRange.PageFrom > pageRange.PageTo)
						{
							int pageTo = pageRange.PageTo;
							pageRange.PageTo = pageRange.PageFrom;
							pageRange.PageFrom = pageTo;
						}
						this._dialog.PageRangeSelection = PageRangeSelection.UserPages;
						this._dialog.PageRange = pageRange;
					}
					else if ((num2 & 1U) == 1U)
					{
						this._dialog.PageRangeSelection = PageRangeSelection.SelectedPages;
					}
					else if ((num2 & 4194304U) == 4194304U)
					{
						this._dialog.PageRangeSelection = PageRangeSelection.CurrentPage;
					}
					else
					{
						this._dialog.PageRangeSelection = PageRangeSelection.AllPages;
					}
				}
				return num;
			}

			// Token: 0x06008D0A RID: 36106 RVA: 0x00258008 File Offset: 0x00256208
			[SecurityCritical]
			internal void SyncToStruct()
			{
				if (this._unmanagedPrintDlgEx != IntPtr.Zero)
				{
					this.FreeUnmanagedPrintDlgExStruct(this._unmanagedPrintDlgEx);
				}
				if (this._ownerHandle == IntPtr.Zero)
				{
					this._ownerHandle = UnsafeNativeMethods.GetDesktopWindow();
				}
				this._unmanagedPrintDlgEx = this.AllocateUnmanagedPrintDlgExStruct();
			}

			// Token: 0x06008D0B RID: 36107 RVA: 0x0025805C File Offset: 0x0025625C
			[SecurityCritical]
			[SecurityTreatAsSafe]
			private void Dispose(bool disposing)
			{
				if (disposing && this._unmanagedPrintDlgEx != IntPtr.Zero)
				{
					this.FreeUnmanagedPrintDlgExStruct(this._unmanagedPrintDlgEx);
					this._unmanagedPrintDlgEx = IntPtr.Zero;
				}
			}

			// Token: 0x06008D0C RID: 36108 RVA: 0x0025808C File Offset: 0x0025628C
			[SecurityCritical]
			private void ExtractPrintDataAndDevMode(IntPtr unmanagedBuffer, out string printerName, out uint flags, out PageRange pageRange, out IntPtr devModeHandle)
			{
				IntPtr intPtr = IntPtr.Zero;
				IntPtr intPtr2 = IntPtr.Zero;
				if (!this.Is64Bit())
				{
					NativeMethods.PRINTDLGEX32 printdlgex = (NativeMethods.PRINTDLGEX32)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX32));
					devModeHandle = printdlgex.hDevMode;
					intPtr = printdlgex.hDevNames;
					flags = printdlgex.Flags;
					intPtr2 = printdlgex.lpPageRanges;
				}
				else
				{
					NativeMethods.PRINTDLGEX64 printdlgex2 = (NativeMethods.PRINTDLGEX64)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX64));
					devModeHandle = printdlgex2.hDevMode;
					intPtr = printdlgex2.hDevNames;
					flags = printdlgex2.Flags;
					intPtr2 = printdlgex2.lpPageRanges;
				}
				if ((flags & 2U) == 2U && intPtr2 != IntPtr.Zero)
				{
					NativeMethods.PRINTPAGERANGE printpagerange = (NativeMethods.PRINTPAGERANGE)Marshal.PtrToStructure(intPtr2, typeof(NativeMethods.PRINTPAGERANGE));
					pageRange = new PageRange((int)printpagerange.nFromPage, (int)printpagerange.nToPage);
				}
				else
				{
					pageRange = new PageRange(1);
				}
				if (intPtr != IntPtr.Zero)
				{
					IntPtr intPtr3 = IntPtr.Zero;
					try
					{
						intPtr3 = UnsafeNativeMethods.GlobalLock(intPtr);
						NativeMethods.DEVNAMES devnames = (NativeMethods.DEVNAMES)Marshal.PtrToStructure(intPtr3, typeof(NativeMethods.DEVNAMES));
						int offset = checked((int)devnames.wDeviceOffset * Marshal.SystemDefaultCharSize);
						printerName = Marshal.PtrToStringAuto(intPtr3 + offset);
						return;
					}
					finally
					{
						if (intPtr3 != IntPtr.Zero)
						{
							UnsafeNativeMethods.GlobalUnlock(intPtr);
						}
					}
				}
				printerName = string.Empty;
			}

			// Token: 0x06008D0D RID: 36109 RVA: 0x002581F0 File Offset: 0x002563F0
			[SecurityCritical]
			private PrintQueue AcquirePrintQueue(string printerName)
			{
				PrintQueue printQueue = null;
				EnumeratedPrintQueueTypes[] enumerationFlag = new EnumeratedPrintQueueTypes[]
				{
					EnumeratedPrintQueueTypes.Local,
					EnumeratedPrintQueueTypes.Connections
				};
				PrintQueueIndexedProperty[] propertiesFilter = new PrintQueueIndexedProperty[]
				{
					PrintQueueIndexedProperty.Name,
					PrintQueueIndexedProperty.QueueAttributes
				};
				SystemDrawingHelper.NewDefaultPrintingPermission().Assert();
				try
				{
					using (LocalPrintServer localPrintServer = new LocalPrintServer())
					{
						foreach (PrintQueue printQueue2 in localPrintServer.GetPrintQueues(propertiesFilter, enumerationFlag))
						{
							if (printerName.Equals(printQueue2.FullName, StringComparison.OrdinalIgnoreCase))
							{
								printQueue = printQueue2;
								break;
							}
						}
					}
					if (printQueue != null)
					{
						printQueue.InPartialTrust = true;
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				return printQueue;
			}

			// Token: 0x06008D0E RID: 36110 RVA: 0x002582B8 File Offset: 0x002564B8
			[SecurityCritical]
			private PrintTicket AcquirePrintTicket(IntPtr devModeHandle, string printQueueName)
			{
				PrintTicket result = null;
				byte[] array = null;
				IntPtr intPtr = IntPtr.Zero;
				try
				{
					intPtr = UnsafeNativeMethods.GlobalLock(devModeHandle);
					NativeMethods.DEVMODE devmode = (NativeMethods.DEVMODE)Marshal.PtrToStructure(intPtr, typeof(NativeMethods.DEVMODE));
					array = new byte[(int)(devmode.dmSize + devmode.dmDriverExtra)];
					Marshal.Copy(intPtr, array, 0, array.Length);
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						UnsafeNativeMethods.GlobalUnlock(devModeHandle);
					}
				}
				SystemDrawingHelper.NewDefaultPrintingPermission().Assert();
				try
				{
					using (PrintTicketConverter printTicketConverter = new PrintTicketConverter(printQueueName, PrintTicketConverter.MaxPrintSchemaVersion))
					{
						result = printTicketConverter.ConvertDevModeToPrintTicket(array);
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				return result;
			}

			// Token: 0x06008D0F RID: 36111 RVA: 0x00258380 File Offset: 0x00256580
			[SecurityCritical]
			private uint AcquireResultFromPrintDlgExStruct(IntPtr unmanagedBuffer)
			{
				uint dwResultAction;
				if (!this.Is64Bit())
				{
					NativeMethods.PRINTDLGEX32 printdlgex = (NativeMethods.PRINTDLGEX32)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX32));
					dwResultAction = printdlgex.dwResultAction;
				}
				else
				{
					NativeMethods.PRINTDLGEX64 printdlgex2 = (NativeMethods.PRINTDLGEX64)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX64));
					dwResultAction = printdlgex2.dwResultAction;
				}
				return dwResultAction;
			}

			// Token: 0x06008D10 RID: 36112 RVA: 0x002583D4 File Offset: 0x002565D4
			[SecurityCritical]
			private IntPtr AllocateUnmanagedPrintDlgExStruct()
			{
				IntPtr intPtr = IntPtr.Zero;
				NativeMethods.PRINTPAGERANGE printpagerange;
				printpagerange.nToPage = (uint)this._dialog.PageRange.PageTo;
				printpagerange.nFromPage = (uint)this._dialog.PageRange.PageFrom;
				uint flags = 1835008U;
				try
				{
					if (!this.Is64Bit())
					{
						NativeMethods.PRINTDLGEX32 printdlgex = new NativeMethods.PRINTDLGEX32();
						printdlgex.hwndOwner = this._ownerHandle;
						printdlgex.nMinPage = this._dialog.MinPage;
						printdlgex.nMaxPage = this._dialog.MaxPage;
						printdlgex.Flags = flags;
						if (this._dialog.SelectedPagesEnabled)
						{
							if (this._dialog.PageRangeSelection == PageRangeSelection.SelectedPages)
							{
								printdlgex.Flags |= 1U;
							}
						}
						else
						{
							printdlgex.Flags |= 4U;
						}
						if (this._dialog.CurrentPageEnabled)
						{
							if (this._dialog.PageRangeSelection == PageRangeSelection.CurrentPage)
							{
								printdlgex.Flags |= 4194304U;
							}
						}
						else
						{
							printdlgex.Flags |= 8388608U;
						}
						if (this._dialog.PageRangeEnabled)
						{
							printdlgex.lpPageRanges = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.PRINTPAGERANGE)));
							printdlgex.nMaxPageRanges = 1U;
							if (this._dialog.PageRangeSelection == PageRangeSelection.UserPages)
							{
								printdlgex.nPageRanges = 1U;
								Marshal.StructureToPtr(printpagerange, printdlgex.lpPageRanges, false);
								printdlgex.Flags |= 2U;
							}
							else
							{
								printdlgex.nPageRanges = 0U;
							}
						}
						else
						{
							printdlgex.lpPageRanges = IntPtr.Zero;
							printdlgex.nMaxPageRanges = 0U;
							printdlgex.Flags |= 8U;
						}
						if (this._dialog.PrintQueue != null)
						{
							printdlgex.hDevNames = this.AllocateAndInitializeDevNames(this._dialog.PrintQueue.FullName);
							if (this._dialog.PrintTicket != null)
							{
								printdlgex.hDevMode = this.AllocateAndInitializeDevMode(this._dialog.PrintQueue.FullName, this._dialog.PrintTicket);
							}
						}
						int cb = Marshal.SizeOf(typeof(NativeMethods.PRINTDLGEX32));
						intPtr = Marshal.AllocHGlobal(cb);
						Marshal.StructureToPtr(printdlgex, intPtr, false);
					}
					else
					{
						NativeMethods.PRINTDLGEX64 printdlgex2 = new NativeMethods.PRINTDLGEX64();
						printdlgex2.hwndOwner = this._ownerHandle;
						printdlgex2.nMinPage = this._dialog.MinPage;
						printdlgex2.nMaxPage = this._dialog.MaxPage;
						printdlgex2.Flags = flags;
						if (this._dialog.SelectedPagesEnabled)
						{
							if (this._dialog.PageRangeSelection == PageRangeSelection.SelectedPages)
							{
								printdlgex2.Flags |= 1U;
							}
						}
						else
						{
							printdlgex2.Flags |= 4U;
						}
						if (this._dialog.CurrentPageEnabled)
						{
							if (this._dialog.PageRangeSelection == PageRangeSelection.CurrentPage)
							{
								printdlgex2.Flags |= 4194304U;
							}
						}
						else
						{
							printdlgex2.Flags |= 8388608U;
						}
						if (this._dialog.PageRangeEnabled)
						{
							printdlgex2.lpPageRanges = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.PRINTPAGERANGE)));
							printdlgex2.nMaxPageRanges = 1U;
							if (this._dialog.PageRangeSelection == PageRangeSelection.UserPages)
							{
								printdlgex2.nPageRanges = 1U;
								Marshal.StructureToPtr(printpagerange, printdlgex2.lpPageRanges, false);
								printdlgex2.Flags |= 2U;
							}
							else
							{
								printdlgex2.nPageRanges = 0U;
							}
						}
						else
						{
							printdlgex2.lpPageRanges = IntPtr.Zero;
							printdlgex2.nMaxPageRanges = 0U;
							printdlgex2.Flags |= 8U;
						}
						if (this._dialog.PrintQueue != null)
						{
							printdlgex2.hDevNames = this.AllocateAndInitializeDevNames(this._dialog.PrintQueue.FullName);
							if (this._dialog.PrintTicket != null)
							{
								printdlgex2.hDevMode = this.AllocateAndInitializeDevMode(this._dialog.PrintQueue.FullName, this._dialog.PrintTicket);
							}
						}
						int cb2 = Marshal.SizeOf(typeof(NativeMethods.PRINTDLGEX64));
						intPtr = Marshal.AllocHGlobal(cb2);
						Marshal.StructureToPtr(printdlgex2, intPtr, false);
					}
				}
				catch (Exception)
				{
					this.FreeUnmanagedPrintDlgExStruct(intPtr);
					intPtr = IntPtr.Zero;
					throw;
				}
				return intPtr;
			}

			// Token: 0x06008D11 RID: 36113 RVA: 0x0025880C File Offset: 0x00256A0C
			[SecurityCritical]
			private void FreeUnmanagedPrintDlgExStruct(IntPtr unmanagedBuffer)
			{
				if (unmanagedBuffer == IntPtr.Zero)
				{
					return;
				}
				IntPtr intPtr = IntPtr.Zero;
				IntPtr intPtr2 = IntPtr.Zero;
				IntPtr intPtr3 = IntPtr.Zero;
				if (!this.Is64Bit())
				{
					NativeMethods.PRINTDLGEX32 printdlgex = (NativeMethods.PRINTDLGEX32)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX32));
					intPtr = printdlgex.hDevMode;
					intPtr2 = printdlgex.hDevNames;
					intPtr3 = printdlgex.lpPageRanges;
				}
				else
				{
					NativeMethods.PRINTDLGEX64 printdlgex2 = (NativeMethods.PRINTDLGEX64)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX64));
					intPtr = printdlgex2.hDevMode;
					intPtr2 = printdlgex2.hDevNames;
					intPtr3 = printdlgex2.lpPageRanges;
				}
				if (intPtr != IntPtr.Zero)
				{
					UnsafeNativeMethods.GlobalFree(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					UnsafeNativeMethods.GlobalFree(intPtr2);
				}
				if (intPtr3 != IntPtr.Zero)
				{
					UnsafeNativeMethods.GlobalFree(intPtr3);
				}
				Marshal.FreeHGlobal(unmanagedBuffer);
			}

			// Token: 0x06008D12 RID: 36114 RVA: 0x002588E0 File Offset: 0x00256AE0
			[SecurityCritical]
			[SecurityTreatAsSafe]
			private bool Is64Bit()
			{
				IntPtr zero = IntPtr.Zero;
				return Marshal.SizeOf(zero) == 8;
			}

			// Token: 0x06008D13 RID: 36115 RVA: 0x00258904 File Offset: 0x00256B04
			[SecurityCritical]
			private IntPtr AllocateAndInitializeDevNames(string printerName)
			{
				IntPtr intPtr = IntPtr.Zero;
				char[] array = printerName.ToCharArray();
				int cb = checked((array.Length + 3) * Marshal.SystemDefaultCharSize + Marshal.SizeOf(typeof(NativeMethods.DEVNAMES)));
				intPtr = Marshal.AllocHGlobal(cb);
				ushort num = (ushort)Marshal.SizeOf(typeof(NativeMethods.DEVNAMES));
				NativeMethods.DEVNAMES devnames;
				devnames.wDeviceOffset = (ushort)((int)num / Marshal.SystemDefaultCharSize);
				IntPtr intPtr2;
				IntPtr destination;
				checked
				{
					devnames.wDriverOffset = (ushort)((int)devnames.wDeviceOffset + array.Length + 1);
					devnames.wOutputOffset = devnames.wDriverOffset + 1;
					devnames.wDefault = 0;
					Marshal.StructureToPtr(devnames, intPtr, false);
					intPtr2 = (IntPtr)((long)intPtr + (long)(unchecked((ulong)num)));
					destination = (IntPtr)((long)intPtr2 + unchecked((long)(checked(array.Length * Marshal.SystemDefaultCharSize))));
				}
				byte[] array2 = new byte[3 * Marshal.SystemDefaultCharSize];
				Array.Clear(array2, 0, array2.Length);
				Marshal.Copy(array, 0, intPtr2, array.Length);
				Marshal.Copy(array2, 0, destination, array2.Length);
				return intPtr;
			}

			// Token: 0x06008D14 RID: 36116 RVA: 0x002589FC File Offset: 0x00256BFC
			[SecurityCritical]
			private IntPtr AllocateAndInitializeDevMode(string printerName, PrintTicket printTicket)
			{
				byte[] array = null;
				SystemDrawingHelper.NewDefaultPrintingPermission().Assert();
				try
				{
					using (PrintTicketConverter printTicketConverter = new PrintTicketConverter(printerName, PrintTicketConverter.MaxPrintSchemaVersion))
					{
						array = printTicketConverter.ConvertPrintTicketToDevMode(printTicket, BaseDevModeType.UserDefault);
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
				Marshal.Copy(array, 0, intPtr, array.Length);
				return intPtr;
			}

			// Token: 0x06008D15 RID: 36117 RVA: 0x00258A70 File Offset: 0x00256C70
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x04004A3C RID: 19004
			private Win32PrintDialog _dialog;

			// Token: 0x04004A3D RID: 19005
			[SecurityCritical]
			private IntPtr _unmanagedPrintDlgEx;

			// Token: 0x04004A3E RID: 19006
			[SecurityCritical]
			private IntPtr _ownerHandle;
		}
	}
}
