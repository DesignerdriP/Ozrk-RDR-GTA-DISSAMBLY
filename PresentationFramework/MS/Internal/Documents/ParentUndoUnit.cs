using System;
using System.Collections;
using System.Windows;

namespace MS.Internal.Documents
{
	// Token: 0x020006E8 RID: 1768
	internal class ParentUndoUnit : IParentUndoUnit, IUndoUnit
	{
		// Token: 0x060071A3 RID: 29091 RVA: 0x00207A1D File Offset: 0x00205C1D
		public ParentUndoUnit(string description)
		{
			this.Init(description);
		}

		// Token: 0x060071A4 RID: 29092 RVA: 0x00207A2C File Offset: 0x00205C2C
		public virtual void Open(IParentUndoUnit newUnit)
		{
			if (newUnit == null)
			{
				throw new ArgumentNullException("newUnit");
			}
			IParentUndoUnit deepestOpenUnit = this.DeepestOpenUnit;
			if (deepestOpenUnit == null)
			{
				if (this.IsInParentUnitChain(newUnit))
				{
					throw new InvalidOperationException(SR.Get("UndoUnitCantBeOpenedTwice"));
				}
				this._openedUnit = newUnit;
				if (newUnit != null)
				{
					newUnit.Container = this;
					return;
				}
			}
			else
			{
				if (newUnit != null)
				{
					newUnit.Container = deepestOpenUnit;
				}
				deepestOpenUnit.Open(newUnit);
			}
		}

		// Token: 0x060071A5 RID: 29093 RVA: 0x00207A8D File Offset: 0x00205C8D
		public virtual void Close(UndoCloseAction closeAction)
		{
			this.Close(this.OpenedUnit, closeAction);
		}

		// Token: 0x060071A6 RID: 29094 RVA: 0x00207A9C File Offset: 0x00205C9C
		public virtual void Close(IParentUndoUnit unit, UndoCloseAction closeAction)
		{
			if (unit == null)
			{
				throw new ArgumentNullException("unit");
			}
			if (this.OpenedUnit == null)
			{
				throw new InvalidOperationException(SR.Get("UndoNoOpenUnit"));
			}
			if (this.OpenedUnit != unit)
			{
				IParentUndoUnit parentUndoUnit = this;
				while (parentUndoUnit.OpenedUnit != null && parentUndoUnit.OpenedUnit != unit)
				{
					parentUndoUnit = parentUndoUnit.OpenedUnit;
				}
				if (parentUndoUnit.OpenedUnit == null)
				{
					throw new ArgumentException(SR.Get("UndoUnitNotFound"), "unit");
				}
				if (parentUndoUnit != this)
				{
					parentUndoUnit.Close(closeAction);
					return;
				}
			}
			UndoManager undoManager = this.TopContainer as UndoManager;
			if (closeAction != UndoCloseAction.Commit)
			{
				if (undoManager != null)
				{
					undoManager.IsEnabled = false;
				}
				if (this.OpenedUnit.OpenedUnit != null)
				{
					this.OpenedUnit.Close(closeAction);
				}
				if (closeAction == UndoCloseAction.Rollback)
				{
					this.OpenedUnit.Do();
				}
				this._openedUnit = null;
				if (this.TopContainer is UndoManager)
				{
					((UndoManager)this.TopContainer).OnNextDiscard();
				}
				else
				{
					((IParentUndoUnit)this.TopContainer).OnNextDiscard();
				}
				if (undoManager != null)
				{
					undoManager.IsEnabled = true;
					return;
				}
			}
			else
			{
				if (this.OpenedUnit.OpenedUnit != null)
				{
					this.OpenedUnit.Close(UndoCloseAction.Commit);
				}
				IParentUndoUnit openedUnit = this.OpenedUnit;
				this._openedUnit = null;
				this.Add(openedUnit);
				this.SetLastUnit(openedUnit);
			}
		}

