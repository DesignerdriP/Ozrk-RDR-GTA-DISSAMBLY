using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using MS.Internal.Interop;
using MS.Win32;

namespace System.Windows.Interop
{
	/// <summary>Enables calls from a XAML browser application (XBAP) to the HTML window that hosts the application. </summary>
	// Token: 0x020005BD RID: 1469
	public sealed class DynamicScriptObject : DynamicObject
	{
		// Token: 0x060061E0 RID: 25056 RVA: 0x001B72FC File Offset: 0x001B54FC
		[SecurityCritical]
		internal DynamicScriptObject(UnsafeNativeMethods.IDispatch scriptObject)
		{
			if (scriptObject == null)
			{
				throw new ArgumentNullException("scriptObject");
			}
			this._scriptObject = scriptObject;
			this._scriptObjectEx = (this._scriptObject as UnsafeNativeMethods.IDispatchEx);
		}

		/// <summary>Calls a method on the script object. </summary>
		/// <param name="binder">The binder provided by the call site.</param>
		/// <param name="args">The arguments to pass to the default method.</param>
		/// <param name="result">The method result.</param>
		/// <returns>Always return <see langword="true" />. </returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="binder" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMethodException">The method does not exist.</exception>
		// Token: 0x060061E1 RID: 25057 RVA: 0x001B7335 File Offset: 0x001B5535
		public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
		{
			if (binder == null)
			{
				throw new ArgumentNullException("binder");
			}
			result = this.InvokeAndReturn(binder.Name, 1, args);
			return true;
		}

		/// <summary>Gets an member value from the script object.</summary>
		/// <param name="binder">The binder provided by the call site.</param>
		/// <param name="result">The method result.</param>
		/// <returns>Always returns<see langword=" true" />. </returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="binder" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMemberException">The member does not exist.</exception>
		// Token: 0x060061E2 RID: 25058 RVA: 0x001B7356 File Offset: 0x001B5556
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			if (binder == null)
			{
				throw new ArgumentNullException("binder");
			}
			result = this.InvokeAndReturn(binder.Name, 2, null);
			return true;
		}

