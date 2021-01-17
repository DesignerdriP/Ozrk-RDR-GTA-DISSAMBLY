using System;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Navigation;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Controls;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Internal.Utility;
using MS.Win32;

namespace System.Windows.Controls
{
	/// <summary>Hosts and navigates between HTML documents. Enables interoperability between WPF managed code and HTML script.</summary>
	// Token: 0x0200055F RID: 1375
	public sealed class WebBrowser : ActiveXHost
	{
		// Token: 0x06005B10 RID: 23312 RVA: 0x0019AF54 File Offset: 0x00199154
		[SecurityCritical]
		[SecurityTreatAsSafe]
		static WebBrowser()
		{
			if (WebBrowser.IsWebOCPermissionRestricted)
			{
				if (BrowserInteropHelper.IsBrowserHosted)
				{
					if ((BrowserInteropHelper.HostingFlags & HostingFlags.hfHostedInIEorWebOC) == (HostingFlags)0)
					{
						int num = AppSecurityManager.MapUrlToZone(BrowserInteropHelper.Source);
						if (num != 1 && num != 2 && num != 0 && !RegistryKeys.ReadLocalMachineBool("Software\\Microsoft\\.NETFramework\\Windows Presentation Foundation\\Hosting", "UnblockWebBrowserControl"))
						{
							throw new SecurityException(SR.Get("AffectedByMsCtfIssue", new object[]
							{
								"http://go.microsoft.com/fwlink/?LinkID=168882"
							}));
						}
					}
				}
				else
				{
					string fileName = Path.GetFileName(UnsafeNativeMethods.GetModuleFileName(default(HandleRef)));
					if (string.Compare(fileName, "AppLaunch.exe", StringComparison.OrdinalIgnoreCase) == 0)
					{
						SecurityHelper.DemandWebBrowserPermission();
					}
				}
				WebBrowser.RegisterWithRBW();
			}
			WebBrowser.TurnOnFeatureControlKeys();
			ControlsTraceLogger.AddControl(TelemetryControls.WebBrowser);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.WebBrowser" /> class.</summary>
		// Token: 0x06005B11 RID: 23313 RVA: 0x0019B00C File Offset: 0x0019920C
		[SecurityCritical]
		public WebBrowser() : base(new Guid("8856f961-340a-11d0-a96b-00c04fd705a2"), true)
		{
			if (SafeSecurityHelper.IsFeatureDisabled(SafeSecurityHelper.KeyToRead.WebBrowserDisable))
			{
				SecurityHelper.DemandWebBrowserPermission();
			}
			else
			{
				new WebBrowserPermission(WebBrowserPermissionLevel.Safe).Demand();
			}
			if (WebBrowser.IsWebOCPermissionRestricted)
			{
				base.Loaded += this.LoadedHandler;
			}
			this._hostingAdaptor = (WebBrowser.IsWebOCHostedInBrowserProcess ? new WebBrowser.WebOCHostedInBrowserAdaptor(this) : new WebBrowser.WebOCHostingAdaptor(this));
		}

		/// <summary>Navigate asynchronously to the document at the specified <see cref="T:System.Uri" />.</summary>
		/// <param name="source">The <see cref="T:System.Uri" /> to navigate to.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Windows.Controls.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to the underlying native WebBrowser could not be retrieved.</exception>
		/// <exception cref="T:System.Security.SecurityException">Navigation from an application that is running in partial trust to a <see cref="T:System.Uri" /> that is not located at the site of origin.</exception>
		// Token: 0x06005B12 RID: 23314 RVA: 0x0019B078 File Offset: 0x00199278
		public void Navigate(Uri source)
		{
			this.Navigate(source, null, null, null);
		}

		/// <summary>Navigates asynchronously to the document at the specified URL.</summary>
		/// <param name="source">The URL to navigate to.</param>
		// Token: 0x06005B13 RID: 23315 RVA: 0x0019B084 File Offset: 0x00199284
		public void Navigate(string source)
		{
			this.Navigate(source, null, null, null);
		}

		/// <summary>Navigate asynchronously to the document at the specified <see cref="T:System.Uri" /> and specify the target frame to load the document's content into. Additional HTTP POST data and HTTP headers can be sent to the server as part of the navigation request.</summary>
		/// <param name="source">The <see cref="T:System.Uri" /> to navigate to.</param>
		/// <param name="targetFrameName">The name of the frame to display the document's content in.</param>
		/// <param name="postData">HTTP POST data to send to the server when the source is requested.</param>
		/// <param name="additionalHeaders">HTTP headers to send to the server when the source is requested.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Windows.Controls.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to the underlying native WebBrowser could not be retrieved.</exception>
		/// <exception cref="T:System.Security.SecurityException">Navigation from an application that is running in partial trust:To a <see cref="T:System.Uri" /> that is not located at the site of origin, or 
		///             <paramref name="targetFrameName" /> name is not <see langword="null" /> or empty.</exception>
		// Token: 0x06005B14 RID: 23316 RVA: 0x0019B090 File Offset: 0x00199290
		public void Navigate(Uri source, string targetFrameName, byte[] postData, string additionalHeaders)
		{
			object obj = targetFrameName;
			object obj2 = postData;
			object obj3 = additionalHeaders;
			this.DoNavigate(source, ref obj, ref obj2, ref obj3, false);
		}

		/// <summary>Navigates asynchronously to the document at the specified URL and specify the target frame to load the document's content into. Additional HTTP POST data and HTTP headers can be sent to the server as part of the navigation request.</summary>
		/// <param name="source">The URL to navigate to.</param>
		/// <param name="targetFrameName">The name of the frame to display the document's content in.</param>
		/// <param name="postData">HTTP POST data to send to the server when the source is requested.</param>
		/// <param name="additionalHeaders">HTTP headers to send to the server when the source is requested.</param>
		// Token: 0x06005B15 RID: 23317 RVA: 0x0019B0B4 File Offset: 0x001992B4
		public void Navigate(string source, string targetFrameName, byte[] postData, string additionalHeaders)
		{
			object obj = targetFrameName;
			object obj2 = postData;
			object obj3 = additionalHeaders;
			Uri source2 = new Uri(source);
			this.DoNavigate(source2, ref obj, ref obj2, ref obj3, true);
		}

		/// <summary>Navigate asynchronously to a <see cref="T:System.IO.Stream" /> that contains the content for a document.</summary>
		/// <param name="stream">The <see cref="T:System.IO.Stream" /> that contains the content for a document.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Windows.Controls.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to the underlying native WebBrowser could not be retrieved.</exception>
		// Token: 0x06005B16 RID: 23318 RVA: 0x0019B0DD File Offset: 0x001992DD
		public void NavigateToStream(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.DocumentStream = stream;
			this.Source = null;
		}

		/// <summary>Navigate asynchronously to a <see cref="T:System.String" /> that contains the content for a document.</summary>
		/// <param name="text">The <see cref="T:System.String" /> that contains the content for a document.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Windows.Controls.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to the underlying native WebBrowser could not be retrieved.</exception>
		// Token: 0x06005B17 RID: 23319 RVA: 0x0019B0FC File Offset: 0x001992FC
		public void NavigateToString(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentNullException("text");
			}
			MemoryStream memoryStream = new MemoryStream(text.Length);
			StreamWriter streamWriter = new StreamWriter(memoryStream);
			streamWriter.Write(text);
			streamWriter.Flush();
			memoryStream.Position = 0L;
			this.NavigateToStream(memoryStream);
		}

