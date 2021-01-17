using System;
using System.Windows;

namespace MS.Internal.PresentationFramework
{
	// Token: 0x02000800 RID: 2048
	internal static class AnimatedTypeHelpers
	{
		// Token: 0x06007D9D RID: 32157 RVA: 0x0023459A File Offset: 0x0023279A
		private static double InterpolateDouble(double from, double to, double progress)
		{
			return from + (to - from) * progress;
		}

		// Token: 0x06007D9E RID: 32158 RVA: 0x002345A4 File Offset: 0x002327A4
		internal static Thickness InterpolateThickness(Thickness from, Thickness to, double progress)
		{
			return new Thickness(AnimatedTypeHelpers.InterpolateDouble(from.Left, to.Left, progress), AnimatedTypeHelpers.InterpolateDouble(from.Top, to.Top, progress), AnimatedTypeHelpers.InterpolateDouble(from.Right, to.Right, progress), AnimatedTypeHelpers.InterpolateDouble(from.Bottom, to.Bottom, progress));
		}

		// Token: 0x06007D9F RID: 32159 RVA: 0x00234606 File Offset: 0x00232806
		private static double AddDouble(double value1, double value2)
		{
			return value1 + value2;
		}

		// Token: 0x06007DA0 RID: 32160 RVA: 0x0023460C File Offset: 0x0023280C
		internal static Thickness AddThickness(Thickness value1, Thickness value2)
		{
			return new Thickness(AnimatedTypeHelpers.AddDouble(value1.Left, value2.Left), AnimatedTypeHelpers.AddDouble(value1.Top, value2.Top), AnimatedTypeHelpers.AddDouble(value1.Right, value2.Right), AnimatedTypeHelpers.AddDouble(value1.Bottom, value2.Bottom));
		}

		// Token: 0x06007DA1 RID: 32161 RVA: 0x0023466C File Offset: 0x0023286C
		internal static Thickness SubtractThickness(Thickness value1, Thickness value2)
		{
			return new Thickness(value1.Left - value2.Left, value1.Top - value2.Top, value1.Right - value2.Right, value1.Bottom - value2.Bottom);
		}

		// Token: 0x06007DA2 RID: 32162 RVA: 0x002346BA File Offset: 0x002328BA
		private static double GetSegmentLengthDouble(double from, double to)
		{
			return Math.Abs(to - from);
		}

		// Token: 0x06007DA3 RID: 32163 RVA: 0x002346C4 File Offset: 0x002328C4
		internal static double GetSegmentLengthThickness(Thickness from, Thickness to)
		{
			double d = Math.Pow(AnimatedTypeHelpers.GetSegmentLengthDouble(from.Left, to.Left), 2.0) + Math.Pow(AnimatedTypeHelpers.GetSegmentLengthDouble(from.Top, to.Top), 2.0) + Math.Pow(AnimatedTypeHelpers.GetSegmentLengthDouble(from.Right, to.Right), 2.0) + Math.Pow(AnimatedTypeHelpers.GetSegmentLengthDouble(from.Bottom, to.Bottom), 2.0);
			return Math.Sqrt(d);
		}

		// Token: 0x06007DA4 RID: 32164 RVA: 0x0023475F File Offset: 0x0023295F
		private static double ScaleDouble(double value, double factor)
		{
			return value * factor;
		}

		// Token: 0x06007DA5 RID: 32165 RVA: 0x00234764 File Offset: 0x00232964
		internal static Thickness ScaleThickness(Thickness value, double factor)
		{
			return new Thickness(AnimatedTypeHelpers.ScaleDouble(value.Left, factor), AnimatedTypeHelpers.ScaleDouble(value.Top, factor), AnimatedTypeHelpers.ScaleDouble(value.Right, factor), AnimatedTypeHelpers.ScaleDouble(value.Bottom, factor));
		}

		// Token: 0x06007DA6 RID: 32166 RVA: 0x0023479F File Offset: 0x0023299F
		private static bool IsValidAnimationValueDouble(double value)
		{
			return !AnimatedTypeHelpers.IsInvalidDouble(value);
		}

		// Token: 0x06007DA7 RID: 32167 RVA: 0x002347AC File Offset: 0x002329AC
		internal static bool IsValidAnimationValueThickness(Thickness value)
		{
			return AnimatedTypeHelpers.IsValidAnimationValueDouble(value.Left) || AnimatedTypeHelpers.IsValidAnimationValueDouble(value.Top) || AnimatedTypeHelpers.IsValidAnimationValueDouble(value.Right) || AnimatedTypeHelpers.IsValidAnimationValueDouble(value.Bottom);
		}

		// Token: 0x06007DA8 RID: 32168 RVA: 0x0018D432 File Offset: 0x0018B632
		private static double GetZeroValueDouble(double baseValue)
		{
			return 0.0;
		}

		// Token: 0x06007DA9 RID: 32169 RVA: 0x002347E9 File Offset: 0x002329E9
		internal static Thickness GetZeroValueThickness(Thickness baseValue)
		{
			return new Thickness(AnimatedTypeHelpers.GetZeroValueDouble(baseValue.Left), AnimatedTypeHelpers.GetZeroValueDouble(baseValue.Top), AnimatedTypeHelpers.GetZeroValueDouble(baseValue.Right), AnimatedTypeHelpers.GetZeroValueDouble(baseValue.Bottom));
		}

		// Token: 0x06007DAA RID: 32170 RVA: 0x00234820 File Offset: 0x00232A20
		private static bool IsInvalidDouble(double value)
		{
			return double.IsInfinity(value) || DoubleUtil.IsNaN(value);
		}
	}
}
