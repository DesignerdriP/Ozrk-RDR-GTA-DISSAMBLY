using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.PresentationFramework;
using MS.Internal.Text;

namespace System.Windows.Documents
{
	/// <summary>An abstract class used as the base class for the abstract <see cref="T:System.Windows.Documents.Block" /> and <see cref="T:System.Windows.Documents.Inline" /> classes.</summary>
	// Token: 0x02000400 RID: 1024
	public abstract class TextElement : FrameworkContentElement, IAddChild
	{
		// Token: 0x06003929 RID: 14633 RVA: 0x001032F4 File Offset: 0x001014F4
		static TextElement()
		{
			PropertyChangedCallback propertyChangedCallback = new PropertyChangedCallback(TextElement.OnTypographyChanged);
			DependencyProperty[] typographyPropertiesList = Typography.TypographyPropertiesList;
			for (int i = 0; i < typographyPropertiesList.Length; i++)
			{
				typographyPropertiesList[i].OverrideMetadata(typeof(TextElement), new FrameworkPropertyMetadata(propertyChangedCallback));
			}
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x001034EC File Offset: 0x001016EC
		internal TextElement()
		{
		}

		// Token: 0x0600392B RID: 14635 RVA: 0x00103500 File Offset: 0x00101700
		internal void Reposition(TextPointer start, TextPointer end)
		{
			if (start != null)
			{
				ValidationHelper.VerifyPositionPair(start, end);
			}
			else if (end != null)
			{
				throw new ArgumentException(SR.Get("TextElement_UnmatchedEndPointer"));
			}
			if (start != null)
			{
				SplayTreeNode splayTreeNode = start.GetScopingNode();
				SplayTreeNode splayTreeNode2 = end.GetScopingNode();
				if (splayTreeNode == this._textElementNode)
				{
					splayTreeNode = this._textElementNode.GetContainingNode();
				}
				if (splayTreeNode2 == this._textElementNode)
				{
					splayTreeNode2 = this._textElementNode.GetContainingNode();
				}
				if (splayTreeNode != splayTreeNode2)
				{
					throw new ArgumentException(SR.Get("InDifferentScope", new object[]
					{
						"start",
						"end"
					}));
				}
			}
			if (this.IsInTree)
			{
				TextContainer textContainer = this.EnsureTextContainer();
				if (start == null)
				{
					textContainer.BeginChange();
					try
					{
						textContainer.ExtractElementInternal(this);
						return;
					}
					finally
					{
						textContainer.EndChange();
					}
				}
				if (textContainer == start.TextContainer)
				{
					textContainer.BeginChange();
					try
					{
						textContainer.ExtractElementInternal(this);
						textContainer.InsertElementInternal(start, end, this);
						return;
					}
					finally
					{
						textContainer.EndChange();
					}
				}
				textContainer.BeginChange();
				try
				{
					textContainer.ExtractElementInternal(this);
				}
				finally
				{
					textContainer.EndChange();
				}
				start.TextContainer.BeginChange();
				try
				{
					start.TextContainer.InsertElementInternal(start, end, this);
					return;
				}
				finally
				{
					start.TextContainer.EndChange();
				}
			}
			if (start != null)
			{
				start.TextContainer.BeginChange();
				try
				{
					start.TextContainer.InsertElementInternal(start, end, this);
				}
				finally
				{
					start.TextContainer.EndChange();
				}
			}
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x00103690 File Offset: 0x00101890
		internal void RepositionWithContent(TextPointer textPosition)
		{
			TextContainer textContainer;
			if (textPosition == null)
			{
				if (!this.IsInTree)
				{
					return;
				}
				textContainer = this.EnsureTextContainer();
				textContainer.BeginChange();
				try
				{
					textContainer.DeleteContentInternal(this.ElementStart, this.ElementEnd);
					return;
				}
				finally
				{
					textContainer.EndChange();
				}
			}
			textContainer = textPosition.TextContainer;
			textContainer.BeginChange();
			try
			{
				textContainer.InsertElementInternal(textPosition, textPosition, this);
			}
			finally
			{
				textContainer.EndChange();
			}
		}

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x0600392D RID: 14637 RVA: 0x0010370C File Offset: 0x0010190C
		internal TextRange TextRange
		{
			get
			{
				base.VerifyAccess();
				TextContainer tree = this.EnsureTextContainer();
				TextPointer textPointer = new TextPointer(tree, this._textElementNode, ElementEdge.AfterStart, LogicalDirection.Backward);
				textPointer.Freeze();
				TextPointer textPointer2 = new TextPointer(tree, this._textElementNode, ElementEdge.BeforeEnd, LogicalDirection.Forward);
				textPointer2.Freeze();
				return new TextRange(textPointer, textPointer2);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.TextPointer" /> that represents the position just before the start of the element.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointerContext" /> that represents the position just before the start of the <see cref="T:System.Windows.Documents.TextElement" />.</returns>
		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x0600392E RID: 14638 RVA: 0x00103758 File Offset: 0x00101958
		public TextPointer ElementStart
		{
			get
			{
				TextContainer tree = this.EnsureTextContainer();
				TextPointer textPointer = new TextPointer(tree, this._textElementNode, ElementEdge.BeforeStart, LogicalDirection.Forward);
				textPointer.Freeze();
				return textPointer;
			}
		}

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x0600392F RID: 14639 RVA: 0x00103784 File Offset: 0x00101984
		internal StaticTextPointer StaticElementStart
		{
			get
			{
				TextContainer textContainer = this.EnsureTextContainer();
				return new StaticTextPointer(textContainer, this._textElementNode, 0);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.TextPointer" /> that represents the start of content in the element.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointerContext" /> that represents the start of the content in the <see cref="T:System.Windows.Documents.TextElement" />.</returns>
		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x06003930 RID: 14640 RVA: 0x001037A8 File Offset: 0x001019A8
		public TextPointer ContentStart
		{
			get
			{
				TextContainer tree = this.EnsureTextContainer();
				TextPointer textPointer = new TextPointer(tree, this._textElementNode, ElementEdge.AfterStart, LogicalDirection.Backward);
				textPointer.Freeze();
				return textPointer;
			}
		}

		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x06003931 RID: 14641 RVA: 0x001037D4 File Offset: 0x001019D4
		internal StaticTextPointer StaticContentStart
		{
			get
			{
				TextContainer textContainer = this.EnsureTextContainer();
				return new StaticTextPointer(textContainer, this._textElementNode, 1);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.TextPointer" /> that represents the end of the content in the element.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> that represents the end of the content in the <see cref="T:System.Windows.Documents.TextElement" />.</returns>
		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x06003932 RID: 14642 RVA: 0x001037F8 File Offset: 0x001019F8
		public TextPointer ContentEnd
		{
			get
			{
				TextContainer tree = this.EnsureTextContainer();
				TextPointer textPointer = new TextPointer(tree, this._textElementNode, ElementEdge.BeforeEnd, LogicalDirection.Forward);
				textPointer.Freeze();
				return textPointer;
			}
		}

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x06003933 RID: 14643 RVA: 0x00103824 File Offset: 0x00101A24
		internal StaticTextPointer StaticContentEnd
		{
			get
			{
				TextContainer textContainer = this.EnsureTextContainer();
				return new StaticTextPointer(textContainer, this._textElementNode, this._textElementNode.SymbolCount - 1);
			}
		}

		// Token: 0x06003934 RID: 14644 RVA: 0x00103854 File Offset: 0x00101A54
		internal bool Contains(TextPointer position)
		{
			TextContainer tree = this.EnsureTextContainer();
			ValidationHelper.VerifyPosition(tree, position);
			return this.ContentStart.CompareTo(position) <= 0 && this.ContentEnd.CompareTo(position) >= 0;
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.TextPointer" /> that represents the position just after the end of the element.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> that represents the position just after the end of the <see cref="T:System.Windows.Documents.TextElement" />.</returns>
		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x06003935 RID: 14645 RVA: 0x00103894 File Offset: 0x00101A94
		public TextPointer ElementEnd
		{
			get
			{
				TextContainer tree = this.EnsureTextContainer();
				TextPointer textPointer = new TextPointer(tree, this._textElementNode, ElementEdge.AfterEnd, LogicalDirection.Backward);
				textPointer.Freeze();
				return textPointer;
			}
		}

		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x06003936 RID: 14646 RVA: 0x001038C0 File Offset: 0x00101AC0
		internal StaticTextPointer StaticElementEnd
		{
			get
			{
				TextContainer textContainer = this.EnsureTextContainer();
				return new StaticTextPointer(textContainer, this._textElementNode, this._textElementNode.SymbolCount);
			}
		}

		/// <summary>Gets or sets the preferred top-level font family for the content of the element.  </summary>
		/// <returns>A <see cref="T:System.Windows.Media.FontFamily" /> object that specifies the preferred font family, or a primary preferred font family with one or more fallback font families. The default is the font determined by the <see cref="P:System.Windows.SystemFonts.MessageFontFamily" /> value.</returns>
		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x06003937 RID: 14647 RVA: 0x001038EB File Offset: 0x00101AEB
		// (set) Token: 0x06003938 RID: 14648 RVA: 0x001038FD File Offset: 0x00101AFD
		[Localizability(LocalizationCategory.Font, Modifiability = Modifiability.Unmodifiable)]
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(TextElement.FontFamilyProperty);
			}
			set
			{
				base.SetValue(TextElement.FontFamilyProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.TextElement.FontFamily" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.TextElement.FontFamily" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="d" /> is <see langword="null" />.</exception>
		// Token: 0x06003939 RID: 14649 RVA: 0x0010390B File Offset: 0x00101B0B
		public static void SetFontFamily(DependencyObject element, FontFamily value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextElement.FontFamilyProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.TextElement.FontFamily" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.TextElement.FontFamily" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.TextElement.FontFamily" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="d" /> is <see langword="null" />.</exception>
		// Token: 0x0600393A RID: 14650 RVA: 0x00103927 File Offset: 0x00101B27
		public static FontFamily GetFontFamily(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontFamily)element.GetValue(TextElement.FontFamilyProperty);
		}

		/// <summary>Gets or sets the font style for the content of the element.  </summary>
		/// <returns>A member of the <see cref="T:System.Windows.FontStyles" /> class that specifies the desired font style. The default is determined by the <see cref="P:System.Windows.SystemFonts.MessageFontStyle" /> value.</returns>
		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x0600393B RID: 14651 RVA: 0x00103947 File Offset: 0x00101B47
		// (set) Token: 0x0600393C RID: 14652 RVA: 0x00103959 File Offset: 0x00101B59
		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(TextElement.FontStyleProperty);
			}
			set
			{
				base.SetValue(TextElement.FontStyleProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.TextElement.FontStyle" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.TextElement.FontStyle" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="d" /> is <see langword="null" />.</exception>
		// Token: 0x0600393D RID: 14653 RVA: 0x0010396C File Offset: 0x00101B6C
		public static void SetFontStyle(DependencyObject element, FontStyle value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextElement.FontStyleProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.TextElement.FontStyle" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.TextElement.FontStyle" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.TextElement.FontStyle" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="d" /> is <see langword="null" />.</exception>
		// Token: 0x0600393E RID: 14654 RVA: 0x0010398D File Offset: 0x00101B8D
		public static FontStyle GetFontStyle(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontStyle)element.GetValue(TextElement.FontStyleProperty);
		}

		/// <summary>Gets or sets the top-level font weight for the content of the element.  </summary>
		/// <returns>A member of the <see cref="T:System.Windows.FontWeights" /> class that specifies the desired font weight. The default value is determined by the <see cref="P:System.Windows.SystemFonts.MessageFontWeight" /> value.</returns>
		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x0600393F RID: 14655 RVA: 0x001039AD File Offset: 0x00101BAD
		// (set) Token: 0x06003940 RID: 14656 RVA: 0x001039BF File Offset: 0x00101BBF
		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(TextElement.FontWeightProperty);
			}
			set
			{
				base.SetValue(TextElement.FontWeightProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.TextElement.FontWeight" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.TextElement.FontWeight" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="d" /> is <see langword="null" />.</exception>
		// Token: 0x06003941 RID: 14657 RVA: 0x001039D2 File Offset: 0x00101BD2
		public static void SetFontWeight(DependencyObject element, FontWeight value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextElement.FontWeightProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.TextElement.FontWeight" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.TextElement.FontWeight" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.TextElement.FontWeight" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="d" /> is <see langword="null" />.</exception>
		// Token: 0x06003942 RID: 14658 RVA: 0x001039F3 File Offset: 0x00101BF3
		public static FontWeight GetFontWeight(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontWeight)element.GetValue(TextElement.FontWeightProperty);
		}

		/// <summary>Gets or sets the font-stretching characteristics for the content of the element.  </summary>
		/// <returns>A <see cref="T:System.Windows.FontStretch" /> structure that specifies the desired font-stretching characteristics to use. The default is <see cref="P:System.Windows.FontStretches.Normal" />.</returns>
		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x06003943 RID: 14659 RVA: 0x00103A13 File Offset: 0x00101C13
		// (set) Token: 0x06003944 RID: 14660 RVA: 0x00103A25 File Offset: 0x00101C25
		public FontStretch FontStretch
		{
			get
			{
				return (FontStretch)base.GetValue(TextElement.FontStretchProperty);
			}
			set
			{
				base.SetValue(TextElement.FontStretchProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.TextElement.FontStretch" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.TextElement.FontStretch" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="d" /> is <see langword="null" />.</exception>
		// Token: 0x06003945 RID: 14661 RVA: 0x00103A38 File Offset: 0x00101C38
		public static void SetFontStretch(DependencyObject element, FontStretch value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextElement.FontStretchProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.TextElement.FontStretch" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.TextElement.FontStretch" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.TextElement.FontStretch" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="d" /> is <see langword="null" />.</exception>
		// Token: 0x06003946 RID: 14662 RVA: 0x00103A59 File Offset: 0x00101C59
		public static FontStretch GetFontStretch(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontStretch)element.GetValue(TextElement.FontStretchProperty);
		}

		/// <summary>Gets or sets the font size for the content of the element.  </summary>
		/// <returns>The desired font size to use in device independent pixels,  greater than 0.001 and less than or equal to 35791.  The default depends on current system settings and depends on the <see cref="P:System.Windows.SystemFonts.MessageFontSize" /> value.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <see cref="P:System.Windows.Documents.TextElement.FontSize" /> is set to a value greater than 35791 or less than or equal to 0.001.</exception>
		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x06003947 RID: 14663 RVA: 0x00103A79 File Offset: 0x00101C79
		// (set) Token: 0x06003948 RID: 14664 RVA: 0x00103A8B File Offset: 0x00101C8B
		[TypeConverter(typeof(FontSizeConverter))]
		[Localizability(LocalizationCategory.None)]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(TextElement.FontSizeProperty);
			}
			set
			{
				base.SetValue(TextElement.FontSizeProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.TextElement.FontSize" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.TextElement.FontSize" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="d" /> is <see langword="null" />.</exception>
		// Token: 0x06003949 RID: 14665 RVA: 0x00103A9E File Offset: 0x00101C9E
		public static void SetFontSize(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextElement.FontSizeProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.TextElement.FontSize" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.TextElement.FontSize" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.TextElement.FontSize" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="d" /> is <see langword="null" />.</exception>
		// Token: 0x0600394A RID: 14666 RVA: 0x00103ABF File Offset: 0x00101CBF
		[TypeConverter(typeof(FontSizeConverter))]
		public static double GetFontSize(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(TextElement.FontSizeProperty);
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> to apply to the content of the element.  </summary>
		/// <returns>The brush used to apply to the text contents. The default is <see cref="P:System.Windows.Media.Brushes.Black" />.</returns>
		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x0600394B RID: 14667 RVA: 0x00103ADF File Offset: 0x00101CDF
		// (set) Token: 0x0600394C RID: 14668 RVA: 0x00103AF1 File Offset: 0x00101CF1
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(TextElement.ForegroundProperty);
			}
			set
			{
				base.SetValue(TextElement.ForegroundProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.TextElement.Foreground" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to set the value of the <see cref="P:System.Windows.Documents.TextElement.Foreground" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="d" /> is <see langword="null" />.</exception>
		// Token: 0x0600394D RID: 14669 RVA: 0x00103AFF File Offset: 0x00101CFF
		public static void SetForeground(DependencyObject element, Brush value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextElement.ForegroundProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.TextElement.Foreground" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object for which to retrieve the value of the <see cref="P:System.Windows.Documents.TextElement.Foreground" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.TextElement.Foreground" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="d" /> is <see langword="null" />.</exception>
		// Token: 0x0600394E RID: 14670 RVA: 0x00103B1B File Offset: 0x00101D1B
		public static Brush GetForeground(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Brush)element.GetValue(TextElement.ForegroundProperty);
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> used to fill the background of the content area.  </summary>
		/// <returns>The brush used to fill the background of the content area, or <see langword="null" /> to not use a background brush. The default is <see langword="null" />.</returns>
		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x0600394F RID: 14671 RVA: 0x00103B3B File Offset: 0x00101D3B
		// (set) Token: 0x06003950 RID: 14672 RVA: 0x00103B4D File Offset: 0x00101D4D
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(TextElement.BackgroundProperty);
			}
			set
			{
				base.SetValue(TextElement.BackgroundProperty, value);
			}
		}

		/// <summary>Gets or sets a collection of text effects to apply to the content of the element.  </summary>
		/// <returns>A <see cref="T:System.Windows.Media.TextEffectCollection" /> containing one or more <see cref="T:System.Windows.Media.TextEffect" /> objects that define effects to apply to the content in this element. The default is <see langword="null" /> (not an empty collection).</returns>
		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x06003951 RID: 14673 RVA: 0x00103B5B File Offset: 0x00101D5B
		// (set) Token: 0x06003952 RID: 14674 RVA: 0x00103B6D File Offset: 0x00101D6D
		public TextEffectCollection TextEffects
		{
			get
			{
				return (TextEffectCollection)base.GetValue(TextElement.TextEffectsProperty);
			}
			set
			{
				base.SetValue(TextElement.TextEffectsProperty, value);
			}
		}

		/// <summary>Gets the currently effective typography variations for the content of the element.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.Typography" /> object that specifies the currently effective typography variations. For a list of default typography values, see <see cref="T:System.Windows.Documents.Typography" />.</returns>
		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x06003953 RID: 14675 RVA: 0x000D53E2 File Offset: 0x000D35E2
		public Typography Typography
		{
			get
			{
				return new Typography(this);
			}
		}

		/// <summary>Adds a child object. </summary>
		/// <param name="value">The child object to add.</param>
		// Token: 0x06003954 RID: 14676 RVA: 0x00103B7C File Offset: 0x00101D7C
		void IAddChild.AddChild(object value)
		{
			Type type = value.GetType();
			TextElement textElement = value as TextElement;
			if (textElement != null)
			{
				TextSchema.ValidateChild(this, textElement, true, true);
				this.Append(textElement);
				return;
			}
			UIElement uielement = value as UIElement;
			if (uielement == null)
			{
				throw new ArgumentException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
				{
					base.GetType().Name,
					value.GetType().Name
				}));
			}
			InlineUIContainer inlineUIContainer = this as InlineUIContainer;
			if (inlineUIContainer != null)
			{
				if (inlineUIContainer.Child != null)
				{
					throw new ArgumentException(SR.Get("TextSchema_ThisInlineUIContainerHasAChildUIElementAlready", new object[]
					{
						base.GetType().Name,
						((InlineUIContainer)this).Child.GetType().Name,
						value.GetType().Name
					}));
				}
				inlineUIContainer.Child = uielement;
				return;
			}
			else
			{
				BlockUIContainer blockUIContainer = this as BlockUIContainer;
				if (blockUIContainer != null)
				{
					if (blockUIContainer.Child != null)
					{
						throw new ArgumentException(SR.Get("TextSchema_ThisBlockUIContainerHasAChildUIElementAlready", new object[]
						{
							base.GetType().Name,
							((BlockUIContainer)this).Child.GetType().Name,
							value.GetType().Name
						}));
					}
					blockUIContainer.Child = uielement;
					return;
				}
				else
				{
					if (TextSchema.IsValidChild(this, typeof(InlineUIContainer)))
					{
						InlineUIContainer inlineUIContainer2 = Inline.CreateImplicitInlineUIContainer(this);
						this.Append(inlineUIContainer2);
						inlineUIContainer2.Child = uielement;
						return;
					}
					throw new ArgumentException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
					{
						base.GetType().Name,
						value.GetType().Name
					}));
				}
			}
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x06003955 RID: 14677 RVA: 0x00103D14 File Offset: 0x00101F14
		void IAddChild.AddText(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			if (TextSchema.IsValidChild(this, typeof(string)))
			{
				this.Append(text);
				return;
			}
			if (TextSchema.IsValidChild(this, typeof(Run)))
			{
				Run run = Inline.CreateImplicitRun(this);
				this.Append(run);
				run.Text = text;
				return;
			}
			if (text.Trim().Length > 0)
			{
				throw new InvalidOperationException(SR.Get("TextSchema_TextIsNotAllowed", new object[]
				{
					base.GetType().Name
				}));
			}
		}

		/// <summary>Gets an enumerator that can iterate the logical children of the element.</summary>
		/// <returns>An enumerator for the logical children.</returns>
		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x06003956 RID: 14678 RVA: 0x00103DA3 File Offset: 0x00101FA3
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (!this.IsEmpty)
				{
					return new RangeContentEnumerator(this.ContentStart, this.ContentEnd);
				}
				return new RangeContentEnumerator(null, null);
			}
		}

		/// <summary>Handles notifications that one or more of the dependency properties that exist on the element have had their effective values changed. </summary>
		/// <param name="e">Arguments associated with the property value change.  The <see cref="P:System.Windows.DependencyPropertyChangedEventArgs.Property" /> property specifies which property has changed, the <see cref="P:System.Windows.DependencyPropertyChangedEventArgs.OldValue" /> property specifies the previous property value, and the <see cref="P:System.Windows.DependencyPropertyChangedEventArgs.NewValue" /> property specifies the new property value.</param>
		// Token: 0x06003957 RID: 14679 RVA: 0x00103DC8 File Offset: 0x00101FC8
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			bool flag = e.NewValueSource == BaseValueSourceInternal.Local || e.OldValueSource == BaseValueSourceInternal.Local;
			if ((flag || e.IsAValueChange || e.IsASubPropertyChange) && this.IsInTree)
			{
				FrameworkPropertyMetadata frameworkPropertyMetadata = e.Metadata as FrameworkPropertyMetadata;
				if (frameworkPropertyMetadata != null)
				{
					bool flag2 = frameworkPropertyMetadata.AffectsMeasure || frameworkPropertyMetadata.AffectsArrange || frameworkPropertyMetadata.AffectsParentMeasure || frameworkPropertyMetadata.AffectsParentArrange;
					bool flag3 = frameworkPropertyMetadata.AffectsRender && (e.IsAValueChange || !frameworkPropertyMetadata.SubPropertiesDoNotAffectRender);
					if (flag2 || flag3)
					{
						TextContainer textContainer = this.EnsureTextContainer();
						textContainer.BeginChange();
						try
						{
							if (flag)
							{
								TextTreeUndo.CreatePropertyUndoUnit(this, e);
							}
							if (e.IsAValueChange || e.IsASubPropertyChange)
							{
								this.NotifyTypographicPropertyChanged(flag2, flag, e.Property);
							}
						}
						finally
						{
							textContainer.EndChange();
						}
					}
				}
			}
		}

		// Token: 0x06003958 RID: 14680 RVA: 0x00103EC8 File Offset: 0x001020C8
		internal void NotifyTypographicPropertyChanged(bool affectsMeasureOrArrange, bool localValueChanged, DependencyProperty property)
		{
			if (!this.IsInTree)
			{
				return;
			}
			TextContainer textContainer = this.EnsureTextContainer();
			textContainer.NextLayoutGeneration();
			if (textContainer.HasListeners)
			{
				TextPointer textPointer = new TextPointer(textContainer, this._textElementNode, ElementEdge.BeforeStart, LogicalDirection.Forward);
				textPointer.Freeze();
				textContainer.BeginChange();
				try
				{
					textContainer.BeforeAddChange();
					if (localValueChanged)
					{
						textContainer.AddLocalValueChange();
					}
					textContainer.AddChange(textPointer, this._textElementNode.SymbolCount, this._textElementNode.IMECharCount, PrecursorTextChangeType.PropertyModified, property, !affectsMeasureOrArrange);
				}
				finally
				{
					textContainer.EndChange();
				}
			}
		}

		// Token: 0x06003959 RID: 14681 RVA: 0x00103F5C File Offset: 0x0010215C
		internal static TypographyProperties GetTypographyProperties(DependencyObject element)
		{
			TypographyProperties typographyProperties = new TypographyProperties();
			typographyProperties.SetStandardLigatures((bool)element.GetValue(Typography.StandardLigaturesProperty));
			typographyProperties.SetContextualLigatures((bool)element.GetValue(Typography.ContextualLigaturesProperty));
			typographyProperties.SetDiscretionaryLigatures((bool)element.GetValue(Typography.DiscretionaryLigaturesProperty));
			typographyProperties.SetHistoricalLigatures((bool)element.GetValue(Typography.HistoricalLigaturesProperty));
			typographyProperties.SetAnnotationAlternates((int)element.GetValue(Typography.AnnotationAlternatesProperty));
			typographyProperties.SetContextualAlternates((bool)element.GetValue(Typography.ContextualAlternatesProperty));
			typographyProperties.SetHistoricalForms((bool)element.GetValue(Typography.HistoricalFormsProperty));
			typographyProperties.SetKerning((bool)element.GetValue(Typography.KerningProperty));
			typographyProperties.SetCapitalSpacing((bool)element.GetValue(Typography.CapitalSpacingProperty));
			typographyProperties.SetCaseSensitiveForms((bool)element.GetValue(Typography.CaseSensitiveFormsProperty));
			typographyProperties.SetStylisticSet1((bool)element.GetValue(Typography.StylisticSet1Property));
			typographyProperties.SetStylisticSet2((bool)element.GetValue(Typography.StylisticSet2Property));
			typographyProperties.SetStylisticSet3((bool)element.GetValue(Typography.StylisticSet3Property));
			typographyProperties.SetStylisticSet4((bool)element.GetValue(Typography.StylisticSet4Property));
			typographyProperties.SetStylisticSet5((bool)element.GetValue(Typography.StylisticSet5Property));
			typographyProperties.SetStylisticSet6((bool)element.GetValue(Typography.StylisticSet6Property));
			typographyProperties.SetStylisticSet7((bool)element.GetValue(Typography.StylisticSet7Property));
			typographyProperties.SetStylisticSet8((bool)element.GetValue(Typography.StylisticSet8Property));
			typographyProperties.SetStylisticSet9((bool)element.GetValue(Typography.StylisticSet9Property));
			typographyProperties.SetStylisticSet10((bool)element.GetValue(Typography.StylisticSet10Property));
			typographyProperties.SetStylisticSet11((bool)element.GetValue(Typography.StylisticSet11Property));
			typographyProperties.SetStylisticSet12((bool)element.GetValue(Typography.StylisticSet12Property));
			typographyProperties.SetStylisticSet13((bool)element.GetValue(Typography.StylisticSet13Property));
			typographyProperties.SetStylisticSet14((bool)element.GetValue(Typography.StylisticSet14Property));
			typographyProperties.SetStylisticSet15((bool)element.GetValue(Typography.StylisticSet15Property));
			typographyProperties.SetStylisticSet16((bool)element.GetValue(Typography.StylisticSet16Property));
			typographyProperties.SetStylisticSet17((bool)element.GetValue(Typography.StylisticSet17Property));
			typographyProperties.SetStylisticSet18((bool)element.GetValue(Typography.StylisticSet18Property));
			typographyProperties.SetStylisticSet19((bool)element.GetValue(Typography.StylisticSet19Property));
			typographyProperties.SetStylisticSet20((bool)element.GetValue(Typography.StylisticSet20Property));
			typographyProperties.SetFraction((FontFraction)element.GetValue(Typography.FractionProperty));
			typographyProperties.SetSlashedZero((bool)element.GetValue(Typography.SlashedZeroProperty));
			typographyProperties.SetMathematicalGreek((bool)element.GetValue(Typography.MathematicalGreekProperty));
			typographyProperties.SetEastAsianExpertForms((bool)element.GetValue(Typography.EastAsianExpertFormsProperty));
			typographyProperties.SetVariants((FontVariants)element.GetValue(Typography.VariantsProperty));
			typographyProperties.SetCapitals((FontCapitals)element.GetValue(Typography.CapitalsProperty));
			typographyProperties.SetNumeralStyle((FontNumeralStyle)element.GetValue(Typography.NumeralStyleProperty));
			typographyProperties.SetNumeralAlignment((FontNumeralAlignment)element.GetValue(Typography.NumeralAlignmentProperty));
			typographyProperties.SetEastAsianWidths((FontEastAsianWidths)element.GetValue(Typography.EastAsianWidthsProperty));
			typographyProperties.SetEastAsianLanguage((FontEastAsianLanguage)element.GetValue(Typography.EastAsianLanguageProperty));
			typographyProperties.SetStandardSwashes((int)element.GetValue(Typography.StandardSwashesProperty));
			typographyProperties.SetContextualSwashes((int)element.GetValue(Typography.ContextualSwashesProperty));
			typographyProperties.SetStylisticAlternates((int)element.GetValue(Typography.StylisticAlternatesProperty));
			return typographyProperties;
		}

		// Token: 0x0600395A RID: 14682 RVA: 0x00104324 File Offset: 0x00102524
		internal void DeepEndInit()
		{
			if (!base.IsInitialized)
			{
				if (!this.IsEmpty)
				{
					IEnumerator logicalChildren = this.LogicalChildren;
					while (logicalChildren.MoveNext())
					{
						object obj = logicalChildren.Current;
						TextElement textElement = obj as TextElement;
						if (textElement != null)
						{
							textElement.DeepEndInit();
						}
					}
				}
				this.EndInit();
				Invariant.Assert(base.IsInitialized);
			}
		}

		// Token: 0x0600395B RID: 14683 RVA: 0x00104378 File Offset: 0x00102578
		internal static TextElement GetCommonAncestor(TextElement element1, TextElement element2)
		{
			if (element1 != element2)
			{
				int i = 0;
				int j = 0;
				TextElement textElement = element1;
				while (textElement.Parent is TextElement)
				{
					i++;
					textElement = (TextElement)textElement.Parent;
				}
				textElement = element2;
				while (textElement.Parent is TextElement)
				{
					j++;
					textElement = (TextElement)textElement.Parent;
				}
				while (i > j)
				{
					if (element1 == element2)
					{
						break;
					}
					element1 = (TextElement)element1.Parent;
					i--;
				}
				while (j > i)
				{
					if (element1 == element2)
					{
						break;
					}
					element2 = (TextElement)element2.Parent;
					j--;
				}
				while (element1 != element2)
				{
					element1 = (element1.Parent as TextElement);
					element2 = (element2.Parent as TextElement);
				}
			}
			Invariant.Assert(element1 == element2);
			return element1;
		}

		// Token: 0x0600395C RID: 14684 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void OnTextUpdated()
		{
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void BeforeLogicalTreeChange()
		{
		}

		// Token: 0x0600395E RID: 14686 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void AfterLogicalTreeChange()
		{
		}

		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x0600395F RID: 14687 RVA: 0x00104432 File Offset: 0x00102632
		internal TextContainer TextContainer
		{
			get
			{
				return this.EnsureTextContainer();
			}
		}

		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x06003960 RID: 14688 RVA: 0x0010443A File Offset: 0x0010263A
		internal bool IsEmpty
		{
			get
			{
				return this._textElementNode == null || this._textElementNode.ContainedNode == null;
			}
		}

		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x06003961 RID: 14689 RVA: 0x00104454 File Offset: 0x00102654
		internal bool IsInTree
		{
			get
			{
				return this._textElementNode != null;
			}
		}

		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x06003962 RID: 14690 RVA: 0x0010445F File Offset: 0x0010265F
		internal int ElementStartOffset
		{
			get
			{
				Invariant.Assert(this.IsInTree, "TextElement is not in any TextContainer, caller should ensure this.");
				return this._textElementNode.GetSymbolOffset(this.EnsureTextContainer().Generation) - 1;
			}
		}

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x06003963 RID: 14691 RVA: 0x00104489 File Offset: 0x00102689
		internal int ContentStartOffset
		{
			get
			{
				Invariant.Assert(this.IsInTree, "TextElement is not in any TextContainer, caller should ensure this.");
				return this._textElementNode.GetSymbolOffset(this.EnsureTextContainer().Generation);
			}
		}

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x06003964 RID: 14692 RVA: 0x001044B1 File Offset: 0x001026B1
		internal int ContentEndOffset
		{
			get
			{
				Invariant.Assert(this.IsInTree, "TextElement is not in any TextContainer, caller should ensure this.");
				return this._textElementNode.GetSymbolOffset(this.EnsureTextContainer().Generation) + this._textElementNode.SymbolCount - 2;
			}
		}

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x06003965 RID: 14693 RVA: 0x001044E7 File Offset: 0x001026E7
		internal int ElementEndOffset
		{
			get
			{
				Invariant.Assert(this.IsInTree, "TextElement is not in any TextContainer, caller should ensure this.");
				return this._textElementNode.GetSymbolOffset(this.EnsureTextContainer().Generation) + this._textElementNode.SymbolCount - 1;
			}
		}

		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x06003966 RID: 14694 RVA: 0x0010451D File Offset: 0x0010271D
		internal int SymbolCount
		{
			get
			{
				if (!this.IsInTree)
				{
					return 2;
				}
				return this._textElementNode.SymbolCount;
			}
		}

		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x06003967 RID: 14695 RVA: 0x00104534 File Offset: 0x00102734
		// (set) Token: 0x06003968 RID: 14696 RVA: 0x0010453C File Offset: 0x0010273C
		internal TextTreeTextElementNode TextElementNode
		{
			get
			{
				return this._textElementNode;
			}
			set
			{
				this._textElementNode = value;
			}
		}

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x06003969 RID: 14697 RVA: 0x00104545 File Offset: 0x00102745
		internal TypographyProperties TypographyPropertiesGroup
		{
			get
			{
				if (this._typographyPropertiesGroup == null)
				{
					this._typographyPropertiesGroup = TextElement.GetTypographyProperties(this);
				}
				return this._typographyPropertiesGroup;
			}
		}

		// Token: 0x0600396A RID: 14698 RVA: 0x00104567 File Offset: 0x00102767
		private static void OnTypographyChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
		{
			((TextElement)element)._typographyPropertiesGroup = null;
		}

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x0600396B RID: 14699 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool IsIMEStructuralElement
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x0600396C RID: 14700 RVA: 0x00104578 File Offset: 0x00102778
		internal int IMELeftEdgeCharCount
		{
			get
			{
				int result = 0;
				if (this.IsIMEStructuralElement)
				{
					if (!this.IsInTree)
					{
						result = -1;
					}
					else
					{
						result = (this.TextElementNode.IsFirstSibling ? 0 : 1);
					}
				}
				return result;
			}
		}

		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x0600396D RID: 14701 RVA: 0x001045B0 File Offset: 0x001027B0
		internal virtual bool IsFirstIMEVisibleSibling
		{
			get
			{
				bool result = false;
				if (this.IsIMEStructuralElement)
				{
					result = (this.TextElementNode == null || this.TextElementNode.IsFirstSibling);
				}
				return result;
			}
		}

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x0600396E RID: 14702 RVA: 0x001045E0 File Offset: 0x001027E0
		internal TextElement NextElement
		{
			get
			{
				if (!this.IsInTree)
				{
					return null;
				}
				TextTreeTextElementNode textTreeTextElementNode = this._textElementNode.GetNextNode() as TextTreeTextElementNode;
				if (textTreeTextElementNode == null)
				{
					return null;
				}
				return textTreeTextElementNode.TextElement;
			}
		}

		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x0600396F RID: 14703 RVA: 0x00104614 File Offset: 0x00102814
		internal TextElement PreviousElement
		{
			get
			{
				if (!this.IsInTree)
				{
					return null;
				}
				TextTreeTextElementNode textTreeTextElementNode = this._textElementNode.GetPreviousNode() as TextTreeTextElementNode;
				if (textTreeTextElementNode == null)
				{
					return null;
				}
				return textTreeTextElementNode.TextElement;
			}
		}

		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x06003970 RID: 14704 RVA: 0x00104648 File Offset: 0x00102848
		internal TextElement FirstChildElement
		{
			get
			{
				if (!this.IsInTree)
				{
					return null;
				}
				TextTreeTextElementNode textTreeTextElementNode = this._textElementNode.GetFirstContainedNode() as TextTreeTextElementNode;
				if (textTreeTextElementNode == null)
				{
					return null;
				}
				return textTreeTextElementNode.TextElement;
			}
		}

		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x06003971 RID: 14705 RVA: 0x0010467C File Offset: 0x0010287C
		internal TextElement LastChildElement
		{
			get
			{
				if (!this.IsInTree)
				{
					return null;
				}
				TextTreeTextElementNode textTreeTextElementNode = this._textElementNode.GetLastContainedNode() as TextTreeTextElementNode;
				if (textTreeTextElementNode == null)
				{
					return null;
				}
				return textTreeTextElementNode.TextElement;
			}
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x001046B0 File Offset: 0x001028B0
		private void Append(string textData)
		{
			if (textData == null)
			{
				throw new ArgumentNullException("textData");
			}
			TextContainer textContainer = this.EnsureTextContainer();
			textContainer.BeginChange();
			try
			{
				textContainer.InsertTextInternal(new TextPointer(textContainer, this._textElementNode, ElementEdge.BeforeEnd), textData);
			}
			finally
			{
				textContainer.EndChange();
			}
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x00104708 File Offset: 0x00102908
		private void Append(TextElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			TextContainer textContainer = this.EnsureTextContainer();
			textContainer.BeginChange();
			try
			{
				TextPointer textPointer = new TextPointer(textContainer, this._textElementNode, ElementEdge.BeforeEnd);
				textContainer.InsertElementInternal(textPointer, textPointer, element);
			}
			finally
			{
				textContainer.EndChange();
			}
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x00104760 File Offset: 0x00102960
		private TextContainer EnsureTextContainer()
		{
			TextContainer textContainer;
			if (this.IsInTree)
			{
				textContainer = this._textElementNode.GetTextTree();
				textContainer.EmptyDeadPositionList();
			}
			else
			{
				textContainer = new TextContainer(null, false);
				TextPointer start = textContainer.Start;
				textContainer.BeginChange();
				try
				{
					textContainer.InsertElementInternal(start, start, this);
				}
				finally
				{
					textContainer.EndChange();
				}
				Invariant.Assert(this.IsInTree);
			}
			return textContainer;
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x001047CC File Offset: 0x001029CC
		private static bool IsValidFontFamily(object o)
		{
			FontFamily fontFamily = o as FontFamily;
			return fontFamily != null;
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x001047E4 File Offset: 0x001029E4
		private static bool IsValidFontSize(object value)
		{
			double num = (double)value;
			double minWidth = TextDpi.MinWidth;
			double num2 = (double)Math.Min(1000000, 160000);
			return !double.IsNaN(num) && num >= minWidth && num <= num2;
		}

		// Token: 0x040025A3 RID: 9635
		internal static readonly UncommonField<TextElement> ContainerTextElementField = new UncommonField<TextElement>();

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TextElement.FontFamily" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TextElement.FontFamily" /> dependency property.</returns>
		// Token: 0x040025A4 RID: 9636
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.RegisterAttached("FontFamily", typeof(FontFamily), typeof(TextElement), new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits), new ValidateValueCallback(TextElement.IsValidFontFamily));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TextElement.FontStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TextElement.FontStyle" /> dependency property.</returns>
		// Token: 0x040025A5 RID: 9637
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontStyleProperty = DependencyProperty.RegisterAttached("FontStyle", typeof(FontStyle), typeof(TextElement), new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TextElement.FontWeight" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TextElement.FontWeight" /> dependency property.</returns>
		// Token: 0x040025A6 RID: 9638
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontWeightProperty = DependencyProperty.RegisterAttached("FontWeight", typeof(FontWeight), typeof(TextElement), new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TextElement.FontStretch" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TextElement.FontStretch" /> dependency property.</returns>
		// Token: 0x040025A7 RID: 9639
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontStretchProperty = DependencyProperty.RegisterAttached("FontStretch", typeof(FontStretch), typeof(TextElement), new FrameworkPropertyMetadata(FontStretches.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TextElement.FontSize" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TextElement.FontSize" /> dependency property.</returns>
		// Token: 0x040025A8 RID: 9640
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontSizeProperty = DependencyProperty.RegisterAttached("FontSize", typeof(double), typeof(TextElement), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits), new ValidateValueCallback(TextElement.IsValidFontSize));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TextElement.Foreground" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TextElement.Foreground" /> dependency property.</returns>
		// Token: 0x040025A9 RID: 9641
		[CommonDependencyProperty]
		public static readonly DependencyProperty ForegroundProperty = DependencyProperty.RegisterAttached("Foreground", typeof(Brush), typeof(TextElement), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TextElement.Background" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TextElement.Background" /> dependency property.</returns>
		// Token: 0x040025AA RID: 9642
		[CommonDependencyProperty]
		public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(TextElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TextElement.TextEffects" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TextElement.TextEffects" /> dependency property.</returns>
		// Token: 0x040025AB RID: 9643
		public static readonly DependencyProperty TextEffectsProperty = DependencyProperty.Register("TextEffects", typeof(TextEffectCollection), typeof(TextElement), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextEffectCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x040025AC RID: 9644
		private TextTreeTextElementNode _textElementNode;

		// Token: 0x040025AD RID: 9645
		private TypographyProperties _typographyPropertiesGroup = Typography.Default;
	}
}
