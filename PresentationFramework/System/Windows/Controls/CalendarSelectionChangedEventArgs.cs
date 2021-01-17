using System;
using System.Collections;

namespace System.Windows.Controls
{
	// Token: 0x0200047D RID: 1149
	internal class CalendarSelectionChangedEventArgs : SelectionChangedEventArgs
	{
		// Token: 0x06004327 RID: 17191 RVA: 0x0013333A File Offset: 0x0013153A
		public CalendarSelectionChangedEventArgs(RoutedEvent eventId, IList removedItems, IList addedItems) : base(eventId, removedItems, addedItems)
		{
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x00133348 File Offset: 0x00131548
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			EventHandler<SelectionChangedEventArgs> eventHandler = genericHandler as EventHandler<SelectionChangedEventArgs>;
			if (eventHandler != null)
			{
				eventHandler(genericTarget, this);
				return;
			}
			base.InvokeEventHandler(genericHandler, genericTarget);
		}
	}
}
