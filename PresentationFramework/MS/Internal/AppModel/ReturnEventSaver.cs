using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000799 RID: 1945
	[Serializable]
	internal class ReturnEventSaver
	{
		// Token: 0x060079D9 RID: 31193 RVA: 0x0000326D File Offset: 0x0000146D
		internal ReturnEventSaver()
		{
		}

		// Token: 0x060079DA RID: 31194 RVA: 0x002286A4 File Offset: 0x002268A4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void _Detach(PageFunctionBase pf)
		{
			if (pf._Return != null && pf._Saver == null)
			{
				Delegate[] invocationList = pf._Return.GetInvocationList();
				ReturnEventSaverInfo[] array = this._returnList = new ReturnEventSaverInfo[invocationList.Length];
				for (int i = 0; i < invocationList.Length; i++)
				{
					Delegate @delegate = invocationList[i];
					bool fSamePf = false;
					if (@delegate.Target == pf)
					{
						fSamePf = true;
					}
					MethodInfo method = @delegate.Method;
					ReturnEventSaverInfo returnEventSaverInfo = new ReturnEventSaverInfo(@delegate.GetType().AssemblyQualifiedName, @delegate.Target.GetType().AssemblyQualifiedName, method.Name, fSamePf);
					array[i] = returnEventSaverInfo;
				}
				pf._Saver = this;
			}
			pf._DetachEvents();
		}

		// Token: 0x060079DB RID: 31195 RVA: 0x00228758 File Offset: 0x00226958
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void _Attach(object caller, PageFunctionBase child)
		{
			ReturnEventSaverInfo[] array = null;
			array = this._returnList;
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (string.Compare(this._returnList[i]._targetTypeName, caller.GetType().AssemblyQualifiedName, StringComparison.Ordinal) != 0)
					{
						throw new NotSupportedException(SR.Get("ReturnEventHandlerMustBeOnParentPage"));
					}
					Delegate d;
					try
					{
						new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Assert();
						d = Delegate.CreateDelegate(Type.GetType(this._returnList[i]._delegateTypeName), caller, this._returnList[i]._delegateMethodName);
					}
					catch (Exception innerException)
					{
						throw new NotSupportedException(SR.Get("ReturnEventHandlerMustBeOnParentPage"), innerException);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					child._AddEventHandler(d);
				}
			}
		}

		// Token: 0x040039A3 RID: 14755
		[SecurityCritical]
		private ReturnEventSaverInfo[] _returnList;
	}
}
