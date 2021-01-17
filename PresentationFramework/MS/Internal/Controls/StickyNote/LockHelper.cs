using System;

namespace MS.Internal.Controls.StickyNote
{
	// Token: 0x02000768 RID: 1896
	internal class LockHelper
	{
		// Token: 0x06007864 RID: 30820 RVA: 0x00224F1D File Offset: 0x0022311D
		public bool IsLocked(LockHelper.LockFlag flag)
		{
			return (this._backingStore & flag) > (LockHelper.LockFlag)0;
		}

		// Token: 0x06007865 RID: 30821 RVA: 0x00224F2A File Offset: 0x0022312A
		private void Lock(LockHelper.LockFlag flag)
		{
			this._backingStore |= flag;
		}

		// Token: 0x06007866 RID: 30822 RVA: 0x00224F3A File Offset: 0x0022313A
		private void Unlock(LockHelper.LockFlag flag)
		{
			this._backingStore &= ~flag;
		}

		// Token: 0x04003907 RID: 14599
		private LockHelper.LockFlag _backingStore;

		// Token: 0x02000B69 RID: 2921
		[Flags]
		public enum LockFlag
		{
			// Token: 0x04004B43 RID: 19267
			AnnotationChanged = 1,
			// Token: 0x04004B44 RID: 19268
			DataChanged = 2
		}

		// Token: 0x02000B6A RID: 2922
		public class AutoLocker : IDisposable
		{
			// Token: 0x06008E0D RID: 36365 RVA: 0x0025B110 File Offset: 0x00259310
			public AutoLocker(LockHelper helper, LockHelper.LockFlag flag)
			{
				if (helper == null)
				{
					throw new ArgumentNullException("helper");
				}
				this._helper = helper;
				this._flag = flag;
				this._helper.Lock(this._flag);
			}

			// Token: 0x06008E0E RID: 36366 RVA: 0x0025B145 File Offset: 0x00259345
			public void Dispose()
			{
				this._helper.Unlock(this._flag);
				GC.SuppressFinalize(this);
			}

			// Token: 0x06008E0F RID: 36367 RVA: 0x0000326D File Offset: 0x0000146D
			private AutoLocker()
			{
			}

			// Token: 0x04004B45 RID: 19269
			private LockHelper _helper;

			// Token: 0x04004B46 RID: 19270
			private LockHelper.LockFlag _flag;
		}
	}
}
