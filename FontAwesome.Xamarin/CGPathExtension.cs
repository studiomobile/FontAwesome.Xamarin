using System;
using System.Runtime.InteropServices;
using ObjCRuntime;
using CoreGraphics;

namespace FontAwesome.iOS
{
	public static class CGPathExtension
	{
		public unsafe static CGPath CopyByTransformingPath(this CGPath path, CGAffineTransform transform)
		{
			var handle = CGPathCreateCopyByTransformingPath (path.Handle, &transform);
			return new CGPath (handle);
		}



		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe static extern IntPtr CGPathCreateCopyByTransformingPath (
			IntPtr path,
			CGAffineTransform *transform
		); 

	}
}

