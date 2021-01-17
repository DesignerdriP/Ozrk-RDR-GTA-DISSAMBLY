using System;
using System.Windows;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020007E5 RID: 2021
	internal abstract class PresentationContext
	{
		// Token: 0x17001D13 RID: 7443
		// (get) Token: 0x06007CFE RID: 31998
		public abstract UIElement Host { get; }

		// Token: 0x17001D14 RID: 7444
		// (get) Token: 0x06007CFF RID: 31999
		public abstract PresentationContext EnclosingContext { get; }

		// Token: 0x06007D00 RID: 32000
		public abstract void AddToHost(IAnnotationComponent component);

		// Token: 0x06007D01 RID: 32001
		public abstract void RemoveFromHost(IAnnotationComponent component, bool reorder);

		// Token: 0x06007D02 RID: 32002
		public abstract void InvalidateTransform(IAnnotationComponent component);

		// Token: 0x06007D03 RID: 32003
		public abstract void BringToFront(IAnnotationComponent component);

		// Token: 0x06007D04 RID: 32004
		public abstract void SendToBack(IAnnotationComponent component);
	}
}
