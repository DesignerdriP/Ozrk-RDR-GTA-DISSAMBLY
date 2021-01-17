using System;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x0200027A RID: 634
	internal abstract class ElementObjectPropertyBase : ElementPropertyBase
	{
		// Token: 0x06002427 RID: 9255 RVA: 0x000B0469 File Offset: 0x000AE669
		protected ElementObjectPropertyBase(ElementMarkupObject obj) : base(obj.Manager)
		{
			this._object = obj;
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x000B047E File Offset: 0x000AE67E
		protected override IValueSerializerContext GetItemContext()
		{
			return this._object.Context;
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x000B048B File Offset: 0x000AE68B
		protected override Type GetObjectType()
		{
			return this._object.ObjectType;
		}

		// Token: 0x04001B26 RID: 6950
		protected readonly ElementMarkupObject _object;
	}
}
