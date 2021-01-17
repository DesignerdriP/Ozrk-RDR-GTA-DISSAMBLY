using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents an implementation of a <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control that enables a <see cref="T:System.Windows.Window" /> to change its size. </summary>
	// Token: 0x020005A2 RID: 1442
	public class ResizeGrip : Control
	{
		// Token: 0x06005F6D RID: 24429 RVA: 0x001ABE40 File Offset: 0x001AA040
		static ResizeGrip()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeGrip), new FrameworkPropertyMetadata(typeof(ResizeGrip)));
			ResizeGrip._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ResizeGrip));
			Window.IWindowServiceProperty.OverrideMetadata(typeof(ResizeGrip), new FrameworkPropertyMetadata(new PropertyChangedCallback(ResizeGrip._OnWindowServiceChanged)));
		}

		// Token: 0x06005F6F RID: 24431 RVA: 0x001ABEAC File Offset: 0x001AA0AC
		private static void _OnWindowServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ResizeGrip resizeGrip = d as ResizeGrip;
			resizeGrip.OnWindowServiceChanged(e.OldValue as Window, e.NewValue as Window);
		}

		// Token: 0x06005F70 RID: 24432 RVA: 0x001ABEDE File Offset: 0x001AA0DE
		private void OnWindowServiceChanged(Window oldWindow, Window newWindow)
		{
			if (oldWindow != null && oldWindow != newWindow)
			{
				oldWindow.ClearResizeGripControl(this);
			}
			if (newWindow != null)
			{
				newWindow.SetResizeGripControl(this);
			}
		}

		// Token: 0x17001700 RID: 5888
		// (get) Token: 0x06005F71 RID: 24433 RVA: 0x001ABEF8 File Offset: 0x001AA0F8
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ResizeGrip._dType;
			}
		}

		// Token: 0x17001701 RID: 5889
		// (get) Token: 0x06005F72 RID: 24434 RVA: 0x000961BF File Offset: 0x000943BF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x040030A3 RID: 12451
		private static DependencyObjectType _dType;
	}
}
