using System;
using System.Collections;
using System.Security;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using MS.Internal.KnownBoxes;

namespace MS.Internal.AppModel
{
	// Token: 0x0200078F RID: 1935
	internal class JournalNavigationScope : DependencyObject, INavigator, INavigatorBase
	{
		// Token: 0x06007979 RID: 31097 RVA: 0x00227493 File Offset: 0x00225693
		internal JournalNavigationScope(IJournalNavigationScopeHost host)
		{
			this._host = host;
			this._rootNavSvc = host.NavigationService;
		}

		// Token: 0x17001CAE RID: 7342
		// (get) Token: 0x0600797A RID: 31098 RVA: 0x002274AE File Offset: 0x002256AE
		// (set) Token: 0x0600797B RID: 31099 RVA: 0x002274BB File Offset: 0x002256BB
		public Uri Source
		{
			get
			{
				return this._host.Source;
			}
			set
			{
				this._host.Source = value;
			}
		}

		// Token: 0x17001CAF RID: 7343
		// (get) Token: 0x0600797C RID: 31100 RVA: 0x002274C9 File Offset: 0x002256C9
		public Uri CurrentSource
		{
			get
			{
				return this._host.CurrentSource;
			}
		}

		// Token: 0x17001CB0 RID: 7344
		// (get) Token: 0x0600797D RID: 31101 RVA: 0x002274D6 File Offset: 0x002256D6
		// (set) Token: 0x0600797E RID: 31102 RVA: 0x002274E3 File Offset: 0x002256E3
		public object Content
		{
			get
			{
				return this._host.Content;
			}
			set
			{
				this._host.Content = value;
			}
		}

		// Token: 0x0600797F RID: 31103 RVA: 0x002274F1 File Offset: 0x002256F1
		public bool Navigate(Uri source)
		{
			return this._host.Navigate(source);
		}

		// Token: 0x06007980 RID: 31104 RVA: 0x002274FF File Offset: 0x002256FF
		public bool Navigate(Uri source, object extraData)
		{
			return this._host.Navigate(source, extraData);
		}

		// Token: 0x06007981 RID: 31105 RVA: 0x0022750E File Offset: 0x0022570E
		public bool Navigate(object content)
		{
			return this._host.Navigate(content);
		}

		// Token: 0x06007982 RID: 31106 RVA: 0x0022751C File Offset: 0x0022571C
		public bool Navigate(object content, object extraData)
		{
			return this._host.Navigate(content, extraData);
		}

		// Token: 0x06007983 RID: 31107 RVA: 0x0022752B File Offset: 0x0022572B
		public void StopLoading()
		{
			this._host.StopLoading();
		}

		// Token: 0x06007984 RID: 31108 RVA: 0x00227538 File Offset: 0x00225738
		public void Refresh()
		{
			this._host.Refresh();
		}

		// Token: 0x14000165 RID: 357
		// (add) Token: 0x06007985 RID: 31109 RVA: 0x00227545 File Offset: 0x00225745
		// (remove) Token: 0x06007986 RID: 31110 RVA: 0x00227553 File Offset: 0x00225753
		public event NavigatingCancelEventHandler Navigating
		{
			add
			{
				this._host.Navigating += value;
			}
			remove
			{
				this._host.Navigating -= value;
			}
		}

		// Token: 0x14000166 RID: 358
		// (add) Token: 0x06007987 RID: 31111 RVA: 0x00227561 File Offset: 0x00225761
		// (remove) Token: 0x06007988 RID: 31112 RVA: 0x0022756F File Offset: 0x0022576F
		public event NavigationProgressEventHandler NavigationProgress
		{
			add
			{
				this._host.NavigationProgress += value;
			}
			remove
			{
				this._host.NavigationProgress -= value;
			}
		}

		// Token: 0x14000167 RID: 359
		// (add) Token: 0x06007989 RID: 31113 RVA: 0x0022757D File Offset: 0x0022577D
		// (remove) Token: 0x0600798A RID: 31114 RVA: 0x0022758B File Offset: 0x0022578B
		public event NavigationFailedEventHandler NavigationFailed
		{
			add
			{
				this._host.NavigationFailed += value;
			}
			remove
			{
				this._host.NavigationFailed -= value;
			}
		}

