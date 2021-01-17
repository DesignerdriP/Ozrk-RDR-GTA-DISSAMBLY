using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Documents;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Represents a misspelled word in an editing control (i.e. <see cref="T:System.Windows.Controls.TextBox" /> or <see cref="T:System.Windows.Controls.RichTextBox" />).</summary>
	// Token: 0x02000561 RID: 1377
	public class SpellingError
	{
		// Token: 0x06005B55 RID: 23381 RVA: 0x0019C11A File Offset: 0x0019A31A
		internal SpellingError(Speller speller, ITextPointer start, ITextPointer end)
		{
			Invariant.Assert(start.CompareTo(end) < 0);
			this._speller = speller;
			this._start = start.GetFrozenPointer(LogicalDirection.Forward);
			this._end = end.GetFrozenPointer(LogicalDirection.Backward);
		}

		/// <summary>Replaces the spelling error text with the specified correction.</summary>
		/// <param name="correctedText">The text used to replace the misspelled text.</param>
		// Token: 0x06005B56 RID: 23382 RVA: 0x0019C154 File Offset: 0x0019A354
		public void Correct(string correctedText)
		{
			if (correctedText == null)
			{
				correctedText = string.Empty;
			}
			ITextRange textRange = new TextRange(this._start, this._end);
			textRange.Text = correctedText;
		}

		/// <summary>Instructs the control to ignore this error and any duplicates for the remainder of the lifetime of the control.</summary>
		// Token: 0x06005B57 RID: 23383 RVA: 0x0019C184 File Offset: 0x0019A384
		public void IgnoreAll()
		{
			this._speller.IgnoreAll(TextRangeBase.GetTextInternal(this._start, this._end));
		}

		/// <summary>Gets a list of suggested spelling replacements for the misspelled word.</summary>
		/// <returns>The collection of spelling suggestions for the misspelled word.</returns>
		// Token: 0x17001620 RID: 5664
		// (get) Token: 0x06005B58 RID: 23384 RVA: 0x0019C1A4 File Offset: 0x0019A3A4
		public IEnumerable<string> Suggestions
		{
			get
			{
				IList suggestions = this._speller.GetSuggestionsForError(this);
				int num;
				for (int i = 0; i < suggestions.Count; i = num + 1)
				{
					yield return (string)suggestions[i];
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17001621 RID: 5665
		// (get) Token: 0x06005B59 RID: 23385 RVA: 0x0019C1C1 File Offset: 0x0019A3C1
		internal ITextPointer Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17001622 RID: 5666
		// (get) Token: 0x06005B5A RID: 23386 RVA: 0x0019C1C9 File Offset: 0x0019A3C9
		internal ITextPointer End
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x04002F73 RID: 12147
		private readonly Speller _speller;

		// Token: 0x04002F74 RID: 12148
		private readonly ITextPointer _start;

		// Token: 0x04002F75 RID: 12149
		private readonly ITextPointer _end;
	}
}
