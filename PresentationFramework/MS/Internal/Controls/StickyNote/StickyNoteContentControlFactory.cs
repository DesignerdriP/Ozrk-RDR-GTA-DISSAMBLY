using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Xml;

namespace MS.Internal.Controls.StickyNote
{
	// Token: 0x0200076B RID: 1899
	internal static class StickyNoteContentControlFactory
	{
		// Token: 0x06007871 RID: 30833 RVA: 0x00224F6C File Offset: 0x0022316C
		public static StickyNoteContentControl CreateContentControl(StickyNoteType type, UIElement content)
		{
			StickyNoteContentControl result = null;
			if (type != StickyNoteType.Text)
			{
				if (type == StickyNoteType.Ink)
				{
					InkCanvas inkCanvas = content as InkCanvas;
					if (inkCanvas == null)
					{
						throw new InvalidOperationException(SR.Get("InvalidStickyNoteTemplate", new object[]
						{
							type,
							typeof(InkCanvas),
							"PART_ContentControl"
						}));
					}
					result = new StickyNoteContentControlFactory.StickyNoteInkCanvas(inkCanvas);
				}
			}
			else
			{
				RichTextBox richTextBox = content as RichTextBox;
				if (richTextBox == null)
				{
					throw new InvalidOperationException(SR.Get("InvalidStickyNoteTemplate", new object[]
					{
						type,
						typeof(RichTextBox),
						"PART_ContentControl"
					}));
				}
				result = new StickyNoteContentControlFactory.StickyNoteRichTextBox(richTextBox);
			}
			return result;
		}

		// Token: 0x02000B6B RID: 2923
		private class StickyNoteRichTextBox : StickyNoteContentControl
		{
			// Token: 0x06008E10 RID: 36368 RVA: 0x0025B15E File Offset: 0x0025935E
			public StickyNoteRichTextBox(RichTextBox rtb) : base(rtb)
			{
				DataObject.AddPastingHandler(rtb, new DataObjectPastingEventHandler(this.OnPastingDataObject));
			}

			// Token: 0x06008E11 RID: 36369 RVA: 0x0025B179 File Offset: 0x00259379
			public override void Clear()
			{
				((RichTextBox)base.InnerControl).Document = new FlowDocument(new Paragraph(new Run()));
			}

			// Token: 0x06008E12 RID: 36370 RVA: 0x0025B19C File Offset: 0x0025939C
			public override void Save(XmlNode node)
			{
				RichTextBox richTextBox = (RichTextBox)base.InnerControl;
				TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
				if (!textRange.IsEmpty)
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						textRange.Save(memoryStream, DataFormats.Xaml);
						if (memoryStream.Length.CompareTo(1610612733L) > 0)
						{
							throw new InvalidOperationException(SR.Get("MaximumNoteSizeExceeded"));
						}
						node.InnerText = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
					}
				}
			}

			// Token: 0x06008E13 RID: 36371 RVA: 0x0025B248 File Offset: 0x00259448
			public override void Load(XmlNode node)
			{
				RichTextBox richTextBox = (RichTextBox)base.InnerControl;
				FlowDocument flowDocument = new FlowDocument();
				TextRange textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd, true);
				using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(node.InnerText)))
				{
					textRange.Load(memoryStream, DataFormats.Xaml);
				}
				richTextBox.Document = flowDocument;
			}

			// Token: 0x17001F98 RID: 8088
			// (get) Token: 0x06008E14 RID: 36372 RVA: 0x0025B2BC File Offset: 0x002594BC
			public override bool IsEmpty
			{
				get
				{
					RichTextBox richTextBox = (RichTextBox)base.InnerControl;
					TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
					return textRange.IsEmpty;
				}
			}

			// Token: 0x17001F99 RID: 8089
			// (get) Token: 0x06008E15 RID: 36373 RVA: 0x0000B02A File Offset: 0x0000922A
			public override StickyNoteType Type
			{
				get
				{
					return StickyNoteType.Text;
				}
			}

			// Token: 0x06008E16 RID: 36374 RVA: 0x0025B2F8 File Offset: 0x002594F8
			private void OnPastingDataObject(object sender, DataObjectPastingEventArgs e)
			{
				if (e.FormatToApply == DataFormats.Rtf)
				{
					UTF8Encoding utf8Encoding = new UTF8Encoding();
					string s = e.DataObject.GetData(DataFormats.Rtf) as string;
					MemoryStream stream = new MemoryStream(utf8Encoding.GetBytes(s));
					FlowDocument flowDocument = new FlowDocument();
					TextRange textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
					textRange.Load(stream, DataFormats.Rtf);
					MemoryStream memoryStream = new MemoryStream();
					textRange.Save(memoryStream, DataFormats.Xaml);
					DataObject dataObject = new DataObject();
					dataObject.SetData(DataFormats.Xaml, utf8Encoding.GetString(memoryStream.GetBuffer()));
					e.DataObject = dataObject;
					e.FormatToApply = DataFormats.Xaml;
					return;
				}
				if (e.FormatToApply == DataFormats.Bitmap || e.FormatToApply == DataFormats.EnhancedMetafile || e.FormatToApply == DataFormats.MetafilePicture || e.FormatToApply == DataFormats.Tiff)
				{
					e.CancelCommand();
					return;
				}
				if (e.FormatToApply == DataFormats.XamlPackage)
				{
					e.FormatToApply = DataFormats.Xaml;
				}
			}
		}

		// Token: 0x02000B6C RID: 2924
		private class StickyNoteInkCanvas : StickyNoteContentControl
		{
			// Token: 0x06008E17 RID: 36375 RVA: 0x0025B41E File Offset: 0x0025961E
			public StickyNoteInkCanvas(InkCanvas canvas) : base(canvas)
			{
			}

			// Token: 0x06008E18 RID: 36376 RVA: 0x0025B427 File Offset: 0x00259627
			public override void Clear()
			{
				((InkCanvas)base.InnerControl).Strokes.Clear();
			}

			// Token: 0x06008E19 RID: 36377 RVA: 0x0025B440 File Offset: 0x00259640
			public override void Save(XmlNode node)
			{
				StrokeCollection strokes = ((InkCanvas)base.InnerControl).Strokes;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					strokes.Save(memoryStream);
					if (memoryStream.Length.CompareTo(1610612733L) > 0)
					{
						throw new InvalidOperationException(SR.Get("MaximumNoteSizeExceeded"));
					}
					node.InnerText = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
				}
			}

			// Token: 0x06008E1A RID: 36378 RVA: 0x0025B4C8 File Offset: 0x002596C8
			public override void Load(XmlNode node)
			{
				StrokeCollection strokes = null;
				if (string.IsNullOrEmpty(node.InnerText))
				{
					strokes = new StrokeCollection();
				}
				else
				{
					using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(node.InnerText)))
					{
						strokes = new StrokeCollection(memoryStream);
					}
				}
				((InkCanvas)base.InnerControl).Strokes = strokes;
			}

			// Token: 0x17001F9A RID: 8090
			// (get) Token: 0x06008E1B RID: 36379 RVA: 0x0025B534 File Offset: 0x00259734
			public override bool IsEmpty
			{
				get
				{
					return ((InkCanvas)base.InnerControl).Strokes.Count == 0;
				}
			}

			// Token: 0x17001F9B RID: 8091
			// (get) Token: 0x06008E1C RID: 36380 RVA: 0x00016748 File Offset: 0x00014948
			public override StickyNoteType Type
			{
				get
				{
					return StickyNoteType.Ink;
				}
			}
		}
	}
}
