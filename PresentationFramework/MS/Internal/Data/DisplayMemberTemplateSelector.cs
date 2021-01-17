using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000723 RID: 1827
	internal sealed class DisplayMemberTemplateSelector : DataTemplateSelector
	{
		// Token: 0x06007504 RID: 29956 RVA: 0x002177BC File Offset: 0x002159BC
		public DisplayMemberTemplateSelector(string displayMemberPath, string stringFormat)
		{
			this._displayMemberPath = displayMemberPath;
			this._stringFormat = stringFormat;
		}

		// Token: 0x06007505 RID: 29957 RVA: 0x002177D4 File Offset: 0x002159D4
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (SystemXmlHelper.IsXmlNode(item))
			{
				if (this._xmlNodeContentTemplate == null)
				{
					this._xmlNodeContentTemplate = new DataTemplate();
					FrameworkElementFactory frameworkElementFactory = ContentPresenter.CreateTextBlockFactory();
					Binding binding = new Binding();
					binding.XPath = this._displayMemberPath;
					binding.StringFormat = this._stringFormat;
					frameworkElementFactory.SetBinding(TextBlock.TextProperty, binding);
					this._xmlNodeContentTemplate.VisualTree = frameworkElementFactory;
					this._xmlNodeContentTemplate.Seal();
				}
				return this._xmlNodeContentTemplate;
			}
			if (this._clrNodeContentTemplate == null)
			{
				this._clrNodeContentTemplate = new DataTemplate();
				FrameworkElementFactory frameworkElementFactory2 = ContentPresenter.CreateTextBlockFactory();
				Binding binding2 = new Binding();
				binding2.Path = new PropertyPath(this._displayMemberPath, new object[0]);
				binding2.StringFormat = this._stringFormat;
				frameworkElementFactory2.SetBinding(TextBlock.TextProperty, binding2);
				this._clrNodeContentTemplate.VisualTree = frameworkElementFactory2;
				this._clrNodeContentTemplate.Seal();
			}
			return this._clrNodeContentTemplate;
		}

		// Token: 0x0400380E RID: 14350
		private string _displayMemberPath;

		// Token: 0x0400380F RID: 14351
		private string _stringFormat;

		// Token: 0x04003810 RID: 14352
		private DataTemplate _xmlNodeContentTemplate;

		// Token: 0x04003811 RID: 14353
		private DataTemplate _clrNodeContentTemplate;
	}
}
