using System;
using System.ComponentModel;
using MS.Internal.Annotations;

namespace System.Windows.Annotations
{
	/// <summary>Represents an object that identifies an item of content.</summary>
	// Token: 0x020005CF RID: 1487
	public abstract class ContentLocatorBase : INotifyPropertyChanged2, INotifyPropertyChanged, IOwnedObject
	{
		// Token: 0x0600631B RID: 25371 RVA: 0x0000326D File Offset: 0x0000146D
		internal ContentLocatorBase()
		{
		}

		/// <summary>Creates a modifiable deep copy clone of this <see cref="T:System.Windows.Annotations.ContentLocatorBase" />.</summary>
		/// <returns>A modifiable deep copy clone of this <see cref="T:System.Windows.Annotations.ContentLocatorBase" />.</returns>
		// Token: 0x0600631C RID: 25372
		public abstract object Clone();

		/// <summary>For a description of this member, see <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged" />.</summary>
		// Token: 0x1400012C RID: 300
		// (add) Token: 0x0600631D RID: 25373 RVA: 0x001BDECA File Offset: 0x001BC0CA
		// (remove) Token: 0x0600631E RID: 25374 RVA: 0x001BDED3 File Offset: 0x001BC0D3
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChanged += value;
			}
			remove
			{
				this._propertyChanged -= value;
			}
		}

		// Token: 0x0600631F RID: 25375 RVA: 0x001BDEDC File Offset: 0x001BC0DC
		internal void FireLocatorChanged(string name)
		{
			if (this._propertyChanged != null)
			{
				this._propertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		// Token: 0x170017C9 RID: 6089
		// (get) Token: 0x06006320 RID: 25376 RVA: 0x001BDEF8 File Offset: 0x001BC0F8
		// (set) Token: 0x06006321 RID: 25377 RVA: 0x001BDF00 File Offset: 0x001BC100
		bool IOwnedObject.Owned
		{
			get
			{
				return this._owned;
			}
			set
			{
				this._owned = value;
			}
		}

		// Token: 0x06006322 RID: 25378
		internal abstract ContentLocatorBase Merge(ContentLocatorBase other);

		// Token: 0x1400012D RID: 301
		// (add) Token: 0x06006323 RID: 25379 RVA: 0x001BDF0C File Offset: 0x001BC10C
		// (remove) Token: 0x06006324 RID: 25380 RVA: 0x001BDF44 File Offset: 0x001BC144
		private event PropertyChangedEventHandler _propertyChanged;

		// Token: 0x040031C5 RID: 12741
		private bool _owned;
	}
}
