using System;
using System.Windows.Data;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents the text input of a <see cref="T:System.Windows.Controls.DatePicker" />.</summary>
	// Token: 0x02000582 RID: 1410
	[TemplatePart(Name = "PART_Watermark", Type = typeof(ContentControl))]
	public sealed class DatePickerTextBox : TextBox
	{
		// Token: 0x06005D5A RID: 23898 RVA: 0x001A463C File Offset: 0x001A283C
		static DatePickerTextBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DatePickerTextBox), new FrameworkPropertyMetadata(typeof(DatePickerTextBox)));
			TextBox.TextProperty.OverrideMetadata(typeof(DatePickerTextBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Primitives.DatePickerTextBox" /> class. </summary>
		// Token: 0x06005D5B RID: 23899 RVA: 0x001A46C8 File Offset: 0x001A28C8
		public DatePickerTextBox()
		{
			base.SetCurrentValue(DatePickerTextBox.WatermarkProperty, SR.Get("DatePickerTextBox_DefaultWatermarkText"));
			base.Loaded += this.OnLoaded;
			base.IsEnabledChanged += this.OnDatePickerTextBoxIsEnabledChanged;
		}

		// Token: 0x17001695 RID: 5781
		// (get) Token: 0x06005D5C RID: 23900 RVA: 0x001A4714 File Offset: 0x001A2914
		// (set) Token: 0x06005D5D RID: 23901 RVA: 0x001A4721 File Offset: 0x001A2921
		internal object Watermark
		{
			get
			{
				return base.GetValue(DatePickerTextBox.WatermarkProperty);
			}
			set
			{
				base.SetValue(DatePickerTextBox.WatermarkProperty, value);
			}
		}

		/// <summary>Builds the visual tree for the <see cref="T:System.Windows.Controls.Primitives.DatePickerTextBox" /> when a new template is applied.</summary>
		// Token: 0x06005D5E RID: 23902 RVA: 0x001A4730 File Offset: 0x001A2930
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.elementContent = this.ExtractTemplatePart<ContentControl>("PART_Watermark");
			if (this.elementContent != null)
			{
				Binding binding = new Binding("Watermark");
				binding.Source = this;
				this.elementContent.SetBinding(ContentControl.ContentProperty, binding);
			}
			this.OnWatermarkChanged();
		}

		// Token: 0x06005D5F RID: 23903 RVA: 0x001A4786 File Offset: 0x001A2986
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
			if (base.IsEnabled && !string.IsNullOrEmpty(base.Text))
			{
				base.Select(0, base.Text.Length);
			}
		}

		// Token: 0x06005D60 RID: 23904 RVA: 0x001A47B6 File Offset: 0x001A29B6
		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			base.ApplyTemplate();
		}

		// Token: 0x06005D61 RID: 23905 RVA: 0x001A47C0 File Offset: 0x001A29C0
		internal override void ChangeVisualState(bool useTransitions)
		{
			base.ChangeVisualState(useTransitions);
			if (this.Watermark != null && string.IsNullOrEmpty(base.Text))
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Watermarked",
					"Unwatermarked"
				});
				return;
			}
			VisualStates.GoToState(this, useTransitions, new string[]
			{
				"Unwatermarked"
			});
		}

		// Token: 0x06005D62 RID: 23906 RVA: 0x001A481C File Offset: 0x001A2A1C
		private T ExtractTemplatePart<T>(string partName) where T : DependencyObject
		{
			DependencyObject templateChild = base.GetTemplateChild(partName);
			return DatePickerTextBox.ExtractTemplatePart<T>(partName, templateChild);
		}

		// Token: 0x06005D63 RID: 23907 RVA: 0x001A4838 File Offset: 0x001A2A38
		private static T ExtractTemplatePart<T>(string partName, DependencyObject obj) where T : DependencyObject
		{
			return obj as T;
		}

		// Token: 0x06005D64 RID: 23908 RVA: 0x001A4848 File Offset: 0x001A2A48
		private void OnDatePickerTextBoxIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			bool flag = (bool)e.NewValue;
			base.SetCurrentValueInternal(TextBoxBase.IsReadOnlyProperty, BooleanBoxes.Box(!flag));
		}

		// Token: 0x06005D65 RID: 23909 RVA: 0x001A4878 File Offset: 0x001A2A78
		private void OnWatermarkChanged()
		{
			if (this.elementContent != null)
			{
				Control control = this.Watermark as Control;
				if (control != null)
				{
					control.IsTabStop = false;
					control.IsHitTestVisible = false;
				}
			}
		}

		// Token: 0x06005D66 RID: 23910 RVA: 0x001A48AC File Offset: 0x001A2AAC
		private static void OnWatermarkPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			DatePickerTextBox datePickerTextBox = sender as DatePickerTextBox;
			datePickerTextBox.OnWatermarkChanged();
			datePickerTextBox.UpdateVisualState();
		}

		// Token: 0x04003013 RID: 12307
		private const string ElementContentName = "PART_Watermark";

		// Token: 0x04003014 RID: 12308
		private ContentControl elementContent;

		// Token: 0x04003015 RID: 12309
		internal static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(DatePickerTextBox), new PropertyMetadata(new PropertyChangedCallback(DatePickerTextBox.OnWatermarkPropertyChanged)));
	}
}
