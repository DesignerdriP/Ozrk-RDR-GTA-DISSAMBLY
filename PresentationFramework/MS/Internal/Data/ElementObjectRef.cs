using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;

namespace MS.Internal.Data
{
	// Token: 0x02000734 RID: 1844
	internal sealed class ElementObjectRef : ObjectRef
	{
		// Token: 0x060075FA RID: 30202 RVA: 0x0021A3CC File Offset: 0x002185CC
		internal ElementObjectRef(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this._name = name.Trim();
		}

		// Token: 0x060075FB RID: 30203 RVA: 0x0021A3F0 File Offset: 0x002185F0
		internal override object GetObject(DependencyObject d, ObjectRefArgs args)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			object obj = null;
			if (args.ResolveNamesInTemplate)
			{
				FrameworkElement frameworkElement = d as FrameworkElement;
				if (frameworkElement != null && frameworkElement.TemplateInternal != null)
				{
					obj = Helper.FindNameInTemplate(this._name, d);
					if (args.IsTracing)
					{
						TraceData.Trace(TraceEventType.Warning, TraceData.ElementNameQueryTemplate(new object[]
						{
							this._name,
							TraceData.Identify(d)
						}));
					}
				}
				if (obj == null)
				{
					args.NameResolvedInOuterScope = true;
				}
			}
			FrameworkObject frameworkObject = new FrameworkObject(d);
			while (obj == null && frameworkObject.DO != null)
			{
				DependencyObject dependencyObject;
				obj = frameworkObject.FindName(this._name, out dependencyObject);
				if (d == dependencyObject && d is IComponentConnector && d.ReadLocalValue(NavigationService.NavigationServiceProperty) == DependencyProperty.UnsetValue)
				{
					DependencyObject dependencyObject2 = LogicalTreeHelper.GetParent(d);
					if (dependencyObject2 == null)
					{
						dependencyObject2 = Helper.FindMentor(d.InheritanceContext);
					}
					if (dependencyObject2 != null)
					{
						obj = null;
						frameworkObject.Reset(dependencyObject2);
						continue;
					}
				}
				if (args.IsTracing)
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.ElementNameQuery(new object[]
					{
						this._name,
						TraceData.Identify(frameworkObject.DO)
					}));
				}
				if (obj == null)
				{
					args.NameResolvedInOuterScope = true;
					FrameworkObject frameworkObject2 = new FrameworkObject(dependencyObject);
					DependencyObject dependencyObject3 = frameworkObject2.TemplatedParent;
					if (dependencyObject3 == null)
					{
						Panel panel = frameworkObject.FrameworkParent.DO as Panel;
						if (panel != null && panel.IsItemsHost)
						{
							dependencyObject3 = panel;
						}
					}
					if (dependencyObject3 == null && dependencyObject == null)
					{
						ContentControl contentControl = LogicalTreeHelper.GetParent(frameworkObject.DO) as ContentControl;
						if (contentControl != null && contentControl.Content == frameworkObject.DO && contentControl.InheritanceBehavior == InheritanceBehavior.Default)
						{
							dependencyObject3 = contentControl;
						}
					}
					if (dependencyObject3 == null && dependencyObject == null)
					{
						dependencyObject3 = frameworkObject.DO;
						for (;;)
						{
							DependencyObject dependencyObject4 = LogicalTreeHelper.GetParent(dependencyObject3);
							if (dependencyObject4 == null)
							{
								dependencyObject4 = Helper.FindMentor(dependencyObject3.InheritanceContext);
							}
							if (dependencyObject4 == null)
							{
								break;
							}
							dependencyObject3 = dependencyObject4;
						}
						ContentPresenter contentPresenter = VisualTreeHelper.IsVisualType(dependencyObject3) ? (VisualTreeHelper.GetParent(dependencyObject3) as ContentPresenter) : null;
						dependencyObject3 = ((contentPresenter != null && contentPresenter.TemplateInternal.CanBuildVisualTree) ? contentPresenter : null);
					}
					frameworkObject.Reset(dependencyObject3);
				}
			}
			if (obj == null)
			{
				obj = DependencyProperty.UnsetValue;
				args.NameResolvedInOuterScope = false;
			}
			return obj;
		}

		// Token: 0x060075FC RID: 30204 RVA: 0x0021A618 File Offset: 0x00218818
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "ElementName={0}", new object[]
			{
				this._name
			});
		}

		// Token: 0x060075FD RID: 30205 RVA: 0x0021A638 File Offset: 0x00218838
		internal override string Identify()
		{
			return "ElementName";
		}

		// Token: 0x04003851 RID: 14417
		private string _name;
	}
}
