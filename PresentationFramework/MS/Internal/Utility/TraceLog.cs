using System;
using System.Collections;
using System.Globalization;

namespace MS.Internal.Utility
{
	// Token: 0x020007EF RID: 2031
	internal class TraceLog
	{
		// Token: 0x06007D3B RID: 32059 RVA: 0x002330FB File Offset: 0x002312FB
		internal TraceLog() : this(int.MaxValue)
		{
		}

		// Token: 0x06007D3C RID: 32060 RVA: 0x00233108 File Offset: 0x00231308
		internal TraceLog(int size)
		{
			this._size = size;
			this._log = new ArrayList();
		}

		// Token: 0x06007D3D RID: 32061 RVA: 0x00233124 File Offset: 0x00231324
		internal void Add(string message, params object[] args)
		{
			string value = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) + " " + string.Format(CultureInfo.InvariantCulture, message, args);
			if (this._log.Count == this._size)
			{
				this._log.RemoveAt(0);
			}
			this._log.Add(value);
		}

		// Token: 0x06007D3E RID: 32062 RVA: 0x00233190 File Offset: 0x00231390
		internal void WriteLog()
		{
			for (int i = 0; i < this._log.Count; i++)
			{
				Console.WriteLine(this._log[i]);
			}
		}

		// Token: 0x06007D3F RID: 32063 RVA: 0x002331C4 File Offset: 0x002313C4
		internal static string IdFor(object o)
		{
			if (o == null)
			{
				return "NULL";
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[]
			{
				o.GetType().Name,
				o.GetHashCode()
			});
		}

		// Token: 0x04003AEC RID: 15084
		private ArrayList _log;

		// Token: 0x04003AED RID: 15085
		private int _size;
	}
}
