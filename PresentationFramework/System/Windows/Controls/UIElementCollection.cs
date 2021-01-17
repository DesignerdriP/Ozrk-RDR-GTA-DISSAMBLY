using System;
using System.Collections;
using System.Windows.Media;

namespace System.Windows.Controls
{
	/// <summary>Represents an ordered collection of <see cref="T:System.Windows.UIElement" /> child elements. </summary>
	// Token: 0x0200054E RID: 1358
	public class UIElementCollection : IList, ICollection, IEnumerable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.UIElementCollection" /> class. </summary>
		/// <param name="visualParent">The <see cref="T:System.Windows.UIElement" /> parent of the collection.</param>
		/// <param name="logicalParent">The logical parent of the elements in the collection.</param>
		// Token: 0x06005938 RID: 22840 RVA: 0x0018A88C File Offset: 0x00188A8C
		public UIElementCollection(UIElement visualParent, FrameworkElement logicalParent)
		{
			if (visualParent == null)
			{
				throw new ArgumentNullException(SR.Get("Panel_NoNullVisualParent", new object[]
				{
					"visualParent",
					base.GetType()
				}));
			}
			this._visualChildren = new VisualCollection(visualParent);
			this._visualParent = visualParent;
			this._logicalParent = logicalParent;
		}

		/// <summary>Gets the actual number of elements in the collection. </summary>
		/// <returns>The actual number of elements in the collection.</returns>
		// Token: 0x170015B7 RID: 5559
		// (get) Token: 0x06005939 RID: 22841 RVA: 0x0018A8E3 File Offset: 0x00188AE3
		public virtual int Count
		{
			get
			{
				return this._visualChildren.Count;
			}
		}

		/// <summary>Gets a value that indicates whether access to the <see cref="T:System.Collections.ICollection" /> interface is synchronized (thread-safe).</summary>
		/// <returns>
		///     <see langword="true" /> if access to the collection is synchronized; otherwise, <see langword="false" />.</returns>
		// Token: 0x170015B8 RID: 5560
		// (get) Token: 0x0600593A RID: 22842 RVA: 0x0018A8F0 File Offset: 0x00188AF0
		public virtual bool IsSynchronized
		{
			get
			{
				return this._visualChildren.IsSynchronized;
			}
		}

		/// <summary>Gets an object that you can use to synchronize access to the <see cref="T:System.Collections.ICollection" /> interface. </summary>
		/// <returns>
		///     <see cref="T:System.Object" /> that you can use to synchronize access to the <see cref="T:System.Collections.ICollection" /> interface.</returns>
		// Token: 0x170015B9 RID: 5561
		// (get) Token: 0x0600593B RID: 22843 RVA: 0x0018A8FD File Offset: 0x00188AFD
		public virtual object SyncRoot
		{
			get
			{
				return this._visualChildren.SyncRoot;
			}
		}

		/// <summary>Copies a <see cref="T:System.Windows.UIElement" /> from a <see cref="T:System.Windows.Controls.UIElementCollection" /> to an array, starting at a specified index position. </summary>
		/// <param name="array">An array into which the collection is copied.</param>
		/// <param name="index">The index position of the element where copying begins.</param>
		// Token: 0x0600593C RID: 22844 RVA: 0x0018A90A File Offset: 0x00188B0A
		public virtual void CopyTo(Array array, int index)
		{
			this._visualChildren.CopyTo(array, index);
		}

		/// <summary>Copies a <see cref="T:System.Windows.UIElement" /> from a <see cref="T:System.Windows.Controls.UIElementCollection" /> to an array, starting at a specified index position. </summary>
		/// <param name="array">An array of <see cref="T:System.Windows.UIElement" /> objects.</param>
		/// <param name="index">The index position of the element where copying begins.</param>
		// Token: 0x0600593D RID: 22845 RVA: 0x0018A919 File Offset: 0x00188B19
		public virtual void CopyTo(UIElement[] array, int index)
		{
			this._visualChildren.CopyTo(array, index);
		}

