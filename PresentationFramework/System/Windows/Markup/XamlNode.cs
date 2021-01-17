﻿using System;

namespace System.Windows.Markup
{
	// Token: 0x0200023E RID: 574
	internal class XamlNode
	{
		// Token: 0x0600229A RID: 8858 RVA: 0x000ABEE5 File Offset: 0x000AA0E5
		internal XamlNode(XamlNodeType tokenType, int lineNumber, int linePosition, int depth)
		{
			this._token = tokenType;
			this._lineNumber = lineNumber;
			this._linePosition = linePosition;
			this._depth = depth;
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x0600229B RID: 8859 RVA: 0x000ABF0A File Offset: 0x000AA10A
		internal XamlNodeType TokenType
		{
			get
			{
				return this._token;
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x0600229C RID: 8860 RVA: 0x000ABF12 File Offset: 0x000AA112
		internal int LineNumber
		{
			get
			{
				return this._lineNumber;
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x0600229D RID: 8861 RVA: 0x000ABF1A File Offset: 0x000AA11A
		internal int LinePosition
		{
			get
			{
				return this._linePosition;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x0600229E RID: 8862 RVA: 0x000ABF22 File Offset: 0x000AA122
		internal int Depth
		{
			get
			{
				return this._depth;
			}
		}

		// Token: 0x04001A21 RID: 6689
		internal static XamlNodeType[] ScopeStartTokens = new XamlNodeType[]
		{
			XamlNodeType.DocumentStart,
			XamlNodeType.ElementStart,
			XamlNodeType.PropertyComplexStart,
			XamlNodeType.PropertyArrayStart,
			XamlNodeType.PropertyIListStart,
			XamlNodeType.PropertyIDictionaryStart
		};

		// Token: 0x04001A22 RID: 6690
		internal static XamlNodeType[] ScopeEndTokens = new XamlNodeType[]
		{
			XamlNodeType.DocumentEnd,
			XamlNodeType.ElementEnd,
			XamlNodeType.PropertyComplexEnd,
			XamlNodeType.PropertyArrayEnd,
			XamlNodeType.PropertyIListEnd,
			XamlNodeType.PropertyIDictionaryEnd
		};

		// Token: 0x04001A23 RID: 6691
		private XamlNodeType _token;

		// Token: 0x04001A24 RID: 6692
		private int _lineNumber;

		// Token: 0x04001A25 RID: 6693
		private int _linePosition;

		// Token: 0x04001A26 RID: 6694
		private int _depth;
	}
}
