using System;

namespace MS.Internal.Data
{
	// Token: 0x02000726 RID: 1830
	internal abstract class DynamicIndexerAccessor : DynamicObjectAccessor
	{
		// Token: 0x0600750F RID: 29967 RVA: 0x00217905 File Offset: 0x00215B05
		protected DynamicIndexerAccessor(Type ownerType, string propertyName) : base(ownerType, propertyName)
		{
		}

		// Token: 0x06007510 RID: 29968
		public abstract object GetValue(object component, object[] args);

		// Token: 0x06007511 RID: 29969
		public abstract void SetValue(object component, object[] args, object value);
	}
}
