using System;
using System.ComponentModel;
using System.IO;
using System.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Represents a lightweight audio playback <see cref="T:System.Windows.TriggerAction" /> used to play .wav files.</summary>
	// Token: 0x02000533 RID: 1331
	public class SoundPlayerAction : TriggerAction, IDisposable
	{
		/// <summary>Releases the resources used by the <see cref="T:System.Windows.Controls.SoundPlayerAction" /> class.</summary>
		// Token: 0x0600564C RID: 22092 RVA: 0x0017E58A File Offset: 0x0017C78A
		public void Dispose()
		{
			if (this.m_player != null)
			{
				this.m_player.Dispose();
			}
		}

		/// <summary>Gets or sets the audio source location. </summary>
		/// <returns>The audio source location.</returns>
		// Token: 0x170014FC RID: 5372
		// (get) Token: 0x0600564D RID: 22093 RVA: 0x0017E59F File Offset: 0x0017C79F
		// (set) Token: 0x0600564E RID: 22094 RVA: 0x0017E5B1 File Offset: 0x0017C7B1
		public Uri Source
		{
			get
			{
				return (Uri)base.GetValue(SoundPlayerAction.SourceProperty);
			}
			set
			{
				base.SetValue(SoundPlayerAction.SourceProperty, value);
			}
		}

		// Token: 0x0600564F RID: 22095 RVA: 0x0017E5C0 File Offset: 0x0017C7C0
		private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SoundPlayerAction soundPlayerAction = (SoundPlayerAction)d;
			soundPlayerAction.OnSourceChangedHelper((Uri)e.NewValue);
		}

		// Token: 0x06005650 RID: 22096 RVA: 0x0017E5E8 File Offset: 0x0017C7E8
		private void OnSourceChangedHelper(Uri newValue)
		{
			if (newValue == null || newValue.IsAbsoluteUri)
			{
				this.m_lastRequestedAbsoluteUri = newValue;
			}
			else
			{
				this.m_lastRequestedAbsoluteUri = BaseUriHelper.GetResolvedUri(BaseUriHelper.BaseUri, newValue);
			}
			this.m_player = null;
			this.m_playRequested = false;
			if (this.m_streamLoadInProgress)
			{
				this.m_uriChangedWhileLoadingStream = true;
				return;
			}
			this.BeginLoadStream();
		}

		// Token: 0x06005651 RID: 22097 RVA: 0x0017E644 File Offset: 0x0017C844
		internal sealed override void Invoke(FrameworkElement el, FrameworkContentElement ctntEl, Style targetStyle, FrameworkTemplate targetTemplate, long layer)
		{
			this.PlayWhenLoaded();
		}

		// Token: 0x06005652 RID: 22098 RVA: 0x0017E644 File Offset: 0x0017C844
		internal sealed override void Invoke(FrameworkElement el)
		{
			this.PlayWhenLoaded();
		}

		// Token: 0x06005653 RID: 22099 RVA: 0x0017E64C File Offset: 0x0017C84C
		private void PlayWhenLoaded()
		{
			if (this.m_streamLoadInProgress)
			{
				this.m_playRequested = true;
				return;
			}
			if (this.m_player != null)
			{
				this.m_player.Play();
			}
		}

		// Token: 0x06005654 RID: 22100 RVA: 0x0017E674 File Offset: 0x0017C874
		private void BeginLoadStream()
		{
			if (this.m_lastRequestedAbsoluteUri != null)
			{
				this.m_streamLoadInProgress = true;
				SoundPlayerAction.LoadStreamCaller loadStreamCaller = new SoundPlayerAction.LoadStreamCaller(this.LoadStreamAsync);
				IAsyncResult asyncResult = loadStreamCaller.BeginInvoke(this.m_lastRequestedAbsoluteUri, new AsyncCallback(this.LoadStreamCallback), loadStreamCaller);
			}
		}

		// Token: 0x06005655 RID: 22101 RVA: 0x0017E6BD File Offset: 0x0017C8BD
		private Stream LoadStreamAsync(Uri uri)
		{
			return WpfWebRequestHelper.CreateRequestAndGetResponseStream(uri);
		}

		// Token: 0x06005656 RID: 22102 RVA: 0x0017E6C8 File Offset: 0x0017C8C8
		private void LoadStreamCallback(IAsyncResult asyncResult)
		{
			DispatcherOperationCallback method = new DispatcherOperationCallback(this.OnLoadStreamCompleted);
			base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, method, asyncResult);
		}

		// Token: 0x06005657 RID: 22103 RVA: 0x0017E6F4 File Offset: 0x0017C8F4
		private object OnLoadStreamCompleted(object asyncResultArg)
		{
			IAsyncResult asyncResult = (IAsyncResult)asyncResultArg;
			SoundPlayerAction.LoadStreamCaller loadStreamCaller = (SoundPlayerAction.LoadStreamCaller)asyncResult.AsyncState;
			Stream stream = loadStreamCaller.EndInvoke(asyncResult);
			if (this.m_uriChangedWhileLoadingStream)
			{
				this.m_uriChangedWhileLoadingStream = false;
				if (stream != null)
				{
					stream.Dispose();
				}
				this.BeginLoadStream();
			}
			else if (stream != null)
			{
				if (this.m_player == null)
				{
					this.m_player = new SoundPlayer(stream);
				}
				else
				{
					this.m_player.Stream = stream;
				}
				this.m_player.LoadCompleted += this.OnSoundPlayerLoadCompleted;
				this.m_player.LoadAsync();
			}
			return null;
		}

		// Token: 0x06005658 RID: 22104 RVA: 0x0017E784 File Offset: 0x0017C984
		private void OnSoundPlayerLoadCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (this.m_player == sender)
			{
				if (this.m_uriChangedWhileLoadingStream)
				{
					this.m_player = null;
					this.m_uriChangedWhileLoadingStream = false;
					this.BeginLoadStream();
					return;
				}
				this.m_streamLoadInProgress = false;
				if (this.m_playRequested)
				{
					this.m_playRequested = false;
					this.m_player.Play();
				}
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.SoundPlayerAction.Source" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.SoundPlayerAction.Source" /> dependency property.</returns>
		// Token: 0x04002E3C RID: 11836
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(SoundPlayerAction), new FrameworkPropertyMetadata(new PropertyChangedCallback(SoundPlayerAction.OnSourceChanged)));

		// Token: 0x04002E3D RID: 11837
		private SoundPlayer m_player;

		// Token: 0x04002E3E RID: 11838
		private Uri m_lastRequestedAbsoluteUri;

		// Token: 0x04002E3F RID: 11839
		private bool m_streamLoadInProgress;

		// Token: 0x04002E40 RID: 11840
		private bool m_playRequested;

		// Token: 0x04002E41 RID: 11841
		private bool m_uriChangedWhileLoadingStream;

		// Token: 0x020009BA RID: 2490
		// (Invoke) Token: 0x0600886F RID: 34927
		private delegate Stream LoadStreamCaller(Uri uri);
	}
}