		// Token: 0x14000168 RID: 360
		// (add) Token: 0x0600798B RID: 31115 RVA: 0x00227599 File Offset: 0x00225799
		// (remove) Token: 0x0600798C RID: 31116 RVA: 0x002275A7 File Offset: 0x002257A7
		public event NavigatedEventHandler Navigated
		{
			add
			{
				this._host.Navigated += value;
			}
			remove
			{
				this._host.Navigated -= value;
			}
		}

		// Token: 0x14000169 RID: 361
		// (add) Token: 0x0600798D RID: 31117 RVA: 0x002275B5 File Offset: 0x002257B5
		// (remove) Token: 0x0600798E RID: 31118 RVA: 0x002275C3 File Offset: 0x002257C3
		public event LoadCompletedEventHandler LoadCompleted
		{
			add
			{
				this._host.LoadCompleted += value;
			}
			remove
			{
				this._host.LoadCompleted -= value;
			}
		}

		// Token: 0x1400016A RID: 362
		// (add) Token: 0x0600798F RID: 31119 RVA: 0x002275D1 File Offset: 0x002257D1
		// (remove) Token: 0x06007990 RID: 31120 RVA: 0x002275DF File Offset: 0x002257DF
		public event NavigationStoppedEventHandler NavigationStopped
		{
			add
			{
				this._host.NavigationStopped += value;
			}
			remove
			{
				this._host.NavigationStopped -= value;
			}
		}

		// Token: 0x1400016B RID: 363
		// (add) Token: 0x06007991 RID: 31121 RVA: 0x002275ED File Offset: 0x002257ED
		// (remove) Token: 0x06007992 RID: 31122 RVA: 0x002275FB File Offset: 0x002257FB
		public event FragmentNavigationEventHandler FragmentNavigation
		{
			add
			{
				this._host.FragmentNavigation += value;
			}
			remove
			{
				this._host.FragmentNavigation -= value;
			}
		}

		// Token: 0x17001CB1 RID: 7345
		// (get) Token: 0x06007993 RID: 31123 RVA: 0x00227609 File Offset: 0x00225809
		public bool CanGoForward
		{
			get
			{
				this._host.VerifyContextAndObjectState();
				return this._journal != null && !this.InAppShutdown && this._journal.CanGoForward;
			}
		}

		// Token: 0x17001CB2 RID: 7346
		// (get) Token: 0x06007994 RID: 31124 RVA: 0x00227633 File Offset: 0x00225833
		public bool CanGoBack
		{
			get
			{
				this._host.VerifyContextAndObjectState();
				return this._journal != null && !this.InAppShutdown && this._journal.CanGoBack;
			}
		}

		// Token: 0x06007995 RID: 31125 RVA: 0x00227660 File Offset: 0x00225860
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void GoForward()
		{
			if (!this.CanGoForward)
			{
				throw new InvalidOperationException(SR.Get("NoForwardEntry"));
			}
			if (!this._host.GoForwardOverride())
			{
				JournalEntry journalEntry = this.Journal.BeginForwardNavigation();
				if (journalEntry == null)
				{
					this._rootNavSvc.StopLoading();
					return;
				}
				this.NavigateToEntry(journalEntry);
			}
		}

		// Token: 0x06007996 RID: 31126 RVA: 0x002276B8 File Offset: 0x002258B8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void GoBack()
		{
			if (!this.CanGoBack)
			{
				throw new InvalidOperationException(SR.Get("NoBackEntry"));
			}
			if (!this._host.GoBackOverride())
			{
				JournalEntry journalEntry = this.Journal.BeginBackNavigation();
				if (journalEntry == null)
				{
					this._rootNavSvc.StopLoading();
					return;
				}
				this.NavigateToEntry(journalEntry);
			}
		}

		// Token: 0x06007997 RID: 31127 RVA: 0x0022770D File Offset: 0x0022590D
		public void AddBackEntry(CustomContentState state)
		{
			this._host.VerifyContextAndObjectState();
			this._rootNavSvc.AddBackEntry(state);
		}

		// Token: 0x06007998 RID: 31128 RVA: 0x00227726 File Offset: 0x00225926
		public JournalEntry RemoveBackEntry()
		{
			this._host.VerifyContextAndObjectState();
			if (this._journal != null)
			{
				return this._journal.RemoveBackEntry();
			}
			return null;
		}

		// Token: 0x17001CB3 RID: 7347
		// (get) Token: 0x06007999 RID: 31129 RVA: 0x00227748 File Offset: 0x00225948
		public IEnumerable BackStack
		{
			get
			{
				this._host.VerifyContextAndObjectState();
				return this.Journal.BackStack;
			}
		}

