using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Data;
using MS.Win32;

namespace System.Windows.Controls
{
	/// <summary>Enables a user to quickly access items in a set by typing prefixes of strings. </summary>
	// Token: 0x02000542 RID: 1346
	public sealed class TextSearch : DependencyObject
	{
		// Token: 0x060057ED RID: 22509 RVA: 0x00185A4C File Offset: 0x00183C4C
		private TextSearch(ItemsControl itemsControl)
		{
			if (itemsControl == null)
			{
				throw new ArgumentNullException("itemsControl");
			}
			this._attachedTo = itemsControl;
			this.ResetState();
		}

		// Token: 0x060057EE RID: 22510 RVA: 0x00185A70 File Offset: 0x00183C70
		internal static TextSearch EnsureInstance(ItemsControl itemsControl)
		{
			TextSearch textSearch = (TextSearch)itemsControl.GetValue(TextSearch.TextSearchInstanceProperty);
			if (textSearch == null)
			{
				textSearch = new TextSearch(itemsControl);
				itemsControl.SetValue(TextSearch.TextSearchInstancePropertyKey, textSearch);
			}
			return textSearch;
		}

		/// <summary> Writes the <see cref="P:System.Windows.Controls.TextSearch.TextPath" /> attached property to the specified element. </summary>
		/// <param name="element">The element to which the property value is written.</param>
		/// <param name="path">The name of the property that identifies an item.</param>
		// Token: 0x060057EF RID: 22511 RVA: 0x00185AA5 File Offset: 0x00183CA5
		public static void SetTextPath(DependencyObject element, string path)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextSearch.TextPathProperty, path);
		}

		/// <summary>Returns the name of the property that identifies an item in the specified element's collection. </summary>
		/// <param name="element">The element from which the property value is read.</param>
		/// <returns>The name of the property that identifies the item to the user.</returns>
		// Token: 0x060057F0 RID: 22512 RVA: 0x00185AC1 File Offset: 0x00183CC1
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static string GetTextPath(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (string)element.GetValue(TextSearch.TextPathProperty);
		}

		/// <summary> Writes the <see cref="P:System.Windows.Controls.TextSearch.Text" /> attached property value to the specified element. </summary>
		/// <param name="element">The element to which the property value is written.</param>
		/// <param name="text">The string that identifies the item.</param>
		// Token: 0x060057F1 RID: 22513 RVA: 0x00185AE1 File Offset: 0x00183CE1
		public static void SetText(DependencyObject element, string text)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextSearch.TextProperty, text);
		}

		/// <summary>Returns the string to that identifies the specified item.</summary>
		/// <param name="element">The element from which the property value is read.</param>
		/// <returns>The string that identifies the specified item.</returns>
		// Token: 0x060057F2 RID: 22514 RVA: 0x00185AFD File Offset: 0x00183CFD
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static string GetText(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (string)element.GetValue(TextSearch.TextProperty);
		}

		// Token: 0x060057F3 RID: 22515 RVA: 0x00185B20 File Offset: 0x00183D20
		internal bool DoSearch(string nextChar)
		{
			bool lookForFallbackMatchToo = false;
			int num = 0;
			ItemCollection items = this._attachedTo.Items;
			if (this.IsActive)
			{
				num = this.MatchedItemIndex;
			}
			if (this._charsEntered.Count > 0 && string.Compare(this._charsEntered[this._charsEntered.Count - 1], nextChar, true, TextSearch.GetCulture(this._attachedTo)) == 0)
			{
				lookForFallbackMatchToo = true;
			}
			string primaryTextPath = TextSearch.GetPrimaryTextPath(this._attachedTo);
			bool flag = false;
			int num2 = TextSearch.FindMatchingPrefix(this._attachedTo, primaryTextPath, this.Prefix, nextChar, num, lookForFallbackMatchToo, ref flag);
			if (num2 != -1)
			{
				if (!this.IsActive || num2 != num)
				{
					object item = items[num2];
					this._attachedTo.NavigateToItem(item, num2, new ItemsControl.ItemNavigateArgs(Keyboard.PrimaryDevice, ModifierKeys.None));
					this.MatchedItemIndex = num2;
				}
				if (flag)
				{
					this.AddCharToPrefix(nextChar);
				}
				if (!this.IsActive)
				{
					this.IsActive = true;
				}
			}
			if (this.IsActive)
			{
				this.ResetTimeout();
			}
			return num2 != -1;
		}

		// Token: 0x060057F4 RID: 22516 RVA: 0x00185C20 File Offset: 0x00183E20
		internal bool DeleteLastCharacter()
		{
			if (this.IsActive && this._charsEntered.Count > 0)
			{
				string text = this._charsEntered[this._charsEntered.Count - 1];
				string prefix = this.Prefix;
				this._charsEntered.RemoveAt(this._charsEntered.Count - 1);
				this.Prefix = prefix.Substring(0, prefix.Length - text.Length);
				this.ResetTimeout();
				return true;
			}
			return false;
		}

		// Token: 0x060057F5 RID: 22517 RVA: 0x00185CA0 File Offset: 0x00183EA0
		private static void GetMatchingPrefixAndRemainingTextLength(string matchedText, string newText, CultureInfo cultureInfo, bool ignoreCase, out int matchedPrefixLength, out int textExcludingPrefixLength)
		{
			matchedPrefixLength = 0;
			textExcludingPrefixLength = 0;
			if (matchedText.Length < newText.Length)
			{
				matchedPrefixLength = matchedText.Length;
				textExcludingPrefixLength = 0;
				return;
			}
			int num = newText.Length;
			int num2 = num + 1;
			for (;;)
			{
				if (num >= 1)
				{
					string strB = matchedText.Substring(0, num);
					if (string.Compare(newText, strB, ignoreCase, cultureInfo) == 0)
					{
						break;
					}
				}
				if (num2 <= matchedText.Length)
				{
					string strB = matchedText.Substring(0, num2);
					if (string.Compare(newText, strB, ignoreCase, cultureInfo) == 0)
					{
						goto Block_5;
					}
				}
				num--;
				num2++;
				if (num < 1 && num2 > matchedText.Length)
				{
					return;
				}
			}
			matchedPrefixLength = num;
			textExcludingPrefixLength = matchedText.Length - num;
			return;
			Block_5:
			matchedPrefixLength = num2;
			textExcludingPrefixLength = matchedText.Length - num2;
		}

		// Token: 0x060057F6 RID: 22518 RVA: 0x00185D48 File Offset: 0x00183F48
		private static int FindMatchingPrefix(ItemsControl itemsControl, string primaryTextPath, string prefix, string newChar, int startItemIndex, bool lookForFallbackMatchToo, ref bool wasNewCharUsed)
		{
			ItemCollection items = itemsControl.Items;
			int num = -1;
			int num2 = -1;
			int count = items.Count;
			if (count == 0)
			{
				return -1;
			}
			string value = prefix + newChar;
			if (string.IsNullOrEmpty(value))
			{
				return -1;
			}
			BindingExpression bindingExpression = null;
			object item = itemsControl.Items[0];
			if (SystemXmlHelper.IsXmlNode(item) || !string.IsNullOrEmpty(primaryTextPath))
			{
				bindingExpression = TextSearch.CreateBindingExpression(itemsControl, item, primaryTextPath);
				TextSearch.TextValueBindingExpression.SetValue(itemsControl, bindingExpression);
			}
			bool flag = true;
			wasNewCharUsed = false;
			CultureInfo culture = TextSearch.GetCulture(itemsControl);
			int i = startItemIndex;
			while (i < count)
			{
				object obj = items[i];
				if (obj != null)
				{
					string primaryText = TextSearch.GetPrimaryText(obj, bindingExpression, itemsControl);
					bool isTextSearchCaseSensitive = itemsControl.IsTextSearchCaseSensitive;
					if (primaryText != null && primaryText.StartsWith(value, !isTextSearchCaseSensitive, culture))
					{
						wasNewCharUsed = true;
						num = i;
						break;
					}
					if (lookForFallbackMatchToo)
					{
						if (!flag && prefix != string.Empty)
						{
							if (primaryText != null && num2 == -1 && primaryText.StartsWith(prefix, !isTextSearchCaseSensitive, culture))
							{
								num2 = i;
							}
						}
						else
						{
							flag = false;
						}
					}
				}
				i++;
				if (i >= count)
				{
					i = 0;
				}
				if (i == startItemIndex)
				{
					break;
				}
			}
			if (bindingExpression != null)
			{
				TextSearch.TextValueBindingExpression.ClearValue(itemsControl);
			}
			if (num == -1 && num2 != -1)
			{
				num = num2;
			}
			return num;
		}

		// Token: 0x060057F7 RID: 22519 RVA: 0x00185E84 File Offset: 0x00184084
		internal static MatchedTextInfo FindMatchingPrefix(ItemsControl itemsControl, string prefix)
		{
			bool flag = false;
			int num = TextSearch.FindMatchingPrefix(itemsControl, TextSearch.GetPrimaryTextPath(itemsControl), prefix, string.Empty, 0, false, ref flag);
			MatchedTextInfo result;
			if (num >= 0)
			{
				CultureInfo culture = TextSearch.GetCulture(itemsControl);
				bool isTextSearchCaseSensitive = itemsControl.IsTextSearchCaseSensitive;
				string primaryTextFromItem = TextSearch.GetPrimaryTextFromItem(itemsControl, itemsControl.Items[num]);
				int matchedPrefixLength;
				int textExcludingPrefixLength;
				TextSearch.GetMatchingPrefixAndRemainingTextLength(primaryTextFromItem, prefix, culture, !isTextSearchCaseSensitive, out matchedPrefixLength, out textExcludingPrefixLength);
				result = new MatchedTextInfo(num, primaryTextFromItem, matchedPrefixLength, textExcludingPrefixLength);
			}
			else
			{
				result = MatchedTextInfo.NoMatch;
			}
			return result;
		}

		// Token: 0x060057F8 RID: 22520 RVA: 0x00185EFC File Offset: 0x001840FC
		private void ResetTimeout()
		{
			if (this._timeoutTimer == null)
			{
				this._timeoutTimer = new DispatcherTimer(DispatcherPriority.Normal);
				this._timeoutTimer.Tick += this.OnTimeout;
			}
			else
			{
				this._timeoutTimer.Stop();
			}
			this._timeoutTimer.Interval = this.TimeOut;
			this._timeoutTimer.Start();
		}

		// Token: 0x060057F9 RID: 22521 RVA: 0x00185F5E File Offset: 0x0018415E
		private void AddCharToPrefix(string newChar)
		{
			this.Prefix += newChar;
			this._charsEntered.Add(newChar);
		}

		// Token: 0x060057FA RID: 22522 RVA: 0x00185F80 File Offset: 0x00184180
		private static string GetPrimaryTextPath(ItemsControl itemsControl)
		{
			string text = (string)itemsControl.GetValue(TextSearch.TextPathProperty);
			if (string.IsNullOrEmpty(text))
			{
				text = itemsControl.DisplayMemberPath;
			}
			return text;
		}

		// Token: 0x060057FB RID: 22523 RVA: 0x00185FB0 File Offset: 0x001841B0
		private static string GetPrimaryText(object item, BindingExpression primaryTextBinding, DependencyObject primaryTextBindingHome)
		{
			DependencyObject dependencyObject = item as DependencyObject;
			if (dependencyObject != null)
			{
				string text = (string)dependencyObject.GetValue(TextSearch.TextProperty);
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
			}
			if (primaryTextBinding != null && primaryTextBindingHome != null)
			{
				primaryTextBinding.Activate(item);
				object value = primaryTextBinding.Value;
				return TextSearch.ConvertToPlainText(value);
			}
			return TextSearch.ConvertToPlainText(item);
		}

		// Token: 0x060057FC RID: 22524 RVA: 0x00186004 File Offset: 0x00184204
		private static string ConvertToPlainText(object o)
		{
			FrameworkElement frameworkElement = o as FrameworkElement;
			if (frameworkElement != null)
			{
				string plainText = frameworkElement.GetPlainText();
				if (plainText != null)
				{
					return plainText;
				}
			}
			if (o == null)
			{
				return string.Empty;
			}
			return o.ToString();
		}

		// Token: 0x060057FD RID: 22525 RVA: 0x00186038 File Offset: 0x00184238
		internal static string GetPrimaryTextFromItem(ItemsControl itemsControl, object item)
		{
			if (item == null)
			{
				return string.Empty;
			}
			BindingExpression bindingExpression = TextSearch.CreateBindingExpression(itemsControl, item, TextSearch.GetPrimaryTextPath(itemsControl));
			TextSearch.TextValueBindingExpression.SetValue(itemsControl, bindingExpression);
			string primaryText = TextSearch.GetPrimaryText(item, bindingExpression, itemsControl);
			TextSearch.TextValueBindingExpression.ClearValue(itemsControl);
			return primaryText;
		}

		// Token: 0x060057FE RID: 22526 RVA: 0x00186080 File Offset: 0x00184280
		private static BindingExpression CreateBindingExpression(ItemsControl itemsControl, object item, string primaryTextPath)
		{
			Binding binding = new Binding();
			if (SystemXmlHelper.IsXmlNode(item))
			{
				binding.XPath = primaryTextPath;
				binding.Path = new PropertyPath("/InnerText", new object[0]);
			}
			else
			{
				binding.Path = new PropertyPath(primaryTextPath, new object[0]);
			}
			binding.Mode = BindingMode.OneWay;
			binding.Source = null;
			return (BindingExpression)BindingExpressionBase.CreateUntargetedBindingExpression(itemsControl, binding);
		}

		// Token: 0x060057FF RID: 22527 RVA: 0x001860E6 File Offset: 0x001842E6
		private void OnTimeout(object sender, EventArgs e)
		{
			this.ResetState();
		}

		// Token: 0x06005800 RID: 22528 RVA: 0x001860F0 File Offset: 0x001842F0
		private void ResetState()
		{
			this.IsActive = false;
			this.Prefix = string.Empty;
			this.MatchedItemIndex = -1;
			if (this._charsEntered == null)
			{
				this._charsEntered = new List<string>(10);
			}
			else
			{
				this._charsEntered.Clear();
			}
			if (this._timeoutTimer != null)
			{
				this._timeoutTimer.Stop();
			}
			this._timeoutTimer = null;
		}

		// Token: 0x1700156D RID: 5485
		// (get) Token: 0x06005801 RID: 22529 RVA: 0x00186152 File Offset: 0x00184352
		private TimeSpan TimeOut
		{
			get
			{
				return TimeSpan.FromMilliseconds((double)(SafeNativeMethods.GetDoubleClickTime() * 2));
			}
		}

		// Token: 0x06005802 RID: 22530 RVA: 0x00186161 File Offset: 0x00184361
		private static TextSearch GetInstance(DependencyObject d)
		{
			return TextSearch.EnsureInstance(d as ItemsControl);
		}

		// Token: 0x06005803 RID: 22531 RVA: 0x0018616E File Offset: 0x0018436E
		private void TypeAKey(string c)
		{
			this.DoSearch(c);
		}

		// Token: 0x06005804 RID: 22532 RVA: 0x00186178 File Offset: 0x00184378
		private void CauseTimeOut()
		{
			if (this._timeoutTimer != null)
			{
				this._timeoutTimer.Stop();
				this.OnTimeout(this._timeoutTimer, EventArgs.Empty);
			}
		}

		// Token: 0x06005805 RID: 22533 RVA: 0x0018619E File Offset: 0x0018439E
		internal string GetCurrentPrefix()
		{
			return this.Prefix;
		}

		// Token: 0x06005806 RID: 22534 RVA: 0x001861A8 File Offset: 0x001843A8
		internal static string GetPrimaryText(FrameworkElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			string text = (string)element.GetValue(TextSearch.TextProperty);
			if (text != null && text != string.Empty)
			{
				return text;
			}
			return element.GetPlainText();
		}

		// Token: 0x1700156E RID: 5486
		// (get) Token: 0x06005807 RID: 22535 RVA: 0x001861EC File Offset: 0x001843EC
		// (set) Token: 0x06005808 RID: 22536 RVA: 0x001861F4 File Offset: 0x001843F4
		private string Prefix
		{
			get
			{
				return this._prefix;
			}
			set
			{
				this._prefix = value;
			}
		}

		// Token: 0x1700156F RID: 5487
		// (get) Token: 0x06005809 RID: 22537 RVA: 0x001861FD File Offset: 0x001843FD
		// (set) Token: 0x0600580A RID: 22538 RVA: 0x00186205 File Offset: 0x00184405
		private bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				this._isActive = value;
			}
		}

		// Token: 0x17001570 RID: 5488
		// (get) Token: 0x0600580B RID: 22539 RVA: 0x0018620E File Offset: 0x0018440E
		// (set) Token: 0x0600580C RID: 22540 RVA: 0x00186216 File Offset: 0x00184416
		private int MatchedItemIndex
		{
			get
			{
				return this._matchedItemIndex;
			}
			set
			{
				this._matchedItemIndex = value;
			}
		}

		// Token: 0x0600580D RID: 22541 RVA: 0x00186220 File Offset: 0x00184420
		private static CultureInfo GetCulture(DependencyObject element)
		{
			object value = element.GetValue(FrameworkElement.LanguageProperty);
			CultureInfo result = null;
			if (value != null)
			{
				XmlLanguage xmlLanguage = (XmlLanguage)value;
				try
				{
					result = xmlLanguage.GetSpecificCulture();
				}
				catch (InvalidOperationException)
				{
				}
			}
			return result;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextSearch.TextPath" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextSearch.TextPath" /> attached property.</returns>
		// Token: 0x04002E9A RID: 11930
		public static readonly DependencyProperty TextPathProperty = DependencyProperty.RegisterAttached("TextPath", typeof(string), typeof(TextSearch), new FrameworkPropertyMetadata(string.Empty));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextSearch.Text" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextSearch.Text" /> attached property.</returns>
		// Token: 0x04002E9B RID: 11931
		public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached("Text", typeof(string), typeof(TextSearch), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		// Token: 0x04002E9C RID: 11932
		private static readonly DependencyProperty CurrentPrefixProperty = DependencyProperty.RegisterAttached("CurrentPrefix", typeof(string), typeof(TextSearch), new FrameworkPropertyMetadata(null));

		// Token: 0x04002E9D RID: 11933
		private static readonly DependencyProperty IsActiveProperty = DependencyProperty.RegisterAttached("IsActive", typeof(bool), typeof(TextSearch), new FrameworkPropertyMetadata(false));

		// Token: 0x04002E9E RID: 11934
		private static readonly DependencyPropertyKey TextSearchInstancePropertyKey = DependencyProperty.RegisterAttachedReadOnly("TextSearchInstance", typeof(TextSearch), typeof(TextSearch), new FrameworkPropertyMetadata(null));

		// Token: 0x04002E9F RID: 11935
		private static readonly DependencyProperty TextSearchInstanceProperty = TextSearch.TextSearchInstancePropertyKey.DependencyProperty;

		// Token: 0x04002EA0 RID: 11936
		private static readonly BindingExpressionUncommonField TextValueBindingExpression = new BindingExpressionUncommonField();

		// Token: 0x04002EA1 RID: 11937
		private ItemsControl _attachedTo;

		// Token: 0x04002EA2 RID: 11938
		private string _prefix;

		// Token: 0x04002EA3 RID: 11939
		private List<string> _charsEntered;

		// Token: 0x04002EA4 RID: 11940
		private bool _isActive;

		// Token: 0x04002EA5 RID: 11941
		private int _matchedItemIndex;

		// Token: 0x04002EA6 RID: 11942
		private DispatcherTimer _timeoutTimer;
	}
}
