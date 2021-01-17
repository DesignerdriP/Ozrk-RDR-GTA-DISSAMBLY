using System;

namespace System.Windows.Diagnostics
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Diagnostics.ResourceDictionaryDiagnostics.StaticResourceResolved" /> event. </summary>
	// Token: 0x02000197 RID: 407
	public class StaticResourceResolvedEventArgs : EventArgs
	{
		// Token: 0x060017C7 RID: 6087 RVA: 0x00073D40 File Offset: 0x00071F40
		internal StaticResourceResolvedEventArgs(object targetObject, object targetProperty, ResourceDictionary rd, object key)
		{
			this.TargetObject = targetObject;
			this.TargetProperty = targetProperty;
			this.ResourceDictionary = rd;
			this.ResourceKey = key;
		}

		/// <summary>Gets the value to assign to the target property. </summary>
		/// <returns>The value to assign to the target property. </returns>
		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x060017C8 RID: 6088 RVA: 0x00073D65 File Offset: 0x00071F65
		// (set) Token: 0x060017C9 RID: 6089 RVA: 0x00073D6D File Offset: 0x00071F6D
		public object TargetObject { get; private set; }

		/// <summary>Gets the target property that the resource sets. </summary>
		/// <returns>The target property that the resource sets. </returns>
		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060017CA RID: 6090 RVA: 0x00073D76 File Offset: 0x00071F76
		// (set) Token: 0x060017CB RID: 6091 RVA: 0x00073D7E File Offset: 0x00071F7E
		public object TargetProperty { get; private set; }

		/// <summary>Gets the name of the resource dictionary. </summary>
		/// <returns>The name of the resource dictionary. </returns>
		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x060017CC RID: 6092 RVA: 0x00073D87 File Offset: 0x00071F87
		// (set) Token: 0x060017CD RID: 6093 RVA: 0x00073D8F File Offset: 0x00071F8F
		public ResourceDictionary ResourceDictionary { get; private set; }

		/// <summary>The key for the requested resource. </summary>
		/// <returns>The key for the requested resource. </returns>
		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x060017CE RID: 6094 RVA: 0x00073D98 File Offset: 0x00071F98
		// (set) Token: 0x060017CF RID: 6095 RVA: 0x00073DA0 File Offset: 0x00071FA0
		public object ResourceKey { get; private set; }
	}
}
