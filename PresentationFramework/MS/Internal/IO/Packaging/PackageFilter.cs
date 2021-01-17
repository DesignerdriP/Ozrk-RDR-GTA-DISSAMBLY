using System;
using System.Collections;
using System.IO;
using System.IO.Packaging;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Win32;
using MS.Internal.Interop;
using MS.Internal.Utility;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200066C RID: 1644
	internal class PackageFilter : IFilter
	{
		// Token: 0x06006CB4 RID: 27828 RVA: 0x001F4580 File Offset: 0x001F2780
		internal PackageFilter(Package package)
		{
			if (package == null)
			{
				throw new ArgumentNullException("package");
			}
			this._package = package;
			this._partIterator = this._package.GetParts().GetEnumerator();
		}

		// Token: 0x06006CB5 RID: 27829 RVA: 0x001F461A File Offset: 0x001F281A
		public IFILTER_FLAGS Init(IFILTER_INIT grfFlags, uint cAttributes, FULLPROPSPEC[] aAttributes)
		{
			this._grfFlags = grfFlags;
			this._cAttributes = cAttributes;
			this._aAttributes = aAttributes;
			this._partIterator.Reset();
			this._progress = PackageFilter.Progress.FilteringNotStarted;
			return IFILTER_FLAGS.IFILTER_FLAGS_NONE;
		}

		// Token: 0x06006CB6 RID: 27830 RVA: 0x001F4644 File Offset: 0x001F2844
		public STAT_CHUNK GetChunk()
		{
			if (this._progress == PackageFilter.Progress.FilteringNotStarted)
			{
				this.MoveToNextFilter();
			}
			if (this._progress == PackageFilter.Progress.FilteringCompleted)
			{
				throw new COMException(SR.Get("FilterEndOfChunks"), -2147215616);
			}
			do
			{
				try
				{
					STAT_CHUNK chunk = this._currentFilter.GetChunk();
					if ((!this._isInternalFilter || chunk.idChunk != 0U) && (this._progress == PackageFilter.Progress.FilteringCoreProperties || (chunk.flags & CHUNKSTATE.CHUNK_VALUE) != CHUNKSTATE.CHUNK_VALUE))
					{
						chunk.idChunk = this.AllocateChunkID();
						chunk.idChunkSource = chunk.idChunk;
						if (this._firstChunkFromFilter)
						{
							chunk.breakType = CHUNK_BREAKTYPE.CHUNK_EOP;
							this._firstChunkFromFilter = false;
						}
						return chunk;
					}
				}
				catch (COMException)
				{
				}
				catch (IOException ex)
				{
					if (this._isInternalFilter)
					{
						throw ex;
					}
				}
				this.MoveToNextFilter();
			}
			while (this._progress != PackageFilter.Progress.FilteringCompleted);
			throw new COMException(SR.Get("FilterEndOfChunks"), -2147215616);
		}

		// Token: 0x06006CB7 RID: 27831 RVA: 0x001F473C File Offset: 0x001F293C
		public void GetText(ref uint bufferCharacterCount, IntPtr pBuffer)
		{
			if (this._progress != PackageFilter.Progress.FilteringContent)
			{
				throw new COMException(SR.Get("FilterGetTextNotSupported"), -2147215611);
			}
			this._currentFilter.GetText(ref bufferCharacterCount, pBuffer);
		}

		// Token: 0x06006CB8 RID: 27832 RVA: 0x001F4769 File Offset: 0x001F2969
		public IntPtr GetValue()
		{
			if (this._progress != PackageFilter.Progress.FilteringCoreProperties)
			{
				throw new COMException(SR.Get("FilterGetValueNotSupported"), -2147215610);
			}
			return this._currentFilter.GetValue();
		}

		// Token: 0x06006CB9 RID: 27833 RVA: 0x001F207F File Offset: 0x001F027F
		public IntPtr BindRegion(FILTERREGION origPos, ref Guid riid)
		{
			throw new NotImplementedException(SR.Get("FilterBindRegionNotImplemented"));
		}

		// Token: 0x06006CBA RID: 27834 RVA: 0x001F4794 File Offset: 0x001F2994
		private IFilter GetFilterFromClsid(Guid clsid)
		{
			Type typeFromCLSID = Type.GetTypeFromCLSID(clsid);
			IFilter result;
			try
			{
				result = (IFilter)Activator.CreateInstance(typeFromCLSID);
			}
			catch (InvalidCastException)
			{
				return null;
			}
			catch (COMException)
			{
				return null;
			}
			return result;
		}

		// Token: 0x06006CBB RID: 27835 RVA: 0x001F47E0 File Offset: 0x001F29E0
		private void MoveToNextFilter()
		{
			this._isInternalFilter = false;
			switch (this._progress)
			{
			case PackageFilter.Progress.FilteringNotStarted:
				this._currentFilter = new IndexingFilterMarshaler(new CorePropertiesFilter(this._package.PackageProperties))
				{
					ThrowOnEndOfChunks = false
				};
				this._currentFilter.Init(this._grfFlags, this._cAttributes, this._aAttributes);
				this._isInternalFilter = true;
				this._progress = PackageFilter.Progress.FilteringCoreProperties;
				return;
			case PackageFilter.Progress.FilteringCoreProperties:
			case PackageFilter.Progress.FilteringContent:
				if (this._currentStream != null)
				{
					this._currentStream.Close();
					this._currentStream = null;
				}
				this._currentFilter = null;
				while (this._partIterator.MoveNext())
				{
					PackagePart packagePart = (PackagePart)this._partIterator.Current;
					ContentType validatedContentType = packagePart.ValidatedContentType;
					string filterClsid = this.GetFilterClsid(validatedContentType, packagePart.Uri);
					if (filterClsid != null)
					{
						this._currentFilter = this.GetFilterFromClsid(new Guid(filterClsid));
						if (this._currentFilter != null)
						{
							this._currentStream = packagePart.GetStream();
							ManagedIStream pstm = new ManagedIStream(this._currentStream);
							try
							{
								IPersistStreamWithArrays persistStreamWithArrays = (IPersistStreamWithArrays)this._currentFilter;
								persistStreamWithArrays.Load(pstm);
								this._currentFilter.Init(this._grfFlags, this._cAttributes, this._aAttributes);
								break;
							}
							catch (InvalidCastException)
							{
							}
							catch (COMException)
							{
							}
							catch (IOException)
							{
							}
						}
					}
					if (BindUriHelper.IsXamlMimeType(validatedContentType))
					{
						if (this._currentStream == null)
						{
							this._currentStream = packagePart.GetStream();
						}
						this._currentFilter = new IndexingFilterMarshaler(new XamlFilter(this._currentStream))
						{
							ThrowOnEndOfChunks = false
						};
						this._currentFilter.Init(this._grfFlags, this._cAttributes, this._aAttributes);
						this._isInternalFilter = true;
						break;
					}
					if (this._currentStream != null)
					{
						this._currentStream.Close();
						this._currentStream = null;
					}
					this._currentFilter = null;
				}
				if (this._currentFilter == null)
				{
					this._progress = PackageFilter.Progress.FilteringCompleted;
					return;
				}
				this._firstChunkFromFilter = true;
				this._progress = PackageFilter.Progress.FilteringContent;
				break;
			case PackageFilter.Progress.FilteringCompleted:
				break;
			default:
				return;
			}
		}

		// Token: 0x06006CBC RID: 27836 RVA: 0x001F4A08 File Offset: 0x001F2C08
		private uint AllocateChunkID()
		{
			Invariant.Assert(this._currentChunkID <= uint.MaxValue);
			this._currentChunkID += 1U;
			return this._currentChunkID;
		}

		// Token: 0x06006CBD RID: 27837 RVA: 0x001F4A30 File Offset: 0x001F2C30
		private string GetFilterClsid(ContentType contentType, Uri partUri)
		{
			string text = null;
			if (contentType != null && !ContentType.Empty.AreTypeAndSubTypeEqual(contentType))
			{
				text = this.FileTypeGuidFromMimeType(contentType);
			}
			else
			{
				string partExtension = this.GetPartExtension(partUri);
				if (partExtension != null)
				{
					text = this.FileTypeGuidFromFileExtension(partExtension);
				}
			}
			if (text == null)
			{
				return null;
			}
			RegistryKey registryKey = PackageFilter.FindSubkey(Registry.ClassesRoot, PackageFilter.MakeRegistryPath(this._IFilterAddinPath, new string[]
			{
				text
			}));
			if (registryKey == null)
			{
				return null;
			}
			return (string)registryKey.GetValue(null);
		}

		// Token: 0x06006CBE RID: 27838 RVA: 0x001F4AA4 File Offset: 0x001F2CA4
		private static RegistryKey FindSubkey(RegistryKey containingKey, string[] keyPath)
		{
			RegistryKey registryKey = containingKey;
			for (int i = 0; i < keyPath.Length; i++)
			{
				if (registryKey == null)
				{
					return null;
				}
				registryKey = registryKey.OpenSubKey(keyPath[i]);
			}
			return registryKey;
		}

		// Token: 0x06006CBF RID: 27839 RVA: 0x001F4AD4 File Offset: 0x001F2CD4
		private string FileTypeGuidFromMimeType(ContentType contentType)
		{
			RegistryKey registryKey = PackageFilter.FindSubkey(Registry.ClassesRoot, this._mimeContentTypeKey);
			RegistryKey registryKey2 = (registryKey == null) ? null : registryKey.OpenSubKey(contentType.ToString());
			if (registryKey2 == null)
			{
				return null;
			}
			string text = (string)registryKey2.GetValue("Extension");
			if (text == null)
			{
				return null;
			}
			return this.FileTypeGuidFromFileExtension(text);
		}

		// Token: 0x06006CC0 RID: 27840 RVA: 0x001F4B28 File Offset: 0x001F2D28
		private string FileTypeGuidFromFileExtension(string dottedExtensionName)
		{
			RegistryKey registryKey = PackageFilter.FindSubkey(Registry.ClassesRoot, PackageFilter.MakeRegistryPath(this._persistentHandlerKey, new string[]
			{
				dottedExtensionName
			}));
			if (registryKey != null)
			{
				return (string)registryKey.GetValue(null);
			}
			return null;
		}

		// Token: 0x06006CC1 RID: 27841 RVA: 0x001F4B68 File Offset: 0x001F2D68
		private string GetPartExtension(Uri partUri)
		{
			Invariant.Assert(partUri != null);
			string stringForPartUri = PackUriHelper.GetStringForPartUri(partUri);
			string extension = Path.GetExtension(stringForPartUri);
			if (extension == string.Empty)
			{
				return null;
			}
			return extension;
		}

		// Token: 0x06006CC2 RID: 27842 RVA: 0x001F4BA0 File Offset: 0x001F2DA0
		private static string[] MakeRegistryPath(string[] pathWithGaps, params string[] stopGaps)
		{
			string[] array = (string[])pathWithGaps.Clone();
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null)
				{
					array[i] = stopGaps[num];
					num++;
				}
			}
			return array;
		}

		// Token: 0x0400355D RID: 13661
		private readonly string[] _IFilterAddinPath = new string[]
		{
			"CLSID",
			null,
			"PersistentAddinsRegistered",
			"{89BCB740-6119-101A-BCB7-00DD010655AF}"
		};

		// Token: 0x0400355E RID: 13662
		private readonly string[] _mimeContentTypeKey = new string[]
		{
			"MIME",
			"Database",
			"Content Type"
		};

		// Token: 0x0400355F RID: 13663
		private readonly string[] _persistentHandlerKey = new string[]
		{
			null,
			"PersistentHandler"
		};

		// Token: 0x04003560 RID: 13664
		private Package _package;

		// Token: 0x04003561 RID: 13665
		private uint _currentChunkID;

		// Token: 0x04003562 RID: 13666
		private IEnumerator _partIterator;

		// Token: 0x04003563 RID: 13667
		private IFilter _currentFilter;

		// Token: 0x04003564 RID: 13668
		private Stream _currentStream;

		// Token: 0x04003565 RID: 13669
		private bool _firstChunkFromFilter;

		// Token: 0x04003566 RID: 13670
		private PackageFilter.Progress _progress;

		// Token: 0x04003567 RID: 13671
		private bool _isInternalFilter;

		// Token: 0x04003568 RID: 13672
		private IFILTER_INIT _grfFlags;

		// Token: 0x04003569 RID: 13673
		private uint _cAttributes;

		// Token: 0x0400356A RID: 13674
		private FULLPROPSPEC[] _aAttributes;

		// Token: 0x0400356B RID: 13675
		private const string _extension = "Extension";

		// Token: 0x02000B1D RID: 2845
		private enum Progress
		{
			// Token: 0x04004A4C RID: 19020
			FilteringNotStarted,
			// Token: 0x04004A4D RID: 19021
			FilteringCoreProperties,
			// Token: 0x04004A4E RID: 19022
			FilteringContent,
			// Token: 0x04004A4F RID: 19023
			FilteringCompleted
		}
	}
}
