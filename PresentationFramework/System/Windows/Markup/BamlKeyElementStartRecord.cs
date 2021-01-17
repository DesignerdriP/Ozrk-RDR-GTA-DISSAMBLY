﻿using System;

namespace System.Windows.Markup
{
	// Token: 0x02000205 RID: 517
	internal class BamlKeyElementStartRecord : BamlDefAttributeKeyTypeRecord, IBamlDictionaryKey
	{
		// Token: 0x0600205A RID: 8282 RVA: 0x000960A6 File Offset: 0x000942A6
		internal BamlKeyElementStartRecord()
		{
			base.Pin();
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x0600205B RID: 8283 RVA: 0x000960B4 File Offset: 0x000942B4
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.KeyElementStart;
			}
		}
	}
}