		/// <summary>Navigate back to the previous document, if there is one.</summary>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Windows.Controls.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to the underlying native WebBrowser could not be retrieved.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">There is no document to navigate back to.</exception>
		// Token: 0x06005B18 RID: 23320 RVA: 0x0019B14B File Offset: 0x0019934B
		[SecurityCritical]
		public void GoBack()
		{
			base.VerifyAccess();
			this.AxIWebBrowser2.GoBack();
		}

		/// <summary>Navigate forward to the next HTML document, if there is one.</summary>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Windows.Controls.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to the underlying native WebBrowser could not be retrieved.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">There is no document to navigate forward to.</exception>
		// Token: 0x06005B19 RID: 23321 RVA: 0x0019B15E File Offset: 0x0019935E
		[SecurityCritical]
		public void GoForward()
		{
			base.VerifyAccess();
			this.AxIWebBrowser2.GoForward();
		}

		/// <summary>Reloads the current page.</summary>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Windows.Controls.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to the underlying native WebBrowser could not be retrieved.</exception>
		// Token: 0x06005B1A RID: 23322 RVA: 0x0019B171 File Offset: 0x00199371
		[SecurityCritical]
		public void Refresh()
		{
			base.VerifyAccess();
			this.AxIWebBrowser2.Refresh();
		}

