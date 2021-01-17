﻿using System;

namespace System.Windows.Documents.Serialization
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Xps.XpsDocumentWriter.WritingCancelled" /> event.</summary>
	// Token: 0x02000443 RID: 1091
	public class WritingCancelledEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Serialization.WritingCancelledEventArgs" /> class.</summary>
		/// <param name="exception">The exception that canceled the write operation.</param>
		// Token: 0x06003FD0 RID: 16336 RVA: 0x00125A0F File Offset: 0x00123C0F
		public WritingCancelledEventArgs(Exception exception)
		{
			this._exception = exception;
		}

		/// <summary>Gets the exception that canceled the write operation.</summary>
		/// <returns>The exception that canceled the write operation.</returns>
		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x06003FD1 RID: 16337 RVA: 0x00125A1E File Offset: 0x00123C1E
		public Exception Error
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x04002757 RID: 10071
		private Exception _exception;
	}
}
