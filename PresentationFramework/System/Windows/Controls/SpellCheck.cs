using System;
using System.Collections;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Provides real-time spell-checking functionality to text-editing controls, such as <see cref="T:System.Windows.Controls.TextBox" /> and <see cref="T:System.Windows.Controls.RichTextBox" />.</summary>
	// Token: 0x02000534 RID: 1332
	public sealed class SpellCheck
	{
		// Token: 0x0600565A RID: 22106 RVA: 0x0017E80E File Offset: 0x0017CA0E
		internal SpellCheck(TextBoxBase owner)
		{
			this._owner = owner;
		}

		/// <summary>Gets or sets a value that determines whether the spelling checker is enabled on this text-editing control, such as <see cref="T:System.Windows.Controls.TextBox" /> or <see cref="T:System.Windows.Controls.RichTextBox" />. </summary>
		/// <returns>
		///     <see langword="true" /> if the spelling checker is enabled on the control; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x170014FD RID: 5373
		// (get) Token: 0x0600565B RID: 22107 RVA: 0x0017E81D File Offset: 0x0017CA1D
		// (set) Token: 0x0600565C RID: 22108 RVA: 0x0017E834 File Offset: 0x0017CA34
		public bool IsEnabled
		{
			get
			{
				return (bool)this._owner.GetValue(SpellCheck.IsEnabledProperty);
			}
			set
			{
				this._owner.SetValue(SpellCheck.IsEnabledProperty, value);
			}
		}

		/// <summary>Enables or disables the spelling checker on the specified text-editing control, such as <see cref="T:System.Windows.Controls.TextBox" /> or <see cref="T:System.Windows.Controls.RichTextBox" />.</summary>
		/// <param name="textBoxBase">The text-editing control on which to enable or disable the spelling checker. Example controls include <see cref="T:System.Windows.Controls.TextBox" /> and <see cref="T:System.Windows.Controls.RichTextBox" />.</param>
		/// <param name="value">A Boolean value that specifies whether the spelling checker is enabled on the text-editing control.</param>
		// Token: 0x0600565D RID: 22109 RVA: 0x0017E847 File Offset: 0x0017CA47
		public static void SetIsEnabled(TextBoxBase textBoxBase, bool value)
		{
			if (textBoxBase == null)
			{
				throw new ArgumentNullException("textBoxBase");
			}
			textBoxBase.SetValue(SpellCheck.IsEnabledProperty, value);
		}

		/// <summary>Returns a value that indicates whether the spelling checker is enabled on the specified text-editing control.</summary>
		/// <param name="textBoxBase">The text-editing control to check. Example controls include <see cref="T:System.Windows.Controls.TextBox" /> and <see cref="T:System.Windows.Controls.RichTextBox" />.</param>
		/// <returns>
		///     <see langword="true" /> if the spelling checker is enabled on the text-editing control; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="textBoxBase " />is <see langword="null" />.</exception>
		// Token: 0x0600565E RID: 22110 RVA: 0x0017E863 File Offset: 0x0017CA63
		public static bool GetIsEnabled(TextBoxBase textBoxBase)
		{
			if (textBoxBase == null)
			{
				throw new ArgumentNullException("textBoxBase");
			}
			return (bool)textBoxBase.GetValue(SpellCheck.IsEnabledProperty);
		}

		/// <summary>Gets or sets the spelling reform rules that are used by the spelling checker. </summary>
		/// <returns>The spelling reform rules that are used by the spelling checker. The default value is <see cref="F:System.Windows.Controls.SpellingReform.PreAndPostreform" /> for French and <see cref="F:System.Windows.Controls.SpellingReform.Postreform" /> for German.</returns>
		// Token: 0x170014FE RID: 5374
		// (get) Token: 0x0600565F RID: 22111 RVA: 0x0017E883 File Offset: 0x0017CA83
		// (set) Token: 0x06005660 RID: 22112 RVA: 0x0017E89A File Offset: 0x0017CA9A
		public SpellingReform SpellingReform
		{
			get
			{
				return (SpellingReform)this._owner.GetValue(SpellCheck.SpellingReformProperty);
			}
			set
			{
				this._owner.SetValue(SpellCheck.SpellingReformProperty, value);
			}
		}

		/// <summary>Determines the spelling reform rules that are used by the spelling checker. </summary>
		/// <param name="textBoxBase">The text-editing control to which the spelling checker is applied. Example controls include <see cref="T:System.Windows.Controls.TextBox" /> and <see cref="T:System.Windows.Controls.RichTextBox" />.</param>
		/// <param name="value">The <see cref="P:System.Windows.Controls.SpellCheck.SpellingReform" /> value that determines the spelling reform rules.</param>
		// Token: 0x06005661 RID: 22113 RVA: 0x0017E8B2 File Offset: 0x0017CAB2
		public static void SetSpellingReform(TextBoxBase textBoxBase, SpellingReform value)
		{
			if (textBoxBase == null)
			{
				throw new ArgumentNullException("textBoxBase");
			}
			textBoxBase.SetValue(SpellCheck.SpellingReformProperty, value);
		}

		/// <summary>Gets the collection of lexicon file locations that are used for custom spell checking.</summary>
		/// <returns>The collection of lexicon file locations.</returns>
		// Token: 0x170014FF RID: 5375
		// (get) Token: 0x06005662 RID: 22114 RVA: 0x0017E8D3 File Offset: 0x0017CAD3
		public IList CustomDictionaries
		{
			get
			{
				return (IList)this._owner.GetValue(SpellCheck.CustomDictionariesProperty);
			}
		}

		/// <summary>Gets the collection of lexicon file locations that are used for custom spelling checkers on a specified text-editing control. </summary>
		/// <param name="textBoxBase">The text-editing control whose collection of lexicon files is retrieved.</param>
		/// <returns>The collection of lexicon file locations.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="textBoxBase " />is <see langword="null" />.</exception>
		// Token: 0x06005663 RID: 22115 RVA: 0x0017E8EA File Offset: 0x0017CAEA
		public static IList GetCustomDictionaries(TextBoxBase textBoxBase)
		{
			if (textBoxBase == null)
			{
				throw new ArgumentNullException("textBoxBase");
			}
			return (IList)textBoxBase.GetValue(SpellCheck.CustomDictionariesProperty);
		}

		// Token: 0x06005664 RID: 22116 RVA: 0x0017E90C File Offset: 0x0017CB0C
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase textBoxBase = d as TextBoxBase;
			if (textBoxBase != null)
			{
				TextEditor textEditor = TextEditor._GetTextEditor(textBoxBase);
				if (textEditor != null)
				{
					textEditor.SetSpellCheckEnabled((bool)e.NewValue);
					if ((bool)e.NewValue != (bool)e.OldValue)
					{
						textEditor.SetCustomDictionaries((bool)e.NewValue);
					}
				}
			}
		}

		// Token: 0x06005665 RID: 22117 RVA: 0x0017E96C File Offset: 0x0017CB6C
		private static void OnSpellingReformChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase textBoxBase = d as TextBoxBase;
			if (textBoxBase != null)
			{
				TextEditor textEditor = TextEditor._GetTextEditor(textBoxBase);
				if (textEditor != null)
				{
					textEditor.SetSpellingReform((SpellingReform)e.NewValue);
				}
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.SpellCheck.IsEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.SpellCheck.IsEnabled" /> dependency property.</returns>
		// Token: 0x04002E42 RID: 11842
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(SpellCheck), new FrameworkPropertyMetadata(new PropertyChangedCallback(SpellCheck.OnIsEnabledChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.SpellCheck.SpellingReform" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.SpellCheck.SpellingReform" /> dependency property.</returns>
		// Token: 0x04002E43 RID: 11843
		public static readonly DependencyProperty SpellingReformProperty = DependencyProperty.RegisterAttached("SpellingReform", typeof(SpellingReform), typeof(SpellCheck), new FrameworkPropertyMetadata((Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName == "de") ? SpellingReform.Postreform : SpellingReform.PreAndPostreform, new PropertyChangedCallback(SpellCheck.OnSpellingReformChanged)));

		// Token: 0x04002E44 RID: 11844
		private static readonly DependencyPropertyKey CustomDictionariesPropertyKey = DependencyProperty.RegisterAttachedReadOnly("CustomDictionaries", typeof(IList), typeof(SpellCheck), new FrameworkPropertyMetadata(new SpellCheck.DictionaryCollectionFactory()));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.SpellCheck.CustomDictionaries" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.SpellCheck.CustomDictionaries" /> dependency property.</returns>
		// Token: 0x04002E45 RID: 11845
		public static readonly DependencyProperty CustomDictionariesProperty = SpellCheck.CustomDictionariesPropertyKey.DependencyProperty;

		// Token: 0x04002E46 RID: 11846
		private readonly TextBoxBase _owner;

		// Token: 0x020009BB RID: 2491
		internal class DictionaryCollectionFactory : DefaultValueFactory
		{
			// Token: 0x06008872 RID: 34930 RVA: 0x001F57F2 File Offset: 0x001F39F2
			internal DictionaryCollectionFactory()
			{
			}

			// Token: 0x17001ECC RID: 7884
			// (get) Token: 0x06008873 RID: 34931 RVA: 0x0000C238 File Offset: 0x0000A438
			internal override object DefaultValue
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06008874 RID: 34932 RVA: 0x00252235 File Offset: 0x00250435
			internal override object CreateDefaultValue(DependencyObject owner, DependencyProperty property)
			{
				return new CustomDictionarySources(owner as TextBoxBase);
			}
		}
	}
}
