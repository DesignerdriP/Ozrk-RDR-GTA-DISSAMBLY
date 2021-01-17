using System;
using System.Collections.Generic;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000285 RID: 645
	internal class ExtensionSimplifierMarkupObject : MarkupObjectWrapper
	{
		// Token: 0x06002472 RID: 9330 RVA: 0x000B0AC2 File Offset: 0x000AECC2
		public ExtensionSimplifierMarkupObject(MarkupObject baseObject, IValueSerializerContext context) : base(baseObject)
		{
			this._context = context;
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x000B0AD2 File Offset: 0x000AECD2
		private IEnumerable<MarkupProperty> GetBaseProperties(bool mapToConstructorArgs)
		{
			return base.GetProperties(mapToConstructorArgs);
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x000B0ADB File Offset: 0x000AECDB
		internal override IEnumerable<MarkupProperty> GetProperties(bool mapToConstructorArgs)
		{
			foreach (MarkupProperty baseProperty in this.GetBaseProperties(mapToConstructorArgs))
			{
				yield return new ExtensionSimplifierProperty(baseProperty, this._context);
			}
			IEnumerator<MarkupProperty> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x000B0AF2 File Offset: 0x000AECF2
		public override void AssignRootContext(IValueSerializerContext context)
		{
			this._context = context;
			base.AssignRootContext(context);
		}

		// Token: 0x04001B33 RID: 6963
		private IValueSerializerContext _context;
	}
}
