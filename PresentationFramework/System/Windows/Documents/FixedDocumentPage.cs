﻿using System;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x02000344 RID: 836
	internal sealed class FixedDocumentPage : DocumentPage, IServiceProvider
	{
		// Token: 0x06002CE4 RID: 11492 RVA: 0x000CA9D1 File Offset: 0x000C8BD1
		internal FixedDocumentPage(FixedDocument panel, FixedPage page, Size fixedSize, int index) : base(page, fixedSize, new Rect(fixedSize), new Rect(fixedSize))
		{
			this._panel = panel;
			this._page = page;
			this._index = index;
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x000CA9FD File Offset: 0x000C8BFD
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextView))
			{
				return this.TextView;
			}
			return null;
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x06002CE6 RID: 11494 RVA: 0x000CAA30 File Offset: 0x000C8C30
		public override Visual Visual
		{
			get
			{
				if (!this._layedOut)
				{
					this._layedOut = true;
					UIElement uielement;
					if ((uielement = (base.Visual as UIElement)) != null)
					{
						uielement.Measure(base.Size);
						uielement.Arrange(new Rect(base.Size));
					}
				}
				return base.Visual;
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06002CE7 RID: 11495 RVA: 0x000CAA80 File Offset: 0x000C8C80
		internal ContentPosition ContentPosition
		{
			get
			{
				FlowPosition pageStartFlowPosition = this._panel.FixedContainer.FixedTextBuilder.GetPageStartFlowPosition(this._index);
				return new FixedTextPointer(true, LogicalDirection.Forward, pageStartFlowPosition);
			}
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06002CE8 RID: 11496 RVA: 0x000CAAB1 File Offset: 0x000C8CB1
		internal FixedPage FixedPage
		{
			get
			{
				return this._page;
			}
		}

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06002CE9 RID: 11497 RVA: 0x000CAAB9 File Offset: 0x000C8CB9
		internal int PageIndex
		{
			get
			{
				return this._panel.GetIndexOfPage(this._page);
			}
		}

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06002CEA RID: 11498 RVA: 0x000CAACC File Offset: 0x000C8CCC
		internal FixedTextView TextView
		{
			get
			{
				if (this._textView == null)
				{
					this._textView = new FixedTextView(this);
				}
				return this._textView;
			}
		}

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06002CEB RID: 11499 RVA: 0x000CAAE8 File Offset: 0x000C8CE8
		internal FixedDocument Owner
		{
			get
			{
				return this._panel;
			}
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x06002CEC RID: 11500 RVA: 0x000CAAF0 File Offset: 0x000C8CF0
		internal FixedTextContainer TextContainer
		{
			get
			{
				return this._panel.FixedContainer;
			}
		}

		// Token: 0x04001D46 RID: 7494
		private readonly FixedDocument _panel;

		// Token: 0x04001D47 RID: 7495
		private readonly FixedPage _page;

		// Token: 0x04001D48 RID: 7496
		private readonly int _index;

		// Token: 0x04001D49 RID: 7497
		private bool _layedOut;

		// Token: 0x04001D4A RID: 7498
		private FixedTextView _textView;
	}
}
