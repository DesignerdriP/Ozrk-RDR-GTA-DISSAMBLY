using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace MS.Internal.Ink
{
	// Token: 0x02000689 RID: 1673
	internal static class HighContrastHelper
	{
		// Token: 0x06006D72 RID: 28018 RVA: 0x001F6BA0 File Offset: 0x001F4DA0
		internal static void RegisterHighContrastCallback(HighContrastCallback highContrastCallback)
		{
			object _lock = HighContrastHelper.__lock;
			lock (_lock)
			{
				int count = HighContrastHelper.__highContrastCallbackList.Count;
				int i = 0;
				int num = 0;
				if (HighContrastHelper.__increaseCount > 100)
				{
					while (i < count)
					{
						WeakReference weakReference = HighContrastHelper.__highContrastCallbackList[num];
						if (weakReference.IsAlive)
						{
							num++;
						}
						else
						{
							HighContrastHelper.__highContrastCallbackList.RemoveAt(num);
						}
						i++;
					}
					HighContrastHelper.__increaseCount = 0;
				}
				HighContrastHelper.__highContrastCallbackList.Add(new WeakReference(highContrastCallback));
				HighContrastHelper.__increaseCount++;
			}
		}

		// Token: 0x06006D73 RID: 28019 RVA: 0x001F6C4C File Offset: 0x001F4E4C
		internal static void OnSettingChanged()
		{
			HighContrastHelper.UpdateHighContrast();
		}

		// Token: 0x06006D74 RID: 28020 RVA: 0x001F6C54 File Offset: 0x001F4E54
		private static void UpdateHighContrast()
		{
			object _lock = HighContrastHelper.__lock;
			lock (_lock)
			{
				int count = HighContrastHelper.__highContrastCallbackList.Count;
				int i = 0;
				int num = 0;
				while (i < count)
				{
					WeakReference weakReference = HighContrastHelper.__highContrastCallbackList[num];
					if (weakReference.IsAlive)
					{
						HighContrastCallback highContrastCallback = weakReference.Target as HighContrastCallback;
						if (highContrastCallback.Dispatcher != null)
						{
							highContrastCallback.Dispatcher.BeginInvoke(DispatcherPriority.Background, new HighContrastHelper.UpdateHighContrastCallback(HighContrastHelper.OnUpdateHighContrast), highContrastCallback);
						}
						else
						{
							HighContrastHelper.OnUpdateHighContrast(highContrastCallback);
						}
						num++;
					}
					else
					{
						HighContrastHelper.__highContrastCallbackList.RemoveAt(num);
					}
					i++;
				}
				HighContrastHelper.__increaseCount = 0;
			}
		}

		// Token: 0x06006D75 RID: 28021 RVA: 0x001F6D14 File Offset: 0x001F4F14
		private static void OnUpdateHighContrast(HighContrastCallback highContrastCallback)
		{
			bool highContrast = SystemParameters.HighContrast;
			Color windowTextColor = SystemColors.WindowTextColor;
			if (highContrast)
			{
				highContrastCallback.TurnHighContrastOn(windowTextColor);
				return;
			}
			highContrastCallback.TurnHighContrastOff();
		}

		// Token: 0x040035E9 RID: 13801
		private static object __lock = new object();

		// Token: 0x040035EA RID: 13802
		private static List<WeakReference> __highContrastCallbackList = new List<WeakReference>();

		// Token: 0x040035EB RID: 13803
		private static int __increaseCount = 0;

		// Token: 0x040035EC RID: 13804
		private const int CleanTolerance = 100;

		// Token: 0x02000B22 RID: 2850
		// (Invoke) Token: 0x06008D30 RID: 36144
		private delegate void UpdateHighContrastCallback(HighContrastCallback highContrastCallback);
	}
}
