using System;

namespace System.Windows.Media.Animation
{
	/// <summary>Animates from the <see cref="T:System.Windows.Thickness" /> value of the previous key frame to its own <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrame.Value" /> using discrete interpolation.  </summary>
	// Token: 0x0200018A RID: 394
	public class DiscreteThicknessKeyFrame : ThicknessKeyFrame
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.DiscreteThicknessKeyFrame" /> class.</summary>
		// Token: 0x06001712 RID: 5906 RVA: 0x00071BA9 File Offset: 0x0006FDA9
		public DiscreteThicknessKeyFrame()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.DiscreteThicknessKeyFrame" /> class with the specified ending value. </summary>
		/// <param name="value">The ending value (also known as "target value") for the key frame.</param>
		// Token: 0x06001713 RID: 5907 RVA: 0x00071BB1 File Offset: 0x0006FDB1
		public DiscreteThicknessKeyFrame(Thickness value) : base(value)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.DiscreteThicknessKeyFrame" /> class with the specified ending value and key time.</summary>
		/// <param name="value">The ending value (also known as "target value") for the key frame.</param>
		/// <param name="keyTime">The key time for the key frame. The key time determines when the target value is reached, which is also when the key frame ends.</param>
		// Token: 0x06001714 RID: 5908 RVA: 0x00071BBA File Offset: 0x0006FDBA
		public DiscreteThicknessKeyFrame(Thickness value, KeyTime keyTime) : base(value, keyTime)
		{
		}

		/// <summary>Creates a new instance of <see cref="T:System.Windows.Media.Animation.DiscreteThicknessKeyFrame" />.</summary>
		/// <returns>The new instance.</returns>
		// Token: 0x06001715 RID: 5909 RVA: 0x00071BC4 File Offset: 0x0006FDC4
		protected override Freezable CreateInstanceCore()
		{
			return new DiscreteThicknessKeyFrame();
		}

		/// <summary>Interpolates, between the previous key frame value and the value of the current key frame using discrete interpolation. </summary>
		/// <param name="baseValue">The value to animate from.</param>
		/// <param name="keyFrameProgress">A value from 0.0 through 1.0 that specifies the percentage of time that has elapsed for this key frame.</param>
		/// <returns>The output value of this key frame given the specified base value and progress.</returns>
		// Token: 0x06001716 RID: 5910 RVA: 0x00071BCB File Offset: 0x0006FDCB
		protected override Thickness InterpolateValueCore(Thickness baseValue, double keyFrameProgress)
		{
			if (keyFrameProgress < 1.0)
			{
				return baseValue;
			}
			return base.Value;
		}
	}
}
