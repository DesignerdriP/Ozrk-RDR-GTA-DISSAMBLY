using System;
using System.Collections;
using System.Collections.Generic;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x020007F4 RID: 2036
	internal class WordSegment
	{
		// Token: 0x06007D5A RID: 32090 RVA: 0x00233808 File Offset: 0x00231A08
		static WordSegment()
		{
			try
			{
				WordSegment.s_WinRTType = Type.GetType(WordSegment.s_TypeName);
			}
			catch
			{
				WordSegment.s_WinRTType = null;
			}
		}

		// Token: 0x06007D5B RID: 32091 RVA: 0x00233850 File Offset: 0x00231A50
		public WordSegment(object wordSegment)
		{
			if (WordSegment.s_WinRTType == null)
			{
				throw new PlatformNotSupportedException();
			}
			if (wordSegment.GetType() != WordSegment.s_WinRTType)
			{
				throw new ArgumentException();
			}
			this._wordSegment = wordSegment;
		}

		// Token: 0x17001D20 RID: 7456
		// (get) Token: 0x06007D5C RID: 32092 RVA: 0x0023388C File Offset: 0x00231A8C
		public IReadOnlyList<AlternateWordForm> AlternateForms
		{
			get
			{
				if (this._alternateForms == null)
				{
					object obj = this._wordSegment.ReflectionGetProperty("AlternateForms");
					List<AlternateWordForm> list = new List<AlternateWordForm>();
					foreach (object alternateWordForm in ((IEnumerable)obj))
					{
						list.Add(new AlternateWordForm(alternateWordForm));
					}
					this._alternateForms = list.AsReadOnly();
				}
				return this._alternateForms;
			}
		}

		// Token: 0x17001D21 RID: 7457
		// (get) Token: 0x06007D5D RID: 32093 RVA: 0x0023391C File Offset: 0x00231B1C
		public TextSegment SourceTextSegment
		{
			get
			{
				if (this._sourceTextSegment == null)
				{
					this._sourceTextSegment = new TextSegment(this._wordSegment.ReflectionGetProperty("SourceTextSegment"));
				}
				return this._sourceTextSegment;
			}
		}

		// Token: 0x17001D22 RID: 7458
		// (get) Token: 0x06007D5E RID: 32094 RVA: 0x00233947 File Offset: 0x00231B47
		public string Text
		{
			get
			{
				if (this._text == null)
				{
					this._text = this._wordSegment.ReflectionGetProperty("Text");
				}
				return this._text;
			}
		}

		// Token: 0x17001D23 RID: 7459
		// (get) Token: 0x06007D5F RID: 32095 RVA: 0x0023396D File Offset: 0x00231B6D
		public static Type WinRTType
		{
			get
			{
				return WordSegment.s_WinRTType;
			}
		}

		// Token: 0x04003AFF RID: 15103
		private object _wordSegment;

		// Token: 0x04003B00 RID: 15104
		private IReadOnlyList<AlternateWordForm> _alternateForms;

		// Token: 0x04003B01 RID: 15105
		private TextSegment _sourceTextSegment;

		// Token: 0x04003B02 RID: 15106
		private string _text;

		// Token: 0x04003B03 RID: 15107
		private static string s_TypeName = "Windows.Data.Text.WordSegment, Windows, ContentType=WindowsRuntime";

		// Token: 0x04003B04 RID: 15108
		private static Type s_WinRTType = null;
	}
}
