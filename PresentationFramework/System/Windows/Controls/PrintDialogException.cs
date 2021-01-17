using System;
using System.Runtime.Serialization;

namespace System.Windows.Controls
{
	/// <summary>The exception that is thrown when an error condition occurs during the opening, accessing, or using of a PrintDialog.</summary>
	// Token: 0x0200051C RID: 1308
	[Serializable]
	public class PrintDialogException : Exception
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.PrintDialogException" /> class.</summary>
		// Token: 0x06005484 RID: 21636 RVA: 0x00127777 File Offset: 0x00125977
		public PrintDialogException()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.PrintDialogException" /> class that provides a specific error condition in a <see cref="T:System.String" /> .</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error condition.</param>
		// Token: 0x06005485 RID: 21637 RVA: 0x0012777F File Offset: 0x0012597F
		public PrintDialogException(string message) : base(message)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.PrintDialogException" /> class that provides a specific error condition, including its underlying cause.</summary>
		/// <param name="message">The <see cref="T:System.String" /> that describes the error condition.</param>
		/// <param name="innerException">The underlying error condition that caused the <see cref="T:System.Windows.Controls.PrintDialogException" />.</param>
		// Token: 0x06005486 RID: 21638 RVA: 0x00127788 File Offset: 0x00125988
		public PrintDialogException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.PrintDialogException" /> class that provides specific <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" />. This constructor is protected.</summary>
		/// <param name="info">The data that is required to serialize or deserialize an object.</param>
		/// <param name="context">The context, including source and destination, of the serialized stream.</param>
		// Token: 0x06005487 RID: 21639 RVA: 0x00176568 File Offset: 0x00174768
		protected PrintDialogException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
