﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Documents.MsSpellCheckLib;
using System.Windows.Documents.Tracing;
using System.Windows.Input;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.WindowsRuntime.Windows.Data.Text;

namespace System.Windows.Documents
{
	// Token: 0x02000430 RID: 1072
	internal class WinRTSpellerInterop : SpellerInteropBase
	{
		// Token: 0x06003EFF RID: 16127 RVA: 0x0011F6D8 File Offset: 0x0011D8D8
		[SecuritySafeCritical]
		internal WinRTSpellerInterop()
		{
			new FileIOPermission(PermissionState.None)
			{
				AllLocalFiles = FileIOPermissionAccess.PathDiscovery
			}.Assert();
			try
			{
				SpellCheckerFactory.Create(false);
			}
			catch (Exception ex) when (ex is InvalidCastException || ex is COMException)
			{
				this.Dispose();
				throw new PlatformNotSupportedException(string.Empty, ex);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			this._spellCheckers = new Dictionary<CultureInfo, Tuple<WordsSegmenter, SpellChecker>>();
			this._customDictionaryFiles = new Dictionary<string, List<string>>();
			InputLanguageManager inputLanguageManager = InputLanguageManager.Current;
			this._defaultCulture = (((inputLanguageManager != null) ? inputLanguageManager.CurrentInputLanguage : null) ?? Thread.CurrentThread.CurrentCulture);
			this._culture = null;
			try
			{
				this.EnsureWordBreakerAndSpellCheckerForCulture(this._defaultCulture, true);
			}
			catch (Exception ex2) when (ex2 is ArgumentException || ex2 is NotSupportedException || ex2 is PlatformNotSupportedException)
			{
				this._spellCheckers = null;
				this.Dispose();
				if (ex2 is PlatformNotSupportedException || ex2 is NotSupportedException)
				{
					throw;
				}
				throw new NotSupportedException(string.Empty, ex2);
			}
			this._dispatcher = new WeakReference<Dispatcher>(Dispatcher.CurrentDispatcher);
			WeakEventManager<AppDomain, UnhandledExceptionEventArgs>.AddHandler(AppDomain.CurrentDomain, "UnhandledException", new EventHandler<UnhandledExceptionEventArgs>(this.ProcessUnhandledException));
		}

		// Token: 0x06003F00 RID: 16128 RVA: 0x0011F848 File Offset: 0x0011DA48
		~WinRTSpellerInterop()
		{
			this.Dispose(false);
		}

		// Token: 0x06003F01 RID: 16129 RVA: 0x000F3064 File Offset: 0x000F1264
		public override void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003F02 RID: 16130 RVA: 0x0011F878 File Offset: 0x0011DA78
		protected override void Dispose(bool disposing)
		{
			if (this._isDisposed)
			{
				throw new ObjectDisposedException(SR.Get("TextEditorSpellerInteropHasBeenDisposed"));
			}
			try
			{
				if (this.BeginInvokeOnUIThread(new Action<bool>(this.Dispose), DispatcherPriority.Normal, new object[]
				{
					disposing
				}) == null)
				{
					this.ReleaseAllResources(disposing);
					this._isDisposed = true;
				}
			}
			catch (InvalidOperationException)
			{
			}
		}

		// Token: 0x06003F03 RID: 16131 RVA: 0x0011F8E8 File Offset: 0x0011DAE8
		internal override void SetLocale(CultureInfo culture)
		{
			this.Culture = culture;
		}

		// Token: 0x17000FB3 RID: 4019
		// (set) Token: 0x06003F04 RID: 16132 RVA: 0x0011F8F1 File Offset: 0x0011DAF1
		internal override SpellerInteropBase.SpellerMode Mode
		{
			set
			{
				this._mode = value;
			}
		}

		// Token: 0x17000FB4 RID: 4020
		// (set) Token: 0x06003F05 RID: 16133 RVA: 0x00002137 File Offset: 0x00000337
		internal override bool MultiWordMode
		{
			set
			{
			}
		}

		// Token: 0x06003F06 RID: 16134 RVA: 0x00002137 File Offset: 0x00000337
		internal override void SetReformMode(CultureInfo culture, SpellingReform spellingReform)
		{
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x0011F8FA File Offset: 0x0011DAFA
		internal override bool CanSpellCheck(CultureInfo culture)
		{
			return !this._isDisposed && this.EnsureWordBreakerAndSpellCheckerForCulture(culture, false);
		}

		// Token: 0x06003F08 RID: 16136 RVA: 0x0011F910 File Offset: 0x0011DB10
		[SecuritySafeCritical]
		internal override void UnloadDictionary(object token)
		{
			if (this._isDisposed)
			{
				return;
			}
			Tuple<string, string> tuple = (Tuple<string, string>)token;
			string item = tuple.Item1;
			string item2 = tuple.Item2;
			try
			{
				new FileIOPermission(FileIOPermissionAccess.AllAccess, item2).Demand();
				using (new SpellerCOMActionTraceLogger(this, SpellerCOMActionTraceLogger.Actions.UnregisterUserDictionary))
				{
					SpellCheckerFactory.UnregisterUserDictionary(item2, item, true);
				}
				FileHelper.DeleteTemporaryFile(item2);
			}
			catch (SecurityException)
			{
			}
		}

		// Token: 0x06003F09 RID: 16137 RVA: 0x0011F98C File Offset: 0x0011DB8C
		internal override object LoadDictionary(string lexiconFilePath)
		{
			if (!this._isDisposed)
			{
				return this.LoadDictionaryImpl(lexiconFilePath);
			}
			return null;
		}

		// Token: 0x06003F0A RID: 16138 RVA: 0x0011F9A0 File Offset: 0x0011DBA0
		[SecuritySafeCritical]
		internal override object LoadDictionary(Uri item, string trustedFolder)
		{
			if (this._isDisposed)
			{
				return null;
			}
			new FileIOPermission(FileIOPermissionAccess.Read, trustedFolder).Assert();
			object result;
			try
			{
				result = this.LoadDictionaryImpl(item.LocalPath);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x06003F0B RID: 16139 RVA: 0x0011F9EC File Offset: 0x0011DBEC
		internal override void ReleaseAllLexicons()
		{
			if (!this._isDisposed)
			{
				this.ClearDictionaries(false);
			}
		}

		// Token: 0x06003F0C RID: 16140 RVA: 0x0011FA00 File Offset: 0x0011DC00
		private bool EnsureWordBreakerAndSpellCheckerForCulture(CultureInfo culture, bool throwOnError = false)
		{
			if (this._isDisposed || culture == null)
			{
				return false;
			}
			if (!this._spellCheckers.ContainsKey(culture))
			{
				WordsSegmenter wordsSegmenter = null;
				try
				{
					wordsSegmenter = WordsSegmenter.Create(culture.Name, true);
				}
				catch when (endfilter(!throwOnError > false))
				{
					wordsSegmenter = null;
				}
				if (wordsSegmenter == null)
				{
					this._spellCheckers[culture] = null;
					return false;
				}
				SpellChecker spellChecker = null;
				try
				{
					using (new SpellerCOMActionTraceLogger(this, SpellerCOMActionTraceLogger.Actions.SpellCheckerCreation))
					{
						spellChecker = new SpellChecker(culture.Name);
					}
				}
				catch (Exception ex)
				{
					spellChecker = null;
					if (throwOnError && ex is ArgumentException)
					{
						throw new NotSupportedException(string.Empty, ex);
					}
				}
				if (spellChecker == null)
				{
					this._spellCheckers[culture] = null;
				}
				else
				{
					this._spellCheckers[culture] = new Tuple<WordsSegmenter, SpellChecker>(wordsSegmenter, spellChecker);
				}
			}
			return this._spellCheckers[culture] != null;
		}

		// Token: 0x06003F0D RID: 16141 RVA: 0x0011FAFC File Offset: 0x0011DCFC
		internal override int EnumTextSegments(char[] text, int count, SpellerInteropBase.EnumSentencesCallback sentenceCallback, SpellerInteropBase.EnumTextSegmentsCallback segmentCallback, object data)
		{
			if (this._isDisposed)
			{
				return 0;
			}
			WordsSegmenter wordsSegmenter = this.CurrentWordBreaker ?? this.DefaultCultureWordBreaker;
			SpellChecker currentSpellChecker = this.CurrentSpellChecker;
			bool flag = this._mode.HasFlag(SpellerInteropBase.SpellerMode.SpellingErrors) || this._mode.HasFlag(SpellerInteropBase.SpellerMode.Suggestions);
			if (wordsSegmenter == null || (flag && currentSpellChecker == null))
			{
				return 0;
			}
			int num = 0;
			bool flag2 = true;
			foreach (string sentence in new string[]
			{
				string.Join<char>(string.Empty, text)
			})
			{
				WinRTSpellerInterop.SpellerSentence spellerSentence = new WinRTSpellerInterop.SpellerSentence(sentence, wordsSegmenter, this.CurrentSpellChecker, this);
				num += spellerSentence.Segments.Count;
				if (segmentCallback != null)
				{
					int num2 = 0;
					while (flag2 && num2 < spellerSentence.Segments.Count)
					{
						flag2 = segmentCallback(spellerSentence.Segments[num2], data);
						num2++;
					}
				}
				if (sentenceCallback != null)
				{
					flag2 = sentenceCallback(spellerSentence, data);
				}
				if (!flag2)
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x06003F0E RID: 16142 RVA: 0x0011FC10 File Offset: 0x0011DE10
		[SecuritySafeCritical]
		private Tuple<string, string> LoadDictionaryImpl(string lexiconFilePath)
		{
			if (this._isDisposed)
			{
				return new Tuple<string, string>(null, null);
			}
			try
			{
				new FileIOPermission(FileIOPermissionAccess.Read, lexiconFilePath).Demand();
			}
			catch (SecurityException innerException)
			{
				throw new ArgumentException(SR.Get("CustomDictionaryFailedToLoadDictionaryUri", new object[]
				{
					lexiconFilePath
				}), innerException);
			}
			if (!File.Exists(lexiconFilePath))
			{
				throw new ArgumentException(SR.Get("CustomDictionaryFailedToLoadDictionaryUri", new object[]
				{
					lexiconFilePath
				}));
			}
			bool flag = false;
			string text = null;
			Tuple<string, string> result;
			try
			{
				CultureInfo cultureInfo = null;
				using (FileStream fileStream = new FileStream(lexiconFilePath, FileMode.Open, FileAccess.Read))
				{
					using (StreamReader streamReader = new StreamReader(fileStream))
					{
						string line = streamReader.ReadLine();
						cultureInfo = WinRTSpellerInterop.TryParseLexiconCulture(line);
					}
				}
				string ietfLanguageTag = cultureInfo.IetfLanguageTag;
				using (FileStream fileStream2 = FileHelper.CreateAndOpenTemporaryFile(out text, FileAccess.Write, FileOptions.None, "dic", "WPF"))
				{
					WinRTSpellerInterop.CopyToUnicodeFile(lexiconFilePath, fileStream2);
					flag = true;
				}
				if (!this._customDictionaryFiles.ContainsKey(ietfLanguageTag))
				{
					this._customDictionaryFiles[ietfLanguageTag] = new List<string>();
				}
				this._customDictionaryFiles[ietfLanguageTag].Add(text);
				using (new SpellerCOMActionTraceLogger(this, SpellerCOMActionTraceLogger.Actions.RegisterUserDictionary))
				{
					SpellCheckerFactory.RegisterUserDictionary(text, ietfLanguageTag, true);
				}
				result = new Tuple<string, string>(ietfLanguageTag, text);
			}
			catch (Exception ex) when (ex is SecurityException || ex is ArgumentException || !flag)
			{
				if (text != null)
				{
					FileHelper.DeleteTemporaryFile(text);
				}
				throw new ArgumentException(SR.Get("CustomDictionaryFailedToLoadDictionaryUri", new object[]
				{
					lexiconFilePath
				}), ex);
			}
			return result;
		}

		// Token: 0x06003F0F RID: 16143 RVA: 0x0011FDF8 File Offset: 0x0011DFF8
		[SecuritySafeCritical]
		private void ClearDictionaries(bool disposing = false)
		{
			if (this._isDisposed)
			{
				return;
			}
			if (this._customDictionaryFiles != null)
			{
				foreach (KeyValuePair<string, List<string>> keyValuePair in this._customDictionaryFiles)
				{
					string key = keyValuePair.Key;
					foreach (string text in keyValuePair.Value)
					{
						try
						{
							new FileIOPermission(FileIOPermissionAccess.AllAccess, text).Demand();
							using (new SpellerCOMActionTraceLogger(this, SpellerCOMActionTraceLogger.Actions.UnregisterUserDictionary))
							{
								SpellCheckerFactory.UnregisterUserDictionary(text, key, true);
							}
							FileHelper.DeleteTemporaryFile(text);
						}
						catch
						{
						}
					}
				}
				this._customDictionaryFiles.Clear();
			}
			if (disposing)
			{
				this._customDictionaryFiles = null;
			}
		}

		// Token: 0x06003F10 RID: 16144 RVA: 0x0011FF08 File Offset: 0x0011E108
		private static CultureInfo TryParseLexiconCulture(string line)
		{
			RegexOptions options = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant;
			CultureInfo result = CultureInfo.InvariantCulture;
			if (line == null)
			{
				return result;
			}
			string[] array = Regex.Split(line.Trim(), "\\s*\\#LID\\s+(\\d+)\\s*", options);
			if (array.Length != 3)
			{
				return result;
			}
			string a = array[0];
			string s = array[1];
			string a2 = array[2];
			int culture;
			if (a != string.Empty || a2 != string.Empty || !int.TryParse(s, out culture))
			{
				return result;
			}
			try
			{
				result = new CultureInfo(culture);
			}
			catch (CultureNotFoundException)
			{
				result = CultureInfo.InvariantCulture;
			}
			return result;
		}

		// Token: 0x06003F11 RID: 16145 RVA: 0x0011FF9C File Offset: 0x0011E19C
		[SecurityCritical]
		private static void CopyToUnicodeFile(string sourcePath, FileStream targetStream)
		{
			new FileIOPermission(FileIOPermissionAccess.Read, sourcePath).Demand();
			using (FileStream fileStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
			{
				bool flag = fileStream.ReadByte() == 255 && fileStream.ReadByte() == 254;
				fileStream.Seek(0L, SeekOrigin.Begin);
				if (flag)
				{
					fileStream.CopyTo(targetStream);
				}
				else
				{
					using (StreamReader streamReader = new StreamReader(fileStream))
					{
						using (StreamWriter streamWriter = new StreamWriter(targetStream, Encoding.Unicode))
						{
							string value;
							while ((value = streamReader.ReadLine()) != null)
							{
								streamWriter.WriteLine(value);
							}
						}
					}
				}
			}
		}

		// Token: 0x06003F12 RID: 16146 RVA: 0x00120068 File Offset: 0x0011E268
		[SecuritySafeCritical]
		private void ProcessUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			bool flag = false;
			try
			{
				if (this.BeginInvokeOnUIThread(new Action<bool>(this.ClearDictionaries), DispatcherPriority.Normal, new object[]
				{
					flag
				}) == null)
				{
					this.ClearDictionaries(flag);
				}
			}
			catch (InvalidOperationException)
			{
			}
		}

		// Token: 0x06003F13 RID: 16147 RVA: 0x001200B8 File Offset: 0x0011E2B8
		private void ReleaseAllResources(bool disposing)
		{
			if (this._spellCheckers != null)
			{
				foreach (Tuple<WordsSegmenter, SpellChecker> tuple in this._spellCheckers.Values)
				{
					SpellChecker spellChecker = (tuple != null) ? tuple.Item2 : null;
					if (spellChecker != null)
					{
						spellChecker.Dispose();
					}
				}
				this._spellCheckers = null;
			}
			this.ClearDictionaries(disposing);
		}

		// Token: 0x06003F14 RID: 16148 RVA: 0x00120138 File Offset: 0x0011E338
		private DispatcherOperation BeginInvokeOnUIThread(Delegate method, DispatcherPriority priority, params object[] args)
		{
			Dispatcher dispatcher = null;
			if (this._dispatcher == null || !this._dispatcher.TryGetTarget(out dispatcher) || dispatcher == null)
			{
				throw new InvalidOperationException();
			}
			if (!dispatcher.CheckAccess())
			{
				return dispatcher.BeginInvoke(method, priority, args);
			}
			return null;
		}

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x06003F15 RID: 16149 RVA: 0x0012017A File Offset: 0x0011E37A
		// (set) Token: 0x06003F16 RID: 16150 RVA: 0x00120182 File Offset: 0x0011E382
		private CultureInfo Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				this._culture = value;
				this.EnsureWordBreakerAndSpellCheckerForCulture(this._culture, false);
			}
		}

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x06003F17 RID: 16151 RVA: 0x00120199 File Offset: 0x0011E399
		private WordsSegmenter CurrentWordBreaker
		{
			get
			{
				if (this.Culture == null)
				{
					return null;
				}
				this.EnsureWordBreakerAndSpellCheckerForCulture(this.Culture, false);
				Tuple<WordsSegmenter, SpellChecker> tuple = this._spellCheckers[this.Culture];
				if (tuple == null)
				{
					return null;
				}
				return tuple.Item1;
			}
		}

		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x06003F18 RID: 16152 RVA: 0x001201CF File Offset: 0x0011E3CF
		private WordsSegmenter DefaultCultureWordBreaker
		{
			get
			{
				if (this._defaultCulture == null)
				{
					return null;
				}
				Tuple<WordsSegmenter, SpellChecker> tuple = this._spellCheckers[this._defaultCulture];
				if (tuple == null)
				{
					return null;
				}
				return tuple.Item1;
			}
		}

		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x06003F19 RID: 16153 RVA: 0x001201F7 File Offset: 0x0011E3F7
		private SpellChecker CurrentSpellChecker
		{
			get
			{
				if (this.Culture == null)
				{
					return null;
				}
				this.EnsureWordBreakerAndSpellCheckerForCulture(this.Culture, false);
				Tuple<WordsSegmenter, SpellChecker> tuple = this._spellCheckers[this.Culture];
				if (tuple == null)
				{
					return null;
				}
				return tuple.Item2;
			}
		}

		// Token: 0x040026CE RID: 9934
		private bool _isDisposed;

		// Token: 0x040026CF RID: 9935
		private SpellerInteropBase.SpellerMode _mode;

		// Token: 0x040026D0 RID: 9936
		private Dictionary<CultureInfo, Tuple<WordsSegmenter, SpellChecker>> _spellCheckers;

		// Token: 0x040026D1 RID: 9937
		private CultureInfo _defaultCulture;

		// Token: 0x040026D2 RID: 9938
		private CultureInfo _culture;

		// Token: 0x040026D3 RID: 9939
		private Dictionary<string, List<string>> _customDictionaryFiles;

		// Token: 0x040026D4 RID: 9940
		private readonly WeakReference<Dispatcher> _dispatcher;

		// Token: 0x0200091C RID: 2332
		internal struct TextRange : SpellerInteropBase.ITextRange
		{
			// Token: 0x0600860E RID: 34318 RVA: 0x0024B92A File Offset: 0x00249B2A
			public TextRange(TextSegment textSegment)
			{
				this = new WinRTSpellerInterop.TextRange((int)textSegment.StartPosition, (int)textSegment.Length);
			}

			// Token: 0x0600860F RID: 34319 RVA: 0x0024B93E File Offset: 0x00249B3E
			public TextRange(int start, int length)
			{
				this._start = start;
				this._length = length;
			}

			// Token: 0x06008610 RID: 34320 RVA: 0x0024B94E File Offset: 0x00249B4E
			public TextRange(SpellerInteropBase.ITextRange textRange)
			{
				this = new WinRTSpellerInterop.TextRange(textRange.Start, textRange.Length);
			}

			// Token: 0x06008611 RID: 34321 RVA: 0x0024B962 File Offset: 0x00249B62
			public static explicit operator WinRTSpellerInterop.TextRange(TextSegment textSegment)
			{
				return new WinRTSpellerInterop.TextRange(textSegment);
			}

			// Token: 0x17001E4F RID: 7759
			// (get) Token: 0x06008612 RID: 34322 RVA: 0x0024B96A File Offset: 0x00249B6A
			public int Start
			{
				get
				{
					return this._start;
				}
			}

			// Token: 0x17001E50 RID: 7760
			// (get) Token: 0x06008613 RID: 34323 RVA: 0x0024B972 File Offset: 0x00249B72
			public int Length
			{
				get
				{
					return this._length;
				}
			}

			// Token: 0x0400434B RID: 17227
			private readonly int _start;

			// Token: 0x0400434C RID: 17228
			private readonly int _length;
		}

		// Token: 0x0200091D RID: 2333
		[DebuggerDisplay("SubSegments.Count = {SubSegments.Count} TextRange = {TextRange.Start},{TextRange.Length}")]
		internal class SpellerSegment : SpellerInteropBase.ISpellerSegment
		{
			// Token: 0x06008614 RID: 34324 RVA: 0x0024B97A File Offset: 0x00249B7A
			public SpellerSegment(string sourceString, SpellerInteropBase.ITextRange textRange, SpellChecker spellChecker, WinRTSpellerInterop owner)
			{
				this._spellChecker = spellChecker;
				this._suggestions = null;
				this.Owner = owner;
				this.SourceString = sourceString;
				this.TextRange = textRange;
			}

			// Token: 0x06008615 RID: 34325 RVA: 0x0024B9A6 File Offset: 0x00249BA6
			public SpellerSegment(string sourceString, WordSegment segment, SpellChecker spellChecker, WinRTSpellerInterop owner) : this(sourceString, new WinRTSpellerInterop.TextRange(segment.SourceTextSegment), spellChecker, owner)
			{
			}

			// Token: 0x06008617 RID: 34327 RVA: 0x0024B9D4 File Offset: 0x00249BD4
			private void EnumerateSuggestions()
			{
				List<string> list = new List<string>();
				this._isClean = new bool?(true);
				if (this._spellChecker == null)
				{
					this._suggestions = list.AsReadOnly();
					return;
				}
				List<SpellChecker.SpellingError> list2 = null;
				using (new SpellerCOMActionTraceLogger(this.Owner, SpellerCOMActionTraceLogger.Actions.ComprehensiveCheck))
				{
					list2 = ((this.Text != null) ? this._spellChecker.ComprehensiveCheck(this.Text, true) : null);
				}
				if (list2 == null)
				{
					this._suggestions = list.AsReadOnly();
					return;
				}
				foreach (SpellChecker.SpellingError spellingError in list2)
				{
					list.AddRange(spellingError.Suggestions);
					if (spellingError.CorrectiveAction != SpellChecker.CorrectiveAction.None)
					{
						this._isClean = new bool?(false);
					}
				}
				this._suggestions = list.AsReadOnly();
			}

			// Token: 0x17001E51 RID: 7761
			// (get) Token: 0x06008618 RID: 34328 RVA: 0x0024BAC8 File Offset: 0x00249CC8
			public string SourceString { get; }

			// Token: 0x17001E52 RID: 7762
			// (get) Token: 0x06008619 RID: 34329 RVA: 0x0024BAD0 File Offset: 0x00249CD0
			public string Text
			{
				get
				{
					string sourceString = this.SourceString;
					if (sourceString == null)
					{
						return null;
					}
					return sourceString.Substring(this.TextRange.Start, this.TextRange.Length);
				}
			}

			// Token: 0x17001E53 RID: 7763
			// (get) Token: 0x0600861A RID: 34330 RVA: 0x0024BAF9 File Offset: 0x00249CF9
			public IReadOnlyList<SpellerInteropBase.ISpellerSegment> SubSegments
			{
				get
				{
					return WinRTSpellerInterop.SpellerSegment._empty;
				}
			}

			// Token: 0x17001E54 RID: 7764
			// (get) Token: 0x0600861B RID: 34331 RVA: 0x0024BB00 File Offset: 0x00249D00
			public SpellerInteropBase.ITextRange TextRange { get; }

			// Token: 0x17001E55 RID: 7765
			// (get) Token: 0x0600861C RID: 34332 RVA: 0x0024BB08 File Offset: 0x00249D08
			public IReadOnlyList<string> Suggestions
			{
				get
				{
					if (this._suggestions == null)
					{
						this.EnumerateSuggestions();
					}
					return this._suggestions;
				}
			}

			// Token: 0x17001E56 RID: 7766
			// (get) Token: 0x0600861D RID: 34333 RVA: 0x0024BB1E File Offset: 0x00249D1E
			internal WinRTSpellerInterop Owner { get; }

			// Token: 0x17001E57 RID: 7767
			// (get) Token: 0x0600861E RID: 34334 RVA: 0x0024BB26 File Offset: 0x00249D26
			public bool IsClean
			{
				get
				{
					if (this._isClean == null)
					{
						this.EnumerateSuggestions();
					}
					return this._isClean.Value;
				}
			}

			// Token: 0x0600861F RID: 34335 RVA: 0x0024BB48 File Offset: 0x00249D48
			public void EnumSubSegments(SpellerInteropBase.EnumTextSegmentsCallback segmentCallback, object data)
			{
				bool flag = true;
				int num = 0;
				while (flag && num < this.SubSegments.Count)
				{
					flag = segmentCallback(this.SubSegments[num], data);
					num++;
				}
			}

			// Token: 0x04004350 RID: 17232
			private SpellChecker _spellChecker;

			// Token: 0x04004351 RID: 17233
			private IReadOnlyList<string> _suggestions;

			// Token: 0x04004352 RID: 17234
			private bool? _isClean;

			// Token: 0x04004353 RID: 17235
			private static readonly IReadOnlyList<SpellerInteropBase.ISpellerSegment> _empty = new List<SpellerInteropBase.ISpellerSegment>().AsReadOnly();
		}

		// Token: 0x0200091E RID: 2334
		[DebuggerDisplay("Sentence = {_sentence}")]
		private class SpellerSentence : SpellerInteropBase.ISpellerSentence
		{
			// Token: 0x06008620 RID: 34336 RVA: 0x0024BB84 File Offset: 0x00249D84
			public SpellerSentence(string sentence, WordsSegmenter wordBreaker, SpellChecker spellChecker, WinRTSpellerInterop owner)
			{
				this._sentence = sentence;
				this._wordBreaker = wordBreaker;
				this._spellChecker = spellChecker;
				this._segments = null;
				this._owner = owner;
			}

			// Token: 0x17001E58 RID: 7768
			// (get) Token: 0x06008621 RID: 34337 RVA: 0x0024BBB0 File Offset: 0x00249DB0
			public IReadOnlyList<SpellerInteropBase.ISpellerSegment> Segments
			{
				get
				{
					if (this._segments == null)
					{
						if (!FrameworkAppContextSwitches.DoNotAugmentWordBreakingUsingSpeller)
						{
							this._segments = this._wordBreaker.ComprehensiveGetTokens(this._sentence, this._spellChecker, this._owner);
						}
						else
						{
							List<WinRTSpellerInterop.SpellerSegment> list = new List<WinRTSpellerInterop.SpellerSegment>();
							foreach (WordSegment segment in this._wordBreaker.GetTokens(this._sentence))
							{
								list.Add(new WinRTSpellerInterop.SpellerSegment(this._sentence, segment, this._spellChecker, this._owner));
							}
							this._segments = list.AsReadOnly();
						}
					}
					return this._segments;
				}
			}

			// Token: 0x17001E59 RID: 7769
			// (get) Token: 0x06008622 RID: 34338 RVA: 0x0024BC70 File Offset: 0x00249E70
			public int EndOffset
			{
				get
				{
					int result = -1;
					if (this.Segments.Count > 0)
					{
						SpellerInteropBase.ITextRange textRange = this.Segments[this.Segments.Count - 1].TextRange;
						result = textRange.Start + textRange.Length;
					}
					return result;
				}
			}

			// Token: 0x04004354 RID: 17236
			private string _sentence;

			// Token: 0x04004355 RID: 17237
			private WordsSegmenter _wordBreaker;

			// Token: 0x04004356 RID: 17238
			private SpellChecker _spellChecker;

			// Token: 0x04004357 RID: 17239
			private IReadOnlyList<WinRTSpellerInterop.SpellerSegment> _segments;

			// Token: 0x04004358 RID: 17240
			private WinRTSpellerInterop _owner;
		}
	}
}
