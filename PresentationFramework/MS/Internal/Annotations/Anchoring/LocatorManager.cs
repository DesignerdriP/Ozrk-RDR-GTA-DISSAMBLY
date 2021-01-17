using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Annotations.Storage;
using System.Windows.Documents;
using System.Windows.Threading;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020007D5 RID: 2005
	internal sealed class LocatorManager : DispatcherObject
	{
		// Token: 0x06007BFC RID: 31740 RVA: 0x0022D8C9 File Offset: 0x0022BAC9
		public LocatorManager() : this(null)
		{
		}

		// Token: 0x06007BFD RID: 31741 RVA: 0x0022D8D4 File Offset: 0x0022BAD4
		public LocatorManager(AnnotationStore store)
		{
			this._locatorPartHandlers = new Hashtable();
			this._subtreeProcessors = new Hashtable();
			this._selectionProcessors = new Hashtable();
			this.RegisterSubTreeProcessor(new DataIdProcessor(this), "Id");
			this.RegisterSubTreeProcessor(new FixedPageProcessor(this), FixedPageProcessor.Id);
			TreeNodeSelectionProcessor processor = new TreeNodeSelectionProcessor();
			this.RegisterSelectionProcessor(processor, typeof(FrameworkElement));
			this.RegisterSelectionProcessor(processor, typeof(FrameworkContentElement));
			TextSelectionProcessor processor2 = new TextSelectionProcessor();
			this.RegisterSelectionProcessor(processor2, typeof(TextRange));
			this.RegisterSelectionProcessor(processor2, typeof(TextAnchor));
			this._internalStore = store;
		}

		// Token: 0x06007BFE RID: 31742 RVA: 0x0022D984 File Offset: 0x0022BB84
		public void RegisterSubTreeProcessor(SubTreeProcessor processor, string processorId)
		{
			base.VerifyAccess();
			if (processor == null)
			{
				throw new ArgumentNullException("processor");
			}
			if (processorId == null)
			{
				throw new ArgumentNullException("processorId");
			}
			XmlQualifiedName[] locatorPartTypes = processor.GetLocatorPartTypes();
			this._subtreeProcessors[processorId] = processor;
			if (locatorPartTypes != null)
			{
				foreach (XmlQualifiedName key in locatorPartTypes)
				{
					this._locatorPartHandlers[key] = processor;
				}
			}
		}

		// Token: 0x06007BFF RID: 31743 RVA: 0x0022D9EC File Offset: 0x0022BBEC
		public SubTreeProcessor GetSubTreeProcessor(DependencyObject node)
		{
			base.VerifyAccess();
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			string text = node.GetValue(LocatorManager.SubTreeProcessorIdProperty) as string;
			if (string.IsNullOrEmpty(text))
			{
				return this._subtreeProcessors["Id"] as SubTreeProcessor;
			}
			SubTreeProcessor subTreeProcessor = (SubTreeProcessor)this._subtreeProcessors[text];
			if (subTreeProcessor != null)
			{
				return subTreeProcessor;
			}
			throw new ArgumentException(SR.Get("InvalidSubTreeProcessor", new object[]
			{
				text
			}));
		}

		// Token: 0x06007C00 RID: 31744 RVA: 0x0022DA6C File Offset: 0x0022BC6C
		public SubTreeProcessor GetSubTreeProcessorForLocatorPart(ContentLocatorPart locatorPart)
		{
			base.VerifyAccess();
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			return this._locatorPartHandlers[locatorPart.PartType] as SubTreeProcessor;
		}

		// Token: 0x06007C01 RID: 31745 RVA: 0x0022DA98 File Offset: 0x0022BC98
		public void RegisterSelectionProcessor(SelectionProcessor processor, Type selectionType)
		{
			base.VerifyAccess();
			if (processor == null)
			{
				throw new ArgumentNullException("processor");
			}
			if (selectionType == null)
			{
				throw new ArgumentNullException("selectionType");
			}
			XmlQualifiedName[] locatorPartTypes = processor.GetLocatorPartTypes();
			this._selectionProcessors[selectionType] = processor;
			if (locatorPartTypes != null)
			{
				foreach (XmlQualifiedName key in locatorPartTypes)
				{
					this._locatorPartHandlers[key] = processor;
				}
			}
		}

		// Token: 0x06007C02 RID: 31746 RVA: 0x0022DB08 File Offset: 0x0022BD08
		public SelectionProcessor GetSelectionProcessor(Type selectionType)
		{
			base.VerifyAccess();
			if (selectionType == null)
			{
				throw new ArgumentNullException("selectionType");
			}
			SelectionProcessor selectionProcessor;
			do
			{
				selectionProcessor = (this._selectionProcessors[selectionType] as SelectionProcessor);
				selectionType = selectionType.BaseType;
			}
			while (selectionProcessor == null && selectionType != null);
			return selectionProcessor;
		}

		// Token: 0x06007C03 RID: 31747 RVA: 0x0022DB58 File Offset: 0x0022BD58
		public SelectionProcessor GetSelectionProcessorForLocatorPart(ContentLocatorPart locatorPart)
		{
			base.VerifyAccess();
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			return this._locatorPartHandlers[locatorPart.PartType] as SelectionProcessor;
		}

		// Token: 0x06007C04 RID: 31748 RVA: 0x0022DB84 File Offset: 0x0022BD84
		public IList<IAttachedAnnotation> ProcessAnnotations(DependencyObject node)
		{
			base.VerifyAccess();
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			IList<IAttachedAnnotation> list = new List<IAttachedAnnotation>();
			IList<ContentLocatorBase> list2 = this.GenerateLocators(node);
			if (list2.Count > 0)
			{
				AnnotationStore annotationStore;
				if (this._internalStore != null)
				{
					annotationStore = this._internalStore;
				}
				else
				{
					AnnotationService service = AnnotationService.GetService(node);
					if (service == null || !service.IsEnabled)
					{
						throw new InvalidOperationException(SR.Get("AnnotationServiceNotEnabled"));
					}
					annotationStore = service.Store;
				}
				ContentLocator[] array = new ContentLocator[list2.Count];
				list2.CopyTo(array, 0);
				IList<Annotation> annotations = annotationStore.GetAnnotations(array[0]);
				foreach (ContentLocatorBase contentLocatorBase in list2)
				{
					ContentLocator contentLocator = (ContentLocator)contentLocatorBase;
					if (contentLocator.Parts[contentLocator.Parts.Count - 1].NameValuePairs.ContainsKey("IncludeOverlaps"))
					{
						contentLocator.Parts.RemoveAt(contentLocator.Parts.Count - 1);
					}
				}
				foreach (Annotation annotation in annotations)
				{
					foreach (AnnotationResource annotationResource in annotation.Anchors)
					{
						foreach (ContentLocatorBase locator in annotationResource.ContentLocators)
						{
							AttachmentLevel attachmentLevel;
							object attachedAnchor = this.FindAttachedAnchor(node, array, locator, out attachmentLevel);
							if (attachmentLevel != AttachmentLevel.Unresolved)
							{
								list.Add(new AttachedAnnotation(this, annotation, annotationResource, attachedAnchor, attachmentLevel));
								break;
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06007C05 RID: 31749 RVA: 0x0022DD80 File Offset: 0x0022BF80
		public IList<ContentLocatorBase> GenerateLocators(object selection)
		{
			base.VerifyAccess();
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			SelectionProcessor selectionProcessor = this.GetSelectionProcessor(selection.GetType());
			if (selectionProcessor != null)
			{
				ICollection nodes = (ICollection)selectionProcessor.GetSelectedNodes(selection);
				IList<ContentLocatorBase> list = null;
				PathNode pathNode = PathNode.BuildPathForElements(nodes);
				if (pathNode != null)
				{
					SubTreeProcessor subTreeProcessor = this.GetSubTreeProcessor(pathNode.Node);
					list = this.GenerateLocators(subTreeProcessor, pathNode, selection);
				}
				if (list == null)
				{
					list = new List<ContentLocatorBase>(0);
				}
				return list;
			}
			throw new ArgumentException("Unsupported Selection", "selection");
		}

		// Token: 0x06007C06 RID: 31750 RVA: 0x0022DE04 File Offset: 0x0022C004
		public object ResolveLocator(ContentLocatorBase locator, int offset, DependencyObject startNode, out AttachmentLevel attachmentLevel)
		{
			base.VerifyAccess();
			if (locator == null)
			{
				throw new ArgumentNullException("locator");
			}
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			ContentLocator contentLocator = locator as ContentLocator;
			if (contentLocator != null && (offset < 0 || offset >= contentLocator.Parts.Count))
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			return this.InternalResolveLocator(locator, offset, startNode, false, out attachmentLevel);
		}

		// Token: 0x06007C07 RID: 31751 RVA: 0x0022DE66 File Offset: 0x0022C066
		public static void SetSubTreeProcessorId(DependencyObject d, string id)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			d.SetValue(LocatorManager.SubTreeProcessorIdProperty, id);
		}

		// Token: 0x06007C08 RID: 31752 RVA: 0x0022DE82 File Offset: 0x0022C082
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public static string GetSubTreeProcessorId(DependencyObject d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			return d.GetValue(LocatorManager.SubTreeProcessorIdProperty) as string;
		}

		// Token: 0x06007C09 RID: 31753 RVA: 0x0022DEA4 File Offset: 0x0022C0A4
		internal IList<IAttachedAnnotation> ProcessSubTree(DependencyObject subTree)
		{
			if (subTree == null)
			{
				throw new ArgumentNullException("subTree");
			}
			LocatorManager.ProcessingTreeState processingTreeState = new LocatorManager.ProcessingTreeState();
			PrePostDescendentsWalker<LocatorManager.ProcessingTreeState> prePostDescendentsWalker = new PrePostDescendentsWalker<LocatorManager.ProcessingTreeState>(TreeWalkPriority.VisualTree, new VisitedCallback<LocatorManager.ProcessingTreeState>(this.PreVisit), new VisitedCallback<LocatorManager.ProcessingTreeState>(this.PostVisit), processingTreeState);
			prePostDescendentsWalker.StartWalk(subTree);
			return processingTreeState.AttachedAnnotations;
		}

		// Token: 0x06007C0A RID: 31754 RVA: 0x0022DEF4 File Offset: 0x0022C0F4
		internal object FindAttachedAnchor(DependencyObject startNode, ContentLocator[] prefixes, ContentLocatorBase locator, out AttachmentLevel attachmentLevel)
		{
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			if (locator == null)
			{
				throw new ArgumentNullException("locator");
			}
			attachmentLevel = AttachmentLevel.Unresolved;
			object result = null;
			bool flag = true;
			int num = this.FindMatchingPrefix(prefixes, locator, out flag);
			if (flag)
			{
				ContentLocator contentLocator = locator as ContentLocator;
				if (contentLocator == null || num < contentLocator.Parts.Count)
				{
					result = this.InternalResolveLocator(locator, num, startNode, num != 0, out attachmentLevel);
				}
				if (attachmentLevel == AttachmentLevel.Unresolved && num > 0)
				{
					if (num == 0)
					{
						attachmentLevel = AttachmentLevel.Unresolved;
					}
					else if (contentLocator != null && num < contentLocator.Parts.Count)
					{
						attachmentLevel = AttachmentLevel.Incomplete;
						result = startNode;
					}
					else
					{
						attachmentLevel = AttachmentLevel.Full;
						result = startNode;
					}
				}
			}
			return result;
		}

		// Token: 0x06007C0B RID: 31755 RVA: 0x0022DF94 File Offset: 0x0022C194
		private int FindMatchingPrefix(ContentLocator[] prefixes, ContentLocatorBase locator, out bool matched)
		{
			matched = true;
			int result = 0;
			ContentLocator contentLocator = locator as ContentLocator;
			if (contentLocator != null && prefixes != null && prefixes.Length != 0)
			{
				matched = false;
				foreach (ContentLocator contentLocator2 in prefixes)
				{
					if (contentLocator.StartsWith(contentLocator2))
					{
						result = contentLocator2.Parts.Count;
						matched = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06007C0C RID: 31756 RVA: 0x0022DFEC File Offset: 0x0022C1EC
		private IList<ContentLocatorBase> GenerateLocators(SubTreeProcessor processor, PathNode startNode, object selection)
		{
			List<ContentLocatorBase> list = new List<ContentLocatorBase>();
			bool flag = true;
			ContentLocator contentLocator = processor.GenerateLocator(startNode, out flag);
			bool flag2 = contentLocator != null;
			if (flag)
			{
				int count = startNode.Children.Count;
				if (count != 0)
				{
					if (count != 1)
					{
						ContentLocatorBase contentLocatorBase = this.GenerateLocatorGroup(startNode, selection);
						if (contentLocatorBase != null)
						{
							flag2 = false;
						}
						if (contentLocator != null)
						{
							list.Add(contentLocator.Merge(contentLocatorBase));
						}
						else if (contentLocatorBase != null)
						{
							list.Add(contentLocatorBase);
						}
					}
					else
					{
						SubTreeProcessor subTreeProcessor = this.GetSubTreeProcessor(startNode.Node);
						IList<ContentLocatorBase> list2 = this.GenerateLocators(subTreeProcessor, (PathNode)startNode.Children[0], selection);
						if (list2 != null && list2.Count > 0)
						{
							flag2 = false;
						}
						if (contentLocator != null)
						{
							list.AddRange(this.Merge(contentLocator, list2));
						}
						else
						{
							list.AddRange(list2);
						}
					}
				}
				else if (contentLocator != null)
				{
					list.Add(contentLocator);
				}
			}
			else if (contentLocator != null)
			{
				list.Add(contentLocator);
			}
			if (flag2 && selection != null)
			{
				SelectionProcessor selectionProcessor = this.GetSelectionProcessor(selection.GetType());
				if (selectionProcessor != null)
				{
					IList<ContentLocatorPart> list3 = selectionProcessor.GenerateLocatorParts(selection, startNode.Node);
					if (list3 != null && list3.Count > 0)
					{
						List<ContentLocatorBase> list4 = new List<ContentLocatorBase>(list.Count * list3.Count);
						foreach (ContentLocatorBase contentLocatorBase2 in list)
						{
							list4.AddRange(((ContentLocator)contentLocatorBase2).DotProduct(list3));
						}
						list = list4;
					}
				}
			}
			return list;
		}

		// Token: 0x06007C0D RID: 31757 RVA: 0x0022E17C File Offset: 0x0022C37C
		private ContentLocatorBase GenerateLocatorGroup(PathNode node, object selection)
		{
			SubTreeProcessor subTreeProcessor = this.GetSubTreeProcessor(node.Node);
			ContentLocatorGroup contentLocatorGroup = new ContentLocatorGroup();
			foreach (object obj in node.Children)
			{
				PathNode startNode = (PathNode)obj;
				IList<ContentLocatorBase> list = this.GenerateLocators(subTreeProcessor, startNode, selection);
				if (list != null && list.Count > 0 && list[0] != null)
				{
					ContentLocator contentLocator = list[0] as ContentLocator;
					if (contentLocator != null && contentLocator.Parts.Count != 0)
					{
						contentLocatorGroup.Locators.Add(contentLocator);
					}
					else
					{
						ContentLocatorGroup contentLocatorGroup2 = list[0] as ContentLocatorGroup;
					}
				}
			}
			if (contentLocatorGroup.Locators.Count == 0)
			{
				return null;
			}
			if (contentLocatorGroup.Locators.Count == 1)
			{
				ContentLocator contentLocator2 = contentLocatorGroup.Locators[0];
				contentLocatorGroup.Locators.Remove(contentLocator2);
				return contentLocator2;
			}
			return contentLocatorGroup;
		}

		// Token: 0x06007C0E RID: 31758 RVA: 0x0022E288 File Offset: 0x0022C488
		private bool PreVisit(DependencyObject dependencyObject, LocatorManager.ProcessingTreeState data, bool visitedViaVisualTree)
		{
			bool flag = false;
			SubTreeProcessor subTreeProcessor = this.GetSubTreeProcessor(dependencyObject);
			IList<IAttachedAnnotation> list = subTreeProcessor.PreProcessNode(dependencyObject, out flag);
			if (list != null)
			{
				data.AttachedAnnotations.AddRange(list);
			}
			data.CalledProcessAnnotations = (data.CalledProcessAnnotations || flag);
			data.Push();
			return !flag;
		}

		// Token: 0x06007C0F RID: 31759 RVA: 0x0022E2D0 File Offset: 0x0022C4D0
		private bool PostVisit(DependencyObject dependencyObject, LocatorManager.ProcessingTreeState data, bool visitedViaVisualTree)
		{
			bool flag = data.Pop();
			SubTreeProcessor subTreeProcessor = this.GetSubTreeProcessor(dependencyObject);
			bool flag2 = false;
			IList<IAttachedAnnotation> list = subTreeProcessor.PostProcessNode(dependencyObject, flag, out flag2);
			if (list != null)
			{
				data.AttachedAnnotations.AddRange(list);
			}
			data.CalledProcessAnnotations = (data.CalledProcessAnnotations || flag2 || flag);
			return true;
		}

		// Token: 0x06007C10 RID: 31760 RVA: 0x0022E31C File Offset: 0x0022C51C
		private object InternalResolveLocator(ContentLocatorBase locator, int offset, DependencyObject startNode, bool skipStartNode, out AttachmentLevel attachmentLevel)
		{
			attachmentLevel = AttachmentLevel.Full;
			object result = null;
			ContentLocatorGroup contentLocatorGroup = locator as ContentLocatorGroup;
			ContentLocator contentLocator = locator as ContentLocator;
			AttachmentLevel attachmentLevel2 = AttachmentLevel.Unresolved;
			if (contentLocator != null && offset == contentLocator.Parts.Count - 1)
			{
				ContentLocatorPart locatorPart = contentLocator.Parts[offset];
				SelectionProcessor selectionProcessorForLocatorPart = this.GetSelectionProcessorForLocatorPart(locatorPart);
				if (selectionProcessorForLocatorPart != null)
				{
					result = selectionProcessorForLocatorPart.ResolveLocatorPart(locatorPart, startNode, out attachmentLevel2);
					attachmentLevel = attachmentLevel2;
					return result;
				}
			}
			IList<ContentLocator> list;
			if (contentLocatorGroup == null)
			{
				list = new List<ContentLocator>(1);
				list.Add(contentLocator);
			}
			else
			{
				AnnotationService service = AnnotationService.GetService(startNode);
				if (service != null)
				{
					startNode = service.Root;
				}
				list = contentLocatorGroup.Locators;
				offset = 0;
				skipStartNode = false;
			}
			bool flag = true;
			if (list.Count > 0)
			{
				LocatorManager.ResolvingLocatorState resolvingLocatorState = this.ResolveSingleLocator(ref result, ref attachmentLevel, AttachmentLevel.StartPortion, list[0], offset, startNode, skipStartNode);
				if (list.Count == 1)
				{
					result = resolvingLocatorState.AttachedAnchor;
					attachmentLevel = resolvingLocatorState.AttachmentLevel;
				}
				else
				{
					if (list.Count > 2)
					{
						AttachmentLevel attachmentLevel3 = AttachmentLevel.Unresolved;
						AttachmentLevel attachmentLevel4 = attachmentLevel;
						for (int i = 1; i < list.Count - 1; i++)
						{
							resolvingLocatorState = this.ResolveSingleLocator(ref result, ref attachmentLevel, AttachmentLevel.MiddlePortion, list[i], offset, startNode, skipStartNode);
							if (attachmentLevel3 == AttachmentLevel.Unresolved || (attachmentLevel & AttachmentLevel.MiddlePortion) != AttachmentLevel.Unresolved)
							{
								attachmentLevel3 = attachmentLevel;
							}
							attachmentLevel = attachmentLevel4;
						}
						attachmentLevel = attachmentLevel3;
					}
					else
					{
						flag = false;
					}
					resolvingLocatorState = this.ResolveSingleLocator(ref result, ref attachmentLevel, AttachmentLevel.EndPortion, list[list.Count - 1], offset, startNode, skipStartNode);
					if (!flag && attachmentLevel == AttachmentLevel.MiddlePortion)
					{
						attachmentLevel &= ~AttachmentLevel.MiddlePortion;
					}
					if (attachmentLevel == (AttachmentLevel.StartPortion | AttachmentLevel.EndPortion))
					{
						attachmentLevel = AttachmentLevel.Full;
					}
				}
			}
			else
			{
				attachmentLevel = AttachmentLevel.Unresolved;
			}
			return result;
		}

		// Token: 0x06007C11 RID: 31761 RVA: 0x0022E4B0 File Offset: 0x0022C6B0
		private LocatorManager.ResolvingLocatorState ResolveSingleLocator(ref object selection, ref AttachmentLevel attachmentLevel, AttachmentLevel attemptedLevel, ContentLocator locator, int offset, DependencyObject startNode, bool skipStartNode)
		{
			LocatorManager.ResolvingLocatorState resolvingLocatorState = new LocatorManager.ResolvingLocatorState();
			resolvingLocatorState.LocatorPartIndex = offset;
			resolvingLocatorState.ContentLocatorBase = locator;
			PrePostDescendentsWalker<LocatorManager.ResolvingLocatorState> prePostDescendentsWalker = new PrePostDescendentsWalker<LocatorManager.ResolvingLocatorState>(TreeWalkPriority.VisualTree, new VisitedCallback<LocatorManager.ResolvingLocatorState>(this.ResolveLocatorPart), new VisitedCallback<LocatorManager.ResolvingLocatorState>(this.TerminateResolve), resolvingLocatorState);
			prePostDescendentsWalker.StartWalk(startNode, skipStartNode);
			if (resolvingLocatorState.AttachmentLevel == AttachmentLevel.Full && resolvingLocatorState.AttachedAnchor != null)
			{
				if (selection != null)
				{
					SelectionProcessor selectionProcessor = this.GetSelectionProcessor(selection.GetType());
					if (selectionProcessor != null)
					{
						object obj;
						if (selectionProcessor.MergeSelections(selection, resolvingLocatorState.AttachedAnchor, out obj))
						{
							selection = obj;
						}
						else
						{
							attachmentLevel &= ~attemptedLevel;
						}
					}
					else
					{
						attachmentLevel &= ~attemptedLevel;
					}
				}
				else
				{
					selection = resolvingLocatorState.AttachedAnchor;
				}
			}
			else
			{
				attachmentLevel &= ~attemptedLevel;
			}
			return resolvingLocatorState;
		}

		// Token: 0x06007C12 RID: 31762 RVA: 0x0022E560 File Offset: 0x0022C760
		private bool ResolveLocatorPart(DependencyObject dependencyObject, LocatorManager.ResolvingLocatorState data, bool visitedViaVisualTree)
		{
			if (data.Finished)
			{
				return false;
			}
			ContentLocator contentLocatorBase = data.ContentLocatorBase;
			bool result = true;
			ContentLocatorPart contentLocatorPart = contentLocatorBase.Parts[data.LocatorPartIndex];
			if (contentLocatorPart == null)
			{
				result = false;
			}
			SubTreeProcessor subTreeProcessorForLocatorPart = this.GetSubTreeProcessorForLocatorPart(contentLocatorPart);
			if (subTreeProcessorForLocatorPart == null)
			{
				result = false;
			}
			if (contentLocatorPart != null && subTreeProcessorForLocatorPart != null)
			{
				DependencyObject dependencyObject2 = subTreeProcessorForLocatorPart.ResolveLocatorPart(contentLocatorPart, dependencyObject, out result);
				if (dependencyObject2 != null)
				{
					data.AttachmentLevel = AttachmentLevel.Incomplete;
					data.AttachedAnchor = dependencyObject2;
					result = true;
					data.LastNodeMatched = dependencyObject2;
					data.LocatorPartIndex++;
					if (data.LocatorPartIndex == contentLocatorBase.Parts.Count)
					{
						data.AttachmentLevel = AttachmentLevel.Full;
						data.AttachedAnchor = dependencyObject2;
						result = false;
					}
					else if (data.LocatorPartIndex == contentLocatorBase.Parts.Count - 1)
					{
						contentLocatorPart = contentLocatorBase.Parts[data.LocatorPartIndex];
						SelectionProcessor selectionProcessorForLocatorPart = this.GetSelectionProcessorForLocatorPart(contentLocatorPart);
						if (selectionProcessorForLocatorPart != null)
						{
							AttachmentLevel attachmentLevel;
							object obj = selectionProcessorForLocatorPart.ResolveLocatorPart(contentLocatorPart, dependencyObject2, out attachmentLevel);
							if (obj != null)
							{
								data.AttachmentLevel = attachmentLevel;
								data.AttachedAnchor = obj;
								result = false;
							}
							else
							{
								result = false;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06007C13 RID: 31763 RVA: 0x0022E677 File Offset: 0x0022C877
		private bool TerminateResolve(DependencyObject dependencyObject, LocatorManager.ResolvingLocatorState data, bool visitedViaVisualTree)
		{
			if (!data.Finished && data.LastNodeMatched == dependencyObject)
			{
				data.Finished = true;
			}
			return false;
		}

		// Token: 0x06007C14 RID: 31764 RVA: 0x0022E694 File Offset: 0x0022C894
		private IList<ContentLocatorBase> Merge(ContentLocatorBase initialLocator, IList<ContentLocatorBase> additionalLocators)
		{
			if (additionalLocators == null || additionalLocators.Count == 0)
			{
				return new List<ContentLocatorBase>(1)
				{
					initialLocator
				};
			}
			for (int i = 1; i < additionalLocators.Count; i++)
			{
				additionalLocators[i] = ((ContentLocatorBase)initialLocator.Clone()).Merge(additionalLocators[i]);
			}
			additionalLocators[0] = initialLocator.Merge(additionalLocators[0]);
			return additionalLocators;
		}

		// Token: 0x04003A4E RID: 14926
		public static readonly DependencyProperty SubTreeProcessorIdProperty = DependencyProperty.RegisterAttached("SubTreeProcessorId", typeof(string), typeof(LocatorManager), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior));

		// Token: 0x04003A4F RID: 14927
		private Hashtable _locatorPartHandlers;

		// Token: 0x04003A50 RID: 14928
		private Hashtable _subtreeProcessors;

		// Token: 0x04003A51 RID: 14929
		private Hashtable _selectionProcessors;

		// Token: 0x04003A52 RID: 14930
		private static readonly char[] Separators = new char[]
		{
			',',
			' ',
			';'
		};

		// Token: 0x04003A53 RID: 14931
		private AnnotationStore _internalStore;

		// Token: 0x02000B7F RID: 2943
		private class ProcessingTreeState
		{
			// Token: 0x06008E3F RID: 36415 RVA: 0x0025B6AB File Offset: 0x002598AB
			public ProcessingTreeState()
			{
				this._calledProcessAnnotations.Push(false);
			}

			// Token: 0x17001FA5 RID: 8101
			// (get) Token: 0x06008E40 RID: 36416 RVA: 0x0025B6D5 File Offset: 0x002598D5
			public List<IAttachedAnnotation> AttachedAnnotations
			{
				get
				{
					return this._attachedAnnotations;
				}
			}

			// Token: 0x17001FA6 RID: 8102
			// (get) Token: 0x06008E41 RID: 36417 RVA: 0x0025B6DD File Offset: 0x002598DD
			// (set) Token: 0x06008E42 RID: 36418 RVA: 0x0025B6EA File Offset: 0x002598EA
			public bool CalledProcessAnnotations
			{
				get
				{
					return this._calledProcessAnnotations.Peek();
				}
				set
				{
					if (this._calledProcessAnnotations.Peek() != value)
					{
						this._calledProcessAnnotations.Pop();
						this._calledProcessAnnotations.Push(value);
					}
				}
			}

			// Token: 0x06008E43 RID: 36419 RVA: 0x0025B712 File Offset: 0x00259912
			public void Push()
			{
				this._calledProcessAnnotations.Push(false);
			}

			// Token: 0x06008E44 RID: 36420 RVA: 0x0025B720 File Offset: 0x00259920
			public bool Pop()
			{
				return this._calledProcessAnnotations.Pop();
			}

			// Token: 0x04004B7B RID: 19323
			private List<IAttachedAnnotation> _attachedAnnotations = new List<IAttachedAnnotation>();

			// Token: 0x04004B7C RID: 19324
			private Stack<bool> _calledProcessAnnotations = new Stack<bool>();
		}

		// Token: 0x02000B80 RID: 2944
		private class ResolvingLocatorState
		{
			// Token: 0x04004B7D RID: 19325
			public ContentLocator ContentLocatorBase;

			// Token: 0x04004B7E RID: 19326
			public int LocatorPartIndex;

			// Token: 0x04004B7F RID: 19327
			public AttachmentLevel AttachmentLevel;

			// Token: 0x04004B80 RID: 19328
			public object AttachedAnchor;

			// Token: 0x04004B81 RID: 19329
			public bool Finished;

			// Token: 0x04004B82 RID: 19330
			public object LastNodeMatched;
		}
	}
}
