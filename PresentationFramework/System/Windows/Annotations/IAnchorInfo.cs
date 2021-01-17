using System;

namespace System.Windows.Annotations
{
	/// <summary>Provides the capabilities for matching annotations with the corresponding annotated objects.</summary>
	// Token: 0x020005C3 RID: 1475
	public interface IAnchorInfo
	{
		/// <summary>Gets the annotation object.</summary>
		/// <returns>The annotation object.</returns>
		// Token: 0x170017A7 RID: 6055
		// (get) Token: 0x0600625C RID: 25180
		Annotation Annotation { get; }

		/// <summary>Gets the anchor of the annotation.</summary>
		/// <returns>The anchor that is resolved.</returns>
		// Token: 0x170017A8 RID: 6056
		// (get) Token: 0x0600625D RID: 25181
		AnnotationResource Anchor { get; }

		/// <summary>Gets the object that represents the location on the tree where the <see cref="P:System.Windows.Annotations.IAnchorInfo.Anchor" /> is resolved. </summary>
		/// <returns>The object that represents the location on the tree where the <see cref="P:System.Windows.Annotations.IAnchorInfo.Anchor" /> is resolved. The type is specified by the type of the annotated object. Sticky notes and highlights in flow or fixed documents always resolve to a <see cref="T:System.Windows.Annotations.TextAnchor" /> object.</returns>
		// Token: 0x170017A9 RID: 6057
		// (get) Token: 0x0600625E RID: 25182
		object ResolvedAnchor { get; }
	}
}
