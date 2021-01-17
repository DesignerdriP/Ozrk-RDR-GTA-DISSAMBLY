using System;

namespace System.Windows.Markup
{
	// Token: 0x020001CB RID: 459
	internal struct AssemblyInfoKey
	{
		// Token: 0x06001D72 RID: 7538 RVA: 0x00089508 File Offset: 0x00087708
		public override bool Equals(object o)
		{
			if (!(o is AssemblyInfoKey))
			{
				return false;
			}
			AssemblyInfoKey assemblyInfoKey = (AssemblyInfoKey)o;
			if (assemblyInfoKey.AssemblyFullName == null)
			{
				return this.AssemblyFullName == null;
			}
			return assemblyInfoKey.AssemblyFullName.Equals(this.AssemblyFullName);
		}

		// Token: 0x06001D73 RID: 7539 RVA: 0x00089549 File Offset: 0x00087749
		public static bool operator ==(AssemblyInfoKey key1, AssemblyInfoKey key2)
		{
			return key1.Equals(key2);
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x0008955E File Offset: 0x0008775E
		public static bool operator !=(AssemblyInfoKey key1, AssemblyInfoKey key2)
		{
			return !key1.Equals(key2);
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x00089576 File Offset: 0x00087776
		public override int GetHashCode()
		{
			if (this.AssemblyFullName == null)
			{
				return 0;
			}
			return this.AssemblyFullName.GetHashCode();
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x0008958D File Offset: 0x0008778D
		public override string ToString()
		{
			return this.AssemblyFullName;
		}

		// Token: 0x0400142F RID: 5167
		internal string AssemblyFullName;
	}
}
