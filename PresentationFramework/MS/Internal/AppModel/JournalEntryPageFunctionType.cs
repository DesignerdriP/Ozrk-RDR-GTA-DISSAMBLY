using System;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x0200078D RID: 1933
	[Serializable]
	internal class JournalEntryPageFunctionType : JournalEntryPageFunctionSaver, ISerializable
	{
		// Token: 0x0600796F RID: 31087 RVA: 0x002272D8 File Offset: 0x002254D8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal JournalEntryPageFunctionType(JournalEntryGroupState jeGroupState, PageFunctionBase pageFunction) : base(jeGroupState, pageFunction)
		{
			string assemblyQualifiedName = pageFunction.GetType().AssemblyQualifiedName;
			this._typeName = new SecurityCriticalDataForSet<string>(assemblyQualifiedName);
		}

		// Token: 0x06007970 RID: 31088 RVA: 0x00227305 File Offset: 0x00225505
		[SecurityCritical]
		protected JournalEntryPageFunctionType(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._typeName = new SecurityCriticalDataForSet<string>(info.GetString("_typeName"));
		}

		// Token: 0x06007971 RID: 31089 RVA: 0x00227325 File Offset: 0x00225525
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_typeName", this._typeName.Value);
		}

		// Token: 0x06007972 RID: 31090 RVA: 0x00227345 File Offset: 0x00225545
		internal override void SaveState(object contentObject)
		{
			base.SaveState(contentObject);
		}

		// Token: 0x06007973 RID: 31091 RVA: 0x00227350 File Offset: 0x00225550
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override PageFunctionBase ResumePageFunction()
		{
			Invariant.Assert(this._typeName.Value != null, "JournalEntry does not contain the Type for the PageFunction to be created");
			Type type = Type.GetType(this._typeName.Value);
			new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Assert();
			PageFunctionBase pageFunctionBase;
			try
			{
				pageFunctionBase = (PageFunctionBase)Activator.CreateInstance(type);
			}
			catch (Exception innerException)
			{
				throw new Exception(SR.Get("FailedResumePageFunction", new object[]
				{
					this._typeName.Value
				}), innerException);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			this.InitializeComponent(pageFunctionBase);
			this.RestoreState(pageFunctionBase);
			return pageFunctionBase;
		}

		// Token: 0x06007974 RID: 31092 RVA: 0x002273F8 File Offset: 0x002255F8
		private void InitializeComponent(PageFunctionBase pageFunction)
		{
			IComponentConnector componentConnector = pageFunction as IComponentConnector;
			if (componentConnector != null)
			{
				componentConnector.InitializeComponent();
			}
		}

		// Token: 0x04003983 RID: 14723
		private SecurityCriticalDataForSet<string> _typeName;
	}
}
