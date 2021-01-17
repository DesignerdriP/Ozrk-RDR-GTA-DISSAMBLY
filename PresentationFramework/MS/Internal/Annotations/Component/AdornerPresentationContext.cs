using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020007DD RID: 2013
	internal class AdornerPresentationContext : PresentationContext
	{
		// Token: 0x06007C64 RID: 31844 RVA: 0x0022FBD8 File Offset: 0x0022DDD8
		private AdornerPresentationContext(AdornerLayer adornerLayer, AnnotationAdorner adorner)
		{
			if (adornerLayer == null)
			{
				throw new ArgumentNullException("adornerLayer");
			}
			this._adornerLayer = adornerLayer;
			if (adorner != null)
			{
				if (adorner.AnnotationComponent == null)
				{
					throw new ArgumentNullException("annotation component");
				}
				if (adorner.AnnotationComponent.PresentationContext != null)
				{
					throw new InvalidOperationException(SR.Get("ComponentAlreadyInPresentationContext", new object[]
					{
						adorner.AnnotationComponent
					}));
				}
				this._annotationAdorner = adorner;
			}
		}

		// Token: 0x06007C65 RID: 31845 RVA: 0x0022FC4C File Offset: 0x0022DE4C
		internal static void HostComponent(AdornerLayer adornerLayer, IAnnotationComponent component, UIElement annotatedElement, bool reorder)
		{
			AnnotationAdorner annotationAdorner = new AnnotationAdorner(component, annotatedElement);
			annotationAdorner.AnnotationComponent.PresentationContext = new AdornerPresentationContext(adornerLayer, annotationAdorner);
			int componentLevel = AdornerPresentationContext.GetComponentLevel(component);
			if (reorder)
			{
				component.ZOrder = AdornerPresentationContext.GetNextZOrder(adornerLayer, componentLevel);
			}
			adornerLayer.Add(annotationAdorner, AdornerPresentationContext.ComponentToAdorner(component.ZOrder, componentLevel));
		}

		// Token: 0x06007C66 RID: 31846 RVA: 0x0022FCA0 File Offset: 0x0022DEA0
		internal static void SetTypeZLevel(Type type, int level)
		{
			Invariant.Assert(level >= 0, "level is < 0");
			Invariant.Assert(type != null, "type is null");
			if (AdornerPresentationContext._ZLevel.ContainsKey(type))
			{
				AdornerPresentationContext._ZLevel[type] = level;
				return;
			}
			AdornerPresentationContext._ZLevel.Add(type, level);
		}

		// Token: 0x06007C67 RID: 31847 RVA: 0x0022FCFF File Offset: 0x0022DEFF
		internal static void SetZLevelRange(int level, int min, int max)
		{
			if (AdornerPresentationContext._ZRanges[level] == null)
			{
				AdornerPresentationContext._ZRanges.Add(level, new AdornerPresentationContext.ZRange(min, max));
			}
		}

		// Token: 0x17001CEC RID: 7404
		// (get) Token: 0x06007C68 RID: 31848 RVA: 0x0022FD2A File Offset: 0x0022DF2A
		public override UIElement Host
		{
			get
			{
				return this._adornerLayer;
			}
		}

		// Token: 0x17001CED RID: 7405
		// (get) Token: 0x06007C69 RID: 31849 RVA: 0x0022FD34 File Offset: 0x0022DF34
		public override PresentationContext EnclosingContext
		{
			get
			{
				Visual visual = VisualTreeHelper.GetParent(this._adornerLayer) as Visual;
				if (visual == null)
				{
					return null;
				}
				AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer((UIElement)visual);
				if (adornerLayer == null)
				{
					return null;
				}
				return new AdornerPresentationContext(adornerLayer, null);
			}
		}

		// Token: 0x06007C6A RID: 31850 RVA: 0x0022FD71 File Offset: 0x0022DF71
		public override void AddToHost(IAnnotationComponent component)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			AdornerPresentationContext.HostComponent(this._adornerLayer, component, component.AnnotatedElement, false);
		}

		// Token: 0x06007C6B RID: 31851 RVA: 0x0022FD94 File Offset: 0x0022DF94
		public override void RemoveFromHost(IAnnotationComponent component, bool reorder)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (this.IsInternalComponent(component))
			{
				this._annotationAdorner.AnnotationComponent.PresentationContext = null;
				this._adornerLayer.Remove(this._annotationAdorner);
				this._annotationAdorner.RemoveChildren();
				this._annotationAdorner = null;
				return;
			}
			AnnotationAdorner annotationAdorner = this.FindAnnotationAdorner(component);
			if (annotationAdorner == null)
			{
				throw new InvalidOperationException(SR.Get("ComponentNotInPresentationContext", new object[]
				{
					component
				}));
			}
			this._adornerLayer.Remove(annotationAdorner);
			annotationAdorner.RemoveChildren();
			AdornerPresentationContext adornerPresentationContext = component.PresentationContext as AdornerPresentationContext;
			if (adornerPresentationContext != null)
			{
				adornerPresentationContext.ResetInternalAnnotationAdorner();
			}
			component.PresentationContext = null;
		}

		// Token: 0x06007C6C RID: 31852 RVA: 0x0022FE48 File Offset: 0x0022E048
		public override void InvalidateTransform(IAnnotationComponent component)
		{
			AnnotationAdorner annotationAdorner = this.GetAnnotationAdorner(component);
			annotationAdorner.InvalidateTransform();
		}

		// Token: 0x06007C6D RID: 31853 RVA: 0x0022FE64 File Offset: 0x0022E064
		public override void BringToFront(IAnnotationComponent component)
		{
			AnnotationAdorner annotationAdorner = this.GetAnnotationAdorner(component);
			int componentLevel = AdornerPresentationContext.GetComponentLevel(component);
			int nextZOrder = AdornerPresentationContext.GetNextZOrder(this._adornerLayer, componentLevel);
			if (nextZOrder != component.ZOrder + 1)
			{
				component.ZOrder = nextZOrder;
				this._adornerLayer.SetAdornerZOrder(annotationAdorner, AdornerPresentationContext.ComponentToAdorner(component.ZOrder, componentLevel));
			}
		}

		// Token: 0x06007C6E RID: 31854 RVA: 0x0022FEB8 File Offset: 0x0022E0B8
		public override void SendToBack(IAnnotationComponent component)
		{
			AnnotationAdorner annotationAdorner = this.GetAnnotationAdorner(component);
			int componentLevel = AdornerPresentationContext.GetComponentLevel(component);
			if (component.ZOrder != 0)
			{
				component.ZOrder = 0;
				this.UpdateComponentZOrder(component);
			}
		}

		// Token: 0x06007C6F RID: 31855 RVA: 0x0022FEEC File Offset: 0x0022E0EC
		public override bool Equals(object o)
		{
			AdornerPresentationContext adornerPresentationContext = o as AdornerPresentationContext;
			return adornerPresentationContext != null && adornerPresentationContext._adornerLayer == this._adornerLayer;
		}

		// Token: 0x06007C70 RID: 31856 RVA: 0x0001D0BF File Offset: 0x0001B2BF
		public static bool operator ==(AdornerPresentationContext left, AdornerPresentationContext right)
		{
			if (left == null)
			{
				return right == null;
			}
			return left.Equals(right);
		}

		// Token: 0x06007C71 RID: 31857 RVA: 0x0022FF19 File Offset: 0x0022E119
		public static bool operator !=(AdornerPresentationContext c1, AdornerPresentationContext c2)
		{
			return !(c1 == c2);
		}

		// Token: 0x06007C72 RID: 31858 RVA: 0x0022FF25 File Offset: 0x0022E125
		public override int GetHashCode()
		{
			return this._adornerLayer.GetHashCode();
		}

		// Token: 0x06007C73 RID: 31859 RVA: 0x0022FF34 File Offset: 0x0022E134
		public void UpdateComponentZOrder(IAnnotationComponent component)
		{
			Invariant.Assert(component != null, "null component");
			int componentLevel = AdornerPresentationContext.GetComponentLevel(component);
			AnnotationAdorner annotationAdorner = this.FindAnnotationAdorner(component);
			if (annotationAdorner == null)
			{
				return;
			}
			this._adornerLayer.SetAdornerZOrder(annotationAdorner, AdornerPresentationContext.ComponentToAdorner(component.ZOrder, componentLevel));
			List<AnnotationAdorner> topAnnotationAdorners = this.GetTopAnnotationAdorners(componentLevel, component);
			if (topAnnotationAdorners == null)
			{
				return;
			}
			int num = component.ZOrder + 1;
			foreach (AnnotationAdorner annotationAdorner2 in topAnnotationAdorners)
			{
				annotationAdorner2.AnnotationComponent.ZOrder = num;
				this._adornerLayer.SetAdornerZOrder(annotationAdorner2, AdornerPresentationContext.ComponentToAdorner(num, componentLevel));
				num++;
			}
		}

		// Token: 0x06007C74 RID: 31860 RVA: 0x0022FFF4 File Offset: 0x0022E1F4
		private void ResetInternalAnnotationAdorner()
		{
			this._annotationAdorner = null;
		}

		// Token: 0x06007C75 RID: 31861 RVA: 0x0022FFFD File Offset: 0x0022E1FD
		private bool IsInternalComponent(IAnnotationComponent component)
		{
			return this._annotationAdorner != null && component == this._annotationAdorner.AnnotationComponent;
		}

		// Token: 0x06007C76 RID: 31862 RVA: 0x00230018 File Offset: 0x0022E218
		private AnnotationAdorner FindAnnotationAdorner(IAnnotationComponent component)
		{
			if (this._adornerLayer == null)
			{
				return null;
			}
			foreach (Adorner adorner in this._adornerLayer.GetAdorners(component.AnnotatedElement))
			{
				AnnotationAdorner annotationAdorner = adorner as AnnotationAdorner;
				if (annotationAdorner != null && annotationAdorner.AnnotationComponent == component)
				{
					return annotationAdorner;
				}
			}
			return null;
		}

		// Token: 0x06007C77 RID: 31863 RVA: 0x0023006C File Offset: 0x0022E26C
		private List<AnnotationAdorner> GetTopAnnotationAdorners(int level, IAnnotationComponent component)
		{
			List<AnnotationAdorner> list = new List<AnnotationAdorner>();
			int childrenCount = VisualTreeHelper.GetChildrenCount(this._adornerLayer);
			if (childrenCount == 0)
			{
				return list;
			}
			for (int i = 0; i < childrenCount; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(this._adornerLayer, i);
				AnnotationAdorner annotationAdorner = child as AnnotationAdorner;
				if (annotationAdorner != null)
				{
					IAnnotationComponent annotationComponent = annotationAdorner.AnnotationComponent;
					if (annotationComponent != component && AdornerPresentationContext.GetComponentLevel(annotationComponent) == level && annotationComponent.ZOrder >= component.ZOrder)
					{
						this.AddAdorner(list, annotationAdorner);
					}
				}
			}
			return list;
		}

		// Token: 0x06007C78 RID: 31864 RVA: 0x002300E8 File Offset: 0x0022E2E8
		private void AddAdorner(List<AnnotationAdorner> adorners, AnnotationAdorner adorner)
		{
			int num = 0;
			if (adorners.Count > 0)
			{
				num = adorners.Count;
				while (num > 0 && adorners[num - 1].AnnotationComponent.ZOrder > adorner.AnnotationComponent.ZOrder)
				{
					num--;
				}
			}
			adorners.Insert(num, adorner);
		}

		// Token: 0x06007C79 RID: 31865 RVA: 0x0023013C File Offset: 0x0022E33C
		private static int GetNextZOrder(AdornerLayer adornerLayer, int level)
		{
			Invariant.Assert(adornerLayer != null, "null adornerLayer");
			int num = 0;
			int childrenCount = VisualTreeHelper.GetChildrenCount(adornerLayer);
			if (childrenCount == 0)
			{
				return num;
			}
			for (int i = 0; i < childrenCount; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(adornerLayer, i);
				AnnotationAdorner annotationAdorner = child as AnnotationAdorner;
				if (annotationAdorner != null && AdornerPresentationContext.GetComponentLevel(annotationAdorner.AnnotationComponent) == level && annotationAdorner.AnnotationComponent.ZOrder >= num)
				{
					num = annotationAdorner.AnnotationComponent.ZOrder + 1;
				}
			}
			return num;
		}

		// Token: 0x06007C7A RID: 31866 RVA: 0x002301B4 File Offset: 0x0022E3B4
		private AnnotationAdorner GetAnnotationAdorner(IAnnotationComponent component)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			AnnotationAdorner annotationAdorner = this._annotationAdorner;
			if (!this.IsInternalComponent(component))
			{
				annotationAdorner = this.FindAnnotationAdorner(component);
				if (annotationAdorner == null)
				{
					throw new InvalidOperationException(SR.Get("ComponentNotInPresentationContext", new object[]
					{
						component
					}));
				}
			}
			return annotationAdorner;
		}

		// Token: 0x06007C7B RID: 31867 RVA: 0x00230208 File Offset: 0x0022E408
		private static int GetComponentLevel(IAnnotationComponent component)
		{
			int result = 0;
			Type type = component.GetType();
			if (AdornerPresentationContext._ZLevel.ContainsKey(type))
			{
				result = (int)AdornerPresentationContext._ZLevel[type];
			}
			return result;
		}

		// Token: 0x06007C7C RID: 31868 RVA: 0x00230240 File Offset: 0x0022E440
		private static int ComponentToAdorner(int zOrder, int level)
		{
			int num = zOrder;
			AdornerPresentationContext.ZRange zrange = (AdornerPresentationContext.ZRange)AdornerPresentationContext._ZRanges[level];
			if (zrange != null)
			{
				num += zrange.Min;
				if (num < zrange.Min)
				{
					num = zrange.Min;
				}
				if (num > zrange.Max)
				{
					num = zrange.Max;
				}
			}
			return num;
		}

		// Token: 0x04003A62 RID: 14946
		private AnnotationAdorner _annotationAdorner;

		// Token: 0x04003A63 RID: 14947
		private AdornerLayer _adornerLayer;

		// Token: 0x04003A64 RID: 14948
		private static Hashtable _ZLevel = new Hashtable();

		// Token: 0x04003A65 RID: 14949
		private static Hashtable _ZRanges = new Hashtable();

		// Token: 0x02000B81 RID: 2945
		private class ZRange
		{
			// Token: 0x06008E46 RID: 36422 RVA: 0x0025B730 File Offset: 0x00259930
			public ZRange(int min, int max)
			{
				if (min > max)
				{
					int num = min;
					min = max;
					max = num;
				}
				this._min = min;
				this._max = max;
			}

			// Token: 0x17001FA7 RID: 8103
			// (get) Token: 0x06008E47 RID: 36423 RVA: 0x0025B75D File Offset: 0x0025995D
			public int Min
			{
				get
				{
					return this._min;
				}
			}

			// Token: 0x17001FA8 RID: 8104
			// (get) Token: 0x06008E48 RID: 36424 RVA: 0x0025B765 File Offset: 0x00259965
			public int Max
			{
				get
				{
					return this._max;
				}
			}

			// Token: 0x04004B83 RID: 19331
			private int _min;

			// Token: 0x04004B84 RID: 19332
			private int _max;
		}
	}
}
