using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.Calendar.DisplayModeChanged" /> event.</summary>
	// Token: 0x0200047C RID: 1148
	public class CalendarModeChangedEventArgs : RoutedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.CalendarModeChangedEventArgs" /> class. </summary>
		/// <param name="oldMode">The previous mode.</param>
		/// <param name="newMode">The new mode.</param>
		// Token: 0x06004322 RID: 17186 RVA: 0x00133302 File Offset: 0x00131502
		public CalendarModeChangedEventArgs(CalendarMode oldMode, CalendarMode newMode)
		{
			this.OldMode = oldMode;
			this.NewMode = newMode;
		}

		/// <summary>Gets the new mode of the <see cref="T:System.Windows.Controls.Calendar" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Controls.CalendarMode" /> that represents the new mode.</returns>
		// Token: 0x1700107C RID: 4220
		// (get) Token: 0x06004323 RID: 17187 RVA: 0x00133318 File Offset: 0x00131518
		// (set) Token: 0x06004324 RID: 17188 RVA: 0x00133320 File Offset: 0x00131520
		public CalendarMode NewMode { get; private set; }

		/// <summary>Gets the previous mode of the <see cref="T:System.Windows.Controls.Calendar" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Controls.CalendarMode" /> that represents the old mode.</returns>
		// Token: 0x1700107D RID: 4221
		// (get) Token: 0x06004325 RID: 17189 RVA: 0x00133329 File Offset: 0x00131529
		// (set) Token: 0x06004326 RID: 17190 RVA: 0x00133331 File Offset: 0x00131531
		public CalendarMode OldMode { get; private set; }
	}
}
