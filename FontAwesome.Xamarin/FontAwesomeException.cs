﻿//
// FontAwesomeException.cs
//
// Author:
//       Neil Kennedy <neil.p.kennedy@outlook.com>
//
// Copyright (c) 2014 neilkennedy
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;

namespace FontAwesome.iOS
{
	[Serializable]
	public class FontAwesomeException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FontAwesomeException"/> class
		/// </summary>
		public FontAwesomeException ()
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FontAwesomeException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		public FontAwesomeException (string message) : base (message)
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FontAwesomeException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. </param>
		public FontAwesomeException (string message, Exception inner) : base (message, inner)
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FontAwesomeException"/> class
		/// </summary>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <param name="info">The object that holds the serialized object data.</param>
		protected FontAwesomeException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
		{
		}
	}
}

