using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x0200045A RID: 1114
	internal class SpellChecker : IDisposable
	{
		// Token: 0x0600404B RID: 16459 RVA: 0x00126158 File Offset: 0x00124358
		public SpellChecker(string languageTag)
		{
			this._speller = new ChangeNotifyWrapper<RCW.ISpellChecker>(null, false);
			this._languageTag = languageTag;
			this._spellCheckerChangedEventHandler = new SpellChecker.SpellCheckerChangedEventHandler(this);
			if (this.Init(false))
			{
				this._speller.PropertyChanged += this.SpellerInstanceChanged;
			}
		}

		// Token: 0x0600404C RID: 16460 RVA: 0x001261AB File Offset: 0x001243AB
		[SecuritySafeCritical]
		private bool Init(bool shouldSuppressCOMExceptions = true)
		{
			this._speller.Value = SpellCheckerFactory.CreateSpellChecker(this._languageTag, shouldSuppressCOMExceptions);
			return this._speller.Value != null;
		}

		// Token: 0x0600404D RID: 16461 RVA: 0x001261D2 File Offset: 0x001243D2
		public string GetLanguageTag()
		{
			if (!this._disposed)
			{
				return this._languageTag;
			}
			return null;
		}

		// Token: 0x0600404E RID: 16462 RVA: 0x001261E4 File Offset: 0x001243E4
		[SecuritySafeCritical]
		public List<string> SuggestImpl(string word)
		{
			RCW.IEnumString enumString = this._speller.Value.Suggest(word);
			if (enumString == null)
			{
				return null;
			}
			return enumString.ToList(false, true);
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x00126210 File Offset: 0x00124410
		public List<string> SuggestImplWithRetries(string word, bool shouldSuppressCOMExceptions = true)
		{
			List<string> result = null;
			if (!RetryHelper.TryExecuteFunction<List<string>>(() => this.SuggestImpl(word), out result, () => this.Init(shouldSuppressCOMExceptions), SpellChecker.SuppressedExceptions[shouldSuppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x00126272 File Offset: 0x00124472
		public List<string> Suggest(string word, bool shouldSuppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.SuggestImplWithRetries(word, shouldSuppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x00126286 File Offset: 0x00124486
		[SecuritySafeCritical]
		private void AddImpl(string word)
		{
			this._speller.Value.Add(word);
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x0012629C File Offset: 0x0012449C
		private void AddImplWithRetries(string word, bool shouldSuppressCOMExceptions = true)
		{
			RetryHelper.TryCallAction(delegate()
			{
				this.AddImpl(word);
			}, () => this.Init(shouldSuppressCOMExceptions), SpellChecker.SuppressedExceptions[shouldSuppressCOMExceptions], 3, false);
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x001262F4 File Offset: 0x001244F4
		public void Add(string word, bool shouldSuppressCOMExceptions = true)
		{
			if (this._disposed)
			{
				return;
			}
			this.AddImplWithRetries(word, shouldSuppressCOMExceptions);
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x00126307 File Offset: 0x00124507
		[SecuritySafeCritical]
		private void IgnoreImpl(string word)
		{
			this._speller.Value.Ignore(word);
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x0012631C File Offset: 0x0012451C
		public void IgnoreImplWithRetries(string word, bool shouldSuppressCOMExceptions = true)
		{
			RetryHelper.TryCallAction(delegate()
			{
				this.IgnoreImpl(word);
			}, () => this.Init(shouldSuppressCOMExceptions), SpellChecker.SuppressedExceptions[shouldSuppressCOMExceptions], 3, false);
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x00126374 File Offset: 0x00124574
		public void Ignore(string word, bool shouldSuppressCOMExceptions = true)
		{
			if (this._disposed)
			{
				return;
			}
			this.IgnoreImplWithRetries(word, shouldSuppressCOMExceptions);
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x00126387 File Offset: 0x00124587
		[SecuritySafeCritical]
		private void AutoCorrectImpl(string from, string to)
		{
			this._speller.Value.AutoCorrect(from, to);
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x0012639C File Offset: 0x0012459C
		private void AutoCorrectImplWithRetries(string from, string to, bool suppressCOMExceptions = true)
		{
			RetryHelper.TryCallAction(delegate()
			{
				this.AutoCorrectImpl(from, to);
			}, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false);
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x001263FB File Offset: 0x001245FB
		public void AutoCorrect(string from, string to, bool suppressCOMExceptions = true)
		{
			this.AutoCorrectImplWithRetries(from, to, suppressCOMExceptions);
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x00126406 File Offset: 0x00124606
		[SecuritySafeCritical]
		private byte GetOptionValueImpl(string optionId)
		{
			return this._speller.Value.GetOptionValue(optionId);
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x0012641C File Offset: 0x0012461C
		private byte GetOptionValueImplWithRetries(string optionId, bool suppressCOMExceptions = true)
		{
			byte result;
			if (!RetryHelper.TryExecuteFunction<byte>(() => this.GetOptionValueImpl(optionId), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return 0;
			}
			return result;
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x0012647C File Offset: 0x0012467C
		public byte GetOptionValue(string optionId, bool suppressCOMExceptions = true)
		{
			return this.GetOptionValueImplWithRetries(optionId, suppressCOMExceptions);
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x00126488 File Offset: 0x00124688
		[SecuritySafeCritical]
		private List<string> GetOptionIdsImpl()
		{
			RCW.IEnumString optionIds = this._speller.Value.OptionIds;
			if (optionIds == null)
			{
				return null;
			}
			return optionIds.ToList(false, true);
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x001264B4 File Offset: 0x001246B4
		private List<string> GetOptionIdsImplWithRetries(bool suppressCOMExceptions)
		{
			List<string> result = null;
			if (!RetryHelper.TryExecuteFunction<List<string>>(new Func<List<string>>(this.GetOptionIdsImpl), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x0012650F File Offset: 0x0012470F
		public List<string> GetOptionIds(bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.GetOptionIdsImplWithRetries(suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x00126522 File Offset: 0x00124722
		[SecuritySafeCritical]
		private string GetIdImpl()
		{
			return this._speller.Value.Id;
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x00126534 File Offset: 0x00124734
		private string GetIdImplWithRetries(bool suppressCOMExceptions)
		{
			string result = null;
			if (!RetryHelper.TryExecuteFunction<string>(new Func<string>(this.GetIdImpl), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x0012658F File Offset: 0x0012478F
		private string GetId(bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.GetIdImplWithRetries(suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x001265A2 File Offset: 0x001247A2
		[SecuritySafeCritical]
		private string GetLocalizedNameImpl()
		{
			return this._speller.Value.LocalizedName;
		}

		// Token: 0x06004064 RID: 16484 RVA: 0x001265B4 File Offset: 0x001247B4
		private string GetLocalizedNameImplWithRetries(bool suppressCOMExceptions)
		{
			string result = null;
			if (!RetryHelper.TryExecuteFunction<string>(new Func<string>(this.GetLocalizedNameImpl), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004065 RID: 16485 RVA: 0x0012660F File Offset: 0x0012480F
		public string GetLocalizedName(bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.GetLocalizedNameImplWithRetries(suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x00126624 File Offset: 0x00124824
		[SecuritySafeCritical]
		private SpellChecker.OptionDescription GetOptionDescriptionImpl(string optionId)
		{
			RCW.IOptionDescription optionDescription = this._speller.Value.GetOptionDescription(optionId);
			if (optionDescription == null)
			{
				return null;
			}
			return SpellChecker.OptionDescription.Create(optionDescription, false, true);
		}

		// Token: 0x06004067 RID: 16487 RVA: 0x00126650 File Offset: 0x00124850
		private SpellChecker.OptionDescription GetOptionDescriptionImplWithRetries(string optionId, bool suppressCOMExceptions)
		{
			SpellChecker.OptionDescription result = null;
			if (!RetryHelper.TryExecuteFunction<SpellChecker.OptionDescription>(() => this.GetOptionDescriptionImpl(optionId), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x001266B2 File Offset: 0x001248B2
		public SpellChecker.OptionDescription GetOptionDescription(string optionId, bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.GetOptionDescriptionImplWithRetries(optionId, suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x001266C8 File Offset: 0x001248C8
		[SecuritySafeCritical]
		private List<SpellChecker.SpellingError> CheckImpl(string text)
		{
			RCW.IEnumSpellingError enumSpellingError = this._speller.Value.Check(text);
			if (enumSpellingError == null)
			{
				return null;
			}
			return enumSpellingError.ToList(this, text, false, true);
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x001266F8 File Offset: 0x001248F8
		private List<SpellChecker.SpellingError> CheckImplWithRetries(string text, bool suppressCOMExceptions)
		{
			List<SpellChecker.SpellingError> result = null;
			if (!RetryHelper.TryExecuteFunction<List<SpellChecker.SpellingError>>(() => this.CheckImpl(text), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600406B RID: 16491 RVA: 0x0012675A File Offset: 0x0012495A
		public List<SpellChecker.SpellingError> Check(string text, bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.CheckImplWithRetries(text, suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x0600406C RID: 16492 RVA: 0x00126770 File Offset: 0x00124970
		[SecuritySafeCritical]
		public List<SpellChecker.SpellingError> ComprehensiveCheckImpl(string text)
		{
			RCW.IEnumSpellingError enumSpellingError = this._speller.Value.ComprehensiveCheck(text);
			if (enumSpellingError == null)
			{
				return null;
			}
			return enumSpellingError.ToList(this, text, false, true);
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x001267A0 File Offset: 0x001249A0
		public List<SpellChecker.SpellingError> ComprehensiveCheckImplWithRetries(string text, bool shouldSuppressCOMExceptions = true)
		{
			List<SpellChecker.SpellingError> result = null;
			if (!RetryHelper.TryExecuteFunction<List<SpellChecker.SpellingError>>(() => this.ComprehensiveCheckImpl(text), out result, () => this.Init(shouldSuppressCOMExceptions), SpellChecker.SuppressedExceptions[shouldSuppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600406E RID: 16494 RVA: 0x00126802 File Offset: 0x00124A02
		public List<SpellChecker.SpellingError> ComprehensiveCheck(string text, bool shouldSuppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.ComprehensiveCheckImplWithRetries(text, shouldSuppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x00126818 File Offset: 0x00124A18
		[SecuritySafeCritical]
		private uint? add_SpellCheckerChangedImpl(RCW.ISpellCheckerChangedEventHandler handler)
		{
			if (handler == null)
			{
				return new uint?(this._speller.Value.add_SpellCheckerChanged(handler));
			}
			return null;
		}

		// Token: 0x06004070 RID: 16496 RVA: 0x00126848 File Offset: 0x00124A48
		private uint? addSpellCheckerChangedImplWithRetries(RCW.ISpellCheckerChangedEventHandler handler, bool suppressCOMExceptions)
		{
			uint? result;
			if (!RetryHelper.TryExecuteFunction<uint?>(() => this.add_SpellCheckerChangedImpl(handler), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004071 RID: 16497 RVA: 0x001268B0 File Offset: 0x00124AB0
		private uint? add_SpellCheckerChanged(RCW.ISpellCheckerChangedEventHandler handler, bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.addSpellCheckerChangedImplWithRetries(handler, suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06004072 RID: 16498 RVA: 0x001268D7 File Offset: 0x00124AD7
		[SecuritySafeCritical]
		private void remove_SpellCheckerChangedImpl(uint eventCookie)
		{
			this._speller.Value.remove_SpellCheckerChanged(eventCookie);
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x001268EC File Offset: 0x00124AEC
		private void remove_SpellCheckerChangedImplWithRetries(uint eventCookie, bool suppressCOMExceptions = true)
		{
			RetryHelper.TryCallAction(delegate()
			{
				this.remove_SpellCheckerChangedImpl(eventCookie);
			}, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false);
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x00126944 File Offset: 0x00124B44
		private void remove_SpellCheckerChanged(uint eventCookie, bool suppressCOMExceptions = true)
		{
			if (this._disposed)
			{
				return;
			}
			this.remove_SpellCheckerChangedImplWithRetries(eventCookie, suppressCOMExceptions);
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x00126958 File Offset: 0x00124B58
		private void SpellerInstanceChanged(object sender, PropertyChangedEventArgs args)
		{
			if (this._changed != null)
			{
				EventHandler<SpellChecker.SpellCheckerChangedEventArgs> changed = this._changed;
				lock (changed)
				{
					if (this._changed != null)
					{
						this._eventCookie = this.add_SpellCheckerChanged(this._spellCheckerChangedEventHandler, true);
					}
				}
			}
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x001269B8 File Offset: 0x00124BB8
		internal virtual void OnChanged(SpellChecker.SpellCheckerChangedEventArgs e)
		{
			EventHandler<SpellChecker.SpellCheckerChangedEventArgs> changed = this._changed;
			if (changed == null)
			{
				return;
			}
			changed(this, e);
		}

		// Token: 0x14000097 RID: 151
		// (add) Token: 0x06004077 RID: 16503 RVA: 0x001269CC File Offset: 0x00124BCC
		// (remove) Token: 0x06004078 RID: 16504 RVA: 0x00126A28 File Offset: 0x00124C28
		public event EventHandler<SpellChecker.SpellCheckerChangedEventArgs> Changed
		{
			add
			{
				EventHandler<SpellChecker.SpellCheckerChangedEventArgs> changed = this._changed;
				lock (changed)
				{
					if (this._changed == null)
					{
						this._eventCookie = this.add_SpellCheckerChanged(this._spellCheckerChangedEventHandler, true);
					}
					this._changed += value;
				}
			}
			remove
			{
				EventHandler<SpellChecker.SpellCheckerChangedEventArgs> changed = this._changed;
				lock (changed)
				{
					this._changed -= value;
					if (this._changed == null && this._eventCookie != null)
					{
						this.remove_SpellCheckerChanged(this._eventCookie.Value, true);
						this._eventCookie = null;
					}
				}
			}
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x00126A9C File Offset: 0x00124C9C
		[SecuritySafeCritical]
		protected virtual void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			ChangeNotifyWrapper<RCW.ISpellChecker> speller = this._speller;
			if (((speller != null) ? speller.Value : null) != null)
			{
				try
				{
					Marshal.ReleaseComObject(this._speller.Value);
				}
				catch
				{
				}
				this._speller = null;
			}
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x00126AFC File Offset: 0x00124CFC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600407B RID: 16507 RVA: 0x00126B0C File Offset: 0x00124D0C
		~SpellChecker()
		{
			this.Dispose(false);
		}

		// Token: 0x14000098 RID: 152
		// (add) Token: 0x0600407C RID: 16508 RVA: 0x00126B3C File Offset: 0x00124D3C
		// (remove) Token: 0x0600407D RID: 16509 RVA: 0x00126B74 File Offset: 0x00124D74
		private event EventHandler<SpellChecker.SpellCheckerChangedEventArgs> _changed;

		// Token: 0x04002763 RID: 10083
		private static readonly Dictionary<bool, List<Type>> SuppressedExceptions = new Dictionary<bool, List<Type>>
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

		// Token: 0x04002764 RID: 10084
		private ChangeNotifyWrapper<RCW.ISpellChecker> _speller;

		// Token: 0x04002765 RID: 10085
		private string _languageTag;

		// Token: 0x04002766 RID: 10086
		private SpellChecker.SpellCheckerChangedEventHandler _spellCheckerChangedEventHandler;

		// Token: 0x04002767 RID: 10087
		private uint? _eventCookie;

		// Token: 0x04002769 RID: 10089
		private bool _disposed;

		// Token: 0x02000937 RID: 2359
		internal class OptionDescription
		{
			// Token: 0x17001E79 RID: 7801
			// (get) Token: 0x060086B1 RID: 34481 RVA: 0x0024E698 File Offset: 0x0024C898
			// (set) Token: 0x060086B2 RID: 34482 RVA: 0x0024E6A0 File Offset: 0x0024C8A0
			internal string Id { get; private set; }

			// Token: 0x17001E7A RID: 7802
			// (get) Token: 0x060086B3 RID: 34483 RVA: 0x0024E6A9 File Offset: 0x0024C8A9
			// (set) Token: 0x060086B4 RID: 34484 RVA: 0x0024E6B1 File Offset: 0x0024C8B1
			internal string Heading { get; private set; }

			// Token: 0x17001E7B RID: 7803
			// (get) Token: 0x060086B5 RID: 34485 RVA: 0x0024E6BA File Offset: 0x0024C8BA
			// (set) Token: 0x060086B6 RID: 34486 RVA: 0x0024E6C2 File Offset: 0x0024C8C2
			internal string Description { get; private set; }

			// Token: 0x17001E7C RID: 7804
			// (get) Token: 0x060086B7 RID: 34487 RVA: 0x0024E6CB File Offset: 0x0024C8CB
			internal IReadOnlyCollection<string> Labels
			{
				get
				{
					return this._labels.AsReadOnly();
				}
			}

			// Token: 0x060086B8 RID: 34488 RVA: 0x0024E6D8 File Offset: 0x0024C8D8
			private OptionDescription(string id, string heading, string description, List<string> labels = null)
			{
				this.Id = id;
				this.Heading = heading;
				this.Description = description;
				this._labels = (labels ?? new List<string>());
			}

			// Token: 0x060086B9 RID: 34489 RVA: 0x0024E708 File Offset: 0x0024C908
			[SecuritySafeCritical]
			internal static SpellChecker.OptionDescription Create(RCW.IOptionDescription optionDescription, bool shouldSuppressCOMExceptions = true, bool shouldReleaseCOMObject = true)
			{
				if (optionDescription == null)
				{
					throw new ArgumentNullException("optionDescription");
				}
				SpellChecker.OptionDescription optionDescription2 = new SpellChecker.OptionDescription(optionDescription.Id, optionDescription.Heading, optionDescription.Description, null);
				try
				{
					optionDescription2._labels = optionDescription.Labels.ToList(true, true);
				}
				catch (COMException obj) when (shouldSuppressCOMExceptions)
				{
				}
				finally
				{
					if (shouldReleaseCOMObject)
					{
						Marshal.ReleaseComObject(optionDescription);
					}
				}
				return optionDescription2;
			}

			// Token: 0x0400439F RID: 17311
			private List<string> _labels;
		}

		// Token: 0x02000938 RID: 2360
		internal class SpellCheckerChangedEventArgs : EventArgs
		{
			// Token: 0x060086BA RID: 34490 RVA: 0x0024E790 File Offset: 0x0024C990
			internal SpellCheckerChangedEventArgs(SpellChecker spellChecker)
			{
				this.SpellChecker = spellChecker;
			}

			// Token: 0x17001E7D RID: 7805
			// (get) Token: 0x060086BB RID: 34491 RVA: 0x0024E79F File Offset: 0x0024C99F
			// (set) Token: 0x060086BC RID: 34492 RVA: 0x0024E7A7 File Offset: 0x0024C9A7
			internal SpellChecker SpellChecker { get; private set; }
		}

		// Token: 0x02000939 RID: 2361
		private class SpellCheckerChangedEventHandler : RCW.ISpellCheckerChangedEventHandler
		{
			// Token: 0x060086BD RID: 34493 RVA: 0x0024E7B0 File Offset: 0x0024C9B0
			internal SpellCheckerChangedEventHandler(SpellChecker spellChecker)
			{
				this._spellChecker = spellChecker;
				this._eventArgs = new SpellChecker.SpellCheckerChangedEventArgs(this._spellChecker);
			}

			// Token: 0x060086BE RID: 34494 RVA: 0x0024E7D0 File Offset: 0x0024C9D0
			public void Invoke(RCW.ISpellChecker sender)
			{
				SpellChecker spellChecker = this._spellChecker;
				RCW.ISpellChecker spellChecker2;
				if (spellChecker == null)
				{
					spellChecker2 = null;
				}
				else
				{
					ChangeNotifyWrapper<RCW.ISpellChecker> speller = spellChecker._speller;
					spellChecker2 = ((speller != null) ? speller.Value : null);
				}
				if (sender == spellChecker2)
				{
					SpellChecker spellChecker3 = this._spellChecker;
					if (spellChecker3 == null)
					{
						return;
					}
					spellChecker3.OnChanged(this._eventArgs);
				}
			}

			// Token: 0x040043A1 RID: 17313
			private SpellChecker.SpellCheckerChangedEventArgs _eventArgs;

			// Token: 0x040043A2 RID: 17314
			private SpellChecker _spellChecker;
		}

		// Token: 0x0200093A RID: 2362
		internal enum CorrectiveAction
		{
			// Token: 0x040043A4 RID: 17316
			None,
			// Token: 0x040043A5 RID: 17317
			GetSuggestions,
			// Token: 0x040043A6 RID: 17318
			Replace,
			// Token: 0x040043A7 RID: 17319
			Delete
		}

		// Token: 0x0200093B RID: 2363
		internal class SpellingError
		{
			// Token: 0x17001E7E RID: 7806
			// (get) Token: 0x060086BF RID: 34495 RVA: 0x0024E809 File Offset: 0x0024CA09
			internal uint StartIndex { get; }

			// Token: 0x17001E7F RID: 7807
			// (get) Token: 0x060086C0 RID: 34496 RVA: 0x0024E811 File Offset: 0x0024CA11
			internal uint Length { get; }

			// Token: 0x17001E80 RID: 7808
			// (get) Token: 0x060086C1 RID: 34497 RVA: 0x0024E819 File Offset: 0x0024CA19
			internal SpellChecker.CorrectiveAction CorrectiveAction { get; }

			// Token: 0x17001E81 RID: 7809
			// (get) Token: 0x060086C2 RID: 34498 RVA: 0x0024E821 File Offset: 0x0024CA21
			internal string Replacement { get; }

			// Token: 0x17001E82 RID: 7810
			// (get) Token: 0x060086C3 RID: 34499 RVA: 0x0024E829 File Offset: 0x0024CA29
			internal IReadOnlyCollection<string> Suggestions
			{
				get
				{
					return this._suggestions.AsReadOnly();
				}
			}

			// Token: 0x060086C4 RID: 34500 RVA: 0x0024E838 File Offset: 0x0024CA38
			[SecuritySafeCritical]
			internal SpellingError(RCW.ISpellingError error, SpellChecker spellChecker, string text, bool shouldSuppressCOMExceptions = true, bool shouldReleaseCOMObject = true)
			{
				if (error == null)
				{
					throw new ArgumentNullException("error");
				}
				this.StartIndex = error.StartIndex;
				this.Length = error.Length;
				this.CorrectiveAction = error.CorrectiveAction;
				this.Replacement = error.Replacement;
				this.PopulateSuggestions(error, spellChecker, text, shouldSuppressCOMExceptions, shouldReleaseCOMObject);
			}

			// Token: 0x060086C5 RID: 34501 RVA: 0x0024E898 File Offset: 0x0024CA98
			[SecuritySafeCritical]
			private void PopulateSuggestions(RCW.ISpellingError error, SpellChecker spellChecker, string text, bool shouldSuppressCOMExceptions, bool shouldReleaseCOMObject)
			{
				try
				{
					this._suggestions = new List<string>();
					if (this.CorrectiveAction == SpellChecker.CorrectiveAction.GetSuggestions)
					{
						List<string> collection = spellChecker.Suggest(text, shouldSuppressCOMExceptions);
						this._suggestions.AddRange(collection);
					}
					else if (this.CorrectiveAction == SpellChecker.CorrectiveAction.Replace)
					{
						this._suggestions.Add(this.Replacement);
					}
				}
				finally
				{
					if (shouldReleaseCOMObject)
					{
						Marshal.ReleaseComObject(error);
					}
				}
			}

			// Token: 0x040043AC RID: 17324
			private List<string> _suggestions;
		}
	}
}
