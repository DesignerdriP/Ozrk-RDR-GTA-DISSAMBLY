using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Commands;
using MS.Internal.Controls;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Provides a control for viewing flow content, with built-in support for multiple viewing modes.</summary>
	// Token: 0x020004D0 RID: 1232
	[TemplatePart(Name = "PART_ContentHost", Type = typeof(Decorator))]
	[TemplatePart(Name = "PART_FindToolBarHost", Type = typeof(Decorator))]
	[ContentProperty("Document")]
	public class FlowDocumentReader : Control, IAddChild, IJournalState
	{
		// Token: 0x06004B16 RID: 19222 RVA: 0x0015277C File Offset: 0x0015097C
		static FlowDocumentReader()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FlowDocumentReader), new FrameworkPropertyMetadata(new ComponentResourceKey(typeof(PresentationUIStyleResources), "PUIFlowDocumentReader")));
			FlowDocumentReader._dType = DependencyObjectType.FromSystemTypeInternal(typeof(FlowDocumentReader));
			TextBoxBase.SelectionBrushProperty.OverrideMetadata(typeof(FlowDocumentReader), new FrameworkPropertyMetadata(new PropertyChangedCallback(FlowDocumentReader.UpdateCaretElement)));
			TextBoxBase.SelectionOpacityProperty.OverrideMetadata(typeof(FlowDocumentReader), new FrameworkPropertyMetadata(0.4, new PropertyChangedCallback(FlowDocumentReader.UpdateCaretElement)));
			FlowDocumentReader.CreateCommandBindings();
			EventManager.RegisterClassHandler(typeof(FlowDocumentReader), Keyboard.KeyDownEvent, new KeyEventHandler(FlowDocumentReader.KeyDownHandler), true);
		}

		/// <summary>Builds the visual tree for the <see cref="T:System.Windows.Controls.FlowDocumentReader" />.</summary>
		// Token: 0x06004B18 RID: 19224 RVA: 0x00152CAC File Offset: 0x00150EAC
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this.CurrentViewer != null)
			{
				this.DetachViewer(this.CurrentViewer);
				this._contentHost.Child = null;
			}
			this._contentHost = (base.GetTemplateChild("PART_ContentHost") as Decorator);
			if (this._contentHost != null)
			{
				if (this._contentHost.Child != null)
				{
					throw new NotSupportedException(SR.Get("FlowDocumentReaderDecoratorMarkedAsContentHostMustHaveNoContent"));
				}
				this.SwitchViewingModeCore(this.ViewingMode);
			}
			if (this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
			}
			this._findToolBarHost = (base.GetTemplateChild("PART_FindToolBarHost") as Decorator);
			this._findButton = (base.GetTemplateChild("FindButton") as ToggleButton);
		}

		/// <summary>Returns a value that indicates whether or the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> is able to jump to the specified page number.</summary>
		/// <param name="pageNumber">A page number to check for as a valid jump target.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> is able to jump to the specified page number; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004B19 RID: 19225 RVA: 0x00152D64 File Offset: 0x00150F64
		public bool CanGoToPage(int pageNumber)
		{
			bool result = false;
			if (this.CurrentViewer != null)
			{
				result = this.CurrentViewer.CanGoToPage(pageNumber);
			}
			return result;
		}

		/// <summary>Toggles the Find dialog.</summary>
		// Token: 0x06004B1A RID: 19226 RVA: 0x00152D89 File Offset: 0x00150F89
		public void Find()
		{
			this.OnFindCommand();
		}

		/// <summary>Invokes a standard Print dialog which can be used to print the contents of the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> and configure printing preferences.</summary>
		// Token: 0x06004B1B RID: 19227 RVA: 0x00152D91 File Offset: 0x00150F91
		public void Print()
		{
			this.OnPrintCommand();
		}

		/// <summary>Cancels any current printing job.</summary>
		// Token: 0x06004B1C RID: 19228 RVA: 0x00152D99 File Offset: 0x00150F99
		public void CancelPrint()
		{
			this.OnCancelPrintCommand();
		}

		/// <summary>Executes the <see cref="P:System.Windows.Input.NavigationCommands.IncreaseZoom" /> routed command.</summary>
		// Token: 0x06004B1D RID: 19229 RVA: 0x00152DA1 File Offset: 0x00150FA1
		public void IncreaseZoom()
		{
			this.OnIncreaseZoomCommand();
		}

		/// <summary>Executes the <see cref="P:System.Windows.Input.NavigationCommands.DecreaseZoom" /> routed command.</summary>
		// Token: 0x06004B1E RID: 19230 RVA: 0x00152DA9 File Offset: 0x00150FA9
		public void DecreaseZoom()
		{
			this.OnDecreaseZoomCommand();
		}

		/// <summary>Executes the <see cref="F:System.Windows.Controls.FlowDocumentReader.SwitchViewingModeCommand" /> command.</summary>
		/// <param name="viewingMode">One of the <see cref="T:System.Windows.Controls.FlowDocumentReaderViewingMode" /> values that specifies the desired viewing mode.</param>
		// Token: 0x06004B1F RID: 19231 RVA: 0x00152DB1 File Offset: 0x00150FB1
		public void SwitchViewingMode(FlowDocumentReaderViewingMode viewingMode)
		{
			this.OnSwitchViewingModeCommand(viewingMode);
		}

		/// <summary>Gets or sets the viewing mode for the <see cref="T:System.Windows.Controls.FlowDocumentReader" />. </summary>
		/// <returns>One of the <see cref="T:System.Windows.Controls.FlowDocumentReaderViewingMode" /> values that specifies the viewing mode. The default is <see cref="F:System.Windows.Controls.FlowDocumentReaderViewingMode.Page" />.</returns>
		// Token: 0x17001251 RID: 4689
		// (get) Token: 0x06004B20 RID: 19232 RVA: 0x00152DBA File Offset: 0x00150FBA
		// (set) Token: 0x06004B21 RID: 19233 RVA: 0x00152DCC File Offset: 0x00150FCC
		public FlowDocumentReaderViewingMode ViewingMode
		{
			get
			{
				return (FlowDocumentReaderViewingMode)base.GetValue(FlowDocumentReader.ViewingModeProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.ViewingModeProperty, value);
			}
		}

		/// <summary>Gets the selected content of the <see cref="T:System.Windows.Controls.FlowDocumentReader" />.</summary>
		/// <returns>The selected content of the <see cref="T:System.Windows.Controls.FlowDocumentReader" />.</returns>
		// Token: 0x17001252 RID: 4690
		// (get) Token: 0x06004B22 RID: 19234 RVA: 0x00152DE0 File Offset: 0x00150FE0
		public TextSelection Selection
		{
			get
			{
				TextSelection result = null;
				if (this._contentHost != null)
				{
					IFlowDocumentViewer flowDocumentViewer = this._contentHost.Child as IFlowDocumentViewer;
					if (flowDocumentViewer != null)
					{
						result = (flowDocumentViewer.TextSelection as TextSelection);
					}
				}
				return result;
			}
		}

		/// <summary>Gets or sets a value that indicates whether <see cref="F:System.Windows.Controls.FlowDocumentReaderViewingMode.Page" /> is available as a viewing mode. </summary>
		/// <returns>
		///     <see langword="true" /> to indicate that single-page viewing mode is available; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		/// <exception cref="T:System.ArgumentException">Setting this property to <see langword="false" /> while <see cref="P:System.Windows.Controls.FlowDocumentReader.IsScrollViewEnabled" /> and <see cref="P:System.Windows.Controls.FlowDocumentReader.IsTwoPageViewEnabled" /> are also <see langword="false" />.</exception>
		// Token: 0x17001253 RID: 4691
		// (get) Token: 0x06004B23 RID: 19235 RVA: 0x00152E18 File Offset: 0x00151018
		// (set) Token: 0x06004B24 RID: 19236 RVA: 0x00152E2A File Offset: 0x0015102A
		public bool IsPageViewEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.IsPageViewEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.IsPageViewEnabledProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets or sets a value that indicates whether <see cref="F:System.Windows.Controls.FlowDocumentReaderViewingMode.TwoPage" /> is available as a viewing mode. </summary>
		/// <returns>
		///     <see langword="true" /> to indicate that <see cref="F:System.Windows.Controls.FlowDocumentReaderViewingMode.TwoPage" /> is available as a viewing mode; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		/// <exception cref="T:System.ArgumentException">Setting this property to <see langword="false" /> while <see cref="P:System.Windows.Controls.FlowDocumentReader.IsPageViewEnabled" /> and <see cref="P:System.Windows.Controls.FlowDocumentReader.IsScrollViewEnabled" /> are also <see langword="false" />.</exception>
		// Token: 0x17001254 RID: 4692
		// (get) Token: 0x06004B25 RID: 19237 RVA: 0x00152E3D File Offset: 0x0015103D
		// (set) Token: 0x06004B26 RID: 19238 RVA: 0x00152E4F File Offset: 0x0015104F
		public bool IsTwoPageViewEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.IsTwoPageViewEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.IsTwoPageViewEnabledProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets or sets a value that indicates whether <see cref="F:System.Windows.Controls.FlowDocumentReaderViewingMode.Scroll" /> is available as a viewing mode. </summary>
		/// <returns>
		///     <see langword="true" /> to indicate that <see cref="F:System.Windows.Controls.FlowDocumentReaderViewingMode.Scroll" /> is available as a viewing mode; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">Setting this property to <see langword="false" /> while <see cref="P:System.Windows.Controls.FlowDocumentReader.IsPageViewEnabled" /> and <see cref="P:System.Windows.Controls.FlowDocumentReader.IsTwoPageViewEnabled" /> are also <see langword="false" />.</exception>
		// Token: 0x17001255 RID: 4693
		// (get) Token: 0x06004B27 RID: 19239 RVA: 0x00152E62 File Offset: 0x00151062
		// (set) Token: 0x06004B28 RID: 19240 RVA: 0x00152E74 File Offset: 0x00151074
		public bool IsScrollViewEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.IsScrollViewEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.IsScrollViewEnabledProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets the current number of display pages for the content hosted by the <see cref="T:System.Windows.Controls.FlowDocumentReader" />. </summary>
		/// <returns>The current number of display pages for the content hosted by the <see cref="T:System.Windows.Controls.FlowDocumentReader" />.</returns>
		// Token: 0x17001256 RID: 4694
		// (get) Token: 0x06004B29 RID: 19241 RVA: 0x00152E87 File Offset: 0x00151087
		public int PageCount
		{
			get
			{
				return (int)base.GetValue(FlowDocumentReader.PageCountProperty);
			}
		}

		/// <summary>Gets the page number for the currently displayed page. </summary>
		/// <returns>The page number for the currently displayed page.</returns>
		// Token: 0x17001257 RID: 4695
		// (get) Token: 0x06004B2A RID: 19242 RVA: 0x00152E99 File Offset: 0x00151099
		public int PageNumber
		{
			get
			{
				return (int)base.GetValue(FlowDocumentReader.PageNumberProperty);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> can execute the <see cref="P:System.Windows.Input.NavigationCommands.PreviousPage" /> routed command to jump to the previous page of content. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> can jump to the previous page of content; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001258 RID: 4696
		// (get) Token: 0x06004B2B RID: 19243 RVA: 0x00152EAB File Offset: 0x001510AB
		public bool CanGoToPreviousPage
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.CanGoToPreviousPageProperty);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> can execute the <see cref="P:System.Windows.Input.NavigationCommands.NextPage" /> routed command to jump to the next page of content. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> can jump to the next page of content; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001259 RID: 4697
		// (get) Token: 0x06004B2C RID: 19244 RVA: 0x00152EBD File Offset: 0x001510BD
		public bool CanGoToNextPage
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.CanGoToNextPageProperty);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="P:System.Windows.Input.ApplicationCommands.Find" /> routed command is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> to enable the <see cref="P:System.Windows.Input.ApplicationCommands.Find" /> routed command; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700125A RID: 4698
		// (get) Token: 0x06004B2D RID: 19245 RVA: 0x00152ECF File Offset: 0x001510CF
		// (set) Token: 0x06004B2E RID: 19246 RVA: 0x00152EE1 File Offset: 0x001510E1
		public bool IsFindEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.IsFindEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.IsFindEnabledProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="P:System.Windows.Input.ApplicationCommands.Print" /> routed command is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> to enable the <see cref="P:System.Windows.Input.ApplicationCommands.Print" /> routed command; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700125B RID: 4699
		// (get) Token: 0x06004B2F RID: 19247 RVA: 0x00152EF4 File Offset: 0x001510F4
		// (set) Token: 0x06004B30 RID: 19248 RVA: 0x00152F06 File Offset: 0x00151106
		public bool IsPrintEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.IsPrintEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.IsPrintEnabledProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Documents.FlowDocument" /> that hosts the content to be displayed by the <see cref="T:System.Windows.Controls.FlowDocumentReader" />. </summary>
		/// <returns>A <see cref="T:System.Windows.Documents.FlowDocument" /> that hosts the content to be displayed by the <see cref="T:System.Windows.Controls.FlowDocumentReader" />. The default is <see langword="null" />.</returns>
		// Token: 0x1700125C RID: 4700
		// (get) Token: 0x06004B31 RID: 19249 RVA: 0x00152F19 File Offset: 0x00151119
		// (set) Token: 0x06004B32 RID: 19250 RVA: 0x00152F2B File Offset: 0x0015112B
		public FlowDocument Document
		{
			get
			{
				return (FlowDocument)base.GetValue(FlowDocumentReader.DocumentProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.DocumentProperty, value);
			}
		}

		/// <summary>Gets or sets the current zoom level. </summary>
		/// <returns>The current zoom level, interpreted as a percentage. The default value 100.0 (zoom level of 100%).</returns>
		// Token: 0x1700125D RID: 4701
		// (get) Token: 0x06004B33 RID: 19251 RVA: 0x00152F39 File Offset: 0x00151139
		// (set) Token: 0x06004B34 RID: 19252 RVA: 0x00152F4B File Offset: 0x0015114B
		public double Zoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentReader.ZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.ZoomProperty, value);
			}
		}

		/// <summary>Gets or sets the maximum allowable <see cref="P:System.Windows.Controls.FlowDocumentReader.Zoom" /> level for the <see cref="T:System.Windows.Controls.FlowDocumentReader" />. </summary>
		/// <returns>The maximum allowable <see cref="P:System.Windows.Controls.FlowDocumentReader.Zoom" /> level for the <see cref="T:System.Windows.Controls.FlowDocumentReader" />, interpreted as a percentage. The default is 200.0 (maximum zoom of 200%).</returns>
		// Token: 0x1700125E RID: 4702
		// (get) Token: 0x06004B35 RID: 19253 RVA: 0x00152F5E File Offset: 0x0015115E
		// (set) Token: 0x06004B36 RID: 19254 RVA: 0x00152F70 File Offset: 0x00151170
		public double MaxZoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentReader.MaxZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.MaxZoomProperty, value);
			}
		}

		/// <summary>Gets or sets the minimum allowable <see cref="P:System.Windows.Controls.FlowDocumentReader.Zoom" /> level for the <see cref="T:System.Windows.Controls.FlowDocumentReader" />. </summary>
		/// <returns>The minimum allowable <see cref="P:System.Windows.Controls.FlowDocumentReader.Zoom" /> level for the <see cref="T:System.Windows.Controls.FlowDocumentReader" />, interpreted as a percentage. The default is 80.0 (minimum zoom of 80%).</returns>
		// Token: 0x1700125F RID: 4703
		// (get) Token: 0x06004B37 RID: 19255 RVA: 0x00152F83 File Offset: 0x00151183
		// (set) Token: 0x06004B38 RID: 19256 RVA: 0x00152F95 File Offset: 0x00151195
		public double MinZoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentReader.MinZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.MinZoomProperty, value);
			}
		}

		/// <summary>Gets or sets the zoom increment. </summary>
		/// <returns>The current zoom increment, interpreted as a percentage. The default is 10.0 (zoom increment of 10%).</returns>
		// Token: 0x17001260 RID: 4704
		// (get) Token: 0x06004B39 RID: 19257 RVA: 0x00152FA8 File Offset: 0x001511A8
		// (set) Token: 0x06004B3A RID: 19258 RVA: 0x00152FBA File Offset: 0x001511BA
		public double ZoomIncrement
		{
			get
			{
				return (double)base.GetValue(FlowDocumentReader.ZoomIncrementProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.ZoomIncrementProperty, value);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="P:System.Windows.Controls.FlowDocumentReader.Zoom" /> level can be increased. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.FlowDocumentReader.Zoom" /> level can be increased; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001261 RID: 4705
		// (get) Token: 0x06004B3B RID: 19259 RVA: 0x00152FCD File Offset: 0x001511CD
		public bool CanIncreaseZoom
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.CanIncreaseZoomProperty);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="P:System.Windows.Controls.FlowDocumentReader.Zoom" /> level can be decreased. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.FlowDocumentReader.Zoom" /> level can be decreased; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001262 RID: 4706
		// (get) Token: 0x06004B3C RID: 19260 RVA: 0x00152FDF File Offset: 0x001511DF
		public bool CanDecreaseZoom
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.CanDecreaseZoomProperty);
			}
		}

		/// <summary>Gets or sets the brush that highlights the selected text.</summary>
		/// <returns>A brush that highlights the selected text.</returns>
		// Token: 0x17001263 RID: 4707
		// (get) Token: 0x06004B3D RID: 19261 RVA: 0x00152FF1 File Offset: 0x001511F1
		// (set) Token: 0x06004B3E RID: 19262 RVA: 0x00153003 File Offset: 0x00151203
		public Brush SelectionBrush
		{
			get
			{
				return (Brush)base.GetValue(FlowDocumentReader.SelectionBrushProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.SelectionBrushProperty, value);
			}
		}

		/// <summary>Gets or sets the opacity of the <see cref="P:System.Windows.Controls.FlowDocumentReader.SelectionBrush" />.</summary>
		/// <returns>The opacity of the <see cref="P:System.Windows.Controls.FlowDocumentReader.SelectionBrush" />. The default is 0.4.</returns>
		// Token: 0x17001264 RID: 4708
		// (get) Token: 0x06004B3F RID: 19263 RVA: 0x00153011 File Offset: 0x00151211
		// (set) Token: 0x06004B40 RID: 19264 RVA: 0x00153023 File Offset: 0x00151223
		public double SelectionOpacity
		{
			get
			{
				return (double)base.GetValue(FlowDocumentReader.SelectionOpacityProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.SelectionOpacityProperty, value);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> has focus and selected text.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> displays selected text when the text box does not have focus; otherwise, <see langword="false" />.The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x17001265 RID: 4709
		// (get) Token: 0x06004B41 RID: 19265 RVA: 0x00153036 File Offset: 0x00151236
		public bool IsSelectionActive
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.IsSelectionActiveProperty);
			}
		}

		/// <summary>Gets or sets a value that indicates whether <see cref="T:System.Windows.Controls.FlowDocumentReader" /> displays selected text when the control does not have focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> displays selected text when the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> does not have focus; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x17001266 RID: 4710
		// (get) Token: 0x06004B42 RID: 19266 RVA: 0x00153048 File Offset: 0x00151248
		// (set) Token: 0x06004B43 RID: 19267 RVA: 0x0015305A File Offset: 0x0015125A
		public bool IsInactiveSelectionHighlightEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.IsInactiveSelectionHighlightEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.IsInactiveSelectionHighlightEnabledProperty, value);
			}
		}

		/// <summary>Called when a printing job has completed.</summary>
		// Token: 0x06004B44 RID: 19268 RVA: 0x00153068 File Offset: 0x00151268
		protected virtual void OnPrintCompleted()
		{
			if (this._printInProgress)
			{
				this._printInProgress = false;
				CommandManager.InvalidateRequerySuggested();
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.ApplicationCommands.Find" /> routed command.</summary>
		// Token: 0x06004B45 RID: 19269 RVA: 0x0015307E File Offset: 0x0015127E
		protected virtual void OnFindCommand()
		{
			if (this.CanShowFindToolBar)
			{
				this.ToggleFindToolBar(this.FindToolBar == null);
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.ApplicationCommands.Print" /> routed command.</summary>
		// Token: 0x06004B46 RID: 19270 RVA: 0x00153097 File Offset: 0x00151297
		protected virtual void OnPrintCommand()
		{
			if (this.CurrentViewer != null)
			{
				this.CurrentViewer.Print();
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.ApplicationCommands.CancelPrint" /> routed command.</summary>
		// Token: 0x06004B47 RID: 19271 RVA: 0x001530AC File Offset: 0x001512AC
		protected virtual void OnCancelPrintCommand()
		{
			if (this.CurrentViewer != null)
			{
				this.CurrentViewer.CancelPrint();
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.NavigationCommands.IncreaseZoom" /> routed command.</summary>
		// Token: 0x06004B48 RID: 19272 RVA: 0x001530C1 File Offset: 0x001512C1
		protected virtual void OnIncreaseZoomCommand()
		{
			if (this.CanIncreaseZoom)
			{
				base.SetCurrentValueInternal(FlowDocumentReader.ZoomProperty, Math.Min(this.Zoom + this.ZoomIncrement, this.MaxZoom));
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.NavigationCommands.DecreaseZoom" /> routed command.</summary>
		// Token: 0x06004B49 RID: 19273 RVA: 0x001530F3 File Offset: 0x001512F3
		protected virtual void OnDecreaseZoomCommand()
		{
			if (this.CanDecreaseZoom)
			{
				base.SetCurrentValueInternal(FlowDocumentReader.ZoomProperty, Math.Max(this.Zoom - this.ZoomIncrement, this.MinZoom));
			}
		}

		/// <summary>Handles the <see cref="M:System.Windows.Controls.FlowDocumentReader.SwitchViewingMode(System.Windows.Controls.FlowDocumentReaderViewingMode)" /> routed command.</summary>
		/// <param name="viewingMode">One of the <see cref="T:System.Windows.Controls.FlowDocumentReaderViewingMode" /> values that specifies the viewing mode to switch to.</param>
		// Token: 0x06004B4A RID: 19274 RVA: 0x00153125 File Offset: 0x00151325
		protected virtual void OnSwitchViewingModeCommand(FlowDocumentReaderViewingMode viewingMode)
		{
			this.SwitchViewingModeCore(viewingMode);
		}

		/// <summary>Handles the <see cref="E:System.Windows.FrameworkElement.Initialized" /> routed event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> object containing the arguments associated with the <see cref="E:System.Windows.FrameworkElement.Initialized" /> routed event.</param>
		// Token: 0x06004B4B RID: 19275 RVA: 0x0015312E File Offset: 0x0015132E
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			if (base.IsInitialized && !this.CanSwitchToViewingMode(this.ViewingMode))
			{
				throw new ArgumentException(SR.Get("FlowDocumentReaderViewingModeEnabledConflict"));
			}
		}

		/// <summary>Called when the DPI at which this Flow Document Reader is rendered changes.</summary>
		/// <param name="oldDpiScaleInfo">The previous DPI scale setting.</param>
		/// <param name="newDpiScaleInfo">The new DPI scale setting.</param>
		// Token: 0x06004B4C RID: 19276 RVA: 0x0015315D File Offset: 0x0015135D
		protected override void OnDpiChanged(DpiScale oldDpiScaleInfo, DpiScale newDpiScaleInfo)
		{
			FlowDocument document = this.Document;
			if (document == null)
			{
				return;
			}
			document.SetDpi(newDpiScaleInfo);
		}

		/// <summary>Creates and returns an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Controls.FlowDocumentReader" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Controls.FlowDocumentReader" />.</returns>
		// Token: 0x06004B4D RID: 19277 RVA: 0x00153170 File Offset: 0x00151370
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new FlowDocumentReaderAutomationPeer(this);
		}

		/// <summary>Handles the <see cref="E:System.Windows.UIElement.IsKeyboardFocusWithinChanged" /> routed event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" /> object containing the arguments associated with the <see cref="E:System.Windows.UIElement.IsKeyboardFocusWithinChanged" /> routed event.</param>
		// Token: 0x06004B4E RID: 19278 RVA: 0x00153178 File Offset: 0x00151378
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsKeyboardFocusWithinChanged(e);
			if (base.IsKeyboardFocusWithin && this.CurrentViewer != null && !this.IsFocusWithinDocument())
			{
				((FrameworkElement)this.CurrentViewer).Focus();
			}
		}

		/// <summary>Invoked whenever an unhandled <see cref="E:System.Windows.Input.Keyboard.KeyDown" /> attached routed event reaches an element derived from this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x06004B4F RID: 19279 RVA: 0x001531B8 File Offset: 0x001513B8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Handled)
			{
				return;
			}
			Key key = e.Key;
			if (key != Key.Escape)
			{
				if (key == Key.F3)
				{
					if (this.CanShowFindToolBar)
					{
						if (this.FindToolBar != null)
						{
							this.FindToolBar.SearchUp = ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift);
							this.OnFindInvoked(this, EventArgs.Empty);
						}
						else
						{
							this.ToggleFindToolBar(true);
						}
						e.Handled = true;
					}
				}
			}
			else if (this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
				e.Handled = true;
			}
			if (!e.Handled)
			{
				base.OnKeyDown(e);
			}
		}

		/// <summary>Gets an enumerator that can iterate the logical children of the <see cref="T:System.Windows.Controls.FlowDocumentReader" />.</summary>
		/// <returns>An enumerator for the logical children.</returns>
		// Token: 0x17001267 RID: 4711
		// (get) Token: 0x06004B50 RID: 19280 RVA: 0x0015324F File Offset: 0x0015144F
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (base.HasLogicalChildren && this.Document != null)
				{
					return new SingleChildEnumerator(this.Document);
				}
				return EmptyEnumerator.Instance;
			}
		}

		// Token: 0x06004B51 RID: 19281 RVA: 0x00153272 File Offset: 0x00151472
		internal override bool BuildRouteCore(EventRoute route, RoutedEventArgs args)
		{
			return base.BuildRouteCoreHelper(route, args, false);
		}

		// Token: 0x06004B52 RID: 19282 RVA: 0x00153280 File Offset: 0x00151480
		internal override bool InvalidateAutomationAncestorsCore(Stack<DependencyObject> branchNodeStack, out bool continuePastCoreTree)
		{
			bool shouldInvalidateIntermediateElements = false;
			return base.InvalidateAutomationAncestorsCoreHelper(branchNodeStack, out continuePastCoreTree, shouldInvalidateIntermediateElements);
		}

		/// <summary>Handles the <see cref="M:System.Windows.Controls.FlowDocumentReader.SwitchViewingMode(System.Windows.Controls.FlowDocumentReaderViewingMode)" /> command.</summary>
		/// <param name="viewingMode">One of the <see cref="T:System.Windows.Controls.FlowDocumentReaderViewingMode" /> values that specifies the desired viewing mode.</param>
		// Token: 0x06004B53 RID: 19283 RVA: 0x00153298 File Offset: 0x00151498
		protected virtual void SwitchViewingModeCore(FlowDocumentReaderViewingMode viewingMode)
		{
			ITextSelection textSelection = null;
			ContentPosition contentPosition = null;
			DependencyObject dependencyObject = null;
			if (this._contentHost != null)
			{
				bool isKeyboardFocusWithin = base.IsKeyboardFocusWithin;
				IFlowDocumentViewer flowDocumentViewer = this._contentHost.Child as IFlowDocumentViewer;
				if (flowDocumentViewer != null)
				{
					if (isKeyboardFocusWithin)
					{
						bool flag = this.IsFocusWithinDocument();
						if (flag)
						{
							dependencyObject = (Keyboard.FocusedElement as DependencyObject);
						}
					}
					textSelection = flowDocumentViewer.TextSelection;
					contentPosition = flowDocumentViewer.ContentPosition;
					this.DetachViewer(flowDocumentViewer);
				}
				flowDocumentViewer = this.GetViewerFromMode(viewingMode);
				FrameworkElement frameworkElement = (FrameworkElement)flowDocumentViewer;
				if (flowDocumentViewer != null)
				{
					this._contentHost.Child = frameworkElement;
					this.AttachViewer(flowDocumentViewer);
					flowDocumentViewer.TextSelection = textSelection;
					flowDocumentViewer.ContentPosition = contentPosition;
					if (isKeyboardFocusWithin)
					{
						if (dependencyObject is UIElement)
						{
							((UIElement)dependencyObject).Focus();
						}
						else if (dependencyObject is ContentElement)
						{
							((ContentElement)dependencyObject).Focus();
						}
						else
						{
							frameworkElement.Focus();
						}
					}
				}
				this.UpdateReadOnlyProperties(true, true);
			}
		}

		// Token: 0x06004B54 RID: 19284 RVA: 0x0015337C File Offset: 0x0015157C
		private bool IsFocusWithinDocument()
		{
			DependencyObject dependencyObject = Keyboard.FocusedElement as DependencyObject;
			while (dependencyObject != null && dependencyObject != this.Document)
			{
				FrameworkElement frameworkElement = dependencyObject as FrameworkElement;
				if (frameworkElement != null && frameworkElement.TemplatedParent != null)
				{
					dependencyObject = frameworkElement.TemplatedParent;
				}
				else
				{
					dependencyObject = LogicalTreeHelper.GetParent(dependencyObject);
				}
			}
			return dependencyObject != null;
		}

		// Token: 0x06004B55 RID: 19285 RVA: 0x001533C8 File Offset: 0x001515C8
		private void DocumentChanged(FlowDocument oldDocument, FlowDocument newDocument)
		{
			if (oldDocument != null && this._documentAsLogicalChild)
			{
				base.RemoveLogicalChild(oldDocument);
			}
			if (base.TemplatedParent != null && newDocument != null && LogicalTreeHelper.GetParent(newDocument) != null)
			{
				ContentOperations.SetParent(newDocument, this);
				this._documentAsLogicalChild = false;
			}
			else
			{
				this._documentAsLogicalChild = true;
			}
			if (newDocument != null)
			{
				newDocument.SetDpi(base.GetDpi());
				if (this._documentAsLogicalChild)
				{
					base.AddLogicalChild(newDocument);
				}
			}
			if (this.CurrentViewer != null)
			{
				this.CurrentViewer.SetDocument(newDocument);
			}
			this.UpdateReadOnlyProperties(true, true);
			if (!this.CanShowFindToolBar && this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
			}
			FlowDocumentReaderAutomationPeer flowDocumentReaderAutomationPeer = UIElementAutomationPeer.FromElement(this) as FlowDocumentReaderAutomationPeer;
			if (flowDocumentReaderAutomationPeer != null)
			{
				flowDocumentReaderAutomationPeer.InvalidatePeer();
			}
		}

		// Token: 0x06004B56 RID: 19286 RVA: 0x00153478 File Offset: 0x00151678
		private void DetachViewer(IFlowDocumentViewer viewer)
		{
			Invariant.Assert(viewer != null && viewer is FrameworkElement);
			FrameworkElement target = (FrameworkElement)viewer;
			BindingOperations.ClearBinding(target, FlowDocumentReader.ZoomProperty);
			BindingOperations.ClearBinding(target, FlowDocumentReader.MaxZoomProperty);
			BindingOperations.ClearBinding(target, FlowDocumentReader.MinZoomProperty);
			BindingOperations.ClearBinding(target, FlowDocumentReader.ZoomIncrementProperty);
			viewer.PageCountChanged -= this.OnPageCountChanged;
			viewer.PageNumberChanged -= this.OnPageNumberChanged;
			viewer.PrintStarted -= this.OnViewerPrintStarted;
			viewer.PrintCompleted -= this.OnViewerPrintCompleted;
			viewer.SetDocument(null);
		}

		// Token: 0x06004B57 RID: 19287 RVA: 0x0015351C File Offset: 0x0015171C
		private void AttachViewer(IFlowDocumentViewer viewer)
		{
			Invariant.Assert(viewer != null && viewer is FrameworkElement);
			FrameworkElement fe = (FrameworkElement)viewer;
			viewer.SetDocument(this.Document);
			viewer.PageCountChanged += this.OnPageCountChanged;
			viewer.PageNumberChanged += this.OnPageNumberChanged;
			viewer.PrintStarted += this.OnViewerPrintStarted;
			viewer.PrintCompleted += this.OnViewerPrintCompleted;
			this.CreateTwoWayBinding(fe, FlowDocumentReader.ZoomProperty, "Zoom");
			this.CreateTwoWayBinding(fe, FlowDocumentReader.MaxZoomProperty, "MaxZoom");
			this.CreateTwoWayBinding(fe, FlowDocumentReader.MinZoomProperty, "MinZoom");
			this.CreateTwoWayBinding(fe, FlowDocumentReader.ZoomIncrementProperty, "ZoomIncrement");
		}

		// Token: 0x06004B58 RID: 19288 RVA: 0x001535DC File Offset: 0x001517DC
		private void CreateTwoWayBinding(FrameworkElement fe, DependencyProperty dp, string propertyPath)
		{
			fe.SetBinding(dp, new Binding(propertyPath)
			{
				Mode = BindingMode.TwoWay,
				Source = this
			});
		}

		// Token: 0x06004B59 RID: 19289 RVA: 0x00153608 File Offset: 0x00151808
		private bool CanSwitchToViewingMode(FlowDocumentReaderViewingMode mode)
		{
			bool result = false;
			switch (mode)
			{
			case FlowDocumentReaderViewingMode.Page:
				result = this.IsPageViewEnabled;
				break;
			case FlowDocumentReaderViewingMode.TwoPage:
				result = this.IsTwoPageViewEnabled;
				break;
			case FlowDocumentReaderViewingMode.Scroll:
				result = this.IsScrollViewEnabled;
				break;
			}
			return result;
		}

		// Token: 0x06004B5A RID: 19290 RVA: 0x00153648 File Offset: 0x00151848
		private IFlowDocumentViewer GetViewerFromMode(FlowDocumentReaderViewingMode mode)
		{
			IFlowDocumentViewer result = null;
			switch (mode)
			{
			case FlowDocumentReaderViewingMode.Page:
				if (this._pageViewer == null)
				{
					this._pageViewer = new ReaderPageViewer();
					this._pageViewer.SetResourceReference(FrameworkElement.StyleProperty, FlowDocumentReader.PageViewStyleKey);
					this._pageViewer.Name = "PageViewer";
					CommandManager.AddPreviewCanExecuteHandler(this._pageViewer, new CanExecuteRoutedEventHandler(this.PreviewCanExecuteRoutedEventHandler));
				}
				result = this._pageViewer;
				break;
			case FlowDocumentReaderViewingMode.TwoPage:
				if (this._twoPageViewer == null)
				{
					this._twoPageViewer = new ReaderTwoPageViewer();
					this._twoPageViewer.SetResourceReference(FrameworkElement.StyleProperty, FlowDocumentReader.TwoPageViewStyleKey);
					this._twoPageViewer.Name = "TwoPageViewer";
					CommandManager.AddPreviewCanExecuteHandler(this._twoPageViewer, new CanExecuteRoutedEventHandler(this.PreviewCanExecuteRoutedEventHandler));
				}
				result = this._twoPageViewer;
				break;
			case FlowDocumentReaderViewingMode.Scroll:
				if (this._scrollViewer == null)
				{
					this._scrollViewer = new ReaderScrollViewer();
					this._scrollViewer.SetResourceReference(FrameworkElement.StyleProperty, FlowDocumentReader.ScrollViewStyleKey);
					this._scrollViewer.Name = "ScrollViewer";
					CommandManager.AddPreviewCanExecuteHandler(this._scrollViewer, new CanExecuteRoutedEventHandler(this.PreviewCanExecuteRoutedEventHandler));
				}
				result = this._scrollViewer;
				break;
			}
			return result;
		}

		// Token: 0x06004B5B RID: 19291 RVA: 0x00153778 File Offset: 0x00151978
		private void UpdateReadOnlyProperties(bool pageCountChanged, bool pageNumberChanged)
		{
			if (pageCountChanged)
			{
				base.SetValue(FlowDocumentReader.PageCountPropertyKey, (this.CurrentViewer != null) ? this.CurrentViewer.PageCount : 0);
			}
			if (pageNumberChanged)
			{
				base.SetValue(FlowDocumentReader.PageNumberPropertyKey, (this.CurrentViewer != null) ? this.CurrentViewer.PageNumber : 0);
				base.SetValue(FlowDocumentReader.CanGoToPreviousPagePropertyKey, this.CurrentViewer != null && this.CurrentViewer.CanGoToPreviousPage);
			}
			if (pageCountChanged || pageNumberChanged)
			{
				base.SetValue(FlowDocumentReader.CanGoToNextPagePropertyKey, this.CurrentViewer != null && this.CurrentViewer.CanGoToNextPage);
			}
		}

		// Token: 0x06004B5C RID: 19292 RVA: 0x0015381E File Offset: 0x00151A1E
		private void OnPageCountChanged(object sender, EventArgs e)
		{
			Invariant.Assert(this.CurrentViewer != null && sender == this.CurrentViewer);
			this.UpdateReadOnlyProperties(true, false);
		}

		// Token: 0x06004B5D RID: 19293 RVA: 0x00153841 File Offset: 0x00151A41
		private void OnPageNumberChanged(object sender, EventArgs e)
		{
			Invariant.Assert(this.CurrentViewer != null && sender == this.CurrentViewer);
			this.UpdateReadOnlyProperties(false, true);
		}

		// Token: 0x06004B5E RID: 19294 RVA: 0x00153864 File Offset: 0x00151A64
		private void OnViewerPrintStarted(object sender, EventArgs e)
		{
			Invariant.Assert(this.CurrentViewer != null && sender == this.CurrentViewer);
			this._printInProgress = true;
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x06004B5F RID: 19295 RVA: 0x0015388B File Offset: 0x00151A8B
		private void OnViewerPrintCompleted(object sender, EventArgs e)
		{
			Invariant.Assert(this.CurrentViewer != null && sender == this.CurrentViewer);
			this.OnPrintCompleted();
		}

		// Token: 0x06004B60 RID: 19296 RVA: 0x001538AC File Offset: 0x00151AAC
		private bool ConvertToViewingMode(object value, out FlowDocumentReaderViewingMode mode)
		{
			bool result;
			if (value is FlowDocumentReaderViewingMode)
			{
				mode = (FlowDocumentReaderViewingMode)value;
				result = true;
			}
			else if (value is string)
			{
				string a = (string)value;
				if (a == FlowDocumentReaderViewingMode.Page.ToString())
				{
					mode = FlowDocumentReaderViewingMode.Page;
					result = true;
				}
				else if (a == FlowDocumentReaderViewingMode.TwoPage.ToString())
				{
					mode = FlowDocumentReaderViewingMode.TwoPage;
					result = true;
				}
				else if (a == FlowDocumentReaderViewingMode.Scroll.ToString())
				{
					mode = FlowDocumentReaderViewingMode.Scroll;
					result = true;
				}
				else
				{
					mode = FlowDocumentReaderViewingMode.Page;
					result = false;
				}
			}
			else
			{
				mode = FlowDocumentReaderViewingMode.Page;
				result = false;
			}
			return result;
		}

		// Token: 0x06004B61 RID: 19297 RVA: 0x00153944 File Offset: 0x00151B44
		private void ToggleFindToolBar(bool enable)
		{
			Invariant.Assert(enable == (this.FindToolBar == null));
			if (this._findButton != null && this._findButton.IsChecked != null && this._findButton.IsChecked.Value != enable)
			{
				this._findButton.IsChecked = new bool?(enable);
			}
			DocumentViewerHelper.ToggleFindToolBar(this._findToolBarHost, new EventHandler(this.OnFindInvoked), enable);
		}

		// Token: 0x06004B62 RID: 19298 RVA: 0x001539C0 File Offset: 0x00151BC0
		private static void CreateCommandBindings()
		{
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(FlowDocumentReader.ExecutedRoutedEventHandler);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(FlowDocumentReader.CanExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), FlowDocumentReader.SwitchViewingModeCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, "KeySwitchViewingMode", "KeySwitchViewingModeDisplayString");
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), ApplicationCommands.Find, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), ApplicationCommands.Print, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), ApplicationCommands.CancelPrint, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), NavigationCommands.PreviousPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), NavigationCommands.NextPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), NavigationCommands.FirstPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), NavigationCommands.LastPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), NavigationCommands.IncreaseZoom, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.OemPlus, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), NavigationCommands.DecreaseZoom, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.OemMinus, ModifierKeys.Control));
		}

		// Token: 0x06004B63 RID: 19299 RVA: 0x00153AE4 File Offset: 0x00151CE4
		private static void CanExecuteRoutedEventHandler(object target, CanExecuteRoutedEventArgs args)
		{
			FlowDocumentReader flowDocumentReader = target as FlowDocumentReader;
			Invariant.Assert(flowDocumentReader != null, "Target of CanExecuteRoutedEventHandler must be FlowDocumentReader.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (flowDocumentReader._printInProgress)
			{
				args.CanExecute = (args.Command == ApplicationCommands.CancelPrint);
				return;
			}
			if (args.Command == FlowDocumentReader.SwitchViewingModeCommand)
			{
				FlowDocumentReaderViewingMode mode;
				if (flowDocumentReader.ConvertToViewingMode(args.Parameter, out mode))
				{
					args.CanExecute = flowDocumentReader.CanSwitchToViewingMode(mode);
					return;
				}
				args.CanExecute = (args.Parameter == null);
				return;
			}
			else
			{
				if (args.Command == ApplicationCommands.Find)
				{
					args.CanExecute = flowDocumentReader.CanShowFindToolBar;
					return;
				}
				if (args.Command == ApplicationCommands.Print)
				{
					args.CanExecute = (flowDocumentReader.Document != null && flowDocumentReader.IsPrintEnabled);
					return;
				}
				if (args.Command == ApplicationCommands.CancelPrint)
				{
					args.CanExecute = false;
					return;
				}
				args.CanExecute = true;
				return;
			}
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x00153BCC File Offset: 0x00151DCC
		private static void ExecutedRoutedEventHandler(object target, ExecutedRoutedEventArgs args)
		{
			FlowDocumentReader flowDocumentReader = target as FlowDocumentReader;
			Invariant.Assert(flowDocumentReader != null, "Target of ExecutedRoutedEventHandler must be FlowDocumentReader.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (args.Command == FlowDocumentReader.SwitchViewingModeCommand)
			{
				flowDocumentReader.TrySwitchViewingMode(args.Parameter);
				return;
			}
			if (args.Command == ApplicationCommands.Find)
			{
				flowDocumentReader.OnFindCommand();
				return;
			}
			if (args.Command == ApplicationCommands.Print)
			{
				flowDocumentReader.OnPrintCommand();
				return;
			}
			if (args.Command == ApplicationCommands.CancelPrint)
			{
				flowDocumentReader.OnCancelPrintCommand();
				return;
			}
			if (args.Command == NavigationCommands.IncreaseZoom)
			{
				flowDocumentReader.OnIncreaseZoomCommand();
				return;
			}
			if (args.Command == NavigationCommands.DecreaseZoom)
			{
				flowDocumentReader.OnDecreaseZoomCommand();
				return;
			}
			if (args.Command == NavigationCommands.PreviousPage)
			{
				flowDocumentReader.OnPreviousPageCommand();
				return;
			}
			if (args.Command == NavigationCommands.NextPage)
			{
				flowDocumentReader.OnNextPageCommand();
				return;
			}
			if (args.Command == NavigationCommands.FirstPage)
			{
				flowDocumentReader.OnFirstPageCommand();
				return;
			}
			if (args.Command == NavigationCommands.LastPage)
			{
				flowDocumentReader.OnLastPageCommand();
				return;
			}
			Invariant.Assert(false, "Command not handled in ExecutedRoutedEventHandler.");
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x00153CD8 File Offset: 0x00151ED8
		private void TrySwitchViewingMode(object parameter)
		{
			FlowDocumentReaderViewingMode flowDocumentReaderViewingMode;
			if (!this.ConvertToViewingMode(parameter, out flowDocumentReaderViewingMode))
			{
				if (parameter != null)
				{
					return;
				}
				flowDocumentReaderViewingMode = (this.ViewingMode + 1) % (FlowDocumentReaderViewingMode)3;
			}
			while (!this.CanSwitchToViewingMode(flowDocumentReaderViewingMode))
			{
				flowDocumentReaderViewingMode = (flowDocumentReaderViewingMode + 1) % (FlowDocumentReaderViewingMode)3;
			}
			base.SetCurrentValueInternal(FlowDocumentReader.ViewingModeProperty, flowDocumentReaderViewingMode);
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x00153D21 File Offset: 0x00151F21
		private void OnPreviousPageCommand()
		{
			if (this.CurrentViewer != null)
			{
				this.CurrentViewer.PreviousPage();
			}
		}

		// Token: 0x06004B67 RID: 19303 RVA: 0x00153D36 File Offset: 0x00151F36
		private void OnNextPageCommand()
		{
			if (this.CurrentViewer != null)
			{
				this.CurrentViewer.NextPage();
			}
		}

		// Token: 0x06004B68 RID: 19304 RVA: 0x00153D4B File Offset: 0x00151F4B
		private void OnFirstPageCommand()
		{
			if (this.CurrentViewer != null)
			{
				this.CurrentViewer.FirstPage();
			}
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x00153D60 File Offset: 0x00151F60
		private void OnLastPageCommand()
		{
			if (this.CurrentViewer != null)
			{
				this.CurrentViewer.LastPage();
			}
		}

		// Token: 0x06004B6A RID: 19306 RVA: 0x00153D78 File Offset: 0x00151F78
		private void OnFindInvoked(object sender, EventArgs e)
		{
			TextEditor textEditor = this.TextEditor;
			FindToolBar findToolBar = this.FindToolBar;
			if (findToolBar != null && textEditor != null)
			{
				if (this.CurrentViewer != null && this.CurrentViewer is UIElement)
				{
					((UIElement)this.CurrentViewer).Focus();
				}
				ITextRange textRange = DocumentViewerHelper.Find(findToolBar, textEditor, textEditor.TextView, textEditor.TextView);
				if (textRange != null && !textRange.IsEmpty)
				{
					if (this.CurrentViewer != null)
					{
						this.CurrentViewer.ShowFindResult(textRange);
						return;
					}
				}
				else
				{
					DocumentViewerHelper.ShowFindUnsuccessfulMessage(findToolBar);
				}
			}
		}

		// Token: 0x06004B6B RID: 19307 RVA: 0x00153DFC File Offset: 0x00151FFC
		private void PreviewCanExecuteRoutedEventHandler(object target, CanExecuteRoutedEventArgs args)
		{
			if (args.Command == ApplicationCommands.Find)
			{
				args.CanExecute = false;
				args.Handled = true;
				return;
			}
			if (args.Command == ApplicationCommands.Print)
			{
				args.CanExecute = this.IsPrintEnabled;
				args.Handled = !this.IsPrintEnabled;
			}
		}

		// Token: 0x06004B6C RID: 19308 RVA: 0x00153E4D File Offset: 0x0015204D
		private static void KeyDownHandler(object sender, KeyEventArgs e)
		{
			DocumentViewerHelper.KeyDownHelper(e, ((FlowDocumentReader)sender)._findToolBarHost);
		}

		// Token: 0x06004B6D RID: 19309 RVA: 0x00153E60 File Offset: 0x00152060
		private static void ViewingModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			if (flowDocumentReader.CanSwitchToViewingMode((FlowDocumentReaderViewingMode)e.NewValue))
			{
				flowDocumentReader.SwitchViewingModeCore((FlowDocumentReaderViewingMode)e.NewValue);
			}
			else if (flowDocumentReader.IsInitialized)
			{
				throw new ArgumentException(SR.Get("FlowDocumentReaderViewingModeEnabledConflict"));
			}
			FlowDocumentReaderAutomationPeer flowDocumentReaderAutomationPeer = UIElementAutomationPeer.FromElement(flowDocumentReader) as FlowDocumentReaderAutomationPeer;
			if (flowDocumentReaderAutomationPeer != null)
			{
				flowDocumentReaderAutomationPeer.RaiseCurrentViewChangedEvent((FlowDocumentReaderViewingMode)e.NewValue, (FlowDocumentReaderViewingMode)e.OldValue);
			}
		}

		// Token: 0x06004B6E RID: 19310 RVA: 0x00153EF8 File Offset: 0x001520F8
		private static bool IsValidViewingMode(object o)
		{
			FlowDocumentReaderViewingMode flowDocumentReaderViewingMode = (FlowDocumentReaderViewingMode)o;
			return flowDocumentReaderViewingMode == FlowDocumentReaderViewingMode.Page || flowDocumentReaderViewingMode == FlowDocumentReaderViewingMode.TwoPage || flowDocumentReaderViewingMode == FlowDocumentReaderViewingMode.Scroll;
		}

		// Token: 0x06004B6F RID: 19311 RVA: 0x00153F1C File Offset: 0x0015211C
		private static void ViewingModeEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			if (!flowDocumentReader.IsPageViewEnabled && !flowDocumentReader.IsTwoPageViewEnabled && !flowDocumentReader.IsScrollViewEnabled)
			{
				throw new ArgumentException(SR.Get("FlowDocumentReaderCannotDisableAllViewingModes"));
			}
			if (flowDocumentReader.IsInitialized && !flowDocumentReader.CanSwitchToViewingMode(flowDocumentReader.ViewingMode))
			{
				throw new ArgumentException(SR.Get("FlowDocumentReaderViewingModeEnabledConflict"));
			}
			FlowDocumentReaderAutomationPeer flowDocumentReaderAutomationPeer = UIElementAutomationPeer.FromElement(flowDocumentReader) as FlowDocumentReaderAutomationPeer;
			if (flowDocumentReaderAutomationPeer != null)
			{
				flowDocumentReaderAutomationPeer.RaiseSupportedViewsChangedEvent(e);
			}
		}

		// Token: 0x06004B70 RID: 19312 RVA: 0x00153FA8 File Offset: 0x001521A8
		private static void IsFindEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			if (!flowDocumentReader.CanShowFindToolBar && flowDocumentReader.FindToolBar != null)
			{
				flowDocumentReader.ToggleFindToolBar(false);
			}
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x06004B71 RID: 19313 RVA: 0x00153FEC File Offset: 0x001521EC
		private static void IsPrintEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x06004B72 RID: 19314 RVA: 0x0015401C File Offset: 0x0015221C
		private static void DocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			flowDocumentReader.DocumentChanged((FlowDocument)e.OldValue, (FlowDocument)e.NewValue);
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x06004B73 RID: 19315 RVA: 0x00154068 File Offset: 0x00152268
		private static void ZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			if (!DoubleUtil.AreClose((double)e.OldValue, (double)e.NewValue))
			{
				flowDocumentReader.SetValue(FlowDocumentReader.CanIncreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.GreaterThan(flowDocumentReader.MaxZoom, flowDocumentReader.Zoom)));
				flowDocumentReader.SetValue(FlowDocumentReader.CanDecreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.LessThan(flowDocumentReader.MinZoom, flowDocumentReader.Zoom)));
			}
		}

		// Token: 0x06004B74 RID: 19316 RVA: 0x001540F4 File Offset: 0x001522F4
		private static object CoerceZoom(DependencyObject d, object value)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			double value2 = (double)value;
			double maxZoom = flowDocumentReader.MaxZoom;
			if (DoubleUtil.LessThan(maxZoom, value2))
			{
				return maxZoom;
			}
			double minZoom = flowDocumentReader.MinZoom;
			if (DoubleUtil.GreaterThan(minZoom, value2))
			{
				return minZoom;
			}
			return value;
		}

		// Token: 0x06004B75 RID: 19317 RVA: 0x00154154 File Offset: 0x00152354
		private static void MaxZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			flowDocumentReader.CoerceValue(FlowDocumentReader.ZoomProperty);
			flowDocumentReader.SetValue(FlowDocumentReader.CanIncreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.GreaterThan(flowDocumentReader.MaxZoom, flowDocumentReader.Zoom)));
		}

		// Token: 0x06004B76 RID: 19318 RVA: 0x001541A8 File Offset: 0x001523A8
		private static object CoerceMaxZoom(DependencyObject d, object value)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			double minZoom = flowDocumentReader.MinZoom;
			if ((double)value >= minZoom)
			{
				return value;
			}
			return minZoom;
		}

		// Token: 0x06004B77 RID: 19319 RVA: 0x001541E8 File Offset: 0x001523E8
		private static void MinZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			flowDocumentReader.CoerceValue(FlowDocumentReader.MaxZoomProperty);
			flowDocumentReader.CoerceValue(FlowDocumentReader.ZoomProperty);
			flowDocumentReader.SetValue(FlowDocumentReader.CanDecreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.LessThan(flowDocumentReader.MinZoom, flowDocumentReader.Zoom)));
		}

		// Token: 0x06004B78 RID: 19320 RVA: 0x00154248 File Offset: 0x00152448
		private static bool ZoomValidateValue(object o)
		{
			double num = (double)o;
			return !double.IsNaN(num) && !double.IsInfinity(num) && DoubleUtil.GreaterThan(num, 0.0);
		}

		// Token: 0x06004B79 RID: 19321 RVA: 0x00154280 File Offset: 0x00152480
		private static void UpdateCaretElement(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			if (flowDocumentReader.Selection != null)
			{
				CaretElement caretElement = flowDocumentReader.Selection.CaretElement;
				if (caretElement != null)
				{
					caretElement.InvalidateVisual();
				}
			}
		}

		// Token: 0x17001268 RID: 4712
		// (get) Token: 0x06004B7A RID: 19322 RVA: 0x001542B1 File Offset: 0x001524B1
		private bool CanShowFindToolBar
		{
			get
			{
				return this._findToolBarHost != null && this.IsFindEnabled && this.Document != null;
			}
		}

		// Token: 0x17001269 RID: 4713
		// (get) Token: 0x06004B7B RID: 19323 RVA: 0x001542D0 File Offset: 0x001524D0
		private TextEditor TextEditor
		{
			get
			{
				TextEditor result = null;
				IFlowDocumentViewer currentViewer = this.CurrentViewer;
				if (currentViewer != null && currentViewer.TextSelection != null)
				{
					result = currentViewer.TextSelection.TextEditor;
				}
				return result;
			}
		}

		// Token: 0x1700126A RID: 4714
		// (get) Token: 0x06004B7C RID: 19324 RVA: 0x001542FE File Offset: 0x001524FE
		private FindToolBar FindToolBar
		{
			get
			{
				if (this._findToolBarHost == null)
				{
					return null;
				}
				return this._findToolBarHost.Child as FindToolBar;
			}
		}

		// Token: 0x1700126B RID: 4715
		// (get) Token: 0x06004B7D RID: 19325 RVA: 0x0015431A File Offset: 0x0015251A
		private IFlowDocumentViewer CurrentViewer
		{
			get
			{
				if (this._contentHost != null)
				{
					return (IFlowDocumentViewer)this._contentHost.Child;
				}
				return null;
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. Use the <see cref="P:System.Windows.Controls.FlowDocumentReader.Document" /> property to add a <see cref="T:System.Windows.Documents.FlowDocument" /> as the content child for the <see cref="T:System.Windows.Controls.FlowDocumentReader" />.</summary>
		/// <param name="value">An object to add as a child. </param>
		// Token: 0x06004B7E RID: 19326 RVA: 0x00154338 File Offset: 0x00152538
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.Document != null)
			{
				throw new ArgumentException(SR.Get("FlowDocumentReaderCanHaveOnlyOneChild"));
			}
			if (!(value is FlowDocument))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(FlowDocument)
				}), "value");
			}
			this.Document = (value as FlowDocument);
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="text">A string to add to the object. </param>
		// Token: 0x06004B7F RID: 19327 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06004B80 RID: 19328 RVA: 0x001543B0 File Offset: 0x001525B0
		CustomJournalStateInternal IJournalState.GetJournalState(JournalReason journalReason)
		{
			int contentPosition = -1;
			LogicalDirection contentPositionDirection = LogicalDirection.Forward;
			IFlowDocumentViewer currentViewer = this.CurrentViewer;
			if (currentViewer != null)
			{
				TextPointer textPointer = currentViewer.ContentPosition as TextPointer;
				if (textPointer != null)
				{
					contentPosition = textPointer.Offset;
					contentPositionDirection = textPointer.LogicalDirection;
				}
			}
			return new FlowDocumentReader.JournalState(contentPosition, contentPositionDirection, this.Zoom, this.ViewingMode);
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x001543FC File Offset: 0x001525FC
		void IJournalState.RestoreJournalState(CustomJournalStateInternal state)
		{
			FlowDocumentReader.JournalState journalState = state as FlowDocumentReader.JournalState;
			if (state != null)
			{
				base.SetCurrentValueInternal(FlowDocumentReader.ZoomProperty, journalState.Zoom);
				base.SetCurrentValueInternal(FlowDocumentReader.ViewingModeProperty, journalState.ViewingMode);
				if (journalState.ContentPosition != -1)
				{
					IFlowDocumentViewer currentViewer = this.CurrentViewer;
					FlowDocument document = this.Document;
					if (currentViewer != null && document != null)
					{
						TextContainer textContainer = document.StructuralCache.TextContainer;
						if (journalState.ContentPosition <= textContainer.SymbolCount)
						{
							TextPointer contentPosition = textContainer.CreatePointerAtOffset(journalState.ContentPosition, journalState.ContentPositionDirection);
							currentViewer.ContentPosition = contentPosition;
						}
					}
				}
			}
		}

		// Token: 0x1700126C RID: 4716
		// (get) Token: 0x06004B82 RID: 19330 RVA: 0x00154492 File Offset: 0x00152692
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return FlowDocumentReader._dType;
			}
		}

		// Token: 0x1700126D RID: 4717
		// (get) Token: 0x06004B83 RID: 19331 RVA: 0x00154499 File Offset: 0x00152699
		private static ResourceKey PageViewStyleKey
		{
			get
			{
				if (FlowDocumentReader._pageViewStyleKey == null)
				{
					FlowDocumentReader._pageViewStyleKey = new ComponentResourceKey(typeof(PresentationUIStyleResources), "PUIPageViewStyleKey");
				}
				return FlowDocumentReader._pageViewStyleKey;
			}
		}

		// Token: 0x1700126E RID: 4718
		// (get) Token: 0x06004B84 RID: 19332 RVA: 0x001544C0 File Offset: 0x001526C0
		private static ResourceKey TwoPageViewStyleKey
		{
			get
			{
				if (FlowDocumentReader._twoPageViewStyleKey == null)
				{
					FlowDocumentReader._twoPageViewStyleKey = new ComponentResourceKey(typeof(PresentationUIStyleResources), "PUITwoPageViewStyleKey");
				}
				return FlowDocumentReader._twoPageViewStyleKey;
			}
		}

		// Token: 0x1700126F RID: 4719
		// (get) Token: 0x06004B85 RID: 19333 RVA: 0x001544E7 File Offset: 0x001526E7
		private static ResourceKey ScrollViewStyleKey
		{
			get
			{
				if (FlowDocumentReader._scrollViewStyleKey == null)
				{
					FlowDocumentReader._scrollViewStyleKey = new ComponentResourceKey(typeof(PresentationUIStyleResources), "PUIScrollViewStyleKey");
				}
				return FlowDocumentReader._scrollViewStyleKey;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.ViewingMode" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.ViewingMode" /> dependency property.</returns>
		// Token: 0x04002AB2 RID: 10930
		public static readonly DependencyProperty ViewingModeProperty = DependencyProperty.Register("ViewingMode", typeof(FlowDocumentReaderViewingMode), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(FlowDocumentReaderViewingMode.Page, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FlowDocumentReader.ViewingModeChanged)), new ValidateValueCallback(FlowDocumentReader.IsValidViewingMode));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.IsPageViewEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.IsPageViewEnabled" /> dependency property.</returns>
		// Token: 0x04002AB3 RID: 10931
		public static readonly DependencyProperty IsPageViewEnabledProperty = DependencyProperty.Register("IsPageViewEnabled", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(FlowDocumentReader.ViewingModeEnabledChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.IsTwoPageViewEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.IsTwoPageViewEnabled" /> dependency property.</returns>
		// Token: 0x04002AB4 RID: 10932
		public static readonly DependencyProperty IsTwoPageViewEnabledProperty = DependencyProperty.Register("IsTwoPageViewEnabled", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(FlowDocumentReader.ViewingModeEnabledChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.IsScrollViewEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.IsScrollViewEnabled" /> dependency property.</returns>
		// Token: 0x04002AB5 RID: 10933
		public static readonly DependencyProperty IsScrollViewEnabledProperty = DependencyProperty.Register("IsScrollViewEnabled", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(FlowDocumentReader.ViewingModeEnabledChanged)));

		// Token: 0x04002AB6 RID: 10934
		private static readonly DependencyPropertyKey PageCountPropertyKey = DependencyProperty.RegisterReadOnly("PageCount", typeof(int), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.PageCount" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.PageCount" /> dependency property.</returns>
		// Token: 0x04002AB7 RID: 10935
		public static readonly DependencyProperty PageCountProperty = FlowDocumentReader.PageCountPropertyKey.DependencyProperty;

		// Token: 0x04002AB8 RID: 10936
		private static readonly DependencyPropertyKey PageNumberPropertyKey = DependencyProperty.RegisterReadOnly("PageNumber", typeof(int), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.PageNumber" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.PageNumber" /> dependency property.</returns>
		// Token: 0x04002AB9 RID: 10937
		public static readonly DependencyProperty PageNumberProperty = FlowDocumentReader.PageNumberPropertyKey.DependencyProperty;

		// Token: 0x04002ABA RID: 10938
		private static readonly DependencyPropertyKey CanGoToPreviousPagePropertyKey = DependencyProperty.RegisterReadOnly("CanGoToPreviousPage", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.CanGoToPreviousPage" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.CanGoToPreviousPage" /> dependency property.</returns>
		// Token: 0x04002ABB RID: 10939
		public static readonly DependencyProperty CanGoToPreviousPageProperty = FlowDocumentReader.CanGoToPreviousPagePropertyKey.DependencyProperty;

		// Token: 0x04002ABC RID: 10940
		private static readonly DependencyPropertyKey CanGoToNextPagePropertyKey = DependencyProperty.RegisterReadOnly("CanGoToNextPage", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.CanGoToNextPage" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.CanGoToNextPage" /> dependency property.</returns>
		// Token: 0x04002ABD RID: 10941
		public static readonly DependencyProperty CanGoToNextPageProperty = FlowDocumentReader.CanGoToNextPagePropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.IsFindEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.IsFindEnabled" /> dependency property.</returns>
		// Token: 0x04002ABE RID: 10942
		public static readonly DependencyProperty IsFindEnabledProperty = DependencyProperty.Register("IsFindEnabled", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(FlowDocumentReader.IsFindEnabledChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.IsPrintEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.IsPrintEnabled" /> dependency property.</returns>
		// Token: 0x04002ABF RID: 10943
		public static readonly DependencyProperty IsPrintEnabledProperty = DependencyProperty.Register("IsPrintEnabled", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(FlowDocumentReader.IsPrintEnabledChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.Document" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.Document" /> dependency property.</returns>
		// Token: 0x04002AC0 RID: 10944
		public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(FlowDocument), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(FlowDocumentReader.DocumentChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.Zoom" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.Zoom" /> dependency property.</returns>
		// Token: 0x04002AC1 RID: 10945
		public static readonly DependencyProperty ZoomProperty = FlowDocumentPageViewer.ZoomProperty.AddOwner(typeof(FlowDocumentReader), new FrameworkPropertyMetadata(100.0, new PropertyChangedCallback(FlowDocumentReader.ZoomChanged), new CoerceValueCallback(FlowDocumentReader.CoerceZoom)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.MaxZoom" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.MaxZoom" /> dependency property.</returns>
		// Token: 0x04002AC2 RID: 10946
		public static readonly DependencyProperty MaxZoomProperty = FlowDocumentPageViewer.MaxZoomProperty.AddOwner(typeof(FlowDocumentReader), new FrameworkPropertyMetadata(200.0, new PropertyChangedCallback(FlowDocumentReader.MaxZoomChanged), new CoerceValueCallback(FlowDocumentReader.CoerceMaxZoom)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.MinZoom" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.MinZoom" /> dependency property.</returns>
		// Token: 0x04002AC3 RID: 10947
		public static readonly DependencyProperty MinZoomProperty = FlowDocumentPageViewer.MinZoomProperty.AddOwner(typeof(FlowDocumentReader), new FrameworkPropertyMetadata(80.0, new PropertyChangedCallback(FlowDocumentReader.MinZoomChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.ZoomIncrement" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.ZoomIncrement" /> dependency property.</returns>
		// Token: 0x04002AC4 RID: 10948
		public static readonly DependencyProperty ZoomIncrementProperty = FlowDocumentPageViewer.ZoomIncrementProperty.AddOwner(typeof(FlowDocumentReader));

		// Token: 0x04002AC5 RID: 10949
		private static readonly DependencyPropertyKey CanIncreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly("CanIncreaseZoom", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.CanIncreaseZoom" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.CanIncreaseZoom" /> dependency property.</returns>
		// Token: 0x04002AC6 RID: 10950
		public static readonly DependencyProperty CanIncreaseZoomProperty = FlowDocumentReader.CanIncreaseZoomPropertyKey.DependencyProperty;

		// Token: 0x04002AC7 RID: 10951
		private static readonly DependencyPropertyKey CanDecreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly("CanDecreaseZoom", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.CanDecreaseZoom" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.CanDecreaseZoom" /> dependency property.</returns>
		// Token: 0x04002AC8 RID: 10952
		public static readonly DependencyProperty CanDecreaseZoomProperty = FlowDocumentReader.CanDecreaseZoomPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.SelectionBrush" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.SelectionBrush" /> dependency property.</returns>
		// Token: 0x04002AC9 RID: 10953
		public static readonly DependencyProperty SelectionBrushProperty = TextBoxBase.SelectionBrushProperty.AddOwner(typeof(FlowDocumentReader));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.SelectionOpacity" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.SelectionOpacity" /> dependency property.</returns>
		// Token: 0x04002ACA RID: 10954
		public static readonly DependencyProperty SelectionOpacityProperty = TextBoxBase.SelectionOpacityProperty.AddOwner(typeof(FlowDocumentReader));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.IsSelectionActive" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.IsSelectionActive" /> dependency property.</returns>
		// Token: 0x04002ACB RID: 10955
		public static readonly DependencyProperty IsSelectionActiveProperty = TextBoxBase.IsSelectionActiveProperty.AddOwner(typeof(FlowDocumentReader));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentReader.IsInactiveSelectionHighlightEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentReader.IsInactiveSelectionHighlightEnabled" /> dependency property.</returns>
		// Token: 0x04002ACC RID: 10956
		public static readonly DependencyProperty IsInactiveSelectionHighlightEnabledProperty = TextBoxBase.IsInactiveSelectionHighlightEnabledProperty.AddOwner(typeof(FlowDocumentReader));

		/// <summary>Gets the value that represents the Switch Viewing Mode command.</summary>
		/// <returns>The command.</returns>
		// Token: 0x04002ACD RID: 10957
		public static readonly RoutedUICommand SwitchViewingModeCommand = new RoutedUICommand(SR.Get("SwitchViewingMode"), "SwitchViewingMode", typeof(FlowDocumentReader), null);

		// Token: 0x04002ACE RID: 10958
		private Decorator _contentHost;

		// Token: 0x04002ACF RID: 10959
		private Decorator _findToolBarHost;

		// Token: 0x04002AD0 RID: 10960
		private ToggleButton _findButton;

		// Token: 0x04002AD1 RID: 10961
		private ReaderPageViewer _pageViewer;

		// Token: 0x04002AD2 RID: 10962
		private ReaderTwoPageViewer _twoPageViewer;

		// Token: 0x04002AD3 RID: 10963
		private ReaderScrollViewer _scrollViewer;

		// Token: 0x04002AD4 RID: 10964
		private bool _documentAsLogicalChild;

		// Token: 0x04002AD5 RID: 10965
		private bool _printInProgress;

		// Token: 0x04002AD6 RID: 10966
		private const string _contentHostTemplateName = "PART_ContentHost";

		// Token: 0x04002AD7 RID: 10967
		private const string _findToolBarHostTemplateName = "PART_FindToolBarHost";

		// Token: 0x04002AD8 RID: 10968
		private const string _findButtonTemplateName = "FindButton";

		// Token: 0x04002AD9 RID: 10969
		private static DependencyObjectType _dType;

		// Token: 0x04002ADA RID: 10970
		private static ComponentResourceKey _pageViewStyleKey;

		// Token: 0x04002ADB RID: 10971
		private static ComponentResourceKey _twoPageViewStyleKey;

		// Token: 0x04002ADC RID: 10972
		private static ComponentResourceKey _scrollViewStyleKey;

		// Token: 0x0200096F RID: 2415
		[Serializable]
		private class JournalState : CustomJournalStateInternal
		{
			// Token: 0x06008772 RID: 34674 RVA: 0x0024FE4A File Offset: 0x0024E04A
			public JournalState(int contentPosition, LogicalDirection contentPositionDirection, double zoom, FlowDocumentReaderViewingMode viewingMode)
			{
				this.ContentPosition = contentPosition;
				this.ContentPositionDirection = contentPositionDirection;
				this.Zoom = zoom;
				this.ViewingMode = viewingMode;
			}

			// Token: 0x04004444 RID: 17476
			public int ContentPosition;

			// Token: 0x04004445 RID: 17477
			public LogicalDirection ContentPositionDirection;

			// Token: 0x04004446 RID: 17478
			public double Zoom;

			// Token: 0x04004447 RID: 17479
			public FlowDocumentReaderViewingMode ViewingMode;
		}
	}
}
