using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Markup;
using MS.Internal;
using MS.Utility;

namespace System.Windows.Media.Animation
{
	/// <summary>A container timeline that provides object and property targeting information for its child animations. </summary>
	// Token: 0x02000189 RID: 393
	public class Storyboard : ParallelTimeline
	{
		// Token: 0x060016AC RID: 5804 RVA: 0x000708AC File Offset: 0x0006EAAC
		static Storyboard()
		{
			PropertyMetadata propertyMetadata = new PropertyMetadata();
			propertyMetadata.FreezeValueCallback = new FreezeValueCallback(Storyboard.TargetFreezeValueCallback);
			Storyboard.TargetProperty = DependencyProperty.RegisterAttached("Target", typeof(DependencyObject), typeof(Storyboard), propertyMetadata);
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Media.Animation.Storyboard" /> class.  </summary>
		/// <returns>A new <see cref="T:System.Windows.Media.Animation.Storyboard" /> instance.</returns>
		// Token: 0x060016AE RID: 5806 RVA: 0x00070957 File Offset: 0x0006EB57
		protected override Freezable CreateInstanceCore()
		{
			return new Storyboard();
		}

		/// <summary>Creates a modifiable clone of this <see cref="T:System.Windows.Media.Animation.Storyboard" />, making deep copies of this object's values. When copying dependency properties, this method copies resource references and data bindings (but they might no longer resolve) but not animations or their current values.</summary>
		/// <returns>A modifiable clone of the current object. The cloned object's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="false" /> even if the source's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true." /></returns>
		// Token: 0x060016AF RID: 5807 RVA: 0x0007095E File Offset: 0x0006EB5E
		public new Storyboard Clone()
		{
			return (Storyboard)base.Clone();
		}

		/// <summary>Makes the specified <see cref="T:System.Windows.Media.Animation.Timeline" /> target the dependency object. </summary>
		/// <param name="element">The <see cref="T:System.Windows.Media.Animation.Timeline" /> that should target the specified dependency object.</param>
		/// <param name="value">The dependency object to target.</param>
		// Token: 0x060016B0 RID: 5808 RVA: 0x0007096B File Offset: 0x0006EB6B
		public static void SetTarget(DependencyObject element, DependencyObject value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Storyboard.TargetProperty, value);
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Storyboard.Target" /> value of the specified <see cref="T:System.Windows.Media.Animation.Timeline" />.</summary>
		/// <param name="element">The timeline from which to retrieve the <see cref="P:System.Windows.Media.Animation.Storyboard.TargetName" />.</param>
		/// <returns>The dependency object targeted by <paramref name="element" />.</returns>
		// Token: 0x060016B1 RID: 5809 RVA: 0x00070987 File Offset: 0x0006EB87
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public static DependencyObject GetTarget(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (DependencyObject)element.GetValue(Storyboard.TargetProperty);
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x00016748 File Offset: 0x00014948
		private static bool TargetFreezeValueCallback(DependencyObject d, DependencyProperty dp, EntryIndex entryIndex, PropertyMetadata metadata, bool isChecking)
		{
			return true;
		}

		/// <summary>Makes the specified <see cref="T:System.Windows.Media.Animation.Timeline" /> target the dependency object with the specified name. </summary>
		/// <param name="element">The <see cref="T:System.Windows.Media.Animation.Timeline" /> that should target the specified dependency object.</param>
		/// <param name="name">The name of the dependency object to target.</param>
		// Token: 0x060016B3 RID: 5811 RVA: 0x000709A7 File Offset: 0x0006EBA7
		public static void SetTargetName(DependencyObject element, string name)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			element.SetValue(Storyboard.TargetNameProperty, name);
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Storyboard.TargetName" /> value of the specified <see cref="T:System.Windows.Media.Animation.Timeline" />. </summary>
		/// <param name="element">The timeline from which to retrieve the <see cref="P:System.Windows.Media.Animation.Storyboard.TargetName" />. </param>
		/// <returns>The name of the dependency object targeted by <paramref name="element" />.</returns>
		// Token: 0x060016B4 RID: 5812 RVA: 0x000709D1 File Offset: 0x0006EBD1
		public static string GetTargetName(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (string)element.GetValue(Storyboard.TargetNameProperty);
		}

		/// <summary>Makes the specified <see cref="T:System.Windows.Media.Animation.Timeline" /> target the specified dependency property.</summary>
		/// <param name="element">The <see cref="T:System.Windows.Media.Animation.Timeline" /> with which to associate the specified dependency property. </param>
		/// <param name="path">A path that describe the dependency property to be animated.</param>
		// Token: 0x060016B5 RID: 5813 RVA: 0x000709F1 File Offset: 0x0006EBF1
		public static void SetTargetProperty(DependencyObject element, PropertyPath path)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			element.SetValue(Storyboard.TargetPropertyProperty, path);
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Storyboard.TargetProperty" /> value of the specified <see cref="T:System.Windows.Media.Animation.Timeline" />. </summary>
		/// <param name="element">The dependency object from which to get the <see cref="P:System.Windows.Media.Animation.Storyboard.TargetProperty" />.</param>
		/// <returns>The property targeted by <paramref name="element" />.</returns>
		// Token: 0x060016B6 RID: 5814 RVA: 0x00070A1B File Offset: 0x0006EC1B
		public static PropertyPath GetTargetProperty(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (PropertyPath)element.GetValue(Storyboard.TargetPropertyProperty);
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x00070A3C File Offset: 0x0006EC3C
		internal static DependencyObject ResolveTargetName(string targetName, INameScope nameScope, DependencyObject element)
		{
			FrameworkElement frameworkElement = element as FrameworkElement;
			FrameworkContentElement frameworkContentElement = element as FrameworkContentElement;
			object obj;
			object obj2;
			if (frameworkElement != null)
			{
				if (nameScope != null)
				{
					obj = ((FrameworkTemplate)nameScope).FindName(targetName, frameworkElement);
					obj2 = nameScope;
				}
				else
				{
					obj = frameworkElement.FindName(targetName);
					obj2 = frameworkElement;
				}
			}
			else
			{
				if (frameworkContentElement == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_NoNameScope", new object[]
					{
						targetName
					}));
				}
				obj = frameworkContentElement.FindName(targetName);
				obj2 = frameworkContentElement;
			}
			if (obj == null)
			{
				throw new InvalidOperationException(SR.Get("Storyboard_NameNotFound", new object[]
				{
					targetName,
					obj2.GetType().ToString()
				}));
			}
			DependencyObject dependencyObject = obj as DependencyObject;
			if (dependencyObject == null)
			{
				throw new InvalidOperationException(SR.Get("Storyboard_TargetNameNotDependencyObject", new object[]
				{
					targetName
				}));
			}
			return dependencyObject;
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x00070B00 File Offset: 0x0006ED00
		internal static BeginStoryboard ResolveBeginStoryboardName(string targetName, INameScope nameScope, FrameworkElement fe, FrameworkContentElement fce)
		{
			object obj;
			if (nameScope != null)
			{
				obj = nameScope.FindName(targetName);
				if (obj == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_NameNotFound", new object[]
					{
						targetName,
						nameScope.GetType().ToString()
					}));
				}
			}
			else if (fe != null)
			{
				obj = fe.FindName(targetName);
				if (obj == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_NameNotFound", new object[]
					{
						targetName,
						fe.GetType().ToString()
					}));
				}
			}
			else
			{
				if (fce == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_NoNameScope", new object[]
					{
						targetName
					}));
				}
				obj = fce.FindName(targetName);
				if (obj == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_NameNotFound", new object[]
					{
						targetName,
						fce.GetType().ToString()
					}));
				}
			}
			BeginStoryboard beginStoryboard = obj as BeginStoryboard;
			if (beginStoryboard == null)
			{
				throw new InvalidOperationException(SR.Get("Storyboard_BeginStoryboardNameNotFound", new object[]
				{
					targetName
				}));
			}
			return beginStoryboard;
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x00070BF8 File Offset: 0x0006EDF8
		private void ClockTreeWalkRecursive(Clock currentClock, DependencyObject containingObject, INameScope nameScope, DependencyObject parentObject, string parentObjectName, PropertyPath parentPropertyPath, HandoffBehavior handoffBehavior, HybridDictionary clockMappings, long layer)
		{
			Timeline timeline = currentClock.Timeline;
			DependencyObject dependencyObject = parentObject;
			string text = parentObjectName;
			PropertyPath propertyPath = parentPropertyPath;
			string text2 = (string)timeline.GetValue(Storyboard.TargetNameProperty);
			if (text2 != null)
			{
				if (nameScope is Style)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_TargetNameNotAllowedInStyle", new object[]
					{
						text2
					}));
				}
				text = text2;
			}
			DependencyObject dependencyObject2 = (DependencyObject)timeline.GetValue(Storyboard.TargetProperty);
			if (dependencyObject2 != null)
			{
				dependencyObject = dependencyObject2;
				text = null;
			}
			PropertyPath propertyPath2 = (PropertyPath)timeline.GetValue(Storyboard.TargetPropertyProperty);
			if (propertyPath2 != null)
			{
				propertyPath = propertyPath2;
			}
			if (currentClock is AnimationClock)
			{
				AnimationClock animationClock = (AnimationClock)currentClock;
				if (dependencyObject == null)
				{
					if (text != null)
					{
						DependencyObject element = Helper.FindMentor(containingObject);
						dependencyObject = Storyboard.ResolveTargetName(text, nameScope, element);
					}
					else
					{
						dependencyObject = (containingObject as FrameworkElement);
						if (dependencyObject == null)
						{
							dependencyObject = (containingObject as FrameworkContentElement);
						}
						if (dependencyObject == null)
						{
							throw new InvalidOperationException(SR.Get("Storyboard_NoTarget", new object[]
							{
								timeline.GetType().ToString()
							}));
						}
					}
				}
				if (propertyPath == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_TargetPropertyRequired", new object[]
					{
						timeline.GetType().ToString()
					}));
				}
				using (propertyPath.SetContext(dependencyObject))
				{
					if (propertyPath.Length < 1)
					{
						throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathEmpty"));
					}
					Storyboard.VerifyPathIsAnimatable(propertyPath);
					if (propertyPath.Length != 1)
					{
						this.ProcessComplexPath(clockMappings, dependencyObject, propertyPath, animationClock, handoffBehavior, layer);
						return;
					}
					DependencyProperty dependencyProperty = propertyPath.GetAccessor(0) as DependencyProperty;
					if (dependencyProperty == null)
					{
						throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathMustPointToDependencyProperty", new object[]
						{
							propertyPath.Path
						}));
					}
					Storyboard.VerifyAnimationIsValid(dependencyProperty, animationClock);
					Storyboard.ObjectPropertyPair mappingKey = new Storyboard.ObjectPropertyPair(dependencyObject, dependencyProperty);
					Storyboard.UpdateMappings(clockMappings, mappingKey, animationClock);
					return;
				}
			}
			if (currentClock is MediaClock)
			{
				Storyboard.ApplyMediaClock(nameScope, containingObject, dependencyObject, text, (MediaClock)currentClock);
				return;
			}
			ClockGroup clockGroup = currentClock as ClockGroup;
			if (clockGroup != null)
			{
				ClockCollection children = clockGroup.Children;
				for (int i = 0; i < children.Count; i++)
				{
					this.ClockTreeWalkRecursive(children[i], containingObject, nameScope, dependencyObject, text, propertyPath, handoffBehavior, clockMappings, layer);
				}
			}
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x00070E24 File Offset: 0x0006F024
		private static void ApplyMediaClock(INameScope nameScope, DependencyObject containingObject, DependencyObject currentObject, string currentObjectName, MediaClock mediaClock)
		{
			MediaElement mediaElement;
			if (currentObjectName != null)
			{
				DependencyObject element = Helper.FindMentor(containingObject);
				mediaElement = (Storyboard.ResolveTargetName(currentObjectName, nameScope, element) as MediaElement);
				if (mediaElement == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_MediaElementNotFound", new object[]
					{
						currentObjectName
					}));
				}
			}
			else if (currentObject != null)
			{
				mediaElement = (currentObject as MediaElement);
			}
			else
			{
				mediaElement = (containingObject as MediaElement);
			}
			if (mediaElement == null)
			{
				throw new InvalidOperationException(SR.Get("Storyboard_MediaElementRequired"));
			}
			mediaElement.Clock = mediaClock;
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x00070E98 File Offset: 0x0006F098
		private static void UpdateMappings(HybridDictionary clockMappings, Storyboard.ObjectPropertyPair mappingKey, AnimationClock animationClock)
		{
			object obj = clockMappings[mappingKey];
			if (obj == null)
			{
				clockMappings[mappingKey] = animationClock;
				return;
			}
			if (obj is AnimationClock)
			{
				clockMappings[mappingKey] = new List<AnimationClock>
				{
					(AnimationClock)obj,
					animationClock
				};
				return;
			}
			List<AnimationClock> list = (List<AnimationClock>)obj;
			list.Add(animationClock);
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x00070EF4 File Offset: 0x0006F0F4
		private static void ApplyAnimationClocks(HybridDictionary clockMappings, HandoffBehavior handoffBehavior, long layer)
		{
			foreach (object obj in clockMappings)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				Storyboard.ObjectPropertyPair objectPropertyPair = (Storyboard.ObjectPropertyPair)dictionaryEntry.Key;
				object value = dictionaryEntry.Value;
				List<AnimationClock> list;
				if (value is AnimationClock)
				{
					list = new List<AnimationClock>(1);
					list.Add((AnimationClock)value);
				}
				else
				{
					list = (List<AnimationClock>)value;
				}
				AnimationStorage.ApplyAnimationClocksToLayer(objectPropertyPair.DependencyObject, objectPropertyPair.DependencyProperty, list, handoffBehavior, layer);
			}
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x00070F98 File Offset: 0x0006F198
		internal static void VerifyPathIsAnimatable(PropertyPath path)
		{
			bool flag = true;
			for (int i = 0; i < path.Length; i++)
			{
				object item = path.GetItem(i);
				object accessor = path.GetAccessor(i);
				if (item == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathObjectNotFound", new object[]
					{
						Storyboard.AccessorName(path, i - 1),
						path.Path
					}));
				}
				if (accessor == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathPropertyNotFound", new object[]
					{
						path.Path
					}));
				}
				if (i == 1)
				{
					Freezable freezable = item as Freezable;
					if (freezable != null && freezable.IsFrozen)
					{
						flag = false;
					}
				}
				else if (flag)
				{
					Freezable freezable = item as Freezable;
					if (freezable != null && freezable.IsFrozen)
					{
						if (i > 0)
						{
							throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathFrozenCheckFailed", new object[]
							{
								Storyboard.AccessorName(path, i - 1),
								path.Path,
								freezable.GetType().ToString()
							}));
						}
						throw new InvalidOperationException(SR.Get("Storyboard_ImmutableTargetNotSupported", new object[]
						{
							path.Path
						}));
					}
				}
				if (i == path.Length - 1)
				{
					DependencyObject dependencyObject = item as DependencyObject;
					DependencyProperty dependencyProperty = accessor as DependencyProperty;
					if (dependencyObject == null)
					{
						throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathMustPointToDependencyObject", new object[]
						{
							Storyboard.AccessorName(path, i - 1),
							path.Path
						}));
					}
					if (dependencyProperty == null)
					{
						throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathMustPointToDependencyProperty", new object[]
						{
							path.Path
						}));
					}
					if (flag && dependencyObject.IsSealed)
					{
						throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathSealedCheckFailed", new object[]
						{
							dependencyProperty.Name,
							path.Path,
							dependencyObject
						}));
					}
					if (!AnimationStorage.IsPropertyAnimatable(dependencyObject, dependencyProperty))
					{
						throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathIncludesNonAnimatableProperty", new object[]
						{
							path.Path,
							dependencyProperty.Name
						}));
					}
				}
			}
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x0007119C File Offset: 0x0006F39C
		private static string AccessorName(PropertyPath path, int index)
		{
			object accessor = path.GetAccessor(index);
			if (accessor is DependencyProperty)
			{
				return ((DependencyProperty)accessor).Name;
			}
			if (accessor is PropertyInfo)
			{
				return ((PropertyInfo)accessor).Name;
			}
			if (accessor is PropertyDescriptor)
			{
				return ((PropertyDescriptor)accessor).Name;
			}
			return "[Unknown]";
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x000711F4 File Offset: 0x0006F3F4
		private static void VerifyAnimationIsValid(DependencyProperty targetProperty, AnimationClock animationClock)
		{
			if (!AnimationStorage.IsAnimationClockValid(targetProperty, animationClock))
			{
				throw new InvalidOperationException(SR.Get("Storyboard_AnimationMismatch", new object[]
				{
					animationClock.Timeline.GetType(),
					targetProperty.Name,
					targetProperty.PropertyType
				}));
			}
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x00071240 File Offset: 0x0006F440
		private void ProcessComplexPath(HybridDictionary clockMappings, DependencyObject targetObject, PropertyPath path, AnimationClock animationClock, HandoffBehavior handoffBehavior, long layer)
		{
			DependencyProperty dependencyProperty = path.GetAccessor(0) as DependencyProperty;
			object value = targetObject.GetValue(dependencyProperty);
			DependencyObject dependencyObject = path.LastItem as DependencyObject;
			DependencyProperty dependencyProperty2 = path.LastAccessor as DependencyProperty;
			if (dependencyObject == null || dependencyProperty2 == null || dependencyProperty == null)
			{
				throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathUnresolved", new object[]
				{
					path.Path
				}));
			}
			Storyboard.VerifyAnimationIsValid(dependencyProperty2, animationClock);
			if (this.PropertyCloningRequired(value))
			{
				this.VerifyComplexPathSupport(targetObject);
				Freezable freezable = ((Freezable)value).Clone();
				Storyboard.SetComplexPathClone(targetObject, dependencyProperty, value, freezable);
				targetObject.InvalidateProperty(dependencyProperty);
				if (targetObject.GetValue(dependencyProperty) != freezable)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_ImmutableTargetNotSupported", new object[]
					{
						path.Path
					}));
				}
				using (path.SetContext(targetObject))
				{
					dependencyObject = (path.LastItem as DependencyObject);
					dependencyProperty2 = (path.LastAccessor as DependencyProperty);
				}
				Storyboard.ChangeListener.ListenToChangesOnFreezable(targetObject, freezable, dependencyProperty, (Freezable)value);
			}
			Storyboard.ObjectPropertyPair mappingKey = new Storyboard.ObjectPropertyPair(dependencyObject, dependencyProperty2);
			Storyboard.UpdateMappings(clockMappings, mappingKey, animationClock);
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x00071368 File Offset: 0x0006F568
		private bool PropertyCloningRequired(object targetPropertyValue)
		{
			return targetPropertyValue is Freezable && ((Freezable)targetPropertyValue).IsFrozen;
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x00071398 File Offset: 0x0006F598
		private void VerifyComplexPathSupport(DependencyObject targetObject)
		{
			if (FrameworkElement.DType.IsInstanceOfType(targetObject))
			{
				return;
			}
			if (FrameworkContentElement.DType.IsInstanceOfType(targetObject))
			{
				return;
			}
			throw new InvalidOperationException(SR.Get("Storyboard_ComplexPathNotSupported", new object[]
			{
				targetObject.GetType().ToString()
			}));
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x000713E4 File Offset: 0x0006F5E4
		internal static void GetComplexPathValue(DependencyObject targetObject, DependencyProperty targetProperty, ref EffectiveValueEntry entry, PropertyMetadata metadata)
		{
			Storyboard.CloneCacheEntry complexPathClone = Storyboard.GetComplexPathClone(targetObject, targetProperty);
			if (complexPathClone != null)
			{
				object value = entry.Value;
				if (value == DependencyProperty.UnsetValue && complexPathClone.Source == metadata.GetDefaultValue(targetObject, targetProperty))
				{
					entry.BaseValueSourceInternal = BaseValueSourceInternal.Default;
					entry.SetAnimatedValue(complexPathClone.Clone, DependencyProperty.UnsetValue);
					return;
				}
				DeferredReference deferredReference = value as DeferredReference;
				if (deferredReference != null)
				{
					value = deferredReference.GetValue(entry.BaseValueSourceInternal);
					entry.Value = value;
				}
				if (complexPathClone.Source == value)
				{
					Storyboard.CloneEffectiveValue(ref entry, complexPathClone);
					return;
				}
				Storyboard.SetComplexPathClone(targetObject, targetProperty, DependencyProperty.UnsetValue, DependencyProperty.UnsetValue);
			}
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x00071474 File Offset: 0x0006F674
		private static void CloneEffectiveValue(ref EffectiveValueEntry entry, Storyboard.CloneCacheEntry cacheEntry)
		{
			object clone = cacheEntry.Clone;
			if (!entry.IsExpression)
			{
				entry.Value = clone;
				return;
			}
			entry.ModifiedValue.ExpressionValue = clone;
		}

		/// <summary>Applies the animations associated with this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to their targets and initiates them.</summary>
		/// <param name="containingObject">An object contained within the same name scope as the targets of this storyboard's animations. Animations without a <see cref="P:System.Windows.Media.Animation.Storyboard.TargetName" /> are applied to <paramref name="containingObject" />.</param>
		// Token: 0x060016C5 RID: 5829 RVA: 0x000714A4 File Offset: 0x0006F6A4
		public void Begin(FrameworkElement containingObject)
		{
			this.Begin(containingObject, HandoffBehavior.SnapshotAndReplace, false);
		}

		/// <summary>Applies the animations associated with this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to their targets and initiates them, using the specified <see cref="T:System.Windows.Media.Animation.HandoffBehavior" />.</summary>
		/// <param name="containingObject">An object contained within the same name scope as the targets of this storyboard's animations. Animations without a specified <see cref="P:System.Windows.Media.Animation.Storyboard.TargetName" /> are applied to <paramref name="containingObject" />.</param>
		/// <param name="handoffBehavior">The behavior the new animation should use to interact with any current animations.</param>
		// Token: 0x060016C6 RID: 5830 RVA: 0x000714AF File Offset: 0x0006F6AF
		public void Begin(FrameworkElement containingObject, HandoffBehavior handoffBehavior)
		{
			this.Begin(containingObject, handoffBehavior, false);
		}

		/// <summary>Applies the animations associated with this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to their targets and initiates them.</summary>
		/// <param name="containingObject">An object contained within the same name scope as the targets of this storyboard's animations. Animations without a <see cref="P:System.Windows.Media.Animation.Storyboard.TargetName" /> are applied to <paramref name="containingObject" />.</param>
		/// <param name="isControllable">
		///       <see langword="true" /> if the storyboard should be interactively controllable; otherwise, <see langword="false" />.</param>
		// Token: 0x060016C7 RID: 5831 RVA: 0x000714BA File Offset: 0x0006F6BA
		public void Begin(FrameworkElement containingObject, bool isControllable)
		{
			this.Begin(containingObject, HandoffBehavior.SnapshotAndReplace, isControllable);
		}

		/// <summary>Applies the animations associated with this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to their targets and initiates them.</summary>
		/// <param name="containingObject">An object contained within the same name scope as the targets of this storyboard's animations. Animations without a specified <see cref="P:System.Windows.Media.Animation.Storyboard.TargetName" /> are applied to <paramref name="containingObject" />.</param>
		/// <param name="handoffBehavior">The behavior the new animation should use to interact with any current animations.</param>
		/// <param name="isControllable">Declares whether the animation is controllable (can be paused) once started.</param>
		// Token: 0x060016C8 RID: 5832 RVA: 0x000714C5 File Offset: 0x0006F6C5
		public void Begin(FrameworkElement containingObject, HandoffBehavior handoffBehavior, bool isControllable)
		{
			this.BeginCommon(containingObject, null, handoffBehavior, isControllable, Storyboard.Layers.Code);
		}

		/// <summary>Applies the animations associated with this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to their targets within the specified template and initiates them.</summary>
		/// <param name="containingObject">The object to which the specified <paramref name="frameworkTemplate" /> has been applied. Animations without a <see cref="P:System.Windows.Media.Animation.Storyboard.TargetName" /> are applied to <paramref name="containingObject" />. </param>
		/// <param name="frameworkTemplate">The template to animate. </param>
		// Token: 0x060016C9 RID: 5833 RVA: 0x000714D6 File Offset: 0x0006F6D6
		public void Begin(FrameworkElement containingObject, FrameworkTemplate frameworkTemplate)
		{
			this.Begin(containingObject, frameworkTemplate, HandoffBehavior.SnapshotAndReplace, false);
		}

		/// <summary>Applies the animations associated with this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to their targets within the specified template and initiates them.</summary>
		/// <param name="containingObject">The object to which the specified <paramref name="frameworkTemplate" /> has been applied. Animations without a <see cref="P:System.Windows.Media.Animation.Storyboard.TargetName" /> are applied to <paramref name="containingObject" />.</param>
		/// <param name="frameworkTemplate">The template to animate.</param>
		/// <param name="handoffBehavior">The behavior the new animation should use to interact with any current animations.</param>
		// Token: 0x060016CA RID: 5834 RVA: 0x000714E2 File Offset: 0x0006F6E2
		public void Begin(FrameworkElement containingObject, FrameworkTemplate frameworkTemplate, HandoffBehavior handoffBehavior)
		{
			this.Begin(containingObject, frameworkTemplate, handoffBehavior, false);
		}

		/// <summary>Applies the animations associated with this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to their targets within the specified template and initiates them.</summary>
		/// <param name="containingObject">The object to which the specified <paramref name="frameworkTemplate" /> has been applied.  Animations without a <see cref="P:System.Windows.Media.Animation.Storyboard.TargetName" /> are applied to <paramref name="containingObject" />.</param>
		/// <param name="frameworkTemplate">The template to animate.</param>
		/// <param name="isControllable">
		///       <see langword="true" /> if the storyboard should be interactively controllable; otherwise, <see langword="false" />.</param>
		// Token: 0x060016CB RID: 5835 RVA: 0x000714EE File Offset: 0x0006F6EE
		public void Begin(FrameworkElement containingObject, FrameworkTemplate frameworkTemplate, bool isControllable)
		{
			this.Begin(containingObject, frameworkTemplate, HandoffBehavior.SnapshotAndReplace, isControllable);
		}

		/// <summary>Applies the animations associated with this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to their targets within the specified template and initiates them.</summary>
		/// <param name="containingObject">The object to which the specified <paramref name="frameworkTemplate" /> has been applied. Animations without a <see cref="P:System.Windows.Media.Animation.Storyboard.TargetName" /> are applied to <paramref name="containingObject" />.</param>
		/// <param name="frameworkTemplate">The template to animate.</param>
		/// <param name="handoffBehavior">The behavior the new animation should use to interact with any current animations.</param>
		/// <param name="isControllable">
		///       <see langword="true" /> if the storyboard should be interactively controllable; otherwise, <see langword="false" />.</param>
		// Token: 0x060016CC RID: 5836 RVA: 0x000714FA File Offset: 0x0006F6FA
		public void Begin(FrameworkElement containingObject, FrameworkTemplate frameworkTemplate, HandoffBehavior handoffBehavior, bool isControllable)
		{
			this.BeginCommon(containingObject, frameworkTemplate, handoffBehavior, isControllable, Storyboard.Layers.Code);
		}

		/// <summary>Applies the animations associated with this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to their targets and initiates them.</summary>
		/// <param name="containingObject">An object contained within the same name scope as the targets of this storyboard's animations. Animations without a <see cref="P:System.Windows.Media.Animation.Storyboard.TargetName" /> are applied to <paramref name="containingObject" />. </param>
		// Token: 0x060016CD RID: 5837 RVA: 0x0007150C File Offset: 0x0006F70C
		public void Begin(FrameworkContentElement containingObject)
		{
			this.Begin(containingObject, HandoffBehavior.SnapshotAndReplace, false);
		}

		/// <summary>Applies the animations associated with this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to their targets and initiates them, using the specified <see cref="T:System.Windows.Media.Animation.HandoffBehavior" />.</summary>
		/// <param name="containingObject">An object contained within the same name scope as the targets of this storyboard's animations. Animations without a <see cref="P:System.Windows.Media.Animation.Storyboard.TargetName" /> are applied to <paramref name="containingObject" />.</param>
		/// <param name="handoffBehavior">The behavior the new animation should use to interact with any current animations.</param>
		// Token: 0x060016CE RID: 5838 RVA: 0x00071517 File Offset: 0x0006F717
		public void Begin(FrameworkContentElement containingObject, HandoffBehavior handoffBehavior)
		{
			this.Begin(containingObject, handoffBehavior, false);
		}

		/// <summary>Applies the animations associated with this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to their targets and initiates them.</summary>
		/// <param name="containingObject">An object contained within the same name scope as the targets of this storyboard's animations. Animations without a <see cref="P:System.Windows.Media.Animation.Storyboard.TargetName" /> are applied to <paramref name="containingObject" />.</param>
		/// <param name="isControllable">
		///       <see langword="true" /> if the storyboard should be interactively controllable; otherwise, <see langword="false" />.</param>
		// Token: 0x060016CF RID: 5839 RVA: 0x00071522 File Offset: 0x0006F722
		public void Begin(FrameworkContentElement containingObject, bool isControllable)
		{
			this.Begin(containingObject, HandoffBehavior.SnapshotAndReplace, isControllable);
		}

		/// <summary>Applies the animations associated with this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to their targets and initiates them, using the specified <see cref="T:System.Windows.Media.Animation.HandoffBehavior" />. </summary>
		/// <param name="containingObject">An object contained within the same name scope as the targets of this storyboard's animations. Animations without a specified <see cref="P:System.Windows.Media.Animation.Storyboard.TargetName" /> are applied to <paramref name="containingObject" />.</param>
		/// <param name="handoffBehavior">The behavior the new animation should use to interact with any current animations.</param>
		/// <param name="isControllable">Declares whether the animation is controllable (can be paused) once started.</param>
		// Token: 0x060016D0 RID: 5840 RVA: 0x000714C5 File Offset: 0x0006F6C5
		public void Begin(FrameworkContentElement containingObject, HandoffBehavior handoffBehavior, bool isControllable)
		{
			this.BeginCommon(containingObject, null, handoffBehavior, isControllable, Storyboard.Layers.Code);
		}

		/// <summary>Applies the animations associated with this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to their targets and initiates them.</summary>
		// Token: 0x060016D1 RID: 5841 RVA: 0x00071530 File Offset: 0x0006F730
		public void Begin()
		{
			INameScope nameScope = null;
			HandoffBehavior handoffBehavior = HandoffBehavior.SnapshotAndReplace;
			bool isControllable = true;
			long code = Storyboard.Layers.Code;
			this.BeginCommon(this, nameScope, handoffBehavior, isControllable, code);
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x00071558 File Offset: 0x0006F758
		internal void BeginCommon(DependencyObject containingObject, INameScope nameScope, HandoffBehavior handoffBehavior, bool isControllable, long layer)
		{
			if (containingObject == null)
			{
				throw new ArgumentNullException("containingObject");
			}
			if (!HandoffBehaviorEnum.IsDefined(handoffBehavior))
			{
				throw new ArgumentException(SR.Get("Storyboard_UnrecognizedHandoffBehavior"));
			}
			if (base.BeginTime == null)
			{
				return;
			}
			if (MediaContext.CurrentMediaContext.TimeManager == null)
			{
				return;
			}
			if (TraceAnimation.IsEnabled)
			{
				TraceAnimation.TraceActivityItem(TraceAnimation.StoryboardBegin, new object[]
				{
					this,
					base.Name,
					containingObject,
					nameScope
				});
			}
			HybridDictionary clockMappings = new HybridDictionary();
			Clock clock = base.CreateClock(isControllable);
			this.ClockTreeWalkRecursive(clock, containingObject, nameScope, null, null, null, handoffBehavior, clockMappings, layer);
			Storyboard.ApplyAnimationClocks(clockMappings, handoffBehavior, layer);
			if (isControllable)
			{
				this.SetStoryboardClock(containingObject, clock);
			}
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Clock.CurrentGlobalSpeed" /> of the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <returns>The current global speed, or <see langword="null" /> if the clock is stopped. </returns>
		// Token: 0x060016D3 RID: 5843 RVA: 0x00071609 File Offset: 0x0006F809
		public double? GetCurrentGlobalSpeed(FrameworkElement containingObject)
		{
			return this.GetCurrentGlobalSpeedImpl(containingObject);
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Clock.CurrentGlobalSpeed" /> of the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />. </summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkContentElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <returns>The current global speed, or <see langword="null" /> if the clock is stopped. </returns>
		// Token: 0x060016D4 RID: 5844 RVA: 0x00071609 File Offset: 0x0006F809
		public double? GetCurrentGlobalSpeed(FrameworkContentElement containingObject)
		{
			return this.GetCurrentGlobalSpeedImpl(containingObject);
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Clock.CurrentGlobalSpeed" /> of the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />. </summary>
		/// <returns>The current global speed, or 0 if the clock is stopped. </returns>
		// Token: 0x060016D5 RID: 5845 RVA: 0x00071614 File Offset: 0x0006F814
		public double GetCurrentGlobalSpeed()
		{
			double? currentGlobalSpeedImpl = this.GetCurrentGlobalSpeedImpl(this);
			if (currentGlobalSpeedImpl != null)
			{
				return currentGlobalSpeedImpl.Value;
			}
			return 0.0;
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x00071644 File Offset: 0x0006F844
		private double? GetCurrentGlobalSpeedImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject);
			if (storyboardClock != null)
			{
				return storyboardClock.CurrentGlobalSpeed;
			}
			return null;
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Clock.CurrentIteration" /> of the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <returns>This clock's current iteration within its current active period, or <see langword="null" /> if this clock is stopped.</returns>
		// Token: 0x060016D7 RID: 5847 RVA: 0x0007166C File Offset: 0x0006F86C
		public int? GetCurrentIteration(FrameworkElement containingObject)
		{
			return this.GetCurrentIterationImpl(containingObject);
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Clock.CurrentIteration" /> of the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkContentElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <returns>This clock's current iteration within its current active period, or <see langword="null" /> if this clock is stopped.</returns>
		// Token: 0x060016D8 RID: 5848 RVA: 0x0007166C File Offset: 0x0006F86C
		public int? GetCurrentIteration(FrameworkContentElement containingObject)
		{
			return this.GetCurrentIterationImpl(containingObject);
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Clock.CurrentIteration" /> of the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
		/// <returns>This clock's current iteration within its current active period, or <see langword="null" /> if this clock is stopped.</returns>
		// Token: 0x060016D9 RID: 5849 RVA: 0x00071678 File Offset: 0x0006F878
		public int GetCurrentIteration()
		{
			int? currentIterationImpl = this.GetCurrentIterationImpl(this);
			if (currentIterationImpl != null)
			{
				return currentIterationImpl.Value;
			}
			return 0;
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x000716A0 File Offset: 0x0006F8A0
		private int? GetCurrentIterationImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject);
			if (storyboardClock != null)
			{
				return storyboardClock.CurrentIteration;
			}
			return null;
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Clock.CurrentProgress" /> of the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <returns>
		///     <see langword="null" /> if this clock is <see cref="F:System.Windows.Media.Animation.ClockState.Stopped" />, or 0.0 if this clock is active and its <see cref="P:System.Windows.Media.Animation.Clock.Timeline" /> has a <see cref="P:System.Windows.Media.Animation.Timeline.Duration" /> of <see cref="P:System.Windows.Duration.Forever" />; otherwise, a value between 0.0 and 1.0 that indicates the current progress of this clock within its current iteration. A value of 0.0 indicates no progress, and a value of 1.0 indicates that the clock is at the end of its current iteration.</returns>
		// Token: 0x060016DB RID: 5851 RVA: 0x000716C8 File Offset: 0x0006F8C8
		public double? GetCurrentProgress(FrameworkElement containingObject)
		{
			return this.GetCurrentProgressImpl(containingObject);
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Clock.CurrentProgress" /> of the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkContentElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <returns>
		///     <see langword="null" /> if this clock is <see cref="F:System.Windows.Media.Animation.ClockState.Stopped" />, or 0.0 if this clock is active and its <see cref="P:System.Windows.Media.Animation.Clock.Timeline" /> has a <see cref="P:System.Windows.Media.Animation.Timeline.Duration" /> of <see cref="P:System.Windows.Duration.Forever" />; otherwise, a value between 0.0 and 1.0 that indicates the current progress of this clock within its current iteration. A value of 0.0 indicates no progress, and a value of 1.0 indicates that the clock is at the end of its current iteration.</returns>
		// Token: 0x060016DC RID: 5852 RVA: 0x000716C8 File Offset: 0x0006F8C8
		public double? GetCurrentProgress(FrameworkContentElement containingObject)
		{
			return this.GetCurrentProgressImpl(containingObject);
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Clock.CurrentProgress" /> of the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
		/// <returns>
		///     <see langword="null" /> if this clock is <see cref="F:System.Windows.Media.Animation.ClockState.Stopped" />, or 0.0 if this clock is active and its <see cref="P:System.Windows.Media.Animation.Clock.Timeline" /> has a <see cref="P:System.Windows.Media.Animation.Timeline.Duration" /> of <see cref="P:System.Windows.Duration.Forever" />; otherwise, a value between 0.0 and 1.0 that indicates the current progress of this clock within its current iteration. A value of 0.0 indicates no progress, and a value of 1.0 indicates that the clock is at the end of its current iteration.</returns>
		// Token: 0x060016DD RID: 5853 RVA: 0x000716D4 File Offset: 0x0006F8D4
		public double GetCurrentProgress()
		{
			double? currentProgressImpl = this.GetCurrentProgressImpl(this);
			if (currentProgressImpl != null)
			{
				return currentProgressImpl.Value;
			}
			return 0.0;
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x00071704 File Offset: 0x0006F904
		private double? GetCurrentProgressImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject);
			if (storyboardClock != null)
			{
				return storyboardClock.CurrentProgress;
			}
			return null;
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Clock.CurrentState" /> of the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <returns>The current state of the clock created for this storyboard: <see cref="F:System.Windows.Media.Animation.ClockState.Active" />, <see cref="F:System.Windows.Media.Animation.ClockState.Filling" />, or <see cref="F:System.Windows.Media.Animation.ClockState.Stopped" />.</returns>
		// Token: 0x060016DF RID: 5855 RVA: 0x0007172C File Offset: 0x0006F92C
		public ClockState GetCurrentState(FrameworkElement containingObject)
		{
			return this.GetCurrentStateImpl(containingObject);
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Clock.CurrentState" /> of the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkContentElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <returns>The current state of the clock created for this storyboard: <see cref="F:System.Windows.Media.Animation.ClockState.Active" />, <see cref="F:System.Windows.Media.Animation.ClockState.Filling" />, or <see cref="F:System.Windows.Media.Animation.ClockState.Stopped" />.</returns>
		// Token: 0x060016E0 RID: 5856 RVA: 0x0007172C File Offset: 0x0006F92C
		public ClockState GetCurrentState(FrameworkContentElement containingObject)
		{
			return this.GetCurrentStateImpl(containingObject);
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Clock.CurrentState" /> of the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
		/// <returns>The current state of the clock created for this storyboard: <see cref="F:System.Windows.Media.Animation.ClockState.Active" />, <see cref="F:System.Windows.Media.Animation.ClockState.Filling" />, or <see cref="F:System.Windows.Media.Animation.ClockState.Stopped" />.</returns>
		// Token: 0x060016E1 RID: 5857 RVA: 0x00071735 File Offset: 0x0006F935
		public ClockState GetCurrentState()
		{
			return this.GetCurrentStateImpl(this);
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x00071740 File Offset: 0x0006F940
		private ClockState GetCurrentStateImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject);
			if (storyboardClock != null)
			{
				return storyboardClock.CurrentState;
			}
			return ClockState.Stopped;
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Clock.CurrentTime" /> of the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <returns>
		///     <see langword="null" /> if this storyboard's clock is <see cref="F:System.Windows.Media.Animation.ClockState.Stopped" />; otherwise, the current time of the storyboard's clock.</returns>
		// Token: 0x060016E3 RID: 5859 RVA: 0x00071760 File Offset: 0x0006F960
		public TimeSpan? GetCurrentTime(FrameworkElement containingObject)
		{
			return this.GetCurrentTimeImpl(containingObject);
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Clock.CurrentTime" /> of the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkContentElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <returns>
		///     <see langword="null" /> if this storyboard's clock is <see cref="F:System.Windows.Media.Animation.ClockState.Stopped" />; otherwise, the current time of the storyboard's clock.</returns>
		// Token: 0x060016E4 RID: 5860 RVA: 0x00071760 File Offset: 0x0006F960
		public TimeSpan? GetCurrentTime(FrameworkContentElement containingObject)
		{
			return this.GetCurrentTimeImpl(containingObject);
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Media.Animation.Clock.CurrentTime" /> of the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
		/// <returns>
		///     <see langword="null" /> if this storyboard's clock is <see cref="F:System.Windows.Media.Animation.ClockState.Stopped" />; otherwise, the current time of the storyboard's clock.</returns>
		// Token: 0x060016E5 RID: 5861 RVA: 0x0007176C File Offset: 0x0006F96C
		public TimeSpan GetCurrentTime()
		{
			TimeSpan? currentTimeImpl = this.GetCurrentTimeImpl(this);
			if (currentTimeImpl != null)
			{
				return currentTimeImpl.Value;
			}
			return default(TimeSpan);
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x0007179C File Offset: 0x0006F99C
		private TimeSpan? GetCurrentTimeImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject);
			if (storyboardClock != null)
			{
				return storyboardClock.CurrentTime;
			}
			return null;
		}

		/// <summary>Retrieves a value that indicates whether the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" /> is paused.</summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Media.Animation.Clock" /> created for this <see cref="T:System.Windows.Media.Animation.Storyboard" /> is paused; otherwise, <see langword="false" />.</returns>
		// Token: 0x060016E7 RID: 5863 RVA: 0x000717C4 File Offset: 0x0006F9C4
		public bool GetIsPaused(FrameworkElement containingObject)
		{
			return this.GetIsPausedImpl(containingObject);
		}

		/// <summary>Retrieves a value that indicates whether the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" /> is paused. </summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkContentElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Media.Animation.Clock" /> created for this <see cref="T:System.Windows.Media.Animation.Storyboard" /> is paused; otherwise, <see langword="false" />.</returns>
		// Token: 0x060016E8 RID: 5864 RVA: 0x000717C4 File Offset: 0x0006F9C4
		public bool GetIsPaused(FrameworkContentElement containingObject)
		{
			return this.GetIsPausedImpl(containingObject);
		}

		/// <summary>Retrieves a value that indicates whether the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" /> is paused.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Media.Animation.Clock" /> created for this <see cref="T:System.Windows.Media.Animation.Storyboard" /> is paused; otherwise, <see langword="false" />.</returns>
		// Token: 0x060016E9 RID: 5865 RVA: 0x000717CD File Offset: 0x0006F9CD
		public bool GetIsPaused()
		{
			return this.GetIsPausedImpl(this);
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x000717D8 File Offset: 0x0006F9D8
		private bool GetIsPausedImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject);
			return storyboardClock != null && storyboardClock.IsPaused;
		}

		/// <summary>Pauses the <see cref="T:System.Windows.Media.Animation.Clock" /> of the specified <see cref="T:System.Windows.FrameworkElement" /> associated with this <see cref="T:System.Windows.Media.Animation.Storyboard" />. </summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		// Token: 0x060016EB RID: 5867 RVA: 0x000717F8 File Offset: 0x0006F9F8
		public void Pause(FrameworkElement containingObject)
		{
			this.PauseImpl(containingObject);
		}

		/// <summary>Pauses the <see cref="T:System.Windows.Media.Animation.Clock" /> of the specified <see cref="T:System.Windows.FrameworkContentElement" /> associated with this <see cref="T:System.Windows.Media.Animation.Storyboard" />. </summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkContentElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		// Token: 0x060016EC RID: 5868 RVA: 0x000717F8 File Offset: 0x0006F9F8
		public void Pause(FrameworkContentElement containingObject)
		{
			this.PauseImpl(containingObject);
		}

		/// <summary>Pauses the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />. </summary>
		// Token: 0x060016ED RID: 5869 RVA: 0x00071801 File Offset: 0x0006FA01
		public void Pause()
		{
			this.PauseImpl(this);
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x0007180C File Offset: 0x0006FA0C
		private void PauseImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject, false, Storyboard.InteractiveOperation.Pause);
			if (storyboardClock != null)
			{
				storyboardClock.Controller.Pause();
			}
			if (TraceAnimation.IsEnabled)
			{
				TraceAnimation.TraceActivityItem(TraceAnimation.StoryboardPause, this, base.Name, this);
			}
		}

		/// <summary>Removes the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />. Animations that belong to this <see cref="T:System.Windows.Media.Animation.Storyboard" /> no longer affect the properties they once animated, regardless of their <see cref="P:System.Windows.Media.Animation.Timeline.FillBehavior" /> setting. </summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		// Token: 0x060016EF RID: 5871 RVA: 0x0007184A File Offset: 0x0006FA4A
		public void Remove(FrameworkElement containingObject)
		{
			this.RemoveImpl(containingObject);
		}

		/// <summary>Removes the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />. Animations that belong to this <see cref="T:System.Windows.Media.Animation.Storyboard" /> no longer affect the properties they once animated, regardless of their <see cref="P:System.Windows.Media.Animation.Timeline.FillBehavior" /> setting.</summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkContentElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		// Token: 0x060016F0 RID: 5872 RVA: 0x0007184A File Offset: 0x0006FA4A
		public void Remove(FrameworkContentElement containingObject)
		{
			this.RemoveImpl(containingObject);
		}

		/// <summary>Removes the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />. Animations that belong to this <see cref="T:System.Windows.Media.Animation.Storyboard" /> no longer affect the properties they once animated, regardless of their <see cref="P:System.Windows.Media.Animation.Timeline.FillBehavior" /> setting.</summary>
		// Token: 0x060016F1 RID: 5873 RVA: 0x00071853 File Offset: 0x0006FA53
		public void Remove()
		{
			this.RemoveImpl(this);
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x0007185C File Offset: 0x0006FA5C
		private void RemoveImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject, false, Storyboard.InteractiveOperation.Remove);
			if (storyboardClock != null)
			{
				storyboardClock.Controller.Remove();
				HybridDictionary value = Storyboard.StoryboardClockTreesField.GetValue(containingObject);
				if (value != null)
				{
					value.Remove(this);
				}
			}
			if (TraceAnimation.IsEnabled)
			{
				TraceAnimation.TraceActivityItem(TraceAnimation.StoryboardRemove, this, base.Name, containingObject);
			}
		}

		/// <summary>Resumes the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />. </summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		// Token: 0x060016F3 RID: 5875 RVA: 0x000718B0 File Offset: 0x0006FAB0
		public void Resume(FrameworkElement containingObject)
		{
			this.ResumeImpl(containingObject);
		}

		/// <summary>Resumes the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />. </summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkContentElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		// Token: 0x060016F4 RID: 5876 RVA: 0x000718B0 File Offset: 0x0006FAB0
		public void Resume(FrameworkContentElement containingObject)
		{
			this.ResumeImpl(containingObject);
		}

		/// <summary>Resumes the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />. </summary>
		// Token: 0x060016F5 RID: 5877 RVA: 0x000718B9 File Offset: 0x0006FAB9
		public void Resume()
		{
			this.ResumeImpl(this);
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x000718C4 File Offset: 0x0006FAC4
		private void ResumeImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject, false, Storyboard.InteractiveOperation.Resume);
			if (storyboardClock != null)
			{
				storyboardClock.Controller.Resume();
			}
			if (TraceAnimation.IsEnabled)
			{
				TraceAnimation.TraceActivityItem(TraceAnimation.StoryboardResume, this, base.Name, containingObject);
			}
		}

		/// <summary>Seeks this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to the specified position. The <see cref="T:System.Windows.Media.Animation.Storyboard" /> performs the requested seek when the next clock tick occurs. </summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <param name="offset">A positive or negative value that describes the amount by which the timeline should move forward or backward from the specified <paramref name="origin" />.</param>
		/// <param name="origin">The position from which <paramref name="offset" /> is applied.</param>
		// Token: 0x060016F7 RID: 5879 RVA: 0x00071902 File Offset: 0x0006FB02
		public void Seek(FrameworkElement containingObject, TimeSpan offset, TimeSeekOrigin origin)
		{
			this.SeekImpl(containingObject, offset, origin);
		}

		/// <summary>Seeks this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to the specified position. The <see cref="T:System.Windows.Media.Animation.Storyboard" /> performs the requested seek when the next clock tick occurs.</summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkContentElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <param name="offset">A positive or negative value that describes the amount by which the timeline should move forward or backward from the specified <paramref name="origin" />.</param>
		/// <param name="origin">The position from which <paramref name="offset" /> is applied.</param>
		// Token: 0x060016F8 RID: 5880 RVA: 0x00071902 File Offset: 0x0006FB02
		public void Seek(FrameworkContentElement containingObject, TimeSpan offset, TimeSeekOrigin origin)
		{
			this.SeekImpl(containingObject, offset, origin);
		}

		/// <summary>Seeks this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to the specified position. The <see cref="T:System.Windows.Media.Animation.Storyboard" /> performs the requested seek when the next clock tick occurs.</summary>
		/// <param name="offset">A positive or negative value that describes the amount by which the timeline should move forward or backward from the specified <paramref name="origin" />.</param>
		/// <param name="origin">The position from which <paramref name="offset" /> is applied.</param>
		// Token: 0x060016F9 RID: 5881 RVA: 0x0007190D File Offset: 0x0006FB0D
		public void Seek(TimeSpan offset, TimeSeekOrigin origin)
		{
			this.SeekImpl(this, offset, origin);
		}

		/// <summary>Seeks this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to the specified position. The <see cref="T:System.Windows.Media.Animation.Storyboard" /> performs the requested seek when the next clock tick occurs.</summary>
		/// <param name="offset">A positive or negative value that describes the amount by which the timeline should move forward or backward. </param>
		// Token: 0x060016FA RID: 5882 RVA: 0x00071918 File Offset: 0x0006FB18
		public void Seek(TimeSpan offset)
		{
			this.SeekImpl(this, offset, TimeSeekOrigin.BeginTime);
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x00071924 File Offset: 0x0006FB24
		private void SeekImpl(DependencyObject containingObject, TimeSpan offset, TimeSeekOrigin origin)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject, false, Storyboard.InteractiveOperation.Seek);
			if (storyboardClock != null)
			{
				storyboardClock.Controller.Seek(offset, origin);
			}
		}

		/// <summary>Seeks this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to a new position immediately (synchronously).</summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <param name="offset">A positive or negative value that describes the amount by which the timeline should move forward or backward from the specified <paramref name="origin" />.</param>
		/// <param name="origin">The position from which <paramref name="offset" /> is applied.</param>
		// Token: 0x060016FC RID: 5884 RVA: 0x0007194B File Offset: 0x0006FB4B
		public void SeekAlignedToLastTick(FrameworkElement containingObject, TimeSpan offset, TimeSeekOrigin origin)
		{
			this.SeekAlignedToLastTickImpl(containingObject, offset, origin);
		}

		/// <summary>Seeks this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to a new position immediately (synchronously).</summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkContentElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <param name="offset">A positive or negative value that describes the amount by which the timeline should move forward or backward from the specified <paramref name="origin" />.</param>
		/// <param name="origin">The position from which <paramref name="offset" /> is applied.</param>
		// Token: 0x060016FD RID: 5885 RVA: 0x0007194B File Offset: 0x0006FB4B
		public void SeekAlignedToLastTick(FrameworkContentElement containingObject, TimeSpan offset, TimeSeekOrigin origin)
		{
			this.SeekAlignedToLastTickImpl(containingObject, offset, origin);
		}

		/// <summary>Seeks this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to a new position immediately (synchronously).</summary>
		/// <param name="offset">A positive or negative value that describes the amount by which the timeline should move forward or backward from the specified <paramref name="origin" />.</param>
		/// <param name="origin">The position from which <paramref name="offset" /> is applied.</param>
		// Token: 0x060016FE RID: 5886 RVA: 0x00071956 File Offset: 0x0006FB56
		public void SeekAlignedToLastTick(TimeSpan offset, TimeSeekOrigin origin)
		{
			this.SeekAlignedToLastTickImpl(this, offset, origin);
		}

		/// <summary>Seeks this <see cref="T:System.Windows.Media.Animation.Storyboard" /> to a new position immediately (synchronously).</summary>
		/// <param name="offset">A positive or negative value that describes the amount by which the timeline should move forward or backward.</param>
		// Token: 0x060016FF RID: 5887 RVA: 0x00071961 File Offset: 0x0006FB61
		public void SeekAlignedToLastTick(TimeSpan offset)
		{
			this.SeekAlignedToLastTickImpl(this, offset, TimeSeekOrigin.BeginTime);
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x0007196C File Offset: 0x0006FB6C
		private void SeekAlignedToLastTickImpl(DependencyObject containingObject, TimeSpan offset, TimeSeekOrigin origin)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject, false, Storyboard.InteractiveOperation.SeekAlignedToLastTick);
			if (storyboardClock != null)
			{
				storyboardClock.Controller.SeekAlignedToLastTick(offset, origin);
			}
		}

		/// <summary>Sets the interactive speed ratio of the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />. </summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <param name="speedRatio">A finite value greater than zero that is the new interactive speed ratio of the storyboard. This value is multiplied against the storyboard's <see cref="P:System.Windows.Media.Animation.Timeline.SpeedRatio" /> value to determine the storyboard's effective speed. This value does not overwrite the storyboard's <see cref="P:System.Windows.Media.Animation.Timeline.SpeedRatio" /> property. For example, calling this method and specifying an interactive speed ratio of 3 on a storyboard with a <see cref="P:System.Windows.Media.Animation.Timeline.SpeedRatio" /> of 0.5 gives the storyboard an effective speed of 1.5. </param>
		// Token: 0x06001701 RID: 5889 RVA: 0x00071993 File Offset: 0x0006FB93
		public void SetSpeedRatio(FrameworkElement containingObject, double speedRatio)
		{
			this.SetSpeedRatioImpl(containingObject, speedRatio);
		}

		/// <summary>Sets the interactive speed ratio of the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkContentElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		/// <param name="speedRatio">A finite value greater than zero that is the new interactive speed ratio of the storyboard. This value is multiplied against the storyboard's <see cref="P:System.Windows.Media.Animation.Timeline.SpeedRatio" /> value to determine the storyboard's effective speed. This value does not overwrite the storyboard's <see cref="P:System.Windows.Media.Animation.Timeline.SpeedRatio" /> property. For example, calling this method and specifying an interactive speed ratio of 3 on a storyboard with a <see cref="P:System.Windows.Media.Animation.Timeline.SpeedRatio" /> of 0.5 gives the storyboard an effective speed of 1.5.</param>
		// Token: 0x06001702 RID: 5890 RVA: 0x00071993 File Offset: 0x0006FB93
		public void SetSpeedRatio(FrameworkContentElement containingObject, double speedRatio)
		{
			this.SetSpeedRatioImpl(containingObject, speedRatio);
		}

		/// <summary>Sets the interactive speed ratio for the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
		/// <param name="speedRatio">A finite value greater than zero that is the new interactive speed ratio of the storyboard. This value is multiplied against the storyboard's <see cref="P:System.Windows.Media.Animation.Timeline.SpeedRatio" /> value to determine the storyboard's effective speed. This value does not overwrite the storyboard's <see cref="P:System.Windows.Media.Animation.Timeline.SpeedRatio" /> property. For example, calling this method and specifying an interactive speed ratio of 3 on a storyboard with a <see cref="P:System.Windows.Media.Animation.Timeline.SpeedRatio" /> of 0.5 gives the storyboard an effective speed of 1.5.</param>
		// Token: 0x06001703 RID: 5891 RVA: 0x0007199D File Offset: 0x0006FB9D
		public void SetSpeedRatio(double speedRatio)
		{
			this.SetSpeedRatioImpl(this, speedRatio);
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x000719A8 File Offset: 0x0006FBA8
		private void SetSpeedRatioImpl(DependencyObject containingObject, double speedRatio)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject, false, Storyboard.InteractiveOperation.SetSpeedRatio);
			if (storyboardClock != null)
			{
				storyboardClock.Controller.SpeedRatio = speedRatio;
			}
		}

		/// <summary>Advances the current time of this storyboard's <see cref="T:System.Windows.Media.Animation.Clock" /> to the end of its active period. </summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		// Token: 0x06001705 RID: 5893 RVA: 0x000719CE File Offset: 0x0006FBCE
		public void SkipToFill(FrameworkElement containingObject)
		{
			this.SkipToFillImpl(containingObject);
		}

		/// <summary>Advances the current time of this storyboard's <see cref="T:System.Windows.Media.Animation.Clock" /> to the end of its active period.</summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkContentElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		// Token: 0x06001706 RID: 5894 RVA: 0x000719CE File Offset: 0x0006FBCE
		public void SkipToFill(FrameworkContentElement containingObject)
		{
			this.SkipToFillImpl(containingObject);
		}

		/// <summary>Advances the current time of this storyboard's <see cref="T:System.Windows.Media.Animation.Clock" /> to the end of its active period.</summary>
		// Token: 0x06001707 RID: 5895 RVA: 0x000719D7 File Offset: 0x0006FBD7
		public void SkipToFill()
		{
			this.SkipToFillImpl(this);
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x000719E0 File Offset: 0x0006FBE0
		private void SkipToFillImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject, false, Storyboard.InteractiveOperation.SkipToFill);
			if (storyboardClock != null)
			{
				storyboardClock.Controller.SkipToFill();
			}
		}

		/// <summary>Stops the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />. </summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		// Token: 0x06001709 RID: 5897 RVA: 0x00071A05 File Offset: 0x0006FC05
		public void Stop(FrameworkElement containingObject)
		{
			this.StopImpl(containingObject);
		}

		/// <summary>Stops the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />. </summary>
		/// <param name="containingObject">The object specified when the <see cref="M:System.Windows.Media.Animation.Storyboard.Begin(System.Windows.FrameworkContentElement,System.Boolean)" /> method was called. This object contains the <see cref="T:System.Windows.Media.Animation.Clock" /> objects that were created for this storyboard and its children.</param>
		// Token: 0x0600170A RID: 5898 RVA: 0x00071A05 File Offset: 0x0006FC05
		public void Stop(FrameworkContentElement containingObject)
		{
			this.StopImpl(containingObject);
		}

		/// <summary>Stops the <see cref="T:System.Windows.Media.Animation.Clock" /> that was created for this <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
		// Token: 0x0600170B RID: 5899 RVA: 0x00071A0E File Offset: 0x0006FC0E
		public void Stop()
		{
			this.StopImpl(this);
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x00071A18 File Offset: 0x0006FC18
		private void StopImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject, false, Storyboard.InteractiveOperation.Stop);
			if (storyboardClock != null)
			{
				storyboardClock.Controller.Stop();
			}
			if (TraceAnimation.IsEnabled)
			{
				TraceAnimation.TraceActivityItem(TraceAnimation.StoryboardStop, this, base.Name, containingObject);
			}
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x00071A56 File Offset: 0x0006FC56
		private Clock GetStoryboardClock(DependencyObject o)
		{
			return this.GetStoryboardClock(o, true, Storyboard.InteractiveOperation.Unknown);
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x00071A64 File Offset: 0x0006FC64
		private Clock GetStoryboardClock(DependencyObject o, bool throwIfNull, Storyboard.InteractiveOperation operation)
		{
			Clock result = null;
			WeakReference weakReference = null;
			HybridDictionary value = Storyboard.StoryboardClockTreesField.GetValue(o);
			if (value != null)
			{
				weakReference = (value[this] as WeakReference);
			}
			if (weakReference == null)
			{
				if (throwIfNull)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_NeverApplied"));
				}
				if (TraceAnimation.IsEnabledOverride)
				{
					TraceAnimation.Trace(TraceEventType.Warning, TraceAnimation.StoryboardNotApplied, operation, this, o);
				}
			}
			if (weakReference != null)
			{
				result = (weakReference.Target as Clock);
			}
			return result;
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x00071AD4 File Offset: 0x0006FCD4
		private void SetStoryboardClock(DependencyObject o, Clock clock)
		{
			HybridDictionary hybridDictionary = Storyboard.StoryboardClockTreesField.GetValue(o);
			if (hybridDictionary == null)
			{
				hybridDictionary = new HybridDictionary();
				Storyboard.StoryboardClockTreesField.SetValue(o, hybridDictionary);
			}
			hybridDictionary[this] = new WeakReference(clock);
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x00071B10 File Offset: 0x0006FD10
		private static Storyboard.CloneCacheEntry GetComplexPathClone(DependencyObject o, DependencyProperty dp)
		{
			FrugalMap value = Storyboard.ComplexPathCloneField.GetValue(o);
			object obj = value[dp.GlobalIndex];
			if (obj != DependencyProperty.UnsetValue)
			{
				return (Storyboard.CloneCacheEntry)value[dp.GlobalIndex];
			}
			return null;
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x00071B54 File Offset: 0x0006FD54
		private static void SetComplexPathClone(DependencyObject o, DependencyProperty dp, object source, object clone)
		{
			FrugalMap value = Storyboard.ComplexPathCloneField.GetValue(o);
			if (clone != DependencyProperty.UnsetValue)
			{
				value[dp.GlobalIndex] = new Storyboard.CloneCacheEntry(source, clone);
			}
			else
			{
				value[dp.GlobalIndex] = DependencyProperty.UnsetValue;
			}
			Storyboard.ComplexPathCloneField.SetValue(o, value);
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Media.Animation.Storyboard.Target" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Media.Animation.Storyboard.Target" /> attached property.</returns>
		// Token: 0x040012AA RID: 4778
		public static readonly DependencyProperty TargetProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Media.Animation.Storyboard.TargetName" /> attached property.  </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Media.Animation.Storyboard.TargetName" /> attached property.</returns>
		// Token: 0x040012AB RID: 4779
		public static readonly DependencyProperty TargetNameProperty = DependencyProperty.RegisterAttached("TargetName", typeof(string), typeof(Storyboard));

		/// <summary>Identifies the <see cref="P:System.Windows.Media.Animation.Storyboard.TargetProperty" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Media.Animation.Storyboard.TargetProperty" /> attached property.</returns>
		// Token: 0x040012AC RID: 4780
		public static readonly DependencyProperty TargetPropertyProperty = DependencyProperty.RegisterAttached("TargetProperty", typeof(PropertyPath), typeof(Storyboard));

		// Token: 0x040012AD RID: 4781
		private static readonly UncommonField<HybridDictionary> StoryboardClockTreesField = new UncommonField<HybridDictionary>();

		// Token: 0x040012AE RID: 4782
		private static readonly UncommonField<FrugalMap> ComplexPathCloneField = new UncommonField<FrugalMap>();

		// Token: 0x02000859 RID: 2137
		private class ObjectPropertyPair
		{
			// Token: 0x060082B9 RID: 33465 RVA: 0x00243A1E File Offset: 0x00241C1E
			public ObjectPropertyPair(DependencyObject o, DependencyProperty p)
			{
				this._object = o;
				this._property = p;
			}

			// Token: 0x060082BA RID: 33466 RVA: 0x00243A34 File Offset: 0x00241C34
			public override int GetHashCode()
			{
				return this._object.GetHashCode() ^ this._property.GetHashCode();
			}

			// Token: 0x060082BB RID: 33467 RVA: 0x00243A4D File Offset: 0x00241C4D
			public override bool Equals(object o)
			{
				return o != null && o is Storyboard.ObjectPropertyPair && this.Equals((Storyboard.ObjectPropertyPair)o);
			}

			// Token: 0x060082BC RID: 33468 RVA: 0x00243A68 File Offset: 0x00241C68
			public bool Equals(Storyboard.ObjectPropertyPair key)
			{
				return this._object.Equals(key._object) && this._property == key._property;
			}

			// Token: 0x17001D97 RID: 7575
			// (get) Token: 0x060082BD RID: 33469 RVA: 0x00243A8D File Offset: 0x00241C8D
			public DependencyObject DependencyObject
			{
				get
				{
					return this._object;
				}
			}

			// Token: 0x17001D98 RID: 7576
			// (get) Token: 0x060082BE RID: 33470 RVA: 0x00243A95 File Offset: 0x00241C95
			public DependencyProperty DependencyProperty
			{
				get
				{
					return this._property;
				}
			}

			// Token: 0x04004077 RID: 16503
			private DependencyObject _object;

			// Token: 0x04004078 RID: 16504
			private DependencyProperty _property;
		}

		// Token: 0x0200085A RID: 2138
		private class CloneCacheEntry
		{
			// Token: 0x060082BF RID: 33471 RVA: 0x00243A9D File Offset: 0x00241C9D
			internal CloneCacheEntry(object source, object clone)
			{
				this.Source = source;
				this.Clone = clone;
			}

			// Token: 0x04004079 RID: 16505
			internal object Source;

			// Token: 0x0400407A RID: 16506
			internal object Clone;
		}

		// Token: 0x0200085B RID: 2139
		internal class ChangeListener
		{
			// Token: 0x060082C0 RID: 33472 RVA: 0x00243AB3 File Offset: 0x00241CB3
			internal ChangeListener(DependencyObject target, Freezable clone, DependencyProperty property, Freezable original)
			{
				this._target = target;
				this._property = property;
				this._clone = clone;
				this._original = original;
			}

			// Token: 0x060082C1 RID: 33473 RVA: 0x00243AD8 File Offset: 0x00241CD8
			internal void InvalidatePropertyOnCloneChange(object source, EventArgs e)
			{
				Storyboard.CloneCacheEntry complexPathClone = Storyboard.GetComplexPathClone(this._target, this._property);
				if (complexPathClone != null && complexPathClone.Clone == this._clone)
				{
					this._target.InvalidateSubProperty(this._property);
					return;
				}
				this.Cleanup();
			}

			// Token: 0x060082C2 RID: 33474 RVA: 0x00243B20 File Offset: 0x00241D20
			internal void InvalidatePropertyOnOriginalChange(object source, EventArgs e)
			{
				this._target.InvalidateProperty(this._property);
				this.Cleanup();
			}

			// Token: 0x060082C3 RID: 33475 RVA: 0x00243B3C File Offset: 0x00241D3C
			internal static void ListenToChangesOnFreezable(DependencyObject target, Freezable clone, DependencyProperty dp, Freezable original)
			{
				Storyboard.ChangeListener changeListener = new Storyboard.ChangeListener(target, clone, dp, original);
				changeListener.Setup();
			}

			// Token: 0x060082C4 RID: 33476 RVA: 0x00243B5C File Offset: 0x00241D5C
			private void Setup()
			{
				EventHandler value = new EventHandler(this.InvalidatePropertyOnCloneChange);
				this._clone.Changed += value;
				if (this._original.IsFrozen)
				{
					this._original = null;
					return;
				}
				value = new EventHandler(this.InvalidatePropertyOnOriginalChange);
				this._original.Changed += value;
			}

			// Token: 0x060082C5 RID: 33477 RVA: 0x00243BB0 File Offset: 0x00241DB0
			private void Cleanup()
			{
				EventHandler value = new EventHandler(this.InvalidatePropertyOnCloneChange);
				this._clone.Changed -= value;
				if (this._original != null)
				{
					value = new EventHandler(this.InvalidatePropertyOnOriginalChange);
					this._original.Changed -= value;
				}
				this._target = null;
				this._property = null;
				this._clone = null;
				this._original = null;
			}

			// Token: 0x0400407B RID: 16507
			private DependencyObject _target;

			// Token: 0x0400407C RID: 16508
			private DependencyProperty _property;

			// Token: 0x0400407D RID: 16509
			private Freezable _clone;

			// Token: 0x0400407E RID: 16510
			private Freezable _original;
		}

		// Token: 0x0200085C RID: 2140
		internal static class Layers
		{
			// Token: 0x0400407F RID: 16511
			internal static long ElementEventTrigger = 1L;

			// Token: 0x04004080 RID: 16512
			internal static long StyleOrTemplateEventTrigger = 1L;

			// Token: 0x04004081 RID: 16513
			internal static long Code = 1L;

			// Token: 0x04004082 RID: 16514
			internal static long PropertyTriggerStartLayer = 2L;
		}

		// Token: 0x0200085D RID: 2141
		private enum InteractiveOperation : ushort
		{
			// Token: 0x04004084 RID: 16516
			Unknown,
			// Token: 0x04004085 RID: 16517
			Pause,
			// Token: 0x04004086 RID: 16518
			Remove,
			// Token: 0x04004087 RID: 16519
			Resume,
			// Token: 0x04004088 RID: 16520
			Seek,
			// Token: 0x04004089 RID: 16521
			SeekAlignedToLastTick,
			// Token: 0x0400408A RID: 16522
			SetSpeedRatio,
			// Token: 0x0400408B RID: 16523
			SkipToFill,
			// Token: 0x0400408C RID: 16524
			Stop
		}
	}
}
