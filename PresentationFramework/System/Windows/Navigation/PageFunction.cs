using System;

namespace System.Windows.Navigation
{
	/// <summary>Represents a special type of page that allows you to treat navigation to a page in a similar fashion to calling a method. </summary>
	/// <typeparam name="T">The type of value that the <see cref="T:System.Windows.Navigation.PageFunction`1" /> returns to a caller.</typeparam>
	// Token: 0x02000323 RID: 803
	public class PageFunction<T> : PageFunctionBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Navigation.PageFunction`1" /> class. </summary>
		// Token: 0x06002A78 RID: 10872 RVA: 0x000C26FF File Offset: 0x000C08FF
		public PageFunction()
		{
			base.RaiseTypedEvent += this.RaiseTypedReturnEvent;
		}

		/// <summary>A <see cref="T:System.Windows.Navigation.PageFunction`1" /> calls <see cref="M:System.Windows.Navigation.PageFunction`1.OnReturn(System.Windows.Navigation.ReturnEventArgs{`0})" /> to return to the caller, passing a return value via a <see cref="T:System.Windows.Navigation.ReturnEventArgs`1" /> object</summary>
		/// <param name="e">A <see cref="T:System.Windows.Navigation.ReturnEventArgs`1" /> object that contains the <see cref="T:System.Windows.Navigation.PageFunction`1" /> return value (<see cref="P:System.Windows.Navigation.ReturnEventArgs`1.Result" />).</param>
		// Token: 0x06002A79 RID: 10873 RVA: 0x000C2719 File Offset: 0x000C0919
		protected virtual void OnReturn(ReturnEventArgs<T> e)
		{
			base._OnReturnUnTyped(e);
		}

		// Token: 0x06002A7A RID: 10874 RVA: 0x000C2724 File Offset: 0x000C0924
		internal void RaiseTypedReturnEvent(PageFunctionBase b, RaiseTypedEventArgs args)
		{
			Delegate d = args.D;
			object o = args.O;
			if (d != null)
			{
				ReturnEventArgs<T> e = o as ReturnEventArgs<T>;
				ReturnEventHandler<T> returnEventHandler = d as ReturnEventHandler<T>;
				returnEventHandler(this, e);
			}
		}

		/// <summary>Occurs when a called <see cref="T:System.Windows.Navigation.PageFunction`1" /> returns, and can only be handled by the calling page.</summary>
		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06002A7B RID: 10875 RVA: 0x000C2758 File Offset: 0x000C0958
		// (remove) Token: 0x06002A7C RID: 10876 RVA: 0x000C2761 File Offset: 0x000C0961
		public event ReturnEventHandler<T> Return
		{
			add
			{
				base._AddEventHandler(value);
			}
			remove
			{
				base._RemoveEventHandler(value);
			}
		}
	}
}
