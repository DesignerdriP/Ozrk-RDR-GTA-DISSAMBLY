using System;
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace MS.Internal.AppModel
{
	// Token: 0x02000793 RID: 1939
	internal class CommandWithArgument
	{
		// Token: 0x060079BB RID: 31163 RVA: 0x00228482 File Offset: 0x00226682
		[SecurityCritical]
		public CommandWithArgument(RoutedCommand command) : this(command, null)
		{
		}

		// Token: 0x060079BC RID: 31164 RVA: 0x0022848C File Offset: 0x0022668C
		[SecurityCritical]
		public CommandWithArgument(RoutedCommand command, object argument)
		{
			this._command = new SecurityCriticalDataForSet<RoutedCommand>(command);
			this._argument = argument;
		}

		// Token: 0x060079BD RID: 31165 RVA: 0x002284A8 File Offset: 0x002266A8
		[SecurityCritical]
		public bool Execute(IInputElement target, object argument)
		{
			if (argument == null)
			{
				argument = this._argument;
			}
			if (this._command.Value is ISecureCommand)
			{
				bool flag;
				if (this._command.Value.CriticalCanExecute(argument, target, true, out flag))
				{
					this._command.Value.ExecuteCore(argument, target, true);
					return true;
				}
				return false;
			}
			else
			{
				if (this._command.Value.CanExecute(argument, target))
				{
					this._command.Value.Execute(argument, target);
					return true;
				}
				return false;
			}
		}

		// Token: 0x060079BE RID: 31166 RVA: 0x0022852C File Offset: 0x0022672C
		[SecurityCritical]
		public bool QueryEnabled(IInputElement target, object argument)
		{
			if (argument == null)
			{
				argument = this._argument;
			}
			if (this._command.Value is ISecureCommand)
			{
				bool flag;
				return this._command.Value.CriticalCanExecute(argument, target, true, out flag);
			}
			return this._command.Value.CanExecute(argument, target);
		}

		// Token: 0x17001CBA RID: 7354
		// (get) Token: 0x060079BF RID: 31167 RVA: 0x0022857E File Offset: 0x0022677E
		public RoutedCommand Command
		{
			get
			{
				return this._command.Value;
			}
		}

		// Token: 0x0400399B RID: 14747
		private object _argument;

		// Token: 0x0400399C RID: 14748
		private SecurityCriticalDataForSet<RoutedCommand> _command;
	}
}
