using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Net;
using System.Security;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Annotations.Component;
using MS.Internal.Documents;
using MS.Internal.IO.Packaging;

namespace System.Windows.Documents
{
	/// <summary>Hosts a portable, high fidelity, fixed-format document with read access for user text selection, keyboard navigation, and search.</summary>
	// Token: 0x02000343 RID: 835
	[ContentProperty("Pages")]
	public class FixedDocument : FrameworkContentElement, IDocumentPaginatorSource, IAddChildInternal, IAddChild, IServiceProvider, IFixedNavigate, IUriContext
	{
		// Token: 0x06002CB0 RID: 11440 RVA: 0x000C98A4 File Offset: 0x000C7AA4
		static FixedDocument()
		{
			ContentElement.FocusableProperty.OverrideMetadata(typeof(FixedDocument), new FrameworkPropertyMetadata(true));
			NavigationService.NavigationServiceProperty.OverrideMetadata(typeof(FixedDocument), new FrameworkPropertyMetadata(new PropertyChangedCallback(FixedHyperLink.OnNavigationServiceChanged)));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.FixedDocument" /> class. </summary>
		// Token: 0x06002CB1 RID: 11441 RVA: 0x000C9950 File Offset: 0x000C7B50
		public FixedDocument()
		{
			this._Init();
		}

		/// <summary>Gets the service object of the specified type.</summary>
		/// <param name="serviceType">An object that specifies the type of service object to get. </param>
		/// <returns>A service object of type <paramref name="serviceType" />.-or- 
		///     <see langword="null" /> if there is no service object of type <paramref name="serviceType" />.</returns>
		// Token: 0x06002CB2 RID: 11442 RVA: 0x000C997C File Offset: 0x000C7B7C
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextContainer))
			{
				return this.FixedContainer;
			}
			if (serviceType == typeof(RubberbandSelector))
			{
				if (this._rubberbandSelector == null)
				{
					this._rubberbandSelector = new RubberbandSelector();
				}
				return this._rubberbandSelector;
			}
			return null;
		}

		/// <summary>Adds a child object. </summary>
		/// <param name="value">The child object to add.</param>
		// Token: 0x06002CB3 RID: 11443 RVA: 0x000C99E4 File Offset: 0x000C7BE4
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PageContent pageContent = value as PageContent;
			if (pageContent == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(PageContent)
				}), "value");
			}
			if (pageContent.IsInitialized)
			{
				this._pages.Add(pageContent);
				return;
			}
			if (this._partialPage == null)
			{
				this._partialPage = pageContent;
				this._partialPage.ChangeLogicalParent(this);
				this._partialPage.Initialized += this.OnPageLoaded;
				return;
			}
			throw new InvalidOperationException(SR.Get("PrevoiusPartialPageContentOutstanding"));
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x06002CB4 RID: 11444 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Gets or sets the base URI of the current application context. </summary>
		/// <returns>The base URI of the application context.</returns>
		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x06002CB5 RID: 11445 RVA: 0x000C204F File Offset: 0x000C024F
		// (set) Token: 0x06002CB6 RID: 11446 RVA: 0x000C2061 File Offset: 0x000C0261
		Uri IUriContext.BaseUri
		{
			get
			{
				return (Uri)base.GetValue(BaseUriHelper.BaseUriProperty);
			}
			set
			{
				base.SetValue(BaseUriHelper.BaseUriProperty, value);
			}
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x000C9A92 File Offset: 0x000C7C92
		void IFixedNavigate.NavigateAsync(string elementID)
		{
			if (this.IsPageCountValid)
			{
				FixedHyperLink.NavigateToElement(this, elementID);
				return;
			}
			this._navigateAfterPagination = true;
			this._navigateFragment = elementID;
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x000C9AB4 File Offset: 0x000C7CB4
		UIElement IFixedNavigate.FindElementByID(string elementID, out FixedPage rootFixedPage)
		{
			UIElement uielement = null;
			rootFixedPage = null;
			if (char.IsDigit(elementID[0]))
			{
				int num = Convert.ToInt32(elementID, CultureInfo.InvariantCulture);
				num--;
				uielement = this.GetFixedPage(num);
				rootFixedPage = this.GetFixedPage(num);
			}
			else
			{
				PageContentCollection pages = this.Pages;
				int i = 0;
				int count = pages.Count;
				while (i < count)
				{
					PageContent pageContent = pages[i];
					if (pageContent.PageStream != null)
					{
						FixedPage fixedPage = this.GetFixedPage(i);
						if (fixedPage != null)
						{
							uielement = ((IFixedNavigate)fixedPage).FindElementByID(elementID, out rootFixedPage);
							if (uielement != null)
							{
								break;
							}
						}
					}
					else if (pageContent.ContainsID(elementID))
					{
						FixedPage fixedPage = this.GetFixedPage(i);
						if (fixedPage != null)
						{
							uielement = ((IFixedNavigate)fixedPage).FindElementByID(elementID, out rootFixedPage);
							if (uielement == null)
							{
								uielement = fixedPage;
								break;
							}
							break;
						}
					}
					i++;
				}
			}
			return uielement;
		}

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x06002CB9 RID: 11449 RVA: 0x000C9B6F File Offset: 0x000C7D6F
		// (set) Token: 0x06002CBA RID: 11450 RVA: 0x000C9B81 File Offset: 0x000C7D81
		internal NavigationService NavigationService
		{
			get
			{
				return (NavigationService)base.GetValue(NavigationService.NavigationServiceProperty);
			}
			set
			{
				base.SetValue(NavigationService.NavigationServiceProperty, value);
			}
		}

		/// <summary>Gets an enumerator for accessing the document's <see cref="T:System.Windows.Documents.PageContent" /> child elements.</summary>
		/// <returns>An enumerator for accessing the document's <see cref="T:System.Windows.Documents.PageContent" /> child elements.</returns>
		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06002CBB RID: 11451 RVA: 0x000C9B8F File Offset: 0x000C7D8F
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return this.Pages.GetEnumerator();
			}
		}

		/// <summary>Gets the paginator for the <see cref="T:System.Windows.Documents.FixedDocument" /> that provides page-oriented services such as getting a particular page and repaginating in response to changes. </summary>
		/// <returns>An object of a class derived from <see cref="T:System.Windows.Documents.DocumentPaginator" /> that provides pagination services.</returns>
		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06002CBC RID: 11452 RVA: 0x000C9B9C File Offset: 0x000C7D9C
		public DocumentPaginator DocumentPaginator
		{
			get
			{
				return this._paginator;
			}
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x000C9BA4 File Offset: 0x000C7DA4
		internal DocumentPage GetPage(int pageNumber)
		{
			if (pageNumber < 0)
			{
				throw new ArgumentOutOfRangeException("pageNumber", SR.Get("IDPNegativePageNumber"));
			}
			if (pageNumber >= this.Pages.Count)
			{
				return DocumentPage.Missing;
			}
			FixedPage fixedPage = this.SyncGetPage(pageNumber, false);
			if (fixedPage == null)
			{
				return DocumentPage.Missing;
			}
			Size size = this.ComputePageSize(fixedPage);
			FixedDocumentPage result = new FixedDocumentPage(this, fixedPage, size, pageNumber);
			fixedPage.Measure(size);
			fixedPage.Arrange(new Rect(default(Point), size));
			return result;
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x000C9C20 File Offset: 0x000C7E20
		internal void GetPageAsync(int pageNumber, object userState)
		{
			if (pageNumber < 0)
			{
				throw new ArgumentOutOfRangeException("pageNumber", SR.Get("IDPNegativePageNumber"));
			}
			if (userState == null)
			{
				throw new ArgumentNullException("userState");
			}
			if (pageNumber < this.Pages.Count)
			{
				PageContent pageContent = this.Pages[pageNumber];
				FixedDocument.GetPageAsyncRequest getPageAsyncRequest = new FixedDocument.GetPageAsyncRequest(pageContent, pageNumber, userState);
				this._asyncOps[userState] = getPageAsyncRequest;
				DispatcherOperationCallback method = new DispatcherOperationCallback(this.GetPageAsyncDelegate);
				base.Dispatcher.BeginInvoke(DispatcherPriority.Background, method, getPageAsyncRequest);
				return;
			}
			this._NotifyGetPageAsyncCompleted(DocumentPage.Missing, pageNumber, null, false, userState);
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x000C9CB4 File Offset: 0x000C7EB4
		internal int GetPageNumber(ContentPosition contentPosition)
		{
			if (contentPosition == null)
			{
				throw new ArgumentNullException("contentPosition");
			}
			FixedTextPointer fixedTextPointer = contentPosition as FixedTextPointer;
			if (fixedTextPointer == null)
			{
				throw new ArgumentException(SR.Get("IDPInvalidContentPosition"));
			}
			return fixedTextPointer.FixedTextContainer.GetPageNumber(fixedTextPointer);
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x000C9CF8 File Offset: 0x000C7EF8
		internal void CancelAsync(object userState)
		{
			if (userState == null)
			{
				throw new ArgumentNullException("userState");
			}
			FixedDocument.GetPageAsyncRequest getPageAsyncRequest;
			if (this._asyncOps.TryGetValue(userState, out getPageAsyncRequest) && getPageAsyncRequest != null)
			{
				getPageAsyncRequest.Cancelled = true;
				getPageAsyncRequest.PageContent.GetPageRootAsyncCancel();
			}
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x000C9D38 File Offset: 0x000C7F38
		internal ContentPosition GetObjectPosition(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			DependencyObject dependencyObject = o as DependencyObject;
			if (dependencyObject == null)
			{
				throw new ArgumentException(SR.Get("FixedDocumentExpectsDependencyObject"));
			}
			FixedPage fixedPage = null;
			int num = -1;
			if (dependencyObject != this)
			{
				DependencyObject dependencyObject2 = dependencyObject;
				while (dependencyObject2 != null)
				{
					fixedPage = (dependencyObject2 as FixedPage);
					if (fixedPage != null)
					{
						num = this.GetIndexOfPage(fixedPage);
						if (num >= 0)
						{
							break;
						}
						dependencyObject2 = fixedPage.Parent;
					}
					else
					{
						dependencyObject2 = LogicalTreeHelper.GetParent(dependencyObject2);
					}
				}
			}
			else if (this.Pages.Count > 0)
			{
				num = 0;
			}
			FixedTextPointer fixedTextPointer = null;
			if (num >= 0)
			{
				FlowPosition flowPosition = null;
				System.Windows.Shapes.Path path = dependencyObject as System.Windows.Shapes.Path;
				if (dependencyObject is Glyphs || dependencyObject is Image || (path != null && path.Fill is ImageBrush))
				{
					FixedPosition fixedPosition = new FixedPosition(fixedPage.CreateFixedNode(num, (UIElement)dependencyObject), 0);
					flowPosition = this.FixedContainer.FixedTextBuilder.CreateFlowPosition(fixedPosition);
				}
				if (flowPosition == null)
				{
					flowPosition = this.FixedContainer.FixedTextBuilder.GetPageStartFlowPosition(num);
				}
				fixedTextPointer = new FixedTextPointer(true, LogicalDirection.Forward, flowPosition);
			}
			if (fixedTextPointer == null)
			{
				return ContentPosition.Missing;
			}
			return fixedTextPointer;
		}

		// Token: 0x06002CC2 RID: 11458 RVA: 0x000C9E44 File Offset: 0x000C8044
		internal ContentPosition GetPagePosition(DocumentPage page)
		{
			FixedDocumentPage fixedDocumentPage = page as FixedDocumentPage;
			if (fixedDocumentPage == null)
			{
				return ContentPosition.Missing;
			}
			return fixedDocumentPage.ContentPosition;
		}

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x06002CC3 RID: 11459 RVA: 0x000C9E67 File Offset: 0x000C8067
		internal bool IsPageCountValid
		{
			get
			{
				return base.IsInitialized;
			}
		}

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x06002CC4 RID: 11460 RVA: 0x000C9E6F File Offset: 0x000C806F
		internal int PageCount
		{
			get
			{
				return this.Pages.Count;
			}
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x06002CC5 RID: 11461 RVA: 0x000C9E7C File Offset: 0x000C807C
		// (set) Token: 0x06002CC6 RID: 11462 RVA: 0x000C9E8F File Offset: 0x000C808F
		internal Size PageSize
		{
			get
			{
				return new Size(this._pageWidth, this._pageHeight);
			}
			set
			{
				this._pageWidth = value.Width;
				this._pageHeight = value.Height;
			}
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x06002CC7 RID: 11463 RVA: 0x000C9EAB File Offset: 0x000C80AB
		internal bool HasExplicitStructure
		{
			get
			{
				return this._hasExplicitStructure;
			}
		}

		/// <summary>Gets a collection of the document's <see cref="T:System.Windows.Documents.PageContent" /> elements. </summary>
		/// <returns>A collection of the document's <see cref="T:System.Windows.Documents.PageContent" /> elements.</returns>
		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x06002CC8 RID: 11464 RVA: 0x000C9EB3 File Offset: 0x000C80B3
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public PageContentCollection Pages
		{
			get
			{
				return this._pages;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Printing.PrintTicket" /> that is associated with this document. </summary>
		/// <returns>The <see cref="T:System.Printing.PrintTicket" /> for this document.</returns>
		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x06002CC9 RID: 11465 RVA: 0x000C9EBC File Offset: 0x000C80BC
		// (set) Token: 0x06002CCA RID: 11466 RVA: 0x000C9ED6 File Offset: 0x000C80D6
		public object PrintTicket
		{
			get
			{
				return base.GetValue(FixedDocument.PrintTicketProperty);
			}
			set
			{
				base.SetValue(FixedDocument.PrintTicketProperty, value);
			}
		}

		/// <summary>Creates an automation peer for the document. </summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.DocumentAutomationPeer" /> that exposes the <see cref="T:System.Windows.Documents.FixedDocument" /> to Microsoft UI Automation.</returns>
		// Token: 0x06002CCB RID: 11467 RVA: 0x000C6984 File Offset: 0x000C4B84
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DocumentAutomationPeer(this);
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x000C9EE4 File Offset: 0x000C80E4
		internal int GetIndexOfPage(FixedPage p)
		{
			PageContentCollection pages = this.Pages;
			int i = 0;
			int count = pages.Count;
			while (i < count)
			{
				if (pages[i].IsOwnerOf(p))
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x000C9F1D File Offset: 0x000C811D
		internal bool IsValidPageIndex(int index)
		{
			return index >= 0 && index < this.Pages.Count;
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x000C9F33 File Offset: 0x000C8133
		internal FixedPage SyncGetPageWithCheck(int index)
		{
			if (this.IsValidPageIndex(index))
			{
				return this.SyncGetPage(index, false);
			}
			return null;
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x000C9F48 File Offset: 0x000C8148
		internal FixedPage SyncGetPage(int index, bool forceReload)
		{
			PageContentCollection pages = this.Pages;
			FixedPage pageRoot;
			try
			{
				pageRoot = pages[index].GetPageRoot(forceReload);
			}
			catch (Exception ex)
			{
				if (ex is InvalidOperationException || ex is ApplicationException)
				{
					ApplicationException ex2 = new ApplicationException(string.Format(CultureInfo.CurrentCulture, SR.Get("ExceptionInGetPage"), new object[]
					{
						index
					}), ex);
					throw ex2;
				}
				throw;
			}
			return pageRoot;
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x000C9FBC File Offset: 0x000C81BC
		internal void OnPageContentAppended(int index)
		{
			this.FixedContainer.FixedTextBuilder.AddVirtualPage();
			this._paginator.NotifyPaginationProgress(new PaginationProgressEventArgs(index, 1));
			if (base.IsInitialized)
			{
				this._paginator.NotifyPagesChanged(new PagesChangedEventArgs(index, 1));
			}
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x000C9FFC File Offset: 0x000C81FC
		internal void EnsurePageSize(FixedPage fp)
		{
			double width = fp.Width;
			if (DoubleUtil.IsNaN(width))
			{
				fp.Width = this._pageWidth;
			}
			double height = fp.Height;
			if (DoubleUtil.IsNaN(height))
			{
				fp.Height = this._pageHeight;
			}
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x000CA040 File Offset: 0x000C8240
		internal bool GetPageSize(ref Size pageSize, int pageNumber)
		{
			if (pageNumber < this.Pages.Count)
			{
				FixedPage fp = null;
				if (!this._pendingPages.Contains(this.Pages[pageNumber]))
				{
					fp = this.SyncGetPage(pageNumber, false);
				}
				pageSize = this.ComputePageSize(fp);
				return true;
			}
			return false;
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x000CA08F File Offset: 0x000C828F
		internal Size ComputePageSize(FixedPage fp)
		{
			if (fp == null)
			{
				return new Size(this._pageWidth, this._pageHeight);
			}
			this.EnsurePageSize(fp);
			return new Size(fp.Width, fp.Height);
		}

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x06002CD4 RID: 11476 RVA: 0x000CA0BE File Offset: 0x000C82BE
		internal FixedTextContainer FixedContainer
		{
			get
			{
				if (this._fixedTextContainer == null)
				{
					this._fixedTextContainer = new FixedTextContainer(this);
					this._fixedTextContainer.Highlights.Changed += this.OnHighlightChanged;
				}
				return this._fixedTextContainer;
			}
		}

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06002CD5 RID: 11477 RVA: 0x000CA0F6 File Offset: 0x000C82F6
		internal Dictionary<FixedPage, ArrayList> Highlights
		{
			get
			{
				return this._highlights;
			}
		}

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x06002CD6 RID: 11478 RVA: 0x000CA0FE File Offset: 0x000C82FE
		// (set) Token: 0x06002CD7 RID: 11479 RVA: 0x000CA106 File Offset: 0x000C8306
		internal DocumentReference DocumentReference
		{
			get
			{
				return this._documentReference;
			}
			set
			{
				this._documentReference = value;
			}
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000CA110 File Offset: 0x000C8310
		private void _Init()
		{
			this._paginator = new FixedDocumentPaginator(this);
			this._pages = new PageContentCollection(this);
			this._highlights = new Dictionary<FixedPage, ArrayList>();
			this._asyncOps = new Dictionary<object, FixedDocument.GetPageAsyncRequest>();
			this._pendingPages = new List<PageContent>();
			this._hasExplicitStructure = false;
			base.Initialized += this.OnInitialized;
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x000CA170 File Offset: 0x000C8370
		private void OnInitialized(object sender, EventArgs e)
		{
			if (this._navigateAfterPagination)
			{
				FixedHyperLink.NavigateToElement(this, this._navigateFragment);
				this._navigateAfterPagination = false;
			}
			this.ValidateDocStructure();
			if (this.PageCount > 0)
			{
				DocumentPage page = this.GetPage(0);
				if (page != null)
				{
					FixedPage fixedPage = page.Visual as FixedPage;
					if (fixedPage != null)
					{
						base.Language = fixedPage.Language;
					}
				}
			}
			this._paginator.NotifyPaginationCompleted(e);
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x000CA1DC File Offset: 0x000C83DC
		internal void ValidateDocStructure()
		{
			Uri baseUri = BaseUriHelper.GetBaseUri(this);
			if (baseUri.Scheme.Equals(PackUriHelper.UriSchemePack, StringComparison.OrdinalIgnoreCase) && !baseUri.Host.Equals(BaseUriHelper.PackAppBaseUri.Host) && !baseUri.Host.Equals(BaseUriHelper.SiteOfOriginBaseUri.Host))
			{
				Uri structureUriFromRelationship = FixedDocument.GetStructureUriFromRelationship(baseUri, "http://schemas.microsoft.com/xps/2005/06/documentstructure");
				if (structureUriFromRelationship != null)
				{
					ContentType contentType;
					FixedDocument.ValidateAndLoadPartFromAbsoluteUri(structureUriFromRelationship, true, "DocumentStructure", out contentType);
					if (!FixedDocument._documentStructureContentType.AreTypeAndSubTypeEqual(contentType))
					{
						throw new FileFormatException(SR.Get("InvalidDSContentType"));
					}
					this._hasExplicitStructure = true;
				}
			}
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x000CA27C File Offset: 0x000C847C
		internal static StoryFragments GetStoryFragments(FixedPage fixedPage)
		{
			object obj = null;
			Uri baseUri = BaseUriHelper.GetBaseUri(fixedPage);
			if (baseUri.Scheme.Equals(PackUriHelper.UriSchemePack, StringComparison.OrdinalIgnoreCase) && !baseUri.Host.Equals(BaseUriHelper.PackAppBaseUri.Host) && !baseUri.Host.Equals(BaseUriHelper.SiteOfOriginBaseUri.Host))
			{
				Uri structureUriFromRelationship = FixedDocument.GetStructureUriFromRelationship(baseUri, "http://schemas.microsoft.com/xps/2005/06/storyfragments");
				if (structureUriFromRelationship != null)
				{
					ContentType contentType;
					obj = FixedDocument.ValidateAndLoadPartFromAbsoluteUri(structureUriFromRelationship, false, null, out contentType);
					if (!FixedDocument._storyFragmentsContentType.AreTypeAndSubTypeEqual(contentType))
					{
						throw new FileFormatException(SR.Get("InvalidSFContentType"));
					}
					if (!(obj is StoryFragments))
					{
						throw new FileFormatException(SR.Get("InvalidStoryFragmentsMarkup"));
					}
				}
			}
			return obj as StoryFragments;
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x000CA334 File Offset: 0x000C8534
		private static object ValidateAndLoadPartFromAbsoluteUri(Uri AbsoluteUriDoc, bool validateOnly, string rootElement, out ContentType mimeType)
		{
			mimeType = null;
			object result = null;
			try
			{
				Stream stream = WpfWebRequestHelper.CreateRequestAndGetResponseStream(AbsoluteUriDoc, out mimeType);
				ParserContext parserContext = new ParserContext();
				parserContext.BaseUri = AbsoluteUriDoc;
				XpsValidatingLoader xpsValidatingLoader = new XpsValidatingLoader();
				if (validateOnly)
				{
					xpsValidatingLoader.Validate(stream, null, parserContext, mimeType, rootElement);
				}
				else
				{
					result = xpsValidatingLoader.Load(stream, null, parserContext, mimeType);
				}
			}
			catch (Exception ex)
			{
				if (!(ex is WebException) && !(ex is InvalidOperationException))
				{
					throw;
				}
			}
			return result;
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x000CA3AC File Offset: 0x000C85AC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static Uri GetStructureUriFromRelationship(Uri contentUri, string relationshipName)
		{
			Uri result = null;
			if (contentUri != null && relationshipName != null)
			{
				Uri partUri = PackUriHelper.GetPartUri(contentUri);
				if (partUri != null)
				{
					Uri packageUri = PackUriHelper.GetPackageUri(contentUri);
					Package package = PreloadedPackages.GetPackage(packageUri);
					if (package == null && SecurityHelper.CheckEnvironmentPermission())
					{
						package = PackageStore.GetPackage(packageUri);
					}
					if (package != null)
					{
						PackagePart part = package.GetPart(partUri);
						PackageRelationshipCollection relationshipsByType = part.GetRelationshipsByType(relationshipName);
						Uri uri = null;
						foreach (PackageRelationship packageRelationship in relationshipsByType)
						{
							uri = PackUriHelper.ResolvePartUri(partUri, packageRelationship.TargetUri);
						}
						if (uri != null)
						{
							result = PackUriHelper.Create(packageUri, uri);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x000CA478 File Offset: 0x000C8678
		private void OnPageLoaded(object sender, EventArgs e)
		{
			PageContent pageContent = (PageContent)sender;
			if (pageContent == this._partialPage)
			{
				this._partialPage.Initialized -= this.OnPageLoaded;
				this._pages.Add(this._partialPage);
				this._partialPage = null;
			}
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x000CA4C8 File Offset: 0x000C86C8
		internal FixedPage GetFixedPage(int pageNumber)
		{
			FixedPage result = null;
			FixedDocumentPage fixedDocumentPage = this.GetPage(pageNumber) as FixedDocumentPage;
			if (fixedDocumentPage != null && fixedDocumentPage != DocumentPage.Missing)
			{
				result = fixedDocumentPage.FixedPage;
			}
			return result;
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x000CA4F8 File Offset: 0x000C86F8
		private void OnHighlightChanged(object sender, HighlightChangedEventArgs args)
		{
			ITextContainer fixedContainer = this.FixedContainer;
			Highlights highlights = null;
			FixedDocumentSequence fixedDocumentSequence = base.Parent as FixedDocumentSequence;
			if (fixedDocumentSequence != null)
			{
				highlights = fixedDocumentSequence.TextContainer.Highlights;
			}
			else
			{
				highlights = this.FixedContainer.Highlights;
			}
			List<FixedPage> list = new List<FixedPage>();
			foreach (FixedPage item in this._highlights.Keys)
			{
				list.Add(item);
			}
			this._highlights.Clear();
			StaticTextPointer staticTextPointer = fixedContainer.CreateStaticPointerAtOffset(0);
			for (;;)
			{
				if (!highlights.IsContentHighlighted(staticTextPointer, LogicalDirection.Forward))
				{
					staticTextPointer = highlights.GetNextHighlightChangePosition(staticTextPointer, LogicalDirection.Forward);
					if (staticTextPointer.IsNull)
					{
						break;
					}
				}
				object highlightValue = highlights.GetHighlightValue(staticTextPointer, LogicalDirection.Forward, typeof(TextSelection));
				StaticTextPointer textPosition = staticTextPointer;
				FixedHighlightType fixedHighlightType = FixedHighlightType.None;
				Brush foregroundBrush = null;
				Brush backgroundBrush = null;
				if (highlightValue != DependencyProperty.UnsetValue)
				{
					do
					{
						staticTextPointer = highlights.GetNextHighlightChangePosition(staticTextPointer, LogicalDirection.Forward);
					}
					while (highlights.GetHighlightValue(staticTextPointer, LogicalDirection.Forward, typeof(TextSelection)) != DependencyProperty.UnsetValue);
					fixedHighlightType = FixedHighlightType.TextSelection;
					foregroundBrush = null;
					backgroundBrush = null;
				}
				else
				{
					AnnotationHighlightLayer.HighlightSegment highlightSegment = highlights.GetHighlightValue(textPosition, LogicalDirection.Forward, typeof(HighlightComponent)) as AnnotationHighlightLayer.HighlightSegment;
					if (highlightSegment != null)
					{
						staticTextPointer = highlights.GetNextHighlightChangePosition(staticTextPointer, LogicalDirection.Forward);
						fixedHighlightType = FixedHighlightType.AnnotationHighlight;
						backgroundBrush = highlightSegment.Fill;
					}
				}
				if (fixedHighlightType != FixedHighlightType.None)
				{
					this.FixedContainer.GetMultiHighlights((FixedTextPointer)textPosition.CreateDynamicTextPointer(LogicalDirection.Forward), (FixedTextPointer)staticTextPointer.CreateDynamicTextPointer(LogicalDirection.Forward), this._highlights, fixedHighlightType, foregroundBrush, backgroundBrush);
				}
			}
			ArrayList arrayList = new ArrayList();
			IList ranges = args.Ranges;
			for (int i = 0; i < ranges.Count; i++)
			{
				TextSegment textSegment = (TextSegment)ranges[i];
				int pageNumber = this.FixedContainer.GetPageNumber(textSegment.Start);
				int pageNumber2 = this.FixedContainer.GetPageNumber(textSegment.End);
				for (int j = pageNumber; j <= pageNumber2; j++)
				{
					if (arrayList.IndexOf(j) < 0)
					{
						arrayList.Add(j);
					}
				}
			}
			ICollection<FixedPage> keys = this._highlights.Keys;
			foreach (FixedPage fixedPage in list)
			{
				if (!keys.Contains(fixedPage))
				{
					int indexOfPage = this.GetIndexOfPage(fixedPage);
					if (indexOfPage >= 0 && indexOfPage < this.PageCount && arrayList.IndexOf(indexOfPage) < 0)
					{
						arrayList.Add(indexOfPage);
					}
				}
			}
			arrayList.Sort();
			foreach (object obj in arrayList)
			{
				int index = (int)obj;
				HighlightVisual highlightVisual = HighlightVisual.GetHighlightVisual(this.SyncGetPage(index, false));
				if (highlightVisual != null)
				{
					highlightVisual.InvalidateHighlights();
				}
			}
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x000CA808 File Offset: 0x000C8A08
		private object GetPageAsyncDelegate(object arg)
		{
			FixedDocument.GetPageAsyncRequest getPageAsyncRequest = (FixedDocument.GetPageAsyncRequest)arg;
			PageContent pageContent = getPageAsyncRequest.PageContent;
			if (!this._pendingPages.Contains(pageContent))
			{
				this._pendingPages.Add(pageContent);
				pageContent.GetPageRootCompleted += this.OnGetPageRootCompleted;
				pageContent.GetPageRootAsync(false);
				if (getPageAsyncRequest.Cancelled)
				{
					pageContent.GetPageRootAsyncCancel();
				}
			}
			return null;
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x000CA868 File Offset: 0x000C8A68
		private void OnGetPageRootCompleted(object sender, GetPageRootCompletedEventArgs args)
		{
			PageContent pageContent = (PageContent)sender;
			pageContent.GetPageRootCompleted -= this.OnGetPageRootCompleted;
			this._pendingPages.Remove(pageContent);
			ArrayList arrayList = new ArrayList();
			IEnumerator<KeyValuePair<object, FixedDocument.GetPageAsyncRequest>> enumerator = this._asyncOps.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<object, FixedDocument.GetPageAsyncRequest> keyValuePair = enumerator.Current;
					FixedDocument.GetPageAsyncRequest value = keyValuePair.Value;
					if (value.PageContent == pageContent)
					{
						ArrayList arrayList2 = arrayList;
						keyValuePair = enumerator.Current;
						arrayList2.Add(keyValuePair.Key);
						DocumentPage page = DocumentPage.Missing;
						if (!value.Cancelled && !args.Cancelled && args.Error == null)
						{
							FixedPage result = args.Result;
							Size fixedSize = this.ComputePageSize(result);
							page = new FixedDocumentPage(this, result, fixedSize, this.Pages.IndexOf(pageContent));
						}
						this._NotifyGetPageAsyncCompleted(page, value.PageNumber, args.Error, value.Cancelled, value.UserState);
					}
				}
			}
			finally
			{
				foreach (object key in arrayList)
				{
					this._asyncOps.Remove(key);
				}
			}
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x000CA9B8 File Offset: 0x000C8BB8
		private void _NotifyGetPageAsyncCompleted(DocumentPage page, int pageNumber, Exception error, bool cancelled, object userState)
		{
			this._paginator.NotifyGetPageCompleted(new GetPageCompletedEventArgs(page, pageNumber, error, cancelled, userState));
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FixedDocument.PrintTicket" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FixedDocument.PrintTicket" /> dependency property.</returns>
		// Token: 0x04001D32 RID: 7474
		public static readonly DependencyProperty PrintTicketProperty = DependencyProperty.RegisterAttached("PrintTicket", typeof(object), typeof(FixedDocument), new FrameworkPropertyMetadata(null));

		// Token: 0x04001D33 RID: 7475
		private IDictionary<object, FixedDocument.GetPageAsyncRequest> _asyncOps;

		// Token: 0x04001D34 RID: 7476
		private IList<PageContent> _pendingPages;

		// Token: 0x04001D35 RID: 7477
		private PageContentCollection _pages;

		// Token: 0x04001D36 RID: 7478
		private PageContent _partialPage;

		// Token: 0x04001D37 RID: 7479
		private Dictionary<FixedPage, ArrayList> _highlights;

		// Token: 0x04001D38 RID: 7480
		private double _pageWidth = 816.0;

		// Token: 0x04001D39 RID: 7481
		private double _pageHeight = 1056.0;

		// Token: 0x04001D3A RID: 7482
		private FixedTextContainer _fixedTextContainer;

		// Token: 0x04001D3B RID: 7483
		private RubberbandSelector _rubberbandSelector;

		// Token: 0x04001D3C RID: 7484
		private bool _navigateAfterPagination;

		// Token: 0x04001D3D RID: 7485
		private string _navigateFragment;

		// Token: 0x04001D3E RID: 7486
		private FixedDocumentPaginator _paginator;

		// Token: 0x04001D3F RID: 7487
		private DocumentReference _documentReference;

		// Token: 0x04001D40 RID: 7488
		private bool _hasExplicitStructure;

		// Token: 0x04001D41 RID: 7489
		private const string _structureRelationshipName = "http://schemas.microsoft.com/xps/2005/06/documentstructure";

		// Token: 0x04001D42 RID: 7490
		private const string _storyFragmentsRelationshipName = "http://schemas.microsoft.com/xps/2005/06/storyfragments";

		// Token: 0x04001D43 RID: 7491
		private static readonly ContentType _storyFragmentsContentType = new ContentType("application/vnd.ms-package.xps-storyfragments+xml");

		// Token: 0x04001D44 RID: 7492
		private static readonly ContentType _documentStructureContentType = new ContentType("application/vnd.ms-package.xps-documentstructure+xml");

		// Token: 0x04001D45 RID: 7493
		private static DependencyObjectType UIElementType = DependencyObjectType.FromSystemTypeInternal(typeof(UIElement));

		// Token: 0x020008CD RID: 2253
		private class GetPageAsyncRequest
		{
			// Token: 0x0600847D RID: 33917 RVA: 0x00248432 File Offset: 0x00246632
			internal GetPageAsyncRequest(PageContent pageContent, int pageNumber, object userState)
			{
				this.PageContent = pageContent;
				this.PageNumber = pageNumber;
				this.UserState = userState;
				this.Cancelled = false;
			}

			// Token: 0x04004232 RID: 16946
			internal PageContent PageContent;

			// Token: 0x04004233 RID: 16947
			internal int PageNumber;

			// Token: 0x04004234 RID: 16948
			internal object UserState;

			// Token: 0x04004235 RID: 16949
			internal bool Cancelled;
		}
	}
}