		/// <summary>Reloads the current page with optional cache validation.</summary>
		/// <param name="noCache">Specifies whether to refresh without cache validation.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Windows.Controls.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to the underlying native WebBrowser could not be retrieved.</exception>
		// Token: 0x06005B1B RID: 23323 RVA: 0x0019B184 File Offset: 0x00199384
		[SecurityCritical]
		public void Refresh(bool noCache)
		{
			base.VerifyAccess();
			int num = noCache ? 3 : 0;
			object obj = num;
			this.AxIWebBrowser2.Refresh2(ref obj);
		}

		/// <summary>Executes a script function that is implemented by the currently loaded document.</summary>
		/// <param name="scriptName">The name of the script function to execute.</param>
		/// <returns>The object returned by the Active Scripting call.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Windows.Controls.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to the underlying native WebBrowser could not be retrieved.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">The script function does not exist.</exception>
		// Token: 0x06005B1C RID: 23324 RVA: 0x0019B1B3 File Offset: 0x001993B3
		public object InvokeScript(string scriptName)
		{
			return this.InvokeScript(scriptName, null);
		}

		/// <summary>Executes a script function that is defined in the currently loaded document.</summary>
		/// <param name="scriptName">The name of the script function to execute.</param>
		/// <param name="args">The parameters to pass to the script function.</param>
		/// <returns>The object returned by the Active Scripting call.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Windows.Controls.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to the underlying native WebBrowser could not be retrieved.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">The script function does not exist.</exception>
		// Token: 0x06005B1D RID: 23325 RVA: 0x0019B1C0 File Offset: 0x001993C0
		[SecurityCritical]
		public object InvokeScript(string scriptName, params object[] args)
		{
			base.VerifyAccess();
			if (string.IsNullOrEmpty(scriptName))
			{
				throw new ArgumentNullException("scriptName");
			}
			UnsafeNativeMethods.IDispatchEx dispatchEx = null;
			UnsafeNativeMethods.IHTMLDocument2 nativeHTMLDocument = this.NativeHTMLDocument;
			if (nativeHTMLDocument != null)
			{
				dispatchEx = (nativeHTMLDocument.GetScript() as UnsafeNativeMethods.IDispatchEx);
			}
			Uri source = this.Source;
			if (source != null)
			{
				SecurityHelper.DemandWebPermission(source);
			}
			if (nativeHTMLDocument != null)
			{
				string url = nativeHTMLDocument.GetUrl();
				if (string.CompareOrdinal(url, this.AxIWebBrowser2.LocationURL) != 0)
				{
					SecurityHelper.DemandWebPermission(new Uri(url, UriKind.Absolute));
				}
			}
			object result = null;
			if (dispatchEx != null)
			{
				NativeMethods.DISPPARAMS dispparams = new NativeMethods.DISPPARAMS();
				dispparams.rgvarg = IntPtr.Zero;
				try
				{
					Guid empty = Guid.Empty;
					string[] rgszNames = new string[]
					{
						scriptName
					};
					int[] array = new int[]
					{
						-1
					};
					dispatchEx.GetIDsOfNames(ref empty, rgszNames, 1, Thread.CurrentThread.CurrentCulture.LCID, array).ThrowIfFailed();
					if (args != null)
					{
						Array.Reverse(args);
					}
					dispparams.rgvarg = ((args == null) ? IntPtr.Zero : UnsafeNativeMethods.ArrayToVARIANTHelper.ArrayToVARIANTVector(args));
					dispparams.cArgs = (uint)((args == null) ? 0 : args.Length);
					dispparams.rgdispidNamedArgs = IntPtr.Zero;
					dispparams.cNamedArgs = 0U;
					dispatchEx.InvokeEx(array[0], Thread.CurrentThread.CurrentCulture.LCID, 1, dispparams, out result, new NativeMethods.EXCEPINFO(), null).ThrowIfFailed();
					return result;
				}
				finally
				{
					if (dispparams.rgvarg != IntPtr.Zero)
					{
						UnsafeNativeMethods.ArrayToVARIANTHelper.FreeVARIANTVector(dispparams.rgvarg, args.Length);
					}
				}
			}
			throw new InvalidOperationException(SR.Get("CannotInvokeScript"));
		}

