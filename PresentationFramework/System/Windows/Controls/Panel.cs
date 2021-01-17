﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal.Controls;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Provides a base class for all <see cref="T:System.Windows.Controls.Panel" /> elements. Use <see cref="T:System.Windows.Controls.Panel" /> elements to position and arrange child objects in Windows Presentation Foundation (WPF) applications. </summary>
	// Token: 0x0200050D RID: 1293
	[Localizability(LocalizationCategory.Ignore)]
	[ContentProperty("Children")]
	public abstract class Panel : FrameworkElement, IAddChild
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Panel" /> class.</summary>
		// Token: 0x060052DB RID: 21211 RVA: 0x00171353 File Offset: 0x0016F553
		protected Panel()
		{
			this._zConsonant = (int)Panel.ZIndexProperty.GetDefaultValue(base.DependencyObjectType);
		}

		/// <summary>Draws the content of a <see cref="T:System.Windows.Media.DrawingContext" /> object during the render pass of a <see cref="T:System.Windows.Controls.Panel" /> element. </summary>
		/// <param name="dc">The <see cref="T:System.Windows.Media.DrawingContext" /> object to draw.</param>
		// Token: 0x060052DC RID: 21212 RVA: 0x00171378 File Offset: 0x0016F578
		protected override void OnRender(DrawingContext dc)
		{
			Brush background = this.Background;
			if (background != null)
			{
				Size renderSize = base.RenderSize;
				dc.DrawRectangle(background, null, new Rect(0.0, 0.0, renderSize.Width, renderSize.Height));
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value"> An object to add as a child.</param>
		// Token: 0x060052DD RID: 21213 RVA: 0x001713C4 File Offset: 0x0016F5C4
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.IsItemsHost)
			{
				throw new InvalidOperationException(SR.Get("Panel_BoundPanel_NoChildren"));
			}
			UIElement uielement = value as UIElement;
			if (uielement == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(UIElement)
				}), "value");
			}
			this.Children.Add(uielement);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="text"> A string to add to the object.</param>
		// Token: 0x060052DE RID: 21214 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.Brush" /> that is used to fill the area between the borders of a <see cref="T:System.Windows.Controls.Panel" />.  </summary>
		/// <returns>A <see cref="T:System.Windows.Media.Brush" />. This default value is <see langword="null" />.</returns>
		// Token: 0x1700141B RID: 5147
		// (get) Token: 0x060052DF RID: 21215 RVA: 0x0017143F File Offset: 0x0016F63F
		// (set) Token: 0x060052E0 RID: 21216 RVA: 0x00171451 File Offset: 0x0016F651
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(Panel.BackgroundProperty);
			}
			set
			{
				base.SetValue(Panel.BackgroundProperty, value);
			}
		}

		/// <summary>Gets an enumerator that can iterate the logical child elements of this <see cref="T:System.Windows.Controls.Panel" /> element. </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" />. This property has no default value.</returns>
		// Token: 0x1700141C RID: 5148
		// (get) Token: 0x060052E1 RID: 21217 RVA: 0x0017145F File Offset: 0x0016F65F
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this.VisualChildrenCount == 0 || this.IsItemsHost)
				{
					return EmptyEnumerator.Instance;
				}
				return this.Children.GetEnumerator();
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Controls.UIElementCollection" /> of child elements of this <see cref="T:System.Windows.Controls.Panel" />. </summary>
		/// <returns>A <see cref="T:System.Windows.Controls.UIElementCollection" />. The default is an empty <see cref="T:System.Windows.Controls.UIElementCollection" />.</returns>
		// Token: 0x1700141D RID: 5149
		// (get) Token: 0x060052E2 RID: 21218 RVA: 0x00171482 File Offset: 0x0016F682
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public UIElementCollection Children
		{
			get
			{
				return this.InternalChildren;
			}
		}

		/// <summary>Determines whether the <see cref="P:System.Windows.Controls.Panel.Children" /> collection of a panel should be serialized.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.Panel.Children" /> collection should be serialized; otherwise, <see langword="false" />. The <see cref="P:System.Windows.Controls.Panel.Children" /> collection is only serialized if it is not empty and not <see langword="null" />.</returns>
		// Token: 0x060052E3 RID: 21219 RVA: 0x0017148A File Offset: 0x0016F68A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeChildren()
		{
			return !this.IsItemsHost && this.Children != null && this.Children.Count > 0;
		}

		/// <summary>Gets or sets a value that indicates that this <see cref="T:System.Windows.Controls.Panel" /> is a container for user interface (UI) items that are generated by an <see cref="T:System.Windows.Controls.ItemsControl" />.  </summary>
		/// <returns>
		///     <see langword="true" /> if this instance of <see cref="T:System.Windows.Controls.Panel" /> is an items host; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x1700141E RID: 5150
		// (get) Token: 0x060052E4 RID: 21220 RVA: 0x001714AD File Offset: 0x0016F6AD
		// (set) Token: 0x060052E5 RID: 21221 RVA: 0x001714BF File Offset: 0x0016F6BF
		[Bindable(false)]
		[Category("Behavior")]
		public bool IsItemsHost
		{
			get
			{
				return (bool)base.GetValue(Panel.IsItemsHostProperty);
			}
			set
			{
				base.SetValue(Panel.IsItemsHostProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x060052E6 RID: 21222 RVA: 0x001714D4 File Offset: 0x0016F6D4
		private static void OnIsItemsHostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Panel panel = (Panel)d;
			panel.OnIsItemsHostChanged((bool)e.OldValue, (bool)e.NewValue);
		}

		/// <summary>Indicates that the <see cref="P:System.Windows.Controls.Panel.IsItemsHost" /> property value has changed.</summary>
		/// <param name="oldIsItemsHost">The old property value.</param>
		/// <param name="newIsItemsHost">The new property value.</param>
		// Token: 0x060052E7 RID: 21223 RVA: 0x00171508 File Offset: 0x0016F708
		protected virtual void OnIsItemsHostChanged(bool oldIsItemsHost, bool newIsItemsHost)
		{
			DependencyObject itemsOwnerInternal = ItemsControl.GetItemsOwnerInternal(this);
			ItemsControl itemsControl = itemsOwnerInternal as ItemsControl;
			Panel panel = null;
			if (itemsControl != null)
			{
				IItemContainerGenerator itemContainerGenerator = itemsControl.ItemContainerGenerator;
				if (itemContainerGenerator != null && itemContainerGenerator == itemContainerGenerator.GetItemContainerGeneratorForPanel(this))
				{
					panel = itemsControl.ItemsHost;
					itemsControl.ItemsHost = this;
				}
			}
			else
			{
				GroupItem groupItem = itemsOwnerInternal as GroupItem;
				if (groupItem != null)
				{
					IItemContainerGenerator generator = groupItem.Generator;
					if (generator != null && generator == generator.GetItemContainerGeneratorForPanel(this))
					{
						panel = groupItem.ItemsHost;
						groupItem.ItemsHost = this;
					}
				}
			}
			if (panel != null && panel != this)
			{
				panel.VerifyBoundState();
			}
			this.VerifyBoundState();
		}

		/// <summary>The <see cref="T:System.Windows.Controls.Orientation" /> of the panel, if the panel supports layout in only a single dimension.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.Orientation" /> of the panel. This property has no default value.</returns>
		// Token: 0x1700141F RID: 5151
		// (get) Token: 0x060052E8 RID: 21224 RVA: 0x00171596 File Offset: 0x0016F796
		public Orientation LogicalOrientationPublic
		{
			get
			{
				return this.LogicalOrientation;
			}
		}

		/// <summary>The <see cref="T:System.Windows.Controls.Orientation" /> of the panel, if the panel supports layout in only a single dimension.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.Orientation" /> of the panel. This property has no default value.</returns>
		// Token: 0x17001420 RID: 5152
		// (get) Token: 0x060052E9 RID: 21225 RVA: 0x00016748 File Offset: 0x00014948
		protected internal virtual Orientation LogicalOrientation
		{
			get
			{
				return Orientation.Vertical;
			}
		}

		/// <summary>Gets a value that indicates whether this <see cref="T:System.Windows.Controls.Panel" /> arranges its descendants in a single dimension.</summary>
		/// <returns>
		///     <see langword="true" /> if the orientation of the <see cref="T:System.Windows.Controls.Panel" /> is in one dimension; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001421 RID: 5153
		// (get) Token: 0x060052EA RID: 21226 RVA: 0x0017159E File Offset: 0x0016F79E
		public bool HasLogicalOrientationPublic
		{
			get
			{
				return this.HasLogicalOrientation;
			}
		}

		/// <summary>Gets a value that indicates whether this <see cref="T:System.Windows.Controls.Panel" /> arranges its descendants in a single dimension.</summary>
		/// <returns>
		///     <see langword="true" /> if the orientation of the <see cref="T:System.Windows.Controls.Panel" /> is in one dimension; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001422 RID: 5154
		// (get) Token: 0x060052EB RID: 21227 RVA: 0x0000B02A File Offset: 0x0000922A
		protected internal virtual bool HasLogicalOrientation
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Controls.UIElementCollection" /> of child elements. </summary>
		/// <returns>An ordered collection of <see cref="T:System.Windows.UIElement" /> objects. This property has no default value.</returns>
		// Token: 0x17001423 RID: 5155
		// (get) Token: 0x060052EC RID: 21228 RVA: 0x001715A6 File Offset: 0x0016F7A6
		protected internal UIElementCollection InternalChildren
		{
			get
			{
				this.VerifyBoundState();
				if (this.IsItemsHost)
				{
					this.EnsureGenerator();
				}
				else if (this._uiElementCollection == null)
				{
					this.EnsureEmptyChildren(this);
				}
				return this._uiElementCollection;
			}
		}

		/// <summary>Gets the number of child <see cref="T:System.Windows.Media.Visual" /> objects in this instance of <see cref="T:System.Windows.Controls.Panel" />.</summary>
		/// <returns>The number of child <see cref="T:System.Windows.Media.Visual" /> objects.</returns>
		// Token: 0x17001424 RID: 5156
		// (get) Token: 0x060052ED RID: 21229 RVA: 0x001715D4 File Offset: 0x0016F7D4
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._uiElementCollection == null)
				{
					return 0;
				}
				return this._uiElementCollection.Count;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Visual" /> child of this <see cref="T:System.Windows.Controls.Panel" /> at the specified index position.</summary>
		/// <param name="index">The index position of the <see cref="T:System.Windows.Media.Visual" /> child.</param>
		/// <returns>A <see cref="T:System.Windows.Media.Visual" /> child of the parent <see cref="T:System.Windows.Controls.Panel" /> element.</returns>
		// Token: 0x060052EE RID: 21230 RVA: 0x001715EC File Offset: 0x0016F7EC
		protected override Visual GetVisualChild(int index)
		{
			if (this._uiElementCollection == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			if (this.IsZStateDirty)
			{
				this.RecomputeZState();
			}
			int index2 = (this._zLut != null) ? this._zLut[index] : index;
			return this._uiElementCollection[index2];
		}

		/// <summary>Creates a new <see cref="T:System.Windows.Controls.UIElementCollection" />.</summary>
		/// <param name="logicalParent">The logical parent element of the collection to be created.</param>
		/// <returns>An ordered collection of elements that have the specified logical parent.</returns>
		// Token: 0x060052EF RID: 21231 RVA: 0x000CD052 File Offset: 0x000CB252
		protected virtual UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
		{
			return new UIElementCollection(this, logicalParent);
		}

		// Token: 0x17001425 RID: 5157
		// (get) Token: 0x060052F0 RID: 21232 RVA: 0x0017164A File Offset: 0x0016F84A
		internal IItemContainerGenerator Generator
		{
			get
			{
				return this._itemContainerGenerator;
			}
		}

		// Token: 0x17001426 RID: 5158
		// (get) Token: 0x060052F1 RID: 21233 RVA: 0x00171652 File Offset: 0x0016F852
		// (set) Token: 0x060052F2 RID: 21234 RVA: 0x0017165B File Offset: 0x0016F85B
		internal bool VSP_IsVirtualizing
		{
			get
			{
				return this.GetBoolField(Panel.BoolField.IsVirtualizing);
			}
			set
			{
				this.SetBoolField(Panel.BoolField.IsVirtualizing, value);
			}
		}

		// Token: 0x17001427 RID: 5159
		// (get) Token: 0x060052F3 RID: 21235 RVA: 0x00171665 File Offset: 0x0016F865
		// (set) Token: 0x060052F4 RID: 21236 RVA: 0x0017166E File Offset: 0x0016F86E
		internal bool VSP_HasMeasured
		{
			get
			{
				return this.GetBoolField(Panel.BoolField.HasMeasured);
			}
			set
			{
				this.SetBoolField(Panel.BoolField.HasMeasured, value);
			}
		}

		// Token: 0x17001428 RID: 5160
		// (get) Token: 0x060052F5 RID: 21237 RVA: 0x00171678 File Offset: 0x0016F878
		// (set) Token: 0x060052F6 RID: 21238 RVA: 0x00171682 File Offset: 0x0016F882
		internal bool VSP_MustDisableVirtualization
		{
			get
			{
				return this.GetBoolField(Panel.BoolField.MustDisableVirtualization);
			}
			set
			{
				this.SetBoolField(Panel.BoolField.MustDisableVirtualization, value);
			}
		}

		// Token: 0x17001429 RID: 5161
		// (get) Token: 0x060052F7 RID: 21239 RVA: 0x0017168D File Offset: 0x0016F88D
		// (set) Token: 0x060052F8 RID: 21240 RVA: 0x00171697 File Offset: 0x0016F897
		internal bool VSP_IsPixelBased
		{
			get
			{
				return this.GetBoolField(Panel.BoolField.IsPixelBased);
			}
			set
			{
				this.SetBoolField(Panel.BoolField.IsPixelBased, value);
			}
		}

		// Token: 0x1700142A RID: 5162
		// (get) Token: 0x060052F9 RID: 21241 RVA: 0x001716A2 File Offset: 0x0016F8A2
		// (set) Token: 0x060052FA RID: 21242 RVA: 0x001716AC File Offset: 0x0016F8AC
		internal bool VSP_InRecyclingMode
		{
			get
			{
				return this.GetBoolField(Panel.BoolField.InRecyclingMode);
			}
			set
			{
				this.SetBoolField(Panel.BoolField.InRecyclingMode, value);
			}
		}

		// Token: 0x1700142B RID: 5163
		// (get) Token: 0x060052FB RID: 21243 RVA: 0x001716B7 File Offset: 0x0016F8B7
		// (set) Token: 0x060052FC RID: 21244 RVA: 0x001716C4 File Offset: 0x0016F8C4
		internal bool VSP_MeasureCaches
		{
			get
			{
				return this.GetBoolField(Panel.BoolField.MeasureCaches);
			}
			set
			{
				this.SetBoolField(Panel.BoolField.MeasureCaches, value);
			}
		}

		// Token: 0x060052FD RID: 21245 RVA: 0x001716D4 File Offset: 0x0016F8D4
		private bool VerifyBoundState()
		{
			bool flag = ItemsControl.GetItemsOwnerInternal(this) != null;
			if (flag)
			{
				if (this._itemContainerGenerator == null)
				{
					this.ClearChildren();
				}
				return this._itemContainerGenerator != null;
			}
			if (this._itemContainerGenerator != null)
			{
				this.DisconnectFromGenerator();
				this.ClearChildren();
			}
			return false;
		}

		// Token: 0x1700142C RID: 5164
		// (get) Token: 0x060052FE RID: 21246 RVA: 0x0017171B File Offset: 0x0016F91B
		internal bool IsDataBound
		{
			get
			{
				return this.IsItemsHost && this._itemContainerGenerator != null;
			}
		}

		// Token: 0x060052FF RID: 21247 RVA: 0x00171730 File Offset: 0x0016F930
		internal static bool IsAboutToGenerateContent(Panel panel)
		{
			return panel.IsItemsHost && panel._itemContainerGenerator == null;
		}

		// Token: 0x06005300 RID: 21248 RVA: 0x00171748 File Offset: 0x0016F948
		private void ConnectToGenerator()
		{
			ItemsControl itemsOwner = ItemsControl.GetItemsOwner(this);
			if (itemsOwner == null)
			{
				throw new InvalidOperationException(SR.Get("Panel_ItemsControlNotFound"));
			}
			IItemContainerGenerator itemContainerGenerator = itemsOwner.ItemContainerGenerator;
			if (itemContainerGenerator != null)
			{
				this._itemContainerGenerator = itemContainerGenerator.GetItemContainerGeneratorForPanel(this);
				if (this._itemContainerGenerator != null)
				{
					this._itemContainerGenerator.ItemsChanged += this.OnItemsChanged;
					((IItemContainerGenerator)this._itemContainerGenerator).RemoveAll();
				}
			}
		}

		// Token: 0x06005301 RID: 21249 RVA: 0x001717B0 File Offset: 0x0016F9B0
		private void DisconnectFromGenerator()
		{
			this._itemContainerGenerator.ItemsChanged -= this.OnItemsChanged;
			((IItemContainerGenerator)this._itemContainerGenerator).RemoveAll();
			this._itemContainerGenerator = null;
		}

		// Token: 0x06005302 RID: 21250 RVA: 0x001717DB File Offset: 0x0016F9DB
		private void EnsureEmptyChildren(FrameworkElement logicalParent)
		{
			if (this._uiElementCollection == null || this._uiElementCollection.LogicalParent != logicalParent)
			{
				this._uiElementCollection = this.CreateUIElementCollection(logicalParent);
				return;
			}
			this.ClearChildren();
		}

		// Token: 0x06005303 RID: 21251 RVA: 0x00171807 File Offset: 0x0016FA07
		internal void EnsureGenerator()
		{
			if (this._itemContainerGenerator == null)
			{
				this.ConnectToGenerator();
				this.EnsureEmptyChildren(null);
				this.GenerateChildren();
			}
		}

		// Token: 0x06005304 RID: 21252 RVA: 0x00171824 File Offset: 0x0016FA24
		private void ClearChildren()
		{
			if (this._itemContainerGenerator != null)
			{
				((IItemContainerGenerator)this._itemContainerGenerator).RemoveAll();
			}
			if (this._uiElementCollection != null && this._uiElementCollection.Count > 0)
			{
				this._uiElementCollection.ClearInternal();
				this.OnClearChildrenInternal();
			}
		}

		// Token: 0x06005305 RID: 21253 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void OnClearChildrenInternal()
		{
		}

		// Token: 0x06005306 RID: 21254 RVA: 0x00171860 File Offset: 0x0016FA60
		internal virtual void GenerateChildren()
		{
			IItemContainerGenerator itemContainerGenerator = this._itemContainerGenerator;
			if (itemContainerGenerator != null)
			{
				using (itemContainerGenerator.StartAt(new GeneratorPosition(-1, 0), GeneratorDirection.Forward))
				{
					UIElement uielement;
					while ((uielement = (itemContainerGenerator.GenerateNext() as UIElement)) != null)
					{
						this._uiElementCollection.AddInternal(uielement);
						itemContainerGenerator.PrepareItemContainer(uielement);
					}
				}
			}
		}

		// Token: 0x06005307 RID: 21255 RVA: 0x001718C8 File Offset: 0x0016FAC8
		private void OnItemsChanged(object sender, ItemsChangedEventArgs args)
		{
			if (this.VerifyBoundState())
			{
				bool flag = this.OnItemsChangedInternal(sender, args);
				if (flag)
				{
					base.InvalidateMeasure();
				}
			}
		}

		// Token: 0x06005308 RID: 21256 RVA: 0x001718F0 File Offset: 0x0016FAF0
		internal virtual bool OnItemsChangedInternal(object sender, ItemsChangedEventArgs args)
		{
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Add:
				this.AddChildren(args.Position, args.ItemCount);
				break;
			case NotifyCollectionChangedAction.Remove:
				this.RemoveChildren(args.Position, args.ItemUICount);
				break;
			case NotifyCollectionChangedAction.Replace:
				this.ReplaceChildren(args.Position, args.ItemCount, args.ItemUICount);
				break;
			case NotifyCollectionChangedAction.Move:
				this.MoveChildren(args.OldPosition, args.Position, args.ItemUICount);
				break;
			case NotifyCollectionChangedAction.Reset:
				this.ResetChildren();
				break;
			}
			return true;
		}

		// Token: 0x06005309 RID: 21257 RVA: 0x00171984 File Offset: 0x0016FB84
		private void AddChildren(GeneratorPosition pos, int itemCount)
		{
			IItemContainerGenerator itemContainerGenerator = this._itemContainerGenerator;
			using (itemContainerGenerator.StartAt(pos, GeneratorDirection.Forward))
			{
				for (int i = 0; i < itemCount; i++)
				{
					UIElement uielement = itemContainerGenerator.GenerateNext() as UIElement;
					if (uielement != null)
					{
						this._uiElementCollection.InsertInternal(pos.Index + 1 + i, uielement);
						itemContainerGenerator.PrepareItemContainer(uielement);
					}
					else
					{
						this._itemContainerGenerator.Verify();
					}
				}
			}
		}

		// Token: 0x0600530A RID: 21258 RVA: 0x00171A04 File Offset: 0x0016FC04
		private void RemoveChildren(GeneratorPosition pos, int containerCount)
		{
			this._uiElementCollection.RemoveRangeInternal(pos.Index, containerCount);
		}

		// Token: 0x0600530B RID: 21259 RVA: 0x00171A1C File Offset: 0x0016FC1C
		private void ReplaceChildren(GeneratorPosition pos, int itemCount, int containerCount)
		{
			IItemContainerGenerator itemContainerGenerator = this._itemContainerGenerator;
			using (itemContainerGenerator.StartAt(pos, GeneratorDirection.Forward, true))
			{
				for (int i = 0; i < itemCount; i++)
				{
					bool flag;
					UIElement uielement = itemContainerGenerator.GenerateNext(out flag) as UIElement;
					if (uielement != null && !flag)
					{
						this._uiElementCollection.SetInternal(pos.Index + i, uielement);
						itemContainerGenerator.PrepareItemContainer(uielement);
					}
					else
					{
						this._itemContainerGenerator.Verify();
					}
				}
			}
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x00171AA4 File Offset: 0x0016FCA4
		private void MoveChildren(GeneratorPosition fromPos, GeneratorPosition toPos, int containerCount)
		{
			if (fromPos == toPos)
			{
				return;
			}
			IItemContainerGenerator itemContainerGenerator = this._itemContainerGenerator;
			int num = itemContainerGenerator.IndexFromGeneratorPosition(toPos);
			UIElement[] array = new UIElement[containerCount];
			for (int i = 0; i < containerCount; i++)
			{
				array[i] = this._uiElementCollection[fromPos.Index + i];
			}
			this._uiElementCollection.RemoveRangeInternal(fromPos.Index, containerCount);
			for (int j = 0; j < containerCount; j++)
			{
				this._uiElementCollection.InsertInternal(num + j, array[j]);
			}
		}

		// Token: 0x0600530D RID: 21261 RVA: 0x00171B2A File Offset: 0x0016FD2A
		private void ResetChildren()
		{
			this.EnsureEmptyChildren(null);
			this.GenerateChildren();
		}

		// Token: 0x0600530E RID: 21262 RVA: 0x00171B39 File Offset: 0x0016FD39
		private bool GetBoolField(Panel.BoolField field)
		{
			return (this._boolFieldStore & field) > ~(Panel.BoolField.IsZStateDirty | Panel.BoolField.IsZStateDiverse | Panel.BoolField.IsVirtualizing | Panel.BoolField.HasMeasured | Panel.BoolField.IsPixelBased | Panel.BoolField.InRecyclingMode | Panel.BoolField.MustDisableVirtualization | Panel.BoolField.MeasureCaches);
		}

		// Token: 0x0600530F RID: 21263 RVA: 0x00171B46 File Offset: 0x0016FD46
		private void SetBoolField(Panel.BoolField field, bool value)
		{
			if (value)
			{
				this._boolFieldStore |= field;
				return;
			}
			this._boolFieldStore &= ~field;
		}

		// Token: 0x1700142D RID: 5165
		// (get) Token: 0x06005310 RID: 21264 RVA: 0x000956EF File Offset: 0x000938EF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 9;
			}
		}

		/// <summary>Invoked when the <see cref="T:System.Windows.Media.VisualCollection" /> of a visual object is modified.</summary>
		/// <param name="visualAdded">The <see cref="T:System.Windows.Media.Visual" /> that was added to the collection.</param>
		/// <param name="visualRemoved">The <see cref="T:System.Windows.Media.Visual" /> that was removed from the collection.</param>
		// Token: 0x06005311 RID: 21265 RVA: 0x00171B6C File Offset: 0x0016FD6C
		protected internal override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			if (!this.IsZStateDirty)
			{
				if (this.IsZStateDiverse)
				{
					this.IsZStateDirty = true;
				}
				else if (visualAdded != null)
				{
					int num = (int)visualAdded.GetValue(Panel.ZIndexProperty);
					if (num != this._zConsonant)
					{
						this.IsZStateDirty = true;
					}
				}
			}
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);
			if (this.IsZStateDirty)
			{
				this.RecomputeZState();
				this.InvalidateZState();
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.Panel.ZIndex" /> attached property for a given element.</summary>
		/// <param name="element">The element on which to apply the property value.</param>
		/// <param name="value">The order on the z-plane in which this element appears.</param>
		/// <exception cref="T:System.ArgumentNullException">The element is <see langword="null" />.</exception>
		// Token: 0x06005312 RID: 21266 RVA: 0x00171BD2 File Offset: 0x0016FDD2
		public static void SetZIndex(UIElement element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Panel.ZIndexProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.Panel.ZIndex" /> property for a given element.</summary>
		/// <param name="element">The element for which to retrieve the <see cref="P:System.Windows.Controls.Panel.ZIndex" /> value.</param>
		/// <returns>The <see cref="P:System.Windows.Controls.Panel.ZIndex" /> position of the element.</returns>
		/// <exception cref="T:System.ArgumentNullException">The element is <see langword="null" />.</exception>
		// Token: 0x06005313 RID: 21267 RVA: 0x00171BF3 File Offset: 0x0016FDF3
		public static int GetZIndex(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Panel.ZIndexProperty);
		}

		// Token: 0x06005314 RID: 21268 RVA: 0x00171C14 File Offset: 0x0016FE14
		private static void OnZIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			int num = (int)e.OldValue;
			int num2 = (int)e.NewValue;
			if (num == num2)
			{
				return;
			}
			UIElement uielement = d as UIElement;
			if (uielement == null)
			{
				return;
			}
			Panel panel = uielement.InternalVisualParent as Panel;
			if (panel == null)
			{
				return;
			}
			panel.InvalidateZState();
		}

		// Token: 0x06005315 RID: 21269 RVA: 0x00171C61 File Offset: 0x0016FE61
		internal void InvalidateZState()
		{
			if (!this.IsZStateDirty && this._uiElementCollection != null)
			{
				base.InvalidateZOrder();
			}
			this.IsZStateDirty = true;
		}

		// Token: 0x1700142E RID: 5166
		// (get) Token: 0x06005316 RID: 21270 RVA: 0x00171C80 File Offset: 0x0016FE80
		// (set) Token: 0x06005317 RID: 21271 RVA: 0x00171C89 File Offset: 0x0016FE89
		private bool IsZStateDirty
		{
			get
			{
				return this.GetBoolField(Panel.BoolField.IsZStateDirty);
			}
			set
			{
				this.SetBoolField(Panel.BoolField.IsZStateDirty, value);
			}
		}

		// Token: 0x1700142F RID: 5167
		// (get) Token: 0x06005318 RID: 21272 RVA: 0x00171C93 File Offset: 0x0016FE93
		// (set) Token: 0x06005319 RID: 21273 RVA: 0x00171C9C File Offset: 0x0016FE9C
		private bool IsZStateDiverse
		{
			get
			{
				return this.GetBoolField(Panel.BoolField.IsZStateDiverse);
			}
			set
			{
				this.SetBoolField(Panel.BoolField.IsZStateDiverse, value);
			}
		}

		// Token: 0x0600531A RID: 21274 RVA: 0x00171CA8 File Offset: 0x0016FEA8
		private void RecomputeZState()
		{
			int num = (this._uiElementCollection != null) ? this._uiElementCollection.Count : 0;
			bool flag = false;
			bool flag2 = false;
			int num2 = (int)Panel.ZIndexProperty.GetDefaultValue(base.DependencyObjectType);
			int num3 = num2;
			List<long> list = null;
			if (num > 0)
			{
				if (this._uiElementCollection[0] != null)
				{
					num3 = (int)this._uiElementCollection[0].GetValue(Panel.ZIndexProperty);
				}
				if (num > 1)
				{
					list = new List<long>(num);
					list.Add((long)num3 << 32);
					int num4 = num3;
					int num5 = 1;
					do
					{
						int num6 = (this._uiElementCollection[num5] != null) ? ((int)this._uiElementCollection[num5].GetValue(Panel.ZIndexProperty)) : num2;
						list.Add(((long)num6 << 32) + (long)num5);
						flag2 |= (num6 < num4);
						num4 = num6;
						flag |= (num6 != num3);
					}
					while (++num5 < num);
				}
			}
			if (flag2)
			{
				list.Sort();
				if (this._zLut == null || this._zLut.Length != num)
				{
					this._zLut = new int[num];
				}
				for (int i = 0; i < num; i++)
				{
					this._zLut[i] = (int)(list[i] & (long)((ulong)-1));
				}
			}
			else
			{
				this._zLut = null;
			}
			this.IsZStateDiverse = flag;
			this._zConsonant = num3;
			this.IsZStateDirty = false;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Panel.Background" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Panel.Background" /> dependency property.</returns>
		// Token: 0x04002CD8 RID: 11480
		[CommonDependencyProperty]
		public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(Panel), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Panel.IsItemsHost" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Panel.IsItemsHost" /> dependency property.</returns>
		// Token: 0x04002CD9 RID: 11481
		public static readonly DependencyProperty IsItemsHostProperty = DependencyProperty.Register("IsItemsHost", typeof(bool), typeof(Panel), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.NotDataBindable, new PropertyChangedCallback(Panel.OnIsItemsHostChanged)));

		// Token: 0x04002CDA RID: 11482
		private UIElementCollection _uiElementCollection;

		// Token: 0x04002CDB RID: 11483
		private ItemContainerGenerator _itemContainerGenerator;

		// Token: 0x04002CDC RID: 11484
		private Panel.BoolField _boolFieldStore;

		// Token: 0x04002CDD RID: 11485
		private const int c_zDefaultValue = 0;

		// Token: 0x04002CDE RID: 11486
		private int _zConsonant;

		// Token: 0x04002CDF RID: 11487
		private int[] _zLut;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Panel.ZIndex" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Panel.ZIndex" /> attached property.</returns>
		// Token: 0x04002CE0 RID: 11488
		public static readonly DependencyProperty ZIndexProperty = DependencyProperty.RegisterAttached("ZIndex", typeof(int), typeof(Panel), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(Panel.OnZIndexPropertyChanged)));

		// Token: 0x020009AC RID: 2476
		[Flags]
		private enum BoolField : byte
		{
			// Token: 0x0400450D RID: 17677
			IsZStateDirty = 1,
			// Token: 0x0400450E RID: 17678
			IsZStateDiverse = 2,
			// Token: 0x0400450F RID: 17679
			IsVirtualizing = 4,
			// Token: 0x04004510 RID: 17680
			HasMeasured = 8,
			// Token: 0x04004511 RID: 17681
			IsPixelBased = 16,
			// Token: 0x04004512 RID: 17682
			InRecyclingMode = 32,
			// Token: 0x04004513 RID: 17683
			MustDisableVirtualization = 64,
			// Token: 0x04004514 RID: 17684
			MeasureCaches = 128
		}
	}
}
