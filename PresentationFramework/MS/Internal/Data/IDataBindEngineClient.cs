using System;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000714 RID: 1812
	internal interface IDataBindEngineClient
	{
		// Token: 0x0600749F RID: 29855
		void TransferValue();

		// Token: 0x060074A0 RID: 29856
		void UpdateValue();

		// Token: 0x060074A1 RID: 29857
		bool AttachToContext(bool lastChance);

		// Token: 0x060074A2 RID: 29858
		void VerifySourceReference(bool lastChance);

		// Token: 0x060074A3 RID: 29859
		void OnTargetUpdated();

		// Token: 0x17001BC0 RID: 7104
		// (get) Token: 0x060074A4 RID: 29860
		DependencyObject TargetElement { get; }
	}
}
