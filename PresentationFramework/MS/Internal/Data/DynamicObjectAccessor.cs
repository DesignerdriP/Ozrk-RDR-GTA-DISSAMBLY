using System;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000724 RID: 1828
	internal class DynamicObjectAccessor
	{
		// Token: 0x06007506 RID: 29958 RVA: 0x002178B5 File Offset: 0x00215AB5
		protected DynamicObjectAccessor(Type ownerType, string propertyName)
		{
			this._ownerType = ownerType;
			this._propertyName = propertyName;
		}

		// Token: 0x17001BD1 RID: 7121
		// (get) Token: 0x06007507 RID: 29959 RVA: 0x002178CB File Offset: 0x00215ACB
		public Type OwnerType
		{
			get
			{
				return this._ownerType;
			}
		}

		// Token: 0x17001BD2 RID: 7122
		// (get) Token: 0x06007508 RID: 29960 RVA: 0x002178D3 File Offset: 0x00215AD3
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
		}

		// Token: 0x17001BD3 RID: 7123
		// (get) Token: 0x06007509 RID: 29961 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001BD4 RID: 7124
		// (get) Token: 0x0600750A RID: 29962 RVA: 0x002178DB File Offset: 0x00215ADB
		public Type PropertyType
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x0600750B RID: 29963 RVA: 0x002178E7 File Offset: 0x00215AE7
		public static string MissingMemberErrorString(object target, string name)
		{
			return SR.Get("PropertyPathNoProperty", new object[]
			{
				target,
				"Items"
			});
		}

		// Token: 0x04003812 RID: 14354
		private Type _ownerType;

		// Token: 0x04003813 RID: 14355
		private string _propertyName;
	}
}
