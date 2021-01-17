using System;
using System.Collections.Generic;
using MS.Internal.Xaml.Context;

namespace System.Windows.Baml2006
{
	// Token: 0x02000162 RID: 354
	internal class Baml2006ReaderContext
	{
		// Token: 0x0600103F RID: 4159 RVA: 0x000413D8 File Offset: 0x0003F5D8
		public Baml2006ReaderContext(Baml2006SchemaContext schemaContext)
		{
			if (schemaContext == null)
			{
				throw new ArgumentNullException("schemaContext");
			}
			this._schemaContext = schemaContext;
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x0004142A File Offset: 0x0003F62A
		public Baml2006SchemaContext SchemaContext
		{
			get
			{
				return this._schemaContext;
			}
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x00041432 File Offset: 0x0003F632
		public void PushScope()
		{
			this._stack.PushScope();
			this.CurrentFrame.FreezeFreezables = this.PreviousFrame.FreezeFreezables;
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x00041455 File Offset: 0x0003F655
		public void PopScope()
		{
			this._stack.PopScope();
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001043 RID: 4163 RVA: 0x00041462 File Offset: 0x0003F662
		public Baml2006ReaderFrame CurrentFrame
		{
			get
			{
				return this._stack.CurrentFrame;
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x0004146F File Offset: 0x0003F66F
		public Baml2006ReaderFrame PreviousFrame
		{
			get
			{
				return this._stack.PreviousFrame;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x0004147C File Offset: 0x0003F67C
		// (set) Token: 0x06001046 RID: 4166 RVA: 0x00041484 File Offset: 0x0003F684
		public List<KeyRecord> KeyList { get; set; }

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001047 RID: 4167 RVA: 0x0004148D File Offset: 0x0003F68D
		// (set) Token: 0x06001048 RID: 4168 RVA: 0x00041495 File Offset: 0x0003F695
		public int CurrentKey { get; set; }

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001049 RID: 4169 RVA: 0x0004149E File Offset: 0x0003F69E
		public KeyRecord LastKey
		{
			get
			{
				if (this.KeyList != null && this.KeyList.Count > 0)
				{
					return this.KeyList[this.KeyList.Count - 1];
				}
				return null;
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x0600104A RID: 4170 RVA: 0x000414D0 File Offset: 0x0003F6D0
		// (set) Token: 0x0600104B RID: 4171 RVA: 0x000414D8 File Offset: 0x0003F6D8
		public bool InsideKeyRecord { get; set; }

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x0600104C RID: 4172 RVA: 0x000414E1 File Offset: 0x0003F6E1
		// (set) Token: 0x0600104D RID: 4173 RVA: 0x000414E9 File Offset: 0x0003F6E9
		public bool InsideStaticResource { get; set; }

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x0600104E RID: 4174 RVA: 0x000414F2 File Offset: 0x0003F6F2
		// (set) Token: 0x0600104F RID: 4175 RVA: 0x000414FA File Offset: 0x0003F6FA
		public int TemplateStartDepth { get; set; }

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001050 RID: 4176 RVA: 0x00041503 File Offset: 0x0003F703
		// (set) Token: 0x06001051 RID: 4177 RVA: 0x0004150B File Offset: 0x0003F70B
		public int LineNumber { get; set; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001052 RID: 4178 RVA: 0x00041514 File Offset: 0x0003F714
		// (set) Token: 0x06001053 RID: 4179 RVA: 0x0004151C File Offset: 0x0003F71C
		public int LineOffset { get; set; }

		// Token: 0x040011DD RID: 4573
		private Baml2006SchemaContext _schemaContext;

		// Token: 0x040011DE RID: 4574
		private XamlContextStack<Baml2006ReaderFrame> _stack = new XamlContextStack<Baml2006ReaderFrame>(() => new Baml2006ReaderFrame());
	}
}
