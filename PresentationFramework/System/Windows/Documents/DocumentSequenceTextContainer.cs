using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x0200033C RID: 828
	internal sealed class DocumentSequenceTextContainer : ITextContainer
	{
		// Token: 0x06002BA4 RID: 11172 RVA: 0x000C7305 File Offset: 0x000C5505
		internal DocumentSequenceTextContainer(DependencyObject parent)
		{
			this._parent = (FixedDocumentSequence)parent;
			this._Initialize();
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x000C731F File Offset: 0x000C551F
		void ITextContainer.BeginChange()
		{
			this._changeBlockLevel++;
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x000C732F File Offset: 0x000C552F
		void ITextContainer.BeginChangeNoUndo()
		{
			((ITextContainer)this).BeginChange();
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x000C7337 File Offset: 0x000C5537
		void ITextContainer.EndChange()
		{
			((ITextContainer)this).EndChange(false);
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x000C7340 File Offset: 0x000C5540
		void ITextContainer.EndChange(bool skipEvents)
		{
			Invariant.Assert(this._changeBlockLevel > 0, "Unmatched EndChange call!");
			this._changeBlockLevel--;
			if (this._changeBlockLevel == 0 && this._changes != null)
			{
				TextContainerChangedEventArgs changes = this._changes;
				this._changes = null;
				if (this.Changed != null && !skipEvents)
				{
					this.Changed(this, changes);
				}
			}
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x000C73A4 File Offset: 0x000C55A4
		ITextPointer ITextContainer.CreatePointerAtOffset(int offset, LogicalDirection direction)
		{
			return ((ITextContainer)this).Start.CreatePointer(offset, direction);
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x0003E264 File Offset: 0x0003C464
		ITextPointer ITextContainer.CreatePointerAtCharOffset(int charOffset, LogicalDirection direction)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x000C73B3 File Offset: 0x000C55B3
		ITextPointer ITextContainer.CreateDynamicTextPointer(StaticTextPointer position, LogicalDirection direction)
		{
			return ((ITextPointer)position.Handle0).CreatePointer(direction);
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x000C73C7 File Offset: 0x000C55C7
		StaticTextPointer ITextContainer.CreateStaticPointerAtOffset(int offset)
		{
			return new StaticTextPointer(this, ((ITextContainer)this).CreatePointerAtOffset(offset, LogicalDirection.Forward));
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x000C73D7 File Offset: 0x000C55D7
		TextPointerContext ITextContainer.GetPointerContext(StaticTextPointer pointer, LogicalDirection direction)
		{
			return ((ITextPointer)pointer.Handle0).GetPointerContext(direction);
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x000C73EB File Offset: 0x000C55EB
		int ITextContainer.GetOffsetToPosition(StaticTextPointer position1, StaticTextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).GetOffsetToPosition((ITextPointer)position2.Handle0);
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x000C740A File Offset: 0x000C560A
		int ITextContainer.GetTextInRun(StaticTextPointer position, LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			return ((ITextPointer)position.Handle0).GetTextInRun(direction, textBuffer, startIndex, count);
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x000C7423 File Offset: 0x000C5623
		object ITextContainer.GetAdjacentElement(StaticTextPointer position, LogicalDirection direction)
		{
			return ((ITextPointer)position.Handle0).GetAdjacentElement(direction);
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x0000C238 File Offset: 0x0000A438
		DependencyObject ITextContainer.GetParent(StaticTextPointer position)
		{
			return null;
		}

		// Token: 0x06002BB2 RID: 11186 RVA: 0x000C7437 File Offset: 0x000C5637
		StaticTextPointer ITextContainer.CreatePointer(StaticTextPointer position, int offset)
		{
			return new StaticTextPointer(this, ((ITextPointer)position.Handle0).CreatePointer(offset));
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x000C7451 File Offset: 0x000C5651
		StaticTextPointer ITextContainer.GetNextContextPosition(StaticTextPointer position, LogicalDirection direction)
		{
			return new StaticTextPointer(this, ((ITextPointer)position.Handle0).GetNextContextPosition(direction));
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x000C746B File Offset: 0x000C566B
		int ITextContainer.CompareTo(StaticTextPointer position1, StaticTextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).CompareTo((ITextPointer)position2.Handle0);
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x000C748A File Offset: 0x000C568A
		int ITextContainer.CompareTo(StaticTextPointer position1, ITextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).CompareTo(position2);
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x000C749E File Offset: 0x000C569E
		object ITextContainer.GetValue(StaticTextPointer position, DependencyProperty formattingProperty)
		{
			return ((ITextPointer)position.Handle0).GetValue(formattingProperty);
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06002BB7 RID: 11191 RVA: 0x00016748 File Offset: 0x00014948
		bool ITextContainer.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06002BB8 RID: 11192 RVA: 0x000C74B2 File Offset: 0x000C56B2
		ITextPointer ITextContainer.Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06002BB9 RID: 11193 RVA: 0x000C74BA File Offset: 0x000C56BA
		ITextPointer ITextContainer.End
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06002BBA RID: 11194 RVA: 0x0000B02A File Offset: 0x0000922A
		uint ITextContainer.Generation
		{
			get
			{
				return 0U;
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06002BBB RID: 11195 RVA: 0x000C74C2 File Offset: 0x000C56C2
		Highlights ITextContainer.Highlights
		{
			get
			{
				return this.Highlights;
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06002BBC RID: 11196 RVA: 0x000C74CA File Offset: 0x000C56CA
		DependencyObject ITextContainer.Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06002BBD RID: 11197 RVA: 0x000C74D2 File Offset: 0x000C56D2
		// (set) Token: 0x06002BBE RID: 11198 RVA: 0x000C74DA File Offset: 0x000C56DA
		ITextSelection ITextContainer.TextSelection
		{
			get
			{
				return this.TextSelection;
			}
			set
			{
				this._textSelection = value;
			}
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06002BBF RID: 11199 RVA: 0x0000C238 File Offset: 0x0000A438
		UndoManager ITextContainer.UndoManager
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06002BC0 RID: 11200 RVA: 0x000C74E3 File Offset: 0x000C56E3
		// (set) Token: 0x06002BC1 RID: 11201 RVA: 0x000C74EB File Offset: 0x000C56EB
		ITextView ITextContainer.TextView
		{
			get
			{
				return this._textview;
			}
			set
			{
				this._textview = value;
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06002BC2 RID: 11202 RVA: 0x000C74F4 File Offset: 0x000C56F4
		int ITextContainer.SymbolCount
		{
			get
			{
				return ((ITextContainer)this).Start.GetOffsetToPosition(((ITextContainer)this).End);
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06002BC3 RID: 11203 RVA: 0x0003E264 File Offset: 0x0003C464
		int ITextContainer.IMECharCount
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06002BC4 RID: 11204 RVA: 0x000C7508 File Offset: 0x000C5708
		// (remove) Token: 0x06002BC5 RID: 11205 RVA: 0x000C7540 File Offset: 0x000C5740
		public event EventHandler Changing;

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06002BC6 RID: 11206 RVA: 0x000C7578 File Offset: 0x000C5778
		// (remove) Token: 0x06002BC7 RID: 11207 RVA: 0x000C75B0 File Offset: 0x000C57B0
		public event TextContainerChangeEventHandler Change;

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x06002BC8 RID: 11208 RVA: 0x000C75E8 File Offset: 0x000C57E8
		// (remove) Token: 0x06002BC9 RID: 11209 RVA: 0x000C7620 File Offset: 0x000C5820
		public event TextContainerChangedEventHandler Changed;

		// Token: 0x06002BCA RID: 11210 RVA: 0x000C7658 File Offset: 0x000C5858
		internal DocumentSequenceTextPointer VerifyPosition(ITextPointer position)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			if (position.TextContainer != this)
			{
				throw new ArgumentException(SR.Get("NotInAssociatedContainer", new object[]
				{
					"position"
				}));
			}
			DocumentSequenceTextPointer documentSequenceTextPointer = position as DocumentSequenceTextPointer;
			if (documentSequenceTextPointer == null)
			{
				throw new ArgumentException(SR.Get("BadFixedTextPosition", new object[]
				{
					"position"
				}));
			}
			return documentSequenceTextPointer;
		}

		// Token: 0x06002BCB RID: 11211 RVA: 0x000C76C4 File Offset: 0x000C58C4
		internal DocumentSequenceTextPointer MapChildPositionToParent(ITextPointer tp)
		{
			for (ChildDocumentBlock childDocumentBlock = this._doclistHead; childDocumentBlock != null; childDocumentBlock = childDocumentBlock.NextBlock)
			{
				if (childDocumentBlock.ChildContainer == tp.TextContainer)
				{
					return new DocumentSequenceTextPointer(childDocumentBlock, tp);
				}
			}
			return null;
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x000C76FC File Offset: 0x000C58FC
		internal ChildDocumentBlock FindChildBlock(DocumentReference docRef)
		{
			for (ChildDocumentBlock nextBlock = this._doclistHead.NextBlock; nextBlock != null; nextBlock = nextBlock.NextBlock)
			{
				if (nextBlock.DocRef == docRef)
				{
					return nextBlock;
				}
			}
			return null;
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x000C7730 File Offset: 0x000C5930
		internal int GetChildBlockDistance(ChildDocumentBlock block1, ChildDocumentBlock block2)
		{
			if (block1 == block2)
			{
				return 0;
			}
			int num = 0;
			for (ChildDocumentBlock childDocumentBlock = block1; childDocumentBlock != null; childDocumentBlock = childDocumentBlock.NextBlock)
			{
				if (childDocumentBlock == block2)
				{
					return num;
				}
				num++;
			}
			num = 0;
			for (ChildDocumentBlock childDocumentBlock = block1; childDocumentBlock != null; childDocumentBlock = childDocumentBlock.PreviousBlock)
			{
				if (childDocumentBlock == block2)
				{
					return num;
				}
				num--;
			}
			return 0;
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06002BCE RID: 11214 RVA: 0x000C7778 File Offset: 0x000C5978
		internal Highlights Highlights
		{
			get
			{
				if (this._highlights == null)
				{
					this._highlights = new DocumentSequenceTextContainer.DocumentSequenceHighlights(this);
				}
				return this._highlights;
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06002BCF RID: 11215 RVA: 0x000C7794 File Offset: 0x000C5994
		internal ITextSelection TextSelection
		{
			get
			{
				return this._textSelection;
			}
		}

		// Token: 0x06002BD0 RID: 11216 RVA: 0x000C779C File Offset: 0x000C599C
		private void _Initialize()
		{
			this._doclistHead = new ChildDocumentBlock(this, new NullTextContainer());
			this._doclistTail = new ChildDocumentBlock(this, new NullTextContainer());
			this._doclistHead.InsertNextBlock(this._doclistTail);
			ChildDocumentBlock childDocumentBlock = this._doclistHead;
			foreach (DocumentReference docRef in this._parent.References)
			{
				childDocumentBlock.InsertNextBlock(new ChildDocumentBlock(this, docRef));
				childDocumentBlock = childDocumentBlock.NextBlock;
			}
			if (this._parent.References.Count != 0)
			{
				this._start = new DocumentSequenceTextPointer(this._doclistHead.NextBlock, this._doclistHead.NextBlock.ChildContainer.Start);
				this._end = new DocumentSequenceTextPointer(this._doclistTail.PreviousBlock, this._doclistTail.PreviousBlock.ChildContainer.End);
			}
			else
			{
				this._start = new DocumentSequenceTextPointer(this._doclistHead, this._doclistHead.ChildContainer.Start);
				this._end = new DocumentSequenceTextPointer(this._doclistTail, this._doclistTail.ChildContainer.End);
			}
			this._parent.References.CollectionChanged += this._OnContentChanged;
			this.Highlights.Changed += this._OnHighlightChanged;
		}

		// Token: 0x06002BD1 RID: 11217 RVA: 0x000C7918 File Offset: 0x000C5B18
		private void AddChange(ITextPointer startPosition, int symbolCount, PrecursorTextChangeType precursorTextChange)
		{
			Invariant.Assert(!this._isReadOnly, "Illegal to modify DocumentSequenceTextContainer inside Change event scope!");
			((ITextContainer)this).BeginChange();
			try
			{
				if (this.Changing != null)
				{
					this.Changing(this, EventArgs.Empty);
				}
				if (this._changes == null)
				{
					this._changes = new TextContainerChangedEventArgs();
				}
				this._changes.AddChange(precursorTextChange, DocumentSequenceTextPointer.GetOffsetToPosition(this._start, startPosition), symbolCount, false);
				if (this.Change != null)
				{
					Invariant.Assert(precursorTextChange == PrecursorTextChangeType.ContentAdded || precursorTextChange == PrecursorTextChangeType.ContentRemoved);
					TextChangeType textChange = (precursorTextChange == PrecursorTextChangeType.ContentAdded) ? TextChangeType.ContentAdded : TextChangeType.ContentRemoved;
					this._isReadOnly = true;
					try
					{
						this.Change(this, new TextContainerChangeEventArgs(startPosition, symbolCount, -1, textChange));
					}
					finally
					{
						this._isReadOnly = false;
					}
				}
			}
			finally
			{
				((ITextContainer)this).EndChange();
			}
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x000C79F0 File Offset: 0x000C5BF0
		private void _OnContentChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (args.Action != NotifyCollectionChangedAction.Add)
			{
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					args.Action
				}));
			}
			if (args.NewItems.Count != 1)
			{
				throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
			}
			object obj = args.NewItems[0];
			int newStartingIndex = args.NewStartingIndex;
			if (newStartingIndex != this._parent.References.Count - 1)
			{
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					args.Action
				}));
			}
			ChildDocumentBlock childDocumentBlock = new ChildDocumentBlock(this, (DocumentReference)obj);
			ChildDocumentBlock previousBlock = this._doclistTail.PreviousBlock;
			previousBlock.InsertNextBlock(childDocumentBlock);
			DocumentSequenceTextPointer startPosition = new DocumentSequenceTextPointer(previousBlock, previousBlock.End);
			this._end = new DocumentSequenceTextPointer(childDocumentBlock, childDocumentBlock.ChildContainer.End);
			if (childDocumentBlock.NextBlock == this._doclistTail && childDocumentBlock.PreviousBlock == this._doclistHead)
			{
				this._start = new DocumentSequenceTextPointer(childDocumentBlock, childDocumentBlock.ChildContainer.Start);
			}
			ITextContainer childContainer = childDocumentBlock.ChildContainer;
			int symbolCount = 1;
			this.AddChange(startPosition, symbolCount, PrecursorTextChangeType.ContentAdded);
		}

		// Token: 0x06002BD3 RID: 11219 RVA: 0x000C7B28 File Offset: 0x000C5D28
		private void _OnHighlightChanged(object sender, HighlightChangedEventArgs args)
		{
			int i = 0;
			DocumentSequenceTextPointer documentSequenceTextPointer = null;
			ChildDocumentBlock childDocumentBlock = null;
			List<TextSegment> list = new List<TextSegment>(4);
			while (i < args.Ranges.Count)
			{
				TextSegment textSegment = (TextSegment)args.Ranges[i];
				DocumentSequenceTextPointer documentSequenceTextPointer2 = (DocumentSequenceTextPointer)textSegment.End;
				if (documentSequenceTextPointer == null)
				{
					documentSequenceTextPointer = (DocumentSequenceTextPointer)textSegment.Start;
				}
				ChildDocumentBlock childDocumentBlock2 = childDocumentBlock;
				childDocumentBlock = documentSequenceTextPointer.ChildBlock;
				if (childDocumentBlock2 != null && childDocumentBlock != childDocumentBlock2 && !(childDocumentBlock2.ChildContainer is NullTextContainer) && list.Count != 0)
				{
					childDocumentBlock2.ChildHighlightLayer.RaiseHighlightChangedEvent(new ReadOnlyCollection<TextSegment>(list));
					list.Clear();
				}
				ITextPointer childPointer = documentSequenceTextPointer.ChildPointer;
				if (documentSequenceTextPointer2.ChildBlock != childDocumentBlock)
				{
					ITextPointer textPointer = documentSequenceTextPointer.ChildPointer.TextContainer.End;
					if (childPointer.CompareTo(textPointer) != 0)
					{
						list.Add(new TextSegment(childPointer, textPointer));
					}
					if (!(childDocumentBlock.ChildContainer is NullTextContainer) && list.Count != 0)
					{
						childDocumentBlock.ChildHighlightLayer.RaiseHighlightChangedEvent(new ReadOnlyCollection<TextSegment>(list));
					}
					childDocumentBlock = childDocumentBlock.NextBlock;
					documentSequenceTextPointer = new DocumentSequenceTextPointer(childDocumentBlock, childDocumentBlock.ChildContainer.Start);
					list.Clear();
				}
				else
				{
					ITextPointer textPointer = documentSequenceTextPointer2.ChildPointer;
					if (childPointer.CompareTo(textPointer) != 0)
					{
						list.Add(new TextSegment(childPointer, textPointer));
					}
					i++;
					documentSequenceTextPointer = null;
				}
			}
			if (list.Count > 0 && childDocumentBlock != null && !(childDocumentBlock.ChildContainer is NullTextContainer))
			{
				childDocumentBlock.ChildHighlightLayer.RaiseHighlightChangedEvent(new ReadOnlyCollection<TextSegment>(list));
			}
		}

		// Token: 0x04001CB9 RID: 7353
		private readonly FixedDocumentSequence _parent;

		// Token: 0x04001CBA RID: 7354
		private DocumentSequenceTextPointer _start;

		// Token: 0x04001CBB RID: 7355
		private DocumentSequenceTextPointer _end;

		// Token: 0x04001CBC RID: 7356
		private ChildDocumentBlock _doclistHead;

		// Token: 0x04001CBD RID: 7357
		private ChildDocumentBlock _doclistTail;

		// Token: 0x04001CBE RID: 7358
		private ITextSelection _textSelection;

		// Token: 0x04001CBF RID: 7359
		private Highlights _highlights;

		// Token: 0x04001CC0 RID: 7360
		private int _changeBlockLevel;

		// Token: 0x04001CC1 RID: 7361
		private TextContainerChangedEventArgs _changes;

		// Token: 0x04001CC2 RID: 7362
		private ITextView _textview;

		// Token: 0x04001CC3 RID: 7363
		private bool _isReadOnly;

		// Token: 0x020008CC RID: 2252
		private sealed class DocumentSequenceHighlights : Highlights
		{
			// Token: 0x06008476 RID: 33910 RVA: 0x00248278 File Offset: 0x00246478
			internal DocumentSequenceHighlights(DocumentSequenceTextContainer textContainer) : base(textContainer)
			{
			}

			// Token: 0x06008477 RID: 33911 RVA: 0x00248284 File Offset: 0x00246484
			internal override object GetHighlightValue(StaticTextPointer textPosition, LogicalDirection direction, Type highlightLayerOwnerType)
			{
				StaticTextPointer textPosition2;
				if (this.EnsureParentPosition(textPosition, direction, out textPosition2))
				{
					return base.GetHighlightValue(textPosition2, direction, highlightLayerOwnerType);
				}
				return DependencyProperty.UnsetValue;
			}

			// Token: 0x06008478 RID: 33912 RVA: 0x002482AC File Offset: 0x002464AC
			internal override bool IsContentHighlighted(StaticTextPointer textPosition, LogicalDirection direction)
			{
				StaticTextPointer textPosition2;
				return this.EnsureParentPosition(textPosition, direction, out textPosition2) && base.IsContentHighlighted(textPosition2, direction);
			}

			// Token: 0x06008479 RID: 33913 RVA: 0x002482D0 File Offset: 0x002464D0
			internal override StaticTextPointer GetNextHighlightChangePosition(StaticTextPointer textPosition, LogicalDirection direction)
			{
				StaticTextPointer staticTextPointer = StaticTextPointer.Null;
				StaticTextPointer textPosition2;
				if (this.EnsureParentPosition(textPosition, direction, out textPosition2))
				{
					staticTextPointer = base.GetNextHighlightChangePosition(textPosition2, direction);
					if (textPosition.TextContainer.Highlights != this)
					{
						staticTextPointer = this.GetStaticPositionInChildContainer(staticTextPointer, direction, textPosition);
					}
				}
				return staticTextPointer;
			}

			// Token: 0x0600847A RID: 33914 RVA: 0x00248314 File Offset: 0x00246514
			internal override StaticTextPointer GetNextPropertyChangePosition(StaticTextPointer textPosition, LogicalDirection direction)
			{
				StaticTextPointer staticTextPointer = StaticTextPointer.Null;
				StaticTextPointer textPosition2;
				if (this.EnsureParentPosition(textPosition, direction, out textPosition2))
				{
					staticTextPointer = base.GetNextPropertyChangePosition(textPosition2, direction);
					if (textPosition.TextContainer.Highlights != this)
					{
						staticTextPointer = this.GetStaticPositionInChildContainer(staticTextPointer, direction, textPosition);
					}
				}
				return staticTextPointer;
			}

			// Token: 0x0600847B RID: 33915 RVA: 0x00248358 File Offset: 0x00246558
			private bool EnsureParentPosition(StaticTextPointer textPosition, LogicalDirection direction, out StaticTextPointer parentPosition)
			{
				parentPosition = textPosition;
				if (textPosition.TextContainer.Highlights != this)
				{
					if (textPosition.GetPointerContext(direction) == TextPointerContext.None)
					{
						return false;
					}
					ITextPointer tp = textPosition.CreateDynamicTextPointer(LogicalDirection.Forward);
					ITextPointer textPointer = ((DocumentSequenceTextContainer)base.TextContainer).MapChildPositionToParent(tp);
					parentPosition = textPointer.CreateStaticPointer();
				}
				return true;
			}

			// Token: 0x0600847C RID: 33916 RVA: 0x002483B0 File Offset: 0x002465B0
			private StaticTextPointer GetStaticPositionInChildContainer(StaticTextPointer textPosition, LogicalDirection direction, StaticTextPointer originalPosition)
			{
				StaticTextPointer result = StaticTextPointer.Null;
				if (!textPosition.IsNull)
				{
					DocumentSequenceTextPointer documentSequenceTextPointer = textPosition.CreateDynamicTextPointer(LogicalDirection.Forward) as DocumentSequenceTextPointer;
					ITextPointer textPointer = documentSequenceTextPointer.ChildPointer;
					if (textPointer.TextContainer != originalPosition.TextContainer)
					{
						if (this.IsContentHighlighted(originalPosition, direction))
						{
							textPointer = ((direction == LogicalDirection.Forward) ? originalPosition.TextContainer.End : originalPosition.TextContainer.Start);
							result = textPointer.CreateStaticPointer();
						}
						else
						{
							result = StaticTextPointer.Null;
						}
					}
					else
					{
						result = textPointer.CreateStaticPointer();
					}
				}
				return result;
			}
		}
	}
}
