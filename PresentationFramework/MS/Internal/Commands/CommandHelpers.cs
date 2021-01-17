using System;
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace MS.Internal.Commands
{
	// Token: 0x0200076C RID: 1900
	internal static class CommandHelpers
	{
		// Token: 0x06007872 RID: 30834 RVA: 0x00225014 File Offset: 0x00223214
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, null);
		}

		// Token: 0x06007873 RID: 30835 RVA: 0x00225020 File Offset: 0x00223220
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, InputGesture inputGesture)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, new InputGesture[]
			{
				inputGesture
			});
		}

		// Token: 0x06007874 RID: 30836 RVA: 0x00225035 File Offset: 0x00223235
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, Key key)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, new InputGesture[]
			{
				new KeyGesture(key)
			});
		}

		// Token: 0x06007875 RID: 30837 RVA: 0x0022504F File Offset: 0x0022324F
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, InputGesture inputGesture, InputGesture inputGesture2)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, new InputGesture[]
			{
				inputGesture,
				inputGesture2
			});
		}

		// Token: 0x06007876 RID: 30838 RVA: 0x00225069 File Offset: 0x00223269
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, null);
		}

		// Token: 0x06007877 RID: 30839 RVA: 0x00225075 File Offset: 0x00223275
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, InputGesture inputGesture)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				inputGesture
			});
		}

		// Token: 0x06007878 RID: 30840 RVA: 0x0022508B File Offset: 0x0022328B
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, Key key)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				new KeyGesture(key)
			});
		}

		// Token: 0x06007879 RID: 30841 RVA: 0x002250A6 File Offset: 0x002232A6
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, InputGesture inputGesture, InputGesture inputGesture2)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				inputGesture,
				inputGesture2
			});
		}

		// Token: 0x0600787A RID: 30842 RVA: 0x002250C1 File Offset: 0x002232C1
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, InputGesture inputGesture, InputGesture inputGesture2, InputGesture inputGesture3, InputGesture inputGesture4)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				inputGesture,
				inputGesture2,
				inputGesture3,
				inputGesture4
			});
		}

		// Token: 0x0600787B RID: 30843 RVA: 0x002250E8 File Offset: 0x002232E8
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, Key key, ModifierKeys modifierKeys, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				new KeyGesture(key, modifierKeys)
			});
		}

		// Token: 0x0600787C RID: 30844 RVA: 0x00225110 File Offset: 0x00223310
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, string srid1, string srid2)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, new InputGesture[]
			{
				KeyGesture.CreateFromResourceStrings(SR.Get(srid1), SR.Get(srid2))
			});
		}

		// Token: 0x0600787D RID: 30845 RVA: 0x00225144 File Offset: 0x00223344
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, string srid1, string srid2)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				KeyGesture.CreateFromResourceStrings(SR.Get(srid1), SR.Get(srid2))
			});
		}

		// Token: 0x0600787E RID: 30846 RVA: 0x00225178 File Offset: 0x00223378
		private static void PrivateRegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, params InputGesture[] inputGestures)
		{
			CommandManager.RegisterClassCommandBinding(controlType, new CommandBinding(command, executedRoutedEventHandler, canExecuteRoutedEventHandler));
			if (inputGestures != null)
			{
				for (int i = 0; i < inputGestures.Length; i++)
				{
					CommandManager.RegisterClassInputBinding(controlType, new InputBinding(command, inputGestures[i]));
				}
			}
		}

		// Token: 0x0600787F RID: 30847 RVA: 0x002251B8 File Offset: 0x002233B8
		internal static bool CanExecuteCommandSource(ICommandSource commandSource)
		{
			ICommand command = commandSource.Command;
			if (command == null)
			{
				return false;
			}
			object commandParameter = commandSource.CommandParameter;
			IInputElement inputElement = commandSource.CommandTarget;
			RoutedCommand routedCommand = command as RoutedCommand;
			if (routedCommand != null)
			{
				if (inputElement == null)
				{
					inputElement = (commandSource as IInputElement);
				}
				return routedCommand.CanExecute(commandParameter, inputElement);
			}
			return command.CanExecute(commandParameter);
		}

		// Token: 0x06007880 RID: 30848 RVA: 0x00225203 File Offset: 0x00223403
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void ExecuteCommandSource(ICommandSource commandSource)
		{
			CommandHelpers.CriticalExecuteCommandSource(commandSource, false);
		}

		// Token: 0x06007881 RID: 30849 RVA: 0x0022520C File Offset: 0x0022340C
		[SecurityCritical]
		internal static void CriticalExecuteCommandSource(ICommandSource commandSource, bool userInitiated)
		{
			ICommand command = commandSource.Command;
			if (command != null)
			{
				object commandParameter = commandSource.CommandParameter;
				IInputElement inputElement = commandSource.CommandTarget;
				RoutedCommand routedCommand = command as RoutedCommand;
				if (routedCommand != null)
				{
					if (inputElement == null)
					{
						inputElement = (commandSource as IInputElement);
					}
					if (routedCommand.CanExecute(commandParameter, inputElement))
					{
						routedCommand.ExecuteCore(commandParameter, inputElement, userInitiated);
						return;
					}
				}
				else if (command.CanExecute(commandParameter))
				{
					command.Execute(commandParameter);
				}
			}
		}

		// Token: 0x06007882 RID: 30850 RVA: 0x0022526C File Offset: 0x0022346C
		internal static void ExecuteCommand(ICommand command, object parameter, IInputElement target)
		{
			RoutedCommand routedCommand = command as RoutedCommand;
			if (routedCommand != null)
			{
				if (routedCommand.CanExecute(parameter, target))
				{
					routedCommand.Execute(parameter, target);
					return;
				}
			}
			else if (command.CanExecute(parameter))
			{
				command.Execute(parameter);
			}
		}
	}
}
