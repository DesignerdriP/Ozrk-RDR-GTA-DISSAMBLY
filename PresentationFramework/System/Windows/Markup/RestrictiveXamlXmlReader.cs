using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Xaml;
using System.Xml;

namespace System.Windows.Markup
{
	// Token: 0x02000229 RID: 553
	internal class RestrictiveXamlXmlReader : XamlXmlReader
	{
		// Token: 0x06002236 RID: 8758 RVA: 0x000AA344 File Offset: 0x000A8544
		static RestrictiveXamlXmlReader()
		{
			RestrictiveXamlXmlReader._unloadedTypes = new ConcurrentDictionary<string, List<RestrictiveXamlXmlReader.RestrictedType>>();
			foreach (RestrictiveXamlXmlReader.RestrictedType restrictedType in RestrictiveXamlXmlReader._restrictedTypes)
			{
				if (!string.IsNullOrEmpty(restrictedType.AssemblyName))
				{
					if (!RestrictiveXamlXmlReader._unloadedTypes.ContainsKey(restrictedType.AssemblyName))
					{
						RestrictiveXamlXmlReader._unloadedTypes[restrictedType.AssemblyName] = new List<RestrictiveXamlXmlReader.RestrictedType>();
					}
					RestrictiveXamlXmlReader._unloadedTypes[restrictedType.AssemblyName].Add(restrictedType);
				}
				else
				{
					Type type = System.Type.GetType(restrictedType.TypeName, false);
					if (type != null)
					{
						restrictedType.TypeReference = type;
					}
				}
			}
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x000AA47C File Offset: 0x000A867C
		public RestrictiveXamlXmlReader(XmlReader xmlReader, XamlSchemaContext schemaContext, XamlXmlReaderSettings settings) : base(xmlReader, schemaContext, settings)
		{
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x000AA494 File Offset: 0x000A8694
		internal RestrictiveXamlXmlReader(XmlReader xmlReader, XamlSchemaContext schemaContext, XamlXmlReaderSettings settings, List<Type> safeTypes) : base(xmlReader, schemaContext, settings)
		{
			if (safeTypes != null)
			{
				foreach (Type item in safeTypes)
				{
					this._safeTypesSet.Add(item);
				}
			}
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x000AA504 File Offset: 0x000A8704
		public override bool Read()
		{
			int num = 0;
			bool result;
			while (result = base.Read())
			{
				if (num <= 0)
				{
					if (this.NodeType != XamlNodeType.StartObject || !this.IsRestrictedType(this.Type.UnderlyingType))
					{
						break;
					}
					num = 1;
				}
				else if (this.NodeType == XamlNodeType.StartObject || this.NodeType == XamlNodeType.GetObject)
				{
					num++;
				}
				else if (this.NodeType == XamlNodeType.EndObject)
				{
					num--;
				}
			}
			return result;
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x000AA56C File Offset: 0x000A876C
		private bool IsRestrictedType(Type type)
		{
			if (type != null)
			{
				if (this._safeTypesSet.Contains(type))
				{
					return false;
				}
				RestrictiveXamlXmlReader.EnsureLatestAssemblyLoadInformation();
				foreach (RestrictiveXamlXmlReader.RestrictedType restrictedType in RestrictiveXamlXmlReader._restrictedTypes)
				{
					Type typeReference = restrictedType.TypeReference;
					if (typeReference != null && typeReference.IsAssignableFrom(type))
					{
						return true;
					}
				}
				this._safeTypesSet.Add(type);
				return false;
			}
			return false;
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x000AA600 File Offset: 0x000A8800
		private static void EnsureLatestAssemblyLoadInformation()
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			if (assemblies.Length != RestrictiveXamlXmlReader._loadedAssembliesCount)
			{
				foreach (Assembly assembly in assemblies)
				{
					RestrictiveXamlXmlReader.RegisterAssembly(assembly);
				}
				RestrictiveXamlXmlReader._loadedAssembliesCount = assemblies.Length;
			}
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x000AA644 File Offset: 0x000A8844
		private static void RegisterAssembly(Assembly assembly)
		{
			if (assembly != null)
			{
				string fullName = assembly.FullName;
				List<RestrictiveXamlXmlReader.RestrictedType> list = null;
				if (RestrictiveXamlXmlReader._unloadedTypes.TryGetValue(fullName, out list))
				{
					if (list != null)
					{
						foreach (RestrictiveXamlXmlReader.RestrictedType restrictedType in list)
						{
							Type type = assembly.GetType(restrictedType.TypeName, false);
							restrictedType.TypeReference = type;
						}
					}
					RestrictiveXamlXmlReader._unloadedTypes.TryRemove(fullName, out list);
				}
			}
		}

		// Token: 0x040019D8 RID: 6616
		private static List<RestrictiveXamlXmlReader.RestrictedType> _restrictedTypes = new List<RestrictiveXamlXmlReader.RestrictedType>
		{
			new RestrictiveXamlXmlReader.RestrictedType("System.Windows.Data.ObjectDataProvider", ""),
			new RestrictiveXamlXmlReader.RestrictedType("System.Windows.ResourceDictionary", ""),
			new RestrictiveXamlXmlReader.RestrictedType("System.Configuration.Install.AssemblyInstaller", "System.Configuration.Install, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"),
			new RestrictiveXamlXmlReader.RestrictedType("System.Activities.Presentation.WorkflowDesigner", "System.Activities.Presentation, Version = 4.0.0.0, Culture = neutral, PublicKeyToken = 31bf3856ad364e35"),
			new RestrictiveXamlXmlReader.RestrictedType("System.Windows.Forms.BindingSource", "System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
		};

		// Token: 0x040019D9 RID: 6617
		private static ConcurrentDictionary<string, List<RestrictiveXamlXmlReader.RestrictedType>> _unloadedTypes = null;

		// Token: 0x040019DA RID: 6618
		private HashSet<Type> _safeTypesSet = new HashSet<Type>();

		// Token: 0x040019DB RID: 6619
		[ThreadStatic]
		private static int _loadedAssembliesCount;

		// Token: 0x02000897 RID: 2199
		private class RestrictedType
		{
			// Token: 0x06008395 RID: 33685 RVA: 0x00245ADE File Offset: 0x00243CDE
			public RestrictedType(string typeName, string assemblyName)
			{
				this.TypeName = typeName;
				this.AssemblyName = assemblyName;
			}

			// Token: 0x17001DD6 RID: 7638
			// (get) Token: 0x06008396 RID: 33686 RVA: 0x00245AF4 File Offset: 0x00243CF4
			// (set) Token: 0x06008397 RID: 33687 RVA: 0x00245AFC File Offset: 0x00243CFC
			public string TypeName { get; set; }

			// Token: 0x17001DD7 RID: 7639
			// (get) Token: 0x06008398 RID: 33688 RVA: 0x00245B05 File Offset: 0x00243D05
			// (set) Token: 0x06008399 RID: 33689 RVA: 0x00245B0D File Offset: 0x00243D0D
			public string AssemblyName { get; set; }

			// Token: 0x17001DD8 RID: 7640
			// (get) Token: 0x0600839A RID: 33690 RVA: 0x00245B16 File Offset: 0x00243D16
			// (set) Token: 0x0600839B RID: 33691 RVA: 0x00245B1E File Offset: 0x00243D1E
			public Type TypeReference { get; set; }
		}
	}
}
