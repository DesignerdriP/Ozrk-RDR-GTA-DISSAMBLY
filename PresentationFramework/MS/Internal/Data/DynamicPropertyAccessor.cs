using System;

namespace MS.Internal.Data
{
	// Token: 0x02000725 RID: 1829
	internal abstract class DynamicPropertyAccessor : DynamicObjectAccessor
	{
		// Token: 0x0600750C RID: 29964 RVA: 0x00217905 File Offset: 0x00215B05
		protected DynamicPropertyAccessor(Type ownerType, string propertyName) : base(ownerType, propertyName)
		{
		}

		// Token: 0x0600750D RID: 29965
		public abstract object GetValue(object component);

		// Token: 0x0600750E RID: 29966
		public abstract void SetValue(object component, object value);
	}
}
