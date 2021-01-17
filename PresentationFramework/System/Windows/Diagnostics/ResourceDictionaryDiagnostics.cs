using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Utility;

namespace System.Windows.Diagnostics
{
	/// <summary>Enables enumeration of generic and themed <see cref="T:System.Windows.ResourceDictionary" /> instances, and provides 
	///     a notification infrastructure for listening to the loading and unloading of <see cref="T:System.Windows.ResourceDictionary" /> instances.</summary>
	// Token: 0x02000193 RID: 403
	public static class ResourceDictionaryDiagnostics
	{
		// Token: 0x06001798 RID: 6040 RVA: 0x000734D8 File Offset: 0x000716D8
		[SecuritySafeCritical]
		static ResourceDictionaryDiagnostics()
		{
			ResourceDictionaryDiagnostics.IgnorableProperties.Add(typeof(ResourceDictionary).GetProperty("DeferrableContent"));
		}

		/// <summary>Gets all instances of themed <see cref="T:System.Windows.ResourceDictionary" /> objects loaded by the application when a managed debugger is attached.</summary>
		/// <returns>
		///   <see cref="P:System.Windows.Diagnostics.ResourceDictionaryDiagnostics.ThemedResourceDictionaries" />
		/// </returns>
		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001799 RID: 6041 RVA: 0x0007357C File Offset: 0x0007177C
		public static IEnumerable<ResourceDictionaryInfo> ThemedResourceDictionaries
		{
			get
			{
				if (!ResourceDictionaryDiagnostics.IsEnabled)
				{
					return ResourceDictionaryDiagnostics.EmptyResourceDictionaryInfos;
				}
				return SystemResources.ThemedResourceDictionaries;
			}
		}

		/// <summary>Gets all instances of generic <see cref="T:System.Windows.ResourceDictionary" /> objects loaded by the application when a managed debugger is attached.</summary>
		/// <returns>
		///   <see cref="P:System.Windows.Diagnostics.ResourceDictionaryDiagnostics.GenericResourceDictionaries" /> property</returns>
		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x0600179A RID: 6042 RVA: 0x00073590 File Offset: 0x00071790
		public static IEnumerable<ResourceDictionaryInfo> GenericResourceDictionaries
		{
			get
			{
				if (!ResourceDictionaryDiagnostics.IsEnabled)
				{
					return ResourceDictionaryDiagnostics.EmptyResourceDictionaryInfos;
				}
				return SystemResources.GenericResourceDictionaries;
			}
		}

		/// <summary>Occurs when a managed debugger is attached, and a themed <see cref="T:System.Windows.ResourceDictionary" /> object is loaded
		///   by the application.</summary>
		// Token: 0x14000041 RID: 65
		// (add) Token: 0x0600179B RID: 6043 RVA: 0x000735A4 File Offset: 0x000717A4
		// (remove) Token: 0x0600179C RID: 6044 RVA: 0x000735B3 File Offset: 0x000717B3
		public static event EventHandler<ResourceDictionaryLoadedEventArgs> ThemedResourceDictionaryLoaded
		{
			add
			{
				if (ResourceDictionaryDiagnostics.IsEnabled)
				{
					SystemResources.ThemedDictionaryLoaded += value;
				}
			}
			remove
			{
				SystemResources.ThemedDictionaryLoaded -= value;
			}
		}

		/// <summary>Occurs when a managed debugger is attached and a themed <see cref="T:System.Windows.ResourceDictionary" /> object is unloaded
		///   from the application.</summary>
		// Token: 0x14000042 RID: 66
		// (add) Token: 0x0600179D RID: 6045 RVA: 0x000735BB File Offset: 0x000717BB
		// (remove) Token: 0x0600179E RID: 6046 RVA: 0x000735CA File Offset: 0x000717CA
		public static event EventHandler<ResourceDictionaryUnloadedEventArgs> ThemedResourceDictionaryUnloaded
		{
			add
			{
				if (ResourceDictionaryDiagnostics.IsEnabled)
				{
					SystemResources.ThemedDictionaryUnloaded += value;
				}
			}
			remove
			{
				SystemResources.ThemedDictionaryUnloaded -= value;
			}
		}

		/// <summary>Occurs when a managed debugger is attached and a generic <see cref="T:System.Windows.ResourceDictionary" /> object is loaded
		///   by the application.</summary>
		// Token: 0x14000043 RID: 67
		// (add) Token: 0x0600179F RID: 6047 RVA: 0x000735D2 File Offset: 0x000717D2
		// (remove) Token: 0x060017A0 RID: 6048 RVA: 0x000735E1 File Offset: 0x000717E1
		public static event EventHandler<ResourceDictionaryLoadedEventArgs> GenericResourceDictionaryLoaded
		{
			add
			{
				if (ResourceDictionaryDiagnostics.IsEnabled)
				{
					SystemResources.GenericDictionaryLoaded += value;
				}
			}
			remove
			{
				SystemResources.GenericDictionaryLoaded -= value;
			}
		}

