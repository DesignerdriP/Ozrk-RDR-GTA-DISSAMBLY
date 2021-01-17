using System;

namespace System.Windows.Documents
{
	// Token: 0x020003AC RID: 940
	internal class RtfToken
	{
		// Token: 0x0600329F RID: 12959 RVA: 0x0000326D File Offset: 0x0000146D
		internal RtfToken()
		{
		}

		// Token: 0x060032A0 RID: 12960 RVA: 0x000E3BD8 File Offset: 0x000E1DD8
		internal void Empty()
		{
			this._type = RtfTokenType.TokenInvalid;
			this._rtfControlWordInfo = null;
			this._parameter = 268435456L;
			this._text = "";
		}

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x060032A1 RID: 12961 RVA: 0x000E3BFF File Offset: 0x000E1DFF
		// (set) Token: 0x060032A2 RID: 12962 RVA: 0x000E3C07 File Offset: 0x000E1E07
		internal RtfTokenType Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x060032A3 RID: 12963 RVA: 0x000E3C10 File Offset: 0x000E1E10
		// (set) Token: 0x060032A4 RID: 12964 RVA: 0x000E3C18 File Offset: 0x000E1E18
		internal RtfControlWordInfo RtfControlWordInfo
		{
			get
			{
				return this._rtfControlWordInfo;
			}
			set
			{
				this._rtfControlWordInfo = value;
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x060032A5 RID: 12965 RVA: 0x000E3C21 File Offset: 0x000E1E21
		// (set) Token: 0x060032A6 RID: 12966 RVA: 0x000E3C34 File Offset: 0x000E1E34
		internal long Parameter
		{
			get
			{
				if (!this.HasParameter)
				{
					return 0L;
				}
				return this._parameter;
			}
			set
			{
				this._parameter = value;
			}
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x060032A7 RID: 12967 RVA: 0x000E3C3D File Offset: 0x000E1E3D
		// (set) Token: 0x060032A8 RID: 12968 RVA: 0x000E3C45 File Offset: 0x000E1E45
		internal string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
			}
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x060032A9 RID: 12969 RVA: 0x000E3C4E File Offset: 0x000E1E4E
		internal long ToggleValue
		{
			get
			{
				if (!this.HasParameter)
				{
					return 1L;
				}
				return this.Parameter;
			}
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x060032AA RID: 12970 RVA: 0x000E3C61 File Offset: 0x000E1E61
		internal bool FlagValue
		{
			get
			{
				return !this.HasParameter || (this.HasParameter && this.Parameter > 0L);
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x060032AB RID: 12971 RVA: 0x000E3C80 File Offset: 0x000E1E80
		internal bool HasParameter
		{
			get
			{
				return this._parameter != 268435456L;
			}
		}

		// Token: 0x040023CD RID: 9165
		internal const long INVALID_PARAMETER = 268435456L;

		// Token: 0x040023CE RID: 9166
		private RtfTokenType _type;

		// Token: 0x040023CF RID: 9167
		private RtfControlWordInfo _rtfControlWordInfo;

		// Token: 0x040023D0 RID: 9168
		private long _parameter;

		// Token: 0x040023D1 RID: 9169
		private string _text;
	}
}
