using System;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000733 RID: 1843
	internal abstract class ObjectRef
	{
		// Token: 0x060075F3 RID: 30195 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual object GetObject(DependencyObject d, ObjectRefArgs args)
		{
			return null;
		}

		// Token: 0x060075F4 RID: 30196 RVA: 0x0021A3B1 File Offset: 0x002185B1
		internal virtual object GetDataObject(DependencyObject d, ObjectRefArgs args)
		{
			return this.GetObject(d, args);
		}

		// Token: 0x060075F5 RID: 30197 RVA: 0x0021A3BB File Offset: 0x002185BB
		internal bool TreeContextIsRequired(DependencyObject target)
		{
			return this.ProtectedTreeContextIsRequired(target);
		}

		// Token: 0x060075F6 RID: 30198 RVA: 0x0000B02A File Offset: 0x0000922A
		protected virtual bool ProtectedTreeContextIsRequired(DependencyObject target)
		{
			return false;
		}

		// Token: 0x17001C19 RID: 7193
		// (get) Token: 0x060075F7 RID: 30199 RVA: 0x0021A3C4 File Offset: 0x002185C4
		internal bool UsesMentor
		{
			get
			{
				return this.ProtectedUsesMentor;
			}
		}

		// Token: 0x17001C1A RID: 7194
		// (get) Token: 0x060075F8 RID: 30200 RVA: 0x00016748 File Offset: 0x00014948
		protected virtual bool ProtectedUsesMentor
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060075F9 RID: 30201
		internal abstract string Identify();
	}
}
