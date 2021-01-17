using System;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Annotations.Storage;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020007D3 RID: 2003
	internal class AttachedAnnotation : IAttachedAnnotation, IAnchorInfo
	{
		// Token: 0x06007BDE RID: 31710 RVA: 0x0022D33B File Offset: 0x0022B53B
		internal AttachedAnnotation(LocatorManager manager, Annotation annotation, AnnotationResource anchor, object attachedAnchor, AttachmentLevel attachmentLevel) : this(manager, annotation, anchor, attachedAnchor, attachmentLevel, null)
		{
		}

		// Token: 0x06007BDF RID: 31711 RVA: 0x0022D34B File Offset: 0x0022B54B
		internal AttachedAnnotation(LocatorManager manager, Annotation annotation, AnnotationResource anchor, object attachedAnchor, AttachmentLevel attachmentLevel, DependencyObject parent)
		{
			this._annotation = annotation;
			this._anchor = anchor;
			this._locatorManager = manager;
			this.Update(attachedAnchor, attachmentLevel, parent);
		}

		// Token: 0x06007BE0 RID: 31712 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsAnchorEqual(object o)
		{
			return false;
		}

		// Token: 0x17001CDF RID: 7391
		// (get) Token: 0x06007BE1 RID: 31713 RVA: 0x0022D374 File Offset: 0x0022B574
		public Annotation Annotation
		{
			get
			{
				return this._annotation;
			}
		}

		// Token: 0x17001CE0 RID: 7392
		// (get) Token: 0x06007BE2 RID: 31714 RVA: 0x0022D37C File Offset: 0x0022B57C
		public AnnotationResource Anchor
		{
			get
			{
				return this._anchor;
			}
		}

		// Token: 0x17001CE1 RID: 7393
		// (get) Token: 0x06007BE3 RID: 31715 RVA: 0x0022D384 File Offset: 0x0022B584
		public object AttachedAnchor
		{
			get
			{
				return this._attachedAnchor;
			}
		}

		// Token: 0x17001CE2 RID: 7394
		// (get) Token: 0x06007BE4 RID: 31716 RVA: 0x0022D38C File Offset: 0x0022B58C
		public object ResolvedAnchor
		{
			get
			{
				return this.FullyAttachedAnchor;
			}
		}

		// Token: 0x17001CE3 RID: 7395
		// (get) Token: 0x06007BE5 RID: 31717 RVA: 0x0022D394 File Offset: 0x0022B594
		public object FullyAttachedAnchor
		{
			get
			{
				if (this._attachmentLevel == AttachmentLevel.Full)
				{
					return this._attachedAnchor;
				}
				return this._fullyAttachedAnchor;
			}
		}

		// Token: 0x17001CE4 RID: 7396
		// (get) Token: 0x06007BE6 RID: 31718 RVA: 0x0022D3AC File Offset: 0x0022B5AC
		public AttachmentLevel AttachmentLevel
		{
			get
			{
				return this._attachmentLevel;
			}
		}

		// Token: 0x17001CE5 RID: 7397
		// (get) Token: 0x06007BE7 RID: 31719 RVA: 0x0022D3B4 File Offset: 0x0022B5B4
		public DependencyObject Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x17001CE6 RID: 7398
		// (get) Token: 0x06007BE8 RID: 31720 RVA: 0x0022D3BC File Offset: 0x0022B5BC
		public Point AnchorPoint
		{
			get
			{
				Point anchorPoint = this._selectionProcessor.GetAnchorPoint(this._attachedAnchor);
				if (!double.IsInfinity(anchorPoint.X) && !double.IsInfinity(anchorPoint.Y))
				{
					this._cachedPoint = anchorPoint;
				}
				return this._cachedPoint;
			}
		}

		// Token: 0x17001CE7 RID: 7399
		// (get) Token: 0x06007BE9 RID: 31721 RVA: 0x0022D404 File Offset: 0x0022B604
		public AnnotationStore Store
		{
			get
			{
				return this.GetStore();
			}
		}

		// Token: 0x06007BEA RID: 31722 RVA: 0x0022D40C File Offset: 0x0022B60C
		internal void Update(object attachedAnchor, AttachmentLevel attachmentLevel, DependencyObject parent)
		{
			this._attachedAnchor = attachedAnchor;
			this._attachmentLevel = attachmentLevel;
			this._selectionProcessor = this._locatorManager.GetSelectionProcessor(attachedAnchor.GetType());
			if (parent != null)
			{
				this._parent = parent;
				return;
			}
			this._parent = this._selectionProcessor.GetParent(this._attachedAnchor);
		}

		// Token: 0x06007BEB RID: 31723 RVA: 0x0022D460 File Offset: 0x0022B660
		internal void SetFullyAttachedAnchor(object fullyAttachedAnchor)
		{
			this._fullyAttachedAnchor = fullyAttachedAnchor;
		}

		// Token: 0x06007BEC RID: 31724 RVA: 0x0022D46C File Offset: 0x0022B66C
		private AnnotationStore GetStore()
		{
			if (this.Parent != null)
			{
				AnnotationService service = AnnotationService.GetService(this.Parent);
				if (service != null)
				{
					return service.Store;
				}
			}
			return null;
		}

		// Token: 0x04003A3F RID: 14911
		private Annotation _annotation;

		// Token: 0x04003A40 RID: 14912
		private AnnotationResource _anchor;

		// Token: 0x04003A41 RID: 14913
		private object _attachedAnchor;

		// Token: 0x04003A42 RID: 14914
		private object _fullyAttachedAnchor;

		// Token: 0x04003A43 RID: 14915
		private AttachmentLevel _attachmentLevel;

		// Token: 0x04003A44 RID: 14916
		private DependencyObject _parent;

		// Token: 0x04003A45 RID: 14917
		private SelectionProcessor _selectionProcessor;

		// Token: 0x04003A46 RID: 14918
		private LocatorManager _locatorManager;

		// Token: 0x04003A47 RID: 14919
		private Point _cachedPoint;
	}
}
