using System;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x0200078A RID: 1930
	[Serializable]
	internal abstract class JournalEntryPageFunction : JournalEntry, ISerializable
	{
		// Token: 0x06007957 RID: 31063 RVA: 0x00226FB3 File Offset: 0x002251B3
		internal JournalEntryPageFunction(JournalEntryGroupState jeGroupState, PageFunctionBase pageFunction) : base(jeGroupState, null)
		{
			this.PageFunctionId = pageFunction.PageFunctionId;
			this.ParentPageFunctionId = pageFunction.ParentPageFunctionId;
		}

		// Token: 0x06007958 RID: 31064 RVA: 0x00226FD8 File Offset: 0x002251D8
		protected JournalEntryPageFunction(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._pageFunctionId = (Guid)info.GetValue("_pageFunctionId", typeof(Guid));
			this._parentPageFunctionId = (Guid)info.GetValue("_parentPageFunctionId", typeof(Guid));
		}

		// Token: 0x06007959 RID: 31065 RVA: 0x0022702D File Offset: 0x0022522D
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_pageFunctionId", this._pageFunctionId);
			info.AddValue("_parentPageFunctionId", this._parentPageFunctionId);
		}

		// Token: 0x17001CAB RID: 7339
		// (get) Token: 0x0600795A RID: 31066 RVA: 0x00227063 File Offset: 0x00225263
		// (set) Token: 0x0600795B RID: 31067 RVA: 0x0022706B File Offset: 0x0022526B
		internal Guid PageFunctionId
		{
			get
			{
				return this._pageFunctionId;
			}
			set
			{
				this._pageFunctionId = value;
			}
		}

		// Token: 0x17001CAC RID: 7340
		// (get) Token: 0x0600795C RID: 31068 RVA: 0x00227074 File Offset: 0x00225274
		// (set) Token: 0x0600795D RID: 31069 RVA: 0x0022707C File Offset: 0x0022527C
		internal Guid ParentPageFunctionId
		{
			get
			{
				return this._parentPageFunctionId;
			}
			set
			{
				this._parentPageFunctionId = value;
			}
		}

		// Token: 0x0600795E RID: 31070 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool IsPageFunction()
		{
			return true;
		}

		// Token: 0x0600795F RID: 31071 RVA: 0x0000B02A File Offset: 0x0000922A
		internal override bool IsAlive()
		{
			return false;
		}

		// Token: 0x06007960 RID: 31072
		internal abstract PageFunctionBase ResumePageFunction();

		// Token: 0x06007961 RID: 31073 RVA: 0x00227088 File Offset: 0x00225288
		internal static int GetParentPageJournalIndex(NavigationService NavigationService, Journal journal, PageFunctionBase endingPF)
		{
			for (int i = journal.CurrentIndex - 1; i >= 0; i--)
			{
				JournalEntry journalEntry = journal[i];
				if (!(journalEntry.NavigationServiceId != NavigationService.GuidId))
				{
					JournalEntryPageFunction journalEntryPageFunction = journalEntry as JournalEntryPageFunction;
					if (endingPF.ParentPageFunctionId == Guid.Empty)
					{
						if (journalEntryPageFunction == null)
						{
							return i;
						}
					}
					else if (journalEntryPageFunction != null && journalEntryPageFunction.PageFunctionId == endingPF.ParentPageFunctionId)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x0400397E RID: 14718
		private Guid _pageFunctionId;

		// Token: 0x0400397F RID: 14719
		private Guid _parentPageFunctionId;

		// Token: 0x04003980 RID: 14720
		internal const int _NoParentPage = -1;
	}
}
