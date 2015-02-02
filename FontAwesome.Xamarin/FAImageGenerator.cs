using CoreGraphics;
using CoreText;
using Foundation;
using System.Runtime.InteropServices;
using ObjCRuntime;
using UIKit;
using System;

namespace FontAwesome.iOS
{
	public interface IFAImageGenerator {
		UIImage CreateImage (string icon);
		UIImage CreateImage (string icon, nfloat sizeOverride);
	}

	public class FAImageGenerator : IDisposable, IFAImageGenerator
	{
		private nfloat size;

		public nfloat Size { 
			get { return size; }
			set { 
				size = value;
			}
		}

		public UIEdgeInsets Insets { get; set; }

		public nfloat StrokeWidth { get; set; }

		public bool IsSquare { get; set; }

		public bool IsPadded { get; set; }

		private CGColor[] cgColors;
		private UIColor[] colors;

		public UIColor[] Colors { 
			get { return colors; } 
			set {
				colors = value;
				cgColors = new CGColor[colors.Length];
				for (var i = 0; i < colors.Length; ++i) {
					cgColors [i] = colors [i].CGColor;
				}
			}
		}

		public UIColor StrokeColor { get; set; }

		public UIImageRenderingMode RenderingMode { get; set; }

		public FAImageGenerator ()
		{
			Size = 32.0f;
			Insets = UIEdgeInsets.Zero;
			StrokeWidth = 0f;
			IsSquare = true;
			IsPadded = true;
			Colors = new UIColor[] { UIColor.DarkGray };
			StrokeColor = UIColor.Black;
			RenderingMode = UIImageRenderingMode.Automatic;
		}

		private static CTFont ctfont;

		static FAImageGenerator ()
		{
			using (var bundle = NSBundle.MainBundle) {
				var path = bundle.PathForResource ("FontAwesome", "ttf");
				using (var provider = new CGDataProvider (path))
				using (var font = CGFont.CreateFromProvider (provider))
				using (var fontDescriptor = new CTFontDescriptor (new CTFontDescriptorAttributes ())) {
					ctfont = new CTFont (font, 0, CGAffineTransform.MakeIdentity(), fontDescriptor);
				}
			}
		}

		public void Dispose ()
		{
			StrokeColor.Dispose ();
			foreach (var color in colors) {
				color.Dispose ();
			}
			foreach (var cgcolor in cgColors) {
				cgcolor.Dispose ();
			}
		}

		public UIImage CreateImage (string icon)
		{
			return CreateImage (icon, Size);
		}

		public UIImage CreateImage (string icon, nfloat sizeOverride)
		{
			using (var path = CreatePath (icon, sizeOverride)) {
				return CreateImageFromPath (path);
			}
		}

		public CGPath CreatePath (string icon, nfloat size)
		{
			var paddedSize = size - StrokeWidth;
			var width = IsSquare ? paddedSize : nfloat.MaxValue;
			return CreatePathForIcon (icon, paddedSize, width);
		}

		private CGPath CreatePathForIcon (string icon, nfloat height, nfloat maxWidth)
		{
			var path = CreatePathForIcon (icon, height);
			var bounds = path.BoundingBox;
			if (bounds.Width > maxWidth) {
				var scaledPath = CreateScaledPath (path, maxWidth / bounds.Width);
				path.Dispose ();
				return scaledPath;
			} else {
				return path;
			}
		}

		private CGPath CreateScaledPath (CGPath path, nfloat factor)
		{
			var scale = CGAffineTransform.MakeScale (factor, factor);
			return path.CopyByTransformingPath (scale);
		}

		private CGPath CreatePathForIcon (string icon, nfloat height)
		{
			var fontHeight = ctfont.Size;
			var scale = CGAffineTransform.MakeScale (height / fontHeight, height / fontHeight);
			scale.Translate (0, ctfont.DescentMetric);
			return ctfont.GetPathForGlyph (GlyphForIcon (icon), ref scale);
		}