		// Token: 0x17001CB4 RID: 7348
		// (get) Token: 0x0600799A RID: 31130 RVA: 0x00227760 File Offset: 0x00225960
		public IEnumerable ForwardStack
		{
			get
			{
				this._host.VerifyContextAndObjectState();
				return this.Journal.ForwardStack;
			}
		}

		// Token: 0x0600799B RID: 31131 RVA: 0x0001B7E3 File Offset: 0x000199E3
		JournalNavigationScope INavigator.GetJournal(bool create)
		{
			return this;
		}

		// Token: 0x0600799C RID: 31132 RVA: 0x00227778 File Offset: 0x00225978
		internal void EnsureJournal()
		{
			Journal journal = this.Journal;
		}

		// Token: 0x0600799D RID: 31133 RVA: 0x0022778C File Offset: 0x0022598C
		internal bool CanInvokeJournalEntry(int entryId)
		{
			if (this._journal == null)
			{
				return false;
			}
			int num = this._journal.FindIndexForEntryWithId(entryId);
			if (num == -1)
			{
				return false;
			}
			JournalEntry entry = this._journal[num];
			return this._journal.IsNavigable(entry);
		}

		// Token: 0x0600799E RID: 31134 RVA: 0x002277D0 File Offset: 0x002259D0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal bool NavigateToEntry(int index)
		{
			JournalEntry entry = this.Journal[index];
			return this.NavigateToEntry(entry);
		}

		// Token: 0x0600799F RID: 31135 RVA: 0x002277F4 File Offset: 0x002259F4
		internal bool NavigateToEntry(JournalEntry entry)
		{
			if (entry == null)
			{
				return false;
			}
			if (!this.Journal.IsNavigable(entry))
			{
				return false;
			}
			NavigationService navigationService = this._rootNavSvc.FindTarget(entry.NavigationServiceId);
			NavigationMode navigationMode = this.Journal.GetNavigationMode(entry);
			bool flag = false;
			try
			{
				flag = entry.Navigate(navigationService.INavigatorHost, navigationMode);
			}
			finally
			{
				if (!flag)
				{
					this.AbortJournalNavigation();
				}
			}
			return flag;
		}

		// Token: 0x060079A0 RID: 31136 RVA: 0x00227864 File Offset: 0x00225A64
		internal void AbortJournalNavigation()
		{
			if (this._journal != null)
			{
				this._journal.AbortJournalNavigation();
			}
		}

		// Token: 0x060079A1 RID: 31137 RVA: 0x00227879 File Offset: 0x00225A79
		internal INavigatorBase FindTarget(string name)
		{
			return this._rootNavSvc.FindTarget(name);
		}

		// Token: 0x060079A2 RID: 31138 RVA: 0x00227887 File Offset: 0x00225A87
		internal static void ClearDPValues(DependencyObject navigator)
		{
			navigator.SetValue(JournalNavigationScope.CanGoBackPropertyKey, BooleanBoxes.FalseBox);
			navigator.SetValue(JournalNavigationScope.CanGoForwardPropertyKey, BooleanBoxes.FalseBox);
			navigator.SetValue(JournalNavigationScope.BackStackPropertyKey, null);
			navigator.SetValue(JournalNavigationScope.ForwardStackPropertyKey, null);
		}

		// Token: 0x17001CB5 RID: 7349
		// (get) Token: 0x060079A3 RID: 31139 RVA: 0x002278C1 File Offset: 0x00225AC1
		// (set) Token: 0x060079A4 RID: 31140 RVA: 0x002278DC File Offset: 0x00225ADC
		internal Journal Journal
		{
			get
			{
				if (this._journal == null)
				{
					this.Journal = new Journal();
				}
				return this._journal;
			}
			set
			{
				this._journal = value;
				this._journal.Filter = new JournalEntryFilter(this.IsEntryNavigable);
				this._journal.BackForwardStateChange += this.OnBackForwardStateChange;
				DependencyObject dependencyObject = (DependencyObject)this._host;
				dependencyObject.SetValue(JournalNavigationScope.BackStackPropertyKey, this._journal.BackStack);
				dependencyObject.SetValue(JournalNavigationScope.ForwardStackPropertyKey, this._journal.ForwardStack);
				this._host.OnJournalAvailable();
			}
		}

