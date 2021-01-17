using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x0200045F RID: 1119
	internal static class Extensions
	{
		// Token: 0x060040A7 RID: 16551 RVA: 0x00127374 File Offset: 0x00125574
		[SecuritySafeCritical]
		internal static List<string> ToList(this RCW.IEnumString enumString, bool shouldSuppressCOMExceptions = true, bool shouldReleaseCOMObject = true)
		{
			List<string> list = new List<string>();
			if (enumString == null)
			{
				throw new ArgumentNullException("enumString");
			}
			try
			{
				uint num = 0U;
				string empty = string.Empty;
				do
				{
					enumString.RemoteNext(1U, out empty, out num);
					if (num > 0U)
					{
						list.Add(empty);
					}
				}
				while (num > 0U);
			}
			catch (COMException obj) when (shouldSuppressCOMExceptions)
			{
			}
			finally
			{
				if (shouldReleaseCOMObject)
				{
					Marshal.ReleaseComObject(enumString);
				}
			}
			return list;
		}

		// Token: 0x060040A8 RID: 16552 RVA: 0x001273F8 File Offset: 0x001255F8
		[SecuritySafeCritical]
		internal static List<SpellChecker.SpellingError> ToList(this RCW.IEnumSpellingError spellingErrors, SpellChecker spellChecker, string text, bool shouldSuppressCOMExceptions = true, bool shouldReleaseCOMObject = true)
		{
			if (spellingErrors == null)
			{
				throw new ArgumentNullException("spellingErrors");
			}
			List<SpellChecker.SpellingError> list = new List<SpellChecker.SpellingError>();
			try
			{
				for (;;)
				{
					RCW.ISpellingError spellingError = spellingErrors.Next();
					if (spellingError == null)
					{
						break;
					}
					SpellChecker.SpellingError item = new SpellChecker.SpellingError(spellingError, spellChecker, text, shouldSuppressCOMExceptions, true);
					list.Add(item);
				}
			}
			catch (COMException obj) when (shouldSuppressCOMExceptions)
			{
			}
			finally
			{
				if (shouldReleaseCOMObject)
				{
					Marshal.ReleaseComObject(spellingErrors);
				}
			}
			return list;
		}

		// Token: 0x060040A9 RID: 16553 RVA: 0x00127478 File Offset: 0x00125678
		internal static bool IsClean(this List<SpellChecker.SpellingError> errors)
		{
			if (errors == null)
			{
				throw new ArgumentNullException("errors");
			}
			bool result = true;
			foreach (SpellChecker.SpellingError spellingError in errors)
			{
				if (spellingError.CorrectiveAction != SpellChecker.CorrectiveAction.None)
				{
					result = false;
					break;
				}
			}
			return result;
		}
	}
}
