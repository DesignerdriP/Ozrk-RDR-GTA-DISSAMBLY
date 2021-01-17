using System;
using System.ComponentModel;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.AutoGeneratingColumn" /> event.</summary>
	// Token: 0x02000493 RID: 1171
	public class DataGridAutoGeneratingColumnEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs" /> class. </summary>
		/// <param name="propertyName">The name of the property bound to the generated column.</param>
		/// <param name="propertyType">The type of the property bound to the generated column.</param>
		/// <param name="column">The generated column.</param>
		// Token: 0x060046BB RID: 18107 RVA: 0x0014124F File Offset: 0x0013F44F
		public DataGridAutoGeneratingColumnEventArgs(string propertyName, Type propertyType, DataGridColumn column) : this(column, propertyName, propertyType, null)
		{
		}

		// Token: 0x060046BC RID: 18108 RVA: 0x0014125B File Offset: 0x0013F45B
		internal DataGridAutoGeneratingColumnEventArgs(DataGridColumn column, ItemPropertyInfo itemPropertyInfo) : this(column, itemPropertyInfo.Name, itemPropertyInfo.PropertyType, itemPropertyInfo.Descriptor)
		{
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x00141276 File Offset: 0x0013F476
		internal DataGridAutoGeneratingColumnEventArgs(DataGridColumn column, string propertyName, Type propertyType, object propertyDescriptor)
		{
			this._column = column;
			this._propertyName = propertyName;
			this._propertyType = propertyType;
			this.PropertyDescriptor = propertyDescriptor;
		}

		/// <summary>Gets or sets the generated column.</summary>
		/// <returns>The generated column.</returns>
		// Token: 0x1700114E RID: 4430
		// (get) Token: 0x060046BE RID: 18110 RVA: 0x0014129B File Offset: 0x0013F49B
		// (set) Token: 0x060046BF RID: 18111 RVA: 0x001412A3 File Offset: 0x0013F4A3
		public DataGridColumn Column
		{
			get
			{
				return this._column;
			}
			set
			{
				this._column = value;
			}
		}

		/// <summary>Gets the name of the property bound to the generated column.</summary>
		/// <returns>The name of the property bound to the generated column.</returns>
		// Token: 0x1700114F RID: 4431
		// (get) Token: 0x060046C0 RID: 18112 RVA: 0x001412AC File Offset: 0x0013F4AC
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
		}

		/// <summary>Gets the type of the property bound to the generated column.</summary>
		/// <returns>The type of the property bound to the generated column.</returns>
		// Token: 0x17001150 RID: 4432
		// (get) Token: 0x060046C1 RID: 18113 RVA: 0x001412B4 File Offset: 0x0013F4B4
		public Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
		}

		/// <summary>Gets the descriptor of the property bound to the generated column.</summary>
		/// <returns>An object that contains metadata for the property.</returns>
		// Token: 0x17001151 RID: 4433
		// (get) Token: 0x060046C2 RID: 18114 RVA: 0x001412BC File Offset: 0x0013F4BC
		// (set) Token: 0x060046C3 RID: 18115 RVA: 0x001412C4 File Offset: 0x0013F4C4
		public object PropertyDescriptor
		{
			get
			{
				return this._propertyDescriptor;
			}
			private set
			{
				if (value == null)
				{
					this._propertyDescriptor = null;
					return;
				}
				this._propertyDescriptor = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the event should be canceled.</summary>
		/// <returns>
		///     <see langword="true" /> if the event should be canceled; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001152 RID: 4434
		// (get) Token: 0x060046C4 RID: 18116 RVA: 0x001412D8 File Offset: 0x0013F4D8
		// (set) Token: 0x060046C5 RID: 18117 RVA: 0x001412E0 File Offset: 0x0013F4E0
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = value;
			}
		}

		// Token: 0x04002932 RID: 10546
		private DataGridColumn _column;

		// Token: 0x04002933 RID: 10547
		private string _propertyName;

		// Token: 0x04002934 RID: 10548
		private Type _propertyType;

		// Token: 0x04002935 RID: 10549
		private object _propertyDescriptor;

		// Token: 0x04002936 RID: 10550
		private bool _cancel;
	}
}
