using System;

namespace System.Windows.Controls
{
	/// <summary>
	///
	///     How the undo stack caused or is affected by a text change. </summary>
	// Token: 0x0200053F RID: 1343
	public enum UndoAction
	{
		/// <summary>
		///
		///     This change will not affect the undo stack at all </summary>
		// Token: 0x04002E92 RID: 11922
		None,
		/// <summary>
		///
		///     This change will merge into the previous undo unit </summary>
		// Token: 0x04002E93 RID: 11923
		Merge,
		/// <summary>
		///
		///     This change is the result of a call to Undo() </summary>
		// Token: 0x04002E94 RID: 11924
		Undo,
		/// <summary>
		///
		///     This change is the result of a call to Redo() </summary>
		// Token: 0x04002E95 RID: 11925
		Redo,
		/// <summary>
		///
		///     This change will clear the undo stack </summary>
		// Token: 0x04002E96 RID: 11926
		Clear,
		/// <summary>
		///
		///     This change will create a new undo unit </summary>
		// Token: 0x04002E97 RID: 11927
		Create
	}
}
