using System;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000399 RID: 921
	internal sealed class NullTextContainer : ITextContainer
	{
		// Token: 0x060031E1 RID: 12769 RVA: 0x000DC1D8 File Offset: 0x000DA3D8
		internal NullTextContainer()
		{
			this._start = new NullTextPointer(this, LogicalDirection.Backward);
			this._end = new NullTextPointer(this, LogicalDirection.Forward);
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x00002137 File Offset: 0x00000337
		void ITextContainer.BeginChange()
		{
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x000C732F File Offset: 0x000C552F
		void ITextContainer.BeginChangeNoUndo()
		{
			((ITextContainer)this).BeginChange();
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x000C7337 File Offset: 0x000C5537
		void ITextContainer.EndChange()
		{
			((ITextContainer)this).EndChange(false);
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x00002137 File Offset: 0x00000337
		void ITextContainer.EndChange(bool skipEvents)
		{
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x000C73A4 File Offset: 0x000C55A4
		ITextPointer ITextContainer.CreatePointerAtOffset(int offset, LogicalDirection direction)
		{
			return ((ITextContainer)this).Start.CreatePointer(offset, direction);
		}

		// Token: 0x060031E7 RID: 12775 RVA: 0x0003E264 File Offset: 0x0003C464
		ITextPointer ITextContainer.CreatePointerAtCharOffset(int charOffset, LogicalDirection direction)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060031E8 RID: 12776 RVA: 0x000C73B3 File Offset: 0x000C55B3
		ITextPointer ITextContainer.CreateDynamicTextPointer(StaticTextPointer position, LogicalDirection direction)
		{
			return ((ITextPointer)position.Handle0).CreatePointer(direction);
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x000C73C7 File Offset: 0x000C55C7
		StaticTextPointer ITextContainer.CreateStaticPointerAtOffset(int offset)
		{
			return new StaticTextPointer(this, ((ITextContainer)this).CreatePointerAtOffset(offset, LogicalDirection.Forward));
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x000C73D7 File Offset: 0x000C55D7
		TextPointerContext ITextContainer.GetPointerContext(StaticTextPointer pointer, LogicalDirection direction)
		{
			return ((ITextPointer)pointer.Handle0).GetPointerContext(direction);
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x000C73EB File Offset: 0x000C55EB
		int ITextContainer.GetOffsetToPosition(StaticTextPointer position1, StaticTextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).GetOffsetToPosition((ITextPointer)position2.Handle0);
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x000C740A File Offset: 0x000C560A
		int ITextContainer.GetTextInRun(StaticTextPointer position, LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			return ((ITextPointer)position.Handle0).GetTextInRun(direction, textBuffer, startIndex, count);
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x000C7423 File Offset: 0x000C5623
		object ITextContainer.GetAdjacentElement(StaticTextPointer position, LogicalDirection direction)
		{
			return ((ITextPointer)position.Handle0).GetAdjacentElement(direction);
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x0000C238 File Offset: 0x0000A438
		DependencyObject ITextContainer.GetParent(StaticTextPointer position)
		{
			return null;
		}

		// Token: 0x060031EF RID: 12783 RVA: 0x000C7437 File Offset: 0x000C5637
		StaticTextPointer ITextContainer.CreatePointer(StaticTextPointer position, int offset)
		{
			return new StaticTextPointer(this, ((ITextPointer)position.Handle0).CreatePointer(offset));
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x000C7451 File Offset: 0x000C5651
		StaticTextPointer ITextContainer.GetNextContextPosition(StaticTextPointer position, LogicalDirection direction)
		{
			return new StaticTextPointer(this, ((ITextPointer)position.Handle0).GetNextContextPosition(direction));
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x000C746B File Offset: 0x000C566B
		int ITextContainer.CompareTo(StaticTextPointer position1, StaticTextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).CompareTo((ITextPointer)position2.Handle0);
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x000C748A File Offset: 0x000C568A
		int ITextContainer.CompareTo(StaticTextPointer position1, ITextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).CompareTo(position2);
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x000C749E File Offset: 0x000C569E
		object ITextContainer.GetValue(StaticTextPointer position, DependencyProperty formattingProperty)
		{
			return ((ITextPointer)position.Handle0).GetValue(formattingProperty);
		}

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x060031F4 RID: 12788 RVA: 0x00016748 File Offset: 0x00014948
		bool ITextContainer.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x060031F5 RID: 12789 RVA: 0x000DC1FA File Offset: 0x000DA3FA
		ITextPointer ITextContainer.Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x060031F6 RID: 12790 RVA: 0x000DC202 File Offset: 0x000DA402
		ITextPointer ITextContainer.End
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x060031F7 RID: 12791 RVA: 0x0000B02A File Offset: 0x0000922A
		uint ITextContainer.Generation
		{
			get
			{
				return 0U;
			}
		}

		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x060031F8 RID: 12792 RVA: 0x0000C238 File Offset: 0x0000A438
		Highlights ITextContainer.Highlights
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x060031F9 RID: 12793 RVA: 0x0000C238 File Offset: 0x0000A438
		DependencyObject ITextContainer.Parent
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x060031FA RID: 12794 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x060031FB RID: 12795 RVA: 0x000DC20A File Offset: 0x000DA40A
		ITextSelection ITextContainer.TextSelection
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "NullTextContainer is never associated with a TextEditor/TextSelection!");
			}
		}

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x060031FC RID: 12796 RVA: 0x0000C238 File Offset: 0x0000A438
		UndoManager ITextContainer.UndoManager
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x060031FD RID: 12797 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x060031FE RID: 12798 RVA: 0x00002137 File Offset: 0x00000337
		ITextView ITextContainer.TextView
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x060031FF RID: 12799 RVA: 0x0000B02A File Offset: 0x0000922A
		int ITextContainer.SymbolCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x06003200 RID: 12800 RVA: 0x000DC217 File Offset: 0x000DA417
		int ITextContainer.IMECharCount
		{
			get
			{
				Invariant.Assert(false);
				return 0;
			}
		}

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06003201 RID: 12801 RVA: 0x00002137 File Offset: 0x00000337
		// (remove) Token: 0x06003202 RID: 12802 RVA: 0x00002137 File Offset: 0x00000337
		public event EventHandler Changing
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x06003203 RID: 12803 RVA: 0x00002137 File Offset: 0x00000337
		// (remove) Token: 0x06003204 RID: 12804 RVA: 0x00002137 File Offset: 0x00000337
		public event TextContainerChangeEventHandler Change
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x06003205 RID: 12805 RVA: 0x00002137 File Offset: 0x00000337
		// (remove) Token: 0x06003206 RID: 12806 RVA: 0x00002137 File Offset: 0x00000337
		public event TextContainerChangedEventHandler Changed
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x04001EA8 RID: 7848
		private NullTextPointer _start;

		// Token: 0x04001EA9 RID: 7849
		private NullTextPointer _end;
	}
}
