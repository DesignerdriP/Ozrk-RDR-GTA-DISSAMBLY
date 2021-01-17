using System;
using System.IO;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows;
using MS.Internal;

namespace MS.Win32
{
	// Token: 0x020005D8 RID: 1496
	internal static class UxThemeWrapper
	{
		// Token: 0x170017D7 RID: 6103
		// (get) Token: 0x0600638A RID: 25482 RVA: 0x001C0425 File Offset: 0x001BE625
		internal static bool IsActive
		{
			get
			{
				return UxThemeWrapper.IsActiveCompatWrapper;
			}
		}

		// Token: 0x170017D8 RID: 6104
		// (get) Token: 0x0600638B RID: 25483 RVA: 0x001C042C File Offset: 0x001BE62C
		internal static string ThemeName
		{
			get
			{
				return UxThemeWrapper.ThemeNameCompatWrapper;
			}
		}

		// Token: 0x170017D9 RID: 6105
		// (get) Token: 0x0600638C RID: 25484 RVA: 0x001C0433 File Offset: 0x001BE633
		internal static string ThemeColor
		{
			get
			{
				return UxThemeWrapper.ThemeColorCompatWrapper;
			}
		}

		// Token: 0x170017DA RID: 6106
		// (get) Token: 0x0600638D RID: 25485 RVA: 0x001C043A File Offset: 0x001BE63A
		internal static string ThemedResourceName
		{
			get
			{
				return UxThemeWrapper.ThemedResourceNameCompatWrapper;
			}
		}

		// Token: 0x0600638E RID: 25486 RVA: 0x001C0444 File Offset: 0x001BE644
		private static UxThemeWrapper.ThemeState EnsureThemeState(bool themeChanged)
		{
			UxThemeWrapper.ThemeState themeState = UxThemeWrapper._themeState;
			bool flag = !themeChanged;
			bool flag2 = true;
			while (flag2)
			{
				UxThemeWrapper.ThemeState themeState2;
				if (themeChanged)
				{
					bool flag3 = !SystemParameters.HighContrast && SafeNativeMethods.IsUxThemeActive();
					string name;
					string color;
					if (flag3 && (flag || themeState.ThemeName != null))
					{
						UxThemeWrapper.GetThemeNameAndColor(out name, out color);
					}
					else
					{
						color = (name = null);
					}
					themeState2 = new UxThemeWrapper.ThemeState(flag3, name, color);
				}
				else if (themeState.IsActive && themeState.ThemeName == null)
				{
					string name;
					string color;
					UxThemeWrapper.GetThemeNameAndColor(out name, out color);
					themeState2 = new UxThemeWrapper.ThemeState(themeState.IsActive, name, color);
				}
				else
				{
					themeState2 = themeState;
					flag2 = false;
				}
				if (flag2)
				{
					UxThemeWrapper.ThemeState themeState3 = Interlocked.CompareExchange<UxThemeWrapper.ThemeState>(ref UxThemeWrapper._themeState, themeState2, themeState);
					if (themeState3 == themeState)
					{
						themeState = themeState2;
						flag2 = false;
					}
					else if (themeState3.IsActive == themeState2.IsActive && (!themeState2.IsActive || themeState2.ThemeName == null || themeState3.ThemeName != null))
					{
						themeState = themeState3;
						flag2 = false;
					}
					else
					{
						themeChanged = true;
						themeState = themeState3;
					}
				}
			}
			return themeState;
		}

		// Token: 0x0600638F RID: 25487 RVA: 0x001C0534 File Offset: 0x001BE734
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void GetThemeNameAndColor(out string themeName, out string themeColor)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			StringBuilder stringBuilder2 = new StringBuilder(260);
			if (UnsafeNativeMethods.GetCurrentThemeName(stringBuilder, stringBuilder.Capacity, stringBuilder2, stringBuilder2.Capacity, null, 0) == 0)
			{
				themeName = stringBuilder.ToString();
				themeName = Path.GetFileNameWithoutExtension(themeName);
				if (string.Compare(themeName, "aero", StringComparison.OrdinalIgnoreCase) == 0 && Utilities.IsOSWindows8OrNewer)
				{
					themeName = "Aero2";
				}
				themeColor = stringBuilder2.ToString();
				return;
			}
			string empty;
			themeColor = (empty = string.Empty);
			themeName = empty;
		}

		// Token: 0x06006390 RID: 25488 RVA: 0x001C05B1 File Offset: 0x001BE7B1
		internal static void OnThemeChanged()
		{
			UxThemeWrapper.RestoreSupportedState();
			UxThemeWrapper.EnsureThemeState(true);
		}

