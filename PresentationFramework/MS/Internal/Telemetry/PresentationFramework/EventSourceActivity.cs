using System;
using System.Diagnostics.Contracts;
using System.Diagnostics.Tracing;

namespace MS.Internal.Telemetry.PresentationFramework
{
	// Token: 0x020007E7 RID: 2023
	internal sealed class EventSourceActivity : IDisposable
	{
		// Token: 0x06007D08 RID: 32008 RVA: 0x00232930 File Offset: 0x00230B30
		internal EventSourceActivity(EventSource eventSource) : this(eventSource, default(EventSourceOptions))
		{
		}

		// Token: 0x06007D09 RID: 32009 RVA: 0x0023294D File Offset: 0x00230B4D
		internal EventSourceActivity(EventSource eventSource, EventSourceOptions startStopOptions) : this(eventSource, startStopOptions, Guid.Empty)
		{
		}

		// Token: 0x06007D0A RID: 32010 RVA: 0x0023295C File Offset: 0x00230B5C
		internal EventSourceActivity(EventSource eventSource, EventSourceOptions startStopOptions, Guid parentActivityId)
		{
			this._id = Guid.NewGuid();
			base..ctor();
			Contract.Requires<ArgumentNullException>(eventSource != null, "eventSource");
			this._eventSource = eventSource;
			this._startStopOptions = startStopOptions;
			this._parentId = parentActivityId;
		}

		// Token: 0x06007D0B RID: 32011 RVA: 0x00232994 File Offset: 0x00230B94
		internal EventSourceActivity(EventSourceActivity parentActivity) : this(parentActivity, default(EventSourceOptions))
		{
		}

		// Token: 0x06007D0C RID: 32012 RVA: 0x002329B1 File Offset: 0x00230BB1
		internal EventSourceActivity(EventSourceActivity parentActivity, EventSourceOptions startStopOptions)
		{
			this._id = Guid.NewGuid();
			base..ctor();
			Contract.Requires<ArgumentNullException>(parentActivity != null, "parentActivity");
			this._eventSource = parentActivity.EventSource;
			this._startStopOptions = startStopOptions;
			this._parentId = parentActivity.Id;
		}

		// Token: 0x17001D15 RID: 7445
		// (get) Token: 0x06007D0D RID: 32013 RVA: 0x002329F1 File Offset: 0x00230BF1
		internal EventSource EventSource
		{
			get
			{
				return this._eventSource;
			}
		}

		// Token: 0x17001D16 RID: 7446
		// (get) Token: 0x06007D0E RID: 32014 RVA: 0x002329F9 File Offset: 0x00230BF9
		internal Guid Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x06007D0F RID: 32015 RVA: 0x00232A04 File Offset: 0x00230C04
		internal void Start(string eventName)
		{
			Contract.Requires<ArgumentNullException>(eventName != null, "eventName");
			EventSourceActivity.EmptyStruct instance = EventSourceActivity.EmptyStruct.Instance;
			this.Start<EventSourceActivity.EmptyStruct>(eventName, ref instance);
		}

		// Token: 0x06007D10 RID: 32016 RVA: 0x00232A2E File Offset: 0x00230C2E
		internal void Start<T>(string eventName, T data)
		{
			this.Start<T>(eventName, ref data);
		}

		// Token: 0x06007D11 RID: 32017 RVA: 0x00232A3C File Offset: 0x00230C3C
		internal void Stop(string eventName)
		{
			Contract.Requires<ArgumentNullException>(eventName != null, "eventName");
			EventSourceActivity.EmptyStruct instance = EventSourceActivity.EmptyStruct.Instance;
			this.Stop<EventSourceActivity.EmptyStruct>(eventName, ref instance);
		}

		// Token: 0x06007D12 RID: 32018 RVA: 0x00232A66 File Offset: 0x00230C66
		internal void Stop<T>(string eventName, T data)
		{
			this.Stop<T>(eventName, ref data);
		}

		// Token: 0x06007D13 RID: 32019 RVA: 0x00232A74 File Offset: 0x00230C74
		internal void Write(string eventName)
		{
			Contract.Requires<ArgumentNullException>(eventName != null, "eventName");
			EventSourceOptions eventSourceOptions = default(EventSourceOptions);
			EventSourceActivity.EmptyStruct instance = EventSourceActivity.EmptyStruct.Instance;
			this.Write<EventSourceActivity.EmptyStruct>(eventName, ref eventSourceOptions, ref instance);
		}

