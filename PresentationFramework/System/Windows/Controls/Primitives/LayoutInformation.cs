using System;
using System.Windows.Media;
using System.Windows.Threading;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Defines methods that provide additional information about the layout state of an element.</summary>
	// Token: 0x02000597 RID: 1431
	public static class LayoutInformation
	{
		// Token: 0x06005E64 RID: 24164 RVA: 0x001A746D File Offset: 0x001A566D
		private static void CheckArgument(FrameworkElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
		}

		/// <summary>Returns a <see cref="T:System.Windows.Rect" /> that represents the layout partition that is reserved for a child element.</summary>
		/// <param name="element">The <see cref="T:System.Windows.FrameworkElement" /> whose layout slot is desired.</param>
		/// <returns>A <see cref="T:System.Windows.Rect" /> that represents the layout slot of the element.</returns>
		// Token: 0x06005E65 RID: 24165 RVA: 0x001A747D File Offset: 0x001A567D
		public static Rect GetLayoutSlot(FrameworkElement element)
		{
			LayoutInformation.CheckArgument(element);
			return element.PreviousArrangeRect;
		}

		/// <summary>Returns a <see cref="T:System.Windows.Media.Geometry" /> that represents the visible region of an element.</summary>
		/// <param name="element">The <see cref="T:System.Windows.FrameworkElement" /> whose layout clip is desired.</param>
		/// <returns>A <see cref="T:System.Windows.Media.Geometry" /> that represents the visible region of an <paramref name="element" />.</returns>
		// Token: 0x06005E66 RID: 24166 RVA: 0x001A748B File Offset: 0x001A568B
		public static Geometry GetLayoutClip(FrameworkElement element)
		{
			LayoutInformation.CheckArgument(element);
			return element.GetLayoutClipInternal();
		}

		/// <summary>Returns a <see cref="T:System.Windows.UIElement" /> that was being processed by the layout engine at the moment of an unhandled exception.</summary>
		/// <param name="dispatcher">The <see cref="T:System.Windows.Threading.Dispatcher" /> object that defines the scope of the operation. There is one dispatcher per layout engine instance.</param>
		/// <returns>A <see cref="T:System.Windows.UIElement" /> that was being processed by the layout engine at the moment of an unhandled exception.</returns>
		/// <exception cref="T:System.ArgumentNullException">Occurs when <paramref name="dispatcher" /> is <see langword="null" />.</exception>
		// Token: 0x06005E67 RID: 24167 RVA: 0x001A749C File Offset: 0x001A569C
		public static UIElement GetLayoutExceptionElement(Dispatcher dispatcher)
		{
			if (dispatcher == null)
			{
				throw new ArgumentNullException("dispatcher");
			}
			UIElement result = null;
			ContextLayoutManager contextLayoutManager = ContextLayoutManager.From(dispatcher);
			if (contextLayoutManager != null)
			{
				result = contextLayoutManager.GetLastExceptionElement();
			}
			return result;
		}
	}
}
