using System;
using System.Collections;

namespace System.Windows.Markup.Localizer
{
	/// <summary>Defines a specialized enumerator that can enumerate over the content of a <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" /> object.</summary>
	// Token: 0x02000290 RID: 656
	public sealed class BamlLocalizationDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x060024E3 RID: 9443 RVA: 0x000B29F1 File Offset: 0x000B0BF1
		internal BamlLocalizationDictionaryEnumerator(IEnumerator enumerator)
		{
			this._enumerator = enumerator;
		}

		/// <summary>Moves to the next item in the collection. </summary>
		/// <returns>
		///     <see langword="true" /> if the enumerator successfully advances to the next element. If there are no remaining elements, this method returns <see langword="false" />.</returns>
		// Token: 0x060024E4 RID: 9444 RVA: 0x000B2A00 File Offset: 0x000B0C00
		public bool MoveNext()
		{
			return this._enumerator.MoveNext();
		}

		/// <summary>Returns the enumerator to its initial position, which is before the first object in the collection. </summary>
		// Token: 0x060024E5 RID: 9445 RVA: 0x000B2A0D File Offset: 0x000B0C0D
		public void Reset()
		{
			this._enumerator.Reset();
		}

		/// <summary>Gets the current position's <see cref="T:System.Collections.DictionaryEntry" /> object. </summary>
		/// <returns>An object containing the key and value of the entry at the current position.</returns>
		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x060024E6 RID: 9446 RVA: 0x000B2A1A File Offset: 0x000B0C1A
		public DictionaryEntry Entry
		{
			get
			{
				return (DictionaryEntry)this._enumerator.Current;
			}
		}

		/// <summary>Gets the key of the current entry. </summary>
		/// <returns>The key of the current entry.</returns>
		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x060024E7 RID: 9447 RVA: 0x000B2A2C File Offset: 0x000B0C2C
		public BamlLocalizableResourceKey Key
		{
			get
			{
				return (BamlLocalizableResourceKey)this.Entry.Key;
			}
		}

		/// <summary>Gets the value of the current entry. </summary>
		/// <returns>The value of the current entry.</returns>
		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x060024E8 RID: 9448 RVA: 0x000B2A4C File Offset: 0x000B0C4C
		public BamlLocalizableResource Value
		{
			get
			{
				return (BamlLocalizableResource)this.Entry.Value;
			}
		}

		/// <summary>Gets the current object in the collection. </summary>
		/// <returns>The current object.</returns>
		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x060024E9 RID: 9449 RVA: 0x000B2A6C File Offset: 0x000B0C6C
		public DictionaryEntry Current
		{
			get
			{
				return this.Entry;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IEnumerator.Current" />.</summary>
		/// <returns>The current element in the collection.</returns>
		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x060024EA RID: 9450 RVA: 0x000B2A74 File Offset: 0x000B0C74
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IDictionaryEnumerator.Key" />.</summary>
		/// <returns>The key of the current element of the enumeration.</returns>
		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x060024EB RID: 9451 RVA: 0x000B2A81 File Offset: 0x000B0C81
		object IDictionaryEnumerator.Key
		{
			get
			{
				return this.Key;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IDictionaryEnumerator.Value" />.</summary>
		/// <returns>The value of the current element of the enumeration.</returns>
		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x060024EC RID: 9452 RVA: 0x000B2A89 File Offset: 0x000B0C89
		object IDictionaryEnumerator.Value
		{
			get
			{
				return this.Value;
			}
		}

		// Token: 0x04001B50 RID: 6992
		private IEnumerator _enumerator;
	}
}
