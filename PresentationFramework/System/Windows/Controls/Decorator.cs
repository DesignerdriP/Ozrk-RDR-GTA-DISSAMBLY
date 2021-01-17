﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal.Controls;

namespace System.Windows.Controls
{
	/// <summary>Provides a base class for elements that apply effects onto or around a single child element, such as <see cref="T:System.Windows.Controls.Border" /> or <see cref="T:System.Windows.Controls.Viewbox" />.</summary>
	// Token: 0x020004C5 RID: 1221
	[Localizability(LocalizationCategory.Ignore, Readability = Readability.Unreadable)]
	[ContentProperty("Child")]
	public class Decorator : FrameworkElement, IAddChild
	{
		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value"> An object to add as a child.</param>
		// Token: 0x06004A3D RID: 19005 RVA: 0x0014F728 File Offset: 0x0014D928
		void IAddChild.AddChild(object value)
		{
			if (!(value is UIElement))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(UIElement)
				}), "value");
			}
			if (this.Child != null)
			{
				throw new ArgumentException(SR.Get("CanOnlyHaveOneChild", new object[]
				{
					base.GetType(),
					value.GetType()
				}));
			}
			this.Child = (UIElement)value;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="text"> A string to add to the object.</param>
		// Token: 0x06004A3E RID: 19006 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Gets or sets the single child element of a <see cref="T:System.Windows.Controls.Decorator" />.</summary>
		/// <returns>The single child element of a <see cref="T:System.Windows.Controls.Decorator" />.</returns>
		// Token: 0x17001218 RID: 4632
		// (get) Token: 0x06004A3F RID: 19007 RVA: 0x0014F7AA File Offset: 0x0014D9AA
		// (set) Token: 0x06004A40 RID: 19008 RVA: 0x0014F7B2 File Offset: 0x0014D9B2
		[DefaultValue(null)]
		public virtual UIElement Child
		{
			get
			{
				return this._child;
			}
			set
			{
				if (this._child != value)
				{
					base.RemoveVisualChild(this._child);
					base.RemoveLogicalChild(this._child);
					this._child = value;
					base.AddLogicalChild(value);
					base.AddVisualChild(value);
					base.InvalidateMeasure();
				}
			}
		}

		/// <summary>Gets an enumerator that can be used to iterate the logical child elements of a <see cref="T:System.Windows.Controls.Decorator" />.</summary>
		/// <returns>An enumerator that can be used to iterate the logical child elements of a <see cref="T:System.Windows.Controls.Decorator" />.</returns>
		// Token: 0x17001219 RID: 4633
		// (get) Token: 0x06004A41 RID: 19009 RVA: 0x0014F7F0 File Offset: 0x0014D9F0
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this._child == null)
				{
					return EmptyEnumerator.Instance;
				}
				return new SingleChildEnumerator(this._child);
			}
		}

		/// <summary>Gets a value that is equal to the number of visual child elements of this instance of <see cref="T:System.Windows.Controls.Decorator" />.</summary>
		/// <returns>The number of visual child elements.</returns>
		// Token: 0x1700121A RID: 4634
		// (get) Token: 0x06004A42 RID: 19010 RVA: 0x0014F80B File Offset: 0x0014DA0B
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._child != null)
				{
					return 1;
				}
				return 0;
			}
		}

		/// <summary>Gets the child <see cref="T:System.Windows.Media.Visual" /> element at the specified <paramref name="index" /> position.</summary>
		/// <param name="index">Index position of the child element.</param>
		/// <returns>The child element at the specified <paramref name="index" /> position.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is greater than the number of visual child elements.</exception>
		// Token: 0x06004A43 RID: 19011 RVA: 0x0014F818 File Offset: 0x0014DA18
		protected override Visual GetVisualChild(int index)
		{
			if (this._child == null || index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._child;
		}

		/// <summary>Measures the child element of a <see cref="T:System.Windows.Controls.Decorator" /> to prepare for arranging it during the <see cref="M:System.Windows.Controls.Decorator.ArrangeOverride(System.Windows.Size)" /> pass.</summary>
		/// <param name="constraint">An upper limit <see cref="T:System.Windows.Size" /> that should not be exceeded.</param>
		/// <returns>The target <see cref="T:System.Windows.Size" /> of the element.</returns>
		// Token: 0x06004A44 RID: 19012 RVA: 0x0014F848 File Offset: 0x0014DA48
		protected override Size MeasureOverride(Size constraint)
		{
			UIElement child = this.Child;
			if (child != null)
			{
				child.Measure(constraint);
				return child.DesiredSize;
			}
			return default(Size);
		}

		/// <summary>Arranges the content of a <see cref="T:System.Windows.Controls.Decorator" /> element.</summary>
		/// <param name="arrangeSize">The <see cref="T:System.Windows.Size" /> this element uses to arrange its child content.</param>
		/// <returns>The <see cref="T:System.Windows.Size" /> that represents the arranged size of this <see cref="T:System.Windows.Controls.Decorator" /> element and its child.</returns>
		// Token: 0x06004A45 RID: 19013 RVA: 0x0014F878 File Offset: 0x0014DA78
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			UIElement child = this.Child;
			if (child != null)
			{
				child.Arrange(new Rect(arrangeSize));
			}
			return arrangeSize;
		}

		// Token: 0x1700121B RID: 4635
		// (get) Token: 0x06004A46 RID: 19014 RVA: 0x0014F7AA File Offset: 0x0014D9AA
		// (set) Token: 0x06004A47 RID: 19015 RVA: 0x0014F89C File Offset: 0x0014DA9C
		internal UIElement IntChild
		{
			get
			{
				return this._child;
			}
			set
			{
				this._child = value;
			}
		}

		// Token: 0x04002A4F RID: 10831
		private UIElement _child;
	}
}
