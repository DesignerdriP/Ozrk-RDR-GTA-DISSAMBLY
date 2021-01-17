using System;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Annotations.Storage;

namespace MS.Internal.Annotations
{
	// Token: 0x020007C8 RID: 1992
	internal interface IAttachedAnnotation : IAnchorInfo
	{
		// Token: 0x17001CCE RID: 7374
		// (get) Token: 0x06007B82 RID: 31618
		object AttachedAnchor { get; }

		// Token: 0x17001CCF RID: 7375
		// (get) Token: 0x06007B83 RID: 31619
		object FullyAttachedAnchor { get; }

		// Token: 0x17001CD0 RID: 7376
		// (get) Token: 0x06007B84 RID: 31620
		AttachmentLevel AttachmentLevel { get; }

		// Token: 0x17001CD1 RID: 7377
		// (get) Token: 0x06007B85 RID: 31621
		DependencyObject Parent { get; }

		// Token: 0x17001CD2 RID: 7378
		// (get) Token: 0x06007B86 RID: 31622
		Point AnchorPoint { get; }

		// Token: 0x06007B87 RID: 31623
		bool IsAnchorEqual(object o);

		// Token: 0x17001CD3 RID: 7379
		// (get) Token: 0x06007B88 RID: 31624
		AnnotationStore Store { get; }
	}
}
