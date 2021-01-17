﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.Primitives.TextBoxBase.TextChanged" /> event.</summary>
	// Token: 0x02000541 RID: 1345
	public class TextChangedEventArgs : RoutedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.TextChangedEventArgs" /> class, using the specified event ID, undo action, and text changes. </summary>
		/// <param name="id">The event identifier (ID).</param>
		/// <param name="action">The <see cref="P:System.Windows.Controls.TextChangedEventArgs.UndoAction" /> caused by the text change.</param>
		/// <param name="changes">The changes that occurred during this event. For more information, see <see cref="P:System.Windows.Controls.TextChangedEventArgs.Changes" />.</param>
		// Token: 0x060057E8 RID: 22504 RVA: 0x001859B8 File Offset: 0x00183BB8
		public TextChangedEventArgs(RoutedEvent id, UndoAction action, ICollection<TextChange> changes)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			if (action < UndoAction.None || action > UndoAction.Create)
			{
				throw new InvalidEnumArgumentException("action", (int)action, typeof(UndoAction));
			}
			base.RoutedEvent = id;
			this._undoAction = action;
			this._changes = changes;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.TextChangedEventArgs" /> class, using the specified event ID and undo action.     </summary>
		/// <param name="id">The event identifier (ID).</param>
		/// <param name="action">The <see cref="P:System.Windows.Controls.TextChangedEventArgs.UndoAction" /> caused by the text change.</param>
		// Token: 0x060057E9 RID: 22505 RVA: 0x00185A0C File Offset: 0x00183C0C
		public TextChangedEventArgs(RoutedEvent id, UndoAction action) : this(id, action, new ReadOnlyCollection<TextChange>(new List<TextChange>()))
		{
		}

		/// <summary>Gets how the undo stack is caused or affected by this text change </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.UndoAction" /> appropriate for this text change.</returns>
		// Token: 0x1700156B RID: 5483
		// (get) Token: 0x060057EA RID: 22506 RVA: 0x00185A20 File Offset: 0x00183C20
		public UndoAction UndoAction
		{
			get
			{
				return this._undoAction;
			}
		}

		/// <summary>Gets a collection of objects that contains information about the changes that occurred.</summary>
		/// <returns>A collection of objects that contains information about the changes that occurred.</returns>
		// Token: 0x1700156C RID: 5484
		// (get) Token: 0x060057EB RID: 22507 RVA: 0x00185A28 File Offset: 0x00183C28
		public ICollection<TextChange> Changes
		{
			get
			{
				return this._changes;
			}
		}

		/// <summary>Performs the proper type casting to call the type-safe <see cref="T:System.Windows.Controls.TextChangedEventHandler" />  delegate for the <see cref="E:System.Windows.Controls.Primitives.TextBoxBase.TextChanged" /> event.</summary>
		/// <param name="genericHandler">The handler to invoke.</param>
		/// <param name="genericTarget">The current object along the event's route.</param>
		// Token: 0x060057EC RID: 22508 RVA: 0x00185A30 File Offset: 0x00183C30
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			TextChangedEventHandler textChangedEventHandler = (TextChangedEventHandler)genericHandler;
			textChangedEventHandler(genericTarget, this);
		}

		// Token: 0x04002E98 RID: 11928
		private UndoAction _undoAction;

		// Token: 0x04002E99 RID: 11929
		private readonly ICollection<TextChange> _changes;
	}
}
