using System;

namespace System.Windows.Documents
{
	// Token: 0x02000333 RID: 819
	internal sealed class ChildDocumentBlock
	{
		// Token: 0x06002B38 RID: 11064 RVA: 0x000C5613 File Offset: 0x000C3813
		internal ChildDocumentBlock(DocumentSequenceTextContainer aggregatedContainer, ITextContainer childContainer)
		{
			this._aggregatedContainer = aggregatedContainer;
			this._container = childContainer;
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x000C5629 File Offset: 0x000C3829
		internal ChildDocumentBlock(DocumentSequenceTextContainer aggregatedContainer, DocumentReference docRef)
		{
			this._aggregatedContainer = aggregatedContainer;
			this._docRef = docRef;
			this._SetStatus(ChildDocumentBlock.BlockStatus.UnloadedBlock);
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x000C5646 File Offset: 0x000C3846
		internal ChildDocumentBlock InsertNextBlock(ChildDocumentBlock newBlock)
		{
			newBlock._nextBlock = this._nextBlock;
			newBlock._previousBlock = this;
			if (this._nextBlock != null)
			{
				this._nextBlock._previousBlock = newBlock;
			}
			this._nextBlock = newBlock;
			return newBlock;
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06002B3B RID: 11067 RVA: 0x000C5677 File Offset: 0x000C3877
		internal DocumentSequenceTextContainer AggregatedContainer
		{
			get
			{
				return this._aggregatedContainer;
			}
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06002B3C RID: 11068 RVA: 0x000C567F File Offset: 0x000C387F
		internal ITextContainer ChildContainer
		{
			get
			{
				this._EnsureBlockLoaded();
				return this._container;
			}
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06002B3D RID: 11069 RVA: 0x000C568D File Offset: 0x000C388D
		internal DocumentSequenceHighlightLayer ChildHighlightLayer
		{
			get
			{
				if (this._highlightLayer == null)
				{
					this._highlightLayer = new DocumentSequenceHighlightLayer(this._aggregatedContainer);
					this.ChildContainer.Highlights.AddLayer(this._highlightLayer);
				}
				return this._highlightLayer;
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06002B3E RID: 11070 RVA: 0x000C56C4 File Offset: 0x000C38C4
		internal DocumentReference DocRef
		{
			get
			{
				return this._docRef;
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06002B3F RID: 11071 RVA: 0x000C56CC File Offset: 0x000C38CC
		internal ITextPointer End
		{
			get
			{
				return this.ChildContainer.End;
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06002B40 RID: 11072 RVA: 0x000C56D9 File Offset: 0x000C38D9
		internal bool IsHead
		{
			get
			{
				return this._previousBlock == null;
			}
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06002B41 RID: 11073 RVA: 0x000C56E4 File Offset: 0x000C38E4
		internal bool IsTail
		{
			get
			{
				return this._nextBlock == null;
			}
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06002B42 RID: 11074 RVA: 0x000C56EF File Offset: 0x000C38EF
		internal ChildDocumentBlock PreviousBlock
		{
			get
			{
				return this._previousBlock;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06002B43 RID: 11075 RVA: 0x000C56F7 File Offset: 0x000C38F7
		internal ChildDocumentBlock NextBlock
		{
			get
			{
				return this._nextBlock;
			}
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x000C5700 File Offset: 0x000C3900
		private void _EnsureBlockLoaded()
		{
			if (this._HasStatus(ChildDocumentBlock.BlockStatus.UnloadedBlock))
			{
				this._ClearStatus(ChildDocumentBlock.BlockStatus.UnloadedBlock);
				IDocumentPaginatorSource document = this._docRef.GetDocument(false);
				IServiceProvider serviceProvider = document as IServiceProvider;
				if (serviceProvider != null)
				{
					ITextContainer textContainer = serviceProvider.GetService(typeof(ITextContainer)) as ITextContainer;
					if (textContainer != null)
					{
						this._container = textContainer;
					}
				}
				if (this._container == null)
				{
					this._container = new NullTextContainer();
				}
			}
		}

		// Token: 0x06002B45 RID: 11077 RVA: 0x000C5767 File Offset: 0x000C3967
		private bool _HasStatus(ChildDocumentBlock.BlockStatus flags)
		{
			return (this._status & flags) == flags;
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x000C5774 File Offset: 0x000C3974
		private void _SetStatus(ChildDocumentBlock.BlockStatus flags)
		{
			this._status |= flags;
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x000C5784 File Offset: 0x000C3984
		private void _ClearStatus(ChildDocumentBlock.BlockStatus flags)
		{
			this._status &= ~flags;
		}

		// Token: 0x04001C83 RID: 7299
		private readonly DocumentSequenceTextContainer _aggregatedContainer;

		// Token: 0x04001C84 RID: 7300
		private readonly DocumentReference _docRef;

		// Token: 0x04001C85 RID: 7301
		private ITextContainer _container;

		// Token: 0x04001C86 RID: 7302
		private DocumentSequenceHighlightLayer _highlightLayer;

		// Token: 0x04001C87 RID: 7303
		private ChildDocumentBlock.BlockStatus _status;

		// Token: 0x04001C88 RID: 7304
		private ChildDocumentBlock _previousBlock;

		// Token: 0x04001C89 RID: 7305
		private ChildDocumentBlock _nextBlock;

		// Token: 0x020008C6 RID: 2246
		[Flags]
		internal enum BlockStatus
		{
			// Token: 0x0400421F RID: 16927
			None = 0,
			// Token: 0x04004220 RID: 16928
			UnloadedBlock = 1
		}
	}
}
