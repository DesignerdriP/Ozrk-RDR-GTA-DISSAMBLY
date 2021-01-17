using System;
using System.ComponentModel;

namespace System.Windows.Annotations
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Annotations.Annotation.AnchorChanged" /> and <see cref="E:System.Windows.Annotations.Annotation.CargoChanged" /> events.</summary>
	// Token: 0x020005CC RID: 1484
	public sealed class AnnotationResourceChangedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="M:System.Windows.Annotations.AnnotationResourceChangedEventArgs.#ctor(System.Windows.Annotations.Annotation,System.Windows.Annotations.AnnotationAction,System.Windows.Annotations.AnnotationResource)" /> class.</summary>
		/// <param name="annotation">The annotation that raised the event.</param>
		/// <param name="action">The action of the event.</param>
		/// <param name="resource">The <see cref="P:System.Windows.Annotations.Annotation.Anchors" /> or <see cref="P:System.Windows.Annotations.Annotation.Cargos" /> resource of the event.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="annotation" /> or <paramref name="action" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="action" /> is not a valid <see cref="T:System.Windows.Annotations.AnnotationAction" /> value.</exception>
		// Token: 0x060062B7 RID: 25271 RVA: 0x001BB128 File Offset: 0x001B9328
		public AnnotationResourceChangedEventArgs(Annotation annotation, AnnotationAction action, AnnotationResource resource)
		{
			if (annotation == null)
			{
				throw new ArgumentNullException("annotation");
			}
			if (action < AnnotationAction.Added || action > AnnotationAction.Modified)
			{
				throw new InvalidEnumArgumentException("action", (int)action, typeof(AnnotationAction));
			}
			this._annotation = annotation;
			this._resource = resource;
			this._action = action;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Annotations.Annotation" /> that raised the event.</summary>
		/// <returns>The <see cref="T:System.Windows.Annotations.Annotation" /> that raised the event.</returns>
		// Token: 0x170017C2 RID: 6082
		// (get) Token: 0x060062B8 RID: 25272 RVA: 0x001BB17C File Offset: 0x001B937C
		public Annotation Annotation
		{
			get
			{
				return this._annotation;
			}
		}

		/// <summary>Gets the <see cref="P:System.Windows.Annotations.Annotation.Anchors" /> or <see cref="P:System.Windows.Annotations.Annotation.Cargos" /> resource associated with the event.</summary>
		/// <returns>The annotation anchor or cargo resource that was <see cref="F:System.Windows.Annotations.AnnotationAction.Added" />, <see cref="F:System.Windows.Annotations.AnnotationAction.Removed" />, or <see cref="F:System.Windows.Annotations.AnnotationAction.Modified" />.</returns>
		// Token: 0x170017C3 RID: 6083
		// (get) Token: 0x060062B9 RID: 25273 RVA: 0x001BB184 File Offset: 0x001B9384
		public AnnotationResource Resource
		{
			get
			{
				return this._resource;
			}
		}

		/// <summary>Gets the action of the annotation <see cref="P:System.Windows.Annotations.AnnotationResourceChangedEventArgs.Resource" />.</summary>
		/// <returns>The action of the annotation <see cref="P:System.Windows.Annotations.AnnotationResourceChangedEventArgs.Resource" />.</returns>
		// Token: 0x170017C4 RID: 6084
		// (get) Token: 0x060062BA RID: 25274 RVA: 0x001BB18C File Offset: 0x001B938C
		public AnnotationAction Action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x040031AC RID: 12716
		private Annotation _annotation;

		// Token: 0x040031AD RID: 12717
		private AnnotationResource _resource;

		// Token: 0x040031AE RID: 12718
		private AnnotationAction _action;
	}
}
