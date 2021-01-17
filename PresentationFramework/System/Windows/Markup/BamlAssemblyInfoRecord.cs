﻿using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x02000208 RID: 520
	internal class BamlAssemblyInfoRecord : BamlVariableSizedRecord
	{
		// Token: 0x06002060 RID: 8288 RVA: 0x000960BC File Offset: 0x000942BC
		internal BamlAssemblyInfoRecord()
		{
			base.Pin();
			this.AssemblyId = -1;
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x000960D1 File Offset: 0x000942D1
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AssemblyId = bamlBinaryReader.ReadInt16();
			this.AssemblyFullName = bamlBinaryReader.ReadString();
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x000960EB File Offset: 0x000942EB
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.AssemblyId);
			bamlBinaryWriter.Write(this.AssemblyFullName);
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x00096108 File Offset: 0x00094308
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlAssemblyInfoRecord bamlAssemblyInfoRecord = (BamlAssemblyInfoRecord)record;
			bamlAssemblyInfoRecord._assemblyFullName = this._assemblyFullName;
			bamlAssemblyInfoRecord._assembly = this._assembly;
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06002064 RID: 8292 RVA: 0x0009613C File Offset: 0x0009433C
		// (set) Token: 0x06002065 RID: 8293 RVA: 0x00096173 File Offset: 0x00094373
		internal short AssemblyId
		{
			get
			{
				short num = (short)this._flags[BamlAssemblyInfoRecord._assemblyIdLowSection];
				return num | (short)(this._flags[BamlAssemblyInfoRecord._assemblyIdHighSection] << 8);
			}
			set
			{
				this._flags[BamlAssemblyInfoRecord._assemblyIdLowSection] = (int)(value & 255);
				this._flags[BamlAssemblyInfoRecord._assemblyIdHighSection] = (int)((short)(((int)value & 65280) >> 8));
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06002066 RID: 8294 RVA: 0x000961A7 File Offset: 0x000943A7
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlAssemblyInfoRecord._assemblyIdHighSection;
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06002067 RID: 8295 RVA: 0x000961AE File Offset: 0x000943AE
		// (set) Token: 0x06002068 RID: 8296 RVA: 0x000961B6 File Offset: 0x000943B6
		internal string AssemblyFullName
		{
			get
			{
				return this._assemblyFullName;
			}
			set
			{
				this._assemblyFullName = value;
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06002069 RID: 8297 RVA: 0x000961BF File Offset: 0x000943BF
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.AssemblyInfo;
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x0600206A RID: 8298 RVA: 0x000961C3 File Offset: 0x000943C3
		// (set) Token: 0x0600206B RID: 8299 RVA: 0x000961CB File Offset: 0x000943CB
		internal Assembly Assembly
		{
			get
			{
				return this._assembly;
			}
			set
			{
				this._assembly = value;
			}
		}

		// Token: 0x0400154D RID: 5453
		private static BitVector32.Section _assemblyIdLowSection = BitVector32.CreateSection(255, BamlVariableSizedRecord.LastFlagsSection);

		// Token: 0x0400154E RID: 5454
		private static BitVector32.Section _assemblyIdHighSection = BitVector32.CreateSection(255, BamlAssemblyInfoRecord._assemblyIdLowSection);

		// Token: 0x0400154F RID: 5455
		private string _assemblyFullName;

		// Token: 0x04001550 RID: 5456
		private Assembly _assembly;
	}
}
