using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x0200027C RID: 636
	internal class ElementStringValueProperty : MarkupProperty
	{
		// Token: 0x06002434 RID: 9268 RVA: 0x000B0744 File Offset: 0x000AE944
		internal ElementStringValueProperty(ElementMarkupObject obj)
		{
			this._object = obj;
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06002435 RID: 9269 RVA: 0x000B0753 File Offset: 0x000AE953
		public override string Name
		{
			get
			{
				return "StringValue";
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06002436 RID: 9270 RVA: 0x000B075A File Offset: 0x000AE95A
		public override Type PropertyType
		{
			get
			{
				return typeof(string);
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06002437 RID: 9271 RVA: 0x00016748 File Offset: 0x00014948
		public override bool IsValueAsString
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06002438 RID: 9272 RVA: 0x000B0766 File Offset: 0x000AE966
		public override object Value
		{
			get
			{
				return this.StringValue;
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06002439 RID: 9273 RVA: 0x000B0770 File Offset: 0x000AE970
		public override string StringValue
		{
			get
			{
				ValueSerializer serializerFor = ValueSerializer.GetSerializerFor(this._object.ObjectType, this._object.Context);
				return serializerFor.ConvertToString(this._object.Instance, this._object.Context);
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x0600243A RID: 9274 RVA: 0x0000C238 File Offset: 0x0000A438
		public override IEnumerable<MarkupObject> Items
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x0600243B RID: 9275 RVA: 0x000B07B5 File Offset: 0x000AE9B5
		public override AttributeCollection Attributes
		{
			get
			{
				return AttributeCollection.Empty;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x0600243C RID: 9276 RVA: 0x000B07BC File Offset: 0x000AE9BC
		public override IEnumerable<Type> TypeReferences
		{
			get
			{
				ValueSerializer serializerFor = ValueSerializer.GetSerializerFor(this._object.ObjectType, this._object.Context);
				return serializerFor.TypeReferences(this._object.Instance, this._object.Context);
			}
		}

		// Token: 0x04001B2B RID: 6955
		private ElementMarkupObject _object;
	}
}