		// Token: 0x060071A7 RID: 29095 RVA: 0x00207BD4 File Offset: 0x00205DD4
		public virtual void Add(IUndoUnit unit)
		{
			if (unit == null)
			{
				throw new ArgumentNullException("unit");
			}
			IParentUndoUnit deepestOpenUnit = this.DeepestOpenUnit;
			if (deepestOpenUnit != null)
			{
				deepestOpenUnit.Add(unit);
				return;
			}
			if (this.IsInParentUnitChain(unit))
			{
				throw new InvalidOperationException(SR.Get("UndoUnitCantBeAddedTwice"));
			}
			if (this.Locked)
			{
				throw new InvalidOperationException(SR.Get("UndoUnitLocked"));
			}
			if (!this.Merge(unit))
			{
				this._units.Push(unit);
				if (this.LastUnit is IParentUndoUnit)
				{
					((IParentUndoUnit)this.LastUnit).OnNextAdd();
				}
				this.SetLastUnit(unit);
			}
		}

		// Token: 0x060071A8 RID: 29096 RVA: 0x00207C6B File Offset: 0x00205E6B
		public virtual void Clear()
		{
			if (this.Locked)
			{
				throw new InvalidOperationException(SR.Get("UndoUnitLocked"));
			}
			this._units.Clear();
			this.SetOpenedUnit(null);
			this.SetLastUnit(null);
		}

		// Token: 0x060071A9 RID: 29097 RVA: 0x00207CA0 File Offset: 0x00205EA0
		public virtual void OnNextAdd()
		{
			this._locked = true;
			foreach (object obj in this._units)
			{
				IUndoUnit undoUnit = (IUndoUnit)obj;
				if (undoUnit is IParentUndoUnit)
				{
					((IParentUndoUnit)undoUnit).OnNextAdd();
				}
			}
		}

		// Token: 0x060071AA RID: 29098 RVA: 0x00207D0C File Offset: 0x00205F0C
		public virtual void OnNextDiscard()
		{
			this._locked = false;
			IParentUndoUnit parentUndoUnit = this;
			foreach (object obj in this._units)
			{
				IUndoUnit undoUnit = (IUndoUnit)obj;
				if (undoUnit is IParentUndoUnit)
				{
					parentUndoUnit = (undoUnit as IParentUndoUnit);
				}
			}
			if (parentUndoUnit != this)
			{
				parentUndoUnit.OnNextDiscard();
			}
		}

		// Token: 0x060071AB RID: 29099 RVA: 0x00207D80 File Offset: 0x00205F80
		public virtual void Do()
		{
			IParentUndoUnit unit = this.CreateParentUndoUnitForSelf();
			UndoManager undoManager = this.TopContainer as UndoManager;
			if (undoManager != null && undoManager.IsEnabled)
			{
				undoManager.Open(unit);
			}
			while (this._units.Count > 0)
			{
				IUndoUnit undoUnit = this._units.Pop() as IUndoUnit;
				undoUnit.Do();
			}
			if (undoManager != null && undoManager.IsEnabled)
			{
				undoManager.Close(unit, UndoCloseAction.Commit);
			}
		}

		// Token: 0x060071AC RID: 29100 RVA: 0x000D4A23 File Offset: 0x000D2C23
		public virtual bool Merge(IUndoUnit unit)
		{
			Invariant.Assert(unit != null);
			return false;
		}

