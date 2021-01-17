using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x0200033E RID: 830
	internal sealed class DocumentSequenceTextView : TextViewBase
	{
		// Token: 0x06002C20 RID: 11296 RVA: 0x000C86DE File Offset: 0x000C68DE
		internal DocumentSequenceTextView(FixedDocumentSequenceDocumentPage docPage)
		{
			this._docPage = docPage;
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x000C86F0 File Offset: 0x000C68F0
		internal override ITextPointer GetTextPositionFromPoint(Point point, bool snapToText)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = null;
			LogicalDirection gravity = LogicalDirection.Forward;
			if (this.ChildTextView != null)
			{
				ITextPointer textPositionFromPoint = this.ChildTextView.GetTextPositionFromPoint(point, snapToText);
				if (textPositionFromPoint != null)
				{
					documentSequenceTextPointer = new DocumentSequenceTextPointer(this.ChildBlock, textPositionFromPoint);
					gravity = textPositionFromPoint.LogicalDirection;
				}
			}
			if (documentSequenceTextPointer != null)
			{
				return DocumentSequenceTextPointer.CreatePointer(documentSequenceTextPointer, gravity);
			}
			return null;
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x000C873C File Offset: 0x000C693C
		internal override Rect GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = null;
			transform = Transform.Identity;
			if (position != null)
			{
				documentSequenceTextPointer = this._docPage.FixedDocumentSequence.TextContainer.VerifyPosition(position);
			}
			if (documentSequenceTextPointer != null && this.ChildTextView != null && this.ChildTextView.TextContainer == documentSequenceTextPointer.ChildBlock.ChildContainer)
			{
				return this.ChildTextView.GetRawRectangleFromTextPosition(documentSequenceTextPointer.ChildPointer.CreatePointer(position.LogicalDirection), out transform);
			}
			return Rect.Empty;
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x000C87B4 File Offset: 0x000C69B4
		internal override Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			if (startPosition != null && endPosition != null && this.ChildTextView != null)
			{
				DocumentSequenceTextPointer documentSequenceTextPointer = this._docPage.FixedDocumentSequence.TextContainer.VerifyPosition(startPosition);
				DocumentSequenceTextPointer documentSequenceTextPointer2 = this._docPage.FixedDocumentSequence.TextContainer.VerifyPosition(endPosition);
				if (documentSequenceTextPointer != null && documentSequenceTextPointer2 != null)
				{
					return this.ChildTextView.GetTightBoundingGeometryFromTextPositions(documentSequenceTextPointer.ChildPointer, documentSequenceTextPointer2.ChildPointer);
				}
			}
			return new PathGeometry();
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x000C8824 File Offset: 0x000C6A24
		internal override ITextPointer GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved)
		{
			newSuggestedX = suggestedX;
			linesMoved = count;
			DocumentSequenceTextPointer thisTp = null;
			LogicalDirection gravity = LogicalDirection.Forward;
			DocumentSequenceTextPointer documentSequenceTextPointer = null;
			if (position != null)
			{
				documentSequenceTextPointer = this._docPage.FixedDocumentSequence.TextContainer.VerifyPosition(position);
			}
			if (documentSequenceTextPointer != null && this.ChildTextView != null && this.ChildTextView.TextContainer == documentSequenceTextPointer.ChildBlock.ChildContainer)
			{
				ITextPointer positionAtNextLine = this.ChildTextView.GetPositionAtNextLine(documentSequenceTextPointer.ChildPointer.CreatePointer(position.LogicalDirection), suggestedX, count, out newSuggestedX, out linesMoved);
				if (positionAtNextLine != null)
				{
					thisTp = new DocumentSequenceTextPointer(this.ChildBlock, positionAtNextLine);
					gravity = positionAtNextLine.LogicalDirection;
				}
			}
			return DocumentSequenceTextPointer.CreatePointer(thisTp, gravity);
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x000C88C0 File Offset: 0x000C6AC0
		internal override bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			Invariant.Assert(position != null);
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			Invariant.Assert(this.ChildTextView != null);
			DocumentSequenceTextPointer documentSequenceTextPointer = this.DocumentSequenceTextContainer.VerifyPosition(position);
			return this.ChildTextView.IsAtCaretUnitBoundary(documentSequenceTextPointer.ChildPointer);
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x000C8910 File Offset: 0x000C6B10
		internal override ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			Invariant.Assert(position != null);
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			Invariant.Assert(this.ChildTextView != null);
			DocumentSequenceTextPointer documentSequenceTextPointer = this.DocumentSequenceTextContainer.VerifyPosition(position);
			return this.ChildTextView.GetNextCaretUnitPosition(documentSequenceTextPointer.ChildPointer, direction);
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x000C8964 File Offset: 0x000C6B64
		internal override ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			Invariant.Assert(position != null);
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			Invariant.Assert(this.ChildTextView != null);
			DocumentSequenceTextPointer documentSequenceTextPointer = this.DocumentSequenceTextContainer.VerifyPosition(position);
			return this.ChildTextView.GetBackspaceCaretUnitPosition(documentSequenceTextPointer.ChildPointer);
		}

		// Token: 0x06002C28 RID: 11304 RVA: 0x000C89B4 File Offset: 0x000C6BB4
		internal override TextSegment GetLineRange(ITextPointer position)
		{
			if (position != null && this.ChildTextView != null)
			{
				DocumentSequenceTextPointer documentSequenceTextPointer = this._docPage.FixedDocumentSequence.TextContainer.VerifyPosition(position);
				if (this.ChildTextView.TextContainer == documentSequenceTextPointer.ChildBlock.ChildContainer)
				{
					TextSegment lineRange = this.ChildTextView.GetLineRange(documentSequenceTextPointer.ChildPointer.CreatePointer(position.LogicalDirection));
					if (!lineRange.IsNull)
					{
						DocumentSequenceTextPointer startPosition = new DocumentSequenceTextPointer(this.ChildBlock, lineRange.Start);
						DocumentSequenceTextPointer endPosition = new DocumentSequenceTextPointer(this.ChildBlock, lineRange.End);
						return new TextSegment(startPosition, endPosition, true);
					}
				}
			}
			return TextSegment.Null;
		}

		// Token: 0x06002C29 RID: 11305 RVA: 0x0003E264 File Offset: 0x0003C464
		internal override ReadOnlyCollection<GlyphRun> GetGlyphRuns(ITextPointer start, ITextPointer end)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x000C8A64 File Offset: 0x000C6C64
		internal override bool Contains(ITextPointer position)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = null;
			if (position != null)
			{
				documentSequenceTextPointer = this._docPage.FixedDocumentSequence.TextContainer.VerifyPosition(position);
			}
			return documentSequenceTextPointer != null && this.ChildTextView != null && this.ChildTextView.TextContainer == documentSequenceTextPointer.ChildBlock.ChildContainer && this.ChildTextView.Contains(documentSequenceTextPointer.ChildPointer.CreatePointer(position.LogicalDirection));
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x000C8ACE File Offset: 0x000C6CCE
		internal override bool Validate()
		{
			if (this.ChildTextView != null)
			{
				this.ChildTextView.Validate();
			}
			return ((ITextView)this).IsValid;
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x000C8AEA File Offset: 0x000C6CEA
		internal override bool Validate(Point point)
		{
			if (this.ChildTextView != null)
			{
				this.ChildTextView.Validate(point);
			}
			return ((ITextView)this).IsValid;
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06002C2D RID: 11309 RVA: 0x000C8B08 File Offset: 0x000C6D08
		internal override UIElement RenderScope
		{
			get
			{
				Visual visual = this._docPage.Visual;
				while (visual != null && !(visual is UIElement))
				{
					visual = (VisualTreeHelper.GetParent(visual) as Visual);
				}
				return visual as UIElement;
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06002C2E RID: 11310 RVA: 0x000C8B40 File Offset: 0x000C6D40
		internal override ITextContainer TextContainer
		{
			get
			{
				return this._docPage.FixedDocumentSequence.TextContainer;
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06002C2F RID: 11311 RVA: 0x000C8B52 File Offset: 0x000C6D52
		internal override bool IsValid
		{
			get
			{
				return this.ChildTextView == null || this.ChildTextView.IsValid;
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06002C30 RID: 11312 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool RendersOwnSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06002C31 RID: 11313 RVA: 0x000C8B6C File Offset: 0x000C6D6C
		internal override ReadOnlyCollection<TextSegment> TextSegments
		{
			get
			{
				if (this._textSegments == null)
				{
					ReadOnlyCollection<TextSegment> textSegments = this.ChildTextView.TextSegments;
					if (textSegments != null)
					{
						List<TextSegment> list = new List<TextSegment>(textSegments.Count);
						foreach (TextSegment textSegment in textSegments)
						{
							DocumentSequenceTextPointer startPosition = this._docPage.FixedDocumentSequence.TextContainer.MapChildPositionToParent(textSegment.Start);
							DocumentSequenceTextPointer endPosition = this._docPage.FixedDocumentSequence.TextContainer.MapChildPositionToParent(textSegment.End);
							list.Add(new TextSegment(startPosition, endPosition, true));
						}
						this._textSegments = new ReadOnlyCollection<TextSegment>(list);
					}
				}
				return this._textSegments;
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06002C32 RID: 11314 RVA: 0x000C8C34 File Offset: 0x000C6E34
		private ITextView ChildTextView
		{
			get
			{
				if (this._childTextView == null)
				{
					IServiceProvider serviceProvider = this._docPage.ChildDocumentPage as IServiceProvider;
					if (serviceProvider != null)
					{
						this._childTextView = (ITextView)serviceProvider.GetService(typeof(ITextView));
					}
				}
				return this._childTextView;
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06002C33 RID: 11315 RVA: 0x000C8C7E File Offset: 0x000C6E7E
		private ChildDocumentBlock ChildBlock
		{
			get
			{
				if (this._childBlock == null)
				{
					this._childBlock = this._docPage.FixedDocumentSequence.TextContainer.FindChildBlock(this._docPage.ChildDocumentReference);
				}
				return this._childBlock;
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06002C34 RID: 11316 RVA: 0x000C8B40 File Offset: 0x000C6D40
		private DocumentSequenceTextContainer DocumentSequenceTextContainer
		{
			get
			{
				return this._docPage.FixedDocumentSequence.TextContainer;
			}
		}

		// Token: 0x04001CC7 RID: 7367
		private readonly FixedDocumentSequenceDocumentPage _docPage;

		// Token: 0x04001CC8 RID: 7368
		private ITextView _childTextView;

		// Token: 0x04001CC9 RID: 7369
		private ReadOnlyCollection<TextSegment> _textSegments;

		// Token: 0x04001CCA RID: 7370
		private ChildDocumentBlock _childBlock;
	}
}
