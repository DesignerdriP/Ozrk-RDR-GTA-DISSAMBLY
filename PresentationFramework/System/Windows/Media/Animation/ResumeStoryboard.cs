using System;

namespace System.Windows.Media.Animation
{
	/// <summary>Supports a trigger action that resumes a paused <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
	// Token: 0x02000184 RID: 388
	public sealed class ResumeStoryboard : ControllableStoryboardAction
	{
		// Token: 0x0600169B RID: 5787 RVA: 0x0007070A File Offset: 0x0006E90A
		internal override void Invoke(FrameworkElement containingFE, FrameworkContentElement containingFCE, Storyboard storyboard)
		{
			if (containingFE != null)
			{
				storyboard.Resume(containingFE);
				return;
			}
			storyboard.Resume(containingFCE);
		}
	}
}
