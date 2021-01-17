using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xaml;

namespace System.Windows.Markup
{
	/// <summary>Represents a dictionary that contains xmlns mappings for XAML namespaces in WPF. </summary>
	// Token: 0x02000276 RID: 630
	public class XmlnsDictionary : IDictionary, ICollection, IEnumerable, IXamlNamespaceResolver
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.XmlnsDictionary" /> class.</summary>
		// Token: 0x060023D7 RID: 9175 RVA: 0x000AEE8A File Offset: 0x000AD08A
		public XmlnsDictionary()
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.XmlnsDictionary" /> class by using the specified dictionary as a copy source.</summary>
		/// <param name="xmlnsDictionary">The dictionary on which to base the new <see cref="T:System.Windows.Markup.XmlnsDictionary" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="xmlnsDictionary" /> is <see langword="null" />.</exception>
		// Token: 0x060023D8 RID: 9176 RVA: 0x000AEE98 File Offset: 0x000AD098
		public XmlnsDictionary(XmlnsDictionary xmlnsDictionary)
		{
			if (xmlnsDictionary == null)
			{
				throw new ArgumentNullException("xmlnsDictionary");
			}
			if (xmlnsDictionary != null && xmlnsDictionary.Count > 0)
			{
				this._lastDecl = xmlnsDictionary._lastDecl;
				if (this._nsDeclarations == null)
				{
					this._nsDeclarations = new XmlnsDictionary.NamespaceDeclaration[this._lastDecl + 1];
				}
				this._countDecl = 0;
				for (int i = 0; i <= this._lastDecl; i++)
				{
					if (xmlnsDictionary._nsDeclarations[i].Uri != null)
					{
						this._countDecl++;
					}
					this._nsDeclarations[i].Prefix = xmlnsDictionary._nsDeclarations[i].Prefix;
					this._nsDeclarations[i].Uri = xmlnsDictionary._nsDeclarations[i].Uri;
					this._nsDeclarations[i].ScopeCount = xmlnsDictionary._nsDeclarations[i].ScopeCount;
				}
				return;
			}
			this.Initialize();
		}

		/// <summary>Adds a prefix-URI pair to this <see cref="T:System.Windows.Markup.XmlnsDictionary" />.</summary>
		/// <param name="prefix">The prefix of the XAML namespace to be added. </param>
		/// <param name="xmlNamespace">The XAML namespace URI the prefix maps to. </param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="prefix" /> or <paramref name="xmlNamespace" /> is not a string. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="prefix" /> or <paramref name="xmlNamespace" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Markup.XmlnsDictionary" /> is sealed.</exception>
		// Token: 0x060023D9 RID: 9177 RVA: 0x000AEF9E File Offset: 0x000AD19E
		public void Add(object prefix, object xmlNamespace)
		{
			if (!(prefix is string) || !(xmlNamespace is string))
			{
				throw new ArgumentException(SR.Get("ParserKeysAreStrings"));
			}
			this.AddNamespace((string)prefix, (string)xmlNamespace);
		}

		/// <summary>Adds a prefix-URI pair to this <see cref="T:System.Windows.Markup.XmlnsDictionary" />.</summary>
		/// <param name="prefix">The prefix of this XML namespace.</param>
		/// <param name="xmlNamespace">The XML namespace URI the prefix maps to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="prefix" /> or <paramref name="xmlNamespace" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Markup.XmlnsDictionary" /> is sealed.</exception>
		// Token: 0x060023DA RID: 9178 RVA: 0x000AEFD2 File Offset: 0x000AD1D2
		public void Add(string prefix, string xmlNamespace)
		{
			this.AddNamespace(prefix, xmlNamespace);
		}

		/// <summary>Removes all entries from this <see cref="T:System.Windows.Markup.XmlnsDictionary" />. </summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Markup.XmlnsDictionary" /> is sealed.</exception>
		// Token: 0x060023DB RID: 9179 RVA: 0x000AEFDC File Offset: 0x000AD1DC
		public void Clear()
		{
			this.CheckSealed();
			this._lastDecl = 0;
			this._countDecl = 0;
		}

