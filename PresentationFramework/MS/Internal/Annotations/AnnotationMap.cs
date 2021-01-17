using System;
using System.Collections.Generic;

namespace MS.Internal.Annotations
{
	// Token: 0x020007C2 RID: 1986
	internal class AnnotationMap
	{
		// Token: 0x06007B67 RID: 31591 RVA: 0x0022B4B0 File Offset: 0x002296B0
		internal void AddAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			List<IAttachedAnnotation> list = null;
			if (!this._annotationIdToAttachedAnnotations.TryGetValue(attachedAnnotation.Annotation.Id, out list))
			{
				list = new List<IAttachedAnnotation>(1);
				this._annotationIdToAttachedAnnotations.Add(attachedAnnotation.Annotation.Id, list);
			}
			list.Add(attachedAnnotation);
		}

		// Token: 0x06007B68 RID: 31592 RVA: 0x0022B500 File Offset: 0x00229700
		internal void RemoveAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			List<IAttachedAnnotation> list = null;
			if (this._annotationIdToAttachedAnnotations.TryGetValue(attachedAnnotation.Annotation.Id, out list))
			{
				list.Remove(attachedAnnotation);
				if (list.Count == 0)
				{
					this._annotationIdToAttachedAnnotations.Remove(attachedAnnotation.Annotation.Id);
				}
			}
		}

		// Token: 0x17001CCD RID: 7373
		// (get) Token: 0x06007B69 RID: 31593 RVA: 0x0022B550 File Offset: 0x00229750
		internal bool IsEmpty
		{
			get
			{
				return this._annotationIdToAttachedAnnotations.Count == 0;
			}
		}

		// Token: 0x06007B6A RID: 31594 RVA: 0x0022B560 File Offset: 0x00229760
		internal List<IAttachedAnnotation> GetAttachedAnnotations(Guid annotationId)
		{
			List<IAttachedAnnotation> result = null;
			if (!this._annotationIdToAttachedAnnotations.TryGetValue(annotationId, out result))
			{
				return AnnotationMap._emptyList;
			}
			return result;
		}

		// Token: 0x06007B6B RID: 31595 RVA: 0x0022B588 File Offset: 0x00229788
		internal List<IAttachedAnnotation> GetAllAttachedAnnotations()
		{
			List<IAttachedAnnotation> list = new List<IAttachedAnnotation>(this._annotationIdToAttachedAnnotations.Keys.Count);
			foreach (Guid key in this._annotationIdToAttachedAnnotations.Keys)
			{
				List<IAttachedAnnotation> collection = this._annotationIdToAttachedAnnotations[key];
				list.AddRange(collection);
			}
			if (list.Count == 0)
			{
				return AnnotationMap._emptyList;
			}
			return list;
		}

		// Token: 0x04003A18 RID: 14872
		private Dictionary<Guid, List<IAttachedAnnotation>> _annotationIdToAttachedAnnotations = new Dictionary<Guid, List<IAttachedAnnotation>>();

		// Token: 0x04003A19 RID: 14873
		private static readonly List<IAttachedAnnotation> _emptyList = new List<IAttachedAnnotation>(0);
	}
}