		/// <summary>Gets or sets the <see cref="T:System.Uri" /> of the current document hosted in the <see cref="T:System.Windows.Controls.WebBrowser" />.</summary>
		/// <returns>The <see cref="T:System.Uri" /> for the current HTML document.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Windows.Controls.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to the underlying native WebBrowser could not be retrieved.</exception>
		/// <exception cref="T:System.Security.SecurityException">Navigation from an application that is running in partial trust to a <see cref="T:System.Uri" /> that is not located at the site of origin.</exception>
		// Token: 0x17001610 RID: 5648
		// (get) Token: 0x06005B1F RID: 23327 RVA: 0x0019B368 File Offset: 0x00199568
		// (set) Token: 0x06005B1E RID: 23326 RVA: 0x0019B358 File Offset: 0x00199558
		public Uri Source
		{
			[SecurityCritical]
			get
			{
				base.VerifyAccess();
				string text = this.AxIWebBrowser2.LocationURL;
				if (this.NavigatingToAboutBlank)
				{
					text = null;
				}
				if (!string.IsNullOrEmpty(text))
				{
					return new Uri(text);
				}
				return null;
			}
			set
			{
				base.VerifyAccess();
				this.Navigate(value);
			}
		}

		/// <summary>Gets a value that indicates whether there is a document to navigate back to.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that indicates whether there is a document to navigate back to.</returns>
		// Token: 0x17001611 RID: 5649
		// (get) Token: 0x06005B20 RID: 23328 RVA: 0x0019B3A1 File Offset: 0x001995A1
		public bool CanGoBack
		{
			get
			{
				base.VerifyAccess();
				return !base.IsDisposed && this._canGoBack;
			}
		}

		/// <summary>Gets a value that indicates whether there is a document to navigate forward to.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that indicates whether there is a document to navigate forward to.</returns>
		// Token: 0x17001612 RID: 5650
		// (get) Token: 0x06005B21 RID: 23329 RVA: 0x0019B3B9 File Offset: 0x001995B9
		public bool CanGoForward
		{
			get
			{
				base.VerifyAccess();
				return !base.IsDisposed && this._canGoForward;
			}
		}

		/// <summary>Gets or sets an instance of a public class, implemented by the host application, that can be accessed by script from a hosted document.</summary>
		/// <returns>The <see cref="T:System.Object" /> that is an instance of a <see langword="public" /> class, implemented by the host application, that can be accessed by script from a hosted document.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <see cref="P:System.Windows.Controls.WebBrowser.ObjectForScripting" /> is set with an instance of type that is not <see langword="COMVisible" />.</exception>
		// Token: 0x17001613 RID: 5651
		// (get) Token: 0x06005B22 RID: 23330 RVA: 0x0019B3D1 File Offset: 0x001995D1
		// (set) Token: 0x06005B23 RID: 23331 RVA: 0x0019B3E0 File Offset: 0x001995E0
		public object ObjectForScripting
		{
			get
			{
				base.VerifyAccess();
				return this._objectForScripting;
			}
			[SecurityCritical]
			set
			{
				base.VerifyAccess();
				if (value != null)
				{
					Type type = value.GetType();
					if (!Marshal.IsTypeVisibleFromCom(type))
					{
						throw new ArgumentException(SR.Get("NeedToBeComVisible"));
					}
				}
				this._objectForScripting = value;
				this._hostingAdaptor.ObjectForScripting = value;
			}
		}

		/// <summary>Gets the Document object that represents the hosted HTML page. </summary>
		/// <returns>A Document object.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Windows.Controls.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to the underlying native WebBrowser could not be retrieved.</exception>
		// Token: 0x17001614 RID: 5652
		// (get) Token: 0x06005B24 RID: 23332 RVA: 0x0019B428 File Offset: 0x00199628
		public object Document
		{
			[SecurityCritical]
			get
			{
				base.VerifyAccess();
				SecurityHelper.DemandUnmanagedCode();
				return this.AxIWebBrowser2.Document;
			}
		}

		/// <summary>Occurs just before navigation to a document.</summary>
		// Token: 0x1400010F RID: 271
		// (add) Token: 0x06005B25 RID: 23333 RVA: 0x0019B440 File Offset: 0x00199640
		// (remove) Token: 0x06005B26 RID: 23334 RVA: 0x0019B478 File Offset: 0x00199678
		public event NavigatingCancelEventHandler Navigating;

		/// <summary>Occurs when the document being navigated to is located and has started downloading.</summary>
		// Token: 0x14000110 RID: 272
		// (add) Token: 0x06005B27 RID: 23335 RVA: 0x0019B4B0 File Offset: 0x001996B0
		// (remove) Token: 0x06005B28 RID: 23336 RVA: 0x0019B4E8 File Offset: 0x001996E8
		public event NavigatedEventHandler Navigated;

