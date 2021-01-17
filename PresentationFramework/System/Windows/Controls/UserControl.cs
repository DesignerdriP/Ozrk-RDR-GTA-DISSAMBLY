using System;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Provides a simple way to create a control.</summary>
	// Token: 0x0200054F RID: 1359
	public class UserControl : ContentControl
	{
		// Token: 0x06005963 RID: 22883 RVA: 0x0018ADB4 File Offset: 0x00188FB4
		static UserControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(UserControl), new FrameworkPropertyMetadata(typeof(UserControl)));
			UserControl._dType = DependencyObjectType.FromSystemTypeInternal(typeof(UserControl));
			UIElement.FocusableProperty.OverrideMetadata(typeof(UserControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(UserControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			Control.HorizontalContentAlignmentProperty.OverrideMetadata(typeof(UserControl), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch));
			Control.VerticalContentAlignmentProperty.OverrideMetadata(typeof(UserControl), new FrameworkPropertyMetadata(VerticalAlignment.Stretch));
		}

		// Token: 0x06005965 RID: 22885 RVA: 0x0018AE72 File Offset: 0x00189072
		internal override void AdjustBranchSource(RoutedEventArgs e)
		{
			e.Source = this;
		}

		/// <summary>Creates and returns an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for this <see cref="T:System.Windows.Controls.UserControl" />.</summary>
		/// <returns>A new <see cref="T:System.Windows.Automation.Peers.UserControlAutomationPeer" /> for this <see cref="T:System.Windows.Controls.UserControl" />.</returns>
		// Token: 0x06005966 RID: 22886 RVA: 0x0018AE7B File Offset: 0x0018907B
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new UserControlAutomationPeer(this);
		}

		// Token: 0x170015C1 RID: 5569
		// (get) Token: 0x06005967 RID: 22887 RVA: 0x0014202D File Offset: 0x0014022D
		internal override FrameworkElement StateGroupsRoot
		{
			get
			{
				return base.Content as FrameworkElement;
			}
		}

		// Token: 0x170015C2 RID: 5570
		// (get) Token: 0x06005968 RID: 22888 RVA: 0x0018AE83 File Offset: 0x00189083
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return UserControl._dType;
			}
		}

		// Token: 0x04002EFD RID: 12029
		private static DependencyObjectType _dType;
	}
}
