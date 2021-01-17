using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MS.Internal.Ink
{
	// Token: 0x02000695 RID: 1685
	internal class TextClipboardData : ElementsClipboardData
	{
		// Token: 0x06006DFC RID: 28156 RVA: 0x001FA363 File Offset: 0x001F8563
		internal TextClipboardData() : this(null)
		{
		}

		// Token: 0x06006DFD RID: 28157 RVA: 0x001FA36C File Offset: 0x001F856C
		internal TextClipboardData(string text)
		{
			this._text = text;
		}

		// Token: 0x06006DFE RID: 28158 RVA: 0x001FA37B File Offset: 0x001F857B
		internal override bool CanPaste(IDataObject dataObject)
		{
			return dataObject.GetDataPresent(DataFormats.UnicodeText, false) || dataObject.GetDataPresent(DataFormats.Text, false) || dataObject.GetDataPresent(DataFormats.OemText, false);
		}

		// Token: 0x06006DFF RID: 28159 RVA: 0x001FA3A7 File Offset: 0x001F85A7
		protected override bool CanCopy()
		{
			return !string.IsNullOrEmpty(this._text);
		}

		// Token: 0x06006E00 RID: 28160 RVA: 0x001FA3B7 File Offset: 0x001F85B7
		protected override void DoCopy(IDataObject dataObject)
		{
			dataObject.SetData(DataFormats.UnicodeText, this._text, true);
		}

		// Token: 0x06006E01 RID: 28161 RVA: 0x001FA3CC File Offset: 0x001F85CC
		protected override void DoPaste(IDataObject dataObject)
		{
			base.ElementList = new List<UIElement>();
			string text = dataObject.GetData(DataFormats.UnicodeText, true) as string;
			if (string.IsNullOrEmpty(text))
			{
				text = (dataObject.GetData(DataFormats.Text, true) as string);
			}
			if (!string.IsNullOrEmpty(text))
			{
				TextBox textBox = new TextBox();
				textBox.Text = text;
				textBox.TextWrapping = TextWrapping.Wrap;
				base.ElementList.Add(textBox);
			}
		}

		// Token: 0x0400361E RID: 13854
		private string _text;
	}
}