		/// <summary>Occurs when the document being navigated to has finished downloading.</summary>
		// Token: 0x14000111 RID: 273
		// (add) Token: 0x06005B29 RID: 23337 RVA: 0x0019B520 File Offset: 0x00199720
		// (remove) Token: 0x06005B2A RID: 23338 RVA: 0x0019B558 File Offset: 0x00199758
		public event LoadCompletedEventHandler LoadCompleted;

		// Token: 0x06005B2B RID: 23339 RVA: 0x0019B58D File Offset: 0x0019978D
		internal void OnNavigating(NavigatingCancelEventArgs e)
		{
			base.VerifyAccess();
			if (this.Navigating != null)
			{
				this.Navigating(this, e);
			}
		}

		// Token: 0x06005B2C RID: 23340 RVA: 0x0019B5AA File Offset: 0x001997AA
		internal void OnNavigated(NavigationEventArgs e)
		{
			base.VerifyAccess();
			if (this.Navigated != null)
			{
				this.Navigated(this, e);
			}
		}

		// Token: 0x06005B2D RID: 23341 RVA: 0x0019B5C7 File Offset: 0x001997C7
		internal void OnLoadCompleted(NavigationEventArgs e)
		{
			base.VerifyAccess();
			if (this.LoadCompleted != null)
			{
				this.LoadCompleted(this, e);
			}
		}

		// Token: 0x06005B2E RID: 23342 RVA: 0x0019B5E4 File Offset: 0x001997E4
		[SecurityCritical]
		internal override object CreateActiveXObject(Guid clsid)
		{
			return this._hostingAdaptor.CreateWebOC();
		}

		// Token: 0x06005B2F RID: 23343 RVA: 0x0019B5F1 File Offset: 0x001997F1
		[SecurityCritical]
		internal override void AttachInterfaces(object nativeActiveXObject)
		{
			this._axIWebBrowser2 = (UnsafeNativeMethods.IWebBrowser2)nativeActiveXObject;
		}

		// Token: 0x06005B30 RID: 23344 RVA: 0x0019B5FF File Offset: 0x001997FF
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override void DetachInterfaces()
		{
			this._axIWebBrowser2 = null;
		}

		// Token: 0x06005B31 RID: 23345 RVA: 0x0019B608 File Offset: 0x00199808
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override void CreateSink()
		{
			this._cookie = new ConnectionPointCookie(this._axIWebBrowser2, this._hostingAdaptor.CreateEventSink(), typeof(UnsafeNativeMethods.DWebBrowserEvents2));
		}

		// Token: 0x06005B32 RID: 23346 RVA: 0x0019B630 File Offset: 0x00199830
		[SecurityCritical]
		internal override void DetachSink()
		{
			if (this._cookie != null)
			{
				this._cookie.Disconnect();
				this._cookie = null;
			}
		}

		// Token: 0x06005B33 RID: 23347 RVA: 0x0019B64C File Offset: 0x0019984C
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		internal override ActiveXSite CreateActiveXSite()
		{
			return new WebBrowserSite(this);
		}

