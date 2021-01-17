using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml;
using MS.Internal;
using MS.Internal.PresentationFramework.Markup;

namespace System.Windows.Documents
{
	/// <summary>Represents a selection of content between two <see cref="T:System.Windows.Documents.TextPointer" /> positions.</summary>
	// Token: 0x0200040B RID: 1035
	public class TextRange : ITextRange
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.TextRange" /> class, taking two specified <see cref="T:System.Windows.Documents.TextPointer" /> positions as the beginning and end positions for the new range.</summary>
		/// <param name="position1">A fixed anchor position that marks one end of the selection used to form the new <see cref="T:System.Windows.Documents.TextRange" />.</param>
		/// <param name="position2">A movable position that marks the other end of the selection used to form the new <see cref="T:System.Windows.Documents.TextRange" />.</param>
		/// <exception cref="T:System.ArgumentException">Occurs when <paramref name="position1" /> and <paramref name="position2" /> are not positioned within the same document.</exception>
		/// <exception cref="T:System.ArgumentNullException">Occurs when <paramref name="position1" /> or <paramref name="position2" /> is <see langword="null" />.</exception>
		// Token: 0x06003AA2 RID: 15010 RVA: 0x00109ECC File Offset: 0x001080CC
		public TextRange(TextPointer position1, TextPointer position2) : this(position1, position2)
		{
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x00109ED6 File Offset: 0x001080D6
		internal TextRange(ITextPointer position1, ITextPointer position2) : this(position1, position2, false)
		{
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x00109EE4 File Offset: 0x001080E4
		internal TextRange(ITextPointer position1, ITextPointer position2, bool ignoreTextUnitBoundaries)
		{
			if (position1 == null)
			{
				throw new ArgumentNullException("position1");
			}
			if (position2 == null)
			{
				throw new ArgumentNullException("position2");
			}
			this.SetFlags(ignoreTextUnitBoundaries, TextRange.Flags.IgnoreTextUnitBoundaries);
			ValidationHelper.VerifyPosition(position1.TextContainer, position1, "position1");
			ValidationHelper.VerifyPosition(position1.TextContainer, position2, "position2");
			TextRangeBase.Select(this, position1, position2);
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x00109F45 File Offset: 0x00108145
		internal TextRange(TextPointer position1, TextPointer position2, bool useRestrictiveXamlXmlReader) : this(position1, position2)
		{
			this._useRestrictiveXamlXmlReader = useRestrictiveXamlXmlReader;
		}

		// Token: 0x06003AA6 RID: 15014 RVA: 0x00109F56 File Offset: 0x00108156
		bool ITextRange.Contains(ITextPointer position)
		{
			return TextRangeBase.Contains(this, position);
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x00109F5F File Offset: 0x0010815F
		void ITextRange.Select(ITextPointer position1, ITextPointer position2)
		{
			TextRangeBase.Select(this, position1, position2);
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x00109F69 File Offset: 0x00108169
		void ITextRange.SelectWord(ITextPointer position)
		{
			TextRangeBase.SelectWord(this, position);
		}

		// Token: 0x06003AA9 RID: 15017 RVA: 0x00109F72 File Offset: 0x00108172
		void ITextRange.SelectParagraph(ITextPointer position)
		{
			TextRangeBase.SelectParagraph(this, position);
		}

		// Token: 0x06003AAA RID: 15018 RVA: 0x00109F7B File Offset: 0x0010817B
		void ITextRange.ApplyTypingHeuristics(bool overType)
		{
			TextRangeBase.ApplyTypingHeuristics(this, overType);
		}

		// Token: 0x06003AAB RID: 15019 RVA: 0x00109F84 File Offset: 0x00108184
		object ITextRange.GetPropertyValue(DependencyProperty formattingProperty)
		{
			return TextRangeBase.GetPropertyValue(this, formattingProperty);
		}

		// Token: 0x06003AAC RID: 15020 RVA: 0x00109F8D File Offset: 0x0010818D
		UIElement ITextRange.GetUIElementSelected()
		{
			return TextRangeBase.GetUIElementSelected(this);
		}

		// Token: 0x06003AAD RID: 15021 RVA: 0x00109F95 File Offset: 0x00108195
		bool ITextRange.CanSave(string dataFormat)
		{
			return TextRangeBase.CanSave(this, dataFormat);
		}

		// Token: 0x06003AAE RID: 15022 RVA: 0x00109F9E File Offset: 0x0010819E
		void ITextRange.Save(Stream stream, string dataFormat)
		{
			TextRangeBase.Save(this, stream, dataFormat, false);
		}

		// Token: 0x06003AAF RID: 15023 RVA: 0x00109FA9 File Offset: 0x001081A9
		void ITextRange.Save(Stream stream, string dataFormat, bool preserveTextElements)
		{
			TextRangeBase.Save(this, stream, dataFormat, preserveTextElements);
		}

		// Token: 0x06003AB0 RID: 15024 RVA: 0x00109FB4 File Offset: 0x001081B4
		void ITextRange.BeginChange()
		{
			TextRangeBase.BeginChange(this);
		}

		// Token: 0x06003AB1 RID: 15025 RVA: 0x00109FBC File Offset: 0x001081BC
		void ITextRange.BeginChangeNoUndo()
		{
			TextRangeBase.BeginChangeNoUndo(this);
		}

		// Token: 0x06003AB2 RID: 15026 RVA: 0x00109FC4 File Offset: 0x001081C4
		void ITextRange.EndChange()
		{
			TextRangeBase.EndChange(this, false, false);
		}

		// Token: 0x06003AB3 RID: 15027 RVA: 0x00109FCE File Offset: 0x001081CE
		void ITextRange.EndChange(bool disableScroll, bool skipEvents)
		{
			TextRangeBase.EndChange(this, disableScroll, skipEvents);
		}

		// Token: 0x06003AB4 RID: 15028 RVA: 0x00109FD8 File Offset: 0x001081D8
		IDisposable ITextRange.DeclareChangeBlock()
		{
			return new TextRange.ChangeBlock(this, false);
		}

		// Token: 0x06003AB5 RID: 15029 RVA: 0x00109FE1 File Offset: 0x001081E1
		IDisposable ITextRange.DeclareChangeBlock(bool disableScroll)
		{
			return new TextRange.ChangeBlock(this, disableScroll);
		}

		// Token: 0x06003AB6 RID: 15030 RVA: 0x00109FEA File Offset: 0x001081EA
		void ITextRange.NotifyChanged(bool disableScroll, bool skipEvents)
		{
			TextRangeBase.NotifyChanged(this, disableScroll);
		}

		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x06003AB7 RID: 15031 RVA: 0x00109FF3 File Offset: 0x001081F3
		bool ITextRange.IgnoreTextUnitBoundaries
		{
			get
			{
				return this.CheckFlags(TextRange.Flags.IgnoreTextUnitBoundaries);
			}
		}

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x06003AB8 RID: 15032 RVA: 0x00109FFC File Offset: 0x001081FC
		ITextPointer ITextRange.Start
		{
			get
			{
				return TextRangeBase.GetStart(this);
			}
		}

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x06003AB9 RID: 15033 RVA: 0x0010A004 File Offset: 0x00108204
		ITextPointer ITextRange.End
		{
			get
			{
				return TextRangeBase.GetEnd(this);
			}
		}

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x06003ABA RID: 15034 RVA: 0x0010A00C File Offset: 0x0010820C
		bool ITextRange.IsEmpty
		{
			get
			{
				return TextRangeBase.GetIsEmpty(this);
			}
		}

		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x06003ABB RID: 15035 RVA: 0x0010A014 File Offset: 0x00108214
		List<TextSegment> ITextRange.TextSegments
		{
			get
			{
				return TextRangeBase.GetTextSegments(this);
			}
		}

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x06003ABC RID: 15036 RVA: 0x0010A01C File Offset: 0x0010821C
		bool ITextRange.HasConcreteTextContainer
		{
			get
			{
				Invariant.Assert(this._textSegments != null, "_textSegments must not be null");
				Invariant.Assert(this._textSegments.Count > 0, "_textSegments.Count must be > 0");
				return this._textSegments[0].Start is TextPointer;
			}
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x06003ABD RID: 15037 RVA: 0x0010A070 File Offset: 0x00108270
		// (set) Token: 0x06003ABE RID: 15038 RVA: 0x0010A078 File Offset: 0x00108278
		string ITextRange.Text
		{
			get
			{
				return TextRangeBase.GetText(this);
			}
			set
			{
				TextRangeBase.SetText(this, value);
			}
		}

		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x06003ABF RID: 15039 RVA: 0x0010A081 File Offset: 0x00108281
		string ITextRange.Xml
		{
			get
			{
				return TextRangeBase.GetXml(this);
			}
		}

		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x06003AC0 RID: 15040 RVA: 0x0010A089 File Offset: 0x00108289
		int ITextRange.ChangeBlockLevel
		{
			get
			{
				return TextRangeBase.GetChangeBlockLevel(this);
			}
		}

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x06003AC1 RID: 15041 RVA: 0x0010A091 File Offset: 0x00108291
		bool ITextRange.IsTableCellRange
		{
			get
			{
				return TextRangeBase.GetIsTableCellRange(this);
			}
		}

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x06003AC2 RID: 15042 RVA: 0x0010A099 File Offset: 0x00108299
		// (remove) Token: 0x06003AC3 RID: 15043 RVA: 0x0010A0A2 File Offset: 0x001082A2
		event EventHandler ITextRange.Changed
		{
			add
			{
				this.Changed += value;
			}
			remove
			{
				this.Changed -= value;
			}
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x0010A0AB File Offset: 0x001082AB
		void ITextRange.FireChanged()
		{
			if (this.Changed != null)
			{
				this.Changed(this, EventArgs.Empty);
			}
		}

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x06003AC5 RID: 15045 RVA: 0x0010A0C6 File Offset: 0x001082C6
		// (set) Token: 0x06003AC6 RID: 15046 RVA: 0x0010A0CF File Offset: 0x001082CF
		bool ITextRange._IsTableCellRange
		{
			get
			{
				return this.CheckFlags(TextRange.Flags.IsTableCellRange);
			}
			set
			{
				this.SetFlags(value, TextRange.Flags.IsTableCellRange);
			}
		}

		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x06003AC7 RID: 15047 RVA: 0x0010A0D9 File Offset: 0x001082D9
		// (set) Token: 0x06003AC8 RID: 15048 RVA: 0x0010A0E1 File Offset: 0x001082E1
		List<TextSegment> ITextRange._TextSegments
		{
			get
			{
				return this._textSegments;
			}
			set
			{
				this._textSegments = value;
			}
		}

		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x06003AC9 RID: 15049 RVA: 0x0010A0EA File Offset: 0x001082EA
		// (set) Token: 0x06003ACA RID: 15050 RVA: 0x0010A0F2 File Offset: 0x001082F2
		int ITextRange._ChangeBlockLevel
		{
			get
			{
				return this._changeBlockLevel;
			}
			set
			{
				this._changeBlockLevel = value;
			}
		}

		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x06003ACB RID: 15051 RVA: 0x0010A0FB File Offset: 0x001082FB
		// (set) Token: 0x06003ACC RID: 15052 RVA: 0x0010A103 File Offset: 0x00108303
		ChangeBlockUndoRecord ITextRange._ChangeBlockUndoRecord
		{
			get
			{
				return this._changeBlockUndoRecord;
			}
			set
			{
				this._changeBlockUndoRecord = value;
			}
		}

		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x06003ACD RID: 15053 RVA: 0x0010A10C File Offset: 0x0010830C
		// (set) Token: 0x06003ACE RID: 15054 RVA: 0x0010A114 File Offset: 0x00108314
		bool ITextRange._IsChanged
		{
			get
			{
				return this._IsChanged;
			}
			set
			{
				this._IsChanged = value;
			}
		}

		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x06003ACF RID: 15055 RVA: 0x0010A11D File Offset: 0x0010831D
		// (set) Token: 0x06003AD0 RID: 15056 RVA: 0x0010A125 File Offset: 0x00108325
		uint ITextRange._ContentGeneration
		{
			get
			{
				return this._ContentGeneration;
			}
			set
			{
				this._ContentGeneration = value;
			}
		}

		/// <summary>Checks whether a position (specified by a <see cref="T:System.Windows.Documents.TextPointer" />) is located within the current selection.</summary>
		/// <param name="textPointer">A position to test for inclusion in the current selection.</param>
		/// <returns>
		///     <see langword="true" /> if the specified position is located within the current selection; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">Occurs when textPointer is not in the same document as the current selection.</exception>
		// Token: 0x06003AD1 RID: 15057 RVA: 0x0010A12E File Offset: 0x0010832E
		public bool Contains(TextPointer textPointer)
		{
			return ((ITextRange)this).Contains(textPointer);
		}

		/// <summary>Updates the current selection, taking two <see cref="T:System.Windows.Documents.TextPointer" /> positions to indicate the updated selection.</summary>
		/// <param name="position1">A fixed anchor position that marks one end of the updated selection.</param>
		/// <param name="position2">A movable position that marks the other end of the updated selection.</param>
		/// <exception cref="T:System.ArgumentException">Occurs when <paramref name="position1" /> and <paramref name="position2" /> are not positioned within the same document.</exception>
		/// <exception cref="T:System.ArgumentNullException">Occurs when <paramref name="position1" /> or <paramref name="position2" /> is <see langword="null" />.</exception>
		// Token: 0x06003AD2 RID: 15058 RVA: 0x0010A137 File Offset: 0x00108337
		public void Select(TextPointer position1, TextPointer position2)
		{
			((ITextRange)this).Select(position1, position2);
		}

		// Token: 0x06003AD3 RID: 15059 RVA: 0x0010A141 File Offset: 0x00108341
		internal void SelectWord(TextPointer textPointer)
		{
			((ITextRange)this).SelectWord(textPointer);
		}

		// Token: 0x06003AD4 RID: 15060 RVA: 0x0010A14A File Offset: 0x0010834A
		internal void SelectParagraph(ITextPointer position)
		{
			((ITextRange)this).SelectParagraph(position);
		}

		/// <summary>Applies a specified formatting property and value to the current selection.</summary>
		/// <param name="formattingProperty">A formatting property to apply.</param>
		/// <param name="value">The value for the formatting property.</param>
		/// <exception cref="T:System.ArgumentException">Occurs when <paramref name="formattingProperty" /> does not specify a valid formatting property, or <paramref name="value" /> specifies an invalid value for <paramref name="formattingProperty" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">Occurs when <paramref name="formattingProperty" /> is <see langword="null" />.</exception>
		// Token: 0x06003AD5 RID: 15061 RVA: 0x0010A153 File Offset: 0x00108353
		public void ApplyPropertyValue(DependencyProperty formattingProperty, object value)
		{
			this.ApplyPropertyValue(formattingProperty, value, false, PropertyValueAction.SetValue);
		}

		// Token: 0x06003AD6 RID: 15062 RVA: 0x0010A15F File Offset: 0x0010835F
		internal void ApplyPropertyValue(DependencyProperty formattingProperty, object value, bool applyToParagraphs)
		{
			this.ApplyPropertyValue(formattingProperty, value, applyToParagraphs, PropertyValueAction.SetValue);
		}

		// Token: 0x06003AD7 RID: 15063 RVA: 0x0010A16C File Offset: 0x0010836C
		internal void ApplyPropertyValue(DependencyProperty formattingProperty, object value, bool applyToParagraphs, PropertyValueAction propertyValueAction)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "Can't apply property to non-TextContainer range!");
			if (formattingProperty == null)
			{
				throw new ArgumentNullException("formattingProperty");
			}
			if (!TextSchema.IsCharacterProperty(formattingProperty) && !TextSchema.IsParagraphProperty(formattingProperty))
			{
				throw new ArgumentException(SR.Get("TextEditorPropertyIsNotApplicableForTextFormatting", new object[]
				{
					formattingProperty.Name
				}));
			}
			if (value is string && formattingProperty.PropertyType != typeof(string))
			{
				TypeConverter converter = TypeDescriptor.GetConverter(formattingProperty.PropertyType);
				Invariant.Assert(converter != null);
				value = converter.ConvertFromString((string)value);
			}
			if (!formattingProperty.IsValidValue(value) && (!(formattingProperty.PropertyType == typeof(Thickness)) || !(value is Thickness)))
			{
				throw new ArgumentException(SR.Get("TextEditorTypeOfParameterIsNotAppropriateForFormattingProperty", new object[]
				{
					(value == null) ? "null" : value.GetType().Name,
					formattingProperty.Name
				}), "value");
			}
			if (propertyValueAction != PropertyValueAction.SetValue && propertyValueAction != PropertyValueAction.IncreaseByAbsoluteValue && propertyValueAction != PropertyValueAction.DecreaseByAbsoluteValue && propertyValueAction != PropertyValueAction.IncreaseByPercentageValue && propertyValueAction != PropertyValueAction.DecreaseByPercentageValue)
			{
				throw new ArgumentException(SR.Get("TextRange_InvalidParameterValue"), "propertyValueAction");
			}
			if (propertyValueAction != PropertyValueAction.SetValue && !TextSchema.IsPropertyIncremental(formattingProperty))
			{
				throw new ArgumentException(SR.Get("TextRange_PropertyCannotBeIncrementedOrDecremented", new object[]
				{
					formattingProperty.Name
				}), "propertyValueAction");
			}
			this.ApplyPropertyToTextVirtual(formattingProperty, value, applyToParagraphs, propertyValueAction);
		}

		/// <summary>Removes all formatting properties (represented by <see cref="T:System.Windows.Documents.Inline" /> elements) from the current selection.</summary>
		// Token: 0x06003AD8 RID: 15064 RVA: 0x0010A2D4 File Offset: 0x001084D4
		public void ClearAllProperties()
		{
			Invariant.Assert(this.HasConcreteTextContainer, "Can't clear properties in non-TextContainer range");
			this.ClearAllPropertiesVirtual();
		}

		/// <summary>Returns the effective value of a specified formatting property on the current selection.</summary>
		/// <param name="formattingProperty">A formatting property to get the value of with respect to the current selection.</param>
		/// <returns>An object specifying the value of the specified formatting property.</returns>
		/// <exception cref="T:System.ArgumentException">Occurs when <paramref name="formattingProperty" /> does not specify a valid formatting property, or <paramref name="value" /> specifies an invalid value for <paramref name="formattingProperty" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">Occurs when <paramref name="formattingProperty" /> is <see langword="null" />.</exception>
		// Token: 0x06003AD9 RID: 15065 RVA: 0x0010A2EC File Offset: 0x001084EC
		public object GetPropertyValue(DependencyProperty formattingProperty)
		{
			if (formattingProperty == null)
			{
				throw new ArgumentNullException("formattingProperty");
			}
			if (!TextSchema.IsCharacterProperty(formattingProperty) && !TextSchema.IsParagraphProperty(formattingProperty))
			{
				throw new ArgumentException(SR.Get("TextEditorPropertyIsNotApplicableForTextFormatting", new object[]
				{
					formattingProperty.Name
				}));
			}
			return ((ITextRange)this).GetPropertyValue(formattingProperty);
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x0010A33D File Offset: 0x0010853D
		internal UIElement GetUIElementSelected()
		{
			return ((ITextRange)this).GetUIElementSelected();
		}

		/// <summary>Checks whether the current selection can be saved as a specified data format.</summary>
		/// <param name="dataFormat">A data format to check for save compatibility with the current selection.  See <see cref="T:System.Windows.DataFormats" /> for a list of predefined data formats.</param>
		/// <returns>
		///     <see langword="true" /> if the current selection can be saved as the specified data format; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003ADB RID: 15067 RVA: 0x0010A345 File Offset: 0x00108545
		public bool CanSave(string dataFormat)
		{
			return ((ITextRange)this).CanSave(dataFormat);
		}

		/// <summary>Checks whether the current selection can be loaded with content in a specified data format.</summary>
		/// <param name="dataFormat">A data format to check for load-compatibility into the current selection.  See <see cref="T:System.Windows.DataFormats" /> for a list of predefined data formats.</param>
		/// <returns>
		///     <see langword="true" /> if the current selection can be loaded with content in the specified data format; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003ADC RID: 15068 RVA: 0x0010A34E File Offset: 0x0010854E
		public bool CanLoad(string dataFormat)
		{
			return TextRangeBase.CanLoad(this, dataFormat);
		}

		/// <summary>Saves the current selection to a specified stream in a specified data format.</summary>
		/// <param name="stream">An empty, writable stream to save the current selection to.</param>
		/// <param name="dataFormat">A data format to save the current selection as.  Currently supported data formats are <see cref="F:System.Windows.DataFormats.Rtf" />, <see cref="F:System.Windows.DataFormats.Text" />, <see cref="F:System.Windows.DataFormats.Xaml" />, and <see cref="F:System.Windows.DataFormats.XamlPackage" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="stream" /> or <paramref name="dataFormat" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The specified data format is unsupported.-orContent loaded from <paramref name="stream" /> does not match the specified data format.</exception>
		// Token: 0x06003ADD RID: 15069 RVA: 0x0010A357 File Offset: 0x00108557
		public void Save(Stream stream, string dataFormat)
		{
			((ITextRange)this).Save(stream, dataFormat);
		}

		/// <summary>Saves the current selection to a specified stream in a specified data format, with the option of preserving custom <see cref="T:System.Windows.Documents.TextElement" /> objects.</summary>
		/// <param name="stream">An empty, writable stream to save the current selection to.</param>
		/// <param name="dataFormat">A data format to save the current selection as.  Currently supported data formats are <see cref="F:System.Windows.DataFormats.Rtf" />, <see cref="F:System.Windows.DataFormats.Text" />, <see cref="F:System.Windows.DataFormats.Xaml" />, and <see cref="F:System.Windows.DataFormats.XamlPackage" />.</param>
		/// <param name="preserveTextElements">
		///       <see langword="true" /> to preserve custom <see cref="T:System.Windows.Documents.TextElement" /> objects; otherwise, <see langword="false" />.</param>
		/// <exception cref="T:System.ArgumentNullException">Occurs when <paramref name="stream" /> or <paramref name="dataFormat" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">Occurs when the specified data format is unsupported.  May also be raised if content loaded from <paramref name="stream" /> does not match the specified data format.</exception>
		// Token: 0x06003ADE RID: 15070 RVA: 0x0010A361 File Offset: 0x00108561
		public void Save(Stream stream, string dataFormat, bool preserveTextElements)
		{
			((ITextRange)this).Save(stream, dataFormat, preserveTextElements);
		}

		/// <summary>Loads the current selection in a specified data format from a specified stream.</summary>
		/// <param name="stream">A readable stream that contains data to load into the current selection.</param>
		/// <param name="dataFormat">A data format to load the data as.  Currently supported data formats are <see cref="F:System.Windows.DataFormats.Rtf" />, <see cref="F:System.Windows.DataFormats.Text" />, <see cref="F:System.Windows.DataFormats.Xaml" />, and <see cref="F:System.Windows.DataFormats.XamlPackage" />.</param>
		/// <exception cref="T:System.ArgumentNullException">Occurs when <paramref name="stream" /> or <paramref name="dataFormat" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">Occurs when the specified data format is unsupported.  May also be raised if content loaded from <paramref name="stream" /> does not match the specified data format.</exception>
		// Token: 0x06003ADF RID: 15071 RVA: 0x0010A36C File Offset: 0x0010856C
		public void Load(Stream stream, string dataFormat)
		{
			this.LoadVirtual(stream, dataFormat);
		}

		// Token: 0x06003AE0 RID: 15072 RVA: 0x0010A376 File Offset: 0x00108576
		internal void InsertEmbeddedUIElement(FrameworkElement embeddedElement)
		{
			Invariant.Assert(embeddedElement != null);
			this.InsertEmbeddedUIElementVirtual(embeddedElement);
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x0010A388 File Offset: 0x00108588
		internal void InsertImage(Image image)
		{
			BitmapSource bitmapSource = (BitmapSource)image.Source;
			Invariant.Assert(bitmapSource != null);
			if (double.IsNaN(image.Height))
			{
				if ((double)bitmapSource.PixelHeight < 300.0)
				{
					image.Height = (double)bitmapSource.PixelHeight;
				}
				else
				{
					image.Height = 300.0;
				}
			}
			if (double.IsNaN(image.Width))
			{
				if ((double)bitmapSource.PixelHeight < 300.0)
				{
					image.Width = (double)bitmapSource.PixelWidth;
				}
				else
				{
					image.Width = 300.0 / (double)bitmapSource.PixelHeight * (double)bitmapSource.PixelWidth;
				}
			}
			this.InsertEmbeddedUIElement(image);
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x0010A43B File Offset: 0x0010863B
		internal virtual void SetXmlVirtual(TextElement fragment)
		{
			if (!this.IsTableCellRange)
			{
				TextRangeSerialization.PasteXml(this, fragment);
			}
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x0010A44C File Offset: 0x0010864C
		internal virtual void LoadVirtual(Stream stream, string dataFormat)
		{
			TextRangeBase.Load(this, stream, dataFormat);
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x0010A456 File Offset: 0x00108656
		internal Table InsertTable(int rowCount, int columnCount)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "InsertTable: TextRange must belong to non-abstract TextContainer");
			return this.InsertTableVirtual(rowCount, columnCount);
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x0010A470 File Offset: 0x00108670
		internal TextRange InsertRows(int rowCount)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "InsertRows: TextRange must belong to non-abstract TextContainer");
			return this.InsertRowsVirtual(rowCount);
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x0010A489 File Offset: 0x00108689
		internal bool DeleteRows()
		{
			Invariant.Assert(this.HasConcreteTextContainer, "DeleteRows: TextRange must belong to non-abstract TextContainer");
			return this.DeleteRowsVirtual();
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x0010A4A1 File Offset: 0x001086A1
		internal TextRange InsertColumns(int columnCount)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "InsertColumns: TextRange must belong to non-abstract TextContainer");
			return this.InsertColumnsVirtual(columnCount);
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x0010A4BA File Offset: 0x001086BA
		internal bool DeleteColumns()
		{
			Invariant.Assert(this.HasConcreteTextContainer, "DeleteColumns: TextRange must belong to non-abstract TextContainer");
			return this.DeleteColumnsVirtual();
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x0010A4D2 File Offset: 0x001086D2
		internal TextRange MergeCells()
		{
			Invariant.Assert(this.HasConcreteTextContainer, "MergeCells: TextRange must belong to non-abstract TextContainer");
			return this.MergeCellsVirtual();
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x0010A4EA File Offset: 0x001086EA
		internal TextRange SplitCell(int splitCountHorizontal, int splitCountVertical)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "SplitCells: TextRange must belong to non-abstract TextContainer");
			return this.SplitCellVirtual(splitCountHorizontal, splitCountVertical);
		}

		/// <summary>Gets the position that marks the beginning of the current selection.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> that points to the beginning of the current selection.</returns>
		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x06003AEB RID: 15083 RVA: 0x0010A504 File Offset: 0x00108704
		public TextPointer Start
		{
			get
			{
				return (TextPointer)((ITextRange)this).Start;
			}
		}

		/// <summary>Get the position that marks the end of the current selection.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> that points to the end of the current selection.</returns>
		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x06003AEC RID: 15084 RVA: 0x0010A511 File Offset: 0x00108711
		public TextPointer End
		{
			get
			{
				return (TextPointer)((ITextRange)this).End;
			}
		}

		/// <summary>Gets a value indicating whether or not the current selection is empty.</summary>
		/// <returns>
		///     <see langword="true" /> if the current selection is empty; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x06003AED RID: 15085 RVA: 0x0010A51E File Offset: 0x0010871E
		public bool IsEmpty
		{
			get
			{
				return ((ITextRange)this).IsEmpty;
			}
		}

		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x06003AEE RID: 15086 RVA: 0x0010A526 File Offset: 0x00108726
		internal bool HasConcreteTextContainer
		{
			get
			{
				return ((ITextRange)this).HasConcreteTextContainer;
			}
		}

		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x06003AEF RID: 15087 RVA: 0x0010A52E File Offset: 0x0010872E
		internal FrameworkElement ContainingFrameworkElement
		{
			get
			{
				if (this.HasConcreteTextContainer)
				{
					return this.Start.ContainingFrameworkElement;
				}
				return null;
			}
		}

		/// <summary>Gets or sets the plain text contents of the current selection.</summary>
		/// <returns>A string containing the plain text contents of the current selection.</returns>
		/// <exception cref="T:System.ArgumentNullException">Occurs when an attempt is made to set this property to <see langword="null" />.</exception>
		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x06003AF0 RID: 15088 RVA: 0x0010A545 File Offset: 0x00108745
		// (set) Token: 0x06003AF1 RID: 15089 RVA: 0x0010A54D File Offset: 0x0010874D
		public string Text
		{
			get
			{
				return ((ITextRange)this).Text;
			}
			set
			{
				((ITextRange)this).Text = value;
			}
		}

		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x06003AF2 RID: 15090 RVA: 0x0010A556 File Offset: 0x00108756
		// (set) Token: 0x06003AF3 RID: 15091 RVA: 0x0010A560 File Offset: 0x00108760
		internal string Xml
		{
			get
			{
				return ((ITextRange)this).Xml;
			}
			set
			{
				TextRangeBase.BeginChange(this);
				try
				{
					object obj = XamlReaderProxy.Load(new XmlTextReader(new StringReader(value)), this._useRestrictiveXamlXmlReader);
					TextElement textElement = obj as TextElement;
					if (textElement != null)
					{
						this.SetXmlVirtual(textElement);
					}
				}
				finally
				{
					TextRangeBase.EndChange(this);
				}
			}
		}

		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x06003AF4 RID: 15092 RVA: 0x0010A5B4 File Offset: 0x001087B4
		internal bool IsTableCellRange
		{
			get
			{
				return ((ITextRange)this).IsTableCellRange;
			}
		}

		/// <summary>Occurs when the range is repositioned to cover a new span of content.</summary>
		// Token: 0x14000091 RID: 145
		// (add) Token: 0x06003AF5 RID: 15093 RVA: 0x0010A5BC File Offset: 0x001087BC
		// (remove) Token: 0x06003AF6 RID: 15094 RVA: 0x0010A5F4 File Offset: 0x001087F4
		public event EventHandler Changed;

		// Token: 0x06003AF7 RID: 15095 RVA: 0x0010A629 File Offset: 0x00108829
		internal void BeginChange()
		{
			((ITextRange)this).BeginChange();
		}

		// Token: 0x06003AF8 RID: 15096 RVA: 0x0010A631 File Offset: 0x00108831
		internal void EndChange()
		{
			((ITextRange)this).EndChange();
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x0010A639 File Offset: 0x00108839
		internal IDisposable DeclareChangeBlock()
		{
			return ((ITextRange)this).DeclareChangeBlock();
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x0010A641 File Offset: 0x00108841
		internal IDisposable DeclareChangeBlock(bool disableScroll)
		{
			return ((ITextRange)this).DeclareChangeBlock(disableScroll);
		}

		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x06003AFB RID: 15099 RVA: 0x0010A64A File Offset: 0x0010884A
		// (set) Token: 0x06003AFC RID: 15100 RVA: 0x0010A653 File Offset: 0x00108853
		internal bool _IsChanged
		{
			get
			{
				return this.CheckFlags(TextRange.Flags.IsChanged);
			}
			set
			{
				this.SetFlags(value, TextRange.Flags.IsChanged);
			}
		}

		// Token: 0x06003AFD RID: 15101 RVA: 0x0010A660 File Offset: 0x00108860
		internal virtual void InsertEmbeddedUIElementVirtual(FrameworkElement embeddedElement)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "Can't insert embedded object to non-TextContainer range!");
			Invariant.Assert(embeddedElement != null);
			TextRangeBase.BeginChange(this);
			try
			{
				this.Text = string.Empty;
				TextPointer textPointer = TextRangeEditTables.EnsureInsertionPosition(this.Start);
				Paragraph paragraph = textPointer.Paragraph;
				if (paragraph != null)
				{
					if (Paragraph.HasNoTextContent(paragraph))
					{
						BlockUIContainer blockUIContainer = new BlockUIContainer(embeddedElement);
						blockUIContainer.TextAlignment = TextRangeEdit.GetTextAlignmentFromHorizontalAlignment(embeddedElement.HorizontalAlignment);
						paragraph.SiblingBlocks.InsertAfter(paragraph, blockUIContainer);
						paragraph.SiblingBlocks.Remove(paragraph);
						this.Select(blockUIContainer.ContentStart, blockUIContainer.ContentEnd);
					}
					else
					{
						InlineUIContainer inlineUIContainer = new InlineUIContainer(embeddedElement);
						TextPointer textPointer2 = TextRangeEdit.SplitFormattingElements(this.Start, false);
						textPointer2.InsertTextElement(inlineUIContainer);
						this.Select(inlineUIContainer.ElementStart, inlineUIContainer.ElementEnd);
					}
				}
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x0010A744 File Offset: 0x00108944
		internal virtual void ApplyPropertyToTextVirtual(DependencyProperty formattingProperty, object value, bool applyToParagraphs, PropertyValueAction propertyValueAction)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				for (int i = 0; i < this._textSegments.Count; i++)
				{
					TextSegment textSegment = this._textSegments[i];
					if (formattingProperty == FrameworkElement.FlowDirectionProperty)
					{
						if (applyToParagraphs || this.IsEmpty || TextRangeBase.IsParagraphBoundaryCrossed(this))
						{
							TextRangeEdit.SetParagraphProperty((TextPointer)textSegment.Start, (TextPointer)textSegment.End, formattingProperty, value, propertyValueAction);
						}
						else
						{
							TextRangeEdit.SetInlineProperty((TextPointer)textSegment.Start, (TextPointer)textSegment.End, formattingProperty, value, propertyValueAction);
						}
					}
					else if (TextSchema.IsCharacterProperty(formattingProperty))
					{
						TextRangeEdit.SetInlineProperty((TextPointer)textSegment.Start, (TextPointer)textSegment.End, formattingProperty, value, propertyValueAction);
					}
					else if (TextSchema.IsParagraphProperty(formattingProperty))
					{
						if (formattingProperty.PropertyType == typeof(Thickness) && (FlowDirection)textSegment.Start.GetValue(Block.FlowDirectionProperty) == FlowDirection.RightToLeft)
						{
							value = new Thickness(((Thickness)value).Right, ((Thickness)value).Top, ((Thickness)value).Left, ((Thickness)value).Bottom);
						}
						TextRangeEdit.SetParagraphProperty((TextPointer)textSegment.Start, (TextPointer)textSegment.End, formattingProperty, value, propertyValueAction);
					}
				}
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x0010A8DC File Offset: 0x00108ADC
		internal virtual void ClearAllPropertiesVirtual()
		{
			TextRangeBase.BeginChange(this);
			try
			{
				TextRangeEdit.CharacterResetFormatting(this.Start, this.End);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x0010A91C File Offset: 0x00108B1C
		internal virtual Table InsertTableVirtual(int rowCount, int columnCount)
		{
			TextRangeBase.BeginChange(this);
			Table result;
			try
			{
				result = TextRangeEditTables.InsertTable(this.End, rowCount, columnCount);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x0010A958 File Offset: 0x00108B58
		internal virtual TextRange InsertRowsVirtual(int rowCount)
		{
			TextRangeBase.BeginChange(this);
			TextRange result;
			try
			{
				result = TextRangeEditTables.InsertRows(this, rowCount);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x0010A990 File Offset: 0x00108B90
		internal virtual bool DeleteRowsVirtual()
		{
			TextRangeBase.BeginChange(this);
			bool result;
			try
			{
				result = TextRangeEditTables.DeleteRows(this);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x0010A9C4 File Offset: 0x00108BC4
		internal virtual TextRange InsertColumnsVirtual(int columnCount)
		{
			TextRangeBase.BeginChange(this);
			TextRange result;
			try
			{
				result = TextRangeEditTables.InsertColumns(this, columnCount);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x0010A9FC File Offset: 0x00108BFC
		internal virtual bool DeleteColumnsVirtual()
		{
			TextRangeBase.BeginChange(this);
			bool result;
			try
			{
				result = TextRangeEditTables.DeleteColumns(this);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x0010AA30 File Offset: 0x00108C30
		internal virtual TextRange MergeCellsVirtual()
		{
			TextRangeBase.BeginChange(this);
			TextRange result;
			try
			{
				result = TextRangeEditTables.MergeCells(this);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x0010AA64 File Offset: 0x00108C64
		internal virtual TextRange SplitCellVirtual(int splitCountHorizontal, int splitCountVertical)
		{
			TextRangeBase.BeginChange(this);
			TextRange result;
			try
			{
				result = TextRangeEditTables.SplitCell(this, splitCountHorizontal, splitCountVertical);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x06003B07 RID: 15111 RVA: 0x0010A0EA File Offset: 0x001082EA
		internal int ChangeBlockLevel
		{
			get
			{
				return this._changeBlockLevel;
			}
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x0010AA9C File Offset: 0x00108C9C
		private void SetFlags(bool value, TextRange.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x0010AABA File Offset: 0x00108CBA
		private bool CheckFlags(TextRange.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x040025E0 RID: 9696
		private bool _useRestrictiveXamlXmlReader;

		// Token: 0x040025E1 RID: 9697
		private List<TextSegment> _textSegments;

		// Token: 0x040025E2 RID: 9698
		private int _changeBlockLevel;

		// Token: 0x040025E3 RID: 9699
		private ChangeBlockUndoRecord _changeBlockUndoRecord;

		// Token: 0x040025E4 RID: 9700
		private uint _ContentGeneration;

		// Token: 0x040025E5 RID: 9701
		private TextRange.Flags _flags;

		// Token: 0x02000908 RID: 2312
		private class ChangeBlock : IDisposable
		{
			// Token: 0x060085D2 RID: 34258 RVA: 0x0024B004 File Offset: 0x00249204
			internal ChangeBlock(ITextRange range, bool disableScroll)
			{
				this._range = range;
				this._disableScroll = disableScroll;
				this._range.BeginChange();
			}

			// Token: 0x060085D3 RID: 34259 RVA: 0x0024B025 File Offset: 0x00249225
			void IDisposable.Dispose()
			{
				this._range.EndChange(this._disableScroll, false);
				GC.SuppressFinalize(this);
			}

			// Token: 0x0400430D RID: 17165
			private readonly ITextRange _range;

			// Token: 0x0400430E RID: 17166
			private readonly bool _disableScroll;
		}

		// Token: 0x02000909 RID: 2313
		[Flags]
		private enum Flags
		{
			// Token: 0x04004310 RID: 17168
			IgnoreTextUnitBoundaries = 1,
			// Token: 0x04004311 RID: 17169
			IsChanged = 2,
			// Token: 0x04004312 RID: 17170
			IsTableCellRange = 4
		}
	}
}
