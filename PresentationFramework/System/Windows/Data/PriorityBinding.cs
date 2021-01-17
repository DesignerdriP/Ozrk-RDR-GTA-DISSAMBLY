using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal.Data;

namespace System.Windows.Data
{
	/// <summary>Describes a collection of <see cref="T:System.Windows.Data.Binding" /> objects that is attached to a single binding target property, which receives its value from the first binding in the collection that produces a value successfully.</summary>
	// Token: 0x020001B7 RID: 439
	[ContentProperty("Bindings")]
	public class PriorityBinding : BindingBase, IAddChild
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.PriorityBinding" /> class.</summary>
		// Token: 0x06001C5F RID: 7263 RVA: 0x000859F4 File Offset: 0x00083BF4
		public PriorityBinding()
		{
			this._bindingCollection = new BindingCollection(this, new BindingCollectionChangedCallback(this.OnBindingCollectionChanged));
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value">An object to add as a child.</param>
		// Token: 0x06001C60 RID: 7264 RVA: 0x00085A14 File Offset: 0x00083C14
		void IAddChild.AddChild(object value)
		{
			BindingBase bindingBase = value as BindingBase;
			if (bindingBase != null)
			{
				this.Bindings.Add(bindingBase);
				return;
			}
			throw new ArgumentException(SR.Get("ChildHasWrongType", new object[]
			{
				base.GetType().Name,
				"BindingBase",
				value.GetType().FullName
			}), "value");
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="text">A string to add to the object.</param>
		// Token: 0x06001C61 RID: 7265 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Gets the collection of <see cref="T:System.Windows.Data.Binding" /> objects that is established for this instance of <see cref="T:System.Windows.Data.PriorityBinding" />.</summary>
		/// <returns>A collection of <see cref="T:System.Windows.Data.Binding" /> objects. <see cref="T:System.Windows.Data.PriorityBinding" /> currently supports only objects of type <see cref="T:System.Windows.Data.Binding" /> and not <see cref="T:System.Windows.Data.MultiBinding" /> or <see cref="T:System.Windows.Data.PriorityBinding" />. Adding a <see cref="T:System.Windows.Data.Binding" /> child to a <see cref="T:System.Windows.Data.PriorityBinding" /> object implicitly adds the child to the <see cref="T:System.Windows.Data.BindingBase" /> collection for the <see cref="T:System.Windows.Data.MultiBinding" /> object. The default is an empty collection.</returns>
		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06001C62 RID: 7266 RVA: 0x00085A76 File Offset: 0x00083C76
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Collection<BindingBase> Bindings
		{
			get
			{
				return this._bindingCollection;
			}
		}

		/// <summary>Returns a value that indicates whether serialization processes should serialize the effective value of the <see cref="P:System.Windows.Data.PriorityBinding.Bindings" /> property on instances of this class.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Data.PriorityBinding.Bindings" /> property value should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001C63 RID: 7267 RVA: 0x00085A7E File Offset: 0x00083C7E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBindings()
		{
			return this.Bindings != null && this.Bindings.Count > 0;
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x00085A98 File Offset: 0x00083C98
		internal override BindingExpressionBase CreateBindingExpressionOverride(DependencyObject target, DependencyProperty dp, BindingExpressionBase owner)
		{
			return PriorityBindingExpression.CreateBindingExpression(target, dp, this, owner);
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x00085AA3 File Offset: 0x00083CA3
		internal override BindingBase CreateClone()
		{
			return new PriorityBinding();
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x00085AAC File Offset: 0x00083CAC
		internal override void InitializeClone(BindingBase baseClone, BindingMode mode)
		{
			PriorityBinding priorityBinding = (PriorityBinding)baseClone;
			for (int i = 0; i <= this._bindingCollection.Count; i++)
			{
				priorityBinding._bindingCollection.Add(this._bindingCollection[i].Clone(mode));
			}
			base.InitializeClone(baseClone, mode);
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x00083E93 File Offset: 0x00082093
		private void OnBindingCollectionChanged()
		{
			base.CheckSealed();
		}

		// Token: 0x040013D3 RID: 5075
		private BindingCollection _bindingCollection;
	}
}