		// Token: 0x17001B07 RID: 6919
		// (get) Token: 0x060071AD RID: 29101 RVA: 0x00207DEC File Offset: 0x00205FEC
		// (set) Token: 0x060071AE RID: 29102 RVA: 0x00207DF4 File Offset: 0x00205FF4
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this._description = value;
			}
		}

		// Token: 0x17001B08 RID: 6920
		// (get) Token: 0x060071AF RID: 29103 RVA: 0x00207E07 File Offset: 0x00206007
		public IParentUndoUnit OpenedUnit
		{
			get
			{
				return this._openedUnit;
			}
		}

		// Token: 0x17001B09 RID: 6921
		// (get) Token: 0x060071B0 RID: 29104 RVA: 0x00207E0F File Offset: 0x0020600F
		public IUndoUnit LastUnit
		{
			get
			{
				return this._lastUnit;
			}
		}

		// Token: 0x17001B0A RID: 6922
		// (get) Token: 0x060071B1 RID: 29105 RVA: 0x00207E17 File Offset: 0x00206017
		// (set) Token: 0x060071B2 RID: 29106 RVA: 0x00207E1F File Offset: 0x0020601F
		public virtual bool Locked
		{
			get
			{
				return this._locked;
			}
			protected set
			{
				this._locked = value;
			}
		}

		// Token: 0x17001B0B RID: 6923
		// (get) Token: 0x060071B3 RID: 29107 RVA: 0x00207E28 File Offset: 0x00206028
		// (set) Token: 0x060071B4 RID: 29108 RVA: 0x00207E30 File Offset: 0x00206030
		public object Container
		{
			get
			{
				return this._container;
			}
			set
			{
				if (!(value is IParentUndoUnit) && !(value is UndoManager))
				{
					throw new Exception(SR.Get("UndoContainerTypeMismatch"));
				}
				this._container = value;
			}
		}

		// Token: 0x060071B5 RID: 29109 RVA: 0x00207E59 File Offset: 0x00206059
		protected void Init(string description)
		{
			if (description == null)
			{
				description = string.Empty;
			}
			this._description = description;
			this._locked = false;
			this._openedUnit = null;
			this._units = new Stack(2);
			this._container = null;
		}

		// Token: 0x060071B6 RID: 29110 RVA: 0x00207E8D File Offset: 0x0020608D
		protected void SetOpenedUnit(IParentUndoUnit value)
		{
			this._openedUnit = value;
		}

		// Token: 0x060071B7 RID: 29111 RVA: 0x00207E96 File Offset: 0x00206096
		protected void SetLastUnit(IUndoUnit value)
		{
			this._lastUnit = value;
		}

		// Token: 0x060071B8 RID: 29112 RVA: 0x00207E9F File Offset: 0x0020609F
		protected virtual IParentUndoUnit CreateParentUndoUnitForSelf()
		{
			return new ParentUndoUnit(this.Description);
		}

		// Token: 0x17001B0C RID: 6924
		// (get) Token: 0x060071B9 RID: 29113 RVA: 0x00207EAC File Offset: 0x002060AC
		protected IParentUndoUnit DeepestOpenUnit
		{
			get
			{
				IParentUndoUnit openedUnit = this._openedUnit;
				if (openedUnit != null)
				{
					while (openedUnit.OpenedUnit != null)
					{
						openedUnit = openedUnit.OpenedUnit;
					}
				}
				return openedUnit;
			}
		}

		// Token: 0x17001B0D RID: 6925
		// (get) Token: 0x060071BA RID: 29114 RVA: 0x00207ED8 File Offset: 0x002060D8
		protected object TopContainer
		{
			get
			{
				object obj = this;
				while (obj is IParentUndoUnit && ((IParentUndoUnit)obj).Container != null)
				{
					obj = ((IParentUndoUnit)obj).Container;
				}
				return obj;
			}
		}

		// Token: 0x17001B0E RID: 6926
		// (get) Token: 0x060071BB RID: 29115 RVA: 0x00207F0B File Offset: 0x0020610B
		protected Stack Units
		{
			get
			{
				return this._units;
			}
		}

		// Token: 0x060071BC RID: 29116 RVA: 0x00207F14 File Offset: 0x00206114
		private bool IsInParentUnitChain(IUndoUnit unit)
		{
			if (unit is IParentUndoUnit)
			{
				IParentUndoUnit parentUndoUnit = this;
				while (parentUndoUnit != unit)
				{
					parentUndoUnit = (parentUndoUnit.Container as IParentUndoUnit);
					if (parentUndoUnit == null)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x04003731 RID: 14129
		private string _description;

		// Token: 0x04003732 RID: 14130
		private bool _locked;

		// Token: 0x04003733 RID: 14131
		private IParentUndoUnit _openedUnit;

		// Token: 0x04003734 RID: 14132
		private IUndoUnit _lastUnit;

		// Token: 0x04003735 RID: 14133
		private Stack _units;

		// Token: 0x04003736 RID: 14134
		private object _container;
	}
}