		/// <summary>Returns a value that indicates whether the specified prefix key is in this <see cref="T:System.Windows.Markup.XmlnsDictionary" />. </summary>
		/// <param name="key">The prefix key to search for.</param>
		/// <returns>
		///     <see langword="true" /> if the requested prefix key is in the dictionary; otherwise, <see langword="false" />.</returns>
		// Token: 0x060023DC RID: 9180 RVA: 0x000AEFF2 File Offset: 0x000AD1F2
		public bool Contains(object key)
		{
			return this.HasNamespace((string)key);
		}

		/// <summary>Removes the item with the specified prefix key from the <see cref="T:System.Windows.Markup.XmlnsDictionary" />. </summary>
		/// <param name="prefix">The prefix key to remove.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Markup.XmlnsDictionary" /> is sealed.</exception>
		// Token: 0x060023DD RID: 9181 RVA: 0x000AF000 File Offset: 0x000AD200
		public void Remove(string prefix)
		{
			string xmlNamespace = this.LookupNamespace(prefix);
			this.RemoveNamespace(prefix, xmlNamespace);
		}

		/// <summary>Removes the item with the specified prefix key from the <see cref="T:System.Windows.Markup.XmlnsDictionary" />. </summary>
		/// <param name="prefix">The prefix key to remove.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Markup.XmlnsDictionary" /> is sealed.</exception>
		// Token: 0x060023DE RID: 9182 RVA: 0x000AF01D File Offset: 0x000AD21D
		public void Remove(object prefix)
		{
			this.Remove((string)prefix);
		}

