using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Media;
using System.Windows.Shapes;
using MS.Internal;
using MS.Utility;

namespace System.Windows.Documents
{
	// Token: 0x02000368 RID: 872
	internal sealed class FixedTextBuilder
	{
		// Token: 0x06002E4D RID: 11853 RVA: 0x000D1384 File Offset: 0x000CF584
		internal static bool AlwaysAdjacent(CultureInfo ci)
		{
			foreach (CultureInfo obj in FixedTextBuilder.AdjacentLanguage)
			{
				if (ci.Equals(obj))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x000D13B8 File Offset: 0x000CF5B8
		internal static bool IsHyphen(char target)
		{
			foreach (char c in FixedTextBuilder.HyphenSet)
			{
				if (c == target)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x000D13E4 File Offset: 0x000CF5E4
		internal static bool IsSpace(char target)
		{
			return target == ' ';
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x000D13EB File Offset: 0x000CF5EB
		internal FixedTextBuilder(FixedTextContainer container)
		{
			this._container = container;
			this._Init();
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x000D1400 File Offset: 0x000CF600
		internal void AddVirtualPage()
		{
			FixedPageStructure fixedPageStructure = new FixedPageStructure(this._pageStructures.Count);
			this._pageStructures.Add(fixedPageStructure);
			this._fixedFlowMap.FlowOrderInsertBefore(this._fixedFlowMap.FlowEndEdge, fixedPageStructure.FlowStart);
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x000D1448 File Offset: 0x000CF648
		internal bool EnsureTextOMForPage(int pageIndex)
		{
			FixedPageStructure fixedPageStructure = this._pageStructures[pageIndex];
			if (!fixedPageStructure.Loaded)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXEnsureOMBegin);
				try
				{
					FixedPage fixedPage = this._container.FixedDocument.SyncGetPage(pageIndex, false);
					if (fixedPage == null)
					{
						return false;
					}
					Size size = this._container.FixedDocument.ComputePageSize(fixedPage);
					fixedPage.Measure(size);
					fixedPage.Arrange(new Rect(new Point(0.0, 0.0), size));
					bool flag = true;
					StoryFragments pageStructure = fixedPage.GetPageStructure();
					if (pageStructure != null)
					{
						flag = false;
						FixedDSBuilder fixedDSBuilder = new FixedDSBuilder(fixedPage, pageStructure);
						fixedPageStructure.FixedDSBuilder = fixedDSBuilder;
					}
					if (flag)
					{
						FixedSOMPageConstructor fixedSOMPageConstructor = new FixedSOMPageConstructor(fixedPage, pageIndex);
						fixedPageStructure.PageConstructor = fixedSOMPageConstructor;
						fixedPageStructure.FixedSOMPage = fixedSOMPageConstructor.FixedSOMPage;
					}
					this._CreateFixedMappingAndElementForPage(fixedPageStructure, fixedPage, flag);
				}
				finally
				{
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXEnsureOMEnd);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002E53 RID: 11859 RVA: 0x000D1544 File Offset: 0x000CF744
		internal FixedPage GetFixedPage(FixedNode node)
		{
			FixedDocument fixedDocument = this._container.FixedDocument;
			return fixedDocument.SyncGetPageWithCheck(node.Page);
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x000D156C File Offset: 0x000CF76C
		internal Glyphs GetGlyphsElement(FixedNode node)
		{
			FixedPage fixedPage = this.GetFixedPage(node);
			if (fixedPage != null)
			{
				return fixedPage.GetGlyphsElement(node);
			}
			return null;
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x000D1590 File Offset: 0x000CF790
		internal FixedNode[] GetNextLine(FixedNode currentNode, bool forward, ref int count)
		{
			if (this._IsBoundaryPage(currentNode.Page))
			{
				return null;
			}
			this.EnsureTextOMForPage(currentNode.Page);
			FixedPageStructure fixedPageStructure = this._pageStructures[currentNode.Page];
			if (this._IsStartVisual(currentNode[1]))
			{
				FixedNode[] firstLine = fixedPageStructure.FirstLine;
				if (firstLine == null)
				{
					return null;
				}
				currentNode = firstLine[0];
				count--;
			}
			else if (this._IsEndVisual(currentNode[1]))
			{
				FixedNode[] lastLine = fixedPageStructure.LastLine;
				if (lastLine == null)
				{
					return null;
				}
				currentNode = lastLine[0];
				count--;
			}
			FixedSOMTextRun fixedSOMTextRun = this._fixedFlowMap.MappingGetFixedSOMElement(currentNode, 0) as FixedSOMTextRun;
			if (fixedSOMTextRun == null)
			{
				return null;
			}
			int lineIndex = fixedSOMTextRun.LineIndex;
			return fixedPageStructure.GetNextLine(lineIndex, forward, ref count);
		}

		// Token: 0x06002E56 RID: 11862 RVA: 0x000D1652 File Offset: 0x000CF852
		internal FixedNode[] GetLine(int pageIndex, Point pt)
		{
			this.EnsureTextOMForPage(pageIndex);
			return this._pageStructures[pageIndex].FindSnapToLine(pt);
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x000D166E File Offset: 0x000CF86E
		internal FixedNode[] GetFirstLine(int pageIndex)
		{
			this.EnsureTextOMForPage(pageIndex);
			return this._pageStructures[pageIndex].FirstLine;
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x000D168C File Offset: 0x000CF88C
		internal FlowPosition CreateFlowPosition(FixedPosition fixedPosition)
		{
			this.EnsureTextOMForPage(fixedPosition.Page);
			FixedSOMElement fixedSOMElement = this._fixedFlowMap.MappingGetFixedSOMElement(fixedPosition.Node, fixedPosition.Offset);
			if (fixedSOMElement != null)
			{
				FlowNode flowNode = fixedSOMElement.FlowNode;
				int num = fixedPosition.Offset;
				FixedSOMTextRun fixedSOMTextRun = fixedSOMElement as FixedSOMTextRun;
				if (fixedSOMTextRun != null && fixedSOMTextRun.IsReversed)
				{
					num = fixedSOMTextRun.EndIndex - fixedSOMTextRun.StartIndex - num;
				}
				int offset = fixedSOMElement.OffsetInFlowNode + num - fixedSOMElement.StartIndex;
				return new FlowPosition(this._container, flowNode, offset);
			}
			return null;
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x000D1718 File Offset: 0x000CF918
		internal FlowPosition GetPageStartFlowPosition(int pageIndex)
		{
			this.EnsureTextOMForPage(pageIndex);
			FlowNode flowStart = this._pageStructures[pageIndex].FlowStart;
			return new FlowPosition(this._container, flowStart, 0);
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x000D174C File Offset: 0x000CF94C
		internal FlowPosition GetPageEndFlowPosition(int pageIndex)
		{
			this.EnsureTextOMForPage(pageIndex);
			FlowNode flowEnd = this._pageStructures[pageIndex].FlowEnd;
			return new FlowPosition(this._container, flowEnd, 1);
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x000D1780 File Offset: 0x000CF980
		internal bool GetFixedPosition(FlowPosition position, LogicalDirection textdir, out FixedPosition fixedp)
		{
			fixedp = new FixedPosition(this.FixedFlowMap.FixedStartEdge, 0);
			FlowNode flowNode;
			int num;
			position.GetFlowNode(textdir, out flowNode, out num);
			FixedSOMElement[] fixedSOMElements = flowNode.FixedSOMElements;
			if (fixedSOMElements == null)
			{
				return false;
			}
			int num2 = 0;
			int i = fixedSOMElements.Length - 1;
			while (i > num2)
			{
				int num3 = i + num2 + 1 >> 1;
				if (fixedSOMElements[num3].OffsetInFlowNode > num)
				{
					i = num3 - 1;
				}
				else
				{
					num2 = num3;
				}
			}
			FixedSOMElement fixedSOMElement = fixedSOMElements[num2];
			FixedSOMTextRun fixedSOMTextRun;
			if (num2 > 0 && textdir == LogicalDirection.Backward && fixedSOMElement.OffsetInFlowNode == num)
			{
				FixedSOMElement fixedSOMElement2 = fixedSOMElements[num2 - 1];
				int num4 = num - fixedSOMElement2.OffsetInFlowNode + fixedSOMElement2.StartIndex;
				if (num4 == fixedSOMElement2.EndIndex)
				{
					fixedSOMTextRun = (fixedSOMElement2 as FixedSOMTextRun);
					if (fixedSOMTextRun != null && fixedSOMTextRun.IsReversed)
					{
						num4 = fixedSOMTextRun.EndIndex - fixedSOMTextRun.StartIndex - num4;
					}
					fixedp = new FixedPosition(fixedSOMElement2.FixedNode, num4);
					return true;
				}
			}
			fixedSOMTextRun = (fixedSOMElement as FixedSOMTextRun);
			int num5 = num - fixedSOMElement.OffsetInFlowNode + fixedSOMElement.StartIndex;
			if (fixedSOMTextRun != null && fixedSOMTextRun.IsReversed)
			{
				num5 = fixedSOMTextRun.EndIndex - fixedSOMTextRun.StartIndex - num5;
			}
			fixedp = new FixedPosition(fixedSOMElement.FixedNode, num5);
			return true;
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x000D18BC File Offset: 0x000CFABC
		internal bool GetFixedNodesForFlowRange(FlowPosition pStart, FlowPosition pEnd, out FixedSOMElement[] somElements, out int firstElementStart, out int lastElementEnd)
		{
			somElements = null;
			firstElementStart = 0;
			lastElementEnd = 0;
			int num = 0;
			int num2 = -1;
			FlowNode[] array;
			int num3;
			int num4;
			pStart.GetFlowNodes(pEnd, out array, out num3, out num4);
			if (array.Length == 0)
			{
				return false;
			}
			ArrayList arrayList = new ArrayList();
			FlowNode flowNode = array[0];
			FlowNode flowNode2 = array[array.Length - 1];
			foreach (FlowNode flowNode3 in array)
			{
				int num5 = 0;
				int num6 = int.MaxValue;
				if (flowNode3 == flowNode)
				{
					num5 = num3;
				}
				if (flowNode3 == flowNode2)
				{
					num6 = num4;
				}
				if (flowNode3.Type == FlowNodeType.Object)
				{
					FixedSOMElement[] fixedSOMElements = flowNode3.FixedSOMElements;
					arrayList.Add(fixedSOMElements[0]);
				}
				if (flowNode3.Type == FlowNodeType.Run)
				{
					FixedSOMElement[] fixedSOMElements2 = flowNode3.FixedSOMElements;
					foreach (FixedSOMElement fixedSOMElement in fixedSOMElements2)
					{
						int offsetInFlowNode = fixedSOMElement.OffsetInFlowNode;
						if (offsetInFlowNode >= num6)
						{
							break;
						}
						int num7 = offsetInFlowNode + fixedSOMElement.EndIndex - fixedSOMElement.StartIndex;
						if (num7 > num5)
						{
							arrayList.Add(fixedSOMElement);
							if (num5 >= offsetInFlowNode && flowNode3 == flowNode)
							{
								num = fixedSOMElement.StartIndex + num5 - offsetInFlowNode;
							}
							if (num6 <= num7 && flowNode3 == flowNode2)
							{
								num2 = fixedSOMElement.StartIndex + num6 - offsetInFlowNode;
								break;
							}
							if (num6 == num7 + 1)
							{
								num2 = fixedSOMElement.EndIndex;
							}
						}
					}
				}
			}
			somElements = (FixedSOMElement[])arrayList.ToArray(typeof(FixedSOMElement));
			if (somElements.Length == 0)
			{
				return false;
			}
			if (flowNode.Type == FlowNodeType.Object)
			{
				firstElementStart = num3;
			}
			else
			{
				firstElementStart = num;
			}
			if (flowNode2.Type == FlowNodeType.Object)
			{
				lastElementEnd = num4;
			}
			else
			{
				lastElementEnd = num2;
			}
			return true;
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x000D1A5C File Offset: 0x000CFC5C
		internal string GetFlowText(FlowNode flowNode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			FixedSOMElement[] fixedSOMElements = flowNode.FixedSOMElements;
			foreach (FixedSOMTextRun fixedSOMTextRun in fixedSOMElements)
			{
				stringBuilder.Append(fixedSOMTextRun.Text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x000D1AA4 File Offset: 0x000CFCA4
		internal static bool MostlyRTL(string s)
		{
			int num = 0;
			int num2 = 0;
			foreach (char c in s)
			{
				if (FixedTextBuilder._IsRTL(c))
				{
					num++;
				}
				else if (c != ' ')
				{
					num2++;
				}
			}
			return num > 0 && (num2 == 0 || num / num2 >= 2);
		}

		// Token: 0x06002E5F RID: 11871 RVA: 0x000D1B00 File Offset: 0x000CFD00
		internal static bool IsSameLine(double verticalDistance, double fontSize1, double fontSize2)
		{
			double num = (fontSize1 < fontSize2) ? fontSize1 : fontSize2;
			double num2 = (verticalDistance > 0.0) ? (fontSize1 - verticalDistance) : (fontSize2 + verticalDistance);
			return num2 / num > 0.5;
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x000D1B3C File Offset: 0x000CFD3C
		internal static bool IsNonContiguous(CultureInfo ciPrev, CultureInfo ciCurrent, bool isSidewaysPrev, bool isSidewaysCurrent, string strPrev, string strCurrent, FixedTextBuilder.GlyphComparison comparison)
		{
			if (ciPrev != ciCurrent)
			{
				return true;
			}
			if (FixedTextBuilder.AlwaysAdjacent(ciPrev))
			{
				return false;
			}
			if (isSidewaysPrev != isSidewaysCurrent)
			{
				return true;
			}
			if (strPrev.Length == 0 || strCurrent.Length == 0)
			{
				return false;
			}
			if (!isSidewaysPrev)
			{
				int length = strPrev.Length;
				char target = strPrev[length - 1];
				if (FixedTextBuilder.IsSpace(target))
				{
					return false;
				}
				if (comparison != FixedTextBuilder.GlyphComparison.DifferentLine && comparison != FixedTextBuilder.GlyphComparison.Unknown)
				{
					return comparison != FixedTextBuilder.GlyphComparison.Adjacent;
				}
				if (!FixedTextBuilder.IsHyphen(target))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06002E61 RID: 11873 RVA: 0x000D1BB0 File Offset: 0x000CFDB0
		internal FixedFlowMap FixedFlowMap
		{
			get
			{
				return this._fixedFlowMap;
			}
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x000D1BB8 File Offset: 0x000CFDB8
		private void _Init()
		{
			this._nextScopeId = 0;
			this._fixedFlowMap = new FixedFlowMap();
			this._pageStructures = new List<FixedPageStructure>();
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x000D1BD8 File Offset: 0x000CFDD8
		private FixedNode _NewFixedNode(int pageIndex, int nestingLevel, int level1Index, int[] pathPrefix, int childIndex)
		{
			if (nestingLevel == 1)
			{
				return FixedNode.Create(pageIndex, nestingLevel, childIndex, -1, null);
			}
			if (nestingLevel == 2)
			{
				return FixedNode.Create(pageIndex, nestingLevel, level1Index, childIndex, null);
			}
			int[] array = new int[pathPrefix.Length + 1];
			pathPrefix.CopyTo(array, 0);
			array[array.Length - 1] = childIndex;
			return FixedNode.Create(pageIndex, nestingLevel, -1, -1, array);
		}

		// Token: 0x06002E64 RID: 11876 RVA: 0x000D1C30 File Offset: 0x000CFE30
		private bool _IsImage(object o)
		{
			Path path = o as Path;
			if (path != null)
			{
				return path.Fill is ImageBrush && path.Data != null;
			}
			return o is Image;
		}

		// Token: 0x06002E65 RID: 11877 RVA: 0x000D1C6C File Offset: 0x000CFE6C
		private bool _IsNonContiguous(FixedSOMTextRun prevRun, FixedSOMTextRun currentRun, FixedTextBuilder.GlyphComparison comparison)
		{
			if (prevRun.FixedNode == currentRun.FixedNode)
			{
				return currentRun.StartIndex != prevRun.EndIndex;
			}
			return FixedTextBuilder.IsNonContiguous(prevRun.CultureInfo, currentRun.CultureInfo, prevRun.IsSideways, currentRun.IsSideways, prevRun.Text, currentRun.Text, comparison);
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x000D1CC8 File Offset: 0x000CFEC8
		private FixedTextBuilder.GlyphComparison _CompareGlyphs(Glyphs glyph1, Glyphs glyph2)
		{
			FixedTextBuilder.GlyphComparison result = FixedTextBuilder.GlyphComparison.DifferentLine;
			if (glyph1 == glyph2)
			{
				result = FixedTextBuilder.GlyphComparison.SameLine;
			}
			else if (glyph1 != null && glyph2 != null)
			{
				GlyphRun glyphRun = glyph1.ToGlyphRun();
				GlyphRun glyphRun2 = glyph2.ToGlyphRun();
				if (glyphRun != null && glyphRun2 != null)
				{
					Rect rect = glyphRun.ComputeAlignmentBox();
					rect.Offset(glyph1.OriginX, glyph1.OriginY);
					Rect rect2 = glyphRun2.ComputeAlignmentBox();
					rect2.Offset(glyph2.OriginX, glyph2.OriginY);
					bool flag = (glyph1.BidiLevel & 1) == 0;
					bool flag2 = (glyph2.BidiLevel & 1) == 0;
					GeneralTransform generalTransform = glyph2.TransformToVisual(glyph1);
					Point point = flag ? rect.TopRight : rect.TopLeft;
					Point inPoint = flag2 ? rect2.TopLeft : rect2.TopRight;
					if (generalTransform != null)
					{
						generalTransform.TryTransform(inPoint, out inPoint);
					}
					if (FixedTextBuilder.IsSameLine(inPoint.Y - point.Y, rect.Height, rect2.Height))
					{
						result = FixedTextBuilder.GlyphComparison.SameLine;
						if (flag == flag2)
						{
							double num = Math.Abs(inPoint.X - point.X);
							double num2 = Math.Max(rect.Height, rect2.Height);
							if (num / num2 < 0.05)
							{
								result = FixedTextBuilder.GlyphComparison.Adjacent;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x000D1E08 File Offset: 0x000D0008
		private void _CreateFixedMappingAndElementForPage(FixedPageStructure pageStructure, FixedPage page, bool constructSOM)
		{
			List<FixedNode> list = new List<FixedNode>();
			this._GetFixedNodes(pageStructure, page.Children, 1, -1, null, constructSOM, list, Matrix.Identity);
			FixedTextBuilder.FlowModelBuilder flowModelBuilder = new FixedTextBuilder.FlowModelBuilder(this, pageStructure, page);
			flowModelBuilder.FindHyperlinkPaths(page);
			if (constructSOM)
			{
				pageStructure.FixedSOMPage.MarkupOrder = list;
				pageStructure.ConstructFixedSOMPage(list);
				this._CreateFlowNodes(pageStructure.FixedSOMPage, flowModelBuilder);
				pageStructure.PageConstructor = null;
			}
			else
			{
				pageStructure.FixedDSBuilder.ConstructFlowNodes(flowModelBuilder, list);
			}
			flowModelBuilder.FinishMapping();
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x000D1E84 File Offset: 0x000D0084
		private void _GetFixedNodes(FixedPageStructure pageStructure, IEnumerable oneLevel, int nestingLevel, int level1Index, int[] pathPrefix, bool constructLines, List<FixedNode> fixedNodes, Matrix transform)
		{
			int pageIndex = pageStructure.PageIndex;
			int num = this._NewScopeId();
			int num2 = 0;
			IEnumerator enumerator = oneLevel.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!constructLines)
				{
					IFrameworkInputElement frameworkInputElement = enumerator.Current as IFrameworkInputElement;
					if (frameworkInputElement != null && frameworkInputElement.Name != null && frameworkInputElement.Name.Length != 0)
					{
						pageStructure.FixedDSBuilder.BuildNameHashTable(frameworkInputElement.Name, enumerator.Current as UIElement, fixedNodes.Count);
					}
				}
				if (this._IsImage(enumerator.Current) || (enumerator.Current is Glyphs && (enumerator.Current as Glyphs).MeasurementGlyphRun != null))
				{
					fixedNodes.Add(this._NewFixedNode(pageIndex, nestingLevel, level1Index, pathPrefix, num2));
				}
				else if (constructLines && enumerator.Current is Path)
				{
					pageStructure.PageConstructor.ProcessPath(enumerator.Current as Path, transform);
				}
				else if (enumerator.Current is Canvas)
				{
					Transform transform2 = Transform.Identity;
					Canvas canvas = enumerator.Current as Canvas;
					IEnumerable children = canvas.Children;
					transform2 = canvas.RenderTransform;
					if (transform2 == null)
					{
						transform2 = Transform.Identity;
					}
					if (children != null)
					{
						int[] array = null;
						if (nestingLevel >= 2)
						{
							if (nestingLevel == 2)
							{
								array = new int[2];
								array[0] = level1Index;
							}
							else
							{
								array = new int[pathPrefix.Length + 1];
								pathPrefix.CopyTo(array, 0);
							}
							array[array.Length - 1] = num2;
						}
						Matrix transform3 = FrameworkAppContextSwitches.OptOutOfFixedDocumentModelConstructionFix ? (transform * transform2.Value) : (transform2.Value * transform);
						this._GetFixedNodes(pageStructure, children, nestingLevel + 1, (nestingLevel == 1) ? num2 : -1, array, constructLines, fixedNodes, transform3);
					}
				}
				num2++;
			}
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x000D204C File Offset: 0x000D024C
		private void _CreateFlowNodes(FixedSOMPage somPage, FixedTextBuilder.FlowModelBuilder flowBuilder)
		{
			flowBuilder.AddStartNode(FixedElement.ElementType.Section);
			somPage.SetRTFProperties(flowBuilder.FixedElement);
			foreach (FixedSOMSemanticBox fixedSOMSemanticBox in somPage.SemanticBoxes)
			{
				FixedSOMContainer node = (FixedSOMContainer)fixedSOMSemanticBox;
				this._CreateFlowNodes(node, flowBuilder);
			}
			flowBuilder.AddLeftoverHyperlinks();
			flowBuilder.AddEndNode();
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x000D20C8 File Offset: 0x000D02C8
		private void _CreateFlowNodes(FixedSOMContainer node, FixedTextBuilder.FlowModelBuilder flowBuilder)
		{
			FixedElement.ElementType[] elementTypes = node.ElementTypes;
			foreach (FixedElement.ElementType type in elementTypes)
			{
				flowBuilder.AddStartNode(type);
				node.SetRTFProperties(flowBuilder.FixedElement);
			}
			List<FixedSOMSemanticBox> semanticBoxes = node.SemanticBoxes;
			foreach (FixedSOMSemanticBox fixedSOMSemanticBox in semanticBoxes)
			{
				if (fixedSOMSemanticBox is FixedSOMElement)
				{
					flowBuilder.AddElement((FixedSOMElement)fixedSOMSemanticBox);
				}
				else if (fixedSOMSemanticBox is FixedSOMContainer)
				{
					this._CreateFlowNodes((FixedSOMContainer)fixedSOMSemanticBox, flowBuilder);
				}
			}
			foreach (FixedElement.ElementType elementType in elementTypes)
			{
				flowBuilder.AddEndNode();
			}
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x000D219C File Offset: 0x000D039C
		private bool _IsStartVisual(int visualIndex)
		{
			return visualIndex == int.MinValue;
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x000D21A6 File Offset: 0x000D03A6
		private bool _IsEndVisual(int visualIndex)
		{
			return visualIndex == int.MaxValue;
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x000D21B0 File Offset: 0x000D03B0
		private bool _IsBoundaryPage(int pageIndex)
		{
			return pageIndex == int.MinValue || pageIndex == int.MaxValue;
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x000D21C4 File Offset: 0x000D03C4
		private int _NewScopeId()
		{
			int nextScopeId = this._nextScopeId;
			this._nextScopeId = nextScopeId + 1;
			return nextScopeId;
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x000D21E4 File Offset: 0x000D03E4
		private static bool _IsRTL(char c)
		{
			return (c >= 'א' && c <= '؋') || c == '؍' || (c >= '؛' && c <= 'ي') || (c >= '٭' && c <= 'ە' && c != 'ٰ') || c == '۝' || c == 'ۥ' || c == 'ۦ' || c == 'ۮ' || c == 'ۯ' || (c >= 'ۺ' && c <= '܍') || c == 'ܐ' || (c >= 'ܒ' && c <= 'ܯ') || (c >= 'ݍ' && c <= 'ޥ') || c == 'ޱ' || c == 'יִ' || (c >= 'ײַ' && c <= 'ﴽ' && c != '﬩') || (c >= 'ﵐ' && c <= '﷼') || (c >= 'ﹰ' && c <= 'ﻼ');
		}

		// Token: 0x04001E02 RID: 7682
		internal const char FLOWORDER_SEPARATOR = ' ';

		// Token: 0x04001E03 RID: 7683
		internal static CultureInfo[] AdjacentLanguage = new CultureInfo[]
		{
			new CultureInfo("zh-HANS"),
			new CultureInfo("zh-HANT"),
			new CultureInfo("zh-HK"),
			new CultureInfo("zh-MO"),
			new CultureInfo("zh-CN"),
			new CultureInfo("zh-SG"),
			new CultureInfo("zh-TW"),
			new CultureInfo("ja-JP"),
			new CultureInfo("ko-KR"),
			new CultureInfo("th-TH")
		};

		// Token: 0x04001E04 RID: 7684
		internal static char[] HyphenSet = new char[]
		{
			'-',
			'‐',
			'‑',
			'‒',
			'–',
			'−',
			'­'
		};

		// Token: 0x04001E05 RID: 7685
		private readonly FixedTextContainer _container;

		// Token: 0x04001E06 RID: 7686
		private List<FixedPageStructure> _pageStructures;

		// Token: 0x04001E07 RID: 7687
		private int _nextScopeId;

		// Token: 0x04001E08 RID: 7688
		private FixedFlowMap _fixedFlowMap;

		// Token: 0x04001E09 RID: 7689
		private static bool[] _cTable = new bool[]
		{
			true,
			false,
			true,
			true,
			false,
			true,
			true,
			false,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			false,
			false,
			true,
			true,
			true,
			true,
			true,
			true,
			false,
			false,
			true,
			true,
			false,
			true,
			true,
			true,
			true
		};

		// Token: 0x020008D3 RID: 2259
		internal sealed class FlowModelBuilder
		{
			// Token: 0x0600848A RID: 33930 RVA: 0x002486E8 File Offset: 0x002468E8
			public FlowModelBuilder(FixedTextBuilder builder, FixedPageStructure pageStructure, FixedPage page)
			{
				this._builder = builder;
				this._container = builder._container;
				this._pageIndex = pageStructure.PageIndex;
				this._textRuns = new List<FixedSOMTextRun>();
				this._flowNodes = new List<FlowNode>();
				this._fixedNodes = new List<FixedNode>();
				this._nodesInLine = new List<FixedNode>();
				this._lineResults = new List<FixedLineResult>();
				this._endNodes = new Stack();
				this._fixedElements = new Stack();
				this._mapping = builder._fixedFlowMap;
				this._pageStructure = pageStructure;
				this._currentFixedElement = this._container.ContainerElement;
				this._lineLayoutBox = Rect.Empty;
				this._logicalHyperlinkContainer = new FixedTextBuilder.FlowModelBuilder.LogicalHyperlinkContainer();
				this._fixedPage = page;
			}

			// Token: 0x0600848B RID: 33931 RVA: 0x002487A8 File Offset: 0x002469A8
			public void FindHyperlinkPaths(FrameworkElement elem)
			{
				IEnumerable children = LogicalTreeHelper.GetChildren(elem);
				foreach (object obj in children)
				{
					UIElement uielement = (UIElement)obj;
					Canvas canvas = uielement as Canvas;
					if (canvas != null)
					{
						this.FindHyperlinkPaths(canvas);
					}
					if (uielement is Path && !(((Path)uielement).Fill is ImageBrush))
					{
						Uri navigateUri = FixedPage.GetNavigateUri(uielement);
						if (navigateUri != null && ((Path)uielement).Data != null)
						{
							Transform transform = uielement.TransformToAncestor(this._fixedPage) as Transform;
							Geometry geometry = ((Path)uielement).Data;
							if (transform != null && !transform.Value.IsIdentity)
							{
								geometry = PathGeometry.CreateFromGeometry(geometry);
								geometry.Transform = transform;
							}
							this._logicalHyperlinkContainer.AddLogicalHyperlink(navigateUri, geometry, uielement);
						}
					}
				}
			}

			// Token: 0x0600848C RID: 33932 RVA: 0x002488AC File Offset: 0x00246AAC
			public void AddLeftoverHyperlinks()
			{
				foreach (FixedTextBuilder.FlowModelBuilder.LogicalHyperlink logicalHyperlink in ((IEnumerable<FixedTextBuilder.FlowModelBuilder.LogicalHyperlink>)this._logicalHyperlinkContainer))
				{
					if (!logicalHyperlink.Used)
					{
						this._AddStartNode(FixedElement.ElementType.Paragraph);
						this._AddStartNode(FixedElement.ElementType.Hyperlink);
						this._currentFixedElement.SetValue(Hyperlink.NavigateUriProperty, logicalHyperlink.Uri);
						this._currentFixedElement.SetValue(FixedElement.HelpTextProperty, (string)logicalHyperlink.UIElement.GetValue(AutomationProperties.HelpTextProperty));
						this._currentFixedElement.SetValue(FixedElement.NameProperty, (string)logicalHyperlink.UIElement.GetValue(AutomationProperties.NameProperty));
						this._AddEndNode();
						this._AddEndNode();
					}
				}
			}

			// Token: 0x0600848D RID: 33933 RVA: 0x0024897C File Offset: 0x00246B7C
			public void AddStartNode(FixedElement.ElementType type)
			{
				this._FinishTextRun(true);
				this._FinishHyperlink();
				this._AddStartNode(type);
			}

			// Token: 0x0600848E RID: 33934 RVA: 0x00248992 File Offset: 0x00246B92
			public void AddEndNode()
			{
				this._FinishTextRun(false);
				this._FinishHyperlink();
				this._AddEndNode();
			}

			// Token: 0x0600848F RID: 33935 RVA: 0x002489A8 File Offset: 0x00246BA8
			public void AddElement(FixedSOMElement element)
			{
				FixedPage fixedPage = this._builder.GetFixedPage(element.FixedNode);
				UIElement shadowHyperlink;
				Uri uri = this._logicalHyperlinkContainer.GetUri(element, fixedPage, out shadowHyperlink);
				if (element is FixedSOMTextRun)
				{
					FixedSOMTextRun fixedSOMTextRun = element as FixedSOMTextRun;
					bool flag = this._currentRun == null || !fixedSOMTextRun.HasSameRichProperties(this._currentRun) || uri != this._currentNavUri || (uri != null && uri.ToString() != this._currentNavUri.ToString());
					if (flag)
					{
						if (this._currentRun != null)
						{
							FixedSOMFixedBlock fixedBlock = fixedSOMTextRun.FixedBlock;
							FixedSOMTextRun fixedSOMTextRun2 = this._textRuns[this._textRuns.Count - 1];
							Glyphs glyphsElement = this._builder.GetGlyphsElement(fixedSOMTextRun2.FixedNode);
							Glyphs glyphsElement2 = this._builder.GetGlyphsElement(fixedSOMTextRun.FixedNode);
							FixedTextBuilder.GlyphComparison comparison = this._builder._CompareGlyphs(glyphsElement, glyphsElement2);
							bool addSpace = false;
							if (this._builder._IsNonContiguous(fixedSOMTextRun2, fixedSOMTextRun, comparison))
							{
								addSpace = true;
							}
							this._FinishTextRun(addSpace);
						}
						this._SetHyperlink(uri, fixedSOMTextRun.FixedNode, shadowHyperlink);
						this._AddStartNode(FixedElement.ElementType.Run);
						fixedSOMTextRun.SetRTFProperties(this._currentFixedElement);
						this._currentRun = fixedSOMTextRun;
					}
					this._textRuns.Add((FixedSOMTextRun)element);
					if (this._fixedNodes.Count == 0 || this._fixedNodes[this._fixedNodes.Count - 1] != element.FixedNode)
					{
						this._fixedNodes.Add(element.FixedNode);
						return;
					}
				}
				else if (element is FixedSOMImage)
				{
					FixedSOMImage fixedSOMImage = (FixedSOMImage)element;
					this._FinishTextRun(true);
					this._SetHyperlink(uri, fixedSOMImage.FixedNode, shadowHyperlink);
					this._AddStartNode(FixedElement.ElementType.InlineUIContainer);
					FlowNode flowNode = new FlowNode(this._NewScopeId(), FlowNodeType.Object, null);
					this._container.OnNewFlowElement(this._currentFixedElement, FixedElement.ElementType.Object, new FlowPosition(this._container, flowNode, 0), new FlowPosition(this._container, flowNode, 1), fixedSOMImage.Source, this._pageIndex);
					this._flowNodes.Add(flowNode);
					element.FlowNode = flowNode;
					flowNode.FixedSOMElements = new FixedSOMElement[]
					{
						element
					};
					this._mapping.AddFixedElement(element);
					this._fixedNodes.Add(element.FixedNode);
					FixedElement fixedElement = (FixedElement)flowNode.Cookie;
					fixedElement.SetValue(FixedElement.NameProperty, fixedSOMImage.Name);
					fixedElement.SetValue(FixedElement.HelpTextProperty, fixedSOMImage.HelpText);
					this._AddEndNode();
				}
			}

			// Token: 0x06008490 RID: 33936 RVA: 0x00248C38 File Offset: 0x00246E38
			public void FinishMapping()
			{
				this._FinishLine();
				this._mapping.MappingReplace(this._pageStructure.FlowStart, this._flowNodes);
				this._pageStructure.SetFlowBoundary(this._flowNodes[0], this._flowNodes[this._flowNodes.Count - 1]);
				this._pageStructure.SetupLineResults(this._lineResults.ToArray());
			}

			// Token: 0x06008491 RID: 33937 RVA: 0x00248CAC File Offset: 0x00246EAC
			private void _AddStartNode(FixedElement.ElementType type)
			{
				FlowNode flowNode = new FlowNode(this._NewScopeId(), FlowNodeType.Start, this._pageIndex);
				FlowNode flowNode2 = new FlowNode(this._NewScopeId(), FlowNodeType.End, this._pageIndex);
				this._container.OnNewFlowElement(this._currentFixedElement, type, new FlowPosition(this._container, flowNode, 1), new FlowPosition(this._container, flowNode2, 0), null, this._pageIndex);
				this._fixedElements.Push(this._currentFixedElement);
				this._currentFixedElement = (FixedElement)flowNode.Cookie;
				this._flowNodes.Add(flowNode);
				this._endNodes.Push(flowNode2);
			}

			// Token: 0x06008492 RID: 33938 RVA: 0x00248D56 File Offset: 0x00246F56
			private void _AddEndNode()
			{
				this._flowNodes.Add((FlowNode)this._endNodes.Pop());
				this._currentFixedElement = (FixedElement)this._fixedElements.Pop();
			}

			// Token: 0x06008493 RID: 33939 RVA: 0x00248D8C File Offset: 0x00246F8C
			private void _FinishTextRun(bool addSpace)
			{
				if (this._textRuns.Count > 0)
				{
					int num = 0;
					FixedSOMTextRun fixedSOMTextRun = null;
					for (int i = 0; i < this._textRuns.Count; i++)
					{
						fixedSOMTextRun = this._textRuns[i];
						Glyphs glyphsElement = this._builder.GetGlyphsElement(fixedSOMTextRun.FixedNode);
						FixedTextBuilder.GlyphComparison glyphComparison = this._builder._CompareGlyphs(this._lastGlyphs, glyphsElement);
						if (glyphComparison == FixedTextBuilder.GlyphComparison.DifferentLine)
						{
							this._FinishLine();
						}
						this._lastGlyphs = glyphsElement;
						this._lineLayoutBox.Union(fixedSOMTextRun.BoundingRect);
						fixedSOMTextRun.LineIndex = this._lineResults.Count;
						if (this._nodesInLine.Count == 0 || this._nodesInLine[this._nodesInLine.Count - 1] != fixedSOMTextRun.FixedNode)
						{
							this._nodesInLine.Add(fixedSOMTextRun.FixedNode);
						}
						num += fixedSOMTextRun.EndIndex - fixedSOMTextRun.StartIndex;
						if (i > 0 && this._builder._IsNonContiguous(this._textRuns[i - 1], fixedSOMTextRun, glyphComparison))
						{
							this._textRuns[i - 1].Text = this._textRuns[i - 1].Text + " ";
							num++;
						}
					}
					if (addSpace && fixedSOMTextRun.Text.Length > 0 && !fixedSOMTextRun.Text.EndsWith(" ", StringComparison.Ordinal) && !FixedTextBuilder.IsHyphen(fixedSOMTextRun.Text[fixedSOMTextRun.Text.Length - 1]))
					{
						fixedSOMTextRun.Text += " ";
						num++;
					}
					if (num != 0)
					{
						FlowNode flowNode = new FlowNode(this._NewScopeId(), FlowNodeType.Run, num);
						flowNode.FixedSOMElements = this._textRuns.ToArray();
						int num2 = 0;
						foreach (FixedSOMTextRun fixedSOMTextRun2 in this._textRuns)
						{
							fixedSOMTextRun2.FlowNode = flowNode;
							fixedSOMTextRun2.OffsetInFlowNode = num2;
							this._mapping.AddFixedElement(fixedSOMTextRun2);
							num2 += fixedSOMTextRun2.Text.Length;
						}
						this._flowNodes.Add(flowNode);
						this._textRuns.Clear();
					}
				}
				if (this._currentRun != null)
				{
					this._AddEndNode();
					this._currentRun = null;
				}
			}

			// Token: 0x06008494 RID: 33940 RVA: 0x00249004 File Offset: 0x00247204
			private void _FinishHyperlink()
			{
				if (this._currentNavUri != null)
				{
					this._AddEndNode();
					this._currentNavUri = null;
				}
			}

			// Token: 0x06008495 RID: 33941 RVA: 0x00249024 File Offset: 0x00247224
			private void _SetHyperlink(Uri navUri, FixedNode node, UIElement shadowHyperlink)
			{
				if (navUri != this._currentNavUri || (navUri != null && navUri.ToString() != this._currentNavUri.ToString()))
				{
					if (this._currentNavUri != null)
					{
						this._AddEndNode();
					}
					if (navUri != null)
					{
						this._AddStartNode(FixedElement.ElementType.Hyperlink);
						this._currentFixedElement.SetValue(Hyperlink.NavigateUriProperty, navUri);
						UIElement uielement = this._fixedPage.GetElement(node) as UIElement;
						if (uielement != null)
						{
							this._currentFixedElement.SetValue(FixedElement.HelpTextProperty, (string)uielement.GetValue(AutomationProperties.HelpTextProperty));
							this._currentFixedElement.SetValue(FixedElement.NameProperty, (string)uielement.GetValue(AutomationProperties.NameProperty));
							if (shadowHyperlink != null)
							{
								this._logicalHyperlinkContainer.MarkAsUsed(shadowHyperlink);
							}
						}
					}
					this._currentNavUri = navUri;
				}
			}

			// Token: 0x06008496 RID: 33942 RVA: 0x00249108 File Offset: 0x00247308
			private void _FinishLine()
			{
				if (this._nodesInLine.Count > 0)
				{
					FixedLineResult item = new FixedLineResult(this._nodesInLine.ToArray(), this._lineLayoutBox);
					this._lineResults.Add(item);
					this._nodesInLine.Clear();
					this._lineLayoutBox = Rect.Empty;
				}
			}

			// Token: 0x06008497 RID: 33943 RVA: 0x0024915C File Offset: 0x0024735C
			private int _NewScopeId()
			{
				FixedTextBuilder builder = this._builder;
				int nextScopeId = builder._nextScopeId;
				builder._nextScopeId = nextScopeId + 1;
				return nextScopeId;
			}

			// Token: 0x17001E04 RID: 7684
			// (get) Token: 0x06008498 RID: 33944 RVA: 0x0024917F File Offset: 0x0024737F
			public FixedElement FixedElement
			{
				get
				{
					return this._currentFixedElement;
				}
			}

			// Token: 0x0400425D RID: 16989
			private int _pageIndex;

			// Token: 0x0400425E RID: 16990
			private FixedTextContainer _container;

			// Token: 0x0400425F RID: 16991
			private FixedTextBuilder _builder;

			// Token: 0x04004260 RID: 16992
			private List<FixedSOMTextRun> _textRuns;

			// Token: 0x04004261 RID: 16993
			private List<FlowNode> _flowNodes;

			// Token: 0x04004262 RID: 16994
			private List<FixedNode> _fixedNodes;

			// Token: 0x04004263 RID: 16995
			private List<FixedNode> _nodesInLine;

			// Token: 0x04004264 RID: 16996
			private List<FixedLineResult> _lineResults;

			// Token: 0x04004265 RID: 16997
			private Rect _lineLayoutBox;

			// Token: 0x04004266 RID: 16998
			private Stack _endNodes;

			// Token: 0x04004267 RID: 16999
			private Stack _fixedElements;

			// Token: 0x04004268 RID: 17000
			private FixedElement _currentFixedElement;

			// Token: 0x04004269 RID: 17001
			private FixedFlowMap _mapping;

			// Token: 0x0400426A RID: 17002
			private FixedPageStructure _pageStructure;

			// Token: 0x0400426B RID: 17003
			private Glyphs _lastGlyphs;

			// Token: 0x0400426C RID: 17004
			private FixedSOMTextRun _currentRun;

			// Token: 0x0400426D RID: 17005
			private FixedTextBuilder.FlowModelBuilder.LogicalHyperlinkContainer _logicalHyperlinkContainer;

			// Token: 0x0400426E RID: 17006
			private FixedPage _fixedPage;

			// Token: 0x0400426F RID: 17007
			private Uri _currentNavUri;

			// Token: 0x02000BA3 RID: 2979
			private sealed class LogicalHyperlink
			{
				// Token: 0x0600919F RID: 37279 RVA: 0x0025ED6F File Offset: 0x0025CF6F
				public LogicalHyperlink(Uri uri, Geometry geom, UIElement uiElement)
				{
					this._uiElement = uiElement;
					this._uri = uri;
					this._geometry = geom;
					this._boundingRect = geom.Bounds;
					this._used = false;
				}

				// Token: 0x17001FBD RID: 8125
				// (get) Token: 0x060091A0 RID: 37280 RVA: 0x0025ED9F File Offset: 0x0025CF9F
				public Uri Uri
				{
					get
					{
						return this._uri;
					}
				}

				// Token: 0x17001FBE RID: 8126
				// (get) Token: 0x060091A1 RID: 37281 RVA: 0x0025EDA7 File Offset: 0x0025CFA7
				public Geometry Geometry
				{
					get
					{
						return this._geometry;
					}
				}

				// Token: 0x17001FBF RID: 8127
				// (get) Token: 0x060091A2 RID: 37282 RVA: 0x0025EDAF File Offset: 0x0025CFAF
				public Rect BoundingRect
				{
					get
					{
						return this._boundingRect;
					}
				}

				// Token: 0x17001FC0 RID: 8128
				// (get) Token: 0x060091A3 RID: 37283 RVA: 0x0025EDB7 File Offset: 0x0025CFB7
				public UIElement UIElement
				{
					get
					{
						return this._uiElement;
					}
				}

				// Token: 0x17001FC1 RID: 8129
				// (get) Token: 0x060091A4 RID: 37284 RVA: 0x0025EDBF File Offset: 0x0025CFBF
				// (set) Token: 0x060091A5 RID: 37285 RVA: 0x0025EDC7 File Offset: 0x0025CFC7
				public bool Used
				{
					get
					{
						return this._used;
					}
					set
					{
						this._used = value;
					}
				}

				// Token: 0x04004EB7 RID: 20151
				private UIElement _uiElement;

				// Token: 0x04004EB8 RID: 20152
				private Uri _uri;

				// Token: 0x04004EB9 RID: 20153
				private Geometry _geometry;

				// Token: 0x04004EBA RID: 20154
				private Rect _boundingRect;

				// Token: 0x04004EBB RID: 20155
				private bool _used;
			}

			// Token: 0x02000BA4 RID: 2980
			private sealed class LogicalHyperlinkContainer : IEnumerable<FixedTextBuilder.FlowModelBuilder.LogicalHyperlink>, IEnumerable
			{
				// Token: 0x060091A6 RID: 37286 RVA: 0x0025EDD0 File Offset: 0x0025CFD0
				public LogicalHyperlinkContainer()
				{
					this._hyperlinks = new List<FixedTextBuilder.FlowModelBuilder.LogicalHyperlink>();
				}

				// Token: 0x060091A7 RID: 37287 RVA: 0x0025EDE3 File Offset: 0x0025CFE3
				IEnumerator<FixedTextBuilder.FlowModelBuilder.LogicalHyperlink> IEnumerable<FixedTextBuilder.FlowModelBuilder.LogicalHyperlink>.GetEnumerator()
				{
					return this._hyperlinks.GetEnumerator();
				}

				// Token: 0x060091A8 RID: 37288 RVA: 0x0025EDE3 File Offset: 0x0025CFE3
				IEnumerator IEnumerable.GetEnumerator()
				{
					return this._hyperlinks.GetEnumerator();
				}

				// Token: 0x060091A9 RID: 37289 RVA: 0x0025EDF8 File Offset: 0x0025CFF8
				public void AddLogicalHyperlink(Uri uri, Geometry geometry, UIElement uiElement)
				{
					FixedTextBuilder.FlowModelBuilder.LogicalHyperlink item = new FixedTextBuilder.FlowModelBuilder.LogicalHyperlink(uri, geometry, uiElement);
					this._hyperlinks.Add(item);
				}

				// Token: 0x060091AA RID: 37290 RVA: 0x0025EE1C File Offset: 0x0025D01C
				public Uri GetUri(FixedSOMElement element, FixedPage p, out UIElement shadowElement)
				{
					shadowElement = null;
					UIElement uielement = p.GetElement(element.FixedNode) as UIElement;
					if (uielement == null)
					{
						return null;
					}
					Uri uri = FixedPage.GetNavigateUri(uielement);
					if (uri == null && this._hyperlinks.Count > 0)
					{
						Transform t = uielement.TransformToAncestor(p) as Transform;
						Geometry geom;
						if (uielement is Glyphs)
						{
							GlyphRun glyphRun = ((Glyphs)uielement).ToGlyphRun();
							Rect rect = glyphRun.ComputeAlignmentBox();
							rect.Offset(glyphRun.BaselineOrigin.X, glyphRun.BaselineOrigin.Y);
							geom = new RectangleGeometry(rect);
						}
						else if (uielement is Path)
						{
							geom = ((Path)uielement).Data;
						}
						else
						{
							Image image = (Image)uielement;
							geom = new RectangleGeometry(new Rect(0.0, 0.0, image.Width, image.Height));
						}
						FixedTextBuilder.FlowModelBuilder.LogicalHyperlink logicalHyperlink = this._GetHyperlinkFromGeometry(geom, t);
						if (logicalHyperlink != null)
						{
							uri = logicalHyperlink.Uri;
							shadowElement = logicalHyperlink.UIElement;
						}
					}
					if (uri == null)
					{
						return null;
					}
					return FixedPage.GetLinkUri(p, uri);
				}

				// Token: 0x060091AB RID: 37291 RVA: 0x0025EF40 File Offset: 0x0025D140
				public void MarkAsUsed(UIElement uiElement)
				{
					for (int i = 0; i < this._hyperlinks.Count; i++)
					{
						FixedTextBuilder.FlowModelBuilder.LogicalHyperlink logicalHyperlink = this._hyperlinks[i];
						if (logicalHyperlink.UIElement == uiElement)
						{
							logicalHyperlink.Used = true;
							return;
						}
					}
				}

				// Token: 0x060091AC RID: 37292 RVA: 0x0025EF84 File Offset: 0x0025D184
				private FixedTextBuilder.FlowModelBuilder.LogicalHyperlink _GetHyperlinkFromGeometry(Geometry geom, Transform t)
				{
					Geometry geometry = geom;
					if (t != null && !t.Value.IsIdentity)
					{
						geometry = PathGeometry.CreateFromGeometry(geom);
						geometry.Transform = t;
					}
					double num = geometry.GetArea() * 0.99;
					Rect bounds = geometry.Bounds;
					for (int i = 0; i < this._hyperlinks.Count; i++)
					{
						if (bounds.IntersectsWith(this._hyperlinks[i].BoundingRect))
						{
							Geometry geometry2 = Geometry.Combine(geometry, this._hyperlinks[i].Geometry, GeometryCombineMode.Intersect, Transform.Identity);
							if (geometry2.GetArea() > num)
							{
								return this._hyperlinks[i];
							}
						}
					}
					return null;
				}

				// Token: 0x04004EBC RID: 20156
				private List<FixedTextBuilder.FlowModelBuilder.LogicalHyperlink> _hyperlinks;
			}
		}

		// Token: 0x020008D4 RID: 2260
		internal enum GlyphComparison
		{
			// Token: 0x04004271 RID: 17009
			DifferentLine,
			// Token: 0x04004272 RID: 17010
			SameLine,
			// Token: 0x04004273 RID: 17011
			Adjacent,
			// Token: 0x04004274 RID: 17012
			Unknown
		}
	}
}
