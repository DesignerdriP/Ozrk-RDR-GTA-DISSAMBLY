using System;
using System.Collections;
using System.Windows.Controls;

namespace System.Windows.Documents
{
	// Token: 0x02000371 RID: 881
	internal sealed class FlowPosition : IComparable
	{
		// Token: 0x06002F8D RID: 12173 RVA: 0x000D6319 File Offset: 0x000D4519
		internal FlowPosition(FixedTextContainer container, FlowNode node, int offset)
		{
			this._container = container;
			this._flowNode = node;
			this._offset = offset;
		}

		// Token: 0x06002F8E RID: 12174 RVA: 0x000D6336 File Offset: 0x000D4536
		public object Clone()
		{
			return new FlowPosition(this._container, this._flowNode, this._offset);
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x000D6350 File Offset: 0x000D4550
		public int CompareTo(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			FlowPosition flowPosition = o as FlowPosition;
			if (flowPosition == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					o.GetType(),
					typeof(FlowPosition)
				}), "o");
			}
			return this._OverlapAwareCompare(flowPosition);
		}

		// Token: 0x06002F90 RID: 12176 RVA: 0x000D63AD File Offset: 0x000D45AD
		public override int GetHashCode()
		{
			return this._flowNode.GetHashCode() ^ this._offset.GetHashCode();
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x000D63C8 File Offset: 0x000D45C8
		internal int GetDistance(FlowPosition flow)
		{
			if (this._flowNode.Equals(flow._flowNode))
			{
				return flow._offset - this._offset;
			}
			int num = this._OverlapAwareCompare(flow);
			FlowPosition flowPosition;
			FlowPosition flowPosition2;
			if (num == -1)
			{
				flowPosition = (FlowPosition)this.Clone();
				flowPosition2 = flow;
			}
			else
			{
				flowPosition = (FlowPosition)flow.Clone();
				flowPosition2 = this;
			}
			int num2 = 0;
			while (!flowPosition._IsSamePosition(flowPosition2))
			{
				if (flowPosition._flowNode.Equals(flowPosition2._flowNode))
				{
					num2 += flowPosition2._offset - flowPosition._offset;
					break;
				}
				int num3 = flowPosition._vScan(LogicalDirection.Forward, -1);
				num2 += num3;
			}
			return num * -1 * num2;
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x000D6466 File Offset: 0x000D4666
		internal TextPointerContext GetPointerContext(LogicalDirection dir)
		{
			return this._vGetSymbolType(dir);
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x000D6470 File Offset: 0x000D4670
		internal int GetTextRunLength(LogicalDirection dir)
		{
			FlowPosition clingPosition = this.GetClingPosition(dir);
			if (dir == LogicalDirection.Forward)
			{
				return clingPosition._NodeLength - clingPosition._offset;
			}
			return clingPosition._offset;
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x000D64A0 File Offset: 0x000D46A0
		internal int GetTextInRun(LogicalDirection dir, int maxLength, char[] chars, int startIndex)
		{
			FlowPosition clingPosition = this.GetClingPosition(dir);
			int nodeLength = clingPosition._NodeLength;
			int val;
			if (dir == LogicalDirection.Forward)
			{
				val = nodeLength - clingPosition._offset;
			}
			else
			{
				val = clingPosition._offset;
			}
			maxLength = Math.Min(maxLength, val);
			string flowText = this._container.FixedTextBuilder.GetFlowText(clingPosition._flowNode);
			if (dir == LogicalDirection.Forward)
			{
				Array.Copy(flowText.ToCharArray(clingPosition._offset, maxLength), 0, chars, startIndex, maxLength);
			}
			else
			{
				Array.Copy(flowText.ToCharArray(clingPosition._offset - maxLength, maxLength), 0, chars, startIndex, maxLength);
			}
			return maxLength;
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x000D652C File Offset: 0x000D472C
		internal object GetAdjacentElement(LogicalDirection dir)
		{
			FlowPosition clingPosition = this.GetClingPosition(dir);
			FlowNodeType type = clingPosition._flowNode.Type;
			if (type == FlowNodeType.Noop)
			{
				return string.Empty;
			}
			object @object = ((FixedElement)clingPosition._flowNode.Cookie).GetObject();
			Image image = @object as Image;
			if (type == FlowNodeType.Object && image != null)
			{
				FixedSOMElement[] fixedSOMElements = clingPosition._flowNode.FixedSOMElements;
				if (fixedSOMElements != null && fixedSOMElements.Length != 0)
				{
					FixedSOMImage fixedSOMImage = fixedSOMElements[0] as FixedSOMImage;
					if (fixedSOMImage != null)
					{
						image.Width = fixedSOMImage.BoundingRect.Width;
						image.Height = fixedSOMImage.BoundingRect.Height;
					}
				}
			}
			return @object;
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x000D65D0 File Offset: 0x000D47D0
		internal FixedElement GetElement(LogicalDirection dir)
		{
			FlowPosition clingPosition = this.GetClingPosition(dir);
			return (FixedElement)clingPosition._flowNode.Cookie;
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x000D65F8 File Offset: 0x000D47F8
		internal FixedElement GetScopingElement()
		{
			FlowPosition flowPosition = (FlowPosition)this.Clone();
			int num = 0;
			TextPointerContext pointerContext;
			while (flowPosition.FlowNode.Fp > 0 && !this.IsVirtual(this._FixedFlowMap[flowPosition.FlowNode.Fp - 1]) && (pointerContext = flowPosition.GetPointerContext(LogicalDirection.Backward)) != TextPointerContext.None)
			{
				if (pointerContext == TextPointerContext.ElementStart)
				{
					if (num == 0)
					{
						FlowPosition clingPosition = flowPosition.GetClingPosition(LogicalDirection.Backward);
						return (FixedElement)clingPosition._flowNode.Cookie;
					}
					num--;
				}
				else if (pointerContext == TextPointerContext.ElementEnd)
				{
					num++;
				}
				flowPosition.Move(LogicalDirection.Backward);
			}
			return this._container.ContainerElement;
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x000D6690 File Offset: 0x000D4890
		internal bool Move(int distance)
		{
			LogicalDirection dir = (distance >= 0) ? LogicalDirection.Forward : LogicalDirection.Backward;
			distance = Math.Abs(distance);
			FlowNode flowNode = this._flowNode;
			int offset = this._offset;
			while (distance > 0)
			{
				int num = this._vScan(dir, distance);
				if (num == 0)
				{
					this._flowNode = flowNode;
					this._offset = offset;
					return false;
				}
				distance -= num;
			}
			return true;
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x000D66E4 File Offset: 0x000D48E4
		internal bool Move(LogicalDirection dir)
		{
			return this._vScan(dir, -1) > 0;
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x000D66F4 File Offset: 0x000D48F4
		internal void MoveTo(FlowPosition flow)
		{
			this._flowNode = flow._flowNode;
			this._offset = flow._offset;
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x000D670E File Offset: 0x000D490E
		internal void AttachElement(FixedElement e)
		{
			this._flowNode.AttachElement(e);
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x000D671C File Offset: 0x000D491C
		internal void GetFlowNode(LogicalDirection direction, out FlowNode flowNode, out int offsetStart)
		{
			FlowPosition clingPosition = this.GetClingPosition(direction);
			offsetStart = clingPosition._offset;
			flowNode = clingPosition._flowNode;
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x000D6744 File Offset: 0x000D4944
		internal void GetFlowNodes(FlowPosition pEnd, out FlowNode[] flowNodes, out int offsetStart, out int offsetEnd)
		{
			flowNodes = null;
			offsetStart = 0;
			offsetEnd = 0;
			FlowPosition clingPosition = this.GetClingPosition(LogicalDirection.Forward);
			offsetStart = clingPosition._offset;
			ArrayList arrayList = new ArrayList();
			int i = this.GetDistance(pEnd);
			while (i > 0)
			{
				int num = clingPosition._vScan(LogicalDirection.Forward, i);
				i -= num;
				if (clingPosition.IsRun || clingPosition.IsObject)
				{
					arrayList.Add(clingPosition._flowNode);
					offsetEnd = clingPosition._offset;
				}
			}
			flowNodes = (FlowNode[])arrayList.ToArray(typeof(FlowNode));
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x000D67CC File Offset: 0x000D49CC
		internal FlowPosition GetClingPosition(LogicalDirection dir)
		{
			FlowPosition flowPosition = (FlowPosition)this.Clone();
			if (dir == LogicalDirection.Forward)
			{
				if (this._offset == this._NodeLength)
				{
					FlowNode flowNode = this._xGetNextFlowNode();
					if (!FlowNode.IsNull(flowNode))
					{
						flowPosition._flowNode = flowNode;
						flowPosition._offset = 0;
					}
				}
			}
			else if (this._offset == 0)
			{
				FlowNode flowNode = this._xGetPreviousFlowNode();
				if (!FlowNode.IsNull(flowNode))
				{
					flowPosition._flowNode = flowNode;
					flowPosition._offset = flowPosition._NodeLength;
				}
			}
			return flowPosition;
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x000D6841 File Offset: 0x000D4A41
		internal bool IsVirtual(FlowNode flowNode)
		{
			return flowNode.Type == FlowNodeType.Virtual;
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06002FA0 RID: 12192 RVA: 0x000D684D File Offset: 0x000D4A4D
		internal FixedTextContainer TextContainer
		{
			get
			{
				return this._container;
			}
		}

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x06002FA1 RID: 12193 RVA: 0x000D6855 File Offset: 0x000D4A55
		internal bool IsBoundary
		{
			get
			{
				return this._flowNode.Type == FlowNodeType.Boundary;
			}
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x06002FA2 RID: 12194 RVA: 0x000D6865 File Offset: 0x000D4A65
		internal bool IsStart
		{
			get
			{
				return this._flowNode.Type == FlowNodeType.Start;
			}
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06002FA3 RID: 12195 RVA: 0x000D6875 File Offset: 0x000D4A75
		internal bool IsEnd
		{
			get
			{
				return this._flowNode.Type == FlowNodeType.End;
			}
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06002FA4 RID: 12196 RVA: 0x000D6888 File Offset: 0x000D4A88
		internal bool IsSymbol
		{
			get
			{
				FlowNodeType type = this._flowNode.Type;
				return type == FlowNodeType.Start || type == FlowNodeType.End || type == FlowNodeType.Object;
			}
		}

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x06002FA5 RID: 12197 RVA: 0x000D68AF File Offset: 0x000D4AAF
		internal bool IsRun
		{
			get
			{
				return this._flowNode.Type == FlowNodeType.Run;
			}
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06002FA6 RID: 12198 RVA: 0x000D68BF File Offset: 0x000D4ABF
		internal bool IsObject
		{
			get
			{
				return this._flowNode.Type == FlowNodeType.Object;
			}
		}

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x06002FA7 RID: 12199 RVA: 0x000D68CF File Offset: 0x000D4ACF
		internal FlowNode FlowNode
		{
			get
			{
				return this._flowNode;
			}
		}

		// Token: 0x06002FA8 RID: 12200 RVA: 0x000D68D8 File Offset: 0x000D4AD8
		private int _vScan(LogicalDirection dir, int limit)
		{
			if (limit == 0)
			{
				return 0;
			}
			FlowNode flowNode = this._flowNode;
			int num = 0;
			if (dir == LogicalDirection.Forward)
			{
				if (this._offset == this._NodeLength || flowNode.Type == FlowNodeType.Boundary)
				{
					flowNode = this._xGetNextFlowNode();
					if (FlowNode.IsNull(flowNode))
					{
						return num;
					}
					this._flowNode = flowNode;
					num = this._NodeLength;
				}
				else
				{
					num = this._NodeLength - this._offset;
				}
				this._offset = this._NodeLength;
				if (limit > 0 && num > limit)
				{
					int num2 = num - limit;
					num = limit;
					this._offset -= num2;
				}
			}
			else
			{
				if (this._offset == 0 || flowNode.Type == FlowNodeType.Boundary)
				{
					flowNode = this._xGetPreviousFlowNode();
					if (FlowNode.IsNull(flowNode))
					{
						return num;
					}
					this._flowNode = flowNode;
					num = this._NodeLength;
				}
				else
				{
					num = this._offset;
				}
				this._offset = 0;
				if (limit > 0 && num > limit)
				{
					int num3 = num - limit;
					num = limit;
					this._offset += num3;
				}
			}
			return num;
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x000D69C4 File Offset: 0x000D4BC4
		private TextPointerContext _vGetSymbolType(LogicalDirection dir)
		{
			FlowNode flowNode = this._flowNode;
			if (dir == LogicalDirection.Forward)
			{
				if (this._offset == this._NodeLength)
				{
					flowNode = this._xGetNextFlowNode();
				}
				if (!FlowNode.IsNull(flowNode))
				{
					return this._FlowNodeTypeToTextSymbol(flowNode.Type);
				}
			}
			else
			{
				if (this._offset == 0)
				{
					flowNode = this._xGetPreviousFlowNode();
				}
				if (!FlowNode.IsNull(flowNode))
				{
					return this._FlowNodeTypeToTextSymbol(flowNode.Type);
				}
			}
			return TextPointerContext.None;
		}

		// Token: 0x06002FAA RID: 12202 RVA: 0x000D6A2C File Offset: 0x000D4C2C
		private FlowNode _xGetPreviousFlowNode()
		{
			if (this._flowNode.Fp > 1)
			{
				FlowNode flowNode = this._FixedFlowMap[this._flowNode.Fp - 1];
				if (this.IsVirtual(flowNode))
				{
					this._FixedTextBuilder.EnsureTextOMForPage((int)flowNode.Cookie);
					flowNode = this._FixedFlowMap[this._flowNode.Fp - 1];
				}
				if (flowNode.Type != FlowNodeType.Boundary)
				{
					return flowNode;
				}
			}
			return null;
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x000D6AA4 File Offset: 0x000D4CA4
		private FlowNode _xGetNextFlowNode()
		{
			if (this._flowNode.Fp < this._FixedFlowMap.FlowCount - 1)
			{
				FlowNode flowNode = this._FixedFlowMap[this._flowNode.Fp + 1];
				if (this.IsVirtual(flowNode))
				{
					this._FixedTextBuilder.EnsureTextOMForPage((int)flowNode.Cookie);
					flowNode = this._FixedFlowMap[this._flowNode.Fp + 1];
				}
				if (flowNode.Type != FlowNodeType.Boundary)
				{
					return flowNode;
				}
			}
			return null;
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x000D6B28 File Offset: 0x000D4D28
		private bool _IsSamePosition(FlowPosition flow)
		{
			return flow != null && this._OverlapAwareCompare(flow) == 0;
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x000D6B3C File Offset: 0x000D4D3C
		private int _OverlapAwareCompare(FlowPosition flow)
		{
			if (this == flow)
			{
				return 0;
			}
			int num = this._flowNode.CompareTo(flow._flowNode);
			if (num < 0)
			{
				if (this._flowNode.Fp == flow._flowNode.Fp - 1 && this._offset == this._NodeLength && flow._offset == 0)
				{
					return 0;
				}
			}
			else if (num > 0)
			{
				if (flow._flowNode.Fp == this._flowNode.Fp - 1 && flow._offset == flow._NodeLength && this._offset == 0)
				{
					return 0;
				}
			}
			else
			{
				num = this._offset.CompareTo(flow._offset);
			}
			return num;
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x000D6BE0 File Offset: 0x000D4DE0
		private TextPointerContext _FlowNodeTypeToTextSymbol(FlowNodeType t)
		{
			switch (t)
			{
			case FlowNodeType.Start:
				return TextPointerContext.ElementStart;
			case FlowNodeType.Run:
				return TextPointerContext.Text;
			case (FlowNodeType)3:
				break;
			case FlowNodeType.End:
				return TextPointerContext.ElementEnd;
			default:
				if (t == FlowNodeType.Object || t == FlowNodeType.Noop)
				{
					return TextPointerContext.EmbeddedElement;
				}
				break;
			}
			return TextPointerContext.None;
		}

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x06002FAF RID: 12207 RVA: 0x000D6C0E File Offset: 0x000D4E0E
		private int _NodeLength
		{
			get
			{
				if (this.IsRun)
				{
					return (int)this._flowNode.Cookie;
				}
				return 1;
			}
		}

		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x06002FB0 RID: 12208 RVA: 0x000D6C2A File Offset: 0x000D4E2A
		private FixedTextBuilder _FixedTextBuilder
		{
			get
			{
				return this._container.FixedTextBuilder;
			}
		}

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x06002FB1 RID: 12209 RVA: 0x000D6C37 File Offset: 0x000D4E37
		private FixedFlowMap _FixedFlowMap
		{
			get
			{
				return this._container.FixedTextBuilder.FixedFlowMap;
			}
		}

		// Token: 0x04001E47 RID: 7751
		private FixedTextContainer _container;

		// Token: 0x04001E48 RID: 7752
		private FlowNode _flowNode;

		// Token: 0x04001E49 RID: 7753
		private int _offset;
	}
}