		// Token: 0x06005B34 RID: 23348 RVA: 0x0019B654 File Offset: 0x00199854
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override DrawingGroup GetDrawing()
		{
			new UIPermission(UIPermissionWindow.AllWindows).Assert();
			DrawingGroup drawing;
			try
			{
				drawing = base.GetDrawing();
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return drawing;
		}

		// Token: 0x06005B35 RID: 23349 RVA: 0x0019B68C File Offset: 0x0019988C
		[SecurityCritical]
		internal void CleanInternalState()
		{
			this.NavigatingToAboutBlank = false;
			this.DocumentStream = null;
		}

		// Token: 0x17001615 RID: 5653
		// (get) Token: 0x06005B36 RID: 23350 RVA: 0x0019B69C File Offset: 0x0019989C
		internal UnsafeNativeMethods.IHTMLDocument2 NativeHTMLDocument
		{
			[SecurityCritical]
			get
			{
				object document = this.AxIWebBrowser2.Document;
				return document as UnsafeNativeMethods.IHTMLDocument2;
			}
		}

		// Token: 0x17001616 RID: 5654
		// (get) Token: 0x06005B37 RID: 23351 RVA: 0x0019B6BC File Offset: 0x001998BC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal UnsafeNativeMethods.IWebBrowser2 AxIWebBrowser2
		{
			[SecurityCritical]
			get
			{
				if (this._axIWebBrowser2 == null)
				{
					if (base.IsDisposed)
					{
						throw new ObjectDisposedException(base.GetType().Name);
					}
					base.TransitionUpTo(ActiveXHelper.ActiveXState.Running);
				}
				if (this._axIWebBrowser2 == null)
				{
					throw new InvalidOperationException(SR.Get("WebBrowserNoCastToIWebBrowser2"));
				}
				return this._axIWebBrowser2;
			}
		}

		// Token: 0x17001617 RID: 5655
		// (get) Token: 0x06005B38 RID: 23352 RVA: 0x0019B711 File Offset: 0x00199911
		internal WebBrowser.WebOCHostingAdaptor HostingAdaptor
		{
			get
			{
				return this._hostingAdaptor;
			}
		}

		// Token: 0x17001618 RID: 5656
		// (get) Token: 0x06005B39 RID: 23353 RVA: 0x0019B719 File Offset: 0x00199919
		// (set) Token: 0x06005B3A RID: 23354 RVA: 0x0019B721 File Offset: 0x00199921
		internal Stream DocumentStream
		{
			get
			{
				return this._documentStream;
			}
			set
			{
				this._documentStream = value;
			}
		}

		// Token: 0x17001619 RID: 5657
		// (get) Token: 0x06005B3B RID: 23355 RVA: 0x0019B72A File Offset: 0x0019992A
		// (set) Token: 0x06005B3C RID: 23356 RVA: 0x0019B737 File Offset: 0x00199937
		internal bool NavigatingToAboutBlank
		{
			get
			{
				return this._navigatingToAboutBlank.Value;
			}
			[SecurityCritical]
			set
			{
				this._navigatingToAboutBlank.Value = value;
			}
		}

		// Token: 0x1700161A RID: 5658
		// (get) Token: 0x06005B3D RID: 23357 RVA: 0x0019B745 File Offset: 0x00199945
		// (set) Token: 0x06005B3E RID: 23358 RVA: 0x0019B752 File Offset: 0x00199952
		internal Guid LastNavigation
		{
			get
			{
				return this._lastNavigation.Value;
			}
			[SecurityCritical]
			set
			{
				this._lastNavigation.Value = value;
			}
		}

		// Token: 0x1700161B RID: 5659
		// (get) Token: 0x06005B3F RID: 23359 RVA: 0x0019B760 File Offset: 0x00199960
		internal static bool IsWebOCHostedInBrowserProcess
		{
			get
			{
				if (!WebBrowser.IsWebOCPermissionRestricted)
				{
					return false;
				}
				HostingFlags hostingFlags = BrowserInteropHelper.HostingFlags;
				return (hostingFlags & HostingFlags.hfHostedInIE) != (HostingFlags)0 || (hostingFlags & HostingFlags.hfIsBrowserLowIntegrityProcess) > (HostingFlags)0;
			}
		}

		// Token: 0x06005B40 RID: 23360 RVA: 0x0019B78C File Offset: 0x0019998C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void LoadedHandler(object sender, RoutedEventArgs args)
		{
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(this);
			if (presentationSource != null && presentationSource.RootVisual is PopupRoot)
			{
				throw new InvalidOperationException(SR.Get("CannotBeInsidePopup"));
			}
		}

		// Token: 0x06005B41 RID: 23361 RVA: 0x0019B7C0 File Offset: 0x001999C0
		private static void RegisterWithRBW()
		{
			if (WebBrowser.RootBrowserWindow != null)
			{
				WebBrowser.RootBrowserWindow.AddLayoutUpdatedHandler();
			}
		}

		// Token: 0x06005B42 RID: 23362 RVA: 0x0019B7D4 File Offset: 0x001999D4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void TurnOnFeatureControlKeys()
		{
			Version version = Environment.OSVersion.Version;
			if (version.Major == 5 && version.Minor == 2 && version.MajorRevision == 0)
			{
				return;
			}
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(0, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(1, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(2, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(3, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(4, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(5, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(6, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(7, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(8, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(9, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(10, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(11, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(12, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(13, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(14, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(15, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(16, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(17, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(18, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(20, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(22, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(25, 2, true);
			if (WebBrowser.IsWebOCPermissionRestricted)
			{
				UnsafeNativeMethods.CoInternetSetFeatureEnabled(23, 2, true);
			}
		}

		// Token: 0x06005B43 RID: 23363 RVA: 0x0019B8EC File Offset: 0x00199AEC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void DoNavigate(Uri source, ref object targetFrameName, ref object postData, ref object headers, bool ignoreEscaping = false)
		{
			base.VerifyAccess();
			NativeMethods.IOleCommandTarget oleCommandTarget = (NativeMethods.IOleCommandTarget)this.AxIWebBrowser2;
			object obj = false;
			oleCommandTarget.Exec(null, 23, 0, new object[]
			{
				obj
			}, 0);
			this.LastNavigation = Guid.NewGuid();
			if (source == null)
			{
				this.NavigatingToAboutBlank = true;
				source = new Uri("about:blank");
			}
			else
			{
				this.CleanInternalState();
			}
			if (!source.IsAbsoluteUri)
			{
				throw new ArgumentException(SR.Get("AbsoluteUriOnly"), "source");
			}
			if (PackUriHelper.IsPackUri(source))
			{
				source = BaseUriHelper.ConvertPackUriToAbsoluteExternallyVisibleUri(source);
			}
			if (!string.IsNullOrEmpty((string)targetFrameName))
			{
				new WebPermission(PermissionState.Unrestricted).Demand();
			}
			else if (!this.NavigatingToAboutBlank)
			{
				SecurityHelper.DemandWebPermission(source);
			}
			object obj2 = null;
			object obj3 = ignoreEscaping ? source.AbsoluteUri : BindUriHelper.UriToString(source);
			try
			{
				this.AxIWebBrowser2.Navigate2(ref obj3, ref obj2, ref targetFrameName, ref postData, ref headers);
			}
			catch (COMException ex)
			{
				this.CleanInternalState();
				if (ex.ErrorCode != -2147023673)
				{
					throw;
				}
			}
		}

		// Token: 0x06005B44 RID: 23364 RVA: 0x0019BA04 File Offset: 0x00199C04
		private void SyncUIActiveState()
		{
			if (base.ActiveXState != ActiveXHelper.ActiveXState.UIActive && this.HasFocusWithinCore())
			{
				Invariant.Assert(base.ActiveXState == ActiveXHelper.ActiveXState.InPlaceActive);
				base.ActiveXState = ActiveXHelper.ActiveXState.UIActive;
			}
		}

		// Token: 0x06005B45 RID: 23365 RVA: 0x0019BA2C File Offset: 0x00199C2C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override bool TranslateAcceleratorCore(ref MSG msg, ModifierKeys modifiers)
		{
			this.SyncUIActiveState();
			Invariant.Assert(base.ActiveXState >= ActiveXHelper.ActiveXState.UIActive, "Should be at least UIActive when we are processing accelerator keys");
			return base.ActiveXInPlaceActiveObject.TranslateAccelerator(ref msg) == 0;
		}

		// Token: 0x06005B46 RID: 23366 RVA: 0x0019BA5C File Offset: 0x00199C5C
		[SecurityCritical]
		protected override bool TabIntoCore(TraversalRequest request)
		{
			Invariant.Assert(base.ActiveXState >= ActiveXHelper.ActiveXState.InPlaceActive, "Should be at least InPlaceActive when tabbed into");
			bool flag = base.DoVerb(-4);
			if (flag)
			{
				base.ActiveXState = ActiveXHelper.ActiveXState.UIActive;
			}
			return flag;
		}

		// Token: 0x1700161C RID: 5660
		// (get) Token: 0x06005B47 RID: 23367 RVA: 0x0019BA93 File Offset: 0x00199C93
		private static RootBrowserWindow RootBrowserWindow
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				if (WebBrowser._rbw.Value == null && Application.Current != null)
				{
					WebBrowser._rbw.Value = (Application.Current.MainWindow as RootBrowserWindow);
				}
				return WebBrowser._rbw.Value;
			}
		}

		// Token: 0x04002F63 RID: 12131
		internal bool _canGoBack;

		// Token: 0x04002F64 RID: 12132
		internal bool _canGoForward;

		// Token: 0x04002F65 RID: 12133
		internal const string AboutBlankUriString = "about:blank";

		// Token: 0x04002F66 RID: 12134
		private static readonly bool IsWebOCPermissionRestricted = !SecurityHelper.CallerAndAppDomainHaveUnrestrictedWebBrowserPermission();

		// Token: 0x04002F67 RID: 12135
		[SecurityCritical]
		private UnsafeNativeMethods.IWebBrowser2 _axIWebBrowser2;

		// Token: 0x04002F68 RID: 12136
		private WebBrowser.WebOCHostingAdaptor _hostingAdaptor;

		// Token: 0x04002F69 RID: 12137
		private ConnectionPointCookie _cookie;

		// Token: 0x04002F6A RID: 12138
		private static SecurityCriticalDataForSet<RootBrowserWindow> _rbw;

		// Token: 0x04002F6B RID: 12139
		private object _objectForScripting;

		// Token: 0x04002F6C RID: 12140
		private Stream _documentStream;

		// Token: 0x04002F6D RID: 12141
		private SecurityCriticalDataForSet<bool> _navigatingToAboutBlank;

		// Token: 0x04002F6E RID: 12142
		private SecurityCriticalDataForSet<Guid> _lastNavigation;

		// Token: 0x020009DB RID: 2523
		internal class WebOCHostingAdaptor
		{
			// Token: 0x0600892D RID: 35117 RVA: 0x002544C4 File Offset: 0x002526C4
			internal WebOCHostingAdaptor(WebBrowser webBrowser)
			{
				this._webBrowser = webBrowser;
			}

			// Token: 0x17001F04 RID: 7940
			// (get) Token: 0x0600892E RID: 35118 RVA: 0x002544D3 File Offset: 0x002526D3
			// (set) Token: 0x0600892F RID: 35119 RVA: 0x00002137 File Offset: 0x00000337
			internal virtual object ObjectForScripting
			{
				get
				{
					return this._webBrowser.ObjectForScripting;
				}
				set
				{
				}
			}

			// Token: 0x06008930 RID: 35120 RVA: 0x002544E0 File Offset: 0x002526E0
			[SecurityCritical]
			internal virtual object CreateWebOC()
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
				object result;
				try
				{
					result = Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("8856f961-340a-11d0-a96b-00c04fd705a2")));
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				return result;
			}

			// Token: 0x06008931 RID: 35121 RVA: 0x00254528 File Offset: 0x00252728
			[SecurityCritical]
			internal virtual object CreateEventSink()
			{
				return new WebBrowserEvent(this._webBrowser);
			}

			// Token: 0x04004638 RID: 17976
			protected WebBrowser _webBrowser;
		}

		// Token: 0x020009DC RID: 2524
		private class WebOCHostedInBrowserAdaptor : WebBrowser.WebOCHostingAdaptor
		{
			// Token: 0x06008932 RID: 35122 RVA: 0x00254535 File Offset: 0x00252735
			internal WebOCHostedInBrowserAdaptor(WebBrowser webBrowser) : base(webBrowser)
			{
			}

			// Token: 0x06008933 RID: 35123 RVA: 0x00254540 File Offset: 0x00252740
			[SecurityCritical]
			[SecurityTreatAsSafe]
			static WebOCHostedInBrowserAdaptor()
			{
				Guid guid = typeof(UnsafeNativeMethods.IDocHostUIHandler).GUID;
				Guid guid2 = new Guid("e302cb55-5f9d-41a3-9ef3-61827fb8b46d");
				int num = UnsafeNativeMethods.CoRegisterPSClsid(ref guid, ref guid2);
				if (num != 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
			}

			// Token: 0x17001F05 RID: 7941
			// (get) Token: 0x06008934 RID: 35124 RVA: 0x0025457C File Offset: 0x0025277C
			// (set) Token: 0x06008935 RID: 35125 RVA: 0x00254584 File Offset: 0x00252784
			internal override object ObjectForScripting
			{
				[SecurityCritical]
				[SecurityTreatAsSafe]
				get
				{
					return this._threadBoundObjectForScripting;
				}
				[SecurityCritical]
				[SecurityTreatAsSafe]
				set
				{
					this._threadBoundObjectForScripting = ((value == null) ? null : ActiveXHelper.CreateIDispatchSTAForwarder(value));
				}
			}

			// Token: 0x06008936 RID: 35126 RVA: 0x00254598 File Offset: 0x00252798
			[SecurityCritical]
			internal override object CreateWebOC()
			{
				IntPtr pUnk = Application.Current.BrowserCallbackServices.CreateWebBrowserControlInBrowserProcess();
				object typedObjectForIUnknown = Marshal.GetTypedObjectForIUnknown(pUnk, typeof(UnsafeNativeMethods.IWebBrowser2));
				Marshal.Release(pUnk);
				return typedObjectForIUnknown;
			}

			// Token: 0x06008937 RID: 35127 RVA: 0x002545CE File Offset: 0x002527CE
			[SecurityCritical]
			internal override object CreateEventSink()
			{
				return ActiveXHelper.CreateIDispatchSTAForwarder((UnsafeNativeMethods.DWebBrowserEvents2)base.CreateEventSink());
			}

			// Token: 0x04004639 RID: 17977
			private object _threadBoundObjectForScripting;
		}
	}
}
