using System;
using System.Globalization;
using System.IO.Packaging;
using System.Runtime.InteropServices;
using System.Windows;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200066E RID: 1646
	internal class CorePropertiesFilter : IManagedFilter
	{
		// Token: 0x06006CC9 RID: 27849 RVA: 0x001F4C45 File Offset: 0x001F2E45
		internal CorePropertiesFilter(PackageProperties coreProperties)
		{
			if (coreProperties == null)
			{
				throw new ArgumentNullException("coreProperties");
			}
			this._coreProperties = coreProperties;
		}

		// Token: 0x06006CCA RID: 27850 RVA: 0x001F4C62 File Offset: 0x001F2E62
		public IFILTER_FLAGS Init(IFILTER_INIT grfFlags, ManagedFullPropSpec[] aAttributes)
		{
			this._grfFlags = grfFlags;
			this._aAttributes = aAttributes;
			this._corePropertyEnumerator = new CorePropertyEnumerator(this._coreProperties, this._grfFlags, this._aAttributes);
			return IFILTER_FLAGS.IFILTER_FLAGS_NONE;
		}

		// Token: 0x06006CCB RID: 27851 RVA: 0x001F4C90 File Offset: 0x001F2E90
		public ManagedChunk GetChunk()
		{
			this._pendingGetValue = false;
			if (!this.CorePropertyEnumerator.MoveNext())
			{
				return null;
			}
			ManagedChunk result = new CorePropertiesFilter.PropertyChunk(this.AllocateChunkID(), this.CorePropertyEnumerator.CurrentGuid, this.CorePropertyEnumerator.CurrentPropId);
			this._pendingGetValue = true;
			return result;
		}

		// Token: 0x06006CCC RID: 27852 RVA: 0x001F4C22 File Offset: 0x001F2E22
		public string GetText(int bufferCharacterCount)
		{
			throw new COMException(SR.Get("FilterGetTextNotSupported"), -2147215611);
		}

		// Token: 0x06006CCD RID: 27853 RVA: 0x001F4CDD File Offset: 0x001F2EDD
		public object GetValue()
		{
			if (!this._pendingGetValue)
			{
				throw new COMException(SR.Get("FilterGetValueAlreadyCalledOnCurrentChunk"), -2147215614);
			}
			this._pendingGetValue = false;
			return this.CorePropertyEnumerator.CurrentValue;
		}

		// Token: 0x06006CCE RID: 27854 RVA: 0x001F4D0E File Offset: 0x001F2F0E
		private uint AllocateChunkID()
		{
			if (this._chunkID == 4294967295U)
			{
				this._chunkID = 1U;
			}
			else
			{
				this._chunkID += 1U;
			}
			return this._chunkID;
		}

		// Token: 0x170019FF RID: 6655
		// (get) Token: 0x06006CCF RID: 27855 RVA: 0x001F4D36 File Offset: 0x001F2F36
		private CorePropertyEnumerator CorePropertyEnumerator
		{
			get
			{
				if (this._corePropertyEnumerator == null)
				{
					this._corePropertyEnumerator = new CorePropertyEnumerator(this._coreProperties, this._grfFlags, this._aAttributes);
				}
				return this._corePropertyEnumerator;
			}
		}

		// Token: 0x0400356D RID: 13677
		private IFILTER_INIT _grfFlags;

		// Token: 0x0400356E RID: 13678
		private ManagedFullPropSpec[] _aAttributes;

		// Token: 0x0400356F RID: 13679
		private uint _chunkID;

		// Token: 0x04003570 RID: 13680
		private bool _pendingGetValue;

		// Token: 0x04003571 RID: 13681
		private CorePropertyEnumerator _corePropertyEnumerator;

		// Token: 0x04003572 RID: 13682
		private PackageProperties _coreProperties;

		// Token: 0x02000B1E RID: 2846
		private class PropertyChunk : ManagedChunk
		{
			// Token: 0x06008D23 RID: 36131 RVA: 0x00258AAE File Offset: 0x00256CAE
			internal PropertyChunk(uint chunkId, Guid guid, uint propId) : base(chunkId, CHUNK_BREAKTYPE.CHUNK_EOS, new ManagedFullPropSpec(guid, propId), (uint)CultureInfo.InvariantCulture.LCID, CHUNKSTATE.CHUNK_VALUE)
			{
			}
		}
	}
}
