using System;
using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000748 RID: 1864
	internal struct SynchronizationInfo
	{
		// Token: 0x060076F1 RID: 30449 RVA: 0x0021FAE4 File Offset: 0x0021DCE4
		public SynchronizationInfo(object context, CollectionSynchronizationCallback callback)
		{
			if (callback == null)
			{
				this._context = context;
				this._callbackMethod = null;
				this._callbackTarget = null;
				return;
			}
			this._context = new WeakReference(context);
			this._callbackMethod = callback.Method;
			object target = callback.Target;
			this._callbackTarget = ((target != null) ? new WeakReference(target) : ViewManager.StaticWeakRef);
		}

		// Token: 0x17001C48 RID: 7240
		// (get) Token: 0x060076F2 RID: 30450 RVA: 0x0021FB3F File Offset: 0x0021DD3F
		public bool IsSynchronized
		{
			get
			{
				return this._context != null || this._callbackMethod != null;
			}
		}

		// Token: 0x060076F3 RID: 30451 RVA: 0x0021FB58 File Offset: 0x0021DD58
		public void AccessCollection(IEnumerable collection, Action accessMethod, bool writeAccess)
		{
			if (!(this._callbackMethod != null))
			{
				if (this._context != null)
				{
					object context = this._context;
					lock (context)
					{
						accessMethod();
						return;
					}
				}
				accessMethod();
				return;
			}
			object obj = this._callbackTarget.Target;
			if (obj == null)
			{
				throw new InvalidOperationException(SR.Get("CollectionView_MissingSynchronizationCallback", new object[]
				{
					collection
				}));
			}
			if (this._callbackTarget == ViewManager.StaticWeakRef)
			{
				obj = null;
			}
			WeakReference weakReference = this._context as WeakReference;
			object obj2 = (weakReference != null) ? weakReference.Target : this._context;
			this._callbackMethod.Invoke(obj, new object[]
			{
				collection,
				obj2,
				accessMethod,
				writeAccess
			});
		}

		// Token: 0x17001C49 RID: 7241
		// (get) Token: 0x060076F4 RID: 30452 RVA: 0x0021FC38 File Offset: 0x0021DE38
		public bool IsAlive
		{
			get
			{
				return (this._callbackMethod != null && this._callbackTarget.IsAlive) || (this._callbackMethod == null && this._context != null);
			}
		}

		// Token: 0x0400389F RID: 14495
		public static readonly SynchronizationInfo None = new SynchronizationInfo(null, null);

		// Token: 0x040038A0 RID: 14496
		private object _context;

		// Token: 0x040038A1 RID: 14497
		private MethodInfo _callbackMethod;

		// Token: 0x040038A2 RID: 14498
		private WeakReference _callbackTarget;
	}
}
