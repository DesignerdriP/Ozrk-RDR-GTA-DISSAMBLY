using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	/// <summary>Defines the types of culture lists that can be retrieved using the <see cref="M:System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes)" /> method.</summary>
	// Token: 0x0200037E RID: 894
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum CultureTypes
	{
		/// <summary>Cultures that are associated with a language but are not specific to a country/region. The names of .NET Framework cultures consist of the lowercase two-letter code derived from ISO 639-1. For example: "en" (English) is a neutral culture. </summary>
		// Token: 0x0400129E RID: 4766
		NeutralCultures = 1,
		/// <summary>Cultures that are specific to a country/region. The names of these cultures follow RFC 4646 (Windows Vista and later). The format is "&lt;languagecode2&gt;-&lt;country/regioncode2&gt;", where &lt;languagecode2&gt; is a lowercase two-letter code derived from ISO 639-1 and &lt;country/regioncode2&gt; is an uppercase two-letter code derived from ISO 3166. For example, "en-US" for English (United States) is a specific culture.</summary>
		// Token: 0x0400129F RID: 4767
		SpecificCultures = 2,
		/// <summary>All cultures that are installed in the Windows operating system. Note that not all cultures supported by the .NET Framework are installed in the operating system.</summary>
		// Token: 0x040012A0 RID: 4768
		InstalledWin32Cultures = 4,
		/// <summary>All cultures that ship with the .NET Framework, including neutral and specific cultures, cultures installed in the Windows operating system, and custom cultures created by the user.
		///     <see cref="F:System.Globalization.CultureTypes.AllCultures" /> is a composite field that includes the <see cref="F:System.Globalization.CultureTypes.NeutralCultures" />, <see cref="F:System.Globalization.CultureTypes.SpecificCultures" />, and <see cref="F:System.Globalization.CultureTypes.InstalledWin32Cultures" /> values. </summary>
		// Token: 0x040012A1 RID: 4769
		AllCultures = 7,
		/// <summary>Custom cultures created by the user.</summary>
		// Token: 0x040012A2 RID: 4770
		UserCustomCulture = 8,
		/// <summary>Custom cultures created by the user that replace cultures shipped with the .NET Framework.</summary>
		// Token: 0x040012A3 RID: 4771
		ReplacementCultures = 16,
		/// <summary>This member is deprecated. If it is used as an argument to the <see cref="M:System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes)" /> method, the method returns an empty array. </summary>
		// Token: 0x040012A4 RID: 4772
		[Obsolete("This value has been deprecated.  Please use other values in CultureTypes.")]
		WindowsOnlyCultures = 32,
		/// <summary>This member is deprecated; using this value with <see cref="M:System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes)" />  returns neutral and specific cultures shipped with the .NET Framework 2.0. </summary>
		// Token: 0x040012A5 RID: 4773
		[Obsolete("This value has been deprecated.  Please use other values in CultureTypes.")]
		FrameworkCultures = 64
	}
}
