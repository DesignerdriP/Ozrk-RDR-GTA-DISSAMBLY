using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200033D RID: 829
	internal sealed class DocumentSequenceTextPointer : ContentPosition, ITextPointer
	{
		// Token: 0x06002BD4 RID: 11220 RVA: 0x000C7CA2 File Offset: 0x000C5EA2
		internal DocumentSequenceTextPointer(ChildDocumentBlock childBlock, ITextPointer childPosition)
		{
			this._childBlock = childBlock;
			this._childTp = childPosition;
		}

		// Token: 0x06002BD5 RID: 11221 RVA: 0x000C7CB8 File Offset: 0x000C5EB8
		void ITextPointer.SetLogicalDirection(LogicalDirection direction)
		{
			this._childTp.SetLogicalDirection(direction);
		}

		// Token: 0x06002BD6 RID: 11222 RVA: 0x000C7CC6 File Offset: 0x000C5EC6
		int ITextPointer.CompareTo(ITextPointer position)
		{
			return DocumentSequenceTextPointer.CompareTo(this, position);
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x000C7CCF File Offset: 0x000C5ECF
		int ITextPointer.CompareTo(StaticTextPointer position)
		{
			return ((ITextPointer)this).CompareTo((ITextPointer)position.Handle0);
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x000C7CE3 File Offset: 0x000C5EE3
		int ITextPointer.GetOffsetToPosition(ITextPointer position)
		{
			return DocumentSequenceTextPointer.GetOffsetToPosition(this, position);
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x000C7CEC File Offset: 0x000C5EEC
		TextPointerContext ITextPointer.GetPointerContext(LogicalDirection direction)
		{
			return DocumentSequenceTextPointer.GetPointerContext(this, direction);
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x000C7CF5 File Offset: 0x000C5EF5
		int ITextPointer.GetTextRunLength(LogicalDirection direction)
		{
			return DocumentSequenceTextPointer.GetTextRunLength(this, direction);
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x000C7CFE File Offset: 0x000C5EFE
		string ITextPointer.GetTextInRun(LogicalDirection direction)
		{
			return TextPointerBase.GetTextInRun(this, direction);
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x000C7D07 File Offset: 0x000C5F07
		int ITextPointer.GetTextInRun(LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			return DocumentSequenceTextPointer.GetTextInRun(this, direction, textBuffer, startIndex, count);
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x000C7D14 File Offset: 0x000C5F14
		object ITextPointer.GetAdjacentElement(LogicalDirection direction)
		{
			return DocumentSequenceTextPointer.GetAdjacentElement(this, direction);
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x000C7D1D File Offset: 0x000C5F1D
		Type ITextPointer.GetElementType(LogicalDirection direction)
		{
			return DocumentSequenceTextPointer.GetElementType(this, direction);
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x000C7D26 File Offset: 0x000C5F26
		bool ITextPointer.HasEqualScope(ITextPointer position)
		{
			return DocumentSequenceTextPointer.HasEqualScope(this, position);
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x000C7D2F File Offset: 0x000C5F2F
		object ITextPointer.GetValue(DependencyProperty property)
		{
			return DocumentSequenceTextPointer.GetValue(this, property);
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x000C7D38 File Offset: 0x000C5F38
		object ITextPointer.ReadLocalValue(DependencyProperty property)
		{
			return DocumentSequenceTextPointer.ReadLocalValue(this, property);
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x000C7D41 File Offset: 0x000C5F41
		LocalValueEnumerator ITextPointer.GetLocalValueEnumerator()
		{
			return DocumentSequenceTextPointer.GetLocalValueEnumerator(this);
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x000C7D49 File Offset: 0x000C5F49
		ITextPointer ITextPointer.CreatePointer()
		{
			return DocumentSequenceTextPointer.CreatePointer(this);
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x000C7D51 File Offset: 0x000C5F51
		StaticTextPointer ITextPointer.CreateStaticPointer()
		{
			return new StaticTextPointer(((ITextPointer)this).TextContainer, ((ITextPointer)this).CreatePointer());
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x000C7D64 File Offset: 0x000C5F64
		ITextPointer ITextPointer.CreatePointer(int distance)
		{
			return DocumentSequenceTextPointer.CreatePointer(this, distance);
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x000C7D6D File Offset: 0x000C5F6D
		ITextPointer ITextPointer.CreatePointer(LogicalDirection gravity)
		{
			return DocumentSequenceTextPointer.CreatePointer(this, gravity);
		}

		// Token: 0x06002BE7 RID: 11239 RVA: 0x000C7D76 File Offset: 0x000C5F76
		ITextPointer ITextPointer.CreatePointer(int distance, LogicalDirection gravity)
		{
			return DocumentSequenceTextPointer.CreatePointer(this, distance, gravity);
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x000C7D80 File Offset: 0x000C5F80
		void ITextPointer.Freeze()
		{
			this._isFrozen = true;
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x000C7D89 File Offset: 0x000C5F89
		ITextPointer ITextPointer.GetFrozenPointer(LogicalDirection logicalDirection)
		{
			return TextPointerBase.GetFrozenPointer(this, logicalDirection);
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x000C7D92 File Offset: 0x000C5F92
		void ITextPointer.InsertTextInRun(string textData)
		{
			throw new InvalidOperationException(SR.Get("DocumentReadOnly"));
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x000C7D92 File Offset: 0x000C5F92
		void ITextPointer.DeleteContentToPosition(ITextPointer limit)
		{
			throw new InvalidOperationException(SR.Get("DocumentReadOnly"));
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x000C7DA4 File Offset: 0x000C5FA4
		ITextPointer ITextPointer.GetNextContextPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			if (textPointer.MoveToNextContextPosition(direction))
			{
				textPointer.Freeze();
			}
			else
			{
				textPointer = null;
			}
			return textPointer;
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x000C7DCC File Offset: 0x000C5FCC
		ITextPointer ITextPointer.GetInsertionPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			textPointer.MoveToInsertionPosition(direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x000C7DF0 File Offset: 0x000C5FF0
		ITextPointer ITextPointer.GetFormatNormalizedPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			TextPointerBase.MoveToFormatNormalizedPosition(textPointer, direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x000C7E14 File Offset: 0x000C6014
		ITextPointer ITextPointer.GetNextInsertionPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			if (textPointer.MoveToNextInsertionPosition(direction))
			{
				textPointer.Freeze();
			}
			else
			{
				textPointer = null;
			}
			return textPointer;
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x000C7E3C File Offset: 0x000C603C
		bool ITextPointer.ValidateLayout()
		{
			return TextPointerBase.ValidateLayout(this, ((ITextPointer)this).TextContainer.TextView);
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06002BF1 RID: 11249 RVA: 0x000C7E4F File Offset: 0x000C604F
		Type ITextPointer.ParentType
		{
			get
			{
				return DocumentSequenceTextPointer.GetElementType(this);
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06002BF2 RID: 11250 RVA: 0x000C7E57 File Offset: 0x000C6057
		ITextContainer ITextPointer.TextContainer
		{
			get
			{
				return this.AggregatedContainer;
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06002BF3 RID: 11251 RVA: 0x000C7E5F File Offset: 0x000C605F
		bool ITextPointer.HasValidLayout
		{
			get
			{
				return ((ITextPointer)this).TextContainer.TextView != null && ((ITextPointer)this).TextContainer.TextView.IsValid && ((ITextPointer)this).TextContainer.TextView.Contains(this);
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06002BF4 RID: 11252 RVA: 0x000C7E94 File Offset: 0x000C6094
		bool ITextPointer.IsAtCaretUnitBoundary
		{
			get
			{
				Invariant.Assert(((ITextPointer)this).HasValidLayout);
				ITextView textView = ((ITextPointer)this).TextContainer.TextView;
				bool flag = textView.IsAtCaretUnitBoundary(this);
				if (!flag && ((ITextPointer)this).LogicalDirection == LogicalDirection.Backward)
				{
					ITextPointer position = ((ITextPointer)this).CreatePointer(LogicalDirection.Forward);
					flag = textView.IsAtCaretUnitBoundary(position);
				}
				return flag;
			}
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06002BF5 RID: 11253 RVA: 0x000C7EDC File Offset: 0x000C60DC
		LogicalDirection ITextPointer.LogicalDirection
		{
			get
			{
				return this._childTp.LogicalDirection;
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06002BF6 RID: 11254 RVA: 0x000C7EE9 File Offset: 0x000C60E9
		bool ITextPointer.IsAtInsertionPosition
		{
			get
			{
				return TextPointerBase.IsAtInsertionPosition(this);
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06002BF7 RID: 11255 RVA: 0x000C7EF1 File Offset: 0x000C60F1
		bool ITextPointer.IsFrozen
		{
			get
			{
				return this._isFrozen;
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06002BF8 RID: 11256 RVA: 0x000C7EF9 File Offset: 0x000C60F9
		int ITextPointer.Offset
		{
			get
			{
				return TextPointerBase.GetOffset(this);
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06002BF9 RID: 11257 RVA: 0x0003E264 File Offset: 0x0003C464
		int ITextPointer.CharOffset
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x000C7F01 File Offset: 0x000C6101
		bool ITextPointer.MoveToNextContextPosition(LogicalDirection direction)
		{
			return DocumentSequenceTextPointer.iScan(this, direction);
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x000C7F0A File Offset: 0x000C610A
		int ITextPointer.MoveByOffset(int offset)
		{
			if (this._isFrozen)
			{
				throw new InvalidOperationException(SR.Get("TextPositionIsFrozen"));
			}
			if (DocumentSequenceTextPointer.iScan(this, offset))
			{
				return offset;
			}
			return 0;
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x000C7F30 File Offset: 0x000C6130
		void ITextPointer.MoveToPosition(ITextPointer position)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = this.AggregatedContainer.VerifyPosition(position);
			LogicalDirection logicalDirection = this.ChildPointer.LogicalDirection;
			this.ChildBlock = documentSequenceTextPointer.ChildBlock;
			if (this.ChildPointer.TextContainer == documentSequenceTextPointer.ChildPointer.TextContainer)
			{
				this.ChildPointer.MoveToPosition(documentSequenceTextPointer.ChildPointer);
				return;
			}
			this.ChildPointer = documentSequenceTextPointer.ChildPointer.CreatePointer();
			this.ChildPointer.SetLogicalDirection(logicalDirection);
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x000C7FA9 File Offset: 0x000C61A9
		void ITextPointer.MoveToElementEdge(ElementEdge edge)
		{
			this.ChildPointer.MoveToElementEdge(edge);
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x000C7FB7 File Offset: 0x000C61B7
		int ITextPointer.MoveToLineBoundary(int count)
		{
			return TextPointerBase.MoveToLineBoundary(this, ((ITextPointer)this).TextContainer.TextView, count, true);
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x000C7FCC File Offset: 0x000C61CC
		Rect ITextPointer.GetCharacterRect(LogicalDirection direction)
		{
			return TextPointerBase.GetCharacterRect(this, direction);
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x000C7FD5 File Offset: 0x000C61D5
		bool ITextPointer.MoveToInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToInsertionPosition(this, direction);
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x000C7FDE File Offset: 0x000C61DE
		bool ITextPointer.MoveToNextInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToNextInsertionPosition(this, direction);
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06002C02 RID: 11266 RVA: 0x000C7FE7 File Offset: 0x000C61E7
		internal DocumentSequenceTextContainer AggregatedContainer
		{
			get
			{
				return this._childBlock.AggregatedContainer;
			}
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06002C03 RID: 11267 RVA: 0x000C7FF4 File Offset: 0x000C61F4
		// (set) Token: 0x06002C04 RID: 11268 RVA: 0x000C7FFC File Offset: 0x000C61FC
		internal ChildDocumentBlock ChildBlock
		{
			get
			{
				return this._childBlock;
			}
			set
			{
				this._childBlock = value;
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06002C05 RID: 11269 RVA: 0x000C8005 File Offset: 0x000C6205
		// (set) Token: 0x06002C06 RID: 11270 RVA: 0x000C800D File Offset: 0x000C620D
		internal ITextPointer ChildPointer
		{
			get
			{
				return this._childTp;
			}
			set
			{
				this._childTp = value;
			}
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x000C8018 File Offset: 0x000C6218
		public static int CompareTo(DocumentSequenceTextPointer thisTp, ITextPointer position)
		{
			DocumentSequenceTextPointer tp = thisTp.AggregatedContainer.VerifyPosition(position);
			return DocumentSequenceTextPointer.xGapAwareCompareTo(thisTp, tp);
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x000C803C File Offset: 0x000C623C
		public static int GetOffsetToPosition(DocumentSequenceTextPointer thisTp, ITextPointer position)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = thisTp.AggregatedContainer.VerifyPosition(position);
			int num = DocumentSequenceTextPointer.xGapAwareCompareTo(thisTp, documentSequenceTextPointer);
			if (num == 0)
			{
				return 0;
			}
			if (num <= 0)
			{
				return DocumentSequenceTextPointer.xGapAwareGetDistance(thisTp, documentSequenceTextPointer);
			}
			return -1 * DocumentSequenceTextPointer.xGapAwareGetDistance(documentSequenceTextPointer, thisTp);
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x000C8078 File Offset: 0x000C6278
		public static TextPointerContext GetPointerContext(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			return DocumentSequenceTextPointer.xGapAwareGetSymbolType(thisTp, direction);
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x000C808C File Offset: 0x000C628C
		public static int GetTextRunLength(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			return thisTp.ChildPointer.GetTextRunLength(direction);
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x000C80A8 File Offset: 0x000C62A8
		public static int GetTextInRun(DocumentSequenceTextPointer thisTp, LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			if (textBuffer == null)
			{
				throw new ArgumentNullException("textBuffer");
			}
			if (startIndex < 0)
			{
				throw new ArgumentException(SR.Get("NegativeValue", new object[]
				{
					"startIndex"
				}));
			}
			if (startIndex > textBuffer.Length)
			{
				throw new ArgumentException(SR.Get("StartIndexExceedsBufferSize", new object[]
				{
					startIndex,
					textBuffer.Length
				}));
			}
			if (count < 0)
			{
				throw new ArgumentException(SR.Get("NegativeValue", new object[]
				{
					"count"
				}));
			}
			if (count > textBuffer.Length - startIndex)
			{
				throw new ArgumentException(SR.Get("MaxLengthExceedsBufferSize", new object[]
				{
					count,
					textBuffer.Length,
					startIndex
				}));
			}
			return thisTp.ChildPointer.GetTextInRun(direction, textBuffer, startIndex, count);
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x000C8190 File Offset: 0x000C6390
		public static object GetAdjacentElement(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			return DocumentSequenceTextPointer.xGapAwareGetEmbeddedElement(thisTp, direction);
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x000C81A4 File Offset: 0x000C63A4
		public static Type GetElementType(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			DocumentSequenceTextPointer documentSequenceTextPointer = DocumentSequenceTextPointer.xGetClingDSTP(thisTp, direction);
			return documentSequenceTextPointer.ChildPointer.GetElementType(direction);
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x000C81D0 File Offset: 0x000C63D0
		public static Type GetElementType(DocumentSequenceTextPointer thisTp)
		{
			return thisTp.ChildPointer.ParentType;
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x000C81E0 File Offset: 0x000C63E0
		public static bool HasEqualScope(DocumentSequenceTextPointer thisTp, ITextPointer position)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = thisTp.AggregatedContainer.VerifyPosition(position);
			if (thisTp.ChildPointer.TextContainer == documentSequenceTextPointer.ChildPointer.TextContainer)
			{
				return thisTp.ChildPointer.HasEqualScope(documentSequenceTextPointer.ChildPointer);
			}
			return thisTp.ChildPointer.ParentType == typeof(FixedDocument) && documentSequenceTextPointer.ChildPointer.ParentType == typeof(FixedDocument);
		}

		// Token: 0x06002C10 RID: 11280 RVA: 0x000C825C File Offset: 0x000C645C
		public static object GetValue(DocumentSequenceTextPointer thisTp, DependencyProperty property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			return thisTp.ChildPointer.GetValue(property);
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x000C8278 File Offset: 0x000C6478
		public static object ReadLocalValue(DocumentSequenceTextPointer thisTp, DependencyProperty property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			return thisTp.ChildPointer.ReadLocalValue(property);
		}

		// Token: 0x06002C12 RID: 11282 RVA: 0x000C8294 File Offset: 0x000C6494
		public static LocalValueEnumerator GetLocalValueEnumerator(DocumentSequenceTextPointer thisTp)
		{
			return thisTp.ChildPointer.GetLocalValueEnumerator();
		}

		// Token: 0x06002C13 RID: 11283 RVA: 0x000C82A1 File Offset: 0x000C64A1
		public static ITextPointer CreatePointer(DocumentSequenceTextPointer thisTp)
		{
			return DocumentSequenceTextPointer.CreatePointer(thisTp, 0, thisTp.ChildPointer.LogicalDirection);
		}

		// Token: 0x06002C14 RID: 11284 RVA: 0x000C82B5 File Offset: 0x000C64B5
		public static ITextPointer CreatePointer(DocumentSequenceTextPointer thisTp, int distance)
		{
			return DocumentSequenceTextPointer.CreatePointer(thisTp, distance, thisTp.ChildPointer.LogicalDirection);
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x000C82C9 File Offset: 0x000C64C9
		public static ITextPointer CreatePointer(DocumentSequenceTextPointer thisTp, LogicalDirection gravity)
		{
			return DocumentSequenceTextPointer.CreatePointer(thisTp, 0, gravity);
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x000C82D4 File Offset: 0x000C64D4
		public static ITextPointer CreatePointer(DocumentSequenceTextPointer thisTp, int distance, LogicalDirection gravity)
		{
			ValidationHelper.VerifyDirection(gravity, "gravity");
			DocumentSequenceTextPointer documentSequenceTextPointer = new DocumentSequenceTextPointer(thisTp.ChildBlock, thisTp.ChildPointer.CreatePointer(gravity));
			if (distance != 0 && !DocumentSequenceTextPointer.xGapAwareScan(documentSequenceTextPointer, distance))
			{
				throw new ArgumentException(SR.Get("BadDistance"), "distance");
			}
			return documentSequenceTextPointer;
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x000C8328 File Offset: 0x000C6528
		internal static bool iScan(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			bool flag = thisTp.ChildPointer.MoveToNextContextPosition(direction);
			if (!flag)
			{
				flag = DocumentSequenceTextPointer.xGapAwareScan(thisTp, (direction == LogicalDirection.Forward) ? 1 : -1);
			}
			return flag;
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x000C8355 File Offset: 0x000C6555
		internal static bool iScan(DocumentSequenceTextPointer thisTp, int distance)
		{
			return DocumentSequenceTextPointer.xGapAwareScan(thisTp, distance);
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x000C8360 File Offset: 0x000C6560
		private static DocumentSequenceTextPointer xGetClingDSTP(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			TextPointerContext pointerContext = thisTp.ChildPointer.GetPointerContext(direction);
			if (pointerContext != TextPointerContext.None)
			{
				return thisTp;
			}
			ChildDocumentBlock childDocumentBlock = thisTp.ChildBlock;
			ITextPointer textPointer = thisTp.ChildPointer;
			if (direction == LogicalDirection.Forward)
			{
				while (pointerContext == TextPointerContext.None)
				{
					if (childDocumentBlock.IsTail)
					{
						break;
					}
					childDocumentBlock = childDocumentBlock.NextBlock;
					textPointer = childDocumentBlock.ChildContainer.Start;
					pointerContext = textPointer.GetPointerContext(direction);
				}
			}
			else
			{
				while (pointerContext == TextPointerContext.None && !childDocumentBlock.IsHead)
				{
					childDocumentBlock = childDocumentBlock.PreviousBlock;
					textPointer = childDocumentBlock.ChildContainer.End;
					pointerContext = textPointer.GetPointerContext(direction);
				}
			}
			return new DocumentSequenceTextPointer(childDocumentBlock, textPointer);
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x000C83E8 File Offset: 0x000C65E8
		private static TextPointerContext xGapAwareGetSymbolType(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = DocumentSequenceTextPointer.xGetClingDSTP(thisTp, direction);
			return documentSequenceTextPointer.ChildPointer.GetPointerContext(direction);
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x000C840C File Offset: 0x000C660C
		private static object xGapAwareGetEmbeddedElement(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = DocumentSequenceTextPointer.xGetClingDSTP(thisTp, direction);
			return documentSequenceTextPointer.ChildPointer.GetAdjacentElement(direction);
		}

		// Token: 0x06002C1C RID: 11292 RVA: 0x000C8430 File Offset: 0x000C6630
		private static int xGapAwareCompareTo(DocumentSequenceTextPointer thisTp, DocumentSequenceTextPointer tp)
		{
			if (thisTp == tp)
			{
				return 0;
			}
			ChildDocumentBlock childBlock = thisTp.ChildBlock;
			ChildDocumentBlock childBlock2 = tp.ChildBlock;
			int childBlockDistance = thisTp.AggregatedContainer.GetChildBlockDistance(childBlock, childBlock2);
			if (childBlockDistance == 0)
			{
				return thisTp.ChildPointer.CompareTo(tp.ChildPointer);
			}
			if (childBlockDistance < 0)
			{
				if (!DocumentSequenceTextPointer.xUnseparated(tp, thisTp))
				{
					return 1;
				}
				return 0;
			}
			else
			{
				if (!DocumentSequenceTextPointer.xUnseparated(thisTp, tp))
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x06002C1D RID: 11293 RVA: 0x000C8494 File Offset: 0x000C6694
		private static bool xUnseparated(DocumentSequenceTextPointer tp1, DocumentSequenceTextPointer tp2)
		{
			if (tp1.ChildPointer.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.None || tp2.ChildPointer.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.None)
			{
				return false;
			}
			for (ChildDocumentBlock nextBlock = tp1.ChildBlock.NextBlock; nextBlock != tp2.ChildBlock; nextBlock = nextBlock.NextBlock)
			{
				if (nextBlock.ChildContainer.Start.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.None)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x000C84F4 File Offset: 0x000C66F4
		private static int xGapAwareGetDistance(DocumentSequenceTextPointer tp1, DocumentSequenceTextPointer tp2)
		{
			if (tp1 == tp2)
			{
				return 0;
			}
			int num = 0;
			DocumentSequenceTextPointer documentSequenceTextPointer = new DocumentSequenceTextPointer(tp1.ChildBlock, tp1.ChildPointer);
			while (documentSequenceTextPointer.ChildBlock != tp2.ChildBlock)
			{
				num += documentSequenceTextPointer.ChildPointer.GetOffsetToPosition(documentSequenceTextPointer.ChildPointer.TextContainer.End);
				ChildDocumentBlock nextBlock = documentSequenceTextPointer.ChildBlock.NextBlock;
				documentSequenceTextPointer.ChildBlock = nextBlock;
				documentSequenceTextPointer.ChildPointer = nextBlock.ChildContainer.Start;
			}
			return num + documentSequenceTextPointer.ChildPointer.GetOffsetToPosition(tp2.ChildPointer);
		}

		// Token: 0x06002C1F RID: 11295 RVA: 0x000C8584 File Offset: 0x000C6784
		private static bool xGapAwareScan(DocumentSequenceTextPointer thisTp, int distance)
		{
			ChildDocumentBlock childDocumentBlock = thisTp.ChildBlock;
			bool flag = true;
			ITextPointer textPointer = thisTp.ChildPointer;
			if (textPointer == null)
			{
				flag = false;
				textPointer = thisTp.ChildPointer.CreatePointer();
			}
			LogicalDirection logicalDirection = (distance > 0) ? LogicalDirection.Forward : LogicalDirection.Backward;
			distance = Math.Abs(distance);
			while (distance > 0)
			{
				switch (textPointer.GetPointerContext(logicalDirection))
				{
				case TextPointerContext.None:
					if ((childDocumentBlock.IsHead && logicalDirection == LogicalDirection.Backward) || (childDocumentBlock.IsTail && logicalDirection == LogicalDirection.Forward))
					{
						return false;
					}
					childDocumentBlock = ((logicalDirection == LogicalDirection.Forward) ? childDocumentBlock.NextBlock : childDocumentBlock.PreviousBlock);
					textPointer = ((logicalDirection == LogicalDirection.Forward) ? childDocumentBlock.ChildContainer.Start.CreatePointer(textPointer.LogicalDirection) : childDocumentBlock.ChildContainer.End.CreatePointer(textPointer.LogicalDirection));
					break;
				case TextPointerContext.Text:
				{
					int textRunLength = textPointer.GetTextRunLength(logicalDirection);
					int num = (textRunLength < distance) ? textRunLength : distance;
					distance -= num;
					if (logicalDirection == LogicalDirection.Backward)
					{
						num *= -1;
					}
					textPointer.MoveByOffset(num);
					break;
				}
				case TextPointerContext.EmbeddedElement:
					textPointer.MoveToNextContextPosition(logicalDirection);
					distance--;
					break;
				case TextPointerContext.ElementStart:
					textPointer.MoveToNextContextPosition(logicalDirection);
					distance--;
					break;
				case TextPointerContext.ElementEnd:
					textPointer.MoveToNextContextPosition(logicalDirection);
					distance--;
					break;
				}
			}
			thisTp.ChildBlock = childDocumentBlock;
			if (flag)
			{
				thisTp.ChildPointer = textPointer;
			}
			else
			{
				thisTp.ChildPointer = textPointer.CreatePointer();
			}
			return true;
		}

		// Token: 0x04001CC4 RID: 7364
		private ChildDocumentBlock _childBlock;

		// Token: 0x04001CC5 RID: 7365
		private ITextPointer _childTp;

		// Token: 0x04001CC6 RID: 7366
		private bool _isFrozen;
	}
}
