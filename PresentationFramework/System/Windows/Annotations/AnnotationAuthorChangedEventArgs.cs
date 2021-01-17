using System;
using System.ComponentModel;

namespace System.Windows.Annotations
{
	/// <summary> Provides data for the <see cref="E:System.Windows.Annotations.Annotation.AuthorChanged" /> event. </summary>
	// Token: 0x020005C7 RID: 1479
	public sealed class AnnotationAuthorChangedEventArgs : EventArgs
	{
		/// <summary> Initializes a new instance of the <see cref="T:System.Windows.Annotations.AnnotationAuthorChangedEventArgs" /> class. </summary>
		/// <param name="annotation">The annotation raising the event.</param>
		/// <param name="action">The author operation performed: <see cref="F:System.Windows.Annotations.AnnotationAction.Added" />, <see cref="F:System.Windows.Annotations.AnnotationAction.Removed" />, or <see cref="F:System.Windows.Annotations.AnnotationAction.Modified" />.</param>
		/// <param name="author">The author object being changed by the event.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="annotation" /> or <paramref name="action" /> is a null reference (Nothing in Visual Basic).</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="action" /> is an invalid <see cref="T:System.Windows.Annotations.AnnotationAction" />.</exception>
		// Token: 0x06006283 RID: 25219 RVA: 0x001BA58C File Offset: 0x001B878C
		public AnnotationAuthorChangedEventArgs(Annotation annotation, AnnotationAction action, object author)
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
			this._author = author;
			this._action = action;
		}

		/// <summary> Gets the annotation that raised the event. </summary>
		/// <returns>The annotation that raised the event.</returns>
		// Token: 0x170017B2 RID: 6066
		// (get) Token: 0x06006284 RID: 25220 RVA: 0x001BA5E0 File Offset: 0x001B87E0
		public Annotation Annotation
		{
			get
			{
				return this._annotation;
			}
		}

		/// <summary> Gets the author object that is the target of the event. </summary>
		/// <returns>The author object that is the target of the event.</returns>
		// Token: 0x170017B3 RID: 6067
		// (get) Token: 0x06006285 RID: 25221 RVA: 0x001BA5E8 File Offset: 0x001B87E8
		public object Author
		{
			get
			{
				return this._author;
			}
		}

		/// <summary> Gets the author change operation for the event. </summary>
		/// <returns>The author change operation: <see cref="F:System.Windows.Annotations.AnnotationAction.Added" />, <see cref="F:System.Windows.Annotations.AnnotationAction.Removed" />, or <see cref="F:System.Windows.Annotations.AnnotationAction.Modified" />.</returns>
		// Token: 0x170017B4 RID: 6068
		// (get) Token: 0x06006286 RID: 25222 RVA: 0x001BA5F0 File Offset: 0x001B87F0
		public AnnotationAction Action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x0400319C RID: 12700
		private Annotation _annotation;

		// Token: 0x0400319D RID: 12701
		private object _author;

		// Token: 0x0400319E RID: 12702
		private AnnotationAction _action;
	}
}
