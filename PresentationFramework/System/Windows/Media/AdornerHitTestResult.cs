using System;
using System.Windows.Documents;

namespace System.Windows.Media
{
	/// <summary>Represents data returned from calling the <see cref="M:System.Windows.Documents.AdornerLayer.AdornerHitTest(System.Windows.Point)" /> method.</summary>
	// Token: 0x0200017E RID: 382
	public class AdornerHitTestResult : PointHitTestResult
	{
		// Token: 0x06001679 RID: 5753 RVA: 0x0007027B File Offset: 0x0006E47B
		internal AdornerHitTestResult(Visual visual, Point pt, Adorner adorner) : base(visual, pt)
		{
			this._adorner = adorner;
		}

		/// <summary> Gets the visual that was hit. </summary>
		/// <returns>The visual that was hit.</returns>
		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x0600167A RID: 5754 RVA: 0x0007028C File Offset: 0x0006E48C
		public Adorner Adorner
		{
			get
			{
				return this._adorner;
			}
		}

		// Token: 0x0400129F RID: 4767
		private readonly Adorner _adorner;
	}
}
