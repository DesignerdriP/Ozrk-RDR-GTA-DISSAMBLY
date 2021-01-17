using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000647 RID: 1607
	internal sealed class StructuralCache
	{
		// Token: 0x06006A55 RID: 27221 RVA: 0x001E4378 File Offset: 0x001E2578
		internal StructuralCache(FlowDocument owner, TextContainer textContainer)
		{
			Invariant.Assert(owner != null);
			Invariant.Assert(textContainer != null);
			Invariant.Assert(textContainer.Parent != null);
			this._owner = owner;
			this._textContainer = textContainer;
			this._backgroundFormatInfo = new BackgroundFormatInfo(this);
		}

		// Token: 0x06006A56 RID: 27222 RVA: 0x001E43C8 File Offset: 0x001E25C8
		~StructuralCache()
		{
			if (this._ptsContext != null)
			{
				PtsCache.ReleaseContext(this._ptsContext);
			}
		}

		// Token: 0x06006A57 RID: 27223 RVA: 0x001E4404 File Offset: 0x001E2604
		internal IDisposable SetDocumentFormatContext(FlowDocumentPage currentPage)
		{
			if (!this.CheckFlags(StructuralCache.Flags.FormattedOnce))
			{
				this.SetFlags(true, StructuralCache.Flags.FormattedOnce);
				this._owner.InitializeForFirstFormatting();
			}
			return new StructuralCache.DocumentFormatContext(this, currentPage);
		}

		// Token: 0x06006A58 RID: 27224 RVA: 0x001E4429 File Offset: 0x001E2629
		internal IDisposable SetDocumentArrangeContext(FlowDocumentPage currentPage)
		{
			return new StructuralCache.DocumentArrangeContext(this, currentPage);
		}

		// Token: 0x06006A59 RID: 27225 RVA: 0x001E4432 File Offset: 0x001E2632
		internal IDisposable SetDocumentVisualValidationContext(FlowDocumentPage currentPage)
		{
			return new StructuralCache.DocumentVisualValidationContext(this, currentPage);
		}

		// Token: 0x06006A5A RID: 27226 RVA: 0x001E443B File Offset: 0x001E263B
		internal void DetectInvalidOperation()
		{
			if (this._illegalTreeChangeDetected)
			{
				throw new InvalidOperationException(SR.Get("IllegalTreeChangeDetectedPostAction"));
			}
		}

		// Token: 0x06006A5B RID: 27227 RVA: 0x001E4455 File Offset: 0x001E2655
		internal void OnInvalidOperationDetected()
		{
			if (this._currentPage != null)
			{
				this._illegalTreeChangeDetected = true;
			}
		}

		// Token: 0x06006A5C RID: 27228 RVA: 0x001E4466 File Offset: 0x001E2666
		internal void InvalidateFormatCache(bool destroyStructure)
		{
			if (this._section != null)
			{
				this._section.InvalidateFormatCache();
				this._destroyStructure = (this._destroyStructure || destroyStructure);
				this._forceReformat = true;
			}
		}

		// Token: 0x06006A5D RID: 27229 RVA: 0x001E4490 File Offset: 0x001E2690
		internal void AddDirtyTextRange(DirtyTextRange dtr)
		{
			if (this._dtrs == null)
			{
				this._dtrs = new DtrList();
			}
			this._dtrs.Merge(dtr);
		}

		// Token: 0x06006A5E RID: 27230 RVA: 0x001E44B1 File Offset: 0x001E26B1
		internal DtrList DtrsFromRange(int dcpNew, int cchOld)
		{
			if (this._dtrs == null)
			{
				return null;
			}
			return this._dtrs.DtrsFromRange(dcpNew, cchOld);
		}

		// Token: 0x06006A5F RID: 27231 RVA: 0x001E44CC File Offset: 0x001E26CC
		internal void ClearUpdateInfo(bool destroyStructureCache)
		{
			this._dtrs = null;
			this._forceReformat = false;
			this._destroyStructure = false;
			if (this._section != null && !this._ptsContext.Disposed)
			{
				if (destroyStructureCache)
				{
					this._section.DestroyStructure();
				}
				this._section.ClearUpdateInfo();
			}
		}

		// Token: 0x06006A60 RID: 27232 RVA: 0x001E451C File Offset: 0x001E271C
		internal void ThrottleBackgroundFormatting()
		{
			this._backgroundFormatInfo.ThrottleBackgroundFormatting();
		}

		// Token: 0x06006A61 RID: 27233 RVA: 0x001E4529 File Offset: 0x001E2729
		internal bool HasPtsContext()
		{
			return this._ptsContext != null;
		}

		// Token: 0x1700198E RID: 6542
		// (get) Token: 0x06006A62 RID: 27234 RVA: 0x001E4534 File Offset: 0x001E2734
		internal DependencyObject PropertyOwner
		{
			get
			{
				return this._textContainer.Parent;
			}
		}

		// Token: 0x1700198F RID: 6543
		// (get) Token: 0x06006A63 RID: 27235 RVA: 0x001E4541 File Offset: 0x001E2741
		internal FlowDocument FormattingOwner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17001990 RID: 6544
		// (get) Token: 0x06006A64 RID: 27236 RVA: 0x001E4549 File Offset: 0x001E2749
		internal Section Section
		{
			get
			{
				this.EnsurePtsContext();
				return this._section;
			}
		}

		// Token: 0x17001991 RID: 6545
		// (get) Token: 0x06006A65 RID: 27237 RVA: 0x001E4557 File Offset: 0x001E2757
		internal NaturalLanguageHyphenator Hyphenator
		{
			get
			{
				this.EnsureHyphenator();
				return this._hyphenator;
			}
		}

		// Token: 0x17001992 RID: 6546
		// (get) Token: 0x06006A66 RID: 27238 RVA: 0x001E4565 File Offset: 0x001E2765
		internal PtsContext PtsContext
		{
			get
			{
				this.EnsurePtsContext();
				return this._ptsContext;
			}
		}

		// Token: 0x17001993 RID: 6547
		// (get) Token: 0x06006A67 RID: 27239 RVA: 0x001E4573 File Offset: 0x001E2773
		internal StructuralCache.DocumentFormatContext CurrentFormatContext
		{
			get
			{
				return this._documentFormatContext;
			}
		}

		// Token: 0x17001994 RID: 6548
		// (get) Token: 0x06006A68 RID: 27240 RVA: 0x001E457B File Offset: 0x001E277B
		internal StructuralCache.DocumentArrangeContext CurrentArrangeContext
		{
			get
			{
				return this._documentArrangeContext;
			}
		}

		// Token: 0x17001995 RID: 6549
		// (get) Token: 0x06006A69 RID: 27241 RVA: 0x001E4583 File Offset: 0x001E2783
		internal TextFormatterHost TextFormatterHost
		{
			get
			{
				this.EnsurePtsContext();
				return this._textFormatterHost;
			}
		}

		// Token: 0x17001996 RID: 6550
		// (get) Token: 0x06006A6A RID: 27242 RVA: 0x001E4591 File Offset: 0x001E2791
		internal TextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x17001997 RID: 6551
		// (get) Token: 0x06006A6B RID: 27243 RVA: 0x001E4599 File Offset: 0x001E2799
		// (set) Token: 0x06006A6C RID: 27244 RVA: 0x001E45A1 File Offset: 0x001E27A1
		internal FlowDirection PageFlowDirection
		{
			get
			{
				return this._pageFlowDirection;
			}
			set
			{
				this._pageFlowDirection = value;
			}
		}

		// Token: 0x17001998 RID: 6552
		// (get) Token: 0x06006A6D RID: 27245 RVA: 0x001E45AA File Offset: 0x001E27AA
		// (set) Token: 0x06006A6E RID: 27246 RVA: 0x001E45B2 File Offset: 0x001E27B2
		internal bool ForceReformat
		{
			get
			{
				return this._forceReformat;
			}
			set
			{
				this._forceReformat = value;
			}
		}

		// Token: 0x17001999 RID: 6553
		// (get) Token: 0x06006A6F RID: 27247 RVA: 0x001E45BB File Offset: 0x001E27BB
		internal bool DestroyStructure
		{
			get
			{
				return this._destroyStructure;
			}
		}

		// Token: 0x1700199A RID: 6554
		// (get) Token: 0x06006A70 RID: 27248 RVA: 0x001E45C3 File Offset: 0x001E27C3
		internal DtrList DtrList
		{
			get
			{
				return this._dtrs;
			}
		}

		// Token: 0x1700199B RID: 6555
		// (get) Token: 0x06006A71 RID: 27249 RVA: 0x001E45CB File Offset: 0x001E27CB
		internal bool IsDeferredVisualCreationSupported
		{
			get
			{
				return this._currentPage != null && !this._currentPage.FinitePage;
			}
		}

		// Token: 0x1700199C RID: 6556
		// (get) Token: 0x06006A72 RID: 27250 RVA: 0x001E45E5 File Offset: 0x001E27E5
		internal BackgroundFormatInfo BackgroundFormatInfo
		{
			get
			{
				return this._backgroundFormatInfo;
			}
		}

		// Token: 0x1700199D RID: 6557
		// (get) Token: 0x06006A73 RID: 27251 RVA: 0x001E45ED File Offset: 0x001E27ED
		internal bool IsOptimalParagraphEnabled
		{
			get
			{
				return this.PtsContext.IsOptimalParagraphEnabled && (bool)this.PropertyOwner.GetValue(FlowDocument.IsOptimalParagraphEnabledProperty);
			}
		}

		// Token: 0x1700199E RID: 6558
		// (get) Token: 0x06006A74 RID: 27252 RVA: 0x001E4613 File Offset: 0x001E2813
		// (set) Token: 0x06006A75 RID: 27253 RVA: 0x001E461C File Offset: 0x001E281C
		internal bool IsFormattingInProgress
		{
			get
			{
				return this.CheckFlags(StructuralCache.Flags.FormattingInProgress);
			}
			set
			{
				this.SetFlags(value, StructuralCache.Flags.FormattingInProgress);
			}
		}

		// Token: 0x1700199F RID: 6559
		// (get) Token: 0x06006A76 RID: 27254 RVA: 0x001E4626 File Offset: 0x001E2826
		// (set) Token: 0x06006A77 RID: 27255 RVA: 0x001E462F File Offset: 0x001E282F
		internal bool IsContentChangeInProgress
		{
			get
			{
				return this.CheckFlags(StructuralCache.Flags.ContentChangeInProgress);
			}
			set
			{
				this.SetFlags(value, StructuralCache.Flags.ContentChangeInProgress);
			}
		}

		// Token: 0x170019A0 RID: 6560
		// (get) Token: 0x06006A78 RID: 27256 RVA: 0x001E4639 File Offset: 0x001E2839
		// (set) Token: 0x06006A79 RID: 27257 RVA: 0x001E4642 File Offset: 0x001E2842
		internal bool IsFormattedOnce
		{
			get
			{
				return this.CheckFlags(StructuralCache.Flags.FormattedOnce);
			}
			set
			{
				this.SetFlags(value, StructuralCache.Flags.FormattedOnce);
			}
		}

		// Token: 0x06006A7A RID: 27258 RVA: 0x001E464C File Offset: 0x001E284C
		private void EnsureHyphenator()
		{
			if (this._hyphenator == null)
			{
				this._hyphenator = new NaturalLanguageHyphenator();
			}
		}

		// Token: 0x06006A7B RID: 27259 RVA: 0x001E4664 File Offset: 0x001E2864
		private void EnsurePtsContext()
		{
			if (this._ptsContext == null)
			{
				TextFormattingMode textFormattingMode = TextOptions.GetTextFormattingMode(this.PropertyOwner);
				this._ptsContext = new PtsContext(true, textFormattingMode);
				this._textFormatterHost = new TextFormatterHost(this._ptsContext.TextFormatter, textFormattingMode, this._owner.PixelsPerDip);
				this._section = new Section(this);
			}
		}

		// Token: 0x06006A7C RID: 27260 RVA: 0x001E46C0 File Offset: 0x001E28C0
		private void SetFlags(bool value, StructuralCache.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x06006A7D RID: 27261 RVA: 0x001E46DE File Offset: 0x001E28DE
		private bool CheckFlags(StructuralCache.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x04003436 RID: 13366
		private readonly FlowDocument _owner;

		// Token: 0x04003437 RID: 13367
		private PtsContext _ptsContext;

		// Token: 0x04003438 RID: 13368
		private Section _section;

		// Token: 0x04003439 RID: 13369
		private TextContainer _textContainer;

		// Token: 0x0400343A RID: 13370
		private TextFormatterHost _textFormatterHost;

		// Token: 0x0400343B RID: 13371
		private FlowDocumentPage _currentPage;

		// Token: 0x0400343C RID: 13372
		private StructuralCache.DocumentFormatContext _documentFormatContext;

		// Token: 0x0400343D RID: 13373
		private StructuralCache.DocumentArrangeContext _documentArrangeContext;

		// Token: 0x0400343E RID: 13374
		private DtrList _dtrs;

		// Token: 0x0400343F RID: 13375
		private bool _illegalTreeChangeDetected;

		// Token: 0x04003440 RID: 13376
		private bool _forceReformat;

		// Token: 0x04003441 RID: 13377
		private bool _destroyStructure;

		// Token: 0x04003442 RID: 13378
		private BackgroundFormatInfo _backgroundFormatInfo;

		// Token: 0x04003443 RID: 13379
		private FlowDirection _pageFlowDirection;

		// Token: 0x04003444 RID: 13380
		private NaturalLanguageHyphenator _hyphenator;

		// Token: 0x04003445 RID: 13381
		private StructuralCache.Flags _flags;

		// Token: 0x02000A21 RID: 2593
		[Flags]
		private enum Flags
		{
			// Token: 0x0400470E RID: 18190
			FormattedOnce = 1,
			// Token: 0x0400470F RID: 18191
			ContentChangeInProgress = 2,
			// Token: 0x04004710 RID: 18192
			FormattingInProgress = 8
		}

		// Token: 0x02000A22 RID: 2594
		internal abstract class DocumentOperationContext
		{
			// Token: 0x06008AB0 RID: 35504 RVA: 0x002576EC File Offset: 0x002558EC
			internal DocumentOperationContext(StructuralCache owner, FlowDocumentPage page)
			{
				Invariant.Assert(owner != null, "Invalid owner object.");
				Invariant.Assert(page != null, "Invalid page object.");
				Invariant.Assert(owner._currentPage == null, "Page formatting reentrancy detected. Trying to create second _DocumentPageContext for the same StructuralCache.");
				this._owner = owner;
				this._owner._currentPage = page;
				this._owner._illegalTreeChangeDetected = false;
				owner.PtsContext.Enter();
			}

			// Token: 0x06008AB1 RID: 35505 RVA: 0x00257758 File Offset: 0x00255958
			protected void Dispose()
			{
				Invariant.Assert(this._owner._currentPage != null, "DocumentPageContext is already disposed.");
				try
				{
					this._owner.PtsContext.Leave();
				}
				finally
				{
					this._owner._currentPage = null;
				}
			}

			// Token: 0x17001F57 RID: 8023
			// (get) Token: 0x06008AB2 RID: 35506 RVA: 0x002577AC File Offset: 0x002559AC
			internal Size DocumentPageSize
			{
				get
				{
					return this._owner._currentPage.Size;
				}
			}

			// Token: 0x17001F58 RID: 8024
			// (get) Token: 0x06008AB3 RID: 35507 RVA: 0x002577BE File Offset: 0x002559BE
			internal Thickness DocumentPageMargin
			{
				get
				{
					return this._owner._currentPage.Margin;
				}
			}

			// Token: 0x04004711 RID: 18193
			protected readonly StructuralCache _owner;
		}

		// Token: 0x02000A23 RID: 2595
		internal class DocumentFormatContext : StructuralCache.DocumentOperationContext, IDisposable
		{
			// Token: 0x06008AB4 RID: 35508 RVA: 0x002577D0 File Offset: 0x002559D0
			internal DocumentFormatContext(StructuralCache owner, FlowDocumentPage page) : base(owner, page)
			{
				this._owner._documentFormatContext = this;
			}

			// Token: 0x06008AB5 RID: 35509 RVA: 0x002577F1 File Offset: 0x002559F1
			void IDisposable.Dispose()
			{
				this._owner._documentFormatContext = null;
				base.Dispose();
				GC.SuppressFinalize(this);
			}

			// Token: 0x06008AB6 RID: 35510 RVA: 0x0025780B File Offset: 0x00255A0B
			internal void OnFormatLine()
			{
				this._owner._currentPage.OnFormatLine();
			}

			// Token: 0x06008AB7 RID: 35511 RVA: 0x00257820 File Offset: 0x00255A20
			internal void PushNewPageData(Size pageSize, Thickness pageMargin, bool incrementalUpdate, bool finitePage)
			{
				this._documentFormatInfoStack.Push(this._currentFormatInfo);
				this._currentFormatInfo.PageSize = pageSize;
				this._currentFormatInfo.PageMargin = pageMargin;
				this._currentFormatInfo.IncrementalUpdate = incrementalUpdate;
				this._currentFormatInfo.FinitePage = finitePage;
			}

			// Token: 0x06008AB8 RID: 35512 RVA: 0x0025786F File Offset: 0x00255A6F
			internal void PopPageData()
			{
				this._currentFormatInfo = this._documentFormatInfoStack.Pop();
			}

			// Token: 0x17001F59 RID: 8025
			// (get) Token: 0x06008AB9 RID: 35513 RVA: 0x00257882 File Offset: 0x00255A82
			internal double PageHeight
			{
				get
				{
					return this._currentFormatInfo.PageSize.Height;
				}
			}

			// Token: 0x17001F5A RID: 8026
			// (get) Token: 0x06008ABA RID: 35514 RVA: 0x00257894 File Offset: 0x00255A94
			internal double PageWidth
			{
				get
				{
					return this._currentFormatInfo.PageSize.Width;
				}
			}

			// Token: 0x17001F5B RID: 8027
			// (get) Token: 0x06008ABB RID: 35515 RVA: 0x002578A6 File Offset: 0x00255AA6
			internal Size PageSize
			{
				get
				{
					return this._currentFormatInfo.PageSize;
				}
			}

			// Token: 0x17001F5C RID: 8028
			// (get) Token: 0x06008ABC RID: 35516 RVA: 0x002578B3 File Offset: 0x00255AB3
			internal Thickness PageMargin
			{
				get
				{
					return this._currentFormatInfo.PageMargin;
				}
			}

			// Token: 0x17001F5D RID: 8029
			// (get) Token: 0x06008ABD RID: 35517 RVA: 0x002578C0 File Offset: 0x00255AC0
			internal bool IncrementalUpdate
			{
				get
				{
					return this._currentFormatInfo.IncrementalUpdate;
				}
			}

			// Token: 0x17001F5E RID: 8030
			// (get) Token: 0x06008ABE RID: 35518 RVA: 0x002578CD File Offset: 0x00255ACD
			internal bool FinitePage
			{
				get
				{
					return this._currentFormatInfo.FinitePage;
				}
			}

			// Token: 0x17001F5F RID: 8031
			// (get) Token: 0x06008ABF RID: 35519 RVA: 0x002578DA File Offset: 0x00255ADA
			internal PTS.FSRECT PageRect
			{
				get
				{
					return new PTS.FSRECT(new Rect(0.0, 0.0, this.PageWidth, this.PageHeight));
				}
			}

			// Token: 0x17001F60 RID: 8032
			// (get) Token: 0x06008AC0 RID: 35520 RVA: 0x00257904 File Offset: 0x00255B04
			internal PTS.FSRECT PageMarginRect
			{
				get
				{
					return new PTS.FSRECT(new Rect(this.PageMargin.Left, this.PageMargin.Top, this.PageSize.Width - this.PageMargin.Left - this.PageMargin.Right, this.PageSize.Height - this.PageMargin.Top - this.PageMargin.Bottom));
				}
			}

			// Token: 0x17001F61 RID: 8033
			// (set) Token: 0x06008AC1 RID: 35521 RVA: 0x0025798F File Offset: 0x00255B8F
			internal TextPointer DependentMax
			{
				set
				{
					this._owner._currentPage.DependentMax = value;
				}
			}

			// Token: 0x04004712 RID: 18194
			private StructuralCache.DocumentFormatContext.DocumentFormatInfo _currentFormatInfo;

			// Token: 0x04004713 RID: 18195
			private Stack<StructuralCache.DocumentFormatContext.DocumentFormatInfo> _documentFormatInfoStack = new Stack<StructuralCache.DocumentFormatContext.DocumentFormatInfo>();

			// Token: 0x02000BB4 RID: 2996
			private struct DocumentFormatInfo
			{
				// Token: 0x04004EEE RID: 20206
				internal Size PageSize;

				// Token: 0x04004EEF RID: 20207
				internal Thickness PageMargin;

				// Token: 0x04004EF0 RID: 20208
				internal bool IncrementalUpdate;

				// Token: 0x04004EF1 RID: 20209
				internal bool FinitePage;
			}
		}

		// Token: 0x02000A24 RID: 2596
		internal class DocumentArrangeContext : StructuralCache.DocumentOperationContext, IDisposable
		{
			// Token: 0x06008AC2 RID: 35522 RVA: 0x002579A2 File Offset: 0x00255BA2
			internal DocumentArrangeContext(StructuralCache owner, FlowDocumentPage page) : base(owner, page)
			{
				this._owner._documentArrangeContext = this;
			}

			// Token: 0x06008AC3 RID: 35523 RVA: 0x002579C3 File Offset: 0x00255BC3
			internal void PushNewPageData(PageContext pageContext, PTS.FSRECT columnRect, bool finitePage)
			{
				this._documentArrangeInfoStack.Push(this._currentArrangeInfo);
				this._currentArrangeInfo.PageContext = pageContext;
				this._currentArrangeInfo.ColumnRect = columnRect;
				this._currentArrangeInfo.FinitePage = finitePage;
			}

			// Token: 0x06008AC4 RID: 35524 RVA: 0x002579FA File Offset: 0x00255BFA
			internal void PopPageData()
			{
				this._currentArrangeInfo = this._documentArrangeInfoStack.Pop();
			}

			// Token: 0x06008AC5 RID: 35525 RVA: 0x00257A0D File Offset: 0x00255C0D
			void IDisposable.Dispose()
			{
				GC.SuppressFinalize(this);
				this._owner._documentArrangeContext = null;
				base.Dispose();
			}

			// Token: 0x17001F62 RID: 8034
			// (get) Token: 0x06008AC6 RID: 35526 RVA: 0x00257A27 File Offset: 0x00255C27
			internal PageContext PageContext
			{
				get
				{
					return this._currentArrangeInfo.PageContext;
				}
			}

			// Token: 0x17001F63 RID: 8035
			// (get) Token: 0x06008AC7 RID: 35527 RVA: 0x00257A34 File Offset: 0x00255C34
			internal PTS.FSRECT ColumnRect
			{
				get
				{
					return this._currentArrangeInfo.ColumnRect;
				}
			}

			// Token: 0x17001F64 RID: 8036
			// (get) Token: 0x06008AC8 RID: 35528 RVA: 0x00257A41 File Offset: 0x00255C41
			internal bool FinitePage
			{
				get
				{
					return this._currentArrangeInfo.FinitePage;
				}
			}

			// Token: 0x04004714 RID: 18196
			private StructuralCache.DocumentArrangeContext.DocumentArrangeInfo _currentArrangeInfo;

			// Token: 0x04004715 RID: 18197
			private Stack<StructuralCache.DocumentArrangeContext.DocumentArrangeInfo> _documentArrangeInfoStack = new Stack<StructuralCache.DocumentArrangeContext.DocumentArrangeInfo>();

			// Token: 0x02000BB5 RID: 2997
			private struct DocumentArrangeInfo
			{
				// Token: 0x04004EF2 RID: 20210
				internal PageContext PageContext;

				// Token: 0x04004EF3 RID: 20211
				internal PTS.FSRECT ColumnRect;

				// Token: 0x04004EF4 RID: 20212
				internal bool FinitePage;
			}
		}

		// Token: 0x02000A25 RID: 2597
		internal class DocumentVisualValidationContext : StructuralCache.DocumentOperationContext, IDisposable
		{
			// Token: 0x06008AC9 RID: 35529 RVA: 0x00257A4E File Offset: 0x00255C4E
			internal DocumentVisualValidationContext(StructuralCache owner, FlowDocumentPage page) : base(owner, page)
			{
			}

			// Token: 0x06008ACA RID: 35530 RVA: 0x00257A58 File Offset: 0x00255C58
			void IDisposable.Dispose()
			{
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}
	}
}