		/// <summary>Finds the resource dictionaries created from a given source URI. </summary>
		/// <param name="uri">The source URI. The method return <see langword="null" /> if no resource dictionaries were created from <paramref name="uri" /> </param>
		/// <returns>The resource dictionaries created from <paramref name="uri" />. The method returns <see langword="null" /> if Visual Diagnostics is not enabled, the <see langword="ENABLE_XAML_DIAGNOSTICS_SOURCE_INFO" /> environment variable is not set or is set to <see langword="false" />, or no resource dictionaries were created from <paramref name="uri" />. 
		/// </returns>
		// Token: 0x060017A1 RID: 6049 RVA: 0x000735EC File Offset: 0x000717EC
		public static IEnumerable<ResourceDictionary> GetResourceDictionariesForSource(Uri uri)
		{
			if (!ResourceDictionaryDiagnostics.IsEnabled || ResourceDictionaryDiagnostics._dictionariesFromUri == null)
			{
				return ResourceDictionaryDiagnostics.EmptyResourceDictionaries;
			}
			object dictionariesFromUriLock = ResourceDictionaryDiagnostics._dictionariesFromUriLock;
			IEnumerable<ResourceDictionary> result;
			lock (dictionariesFromUriLock)
			{
				List<WeakReference<ResourceDictionary>> list;
				if (!ResourceDictionaryDiagnostics._dictionariesFromUri.TryGetValue(uri, out list) || list.Count == 0)
				{
					result = ResourceDictionaryDiagnostics.EmptyResourceDictionaries;
				}
				else
				{
					List<ResourceDictionary> list2 = new List<ResourceDictionary>(list.Count);
					List<WeakReference<ResourceDictionary>> list3 = null;
					foreach (WeakReference<ResourceDictionary> weakReference in list)
					{
						ResourceDictionary item;
						if (weakReference.TryGetTarget(out item))
						{
							list2.Add(item);
						}
						else
						{
							if (list3 == null)
							{
								list3 = new List<WeakReference<ResourceDictionary>>();
							}
							list3.Add(weakReference);
						}
					}
					if (list3 != null)
					{
						ResourceDictionaryDiagnostics.RemoveEntries(uri, list, list3);
					}
					result = list2.AsReadOnly();
				}
			}
			return result;
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x000736E4 File Offset: 0x000718E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void AddResourceDictionaryForUri(Uri uri, ResourceDictionary rd)
		{
			if (uri != null && ResourceDictionaryDiagnostics.IsEnabled)
			{
				ResourceDictionaryDiagnostics.AddResourceDictionaryForUriImpl(uri, rd);
			}
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x00073700 File Offset: 0x00071900
		private static void AddResourceDictionaryForUriImpl(Uri uri, ResourceDictionary rd)
		{
			object dictionariesFromUriLock = ResourceDictionaryDiagnostics._dictionariesFromUriLock;
			lock (dictionariesFromUriLock)
			{
				if (ResourceDictionaryDiagnostics._dictionariesFromUri == null)
				{
					ResourceDictionaryDiagnostics._dictionariesFromUri = new Dictionary<Uri, List<WeakReference<ResourceDictionary>>>();
				}
				List<WeakReference<ResourceDictionary>> list;
				if (!ResourceDictionaryDiagnostics._dictionariesFromUri.TryGetValue(uri, out list))
				{
					list = new List<WeakReference<ResourceDictionary>>(1);
					ResourceDictionaryDiagnostics._dictionariesFromUri.Add(uri, list);
				}
				list.Add(new WeakReference<ResourceDictionary>(rd));
			}
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x00073778 File Offset: 0x00071978
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void RemoveResourceDictionaryForUri(Uri uri, ResourceDictionary rd)
		{
			if (uri != null && ResourceDictionaryDiagnostics.IsEnabled)
			{
				ResourceDictionaryDiagnostics.RemoveResourceDictionaryForUriImpl(uri, rd);
			}
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x00073794 File Offset: 0x00071994
		private static void RemoveResourceDictionaryForUriImpl(Uri uri, ResourceDictionary rd)
		{
			object dictionariesFromUriLock = ResourceDictionaryDiagnostics._dictionariesFromUriLock;
			lock (dictionariesFromUriLock)
			{
				List<WeakReference<ResourceDictionary>> list;
				if (ResourceDictionaryDiagnostics._dictionariesFromUri != null && ResourceDictionaryDiagnostics._dictionariesFromUri.TryGetValue(uri, out list))
				{
					List<WeakReference<ResourceDictionary>> list2 = new List<WeakReference<ResourceDictionary>>();
					foreach (WeakReference<ResourceDictionary> weakReference in list)
					{
						ResourceDictionary resourceDictionary;
						if (!weakReference.TryGetTarget(out resourceDictionary) || resourceDictionary == rd)
						{
							list2.Add(weakReference);
						}
					}
					ResourceDictionaryDiagnostics.RemoveEntries(uri, list, list2);
				}
			}
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x00073844 File Offset: 0x00071A44
		private static void RemoveEntries(Uri uri, List<WeakReference<ResourceDictionary>> list, List<WeakReference<ResourceDictionary>> toRemove)
		{
			foreach (WeakReference<ResourceDictionary> item in toRemove)
			{
				list.Remove(item);
			}
			if (list.Count == 0)
			{
				ResourceDictionaryDiagnostics._dictionariesFromUri.Remove(uri);
			}
		}

		/// <summary>Gets the framework element owners of a specified resource dictionary. </summary>
		/// <param name="dictionary">A resource dictionary. </param>
		/// <returns>The framework element owners of <paramref name="dictionary" />. If there are no freamework element owners, the method returns <see langword="null" />. </returns>
		// Token: 0x060017A7 RID: 6055 RVA: 0x000738A8 File Offset: 0x00071AA8
		public static IEnumerable<FrameworkElement> GetFrameworkElementOwners(ResourceDictionary dictionary)
		{
			return ResourceDictionaryDiagnostics.GetOwners<FrameworkElement>(dictionary.FrameworkElementOwners, ResourceDictionaryDiagnostics.EmptyFrameworkElementList);
		}

		/// <summary>Gets the framework content owners of a specified resource dictionary. </summary>
		/// <param name="dictionary">A resource dictionary. </param>
		/// <returns>The framework content owners of <paramref name="dictionary" />. If there are no framework content owners, the method returns <see langword="null" />. </returns>
		// Token: 0x060017A8 RID: 6056 RVA: 0x000738BA File Offset: 0x00071ABA
		public static IEnumerable<FrameworkContentElement> GetFrameworkContentElementOwners(ResourceDictionary dictionary)
		{
			return ResourceDictionaryDiagnostics.GetOwners<FrameworkContentElement>(dictionary.FrameworkContentElementOwners, ResourceDictionaryDiagnostics.EmptyFrameworkContentElementList);
		}

		/// <summary>Gets the application owners of a specified resource dictionary. </summary>
		/// <param name="dictionary">A resource dictionary. </param>
		/// <returns>The application owners of <paramref name="dictionary" />. If there are no application owners, the method returns <see langword="null" />. </returns>
		// Token: 0x060017A9 RID: 6057 RVA: 0x000738CC File Offset: 0x00071ACC
		public static IEnumerable<Application> GetApplicationOwners(ResourceDictionary dictionary)
		{
			return ResourceDictionaryDiagnostics.GetOwners<Application>(dictionary.ApplicationOwners, ResourceDictionaryDiagnostics.EmptyApplicationList);
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x000738E0 File Offset: 0x00071AE0
		private static IEnumerable<T> GetOwners<T>(WeakReferenceList list, IEnumerable<T> emptyList) where T : DispatcherObject
		{
			if (!ResourceDictionaryDiagnostics.IsEnabled || list == null || list.Count == 0)
			{
				return emptyList;
			}
			List<T> list2 = new List<T>(list.Count);
			foreach (object obj in list)
			{
				T t = obj as T;
				if (t != null)
				{
					list2.Add(t);
				}
			}
			return list2.AsReadOnly();
		}

		/// <summary>Occurs when s static resource reference is resolved. </summary>
		// Token: 0x14000044 RID: 68
		// (add) Token: 0x060017AB RID: 6059 RVA: 0x00073948 File Offset: 0x00071B48
		// (remove) Token: 0x060017AC RID: 6060 RVA: 0x0007397C File Offset: 0x00071B7C
		public static event EventHandler<StaticResourceResolvedEventArgs> StaticResourceResolved;

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x060017AD RID: 6061 RVA: 0x000739AF File Offset: 0x00071BAF
		internal static bool HasStaticResourceResolvedListeners
		{
			get
			{
				return ResourceDictionaryDiagnostics.IsEnabled && ResourceDictionaryDiagnostics.StaticResourceResolved != null;
			}
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x000739C2 File Offset: 0x00071BC2
		internal static bool ShouldIgnoreProperty(object targetProperty)
		{
			return ResourceDictionaryDiagnostics.IgnorableProperties.Contains(targetProperty);
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x000739D0 File Offset: 0x00071BD0
		internal static ResourceDictionaryDiagnostics.LookupResult RequestLookupResult(StaticResourceExtension requester)
		{
			if (ResourceDictionaryDiagnostics._lookupResultStack == null)
			{
				ResourceDictionaryDiagnostics._lookupResultStack = new Stack<ResourceDictionaryDiagnostics.LookupResult>();
			}
			ResourceDictionaryDiagnostics.LookupResult lookupResult = new ResourceDictionaryDiagnostics.LookupResult(requester);
			ResourceDictionaryDiagnostics._lookupResultStack.Push(lookupResult);
			return lookupResult;
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x00073A01 File Offset: 0x00071C01
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void RecordLookupResult(object key, ResourceDictionary rd)
		{
			if (ResourceDictionaryDiagnostics.IsEnabled && ResourceDictionaryDiagnostics._lookupResultStack != null)
			{
				ResourceDictionaryDiagnostics.RecordLookupResultImpl(key, rd);
			}
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x00073A18 File Offset: 0x00071C18
		private static void RecordLookupResultImpl(object key, ResourceDictionary rd)
		{
			if (ResourceDictionaryDiagnostics._lookupResultStack.Count > 0)
			{
				ResourceDictionaryDiagnostics.LookupResult lookupResult = ResourceDictionaryDiagnostics._lookupResultStack.Peek();
				if (!object.Equals(lookupResult.Requester.ResourceKey, key))
				{
					return;
				}
				if (lookupResult.Requester.GetType() == typeof(StaticResourceExtension))
				{
					lookupResult.Key = key;
					lookupResult.Dictionary = rd;
					return;
				}
				lookupResult.Key = key;
				lookupResult.Dictionary = rd;
			}
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x00073A8C File Offset: 0x00071C8C
		internal static void RevertRequest(StaticResourceExtension requester, bool success)
		{
			ResourceDictionaryDiagnostics.LookupResult lookupResult = ResourceDictionaryDiagnostics._lookupResultStack.Pop();
			if (!success)
			{
				return;
			}
			if (lookupResult.Requester.GetType() == typeof(StaticResourceExtension))
			{
				return;
			}
			if (ResourceDictionaryDiagnostics._resultCache == null)
			{
				ResourceDictionaryDiagnostics._resultCache = new Dictionary<WeakReferenceKey<StaticResourceExtension>, WeakReference<ResourceDictionary>>();
			}
			WeakReferenceKey<StaticResourceExtension> key = new WeakReferenceKey<StaticResourceExtension>(requester);
			ResourceDictionary dictionary = null;
			WeakReference<ResourceDictionary> weakReference;
			bool flag = ResourceDictionaryDiagnostics._resultCache.TryGetValue(key, out weakReference);
			if (flag)
			{
				weakReference.TryGetTarget(out dictionary);
			}
			if (lookupResult.Dictionary != null)
			{
				ResourceDictionaryDiagnostics._resultCache[key] = new WeakReference<ResourceDictionary>(lookupResult.Dictionary);
				return;
			}
			lookupResult.Key = requester.ResourceKey;
			lookupResult.Dictionary = dictionary;
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x00073B30 File Offset: 0x00071D30
		internal static void OnStaticResourceResolved(object targetObject, object targetProperty, ResourceDictionaryDiagnostics.LookupResult result)
		{
			EventHandler<StaticResourceResolvedEventArgs> staticResourceResolved = ResourceDictionaryDiagnostics.StaticResourceResolved;
			if (staticResourceResolved != null && result.Dictionary != null)
			{
				staticResourceResolved(null, new StaticResourceResolvedEventArgs(targetObject, targetProperty, result.Dictionary, result.Key));
			}
			ResourceDictionaryDiagnostics.RequestCacheCleanup(targetObject);
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x00073B70 File Offset: 0x00071D70
		private static void RequestCacheCleanup(object targetObject)
		{
			DispatcherObject dispatcherObject;
			Dispatcher dispatcher;
			if (ResourceDictionaryDiagnostics._resultCache == null || ResourceDictionaryDiagnostics._cleanupOperation != null || (dispatcherObject = (targetObject as DispatcherObject)) == null || (dispatcher = dispatcherObject.Dispatcher) == null)
			{
				return;
			}
			ResourceDictionaryDiagnostics._cleanupOperation = dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(ResourceDictionaryDiagnostics.DoCleanup));
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x00073BB8 File Offset: 0x00071DB8
		private static void DoCleanup()
		{
			ResourceDictionaryDiagnostics._cleanupOperation = null;
			List<WeakReferenceKey<StaticResourceExtension>> list = null;
			foreach (KeyValuePair<WeakReferenceKey<StaticResourceExtension>, WeakReference<ResourceDictionary>> keyValuePair in ResourceDictionaryDiagnostics._resultCache)
			{
				ResourceDictionary resourceDictionary;
				if (keyValuePair.Key.Item == null || !keyValuePair.Value.TryGetTarget(out resourceDictionary))
				{
					if (list == null)
					{
						list = new List<WeakReferenceKey<StaticResourceExtension>>();
					}
					list.Add(keyValuePair.Key);
				}
			}
			if (list != null)
			{
				foreach (WeakReferenceKey<StaticResourceExtension> key in list)
				{
					ResourceDictionaryDiagnostics._resultCache.Remove(key);
				}
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x060017B6 RID: 6070 RVA: 0x00073C88 File Offset: 0x00071E88
		// (set) Token: 0x060017B7 RID: 6071 RVA: 0x00073C8F File Offset: 0x00071E8F
		internal static bool IsEnabled { get; private set; } = VisualDiagnostics.IsEnabled && VisualDiagnostics.IsEnvironmentVariableSet(null, "ENABLE_XAML_DIAGNOSTICS_SOURCE_INFO");

		// Token: 0x040012BF RID: 4799
		private static Dictionary<Uri, List<WeakReference<ResourceDictionary>>> _dictionariesFromUri;

		// Token: 0x040012C0 RID: 4800
		private static object _dictionariesFromUriLock = new object();

		// Token: 0x040012C1 RID: 4801
		private static readonly ReadOnlyCollection<ResourceDictionary> EmptyResourceDictionaries = new List<ResourceDictionary>().AsReadOnly();

		// Token: 0x040012C2 RID: 4802
		private static readonly ReadOnlyCollection<FrameworkElement> EmptyFrameworkElementList = new List<FrameworkElement>().AsReadOnly();

		// Token: 0x040012C3 RID: 4803
		private static readonly ReadOnlyCollection<FrameworkContentElement> EmptyFrameworkContentElementList = new List<FrameworkContentElement>().AsReadOnly();

		// Token: 0x040012C4 RID: 4804
		private static readonly ReadOnlyCollection<Application> EmptyApplicationList = new List<Application>().AsReadOnly();

		// Token: 0x040012C6 RID: 4806
		private static List<object> IgnorableProperties = new List<object>();

		// Token: 0x040012C7 RID: 4807
		[ThreadStatic]
		private static Stack<ResourceDictionaryDiagnostics.LookupResult> _lookupResultStack;

		// Token: 0x040012C8 RID: 4808
		[ThreadStatic]
		private static Dictionary<WeakReferenceKey<StaticResourceExtension>, WeakReference<ResourceDictionary>> _resultCache;

		// Token: 0x040012C9 RID: 4809
		[ThreadStatic]
		private static DispatcherOperation _cleanupOperation;

		// Token: 0x040012CB RID: 4811
		private static readonly ReadOnlyCollection<ResourceDictionaryInfo> EmptyResourceDictionaryInfos = new List<ResourceDictionaryInfo>().AsReadOnly();

		// Token: 0x0200085F RID: 2143
		internal class LookupResult
		{
			// Token: 0x17001D99 RID: 7577
			// (get) Token: 0x060082C7 RID: 33479 RVA: 0x00243C31 File Offset: 0x00241E31
			// (set) Token: 0x060082C8 RID: 33480 RVA: 0x00243C39 File Offset: 0x00241E39
			public StaticResourceExtension Requester { get; set; }

			// Token: 0x17001D9A RID: 7578
			// (get) Token: 0x060082C9 RID: 33481 RVA: 0x00243C42 File Offset: 0x00241E42
			// (set) Token: 0x060082CA RID: 33482 RVA: 0x00243C4A File Offset: 0x00241E4A
			public object Key { get; set; }

			// Token: 0x17001D9B RID: 7579
			// (get) Token: 0x060082CB RID: 33483 RVA: 0x00243C53 File Offset: 0x00241E53
			// (set) Token: 0x060082CC RID: 33484 RVA: 0x00243C5B File Offset: 0x00241E5B
			public ResourceDictionary Dictionary { get; set; }

			// Token: 0x060082CD RID: 33485 RVA: 0x00243C64 File Offset: 0x00241E64
			public LookupResult(StaticResourceExtension requester)
			{
				this.Requester = requester;
			}
		}
	}
}
