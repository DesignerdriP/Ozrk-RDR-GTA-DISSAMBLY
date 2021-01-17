using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Ink;

namespace MS.Internal.Ink
{
	// Token: 0x0200068C RID: 1676
	internal class ISFClipboardData : ClipboardData
	{
		// Token: 0x06006DA4 RID: 28068 RVA: 0x001F6682 File Offset: 0x001F4882
		internal ISFClipboardData()
		{
		}

		// Token: 0x06006DA5 RID: 28069 RVA: 0x001F7E41 File Offset: 0x001F6041
		internal ISFClipboardData(StrokeCollection strokes)
		{
			this._strokes = strokes;
		}

		// Token: 0x06006DA6 RID: 28070 RVA: 0x001F7E50 File Offset: 0x001F6050
		internal override bool CanPaste(IDataObject dataObject)
		{
			return dataObject.GetDataPresent(StrokeCollection.InkSerializedFormat, false);
		}

		// Token: 0x06006DA7 RID: 28071 RVA: 0x001F7E5E File Offset: 0x001F605E
		protected override bool CanCopy()
		{
			return this.Strokes != null && this.Strokes.Count != 0;
		}

		// Token: 0x06006DA8 RID: 28072 RVA: 0x001F7E78 File Offset: 0x001F6078
		[SecurityCritical]
		protected override void DoCopy(IDataObject dataObject)
		{
			MemoryStream memoryStream = new MemoryStream();
			this.Strokes.Save(memoryStream);
			memoryStream.Position = 0L;
			new UIPermission(UIPermissionClipboard.AllClipboard).Assert();
			try
			{
				dataObject.SetData(StrokeCollection.InkSerializedFormat, memoryStream);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06006DA9 RID: 28073 RVA: 0x001F7ED0 File Offset: 0x001F60D0
		protected override void DoPaste(IDataObject dataObject)
		{
			MemoryStream memoryStream = dataObject.GetData(StrokeCollection.InkSerializedFormat) as MemoryStream;
			StrokeCollection strokeCollection = null;
			bool flag = false;
			if (memoryStream != null && memoryStream != Stream.Null)
			{
				try
				{
					strokeCollection = new StrokeCollection(memoryStream);
					flag = true;
				}
				catch (ArgumentException)
				{
					flag = false;
				}
			}
			this._strokes = (flag ? strokeCollection : new StrokeCollection());
		}

		// Token: 0x17001A21 RID: 6689
		// (get) Token: 0x06006DAA RID: 28074 RVA: 0x001F7F30 File Offset: 0x001F6130
		internal StrokeCollection Strokes
		{
			get
			{
				return this._strokes;
			}
		}

		// Token: 0x040035FA RID: 13818
		private StrokeCollection _strokes;
	}
}
