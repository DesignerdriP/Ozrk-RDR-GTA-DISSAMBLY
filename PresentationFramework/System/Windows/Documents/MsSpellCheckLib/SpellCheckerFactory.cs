using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using MS.Internal;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x0200045B RID: 1115
	internal class SpellCheckerFactory
	{
		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x0600407F RID: 16511 RVA: 0x00126BFF File Offset: 0x00124DFF
		// (set) Token: 0x06004080 RID: 16512 RVA: 0x00126C07 File Offset: 0x00124E07
		internal RCW.ISpellCheckerFactory ComFactory { get; private set; }

		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x06004081 RID: 16513 RVA: 0x00126C10 File Offset: 0x00124E10
		// (set) Token: 0x06004082 RID: 16514 RVA: 0x00126C17 File Offset: 0x00124E17
		internal static SpellCheckerFactory Singleton { get; private set; } = new SpellCheckerFactory();

		// Token: 0x06004083 RID: 16515 RVA: 0x00126C20 File Offset: 0x00124E20
		public static SpellCheckerFactory Create(bool shouldSuppressCOMExceptions = false)
		{
			SpellCheckerFactory result = null;
			bool flag = false;
			if (SpellCheckerFactory._factoryLock.WithWriteLock<bool, bool, bool>(new Func<bool, bool, bool>(SpellCheckerFactory.CreateLockFree), shouldSuppressCOMExceptions, false, out flag) && flag)
			{
				result = SpellCheckerFactory.Singleton;
			}
			return result;
		}

		// Token: 0x06004084 RID: 16516 RVA: 0x0000326D File Offset: 0x0000146D
		private SpellCheckerFactory()
		{
		}

		// Token: 0x06004085 RID: 16517 RVA: 0x00126C58 File Offset: 0x00124E58
		static SpellCheckerFactory()
		{
			bool flag = false;
			SpellCheckerFactory._factoryLock.WithWriteLock<bool, bool, bool>(new Func<bool, bool, bool>(SpellCheckerFactory.CreateLockFree), true, true, out flag);
		}

		// Token: 0x06004086 RID: 16518 RVA: 0x00126CE0 File Offset: 0x00124EE0
		private static bool Reinitalize()
		{
			bool flag = false;
			return SpellCheckerFactory._factoryLock.WithWriteLock<bool, bool, bool>(new Func<bool, bool, bool>(SpellCheckerFactory.CreateLockFree), false, false, out flag) && flag;
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x00126D0C File Offset: 0x00124F0C
		[SecuritySafeCritical]
		private static bool CreateLockFree(bool suppressCOMExceptions = true, bool suppressOtherExceptions = true)
		{
			if (SpellCheckerFactory.Singleton.ComFactory != null)
			{
				try
				{
					Marshal.ReleaseComObject(SpellCheckerFactory.Singleton.ComFactory);
				}
				catch
				{
				}
				SpellCheckerFactory.Singleton.ComFactory = null;
			}
			bool flag = false;
			RCW.ISpellCheckerFactory comFactory = null;
			try
			{
				comFactory = new RCW.SpellCheckerFactoryCoClass();
				flag = true;
			}
			catch (COMException obj) when (suppressCOMExceptions)
			{
			}
			catch (UnauthorizedAccessException inner)
			{
				if (!suppressCOMExceptions)
				{
					throw new COMException(string.Empty, inner);
				}
			}
			catch (InvalidCastException inner2)
			{
				if (!suppressCOMExceptions)
				{
					throw new COMException(string.Empty, inner2);
				}
			}
			catch (Exception ex) when (suppressOtherExceptions && !(ex is COMException) && !(ex is UnauthorizedAccessException))
			{
			}
			if (flag)
			{
				SpellCheckerFactory.Singleton.ComFactory = comFactory;
			}
			return flag;
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x00126E10 File Offset: 0x00125010
		[SecuritySafeCritical]
		private List<string> SupportedLanguagesImpl()
		{
			RCW.ISpellCheckerFactory comFactory = this.ComFactory;
			RCW.IEnumString enumString = (comFactory != null) ? comFactory.SupportedLanguages : null;
			List<string> result = null;
			if (enumString != null)
			{
				result = enumString.ToList(true, true);
			}
			return result;
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x00126E40 File Offset: 0x00125040
		private List<string> SupportedLanguagesImplWithRetries(bool shouldSuppressCOMExceptions)
		{
			List<string> result = null;
			if (!RetryHelper.TryExecuteFunction<List<string>>(new Func<List<string>>(this.SupportedLanguagesImpl), out result, () => SpellCheckerFactory.Reinitalize(), SpellCheckerFactory.SuppressedExceptions[shouldSuppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x00126E98 File Offset: 0x00125098
		private List<string> GetSupportedLanguagesPrivate(bool shouldSuppressCOMExceptions = true)
		{
			List<string> result = null;
			if (!SpellCheckerFactory._factoryLock.WithWriteLock<bool, List<string>>(new Func<bool, List<string>>(this.SupportedLanguagesImplWithRetries), shouldSuppressCOMExceptions, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600408B RID: 16523 RVA: 0x00126EC7 File Offset: 0x001250C7
		internal static List<string> GetSupportedLanguages(bool shouldSuppressCOMExceptions = true)
		{
			SpellCheckerFactory singleton = SpellCheckerFactory.Singleton;
			if (singleton == null)
			{
				return null;
			}
			return singleton.GetSupportedLanguagesPrivate(shouldSuppressCOMExceptions);
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x00126EDA File Offset: 0x001250DA
		[SecuritySafeCritical]
		private bool IsSupportedImpl(string languageTag)
		{
			return this.ComFactory != null && this.ComFactory.IsSupported(languageTag) != 0;
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x00126EF8 File Offset: 0x001250F8
		private bool IsSupportedImplWithRetries(string languageTag, bool suppressCOMExceptions = true)
		{
			bool flag = false;
			return RetryHelper.TryExecuteFunction<bool>(() => this.IsSupportedImpl(languageTag), out flag, () => SpellCheckerFactory.Reinitalize(), SpellCheckerFactory.SuppressedExceptions[suppressCOMExceptions], 3, false) && flag;
		}

		// Token: 0x0600408E RID: 16526 RVA: 0x00126F64 File Offset: 0x00125164
		private bool IsSupportedPrivate(string languageTag, bool suppressCOMExceptons = true)
		{
			bool flag = false;
			return SpellCheckerFactory._factoryLock.WithWriteLock<string, bool, bool>(new Func<string, bool, bool>(this.IsSupportedImplWithRetries), languageTag, suppressCOMExceptons, out flag) && flag;
		}

		// Token: 0x0600408F RID: 16527 RVA: 0x00126F94 File Offset: 0x00125194
		internal static bool IsSupported(string languageTag, bool suppressCOMExceptons = true)
		{
			return SpellCheckerFactory.Singleton != null && SpellCheckerFactory.Singleton.IsSupportedPrivate(languageTag, suppressCOMExceptons);
		}

		// Token: 0x06004090 RID: 16528 RVA: 0x00126FAB File Offset: 0x001251AB
		[SecurityCritical]
		private RCW.ISpellChecker CreateSpellCheckerImpl(string languageTag)
		{
			return SpellCheckerFactory.SpellCheckerCreationHelper.Helper(languageTag).CreateSpellChecker();
		}

		// Token: 0x06004091 RID: 16529 RVA: 0x00126FB8 File Offset: 0x001251B8
		[SecurityCritical]
		private RCW.ISpellChecker CreateSpellCheckerImplWithRetries(string languageTag, bool suppressCOMExceptions = true)
		{
			RCW.ISpellChecker result = null;
			if (!RetryHelper.TryExecuteFunction<RCW.ISpellChecker>(new Func<RCW.ISpellChecker>(SpellCheckerFactory.SpellCheckerCreationHelper.Helper(languageTag).CreateSpellChecker), out result, new RetryHelper.RetryFunctionPreamble<RCW.ISpellChecker>(SpellCheckerFactory.SpellCheckerCreationHelper.Helper(languageTag).CreateSpellCheckerRetryPreamble), SpellCheckerFactory.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004092 RID: 16530 RVA: 0x00127004 File Offset: 0x00125204
		[SecurityCritical]
		private RCW.ISpellChecker CreateSpellCheckerPrivate(string languageTag, bool suppressCOMExceptions = true)
		{
			RCW.ISpellChecker result = null;
			if (!SpellCheckerFactory._factoryLock.WithWriteLock<string, bool, RCW.ISpellChecker>(new Func<string, bool, RCW.ISpellChecker>(this.CreateSpellCheckerImplWithRetries), languageTag, suppressCOMExceptions, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004093 RID: 16531 RVA: 0x00127034 File Offset: 0x00125234
		[SecurityCritical]
		internal static RCW.ISpellChecker CreateSpellChecker(string languageTag, bool suppressCOMExceptions = true)
		{
			SpellCheckerFactory singleton = SpellCheckerFactory.Singleton;
			if (singleton == null)
			{
				return null;
			}
			return singleton.CreateSpellCheckerPrivate(languageTag, suppressCOMExceptions);
		}

		// Token: 0x06004094 RID: 16532 RVA: 0x00127048 File Offset: 0x00125248
		[SecuritySafeCritical]
		private void RegisterUserDicionaryImpl(string dictionaryPath, string languageTag)
		{
			RCW.IUserDictionariesRegistrar userDictionariesRegistrar = (RCW.IUserDictionariesRegistrar)this.ComFactory;
			if (userDictionariesRegistrar != null)
			{
				userDictionariesRegistrar.RegisterUserDictionary(dictionaryPath, languageTag);
			}
		}

		// Token: 0x06004095 RID: 16533 RVA: 0x0012706C File Offset: 0x0012526C
		private void RegisterUserDictionaryImplWithRetries(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			if (dictionaryPath == null)
			{
				throw new ArgumentNullException("dictionaryPath");
			}
			if (languageTag == null)
			{
				throw new ArgumentNullException("languageTag");
			}
			RetryHelper.TryCallAction(delegate()
			{
				this.RegisterUserDicionaryImpl(dictionaryPath, languageTag);
			}, () => SpellCheckerFactory.Reinitalize(), SpellCheckerFactory.SuppressedExceptions[suppressCOMExceptions], 3, false);
		}

		// Token: 0x06004096 RID: 16534 RVA: 0x001270F8 File Offset: 0x001252F8
		private void RegisterUserDictionaryPrivate(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			SpellCheckerFactory._factoryLock.WithWriteLock(delegate()
			{
				this.RegisterUserDictionaryImplWithRetries(dictionaryPath, languageTag, suppressCOMExceptions);
			});
		}

		// Token: 0x06004097 RID: 16535 RVA: 0x0012713E File Offset: 0x0012533E
		internal static void RegisterUserDictionary(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			SpellCheckerFactory singleton = SpellCheckerFactory.Singleton;
			if (singleton == null)
			{
				return;
			}
			singleton.RegisterUserDictionaryPrivate(dictionaryPath, languageTag, suppressCOMExceptions);
		}

		// Token: 0x06004098 RID: 16536 RVA: 0x00127154 File Offset: 0x00125354
		[SecuritySafeCritical]
		private void UnregisterUserDictionaryImpl(string dictionaryPath, string languageTag)
		{
			RCW.IUserDictionariesRegistrar userDictionariesRegistrar = (RCW.IUserDictionariesRegistrar)this.ComFactory;
			if (userDictionariesRegistrar != null)
			{
				userDictionariesRegistrar.UnregisterUserDictionary(dictionaryPath, languageTag);
			}
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x00127178 File Offset: 0x00125378
		private void UnregisterUserDictionaryImplWithRetries(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			if (dictionaryPath == null)
			{
				throw new ArgumentNullException("dictionaryPath");
			}
			if (languageTag == null)
			{
				throw new ArgumentNullException("languageTag");
			}
			RetryHelper.TryCallAction(delegate()
			{
				this.UnregisterUserDictionaryImpl(dictionaryPath, languageTag);
			}, () => SpellCheckerFactory.Reinitalize(), SpellCheckerFactory.SuppressedExceptions[suppressCOMExceptions], 3, false);
		}

		// Token: 0x0600409A RID: 16538 RVA: 0x00127204 File Offset: 0x00125404
		private void UnregisterUserDictionaryPrivate(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			SpellCheckerFactory._factoryLock.WithWriteLock(delegate()
			{
				this.UnregisterUserDictionaryImplWithRetries(dictionaryPath, languageTag, suppressCOMExceptions);
			});
		}

		// Token: 0x0600409B RID: 16539 RVA: 0x0012724A File Offset: 0x0012544A
		internal static void UnregisterUserDictionary(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			SpellCheckerFactory singleton = SpellCheckerFactory.Singleton;
			if (singleton == null)
			{
				return;
			}
			singleton.UnregisterUserDictionaryPrivate(dictionaryPath, languageTag, suppressCOMExceptions);
		}

		// Token: 0x0400276B RID: 10091
		private static ReaderWriterLockSlimWrapper _factoryLock = new ReaderWriterLockSlimWrapper(LockRecursionPolicy.SupportsRecursion, true);

		// Token: 0x0400276D RID: 10093
		private static Dictionary<bool, List<Type>> SuppressedExceptions = new Dictionary<bool, List<Type>>
		{
			{
				false,
				new List<Type>()
			},
			{
				true,
				new List<Type>
				{
					typeof(COMException),
					typeof(UnauthorizedAccessException)
				}
			}
		};

		// Token: 0x02000949 RID: 2377
		private class SpellCheckerCreationHelper
		{
			// Token: 0x17001E83 RID: 7811
			// (get) Token: 0x060086EA RID: 34538 RVA: 0x0024EAC7 File Offset: 0x0024CCC7
			public static RCW.ISpellCheckerFactory ComFactory
			{
				[SecurityCritical]
				get
				{
					return SpellCheckerFactory.Singleton.ComFactory;
				}
			}

			// Token: 0x060086EB RID: 34539 RVA: 0x0024EAD3 File Offset: 0x0024CCD3
			private SpellCheckerCreationHelper(string language)
			{
				this._language = language;
			}

			// Token: 0x060086EC RID: 34540 RVA: 0x0024EAE2 File Offset: 0x0024CCE2
			private static void CreateForLanguage(string language)
			{
				SpellCheckerFactory.SpellCheckerCreationHelper._instances[language] = new SpellCheckerFactory.SpellCheckerCreationHelper(language);
			}

			// Token: 0x060086ED RID: 34541 RVA: 0x0024EAF5 File Offset: 0x0024CCF5
			public static SpellCheckerFactory.SpellCheckerCreationHelper Helper(string language)
			{
				if (!SpellCheckerFactory.SpellCheckerCreationHelper._instances.ContainsKey(language))
				{
					SpellCheckerFactory.SpellCheckerCreationHelper._lock.WithWriteLock<string>(new Action<string>(SpellCheckerFactory.SpellCheckerCreationHelper.CreateForLanguage), language);
				}
				return SpellCheckerFactory.SpellCheckerCreationHelper._instances[language];
			}

			// Token: 0x060086EE RID: 34542 RVA: 0x0024EB27 File Offset: 0x0024CD27
			[SecurityCritical]
			public RCW.ISpellChecker CreateSpellChecker()
			{
				RCW.ISpellCheckerFactory comFactory = SpellCheckerFactory.SpellCheckerCreationHelper.ComFactory;
				if (comFactory == null)
				{
					return null;
				}
				return comFactory.CreateSpellChecker(this._language);
			}

			// Token: 0x060086EF RID: 34543 RVA: 0x0024EB40 File Offset: 0x0024CD40
			[SecurityCritical]
			public bool CreateSpellCheckerRetryPreamble(out Func<RCW.ISpellChecker> func)
			{
				func = null;
				bool result;
				if (result = SpellCheckerFactory.Reinitalize())
				{
					func = new Func<RCW.ISpellChecker>(SpellCheckerFactory.SpellCheckerCreationHelper.Helper(this._language).CreateSpellChecker);
				}
				return result;
			}

			// Token: 0x040043D2 RID: 17362
			private static Dictionary<string, SpellCheckerFactory.SpellCheckerCreationHelper> _instances = new Dictionary<string, SpellCheckerFactory.SpellCheckerCreationHelper>();

			// Token: 0x040043D3 RID: 17363
			private static ReaderWriterLockSlimWrapper _lock = new ReaderWriterLockSlimWrapper(LockRecursionPolicy.NoRecursion, true);

			// Token: 0x040043D4 RID: 17364
			private string _language;
		}
	}
}
