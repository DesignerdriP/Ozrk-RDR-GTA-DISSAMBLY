using System;
using System.Collections.Generic;
using MS.Internal;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x02000460 RID: 1120
	internal static class RetryHelper
	{
		// Token: 0x060040AA RID: 16554 RVA: 0x001274DC File Offset: 0x001256DC
		internal static bool TryCallAction(Action action, RetryHelper.RetryActionPreamble preamble, List<Type> ignoredExceptions, int retries = 3, bool throwOnFailure = false)
		{
			RetryHelper.ValidateExceptionTypeList(ignoredExceptions);
			int num = retries;
			bool flag = false;
			bool flag2 = true;
			do
			{
				try
				{
					if (action != null)
					{
						action();
					}
					flag = true;
					break;
				}
				catch (Exception exception) when (RetryHelper.MatchException(exception, ignoredExceptions))
				{
				}
				num--;
				if (num > 0)
				{
					flag2 = preamble(out action);
				}
			}
			while (num > 0 && flag2);
			if (!flag && throwOnFailure)
			{
				throw new RetriesExhaustedException();
			}
			return flag;
		}

		// Token: 0x060040AB RID: 16555 RVA: 0x00127558 File Offset: 0x00125758
		internal static bool TryCallAction(Action action, RetryHelper.RetryPreamble preamble, List<Type> ignoredExceptions, int retries = 3, bool throwOnFailure = false)
		{
			RetryHelper.ValidateExceptionTypeList(ignoredExceptions);
			int num = retries;
			bool flag = false;
			bool flag2 = true;
			do
			{
				try
				{
					if (action != null)
					{
						action();
					}
					flag = true;
					break;
				}
				catch (Exception exception) when (RetryHelper.MatchException(exception, ignoredExceptions))
				{
				}
				num--;
				if (num > 0)
				{
					flag2 = preamble();
				}
			}
			while (num > 0 && flag2);
			if (!flag && throwOnFailure)
			{
				throw new RetriesExhaustedException();
			}
			return flag;
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x001275D4 File Offset: 0x001257D4
		internal static bool TryExecuteFunction<TResult>(Func<TResult> func, out TResult result, RetryHelper.RetryFunctionPreamble<TResult> preamble, List<Type> ignoredExceptions, int retries = 3, bool throwOnFailure = false)
		{
			RetryHelper.ValidateExceptionTypeList(ignoredExceptions);
			result = default(TResult);
			int num = retries;
			bool flag = false;
			bool flag2 = true;
			do
			{
				try
				{
					if (func != null)
					{
						result = func();
					}
					flag = true;
					break;
				}
				catch (Exception exception) when (RetryHelper.MatchException(exception, ignoredExceptions))
				{
				}
				num--;
				if (num > 0)
				{
					flag2 = preamble(out func);
				}
			}
			while (num > 0 && flag2);
			if (!flag && throwOnFailure)
			{
				throw new RetriesExhaustedException();
			}
			return flag;
		}

		// Token: 0x060040AD RID: 16557 RVA: 0x00127660 File Offset: 0x00125860
		internal static bool TryExecuteFunction<TResult>(Func<TResult> func, out TResult result, RetryHelper.RetryPreamble preamble, List<Type> ignoredExceptions, int retries = 3, bool throwOnFailure = false)
		{
			RetryHelper.ValidateExceptionTypeList(ignoredExceptions);
			result = default(TResult);
			int num = retries;
			bool flag = false;
			bool flag2 = true;
			do
			{
				try
				{
					if (func != null)
					{
						result = func();
					}
					flag = true;
					break;
				}
				catch (Exception exception) when (RetryHelper.MatchException(exception, ignoredExceptions))
				{
				}
				num--;
				if (num > 0)
				{
					flag2 = preamble();
				}
			}
			while (num > 0 && flag2);
			if (!flag && throwOnFailure)
			{
				throw new RetriesExhaustedException();
			}
			return flag;
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x001276E8 File Offset: 0x001258E8
		private static bool MatchException(Exception exception, List<Type> exceptions)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			if (exceptions == null)
			{
				throw new ArgumentNullException("exceptions");
			}
			Type exceptionType = exception.GetType();
			Type left = exceptions.Find((Type e) => e.IsAssignableFrom(exceptionType));
			return left != null;
		}

		// Token: 0x060040AF RID: 16559 RVA: 0x0012773D File Offset: 0x0012593D
		private static void ValidateExceptionTypeList(List<Type> exceptions)
		{
			if (exceptions == null)
			{
				throw new ArgumentNullException("exceptions");
			}
			Invariant.Assert(exceptions.TrueForAll((Type t) => typeof(Exception).IsAssignableFrom(t)));
		}

		// Token: 0x02000950 RID: 2384
		// (Invoke) Token: 0x06008702 RID: 34562
		internal delegate bool RetryPreamble();

		// Token: 0x02000951 RID: 2385
		// (Invoke) Token: 0x06008706 RID: 34566
		internal delegate bool RetryActionPreamble(out Action action);

		// Token: 0x02000952 RID: 2386
		// (Invoke) Token: 0x0600870A RID: 34570
		internal delegate bool RetryFunctionPreamble<TResult>(out Func<TResult> func);
	}
}
