using System;
using UIKit;
using CoreGraphics;

namespace FontAwesome.iOS
{
	public static class FAImages
	{
		public static readonly IFAImageGenerator TabBar = new FAImageGenerator () {
			Size = 24,
			Insets = new UIEdgeInsets (1, 1, 1, 1)
		};
	}

}