		// Token: 0x06007D14 RID: 32020 RVA: 0x00232AA8 File Offset: 0x00230CA8
		internal void Write(string eventName, EventSourceOptions options)
		{
			Contract.Requires<ArgumentNullException>(eventName != null, "eventName");
			EventSourceActivity.EmptyStruct instance = EventSourceActivity.EmptyStruct.Instance;
			this.Write<EventSourceActivity.EmptyStruct>(eventName, ref options, ref instance);
		}

		// Token: 0x06007D15 RID: 32021 RVA: 0x00232AD4 File Offset: 0x00230CD4
		internal void Write<T>(string eventName, T data)
		{
			EventSourceOptions eventSourceOptions = default(EventSourceOptions);
			this.Write<T>(eventName, ref eventSourceOptions, ref data);
		}

		// Token: 0x06007D16 RID: 32022 RVA: 0x00232AF4 File Offset: 0x00230CF4
		internal void Write<T>(string eventName, EventSourceOptions options, T data)
		{
			this.Write<T>(eventName, ref options, ref data);
		}

		// Token: 0x06007D17 RID: 32023 RVA: 0x00232B04 File Offset: 0x00230D04
		public void Dispose()
		{
			if (this._state == EventSourceActivity.State.Started)
			{
				this._state = EventSourceActivity.State.Stopped;
				EventSourceActivity.EmptyStruct instance = EventSourceActivity.EmptyStruct.Instance;
				this._eventSource.Write<EventSourceActivity.EmptyStruct>("Dispose", ref this._startStopOptions, ref this._id, ref EventSourceActivity._emptyGuid, ref instance);
			}
		}

		// Token: 0x06007D18 RID: 32024 RVA: 0x00232B4C File Offset: 0x00230D4C
		private void Start<T>(string eventName, ref T data)
		{
			if (this._state != EventSourceActivity.State.Initialized)
			{
				throw new InvalidOperationException();
			}
			this._state = EventSourceActivity.State.Started;
			this._startStopOptions.Opcode = EventOpcode.Start;
			this._eventSource.Write<T>(eventName, ref this._startStopOptions, ref this._id, ref this._parentId, ref data);
			this._startStopOptions.Opcode = EventOpcode.Stop;
		}

		// Token: 0x06007D19 RID: 32025 RVA: 0x00232BA5 File Offset: 0x00230DA5
		private void Write<T>(string eventName, ref EventSourceOptions options, ref T data)
		{
			if (this._state != EventSourceActivity.State.Started)
			{
				throw new InvalidOperationException();
			}
			this._eventSource.Write<T>(eventName, ref options, ref this._id, ref EventSourceActivity._emptyGuid, ref data);
		}

		// Token: 0x06007D1A RID: 32026 RVA: 0x00232BCF File Offset: 0x00230DCF
		private void Stop<T>(string eventName, ref T data)
		{
			if (this._state != EventSourceActivity.State.Started)
			{
				throw new InvalidOperationException();
			}
			this._state = EventSourceActivity.State.Stopped;
			this._eventSource.Write<T>(eventName, ref this._startStopOptions, ref this._id, ref EventSourceActivity._emptyGuid, ref data);
		}

		// Token: 0x04003A9A RID: 15002
		private static Guid _emptyGuid;

		// Token: 0x04003A9B RID: 15003
		private readonly EventSource _eventSource;

		// Token: 0x04003A9C RID: 15004
		private EventSourceOptions _startStopOptions;

		// Token: 0x04003A9D RID: 15005
		private Guid _parentId;

		// Token: 0x04003A9E RID: 15006
		private Guid _id;

		// Token: 0x04003A9F RID: 15007
		private EventSourceActivity.State _state;

		// Token: 0x02000B86 RID: 2950
		private enum State
		{
			// Token: 0x04004B92 RID: 19346
			Initialized,
			// Token: 0x04004B93 RID: 19347
			Started,
			// Token: 0x04004B94 RID: 19348
			Stopped
		}

		// Token: 0x02000B87 RID: 2951
		[EventData]
		private class EmptyStruct
		{
			// Token: 0x06008E6D RID: 36461 RVA: 0x0000326D File Offset: 0x0000146D
			private EmptyStruct()
			{
			}

			// Token: 0x17001FB4 RID: 8116
			// (get) Token: 0x06008E6E RID: 36462 RVA: 0x0025C3CF File Offset: 0x0025A5CF
			internal static EventSourceActivity.EmptyStruct Instance
			{
				get
				{
					if (EventSourceActivity.EmptyStruct._instance == null)
					{
						EventSourceActivity.EmptyStruct._instance = new EventSourceActivity.EmptyStruct();
					}
					return EventSourceActivity.EmptyStruct._instance;
				}
			}

			// Token: 0x04004B95 RID: 19349
			private static EventSourceActivity.EmptyStruct _instance;
		}
	}
}
