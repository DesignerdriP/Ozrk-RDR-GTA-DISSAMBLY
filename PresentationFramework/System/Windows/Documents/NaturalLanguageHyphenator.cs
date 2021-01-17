using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Media.TextFormatting;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000398 RID: 920
	internal class NaturalLanguageHyphenator : TextLexicalService, IDisposable
	{
		// Token: 0x060031DB RID: 12763 RVA: 0x000DC098 File Offset: 0x000DA298
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal NaturalLanguageHyphenator()
		{
			try
			{
				this._hyphenatorResource = NaturalLanguageHyphenator.UnsafeNativeMethods.NlCreateHyphenator();
			}
			catch (DllNotFoundException)
			{
			}
			catch (EntryPointNotFoundException)
			{
			}
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x000DC0DC File Offset: 0x000DA2DC
		~NaturalLanguageHyphenator()
		{
			this.CleanupInternal(true);
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x000DC10C File Offset: 0x000DA30C
		void IDisposable.Dispose()
		{
			GC.SuppressFinalize(this);
			this.CleanupInternal(false);
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x000DC11B File Offset: 0x000DA31B
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void CleanupInternal(bool finalizing)
		{
			if (!this._disposed && this._hyphenatorResource != IntPtr.Zero)
			{
				NaturalLanguageHyphenator.UnsafeNativeMethods.NlDestroyHyphenator(ref this._hyphenatorResource);
				this._disposed = true;
			}
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x00016748 File Offset: 0x00014948
		public override bool IsCultureSupported(CultureInfo culture)
		{
			return true;
		}

		// Token: 0x060031E0 RID: 12768 RVA: 0x000DC14C File Offset: 0x000DA34C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public override TextLexicalBreaks AnalyzeText(char[] characterSource, int length, CultureInfo textCulture)
		{
			Invariant.Assert(characterSource != null && characterSource.Length != 0 && length > 0 && length <= characterSource.Length);
			if (this._hyphenatorResource == IntPtr.Zero)
			{
				return null;
			}
			if (this._disposed)
			{
				throw new ObjectDisposedException(SR.Get("HyphenatorDisposed"));
			}
			byte[] array = new byte[(length + 7) / 8];
			NaturalLanguageHyphenator.UnsafeNativeMethods.NlHyphenate(this._hyphenatorResource, characterSource, length, (textCulture != null && textCulture != CultureInfo.InvariantCulture) ? textCulture.LCID : 0, array, array.Length);
			return new NaturalLanguageHyphenator.HyphenBreaks(array, length);
		}

		// Token: 0x04001EA6 RID: 7846
		[SecurityCritical]
		private IntPtr _hyphenatorResource;

		// Token: 0x04001EA7 RID: 7847
		private bool _disposed;

		// Token: 0x020008D8 RID: 2264
		private class HyphenBreaks : TextLexicalBreaks
		{
			// Token: 0x0600849F RID: 33951 RVA: 0x0024921F File Offset: 0x0024741F
			internal HyphenBreaks(byte[] isHyphenPositions, int numPositions)
			{
				this._isHyphenPositions = isHyphenPositions;
				this._numPositions = numPositions;
			}

			// Token: 0x17001E07 RID: 7687
			private bool this[int index]
			{
				get
				{
					return ((int)this._isHyphenPositions[index / 8] & 1 << index % 8) != 0;
				}
			}

			// Token: 0x17001E08 RID: 7688
			// (get) Token: 0x060084A1 RID: 33953 RVA: 0x0024924D File Offset: 0x0024744D
			public override int Length
			{
				get
				{
					return this._numPositions;
				}
			}

			// Token: 0x060084A2 RID: 33954 RVA: 0x00249258 File Offset: 0x00247458
			public override int GetNextBreak(int currentIndex)
			{
				if (this._isHyphenPositions != null && currentIndex >= 0)
				{
					int num = currentIndex + 1;
					while (num < this._numPositions && !this[num])
					{
						num++;
					}
					if (num < this._numPositions)
					{
						return num;
					}
				}
				return -1;
			}

			// Token: 0x060084A3 RID: 33955 RVA: 0x0024929C File Offset: 0x0024749C
			public override int GetPreviousBreak(int currentIndex)
			{
				if (this._isHyphenPositions != null && currentIndex < this._numPositions)
				{
					int num = currentIndex;
					while (num > 0 && !this[num])
					{
						num--;
					}
					if (num > 0)
					{
						return num;
					}
				}
				return -1;
			}

			// Token: 0x04004287 RID: 17031
			private byte[] _isHyphenPositions;

			// Token: 0x04004288 RID: 17032
			private int _numPositions;
		}

		// Token: 0x020008D9 RID: 2265
		private static class UnsafeNativeMethods
		{
			// Token: 0x060084A4 RID: 33956
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			[DllImport("PresentationNative_v0400.dll", PreserveSig = false)]
			internal static extern IntPtr NlCreateHyphenator();

			// Token: 0x060084A5 RID: 33957
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			[DllImport("PresentationNative_v0400.dll")]
			internal static extern void NlDestroyHyphenator(ref IntPtr hyphenator);

			// Token: 0x060084A6 RID: 33958
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			[DllImport("PresentationNative_v0400.dll", PreserveSig = false)]
			internal static extern void NlHyphenate(IntPtr hyphenator, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeParamIndex = 2)] [In] char[] inputText, int textLength, int localeID, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)] [In] byte[] hyphenBreaks, int numPositions);
		}
	}
}
