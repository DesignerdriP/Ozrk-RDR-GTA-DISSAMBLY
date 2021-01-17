using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using MS.Internal;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Renders the contained 3-D content within the 2-D layout bounds of the <see cref="T:System.Windows.Controls.Viewport3D" /> element.  </summary>
	// Token: 0x0200055A RID: 1370
	[ContentProperty("Children")]
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class Viewport3D : FrameworkElement, IAddChild
	{
		// Token: 0x060059C6 RID: 22982 RVA: 0x0018BDFC File Offset: 0x00189FFC
		static Viewport3D()
		{
			UIElement.ClipToBoundsProperty.OverrideMetadata(typeof(Viewport3D), new PropertyMetadata(BooleanBoxes.TrueBox));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Viewport3D" /> class.</summary>
		// Token: 0x060059C7 RID: 22983 RVA: 0x0018BE94 File Offset: 0x0018A094
		public Viewport3D()
		{
			this._viewport3DVisual = new Viewport3DVisual();
			this._viewport3DVisual.CanBeInheritanceContext = false;
			base.AddVisualChild(this._viewport3DVisual);
			base.SetValue(Viewport3D.ChildrenPropertyKey, this._viewport3DVisual.Children);
			this._viewport3DVisual.SetInheritanceContextForChildren(this);
		}

		// Token: 0x060059C8 RID: 22984 RVA: 0x0018BEEC File Offset: 0x0018A0EC
		private static void OnCameraChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Viewport3D viewport3D = (Viewport3D)d;
			if (!e.IsASubPropertyChange)
			{
				viewport3D._viewport3DVisual.Camera = (Camera)e.NewValue;
			}
		}

		/// <summary>Gets or sets a camera object that projects the 3-D contents of the <see cref="T:System.Windows.Controls.Viewport3D" /> to the 2-D surface of the <see cref="T:System.Windows.Controls.Viewport3D" />.  </summary>
		/// <returns>The camera that projects the 3-D contents to the 2-D surface.</returns>
		// Token: 0x170015DA RID: 5594
		// (get) Token: 0x060059C9 RID: 22985 RVA: 0x0018BF20 File Offset: 0x0018A120
		// (set) Token: 0x060059CA RID: 22986 RVA: 0x0018BF32 File Offset: 0x0018A132
		public Camera Camera
		{
			get
			{
				return (Camera)base.GetValue(Viewport3D.CameraProperty);
			}
			set
			{
				base.SetValue(Viewport3D.CameraProperty, value);
			}
		}

		/// <summary>Gets a collection of the <see cref="T:System.Windows.Media.Media3D.Visual3D" /> children of the <see cref="T:System.Windows.Controls.Viewport3D" />.  </summary>
		/// <returns>A collection of the <see cref="T:System.Windows.Media.Media3D.Visual3D" /> children of the <see cref="T:System.Windows.Controls.Viewport3D" />.</returns>
		// Token: 0x170015DB RID: 5595
		// (get) Token: 0x060059CB RID: 22987 RVA: 0x0018BF40 File Offset: 0x0018A140
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Visual3DCollection Children
		{
			get
			{
				return (Visual3DCollection)base.GetValue(Viewport3D.ChildrenProperty);
			}
		}

		/// <summary>Creates and returns a <see cref="T:System.Windows.Automation.Peers.Viewport3DAutomationPeer" /> object for this <see cref="T:System.Windows.Controls.Viewport3D" />.</summary>
		/// <returns>
		///     <see cref="T:System.Windows.Automation.Peers.Viewport3DAutomationPeer" /> object for this <see cref="T:System.Windows.Controls.Viewport3D" />.</returns>
		// Token: 0x060059CC RID: 22988 RVA: 0x0018BF52 File Offset: 0x0018A152
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new Viewport3DAutomationPeer(this);
		}

		/// <summary>Causes the <see cref="T:System.Windows.Controls.Viewport3D" /> to arrange its visual content to fit a specified size. </summary>
		/// <param name="finalSize">Size that <see cref="T:System.Windows.Controls.Viewport3D" /> will assume.</param>
		/// <returns>The final size of the arranged <see cref="T:System.Windows.Controls.Viewport3D" />.</returns>
		// Token: 0x060059CD RID: 22989 RVA: 0x0018BF5C File Offset: 0x0018A15C
		protected override Size ArrangeOverride(Size finalSize)
		{
			Rect viewport = new Rect(default(Point), finalSize);
			this._viewport3DVisual.Viewport = viewport;
			return finalSize;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Media.Visual" /> at a specified position in the <see cref="P:System.Windows.Controls.Viewport3D.Children" /> collection of the <see cref="T:System.Windows.Controls.Viewport3D" />.</summary>
		/// <param name="index">Position of the element in the collection.</param>
		/// <returns>Visual at the specified position in the <see cref="P:System.Windows.Controls.Viewport3D.Children" /> collection.</returns>
		// Token: 0x060059CE RID: 22990 RVA: 0x0018BF87 File Offset: 0x0018A187
		protected override Visual GetVisualChild(int index)
		{
			if (index == 0)
			{
				return this._viewport3DVisual;
			}
			throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
		}

		/// <summary>Gets an integer that represents the number of <see cref="T:System.Windows.Media.Visual" /> objects in the <see cref="P:System.Windows.Media.Media3D.ModelVisual3D.Children" /> collection of the <see cref="T:System.Windows.Media.Media3D.Visual3D" />.</summary>
		/// <returns>Integer that represents the number of Visuals in the Children collection of the <see cref="T:System.Windows.Media.Media3D.Visual3D" />.</returns>
		// Token: 0x170015DC RID: 5596
		// (get) Token: 0x060059CF RID: 22991 RVA: 0x00016748 File Offset: 0x00014948
		protected override int VisualChildrenCount
		{
			get
			{
				return 1;
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value">   An object to add as a child.</param>
		// Token: 0x060059D0 RID: 22992 RVA: 0x0018BFB0 File Offset: 0x0018A1B0
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Visual3D visual3D = value as Visual3D;
			if (visual3D == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(Visual3D)
				}), "value");
			}
			this.Children.Add(visual3D);
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="text">   A string to add to the object.</param>
		// Token: 0x060059D1 RID: 22993 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Viewport3D.Camera" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Viewport3D.Camera" /> dependency property.</returns>
		// Token: 0x04002F24 RID: 12068
		public static readonly DependencyProperty CameraProperty = Viewport3DVisual.CameraProperty.AddOwner(typeof(Viewport3D), new FrameworkPropertyMetadata(FreezableOperations.GetAsFrozen(new PerspectiveCamera()), new PropertyChangedCallback(Viewport3D.OnCameraChanged)));

		// Token: 0x04002F25 RID: 12069
		private static readonly DependencyPropertyKey ChildrenPropertyKey = DependencyProperty.RegisterReadOnly("Children", typeof(Visual3DCollection), typeof(Viewport3D), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Viewport3D.Children" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Viewport3D.Children" /> dependency property.</returns>
		// Token: 0x04002F26 RID: 12070
		public static readonly DependencyProperty ChildrenProperty = Viewport3D.ChildrenPropertyKey.DependencyProperty;

		// Token: 0x04002F27 RID: 12071
		private readonly Viewport3DVisual _viewport3DVisual;
	}
}
