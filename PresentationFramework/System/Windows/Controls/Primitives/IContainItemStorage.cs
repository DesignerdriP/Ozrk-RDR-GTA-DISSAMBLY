using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Provides a property bag implementation for item panels.</summary>
	// Token: 0x0200058D RID: 1421
	public interface IContainItemStorage
	{
		/// <summary>Stores the specified property and value and associates them with the specified item.</summary>
		/// <param name="item">The item to associate the value and property with.</param>
		/// <param name="dp">The property that is associated with the specified item.</param>
		/// <param name="value">The value of the associated property.</param>
		// Token: 0x06005E1A RID: 24090
		void StoreItemValue(object item, DependencyProperty dp, object value);

		/// <summary>Returns the value of the specified property that is associated with the specified item.</summary>
		/// <param name="item">The item that has the specified property associated with it.</param>
		/// <param name="dp">The property whose value to return.</param>
		/// <returns>The value of the specified property that is associated with the specified item.</returns>
		// Token: 0x06005E1B RID: 24091
		object ReadItemValue(object item, DependencyProperty dp);

		/// <summary>Removes the association between the specified item and property. </summary>
		/// <param name="item">The associated item.</param>
		/// <param name="dp">The associated property.</param>
		// Token: 0x06005E1C RID: 24092
		void ClearItemValue(object item, DependencyProperty dp);

		/// <summary>Removes the specified property from all property bags.</summary>
		/// <param name="dp">The property to remove.</param>
		// Token: 0x06005E1D RID: 24093
		void ClearValue(DependencyProperty dp);

		/// <summary>Clears all property associations.</summary>
		// Token: 0x06005E1E RID: 24094
		void Clear();
	}
}