		// Token: 0x17001CB6 RID: 7350
		// (get) Token: 0x060079A5 RID: 31141 RVA: 0x00227961 File Offset: 0x00225B61
		internal NavigationService RootNavigationService
		{
			get
			{
				return this._rootNavSvc;
			}
		}

		// Token: 0x17001CB7 RID: 7351
		// (get) Token: 0x060079A6 RID: 31142 RVA: 0x00227969 File Offset: 0x00225B69
		internal INavigatorBase NavigatorHost
		{
			get
			{
				return this._host;
			}
		}

		// Token: 0x060079A7 RID: 31143 RVA: 0x00227974 File Offset: 0x00225B74
		private void OnBackForwardStateChange(object sender, EventArgs e)
		{
			DependencyObject dependencyObject = (DependencyObject)this._host;
			bool flag = false;
			bool flag2 = this._journal.CanGoBack;
			if (flag2 != (bool)dependencyObject.GetValue(JournalNavigationScope.CanGoBackProperty))
			{
				dependencyObject.SetValue(JournalNavigationScope.CanGoBackPropertyKey, BooleanBoxes.Box(flag2));
				flag = true;
			}
			flag2 = this._journal.CanGoForward;
			if (flag2 != (bool)dependencyObject.GetValue(JournalNavigationScope.CanGoForwardProperty))
			{
				dependencyObject.SetValue(JournalNavigationScope.CanGoForwardPropertyKey, BooleanBoxes.Box(flag2));
				flag = true;
			}
			if (flag)
			{
				CommandManager.InvalidateRequerySuggested();
			}
		}

		// Token: 0x060079A8 RID: 31144 RVA: 0x002279FC File Offset: 0x00225BFC
		private bool IsEntryNavigable(JournalEntry entry)
		{
			if (entry == null || !entry.IsNavigable())
			{
				return false;
			}
			NavigationService navigationService = this._rootNavSvc.FindTarget(entry.NavigationServiceId);
			return navigationService != null && (navigationService.ContentId == entry.ContentId || entry.JEGroupState.GroupExitEntry == entry);
		}

		// Token: 0x17001CB8 RID: 7352
		// (get) Token: 0x060079A9 RID: 31145 RVA: 0x000C253B File Offset: 0x000C073B
		private bool InAppShutdown
		{
			get
			{
				return Application.IsShuttingDown;
			}
		}

		// Token: 0x04003985 RID: 14725
		private static readonly DependencyPropertyKey CanGoBackPropertyKey = DependencyProperty.RegisterReadOnly("CanGoBack", typeof(bool), typeof(JournalNavigationScope), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003986 RID: 14726
		internal static readonly DependencyProperty CanGoBackProperty = JournalNavigationScope.CanGoBackPropertyKey.DependencyProperty;

		// Token: 0x04003987 RID: 14727
		private static readonly DependencyPropertyKey CanGoForwardPropertyKey = DependencyProperty.RegisterReadOnly("CanGoForward", typeof(bool), typeof(JournalNavigationScope), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003988 RID: 14728
		internal static readonly DependencyProperty CanGoForwardProperty = JournalNavigationScope.CanGoForwardPropertyKey.DependencyProperty;

		// Token: 0x04003989 RID: 14729
		private static readonly DependencyPropertyKey BackStackPropertyKey = DependencyProperty.RegisterReadOnly("BackStack", typeof(IEnumerable), typeof(JournalNavigationScope), new FrameworkPropertyMetadata(null));

		// Token: 0x0400398A RID: 14730
		internal static readonly DependencyProperty BackStackProperty = JournalNavigationScope.BackStackPropertyKey.DependencyProperty;

		// Token: 0x0400398B RID: 14731
		private static readonly DependencyPropertyKey ForwardStackPropertyKey = DependencyProperty.RegisterReadOnly("ForwardStack", typeof(IEnumerable), typeof(JournalNavigationScope), new FrameworkPropertyMetadata(null));

		// Token: 0x0400398C RID: 14732
		internal static readonly DependencyProperty ForwardStackProperty = JournalNavigationScope.ForwardStackPropertyKey.DependencyProperty;

		// Token: 0x0400398D RID: 14733
		private IJournalNavigationScopeHost _host;

		// Token: 0x0400398E RID: 14734
		private NavigationService _rootNavSvc;

		// Token: 0x0400398F RID: 14735
		private Journal _journal;
	}
}
