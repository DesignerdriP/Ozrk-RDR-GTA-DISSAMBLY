using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;
using System.Windows.Markup.Localizer;

namespace MS.Internal.Globalization
{
	// Token: 0x0200069B RID: 1691
	internal sealed class BamlResourceSerializer
	{
		// Token: 0x06006E17 RID: 28183 RVA: 0x001FB05E File Offset: 0x001F925E
		internal static void Serialize(BamlLocalizer localizer, BamlTree tree, Stream output)
		{
			new BamlResourceSerializer().SerializeImp(localizer, tree, output);
		}

		// Token: 0x06006E18 RID: 28184 RVA: 0x0000326D File Offset: 0x0000146D
		private BamlResourceSerializer()
		{
		}

		// Token: 0x06006E19 RID: 28185 RVA: 0x001FB070 File Offset: 0x001F9270
		private void SerializeImp(BamlLocalizer localizer, BamlTree tree, Stream output)
		{
			this._writer = new BamlWriter(output);
			this._bamlTreeStack = new Stack<BamlTreeNode>();
			this._bamlTreeStack.Push(tree.Root);
			while (this._bamlTreeStack.Count > 0)
			{
				BamlTreeNode bamlTreeNode = this._bamlTreeStack.Pop();
				if (!bamlTreeNode.Visited)
				{
					bamlTreeNode.Visited = true;
					bamlTreeNode.Serialize(this._writer);
					this.PushChildrenToStack(bamlTreeNode.Children);
				}
				else
				{
					BamlStartElementNode bamlStartElementNode = bamlTreeNode as BamlStartElementNode;
					if (bamlStartElementNode != null)
					{
						localizer.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(BamlTreeMap.GetKey(bamlStartElementNode), BamlLocalizerError.DuplicateElement));
					}
				}
			}
		}

		// Token: 0x06006E1A RID: 28186 RVA: 0x001FB108 File Offset: 0x001F9308
		private void PushChildrenToStack(List<BamlTreeNode> children)
		{
			if (children == null)
			{
				return;
			}
			for (int i = children.Count - 1; i >= 0; i--)
			{
				this._bamlTreeStack.Push(children[i]);
			}
		}

		// Token: 0x0400362F RID: 13871
		private BamlWriter _writer;

		// Token: 0x04003630 RID: 13872
		private Stack<BamlTreeNode> _bamlTreeStack;
	}
}