		private char[] getGlyphCharBuffer = new char[1];
		ushort[] getGlyphGlyphBuffer = new ushort[1];
		[DllImport(Constants.CoreTextLibrary)]
		private static extern bool CTFontGetGlyphsForCharacters (
			IntPtr font, 
			[In, MarshalAs(UnmanagedType.LPWStr)] string characters, 
			[Out] ushort[] glyphs, 
			nint count);

		private ushort GlyphForIcon (string icon)
		{
			getGlyphCharBuffer [0] = icon [0];
			getGlyphGlyphBuffer [0] = 0;
			CTFontGetGlyphsForCharacters (ctfont.Handle, icon, getGlyphGlyphBuffer, 1);
			return getGlyphGlyphBuffer [0];
		}

		// Prevent +1 on values that are slightly too big (e.g. 24.000001).
		private const float EPSILON = 0.01f;

		private CGSize RoundImageSize (CGSize size)
		{
			return new CGSize (
				(float)Math.Ceiling (size.Width - EPSILON),
				(float)Math.Ceiling (size.Height - EPSILON));
		}

		public UIImage CreateImageFromPath (CGPath path)
		{
			var bounds = path.BoundingBox;
			var imageSize = bounds.Size;
			var offset = CGPoint.Empty;
			if (IsPadded) {
				imageSize.Height = Size;
				imageSize.Width += StrokeWidth;
			} else {
				// remove padding
				offset.X = -bounds.X;
				offset.Y = -bounds.Y;
				if (imageSize.Height > Size) {
					// NIKFontAwesomeIconSun and NIKFontAwesomeIconLinux
					imageSize.Height = Size;
				}
			}
			imageSize = RoundImageSize (imageSize);
			if (IsSquare) {
				var diff = imageSize.Height - imageSize.Width;
				if (diff > 0) {
					offset.X += 0.5f * diff;
					imageSize.Width = imageSize.Height;
				} else {
					offset.Y += 0.5f * -diff;
					imageSize.Height = imageSize.Width;
				}
			}
			;
			var padding = StrokeWidth * 0.5f;
			offset.X += padding + Insets.Left;
			offset.Y += padding + Insets.Bottom;
			imageSize.Width += Insets.Left + Insets.Right;
			imageSize.Height += Insets.Top + Insets.Bottom;

			UIGraphics.BeginImageContextWithOptions (imageSize, false, 0f);
			using (var context = UIGraphics.GetCurrentContext ()) {
				context.TranslateCTM (0, imageSize.Height);
				context.ScaleCTM (1, -1);
				RenderInContext (context, path, bounds, offset);
				var image = UIGraphics.GetImageFromCurrentImageContext ();
				UIGraphics.EndImageContext ();
				if (image.RenderingMode != RenderingMode) {
					image = image.ImageWithRenderingMode (RenderingMode);
				}
				return image;
			}
		}

		private void RenderInContext (CGContext context, CGPath path, CGRect bounds, CGPoint offset)
		{
			context.TranslateCTM (offset.X, offset.Y);
			context.AddPath (path);

			if (Colors.Length > 1) {
				context.SaveState ();
				context.Clip ();
				RenderGradientInRect (context, bounds);
				context.RestoreState ();
			} else {
				context.SetFillColor (cgColors [0]);
				context.FillPath ();
			}
			context.AddPath (path);

			if (StrokeColor != null && StrokeWidth > 0.0f) {
				context.SetStrokeColor (StrokeColor.CGColor);
				context.SetLineWidth (StrokeWidth);
				context.StrokePath ();
			}
		}

		private void RenderGradientInRect (CGContext context, CGRect bounds)
		{
			var n = Colors.Length;
			nfloat[] locations = new nfloat[n];
			for (var i = 0; i < n; i++) {
				locations [i] = (nfloat)i / (n - 1);
			}
			using (var gradient = new CGGradient (null, cgColors, locations)) {
				var topLeft = new CGPoint (bounds.GetMinX (), bounds.GetMinY ());
				var bottomLeft = new CGPoint (bounds.GetMinX (), bounds.GetMaxY ());
				context.DrawLinearGradient (gradient, topLeft, bottomLeft, 0);
			}
		}

	}
}
