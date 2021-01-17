using System;
using System.Windows.Controls;
using MS.Internal.Annotations;
using MS.Internal.Annotations.Component;

namespace System.Windows.Annotations
{
	// Token: 0x020005C8 RID: 1480
	internal sealed class AnnotationComponentChooser
	{
		// Token: 0x06006288 RID: 25224 RVA: 0x001BA5F8 File Offset: 0x001B87F8
		public IAnnotationComponent ChooseAnnotationComponent(IAttachedAnnotation attachedAnnotation)
		{
			if (attachedAnnotation == null)
			{
				throw new ArgumentNullException("attachedAnnotation");
			}
			IAnnotationComponent result = null;
			if (attachedAnnotation.Annotation.AnnotationType == StickyNoteControl.TextSchemaName)
			{
				result = new StickyNoteControl(StickyNoteType.Text);
			}
			else if (attachedAnnotation.Annotation.AnnotationType == StickyNoteControl.InkSchemaName)
			{
				result = new StickyNoteControl(StickyNoteType.Ink);
			}
			else if (attachedAnnotation.Annotation.AnnotationType == HighlightComponent.TypeName)
			{
				result = new HighlightComponent();
			}
			return result;
		}
	}
}