		/// <summary>Gets or sets the number of elements that the <see cref="T:System.Windows.Controls.UIElementCollection" /> can contain. </summary>
		/// <returns>The total number of elements the collection can contain.</returns>
		// Token: 0x170015BA RID: 5562
		// (get) Token: 0x0600593E RID: 22846 RVA: 0x0018A928 File Offset: 0x00188B28
		// (set) Token: 0x0600593F RID: 22847 RVA: 0x0018A935 File Offset: 0x00188B35
		public virtual int Capacity
		{
			get
			{
				return this._visualChildren.Capacity;
			}
			set
			{
				this.VerifyWriteAccess();
				this._visualChildren.Capacity = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.UIElement" /> stored at the zero-based index position of the <see cref="T:System.Windows.Controls.UIElementCollection" />. </summary>
		/// <param name="index">The index position of the <see cref="T:System.Windows.UIElement" />.</param>
		/// <returns>A <see cref="T:System.Windows.UIElement" /> at the specified <paramref name="index" /> position.</returns>
		// Token: 0x170015BB RID: 5563
		public virtual UIElement this[int index]
		{
			get
			{
				return this._visualChildren[index] as UIElement;
			}
			set
			{
				this.VerifyWriteAccess();
				this.ValidateElement(value);
				VisualCollection visualChildren = this._visualChildren;
				if (visualChildren[index] != value)
				{
					UIElement uielement = visualChildren[index] as UIElement;
					if (uielement != null)
					{
						this.ClearLogicalParent(uielement);
					}
					visualChildren[index] = value;
					this.SetLogicalParent(value);
					this._visualParent.InvalidateMeasure();
				}
			}
		}

		// Token: 0x06005942 RID: 22850 RVA: 0x0018A9B8 File Offset: 0x00188BB8
		internal void SetInternal(int index, UIElement item)
		{
			this.ValidateElement(item);
			VisualCollection visualChildren = this._visualChildren;
			if (visualChildren[index] != item)
			{
				visualChildren[index] = null;
				visualChildren[index] = item;
				this._visualParent.InvalidateMeasure();
			}
		}

		/// <summary>Adds the specified element to the <see cref="T:System.Windows.Controls.UIElementCollection" />. </summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> to add.</param>
		/// <returns>The index position of the added element.</returns>
		// Token: 0x06005943 RID: 22851 RVA: 0x0018A9F8 File Offset: 0x00188BF8
		public virtual int Add(UIElement element)
		{
			this.VerifyWriteAccess();
			return this.AddInternal(element);
		}

		// Token: 0x06005944 RID: 22852 RVA: 0x0018AA08 File Offset: 0x00188C08
		internal int AddInternal(UIElement element)
		{
			this.ValidateElement(element);
			this.SetLogicalParent(element);
			int result = this._visualChildren.Add(element);
			this._visualParent.InvalidateMeasure();
			return result;
		}

		/// <summary>Returns the index position of a specified element in a <see cref="T:System.Windows.Controls.UIElementCollection" />. </summary>
		/// <param name="element">The element whose index position is required.</param>
		/// <returns>The index position of the element.  -1 if the element is not in the collection.</returns>
		// Token: 0x06005945 RID: 22853 RVA: 0x0018AA3C File Offset: 0x00188C3C
		public virtual int IndexOf(UIElement element)
		{
			return this._visualChildren.IndexOf(element);
		}

		/// <summary>Removes the specified element from a <see cref="T:System.Windows.Controls.UIElementCollection" />. </summary>
		/// <param name="element">The element to remove from the collection.</param>
		// Token: 0x06005946 RID: 22854 RVA: 0x0018AA4A File Offset: 0x00188C4A
		public virtual void Remove(UIElement element)
		{
			this.VerifyWriteAccess();
			this.RemoveInternal(element);
		}

		// Token: 0x06005947 RID: 22855 RVA: 0x0018AA59 File Offset: 0x00188C59
		internal void RemoveInternal(UIElement element)
		{
			this._visualChildren.Remove(element);
			this.ClearLogicalParent(element);
			this._visualParent.InvalidateMeasure();
		}

		// Token: 0x06005948 RID: 22856 RVA: 0x0018AA79 File Offset: 0x00188C79
		internal virtual void RemoveNoVerify(UIElement element)
		{
			this._visualChildren.Remove(element);
		}

		/// <summary>Determines whether a specified element is in the <see cref="T:System.Windows.Controls.UIElementCollection" />.</summary>
		/// <param name="element">The element to find.</param>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.UIElement" /> is found in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005949 RID: 22857 RVA: 0x0018AA87 File Offset: 0x00188C87
		public virtual bool Contains(UIElement element)
		{
			return this._visualChildren.Contains(element);
		}

		/// <summary>Removes all elements from a <see cref="T:System.Windows.Controls.UIElementCollection" />. </summary>
		// Token: 0x0600594A RID: 22858 RVA: 0x0018AA95 File Offset: 0x00188C95
		public virtual void Clear()
		{
			this.VerifyWriteAccess();
			this.ClearInternal();
		}

		// Token: 0x0600594B RID: 22859 RVA: 0x0018AAA4 File Offset: 0x00188CA4
		internal void ClearInternal()
		{
			VisualCollection visualChildren = this._visualChildren;
			int count = visualChildren.Count;
			if (count > 0)
			{
				Visual[] array = new Visual[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = visualChildren[i];
				}
				visualChildren.Clear();
				for (int j = 0; j < count; j++)
				{
					UIElement uielement = array[j] as UIElement;
					if (uielement != null)
					{
						this.ClearLogicalParent(uielement);
					}
				}
				this._visualParent.InvalidateMeasure();
			}
		}

		/// <summary>Inserts an element into a <see cref="T:System.Windows.Controls.UIElementCollection" /> at the specified index position. </summary>
		/// <param name="index">The index position where you want to insert the element.</param>
		/// <param name="element">The element to insert into the <see cref="T:System.Windows.Controls.UIElementCollection" />.</param>
		// Token: 0x0600594C RID: 22860 RVA: 0x0018AB18 File Offset: 0x00188D18
		public virtual void Insert(int index, UIElement element)
		{
			this.VerifyWriteAccess();
			this.InsertInternal(index, element);
		}

		// Token: 0x0600594D RID: 22861 RVA: 0x0018AB28 File Offset: 0x00188D28
		internal void InsertInternal(int index, UIElement element)
		{
			this.ValidateElement(element);
			this.SetLogicalParent(element);
			this._visualChildren.Insert(index, element);
			this._visualParent.InvalidateMeasure();
		}

		/// <summary>Removes the <see cref="T:System.Windows.UIElement" /> at the specified index. </summary>
		/// <param name="index">The index of the <see cref="T:System.Windows.UIElement" /> that you want to remove.</param>
		// Token: 0x0600594E RID: 22862 RVA: 0x0018AB50 File Offset: 0x00188D50
		public virtual void RemoveAt(int index)
		{
			this.VerifyWriteAccess();
			VisualCollection visualChildren = this._visualChildren;
			UIElement uielement = visualChildren[index] as UIElement;
			visualChildren.RemoveAt(index);
			if (uielement != null)
			{
				this.ClearLogicalParent(uielement);
			}
			this._visualParent.InvalidateMeasure();
		}

		/// <summary>Removes a range of elements from a <see cref="T:System.Windows.Controls.UIElementCollection" />. </summary>
		/// <param name="index">The index position of the element where removal begins.</param>
		/// <param name="count">The number of elements to remove.</param>
		// Token: 0x0600594F RID: 22863 RVA: 0x0018AB93 File Offset: 0x00188D93
		public virtual void RemoveRange(int index, int count)
		{
			this.VerifyWriteAccess();
			this.RemoveRangeInternal(index, count);
		}

		// Token: 0x06005950 RID: 22864 RVA: 0x0018ABA4 File Offset: 0x00188DA4
		internal void RemoveRangeInternal(int index, int count)
		{
			VisualCollection visualChildren = this._visualChildren;
			int count2 = visualChildren.Count;
			if (count > count2 - index)
			{
				count = count2 - index;
			}
			if (count > 0)
			{
				Visual[] array = new Visual[count];
				int i = index;
				for (int j = 0; j < count; j++)
				{
					array[j] = visualChildren[i];
					i++;
				}
				visualChildren.RemoveRange(index, count);
				for (i = 0; i < count; i++)
				{
					UIElement uielement = array[i] as UIElement;
					if (uielement != null)
					{
						this.ClearLogicalParent(uielement);
					}
				}
				this._visualParent.InvalidateMeasure();
			}
		}

		// Token: 0x06005951 RID: 22865 RVA: 0x0018AC2B File Offset: 0x00188E2B
		internal void MoveVisualChild(Visual visual, Visual destination)
		{
			this._visualChildren.Move(visual, destination);
		}

		// Token: 0x06005952 RID: 22866 RVA: 0x0018AC3C File Offset: 0x00188E3C
		private UIElement Cast(object value)
		{
			if (value == null)
			{
				throw new ArgumentException(SR.Get("Collection_NoNull", new object[]
				{
					"UIElementCollection"
				}));
			}
			UIElement uielement = value as UIElement;
			if (uielement == null)
			{
				throw new ArgumentException(SR.Get("Collection_BadType", new object[]
				{
					"UIElementCollection",
					value.GetType().Name,
					"UIElement"
				}));
			}
			return uielement;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. For a description of this member, see <see cref="M:System.Collections.IList.Add(System.Object)" />.</summary>
		/// <param name="value">The object to add.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		// Token: 0x06005953 RID: 22867 RVA: 0x0018ACA9 File Offset: 0x00188EA9
		int IList.Add(object value)
		{
			return this.Add(this.Cast(value));
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. For a description of this member, see <see cref="M:System.Collections.IList.Contains(System.Object)" />.</summary>
		/// <param name="value">The object to locate in the list.</param>
		/// <returns>
		///     <see langword="true" /> if the object was found in the list; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005954 RID: 22868 RVA: 0x0018ACB8 File Offset: 0x00188EB8
		bool IList.Contains(object value)
		{
			return this.Contains(value as UIElement);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. For a description of this member, see <see cref="M:System.Collections.IList.IndexOf(System.Object)" />.</summary>
		/// <param name="value">The object to locate in the list.</param>
		/// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1. </returns>
		// Token: 0x06005955 RID: 22869 RVA: 0x0018ACC6 File Offset: 0x00188EC6
		int IList.IndexOf(object value)
		{
			return this.IndexOf(value as UIElement);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. For a description of this member, see <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" />.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted. </param>
		/// <param name="value">The object to insert to the list.</param>
		// Token: 0x06005956 RID: 22870 RVA: 0x0018ACD4 File Offset: 0x00188ED4
		void IList.Insert(int index, object value)
		{
			this.Insert(index, this.Cast(value));
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. For a description of this member, see <see cref="P:System.Collections.IList.IsFixedSize" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the list has a fixed size; otherwise, <see langword="false" />. </returns>
		// Token: 0x170015BC RID: 5564
		// (get) Token: 0x06005957 RID: 22871 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. For a description of this member, see <see cref="P:System.Collections.IList.IsReadOnly" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the list is read-only; otherwise, <see langword="false" />. </returns>
		// Token: 0x170015BD RID: 5565
		// (get) Token: 0x06005958 RID: 22872 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. For a description of this member, see <see cref="M:System.Collections.IList.Remove(System.Object)" />.</summary>
		/// <param name="value">The object to remove from the list.</param>
		// Token: 0x06005959 RID: 22873 RVA: 0x0018ACE4 File Offset: 0x00188EE4
		void IList.Remove(object value)
		{
			this.Remove(value as UIElement);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. For a description of this member, see <see cref="P:System.Collections.IList.Item(System.Int32)" />.</summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <returns>The element at the specified index.</returns>
		// Token: 0x170015BE RID: 5566
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = this.Cast(value);
			}
		}

		/// <summary>Returns an enumerator that can iterate the <see cref="T:System.Windows.Controls.UIElementCollection" />. </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can list the members of this collection.</returns>
		// Token: 0x0600595C RID: 22876 RVA: 0x0018AD0B File Offset: 0x00188F0B
		public virtual IEnumerator GetEnumerator()
		{
			return this._visualChildren.GetEnumerator();
		}

		/// <summary>Sets the logical parent of an element in a <see cref="T:System.Windows.Controls.UIElementCollection" />. </summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> whose logical parent is set.</param>
		// Token: 0x0600595D RID: 22877 RVA: 0x0018AD1D File Offset: 0x00188F1D
		protected void SetLogicalParent(UIElement element)
		{
			if (this._logicalParent != null)
			{
				this._logicalParent.AddLogicalChild(element);
			}
		}

		/// <summary>Clears the logical parent of an element when the element leaves a <see cref="T:System.Windows.Controls.UIElementCollection" />. </summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> whose logical parent is being cleared.</param>
		// Token: 0x0600595E RID: 22878 RVA: 0x0018AD33 File Offset: 0x00188F33
		protected void ClearLogicalParent(UIElement element)
		{
			if (this._logicalParent != null)
			{
				this._logicalParent.RemoveLogicalChild(element);
			}
		}

		// Token: 0x170015BF RID: 5567
		// (get) Token: 0x0600595F RID: 22879 RVA: 0x0018AD49 File Offset: 0x00188F49
		internal UIElement VisualParent
		{
			get
			{
				return this._visualParent;
			}
		}

		// Token: 0x06005960 RID: 22880 RVA: 0x0018AD51 File Offset: 0x00188F51
		private void ValidateElement(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException(SR.Get("Panel_NoNullChildren", new object[]
				{
					base.GetType()
				}));
			}
		}

		// Token: 0x06005961 RID: 22881 RVA: 0x0018AD78 File Offset: 0x00188F78
		private void VerifyWriteAccess()
		{
			Panel panel = this._visualParent as Panel;
			if (panel != null && panel.IsDataBound)
			{
				throw new InvalidOperationException(SR.Get("Panel_BoundPanel_NoChildren"));
			}
		}

		// Token: 0x170015C0 RID: 5568
		// (get) Token: 0x06005962 RID: 22882 RVA: 0x0018ADAC File Offset: 0x00188FAC
		internal FrameworkElement LogicalParent
		{
			get
			{
				return this._logicalParent;
			}
		}

		// Token: 0x04002EFA RID: 12026
		private readonly VisualCollection _visualChildren;

		// Token: 0x04002EFB RID: 12027
		private readonly UIElement _visualParent;

		// Token: 0x04002EFC RID: 12028
		private readonly FrameworkElement _logicalParent;
	}
}
