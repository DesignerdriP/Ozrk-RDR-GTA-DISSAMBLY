﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.Interop
{
	// Token: 0x02000670 RID: 1648
	internal abstract class InternalDispatchObject<IDispInterface> : IReflect
	{
		// Token: 0x06006CD7 RID: 27863 RVA: 0x001F5140 File Offset: 0x001F3340
		[SecurityCritical]
		protected InternalDispatchObject()
		{
			MethodInfo[] methods = typeof(IDispInterface).GetMethods();
			this._dispId2MethodMap = new Dictionary<int, MethodInfo>(methods.Length);
			foreach (MethodInfo methodInfo in methods)
			{
				int value = ((DispIdAttribute[])methodInfo.GetCustomAttributes(typeof(DispIdAttribute), false))[0].Value;
				this._dispId2MethodMap[value] = methodInfo;
			}
		}

		// Token: 0x06006CD8 RID: 27864 RVA: 0x0003E264 File Offset: 0x0003C464
		FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06006CD9 RID: 27865 RVA: 0x0000C238 File Offset: 0x0000A438
		FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
		{
			return null;
		}

		// Token: 0x06006CDA RID: 27866 RVA: 0x0003E264 File Offset: 0x0003C464
		MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06006CDB RID: 27867 RVA: 0x0003E264 File Offset: 0x0003C464
		MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06006CDC RID: 27868 RVA: 0x0003E264 File Offset: 0x0003C464
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06006CDD RID: 27869 RVA: 0x0003E264 File Offset: 0x0003C464
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06006CDE RID: 27870 RVA: 0x0000C238 File Offset: 0x0000A438
		MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
		{
			return null;
		}

		// Token: 0x06006CDF RID: 27871 RVA: 0x0000C238 File Offset: 0x0000A438
		PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
		{
			return null;
		}

		// Token: 0x06006CE0 RID: 27872 RVA: 0x0003E264 File Offset: 0x0003C464
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06006CE1 RID: 27873 RVA: 0x0003E264 File Offset: 0x0003C464
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06006CE2 RID: 27874 RVA: 0x001F51B4 File Offset: 0x001F33B4
		object IReflect.InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			if (name.StartsWith("[DISPID=", StringComparison.OrdinalIgnoreCase))
			{
				int key = int.Parse(name.Substring(8, name.Length - 9), CultureInfo.InvariantCulture);
				MethodInfo methodInfo;
				if (this._dispId2MethodMap.TryGetValue(key, out methodInfo))
				{
					return methodInfo.Invoke(this, invokeAttr, binder, args, culture);
				}
			}
			throw new MissingMethodException(base.GetType().Name, name);
		}

		// Token: 0x17001A03 RID: 6659
		// (get) Token: 0x06006CE3 RID: 27875 RVA: 0x001F5219 File Offset: 0x001F3419
		Type IReflect.UnderlyingSystemType
		{
			get
			{
				return typeof(IDispInterface);
			}
		}

		// Token: 0x04003576 RID: 13686
		private Dictionary<int, MethodInfo> _dispId2MethodMap;
	}
}
