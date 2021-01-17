using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a control that contains audio and/or video. </summary>
	// Token: 0x02000500 RID: 1280
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class MediaElement : FrameworkElement, IUriContext
	{
		/// <summary>Instantiates a new instance of the <see cref="T:System.Windows.Controls.MediaElement" /> class.</summary>
		// Token: 0x06005175 RID: 20853 RVA: 0x0016D44A File Offset: 0x0016B64A
		public MediaElement()
		{
			this.Initialize();
		}

		// Token: 0x06005176 RID: 20854 RVA: 0x0016D458 File Offset: 0x0016B658
		static MediaElement()
		{
			MediaElement.MediaFailedEvent = EventManager.RegisterRoutedEvent("MediaFailed", RoutingStrategy.Bubble, typeof(EventHandler<ExceptionRoutedEventArgs>), typeof(MediaElement));
			MediaElement.MediaOpenedEvent = EventManager.RegisterRoutedEvent("MediaOpened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MediaElement));
			MediaElement.BufferingStartedEvent = EventManager.RegisterRoutedEvent("BufferingStarted", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MediaElement));
			MediaElement.BufferingEndedEvent = EventManager.RegisterRoutedEvent("BufferingEnded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MediaElement));
			MediaElement.ScriptCommandEvent = EventManager.RegisterRoutedEvent("ScriptCommand", RoutingStrategy.Bubble, typeof(EventHandler<MediaScriptCommandRoutedEventArgs>), typeof(MediaElement));
			MediaElement.MediaEndedEvent = EventManager.RegisterRoutedEvent("MediaEnded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MediaElement));
			Style defaultValue = MediaElement.CreateDefaultStyles();
			FrameworkElement.StyleProperty.OverrideMetadata(typeof(MediaElement), new FrameworkPropertyMetadata(defaultValue));
			MediaElement.StretchProperty.OverrideMetadata(typeof(MediaElement), new FrameworkPropertyMetadata(Stretch.Uniform, FrameworkPropertyMetadataOptions.AffectsMeasure));
			MediaElement.StretchDirectionProperty.OverrideMetadata(typeof(MediaElement), new FrameworkPropertyMetadata(StretchDirection.Both, FrameworkPropertyMetadataOptions.AffectsMeasure));
			ControlsTraceLogger.AddControl(TelemetryControls.MediaElement);
		}

		// Token: 0x06005177 RID: 20855 RVA: 0x0016D784 File Offset: 0x0016B984
		private static Style CreateDefaultStyles()
		{
			Style style = new Style(typeof(MediaElement), null);
			style.Setters.Add(new Setter(FrameworkElement.FlowDirectionProperty, FlowDirection.LeftToRight));
			style.Seal();
			return style;
		}

		/// <summary>Gets or sets a media source on the <see cref="T:System.Windows.Controls.MediaElement" />.  </summary>
		/// <returns>The URI that specifies the source of the element. The default is <see langword="null" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Controls.MediaElement.Clock" /> property is not <see langword="null" />.</exception>
		// Token: 0x170013BE RID: 5054
		// (get) Token: 0x06005178 RID: 20856 RVA: 0x0016D7C4 File Offset: 0x0016B9C4
		// (set) Token: 0x06005179 RID: 20857 RVA: 0x0016D7D6 File Offset: 0x0016B9D6
		public Uri Source
		{
			get
			{
				return (Uri)base.GetValue(MediaElement.SourceProperty);
			}
			set
			{
				base.SetValue(MediaElement.SourceProperty, value);
			}
		}

		/// <summary>Gets or sets the clock associated with the <see cref="T:System.Windows.Media.MediaTimeline" /> that controls media playback.</summary>
		/// <returns>A clock associated with the <see cref="T:System.Windows.Media.MediaTimeline" /> that controls media playback. The default value is <see langword="null" />.</returns>
		// Token: 0x170013BF RID: 5055
		// (get) Token: 0x0600517A RID: 20858 RVA: 0x0016D7E4 File Offset: 0x0016B9E4
		// (set) Token: 0x0600517B RID: 20859 RVA: 0x0016D7F1 File Offset: 0x0016B9F1
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MediaClock Clock
		{
			get
			{
				return this._helper.Clock;
			}
			set
			{
				this._helper.SetClock(value);
			}
		}

		/// <summary>Plays media from the current position.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Controls.MediaElement.Clock" /> property is not <see langword="null" />.</exception>
		// Token: 0x0600517C RID: 20860 RVA: 0x0016D7FF File Offset: 0x0016B9FF
		public void Play()
		{
			this._helper.SetState(MediaState.Play);
		}

		/// <summary>Pauses media at the current position.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Controls.MediaElement.Clock" /> property is not <see langword="null" />.</exception>
		// Token: 0x0600517D RID: 20861 RVA: 0x0016D80D File Offset: 0x0016BA0D
		public void Pause()
		{
			this._helper.SetState(MediaState.Pause);
		}

		/// <summary>Stops and resets media to be played from the beginning.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Controls.MediaElement.Clock" /> property is not <see langword="null" />.</exception>
		// Token: 0x0600517E RID: 20862 RVA: 0x0016D81B File Offset: 0x0016BA1B
		public void Stop()
		{
			this._helper.SetState(MediaState.Stop);
		}

		/// <summary>Closes the media.</summary>
		// Token: 0x0600517F RID: 20863 RVA: 0x0016D829 File Offset: 0x0016BA29
		public void Close()
		{
			this._helper.SetState(MediaState.Close);
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.Stretch" /> value that describes how a <see cref="T:System.Windows.Controls.MediaElement" /> fills the destination rectangle.  </summary>
		/// <returns>The stretch value for the rendered media. The default is <see cref="F:System.Windows.Media.Stretch.Uniform" />.</returns>
		// Token: 0x170013C0 RID: 5056
		// (get) Token: 0x06005180 RID: 20864 RVA: 0x0016D837 File Offset: 0x0016BA37
		// (set) Token: 0x06005181 RID: 20865 RVA: 0x0016D849 File Offset: 0x0016BA49
		public Stretch Stretch
		{
			get
			{
				return (Stretch)base.GetValue(MediaElement.StretchProperty);
			}
			set
			{
				base.SetValue(MediaElement.StretchProperty, value);
			}
		}

		/// <summary>Gets or sets a value that determines the restrictions on scaling that are applied to the image.  </summary>
		/// <returns>The value that specifies the direction the element is stretched. The default is <see cref="F:System.Windows.Controls.StretchDirection.Both" />.</returns>
		// Token: 0x170013C1 RID: 5057
		// (get) Token: 0x06005182 RID: 20866 RVA: 0x0016D85C File Offset: 0x0016BA5C
		// (set) Token: 0x06005183 RID: 20867 RVA: 0x0016D86E File Offset: 0x0016BA6E
		public StretchDirection StretchDirection
		{
			get
			{
				return (StretchDirection)base.GetValue(MediaElement.StretchDirectionProperty);
			}
			set
			{
				base.SetValue(MediaElement.StretchDirectionProperty, value);
			}
		}

		/// <summary>Gets or sets the media's volume. </summary>
		/// <returns>The media's volume represented on a linear scale between 0 and 1. The default is 0.5.</returns>
		// Token: 0x170013C2 RID: 5058
		// (get) Token: 0x06005184 RID: 20868 RVA: 0x0016D881 File Offset: 0x0016BA81
		// (set) Token: 0x06005185 RID: 20869 RVA: 0x0016D893 File Offset: 0x0016BA93
		public double Volume
		{
			get
			{
				return (double)base.GetValue(MediaElement.VolumeProperty);
			}
			set
			{
				base.SetValue(MediaElement.VolumeProperty, value);
			}
		}

		/// <summary>Gets or sets a ratio of volume across speakers.  </summary>
		/// <returns>The ratio of volume across speakers in the range between -1 and 1. The default is 0.</returns>
		// Token: 0x170013C3 RID: 5059
		// (get) Token: 0x06005186 RID: 20870 RVA: 0x0016D8A6 File Offset: 0x0016BAA6
		// (set) Token: 0x06005187 RID: 20871 RVA: 0x0016D8B8 File Offset: 0x0016BAB8
		public double Balance
		{
			get
			{
				return (double)base.GetValue(MediaElement.BalanceProperty);
			}
			set
			{
				base.SetValue(MediaElement.BalanceProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the audio is muted.  </summary>
		/// <returns>
		///     <see langword="true" /> if audio is muted; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170013C4 RID: 5060
		// (get) Token: 0x06005188 RID: 20872 RVA: 0x0016D8CB File Offset: 0x0016BACB
		// (set) Token: 0x06005189 RID: 20873 RVA: 0x0016D8DD File Offset: 0x0016BADD
		public bool IsMuted
		{
			get
			{
				return (bool)base.GetValue(MediaElement.IsMutedProperty);
			}
			set
			{
				base.SetValue(MediaElement.IsMutedProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.MediaElement" /> will update frames for seek operations while paused. </summary>
		/// <returns>
		///     <see langword="true" /> if frames are updated while paused; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170013C5 RID: 5061
		// (get) Token: 0x0600518A RID: 20874 RVA: 0x0016D8EB File Offset: 0x0016BAEB
		// (set) Token: 0x0600518B RID: 20875 RVA: 0x0016D8FD File Offset: 0x0016BAFD
		public bool ScrubbingEnabled
		{
			get
			{
				return (bool)base.GetValue(MediaElement.ScrubbingEnabledProperty);
			}
			set
			{
				base.SetValue(MediaElement.ScrubbingEnabledProperty, value);
			}
		}

		/// <summary>Gets or sets the unload behavior <see cref="T:System.Windows.Controls.MediaState" /> for the media. </summary>
		/// <returns>The unload behavior <see cref="T:System.Windows.Controls.MediaState" /> for the media.</returns>
		// Token: 0x170013C6 RID: 5062
		// (get) Token: 0x0600518C RID: 20876 RVA: 0x0016D90B File Offset: 0x0016BB0B
		// (set) Token: 0x0600518D RID: 20877 RVA: 0x0016D91D File Offset: 0x0016BB1D
		public MediaState UnloadedBehavior
		{
			get
			{
				return (MediaState)base.GetValue(MediaElement.UnloadedBehaviorProperty);
			}
			set
			{
				base.SetValue(MediaElement.UnloadedBehaviorProperty, value);
			}
		}

		/// <summary>Gets or sets the load behavior <see cref="T:System.Windows.Controls.MediaState" /> for the media. </summary>
		/// <returns>The load behavior <see cref="T:System.Windows.Controls.MediaState" /> set for the media. The default value is <see cref="F:System.Windows.Controls.MediaState.Play" />.</returns>
		// Token: 0x170013C7 RID: 5063
		// (get) Token: 0x0600518E RID: 20878 RVA: 0x0016D930 File Offset: 0x0016BB30
		// (set) Token: 0x0600518F RID: 20879 RVA: 0x0016D942 File Offset: 0x0016BB42
		public MediaState LoadedBehavior
		{
			get
			{
				return (MediaState)base.GetValue(MediaElement.LoadedBehaviorProperty);
			}
			set
			{
				base.SetValue(MediaElement.LoadedBehaviorProperty, value);
			}
		}

		/// <summary>Gets a value indicating whether the media can be paused.</summary>
		/// <returns>
		///     <see langword="true" /> if the media can be paused; otherwise, <see langword="false" />.</returns>
		// Token: 0x170013C8 RID: 5064
		// (get) Token: 0x06005190 RID: 20880 RVA: 0x0016D955 File Offset: 0x0016BB55
		public bool CanPause
		{
			get
			{
				return this._helper.Player.CanPause;
			}
		}

		/// <summary>Get a value indicating whether the media is buffering.</summary>
		/// <returns>
		///     <see langword="true" /> if the media is buffering; otherwise, <see langword="false" />.</returns>
		// Token: 0x170013C9 RID: 5065
		// (get) Token: 0x06005191 RID: 20881 RVA: 0x0016D967 File Offset: 0x0016BB67
		public bool IsBuffering
		{
			get
			{
				return this._helper.Player.IsBuffering;
			}
		}

		/// <summary>Gets a percentage value indicating the amount of download completed for content located on a remote server.</summary>
		/// <returns>A percentage value indicating the amount of download completed for content located on a remote server. The value ranges from 0 to 1. The default value is 0.</returns>
		// Token: 0x170013CA RID: 5066
		// (get) Token: 0x06005192 RID: 20882 RVA: 0x0016D979 File Offset: 0x0016BB79
		public double DownloadProgress
		{
			get
			{
				return this._helper.Player.DownloadProgress;
			}
		}

		/// <summary>Gets a value that indicates the percentage of buffering progress made.</summary>
		/// <returns>The percentage of buffering completed for streaming content. The value ranges from 0 to 1.</returns>
		// Token: 0x170013CB RID: 5067
		// (get) Token: 0x06005193 RID: 20883 RVA: 0x0016D98B File Offset: 0x0016BB8B
		public double BufferingProgress
		{
			get
			{
				return this._helper.Player.BufferingProgress;
			}
		}

		/// <summary>Gets the height of the video associated with the media.</summary>
		/// <returns>The height of the video associated with the media. Audio files will return zero.</returns>
		// Token: 0x170013CC RID: 5068
		// (get) Token: 0x06005194 RID: 20884 RVA: 0x0016D99D File Offset: 0x0016BB9D
		public int NaturalVideoHeight
		{
			get
			{
				return this._helper.Player.NaturalVideoHeight;
			}
		}

		/// <summary>Gets the width of the video associated with the media.</summary>
		/// <returns>The width of the video associated with the media.</returns>
		// Token: 0x170013CD RID: 5069
		// (get) Token: 0x06005195 RID: 20885 RVA: 0x0016D9AF File Offset: 0x0016BBAF
		public int NaturalVideoWidth
		{
			get
			{
				return this._helper.Player.NaturalVideoWidth;
			}
		}

		/// <summary>Gets a value indicating whether the media has audio.</summary>
		/// <returns>
		///     <see langword="true" /> if the media has audio; otherwise, <see langword="false" />.</returns>
		// Token: 0x170013CE RID: 5070
		// (get) Token: 0x06005196 RID: 20886 RVA: 0x0016D9C1 File Offset: 0x0016BBC1
		public bool HasAudio
		{
			get
			{
				return this._helper.Player.HasAudio;
			}
		}

		/// <summary>Gets a value indicating whether the media has video.</summary>
		/// <returns>
		///     <see langword="true" /> if the media has video; otherwise, <see langword="false" />.</returns>
		// Token: 0x170013CF RID: 5071
		// (get) Token: 0x06005197 RID: 20887 RVA: 0x0016D9D3 File Offset: 0x0016BBD3
		public bool HasVideo
		{
			get
			{
				return this._helper.Player.HasVideo;
			}
		}

		/// <summary>Gets the natural duration of the media.</summary>
		/// <returns>The natural duration of the media.</returns>
		// Token: 0x170013D0 RID: 5072
		// (get) Token: 0x06005198 RID: 20888 RVA: 0x0016D9E5 File Offset: 0x0016BBE5
		public Duration NaturalDuration
		{
			get
			{
				return this._helper.Player.NaturalDuration;
			}
		}

		/// <summary>Gets or sets the current position of progress through the media's playback time.</summary>
		/// <returns>The amount of time since the beginning of the media. The default is 00:00:00.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Controls.MediaElement.Clock" /> property is not <see langword="null" />.</exception>
		// Token: 0x170013D1 RID: 5073
		// (get) Token: 0x06005199 RID: 20889 RVA: 0x0016D9F7 File Offset: 0x0016BBF7
		// (set) Token: 0x0600519A RID: 20890 RVA: 0x0016DA04 File Offset: 0x0016BC04
		public TimeSpan Position
		{
			get
			{
				return this._helper.Position;
			}
			set
			{
				this._helper.SetPosition(value);
			}
		}

		/// <summary>Gets or sets the speed ratio of the media.</summary>
		/// <returns>The speed ratio of the media. The valid range is between 0 (zero) and infinity. Values less than 1 yield slower than normal playback, and values greater than 1 yield faster than normal playback. Negative values are treated as 0. The default value is 1. </returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Controls.MediaElement.Clock" /> property is not <see langword="null" />.</exception>
		// Token: 0x170013D2 RID: 5074
		// (get) Token: 0x0600519B RID: 20891 RVA: 0x0016DA12 File Offset: 0x0016BC12
		// (set) Token: 0x0600519C RID: 20892 RVA: 0x0016DA1F File Offset: 0x0016BC1F
		public double SpeedRatio
		{
			get
			{
				return this._helper.SpeedRatio;
			}
			set
			{
				this._helper.SetSpeedRatio(value);
			}
		}

		/// <summary>Occurs when an error is encountered.</summary>
		// Token: 0x140000F6 RID: 246
		// (add) Token: 0x0600519D RID: 20893 RVA: 0x0016DA2D File Offset: 0x0016BC2D
		// (remove) Token: 0x0600519E RID: 20894 RVA: 0x0016DA3B File Offset: 0x0016BC3B
		public event EventHandler<ExceptionRoutedEventArgs> MediaFailed
		{
			add
			{
				base.AddHandler(MediaElement.MediaFailedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MediaElement.MediaFailedEvent, value);
			}
		}

		/// <summary>Occurs when media loading has finished.</summary>
		// Token: 0x140000F7 RID: 247
		// (add) Token: 0x0600519F RID: 20895 RVA: 0x0016DA49 File Offset: 0x0016BC49
		// (remove) Token: 0x060051A0 RID: 20896 RVA: 0x0016DA57 File Offset: 0x0016BC57
		public event RoutedEventHandler MediaOpened
		{
			add
			{
				base.AddHandler(MediaElement.MediaOpenedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MediaElement.MediaOpenedEvent, value);
			}
		}

		/// <summary>Occurs when media buffering has begun.</summary>
		// Token: 0x140000F8 RID: 248
		// (add) Token: 0x060051A1 RID: 20897 RVA: 0x0016DA65 File Offset: 0x0016BC65
		// (remove) Token: 0x060051A2 RID: 20898 RVA: 0x0016DA73 File Offset: 0x0016BC73
		public event RoutedEventHandler BufferingStarted
		{
			add
			{
				base.AddHandler(MediaElement.BufferingStartedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MediaElement.BufferingStartedEvent, value);
			}
		}

		/// <summary>Occurs when media buffering has ended.</summary>
		// Token: 0x140000F9 RID: 249
		// (add) Token: 0x060051A3 RID: 20899 RVA: 0x0016DA81 File Offset: 0x0016BC81
		// (remove) Token: 0x060051A4 RID: 20900 RVA: 0x0016DA8F File Offset: 0x0016BC8F
		public event RoutedEventHandler BufferingEnded
		{
			add
			{
				base.AddHandler(MediaElement.BufferingEndedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MediaElement.BufferingEndedEvent, value);
			}
		}

		/// <summary>Occurs when a script command is encountered in the media.</summary>
		// Token: 0x140000FA RID: 250
		// (add) Token: 0x060051A5 RID: 20901 RVA: 0x0016DA9D File Offset: 0x0016BC9D
		// (remove) Token: 0x060051A6 RID: 20902 RVA: 0x0016DAAB File Offset: 0x0016BCAB
		public event EventHandler<MediaScriptCommandRoutedEventArgs> ScriptCommand
		{
			add
			{
				base.AddHandler(MediaElement.ScriptCommandEvent, value);
			}
			remove
			{
				base.RemoveHandler(MediaElement.ScriptCommandEvent, value);
			}
		}

		/// <summary>Occurs when the media has ended.</summary>
		// Token: 0x140000FB RID: 251
		// (add) Token: 0x060051A7 RID: 20903 RVA: 0x0016DAB9 File Offset: 0x0016BCB9
		// (remove) Token: 0x060051A8 RID: 20904 RVA: 0x0016DAC7 File Offset: 0x0016BCC7
		public event RoutedEventHandler MediaEnded
		{
			add
			{
				base.AddHandler(MediaElement.MediaEndedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MediaElement.MediaEndedEvent, value);
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The base URI of the current context.</returns>
		// Token: 0x170013D3 RID: 5075
		// (get) Token: 0x060051A9 RID: 20905 RVA: 0x0016DAD5 File Offset: 0x0016BCD5
		// (set) Token: 0x060051AA RID: 20906 RVA: 0x0016DAE2 File Offset: 0x0016BCE2
		Uri IUriContext.BaseUri
		{
			get
			{
				return this._helper.BaseUri;
			}
			set
			{
				this._helper.BaseUri = value;
			}
		}

		/// <summary>Creates and returns an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Controls.MediaElement" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for this <see cref="T:System.Windows.Controls.MediaElement" />.</returns>
		// Token: 0x060051AB RID: 20907 RVA: 0x0016DAF0 File Offset: 0x0016BCF0
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new MediaElementAutomationPeer(this);
		}

		/// <summary>Updates the <see cref="P:System.Windows.UIElement.DesiredSize" /> of the <see cref="T:System.Windows.Controls.MediaElement" />. This method is called by a parent <see cref="T:System.Windows.UIElement" />. This is the first pass of layout.</summary>
		/// <param name="availableSize">The upper limit the element should not exceed.</param>
		/// <returns>The desired size.</returns>
		// Token: 0x060051AC RID: 20908 RVA: 0x0016DAF8 File Offset: 0x0016BCF8
		protected override Size MeasureOverride(Size availableSize)
		{
			return this.MeasureArrangeHelper(availableSize);
		}

		/// <summary>Arranges and sizes a <see cref="T:System.Windows.Controls.MediaElement" /> control.</summary>
		/// <param name="finalSize">Size used to arrange the control.</param>
		/// <returns>Size of the control.</returns>
		// Token: 0x060051AD RID: 20909 RVA: 0x0016DAF8 File Offset: 0x0016BCF8
		protected override Size ArrangeOverride(Size finalSize)
		{
			return this.MeasureArrangeHelper(finalSize);
		}

		/// <summary>Draws the content of a <see cref="T:System.Windows.Media.DrawingContext" /> object during the render pass of a <see cref="T:System.Windows.Controls.MediaElement" /> control. </summary>
		/// <param name="drawingContext">The <see cref="T:System.Windows.Media.DrawingContext" /> to draw.</param>
		// Token: 0x060051AE RID: 20910 RVA: 0x0016DB04 File Offset: 0x0016BD04
		protected override void OnRender(DrawingContext drawingContext)
		{
			if (this._helper.Player == null)
			{
				return;
			}
			drawingContext.DrawVideo(this._helper.Player, new Rect(default(Point), base.RenderSize));
		}

		// Token: 0x170013D4 RID: 5076
		// (get) Token: 0x060051AF RID: 20911 RVA: 0x0016DB44 File Offset: 0x0016BD44
		internal AVElementHelper Helper
		{
			get
			{
				return this._helper;
			}
		}

		// Token: 0x060051B0 RID: 20912 RVA: 0x0016DB4C File Offset: 0x0016BD4C
		private void Initialize()
		{
			this._helper = new AVElementHelper(this);
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x0016DB5C File Offset: 0x0016BD5C
		private Size MeasureArrangeHelper(Size inputSize)
		{
			MediaPlayer player = this._helper.Player;
			if (player == null)
			{
				return default(Size);
			}
			Size contentSize = new Size((double)player.NaturalVideoWidth, (double)player.NaturalVideoHeight);
			Size size = Viewbox.ComputeScaleFactor(inputSize, contentSize, this.Stretch, this.StretchDirection);
			return new Size(contentSize.Width * size.Width, contentSize.Height * size.Height);
		}

		// Token: 0x060051B2 RID: 20914 RVA: 0x0016DBD0 File Offset: 0x0016BDD0
		private static void VolumePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			MediaElement mediaElement = (MediaElement)d;
			if (mediaElement != null)
			{
				mediaElement._helper.SetVolume((double)e.NewValue);
			}
		}

		// Token: 0x060051B3 RID: 20915 RVA: 0x0016DC08 File Offset: 0x0016BE08
		private static void BalancePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			MediaElement mediaElement = (MediaElement)d;
			if (mediaElement != null)
			{
				mediaElement._helper.SetBalance((double)e.NewValue);
			}
		}

		// Token: 0x060051B4 RID: 20916 RVA: 0x0016DC40 File Offset: 0x0016BE40
		private static void IsMutedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			MediaElement mediaElement = (MediaElement)d;
			if (mediaElement != null)
			{
				mediaElement._helper.SetIsMuted((bool)e.NewValue);
			}
		}

		// Token: 0x060051B5 RID: 20917 RVA: 0x0016DC78 File Offset: 0x0016BE78
		private static void ScrubbingEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			MediaElement mediaElement = (MediaElement)d;
			if (mediaElement != null)
			{
				mediaElement._helper.SetScrubbingEnabled((bool)e.NewValue);
			}
		}

		// Token: 0x060051B6 RID: 20918 RVA: 0x0016DCB0 File Offset: 0x0016BEB0
		private static void UnloadedBehaviorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			MediaElement mediaElement = (MediaElement)d;
			if (mediaElement != null)
			{
				mediaElement._helper.SetUnloadedBehavior((MediaState)e.NewValue);
			}
		}

		// Token: 0x060051B7 RID: 20919 RVA: 0x0016DCE8 File Offset: 0x0016BEE8
		private static void LoadedBehaviorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			MediaElement mediaElement = (MediaElement)d;
			if (mediaElement != null)
			{
				mediaElement._helper.SetLoadedBehavior((MediaState)e.NewValue);
			}
		}

		// Token: 0x060051B8 RID: 20920 RVA: 0x0016DD20 File Offset: 0x0016BF20
		internal void OnMediaFailed(object sender, ExceptionEventArgs args)
		{
			base.RaiseEvent(new ExceptionRoutedEventArgs(MediaElement.MediaFailedEvent, this, args.ErrorException));
		}

		// Token: 0x060051B9 RID: 20921 RVA: 0x0016DD39 File Offset: 0x0016BF39
		internal void OnMediaOpened(object sender, EventArgs args)
		{
			base.RaiseEvent(new RoutedEventArgs(MediaElement.MediaOpenedEvent, this));
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x0016DD4C File Offset: 0x0016BF4C
		internal void OnBufferingStarted(object sender, EventArgs args)
		{
			base.RaiseEvent(new RoutedEventArgs(MediaElement.BufferingStartedEvent, this));
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x0016DD5F File Offset: 0x0016BF5F
		internal void OnBufferingEnded(object sender, EventArgs args)
		{
			base.RaiseEvent(new RoutedEventArgs(MediaElement.BufferingEndedEvent, this));
		}

		// Token: 0x060051BC RID: 20924 RVA: 0x0016DD72 File Offset: 0x0016BF72
		internal void OnMediaEnded(object sender, EventArgs args)
		{
			base.RaiseEvent(new RoutedEventArgs(MediaElement.MediaEndedEvent, this));
		}

		// Token: 0x060051BD RID: 20925 RVA: 0x0016DD85 File Offset: 0x0016BF85
		internal void OnScriptCommand(object sender, MediaScriptCommandEventArgs args)
		{
			base.RaiseEvent(new MediaScriptCommandRoutedEventArgs(MediaElement.ScriptCommandEvent, this, args.ParameterType, args.ParameterValue));
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MediaElement.Source" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MediaElement.Source" /> dependency property.</returns>
		// Token: 0x04002C6D RID: 11373
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(MediaElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(AVElementHelper.OnSourceChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MediaElement.Volume" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MediaElement.Volume" /> dependency property.</returns>
		// Token: 0x04002C6E RID: 11374
		public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register("Volume", typeof(double), typeof(MediaElement), new FrameworkPropertyMetadata(0.5, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(MediaElement.VolumePropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MediaElement.Balance" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MediaElement.Balance" /> dependency property.</returns>
		// Token: 0x04002C6F RID: 11375
		public static readonly DependencyProperty BalanceProperty = DependencyProperty.Register("Balance", typeof(double), typeof(MediaElement), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(MediaElement.BalancePropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MediaElement.IsMuted" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MediaElement.IsMuted" /> dependency property.</returns>
		// Token: 0x04002C70 RID: 11376
		public static readonly DependencyProperty IsMutedProperty = DependencyProperty.Register("IsMuted", typeof(bool), typeof(MediaElement), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(MediaElement.IsMutedPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MediaElement.ScrubbingEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MediaElement.ScrubbingEnabled" /> dependency property.</returns>
		// Token: 0x04002C71 RID: 11377
		public static readonly DependencyProperty ScrubbingEnabledProperty = DependencyProperty.Register("ScrubbingEnabled", typeof(bool), typeof(MediaElement), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(MediaElement.ScrubbingEnabledPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MediaElement.UnloadedBehavior" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MediaElement.UnloadedBehavior" /> dependency property.</returns>
		// Token: 0x04002C72 RID: 11378
		public static readonly DependencyProperty UnloadedBehaviorProperty = DependencyProperty.Register("UnloadedBehavior", typeof(MediaState), typeof(MediaElement), new FrameworkPropertyMetadata(MediaState.Close, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(MediaElement.UnloadedBehaviorPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MediaElement.LoadedBehavior" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MediaElement.LoadedBehavior" /> dependency property.</returns>
		// Token: 0x04002C73 RID: 11379
		public static readonly DependencyProperty LoadedBehaviorProperty = DependencyProperty.Register("LoadedBehavior", typeof(MediaState), typeof(MediaElement), new FrameworkPropertyMetadata(MediaState.Play, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(MediaElement.LoadedBehaviorPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MediaElement.Stretch" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MediaElement.Stretch" /> dependency property.</returns>
		// Token: 0x04002C74 RID: 11380
		public static readonly DependencyProperty StretchProperty = Viewbox.StretchProperty.AddOwner(typeof(MediaElement));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MediaElement.StretchDirection" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MediaElement.StretchDirection" /> dependency property.</returns>
		// Token: 0x04002C75 RID: 11381
		public static readonly DependencyProperty StretchDirectionProperty = Viewbox.StretchDirectionProperty.AddOwner(typeof(MediaElement));

		// Token: 0x04002C7C RID: 11388
		private AVElementHelper _helper;
	}
}
