using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Annotations;

namespace MS.Internal.Annotations
{
	// Token: 0x020007C5 RID: 1989
	internal sealed class AnnotationResourceCollection : AnnotationObservableCollection<AnnotationResource>
	{
		// Token: 0x1400016C RID: 364
		// (add) Token: 0x06007B7B RID: 31611 RVA: 0x0022B86C File Offset: 0x00229A6C
		// (remove) Token: 0x06007B7C RID: 31612 RVA: 0x0022B8A4 File Offset: 0x00229AA4
		public event PropertyChangedEventHandler ItemChanged;

		// Token: 0x06007B7D RID: 31613 RVA: 0x0022B8DC File Offset: 0x00229ADC
		protected override void ProtectedClearItems()
		{
			List<AnnotationResource> list = new List<AnnotationResource>(this);
			base.Items.Clear();
			this.OnPropertyChanged(this.CountString);
			this.OnPropertyChanged(this.IndexerName);
			this.OnCollectionCleared(list);
		}

		// Token: 0x06007B7E RID: 31614 RVA: 0x0022B91A File Offset: 0x00229B1A
		protected override void ProtectedSetItem(int index, AnnotationResource item)
		{
			base.ObservableCollectionSetItem(index, item);
		}

		// Token: 0x06007B7F RID: 31615 RVA: 0x0022B924 File Offset: 0x00229B24
		private void OnCollectionCleared(IEnumerable<AnnotationResource> list)
		{
			foreach (object changedItem in list)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, changedItem, 0));
			}
		}

		// Token: 0x06007B80 RID: 31616 RVA: 0x0022B974 File Offset: 0x00229B74
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x06007B81 RID: 31617 RVA: 0x0022B982 File Offset: 0x00229B82
		protected override void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this.ItemChanged != null)
			{
				this.ItemChanged(sender, e);
			}
		}
	}
}