		/// <summary>Sets a member on the script object to the specified value.</summary>
		/// <param name="binder">The binder provided by the call site.</param>
		/// <param name="value">The value to set for the member.</param>
		/// <returns>Always returns <see langword="true" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="binder" /> is <see langword="null" />.-or-
		///         <paramref name="indexes" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="indexes" /> is not equal to 1.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The first <paramref name="indexes" /> value is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMemberException">The member does not exist.</exception>
		// Token: 0x060061E3 RID: 25059 RVA: 0x001B7378 File Offset: 0x001B5578
		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			if (binder == null)
			{
				throw new ArgumentNullException("binder");
			}
			int propertyPutMethod = DynamicScriptObject.GetPropertyPutMethod(value);
			object obj = this.InvokeAndReturn(binder.Name, propertyPutMethod, new object[]
			{
				value
			});
			return true;
		}

		/// <summary>Gets an indexed value from the script object by using the first index value from the <paramref name="indexes" /> collection.</summary>
		/// <param name="binder">The binder provided by the call site.</param>
		/// <param name="indexes">The index to be retrieved.</param>
		/// <param name="result">The method result.</param>
		/// <returns>Always returns <see langword="true" />. </returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="binder" /> is <see langword="null" />.-or-
		///         <paramref name="indexes" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="indexes" /> is not equal to 1.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The first <paramref name="indexes" /> value is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMemberException">The member does not exist.</exception>
		// Token: 0x060061E4 RID: 25060 RVA: 0x001B73B4 File Offset: 0x001B55B4
		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			if (binder == null)
			{
				throw new ArgumentNullException("binder");
			}
			if (indexes == null)
			{
				throw new ArgumentNullException("indexes");
			}
			if (BrowserInteropHelper.IsHostedInIEorWebOC && this.TryFindMemberAndInvoke(null, 1, false, indexes, out result))
			{
				return true;
			}
			if (indexes.Length != 1)
			{
				throw new ArgumentException("indexes", HRESULT.DISP_E_BADPARAMCOUNT.GetException());
			}
			object obj = indexes[0];
			if (obj == null)
			{
				throw new ArgumentOutOfRangeException("indexes");
			}
			result = this.InvokeAndReturn(obj.ToString(), 2, false, null);
			return true;
		}

		/// <summary>Sets a member on the script object by using the first index specified in the <paramref name="indexes" /> collection.</summary>
		/// <param name="binder">The binder provided by the call site.</param>
		/// <param name="indexes">The index to be retrieved.</param>
		/// <param name="value">The method result</param>
		/// <returns>Always returns <see langword="true" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="binder" /> is <see langword="null" />.-or-
		///         <paramref name="indexes" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="indexes" /> is not equal to 1.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The first <paramref name="indexes" /> value is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMemberException">The member does not exist.</exception>
		// Token: 0x060061E5 RID: 25061 RVA: 0x001B7438 File Offset: 0x001B5638
		public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
		{
			if (binder == null)
			{
				throw new ArgumentNullException("binder");
			}
			if (indexes == null)
			{
				throw new ArgumentNullException("indexes");
			}
			if (indexes.Length != 1)
			{
				throw new ArgumentException("indexes", HRESULT.DISP_E_BADPARAMCOUNT.GetException());
			}
			object obj = indexes[0];
			if (obj == null)
			{
				throw new ArgumentOutOfRangeException("indexes");
			}
			object obj2 = this.InvokeAndReturn(obj.ToString(), 4, false, new object[]
			{
				value
			});
			return true;
		}

		/// <summary>Calls the default script method.</summary>
		/// <param name="binder">The binder provided by the call site.</param>
		/// <param name="args">The arguments to pass to the default method.</param>
		/// <param name="result">The method result.</param>
		/// <returns>Always return <see langword="true" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="binder" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMethodException">The method does not exist.</exception>
		// Token: 0x060061E6 RID: 25062 RVA: 0x001B74AB File Offset: 0x001B56AB
		public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
		{
			if (binder == null)
			{
				throw new ArgumentNullException("binder");
			}
			result = this.InvokeAndReturn(null, 1, args);
			return true;
		}

		/// <summary>Attempts to convert the script object to a string representation.</summary>
		/// <returns>A string representation of the script object, if the object can be converted; otherwise, a string representation of the object's default property or method.</returns>
		// Token: 0x060061E7 RID: 25063 RVA: 0x001B74C8 File Offset: 0x001B56C8
		public override string ToString()
		{
			Guid empty = Guid.Empty;
			object obj = null;
			NativeMethods.DISPPARAMS dp = new NativeMethods.DISPPARAMS();
			int dispid;
			HRESULT hresult;
			if (this.TryGetDispIdForMember("toString", true, out dispid))
			{
				hresult = this.InvokeOnScriptObject(dispid, 1, dp, null, out obj);
			}
			else
			{
				dispid = 0;
				hresult = this.InvokeOnScriptObject(dispid, 2, dp, null, out obj);
				if (hresult.Failed)
				{
					hresult = this.InvokeOnScriptObject(dispid, 1, dp, null, out obj);
				}
			}
			if (hresult.Succeeded && obj != null)
			{
				return obj.ToString();
			}
			return base.ToString();
		}

		// Token: 0x1700178B RID: 6027
		// (get) Token: 0x060061E8 RID: 25064 RVA: 0x001B7544 File Offset: 0x001B5744
		internal UnsafeNativeMethods.IDispatch ScriptObject
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				return this._scriptObject;
			}
		}

		// Token: 0x060061E9 RID: 25065 RVA: 0x001B754C File Offset: 0x001B574C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal unsafe bool TryFindMemberAndInvokeNonWrapped(string memberName, int flags, bool cacheDispId, object[] args, out object result)
		{
			result = null;
			int dispid;
			if (!this.TryGetDispIdForMember(memberName, cacheDispId, out dispid))
			{
				return false;
			}
			NativeMethods.DISPPARAMS dispparams = new NativeMethods.DISPPARAMS();
			int num = -3;
			if (flags == 4 || flags == 8)
			{
				dispparams.cNamedArgs = 1U;
				dispparams.rgdispidNamedArgs = new IntPtr((void*)(&num));
			}
			try
			{
				if (args != null)
				{
					args = (object[])args.Clone();
					Array.Reverse(args);
					for (int i = 0; i < args.Length; i++)
					{
						DynamicScriptObject dynamicScriptObject = args[i] as DynamicScriptObject;
						if (dynamicScriptObject != null)
						{
							args[i] = dynamicScriptObject._scriptObject;
						}
						if (args[i] != null)
						{
							Type type = args[i].GetType();
							if (type.IsArray)
							{
								type = type.GetElementType();
							}
							if (!Marshal.IsTypeVisibleFromCom(type) && !type.IsCOMObject && type != typeof(DateTime))
							{
								throw new ArgumentException(SR.Get("NeedToBeComVisible"));
							}
						}
					}
					dispparams.rgvarg = UnsafeNativeMethods.ArrayToVARIANTHelper.ArrayToVARIANTVector(args);
					dispparams.cArgs = (uint)args.Length;
				}
				NativeMethods.EXCEPINFO excepinfo = new NativeMethods.EXCEPINFO();
				HRESULT hrLeft = this.InvokeOnScriptObject(dispid, flags, dispparams, excepinfo, out result);
				if (hrLeft.Failed)
				{
					if (hrLeft == HRESULT.DISP_E_MEMBERNOTFOUND)
					{
						return false;
					}
					if (hrLeft == HRESULT.SCRIPT_E_REPORTED)
					{
						excepinfo.scode = hrLeft.Code;
						hrLeft = HRESULT.DISP_E_EXCEPTION;
					}
					string text = "[" + (memberName ?? "(default)") + "]";
					Exception exception = hrLeft.GetException();
					if (hrLeft == HRESULT.DISP_E_EXCEPTION)
					{
						int code = (excepinfo.scode != 0) ? excepinfo.scode : ((int)excepinfo.wCode);
						hrLeft = HRESULT.Make(true, Facility.Dispatch, code);
						string message = text + " " + (excepinfo.bstrDescription ?? string.Empty);
						throw new TargetInvocationException(message, exception)
						{
							HelpLink = excepinfo.bstrHelpFile,
							Source = excepinfo.bstrSource
						};
					}
					if (hrLeft == HRESULT.DISP_E_BADPARAMCOUNT || hrLeft == HRESULT.DISP_E_PARAMNOTOPTIONAL)
					{
						throw new TargetParameterCountException(text, exception);
					}
					if (hrLeft == HRESULT.DISP_E_OVERFLOW || hrLeft == HRESULT.DISP_E_TYPEMISMATCH)
					{
						throw new ArgumentException(text, new InvalidCastException(exception.Message, hrLeft.Code));
					}
					throw exception;
				}
			}
			finally
			{
				if (dispparams.rgvarg != IntPtr.Zero)
				{
					UnsafeNativeMethods.ArrayToVARIANTHelper.FreeVARIANTVector(dispparams.rgvarg, args.Length);
				}
			}
			return true;
		}

		// Token: 0x060061EA RID: 25066 RVA: 0x001B77E4 File Offset: 0x001B59E4
		private object InvokeAndReturn(string memberName, int flags, object[] args)
		{
			return this.InvokeAndReturn(memberName, flags, true, args);
		}

		// Token: 0x060061EB RID: 25067 RVA: 0x001B77F0 File Offset: 0x001B59F0
		private object InvokeAndReturn(string memberName, int flags, bool cacheDispId, object[] args)
		{
			object result;
			if (this.TryFindMemberAndInvoke(memberName, flags, cacheDispId, args, out result))
			{
				return result;
			}
			if (flags == 1)
			{
				throw new MissingMethodException(this.ToString(), memberName);
			}
			throw new MissingMemberException(this.ToString(), memberName);
		}

		// Token: 0x060061EC RID: 25068 RVA: 0x001B782B File Offset: 0x001B5A2B
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool TryFindMemberAndInvoke(string memberName, int flags, bool cacheDispId, object[] args, out object result)
		{
			if (!this.TryFindMemberAndInvokeNonWrapped(memberName, flags, cacheDispId, args, out result))
			{
				return false;
			}
			if (result != null && Marshal.IsComObject(result))
			{
				result = new DynamicScriptObject((UnsafeNativeMethods.IDispatch)result);
			}
			return true;
		}

		// Token: 0x060061ED RID: 25069 RVA: 0x001B7860 File Offset: 0x001B5A60
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool TryGetDispIdForMember(string memberName, bool cacheDispId, out int dispid)
		{
			dispid = 0;
			if (!string.IsNullOrEmpty(memberName) && (!cacheDispId || !this._dispIdCache.TryGetValue(memberName, out dispid)))
			{
				Guid empty = Guid.Empty;
				string[] rgszNames = new string[]
				{
					memberName
				};
				int[] array = new int[]
				{
					-1
				};
				HRESULT idsOfNames = this._scriptObject.GetIDsOfNames(ref empty, rgszNames, array.Length, Thread.CurrentThread.CurrentCulture.LCID, array);
				if (idsOfNames == HRESULT.DISP_E_UNKNOWNNAME)
				{
					return false;
				}
				idsOfNames.ThrowIfFailed();
				dispid = array[0];
				if (cacheDispId)
				{
					this._dispIdCache[memberName] = dispid;
				}
			}
			return true;
		}

		// Token: 0x060061EE RID: 25070 RVA: 0x001B78F8 File Offset: 0x001B5AF8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private HRESULT InvokeOnScriptObject(int dispid, int flags, NativeMethods.DISPPARAMS dp, NativeMethods.EXCEPINFO exInfo, out object result)
		{
			if (this._scriptObjectEx != null)
			{
				return this._scriptObjectEx.InvokeEx(dispid, Thread.CurrentThread.CurrentCulture.LCID, flags, dp, out result, exInfo, BrowserInteropHelper.HostHtmlDocumentServiceProvider);
			}
			Guid empty = Guid.Empty;
			return this._scriptObject.Invoke(dispid, ref empty, Thread.CurrentThread.CurrentCulture.LCID, flags, dp, out result, exInfo, null);
		}

		// Token: 0x060061EF RID: 25071 RVA: 0x001B7960 File Offset: 0x001B5B60
		private static int GetPropertyPutMethod(object value)
		{
			if (value == null)
			{
				return 8;
			}
			Type type = value.GetType();
			if (type.IsValueType || type.IsArray || type == typeof(string) || type == typeof(CurrencyWrapper) || type == typeof(DBNull) || type == typeof(Missing))
			{
				return 4;
			}
			return 8;
		}

		// Token: 0x04003178 RID: 12664
		[SecurityCritical]
		private UnsafeNativeMethods.IDispatch _scriptObject;

		// Token: 0x04003179 RID: 12665
		[SecurityCritical]
		private UnsafeNativeMethods.IDispatchEx _scriptObjectEx;

		// Token: 0x0400317A RID: 12666
		private Dictionary<string, int> _dispIdCache = new Dictionary<string, int>();
	}
}
