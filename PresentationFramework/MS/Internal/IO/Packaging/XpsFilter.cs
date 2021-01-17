using System;
using System.IO;
using System.IO.Packaging;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200066B RID: 1643
	[ComVisible(true)]
	[Guid("0B8732A6-AF74-498c-A251-9DC86B0538B0")]
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class XpsFilter : IFilter, IPersistFile, IPersistStream
	{
		// Token: 0x06006CA0 RID: 27808 RVA: 0x001F3FA0 File Offset: 0x001F21A0
		IFILTER_FLAGS IFilter.Init([In] IFILTER_INIT grfFlags, [In] uint cAttributes, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] FULLPROPSPEC[] aAttributes)
		{
			if (this._filter == null)
			{
				throw new COMException(SR.Get("FileToFilterNotLoaded"), -2147467259);
			}
			if (cAttributes > 0U && aAttributes == null)
			{
				throw new COMException(SR.Get("FilterInitInvalidAttributes"), -2147024809);
			}
			return this._filter.Init(grfFlags, cAttributes, aAttributes);
		}

		// Token: 0x06006CA1 RID: 27809 RVA: 0x001F3FF4 File Offset: 0x001F21F4
		STAT_CHUNK IFilter.GetChunk()
		{
			if (this._filter == null)
			{
				throw new COMException(SR.Get("FileToFilterNotLoaded"), -2147215613);
			}
			STAT_CHUNK chunk;
			try
			{
				chunk = this._filter.GetChunk();
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147215616)
				{
					this.ReleaseResources();
				}
				throw ex;
			}
			return chunk;
		}

		// Token: 0x06006CA2 RID: 27810 RVA: 0x001F4054 File Offset: 0x001F2254
		[SecurityCritical]
		void IFilter.GetText(ref uint bufCharacterCount, IntPtr pBuffer)
		{
			if (this._filter == null)
			{
				throw new COMException(SR.Get("FileToFilterNotLoaded"), -2147215613);
			}
			if (pBuffer == IntPtr.Zero)
			{
				throw new NullReferenceException(SR.Get("FilterNullGetTextBufferPointer"));
			}
			if (bufCharacterCount == 0U)
			{
				return;
			}
			if (bufCharacterCount == 1U)
			{
				Marshal.WriteInt16(pBuffer, 0);
				return;
			}
			uint num = bufCharacterCount;
			if (bufCharacterCount > 4096U)
			{
				bufCharacterCount = 4096U;
			}
			uint num2 = bufCharacterCount - 1U;
			bufCharacterCount = num2;
			uint num3 = num2;
			this._filter.GetText(ref bufCharacterCount, pBuffer);
			if (bufCharacterCount > num3)
			{
				throw new COMException(SR.Get("AuxiliaryFilterReturnedAnomalousCountOfCharacters"), -2147215613);
			}
			if (num == 2U && Marshal.ReadInt16(pBuffer) == 0)
			{
				bufCharacterCount = 2U;
				this._filter.GetText(ref bufCharacterCount, pBuffer);
				if (bufCharacterCount > 2U)
				{
					throw new COMException(SR.Get("AuxiliaryFilterReturnedAnomalousCountOfCharacters"), -2147215613);
				}
				if (bufCharacterCount == 2U)
				{
					short num4 = Marshal.ReadInt16(pBuffer, 2);
					Invariant.Assert(num4 == 0);
					bufCharacterCount = 1U;
				}
			}
			Marshal.WriteInt16(pBuffer, (int)(bufCharacterCount * 2U), 0);
			bufCharacterCount += 1U;
		}

		// Token: 0x06006CA3 RID: 27811 RVA: 0x001F4153 File Offset: 0x001F2353
		IntPtr IFilter.GetValue()
		{
			if (this._filter == null)
			{
				throw new COMException(SR.Get("FileToFilterNotLoaded"), -2147215613);
			}
			return this._filter.GetValue();
		}

		// Token: 0x06006CA4 RID: 27812 RVA: 0x001F207F File Offset: 0x001F027F
		IntPtr IFilter.BindRegion([In] FILTERREGION origPos, [In] ref Guid riid)
		{
			throw new NotImplementedException(SR.Get("FilterBindRegionNotImplemented"));
		}

		// Token: 0x06006CA5 RID: 27813 RVA: 0x001F417D File Offset: 0x001F237D
		void IPersistFile.GetClassID(out Guid pClassID)
		{
			pClassID = XpsFilter._filterClsid;
		}

		// Token: 0x06006CA6 RID: 27814 RVA: 0x001F418A File Offset: 0x001F238A
		[PreserveSig]
		int IPersistFile.GetCurFile(out string ppszFileName)
		{
			ppszFileName = null;
			if (this._filter == null || this._xpsFileName == null)
			{
				ppszFileName = "*.xps";
				return 1;
			}
			ppszFileName = this._xpsFileName;
			return 0;
		}

		// Token: 0x06006CA7 RID: 27815 RVA: 0x00016748 File Offset: 0x00014948
		[PreserveSig]
		int IPersistFile.IsDirty()
		{
			return 1;
		}

		// Token: 0x06006CA8 RID: 27816 RVA: 0x001F41B4 File Offset: 0x001F23B4
		void IPersistFile.Load(string pszFileName, int dwMode)
		{
			if (pszFileName == null || pszFileName == string.Empty)
			{
				throw new ArgumentException(SR.Get("FileNameNullOrEmpty"), "pszFileName");
			}
			STGM_FLAGS stgm_FLAGS = (STGM_FLAGS)(dwMode & 4096);
			if (stgm_FLAGS == STGM_FLAGS.CREATE)
			{
				throw new ArgumentException(SR.Get("FilterLoadInvalidModeFlag"), "dwMode");
			}
			FileMode fileMode = FileMode.Open;
			stgm_FLAGS = (STGM_FLAGS)(dwMode & 3);
			if (stgm_FLAGS == STGM_FLAGS.READ || stgm_FLAGS == STGM_FLAGS.READWRITE)
			{
				FileAccess fileAccess = FileAccess.Read;
				FileShare fileSharing = FileShare.ReadWrite;
				Invariant.Assert(this._package == null || this._encryptedPackage == null);
				this.ReleaseResources();
				this._filter = null;
				this._xpsFileName = null;
				bool flag = EncryptedPackageEnvelope.IsEncryptedPackageEnvelope(pszFileName);
				try
				{
					this._packageStream = XpsFilter.FileToStream(pszFileName, fileMode, fileAccess, fileSharing, 1048576L);
					if (flag)
					{
						this._encryptedPackage = EncryptedPackageEnvelope.Open(this._packageStream);
						this._filter = new EncryptedPackageFilter(this._encryptedPackage);
					}
					else
					{
						this._package = Package.Open(this._packageStream);
						this._filter = new PackageFilter(this._package);
					}
				}
				catch (IOException ex)
				{
					throw new COMException(ex.Message, -2147215613);
				}
				catch (FileFormatException ex2)
				{
					throw new COMException(ex2.Message, -2147215604);
				}
				finally
				{
					if (this._filter == null)
					{
						this.ReleaseResources();
					}
				}
				this._xpsFileName = pszFileName;
				return;
			}
			throw new ArgumentException(SR.Get("FilterLoadInvalidModeFlag"), "dwMode");
		}

		// Token: 0x06006CA9 RID: 27817 RVA: 0x001F4334 File Offset: 0x001F2534
		void IPersistFile.Save(string pszFileName, bool fRemember)
		{
			throw new COMException(SR.Get("FilterIPersistFileIsReadOnly"), -2147286781);
		}

		// Token: 0x06006CAA RID: 27818 RVA: 0x00002137 File Offset: 0x00000337
		void IPersistFile.SaveCompleted(string pszFileName)
		{
		}

		// Token: 0x06006CAB RID: 27819 RVA: 0x001F417D File Offset: 0x001F237D
		void IPersistStream.GetClassID(out Guid pClassID)
		{
			pClassID = XpsFilter._filterClsid;
		}

		// Token: 0x06006CAC RID: 27820 RVA: 0x00016748 File Offset: 0x00014948
		[PreserveSig]
		int IPersistStream.IsDirty()
		{
			return 1;
		}

		// Token: 0x06006CAD RID: 27821 RVA: 0x001F434C File Offset: 0x001F254C
		[SecurityCritical]
		void IPersistStream.Load(IStream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			Invariant.Assert(this._package == null || this._encryptedPackage == null);
			this.ReleaseResources();
			this._filter = null;
			this._xpsFileName = null;
			try
			{
				this._packageStream = new UnsafeIndexingFilterStream(stream);
				if (EncryptedPackageEnvelope.IsEncryptedPackageEnvelope(this._packageStream))
				{
					this._encryptedPackage = EncryptedPackageEnvelope.Open(this._packageStream);
					this._filter = new EncryptedPackageFilter(this._encryptedPackage);
				}
				else
				{
					this._package = Package.Open(this._packageStream);
					this._filter = new PackageFilter(this._package);
				}
			}
			catch (IOException ex)
			{
				throw new COMException(ex.Message, -2147215613);
			}
			catch (Exception ex2)
			{
				throw new COMException(ex2.Message, -2147215604);
			}
			finally
			{
				if (this._filter == null)
				{
					this.ReleaseResources();
				}
			}
		}

		// Token: 0x06006CAE RID: 27822 RVA: 0x001F4450 File Offset: 0x001F2650
		void IPersistStream.Save(IStream stream, bool fClearDirty)
		{
			throw new COMException(SR.Get("FilterIPersistStreamIsReadOnly"), -2147286781);
		}

		// Token: 0x06006CAF RID: 27823 RVA: 0x001F4466 File Offset: 0x001F2666
		void IPersistStream.GetSizeMax(out long pcbSize)
		{
			throw new NotSupportedException(SR.Get("FilterIPersistFileIsReadOnly"));
		}

		// Token: 0x06006CB0 RID: 27824 RVA: 0x001F4478 File Offset: 0x001F2678
		private void ReleaseResources()
		{
			if (this._encryptedPackage != null)
			{
				this._encryptedPackage.Close();
				this._encryptedPackage = null;
			}
			else if (this._package != null)
			{
				this._package.Close();
				this._package = null;
			}
			if (this._packageStream != null)
			{
				this._packageStream.Close();
				this._packageStream = null;
			}
		}

		// Token: 0x06006CB1 RID: 27825 RVA: 0x001F44D8 File Offset: 0x001F26D8
		private static Stream FileToStream(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileSharing, long maxMemoryStream)
		{
			FileInfo fileInfo = new FileInfo(filePath);
			long length = fileInfo.Length;
			Stream stream = new FileStream(filePath, fileMode, fileAccess, fileSharing);
			if (length < maxMemoryStream)
			{
				MemoryStream memoryStream = new MemoryStream((int)length);
				using (stream)
				{
					PackagingUtilities.CopyStream(stream, memoryStream, length, 4096);
				}
				stream = memoryStream;
			}
			return stream;
		}

		// Token: 0x04003554 RID: 13652
		[ComVisible(false)]
		private static readonly Guid _filterClsid = new Guid(193409702U, 44916, 18828, 162, 81, 157, 200, 107, 5, 56, 176);

		// Token: 0x04003555 RID: 13653
		[ComVisible(false)]
		private IFilter _filter;

		// Token: 0x04003556 RID: 13654
		[ComVisible(false)]
		private Package _package;

		// Token: 0x04003557 RID: 13655
		[ComVisible(false)]
		private EncryptedPackageEnvelope _encryptedPackage;

		// Token: 0x04003558 RID: 13656
		[ComVisible(false)]
		private string _xpsFileName;

		// Token: 0x04003559 RID: 13657
		[ComVisible(false)]
		private Stream _packageStream;

		// Token: 0x0400355A RID: 13658
		[ComVisible(false)]
		private const int _int16Size = 2;

		// Token: 0x0400355B RID: 13659
		[ComVisible(false)]
		private const uint _maxTextBufferSizeInCharacters = 4096U;

		// Token: 0x0400355C RID: 13660
		[ComVisible(false)]
		private const int _maxMemoryStreamBuffer = 1048576;
	}
}
