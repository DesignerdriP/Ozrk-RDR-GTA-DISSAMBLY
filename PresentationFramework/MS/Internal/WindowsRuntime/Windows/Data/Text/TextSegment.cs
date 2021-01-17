using System;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x020007F3 RID: 2035
	internal class TextSegment
	{
		// Token: 0x06007D55 RID: 32085 RVA: 0x00233718 File Offset: 0x00231918
		static TextSegment()
		{
			try
			{
				TextSegment.s_WinRTType = Type.GetType(TextSegment.s_TypeName);
			}
			catch
			{
				TextSegment.s_WinRTType = null;
			}
		}

		// Token: 0x06007D56 RID: 32086 RVA: 0x0023375C File Offset: 0x0023195C
		public TextSegment(object textSegment)
		{
			if (TextSegment.s_WinRTType == null)
			{
				throw new PlatformNotSupportedException();
			}
			if (textSegment.GetType() != TextSegment.s_WinRTType)
			{
				throw new ArgumentException();
			}
			this._textSegment = textSegment;
		}

		// Token: 0x17001D1D RID: 7453
		// (get) Token: 0x06007D57 RID: 32087 RVA: 0x00233796 File Offset: 0x00231996
		public uint Length
		{
			get
			{
				if (this._length == null)
				{
					this._length = new uint?(this._textSegment.ReflectionGetField("Length"));
				}
				return this._length.Value;
			}
		}

		// Token: 0x17001D1E RID: 7454
		// (get) Token: 0x06007D58 RID: 32088 RVA: 0x002337CB File Offset: 0x002319CB
		public uint StartPosition
		{
			get
			{
				if (this._startPosition == null)
				{
					this._startPosition = new uint?(this._textSegment.ReflectionGetField("StartPosition"));
				}
				return this._startPosition.Value;
			}
		}

		// Token: 0x17001D1F RID: 7455
		// (get) Token: 0x06007D59 RID: 32089 RVA: 0x00233800 File Offset: 0x00231A00
		public static Type WinRTType
		{
			get
			{
				return TextSegment.s_WinRTType;
			}
		}

		// Token: 0x04003AFA RID: 15098
		private static Type s_WinRTType;

		// Token: 0x04003AFB RID: 15099
		private static string s_TypeName = "Windows.Data.Text.TextSegment, Windows, ContentType=WindowsRuntime";

		// Token: 0x04003AFC RID: 15100
		private object _textSegment;

		// Token: 0x04003AFD RID: 15101
		private uint? _length;

		// Token: 0x04003AFE RID: 15102
		private uint? _startPosition;
	}
}
