using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000663 RID: 1635
	internal class ManagedPropSpec
	{
		// Token: 0x170019E2 RID: 6626
		// (get) Token: 0x06006C5C RID: 27740 RVA: 0x001F37E3 File Offset: 0x001F19E3
		internal PropSpecType PropType
		{
			get
			{
				return this._propType;
			}
		}

		// Token: 0x170019E3 RID: 6627
		// (get) Token: 0x06006C5D RID: 27741 RVA: 0x001F37EB File Offset: 0x001F19EB
		// (set) Token: 0x06006C5E RID: 27742 RVA: 0x001F37F3 File Offset: 0x001F19F3
		internal string PropName
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._name = value;
				this._id = 0U;
				this._propType = PropSpecType.Name;
			}
		}

		// Token: 0x170019E4 RID: 6628
		// (get) Token: 0x06006C5F RID: 27743 RVA: 0x001F3818 File Offset: 0x001F1A18
		// (set) Token: 0x06006C60 RID: 27744 RVA: 0x001F3820 File Offset: 0x001F1A20
		internal uint PropId
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
				this._name = null;
				this._propType = PropSpecType.Id;
			}
		}

		// Token: 0x06006C61 RID: 27745 RVA: 0x001F3837 File Offset: 0x001F1A37
		internal ManagedPropSpec(uint id)
		{
			this.PropId = id;
		}

		// Token: 0x06006C62 RID: 27746 RVA: 0x001F3848 File Offset: 0x001F1A48
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal ManagedPropSpec(PROPSPEC propSpec)
		{
			SecurityHelper.DemandUnmanagedCode();
			PropSpecType propType = (PropSpecType)propSpec.propType;
			if (propType == PropSpecType.Name)
			{
				this.PropName = Marshal.PtrToStringUni(propSpec.union.name);
				return;
			}
			if (propType == PropSpecType.Id)
			{
				this.PropId = propSpec.union.propId;
				return;
			}
			throw new ArgumentException(SR.Get("FilterPropSpecUnknownUnionSelector"), "propSpec");
		}

		// Token: 0x04003538 RID: 13624
		private PropSpecType _propType;

		// Token: 0x04003539 RID: 13625
		private uint _id;

		// Token: 0x0400353A RID: 13626
		private string _name;
	}
}
