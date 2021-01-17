using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Win32;

namespace System.Windows.Navigation
{
	// Token: 0x02000300 RID: 768
	[Serializable]
	internal sealed class Journal : ISerializable
	{
		// Token: 0x060028C5 RID: 10437 RVA: 0x000BD305 File Offset: 0x000BB505
		internal Journal()
		{
			this._Initialize();
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x000BD329 File Offset: 0x000BB529
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_journalEntryList", this._journalEntryList);
			info.AddValue("_currentEntryIndex", this._currentEntryIndex);
			info.AddValue("_journalEntryId", this._journalEntryId);
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x000BD360 File Offset: 0x000BB560
		private Journal(SerializationInfo info, StreamingContext context)
		{
			this._Initialize();
			this._journalEntryList = (List<JournalEntry>)info.GetValue("_journalEntryList", typeof(List<JournalEntry>));
			this._currentEntryIndex = info.GetInt32("_currentEntryIndex");
			this._uncommittedCurrentIndex = this._currentEntryIndex;
			this._journalEntryId = info.GetInt32("_journalEntryId");
		}

		// Token: 0x170009D6 RID: 2518
		internal JournalEntry this[int index]
		{
			get
			{
				return this._journalEntryList[index];
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x060028C9 RID: 10441 RVA: 0x000BD3EB File Offset: 0x000BB5EB
		internal int TotalCount
		{
			get
			{
				return this._journalEntryList.Count;
			}
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x060028CA RID: 10442 RVA: 0x000BD3F8 File Offset: 0x000BB5F8
		internal int CurrentIndex
		{
			get
			{
				return this._currentEntryIndex;
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x060028CB RID: 10443 RVA: 0x000BD400 File Offset: 0x000BB600
		internal JournalEntry CurrentEntry
		{
			get
			{
				if (this._currentEntryIndex >= 0 && this._currentEntryIndex < this.TotalCount)
				{
					return this._journalEntryList[this._currentEntryIndex];
				}
				return null;
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x060028CC RID: 10444 RVA: 0x000BD42C File Offset: 0x000BB62C
		internal bool HasUncommittedNavigation
		{
			get
			{
				return this._uncommittedCurrentIndex != this._currentEntryIndex;
			}
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x060028CD RID: 10445 RVA: 0x000BD43F File Offset: 0x000BB63F
		internal JournalEntryStack BackStack
		{
			get
			{
				return this._backStack;
			}
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x060028CE RID: 10446 RVA: 0x000BD447 File Offset: 0x000BB647
		internal JournalEntryStack ForwardStack
		{
			get
			{
				return this._forwardStack;
			}
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x060028CF RID: 10447 RVA: 0x000BD44F File Offset: 0x000BB64F
		internal bool CanGoBack
		{
			get
			{
				return this.GetGoBackEntry() != null;
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x060028D0 RID: 10448 RVA: 0x000BD45C File Offset: 0x000BB65C
		internal bool CanGoForward
		{
			get
			{
				int num;
				this.GetGoForwardEntryIndex(out num);
				return num != -1;
			}
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x060028D1 RID: 10449 RVA: 0x000BD478 File Offset: 0x000BB678
		internal int Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x060028D2 RID: 10450 RVA: 0x000BD480 File Offset: 0x000BB680
		// (set) Token: 0x060028D3 RID: 10451 RVA: 0x000BD488 File Offset: 0x000BB688
		internal JournalEntryFilter Filter
		{
			get
			{
				return this._filter;
			}
			set
			{
				this._filter = value;
				this.BackStack.Filter = this._filter;
				this.ForwardStack.Filter = this._filter;
			}
		}

		// Token: 0x1400005A RID: 90
		// (add) Token: 0x060028D4 RID: 10452 RVA: 0x000BD4B3 File Offset: 0x000BB6B3
		// (remove) Token: 0x060028D5 RID: 10453 RVA: 0x000BD4CC File Offset: 0x000BB6CC
		internal event EventHandler BackForwardStateChange
		{
			add
			{
				this._backForwardStateChange = (EventHandler)Delegate.Combine(this._backForwardStateChange, value);
			}
			remove
			{
				this._backForwardStateChange = (EventHandler)Delegate.Remove(this._backForwardStateChange, value);
			}
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x000BD4E8 File Offset: 0x000BB6E8
		internal JournalEntry RemoveBackEntry()
		{
			int num = this._currentEntryIndex;
			while (--num >= 0)
			{
				if (this.IsNavigable(this._journalEntryList[num]))
				{
					JournalEntry result = this.RemoveEntryInternal(num);
					this.UpdateView();
					return result;
				}
			}
			return null;
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x000BD52C File Offset: 0x000BB72C
		internal void UpdateCurrentEntry(JournalEntry journalEntry)
		{
			if (journalEntry == null)
			{
				throw new ArgumentNullException("journalEntry");
			}
			if (this._currentEntryIndex > -1 && this._currentEntryIndex < this.TotalCount)
			{
				JournalEntry journalEntry2 = this._journalEntryList[this._currentEntryIndex];
				journalEntry.Id = journalEntry2.Id;
				this._journalEntryList[this._currentEntryIndex] = journalEntry;
			}
			else
			{
				int num = this._journalEntryId + 1;
				this._journalEntryId = num;
				journalEntry.Id = num;
				this._journalEntryList.Add(journalEntry);
			}
			this._version++;
			journalEntry.JEGroupState.GroupExitEntry = journalEntry;
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x000BD5CD File Offset: 0x000BB7CD
		internal void RecordNewNavigation()
		{
			Invariant.Assert(this.ValidateIndexes());
			this._currentEntryIndex++;
			this._uncommittedCurrentIndex = this._currentEntryIndex;
			if (!this.ClearForwardStack())
			{
				this.UpdateView();
			}
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x000BD604 File Offset: 0x000BB804
		internal bool ClearForwardStack()
		{
			if (this._currentEntryIndex >= this.TotalCount)
			{
				return false;
			}
			if (this._uncommittedCurrentIndex > this._currentEntryIndex)
			{
				throw new InvalidOperationException(SR.Get("InvalidOperation_CannotClearFwdStack"));
			}
			this._journalEntryList.RemoveRange(this._currentEntryIndex, this._journalEntryList.Count - this._currentEntryIndex);
			this.UpdateView();
			return true;
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x000BD669 File Offset: 0x000BB869
		internal void CommitJournalNavigation(JournalEntry navigated)
		{
			this.NavigateTo(navigated);
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x000BD672 File Offset: 0x000BB872
		internal void AbortJournalNavigation()
		{
			this._uncommittedCurrentIndex = this._currentEntryIndex;
			this.UpdateView();
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x000BD688 File Offset: 0x000BB888
		internal JournalEntry BeginBackNavigation()
		{
			Invariant.Assert(this.ValidateIndexes());
			int uncommittedCurrentIndex;
			JournalEntry goBackEntry = this.GetGoBackEntry(out uncommittedCurrentIndex);
			if (goBackEntry == null)
			{
				throw new InvalidOperationException(SR.Get("NoBackEntry"));
			}
			this._uncommittedCurrentIndex = uncommittedCurrentIndex;
			this.UpdateView();
			if (this._uncommittedCurrentIndex == this._currentEntryIndex)
			{
				return null;
			}
			return goBackEntry;
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x000BD6DC File Offset: 0x000BB8DC
		internal JournalEntry BeginForwardNavigation()
		{
			Invariant.Assert(this.ValidateIndexes());
			int num;
			this.GetGoForwardEntryIndex(out num);
			if (num == -1)
			{
				throw new InvalidOperationException(SR.Get("NoForwardEntry"));
			}
			this._uncommittedCurrentIndex = num;
			this.UpdateView();
			if (num == this._currentEntryIndex)
			{
				return null;
			}
			return this._journalEntryList[num];
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x000BD734 File Offset: 0x000BB934
		internal NavigationMode GetNavigationMode(JournalEntry entry)
		{
			int num = this._journalEntryList.IndexOf(entry);
			if (num <= this._currentEntryIndex)
			{
				return NavigationMode.Back;
			}
			return NavigationMode.Forward;
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x000BD75C File Offset: 0x000BB95C
		internal void NavigateTo(JournalEntry target)
		{
			int num = this._journalEntryList.IndexOf(target);
			if (num > -1)
			{
				this._currentEntryIndex = num;
				this._uncommittedCurrentIndex = this._currentEntryIndex;
				this.UpdateView();
			}
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x000BD794 File Offset: 0x000BB994
		internal int FindIndexForEntryWithId(int id)
		{
			for (int i = 0; i < this.TotalCount; i++)
			{
				if (this[i].Id == id)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x000BD7C4 File Offset: 0x000BB9C4
		internal void PruneKeepAliveEntries()
		{
			for (int i = this.TotalCount - 1; i >= 0; i--)
			{
				JournalEntry journalEntry = this._journalEntryList[i];
				if (journalEntry.IsAlive())
				{
					this.RemoveEntryInternal(i);
				}
				else
				{
					DataStreams journalDataStreams = journalEntry.JEGroupState.JournalDataStreams;
					if (journalDataStreams != null)
					{
						journalDataStreams.PrepareForSerialization();
					}
					if (journalEntry.RootViewerState != null)
					{
						journalEntry.RootViewerState.PrepareForSerialization();
					}
				}
			}
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x000BD82C File Offset: 0x000BBA2C
		internal JournalEntry RemoveEntryInternal(int index)
		{
			JournalEntry result = this._journalEntryList[index];
			this._version++;
			this._journalEntryList.RemoveAt(index);
			if (this._currentEntryIndex > index)
			{
				this._currentEntryIndex--;
			}
			if (this._uncommittedCurrentIndex > index)
			{
				this._uncommittedCurrentIndex--;
			}
			return result;
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x000BD890 File Offset: 0x000BBA90
		internal void RemoveEntries(Guid navSvcId)
		{
			for (int i = this.TotalCount - 1; i >= 0; i--)
			{
				if (i != this._currentEntryIndex)
				{
					JournalEntry journalEntry = this._journalEntryList[i];
					if (journalEntry.NavigationServiceId == navSvcId)
					{
						this.RemoveEntryInternal(i);
					}
				}
			}
			this.UpdateView();
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x000BD8E2 File Offset: 0x000BBAE2
		internal void UpdateView()
		{
			this.BackStack.OnCollectionChanged();
			this.ForwardStack.OnCollectionChanged();
			if (this._backForwardStateChange != null)
			{
				this._backForwardStateChange(this, EventArgs.Empty);
			}
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x000BD914 File Offset: 0x000BBB14
		internal JournalEntry GetGoBackEntry(out int index)
		{
			for (index = this._uncommittedCurrentIndex - 1; index >= 0; index--)
			{
				JournalEntry journalEntry = this._journalEntryList[index];
				if (this.IsNavigable(journalEntry))
				{
					return journalEntry;
				}
			}
			return null;
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x000BD954 File Offset: 0x000BBB54
		internal JournalEntry GetGoBackEntry()
		{
			int num;
			return this.GetGoBackEntry(out num);
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x000BD969 File Offset: 0x000BBB69
		internal void GetGoForwardEntryIndex(out int index)
		{
			index = this._uncommittedCurrentIndex;
			for (;;)
			{
				index++;
				if (index == this._currentEntryIndex)
				{
					break;
				}
				if (index >= this.TotalCount)
				{
					goto Block_2;
				}
				if (this.IsNavigable(this._journalEntryList[index]))
				{
					return;
				}
			}
			return;
			Block_2:
			index = -1;
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x000BD9A7 File Offset: 0x000BBBA7
		private bool ValidateIndexes()
		{
			return this._currentEntryIndex >= 0 && this._currentEntryIndex <= this.TotalCount && this._uncommittedCurrentIndex >= 0 && this._uncommittedCurrentIndex <= this.TotalCount;
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x000BD9DC File Offset: 0x000BBBDC
		private void _Initialize()
		{
			this._backStack = new JournalEntryBackStack(this);
			this._forwardStack = new JournalEntryForwardStack(this);
		}

		// Token: 0x060028EA RID: 10474 RVA: 0x000BD9F6 File Offset: 0x000BBBF6
		internal bool IsNavigable(JournalEntry entry)
		{
			if (entry == null)
			{
				return false;
			}
			if (this.Filter == null)
			{
				return entry.IsNavigable();
			}
			return this.Filter(entry);
		}

		// Token: 0x04001BB6 RID: 7094
		[NonSerialized]
		private EventHandler _backForwardStateChange;

		// Token: 0x04001BB7 RID: 7095
		private JournalEntryFilter _filter;

		// Token: 0x04001BB8 RID: 7096
		private JournalEntryBackStack _backStack;

		// Token: 0x04001BB9 RID: 7097
		private JournalEntryForwardStack _forwardStack;

		// Token: 0x04001BBA RID: 7098
		private int _journalEntryId = SafeNativeMethods.GetTickCount();

		// Token: 0x04001BBB RID: 7099
		private List<JournalEntry> _journalEntryList = new List<JournalEntry>();

		// Token: 0x04001BBC RID: 7100
		private int _currentEntryIndex;

		// Token: 0x04001BBD RID: 7101
		private int _uncommittedCurrentIndex;

		// Token: 0x04001BBE RID: 7102
		private int _version;
	}
}
