using System;

namespace MS.Internal.Annotations
{
	// Token: 0x020007CA RID: 1994
	internal class AttachedAnnotationChangedEventArgs : EventArgs
	{
		// Token: 0x06007B89 RID: 31625 RVA: 0x0022B999 File Offset: 0x00229B99
		internal AttachedAnnotationChangedEventArgs(AttachedAnnotationAction action, IAttachedAnnotation attachedAnnotation, object previousAttachedAnchor, AttachmentLevel previousAttachmentLevel)
		{
			Invariant.Assert(attachedAnnotation != null);
			this._action = action;
			this._attachedAnnotation = attachedAnnotation;
			this._previousAttachedAnchor = previousAttachedAnchor;
			this._previousAttachmentLevel = previousAttachmentLevel;
		}

		// Token: 0x17001CD4 RID: 7380
		// (get) Token: 0x06007B8A RID: 31626 RVA: 0x0022B9C7 File Offset: 0x00229BC7
		public AttachedAnnotationAction Action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x17001CD5 RID: 7381
		// (get) Token: 0x06007B8B RID: 31627 RVA: 0x0022B9CF File Offset: 0x00229BCF
		public IAttachedAnnotation AttachedAnnotation
		{
			get
			{
				return this._attachedAnnotation;
			}
		}

		// Token: 0x17001CD6 RID: 7382
		// (get) Token: 0x06007B8C RID: 31628 RVA: 0x0022B9D7 File Offset: 0x00229BD7
		public object PreviousAttachedAnchor
		{
			get
			{
				return this._previousAttachedAnchor;
			}
		}

		// Token: 0x17001CD7 RID: 7383
		// (get) Token: 0x06007B8D RID: 31629 RVA: 0x0022B9DF File Offset: 0x00229BDF
		public AttachmentLevel PreviousAttachmentLevel
		{
			get
			{
				return this._previousAttachmentLevel;
			}
		}

		// Token: 0x06007B8E RID: 31630 RVA: 0x0022B9E7 File Offset: 0x00229BE7
		internal static AttachedAnnotationChangedEventArgs Added(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null);
			return new AttachedAnnotationChangedEventArgs(AttachedAnnotationAction.Added, attachedAnnotation, null, AttachmentLevel.Unresolved);
		}

		// Token: 0x06007B8F RID: 31631 RVA: 0x0022B9FB File Offset: 0x00229BFB
		internal static AttachedAnnotationChangedEventArgs Loaded(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null);
			return new AttachedAnnotationChangedEventArgs(AttachedAnnotationAction.Loaded, attachedAnnotation, null, AttachmentLevel.Unresolved);
		}

		// Token: 0x06007B90 RID: 31632 RVA: 0x0022BA0F File Offset: 0x00229C0F
		internal static AttachedAnnotationChangedEventArgs Deleted(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null);
			return new AttachedAnnotationChangedEventArgs(AttachedAnnotationAction.Deleted, attachedAnnotation, null, AttachmentLevel.Unresolved);
		}

		// Token: 0x06007B91 RID: 31633 RVA: 0x0022BA23 File Offset: 0x00229C23
		internal static AttachedAnnotationChangedEventArgs Unloaded(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null);
			return new AttachedAnnotationChangedEventArgs(AttachedAnnotationAction.Unloaded, attachedAnnotation, null, AttachmentLevel.Unresolved);
		}

		// Token: 0x06007B92 RID: 31634 RVA: 0x0022BA37 File Offset: 0x00229C37
		internal static AttachedAnnotationChangedEventArgs Modified(IAttachedAnnotation attachedAnnotation, object previousAttachedAnchor, AttachmentLevel previousAttachmentLevel)
		{
			Invariant.Assert(attachedAnnotation != null && previousAttachedAnchor != null);
			return new AttachedAnnotationChangedEventArgs(AttachedAnnotationAction.AnchorModified, attachedAnnotation, previousAttachedAnchor, previousAttachmentLevel);
		}

		// Token: 0x04003A2B RID: 14891
		private AttachedAnnotationAction _action;

		// Token: 0x04003A2C RID: 14892
		private IAttachedAnnotation _attachedAnnotation;

		// Token: 0x04003A2D RID: 14893
		private object _previousAttachedAnchor;

		// Token: 0x04003A2E RID: 14894
		private AttachmentLevel _previousAttachmentLevel;
	}
}
