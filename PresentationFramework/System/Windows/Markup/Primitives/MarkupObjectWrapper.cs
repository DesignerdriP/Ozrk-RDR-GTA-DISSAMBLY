using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000283 RID: 643
	internal class MarkupObjectWrapper : MarkupObject
	{
		// Token: 0x0600245B RID: 9307 RVA: 0x000B0991 File Offset: 0x000AEB91
		public MarkupObjectWrapper(MarkupObject baseObject)
		{
			this._baseObject = baseObject;
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x000B09A0 File Offset: 0x000AEBA0
		public override void AssignRootContext(IValueSerializerContext context)
		{
			this._baseObject.AssignRootContext(context);
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x0600245D RID: 9309 RVA: 0x000B09AE File Offset: 0x000AEBAE
		public override AttributeCollection Attributes
		{
			get
			{
				return this._baseObject.Attributes;
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x0600245E RID: 9310 RVA: 0x000B09BB File Offset: 0x000AEBBB
		public override Type ObjectType
		{
			get
			{
				return this._baseObject.ObjectType;
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x0600245F RID: 9311 RVA: 0x000B09C8 File Offset: 0x000AEBC8
		public override object Instance
		{
			get
			{
				return this._baseObject.Instance;
			}
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x000B09D5 File Offset: 0x000AEBD5
		internal override IEnumerable<MarkupProperty> GetProperties(bool mapToConstructorArgs)
		{
			return this._baseObject.GetProperties(mapToConstructorArgs);
		}

		// Token: 0x04001B31 RID: 6961
		private MarkupObject _baseObject;
	}
}
