﻿using System;

namespace System.Windows.Controls
{
	/// <summary>Specifies the spelling reform rules used by the spellchecker of the text editing control (i.e. <see cref="T:System.Windows.Controls.TextBox" /> or <see cref="T:System.Windows.Controls.RichTextBox" />).</summary>
	// Token: 0x02000535 RID: 1333
	public enum SpellingReform
	{
		/// <summary>Use spelling rules from both before and after the spelling reform.</summary>
		// Token: 0x04002E48 RID: 11848
		PreAndPostreform,
		/// <summary>Use spelling rules from before the spelling reform.</summary>
		// Token: 0x04002E49 RID: 11849
		Prereform,
		/// <summary>Use spelling rules from after the spelling reform.</summary>
		// Token: 0x04002E4A RID: 11850
		Postreform
	}
}
