using System;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000708 RID: 1800
	internal class BindingExpressionUncommonField : UncommonField<BindingExpression>
	{
		// Token: 0x0600736C RID: 29548 RVA: 0x00211231 File Offset: 0x0020F431
		internal new void SetValue(DependencyObject instance, BindingExpression bindingExpr)
		{
			base.SetValue(instance, bindingExpr);
			bindingExpr.Attach(instance);
		}

		// Token: 0x0600736D RID: 29549 RVA: 0x00211244 File Offset: 0x0020F444
		internal new void ClearValue(DependencyObject instance)
		{
			BindingExpression value = base.GetValue(instance);
			if (value != null)
			{
				value.Detach();
			}
			base.ClearValue(instance);
		}
	}
}
