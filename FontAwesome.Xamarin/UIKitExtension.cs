using System;
using UIKit;
using Foundation;
using CoreGraphics;

namespace FontAwesome.iOS
{
	public static class UIKitExtension
	{
		public static void SetIcon(this UILabel label, string icon, nfloat size)
		{
			label.Font = FAIcons.Font (size);
			label.Text = icon;
		}

		public static void SetIcon(this UILabel label, string icon)
		{
			SetIcon (label, icon, label.Font.PointSize);
		}

		private static FAImageGenerator generator = new FAImageGenerator();
		public static void AppendIcon(this UILabel label, string icon)
		{
			using (var text = new NSMutableAttributedString ()) {
				var iconStr = text.FromTextAttachment (label.IconAttachment(icon));
				text.Append (iconStr);
				label.AttributedText = text;
			}
		}

		public static void AppendText(this UILabel label, string text)
		{
			using (var labelText = new NSMutableAttributedString(label.AttributedText))
			using (var textToAppend = new NSAttributedString(text)) {
				labelText.Append(textToAppend);
				label.AttributedText = labelText;
			}
		}

		public static void SetIconBeforeText(this UILabel label, string icon, string text)
		{
			using (var finalText = new NSMutableAttributedString ())
			using (var iconAttachment = label.IconAttachment (icon))
			using (var iconText = finalText.FromTextAttachment (iconAttachment))
			using (var labelText = new NSAttributedString (text)) {
				finalText.Append (iconText);
				finalText.Append (labelText);
				label.AttributedText = finalText;
			}
		}

		public static void SetIconAfterText(this UILabel label, string icon, string text)
		{
			using (var finalText = new NSMutableAttributedString ())
			using (var iconAttachment = label.IconAttachment (icon))
			using (var iconText = finalText.FromTextAttachment (iconAttachment))
			using (var labelText = new NSAttributedString (text)) {
				finalText.Append (labelText);
				finalText.Append (iconText);
				label.AttributedText = finalText;
			}
		}

		public static NSTextAttachment IconAttachment(this UILabel label, string icon)
		{
			var font = label.Font;
			var imageSize = font.xHeight + font.CapHeight;

			generator.Size = imageSize;
			generator.Insets = new UIEdgeInsets (1, 1, 1, 1);
			generator.Colors = new UIColor[] { label.TextColor };

			using (var text = new NSMutableAttributedString ()) {
				var iconAttachment = new NSTextAttachment ();
				iconAttachment.Bounds = new CGRect (new CGPoint (0, font.Descender), new CGSize (imageSize, imageSize));
				iconAttachment.Image = generator.CreateImage (FAIcons.FAClockO);
				return iconAttachment;
			}
		}


		public static void SetIcon(this UIButton btn, string icon, nfloat size, UIControlState state = UIControlState.Normal)
		{
			btn.Font = FAIcons.Font (size);
			btn.SetTitle(icon, state);
		}

		public static void SetIcon(this UIButton btn, string icon, UIControlState state = UIControlState.Normal)
		{
			SetIcon (btn, icon, btn.Font.PointSize, state);
		}

		public static void SetIcon(this UIBarButtonItem btn, string icon, UIColor color, nfloat size)
		{
			var attrs = new UITextAttributes () {
				Font = FAIcons.Font(size),
				TextColor = color
			};
			btn.Title = icon;
			btn.SetTitleTextAttributes (attrs, UIControlState.Normal);
		}

		public static void SetIcon(this UIBarButtonItem btn, string icon, UIColor color)
		{
			SetIcon (btn, icon, color, 22);
		}

		public static void SetIcon(this UIBarButtonItem btn, string icon)
		{
			SetIcon (btn, icon, null, 22);
		}

		public static void SetIcon(this UITabBarItem tab, string icon)
		{
			tab.Image = FAImages.TabBar.CreateImage (icon);
		}

		public static void SetIcon(this UITabBarItem tab, string icon, nfloat size)
		{
			tab.Image = FAImages.TabBar.CreateImage (icon, size);
		}
	}
}