		/// <summary>Copies the entries in the <see cref="T:System.Windows.Markup.XmlnsDictionary" /> to the specified <see cref="T:System.Collections.DictionaryEntry" /> array. </summary>
		/// <param name="array">The array to copy the table data into.</param>
		/// <param name="index">The zero-based index in the destination array where copying starts.</param>
		// Token: 0x060023DF RID: 9183 RVA: 0x000AF02B File Offset: 0x000AD22B
		public void CopyTo(DictionaryEntry[] array, int index)
		{
			this.CopyTo(array, index);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IDictionary.GetEnumerator" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> object that can be used to iterate through the collection.</returns>
		// Token: 0x060023E0 RID: 9184 RVA: 0x000AF038 File Offset: 0x000AD238
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			HybridDictionary hybridDictionary = new HybridDictionary(this._lastDecl);
			for (int i = 0; i < this._lastDecl; i++)
			{
				if (this._nsDeclarations[i].Uri != null)
				{
					hybridDictionary[this._nsDeclarations[i].Prefix] = this._nsDeclarations[i].Uri;
				}
			}
			return hybridDictionary.GetEnumerator();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IEnumerable.GetEnumerator" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		// Token: 0x060023E1 RID: 9185 RVA: 0x000AF0A3 File Offset: 0x000AD2A3
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>Copies the entries in the <see cref="T:System.Windows.Markup.XmlnsDictionary" /> to the specified array. </summary>
		/// <param name="array">The array to copy the table data into.</param>
		/// <param name="index">The zero-based index in the destination array where copying starts.</param>
		// Token: 0x060023E2 RID: 9186 RVA: 0x000AF0AC File Offset: 0x000AD2AC
		public void CopyTo(Array array, int index)
		{
			IDictionary namespacesInScope = this.GetNamespacesInScope(XmlnsDictionary.NamespaceScope.All);
			if (namespacesInScope != null)
			{
				namespacesInScope.CopyTo(array, index);
			}
		}

		/// <summary>Retrieves a XAML namespace for the provided prefix string.</summary>
		/// <param name="prefix">The prefix to retrieve the XAML namespace for.</param>
		/// <returns>The requested XAML namespace URI.</returns>
		// Token: 0x060023E3 RID: 9187 RVA: 0x000AF0CC File Offset: 0x000AD2CC
		public string GetNamespace(string prefix)
		{
			return this.LookupNamespace(prefix);
		}

		/// <summary>Returns all possible prefix-XAML namespace mappings (<see cref="T:System.Xaml.NamespaceDeclaration" /> values) that are available in the active schema context.</summary>
		/// <returns>An enumerable set of <see cref="T:System.Xaml.NamespaceDeclaration" /> values. To get the prefix strings specifically, get the <see cref="P:System.Xaml.NamespaceDeclaration.Prefix" /> value from each value returned.</returns>
		// Token: 0x060023E4 RID: 9188 RVA: 0x000AF0D5 File Offset: 0x000AD2D5
		public IEnumerable<System.Xaml.NamespaceDeclaration> GetNamespacePrefixes()
		{
			if (this._lastDecl > 0)
			{
				int num;
				for (int i = this._lastDecl - 1; i >= 0; i = num - 1)
				{
					yield return new System.Xaml.NamespaceDeclaration(this._nsDeclarations[i].Uri, this._nsDeclarations[i].Prefix);
					num = i;
				}
			}
			yield break;
		}

		/// <summary>Returns a dictionary enumerator that iterates through this <see cref="T:System.Windows.Markup.XmlnsDictionary" />.</summary>
		/// <returns>The dictionary enumerator for this dictionary.</returns>
		// Token: 0x060023E5 RID: 9189 RVA: 0x000AF0E8 File Offset: 0x000AD2E8
		protected IDictionaryEnumerator GetDictionaryEnumerator()
		{
			HybridDictionary hybridDictionary = new HybridDictionary(this._lastDecl);
			for (int i = 0; i < this._lastDecl; i++)
			{
				if (this._nsDeclarations[i].Uri != null)
				{
					hybridDictionary[this._nsDeclarations[i].Prefix] = this._nsDeclarations[i].Uri;
				}
			}
			return hybridDictionary.GetEnumerator();
		}

		/// <summary>Returns an enumerator that iterates through this <see cref="T:System.Windows.Markup.XmlnsDictionary" />.</summary>
		/// <returns>The enumerator for this dictionary.</returns>
		// Token: 0x060023E6 RID: 9190 RVA: 0x000AF153 File Offset: 0x000AD353
		protected IEnumerator GetEnumerator()
		{
			return this.Keys.GetEnumerator();
		}

		/// <summary>Locks the dictionary so that it cannot be changed. </summary>
		// Token: 0x060023E7 RID: 9191 RVA: 0x000AF160 File Offset: 0x000AD360
		public void Seal()
		{
			this._sealed = true;
		}

		/// <summary>Returns the XAML namespace URI that corresponds to the specified XML namespace prefix. </summary>
		/// <param name="prefix">The XAML namespace prefix to look up.</param>
		/// <returns>The XAML namespace URI that corresponds to the specified prefix if it exists in this <see cref="T:System.Windows.Markup.XmlnsDictionary" />; otherwise, <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="prefix" /> is <see langword="null" />.</exception>
		// Token: 0x060023E8 RID: 9192 RVA: 0x000AF16C File Offset: 0x000AD36C
		public string LookupNamespace(string prefix)
		{
			if (prefix == null)
			{
				throw new ArgumentNullException("prefix");
			}
			if (this._lastDecl > 0)
			{
				for (int i = this._lastDecl - 1; i >= 0; i--)
				{
					if (this._nsDeclarations[i].Prefix == prefix && this._nsDeclarations[i].Uri != null && this._nsDeclarations[i].Uri != string.Empty)
					{
						return this._nsDeclarations[i].Uri;
					}
				}
			}
			return null;
		}

		/// <summary>Returns the prefix that corresponds to the specified XAML namespace URI. </summary>
		/// <param name="xmlNamespace">The XAML namespace URI to look up.</param>
		/// <returns>The XML prefix that corresponds to the given namespace; otherwise, <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="xmlNamespace" /> is <see langword="null" />.</exception>
		// Token: 0x060023E9 RID: 9193 RVA: 0x000AF200 File Offset: 0x000AD400
		public string LookupPrefix(string xmlNamespace)
		{
			if (xmlNamespace == null)
			{
				throw new ArgumentNullException("xmlNamespace");
			}
			if (this._lastDecl > 0)
			{
				for (int i = this._lastDecl - 1; i >= 0; i--)
				{
					if (this._nsDeclarations[i].Uri == xmlNamespace)
					{
						return this._nsDeclarations[i].Prefix;
					}
				}
			}
			return null;
		}

		/// <summary>Looks up the XAML namespace that corresponds to the default XAML namespace. </summary>
		/// <returns>The namespace that corresponds to the default XML namespace if one exists; otherwise, <see langword="null" />.</returns>
		// Token: 0x060023EA RID: 9194 RVA: 0x000AF264 File Offset: 0x000AD464
		public string DefaultNamespace()
		{
			string text = this.LookupNamespace(string.Empty);
			if (text != null)
			{
				return text;
			}
			return string.Empty;
		}

		/// <summary>Pushes the scope of the <see cref="T:System.Windows.Markup.XmlnsDictionary" />. </summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Markup.XmlnsDictionary" /> is sealed.</exception>
		// Token: 0x060023EB RID: 9195 RVA: 0x000AF287 File Offset: 0x000AD487
		public void PushScope()
		{
			this.CheckSealed();
			XmlnsDictionary.NamespaceDeclaration[] nsDeclarations = this._nsDeclarations;
			int lastDecl = this._lastDecl;
			nsDeclarations[lastDecl].ScopeCount = nsDeclarations[lastDecl].ScopeCount + 1;
		}

		/// <summary>Pops the scope of the <see cref="T:System.Windows.Markup.XmlnsDictionary" />. </summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Markup.XmlnsDictionary" /> is sealed.</exception>
		// Token: 0x060023EC RID: 9196 RVA: 0x000AF2AC File Offset: 0x000AD4AC
		public void PopScope()
		{
			this.CheckSealed();
			int scopeCount = this._nsDeclarations[this._lastDecl].ScopeCount;
			int num = this._lastDecl;
			while (num > 0 && this._nsDeclarations[num - 1].ScopeCount == scopeCount)
			{
				num--;
			}
			if (this._nsDeclarations[num].ScopeCount > 0)
			{
				XmlnsDictionary.NamespaceDeclaration[] nsDeclarations = this._nsDeclarations;
				int num2 = num;
				nsDeclarations[num2].ScopeCount = nsDeclarations[num2].ScopeCount - 1;
				this._nsDeclarations[num].Prefix = string.Empty;
				this._nsDeclarations[num].Uri = null;
			}
			this._lastDecl = num;
		}

		/// <summary>Gets a value that indicates whether the size of the <see cref="T:System.Windows.Markup.XmlnsDictionary" /> is fixed. </summary>
		/// <returns>
		///     <see langword="true" /> if the size is fixed; otherwise, <see langword="false" />. </returns>
		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x060023ED RID: 9197 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Markup.XmlnsDictionary" /> is read-only. </summary>
		/// <returns>
		///     <see langword="true" /> if the dictionary is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x060023EE RID: 9198 RVA: 0x000AF356 File Offset: 0x000AD556
		public bool IsReadOnly
		{
			get
			{
				return this._sealed;
			}
		}

		/// <summary>Gets or sets the XAML namespace URI associated with the specified prefix.</summary>
		/// <param name="prefix">The prefix from which to get or set the associated namespace.</param>
		/// <returns>The corresponding XML namespace URI.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="prefix" /> is <see langword="null" />-or-The value to set is <see langword="null" />.</exception>
		// Token: 0x170008AC RID: 2220
		public string this[string prefix]
		{
			get
			{
				return this.LookupNamespace(prefix);
			}
			set
			{
				this.AddNamespace(prefix, value);
			}
		}

		/// <summary>Gets or sets the XAML namespace URI associated with the specified prefix.</summary>
		/// <param name="prefix">The prefix from which to get or set the associated XML namespace URI.</param>
		/// <returns>The corresponding XAML namespace URI.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="prefix" /> is not a string-or-The value to set is not a string.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="prefix" /> is <see langword="null" />-or-The value to set is <see langword="null" />.</exception>
		// Token: 0x170008AD RID: 2221
		public object this[object prefix]
		{
			get
			{
				if (!(prefix is string))
				{
					throw new ArgumentException(SR.Get("ParserKeysAreStrings"));
				}
				return this.LookupNamespace((string)prefix);
			}
			set
			{
				if (!(prefix is string) || !(value is string))
				{
					throw new ArgumentException(SR.Get("ParserKeysAreStrings"));
				}
				this.AddNamespace((string)prefix, (string)value);
			}
		}

		/// <summary>Gets a collection of all the keys in the <see cref="T:System.Windows.Markup.XmlnsDictionary" />. </summary>
		/// <returns>The collection of all the keys in the dictionary.</returns>
		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x060023F3 RID: 9203 RVA: 0x000AF384 File Offset: 0x000AD584
		public ICollection Keys
		{
			get
			{
				ArrayList arrayList = new ArrayList(this._lastDecl + 1);
				for (int i = 0; i < this._lastDecl; i++)
				{
					if (this._nsDeclarations[i].Uri != null && !arrayList.Contains(this._nsDeclarations[i].Prefix))
					{
						arrayList.Add(this._nsDeclarations[i].Prefix);
					}
				}
				return arrayList;
			}
		}

		/// <summary>Gets a collection of all the values in the <see cref="T:System.Windows.Markup.XmlnsDictionary" />. </summary>
		/// <returns>A collection of all the values in the <see cref="T:System.Windows.Markup.XmlnsDictionary" />.</returns>
		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x060023F4 RID: 9204 RVA: 0x000AF3F8 File Offset: 0x000AD5F8
		public ICollection Values
		{
			get
			{
				HybridDictionary hybridDictionary = new HybridDictionary(this._lastDecl + 1);
				for (int i = 0; i < this._lastDecl; i++)
				{
					if (this._nsDeclarations[i].Uri != null)
					{
						hybridDictionary[this._nsDeclarations[i].Prefix] = this._nsDeclarations[i].Uri;
					}
				}
				return hybridDictionary.Values;
			}
		}

		/// <summary>Gets the number of items in the <see cref="T:System.Windows.Markup.XmlnsDictionary" />. </summary>
		/// <returns>The number of items in the <see cref="T:System.Windows.Markup.XmlnsDictionary" />.</returns>
		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x060023F5 RID: 9205 RVA: 0x000AF465 File Offset: 0x000AD665
		public int Count
		{
			get
			{
				return this._countDecl;
			}
		}

		/// <summary>Gets a value that indicates whether access to this <see cref="T:System.Windows.Markup.XmlnsDictionary" /> is thread safe. </summary>
		/// <returns>
		///     <see langword="true" /> if access to this dictionary is thread-safe; otherwise, <see langword="false" />.</returns>
		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x060023F6 RID: 9206 RVA: 0x000AF46D File Offset: 0x000AD66D
		public bool IsSynchronized
		{
			get
			{
				return this._nsDeclarations.IsSynchronized;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Windows.Markup.XmlnsDictionary" />. </summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Markup.XmlnsDictionary" />.</returns>
		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x060023F7 RID: 9207 RVA: 0x000AF47A File Offset: 0x000AD67A
		public object SyncRoot
		{
			get
			{
				return this._nsDeclarations.SyncRoot;
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Markup.XmlnsDictionary" /> is sealed. </summary>
		/// <returns>
		///     <see langword="true" /> if the dictionary is sealed; otherwise, <see langword="false" />.</returns>
		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x060023F8 RID: 9208 RVA: 0x000AF356 File Offset: 0x000AD556
		public bool Sealed
		{
			get
			{
				return this._sealed;
			}
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x000AF487 File Offset: 0x000AD687
		internal void Unseal()
		{
			this._sealed = false;
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x000AF490 File Offset: 0x000AD690
		private void Initialize()
		{
			this._nsDeclarations = new XmlnsDictionary.NamespaceDeclaration[8];
			this._nsDeclarations[0].Prefix = string.Empty;
			this._nsDeclarations[0].Uri = null;
			this._nsDeclarations[0].ScopeCount = 0;
			this._lastDecl = 0;
			this._countDecl = 0;
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x000AF4F1 File Offset: 0x000AD6F1
		private void CheckSealed()
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("ParserDictionarySealed"));
			}
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x000AF50C File Offset: 0x000AD70C
		private void AddNamespace(string prefix, string xmlNamespace)
		{
			this.CheckSealed();
			if (xmlNamespace == null)
			{
				throw new ArgumentNullException("xmlNamespace");
			}
			if (prefix == null)
			{
				throw new ArgumentNullException("prefix");
			}
			int scopeCount = this._nsDeclarations[this._lastDecl].ScopeCount;
			if (this._lastDecl > 0)
			{
				int num = this._lastDecl - 1;
				while (num >= 0 && this._nsDeclarations[num].ScopeCount == scopeCount)
				{
					if (string.Equals(this._nsDeclarations[num].Prefix, prefix))
					{
						this._nsDeclarations[num].Uri = xmlNamespace;
						return;
					}
					num--;
				}
				if (this._lastDecl == this._nsDeclarations.Length - 1)
				{
					XmlnsDictionary.NamespaceDeclaration[] array = new XmlnsDictionary.NamespaceDeclaration[this._nsDeclarations.Length * 2];
					Array.Copy(this._nsDeclarations, 0, array, 0, this._nsDeclarations.Length);
					this._nsDeclarations = array;
				}
			}
			this._countDecl++;
			this._nsDeclarations[this._lastDecl].Prefix = prefix;
			this._nsDeclarations[this._lastDecl].Uri = xmlNamespace;
			this._lastDecl++;
			this._nsDeclarations[this._lastDecl].ScopeCount = scopeCount;
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x000AF654 File Offset: 0x000AD854
		private void RemoveNamespace(string prefix, string xmlNamespace)
		{
			this.CheckSealed();
			if (this._lastDecl > 0)
			{
				if (xmlNamespace == null)
				{
					throw new ArgumentNullException("xmlNamespace");
				}
				if (prefix == null)
				{
					throw new ArgumentNullException("prefix");
				}
				int scopeCount = this._nsDeclarations[this._lastDecl - 1].ScopeCount;
				int num = this._lastDecl - 1;
				while (num >= 0 && this._nsDeclarations[num].ScopeCount == scopeCount)
				{
					if (this._nsDeclarations[num].Prefix == prefix && this._nsDeclarations[num].Uri == xmlNamespace)
					{
						this._nsDeclarations[num].Uri = null;
						this._countDecl--;
					}
					num--;
				}
			}
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x000AF724 File Offset: 0x000AD924
		private IDictionary GetNamespacesInScope(XmlnsDictionary.NamespaceScope scope)
		{
			int i = 0;
			if (scope != XmlnsDictionary.NamespaceScope.All)
			{
				if (scope == XmlnsDictionary.NamespaceScope.Local)
				{
					i = this._lastDecl;
					int scopeCount = this._nsDeclarations[i].ScopeCount;
					while (this._nsDeclarations[i].ScopeCount == scopeCount)
					{
						i--;
					}
					i++;
				}
			}
			else
			{
				i = 0;
			}
			HybridDictionary hybridDictionary = new HybridDictionary(this._lastDecl - i + 1);
			while (i < this._lastDecl)
			{
				string prefix = this._nsDeclarations[i].Prefix;
				string uri = this._nsDeclarations[i].Uri;
				if (uri.Length > 0 || prefix.Length > 0)
				{
					hybridDictionary[prefix] = uri;
				}
				else
				{
					hybridDictionary.Remove(prefix);
				}
				i++;
			}
			return hybridDictionary;
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x000AF7E4 File Offset: 0x000AD9E4
		private bool HasNamespace(string prefix)
		{
			if (this._lastDecl > 0)
			{
				for (int i = this._lastDecl - 1; i >= 0; i--)
				{
					if (this._nsDeclarations[i].Prefix == prefix && this._nsDeclarations[i].Uri != null)
					{
						return prefix.Length > 0 || this._nsDeclarations[i].Uri.Length > 0;
					}
				}
			}
			return false;
		}

		// Token: 0x04001B11 RID: 6929
		private XmlnsDictionary.NamespaceDeclaration[] _nsDeclarations;

		// Token: 0x04001B12 RID: 6930
		private int _lastDecl;

		// Token: 0x04001B13 RID: 6931
		private int _countDecl;

		// Token: 0x04001B14 RID: 6932
		private bool _sealed;

		// Token: 0x0200089F RID: 2207
		private struct NamespaceDeclaration
		{
			// Token: 0x040041AC RID: 16812
			public string Prefix;

			// Token: 0x040041AD RID: 16813
			public string Uri;

			// Token: 0x040041AE RID: 16814
			public int ScopeCount;
		}

		// Token: 0x020008A0 RID: 2208
		private enum NamespaceScope
		{
			// Token: 0x040041B0 RID: 16816
			All,
			// Token: 0x040041B1 RID: 16817
			Local
		}
	}
}
