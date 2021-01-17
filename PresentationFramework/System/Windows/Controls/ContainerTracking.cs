using System;
using System.Diagnostics;

namespace System.Windows.Controls
{
	// Token: 0x02000487 RID: 1159
	internal class ContainerTracking<T>
	{
		// Token: 0x060043C4 RID: 17348 RVA: 0x00135A05 File Offset: 0x00133C05
		internal ContainerTracking(T container)
		{
			this._container = container;
		}

		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x060043C5 RID: 17349 RVA: 0x00135A14 File Offset: 0x00133C14
		internal T Container
		{
			get
			{
				return this._container;
			}
		}

		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x060043C6 RID: 17350 RVA: 0x00135A1C File Offset: 0x00133C1C
		internal ContainerTracking<T> Next
		{
			get
			{
				return this._next;
			}
		}

		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x060043C7 RID: 17351 RVA: 0x00135A24 File Offset: 0x00133C24
		internal ContainerTracking<T> Previous
		{
			get
			{
				return this._previous;
			}
		}

		// Token: 0x060043C8 RID: 17352 RVA: 0x00135A2C File Offset: 0x00133C2C
		internal void StartTracking(ref ContainerTracking<T> root)
		{
			if (root != null)
			{
				root._previous = this;
			}
			this._next = root;
			root = this;
		}

		// Token: 0x060043C9 RID: 17353 RVA: 0x00135A48 File Offset: 0x00133C48
		internal void StopTracking(ref ContainerTracking<T> root)
		{
			if (this._previous != null)
			{
				this._previous._next = this._next;
			}
			if (this._next != null)
			{
				this._next._previous = this._previous;
			}
			if (root == this)
			{
				root = this._next;
			}
			this._previous = null;
			this._next = null;
		}

		// Token: 0x060043CA RID: 17354 RVA: 0x00002137 File Offset: 0x00000337
		[Conditional("DEBUG")]
		internal void Debug_AssertIsInList(ContainerTracking<T> root)
		{
		}

		// Token: 0x060043CB RID: 17355 RVA: 0x00002137 File Offset: 0x00000337
		[Conditional("DEBUG")]
		internal void Debug_AssertNotInList(ContainerTracking<T> root)
		{
		}

		// Token: 0x04002861 RID: 10337
		private T _container;

		// Token: 0x04002862 RID: 10338
		private ContainerTracking<T> _next;

		// Token: 0x04002863 RID: 10339
		private ContainerTracking<T> _previous;
	}
}
