using System;
using System.IO.Packaging;
using System.Security;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x0200046F RID: 1135
	internal class AVElementHelper
	{
		// Token: 0x06004230 RID: 16944 RVA: 0x0012EB6C File Offset: 0x0012CD6C
		internal AVElementHelper(MediaElement element)
		{
			this._element = element;
			this._position = new SettableState<TimeSpan>(new TimeSpan(0L));
			this._mediaState = new SettableState<MediaState>(MediaState.Close);
			this._source = new SettableState<Uri>(null);
			this._clock = new SettableState<MediaClock>(null);
			this._speedRatio = new SettableState<double>(1.0);
			this._volume = new SettableState<double>(0.5);
			this._isMuted = new SettableState<bool>(false);
			this._balance = new SettableState<double>(0.0);
			this._isScrubbingEnabled = new SettableState<bool>(false);
			this._mediaPlayer = new MediaPlayer();
			this.HookEvents();
		}

		// Token: 0x06004231 RID: 16945 RVA: 0x0012EC38 File Offset: 0x0012CE38
		internal static AVElementHelper GetHelper(DependencyObject d)
		{
			MediaElement mediaElement = d as MediaElement;
			if (mediaElement != null)
			{
				return mediaElement.Helper;
			}
			throw new ArgumentException(SR.Get("AudioVideo_InvalidDependencyObject"));
		}

		// Token: 0x17001049 RID: 4169
		// (get) Token: 0x06004232 RID: 16946 RVA: 0x0012EC65 File Offset: 0x0012CE65
		internal MediaPlayer Player
		{
			get
			{
				return this._mediaPlayer;
			}
		}

		// Token: 0x1700104A RID: 4170
		// (get) Token: 0x06004233 RID: 16947 RVA: 0x0012EC6D File Offset: 0x0012CE6D
		// (set) Token: 0x06004234 RID: 16948 RVA: 0x0012EC75 File Offset: 0x0012CE75
		internal Uri BaseUri
		{
			get
			{
				return this._baseUri;
			}
			set
			{
				if (value.Scheme != PackUriHelper.UriSchemePack)
				{
					this._baseUri = value;
					return;
				}
				this._baseUri = null;
			}
		}

		// Token: 0x06004235 RID: 16949 RVA: 0x0012EC98 File Offset: 0x0012CE98
		internal void SetUnloadedBehavior(MediaState unloadedBehavior)
		{
			this._unloadedBehavior = unloadedBehavior;
			this.HandleStateChange();
		}

		// Token: 0x06004236 RID: 16950 RVA: 0x0012ECA7 File Offset: 0x0012CEA7
		internal void SetLoadedBehavior(MediaState loadedBehavior)
		{
			this._loadedBehavior = loadedBehavior;
			this.HandleStateChange();
		}

		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x06004237 RID: 16951 RVA: 0x0012ECB6 File Offset: 0x0012CEB6
		internal TimeSpan Position
		{
			get
			{
				if (this._currentState == MediaState.Close)
				{
					return this._position._value;
				}
				return this._mediaPlayer.Position;
			}
		}

		// Token: 0x06004238 RID: 16952 RVA: 0x0012ECD8 File Offset: 0x0012CED8
		internal void SetPosition(TimeSpan position)
		{
			this._position._isSet = true;
			this._position._value = position;
			this.HandleStateChange();
		}

		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x06004239 RID: 16953 RVA: 0x0012ECF8 File Offset: 0x0012CEF8
		internal MediaClock Clock
		{
			get
			{
				return this._clock._value;
			}
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x0012ED05 File Offset: 0x0012CF05
		internal void SetClock(MediaClock clock)
		{
			this._clock._value = clock;
			this._clock._isSet = true;
			this.HandleStateChange();
		}

		// Token: 0x1700104D RID: 4173
		// (get) Token: 0x0600423B RID: 16955 RVA: 0x0012ED25 File Offset: 0x0012CF25
		internal double SpeedRatio
		{
			get
			{
				return this._speedRatio._value;
			}
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x0012ED34 File Offset: 0x0012CF34
		internal void SetSpeedRatio(double speedRatio)
		{
			this._speedRatio._wasSet = (this._speedRatio._isSet = true);
			this._speedRatio._value = speedRatio;
			this.HandleStateChange();
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x0012ED6D File Offset: 0x0012CF6D
		internal void SetState(MediaState mediaState)
		{
			if (this._loadedBehavior != MediaState.Manual && this._unloadedBehavior != MediaState.Manual)
			{
				throw new NotSupportedException(SR.Get("AudioVideo_CannotControlMedia"));
			}
			this._mediaState._value = mediaState;
			this._mediaState._isSet = true;
			this.HandleStateChange();
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x0012EDB0 File Offset: 0x0012CFB0
		internal void SetVolume(double volume)
		{
			this._volume._wasSet = (this._volume._isSet = true);
			this._volume._value = volume;
			this.HandleStateChange();
		}

		// Token: 0x0600423F RID: 16959 RVA: 0x0012EDEC File Offset: 0x0012CFEC
		internal void SetBalance(double balance)
		{
			this._balance._wasSet = (this._balance._isSet = true);
			this._balance._value = balance;
			this.HandleStateChange();
		}

		// Token: 0x06004240 RID: 16960 RVA: 0x0012EE28 File Offset: 0x0012D028
		internal void SetIsMuted(bool isMuted)
		{
			this._isMuted._wasSet = (this._isMuted._isSet = true);
			this._isMuted._value = isMuted;
			this.HandleStateChange();
		}

		// Token: 0x06004241 RID: 16961 RVA: 0x0012EE64 File Offset: 0x0012D064
		internal void SetScrubbingEnabled(bool isScrubbingEnabled)
		{
			this._isScrubbingEnabled._wasSet = (this._isScrubbingEnabled._isSet = true);
			this._isScrubbingEnabled._value = isScrubbingEnabled;
			this.HandleStateChange();
		}

		// Token: 0x06004242 RID: 16962 RVA: 0x0012EEA0 File Offset: 0x0012D0A0
		private void HookEvents()
		{
			this._mediaPlayer.MediaOpened += this.OnMediaOpened;
			this._mediaPlayer.MediaFailed += this.OnMediaFailed;
			this._mediaPlayer.BufferingStarted += this.OnBufferingStarted;
			this._mediaPlayer.BufferingEnded += this.OnBufferingEnded;
			this._mediaPlayer.MediaEnded += this.OnMediaEnded;
			this._mediaPlayer.ScriptCommand += this.OnScriptCommand;
			this._element.Loaded += this.OnLoaded;
			this._element.Unloaded += this.OnUnloaded;
		}

		// Token: 0x06004243 RID: 16963 RVA: 0x0012EF68 File Offset: 0x0012D168
		private void HandleStateChange()
		{
			MediaState mediaState = this._mediaState._value;
			bool flag = false;
			bool flag2 = false;
			if (this._isLoaded)
			{
				if (this._clock._value != null)
				{
					mediaState = MediaState.Manual;
					flag = true;
				}
				else if (this._loadedBehavior != MediaState.Manual)
				{
					mediaState = this._loadedBehavior;
				}
				else if (this._source._wasSet)
				{
					if (this._loadedBehavior != MediaState.Manual)
					{
						mediaState = MediaState.Play;
					}
					else
					{
						flag2 = true;
					}
				}
			}
			else if (this._unloadedBehavior != MediaState.Manual)
			{
				mediaState = this._unloadedBehavior;
			}
			else
			{
				Invariant.Assert(this._unloadedBehavior == MediaState.Manual);
				if (this._clock._value != null)
				{
					mediaState = MediaState.Manual;
					flag = true;
				}
				else
				{
					flag2 = true;
				}
			}
			bool flag3 = false;
			if (mediaState != MediaState.Close && mediaState != MediaState.Manual)
			{
				Invariant.Assert(!flag);
				if (this._mediaPlayer.Clock != null)
				{
					this._mediaPlayer.Clock = null;
				}
				if (this._currentState == MediaState.Close || this._source._isSet)
				{
					if (this._isScrubbingEnabled._wasSet)
					{
						this._mediaPlayer.ScrubbingEnabled = this._isScrubbingEnabled._value;
						this._isScrubbingEnabled._isSet = false;
					}
					if (this._clock._value == null)
					{
						this._mediaPlayer.Open(this.UriFromSourceUri(this._source._value));
					}
					flag3 = true;
				}
			}
			else if (flag)
			{
				if (this._currentState == MediaState.Close || this._clock._isSet)
				{
					if (this._isScrubbingEnabled._wasSet)
					{
						this._mediaPlayer.ScrubbingEnabled = this._isScrubbingEnabled._value;
						this._isScrubbingEnabled._isSet = false;
					}
					this._mediaPlayer.Clock = this._clock._value;
					this._clock._isSet = false;
					flag3 = true;
				}
			}
			else if (mediaState == MediaState.Close && this._currentState != MediaState.Close)
			{
				this._mediaPlayer.Clock = null;
				this._mediaPlayer.Close();
				this._currentState = MediaState.Close;
			}
			if (this._currentState != MediaState.Close || flag3)
			{
				if (this._position._isSet)
				{
					this._mediaPlayer.Position = this._position._value;
					this._position._isSet = false;
				}
				if (this._volume._isSet || (flag3 && this._volume._wasSet))
				{
					this._mediaPlayer.Volume = this._volume._value;
					this._volume._isSet = false;
				}
				if (this._balance._isSet || (flag3 && this._balance._wasSet))
				{
					this._mediaPlayer.Balance = this._balance._value;
					this._balance._isSet = false;
				}
				if (this._isMuted._isSet || (flag3 && this._isMuted._wasSet))
				{
					this._mediaPlayer.IsMuted = this._isMuted._value;
					this._isMuted._isSet = false;
				}
				if (this._isScrubbingEnabled._isSet)
				{
					this._mediaPlayer.ScrubbingEnabled = this._isScrubbingEnabled._value;
					this._isScrubbingEnabled._isSet = false;
				}
				if (mediaState == MediaState.Play && this._source._isSet)
				{
					this._mediaPlayer.Play();
					if (!this._speedRatio._wasSet)
					{
						this._mediaPlayer.SpeedRatio = 1.0;
					}
					this._source._isSet = false;
					this._mediaState._isSet = false;
				}
				else if (this._currentState != mediaState || (flag2 && this._mediaState._isSet))
				{
					switch (mediaState)
					{
					case MediaState.Manual:
						goto IL_3BE;
					case MediaState.Play:
						this._mediaPlayer.Play();
						goto IL_3BE;
					case MediaState.Pause:
						this._mediaPlayer.Pause();
						goto IL_3BE;
					case MediaState.Stop:
						this._mediaPlayer.Stop();
						goto IL_3BE;
					}
					Invariant.Assert(false, "Unexpected state request.");
					IL_3BE:
					if (flag2)
					{
						this._mediaState._isSet = false;
					}
				}
				this._currentState = mediaState;
				if (this._speedRatio._isSet || (flag3 && this._speedRatio._wasSet))
				{
					this._mediaPlayer.SpeedRatio = this._speedRatio._value;
					this._speedRatio._isSet = false;
				}
			}
		}

		// Token: 0x06004244 RID: 16964 RVA: 0x0012F388 File Offset: 0x0012D588
		private Uri UriFromSourceUri(Uri sourceUri)
		{
			if (sourceUri != null)
			{
				if (sourceUri.IsAbsoluteUri)
				{
					return sourceUri;
				}
				if (this.BaseUri != null)
				{
					return new Uri(this.BaseUri, sourceUri);
				}
			}
			return sourceUri;
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x0012F3BC File Offset: 0x0012D5BC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			AVElementHelper helper = AVElementHelper.GetHelper(d);
			helper.MemberOnInvalidateSource(e);
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x0012F3E4 File Offset: 0x0012D5E4
		private void MemberOnInvalidateSource(DependencyPropertyChangedEventArgs e)
		{
			if (this._clock._value != null)
			{
				throw new InvalidOperationException(SR.Get("MediaElement_CannotSetSourceOnMediaElementDrivenByClock"));
			}
			this._source._value = (Uri)e.NewValue;
			this._source._wasSet = (this._source._isSet = true);
			this.HandleStateChange();
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x0012F445 File Offset: 0x0012D645
		private void OnMediaFailed(object sender, ExceptionEventArgs args)
		{
			this._element.OnMediaFailed(sender, args);
		}

		// Token: 0x06004248 RID: 16968 RVA: 0x0012F454 File Offset: 0x0012D654
		private void OnMediaOpened(object sender, EventArgs args)
		{
			this._element.InvalidateMeasure();
			this._element.OnMediaOpened(sender, args);
		}

		// Token: 0x06004249 RID: 16969 RVA: 0x0012F46E File Offset: 0x0012D66E
		private void OnBufferingStarted(object sender, EventArgs args)
		{
			this._element.OnBufferingStarted(sender, args);
		}

		// Token: 0x0600424A RID: 16970 RVA: 0x0012F47D File Offset: 0x0012D67D
		private void OnBufferingEnded(object sender, EventArgs args)
		{
			this._element.OnBufferingEnded(sender, args);
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x0012F48C File Offset: 0x0012D68C
		private void OnMediaEnded(object sender, EventArgs args)
		{
			this._element.OnMediaEnded(sender, args);
		}

		// Token: 0x0600424C RID: 16972 RVA: 0x0012F49B File Offset: 0x0012D69B
		private void OnScriptCommand(object sender, MediaScriptCommandEventArgs args)
		{
			this._element.OnScriptCommand(sender, args);
		}

		// Token: 0x0600424D RID: 16973 RVA: 0x0012F4AA File Offset: 0x0012D6AA
		private void OnLoaded(object sender, RoutedEventArgs args)
		{
			this._isLoaded = true;
			this.HandleStateChange();
		}

		// Token: 0x0600424E RID: 16974 RVA: 0x0012F4B9 File Offset: 0x0012D6B9
		private void OnUnloaded(object sender, RoutedEventArgs args)
		{
			this._isLoaded = false;
			this.HandleStateChange();
		}

		// Token: 0x040027DA RID: 10202
		private MediaPlayer _mediaPlayer;

		// Token: 0x040027DB RID: 10203
		private MediaElement _element;

		// Token: 0x040027DC RID: 10204
		private Uri _baseUri;

		// Token: 0x040027DD RID: 10205
		private MediaState _unloadedBehavior = MediaState.Close;

		// Token: 0x040027DE RID: 10206
		private MediaState _loadedBehavior = MediaState.Play;

		// Token: 0x040027DF RID: 10207
		private MediaState _currentState = MediaState.Close;

		// Token: 0x040027E0 RID: 10208
		private bool _isLoaded;

		// Token: 0x040027E1 RID: 10209
		private SettableState<TimeSpan> _position;

		// Token: 0x040027E2 RID: 10210
		private SettableState<MediaState> _mediaState;

		// Token: 0x040027E3 RID: 10211
		private SettableState<Uri> _source;

		// Token: 0x040027E4 RID: 10212
		private SettableState<MediaClock> _clock;

		// Token: 0x040027E5 RID: 10213
		private SettableState<double> _speedRatio;

		// Token: 0x040027E6 RID: 10214
		private SettableState<double> _volume;

		// Token: 0x040027E7 RID: 10215
		private SettableState<bool> _isMuted;

		// Token: 0x040027E8 RID: 10216
		private SettableState<double> _balance;

		// Token: 0x040027E9 RID: 10217
		private SettableState<bool> _isScrubbingEnabled;
	}
}
