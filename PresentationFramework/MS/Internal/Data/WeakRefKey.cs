using System;

namespace MS.Internal.Data
{
	// Token: 0x02000744 RID: 1860
	internal struct WeakRefKey
	{
		// Token: 0x060076DD RID: 30429 RVA: 0x0021F86E File Offset: 0x0021DA6E
		internal WeakRefKey(object target)
		{
			this._weakRef = new WeakReference(target);
			this._hashCode = ((target != null) ? target.GetHashCode() : 314159);
		}

		// Token: 0x17001C41 RID: 7233
		// (get) Token: 0x060076DE RID: 30430 RVA: 0x0021F892 File Offset: 0x0021DA92
		internal object Target
		{
			get
			{
				return this._weakRef.Target;
			}
		}

		// Token: 0x060076DF RID: 30431 RVA: 0x0021F89F File Offset: 0x0021DA9F
		public override int GetHashCode()
		{
			return this._hashCode;
		}

		// Token: 0x060076E0 RID: 30432 RVA: 0x0021F8A8 File Offset: 0x0021DAA8
		public override bool Equals(object o)
		{
			if (!(o is WeakRefKey))
			{
				return false;
			}
			WeakRefKey weakRefKey = (WeakRefKey)o;
			object target = this.Target;
			object target2 = weakRefKey.Target;
			if (target != null && target2 != null)
			{
				return target == target2;
			}
			return this._weakRef == weakRefKey._weakRef;
		}

		// Token: 0x060076E1 RID: 30433 RVA: 0x0021F8EE File Offset: 0x0021DAEE
		public static bool operator ==(WeakRefKey left, WeakRefKey right)
		{
			if (left == null)
			{
				return right == null;
			}
			return left.Equals(right);
		}

		// Token: 0x060076E2 RID: 30434 RVA: 0x0021F915 File Offset: 0x0021DB15
		public static bool operator !=(WeakRefKey left, WeakRefKey right)
		{
			return !(left == right);
		}

		// Token: 0x04003898 RID: 14488
		private WeakReference _weakRef;

		// Token: 0x04003899 RID: 14489
		private int _hashCode;
	}
}
