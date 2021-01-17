using System;
using System.IO.Packaging;
using System.Windows;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200066F RID: 1647
	internal class CorePropertyEnumerator
	{
		// Token: 0x06006CD0 RID: 27856 RVA: 0x001F4D64 File Offset: 0x001F2F64
		internal CorePropertyEnumerator(PackageProperties coreProperties, IFILTER_INIT grfFlags, ManagedFullPropSpec[] attributes)
		{
			if (attributes != null && attributes.Length != 0)
			{
				this._attributes = attributes;
			}
			else if ((grfFlags & IFILTER_INIT.IFILTER_INIT_APPLY_INDEX_ATTRIBUTES) == IFILTER_INIT.IFILTER_INIT_APPLY_INDEX_ATTRIBUTES)
			{
				this._attributes = new ManagedFullPropSpec[]
				{
					new ManagedFullPropSpec(FormatId.SummaryInformation, 2U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 3U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 4U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 5U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 6U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 8U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 9U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 11U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 12U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 13U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 2U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 18U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 26U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 27U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 28U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 29U)
				};
			}
			this._coreProperties = coreProperties;
			this._currentIndex = -1;
		}

		// Token: 0x06006CD1 RID: 27857 RVA: 0x001F4EA0 File Offset: 0x001F30A0
		internal bool MoveNext()
		{
			if (this._attributes == null)
			{
				return false;
			}
			this._currentIndex++;
			while (this._currentIndex < this._attributes.Length)
			{
				if (this._attributes[this._currentIndex].Property.PropType == PropSpecType.Id && this.CurrentValue != null)
				{
					return true;
				}
				this._currentIndex++;
			}
			return false;
		}

		// Token: 0x17001A00 RID: 6656
		// (get) Token: 0x06006CD2 RID: 27858 RVA: 0x001F4F0A File Offset: 0x001F310A
		internal Guid CurrentGuid
		{
			get
			{
				this.ValidateCurrent();
				return this._attributes[this._currentIndex].Guid;
			}
		}

		// Token: 0x17001A01 RID: 6657
		// (get) Token: 0x06006CD3 RID: 27859 RVA: 0x001F4F24 File Offset: 0x001F3124
		internal uint CurrentPropId
		{
			get
			{
				this.ValidateCurrent();
				return this._attributes[this._currentIndex].Property.PropId;
			}
		}

		// Token: 0x17001A02 RID: 6658
		// (get) Token: 0x06006CD4 RID: 27860 RVA: 0x001F4F43 File Offset: 0x001F3143
		internal object CurrentValue
		{
			get
			{
				this.ValidateCurrent();
				return this.GetValue(this.CurrentGuid, this.CurrentPropId);
			}
		}

		// Token: 0x06006CD5 RID: 27861 RVA: 0x001F4F5D File Offset: 0x001F315D
		private void ValidateCurrent()
		{
			if (this._currentIndex < 0 || this._currentIndex >= this._attributes.Length)
			{
				throw new InvalidOperationException(SR.Get("CorePropertyEnumeratorPositionedOutOfBounds"));
			}
		}

		// Token: 0x06006CD6 RID: 27862 RVA: 0x001F4F88 File Offset: 0x001F3188
		private object GetValue(Guid guid, uint propId)
		{
			if (guid == FormatId.SummaryInformation)
			{
				switch (propId)
				{
				case 2U:
					return this._coreProperties.Title;
				case 3U:
					return this._coreProperties.Subject;
				case 4U:
					return this._coreProperties.Creator;
				case 5U:
					return this._coreProperties.Keywords;
				case 6U:
					return this._coreProperties.Description;
				case 8U:
					return this._coreProperties.LastModifiedBy;
				case 9U:
					return this._coreProperties.Revision;
				case 11U:
					if (this._coreProperties.LastPrinted != null)
					{
						return this._coreProperties.LastPrinted.Value;
					}
					return null;
				case 12U:
					if (this._coreProperties.Created != null)
					{
						return this._coreProperties.Created.Value;
					}
					return null;
				case 13U:
					if (this._coreProperties.Modified != null)
					{
						return this._coreProperties.Modified.Value;
					}
					return null;
				}
			}
			else if (guid == FormatId.DocumentSummaryInformation)
			{
				if (propId == 2U)
				{
					return this._coreProperties.Category;
				}
				if (propId == 18U)
				{
					return this._coreProperties.Identifier;
				}
				switch (propId)
				{
				case 26U:
					return this._coreProperties.ContentType;
				case 27U:
					return this._coreProperties.Language;
				case 28U:
					return this._coreProperties.Version;
				case 29U:
					return this._coreProperties.ContentStatus;
				}
			}
			return null;
		}

		// Token: 0x04003573 RID: 13683
		private PackageProperties _coreProperties;

		// Token: 0x04003574 RID: 13684
		private ManagedFullPropSpec[] _attributes;

		// Token: 0x04003575 RID: 13685
		private int _currentIndex;
	}
}
