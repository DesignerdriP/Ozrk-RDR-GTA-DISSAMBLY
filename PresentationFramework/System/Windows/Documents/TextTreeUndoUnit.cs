using System;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x0200042A RID: 1066
	internal abstract class TextTreeUndoUnit : IUndoUnit
	{
		// Token: 0x06003E2F RID: 15919 RVA: 0x0011D6D0 File Offset: 0x0011B8D0
		internal TextTreeUndoUnit(TextContainer tree, int symbolOffset)
		{
			this._tree = tree;
			this._symbolOffset = symbolOffset;
			this._treeContentHashCode = this._tree.GetContentHashCode();
		}

		// Token: 0x06003E30 RID: 15920 RVA: 0x0011D6F8 File Offset: 0x0011B8F8
		public void Do()
		{
			this._tree.BeginChange();
			try
			{
				this.DoCore();
			}
			finally
			{
				this._tree.EndChange();
			}
		}

		// Token: 0x06003E31 RID: 15921
		public abstract void DoCore();

		// Token: 0x06003E32 RID: 15922 RVA: 0x000D4A23 File Offset: 0x000D2C23
		public bool Merge(IUndoUnit unit)
		{
			Invariant.Assert(unit != null);
			return false;
		}

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x06003E33 RID: 15923 RVA: 0x0011D734 File Offset: 0x0011B934
		protected TextContainer TextContainer
		{
			get
			{
				return this._tree;
			}
		}

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x06003E34 RID: 15924 RVA: 0x0011D73C File Offset: 0x0011B93C
		protected int SymbolOffset
		{
			get
			{
				return this._symbolOffset;
			}
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x0011D744 File Offset: 0x0011B944
		internal void SetTreeHashCode()
		{
			this._treeContentHashCode = this._tree.GetContentHashCode();
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x0011D757 File Offset: 0x0011B957
		internal void VerifyTreeContentHashCode()
		{
			if (this._tree.GetContentHashCode() != this._treeContentHashCode)
			{
				Invariant.Assert(false, "Undo unit is out of sync with TextContainer!");
			}
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x0011D778 File Offset: 0x0011B978
		internal static PropertyRecord[] GetPropertyRecordArray(DependencyObject d)
		{
			LocalValueEnumerator localValueEnumerator = d.GetLocalValueEnumerator();
			PropertyRecord[] array = new PropertyRecord[localValueEnumerator.Count];
			int num = 0;
			localValueEnumerator.Reset();
			while (localValueEnumerator.MoveNext())
			{
				LocalValueEntry localValueEntry = localValueEnumerator.Current;
				DependencyProperty property = localValueEntry.Property;
				if (!property.ReadOnly)
				{
					array[num].Property = property;
					array[num].Value = d.GetValue(property);
					num++;
				}
			}
			PropertyRecord[] array2;
			if (localValueEnumerator.Count != num)
			{
				array2 = new PropertyRecord[num];
				for (int i = 0; i < num; i++)
				{
					array2[i] = array[i];
				}
			}
			else
			{
				array2 = array;
			}
			return array2;
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x0011D824 File Offset: 0x0011BA24
		internal static LocalValueEnumerator ArrayToLocalValueEnumerator(PropertyRecord[] records)
		{
			DependencyObject dependencyObject = new DependencyObject();
			for (int i = 0; i < records.Length; i++)
			{
				dependencyObject.SetValue(records[i].Property, records[i].Value);
			}
			return dependencyObject.GetLocalValueEnumerator();
		}

		// Token: 0x04002694 RID: 9876
		private readonly TextContainer _tree;

		// Token: 0x04002695 RID: 9877
		private readonly int _symbolOffset;

		// Token: 0x04002696 RID: 9878
		private int _treeContentHashCode;
	}
}
