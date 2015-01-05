using System;
using UIKit;

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

		public static void SetIcon(this UIButton btn, string icon, nfloat size, UIControlState state = UIControlState.Normal)
		{
			btn.Font = FAIcons.Font (size);
			btn.SetTitle(icon, state);
		}

		public static void SetIcon(this UIButton btn, string icon, UIControlState state = UIControlState.Normal)
		{
			SetIcon (btn, icon, btn.Font.PointSize, state);
		}

		public static void SetIcon(this UIBarButtonItem btn, string icon, nfloat size)
		{
			var attrs = new UITextAttributes () {
				Font = FAIcons.Font(size)
			};
			btn.Title = icon;
			btn.SetTitleTextAttributes (attrs, UIControlState.Normal);
		}

		public static void SetIcon(this UIBarButtonItem btn, string icon)
		{
			SetIcon (btn, icon, 22);
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

