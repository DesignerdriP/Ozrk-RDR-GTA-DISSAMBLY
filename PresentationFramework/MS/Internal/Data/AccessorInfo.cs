using System;

namespace MS.Internal.Data
{
	// Token: 0x020006FF RID: 1791
	internal sealed class AccessorInfo
	{
		// Token: 0x0600733E RID: 29502 RVA: 0x00210DA6 File Offset: 0x0020EFA6
		internal AccessorInfo(object accessor, Type propertyType, object[] args)
		{
			this._accessor = accessor;
			this._propertyType = propertyType;
			this._args = args;
		}

		// Token: 0x17001B5A RID: 7002
		// (get) Token: 0x0600733F RID: 29503 RVA: 0x00210DC3 File Offset: 0x0020EFC3
		internal object Accessor
		{
			get
			{
				return this._accessor;
			}
		}

		// Token: 0x17001B5B RID: 7003
		// (get) Token: 0x06007340 RID: 29504 RVA: 0x00210DCB File Offset: 0x0020EFCB
		internal Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
		}

		// Token: 0x17001B5C RID: 7004
		// (get) Token: 0x06007341 RID: 29505 RVA: 0x00210DD3 File Offset: 0x0020EFD3
		internal object[] Args
		{
			get
			{
				return this._args;
			}
		}

		// Token: 0x17001B5D RID: 7005
		// (get) Token: 0x06007342 RID: 29506 RVA: 0x00210DDB File Offset: 0x0020EFDB
		// (set) Token: 0x06007343 RID: 29507 RVA: 0x00210DE3 File Offset: 0x0020EFE3
		internal int Generation
		{
			get
			{
				return this._generation;
			}
			set
			{
				this._generation = value;
			}
		}

		// Token: 0x0400378A RID: 14218
		private object _accessor;

		// Token: 0x0400378B RID: 14219
		private Type _propertyType;

		// Token: 0x0400378C RID: 14220
		private object[] _args;

		// Token: 0x0400378D RID: 14221
		private int _generation;
	}
}
