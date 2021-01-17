using System;
using System.Windows;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.Text
{
	// Token: 0x02000605 RID: 1541
	internal static class TextDpi
	{
		// Token: 0x1700189D RID: 6301
		// (get) Token: 0x06006671 RID: 26225 RVA: 0x001CC488 File Offset: 0x001CA688
		internal static double MinWidth
		{
			get
			{
				return 0.0033333333333333335;
			}
		}

		// Token: 0x1700189E RID: 6302
		// (get) Token: 0x06006672 RID: 26226 RVA: 0x001CC493 File Offset: 0x001CA693
		internal static double MaxWidth
		{
			get
			{
				return 3579139.4066666667;
			}
		}

		// Token: 0x06006673 RID: 26227 RVA: 0x001CC4A0 File Offset: 0x001CA6A0
		internal static int ToTextDpi(double d)
		{
			if (DoubleUtil.IsZero(d))
			{
				return 0;
			}
			if (d > 0.0)
			{
				if (d > 3579139.4066666667)
				{
					d = 3579139.4066666667;
				}
				else if (d < 0.0033333333333333335)
				{
					d = 0.0033333333333333335;
				}
			}
			else if (d < -3579139.4066666667)
			{
				d = -3579139.4066666667;
			}
			else if (d > -0.0033333333333333335)
			{
				d = -0.0033333333333333335;
			}
			return (int)Math.Round(d * 300.0);
		}

		// Token: 0x06006674 RID: 26228 RVA: 0x001CC536 File Offset: 0x001CA736
		internal static double FromTextDpi(int i)
		{
			return (double)i / 300.0;
		}

		// Token: 0x06006675 RID: 26229 RVA: 0x001CC544 File Offset: 0x001CA744
		internal static PTS.FSPOINT ToTextPoint(Point point)
		{
			return new PTS.FSPOINT
			{
				u = TextDpi.ToTextDpi(point.X),
				v = TextDpi.ToTextDpi(point.Y)
			};
		}

		// Token: 0x06006676 RID: 26230 RVA: 0x001CC580 File Offset: 0x001CA780
		internal static PTS.FSVECTOR ToTextSize(Size size)
		{
			return new PTS.FSVECTOR
			{
				du = TextDpi.ToTextDpi(size.Width),
				dv = TextDpi.ToTextDpi(size.Height)
			};
		}

		// Token: 0x06006677 RID: 26231 RVA: 0x001CC5BC File Offset: 0x001CA7BC
		internal static Rect FromTextRect(PTS.FSRECT fsrect)
		{
			return new Rect(TextDpi.FromTextDpi(fsrect.u), TextDpi.FromTextDpi(fsrect.v), TextDpi.FromTextDpi(fsrect.du), TextDpi.FromTextDpi(fsrect.dv));
		}

		// Token: 0x06006678 RID: 26232 RVA: 0x001CC5EF File Offset: 0x001CA7EF
		internal static void EnsureValidLineOffset(ref double offset)
		{
			if (offset > 3579139.4066666667)
			{
				offset = 3579139.4066666667;
				return;
			}
			if (offset < -3579139.4066666667)
			{
				offset = -3579139.4066666667;
			}
		}

		// Token: 0x06006679 RID: 26233 RVA: 0x001CC622 File Offset: 0x001CA822
		internal static void SnapToTextDpi(ref Size size)
		{
			size = new Size(TextDpi.FromTextDpi(TextDpi.ToTextDpi(size.Width)), TextDpi.FromTextDpi(TextDpi.ToTextDpi(size.Height)));
		}

		// Token: 0x0600667A RID: 26234 RVA: 0x001CC64F File Offset: 0x001CA84F
		internal static void EnsureValidLineWidth(ref double width)
		{
			if (width > 3579139.4066666667)
			{
				width = 3579139.4066666667;
				return;
			}
			if (width < 0.0033333333333333335)
			{
				width = 0.0033333333333333335;
			}
		}

		// Token: 0x0600667B RID: 26235 RVA: 0x001CC684 File Offset: 0x001CA884
		internal static void EnsureValidLineWidth(ref Size size)
		{
			if (size.Width > 3579139.4066666667)
			{
				size.Width = 3579139.4066666667;
				return;
			}
			if (size.Width < 0.0033333333333333335)
			{
				size.Width = 0.0033333333333333335;
			}
		}

		// Token: 0x0600667C RID: 26236 RVA: 0x001CC6D2 File Offset: 0x001CA8D2
		internal static void EnsureValidLineWidth(ref int width)
		{
			if (width > 1073741822)
			{
				width = 1073741822;
				return;
			}
			if (width < 1)
			{
				width = 1;
			}
		}

		// Token: 0x0600667D RID: 26237 RVA: 0x001CC6F0 File Offset: 0x001CA8F0
		internal static void EnsureValidPageSize(ref Size size)
		{
			if (size.Width > 3579139.4066666667)
			{
				size.Width = 3579139.4066666667;
			}
			else if (size.Width < 0.0033333333333333335)
			{
				size.Width = 0.0033333333333333335;
			}
			if (size.Height > 3579139.4066666667)
			{
				size.Height = 3579139.4066666667;
				return;
			}
			if (size.Height < 0.0033333333333333335)
			{
				size.Height = 0.0033333333333333335;
			}
		}

		// Token: 0x0600667E RID: 26238 RVA: 0x001CC64F File Offset: 0x001CA84F
		internal static void EnsureValidPageWidth(ref double width)
		{
			if (width > 3579139.4066666667)
			{
				width = 3579139.4066666667;
				return;
			}
			if (width < 0.0033333333333333335)
			{
				width = 0.0033333333333333335;
			}
		}

		// Token: 0x0600667F RID: 26239 RVA: 0x001CC780 File Offset: 0x001CA980
		internal static void EnsureValidPageMargin(ref Thickness pageMargin, Size pageSize)
		{
			if (pageMargin.Left >= pageSize.Width)
			{
				pageMargin.Right = 0.0;
			}
			if (pageMargin.Left + pageMargin.Right >= pageSize.Width)
			{
				pageMargin.Right = Math.Max(0.0, pageSize.Width - pageMargin.Left - 0.0033333333333333335);
				if (pageMargin.Left + pageMargin.Right >= pageSize.Width)
				{
					pageMargin.Left = pageSize.Width - 0.0033333333333333335;
				}
			}
			if (pageMargin.Top >= pageSize.Height)
			{
				pageMargin.Bottom = 0.0;
			}
			if (pageMargin.Top + pageMargin.Bottom >= pageSize.Height)
			{
				pageMargin.Bottom = Math.Max(0.0, pageSize.Height - pageMargin.Top - 0.0033333333333333335);
				if (pageMargin.Top + pageMargin.Bottom >= pageSize.Height)
				{
					pageMargin.Top = pageSize.Height - 0.0033333333333333335;
				}
			}
		}

		// Token: 0x06006680 RID: 26240 RVA: 0x001CC8A8 File Offset: 0x001CAAA8
		internal static void EnsureValidObjSize(ref Size size)
		{
			if (size.Width > 1193046.4688888888)
			{
				size.Width = 1193046.4688888888;
			}
			if (size.Height > 1193046.4688888888)
			{
				size.Height = 1193046.4688888888;
			}
		}

		// Token: 0x04003318 RID: 13080
		private const double _scale = 300.0;

		// Token: 0x04003319 RID: 13081
		private const int _maxSizeInt = 1073741822;

		// Token: 0x0400331A RID: 13082
		private const double _maxSize = 3579139.4066666667;

		// Token: 0x0400331B RID: 13083
		private const int _minSizeInt = 1;

		// Token: 0x0400331C RID: 13084
		private const double _minSize = 0.0033333333333333335;

		// Token: 0x0400331D RID: 13085
		private const double _maxObjSize = 1193046.4688888888;
	}
}
