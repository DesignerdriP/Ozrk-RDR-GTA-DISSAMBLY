using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace MS.Internal.Annotations
{
	// Token: 0x020007CC RID: 1996
	internal class ObservableDictionary : IDictionary<string, string>, ICollection<KeyValuePair<string, string>>, IEnumerable<KeyValuePair<string, string>>, IEnumerable, INotifyPropertyChanged
	{
		// Token: 0x06007B97 RID: 31639 RVA: 0x0022BA51 File Offset: 0x00229C51
		public ObservableDictionary()
		{
			this._nameValues = new Dictionary<string, string>();
		}

		// Token: 0x06007B98 RID: 31640 RVA: 0x0022BA64 File Offset: 0x00229C64
		public void Add(string key, string val)
		{
			if (key == null || val == null)
			{
				throw new ArgumentNullException((key == null) ? "key" : "val");
			}
			this._nameValues.Add(key, val);
			this.FireDictionaryChanged();
		}

		// Token: 0x06007B99 RID: 31641 RVA: 0x0022BA94 File Offset: 0x00229C94
		public void Clear()
		{
			int count = this._nameValues.Count;
			if (count > 0)
			{
				this._nameValues.Clear();
				this.FireDictionaryChanged();
			}
		}

		// Token: 0x06007B9A RID: 31642 RVA: 0x0022BAC2 File Offset: 0x00229CC2
		public bool ContainsKey(string key)
		{
			return this._nameValues.ContainsKey(key);
		}

		// Token: 0x06007B9B RID: 31643 RVA: 0x0022BAD0 File Offset: 0x00229CD0
		public bool Remove(string key)
		{
			bool flag = this._nameValues.Remove(key);
			if (flag)
			{
				this.FireDictionaryChanged();
			}
			return flag;
		}

		// Token: 0x06007B9C RID: 31644 RVA: 0x0022BAF4 File Offset: 0x00229CF4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._nameValues.GetEnumerator();
		}

		// Token: 0x06007B9D RID: 31645 RVA: 0x0022BB06 File Offset: 0x00229D06
		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<string, string>>)this._nameValues).GetEnumerator();
		}

		// Token: 0x06007B9E RID: 31646 RVA: 0x0022BB13 File Offset: 0x00229D13
		public bool TryGetValue(string key, out string value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this._nameValues.TryGetValue(key, out value);
		}

		// Token: 0x06007B9F RID: 31647 RVA: 0x0022BB30 File Offset: 0x00229D30
		void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> pair)
		{
			((ICollection<KeyValuePair<string, string>>)this._nameValues).Add(pair);
		}

		// Token: 0x06007BA0 RID: 31648 RVA: 0x0022BB3E File Offset: 0x00229D3E
		bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> pair)
		{
			return ((ICollection<KeyValuePair<string, string>>)this._nameValues).Contains(pair);
		}

		// Token: 0x06007BA1 RID: 31649 RVA: 0x0022BB4C File Offset: 0x00229D4C
		bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> pair)
		{
			return ((ICollection<KeyValuePair<string, string>>)this._nameValues).Remove(pair);
		}

		// Token: 0x06007BA2 RID: 31650 RVA: 0x0022BB5A File Offset: 0x00229D5A
		void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] target, int startIndex)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (startIndex < 0 || startIndex > target.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			((ICollection<KeyValuePair<string, string>>)this._nameValues).CopyTo(target, startIndex);
		}

		// Token: 0x17001CD8 RID: 7384
		// (get) Token: 0x06007BA3 RID: 31651 RVA: 0x0022BB8C File Offset: 0x00229D8C
		public int Count
		{
			get
			{
				return this._nameValues.Count;
			}
		}

		// Token: 0x17001CD9 RID: 7385
		public string this[string key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				string result = null;
				this._nameValues.TryGetValue(key, out result);
				return result;
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				string text = null;
				this._nameValues.TryGetValue(key, out text);
				if (text == null || text != value)
				{
					this._nameValues[key] = value;
					this.FireDictionaryChanged();
				}
			}
		}

		// Token: 0x17001CDA RID: 7386
		// (get) Token: 0x06007BA6 RID: 31654 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001CDB RID: 7387
		// (get) Token: 0x06007BA7 RID: 31655 RVA: 0x0022BC25 File Offset: 0x00229E25
		public ICollection<string> Keys
		{
			get
			{
				return this._nameValues.Keys;
			}
		}

		// Token: 0x17001CDC RID: 7388
		// (get) Token: 0x06007BA8 RID: 31656 RVA: 0x0022BC32 File Offset: 0x00229E32
		public ICollection<string> Values
		{
			get
			{
				return this._nameValues.Values;
			}
		}

		// Token: 0x1400016D RID: 365
		// (add) Token: 0x06007BA9 RID: 31657 RVA: 0x0022BC40 File Offset: 0x00229E40
		// (remove) Token: 0x06007BAA RID: 31658 RVA: 0x0022BC78 File Offset: 0x00229E78
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06007BAB RID: 31659 RVA: 0x0022BCAD File Offset: 0x00229EAD
		private void FireDictionaryChanged()
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(null));
			}
		}

		// Token: 0x04003A30 RID: 14896
		private Dictionary<string, string> _nameValues;
	}
}