		// Token: 0x170017DB RID: 6107
		// (get) Token: 0x06006391 RID: 25489 RVA: 0x001C05BF File Offset: 0x001BE7BF
		private static bool IsAppSupported
		{
			get
			{
				return UxThemeWrapper._themeName == null;
			}
		}

		// Token: 0x170017DC RID: 6108
		// (get) Token: 0x06006392 RID: 25490 RVA: 0x001C05C9 File Offset: 0x001BE7C9
		private static bool IsActiveCompatWrapper
		{
			get
			{
				if (!UxThemeWrapper.IsAppSupported)
				{
					return UxThemeWrapper._isActive;
				}
				return UxThemeWrapper._themeState.IsActive;
			}
		}

		// Token: 0x170017DD RID: 6109
		// (get) Token: 0x06006393 RID: 25491 RVA: 0x001C05E4 File Offset: 0x001BE7E4
		private static string ThemeNameCompatWrapper
		{
			get
			{
				if (!UxThemeWrapper.IsAppSupported)
				{
					return UxThemeWrapper._themeName;
				}
				UxThemeWrapper.ThemeState themeState = UxThemeWrapper.EnsureThemeState(false);
				if (themeState.IsActive)
				{
					return themeState.ThemeName;
				}
				return "classic";
			}
		}

		// Token: 0x170017DE RID: 6110
		// (get) Token: 0x06006394 RID: 25492 RVA: 0x001C061C File Offset: 0x001BE81C
		private static string ThemeColorCompatWrapper
		{
			get
			{
				if (UxThemeWrapper.IsAppSupported)
				{
					UxThemeWrapper.ThemeState themeState = UxThemeWrapper.EnsureThemeState(false);
					return themeState.ThemeColor;
				}
				return UxThemeWrapper._themeColor;
			}
		}

		// Token: 0x170017DF RID: 6111
		// (get) Token: 0x06006395 RID: 25493 RVA: 0x001C0644 File Offset: 0x001BE844
		private static string ThemedResourceNameCompatWrapper
		{
			get
			{
				if (UxThemeWrapper.IsAppSupported)
				{
					UxThemeWrapper.ThemeState themeState = UxThemeWrapper.EnsureThemeState(false);
					if (themeState.IsActive)
					{
						return "themes/" + themeState.ThemeName.ToLowerInvariant() + "." + themeState.ThemeColor.ToLowerInvariant();
					}
					return "themes/classic";
				}
				else
				{
					if (UxThemeWrapper._isActive)
					{
						return "themes/" + UxThemeWrapper._themeName.ToLowerInvariant() + "." + UxThemeWrapper._themeColor.ToLowerInvariant();
					}
					return "themes/classic";
				}
			}
		}

		// Token: 0x06006396 RID: 25494 RVA: 0x001C06C3 File Offset: 0x001BE8C3
		private static void RestoreSupportedState()
		{
			UxThemeWrapper._isActive = false;
			UxThemeWrapper._themeName = null;
			UxThemeWrapper._themeColor = null;
		}

		// Token: 0x040031E3 RID: 12771
		private static UxThemeWrapper.ThemeState _themeState = new UxThemeWrapper.ThemeState(!SystemParameters.HighContrast && SafeNativeMethods.IsUxThemeActive(), null, null);

		// Token: 0x040031E4 RID: 12772
		private static bool _isActive;

		// Token: 0x040031E5 RID: 12773
		private static string _themeName;

		// Token: 0x040031E6 RID: 12774
		private static string _themeColor;

		// Token: 0x020009FE RID: 2558
		private class ThemeState
		{
			// Token: 0x060089DF RID: 35295 RVA: 0x00256706 File Offset: 0x00254906
			public ThemeState(bool isActive, string name, string color)
			{
				this._isActive = isActive;
				this._themeName = name;
				this._themeColor = color;
			}

			// Token: 0x17001F22 RID: 7970
			// (get) Token: 0x060089E0 RID: 35296 RVA: 0x00256723 File Offset: 0x00254923
			public bool IsActive
			{
				get
				{
					return this._isActive;
				}
			}

			// Token: 0x17001F23 RID: 7971
			// (get) Token: 0x060089E1 RID: 35297 RVA: 0x0025672B File Offset: 0x0025492B
			public string ThemeName
			{
				get
				{
					return this._themeName;
				}
			}

			// Token: 0x17001F24 RID: 7972
			// (get) Token: 0x060089E2 RID: 35298 RVA: 0x00256733 File Offset: 0x00254933
			public string ThemeColor
			{
				get
				{
					return this._themeColor;
				}
			}

			// Token: 0x040046A7 RID: 18087
			private bool _isActive;

			// Token: 0x040046A8 RID: 18088
			private string _themeName;

			// Token: 0x040046A9 RID: 18089
			private string _themeColor;
		}
	}
}
