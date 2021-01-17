using System;

namespace System.Windows.Baml2006
{
	// Token: 0x0200015D RID: 349
	internal class OptimizedStaticResource
	{
		// Token: 0x06000F9B RID: 3995 RVA: 0x0003C8D5 File Offset: 0x0003AAD5
		public OptimizedStaticResource(byte flags, short keyId)
		{
			this._isType = ((flags & OptimizedStaticResource.TypeExtensionValueMask) > 0);
			this._isStatic = ((flags & OptimizedStaticResource.StaticExtensionValueMask) > 0);
			this.KeyId = keyId;
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06000F9C RID: 3996 RVA: 0x0003C904 File Offset: 0x0003AB04
		// (set) Token: 0x06000F9D RID: 3997 RVA: 0x0003C90C File Offset: 0x0003AB0C
		public short KeyId { get; set; }

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06000F9E RID: 3998 RVA: 0x0003C915 File Offset: 0x0003AB15
		// (set) Token: 0x06000F9F RID: 3999 RVA: 0x0003C91D File Offset: 0x0003AB1D
		public object KeyValue { get; set; }

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x0003C926 File Offset: 0x0003AB26
		public bool IsKeyStaticExtension
		{
			get
			{
				return this._isStatic;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x0003C92E File Offset: 0x0003AB2E
		public bool IsKeyTypeExtension
		{
			get
			{
				return this._isType;
			}
		}

		// Token: 0x040011A6 RID: 4518
		private bool _isStatic;

		// Token: 0x040011A7 RID: 4519
		private bool _isType;

		// Token: 0x040011A8 RID: 4520
		private static readonly byte TypeExtensionValueMask = 1;

		// Token: 0x040011A9 RID: 4521
		private static readonly byte StaticExtensionValueMask = 2;
	}
}
