using System;

namespace System.Windows.Media.Animation
{
	/// <summary>A trigger action that advances a <see cref="T:System.Windows.Media.Animation.Storyboard" /> to the end of its fill period. </summary>
	// Token: 0x02000187 RID: 391
	public sealed class SkipStoryboardToFill : ControllableStoryboardAction
	{
		// Token: 0x060016A8 RID: 5800 RVA: 0x00070882 File Offset: 0x0006EA82
		internal override void Invoke(FrameworkElement containingFE, FrameworkContentElement containingFCE, Storyboard storyboard)
		{
			if (containingFE != null)
			{
				storyboard.SkipToFill(containingFE);
				return;
			}
			storyboard.SkipToFill(containingFCE);
		}
	}
}
