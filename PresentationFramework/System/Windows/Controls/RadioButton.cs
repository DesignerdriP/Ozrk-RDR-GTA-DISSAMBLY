using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a button that can be selected, but not cleared, by a user. The <see cref="P:System.Windows.Controls.Primitives.ToggleButton.IsChecked" /> property of a <see cref="T:System.Windows.Controls.RadioButton" /> can be set by clicking it, but it can only be cleared programmatically. </summary>
	// Token: 0x0200051E RID: 1310
	[Localizability(LocalizationCategory.RadioButton)]
	public class RadioButton : ToggleButton
	{
		// Token: 0x0600549E RID: 21662 RVA: 0x00176D2C File Offset: 0x00174F2C
		static RadioButton()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButton), new FrameworkPropertyMetadata(typeof(RadioButton)));
			RadioButton._dType = DependencyObjectType.FromSystemTypeInternal(typeof(RadioButton));
			KeyboardNavigation.AcceptsReturnProperty.OverrideMetadata(typeof(RadioButton), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			ControlsTraceLogger.AddControl(TelemetryControls.RadioButton);
		}

		// Token: 0x060054A0 RID: 21664 RVA: 0x00176DDC File Offset: 0x00174FDC
		private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RadioButton radioButton = (RadioButton)d;
			string text = e.NewValue as string;
			string value = RadioButton._currentlyRegisteredGroupName.GetValue(radioButton);
			if (text != value)
			{
				if (!string.IsNullOrEmpty(value))
				{
					RadioButton.Unregister(value, radioButton);
				}
				if (!string.IsNullOrEmpty(text))
				{
					RadioButton.Register(text, radioButton);
				}
			}
		}

		// Token: 0x060054A1 RID: 21665 RVA: 0x00176E30 File Offset: 0x00175030
		private static void Register(string groupName, RadioButton radioButton)
		{
			if (RadioButton._groupNameToElements == null)
			{
				RadioButton._groupNameToElements = new Hashtable(1);
			}
			Hashtable groupNameToElements = RadioButton._groupNameToElements;
			lock (groupNameToElements)
			{
				ArrayList arrayList = (ArrayList)RadioButton._groupNameToElements[groupName];
				if (arrayList == null)
				{
					arrayList = new ArrayList(1);
					RadioButton._groupNameToElements[groupName] = arrayList;
				}
				else
				{
					RadioButton.PurgeDead(arrayList, null);
				}
				arrayList.Add(new WeakReference(radioButton));
			}
			RadioButton._currentlyRegisteredGroupName.SetValue(radioButton, groupName);
		}

		// Token: 0x060054A2 RID: 21666 RVA: 0x00176EC4 File Offset: 0x001750C4
		private static void Unregister(string groupName, RadioButton radioButton)
		{
			if (RadioButton._groupNameToElements == null)
			{
				return;
			}
			Hashtable groupNameToElements = RadioButton._groupNameToElements;
			lock (groupNameToElements)
			{
				ArrayList arrayList = (ArrayList)RadioButton._groupNameToElements[groupName];
				if (arrayList != null)
				{
					RadioButton.PurgeDead(arrayList, radioButton);
					if (arrayList.Count == 0)
					{
						RadioButton._groupNameToElements.Remove(groupName);
					}
				}
			}
			RadioButton._currentlyRegisteredGroupName.SetValue(radioButton, null);
		}

		// Token: 0x060054A3 RID: 21667 RVA: 0x00176F40 File Offset: 0x00175140
		private static void PurgeDead(ArrayList elements, object elementToRemove)
		{
			int i = 0;
			while (i < elements.Count)
			{
				WeakReference weakReference = (WeakReference)elements[i];
				object target = weakReference.Target;
				if (target == null || target == elementToRemove)
				{
					elements.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x060054A4 RID: 21668 RVA: 0x00176F84 File Offset: 0x00175184
		private void UpdateRadioButtonGroup()
		{
			string groupName = this.GroupName;
			if (!string.IsNullOrEmpty(groupName))
			{
				Visual visualRoot = KeyboardNavigation.GetVisualRoot(this);
				if (RadioButton._groupNameToElements == null)
				{
					RadioButton._groupNameToElements = new Hashtable(1);
				}
				Hashtable groupNameToElements = RadioButton._groupNameToElements;
				lock (groupNameToElements)
				{
					ArrayList arrayList = (ArrayList)RadioButton._groupNameToElements[groupName];
					int i = 0;
					while (i < arrayList.Count)
					{
						WeakReference weakReference = (WeakReference)arrayList[i];
						RadioButton radioButton = weakReference.Target as RadioButton;
						if (radioButton == null)
						{
							arrayList.RemoveAt(i);
						}
						else
						{
							if (radioButton != this && radioButton.IsChecked == true && visualRoot == KeyboardNavigation.GetVisualRoot(radioButton))
							{
								radioButton.UncheckRadioButton();
							}
							i++;
						}
					}
					return;
				}
			}
			DependencyObject parent = base.Parent;
			if (parent != null)
			{
				IEnumerable children = LogicalTreeHelper.GetChildren(parent);
				foreach (object obj in children)
				{
					RadioButton radioButton2 = obj as RadioButton;
					if (radioButton2 != null && radioButton2 != this && string.IsNullOrEmpty(radioButton2.GroupName) && radioButton2.IsChecked == true)
					{
						radioButton2.UncheckRadioButton();
					}
				}
			}
		}

		// Token: 0x060054A5 RID: 21669 RVA: 0x001770F4 File Offset: 0x001752F4
		private void UncheckRadioButton()
		{
			base.SetCurrentValueInternal(ToggleButton.IsCheckedProperty, BooleanBoxes.FalseBox);
		}

		/// <summary>Gets or sets the name that specifies which <see cref="T:System.Windows.Controls.RadioButton" /> controls are mutually exclusive.  </summary>
		/// <returns>The name that specifies which <see cref="T:System.Windows.Controls.RadioButton" /> controls are mutually exclusive.  The default is an empty string.</returns>
		// Token: 0x17001493 RID: 5267
		// (get) Token: 0x060054A6 RID: 21670 RVA: 0x00177106 File Offset: 0x00175306
		// (set) Token: 0x060054A7 RID: 21671 RVA: 0x00177118 File Offset: 0x00175318
		[DefaultValue("")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public string GroupName
		{
			get
			{
				return (string)base.GetValue(RadioButton.GroupNameProperty);
			}
			set
			{
				base.SetValue(RadioButton.GroupNameProperty, value);
			}
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.RadioButtonAutomationPeer" /> implementation for this control, as part of the WPF automation infrastructure.</summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
		// Token: 0x060054A8 RID: 21672 RVA: 0x00177126 File Offset: 0x00175326
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new RadioButtonAutomationPeer(this);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.Primitives.ToggleButton.IsChecked" /> property becomes <see langword="true" />. </summary>
		/// <param name="e">Provides data for <see cref="T:System.Windows.RoutedEventArgs" />.</param>
		// Token: 0x060054A9 RID: 21673 RVA: 0x0017712E File Offset: 0x0017532E
		protected override void OnChecked(RoutedEventArgs e)
		{
			this.UpdateRadioButtonGroup();
			base.OnChecked(e);
		}

		/// <summary>Called by the <see cref="M:System.Windows.Controls.Primitives.ToggleButton.OnClick" /> method to implement a <see cref="T:System.Windows.Controls.RadioButton" /> control's toggle behavior. </summary>
		// Token: 0x060054AA RID: 21674 RVA: 0x0017713D File Offset: 0x0017533D
		protected internal override void OnToggle()
		{
			base.SetCurrentValueInternal(ToggleButton.IsCheckedProperty, BooleanBoxes.TrueBox);
		}

		/// <summary> Called when the <see cref="P:System.Windows.Controls.AccessText.AccessKey" /> for a radio button is invoked. </summary>
		/// <param name="e">Provides data for <see cref="T:System.Windows.Input.AccessKeyEventArgs" />.</param>
		// Token: 0x060054AB RID: 21675 RVA: 0x001338AF File Offset: 0x00131AAF
		protected override void OnAccessKey(AccessKeyEventArgs e)
		{
			if (!base.IsKeyboardFocused)
			{
				base.Focus();
			}
			base.OnAccessKey(e);
		}

		// Token: 0x17001494 RID: 5268
		// (get) Token: 0x060054AC RID: 21676 RVA: 0x0017714F File Offset: 0x0017534F
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return RadioButton._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.RadioButton.GroupName" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.RadioButton.GroupName" /> dependency property.</returns>
		// Token: 0x04002DA7 RID: 11687
		public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(string), typeof(RadioButton), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(RadioButton.OnGroupNameChanged)));

		// Token: 0x04002DA8 RID: 11688
		private static DependencyObjectType _dType;

		// Token: 0x04002DA9 RID: 11689
		[ThreadStatic]
		private static Hashtable _groupNameToElements;

		// Token: 0x04002DAA RID: 11690
		private static readonly UncommonField<string> _currentlyRegisteredGroupName = new UncommonField<string>();
	}
}
